'Imports Contenido.PortalContenido
Imports Contenido.Comun
Imports Contenido.presentacion

Partial Class InfoPropiedad
    Inherits Opciones

    Private Const Dato1 As String = "PropInfo1"
    Private Const Dato2 As String = "PropInfo2"
    Private Const Dato4 As String = "PropInfo3"
    Private Const Dato3 As String = "imgPropiedad"
#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Para el Contenido
    Protected WithEvents lnkInfo1 As CambiarContenido
    Protected WithEvents lnkInfo2 As CambiarContenido
    Protected WithEvents lnkInfo3 As CambiarContenido
    Protected WithEvents lnkPropiedad As CambiarContenido
    'NOTA: el Diseñador de Web Forms necesita la siguiente declaración del marcador de posición.
    'No se debe eliminar o mover.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: el Diseñador de Web Forms requiere esta llamada de método
        'No la modifique con el editor de código.
        InitializeComponent()
    End Sub

#End Region

    Protected WithEvents ctrlTitle As CambiarContenido
    Private Const TITLE As String = "Title"

	Private Sub cargatodo()
        CargaContenido(Dato1, lblInfo1, Opciones.TipoControl.Label, lnkInfo1, Nothing, Nothing, False, False, True, False, False)
        CargaContenido(Dato2, lblInfo2, Opciones.TipoControl.Label, lnkInfo2, Nothing, Nothing, False, False, True, False, False)
        CargaContenido(Dato4, lblInfo3, Opciones.TipoControl.Label, lnkInfo3, Nothing, Nothing, False, False, True, False, False)
        CargaContenido(Dato3, imgPropiedad, Opciones.TipoControl.Imagen, lnkPropiedad, Nothing, Nothing, True, False, False, False, False)

		Me.ctrlTitle.NoPaso = "1."
        Me.ctrlTitle.Texto = PortalCulture.GetString("A00675")    ' "Presione el Icono del lapiz para cambiar el Titulo"

		Me.lnkPropiedad.NoPaso = "1."
        Me.lnkPropiedad.Texto = PortalCulture.GetString("A00674")

		Me.lnkInfo1.NoPaso = "2."
        Me.lnkInfo1.Texto = PortalCulture.GetString("A00676")    '"Presione el Icono del lapiz para cambiar el El texto"

		Me.lnkInfo2.NoPaso = "3."
        Me.lnkInfo2.Texto = PortalCulture.GetString("A00676")    '"Presione el Icono del lapiz para cambiar el El texto"

        Me.lnkInfo3.NoPaso = "4."
        Me.lnkInfo3.Texto = PortalCulture.GetString("A00676")    '"Presione el Icono del lapiz para cambiar el El texto"


        Me.lblTitle.Text = PortalCulture.GetString("A00682")

        'Me.CargaContenido(TITLE, Me.lblTitle, Opciones.TipoControl.Label, Me.ctrlTitle, Nothing, Nothing, False, False, True, False, False)

        '	If MyBase.ModeView = Opciones.ViewMode.Edit Then
        If Me.lblInfo1.Text = "" Then    '' If Me.lblInfo1.Text.Trim = "" Then
            Me.lblInfo1.Text = "[ " & PortalCulture.GetString("A00681") & " ]"
        End If
        If Me.lblInfo2.Text = "" Then     'If Me.lblInfo2.Text.Trim = "" Then
            Me.lblInfo2.Text = "[ " & PortalCulture.GetString("A00680") & " ]"
        End If
        If Me.lblInfo3.Text = "" Then     'If Me.lblInfo2.Text.Trim = "" Then
            Me.lblInfo3.Text = "[ " & PortalCulture.GetString("A00680") & " ]"
        End If
        Me.imgPropiedad.AlternateText = "[ " & PortalCulture.GetString("A00668") & " ]"
        '      End If

        ' Util.Images.loadImage(Me.imgPropiedad.ImageUrl, Me.imgPropiedad, MyBase.ModeView)
	End Sub

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		'Introducir aquí el código de usuario para inicializar la página
        '/**************************************************
        lnkPropiedad.Pagina = "Registro/InformacionPropiedad.aspx"
        lnkPropiedad.Nota = "Edición de la información de propiedad"
        lnkInfo1.Pagina = "Registro/InformacionPropiedad.aspx"
        lnkInfo1.Nota = "Edición de la información de propiedad"
        lnkInfo2.Pagina = "Registro/InformacionPropiedad.aspx"
        lnkInfo2.Nota = "Edición de la información de propiedad"
        lnkInfo3.Pagina = "Registro/InformacionPropiedad.aspx"
        lnkInfo3.Nota = "Edición de la información de propiedad"
		If ModeView <> Opciones.ViewMode.gView Then
			cargatodo()
		End If
	End Sub

	Private Sub Page_ChangeContenido(ByVal sender As Object, ByVal e As ArgsConte) Handles MyBase.ChangeContenido
		cargatodo()
    End Sub

    Private Sub Page_SetAll(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.SetAll
        cargatodo()
    End Sub


    'Private Sub Page_LoadedKeyWords(ByVal gKeyWords As bkHotelesGWS.KeyWords) Handles MyBase.LoadedKeyWords

    '	Me.lblTitle.Text = PortalCulture.GetString("A00541")
    '	lnkInfo1.Visible = False
    '	lnkInfo2.Visible = False
    '	lnkPropiedad.Visible = False

    '	Me.lblInfo2.Text = gKeyWords.Item("BOOK")
    'End Sub

    'Private Sub Page_LoadedKeyWords(ByVal gResponse As bkHotelesGWS.HotelDescription_Response) Handles MyBase.LoadedKeyWords
    '       Me.lblTitle.Text = PortalCulture.GetString("A00682")
    '	lblInfo1.Text = MyBase.GetProperCase(gResponse.KeyWords.Item("DESC"))
    '	'Hide portal objects
    '	lnkInfo2.Visible = False
    '       lnkInfo1.Visible = False
    '       lnkInfo3.Visible = False
    '	lnkPropiedad.Visible = False
    ' End Sub

    '  Private Sub Page_LoadedImages(ByVal aList As System.Collections.ArrayList) Handles MyBase.LoadedImages
    '	For i As Integer = 0 To aList.Count - 1
    '		Dim img As bkHotelesGWS.PictureInfo = aList.Item(i)
    '		If img.Size = "S" Then
    '			Me.imgPropiedad.ImageUrl = img.Url
    '			Exit Sub
    '		End If
    '	Next
    '	Me.imgPropiedad.Visible = False
    '  End Sub
End Class
