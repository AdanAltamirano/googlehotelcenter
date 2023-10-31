Imports System.Configuration.ConfigurationManager
Imports System.Web
Imports System.Web.Services
Imports System.Data.SqlClient
Imports Portal.Hotel.Common.Data
Imports Portal.Hotel.Facade
Imports Portal.General.Common.Data
Imports Portal.General.DataAccess
Imports Portal.General.Facade


Public Class ExecutorServer
    Implements System.Web.IHttpHandler, IRequiresSessionState

    Function GetIdAsociation()
        Dim IdAsociation As Integer = -1
        Try
            If ConfigurationManager.AppSettings("IdAsociation") IsNot Nothing Then Integer.TryParse(ConfigurationManager.AppSettings("IdAsociation"), IdAsociation)
            IdAsociation = If(IdAsociation = 0, -1, IdAsociation)
        Catch ex As Exception
        End Try
        Return IdAsociation
    End Function

    Public Function getAllRooms(ByVal idHotel As String, ByVal idIdioma As String) As DataSet
        Dim room As RoomsHotelData
        With New RoomFacade
            room = .getRooms(idHotel, idIdioma)
        End With
        Return room
    End Function

    Public Function getUserRoles(ByVal idUsuario As Integer) As DataSet
        Dim conection As New SqlConnection(AppSettings("PortalConectionString"))
        Dim command As New SqlCommand("spUserGetRoleByUserId", conection)

        With command
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@idUsuario", idUsuario))
        End With
        Dim adapter As New SqlDataAdapter(command)
        Dim dRes As New DataSet
        adapter.Fill(dRes)

        Return dRes
    End Function

    Public Function getHotelsList(ByVal idUsuario As Integer, ByVal idPais As String, ByVal status As String, ByVal idIdioma As String) As DataSet

        Dim conection As New SqlConnection(AppSettings("PortalConectionString"))
        Dim command As New SqlCommand("spCompanySearchCompanys", conection)

        With command
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@idUsuario", idUsuario))
            .Parameters.Add(New SqlParameter("@idRubro", 10))
            .Parameters.Add(New SqlParameter("@Nombre", ""))
        End With
        Dim adapter As New SqlDataAdapter(command)
        Dim dRes As New DataSet
        adapter.Fill(dRes)

        If dRes.Tables.Count > 0 AndAlso dRes.Tables(0).Rows.Count > 0 Then

            Dim filtro As String = ""

            ' Si se especifica status de hotel, se establece en el filtro
            If Not String.IsNullOrWhiteSpace(status) Then

                If status.Equals("2") Then
                    status = "0"
                End If

                filtro += "Active = '" & CBool(status).ToString & "'"

                If Not String.IsNullOrWhiteSpace(idPais) Then
                    filtro += " and "
                End If
            End If

            ' Si se especifican paises
            If Not String.IsNullOrWhiteSpace(idPais) Then
                Dim paises() As String = idPais.Split(",")
                filtro += "idPais in ("
                For x As Integer = 0 To paises.Count - 1
                    If Not String.IsNullOrWhiteSpace(paises(x)) Then ' Para evitar comas accidentales o con espacios en blanco
                        filtro += "'" & paises(x) & "'"
                        If x < paises.Count - 1 Then
                            filtro += ","
                        End If
                    End If
                Next

                filtro += ")"

            End If

            ' Se aplica el filtro
            If filtro.Length > 0 Then
                Dim tablaFiltrada As DataTable = dRes.Tables(0).Select(filtro).CopyToDataTable

                If tablaFiltrada.Rows.Count > 0 Then
                    tablaFiltrada.TableName = "Properties"

                    For x As Integer = 0 To tablaFiltrada.Rows.Count - 1
                        tablaFiltrada.Rows(x)("NombreEmpresa") = tablaFiltrada.Rows(x)("idEmpresa") & "-" & tablaFiltrada.Rows(x)("NombreEmpresa")
                    Next

                    tablaFiltrada.AcceptChanges()

                    dRes.Clear()
                    dRes = New DataSet
                    dRes.Tables.Add(tablaFiltrada)
                End If

                'Dim newDRes = New DataSet
                'newDRes.Tables.Add(tablaFiltrada)
                'Return newDRes
            End If



        End If


        Return dRes
    End Function

    Private Function LoadRateplans(ByVal idHotel As String, ByVal idCultura As String, ByVal context As HttpContext) As DataSet
        Dim ds As RatePlanData
        Dim idAsociacion As String = GetIdAsociation()
        Dim isSupervisor As String = "false"
        Dim IsUsuarioHotelAssociation As String = "false"

        If Not String.IsNullOrEmpty(context.Request.QueryString("isSupervisor")) Then
            isSupervisor = context.Request.QueryString("isSupervisor")
        End If
        If Not String.IsNullOrEmpty(context.Request.QueryString("IsUsuarioHotelAssociation")) Then
            IsUsuarioHotelAssociation = context.Request.QueryString("IsUsuarioHotelAssociation")
        End If

        With New RatePlanFacade
            If isSupervisor.ToLower = "true" Or IsUsuarioHotelAssociation.ToLower = "true" Then
                'Mostramos todos los rateplans incluidos los de tarifas netas.
                ds = .GetRatePlanByIdHotel(idHotel, idCultura, 1, 1, idAsociacion:=idAsociacion)
            Else
                'Mostramos solamente los ratesplans que no sean de tarifas netas.
                ds = .GetRatePlanByIdHotel(idHotel, idCultura, 1, 0, idAsociacion:=idAsociacion)
            End If

        End With

        Dim dr As DataRow

        For Each dr In ds.Tables(RatePlanData.RATEPLAN_TABLE).Rows
            'If dr(ds.FIELD_SEGMENT) = "K" Or dr(ds.FIELD_SEGMENT) = "I" Then
            '    dr.Delete()
            'End If
            If dr(RatePlanData.FIELD_SEGMENT) = "I" Then
                dr.Delete()
            End If
        Next
        ds.AcceptChanges()
        Return ds
    End Function

    Private Function loadpackages(ByVal idhotel As String, ByVal idIdioma As String) As DataSet
        Dim ds As PackageData
        Dim idAsociacion As String = GetIdAsociation()

        With New PackageFacade
            ds = .GetPackageByIdHotel(idhotel, idIdioma, idAsociacion:=idAsociacion)
        End With
        Return ds
    End Function

    Private Function loadAgencyRates(ByVal idHotel As String, ByVal idIdioma As String) As DataSet
        Dim ds As New DataSet
        Dim idAsoc As Integer = GetIdAsociation()

        Try
            With New AgencyRatesFacade
                ds = .GetAgencyRatesByIdHotel(idHotel, idAsoc, idIdioma:=idIdioma)
            End With
        Catch ex As Exception
            Dim mensaje As String = ex.Message
        End Try
        Return ds
    End Function

    Private Function LoadRatesPlanDeposit(ByVal idHotel As String, ByVal idIdioma As String, ByVal context As HttpContext) As DataSet
        Dim ds As RatePlanData
        Dim idAsoc As Integer = GetIdAsociation()
        Dim incluirNetRatesPlan As String = "0"

        If Not String.IsNullOrEmpty(context.Request.QueryString("incluirNetRatesPlan")) Then
            incluirNetRatesPlan = context.Request.QueryString("incluirNetRatesPlan")
        End If

        ds = (New RatePlanFacade).GetRatePlanByIdHotel(idHotel, idIdioma, incluirNetRatesPlan, idAsociacion:=idAsoc)        
        Return ds
    End Function

    Private Function LoadRatesPlanPrepay(ByVal idHotel As String, ByVal idIdioma As String) As DataSet
        Dim ds As DataSet
        Dim idAsoc As Integer = GetIdAsociation()

        ds = (New RatePlanFacade).GetRatesPlanPrepagoByHotel(idHotel, idIdioma, idAsociacion:=idAsoc)
        ds.Tables(0).TableName = "RatePlan"
        Return ds
    End Function

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim DS As New DataSet
        Dim sError As String = ""
        Dim idIdioma As String = 2
        Dim idHotel As String = "0"
        Dim idPais As String = ""
        Dim status As String = ""
        Dim idSegmento As Integer = 0
        Dim idCorporate As Integer = 0
        Dim idUsuario As Integer = 0
        Dim IsSupervisor As Boolean = False

        If Not String.IsNullOrEmpty(context.Request.QueryString("IsSupervisor")) Then
            IsSupervisor = CBool(context.Request.QueryString("IsSupervisor"))
        End If

        If Not String.IsNullOrEmpty(context.Request.QueryString("idIdioma")) Then
            idIdioma = context.Request.QueryString("idIdioma")
        Else
            If Not String.IsNullOrEmpty(AppSettings("DefaultLanguage")) Then
                idIdioma = AppSettings("DefaultLanguage")
            End If
        End If

        If Not String.IsNullOrEmpty(context.Request.QueryString("idHotel")) Then
            idHotel = context.Request.QueryString("idHotel")
        End If

        If Not String.IsNullOrEmpty(context.Request.QueryString("status")) Then
            status = context.Request.QueryString("status")
        End If

        If Not String.IsNullOrEmpty(context.Request.QueryString("idPais")) Then
            idPais = context.Request.QueryString("idPais")
        End If

        If Not String.IsNullOrEmpty(context.Request.QueryString("idUsuario")) Then
            idUsuario = CType(context.Request.QueryString("idUsuario"), Integer)
        Else
            idUsuario = -1
        End If

        If Not String.IsNullOrEmpty(context.Request.QueryString("idSegmento")) Then
            Integer.TryParse(context.Request.QueryString("idSegmento"), idSegmento)
        End If

        If Not String.IsNullOrEmpty(context.Request.QueryString("idCorporate")) Then
            Integer.TryParse(context.Request.QueryString("idCorporate"), idCorporate)
        End If

        If Not String.IsNullOrEmpty(context.Request.QueryString("catalogo")) Then
            Select Case context.Request.QueryString("catalogo").ToUpper
                Case "ROOMS"
                    DS = getAllRooms(idHotel, idIdioma)
                Case "RATESPLANS"
                    DS = LoadRateplans(idHotel, idIdioma, context)
                Case "PACKAGE"
                    DS = loadpackages(idHotel, idIdioma)
                Case "AGENCYRATE"
                    DS = loadAgencyRates(idHotel, idIdioma)
                Case "DEPOSITRATESPLAN"
                    DS = LoadRatesPlanDeposit(idHotel, idIdioma, context)
                Case "RATESPLANPREPAY"
                    DS = LoadRatesPlanPrepay(idHotel, idIdioma)
                Case "AGENCIES"
                    DS = LoadAgencies(idSegmento)
                Case "GROUPS"
                    DS = LoadGroups(idCorporate)
                Case "AGREEMENTS"
                    DS = LoadAgreements(idCorporate)
                Case "PROPERTIES"
                    If IsSupervisor Then
                        idUsuario = -1
                    End If
                    DS = getHotelsList(idUsuario, idPais, status, idIdioma)
            End Select
        End If

        context.Response.ContentType = "text/xml"
        context.Response.Write(DS.GetXml)

        'context.Response.ContentType = "text/plain"
        'context.Response.Write("Hello World!")

    End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

    Private Function LoadAgencies(ByVal idSegmento As Integer) As DataSet
        Dim ds As DataSet
        ds = (New EmpresaSistema).GetCompanyAgencyByIdSegment(idSegmento)
        ds.Tables(0).TableName = "Agencies"
        Return ds
    End Function

    Private Function LoadGroups(ByVal idCorporate As Integer) As DataSet
        Dim ds As DataSet
        ds = (New RatePlanFacade).GetGroupsByIdCorporate(idCorporate)
        ds.Tables(0).TableName = "Groups"
        Return ds
    End Function
    Private Function LoadAgreements(ByVal idCorporate As Integer) As DataSet
        Dim ds As DataSet
        ds = (New RatePlanAccess).GetAgreementsByIdCorporate(idCorporate)
        ds.Tables(0).TableName = "Agreements"
        Return ds
    End Function

End Class