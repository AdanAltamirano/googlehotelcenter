Imports System.Configuration.ConfigurationManager
Imports System.Data
Imports System.Data.SqlClient
Partial Public Class InfoPropiedadCCT
    Inherits Opciones
    'Public idDelHotel As Integer
    Public Property idDelHotel() As Long
        Get
            Return ViewState("idDelHotel")
        End Get
        Set(ByVal Value As Long)
            ViewState("idDelHotel") = Value
        End Set
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lnkInfo1.Pagina = "Registro/InformacionPropiedadCCT.aspx"
        lnkInfo1.Nota = "Edición de la información de propiedad Call Center"
        ctrlLinkImagecct.Pagina = "Registro/InformacionPropiedadCCT.aspx"
        ctrlLinkImagecct.Nota = "Edición de la información de imagen de propiedad Call Center"
        If ModeView <> Opciones.ViewMode.gView Then
            cargarTodo()
        End If
    End Sub

    Public Sub cargarTodo()
        Dim strPath As String
        CargaContenidoCCT(lblInfoHotel, 0, lnkInfo1, Opciones.TipoControl.Label, Me.idDelHotel, False, False, True, False, False)
        CargaContenidoCCT(imgMapcct, 0, ctrlLinkImagecct, Opciones.TipoControl.Imagen, Me.idDelHotel, True, False, False, True, True)
        Me.lnkInfo1.NoPaso = "2."
        Me.lnkInfo1.Texto = PortalCulture.GetString("A00676")    '"Presione el Icono del lapiz para cambiar el El texto"

        Me.ctrlLinkImagecct.NoPaso = "1."
        Me.ctrlLinkImagecct.Texto = PortalCulture.GetString("A00674")

        Me.lblTitle.Text = PortalCulture.GetString("A00682")

        If Me.lblInfoHotel.Text = "" Then    '' If Me.lblInfo1.Text.Trim = "" Then
            Me.lblInfoHotel.Text = "[ " & PortalCulture.GetString("A00681") & " ]"
        End If
        strPath = Me.imgMapcct.ImageUrl
        If strPath = vbNullString Then
            strPath = GeRequestApplicationPath(String.Concat("/Images/NotAvailable", PortalCulture.GetString("A00000"), ".gif"))
        End If
        Me.imgMapcct.ImageUrl = strPath
    End Sub

    'Public Sub CargaContenidoCCT(ByRef ctrl As Object, ByVal categoria As Integer, ByRef LnkCambiar As CambiarContenido, ByVal tipo As TipoControl, ByVal img As Boolean, ByVal url As Boolean, ByVal cnt As Boolean, ByVal wt As Boolean, ByVal hg As Boolean)
    '    Dim conection As New SqlConnection(AppSettings("HotelConnectionString"))

    '    Dim spname As String = "spGetInfoToCallCenterByIdHotel"
    '    Dim command As New SqlCommand(spname, conection)
    '    'Dim idUsuario = CType(Me.Page, PaginaBase).Usuario
    '    'Dim idAsociacionHotel As Integer = CType(Me.Page, PaginaBase).GetIdAsociation

    '    With command
    '        .CommandType = CommandType.StoredProcedure
    '        .Parameters.Add(New SqlParameter("@IdHotel", Me.idDelHotel))
    '        .Parameters.Add(New SqlParameter("@Categoria", categoria))
    '    End With
    '    Dim adapter As New SqlDataAdapter(command)
    '    Dim dRes As New DataSet
    '    adapter.Fill(dRes)
    '    If dRes IsNot Nothing AndAlso dRes.Tables(0).Rows.Count > 0 Then
    '        If tipo = TipoControl.Label Then
    '            Dim lbl As Label = CType(ctrl, Label)
    '            If IdIdioma = 1 Then
    '                lbl.Text = dRes.Tables(0).Rows(0)("Descripcion_ES").ToString()
    '            Else
    '                lbl.Text = dRes.Tables(0).Rows(0)("Descripcion_EN").ToString()
    '            End If
    '        ElseIf tipo = TipoControl.Imagen Then
    '            Me.imgMapcct.ImageUrl = AppSettings("Albums_url").ToString.ToUpper.Replace("ALBUMS", "") + Replace(dRes.Tables(0).Rows(0)("UrlImagen").ToString(), "\", "/").Replace("//", "/")
    '        End If
    '    End If
    '    If tipo = TipoControl.Imagen Then
    '        LnkCambiar.ShowMode = CambiarContenido.ShowModeType.ViewImage
    '    End If
    '    If Not LnkCambiar Is Nothing Then
    '        LnkCambiar.Visible = True
    '        LnkCambiar.RHeight = hg
    '        LnkCambiar.RHRef = url
    '        LnkCambiar.Rwidth = wt
    '        LnkCambiar.RArch = img
    '        LnkCambiar.RConte = cnt
    '        LnkCambiar.elemento = ctrl
    '        LnkCambiar.IdIdioma = Me.IdIdioma
    '        LnkCambiar.IdHotelCCT = Me.idDelHotel
    '        LnkCambiar.CategoriaCCT = categoria
    '        LnkCambiar.ISCCTContent = True
    '        If Me.IsPortal Then
    '            LnkCambiar.IdEmpresa = 0
    '        Else
    '            LnkCambiar.IdEmpresa = Me.IdEmpresa
    '        End If
    '        'LnkCambiar.text = dr.Item(ComunElementos.NOMBRE_FIELD) ' NombreEle
    '        AddHandler LnkCambiar.ChangeContenido, AddressOf Me.SeleccionaContenido
    '    End If

    'End Sub

    Private Sub Page_ChangeContenido(ByVal sender As Object, ByVal e As ArgsConte) Handles MyBase.ChangeContenido
        cargarTodo()
    End Sub

    Private Sub Page_SetAll(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.SetAll
        cargarTodo()
    End Sub
End Class