Partial Class ctrlAmenidadesCuartos
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

    Protected WithEvents ctrlAmenidadesCuarto1 As ctrlAmenidadesCuarto
    Protected WithEvents ctrlTitle As CambiarContenido
    Private Const TITLE As String = "Title"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        If Not IsPostBack Then
            ctrlAmenidadesCuarto1.Pagina = "Registro/RoomAmenitiesInformation.aspx"
            ctrlAmenidadesCuarto1.Nota = "Edición de la información de amenidaddes de habitación."
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        If Me.ModeView <> Opciones.ViewMode.gView Then
            MyBase.IsRender = True

            Me.ctrlTitle.NoPaso = "1."
            Me.ctrlTitle.Texto = PortalCulture.GetString("A00675")    ' "Presione el Icono del lapiz para cambiar el Titulo"

            'Me.CargaContenido(TITLE, Me.lblTitle, Opciones.TipoControl.Label, Me.ctrlTitle, Nothing, Nothing, False, False, True, False, False)
            lblTitle.Text = PortalCulture.GetString("A00705")

            ctrlAmenidadesCuarto1.IdIdioma = MyBase.IdIdioma
            ctrlAmenidadesCuarto1.IdEmpresa = MyBase.IdEmpresa
            ctrlAmenidadesCuarto1.ModeV = MyBase.ModeView
            'If Not IsPostBack Then CtrlAmenidades1.CargoContenido()
        Else
            'If Me.ModeView <> Opciones.ViewMode.gView Then
            'ctrlAmenidadesCuarto1.IdIdioma = MyBase.IdIdioma
            'ctrlAmenidadesCuarto1.IdEmpresa = MyBase.IdEmpresa
            'ctrlAmenidadesCuarto1.ModeV = MyBase.ModeView
            'lblTitle.Visible = False
            'Else
                ctrlAmenidadesCuarto1.Visible = False
                ctrlTitle.Visible = False
                lblTitle.Visible = False
                Me.Visible = False
            'End If
        End If

    End Sub
End Class
