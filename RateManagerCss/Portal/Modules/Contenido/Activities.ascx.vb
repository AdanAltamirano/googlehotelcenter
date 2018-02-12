Imports System.IO

Partial Class Activities
    Inherits Opciones 'System.Web.UI.UserControl

#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label

    'NOTA: el Diseñador de Web Forms necesita la siguiente declaración del marcador de posición.
    'No se debe eliminar o mover.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: el Diseñador de Web Forms requiere esta llamada de método
        'No la modifique con el editor de código.
        InitializeComponent()
    End Sub

#End Region

    Protected ctrlImgRecreation As CambiarContenido
    Protected ctrlTitle As CambiarContenido
    Protected ctrlTextRecreation As CambiarContenido

    Public Property pagina() As String
        Get
            Return ctrlImgRecreation.Pagina
        End Get
        Set(ByVal Value As String)
            ctrlImgRecreation.Pagina = Value
            ctrlTitle.Pagina = Value
            ctrlTextRecreation.Pagina = Value
        End Set
    End Property
    Public Property nota() As String
        Get
            Return ctrlImgRecreation.Nota
        End Get
        Set(ByVal Value As String)
            ctrlImgRecreation.Nota = Value
            ctrlTitle.Nota = Value
            ctrlTextRecreation.Nota = Value
        End Set
    End Property

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Me.ModeView <> Opciones.ViewMode.gView Then
            Me.lPanel.Visible = True
            Me.gPanel.Visible = False
            Cargatodo()
        Else
            Me.lPanel.Visible = False
            Me.gPanel.Visible = True
        End If
    End Sub

    Private Sub Cargatodo()
        'Dim strPath As String
        'Texto de descripción 
        Me.ctrlImgRecreation.NoPaso = "1."
        Me.ctrlImgRecreation.Texto = PortalCulture.GetString("A00674")

        'Me.ctrlTitle.NoPaso = "2."
        'Me.ctrlTitle.Texto = PortalCulture.GetString("A00340") ' "Presione el Icono del lapiz para cambiar el Titulo"

        Me.ctrlTextRecreation.NoPaso = "2."
        Me.ctrlTextRecreation.Texto = PortalCulture.GetString("A00676")    '"Presione el Icono del lapiz para cambiar el El texto"

        Me.CargaContenido("Text", _
        Me.lblRecreationText, _
        Opciones.TipoControl.Label, _
        Me.ctrlTextRecreation, _
        Nothing, Nothing, False, False, True, False, False)
        ' Imagen 
        Me.CargaContenido("Image", _
        Me.imgRecreation, _
        Opciones.TipoControl.Imagen, _
        Me.ctrlImgRecreation, _
        Nothing, Nothing, _
        True, False, False, False, False)

        'Me.CargaContenido("Title", Me.lblTitle, Opciones.TipoControl.Label, Me.ctrlTitle, Nothing, Nothing, False, False, True, False, False)
        'Me.lblTitle.Text = ""
        If MyBase.ModeView = Opciones.ViewMode.Edit Then
            Dim strTemp As String
            strTemp = Me.IdIdioma
            If Me.lblRecreationText.Text.Trim = "" Then
                Me.lblRecreationText.Text = "[ " & PortalCulture.GetString("A00692") & " ]"
            End If
            'imgRecreation
            If lblTitle.Text.Trim = "" Then
                lblTitle.Text = "[ " & PortalCulture.GetString("A00690") & " ]"
            End If
            If imgRecreation.ImageUrl = vbNullString Then
                imgRecreation.ImageUrl = GeRequestApplicationPath("/Images/NotAvailable.jpg")
            End If

            'Util.Images.loadImage(Me.imgRecreation.ImageUrl, Me.imgRecreation, MyBase.ModeView)
        End If
    End Sub

    Private Sub Page_ChangeContenido(ByVal sender As Object, ByVal e As ArgsConte) Handles MyBase.ChangeContenido
        Cargatodo()
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Me.lblTitle.Text = PortalCulture.GetString("A00693")
    End Sub
End Class
