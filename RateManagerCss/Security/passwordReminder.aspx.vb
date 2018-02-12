Imports Portal.General.Facade

Partial Class passwordReminder
    Inherits PaginaBase

#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected CtrlPasswordReminder1 As ctrlPasswordReminder

    'NOTA: el Diseñador de Web Forms necesita la siguiente declaración del marcador de posición.
    'No se debe eliminar o mover.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: el Diseñador de Web Forms requiere esta llamada de método
        'No la modifique con el editor de código.
        InitializeComponent()
    End Sub

#End Region

    Private SeEnvio As Boolean = False
    Private Enviando As Boolean = False

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        Me.hplBack.NavigateUrl = GeRequestApplicationPath("/Security/logon.aspx?ReturnUrl=/RateManager/Portal/Pages/Welcome.aspx")
        If Not IsPostBack Then
            Me.panelPswChange.Visible = True
            Me.PanelMsg.Visible = False
        End If
    End Sub


    Private Sub btnContinue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnContinue.Click
        Response.Redirect("logon.aspx?ReturnUrl=/RateManager/Portal/Pages/Welcome.aspx")
        Me.panelPswChange.Visible = True
        Me.PanelMsg.Visible = False
        'MyBase.redirectTo(basePage.pages.Home)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Me.lblRecordarContrasena.Text = PortalCulture.GetString("00185")
        Me.lnkReminderme.Text = PortalCulture.GetString("00186")
        Me.hplBack.Text = PortalCulture.GetString("A00143")
        If Enviando Then
            Me.lblTitle.Text = PortalCulture.GetString("00187")
            If SeEnvio Then
                Me.lblMessage.Text = PortalCulture.GetString("00188")
            Else
                Me.lblMessage.Text = PortalCulture.GetString("00189")
            End If
        End If
        Me.btnContinue.Text = PortalCulture.GetString("00190")
    End Sub

    Private Sub lnkReminderme_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkReminderme.Click
        Enviando = True
        If CtrlPasswordReminder1.reminderPassword() Then
            SeEnvio = True
            '"Se envio un correo electronico a la cuenta de correo solicitada"        
        End If
        If Page.IsValid Then
            Me.panelPswChange.Visible = False
            Me.PanelMsg.Visible = True
        End If
    End Sub
End Class
