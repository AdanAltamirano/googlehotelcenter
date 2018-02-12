Imports Portal.Facade
Imports Portal.Common
Imports System.Configuration.ConfigurationManager
Imports Contenido.presentacion
Partial Class AmenitiesInformation
    Inherits PaginaBase
    Protected WithEvents InfoAmenidades1 As InfoAmenidades
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
            Dim ds As DataTable
            With New PresentacionOpciones
                ds = .GetIdModulebyName("InfoAmenidades")
                If Not ds Is Nothing AndAlso ds.Rows.Count = 1 Then
                    InfoAmenidades1.IdModulo = ds.Rows(0)("IdModulo")
                    InfoAmenidades1.IdEmpresa = Me.cInfoActual.Empresa
                    InfoAmenidades1.ModeView = Opciones.ViewMode.Edit
                    InfoAmenidades1.IdIdioma = PortalCulture.GetIDCulture
                End If
            End With
        End If
    End Sub

    Private Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Me.lbltitle.Text = PortalCulture.GetString("A00716")
    End Sub
End Class
