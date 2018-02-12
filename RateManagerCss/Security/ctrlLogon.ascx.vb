Option Strict On
Option Explicit On
Imports System.Configuration.ConfigurationManager
Imports Portal.General.Facade
Imports Portal.General.Common.Data

Imports System.Web
Imports System.Web.Security


Partial Class ctrlLogon
    Inherits System.Web.UI.UserControl

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
    Public ReadOnly Property EmailText() As System.Web.UI.WebControls.TextBox
        Get
            Return Me.txtEmail
        End Get
    End Property
    Public ReadOnly Property PasswordText() As System.Web.UI.WebControls.TextBox
        Get
            Return Me.txtPassword
        End Get
    End Property

    Public Const EMAIL_USER_SESSION As String = "EmailNameSession"

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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        If Not IsPostBack Then
            Me.txtEmail.Text = CType(ReadUserCookie().GetValue(0), String)
        End If
    End Sub

    Public Sub Login(ByVal Email As String, ByVal password As String, ByVal urlRedirect As String)
        Dim persistCookie As Boolean = False

        Dim user As UserData = cSecurity.ValidateUser(Email, password)

        If Not user Is Nothing Then
            'Limpiamos la session
            context.Session.Clear()
            Dim ck As HttpCookie
            Dim identity As Integer = CType(user.Tables(user.USER_TABLE).Rows(0).Item(user.IDUSER_FIELD), Integer)

            'Creamos una cookie de authentificacion... basada en roles
            ck = cSecurity.createCookie(identity, CType(AppSettings("TimeCookie"), Integer), persistCookie)

            context.Response.Cookies.Add(ck)

            Dim infoUser As String() = {CType(user.Tables(user.USER_TABLE).Rows(0).Item(user.EMAIL_FIELD), String)}
            'Creamos una cookie para guardar informacion de usuario ..
            'Correo electronico, etc, etc'
            ck = cSecurity.createUserCookie(identity, CType(AppSettings("TimeCookie"), Integer), True, infoUser)

            context.Response.Cookies.Add(ck)

            Dim strRedirect As String

            strRedirect = urlRedirect.Replace("|", "&")

            'Ponemos en sesion algunos datos del usuario
            context.Session.Add(EMAIL_USER_SESSION, CType(user.Tables(user.USER_TABLE).Rows(0).Item(UserData.EMAIL_FIELD), String))

            ' Redirecciona a la pagina indicada
            context.Response.Redirect(strRedirect, True)
        End If
    End Sub

    Public Sub Login()
        Dim persistCookie As Boolean = False

        Dim user As UserData = cSecurity.ValidateUser(Me.txtEmail.Text, Me.txtPassword.Text)

        If Not user Is Nothing Then
            Dim ck As HttpCookie
            Dim identity As Integer = CType(user.Tables(user.USER_TABLE).Rows(0).Item(user.IDUSER_FIELD), Integer)

            'Creamos una cookie de authentificacion... basada en roles
            ck = cSecurity.createCookie(identity, CType(AppSettings("TimeCookie"), Integer), persistCookie)
            Response.Cookies.Add(ck)

            Dim infoUser As String() = {CType(user.Tables(user.USER_TABLE).Rows(0).Item(user.EMAIL_FIELD), String)}
            'Creamos una cookie para guardar informacion de usuario ..
            'Correo electronico, etc, etc'
            ck = cSecurity.createUserCookie(identity, CType(AppSettings("TimeCookie"), Integer), True, infoUser)
            Response.Cookies.Add(ck)

            Dim strRedirect As String

            If Request.QueryString.Count > 0 Then
                Session("menu") = "true"
                strRedirect = Request.QueryString("ReturnUrl").Replace("|", "&")
            End If

            If strredirect.ToUpper.IndexOf("WELCOME.ASPX") < 0 Then
                Session("LogIn") = "true"
            End If


            'Ponemos en sesion algunos datos del usuario
            context.Session.Add(EMAIL_USER_SESSION, CType(user.Tables(user.USER_TABLE).Rows(0).Item(UserData.EMAIL_FIELD), String))

            If strRedirect <> "" Then
                ' Si se identifica un url en el request entonces se 
                ' Redirecciona a la pagina indicada

                ' Si se identifica un url en el request entonces se 
                ' Redirecciona a la pagina indicada

                Response.Redirect(strRedirect, True)
            End If

        Else
            Dim usuario As UsuarioHotelData
            Dim pass As String = crypto.EncryptString128Bit(Me.txtPassword.Text, crypto.PublicKey)
            usuario = GetUserByName(Me.txtEmail.Text, pass)

            If Not usuario Is Nothing Then


                Dim ck As HttpCookie
                Dim identity As Integer = CType(usuario.Tables(usuario.UsuarioTable).Rows(0).Item(usuario.iduserField), Integer)

                'Creamos una cookie de authentificacion... basada en roles
                ck = cSecurity.createUserHCookie(identity, CType(AppSettings("TimeCookie"), Integer), persistCookie)
                Response.Cookies.Add(ck)


                Dim infoUser As String() = {CType(usuario.Tables(usuario.UsuarioTable).Rows(0).Item(usuario.nameField), String)}
                'Creamos una cookie para guardar informacion de usuario ..
                'Correo electronico, etc, etc'
                ck = cSecurity.createCookieUsuarioHotel(identity, CType(AppSettings("TimeCookie"), Integer), True, infouser)
                Response.Cookies.Add(ck)

                Dim strRedirect As String

                If Request.QueryString.Count > 0 Then
                    Session("menu") = "true"
                    strRedirect = Request.QueryString("ReturnUrl").Replace("|", "&")
                End If

                If strredirect.ToUpper.IndexOf("WELCOME.ASPX") < 0 Then
                    Session("LogIn") = "true"
                End If


                'Ponemos en sesion algunos datos del usuario
                context.Session.Add(EMAIL_USER_SESSION, CType(usuario.Tables(usuario.UsuarioTable).Rows(0).Item(usuario.nameField), String))

                If strRedirect <> "" Then
                    ' Si se identifica un url en el request entonces se 
                    ' Redirecciona a la pagina indicada

                    ' Si se identifica un url en el request entonces se 
                    ' Redirecciona a la pagina indicada

                    Response.Redirect(strRedirect, True)
                End If

            Else
                'Mensaje al usuario
                CType(Me.Page, PaginaBase).SetFocus(PasswordText)
                cvInvalid.Text = PortalCulture.GetString("00184") ' "Correo electronico o contraseña invalida"
                cvInvalid.IsValid = False
            End If
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Me.rfvEmailRequired.Text = PortalCulture.GetString("00179")
        Me.lblEmail.Text = PortalCulture.GetString("00181", True)

        Me.lblPassword.Text = PortalCulture.GetString("00183", True)

        Me.cvInvalid.Text = PortalCulture.GetString("00184")
        Me.rfvPasswordRequired.Text = PortalCulture.GetString("00182")
    End Sub

    'Esta funcion hay que pasarla a usuariohotelfacade

    Public Function GetUserByName(ByVal name As String, ByVal RASPassword As String) As UsuarioHotelData
        Dim dataSet As UsuarioHotelData
        ' Obtenemos User en el dataset con este email
        With New UsuarioHotelFacade
            dataSet = .GetUserByName(name)
        End With

        ' Verificamos el password
        With dataSet.Tables(UsuarioHotelData.UsuarioTable).Rows
            If (.Count = 1) Then
                Dim dbPasswordRas As String = ""
                Dim valido As Boolean = False
                dbPasswordRas = .Item(0).Item(UsuarioHotelData.passwordField).ToString
                If RASPassword = dbPasswordRas Then
                    valido = True
                    Return dataSet
                End If
            End If
        End With
        Return Nothing
    End Function
End Class



