Imports System.Web.Security
Imports System.Runtime.Serialization
Imports System.Configuration.ConfigurationManager
Imports System.IO
Imports System.Web.HttpContext
Imports System.Xml
Imports Portal.Hotel.Common
Imports Portal.Hotel.Facade
Imports System.Threading
Imports System.Security.Principal

'*****************************************************************************************
'* I M P O R T A N T E :
'* La clase paginabase fue cambiada al archivo PaginaBasePss, esta es usada para Passport
'* cualquier Modificacion se debe hacer alli,  
'* LA CLASE "PAGINABASEOLD" ES SOLO PARA AUTENTIFICACION POR FORMA. Y QUEDA OBSOLETA.
'*****************************************************************************************

Public Class PaginaBaseOLD
    Inherits System.Web.UI.Page
    'Private _urlStyleSheet As String = AppSettings("DefaultStyleSheet")
    '  Protected StyleSheet As System.Web.UI.HtmlControls.HtmlGenericControl
    Public Const SESSION_INFO As String = "infoCompany"
    Protected Form1 As HtmlForm
    Private cI As companyInfo
#Region "propiedades"

    Public Enum eUsuario
        uDefault = 0
        uHotelAdministrador = 1
        uPortalAdministrador = 2
    End Enum
    Public Enum modes As Byte
        Normal
        Extended
    End Enum
    Public Property mode() As modes
        Get
            If viewstate.Item("KEY_MODE") Is Nothing Then
                viewstate.Item("KEY_MODE") = modes.Normal
            End If
            Return viewstate.Item("KEY_MODE")
        End Get
        Set(ByVal Value As modes)
            viewstate.Item("KEY_MODE") = Value
            HttpContext.Current.Session("Mode") = viewstate.Item("KEY_MODE")
        End Set
    End Property

    Public Property HotelUsuarioPermisos() As PermisosData
        Get
            Try
                Dim dsPermisosData As PermisosData = Session("HotelUsuarioPermisos")
                If dsPermisosData Is Nothing Then
                    With New PermisosFacade
                        dsPermisosData = .PermisosGetByUser(Usuario)
                        Session("HotelUsuarioPermisos") = dsPermisosData
                    End With
                End If
                Return dsPermisosData
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
            If viewstate.Item("KEY_INFO") Is Nothing Then
                viewstate.Item("KEY_INFO") = New companyInfo
            End If
            Return viewstate.Item("KEY_INFO")
        End Get
        Set(ByVal Value As companyInfo)
            cI = Session(SESSION_INFO)
            If cI.Hotel <> Value.Hotel Then
                'If Not IsSupervisor AndAlso Not IsUsuarioHotel AndAlso Not IsUnibilling AndAlso Not IsContent Then
                If Not IsSupervisor AndAlso Not IsUnibilling AndAlso Not IsContent AndAlso Not isUserChain Then
                    If Not IsUsuarioHotel Then crtUserCookie(Value.UserPerfil) ' No crear la Cookie si es usuario Hotel pero que si cargue el menu de nuevo
                    Session("menu") = "true"
                    Session("LogIn") = "false"
                End If

            End If
            viewstate.Item("KEY_INFO") = Value
            Session(SESSION_INFO) = viewstate.Item("KEY_INFO")
        End Set
    End Property

    Public Property PermisoUser(ByVal permiso As String) As DerechoUsuario
        Get
            Return viewstate(permiso & "_PermisoUser")
        End Get
        Set(ByVal Value As DerechoUsuario)
            viewstate(permiso & "_PermisoUser") = Value
            Session(permiso & "_PermisoUser") = viewstate.Item(permiso & "_PermisoUser")
        End Set
    End Property

    'Public Property urlStyleSheet() As String
    '    Get
    '        Try
    '            If Request.Cookies("StyleSheet").Value <> vbNullString Then
    '                _urlStyleSheet = Request.Cookies("StyleSheet").Value
    '            End If
    '        Catch
    '        End Try
    '        Return _urlStyleSheet
    '    End Get
    '    Set(ByVal Value As String)
    '        If Value.Trim <> "" Then
    '            _urlStyleSheet = Value
    '            Response.Cookies("StyleSheet").Value = Value
    '            Response.Cookies("StyleSheet").Expires = Date.MaxValue
    '        End If
    '    End Set
    'End Property
    Public ReadOnly Property isUserChain() As Boolean
        Get
            Return User.IsInRole("UserChain")
        End Get
    End Property
    Public ReadOnly Property IsHotel() As Boolean
        Get
            Return User.IsInRole("HotelCompany")
        End Get
    End Property
    Public ReadOnly Property IsUnibilling() As Boolean
        Get
            Return User.IsInRole("Unibilling")
        End Get
    End Property
    Public ReadOnly Property IsContent() As Boolean
        Get
            Return User.IsInRole("Content")
        End Get
    End Property

    Public ReadOnly Property IsHotelSelected() As Boolean
        Get
            Return Me.cInfoActual.Hotel > 0
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

    Public ReadOnly Property IsSupervisor() As Boolean
        Get
            Return User.IsInRole("Supervisor")
        End Get
    End Property
    Public ReadOnly Property IsUsuarioHotel() As Boolean
        Get
            Return User.IsInRole("UsuarioHotel")
        End Get
    End Property
    Public ReadOnly Property IsUsuarioCasa() As Boolean
        Get
            Return User.IsInRole("Casa")
        End Get
    End Property
    Public ReadOnly Property IsUsuarioHomeAgency() As Boolean
        Get
            Return User.IsInRole("HomeAgency")
        End Get
    End Property
    Public ReadOnly Property IsUsuarioNivelHotel() As Boolean
        Get
            Return User.IsInRole("HotelAvanzado") Or User.IsInRole("HotelMedio") Or User.IsInRole("HotelBasico")
        End Get
    End Property
#End Region
    Public Enum acciones As Integer
        Crear '0
        Modificar '1
        Eliminar '2
        LogIn '3
    End Enum
    Public Enum PerfilHotel
        Basico
        Medio
        Avanzado
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
        NoRatesReport
        ratechart
        HotelStatus
        InventarioHotel
        InventarioRatePlans
        MainInvoicing
        FaresCopy
        Welcome
        InvoiceByPeriod
    End Enum
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





    Private _Reload As Boolean  'si se va a permitir esto

    Property ReloadMe() As Boolean
        Get
            Return _Reload
        End Get
        Set(ByVal Value As Boolean)
            _Reload = Value
        End Set
    End Property

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

        If Request.RawUrl.ToString.ToUpper.IndexOf("DEFAULT.ASPX") = -1 Then
            Session("urlCurrent") = Request.RawUrl.ToString
        End If
        If Session("LogIn") = "true" Then
            If Not IsSupervisor AndAlso Not IsUnibilling AndAlso Not isUserChain And Not IsContent And Not IsUsuarioHotel Then
                crtUserCookie(Me.cInfoActual.UserPerfil)
                Session("LogIn") = "false"
                Session("menu") = "true"
            End If
        End If
        Response.Expires = 0
        If HttpContext.Current.User.Identity.IsAuthenticated Then
            If IsHotel Or IsSupervisor Or IsUsuarioHotel Or Me.cInfoActual.UserPerfil = PerfilHotel.Avanzado Or _
            Me.cInfoActual.UserPerfil = PerfilHotel.Basico Or Me.cInfoActual.UserPerfil = PerfilHotel.Medio Or Me.IsUnibilling Or Me.IsContent Then
                Usuario = 0
                If User.Identity.Name <> "" Then
                    Usuario = CType(User.Identity.Name, Integer)
                End If
            End If

        End If
        reloadViewstate()

        If IsUsuarioHotel Then PermissionSeePage()
    End Sub
    Public Sub ResizefrmPrincipal()
        Page.RegisterStartupScript("UrlScript", "<script src='" & Request.ApplicationPath & "/Utils_Iframe.js' type='text/javascript'></script>")
        Me.Page.RegisterOnSubmitStatement("FrameResizeLevel2", "if (resizeIframe)resizeIframe('frmPrincipal');")
        Page.RegisterStartupScript("ResizePage", "<script>if (resizeIframe)resizeIframe('frmPrincipal');</script>")
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        'If urlStyleSheet <> "" Then
        '    Try
        '        Me.StyleSheet.Attributes("href") = urlStyleSheet
        '    Catch
        '    End Try
        'End If
        Form1.Attributes.Add("onsubmit", "isValidSubmit();")
        Dim cadScript As String = ""

        cadScript = "<SCRIPT language=""javascript""> " & _
           " function estableceancho() " & vbCrLf & _
           " { var o; " & vbCrLf & _
           " o=document.getElementsByName('capa'); " & vbCrLf & _
           " if(o)" & vbCrLf & _
           " { for(var i=0; i<o.length;i++)" & vbCrLf & _
           " { o[i].style.display = '';" & vbCrLf & _
           " } window.status='" & PortalCulture.GetString("00277") & "';" & vbCrLf & _
           " } else { return false; } " & vbCrLf & _
           "}" & vbCrLf & _
           "</SCRIPT>" & vbCrLf
        cadScript &= "<table id=""Capa"" WIDTH=100% HEIGHT=100% name=""capa"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""DISPLAY:none;FILTER:alpha(opacity=30);LEFT:0px;POSITION:absolute;TOP:0px;BACKGROUND-COLOR:gainsboro;moz-opacity:0.50"">" & _
         "<tr> <td></td> </tr> </table>"
        Page.RegisterClientScriptBlock("LayerCapa", cadScript)
        Dim sScript As New System.Text.StringBuilder


        If Session("menu") = "true" Then
            sScript.Append("<SCRIPT language=""javascript"">" & vbCrLf)
            sScript.Append("   if (parent.UpdateMe) { parent.UpdateMe() };" & vbCrLf)
            sScript.Append("</SCRIPT>" & vbCrLf)
            RegisterStartupScript("UPDATEME", sScript.ToString)
        End If
    End Sub
    Private Function crtUserCookie(ByVal perfil As PerfilHotel) As HttpCookie
        Dim tkt As FormsAuthenticationTicket
        Dim cookiestr As String
        Dim ck As HttpCookie
        Dim roles As String
        Select Case perfil
            Case CInt(PerfilHotel.Avanzado)
                roles = "HotelAvanzado"
            Case CInt(PerfilHotel.Basico)
                roles = "HotelBasico"
            Case CInt(PerfilHotel.Medio)
                roles = "HotelMedio"
        End Select

        tkt = New FormsAuthenticationTicket(1, _
          User.Identity.Name, _
          DateTime.Now(), _
          DateTime.Now.AddMinutes(CType(AppSettings("TimeCookie"), Integer)), _
          False, _
          Join(roles.Split(""), ","))
        'Cookie encriptada
        cookiestr = FormsAuthentication.Encrypt(tkt)
        ck = New HttpCookie(FormsAuthentication.FormsCookieName(), cookiestr)
        ck.Expires = tkt.Expiration
        ck.Path = FormsAuthentication.FormsCookiePath()
        Response.Cookies.Item(FormsAuthentication.FormsCookieName()).Value = ck.Value
        HttpContext.Current.User = New GenericPrincipal(User.Identity, roles.Split(""))

    End Function
    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)

        MyBase.Render(writer)
        Dim cadScript As String
        cadScript = "<SCRIPT language=""javascript""> " & _
        "function OcultacmbsOnsSubmit()" & vbCrLf & _
        "{" & vbCrLf & _
        " var obj;" & vbCrLf & _
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
        "{" & vbCrLf & _
        " var r=true; " & vbCrLf & _
        " if (typeof Page_ValidationActive != 'undefined') " & vbCrLf & _
        " {" & vbCrLf & _
        " r=Page_ValidationActive; " & vbCrLf & _
        " }else { r=false; }" & vbCrLf & _
        " if ((event.returnValue) || (r==false)) " & vbCrLf & _
        " {" & vbCrLf & _
        " OcultacmbsOnsSubmit();" & vbCrLf & _
        " parent.estableceancho();" & vbCrLf & _
        " }else { event.returnValue=false; }" & vbCrLf & _
        "}" & vbCrLf & _
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
        Page.RegisterClientScriptBlock("QuitaCapa", cadScript)
        writer.Write(cadScript)
    End Sub


    Public Sub redirectTo(ByVal page As pages, Optional ByVal queryString As String = Nothing)
        Response.Redirect(UrlPage(page) & queryString)
        'Select Case page
        '    Case pages.Home
        '        Response.Redirect(Request.ApplicationPath & "/Portal/Pages/Home.aspx" & queryString)
        '    Case pages.SearchHotel
        '        Response.Redirect(Request.ApplicationPath & "/Portal/Pages/Search.aspx" & queryString)
        '    Case pages.ReservaDetails
        '        Response.Redirect(Request.ApplicationPath & "/HotelAdministrator/Pages/ReservationDetails.aspx" & queryString)
        '    Case Else
        '        Response.Redirect(UrlPage(page) & queryString)
        'End Select
    End Sub

    Public Function UrlPage(ByVal page As pages) As String
        Dim strpage As String
        Dim sRequestApplicationPath As String = Request.ApplicationPath
        Select Case page
            Case pages.Home
                strpage = sRequestApplicationPath & "/Portal/Pages/Welcome.aspx"
            Case pages.SearchHotel
                strpage = sRequestApplicationPath & "/Portal/Pages/Search.aspx"
            Case pages.ReservaDetails
                strpage = sRequestApplicationPath & "/HotelAdministrator/Pages/ReservationDetails.aspx"
            Case page.RatePlanInventory
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
                strpage = sRequestApplicationPath & "/HotelAdministrator/Pages/Reservations.aspx"
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

        End Select
        Return strpage
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

    Public Sub SetFocus(ByVal ctrl As Control)
        ' Define the JavaScript function for the specified control.
        Dim focusScript As String = "<script language='javascript'>" & _
          "document.getElementById('" + ctrl.ClientID & _
          "').focus();</script>"

        ' Add the JavaScript code to the page.
        RegisterStartupScript("FocusScript", focusScript)
    End Sub


    Private Sub reloadViewstatePermisos()
        Dim ds As PermisosData
        ds = permisos.GetPermisosNames()

        With HttpContext.Current
            For Each r As DataRow In ds.Tables(ds.PermisosTable).Rows
                If Not .Session(r(ds.NameField) & "_PermisoUser") Is Nothing Then
                    Me.PermisoUser(r(ds.NameField)) = .Session(r(ds.NameField) & "_PermisoUser")
                End If
            Next
        End With

    End Sub

    'Public Function creaMenuUsuarioHotel() As String
    '    Dim clsmenu As String = "<cssclass>menustyle</cssclass><mouseovercssclass>mousemenu</mouseovercssclass>"
    '    Dim menu As String = ""
    '    menu &= "<menu>"
    '    menu &= "<menuItem><text>Portal</text>"
    '    menu &= "<roles>usuariohotel</roles>"
    '    menu &= clsmenu & "<subMenu><menuItem>"
    '    menu &= "<text>" & PortalCulture.GetString("00142") & "</text>"
    '    menu &= "<url>/RateManager/Portal/Pages/Home.aspx</url>"
    '    menu &= "</menuItem></subMenu>" & clsmenu & "</menuItem>"

    '    'TRAER LOS DERECHOS DE LOS USUARIOS
    '    If Not Me.PermisoUser(permisos.ConfirmReservations) Is Nothing Then
    '        menu &= "   <menuItem>"
    '        menu &= "<roles>usuariohotel</roles>"
    '        menu &= "       <text> Reservations</text>"
    '        menu &= "<subMenu>"
    '        menu &= "<menuItem><text>" & PortalCulture.GetString("00733") & "</text>"
    '        menu &= "<roles>usuariohotel</roles>"
    '        menu &= "<url>/RateManager/HotelAdministrator/Pages/ConfirmReservas.aspx</url>"
    '        menu &= "</menuItem>"
    '        menu &= "</subMenu>" & clsmenu & "</menuItem>"
    '    End If


    '    If Not Me.PermisoUser(permisos.RatePlan) Is Nothing Or Not Me.PermisoUser(permisos.RatePlanLinks) Is Nothing Or Not Me.PermisoUser(permisos.RatePlanRules) Is Nothing Then
    '        menu &= "   <menuItem>"
    '        menu &= "<roles>usuariohotel</roles>"
    '        menu &= "       <text> Rate Plans</text>"
    '        menu &= "<subMenu>"
    '        If Not Me.PermisoUser(permisos.RatePlan) Is Nothing Then
    '            menu &= "<menuItem><text> Rate Plan</text>"
    '            menu &= "<roles>usuariohotel</roles>"
    '            menu &= "<url>/RateManager/Pages/RatesPlans.aspx</url>"
    '            menu &= "</menuItem>"
    '        End If
    '        If Not Me.PermisoUser(permisos.RatePlanLinks) Is Nothing Then
    '            menu &= "<menuItem><text>Links</text>"
    '            menu &= "<url>/RateManager/Pages/RatePlansLinks.aspx</url>"
    '            menu &= "</menuItem>"
    '        End If
    '        If Not Me.PermisoUser(permisos.RatePlanRules) Is Nothing Then
    '            menu &= "<menuItem><text>" & PortalCulture.GetString("00015") & "</text>"
    '            menu &= "<roles>usuariohotel</roles>"
    '            menu &= "<url>/RateManager/Pages/RatePlansRules.aspx</url>"
    '            menu &= "</menuItem>"
    '        End If
    '        menu &= "</subMenu>" & clsmenu & "</menuItem>"
    '    End If
    '    If Not Me.PermisoUser(permisos.Rooms) Is Nothing Or Not Me.PermisoUser(permisos.RoomsLinks) Is Nothing Then
    '        menu &= "   <menuItem>"
    '        menu &= "<roles>UsuarioHotel</roles>"
    '        menu &= "       <text>" & PortalCulture.GetString("00048") & "</text>"
    '        menu &= "<subMenu>"
    '        If Not Me.PermisoUser(permisos.Rooms) Is Nothing Then
    '            menu &= "<menuItem><text>" & PortalCulture.GetString("00048") & "</text>"
    '            menu &= "<roles>UsuarioHotel</roles>"
    '            menu &= "<url>/RateManager/Pages/Rooms.aspx</url>"
    '            menu &= "</menuItem>"
    '        End If
    '        If Not Me.PermisoUser(permisos.RoomsLinks) Is Nothing Then
    '            menu &= "<menuItem><text>Links</text>"
    '            menu &= "<roles>UsuarioHotel</roles>"
    '            menu &= "<url>/RateManager/Pages/RoomsLinks.aspx</url>"
    '            menu &= "</menuItem>"
    '        End If
    '        menu &= "</subMenu>" & clsmenu & "</menuItem>"
    '    End If
    '    If Not Me.PermisoUser(permisos.Tarifas) Is Nothing Then
    '        menu &= "   <menuItem>"
    '        menu &= "<roles>UsuarioHotel</roles>"
    '        menu &= "       <text>" & PortalCulture.GetString("00299") & "</text>"
    '        menu &= "<subMenu><menuItem><text>" & PortalCulture.GetString("00332") & "</text>"
    '        menu &= "<roles>UsuarioHotel</roles>"
    '        menu &= "<url>/RateManager/Pages/FaresCatalogue.aspx</url>"
    '        menu &= "</menuItem></subMenu>"
    '        menu &= clsmenu & "</menuItem>"
    '    End If


    '    If Not Me.PermisoUser(permisos.RatePlanInventory) Is Nothing Or Not Me.PermisoUser(permisos.InventoryRooms) Is Nothing Or Not Me.PermisoUser(permisos.SoldOutInventory) Is Nothing Then
    '        menu &= "   <menuItem>"
    '        menu &= "<roles>UsuarioHotel</roles>"
    '        menu &= "       <text>" & PortalCulture.GetString("00337") & "</text>"
    '        menu &= "<subMenu>"
    '        If Not Me.PermisoUser(permisos.InventoryRooms) Is Nothing Then
    '            menu &= "<menuItem><text>Hotel</text>"
    '            menu &= "<url>/RateManager/Pages/HomePage.aspx</url>"
    '            menu &= "<roles>UsuarioHotel</roles>"
    '            menu &= "</menuItem>"
    '        End If
    '        If Not Me.PermisoUser(permisos.RatePlanInventory) Is Nothing Then
    '            menu &= "<menuItem><text>" & PortalCulture.GetString("00339") & "</text>"
    '            menu &= "<roles>UsuarioHotel</roles>"
    '            menu &= "<url>/RateManager/Pages/SegmentRoomsAvailability.aspx</url>"
    '            menu &= "</menuItem>"
    '        End If
    '        If Not Me.PermisoUser(permisos.SoldOutInventory) Is Nothing Then
    '            menu &= "<menuItem><text>" & PortalCulture.GetString("00326") & "</text>"
    '            menu &= "<roles>UsuarioHotel</roles>"
    '            menu &= "<url>/RateManager/Pages/RatePlanInventory.aspx</url>"
    '            menu &= "</menuItem>"
    '        End If
    '        menu &= "</subMenu>" & clsmenu & "</menuItem>"
    '    End If
    '    If Not Me.PermisoUser(permisos.Bloqueos) Is Nothing Or Not Me.PermisoUser(permisos.Tarifas) Is Nothing Then
    '        menu &= "   <menuItem>"
    '        menu &= "<roles>UsuarioHotel</roles>"
    '        menu &= "       <text>" & PortalCulture.GetString("00341") & "</text>"
    '        menu &= "<subMenu>"
    '        If Not Me.PermisoUser(permisos.Bloqueos) Is Nothing Then
    '            menu &= "<menuItem><text>" & PortalCulture.GetString("00342") & "</text>"
    '            menu &= "<roles>UsuarioHotel</roles>"
    '            menu &= "<url>/RateManager/Pages/AvailabilityRestrictions.aspx</url>"
    '            menu &= "</menuItem>"
    '        End If
    '        If Not Me.PermisoUser(permisos.Tarifas) Is Nothing Then
    '            menu &= "<menuItem><text>Rate chart</text>"
    '            menu &= "<roles>UsuarioHotel</roles>"
    '            menu &= "<url>/RateManager/Pages/Ratechart.aspx</url>"
    '            menu &= clsmenu & "</menuItem>"
    '            menu &= "<menuItem><text>" & PortalCulture.GetString("00340") & "</text>"
    '            menu &= "<roles>UsuarioHotel</roles>"
    '            menu &= "<url>/RateManager/Pages/NoRatesReport.aspx</url>"
    '            menu &= "</menuItem>"
    '        End If
    '        menu &= "</subMenu>" & clsmenu & "</menuItem>"
    '    End If


    '    menu &= "</menu>"
    '    Return menu
    'End Function
    Public Function creaMenuUsuarioHotel() As String
        Dim ds As PermisosData = HotelUsuarioPermisos
        Dim menu = String.Empty
        Dim currentRol As String = "UsuarioHotel"
        Dim cInfo As companyInfo = Session(PaginaBase.SESSION_INFO)
        If IsHotelSelected AndAlso Not cInfo Is Nothing Then
            currentRol = GetRolHotel(cInfo.UserPerfil)
        End If

        Dim doc As XmlDocument = New XmlDocument
        Try
            doc.Load(Server.MapPath(AppSettings("MenuUsuarios") & PortalCulture.GetString("00000") & ".xml"))

            menu += "<?xml version=""1.0"" encoding=""utf-8"" ?>"
            menu += "<menu>"
            ReadNodeMenu(doc, doc.SelectNodes("/menu/menuItem"), False, ds, currentRol, menu)
            menu += "</menu>"


        Catch ex As Exception
            menu = ""

        End Try
        Return menu
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
                    dv = dsP.Tables(dsP.PermisosTable).DefaultView
                    dv.RowFilter = String.Format("PermisoName = '{0}'", nUrl)
                    show = dv.Count > 0
                End If

                If show AndAlso Not RolInRoles(rol, nRoles) Then
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

    Private Function RolInRoles(ByVal rol As String, ByVal roles As String) As Boolean
        Dim r As String

        If roles.Trim = String.Empty Then
            Return True
        End If

        For Each r In roles.Split(",")
            If (r.ToUpper = rol.ToUpper) Then
                Return True
            End If
        Next

        Return False
    End Function

    Public Sub clearviewstatepermisos()
        'Dim p As PermisosData = permisos.GetPermisosNames
        'For Each r As DataRow In p.Tables(p.PermisosTable).Rows
        '    Me.PermisoUser(r(p.NameField)) = Nothing
        'Next

    End Sub

    Public Function PermissionSeePage(ByVal url As String) As Boolean
        If IsUsuarioHotel Then
            Dim nombrePagina As String = url.ToUpper()
            Dim dsP = HotelUsuarioPermisos

            For Each dr As DataRow In dsP.Tables(dsP.PermisosTable).rows
                If (dr("PermisoName").toupper = nombrePagina) Then
                    Return True
                End If
            Next
        End If
        Return False
    End Function


    Public Sub PermissionSeePage()

        If IsUsuarioHotel Then

            Dim nombrePagina As String = Me.Page.GetType.FullName().ToUpper

            Dim dsP = HotelUsuarioPermisos

            Dim show As Boolean = False

            If nombrePagina.Length > 4 Then

                nombrePagina = nombrePagina.Substring(4)

                nombrePagina = nombrePagina.Replace("_ASPX", ".ASPX")

                If (nombrePagina = "WELCOME.ASPX") Then

                    show = True

                ElseIf nombrePagina = "SEARCH.ASPX" Then

                    show = True

                ElseIf nombrePagina = "DEFAULT.ASPX" Then

                    show = True

                ElseIf nombrePagina = "INDEX.ASPX" Then





                    show = True

                Else

                    nombrePagina = String.Format("{0}/{1}", Me.Page.TemplateSourceDirectory.ToUpper, nombrePagina)
                    For Each dr As DataRow In dsP.Tables(dsP.PermisosTable).rows


                        If (dr("PermisoName").toupper = nombrePagina) Then

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

    End Sub

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

    Public Sub DefaultButton(ByRef objTextControl As TextBox, ByRef objDefaultButton As Object)
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
        RegisterStartupScript("ForceDefaultToScript", sScript.ToString)
    End Sub
    Public Sub guardalog(ByVal pagina As String, ByVal action As acciones, ByVal nota As String)
        If ReadUserCookie.GetValue(0) <> "" Then
            Dim ds As LogData = New LogData
            Dim dr As DataRow = ds.Tables(ds.TABLE_LOG).NewRow
            dr(ds.FIELD_USUARIO) = ReadUserCookie.GetValue(0)
            dr(ds.FIELD_PAGINA) = pagina
            dr(ds.FIELD_ACCION) = action
            If cInfoActual.Hotel <> 0 Then
                dr(ds.FIELD_HOTEL) = cInfoActual.Hotel
            End If
            dr(ds.FIELD_NOTA) = nota
            dr(ds.FIELD_FECHA) = Now.ToString("MM/dd/yyyy") & " " & Now.ToLongTimeString
            ds.Tables(ds.TABLE_LOG).Rows.Add(dr)
            With New LogFacade
                .insertLog(ds)
            End With
        End If
    End Sub
    Public Function Leerlog(ByVal f1 As DateTime, ByVal f2 As DateTime, ByVal idhotel As Integer) As LogData
        With New LogFacade
            Return .GetLog(f1, f2, idhotel)
        End With
    End Function


    Public Function ReadUserCookie() As String()
        Try
            ' Obtenemos los roles desde la cookie
            Dim UserInfoArray As String() = {""}
            ReadUserCookie = UserInfoArray
            Dim CookieName As String = FormsAuthentication.FormsCookieName() & "UI"
            If Not Context.Request.Cookies(CookieName) Is Nothing Then
                Dim ticket As FormsAuthenticationTicket = FormsAuthentication.Decrypt(Context.Request.Cookies(CookieName).Value)
                If Not ticket Is Nothing Then
                    'convertimos a arreglo de elementos la lista de cookies                                
                    UserInfoArray = ticket.UserData.Split(CType(",", Char))
                    If UserInfoArray.Length > 0 Then
                        ReadUserCookie = UserInfoArray
                    End If
                End If
            End If
        Catch cryptoException As System.Security.Cryptography.CryptographicException
            ' Si hay problemas al leer la cookie por aplicacion hacemos que la elimine el navegador
            Dim ck As HttpCookie
            ck = cSecurity.createUserCookie(0, 0, False, New String() {""})
            Response.Cookies.Add(ck)
        Catch e As Exception
        Finally
        End Try
    End Function

    Private Function GetRolHotel(ByVal perfil As PerfilHotel) As String

        Dim roles As String
        Select Case perfil
            Case CInt(PerfilHotel.Avanzado)
                roles = "HotelAvanzado"
            Case CInt(PerfilHotel.Basico)
                roles = "HotelBasico"
            Case CInt(PerfilHotel.Medio)
                roles = "HotelMedio"
        End Select

        Return roles
    End Function



End Class
<Serializable()> Public Class companyInfo
    Implements ISerializable

    Public HotelName As String
    Public CorporateName As String
    Public Rubro As Integer
    Public Empresa As Integer
    Public Hotel As Integer
    Public Address As String
    Public State As String
    Public IdPais As String
    Public City As String
    Public Contact As String
    Public Email As String
    Public Phone As String
    Public Url As String
    Public Rooms As Integer 
    Public EsMoroso As Boolean
    Public UserPerfil As PaginaBase.PerfilHotel

    Public IdCorporate As Integer
    Public IdAsociation As Integer
    Public IsHouse As Boolean
    Public IsSingleImgInv As Boolean


    Public Sub New()

    End Sub

    Public Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
        With info
            Me.HotelName = .GetValue("HotelName", GetType(String))
            Me.CorporateName = .GetValue("CorporateName", GetType(String))
            Me.Rubro = .GetValue("Rubro", GetType(Integer))
            Me.Empresa = .GetValue("Empresa", GetType(Integer))
            Me.Hotel = .GetValue("Hotel", GetType(Integer))
            Me.Address = .GetValue("Address", GetType(String))
            Me.State = .GetValue("State", GetType(String))

            Me.IdPais = .GetValue("IdPais", GetType(String))

            Me.City = .GetValue("City", GetType(String))
            Me.Contact = .GetValue("Contact", GetType(String))
            Me.Email = .GetValue("Email", GetType(String))
            Me.Phone = .GetValue("Phone", GetType(String))
            Me.EsMoroso = .GetValue("EsMoroso", GetType(Boolean))
            Me.UserPerfil = .GetValue("Perfil", GetType(PaginaBase.PerfilHotel))
            Me.Url = .GetValue("Url", GetType(String))
            Me.Rooms = .GetValue("Rooms", GetType(Integer))

            Me.IdAsociation = .GetValue("IdAsociation", GetType(Integer))
            Me.IdCorporate = .GetValue("IdCorporate", GetType(Integer))
            Me.IsHouse = .GetValue("IsHouse", GetType(Boolean))
            Me.IsSingleImgInv = .GetValue("IsSingleImgInv", GetType(Boolean))
        End With
    End Sub

    Public Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext) Implements System.Runtime.Serialization.ISerializable.GetObjectData
        With info
            .AddValue("HotelName", Me.HotelName, GetType(String))
            .AddValue("CorporateName", Me.CorporateName, GetType(String))
            .AddValue("Rubro", Me.Rubro, GetType(Integer))
            .AddValue("Empresa", Me.Empresa, GetType(Integer))
            .AddValue("Hotel", Me.Hotel, GetType(Integer))
            .AddValue("Address", Me.Address, GetType(String))
            .AddValue("State", Me.State, GetType(String))

            .AddValue("IdPais", Me.IdPais, GetType(String))

            .AddValue("City", Me.City, GetType(String))
            .AddValue("Contact", Me.Contact, GetType(String))
            .AddValue("Email", Me.Email, GetType(String))
            .AddValue("Phone", Me.Phone, GetType(String))
            .AddValue("EsMoroso", Me.EsMoroso, GetType(Boolean))
            .AddValue("Perfil", Me.UserPerfil, GetType(PaginaBase.PerfilHotel))
            .AddValue("Url", Me.Url, GetType(String))
            .AddValue("Rooms", Me.Rooms, GetType(Integer))

            .AddValue("IdAsociation", Me.IdAsociation, GetType(Integer))
            .AddValue("IdCorporate", Me.IdCorporate, GetType(Integer))
            .AddValue("IsHouse", Me.IsHouse, GetType(Boolean))
            .AddValue("IsSingleImgInv", Me.IsHouse, GetType(Boolean))
        End With
    End Sub


End Class
<Serializable()> Public Class DerechoUsuario
    Implements ISerializable

    Public PermisoName As String
    Public Permiso As String

    Public Sub New()

    End Sub

    Public Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
        With info
            Me.PermisoName = .GetValue("PermisoName", GetType(String))
            Me.Permiso = .GetValue("Permiso", GetType(String))
        End With
    End Sub

    Public Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext) Implements System.Runtime.Serialization.ISerializable.GetObjectData
        With info
            .AddValue("PermisoName", Me.PermisoName, GetType(String))
            .AddValue("Permiso", Me.Permiso, GetType(String))
        End With
    End Sub




End Class

