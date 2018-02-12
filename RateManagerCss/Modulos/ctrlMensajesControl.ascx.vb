Partial Class ctrlMensajesControl
    Inherits ucBase

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

    Enum Tipos
        Prompt  ' Pregunta un yes o no
        Input   ' Permite la entrada de datos
        Information ' Solo un boton de OK
    End Enum

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página        
    End Sub
    Protected Overrides Sub OnPreRender(ByVal e As System.EventArgs)
        Response.Write("<script>var ClientID='" & Me.ClientID & "' </script>")
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender

    End Sub

    Private ReadOnly Property getcancel() As String
        Get
            Return "cancel('" & PnlBox.ClientID & "','" & TblPnl.ClientID & "')"
        End Get
    End Property

    Public Function getShow(ByVal obj As String, ByVal title As String, ByVal question As String) As String
        lblTitle.Text = title
        lblPrompt.Text = question
        btnCancel.Value = "No"
        btnOk.Value = PortalCulture.GetString("00030")
        btnCancel.Attributes.Add("onclick", getcancel)
        Dim hideCmbs As New System.Text.StringBuilder
        Return "javascript:show('" & PnlBox.ClientID & "','" & TblPnl.ClientID & "','" & obj & "')"

    End Function

End Class

