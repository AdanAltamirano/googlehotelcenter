Imports System.IO
Imports Contenido.Comun
Imports Contenido.presentacion


Partial Class CtrlRoom
    Inherits Opciones
    'Inherits System.Web.UI.UserControl

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
    Protected WithEvents ctrlTitle As CambiarContenido
    Protected WithEvents ctrlImage As CambiarContenido


    Private Const TITLE As String = "Rooms_Title"
    Private Const DESCRIPTION As String = "Rooms_DescriptionRoom"
    Private Const IMAGE As String = "Rooms_ImageRoom"

    Private Sub cargatodo()
        Dim strPath As String
        'Me.CargaContenido(TITLE, Me.lblTitle, Opciones.TipoControl.Label, Me.ctrlTitle, Nothing, Nothing, False, False, True, False, False)
        Me.CargaContenido(DESCRIPTION, Me.lblDescription, Opciones.TipoControl.Label, Me.ctrlLinkDescription, Nothing, Nothing, False, False, True, False, False)
        Me.CargaContenido(IMAGE, Me.imgRoom, Opciones.TipoControl.Imagen, Me.ctrlImage, Nothing, Nothing, True, False, False, False, False)

        Me.ctrlImage.NoPaso = "1."
        Me.ctrlImage.Texto = PortalCulture.GetString("A00674")

        Me.ctrlTitle.NoPaso = "1."
        Me.ctrlTitle.Texto = PortalCulture.GetString("A00675") ' "Presione el Icono del lapiz para cambiar el Titulo"

        Me.ctrlLinkDescription.NoPaso = "2."
        Me.ctrlLinkDescription.Texto = PortalCulture.GetString("A00676") '"Presione el Icono del lapiz para cambiar el El texto"




        'Me.lnkMoreInfo.Text = PortalCulture.GetString("A00018")

        If MyBase.ModeView = Opciones.ViewMode.Edit Then
            If Me.lblDescription.Text.Trim = "" Then
                Me.lblDescription.Text = "[ " & PortalCulture.GetString("A00708") & " ]"
            End If
            
            strPath = Me.imgRoom.ImageUrl

            If strPath = vbNullString Then
                strPath = GeRequestApplicationPath(String.Concat("/Images/NotAvailable", PortalCulture.GetString("A00000"), ".gif"))
            Else
                Try
                Catch ex As Exception
                    strPath = GeRequestApplicationPath(String.Concat("/Images/NotAvailable", PortalCulture.GetString("A00000"), ".gif"))
                End Try
            End If
            Me.imgRoom.ImageUrl = strPath
        End If
    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        'Obtenemos el contenido para la descripcion de habitacion
        If Not IsPostBack Then
            ctrlImage.Nota = "Edición de la información de habitaciones."
            ctrlImage.Pagina = "Registro/RoomsInformation.aspx"
            ctrlLinkDescription.Nota = "Edición de la información de habitaciones."
            ctrlLinkDescription.Pagina = "Registro/RoomsInformation.aspx"
        End If
        If Me.ModeView <> Opciones.ViewMode.gView Then
            cargatodo()
        End If
    End Sub

    Private Sub Page_ChangeContenido(ByVal sender As Object, ByVal e As ArgsConte) Handles MyBase.ChangeContenido
        cargatodo()
    End Sub


    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Me.lblTitle.Text = PortalCulture.GetString("A00041")
    End Sub
End Class
