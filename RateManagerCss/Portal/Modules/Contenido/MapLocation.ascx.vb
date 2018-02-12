Imports System.IO
Imports Contenido.Comun
Imports Contenido.presentacion

Partial Class MapLocation
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


    Protected WithEvents ctrlLinkDescription As CambiarContenido
    Protected WithEvents ctrlLinkImage As CambiarContenido
    Protected WithEvents ctrlTitle As CambiarContenido

    Private Const TITLE As String = "Title"
    Private Const DESCRIPTION As String = "MapLocation_DescriptionML"
    Private Const IMAGE As String = "MapLocation_ImageML"

    Private Sub CargaTodo()
        Dim strPath As String
        ' Obtenemos el contenido para la descripcion de habitacion
        Me.CargaContenido(DESCRIPTION, Me.lblDescription, Opciones.TipoControl.Label, Me.ctrlLinkDescription, Nothing, Nothing, False, False, True, False, False)
        Me.CargaContenido(IMAGE, Me.imgMap, Opciones.TipoControl.Imagen, Me.ctrlLinkImage, Nothing, Nothing, True, False, False, True, True)

        Me.ctrlLinkImage.NoPaso = "1."
        Me.ctrlLinkImage.Texto = PortalCulture.GetString("A00674")

        
        Me.ctrlLinkDescription.NoPaso = "2."
        Me.ctrlLinkDescription.Texto = PortalCulture.GetString("A00676") '"Presione el Icono del lapiz para cambiar el El texto"


        If MyBase.ModeView = Opciones.ViewMode.Edit Then
            If Me.lblDescription.Text.Trim = "" Then
                Me.lblDescription.Text = "[ " & PortalCulture.GetString("A00709") & " ]"
            End If
            strPath = Me.imgMap.ImageUrl

            If strPath = vbNullString Then
                strPath = GeRequestApplicationPath(String.Concat("/Images/NotAvailable", PortalCulture.GetString("A00000"), ".gif"))
            End If
            If lblTitle.Text.Trim = "" Then
                lblTitle.Text = "[ " & PortalCulture.GetString("A00690") & " ]"
            End If
            Me.imgMap.ImageUrl = strPath
        End If
    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        If Not IsPostBack Then
            ctrlLinkDescription.Pagina = "Registro/MapLocation.aspx"
            ctrlLinkDescription.Nota = "Edición de la información de ubicación del hotel."
            ctrlLinkImage.Pagina = "Registro/MapLocation.aspx"
            ctrlLinkImage.Nota = "Edición de la información de ubicación del hotel."
        End If
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
        Me.lblTitle.Text = PortalCulture.GetString("A00710")
    End Sub
End Class


