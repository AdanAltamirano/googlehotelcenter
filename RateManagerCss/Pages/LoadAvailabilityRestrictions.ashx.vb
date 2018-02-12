Imports Microsoft.VisualBasic
Imports Portal.General.Facade
Imports Portal.Hotel.Facade
Imports Portal.Hotel.Common.Data
Imports Portal.General.Common.Data
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager

Imports System.Data
Imports System.Data.Sql
Imports System.Web
Imports System.Web.Services

Public Class LoadAvailabilityRestrictions1
    Implements System.Web.IHttpHandler, IRequiresSessionState
    Protected m_context As HttpContext

    Public Function dsEmpty(ByVal ds As DataSet, Optional ByVal Name As String = "") As Boolean
        '// Si el dataset es valido regresa false.
        Try
            If (Not IsNothing(ds)) AndAlso (ds.Tables.Count > 0) Then
                If (Name = "") Then
                    If (ds.Tables(0).Rows.Count > 0) Then Return (False)
                Else
                    If (Not IsNothing(ds.Tables(Name))) AndAlso _
                        (ds.Tables(Name).Rows.Count > 0) Then Return (False)
                End If
            End If
            Return (True)
        Catch ex As Exception
            Return (False)
        End Try
    End Function

    Function GetIdAsociation()
        Dim IdAsociation As Integer = -1
        Try
            If ConfigurationManager.AppSettings("IdAsociation") IsNot Nothing Then Integer.TryParse(ConfigurationManager.AppSettings("IdAsociation"), IdAsociation)
            IdAsociation = If(IdAsociation = 0, -1, IdAsociation)
        Catch ex As Exception
        End Try
        Return IdAsociation
    End Function

    Private Function IdHotel() As Integer
        Const SESSION_INFO As String = "infoCompany"
        Dim cI As companyInfo = m_context.Session(SESSION_INFO)
        Return cI.Hotel
    End Function


    Private Sub LoadRulesHotel()
        m_context.Response.ContentType = "text/xml"
        'proceso de lectura de las reglas del hotel
        Dim dsHotel As HotelDatos
        Dim status As String = String.Empty
        With New HotelSistema
            dsHotel = .GetHotelById(IdHotel)
        End With
        Select Case dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows(0)("StatusAvailability")
            'Case DBNull.Value
            '    status = IIf(PortalCulture.GetIDCulture = 1, "Sin Especificar", "Unspecified")
            Case "O"
                status = IIf(PortalCulture.GetIDCulture = 1, "Abierto", "Open")
            Case "C"
                status = IIf(PortalCulture.GetIDCulture = 1, "Cerrado", "Closed")
            Case "N"
                status = IIf(PortalCulture.GetIDCulture = 1, "No Llegadas", "No Arrivals")
        End Select
        'Agregamos la columna del status convertida con la cadena
        dsHotel.Tables(dsHotel.HOTEL_TABLE).Columns.Add("StatusAvail", Type.GetType("System.String"))
        dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows(0)("StatusAvail") = status

        Dim PoliticaCancelacion As String = String.Empty
        Dim antesDe As String = String.Empty
        Dim CancelacionValor As String = String.Empty
        If Not dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows(0)("DiasMinCancelar") Is DBNull.Value Then
            PoliticaCancelacion = PortalCulture.GetString("00020")
            antesDe = PortalCulture.GetString("00410")
            CancelacionValor = dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows(0)("DiasMinCancelar")
        ElseIf Not dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows(0)("CancelPriorHours") Is DBNull.Value Then
            PoliticaCancelacion = PortalCulture.GetString("00021")
            antesDe = PortalCulture.GetString("00409")
            CancelacionValor = dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows(0)("CancelPriorHours")
        ElseIf Not dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows(0)("CancelPriorSpecificT") Is DBNull.Value Then
            PoliticaCancelacion = PortalCulture.GetString("00381")
            antesDe = PortalCulture.GetString("00411")
            CancelacionValor = CType(dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows(0)("CancelPriorSpecificT"), String).Substring(0, 2) + ":" + CType(dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows(0)("CancelPriorSpecificT"), String).Substring(2, 2)
        Else
            PoliticaCancelacion = ""
            antesDe = ""
            CancelacionValor = ""
        End If

        dsHotel.Tables(dsHotel.HOTEL_TABLE).Columns.Add("CancelacionPor", Type.GetType("System.String"))
        dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows(0)("CancelacionPor") = PoliticaCancelacion

        dsHotel.Tables(dsHotel.HOTEL_TABLE).Columns.Add("MsgCancelacion", Type.GetType("System.String"))
        dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows(0)("MsgCancelacion") = antesDe

        dsHotel.Tables(dsHotel.HOTEL_TABLE).Columns.Add("CancelacionValor", Type.GetType("System.String"))
        dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows(0)("CancelacionValor") = CancelacionValor


        m_context.Response.Write(dsHotel.GetXml.ToString)
        'm_context.Response.End()
    End Sub

    Private Sub LoadRatePlans(ByVal fecha As String)
        m_context.Response.ContentType = "text/xml"
        'proceso de lectura de los ratesPlans
        Dim daCom As New SqlDataAdapter
        Dim cmCom As New SqlCommand
        Dim ds As DataSet
        Try
            ds = New DataSet
            cmCom = New SqlCommand("spGetRatePlanAvailability", New SqlConnection(AppSettings("HotelConnection")))
            With cmCom
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add("@fecha", fecha)
                .Parameters.Add("@idHotel", IdHotel)
                .Parameters.Add("@idIdioma", PortalCulture.GetIDCulture())
            End With
            daCom.SelectCommand = cmCom
            daCom.Fill(ds)
        Catch ex As Exception
        End Try
        ds.Tables(0).TableName = "RatePlan"
        m_context.Response.Write(ds.GetXml.ToString)
        'm_context.Response.End()
    End Sub


    Private Sub LoadRatePlanRules(ByVal fecha As String, ByVal idRatePlan As String)
        m_context.Response.ContentType = "text/xml"
        'proceso de lectura de los ratesPlans
        Dim daCom As New SqlDataAdapter
        Dim cmCom As New SqlCommand
        Dim ds As New DataSet
        Try
            ds = New DataSet
            cmCom = New SqlCommand("spGetRatePlanAvailabilityRules", New SqlConnection(AppSettings("HotelConnection")))
            With cmCom
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add("@fecha", fecha)
                .Parameters.Add("@idRatePlan", idRatePlan)
                .Parameters.Add("@idHotel", IdHotel)
                .Parameters.Add("@idIdioma", PortalCulture.GetIDCulture)
            End With
            daCom.SelectCommand = cmCom
            daCom.Fill(ds)
        Catch ex As Exception
        End Try

        If Not dsEmpty(ds) Then

            Dim PoliticaCancelacion As String = String.Empty
            Dim antesDe As String = String.Empty
            Dim CancelacionValor As String = String.Empty
            If Not ds.Tables(0).Rows(0)("CancelPriorDays") Is DBNull.Value Then
                PoliticaCancelacion = PortalCulture.GetString("00020")
                antesDe = PortalCulture.GetString("00410")
                CancelacionValor = ds.Tables(0).Rows(0)("CancelPriorDays")
            ElseIf Not ds.Tables(0).Rows(0)("CancelPriorHours") Is DBNull.Value Then
                PoliticaCancelacion = PortalCulture.GetString("00021")
                antesDe = PortalCulture.GetString("00409")
                CancelacionValor = ds.Tables(0).Rows(0)("CancelPriorHours")
            ElseIf Not ds.Tables(0).Rows(0)("CancelPriorSpecificT") Is DBNull.Value Then
                PoliticaCancelacion = PortalCulture.GetString("00381")
                antesDe = PortalCulture.GetString("00411")
                CancelacionValor = CType(ds.Tables(0).Rows(0)("CancelPriorSpecificT"), String).Substring(0, 2) + ":" + CType(ds.Tables(0).Rows(0)("CancelPriorSpecificT"), String).Substring(2, 2)
            Else
                PoliticaCancelacion = PortalCulture.GetString("01090")
                antesDe = ""
                CancelacionValor = ""
            End If

            ds.Tables(0).Columns.Add("CancelacionPor", Type.GetType("System.String"))
            ds.Tables(0).Rows(0)("CancelacionPor") = PoliticaCancelacion

            ds.Tables(0).Columns.Add("MsgCancelacion", Type.GetType("System.String"))
            ds.Tables(0).Rows(0)("MsgCancelacion") = antesDe

            ds.Tables(0).Columns.Add("CancelacionValor", Type.GetType("System.String"))
            ds.Tables(0).Rows(0)("CancelacionValor") = CancelacionValor

            ds.Tables(0).TableName = "RatePlanRules"
        End If

        m_context.Response.Write(ds.GetXml.ToString)
        'm_context.Response.End()
    End Sub

    Private Sub LoadLockHotelRules(ByVal fecha As String)
        m_context.Response.ContentType = "text/xml"
        'proceso de lectura de los ratesPlans
        Dim daCom As New SqlDataAdapter
        Dim cmCom As New SqlCommand
        Dim ds As DataSet
        Try
            ds = New DataSet
            cmCom = New SqlCommand("spGetHotelLockGralAvailability", New SqlConnection(AppSettings("HotelConnection")))
            With cmCom
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add("@fecha", fecha)
                .Parameters.Add("@idHotel", IdHotel)
                .Parameters.Add("@idIdioma", PortalCulture.GetIDCulture)
            End With
            daCom.SelectCommand = cmCom
            daCom.Fill(ds)
        Catch ex As Exception
        End Try

        Dim PoliticaCancelacion As String = String.Empty
        Dim antesDe As String = String.Empty
        Dim CancelacionValor As String = String.Empty
        If Not ds Is Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
            If Not ds.Tables(0).Rows(0)("CancelPriorDays") Is DBNull.Value Then
                PoliticaCancelacion = PortalCulture.GetString("00020")
                antesDe = PortalCulture.GetString("00410")
                CancelacionValor = ds.Tables(0).Rows(0)("CancelPriorDays")
            ElseIf Not ds.Tables(0).Rows(0)("CancelPriorHours") Is DBNull.Value Then
                PoliticaCancelacion = PortalCulture.GetString("00021")
                antesDe = PortalCulture.GetString("00409")
                CancelacionValor = ds.Tables(0).Rows(0)("CancelPriorHours")
            ElseIf Not ds.Tables(0).Rows(0)("CancelPriorSpecificT") Is DBNull.Value Then
                PoliticaCancelacion = PortalCulture.GetString("00381")
                antesDe = PortalCulture.GetString("00411")
                CancelacionValor = CType(ds.Tables(0).Rows(0)("CancelPriorSpecificT"), String).Substring(0, 2) + ":" + CType(ds.Tables(0).Rows(0)("CancelPriorSpecificT"), String).Substring(2, 2)
            Else
                PoliticaCancelacion = PortalCulture.GetString("01090")
                antesDe = ""
                CancelacionValor = ""
            End If
            ds.Tables(0).Columns.Add("CancelacionPor", Type.GetType("System.String"))
            ds.Tables(0).Rows(0)("CancelacionPor") = PoliticaCancelacion

            ds.Tables(0).Columns.Add("MsgCancelacion", Type.GetType("System.String"))
            ds.Tables(0).Rows(0)("MsgCancelacion") = antesDe

            ds.Tables(0).Columns.Add("CancelacionValor", Type.GetType("System.String"))
            ds.Tables(0).Rows(0)("CancelacionValor") = CancelacionValor
        End If

        ds.Tables(0).TableName = "LockHotelRules"
        m_context.Response.Write(ds.GetXml.ToString)
        'm_context.Response.End()
    End Sub

    Private Sub LoadBaseRatePlansHotels()
        m_context.Response.ContentType = "text/xml"
        Dim ds As RatePlanData
        Dim idAsoc As Integer = Me.GetIdAsociation

        With New RatePlanFacade
            ds = .GetRatePlanByIdHotel(IdHotel, PortalCulture.GetIDCulture, 0, idAsociacion:=idAsoc, DeleteFilter:=1)
        End With

        ds.Tables(ds.RATEPLAN_TABLE).Columns.Add("texto", System.Type.GetType("System.String"), "substring(" & ds.FIELD_CODIGOTARIFA & "+ ' ' + '--' + ' ' +" & ds.FIELD_NAME & ",1,25)")

        Dim dr As DataRow
        'For Each dr In ds.Tables(ds.RATEPLAN_TABLE).Rows
        '    'If dr(ds.FIELD_SEGMENT) = "K" Or dr(ds.FIELD_SEGMENT) = "I" Then
        '    '    dr.Delete()
        '    'End If
        '    If dr(ds.FIELD_SEGMENT) = "I" Then
        '        dr.Delete()
        '    End If
        'Next
        ds.AcceptChanges()
        ds.Tables(ds.RATEPLAN_TABLE).Columns.Add("id_Rule", Type.GetType("System.String"))
        ds.AcceptChanges()
        For Each dr2 As DataRow In ds.Tables(ds.RATEPLAN_TABLE).Rows
            If dr2("idRule") Is DBNull.Value Then
                dr2("id_Rule") = "-1"
            Else
                dr2("id_Rule") = CType(dr2("idRule"), String)
            End If
        Next
        ds.AcceptChanges()
        m_context.Response.Write(ds.GetXml.ToString)
        'm_context.Response.End()
    End Sub

    Private Sub LoadRatePlanRulesHotel(ByVal idRule As String)
        m_context.Response.ContentType = "text/xml"
        Dim daCom As New SqlDataAdapter
        Dim cmCom As New SqlCommand
        Dim ds As DataSet
        Try
            ds = New DataSet
            cmCom = New SqlCommand("spGetRatePlanRuleHotel", New SqlConnection(AppSettings("HotelConnection")))
            With cmCom
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add("@idHotel", IdHotel)
                .Parameters.Add("@idRule", idRule)
            End With
            daCom.SelectCommand = cmCom
            daCom.Fill(ds)
        Catch ex As Exception
        End Try


        Dim PoliticaCancelacion As String = String.Empty
        Dim antesDe As String = String.Empty
        Dim CancelacionValor As String = String.Empty
        If Not ds.Tables(0).Rows(0)("CancelPriorDays") Is DBNull.Value Then
            PoliticaCancelacion = PortalCulture.GetString("00020")
            antesDe = PortalCulture.GetString("00410")
            CancelacionValor = ds.Tables(0).Rows(0)("CancelPriorDays")
        ElseIf Not ds.Tables(0).Rows(0)("CancelPriorHours") Is DBNull.Value Then
            PoliticaCancelacion = PortalCulture.GetString("00021")
            antesDe = PortalCulture.GetString("00409")
            CancelacionValor = ds.Tables(0).Rows(0)("CancelPriorHours")
        ElseIf Not ds.Tables(0).Rows(0)("CancelPriorSpecificT") Is DBNull.Value Then
            PoliticaCancelacion = PortalCulture.GetString("00381")
            antesDe = PortalCulture.GetString("00411")
            CancelacionValor = CType(ds.Tables(0).Rows(0)("CancelPriorSpecificT"), String).Substring(0, 2) + ":" + CType(ds.Tables(0).Rows(0)("CancelPriorSpecificT"), String).Substring(2, 2)
        Else
            PoliticaCancelacion = PortalCulture.GetString("01090")
            antesDe = ""
            CancelacionValor = ""
        End If

        ds.Tables(0).Columns.Add("CancelacionPor", Type.GetType("System.String"))
        ds.Tables(0).Rows(0)("CancelacionPor") = PoliticaCancelacion

        ds.Tables(0).Columns.Add("MsgCancelacion", Type.GetType("System.String"))
        ds.Tables(0).Rows(0)("MsgCancelacion") = antesDe

        ds.Tables(0).Columns.Add("CancelacionValor", Type.GetType("System.String"))
        ds.Tables(0).Rows(0)("CancelacionValor") = CancelacionValor

        ds.Tables(0).TableName = "RatePlanRuleHotel"
        m_context.Response.Write(ds.GetXml.ToString)
        'm_context.Response.End()
    End Sub

    Private Sub LoadRateRoom(ByVal fecha As String, ByVal idRoom As Integer, ByVal idRatePlan As String)
        m_context.Response.ContentType = "text/xml"
        Dim daCom As New SqlDataAdapter
        Dim cmCom As New SqlCommand
        Dim ds As DataSet
        Try
            ds = New DataSet
            cmCom = New SqlCommand("spGetTarifasByDates", New SqlConnection(AppSettings("HotelConnection")))
            With cmCom
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add("@idTipoHabitacionHotel", idRoom)
                .Parameters.Add("@idRatePlan", idRatePlan)
                .Parameters.Add("@fecha", fecha)
            End With
            daCom.SelectCommand = cmCom
            daCom.Fill(ds)
        Catch ex As Exception
        End Try
        ds.Tables(0).TableName = "RateRoomRules"
        m_context.Response.Write(ds.GetXml.ToString)
        'm_context.Response.End()
    End Sub



    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        m_context = context


        'Put user code to initialize the page here
        If Not m_context.Request.QueryString("nivelRegla") Is Nothing AndAlso m_context.Request.QueryString("nivelRegla") = 1 Then
            LoadRulesHotel()
        ElseIf Not m_context.Request.QueryString("nivelRegla") Is Nothing AndAlso m_context.Request.QueryString("nivelRegla") = 2 Then
            If Not m_context.Request.QueryString("fecha") Is Nothing AndAlso m_context.Request.QueryString("nivelRegla") <> String.Empty Then
                LoadRatePlans(m_context.Request.QueryString("fecha"))
            End If
        ElseIf Not m_context.Request.QueryString("nivelRegla") Is Nothing AndAlso m_context.Request.QueryString("nivelRegla") = 3 Then
            If Not m_context.Request.QueryString("fecha") Is Nothing AndAlso m_context.Request.QueryString("nivelRegla") <> String.Empty And m_context.Request.QueryString("_idRatePlan") <> String.Empty Then
                LoadRatePlanRules(m_context.Request.QueryString("fecha"), m_context.Request.QueryString("_idRatePlan"))
            End If
        ElseIf Not m_context.Request.QueryString("nivelRegla") Is Nothing AndAlso m_context.Request.QueryString("nivelRegla") = 4 Then
            If Not m_context.Request.QueryString("fecha") Is Nothing AndAlso m_context.Request.QueryString("nivelRegla") <> String.Empty Then
                LoadLockHotelRules(m_context.Request.QueryString("fecha"))
            End If
        ElseIf Not m_context.Request.QueryString("nivelRegla") Is Nothing AndAlso m_context.Request.QueryString("nivelRegla") = 5 Then
            LoadBaseRatePlansHotels()
        ElseIf Not m_context.Request.QueryString("nivelRegla") Is Nothing AndAlso m_context.Request.QueryString("nivelRegla") = 6 Then
            If Not m_context.Request.QueryString("idRule") Is Nothing Then
                LoadRatePlanRulesHotel(m_context.Request.QueryString("idRule"))
            End If
        ElseIf Not m_context.Request.QueryString("nivelRegla") Is Nothing AndAlso m_context.Request.QueryString("nivelRegla") = 7 Then
            If Not m_context.Request.QueryString("fecha") Is Nothing And Not m_context.Request.QueryString("idRoom") Is Nothing And Not m_context.Request.QueryString("idRatePlan") Is Nothing Then
                LoadRateRoom(m_context.Request.QueryString("fecha"), m_context.Request.QueryString("idRoom"), m_context.Request.QueryString("idRatePlan"))
            End If
        End If


        'm_context.Response.ContentType = "text/plain"
        'm_context.Response.Write("Hello World!")

    End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class