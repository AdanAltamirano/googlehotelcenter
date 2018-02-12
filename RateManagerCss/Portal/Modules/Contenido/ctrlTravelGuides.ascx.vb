Partial Class ctrlTravelGuides
    Inherits Opciones 'Inherits System.Web.UI.UserControl

    Private Const Dato1 As String = "PropTravel"                                     
    'Private Const Dato2 As String = "PropInfo2"
    'Private Const Dato4 As String = "PropInfo3"
    'Protected WithEvents lblInfo2 As System.Web.UI.WebControls.Label
    'Protected WithEvents lblInfo3 As System.Web.UI.WebControls.Label
    Private Const Dato3 As String = "imgTravel"

    Protected WithEvents lnkInfo1 As CambiarContenido
    Protected WithEvents lnkPropiedad As CambiarContenido
    Protected WithEvents ctrlTitle As CambiarContenido

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub cargatodo()
        CargaContenido(Dato1, lblInfo1, Opciones.TipoControl.Label, lnkInfo1, Nothing, Nothing, False, False, True, False, False)
        CargaContenido(Dato3, imgPropiedad, Opciones.TipoControl.Imagen, lnkPropiedad, Nothing, Nothing, True, False, False, False, False)

        Me.ctrlTitle.NoPaso = "1."
        Me.ctrlTitle.Texto = PortalCulture.GetString("A00675")    ' "Presione el Icono del lapiz para cambiar el Titulo"

        Me.lnkPropiedad.NoPaso = "1."
        Me.lnkPropiedad.Texto = PortalCulture.GetString("A00674")

        Me.lnkInfo1.NoPaso = "2."
        Me.lnkInfo1.Texto = PortalCulture.GetString("A00676")    '"Presione el Icono del lapiz para cambiar el El texto"
        Me.lblTitle.Text = PortalCulture.GetString("A00745")

        'Me.CargaContenido(TITLE, Me.lblTitle, Opciones.TipoControl.Label, Me.ctrlTitle, Nothing, Nothing, False, False, True, False, False)

        '	If MyBase.ModeView = Opciones.ViewMode.Edit Then
        If Me.lblInfo1.Text = "" Then    '' If Me.lblInfo1.Text.Trim = "" Then
            Me.lblInfo1.Text = "[ " & PortalCulture.GetString("A00747") & " ]"
        End If
        Me.imgPropiedad.AlternateText = "[ " & PortalCulture.GetString("A00668") & " ]"

    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        '/**************************************************
        lnkPropiedad.Pagina = "Registro/InformacionPropiedad.aspx"
        lnkPropiedad.Nota = "Edición de la información de propiedad"
        lnkInfo1.Pagina = "Registro/InformacionPropiedad.aspx"
        lnkInfo1.Nota = "Edición de la información de propiedad"
        If ModeView <> Opciones.ViewMode.gView Then
            cargatodo()
        End If
    End Sub

    Private Sub Page_ChangeContenido(ByVal sender As Object, ByVal e As ArgsConte) Handles MyBase.ChangeContenido
        cargatodo()
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Me.lblTitle.Text = PortalCulture.GetString("A00746")
    End Sub

End Class
