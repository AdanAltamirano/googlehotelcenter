Imports System.Configuration.ConfigurationManager
Imports Contenido.presentacion

Partial Class TravelsInformation
    Inherits PaginaBase
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents ctrlTravelGuides As ctrlTravelGuides

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(Me.pages.Home)
        If Not IsPostBack Then
            'CtrlEventsMeeting.pagina = "Registro/AttractionInformation.aspx"
            'CtrlEventsMeeting.nota = "Edición de eventos en el hotel."
            Dim ds As DataTable
            With New PresentacionOpciones
                ds = .GetIdModulebyName("TravelGuide")
                If Not ds Is Nothing AndAlso ds.Rows.Count = 1 Then
                    ctrlTravelGuides.IdModulo = ds.Rows(0)("IdModulo")
                    ctrlTravelGuides.IdEmpresa = Me.cInfoActual.Empresa
                    ctrlTravelGuides.ModeView = Opciones.ViewMode.Edit
                    ctrlTravelGuides.IdIdioma = PortalCulture.GetIDCulture
                End If
            End With
        End If
    End Sub

    Private Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Me.lbltitle.Text = PortalCulture.GetString("A00745")
    End Sub

End Class
