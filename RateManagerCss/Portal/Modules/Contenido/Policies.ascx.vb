Partial Class Policies
    Inherits Opciones


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
    Protected WithEvents poli As CtrlPoliticas
    Protected WithEvents CambiarContenidoPoliciesText As CambiarContenido
    Public Property pagina() As String
        Get
            Return CambiarContenidoPoliciesText.Pagina
        End Get
        Set(ByVal Value As String)
            CambiarContenidoPoliciesText.Pagina = Value
            poli.Pagina = Value
        End Set
    End Property
    Public Property nota() As String
        Get
            Return CambiarContenidoPoliciesText.Nota
        End Get
        Set(ByVal Value As String)
            CambiarContenidoPoliciesText.Nota = Value
            poli.Nota = Value
        End Set
    End Property

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            poli.ModeV = Me.ModeView
            poli.IdEmpresa = Me.IdEmpresa
            poli.Idrubro = Me.IdRubro
            poli.IdIdioma = Me.IdIdioma
        End If


        If Me.ModeView <> Opciones.ViewMode.gView Then
            Me.lPanel.Visible = True
            Me.gPanel.Visible = False
            Cargatodo()
        Else
            Me.lPanel.Visible = False
            Me.gPanel.Visible = True
        End If
    End Sub

    Private Sub Cargatodo()
        Me.CambiarContenidoPoliciesText.NoPaso = "1."
        Me.CambiarContenidoPoliciesText.Texto = PortalCulture.GetString("A00676") ' "Presione el Icono del lapiz para cambiar el Titulo"

        Me.CargaContenido("lblPoliciesText", _
        Me.lblPoliciesText, _
        Opciones.TipoControl.Label, _
        Me.CambiarContenidoPoliciesText, _
        Nothing, Nothing, False, False, True, False, False)
        'Introducir aquí el código de usuario para inicializar la página
        If MyBase.ModeView = Opciones.ViewMode.Edit Then
            If Me.lblPoliciesText.Text.Trim = "" Then
                Me.lblPoliciesText.Text = "[ " & PortalCulture.GetString("A00699") & " ]"
            End If
        End If
    End Sub

    Private Sub Page_ChangeContenido(ByVal sender As Object, ByVal e As ArgsConte) Handles MyBase.ChangeContenido
        Cargatodo()
    End Sub


    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Me.lblPoliciesTitle.Text = PortalCulture.GetString("A00700")
    End Sub
End Class
