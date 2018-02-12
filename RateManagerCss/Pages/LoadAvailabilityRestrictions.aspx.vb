Imports Microsoft.VisualBasic
Imports Portal.General.Facade
Imports Portal.Hotel.Facade
Imports Portal.Hotel.Common.Data
Imports Portal.General.Common.Data
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager

Partial Class LoadAvailabilityRestrictions
    Inherits PaginaBase

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        If Not Request.QueryString("nivelRegla") Is Nothing AndAlso Request.QueryString("nivelRegla") = 1 Then
            LoadRulesHotel()
        ElseIf Not Request.QueryString("nivelRegla") Is Nothing AndAlso Request.QueryString("nivelRegla") = 2 Then
            If Not Request.QueryString("fecha") Is Nothing AndAlso Request.QueryString("nivelRegla") <> String.Empty Then
                LoadRatePlans(Request.QueryString("fecha"))
            End If
        ElseIf Not Request.QueryString("nivelRegla") Is Nothing AndAlso Request.QueryString("nivelRegla") = 3 Then
            If Not Request.QueryString("fecha") Is Nothing AndAlso Request.QueryString("nivelRegla") <> String.Empty And Request.QueryString("idRatePlan") <> String.Empty Then
                LoadRatePlanRules(Request.QueryString("fecha"), Request.QueryString("idRatePlan"))
            End If
        ElseIf Not Request.QueryString("nivelRegla") Is Nothing AndAlso Request.QueryString("nivelRegla") = 4 Then
            If Not Request.QueryString("fecha") Is Nothing AndAlso Request.QueryString("nivelRegla") <> String.Empty Then
                LoadLockHotelRules(Request.QueryString("fecha"))
            End If
        ElseIf Not Request.QueryString("nivelRegla") Is Nothing AndAlso Request.QueryString("nivelRegla") = 5 Then
            LoadBaseRatePlansHotels()
        ElseIf Not Request.QueryString("nivelRegla") Is Nothing AndAlso Request.QueryString("nivelRegla") = 6 Then
            If Not Request.QueryString("idRule") Is Nothing Then
                LoadRatePlanRulesHotel(Request.QueryString("idRule"))
            End If
        ElseIf Not Request.QueryString("nivelRegla") Is Nothing AndAlso Request.QueryString("nivelRegla") = 7 Then
            If Not Request.QueryString("fecha") Is Nothing And Not Request.QueryString("idRoom") Is Nothing And Not Request.QueryString("idRatePlan") Is Nothing Then
                LoadRateRoom(Request.QueryString("fecha"), Request.QueryString("idRoom"), Request.QueryString("idRatePlan"))
            End If
        End If
    End Sub

    Private Sub LoadRulesHotel()
        Response.ContentType = "text/xml"
        'proceso de lectura de las reglas del hotel
        Dim dsHotel As HotelDatos
        Dim status As String = String.Empty
        With New HotelSistema
            dsHotel = .GetHotelById(MyBase.cInfoActual.Hotel)
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


        Response.Write(dsHotel.GetXml.ToString)
        Response.End()
    End Sub

    Private Sub LoadRatePlans(ByVal fecha As String)
        Response.ContentType = "text/xml"
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
                .Parameters.Add("@idHotel", MyBase.cInfoActual.Hotel)
                .Parameters.Add("@idIdioma", PortalCulture.GetIDCulture())
            End With
            daCom.SelectCommand = cmCom
            daCom.Fill(ds)
        Catch ex As Exception
        End Try
        ds.Tables(0).TableName = "RatePlan"
        Response.Write(ds.GetXml.ToString)
        Response.End()
    End Sub


    Private Sub LoadRatePlanRules(ByVal fecha As String, ByVal idRatePlan As String)
        Response.ContentType = "text/xml"
        'proceso de lectura de los ratesPlans
        Dim daCom As New SqlDataAdapter
        Dim cmCom As New SqlCommand
        Dim ds As DataSet
        Try
            ds = New DataSet
            cmCom = New SqlCommand("spGetRatePlanAvailabilityRules", New SqlConnection(AppSettings("HotelConnection")))
            With cmCom
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add("@fecha", fecha)
                .Parameters.Add("@idRatePlan", idRatePlan)
                .Parameters.Add("@idHotel", MyBase.cInfoActual.Hotel)
                .Parameters.Add("@idIdioma", PortalCulture.GetIDCulture)
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

        ds.Tables(0).TableName = "RatePlanRules"
        Response.Write(ds.GetXml.ToString)
        Response.End()
    End Sub

    Private Sub LoadLockHotelRules(ByVal fecha As String)
        Response.ContentType = "text/xml"
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
                .Parameters.Add("@idHotel", MyBase.cInfoActual.Hotel)
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
        Response.Write(ds.GetXml.ToString)
        Response.End()
    End Sub

    Private Sub LoadBaseRatePlansHotels()
        Response.ContentType = "text/xml"
        Dim ds As RatePlanData
        Dim idAsoc As Integer = Me.GetIdAsociation

        With New RatePlanFacade
            ds = .GetRatePlanByIdHotel(MyBase.cInfoActual.Hotel, PortalCulture.GetIDCulture, 0, idAsociacion:=idAsoc)
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
        Response.Write(ds.GetXml.ToString)
        Response.End()
    End Sub

    Private Sub LoadRatePlanRulesHotel(ByVal idRule As String)
        Response.ContentType = "text/xml"
        Dim daCom As New SqlDataAdapter
        Dim cmCom As New SqlCommand
        Dim ds As DataSet
        Try
            ds = New DataSet
            cmCom = New SqlCommand("spGetRatePlanRuleHotel", New SqlConnection(AppSettings("HotelConnection")))
            With cmCom
                .CommandType = CommandType.StoredProcedure                
                .Parameters.Add("@idHotel", MyBase.cInfoActual.Hotel)
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
        Response.Write(ds.GetXml.ToString)
        Response.End()
    End Sub

    Private Sub LoadRateRoom(ByVal fecha As String, ByVal idRoom As Integer, ByVal idRatePlan As String)
        Response.ContentType = "text/xml"
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
        Response.Write(ds.GetXml.ToString)
        Response.End()
    End Sub

End Class
