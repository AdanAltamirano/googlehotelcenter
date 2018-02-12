Partial Class ctrlAmenities
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


	Protected WithEvents CtrlAmenidades1 As CtrlAmenidades
	Protected WithEvents ctrlTitle As CambiarContenido
	Private Const TITLE As String = "Title"


    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        If Me.ModeView <> Opciones.ViewMode.gView Then
            MyBase.IsRender = True

            Me.ctrlTitle.NoPaso = "1."
            Me.ctrlTitle.Texto = PortalCulture.GetString("A00675")    ' "Presione el Icono del lapiz para cambiar el Titulo"

            'Me.CargaContenido(TITLE, Me.lblTitle, Opciones.TipoControl.Label, Me.ctrlTitle, Nothing, Nothing, False, False, True, False, False)
            'Introducir aquí el código de usuario para inicializar la página
            'Me.lblTitle.Text = PortalCulture.GetString("A00542")
            'If lblTitle.Text.Trim = "" Then
            lblTitle.Text = PortalCulture.GetString("A00688")
            'End If

           
            'If Not IsPostBack Then CtrlAmenidades1.CargoContenido()
        Else
            CtrlAmenidades1.Visible = False
            ctrlTitle.Visible = False
            lblTitle.Visible = False
            Me.Visible = False
        End If
    End Sub

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            CtrlAmenidades1.IdIdioma = MyBase.IdIdioma
            CtrlAmenidades1.IdEmpresa = MyBase.IdEmpresa
            CtrlAmenidades1.Idrubro = MyBase.IdRubro
            CtrlAmenidades1.ModeV = MyBase.ModeView
        End If

    End Sub
End Class
