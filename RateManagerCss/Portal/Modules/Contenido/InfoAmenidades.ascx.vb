Imports System.IO
Imports Contenido.Comun


Partial Class InfoAmenidades
    Inherits Opciones
    '   Private Const Dato1 As String = "ListaAmenidades1"
    '   Private Const Dato2 As String = "ListaAmenidades2"
    Private Const Dato3 As String = "InfoPropiedad"
    Private Const Dato4 As String = "imgPropAmenidades"
    Protected WithEvents CtrlAmen As CtrlAmenidades

    Protected WithEvents ctrlTitle As CambiarContenido
	Private Const TITLE As String = "Title"

#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Para el Contenido
    Protected WithEvents lnkInfoPropiedad As CambiarContenido
    Protected WithEvents lnkImagPropiedades As CambiarContenido


    'NOTA: el Diseñador de Web Forms necesita la siguiente declaración del marcador de posición.
    'No se debe eliminar o mover.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: el Diseñador de Web Forms requiere esta llamada de método
        'No la modifique con el editor de código.        
        InitializeComponent()
    End Sub

#End Region

	Private Sub Cargatodo()
		Dim strPath As String

        'Me.ctrlTitle.NoPaso = "1."
        'Me.ctrlTitle.Texto = PortalCulture.GetString("A00340")	   ' "Presione el Icono del lapiz para cambiar el Titulo"

        Me.lnkImagPropiedades.NoPaso = "1."
        Me.lnkImagPropiedades.Texto = PortalCulture.GetString("A00674")

        Me.lnkInfoPropiedad.NoPaso = "2."
        Me.lnkInfoPropiedad.Texto = PortalCulture.GetString("A00676")    '"Presione el Icono del lapiz para cambiar el El texto"

		'Me.CargaContenido(TITLE, Me.lblTitle, Opciones.TipoControl.Label, Me.ctrlTitle, Nothing, Nothing, False, False, True, False, False)
		CargaContenido(Dato3, lblInfoPropiedad, Opciones.TipoControl.Label, lnkInfoPropiedad, Nothing, Nothing, False, False, True, False, False)
		CargaContenido(Dato4, imgPropAmenidades, Opciones.TipoControl.Imagen, lnkImagPropiedades, Nothing, Nothing, True, False, False, False, False)
		If MyBase.ModeView = Opciones.ViewMode.Edit Then

			If Me.lblInfoPropiedad.Text = vbNullString Then
                Me.lblInfoPropiedad.Text = "[ " & PortalCulture.GetString("A00689") & " ]"
			End If

			If lblTitle.Text.Trim = "" Then
                lblTitle.Text = "[ " & PortalCulture.GetString("A00690") & " ]"
			End If
        End If
        If imgPropAmenidades.ImageUrl = vbNullString Then
            imgPropAmenidades.ImageUrl = GeRequestApplicationPath("/Images/NotAvailable.jpg")
        End If

        'Util.Images.loadImage(Me.imgPropAmenidades.ImageUrl, Me.imgPropAmenidades, MyBase.ModeView)
	End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            CtrlAmen.ModeV = Me.ModeView
            CtrlAmen.IdEmpresa = Me.IdEmpresa
            CtrlAmen.Idrubro = Me.IdRubro
            CtrlAmen.IdIdioma = Me.IdIdioma
            lnkImagPropiedades.Nota = "Edición de la imagen de las amenidades del hotel"
            lnkImagPropiedades.Pagina = "Registro/AmenitiesInformation.aspx"
            lnkInfoPropiedad.Pagina = "Registro/AmenitiesInformation.aspx"
            lnkInfoPropiedad.Nota = "Edición del contenido de las amenidades del hotel"
            CtrlAmen.Pagina = "Registro/AmenitiesInformation.aspx"
            CtrlAmen.Nota = "Edición de las amenidades del hotel"
        End If
        If Me.ModeView <> Opciones.ViewMode.gView Then
            Me.lPanel.Visible = True
            Me.gPanel.Visible = False
            Cargatodo()
        Else
            Me.lPanel.Visible = False
            Me.gPanel.Visible = True
            Me.CtrlAmen.Visible = False
        End If
    End Sub

    Private Sub Page_ChangeContenido(ByVal sender As Object, ByVal e As ArgsConte) Handles MyBase.ChangeContenido
        Cargatodo()
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Me.lblTitle.Text = PortalCulture.GetString("A00691")
    End Sub

End Class
