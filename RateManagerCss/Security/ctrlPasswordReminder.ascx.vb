Imports Portal.General.Common.Data
Imports Portal.General.Facade


Partial Class ctrlPasswordReminder
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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
    End Sub

    Public Function reminderPassword() As Boolean
        If Not Page.IsValid Then Return False
        With New cUserSystem
            reminderPassword = .restorePasswordUser(PortalCulture.GetCulture.ToString, Me.txtEmail.Text.Trim)
        End With
    End Function

    Private Sub loadResources()

        Me.rfvEmailRequired.Text = PortalCulture.GetString("00179")
        Me.revEmailExpresion.Text = PortalCulture.GetString("00180")
        Me.lblEmail.Text = PortalCulture.GetString("00181", True)

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        loadResources()
    End Sub

End Class
