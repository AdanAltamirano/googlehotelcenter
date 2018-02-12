Imports System.Web.Security
Imports System.Configuration.ConfigurationManager
Imports System.Xml
Partial Class CtrMenu
    Inherits System.Web.UI.UserControl
    Public Property UsuarioHotel() As Boolean
        Get
            Return viewstate("_uH")
        End Get
        Set(ByVal Value As Boolean)
            viewstate("_uH") = Value
        End Set
    End Property

#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTA: el Diseñador de Web Forms necesita la siguiente declaración del marcador de posición.
    'No se debe eliminar o mover.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: el Diseñador de Web Forms requiere esta llamada de método
        'No la modifique con el editor de código.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
    End Sub
    Private Sub CargaMenu()
        ReadUserCookie()
        userMenu.UserRoles.Clear()
        userMenu.DataSource = Server.MapPath(AppSettings("MenuUsuarios") & PortalCulture.GetString("00000") & ".xml")

        userMenu.HighlightTopMenu = True
        Dim ticket As FormsAuthenticationTicket = FormsAuthentication.Decrypt(Context.Request.Cookies(FormsAuthentication.FormsCookieName()).Value)
        userMenu.UserRoles.AddRange(ticket.UserData.Split(","))

        userMenu.DataBind()
        'tblLinks.Visible = True
        'Me.lblSession.Text = PortalCulture.GetString("00156", True)
        'linkLogOut.Visible = True


    End Sub

   

    Public Sub PersonalizaMenu(ByVal xml As String)
        Dim doc As New XmlDocument
        ReadUserCookie()
        userMenu.UserRoles.Clear()
        Dim ticket As FormsAuthenticationTicket = FormsAuthentication.Decrypt(Context.Request.Cookies(FormsAuthentication.FormsCookieName()).Value)
        userMenu.UserRoles.AddRange(ticket.UserData.Split(","))
        doc.LoadXml(xml)


        userMenu.DataSource = doc
        userMenu.DataBind()
    End Sub



    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        If TypeOf Me.Page Is PaginaBase Then
            Dim pb As PaginaBase = CType(Me.Page, PaginaBase)
            If pb.IsUsuarioHotel Then
                PersonalizaMenu(pb.creaMenuUsuarioHotel())
            Else
                CargaMenu()
            End If
        Else

            CargaMenu()
        End If

    End Sub
    Private Function ReadUserCookie() As String()
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


End Class
