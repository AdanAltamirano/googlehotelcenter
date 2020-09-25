Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Security
Imports System.Runtime.Serialization
Imports System.Configuration.ConfigurationManager
Imports System.IO
Imports System.Web.HttpContext
Imports System.Xml
Imports Portal.Hotel.Common
Imports Portal.Hotel.Facade
Imports System.Text
Imports System.Threading
Imports System.Security.Principal

Imports LoginAuthenticate
Imports Portal.General.Facade
Imports Portal.General.Common
Imports Portal.General.Common.Data
Imports Portal.General.DataAccess

Public Class PaginaBase
    Inherits System.Web.UI.Page
    Public Const SESSION_INFO As String = "infoCompany"
    Protected Form1 As HtmlForm
    Private cI As companyInfo
    Private _Reload As Boolean  'si se va a permitir esto


    '// Enums
#Region "Public Enum"


    Public Enum eUsuario
        uDefault = 0
        uHotelAdministrador = 1
        uPortalAdministrador = 2
    End Enum

    Public Enum modes As Byte
        Normal
        Extended
    End Enum

    Public Enum acciones As Integer
        Crear '0
        Modificar '1
        Eliminar '2
        LogIn '3
        Publicar '4
        Ver '5
        Reactivar '6
    End Enum

    Public Enum PerfilHotel
        Basico
        Medio
        Avanzado
        NetRate
        Mixto
        Casa
    End Enum

    Public Enum pages As Byte
        Home
        SearchHotel
        Users
        InfProp
        InfAmenities
        Activities
        AreaAtraction
        Policies
        FoodInformation
        RoomAmenities
        RoomsInformation
        MapLocation
        Photo
        ChangeLogo
        ReservaDetails
        RatePlanInventory
        RoomType
        InfoHotel
        ReservationList
        ConfirmReservas
        NoConfirmReservas
        ResByChanel
        Booking
        ChangePassword
        tarjetas
        Logs
        Rooms
        linkroom
        rateplanrules
        rateplans
        rateplanlinks
        FaresCatalogue
        FaresCatalogueNr
        NoRatesReport
        ratechart
        HotelStatus
        InventarioHotel
        InventarioRatePlans
        MainInvoicing
        FaresCopy
        Welcome
        InvoiceByPeriod
        IsDefaulter
        Deposito
        DetailLog
        ReservaDetailsV2
        DisplayCarReservation
        ItineraryDetails
        WaitList
    End Enum

#End Region

#Region "propiedades"

    'Public ReadOnly Property iduser() As Integer
    '    Get
    '        With New HotelSistema 'Procedimientos almacenados 
    '            Dim corporated As DataSet = .GetCorporativos(Me.Usuario)
    '            Dim idcoporate As Integer = corporated.Tables(0).Rows(0).Item("idcorporativo")
    '        End With
    '        Return 5532
    '    End Get
    'End Property

    Property ReloadMe() As Boolean
        Get
            Return _Reload
        End Get
        Set(ByVal Value As Boolean)
            _Reload = Value
        End Set
    End Property

    Public ReadOnly Property UserIdentityName() As Integer
        Get
            Return (New AuthUser).Usuario
        End Get
    End Property


    '// « Propiedades permisos. »
    Public ReadOnly Property IsContent() As Boolean
        Get
            Return (New AuthUser).IsContent
        End Get
    End Property

    Public ReadOnly Property IsHotel() As Boolean
        Get '// HotelCompany
            Return (New AuthUser).IsHotel
        End Get
    End Property

    Public ReadOnly Property IsSupervisor() As Boolean
        Get
            Return (New AuthUser).IsSupervisor
        End Get
    End Property

    Public ReadOnly Property IsUnibilling() As Boolean
        Get
            Return (New AuthUser).IsUnibilling
        End Get
    End Property

    Public ReadOnly Property isUserChain() As Boolean
        Get
            Return (New AuthUser).isUserChain
        End Get
    End Property

    Public ReadOnly Property IsUsuarioHotelAssociation() As Boolean
        Get
            Return (New AuthUser).IsUsuarioHotelAssociation
        End Get
    End Property

    Public ReadOnly Property IsUsuarioNetRates() As Boolean
        Get
            Return (New AuthUser).IsUsuarioNetRates
        End Get
    End Property

    Public ReadOnly Property IsUsuarioHotel() As Boolean
        Get
            Return (New AuthUser).IsUsuarioHotel
        End Get
    End Property

    Public ReadOnly Property IsUsuarioNivelHotel() As Boolean
        Get
            Return (New AuthUser).IsUsuarioHotelAvanzado Or (New AuthUser).IsUsuarioHotelMedio _
                    Or (New AuthUser).IsUsuarioHotelBasico Or (New AuthUser).IsUsuarioHotelNetRate
        End Get
    End Property

    Public ReadOnly Property IsAuthenticated() As Boolean
        Get
            Return ((New AuthUser).IsAuthenticated)
        End Get
    End Property

    Public ReadOnly Property UserInfoArray() As String()
        Get
            Return ((New AuthUser).UserInfoArray)
        End Get
    End Property

    Public ReadOnly Property IsUsuarioCallCenter() As Boolean
        Get
            Return ((New AuthUser).IsUsuarioCallCenter)
        End Get
    End Property
    Public ReadOnly Property IsUsuarioCasas() As Boolean
        Get
            Return ((New AuthUser).IsUsuarioCasas)
        End Get
    End Property

    Public ReadOnly Property IsUsuarioMixto() As Boolean
        Get
            Return ((New AuthUser).IsUsuarioMixto)
        End Get
    End Property

    Public ReadOnly Property IsUsuarioHomeAgency() As Boolean
        Get
            Return ((New AuthUser).IsUsuarioHomeAgency)
        End Get
    End Property

    Public Property Usuario() As Integer
        Get
            If ViewState.Item("KEY_IDUSUARIO") Is Nothing Then
                ViewState.Item("KEY_IDUSUARIO") = 0
            End If
            Return ViewState.Item("KEY_IDUSUARIO")
        End Get
        Set(ByVal Value As Integer)
            ViewState.Item("KEY_IDUSUARIO") = Value
        End Set
    End Property

    Public ReadOnly Property CorporateName As String
        Get
            Dim sessionValues As companyInfo = Session(SESSION_INFO)
            Return sessionValues.CorporateName
        End Get
    End Property

    Public ReadOnly Property CorporateId As Integer
        Get
            Dim sessionValues As companyInfo = Session(SESSION_INFO)
            Return sessionValues.IdCorporate
        End Get
    End Property

    Public Property PermisoUser(ByVal permiso As String) As DerechoUsuario
        Get
            Return ViewState(permiso & "_PermisoUser")
        End Get
        Set(ByVal Value As DerechoUsuario)
            ViewState(permiso & "_PermisoUser") = Value
            Session(permiso & "_PermisoUser") = ViewState.Item(permiso & "_PermisoUser")
        End Set
    End Property

    Public ReadOnly Property IdCorporativoUserChain() As Integer
        Get

            Dim dsHotel As New Portal.General.Common.Data.HotelDatos
            Dim idCorporative As Integer = -1
            With New HotelSistema
                dsHotel = .GetHotelById(cInfoActual.Hotel)
                If Not dsEmpty(dsHotel) Then
                    If Not dsHotel.Tables(0).Rows(0).IsNull("idcorporativo") Then
                        idCorporative = dsHotel.Tables(0).Rows(0)("idcorporativo")
                    End If
                End If
            End With
            Return idCorporative

            'If Not Session("idCorporativoUserChain") Is Nothing Then
            '    Return CType(Session("idCorporativoUserChain"), Integer)
            'Else
            '    Return -1
            'End If
        End Get
    End Property

    Public ReadOnly Property IdAsociation() As Integer
        Get
            IdAsociation = cInfoActual.IdAsociation
            If Me.IsUsuarioHotelAssociation Then
                If Me.Session("idAsociacion") Is Nothing Then
                    Dim data As Portal.General.Common.Data.AdministratorData
                    With New Portal.General.Facade.cAdministratorSystem()
                        data = .GetAdminById(Me.Usuario)
                    End With
                    If data IsNot Nothing AndAlso data.Tables.Contains(data.ADMINISTRATOR_TABLE) AndAlso data.Tables(data.ADMINISTRATOR_TABLE).Rows.Count > 0 Then
                        Me.Session("idAsociacion") = data.Tables(data.ADMINISTRATOR_TABLE).Rows(0)("idAsociacion")
                    Else
                        Me.Session("idAsociacion") = 0
                    End If
                End If
                Integer.TryParse(Me.Session("idAsociacion"), IdAsociation)
            End If
        End Get
    End Property

    '// « Fin Propiedades permisos. »

    Public Property HeaderPanel() As eUsuario
        Get
            'Return viewstate("HeaderPanel")
            Return Session("HeaderPanel")
        End Get
        Set(ByVal Value As eUsuario)
            If Value <> Session("HeaderPanel") Then
                Me.ReloadMe = True
            End If
            ViewState("HeaderPanel") = Value
            Session("HeaderPanel") = Value
            Session("urlCurrent") = Request.RawUrl.ToString
        End Set
    End Property

    Public Property mode() As modes
        Get
            If ViewState.Item("KEY_MODE") Is Nothing Then
                ViewState.Item("KEY_MODE") = modes.Normal
            End If
            Return ViewState.Item("KEY_MODE")
        End Get
        Set(ByVal Value As modes)
            ViewState.Item("KEY_MODE") = Value
            HttpContext.Current.Session("Mode") = ViewState.Item("KEY_MODE")
        End Set
    End Property

    Public ReadOnly Property IsHotelSelected() As Boolean
        Get
            Return Me.cInfoActual.Hotel > 0
        End Get
    End Property

    Public ReadOnly Property IdIdiomaMenu()
        Get
            Return ((New AuthUser).IdIdiomaMenu)
        End Get
    End Property

    Public Property IdIdiomaLogin() As String
        Get
            Return ((New AuthUser).IdIdiomaLogin)
        End Get
        Set(ByVal Value As String)
            Dim AuthUser As New AuthUser
            AuthUser.IdIdiomaLogin = Value
        End Set
    End Property

    Public WriteOnly Property InitIdiomaLogin() As String
        Set(ByVal Value As String)
            Dim AuthUser As New AuthUser
            AuthUser.InitIdiomaLogin = Value
        End Set
    End Property

    '//''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Function Habilitaboton(ByVal PermisoName As String, ByVal btn As Object, ByVal Permiso As String)
        If Me.IsUsuarioHotel Then
            If Not Me.PermisoUser(PermisoName) Is Nothing Then
                If PermisoUser(PermisoName).Permiso.ToString.ToUpper.IndexOf(Permiso) <> -1 Then
                    btn.Enabled = True
                Else
                    btn.Enabled = True
                End If
            Else
                btn.Enabled = True
            End If
        End If
    End Function

    Public Property HotelUsuarioPermisos() As PermisosData
        Get
            Dim dsPermisosData As PermisosData

            Try
                Dim cInfo As companyInfo = Session(PaginaBase.SESSION_INFO)

                If IsHotelSelected AndAlso Not cInfo Is Nothing Then

                    If Not Session("HotelUsuarioPermisos") Is Nothing Then
                        dsPermisosData = CType(Session("HotelUsuarioPermisos"), PermisosData)
                    End If
                    If dsPermisosData Is Nothing Then
                        With New PermisosFacade
                            dsPermisosData = .PermisosGetByUser(Usuario)
                            Session("HotelUsuarioPermisos") = dsPermisosData
                        End With
                    End If
                    Return dsPermisosData
                Else
                    Return New PermisosData
                End If

            Catch ex As Exception
                Return Nothing
            End Try
        End Get
        Set(ByVal Value As PermisosData)
            Session("HotelUsuarioPermisos") = Value
        End Set
    End Property

    Public Property cInfoActual() As companyInfo
        Get
            If ViewState.Item("KEY_INFO") Is Nothing Then
                ViewState.Item("KEY_INFO") = New companyInfo
            End If
            Return ViewState.Item("KEY_INFO")
        End Get
        Set(ByVal Value As companyInfo)
            cI = Session(SESSION_INFO)
            If cI.Hotel <> Value.Hotel Then
                'If cI.Hotel > 0 Then
                'If (Value.IsHouse Or cI.IsHouse) AndAlso Not IsSupervisor Then
                '    If Not IsUsuarioHotel Then crtUserCookie(Value.UserPerfil) ' se volvio a hacer para que si fuera casa también incluyera al UserChain
                '    Session("menu") = "true"
                '    Session("LogIn") = "false"
                'End If
                If IsUsuarioHomeAgency And IsSupervisor Then
                    ' e()
                End If

                If Not IsSupervisor AndAlso Not IsUnibilling AndAlso Not IsContent AndAlso (Not isUserChain Or Value.IsHouse Or cI.IsHouse) AndAlso Not IsUsuarioHomeAgency AndAlso Not IsUsuarioHotelAssociation _
                AndAlso Not IsUsuarioCallCenter Then
                    If Not IsUsuarioHotel Then crtUserCookie(Value.UserPerfil) ' No crear la Cookie si es usuario Hotel pero que si cargue el menu de nuevo
                    Session("menu") = "true"
                    Session("LogIn") = "false"
                End If
                If Me.IsHotel Then

                End If
            Else
                '// Vuelve a leer el perfil del hotel .
                If Not IsSupervisor AndAlso Not IsUnibilling AndAlso Not IsContent AndAlso Not isUserChain AndAlso Not IsUsuarioHomeAgency AndAlso Not IsUsuarioHotelAssociation _
                AndAlso Not IsUsuarioCallCenter Then
                    If Not IsUsuarioHotel Then crtUserCookie(Value.UserPerfil) ' No crear la Cookie si es usuario Hotel pero que si cargue el menu de nuevo
                End If
            End If
            ViewState.Item("KEY_INFO") = Value
            Session(SESSION_INFO) = ViewState.Item("KEY_INFO")
        End Set
    End Property

#End Region

#Region "Funciones Registro Scripts"

    Public Sub ResizefrmPrincipal()
        'Dim script As StringBuilder = New StringBuilder()
        'Dim scriptUrl As String

        'scriptUrl = String.Concat(Request.ApplicationPath, "/Utils_Iframe.js").Replace("//", "/")
        'script.Append(String.Format("<script type=""text/javascript"" src='{0}'></script>", scriptUrl))
        'script.Append("<SCRIPT type=""text/javascript""> ")
        'script.Append("if (resizeIframe)resizeIframe('frmPrincipal');")
        'script.Append("</SCRIPT>")
        'Page.ClientScript.RegisterStartupScript(Me.GetType(), Me.ClientID, script.ToString())


        ''esto ya esta comentado
        ''Page.RegisterStartupScript("UrlScript", "<script src='" & Request.ApplicationPath & "/Utils_Iframe.js' type='text/javascript'></script>")
        ''Me.Page.RegisterOnSubmitStatement("FrameResizeLevel2", "if (resizeIframe)resizeIframe('frmPrincipal');")
        ''Page.RegisterStartupScript("ResizePage", "<script>if (resizeIframe)resizeIframe('frmPrincipal');</script>")
    End Sub

    Public Sub SetFocus(ByVal ctrl As Control)
        ' Define the JavaScript function for the specified control.
        Dim focusScript As String = "<script language='javascript'>" & _
          "document.getElementById('" + ctrl.ClientID & _
          "').focus();</script>"

        ' Add the JavaScript code to the page.
        'RegisterStartupScript("FocusScript", focusScript)
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "FocusScript", focusScript)
    End Sub

    Public Sub DefaultButton(ByRef objTextControl As TextBox, ByVal objDefaultButton As Object)
        Dim sScript As New System.Text.StringBuilder
        sScript.Append("<SCRIPT language=""javascript"">" & vbCrLf)
        sScript.Append("function fnTrapKD(btn){" & vbCrLf)
        sScript.Append(" if (document.all){" & vbCrLf)
        sScript.Append("   if (event.keyCode == 13)" & vbCrLf)
        sScript.Append("   { " & vbCrLf)
        sScript.Append("     event.returnValue=false;" & vbCrLf)
        sScript.Append("     event.cancel = true;" & vbCrLf)
        sScript.Append("     btn.click();" & vbCrLf)
        sScript.Append("   } " & vbCrLf)
        sScript.Append(" } " & vbCrLf)
        sScript.Append("}" & vbCrLf)
        sScript.Append("</SCRIPT>" & vbCrLf)
        objTextControl.Attributes.Add("onkeydown", "fnTrapKD(document.getElementById('" & objDefaultButton.ClientID & "'))")
        'RegisterStartupScript("ForceDefaultToScript", sScript.ToString)
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "ForceDefaultToScript", sScript.ToString)
    End Sub

#End Region

#Region "Funciones logs"

    Public Function Leerlog(ByVal f1 As DateTime, ByVal f2 As DateTime, ByVal idhotel As Integer) As LogData
        With New LogFacade
            If IsSupervisor Then
                Return .GetLog(f1, f2, idhotel)
            Else
                Return .GetLog(f1, f2, idhotel, Usuario)
            End If

        End With
    End Function

    Public Property HotelInfo() As DataRow
        Get
            Dim dsHotel As New Portal.General.Common.Data.HotelDatos
            Dim drHotel As DataRow
            Dim hr As Boolean
            Try
                hr = False
                If Not IsNothing(Session("HotelInfo")) Then
                    drHotel = CType(Session("HotelInfo"), DataRow)
                    If (drHotel("idHotel") <> cInfoActual.Hotel) Then
                        hr = True
                    End If
                Else
                    hr = True
                End If
                If (hr) Then
                    With New HotelSistema
                        dsHotel = .GetHotelById(cInfoActual.Hotel)
                        If Not dsHotel Is Nothing AndAlso dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows.Count > 0 Then
                            drHotel = dsHotel.Tables(HotelDatos.HOTEL_TABLE).Rows(0)
                            Session("HotelInfo") = drHotel
                        End If
                    End With
                End If

                Return Session("HotelInfo")
            Catch ex As Exception
                Return Nothing
            End Try
        End Get
        Set(ByVal Value As DataRow)
            Session("HotelInfo") = Value
        End Set
    End Property

    Public Function isConfigAdolescente() As Boolean
        Dim dsHotel As New Portal.General.Common.Data.HotelDatos
        Dim drhotel As DataRow
        With New HotelSistema
            dsHotel = .GetHotelById(cInfoActual.Hotel)
            If Not dsHotel Is Nothing AndAlso dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows.Count > 0 Then
                drhotel = dsHotel.Tables(HotelDatos.HOTEL_TABLE).Rows(0)
                Return Not drhotel.IsNull("EdadAdolecente")
            End If
        End With
        Return False
    End Function


    Public Sub guardalog(ByVal pagina As String, ByVal action As acciones, ByVal nota As String)
        Try

            If ReadUserCookie.GetValue(0) <> "" Then
                Dim ds As LogData = New LogData
                Dim dr As DataRow = ds.Tables(LogData.TABLE_LOG).NewRow
                dr(LogData.FIELD_USUARIO) = ReadUserCookie.GetValue(0)
                dr(LogData.FIELD_PAGINA) = pagina
                dr(LogData.FIELD_ACCION) = action
                If cInfoActual.Hotel <> 0 Then
                    dr(LogData.FIELD_HOTEL) = cInfoActual.Hotel
                End If
                dr(LogData.FIELD_NOTA) = nota

                Try
                    dr(LogData.FIELD_FECHA) = Now.ToString("MM/dd/yyyy") & " " & Now.ToLongTimeString
                Catch
                    dr(LogData.FIELD_FECHA) = Now.ToString & " " & Now.ToLongTimeString
                End Try

                ds.Tables(LogData.TABLE_LOG).Rows.Add(dr)
                With New LogFacade
                    .insertLog(ds)
                End With
            End If
        Catch

        End Try
    End Sub

    Public Sub guardalog(ByVal pagina As String, ByVal action As acciones, ByVal nota As String,
                         ByVal peticion As String, ByVal datos As String, ByVal datosDespues As String,
                         Optional ByVal hotelId As Integer = 0)
        Try

            If ReadUserCookie.GetValue(0) <> "" Then
                Dim ds As LogData = New LogData
                Dim dr As DataRow = ds.Tables(LogData.TABLE_LOG).NewRow
                dr(LogData.FIELD_USUARIO) = ReadUserCookie.GetValue(0)
                dr(LogData.FIELD_PAGINA) = pagina
                dr(LogData.FIELD_ACCION) = action
                If cInfoActual.Hotel <> 0 Then
                    dr(LogData.FIELD_HOTEL) = cInfoActual.Hotel
                ElseIf hotelId <> 0 Then
                    dr(LogData.FIELD_HOTEL) = hotelId
                End If
                dr(LogData.FIELD_NOTA) = nota
                If Not String.IsNullOrEmpty(datos) Then dr(LogData.FIELD_DATOS) = datos
                If Not String.IsNullOrEmpty(datosDespues) Then dr(LogData.FIELD_DATOSDESPUES) = datosDespues

                Try
                    dr(LogData.FIELD_FECHA) = Now.ToString("MM/dd/yyyy") & " " & Now.ToLongTimeString
                Catch
                    dr(LogData.FIELD_FECHA) = Now.ToString & " " & Now.ToLongTimeString
                End Try

                ds.Tables(LogData.TABLE_LOG).Rows.Add(dr)
                With New LogFacade
                    .insertLog(ds)
                End With

                'Dim dsHotel As New Portal.General.Common.Data.HotelDatos
                'Dim drHotel As DataRow

                'With New HotelSistema
                '    dsHotel = .GetHotelById(cInfoActual.Hotel)
                '    If Not dsHotel Is Nothing AndAlso dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows.Count > 0 Then
                '        drHotel = dsHotel.Tables(HotelDatos.HOTEL_TABLE).Rows(0)
                '        If (New Util.Emails).MandarCorreoLogHotel(dr(LogData.FIELD_USUARIO), peticion, nota, datos, datosDespues, drHotel) Then

                '        End If
                '    End If
                'End With
            End If
        Catch

        End Try
    End Sub

    Public Sub guardalog(ByVal pagina As String, ByVal action As acciones, ByVal nota As String, _
                         ByVal peticion As String, ByVal datos As String, ByVal datosDespues As String, ByVal sdatoCorreo As String)
        ' Try

        If ReadUserCookie.GetValue(0) <> "" Then
            Dim ds As LogData = New LogData
            Dim dr As DataRow = ds.Tables(LogData.TABLE_LOG).NewRow
            dr(LogData.FIELD_USUARIO) = ReadUserCookie.GetValue(0)
            dr(LogData.FIELD_PAGINA) = pagina
            dr(LogData.FIELD_ACCION) = action
            If cInfoActual.Hotel <> 0 Then
                dr(LogData.FIELD_HOTEL) = cInfoActual.Hotel
            End If
            dr(LogData.FIELD_NOTA) = nota
            If Not String.IsNullOrEmpty(datos) Then dr(LogData.FIELD_DATOS) = datos
            If Not String.IsNullOrEmpty(datosDespues) Then dr(LogData.FIELD_DATOSDESPUES) = datosDespues

            Try
                dr(LogData.FIELD_FECHA) = Now.ToString("MM/dd/yyyy") & " " & Now.ToLongTimeString
            Catch
                dr(LogData.FIELD_FECHA) = Now.ToString & " " & Now.ToLongTimeString
            End Try

            ds.Tables(LogData.TABLE_LOG).Rows.Add(dr)
            With New LogFacade
                .insertLog(ds)
            End With

            Dim dsHotel As New Portal.General.Common.Data.HotelDatos
            Dim drHotel As DataRow

            With New HotelSistema
                dsHotel = .GetHotelById(cInfoActual.Hotel)
                If Not dsHotel Is Nothing AndAlso dsHotel.Tables(dsHotel.HOTEL_TABLE).Rows.Count > 0 Then
                    drHotel = dsHotel.Tables(HotelDatos.HOTEL_TABLE).Rows(0)
                    If (New Util.Emails).MandarCorreoLogHotel(dr(LogData.FIELD_USUARIO), cInfoActual.HotelName, peticion, nota, sdatoCorreo, "", drHotel) Then
                    End If
                End If
            End With
        End If
        'Catch

        'End Try
    End Sub
#End Region

#Region "Funciones Cookies, Viewstate, Session"

    Public Function ReadUserCookie() As String()
        Dim UserInfoArray As String() = {""}
        Try
            '// Obtenemos el usuario de la session.
            UserInfoArray = CType((New AuthUser).UserInfoName, String).Split(",")
        Catch e As Exception
        Finally
        End Try
        Return (UserInfoArray)
    End Function

    Private Function crtUserCookie(ByVal perfil As PerfilHotel) As HttpCookie
        'Dim tkt As FormsAuthenticationTicket
        'Dim cookiestr As String
        'Dim ck As HttpCookie
        Dim roles As String = String.Empty
        'Dim stmp As String

        Try
            Select Case perfil
                Case CInt(PerfilHotel.Avanzado)
                    roles = "HotelAvanzado"
                Case CInt(PerfilHotel.Basico)
                    roles = "HotelBasico"
                Case CInt(PerfilHotel.Medio)
                    roles = "HotelMedio"
                Case CInt(PerfilHotel.NetRate)
                    roles = "HotelNetRate"
                Case CInt(PerfilHotel.Mixto)
                    roles = "HotelAvanzado,HotelNetRate"
            End Select

            If Session("RolesUsuario") IsNot Nothing Then
                If Not String.IsNullOrEmpty(roles) Then
                    For Each r As String In roles.Split(",")
                        If Not Session("RolesUsuario").ToString.Contains(r) Then
                            Session.Item("RolesUsuario") += "," & r
                        End If
                    Next
                End If
            End If
        Catch ex As Exception

        End Try
    End Function

    Private Sub reloadViewstate()
        With HttpContext.Current
            If Not .Session(SESSION_INFO) Is Nothing AndAlso CType(.Session(SESSION_INFO), companyInfo).Hotel > 0 Then
                Me.cInfoActual = .Session(SESSION_INFO)
                mode = .Session("Mode")
            Else
                .Session(SESSION_INFO) = Me.cInfoActual
                .Session("Mode") = mode
            End If


        End With
    End Sub

#End Region

#Region "Funciones Permisos"

    Public Sub clearviewstatepermisos()
    End Sub

    Public Function PermissionSeePage(ByVal url As String) As Boolean
        If IsUsuarioHotel Then
            Dim nombrePagina As String = url.ToUpper()
            Dim dsP As PermisosData = HotelUsuarioPermisos

            For Each dr As DataRow In dsP.Tables(PermisosData.PermisosTable).Rows
                If nombrePagina = dr("PermisoName").toupper Then
                    Return True
                End If
            Next
        End If
        Return False
    End Function

    Public Sub PermissionSeePage()
        Dim paginaContext As String

        If IsUsuarioHotel Then
            Dim paginaSolicitada As String() = (Context.Request.Path).Split("/")
            If paginaSolicitada.Length > 0 Then
                Dim nombrePagina As String = paginaSolicitada.GetValue(paginaSolicitada.Length - 1)
                Dim dsP = HotelUsuarioPermisos
                Dim show As Boolean = False

                If nombrePagina.Length > 0 Then
                    nombrePagina = nombrePagina.ToUpper

                    If nombrePagina.ToUpper = "HOME.ASPX" Then
                        show = True
                        'ElseIf nombrePagina.ToUpper = "WELCOME.ASPX" Then
                        '    show = True
                    ElseIf nombrePagina = "SEARCH.ASPX" Then
                        show = True
                        'ElseIf nombrePagina = "DEFAULT.ASPX" Then
                        '    show = True
                        'ElseIf nombrePagina = "INDEX.ASPX" Then
                        '    show = True
                        'ElseIf nombrePagina = "RESERVATIONDETAILS.ASPX" Then
                        '    show = True
                        'ElseIf nombrePagina = "INVOICETOCONCILIATEDETAILS.ASPX" Then
                        '    show = True
                        'ElseIf nombrePagina = "HOTELINVOICEDETAILS.ASPX" Then
                        '    show = True
                        'ElseIf nombrePagina = "PAYMENTREGISTERED.ASPX" Then
                        '    show = True
                    ElseIf (PermissionPage(nombrePagina)) Then
                        show = True
                    Else
                        nombrePagina = String.Format("{0}/{1}", Me.Page.TemplateSourceDirectory.ToUpper, nombrePagina)
                        paginaContext = HttpContext.Current.Request.ApplicationPath & "/"
                        paginaContext = nombrePagina.Replace(paginaContext.ToUpper, "/").ToUpper
                        For Each dr As DataRow In dsP.Tables(0).rows
                            If dr("PermisoName").ToString.ToUpper.Contains(paginaContext) Then
                                'If nombrePagina.Replace("//", "/") = dr("PermisoName").replace("//", "/").toupper Then
                                show = True
                                Exit For
                            End If
                        Next
                    End If

                End If


                If Not show Then
                    redirectTo(pages.Welcome)
                End If
            End If
        End If
    End Sub

    Private Function isInternetPowerUser() As Boolean

        '1:MISION(),3:HOSPITALITYANDFUN(),94:NAVEMEX(),4:BELIVEIN(vcihotels),43 Service Tour(MET), aso:1 Aso Hem
        Return Not (cInfoActual.IdCorporate = 0 _
        Or cInfoActual.IdCorporate = 1 _
        Or cInfoActual.IdCorporate = 3 _
        Or cInfoActual.IdCorporate = 94 _
        Or cInfoActual.IdCorporate = 4 _
        Or cInfoActual.IdCorporate = 43 _
        Or cInfoActual.IdCorporate = 170 _
        Or cInfoActual.IdAsociation = 1 _
        Or cInfoActual.IdCorporate = 274)
    End Function
    Function PermisionContentWelcome(ByVal nombrePagina As String) As Boolean
        Dim paginaContext As String
        Dim show As Boolean = False

        If Not IsUsuarioHotel Then Return True

        Dim dsP = HotelUsuarioPermisos
        nombrePagina = nombrePagina.ToUpper
        ' nombrePagina = String.Format("{0}/{1}", Me.Page.TemplateSourceDirectory.ToUpper, nombrePagina)
        paginaContext = HttpContext.Current.Request.ApplicationPath & "/"
        paginaContext = nombrePagina.Replace(paginaContext.ToUpper, "/").ToUpper
        For Each dr As DataRow In dsP.Tables(0).rows
            If dr("PermisoName").ToString.ToUpper.Contains(nombrePagina) Then
                'If nombrePagina.Replace("//", "/") = dr("PermisoName").replace("//", "/").toupper Then
                show = True
                Exit For
            End If
        Next
        Return show
    End Function

#End Region

#Region "Direccionar Paginas"

    Public Function UrlPage(ByVal page As pages) As String
        Dim strpage As String = ""
        Dim sRequestApplicationPath As String = Request.ApplicationPath

        Select Case page
            Case pages.Home
                strpage = sRequestApplicationPath & "/Portal/Pages/Welcome.aspx"
            Case pages.SearchHotel
                strpage = sRequestApplicationPath & "/Portal/Pages/Search.aspx"
            Case pages.ReservaDetails
                If AppSettings("idSegmento") = "4" Then
                    strpage = sRequestApplicationPath & "/HotelAdministrator/Pages/ReservationDetailsV2.aspx"
                Else
                    strpage = sRequestApplicationPath & "/HotelAdministrator/Pages/ReservationDetails.aspx"
                End If
            Case pages.DisplayCarReservation
                strpage = sRequestApplicationPath & "/HotelAdministrator/Pages/DisplayCarReservation.aspx"
            Case pages.ReservaDetailsV2
                If AppSettings("idSegmento") = "4" Then
                    strpage = sRequestApplicationPath & "/HotelAdministrator/Pages/ReservationDetailsV2.aspx"
                Else
                    strpage = sRequestApplicationPath & "/HotelAdministrator/Pages/ReservationDetails.aspx"
                End If
            Case pages.RatePlanInventory
                strpage = sRequestApplicationPath & "/Pages/RatePlanInventory.aspx"
            Case pages.Policies
                strpage = sRequestApplicationPath & "/Registro/PoliciesInformation.aspx"
            Case pages.FoodInformation
                strpage = sRequestApplicationPath & "/Registro/FoodInformation.aspx"
            Case pages.RoomAmenities
                strpage = sRequestApplicationPath & "/Registro/RoomAmenitiesInformation.aspx"
            Case pages.RoomsInformation
                strpage = sRequestApplicationPath & "/Registro/RoomsInformation.aspx"
            Case pages.MapLocation
                strpage = sRequestApplicationPath & "/Registro/MapLocationInformation.aspx"
            Case pages.Photo
                strpage = sRequestApplicationPath & "/Registro/PhotoInformation.aspx"
            Case pages.ChangeLogo
                strpage = sRequestApplicationPath & "/Registro/ChangeLogo.aspx"
            Case pages.RoomType
                strpage = sRequestApplicationPath & "/HotelAdministrator/PagesGeneral/RoomType.aspx"
            Case pages.InfoHotel
                strpage = sRequestApplicationPath & "/HotelAdministrator/PagesGeneral/Hotel.aspx"
            Case pages.ReservationList
                If AppSettings("idSegmento") = "4" Then
                    strpage = sRequestApplicationPath & "/HotelAdministrator/Pages/ReservationsV2.aspx"
                Else
                    strpage = sRequestApplicationPath & "/HotelAdministrator/Pages/Reservations.aspx"
                End If
            Case pages.ConfirmReservas
                strpage = sRequestApplicationPath & "/HotelAdministrator/Pages/ConfirmReservas.aspx"
            Case pages.NoConfirmReservas
                strpage = sRequestApplicationPath & "/HotelAdministrator/Pages/NoConfirmReservas.aspx"
            Case pages.ResByChanel
                strpage = sRequestApplicationPath & "/HotelAdministrator/Pages/ReservationsReport.aspx"
            Case pages.Booking
                strpage = sRequestApplicationPath & "/CallCenter/Booking.aspx"
            Case pages.ChangePassword
                strpage = sRequestApplicationPath & "/User/ChangePassword.aspx"
            Case pages.tarjetas
                strpage = sRequestApplicationPath & "/HotelAdministrator/Pages/TarjetasHotel.aspx"
            Case pages.Logs
                strpage = sRequestApplicationPath & "/Portal/Pages/LogReport.aspx"
            Case pages.SearchHotel
                strpage = sRequestApplicationPath & "/Portal/Pages/Search.aspx"
            Case pages.Users
                strpage = sRequestApplicationPath & "/Pages/Users.aspx"
            Case pages.Rooms
                strpage = sRequestApplicationPath & "/Pages/Rooms.aspx"
            Case pages.linkroom
                strpage = sRequestApplicationPath & "/Pages/RoomsLinks.aspx"
            Case pages.rateplanrules
                strpage = sRequestApplicationPath & "/Pages/RatePlansRules.aspx"
            Case pages.rateplans
                strpage = sRequestApplicationPath & "/Pages/RatesPlans.aspx"
            Case pages.rateplanlinks
                strpage = sRequestApplicationPath & "/Pages/RatePlansLinks.aspx"
            Case pages.FaresCatalogue
                strpage = sRequestApplicationPath & "/Pages/FaresCatalogue.aspx"
            Case pages.FaresCatalogueNr
                strpage = sRequestApplicationPath & "/Pages/FaresCatalogueNr.aspx"
            Case pages.AreaAtraction
                strpage = sRequestApplicationPath & "/Registro/AttractionInformation.aspx"
            Case pages.Activities
                strpage = sRequestApplicationPath & "/Registro/ActivitiesInformation.aspx"
            Case pages.InfAmenities
                strpage = sRequestApplicationPath & "/Registro/AmenitiesInformation.aspx"
            Case pages.InfProp
                strpage = sRequestApplicationPath & "/Registro/InformacionPropiedad.aspx"
            Case pages.NoRatesReport
                strpage = sRequestApplicationPath & "/Pages/NoRatesReport.aspx"
            Case pages.ratechart
                strpage = sRequestApplicationPath & "/Pages/RateChart.aspx"
            Case pages.HotelStatus
                strpage = sRequestApplicationPath & "/Pages/AvailabilityRestrictions.aspx"
            Case pages.InventarioHotel
                strpage = sRequestApplicationPath & "/Pages/HomePage.aspx"
            Case pages.InventarioRatePlans
                strpage = sRequestApplicationPath & "/Pages/SegmentRoomsAvailability.aspx"
            Case pages.MainInvoicing
                strpage = sRequestApplicationPath & "/HotelAdministrator/Invoicing/MainInvoicing.aspx"
            Case pages.FaresCopy
                strpage = sRequestApplicationPath & "/Pages/FaresCopy.aspx"
            Case pages.Welcome
                strpage = sRequestApplicationPath & "/Portal/Pages/Welcome.aspx"
            Case pages.InvoiceByPeriod
                strpage = sRequestApplicationPath & "/HotelAdministrator/Invoicing/InvoiceByPeriod.aspx"
            Case pages.IsDefaulter
                strpage = sRequestApplicationPath & "/Pages/PresentaAdeudo.aspx"
            Case pages.Deposito
                strpage = sRequestApplicationPath & "/HotelAdministrator/Pages/Deposits.aspx"
            Case pages.DetailLog
                strpage = sRequestApplicationPath & "/Portal/Pages/LogDetalle.aspx"
            Case page.ItineraryDetails
                strpage = sRequestApplicationPath & "/HotelAdministrator/Pages/DisplayItinerary.aspx"
            Case page.WaitList
                strpage = sRequestApplicationPath & "/HotelAdministrator/Pages/WaitList.aspx"
        End Select
        strpage = strpage.Replace("//", "/")
        Return strpage
    End Function

    Public Function GeRequestApplicationPath(ByVal page As String) As String
        Dim sreq As String
        sreq = String.Concat(Request.ApplicationPath, page).Replace("//", "/").Replace("//", "/")
        Return sreq
    End Function

    Public Sub redirectTo(ByVal page As pages, Optional ByVal queryString As String = Nothing)
        Response.Redirect(UrlPage(page) & queryString)
    End Sub

#End Region

#Region "Funciones Menu Hotel"

    Private Function GetRolHotel(ByVal perfil As PerfilHotel) As String

        Dim roles As String
        Select Case perfil
            Case CInt(PerfilHotel.Avanzado)
                roles = "HotelAvanzado"
            Case CInt(PerfilHotel.Basico)
                roles = "HotelBasico"
            Case CInt(PerfilHotel.Medio)
                roles = "HotelMedio"
            Case CInt(PerfilHotel.NetRate)
                roles = "HotelNetRate"
            Case CInt(PerfilHotel.Mixto)
                roles = "HotelAvanzado,HotelNetRate"
            Case Else
                roles = "Casas"
        End Select
        Return roles
    End Function

    Private Function RolInRoles(ByVal rol As String, ByVal roles As String) As Boolean
        Dim r As String

        If roles.Trim = String.Empty Then
            Return True
        End If
        For Each src As String In rol.Split(",")
            For Each r In roles.Split(",")
                If (src.ToUpper = r.ToUpper) Then
                    Return True
                End If
            Next
        Next
        Return False
    End Function

    Private Sub ReadNodeMenu(ByRef doc As XmlDocument, ByRef nodos As XmlNodeList, ByVal isSubNodo As Boolean, ByVal dsP As PermisosData, ByVal rol As String, ByRef menuResult As String)
        If nodos.Count > 0 Then
            For Each nodo As XmlNode In nodos
                Dim nText As String = String.Empty
                Dim nUrl As String = String.Empty
                Dim subNodo As XmlNode = Nothing
                Dim nRoles As String = String.Empty
                Dim show As Boolean = True
                Dim dv As DataView

                For i As Integer = 0 To nodo.ChildNodes.Count - 1
                    Select Case nodo.ChildNodes.Item(i).Name
                        Case "text"
                            nText = nodo.ChildNodes.Item(i).InnerText
                        Case "url"
                            nUrl = nodo.ChildNodes.Item(i).InnerText
                        Case "subMenu"
                            subNodo = nodo.ChildNodes.Item(i)
                        Case "roles"
                            nRoles = nodo.ChildNodes.Item(i).InnerText
                    End Select
                Next

                show = True
                If isSubNodo Then
                    Dim paginaSolicitada As String = HttpContext.Current.Request.ApplicationPath & "/"
                    paginaSolicitada = nUrl.Replace(paginaSolicitada, "/")

                    dv = dsP.Tables(PermisosData.PermisosTable).DefaultView
                    dv.RowFilter = String.Format("PermisoName like '%{0}%'", paginaSolicitada)
                    show = dv.Count > 0
                End If

                If show AndAlso Not RolInRoles(rol, nRoles) Then
                    show = False
                End If

                If String.IsNullOrEmpty(nText) Then
                    show = False
                End If
                If show Then
                    menuResult += "<menuItem>"
                    menuResult += String.Format("<text> {0} </text>", nText)
                    If nUrl <> String.Empty Then menuResult += String.Format("<url>{0}</url>", nUrl)
                    If Not isSubNodo Then menuResult += "<cssclass>menustyle</cssclass>"
                    If Not isSubNodo Then menuResult += " <mouseovercssclass>mousemenu</mouseovercssclass>"
                End If

                If Not subNodo Is Nothing AndAlso show Then
                    menuResult += "<subMenu>"
                    ReadNodeMenu(doc, subNodo.ChildNodes, True, dsP, rol, menuResult)
                    menuResult += "</subMenu>"
                End If
                If show Then menuResult += "</menuItem>"
            Next
        End If
    End Sub

    Public Function creaMenuUsuarioHotel() As String
        Dim ds As PermisosData = HotelUsuarioPermisos
        Dim cMenu As New clsMenu
        Dim menu As String = String.Empty
        Dim currentRol As String = "UsuarioHotel"
        Dim cInfo As companyInfo = Session(PaginaBase.SESSION_INFO)
        Dim menuXml As String

        If IsHotelSelected AndAlso Not cInfo Is Nothing Then
            currentRol &= "," & GetRolHotel(cInfo.UserPerfil)
        End If

        Dim doc As XmlDocument = New XmlDocument
        Try
            menuXml = cMenu.GetMenu(AppSettings("idSistema"), Me.IdIdiomaMenu, "sys_dealsHotel")
            If menuXml <> String.Empty Then
                doc.LoadXml(menuXml)
                'doc.Load(Server.MapPath(AppSettings("MenuUsuarios") & PortalCulture.GetString("00000") & ".xml"))
                menu += "<?xml version=""1.0"" encoding=""utf-8"" ?>"
                menu += "<menu>"
                ReadNodeMenu(doc, doc.SelectNodes("/menu/menuItem"), False, ds, currentRol, menu)
                menu += "</menu>"
            End If

        Catch ex As Exception
            menu = ""
        End Try
        Return menu
    End Function


#End Region

    Function FCurrency(ByVal src As Double, ByVal Decimales As Byte)
        Dim sfrm As String = ""
        Dim tmpCur As String
        Dim sCur As String

        Try
            For i As Integer = 1 To Decimales
                sfrm = sfrm.Insert(0, "0")
            Next
            If Not String.IsNullOrEmpty(sfrm) Then sfrm = "." & sfrm

            tmpCur = String.Format("###,###,##0{0}", sfrm)
            sCur = src.ToString(tmpCur)
        Catch ex As Exception
            sCur = src.ToString()
        End Try
        Return sCur
    End Function


    Public Sub OTA_PushNotif(ByVal IdHotel)
        If Not String.IsNullOrEmpty(AppSettings("WsConnectWcf")) Then
            Dim Service As WsConnectWcf.wsConnectWCFv2 = New WsConnectWcf.wsConnectWCFv2()
            Service.Url = AppSettings("WsConnectWcf")
            Threading.Tasks.Task.Factory.StartNew(Sub() Service.ReservationPushNotifAsync(IdHotel))
        End If
    End Sub

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim log As String = Request.QueryString("lgid")
        Dim id As String = String.Empty


        If Not log Is Nothing Then
            Context.Session.Clear()
            Dim ck As HttpCookie
            Dim identity As Integer = CType(SessionTracking.Load(log, "UserLogged"), Integer)

            'Creamos una cookie de authentificacion... basada en roles
            ck = cSecurity.createCookie(identity, 15, False)
            Context.Response.Cookies.Add(ck)

            Dim infoUser As String() = cSecurity.GetRolesUser(identity)
            'Creamos una cookie para guardar informacion de usuario ..
            'Correo electronico, etc, etc'
            ck = cSecurity.createUserCookie(identity, CType(AppSettings("TimeCookie"), Integer), True, infoUser)
            Context.Response.Cookies.Add(ck)
        End If

        If Request.RawUrl.ToString.ToUpper.IndexOf("HOME.ASPX") = -1 Then
            Session("urlCurrent") = Request.RawUrl.ToString
        End If

        If Session("LogIn") = "true" Then
            If Not IsSupervisor AndAlso Not IsUnibilling AndAlso Not isUserChain And Not IsContent And Not IsUsuarioHotel And Not IsUsuarioCallCenter Then
                crtUserCookie(Me.cInfoActual.UserPerfil)
                Session("LogIn") = "false"
                Session("menu") = "true"
            End If
        End If

        Response.Expires = 0
        If Me.IsAuthenticated Then
            If IsHotel Or IsSupervisor Or IsUsuarioHotel Or Me.cInfoActual.UserPerfil = PerfilHotel.Avanzado Or
            Me.cInfoActual.UserPerfil = PerfilHotel.Basico Or Me.cInfoActual.UserPerfil = PerfilHotel.Medio Or
            Me.IsUnibilling Or Me.IsContent Or Me.cInfoActual.UserPerfil = PerfilHotel.NetRate Or IsUsuarioCallCenter Then
                Usuario = ((New AuthUser).Usuario)
            End If
        End If
        reloadViewstate()
        If isInternetPowerUser() Then

            Dim paginaSolicitada As String() = (Context.Request.Path).Split("/")
            If paginaSolicitada.Length > 0 Then
                Dim nombrePagina As String = paginaSolicitada.GetValue(paginaSolicitada.Length - 1).ToString().ToUpper()

                '*** No permitir acceso a contenido en caso de ser usuario internet power
                If nombrePagina = "HOTEL.ASPX" _
                    Or nombrePagina = "CHANGELOGO.ASPX" _
                    Or nombrePagina = "INFORMACIONPROPIEDAD.ASPX" _
                    Or nombrePagina = "AMENITIESINFORMATION.ASPX" _
                    Or nombrePagina = "ACTIVITIESINFORMATION.ASPX" _
                    Or nombrePagina = "ATTRACTIONINFORMATION.ASPX" _
                    Or nombrePagina = "POLICIESINFORMATION.ASPX" _
                    Or nombrePagina = "FOODINFORMATION.ASPX" _
                    Or nombrePagina = "ROOMAMENITIESINFORMATION.ASPX" _
                    Or nombrePagina = "ROOMSINFORMATION.ASPX" _
                    Or nombrePagina = "MAPLOCATIONINFORMATION.ASPX" _
                    Or nombrePagina = "EVENTSINFORMATION.ASPX" _
                    Or nombrePagina = "TRAVELSINFORMATION.ASPX" _
                    Or nombrePagina = "IMAGENES.ASPX" _
                    Or nombrePagina = "PHOTOINFORMATION.ASPX" _
                    Or nombrePagina = "GENERATORSCRIPTSEASON.ASPX" _
                    Or nombrePagina = "HOTELASOSIATIONCARS.ASPX" _
                    Or nombrePagina = "HOTELASOSIATIONACTIVITYS.ASPX" Then
                    Response.Redirect(Request.ApplicationPath & "/Unavaible.htm")
                End If
            End If

        ElseIf IsUsuarioHotel Then
            PermissionSeePage()
        End If

        If Context.Session("once_script") Is Nothing Then
            Dim script_iframeHeight As String = ""
            script_iframeHeight &= "<script>"
            script_iframeHeight &= "sendHeight = function(){ var height = $('div').offsetHeight; window.parent.postMessage({'height': height}, '*');}"
            script_iframeHeight &= "</script>"
            ClientScript.RegisterStartupScript(Me.GetType(), "clientScript", script_iframeHeight)
            Context.Session("once_script") = True
        End If

        If Me.isUserChain Then
            With New HotelSistema
                Dim corporated As DataSet = .GetCorporativos(Me.Usuario)
                Me.cInfoActual.IdCorporate = corporated.Tables(0).Rows(0).Item("idcorporativo")
                Me.cInfoActual.CorporateName = corporated.Tables(0).Rows(0).Item("NombreCorp")
            End With
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        If Not Form1 Is Nothing Then Form1.Attributes.Add("onsubmit", "if (typeof(isValidSubmit) == 'function' && isValidSubmit ) { isValidSubmit(); }")
        Dim cadScript As String = ""

        'cadScript = "<SCRIPT language=""javascript""> " & _
        '   " function estableceancho() " & vbCrLf & _
        '   " { var o; " & vbCrLf & _
        '   " o=document.getElementsByName('capa'); " & vbCrLf & _
        '   " if(o)" & vbCrLf & _
        '   " { for(var i=0; i<o.length;i++)" & vbCrLf & _
        '   " { o[i].style.display = '';" & vbCrLf & _
        '   " } window.status='" & PortalCulture.GetString("00277") & "';" & vbCrLf & _
        '   " } else { return false; } " & vbCrLf & _
        '   "}" & vbCrLf & _
        '   "</SCRIPT>" & vbCrLf
        'cadScript &= "<table id=""Capa"" WIDTH=100% HEIGHT=100% name=""capa"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""DISPLAY:none;FILTER:alpha(opacity=30);LEFT:0px;POSITION:absolute;TOP:0px;BACKGROUND-COLOR:gainsboro;moz-opacity:0.50"">" & _
        ' "<tr> <td></td> </tr> </table>"
        ''Page.RegisterClientScriptBlock("LayerCapa", cadScript)
        'Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "LayerCapa", cadScript)

        'loading  
        'Dim scriptLoading As New System.Text.StringBuilder
        'scriptLoading.Append("<script>document.getElementsById('loading').display = 'block'; alert('s');</script>")
        'If (Page.Master Is Nothing) Then
        '    Page.ClientScript.RegisterStartupScript(Me.GetType(), "onLoading", scriptLoading.ToString)
        'End If



        Dim sScript As New System.Text.StringBuilder

        If Session("menu") = "true" Then
            sScript.Append("<SCRIPT language=""javascript"">  " & vbCrLf)
            sScript.Append("   if (parent.UpdateMe) { parent.UpdateMe() };" & vbCrLf)
            sScript.Append("</SCRIPT>" & vbCrLf)
            'RegisterStartupScript("UPDATEME", sScript.ToString)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "UPDATEME", sScript.ToString)
        End If

        'Resize Iframe
        Dim scriptIframe As New System.Text.StringBuilder
        scriptIframe.Append("<SCRIPT language=""javascript"">" & vbCrLf)
        scriptIframe.Append("try{onResizeIframe();}catch(err){};" & vbCrLf)
        scriptIframe.Append("function onResizeIframe(addHig)" & vbCrLf)
        scriptIframe.Append("{" & vbCrLf)
        'scriptIframe.Append("   var e = {scrollHeight: document.body.scrollHeight,scrollWidth: document.body.scrollWidth,};" & vbCrLf)
        scriptIframe.Append("   parent.calcHeight(addHig);" & vbCrLf)
        'scriptIframe.Append("   alert(e.scrollHeight);" & vbCrLf)
        scriptIframe.Append("}" & vbCrLf)
        scriptIframe.Append("</SCRIPT>" & vbCrLf)
        If (Page.Master Is Nothing) Then
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "onResizeIframe", scriptIframe.ToString)
        End If


        'SetOptionMenu
        Dim scriptSetOptionMenu As New System.Text.StringBuilder
        scriptIframe.Append("<SCRIPT language=""javascript"">" & vbCrLf)
        scriptIframe.Append("try{onSetOptionMenu();}catch(err){};" & vbCrLf)
        scriptIframe.Append("function onSetOptionMenu()" & vbCrLf)
        scriptIframe.Append("{" & vbCrLf)
        scriptIframe.Append("   parent.SetOptionMenu(document.URL);" & vbCrLf)
        'scriptIframe.Append("   alert(e.scrollHeight);" & vbCrLf)
        scriptIframe.Append("}" & vbCrLf)
        scriptIframe.Append("</SCRIPT>" & vbCrLf)
        If (Page.Master Is Nothing) Then
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "onSetOptionMenu", scriptIframe.ToString)
        End If

    End Sub

    Public Function GetXMLFromObject(o As Object) As String
        Dim sw As New System.IO.StringWriter()
        Dim tw As New XmlTextWriter(sw)
        Try
            Dim serializer As New System.Xml.Serialization.XmlSerializer(o.[GetType]())
            serializer.Serialize(tw, o)
            Return sw.ToString()
            'Handle Exception Code
        Catch ex As Exception
            sw.Close()
            tw.Close()
            Return ex.Message
        Finally
            sw.Close()
            tw.Close()
        End Try
    End Function

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)

        MyBase.Render(writer)
        Dim cadScript As String
        cadScript = "<SCRIPT language=""javascript""> " & _
        "function OcultacmbsOnsSubmit()" & vbCrLf & _
        "{ " & vbCrLf & _
        " var obj;  " & vbCrLf & _
        " obj = document.getElementsByTagName('select');" & vbCrLf & _
        " if (obj)" & vbCrLf & _
        " {" & vbCrLf & _
        " var i; " & vbCrLf & _
        " for (i=0;i<obj.length;i++)" & vbCrLf & _
        " { " & vbCrLf & _
        " //obj[i].style.display='none';" & vbCrLf & _
        " obj[i].style.visibility = 'hidden';" & vbCrLf & _
        " }" & vbCrLf & _
        " }" & vbCrLf & _
        "}" & vbCrLf & _
        "function isValidSubmit()" & vbCrLf & _
        "{ var moz = document.getElementById && !document.all; " & vbCrLf & _
        " var r=true; " & vbCrLf & _
        " if (typeof Page_ValidationActive != 'undefined') " & vbCrLf & _
        " {" & vbCrLf & _
        " r=Page_ValidationActive; " & vbCrLf & _
        " }else { r=false; }  try {" & vbCrLf & _
        " if ((r==false ) || (( event != null && event && (typeof event != 'undefined') && event.returnValue))) " & vbCrLf & _
        " {" & vbCrLf & _
        " OcultacmbsOnsSubmit();" & vbCrLf & _
        " " & vbCrLf & _
        " }else { if ( event != null && event && (typeof event != 'undefined') && event.returnValue) event.returnValue=false; } " & vbCrLf & _
        " } catch (e) { }	}" & vbCrLf & _
        " function quitaceancho() " & vbCrLf & _
        " { var o; " & vbCrLf & _
        " if (parent){ " & _
        " o=document.getElementsByName('capa'); " & vbCrLf & _
        " if(o)" & vbCrLf & _
        " {for(var i=0; i<o.length;i++)" & vbCrLf & _
        " {o[i].style.display = 'none';" & vbCrLf & _
        " }" & vbCrLf & _
        "} " & vbCrLf & _
        "} } " & vbCrLf & _
        "if (parent.quitaceancho) parent.quitaceancho(); " & vbCrLf & _
        "window.status='" & PortalCulture.GetString("00276") & "';" & vbCrLf & _
        "</SCRIPT>" & vbCrLf
        'Page.RegisterClientScriptBlock("QuitaCapa", cadScript)
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "QuitaCapa", cadScript)
        writer.Write(cadScript)
    End Sub

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

    Private Function ParseAbsolutePath(ByVal path As String, ByVal Prov As PortalPartnersCfg) As String
        If (path.IndexOf("~") = 0) Then
            path = path.Replace("~", "")
            path = path.Replace("//", "/")
        End If
        Return Prov.UrlSite & path
    End Function

    Public Sub NotifyContentModification(ByVal note As String, Optional ByVal subject As String = Nothing)
        If Not Me.IsSupervisor Then
            Try
                Dim Mail As emailTemplates.Template = New emailTemplates.Template()

                Mail.Idioma = PortalCulture.GetCulture().ToString()
                Try
                    Mail.TemplateName = "TH_HotelContentModified"
                Catch ex As Exception
                End Try
                Mail.Html = True
                Mail.SubjectParam = String.Format("{0},{1}", Me.cInfoActual.HotelName, If(subject Is Nothing, note, subject))
                If AppSettings("ChangesNotificationMail") IsNot Nothing Then Mail.To = AppSettings("ChangesNotificationMail")

                Dim Prov As New PortalPartnersCfg
                Prov.LoadPartnerById(2)

                Mail.AddParameter("HEADER") = Prov.EmailHeader
                Mail.AddParameter("FOOTER") = Prov.EmailFooter
                Mail.AddParameter("LINKCSS") = "<link href='" & Me.ParseAbsolutePath(Prov.StyleSheets, Prov) & "correos.css' type='text/css' rel='stylesheet'>"

                Mail.AddParameter("USER") = ReadUserCookie.GetValue(0)

                Mail.AddParameter("HOTEL") = Me.cInfoActual.HotelName
                Mail.AddParameter("NOTE") = note

                Mail.Send()

            Catch ex As Exception
            End Try
        End If
    End Sub

    Function GetIdAsociation()
        Dim IdAsociation As Integer = -1
        Try
            If ConfigurationManager.AppSettings("IdAsociation") IsNot Nothing Then Integer.TryParse(ConfigurationManager.AppSettings("IdAsociation"), IdAsociation)
            IdAsociation = If(IdAsociation = 0, -1, IdAsociation)
        Catch ex As Exception
        End Try
        Return IdAsociation
    End Function

    Public Function PermissionPage(ByVal page As String) As Boolean
        Dim sPages As String = ""

        sPages = GetPermissions()
        Return If(sPages.IndexOf(page.ToUpper) > 0, True, False)
    End Function

    Public Function GetPermissions()
        Dim sPages As String = ""
        Try

            If Me.Session("PermissionsPages") Is Nothing Then
                Dim dsPages As New DataSet
                Dim strXML As String = HttpContext.Current.Request.PhysicalApplicationPath & "/Permisos.xml"
                dsPages.ReadXml(strXML)
                For Each dr As DataRow In dsPages.Tables(0).Rows
                    sPages &= String.Format("{0},", dr("Id")).ToUpper
                Next
                Me.Session("PermissionsPages") = sPages
            Else
                sPages = CType(Session("PermissionsPages"), String)
            End If

        Catch ex As Exception
        End Try
        Return sPages
    End Function

    Public Shared Sub WriteLog(ByVal Log As String, LogName As String)
        Try
            Dim path As String = AppSettings("Log_Path")
            If System.IO.Directory.Exists(path) Then
                Dim fileName As String = String.Format("{0}{1}{2}.log", path, LogName, DateTime.Now.ToString("yyyyMMdd"))
                Dim osW As System.IO.StreamWriter = New System.IO.StreamWriter(fileName, True)
                osW.WriteLine(String.Format("{0} ==> {1}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss ->fffffff"), Log))
                osW.Flush()
                osW.Close()
            End If
        Catch ex As Exception
            Dim s As String = ex.Message
        End Try
    End Sub
End Class

Public Class AuthUser
    Enum eTypRole
        Supervisor
        Content
        HotelCompany
        Unibilling
        UserChain
        UsuarioHotel
        PerfilHotel
        HotelAvanzado
        HotelMedio
        HotelBasico
        HotelNetRate
        UserAssociation
        UserNetRate
        CallCenter
        Casas
        HomeAgency
    End Enum

    Function GetRol(ByVal typRol As eTypRole) As Boolean
        Dim rolesUsuario As String
        Dim hresult As Boolean

        hresult = False
        If (Not HttpContext.Current.Session("RolesUsuario") Is Nothing AndAlso HttpContext.Current.Session("RolesUsuario") <> String.Empty) Then
            rolesUsuario = CType(HttpContext.Current.Session("RolesUsuario"), String)
            'rolesUsuario &= "HotelAvanzado, UsuarioHotel"

            If rolesUsuario.Contains("Supervisor") AndAlso rolesUsuario.Contains("HomeAgency") Then
                rolesUsuario = rolesUsuario.Replace("HomeAgency", "")
            End If
            For Each rol As String In rolesUsuario.Split(",")
                If typRol.ToString.ToUpper() = rol.Trim.ToUpper Then
                    hresult = True
                    Exit For
                End If
            Next
        End If
        Return (hresult)
    End Function

    Public ReadOnly Property IsAuthenticated() As Boolean
        Get
            Return (Not HttpContext.Current.Session("idUsuario") Is Nothing AndAlso HttpContext.Current.Session("idUsuario") <> String.Empty)
        End Get
    End Property

    Public ReadOnly Property Usuario() As Integer
        Get
            If (Not HttpContext.Current.Session("idUsuario") Is Nothing AndAlso HttpContext.Current.Session("idUsuario") <> String.Empty) Then
                Return (HttpContext.Current.Session("idUsuario"))
            End If
        End Get
    End Property

    Public ReadOnly Property UserInfoArray() As String()
        Get
            Try
                If (Not HttpContext.Current.Session("RolesUsuario") Is Nothing AndAlso HttpContext.Current.Session("RolesUsuario") <> String.Empty) Then
                    Return (CType(HttpContext.Current.Session("RolesUsuario"), String).Split(","))
                End If

            Catch ex As Exception
                Dim UserArray As String() = {""}
                Return UserArray
            End Try
        End Get
    End Property

    Public ReadOnly Property UserInfoRol() As String
        Get
            Try
                If (Not HttpContext.Current.Session("RolesUsuario") Is Nothing AndAlso HttpContext.Current.Session("RolesUsuario") <> String.Empty) Then
                    Return (CType(HttpContext.Current.Session("RolesUsuario"), String))
                End If

            Catch ex As Exception
                Return ""
            End Try
        End Get
    End Property

    Public ReadOnly Property UserInfoName() As String
        Get
            If (Not HttpContext.Current.Session("EmailLoginUser") Is Nothing AndAlso HttpContext.Current.Session("EmailLoginUser") <> String.Empty) Then
                Return (CType(HttpContext.Current.Session("EmailLoginUser"), String))
            End If
        End Get
    End Property

    Public ReadOnly Property IdIdiomaMenu() As String
        Get
            Dim id As String
            id = "2"
            If PortalCulture.GetCulture.ToString.Substring(0, 2).ToUpper = "ES" Then
                id = "1"
            End If
            Return (id)
        End Get
    End Property

    Public Property IdIdiomaLogin() As String
        Get
            If (Not HttpContext.Current.Session("DefaultLanguajeLogin") Is Nothing AndAlso HttpContext.Current.Session("DefaultLanguajeLogin") <> String.Empty) Then
                Return (CType(HttpContext.Current.Session("DefaultLanguajeLogin"), String))
            End If
        End Get
        Set(ByVal Value As String)
            If (Not HttpContext.Current.Session("DefaultLanguajeLogin") Is Nothing AndAlso HttpContext.Current.Session("DefaultLanguajeLogin") <> String.Empty) Then
                HttpContext.Current.Session.Item("DefaultLanguajeLogin") = Value
            Else
                HttpContext.Current.Session.Add("DefaultLanguajeLogin", Value)
            End If

        End Set
    End Property

    Public WriteOnly Property InitIdiomaLogin() As String
        Set(ByVal Value As String)
            If (Not HttpContext.Current.Session("DefaultLanguajeLogin") Is Nothing AndAlso HttpContext.Current.Session("DefaultLanguajeLogin") <> String.Empty) Then
            Else
                HttpContext.Current.Session.Add("DefaultLanguajeLogin", Value)
            End If
        End Set
    End Property


    Public ReadOnly Property IsContent() As Boolean
        Get
            Return GetRol(eTypRole.Content)
        End Get
    End Property

    Public ReadOnly Property IsHotel() As Boolean
        Get
            Return GetRol(eTypRole.HotelCompany)
        End Get
    End Property

    Public ReadOnly Property IsSupervisor() As Boolean
        Get
            Return GetRol(eTypRole.Supervisor)
        End Get
    End Property

    Public ReadOnly Property IsUnibilling() As Boolean
        Get
            Return GetRol(eTypRole.Unibilling)
        End Get
    End Property

    Public ReadOnly Property isUserChain() As Boolean
        Get
            Return GetRol(eTypRole.UserChain)
        End Get
    End Property

    Public ReadOnly Property IsUsuarioHotel() As Boolean
        Get
            Return GetRol(eTypRole.UsuarioHotel)
        End Get
    End Property

    Public ReadOnly Property IsUsuarioNivelHotel() As Boolean
        Get
            Return GetRol(eTypRole.HotelAvanzado) Or GetRol(eTypRole.HotelMedio) Or GetRol(eTypRole.HotelBasico) Or GetRol(eTypRole.HotelNetRate)
        End Get
    End Property

    Public ReadOnly Property IsUsuarioHotelAvanzado() As Boolean
        Get
            Return GetRol(eTypRole.HotelAvanzado)
        End Get
    End Property

    Public ReadOnly Property IsUsuarioHotelMedio() As Boolean
        Get
            Return GetRol(eTypRole.HotelMedio)
        End Get
    End Property

    Public ReadOnly Property IsUsuarioHotelBasico() As Boolean
        Get
            Return GetRol(eTypRole.HotelBasico)
        End Get
    End Property

    Public ReadOnly Property IsUsuarioHotelNetRate() As Boolean
        Get
            Return GetRol(eTypRole.HotelNetRate)
        End Get
    End Property

    Public ReadOnly Property IsUsuarioHotelAssociation() As Boolean
        Get
            Return GetRol(eTypRole.UserAssociation)
        End Get
    End Property

    Public ReadOnly Property IsUsuarioNetRates() As Boolean
        Get
            Return GetRol(eTypRole.UserNetRate)
        End Get
    End Property

    Public ReadOnly Property IsUsuarioCallCenter() As Boolean
        Get
            Return GetRol(eTypRole.CallCenter)
        End Get
    End Property
    Public ReadOnly Property IsUsuarioCasas() As Boolean
        Get
            Return GetRol(eTypRole.Casas)
        End Get
    End Property

    Public ReadOnly Property IsUsuarioMixto() As Boolean
        Get
            Return GetRol(eTypRole.HotelNetRate) And GetRol(eTypRole.HotelAvanzado)
        End Get
    End Property

    Public ReadOnly Property IsUsuarioHomeAgency() As Boolean
        Get
            Return GetRol(eTypRole.HomeAgency)
        End Get
    End Property
End Class
