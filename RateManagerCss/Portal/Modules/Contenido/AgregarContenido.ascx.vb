Partial Class AgregarContenido
    Inherits System.Web.UI.UserControl
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
    Protected WithEvents EditConte As ContenidoFCk
    Delegate Sub ChangeConte(ByVal sender As Object, ByVal e As ArgsConte)
    Public Event ChangeContenido As ChangeConte
    'Public Event ChangeContenido As EventHandler
    Public elemento As Control

#Region "propiedades"

    Public Enum ShowModeType
        ViewImage
        ViewContenido
    End Enum

    Public _AlbumEnable As Boolean = False
    Public Property IsAlbumEnable() As Boolean
        Get
            Return Me._AlbumEnable
        End Get
        Set(ByVal value As Boolean)
            Me._AlbumEnable = value
        End Set
    End Property

    Public Property IdEmpresa() As Long
        Get
            Return viewstate("IdEmpresa")
        End Get
        Set(ByVal Value As Long)
            viewstate("IdEmpresa") = Value
        End Set
    End Property


    Public Property ididioma() As Long
        Get
            Return viewstate("ididioma")
        End Get
        Set(ByVal Value As Long)
            viewstate("ididioma") = Value
        End Set
    End Property

    Public Property Rwidth() As Boolean
        Get
            Return viewstate("Rwidth")
        End Get
        Set(ByVal Value As Boolean)
            viewstate("Rwidth") = Value
        End Set
    End Property

    Public Property RHeight() As Boolean
        Get
            Return viewstate("RHeight")
        End Get
        Set(ByVal Value As Boolean)
            viewstate("RHeight") = Value
        End Set
    End Property

    Public Property RConte() As Boolean
        Get
            Return viewstate("RConte")
        End Get
        Set(ByVal Value As Boolean)
            viewstate("RConte") = Value
        End Set
    End Property

    Public Property RHRef() As Boolean
        Get
            Return viewstate("RHRef")
        End Get
        Set(ByVal Value As Boolean)
            viewstate("RHRef") = Value
        End Set
    End Property

    Public Property IsMultilanguage() As Boolean
        Get
            Dim flag = True
            If ViewState("Multilanguage") IsNot Nothing AndAlso ViewState("Multilanguage").ToString().Length > 0 Then
                flag = ViewState("Multilanguage")
            Else
                ViewState("Multilanguage") = flag
            End If

            Return flag
        End Get
        Set(ByVal Value As Boolean)
            ViewState("Multilanguage") = Value
        End Set
    End Property

    Public Property RArch() As Boolean
        Get
            Return viewstate("RArch")
        End Get
        Set(ByVal Value As Boolean)
            viewstate("RArch") = Value
        End Set
    End Property

    Public Property idElemento() As Long
        Get
            Return viewstate("idElemento")
        End Get
        Set(ByVal Value As Long)
            viewstate("idElemento") = Value
        End Set
    End Property

    Public Property text() As String
        Get
            '   Return lnkAgregar.Text
        End Get
        Set(ByVal Value As String)
            '  lnkAgregar.Text = "Agregar " & Value
        End Set
    End Property

    Public Property NoPaso() As String
        Get
            Return Me.lblPaso.Text
        End Get
        Set(ByVal Value As String)
            Me.lblPaso.Text = Value
        End Set
    End Property

    Public Property Texto() As String
        Get
            Return Me.lblTexto.Text
        End Get
        Set(ByVal Value As String)
            Me.lblTexto.Text = Value
        End Set
    End Property

    Public Property EditMode() As ShowModeType
        Get
            Return EditConte.ShowMode
        End Get
        Set(ByVal Value As ShowModeType)
            EditConte.ShowMode = Value
        End Set
    End Property

    Public Property IsImgHeader() As Boolean
        Get
            Return EditConte.IsImgHeader
        End Get
        Set(ByVal Value As Boolean)
            EditConte.IsImgHeader = Value
        End Set
    End Property

    Public _ShowMode As ShowModeType = ShowModeType.ViewContenido
    Public Property ShowMode() As ShowModeType
        Get
            If ViewState("SHOW") Is Nothing OrElse ViewState("SHOW").ToString().Length = 0 Then
                ViewState("SHOW") = ShowModeType.ViewContenido
            End If
            _ShowMode = ViewState("SHOW")
            Return _ShowMode
        End Get
        Set(ByVal Value As ShowModeType)
            _ShowMode = Value
            ViewState("SHOW") = _ShowMode
        End Set
    End Property


#End Region

    'Protected Overridable Sub OnChangeContenido(ByVal e As EventArgs)
    '    RaiseEvent ChangeContenido(Me, e)
    'End Sub
    Public Sub OnChangeContenido(ByVal idcon As Long, ByVal idele As Long)
        Dim e As New ArgsConte(idcon, idele, Me.RConte, Me.RHRef, Me.RArch, Me.Rwidth, Me.RHeight)
        RaiseEvent ChangeContenido(Me, e)
    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        'Me.EditConte.IsAlbumEnable = Me.IsAlbumEnable
    End Sub

    Private Sub lnkAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'OnChangeContenido(0, Me.idElemento)
        pEdit.Visible = True
        'lnkAgregar.Visible = False
        EditConte.RArch = Me.RArch
        EditConte.RConte = Me.RConte
        EditConte.RHRef = Me.RHRef
        EditConte.Rwidth = Me.Rwidth
        EditConte.RHeight = Me.RHeight
        EditConte.IdEmp = Me.IdEmpresa
        EditConte.IdCon = 0
        EditConte.Ididioma = Me.ididioma
        EditConte.ShowMode = Me.ShowMode
        'EditConte.IsMultilanguage = Me.IsMultilanguage

    End Sub

    Private Sub cmdGuardar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdGuardar.Click
        If Page.IsValid Then
            EditConte.Update()
            pEdit.Visible = False
            '   lnkAgregar.Visible = True
            'If Not IsNothing(elemento) Then
            'elemento.Visible = True
            'End If
            OnChangeContenido(0, Me.idElemento)
        End If
    End Sub

    Private Sub cmdCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancelar.Click
        pEdit.Visible = False
        'lnkAgregar.Visible = True
        'If Not IsNothing(elemento) Then
        'elemento.Visible = True
        'End If
    End Sub

    Private Sub cmdElminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If EditConte.IdCon > 0 And EditConte.Ididioma > 0 Then
            EditConte.DeleteContenido()
            'If Not IsNothing(elemento) Then
            'elemento.Visible = True
            'End If
            OnChangeContenido(EditConte.IdCon, Me.idElemento)
        End If
    End Sub

    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        'OnChangeContenido(0, Me.idElemento)
        pEdit.Visible = True
        'lnkAgregar.Visible = False
        EditConte.RArch = Me.RArch
        EditConte.RConte = Me.RConte
        EditConte.RHRef = Me.RHRef
        EditConte.Rwidth = Me.Rwidth
        EditConte.RHeight = Me.RHeight
        'EditConte.IsMultilanguage = Me.IsMultilanguage
        EditConte.IdEmp = Me.IdEmpresa
        EditConte.IdCon = 0
        EditConte.IdEle = Me.idElemento
        EditConte.Ididioma = Me.ididioma
        EditConte.ShowMode = Me.ShowMode
        EditConte.CargaDatos()
    End Sub

    Private Sub EditConte_HideComponent(ByVal Visible As Boolean) Handles EditConte.HideComponent
        pEdit.Visible = Visible
        If Not elemento Is Nothing Then elemento.Visible = True
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Me.lblTitle.Text = PortalCulture.GetString("A00665")
        ImageButton1.ToolTip = PortalCulture.GetString("A00665")

        Me.cmdGuardar.Text = PortalCulture.GetString("A00153")
        Me.cmdCancelar.Text = PortalCulture.GetString("A00670")
    End Sub
End Class

