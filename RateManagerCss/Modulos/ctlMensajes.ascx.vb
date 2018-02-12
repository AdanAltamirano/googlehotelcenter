Partial Class ctlMensajes
    Inherits ucBase

#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents txtvalue As System.Web.UI.HtmlControls.HtmlInputHidden

    'NOTA: el Diseñador de Web Forms necesita la siguiente declaración del marcador de posición.
    'No se debe eliminar o mover.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: el Diseñador de Web Forms requiere esta llamada de método
        'No la modifique con el editor de código.
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

    Public Function getShow(ByVal LK As LinkButton, ByVal title As String, ByVal question As String) As String
        lblTitle.Text = title
        lblPrompt.Text = question
        btnCancel.Value = "No"
        btnOk.Value = PortalCulture.GetString("00030")
        btnCancel.Attributes.Add("onclick", getcancel)
        LK.Attributes.Add("onclick", "_ok();")
        Dim hideCmbs As New System.Text.StringBuilder
        Return "javascript:show('" & PnlBox.ClientID & "','" & TblPnl.ClientID & "','" & LK.ClientID & "')"


    End Function

    Public Function getShow(ByVal obj As String, ByVal title As String, ByVal question As String, ByVal enviamsg As Boolean) As String
        lblTitle.Text = title
        lblPrompt.Text = question
        btnCancel.Value = "No"
        btnOk.Value = PortalCulture.GetString("00030")
        btnCancel.Attributes.Add("onclick", getcancel)
        Dim hideCmbs As New System.Text.StringBuilder
        Return "javascript:show('" & PnlBox.ClientID & "','" & TblPnl.ClientID & "','" & obj & "','" & question & "')"


    End Function

End Class
