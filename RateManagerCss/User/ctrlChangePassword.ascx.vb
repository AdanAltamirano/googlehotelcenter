Imports System.Data
Imports System.Text
Imports System.Web.UI
Imports System.Web.UI.WebControls

Imports Portal.General.Common.Data
Imports Portal.General.Facade
Imports System.Security.Cryptography


Partial Class ctrlChangePassword
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

#Region "Propiedades de clase"
    ' Propiedades para recibir el id de usuario o email
    Public WriteOnly Property setUserId() As Integer
        Set(ByVal id As Integer)
            getDataUserById(id)
            loadUser()
        End Set
    End Property

    Public WriteOnly Property setUserByEmail() As String
        Set(ByVal email As String)
            getDataUserByEmail(email)
            loadUser()
        End Set
    End Property

#End Region

    Private user As UserData

    Public Const KEY_USER As String = "idEditAdmin"
    Private Success As Boolean = False

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
    End Sub

    ' Funcion que obtiene un userData con los datos del usuario
    Private Sub getDataUserById(ByVal id As Integer)
        With New cUserSystem
            viewstate.Add(KEY_USER, CType(.GetUserById(id), UserData))
        End With
    End Sub

    ' Funcion que obtiene un userData con los datos del usuario
    Private Sub getDataUserByEmail(ByVal email As String)
        With New cUserSystem
            viewstate.Add(KEY_USER, CType(.GetUserByEmail(email), UserData))
        End With
    End Sub

    Private Sub loadUser()
        user = CType(viewstate.Item(KEY_USER), UserData)
        If Not user Is Nothing AndAlso user.Tables(user.USER_TABLE).Rows.Count > 0 Then
            Me.lblEmailUser.Text = user.Tables(user.USER_TABLE).Rows(0).Item(user.EMAIL_FIELD)
        End If
    End Sub

    Private Function validUser() As Boolean
        Dim passras As String = user.Tables(user.USER_TABLE).Rows(0).Item(user.PASSWORDRAS_FIELD)
        Dim dbpass As Byte() = user.Tables(user.USER_TABLE).Rows(0).Item(user.PASSWORD_FIELD)
        If Not crypto.ComparePasswords(Me.txtPassword.Text.ToString, dbpass) Then
            If crypto.EncryptString128Bit(Me.txtPassword.Text.ToString, crypto.PublicKey) <> passras Then
                cvPassValid.IsValid = False
                Return False
            End If
        End If
        If Not Me.txtNewPassword.Text.Compare(Me.txtNewPassword.Text.ToString, Me.txtPasswordConfirm.Text.ToString) = 0 Then
            Return False
        End If
        Return True
    End Function

    Public Function changePassword() As Boolean
        changePassword = False
        If Me.lblEmailUser.Text = "" Then Return False
       
        If validUser() Then

            Dim newPassword As String = Me.txtNewPassword.Text
            Dim lastPassword As String = Me.txtPassword.Text

            Dim dsUser As UserData
            With New cUserSystem
                'If .changePasswordUser(PortalCulture.GetCulture.ToString, lblEmailUser.Text.Trim, newPassword, lastPassword, dsUser) Then
                '    Success = True
                '    changePassword = True
                '    CType(Me.Page, PaginaBase).guardalog("User/ChangePassword.aspx", PaginaBase.acciones.Modificar, "Cambió la contraseña " & Me.txtPassword.Text & " a: " & newPassword & " del email " & Me.lblEmailUser.Text)
                'End If
                If .changePasswordUser(PortalCulture.GetCulture.ToString, lblEmailUser.Text.Trim, newPassword, dsUser) Then
                    Success = True
                    changePassword = True
                    CType(Me.Page, PaginaBase).guardalog("User/ChangePassword.aspx", PaginaBase.acciones.Modificar, "Cambió la contraseña " & Me.txtPassword.Text & " a: " & newPassword & " del email " & Me.lblEmailUser.Text)
                End If
            End With
        End If

    End Function

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Me.lblMsg.Text = PortalCulture.GetString("M000090")
        Me.lblEmail.Text = PortalCulture.GetString("M000008")
        Me.rfvPasswordRequired.Text = PortalCulture.GetString("M000091")
        Me.lbloldpass.Text = PortalCulture.GetString("M000092", True)
        Me.cvPassValid.Text = PortalCulture.GetString("M000093")
        Me.cvPasswordConfirmCompare.Text = PortalCulture.GetString("M000094")
        Me.rfvNewPass.Text = PortalCulture.GetString("M000095")
        Me.lblnuevopass.Text = PortalCulture.GetString("M000096", True)
        Me.rfvPasswordConfirmRequired.Text = PortalCulture.GetString("M000097")
        Me.lblPasswordConfirm.Text = PortalCulture.GetString("M000098", True)
        If Success Then Me.lblMsg.Text = PortalCulture.GetString("M000099")
    End Sub
End Class



