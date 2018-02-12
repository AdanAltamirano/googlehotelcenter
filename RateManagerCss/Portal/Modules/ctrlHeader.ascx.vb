Imports System.Web.Security
Imports System.Configuration.ConfigurationManager
Imports System.Text
Imports System.Xml
Imports System.Security

Imports LoginAuthenticate

Partial Class ctrlHeader
    Inherits UserControlBase

#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents imgBanderaMexico As System.Web.UI.WebControls.ImageButton
    Protected WithEvents imgBanderaUSA As System.Web.UI.WebControls.ImageButton

    'NOTA: el Diseñador de Web Forms necesita la siguiente declaración del marcador de posición.
    'No se debe eliminar o mover.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: el Diseñador de Web Forms requiere esta llamada de método
        'No la modifique con el editor de código.
        InitializeComponent()
    End Sub

#End Region

    Public Function GetLabel(ByVal key As String) As String
        Return PortalCulture.GetString(key)
    End Function

    Enum Usuario
        uDefault = 0
        uHotelAdministrador = 1
        uPortalAdministrador = 2
    End Enum

    Public Event ChangeLanguage()
    Dim swPanel As Usuario

    Public Property Panel() As Usuario
        Get
            'Return swPanel
            Return Session("HeaderPanel")
        End Get
        Set(ByVal Value As Usuario)
            swPanel = Value
            Session("HeaderPanel") = Value
        End Set
    End Property

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        Me.lnkNameCompany.Attributes.Item("onmouseover") = "javascript:show('" & Me.divCompanyInfo.ClientID & "')"
        Me.lnkNameCompany.Attributes.Item("onmouseout") = "javascript:hide('" & Me.divCompanyInfo.ClientID & "')"
        Me.lnkNameCompany.NavigateUrl = "javascript:;;"

        If Not IsPostBack Then
            'CtrlPreserveScrolls1.Add(CtrlPreserveScrolls.TypeControl.THEWINDOW)
        End If

        If Not Me.IsPostBack Then
            linkLogOut.Visible = False
            Me.lnkhelp.Visible = False
        End If
        Me.lblSession.Text = ""
        If Not Request.QueryString("Culture") Is Nothing Then
            PortalCulture.SetCulture(Request.QueryString("Culture"))
        End If

        tblLinks.Visible = False
        lnkTicketList.HRef = GeRequestApplicationPath(String.Concat("/", Response.Cookies("groupid").Value, "/default.aspx?Url=HotelAdministrator/Pages/TicketList.aspx"))
        lnkTicket.HRef = GeRequestApplicationPath(String.Concat("/", Response.Cookies("groupid").Value, "/default.aspx?Url=HotelAdministrator/Pages/TicketRegister.aspx"))
        lnkhelp.HRef = GeRequestApplicationPath(String.Concat("/", Response.Cookies("groupid").Value, "/default.aspx?Url=Documentacion/ayuda.aspx"))

    End Sub

    Private Sub linkLogOut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles linkLogOut.Click
        With New clsUsuario
            .LogOut(Session("ticketId"))
        End With
        HttpContext.Current.Session.Clear()
        HttpContext.Current.Session.Abandon()
        If HttpContext.Current.User.Identity.IsAuthenticated Then
            FormsAuthentication.SignOut()
        End If

        Response.Redirect(GeRequestApplicationPath(String.Concat("/", Response.Cookies("groupid").Value, "/Default.aspx?Url=", "/Portal/Pages/Welcome.aspx")))

        linkLogOut.Visible = False
        Me.lnkhelp.Visible = False
    End Sub
    Public Sub loadMenu()
        If TypeOf Me.Page Is PaginaBase Then
            Dim pb As PaginaBase = CType(Me.Page, PaginaBase)
            If pb.IsUsuarioHotel Then
                PersonalizaMenu(pb.creaMenuUsuarioHotel())
            Else
                'CargaMenu()
                CargaMenuXmlData()
            End If
        Else
            CargaMenu()
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        loadMenu()
        Dim Version As String
        Dim dsVersion As New XmlDataDocument
        dsVersion.DataSet.ReadXml(Request.PhysicalApplicationPath & "/Portal/Modules/Version.xml")
        If Not dsVersion.DataSet.Tables("Versiones") Is Nothing AndAlso dsVersion.DataSet.Tables("Versiones").Rows.Count > 0 Then
            Version = dsVersion.DataSet.Tables("Versiones").Rows(0).Item("Version").ToString
        End If


        Me.lblTitle.Text = PortalCulture.GetString("00157") & " Ver. " & Version  'sistema de administracion
        Me.lnkNameCompany.Text = PortalCulture.GetString("00158")
        Me.linkLogOut.Text = PortalCulture.GetString("00159")
        'Me.lblInfoEmpresa.Text = PortalCulture.GetString("00160")
        Dim pg As PaginaBase


        swPanel = Session("HeaderPanel")

        If CType(Me.Page, PaginaBase).IsHotelSelected Then
            If Not Session(PaginaBase.SESSION_INFO) Is Nothing Then
                Dim cInfo As companyInfo = Session(PaginaBase.SESSION_INFO)
                If cInfo.Hotel > 0 Then
                    With cInfo
                        Me.lnkNameCompany.Text = .HotelName
                        'Me.imgArrow.Visible = True
                        Dim msgInfo As String
                        msgInfo = .HotelName & "<br>"
                        msgInfo &= .Address & "<br>"
                        msgInfo &= .City & "," & .State & "<br>"
                        msgInfo &= PortalCulture.GetString("00162", True) & .Contact & "<br>"
                        msgInfo &= PortalCulture.GetString("00163", True) & .Email & "<br>"
                        msgInfo &= PortalCulture.GetString("00164", True) & .Phone & "<br>"
                        Me.Page.RegisterStartupScript("", "<script>SetDiv('" & msgInfo & "')</script>")
                    End With
                End If
            End If
        Else
            Me.Page.RegisterStartupScript("", "<script>ClearDiv()</script>")
            Me.lnkNameCompany.Text = PortalCulture.GetString("00161")
        End If
        'If HttpContext.Current.User.Identity.IsAuthenticated Then
        If (New AuthUser).IsAuthenticated Then
            Me.linkLogOut.Visible = True
            Me.lnkhelp.Visible = True
            lblUser.Text = ReadUserCookie.GetValue(0)
            Me.lblSession.Text = PortalCulture.GetString("00270", True)
            tblLinks.Visible = True
        End If
    End Sub



    Private Sub imgBanderaMexico_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBanderaMexico.Click
        PortalCulture.SetCulture("es-MX")
        RaiseEvent ChangeLanguage()
    End Sub

    Private Sub imgBanderaUSA_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBanderaUSA.Click
        PortalCulture.SetCulture("en-US")
        RaiseEvent ChangeLanguage()
    End Sub

    Private Function ReadUserCookie() As String()
        Try
            Dim UserInfoArray As String() = {""}
            '// Obtenemos el usuario de la session.
            UserInfoArray = CType((New AuthUser).UserInfoName, String).Split(",")
            Return (UserInfoArray)

            '// Esta parte no se debe ejecutar.
            ' Obtenemos los roles desde la cookie
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

    Private Sub CargaMenu()
        If HttpContext.Current.User.Identity.IsAuthenticated Then
            lblUser.Text = ReadUserCookie.GetValue(0)
            userMenu.UserRoles.Clear()
            userMenu.DataSource = Server.MapPath(AppSettings("MenuUsuarios") & PortalCulture.GetString("00000") & ".xml")
            userMenu.HighlightTopMenu = True
            Dim ticket As FormsAuthenticationTicket = FormsAuthentication.Decrypt(Context.Request.Cookies(FormsAuthentication.FormsCookieName()).Value)
            userMenu.UserRoles.AddRange(ticket.UserData.Split(","))
            userMenu.DataBind()
            tblLinks.Visible = True
            Me.lblSession.Text = PortalCulture.GetString("00257", True)
            linkLogOut.Visible = True
        Else
            linkLogOut.Visible = False
        End If
    End Sub

    Private Function CargaMenuXmlData()
        Dim cMenu As New clsMenu
        Dim doc As New XmlDocument
        Dim roleListArray As String()
        Dim menuXml As String

        If (New AuthUser).IsAuthenticated Then
            roleListArray = (New AuthUser).UserInfoArray
            '// Agrega los roles del usuario.
            HttpContext.Current.User = New Principal.GenericPrincipal(HttpContext.Current.User.Identity, roleListArray)

            menuXml = cMenu.GetMenu(AppSettings("idSistema"), (New AuthUser).IdIdiomaMenu, "sys_deals")
            If menuXml <> String.Empty Then
                doc.LoadXml(menuXml)
                userMenu.UserRoles.Clear()
                userMenu.DataSource = doc
                'userMenu.DataSource = Server.MapPath(AppSettings("MenuUsuarios") & PortalCulture.GetString("00000") & ".xml")
                For Each rol As String In roleListArray
                    userMenu.UserRoles.Add(rol.ToUpper)
                Next
                userMenu.DataBind()
            End If      
        End If
    End Function

    Public Sub PersonalizaMenu(ByVal xml As String)
        Dim doc As New XmlDocument
        Dim UserInfoArray As String()

        userMenu.UserRoles.Clear()
        userMenu.HighlightTopMenu = True
        UserInfoArray = (New AuthUser).UserInfoArray
        userMenu.UserRoles.AddRange(UserInfoArray)
        doc.LoadXml(xml)
        userMenu.DataSource = doc
        userMenu.DataBind()

    End Sub
   
End Class