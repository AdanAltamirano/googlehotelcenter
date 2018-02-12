Imports Contenido.presentacion
Partial Class ActivitiesInformation
    Inherits PaginaBase
    Protected WithEvents Activities1 As Activities
    Protected WithEvents CtlServices1 As ctlServices

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
        Activities1.Pagina = "Registro/ActivitiesInformation.aspx"
        Activities1.Nota = "Edición de las actividades del hotel"
        CtlServices1.Pagina = "Registro/ActivitiesInformation.aspx"
        CtlServices1.Nota = "Edición de las actividades del hotel"

        If Not IsPostBack Then
            Dim ds As DataTable
            With New PresentacionOpciones
                ds = .GetIdModulebyName("Activities")
                If Not ds Is Nothing AndAlso ds.Rows.Count = 1 Then
                    Activities1.IdModulo = ds.Rows(0)("IdModulo")
                    Activities1.IdEmpresa = Me.cInfoActual.Empresa
                    Activities1.ModeView = Opciones.ViewMode.Edit
                    Activities1.IdIdioma = PortalCulture.GetIDCulture

                End If
                CtlServices1.IdEmpresa = Me.cInfoActual.Empresa
                CtlServices1.ModeView = Opciones.ViewMode.Edit
                CtlServices1.IdIdioma = PortalCulture.GetIDCulture

            End With

        End If
    End Sub

    Private Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Me.lblTitle.Text = PortalCulture.GetString("A00715")
    End Sub
End Class
