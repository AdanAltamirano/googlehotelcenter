
Partial Class EliminarContenido
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
    Delegate Sub ChangeConte(ByVal sender As Object, ByVal e As ArgsConte)
    Public Event ChangeContenido As ChangeConte

    Public Property idContenido() As Long
        Get
            Return viewstate("idContenido")
        End Get
        Set(ByVal Value As Long)
            viewstate("idContenido") = Value
        End Set
    End Property

    Public Property idElemento() As Long
        Get
            Return viewstate("idElemento")
        End Get
        Set(ByVal Value As Long)
            viewstate("idElemento") = Value
        End Set
    End Property

    Public Property text() As String
        Get
            Return lnkCambiar.Text
        End Get
        Set(ByVal Value As String)
            lnkCambiar.Text = "Eliminar " & Value
        End Set
    End Property

    Public Sub OnChangeContenido(ByVal idcon As Long, ByVal idele As Long)
        Dim e As New ArgsConte(idcon, idele, False, False, False, False, False)
        RaiseEvent ChangeContenido(Me, e)
    End Sub

    Private Sub lnkCambiar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkCambiar.Click
        OnChangeContenido(Me.idContenido, Me.idElemento)
    End Sub

    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        OnChangeContenido(Me.idContenido, Me.idElemento)
    End Sub
End Class

