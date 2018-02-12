Imports System.IO

Partial Class AreaAttraction
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
	Protected WithEvents CambiarContenidoAttractionText As CambiarContenido
	Protected WithEvents CambiarContenidoAttractionImage As CambiarContenido

	Protected WithEvents ctrlTitle As CambiarContenido
    Private Const TITLE As String = "Title"
    Public Property pagina() As String
        Get
            Return CambiarContenidoAttractionText.Pagina
        End Get
        Set(ByVal Value As String)
            CambiarContenidoAttractionImage.Pagina = Value
            ctrlTitle.Pagina = Value
            CambiarContenidoAttractionText.Pagina = Value
        End Set
    End Property
    Public Property nota() As String
        Get
            Return CambiarContenidoAttractionText.Nota
        End Get
        Set(ByVal Value As String)
            CambiarContenidoAttractionImage.Nota = Value
            ctrlTitle.Nota = Value
            CambiarContenidoAttractionText.Nota = Value
        End Set
    End Property

    Private Sub CargaTodo()
        Dim strPath As String
        Me.ctrlTitle.NoPaso = "1."
        Me.ctrlTitle.Texto = PortalCulture.GetString("A00675")

        Me.CambiarContenidoAttractionImage.NoPaso = "2."
        Me.CambiarContenidoAttractionImage.Texto = PortalCulture.GetString("A00674")

        Me.CambiarContenidoAttractionText.NoPaso = "3."
        Me.CambiarContenidoAttractionText.Texto = PortalCulture.GetString("A00676")    '"Presione el Icono del lapiz para cambiar el El texto"


        Me.CargaContenido("lblAttractionText", _
        Me.lblAttractionText, _
        Opciones.TipoControl.Label, _
        Me.CambiarContenidoAttractionText, _
        Nothing, Nothing, False, False, True, False, False)

        ' Imagen de la atracción
        Me.CargaContenido("imgAreaAttraction", _
        Me.imgArea, _
        Opciones.TipoControl.Imagen, _
        Me.CambiarContenidoAttractionImage, _
        Nothing, Nothing, _
        True, False, False, True, True)

        Me.CargaContenido(TITLE, Me.lblTitle, Opciones.TipoControl.Label, Me.ctrlTitle, Nothing, Nothing, False, False, True, False, False)

        If MyBase.ModeView = Opciones.ViewMode.Edit Then
            If Me.lblAttractionText.Text.Trim = "" Then
                Me.lblAttractionText.Text = "[ " & PortalCulture.GetString("A00696") & " ]"
            End If
            If lblTitle.Text.Trim = "" Then
                lblTitle.Text = "[ " & PortalCulture.GetString("A00690") & " ]"
            End If
            If imgArea.ImageUrl = vbNullString Then
                imgArea.ImageUrl = GeRequestApplicationPath("/Images/NotAvailable.jpg")
            End If
        End If


    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Texto de descripción de la atracción
        If Me.ModeView <> Opciones.ViewMode.gView Then
            Me.lPanel.Visible = True
            Me.gPanel.Visible = False
            CargaTodo()
        Else
            Me.lPanel.Visible = False
            Me.gPanel.Visible = True
        End If
    End Sub

    Private Sub Page_ChangeContenido(ByVal sender As Object, ByVal e As ArgsConte) Handles MyBase.ChangeContenido
        CargaTodo()
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        '  Me.lblTitle.Text = PortalCulture.GetString("A00544")
    End Sub
End Class
