Imports Contenido.presentacion
Partial Class PoliciesInformation
    Inherits Paginabase
    Protected WithEvents Policies1 As Policies

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
     If Not MyBase.IsHotelSelected Then MyBase.redirectTo(Me.pages.Home)

        If Not IsPostBack Then
            Policies1.pagina = "Registro/PoliciesInformation.aspx"
            Policies1.nota = "Modificación del listado de las políticas del hotel."
            Dim ds As DataTable
            With New PresentacionOpciones
                ds = .GetIdModulebyName("Politicas")
                If Not ds Is Nothing AndAlso ds.Rows.Count = 1 Then
                    Policies1.IdModulo = ds.Rows(0)("IdModulo")
                    Policies1.IdEmpresa = Me.cInfoActual.Empresa
                    Policies1.ModeView = Opciones.ViewMode.Edit
                    Policies1.IdIdioma = PortalCulture.GetIDCulture
                End If
            End With
        End If
    End Sub

    Private Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Me.lbltitle.Text = PortalCulture.GetString("A00722")
    End Sub
End Class
