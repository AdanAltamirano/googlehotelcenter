Imports System.Data.SqlClient
Imports System.Data
Imports Contenido.presentacion
Imports Contenido.Comun
Imports System.Configuration.ConfigurationManager
'Imports bkHoteles
'Imports bkAutoRentas
Imports System.Text.RegularExpressions

Public Class ArgsConte
    Inherits EventArgs

    Public Sub New(ByVal idcon As Long, ByVal IdEle As Long, ByVal conte As Boolean, ByVal url As Boolean, ByVal arch As Boolean, Optional ByVal w As Boolean = False, Optional ByVal h As Boolean = False)
        IdContenido = idcon
        IdElemento = IdEle
        contenido = conte
        href = url
        archivo = arch
        width = w
        height = h
    End Sub

    Public IdContenido As Long
    Public IdElemento As Long
    Public contenido As Boolean
    Public href As Boolean
    Public archivo As Boolean
    Public width As Boolean
    Public height As Boolean
End Class

Public Class Opciones
    Inherits UserControlBase

#Region "  Delegados Change  Add Ele Contenido  "

    Delegate Sub ChangeConte(ByVal sender As Object, ByVal e As ArgsConte)
    Delegate Sub AddConte(ByVal sender As Object, ByVal e As ArgsConte)
    Delegate Sub EleConte(ByVal sender As Object, ByVal e As ArgsConte)

#End Region

#Region "  Eventos Change, Add, Delete, SetAll "

    Public Event ChangeContenido As ChangeConte
    Public Event AddContenido As AddConte
    Public Event DeleteContenido As EleConte
    Public Event SetAll As EventHandler

#End Region

#Region "  Eventos OnSelRoom,ChangeRoomsPerson,OnSelectedEmpresa,OnNewSearch  "
    Public Event onRedirectInfo(ByVal path As String)
    'Public Event onSelroomGWS(ByVal Room As String, ByVal Rate As String, ByVal tarifa As Double, ByVal moneda As String, ByVal nameroom As String, ByVal MonedaReal As String, ByVal THOR As String, ByVal TaxInc As Boolean)
    'Public Event onSelroom As EventHandler
    'Public Event OnChangeRoomsPersons As EventHandler
    'Public Event OnSelectedEmpresa As EventHandler
    'Public Event onNewSearch As EventHandler
    'Public Event onChangeTypeSearch(ByVal ts As enumtypesearch)
    'Public Event LoadedKeyWords(ByVal gResponse As bkHotelesGWS.HotelDescription_Response)
    'Public Event LoadedImages(ByVal aList As ArrayList)
#End Region

    ''table de los registros seleccionados
    Dim dt As DataTable '= New DataTable
    Dim PElemento As Long = 0
    Dim dsCA As DataSet '// DataSet para los
    ' Public reservation As reservation  'de hotel
    'Public reservationCar As CarReservation   'de autos

#Region "  Enumerativos de ViewMode, TipoControl"

    'trae el contenido de un modulo y empresa e idioma
    '''como se va mostrar el modulo(edicion,preview,produccion)
    Public Enum ViewMode
        Preview = 0
        Production = 1
        Edit = 2
        gView = 3
    End Enum
    ''tipo de elemento referenciado al control
    Public Enum TipoControl
        TextBox = 0
        Label = 1
        Link = 2
        Imagen = 3
        Flash = 4
    End Enum

#End Region

#Region "  Propiedades IdIdioma,IsPortal,HasContent,IdModulo,IdRubro,IdPagina,IdPaginaModulo,ModeView,PathApli,IsRender"

    '''idioma que se va a buscar
    Public Property IdIdioma() As Long
        Get
            Return viewstate("IdIdioma")
        End Get
        Set(ByVal Value As Long)
            viewstate("IdIdioma") = Value
            CargoTodo()
        End Set
    End Property
    '''idempresa para buscar
    Public Property IsPortal() As Boolean
        Get
            Return viewstate("IsPortal")
        End Get
        Set(ByVal Value As Boolean)
            viewstate("IsPortal") = Value
        End Set
    End Property
    Public ReadOnly Property HasContent() As Boolean
        Get
            Try
                If dt.Rows.Count > 0 Then
                    Dim r As DataRow
                    For Each r In dt.Rows
                        If Not r.IsNull(ComunContenido.DESCRIPCION_FIELD) Then
                            'encontro al menos una descripcion
                            Return True
                        End If
                    Next
                End If
                Return False
            Catch e As Exception
                'no tiene elementos dinamicos
                Return True
            End Try
        End Get
    End Property

    Public Property IdEmpresa() As Long
        Get
            Return viewstate("IdEmpresa")
        End Get
        Set(ByVal Value As Long)
            viewstate("IdEmpresa") = Value
            CargoTodo()
        End Set
    End Property

    Public Property IdEmpresaGalileo() As String
        Get
            If IsNothing(viewstate("IdEmpresaGal")) Then viewstate("IdEmpresaGal") = ""
            Return viewstate("IdEmpresaGal")
        End Get
        Set(ByVal Value As String)
            viewstate("IdEmpresaGal") = Value
            CargoTodo()
        End Set
    End Property

    '''IdModulo para buscar los elementos del modulo
    Public Property IdModulo() As Long
        Get
            Return viewstate("IdModulo")
        End Get
        Set(ByVal Value As Long)
            viewstate("IdModulo") = Value
            CargoTodo()
        End Set
    End Property
    Public ReadOnly Property IdRubro() As Long
        Get
            Return AppSettings("idRubro")
        End Get
    End Property
    Public Property IdPagina() As Long
        Get
            Return viewstate("IdPagina")
        End Get
        Set(ByVal Value As Long)
            viewstate("IdPagina") = Value
        End Set
    End Property
    Public Property IdPaginaModulo() As Long
        Get
            Return viewstate("IdPaginaModulo")
        End Get
        Set(ByVal Value As Long)
            viewstate("IdPaginaModulo") = Value
        End Set
    End Property
    ''idelemento seleccionado'''''
    Public Property IdElementoSelected() As Long
        Get
            Return viewstate("IdElementoSelected")
        End Get
        Set(ByVal Value As Long)
            viewstate("IdElementoSelected") = Value
        End Set
    End Property
    '''idcontnenido seleccionado
    Public Property IdContenidoSelected() As Long
        Get
            Return viewstate("IdContenidoSelected")
        End Get
        Set(ByVal Value As Long)
            viewstate("IdContenidoSelected") = Value
        End Set
    End Property
    ''modo de preview seleccionado

    Public Property ModeView() As ViewMode
        Get
            Return viewstate("ModeView")
        End Get
        Set(ByVal Value As ViewMode)
            viewstate("ModeView") = Value
        End Set
    End Property
    ''path de la aplicacion
    Public Property PathApli() As String
        Get
            Return viewstate("PathApli")
        End Get
        Set(ByVal Value As String)
            viewstate("PathApli") = Value
        End Set
    End Property
    Public Property IsRender() As Boolean
        Get
            Return viewstate("IsRender")
        End Get
        Set(ByVal Value As Boolean)
            viewstate("IsRender") = Value
        End Set
    End Property

    Public Property IsImgHeader() As Boolean
        Get
            Return viewstate("IsImgHeader")
        End Get
        Set(ByVal Value As Boolean)
            viewstate("IsImgHeader") = Value
        End Set
    End Property

#End Region

#Region "  Carga de Datos TraerDatos,Contenido  "

    Private Sub CargoTodo()
        If Me.IdEmpresa > 0 And Me.IdIdioma > 0 And Me.IdModulo > 0 _
         And Me.IdRubro > 0 Then
            OnChangeAll(EventArgs.Empty)
        End If
    End Sub

    Public Sub TraeDatos(ByVal IdEmp As Long)
        Dim def As String
        def = AppSettings("LanguageDefault").ToString
        If Me.IdEmpresa >= 0 Then
            With New PresentacionOpciones
                dt = .GetContenidoPagMod(Me.IdIdioma, Me.IdModulo, IdEmp, def)
            End With
        End If
    End Sub

    Public Sub GetImagesRoomsHotel(ByVal IdEmpresa As Long, ByVal idTipoHabitacionHotel As Integer)
        Dim def As String
        def = AppSettings("LanguageDefault").ToString

        If Me.IdEmpresa >= 0 Then
            With New PresentacionOpciones
                'dt es global
                dt = .GetContenidoTipoHabitacionHotel(Me.IdIdioma, CType(AppSettings("idCtrlImagesRooms"), Integer), IdEmpresa, def, idTipoHabitacionHotel)
            End With
        End If
    End Sub

    Public Function GetContenidoImagesRoomsHotel(ByVal NombreEle As String, ByVal idTipoHabitacionHotel As Integer) As DataRow()
        Dim dr() As DataRow = Nothing

        GetImagesRoomsHotel(Me.IdEmpresa, idTipoHabitacionHotel)
        If dt.Rows.Count > 0 Then
            dr = dt.Select(" Nombre = '" & NombreEle & "'")
        End If
        Return dr
    End Function

    Public Sub GetImagesHotelitem(ByVal IdEmpresa As Long, ByVal idHotelItem As Integer)
        Dim def As String
        def = AppSettings("LanguageDefault").ToString

        If Me.IdEmpresa >= 0 Then
            With New PresentacionOpciones
                'dt es global
                dt = .GetContenidoHotelItem(Me.IdIdioma, CType(AppSettings("idCtrlImagesHotelItem"), Integer), IdEmpresa, def, idHotelItem) '1094
            End With
        End If
    End Sub


    Public Function GetContenidoImagesHotelItem(ByVal NombreEle As String, ByVal idHotelItem As Integer) As DataRow()
        Dim dr() As DataRow = Nothing

        GetImagesHotelItem(Me.IdEmpresa, idHotelItem)
        If dt.Rows.Count > 0 Then
            dr = dt.Select(" Nombre = '" & NombreEle & "'")
        End If
        Return dr
    End Function


    ''funcion para obtener el elemento de una coleccion de datarows
    Public Function Contenido(ByVal NombreEle As String) As DataRow()
        Dim dr() As DataRow = Nothing
        If dt Is Nothing Then
            If Me.IsPortal Then
                TraeDatos(0)
            Else
                TraeDatos(Me.IdEmpresa)
            End If
        End If
        If dt.Rows.Count > 0 Then
            dr = dt.Select(" Nombre = '" & NombreEle & "'")

        End If
        Return dr
    End Function
    Public Function GetContenidoUpdated(ByVal NombreEle As String) As DataRow()
        Dim dr() As DataRow = Nothing

        If Me.IsPortal Then
            TraeDatos(0)
        Else
            TraeDatos(Me.IdEmpresa)
        End If

        If dt.Rows.Count > 0 Then
            dr = dt.Select(" Nombre = '" & NombreEle & "'")
        End If
        Return dr
    End Function
    'Este mapea el contro con el contenido a traves del resultado en un datarow
    Public Sub CargaContenido(ByVal dr As DataRow, ByRef ctrl As Object, ByVal tipo As TipoControl, ByRef LnkCambiar As CambiarContenido, ByRef lnkeliminar As EliminarContenido, ByRef lnkAgregar As AgregarContenido, ByVal img As Boolean, ByVal url As Boolean, ByVal cnt As Boolean, ByVal wt As Boolean, ByVal hg As Boolean)
        Dim con As Long
        Dim ele As Long
        If Not dr Is Nothing Then
            'If PElemento = dr.Item(ComunElementos.PKIDElemento_FIELD) And dr.IsNull(ComunContenido.PKIDCONTENIDO_FIELD) Then Exit Sub
            'PElemento = dr.Item(ComunElementos.PKIDElemento_FIELD)
            Select Case tipo
                Case TipoControl.TextBox
                    Dim txt As TextBox = CType(ctrl, TextBox)
                    If cnt AndAlso Not dr.IsNull(ComunContenido.DESCRIPCION_FIELD) Then
                        txt.Text = dr.Item(ComunContenido.DESCRIPCION_FIELD)
                    End If
                    If cnt AndAlso Not dr.IsNull(ComunContenido.WIDTH_FIELD) Then
                        txt.Width = System.Web.UI.WebControls.Unit.Parse(dr.Item(ComunContenido.WIDTH_FIELD))
                    End If
                    If cnt AndAlso Not dr.IsNull(ComunContenido.WIDTH_FIELD) Then
                        txt.Height = System.Web.UI.WebControls.Unit.Parse(dr.Item(ComunContenido.HEIGHT_FIELD))
                    End If
                Case TipoControl.Label
                    Dim lbl As Label = CType(ctrl, Label)
                    If cnt AndAlso Not dr.IsNull(ComunContenido.DESCRIPCION_FIELD) Then
                        lbl.Text = CType(dr.Item(ComunContenido.DESCRIPCION_FIELD), String).Replace(vbCrLf.ToString, "<BR>")
                    End If
                    If wt AndAlso Not dr.IsNull(ComunContenido.WIDTH_FIELD) Then
                        lbl.Width = System.Web.UI.WebControls.Unit.Parse(dr.Item(ComunContenido.WIDTH_FIELD))
                    End If
                    If hg AndAlso Not dr.IsNull(ComunContenido.WIDTH_FIELD) Then
                        lbl.Height = System.Web.UI.WebControls.Unit.Parse(dr.Item(ComunContenido.HEIGHT_FIELD))
                    End If
                Case TipoControl.Link
                    Dim lnk As HyperLink = CType(ctrl, HyperLink)
                    If img AndAlso Not dr.IsNull(ComunContenido.ARCHIVO_FIELD) Then
                        'Añadimos un control image al link image para poder
                        'Establecer el tamaño de la imagen
                        Dim imageInLink As New WebControls.Image
                        imageInLink.ImageUrl = PathApli & "/" & Replace(dr.Item(ComunContenido.ARCHIVO_FIELD).ToString, "\", "/")

                        If wt AndAlso Not dr.IsNull(ComunContenido.WIDTH_FIELD) Then
                            Dim ctrlWith As Unit = System.Web.UI.WebControls.Unit.Parse(dr.Item(ComunContenido.WIDTH_FIELD))
                            If ctrlWith.Value > 0 Then
                                imageInLink.Width = ctrlWith
                            End If
                        End If
                        If hg AndAlso Not dr.IsNull(ComunContenido.WIDTH_FIELD) Then
                            Dim ctrlHeigth As Unit = System.Web.UI.WebControls.Unit.Parse(dr.Item(ComunContenido.HEIGHT_FIELD))
                            If ctrlHeigth.Value > 0 Then
                                imageInLink.Height = ctrlHeigth          ' System.Web.UI.WebControls.Unit.Parse(dr.Item(ComunContenido.HEIGHT_FIELD))
                            End If

                        End If
                        lnk.Controls.Add(imageInLink)
                    End If
                    If url AndAlso Not dr.IsNull(ComunContenido.URL_FIELD) Then
                        lnk.NavigateUrl = dr.Item(ComunContenido.URL_FIELD)
                    End If
                    If cnt AndAlso Not dr.IsNull(ComunContenido.DESCRIPCION_FIELD) Then
                        lnk.Text = dr.Item(ComunContenido.DESCRIPCION_FIELD)
                    End If
                    If wt AndAlso Not dr.IsNull(ComunContenido.WIDTH_FIELD) Then
                        lnk.Width = System.Web.UI.WebControls.Unit.Parse(dr.Item(ComunContenido.WIDTH_FIELD))
                    End If
                    If hg AndAlso Not dr.IsNull(ComunContenido.WIDTH_FIELD) Then
                        lnk.Height = System.Web.UI.WebControls.Unit.Parse(dr.Item(ComunContenido.HEIGHT_FIELD))
                    End If
                Case TipoControl.Imagen
                    If lnkAgregar IsNot Nothing Then lnkAgregar.ShowMode = AgregarContenido.ShowModeType.ViewImage
                    If LnkCambiar IsNot Nothing Then LnkCambiar.ShowMode = AgregarContenido.ShowModeType.ViewImage
                    Dim ima As WebControls.Image
                    ima = CType(ctrl, WebControls.Image)
                    If img AndAlso Not dr.IsNull(ComunContenido.ARCHIVO_FIELD) Then
                        If dr.Item(ComunContenido.ARCHIVO_FIELD).ToString.Trim <> "" Then
                            'PathApli = AppSettings("Albums_url").ToString.ToUpper.Replace("ALBUMS", "") & Replace(dr.Item(ComunContenido.ARCHIVO_FIELD).ToString, "\", "/") & AppSettings("Img_Prefix_Module") & "?" & Now.ToString
                            PathApli = AppSettings("Albums_url").ToString.ToUpper.Replace("ALBUMS", "") & Replace(dr.Item(ComunContenido.ARCHIVO_FIELD).ToString, "\", "/").Replace("//", "/")
                            PathApli = PathApli & IIf(IsImgHeader, "", AppSettings("Img_Prefix_Module")) & "?" & Now.ToString
                            ima.ImageUrl = PathApli '.Replace("//", "/")
                        Else
                            ima.ImageUrl = GeRequestApplicationPath(PortalCulture.GetString("00683"))
                        End If
                    End If
                    If ima.ImageUrl.Trim = vbNullString Then
                        ima.ImageUrl = GeRequestApplicationPath(PortalCulture.GetString("00683"))
                    End If

                    If wt AndAlso Not dr.IsNull(ComunContenido.WIDTH_FIELD) Then
                        Dim ctrlWith As Unit = System.Web.UI.WebControls.Unit.Parse(dr.Item(ComunContenido.WIDTH_FIELD))
                        If ctrlWith.Value > 0 Then
                            ima.Width = ctrlWith       'System.Web.UI.WebControls.Unit.Parse(dr.Item(ComunContenido.WIDTH_FIELD))
                        End If
                    End If
                    If hg AndAlso Not dr.IsNull(ComunContenido.WIDTH_FIELD) Then
                        Dim ctrlHeigth As Unit = System.Web.UI.WebControls.Unit.Parse(dr.Item(ComunContenido.HEIGHT_FIELD))
                        If ctrlHeigth.Value > 0 Then
                            ima.Height = ctrlHeigth       ' System.Web.UI.WebControls.Unit.Parse(dr.Item(ComunContenido.HEIGHT_FIELD))
                        End If
                    End If
                Case TipoControl.Flash
                    Dim lc As Literal
                    Dim cad As String = ""
                    Dim W As String = ""
                    Dim H As String = ""
                    lc = CType(ctrl, Literal)
                    If Not dr.IsNull(ComunContenido.ARCHIVO_FIELD) Then
                        cad = PathApli & "/" & Replace(dr.Item(ComunContenido.ARCHIVO_FIELD).ToString, "\", "/")
                    End If
                    If wt AndAlso Not dr.IsNull(ComunContenido.WIDTH_FIELD) Then
                        Dim ctrlWith As Unit = System.Web.UI.WebControls.Unit.Parse(dr.Item(ComunContenido.WIDTH_FIELD))
                        If ctrlWith.Value > 0 Then
                            W = ctrlWith.Value        'System.Web.UI.WebControls.Unit.Parse(dr.Item(ComunContenido.WIDTH_FIELD))
                        End If
                    End If
                    If hg AndAlso Not dr.IsNull(ComunContenido.WIDTH_FIELD) Then
                        Dim ctrlHeigth As Unit = System.Web.UI.WebControls.Unit.Parse(dr.Item(ComunContenido.HEIGHT_FIELD))
                        If ctrlHeigth.Value > 0 Then
                            H = ctrlHeigth.Value        ' System.Web.UI.WebControls.Unit.Parse(dr.Item(ComunContenido.HEIGHT_FIELD))
                        End If
                    End If
                    lc.Text = Cargaflash(cad, W, H)
            End Select
            If Not dr.IsNull(ComunContenido.PKIDCONTENIDO_FIELD) Then
                con = dr.Item(ComunContenido.PKIDCONTENIDO_FIELD)
            End If
            If Not dr.IsNull(ComunContenido.IDELEMENTO_FIELD) Then
                ele = dr.Item(ComunContenido.IDELEMENTO_FIELD)
            End If
            If Not LnkCambiar Is Nothing Then
                LnkCambiar.Visible = True
                LnkCambiar.idElemento = ele
                LnkCambiar.idContenido = con
                LnkCambiar.RHeight = hg
                LnkCambiar.RHRef = url
                LnkCambiar.Rwidth = wt
                LnkCambiar.RArch = img
                LnkCambiar.RConte = cnt
                LnkCambiar.elemento = ctrl
                LnkCambiar.IdIdioma = Me.IdIdioma
                If Me.IsPortal Then
                    LnkCambiar.IdEmpresa = 0
                Else
                    LnkCambiar.IdEmpresa = Me.IdEmpresa
                End If
                'LnkCambiar.text = dr.Item(ComunElementos.NOMBRE_FIELD) ' NombreEle
                AddHandler LnkCambiar.ChangeContenido, AddressOf Me.SeleccionaContenido
            End If
            If Not lnkeliminar Is Nothing Then
                'lnkeliminar.Visible = True
                'lnkeliminar.idElemento = ele
                'lnkeliminar.idContenido = con
                'lnkeliminar.text = 'dr.Item(ComunElementos.NOMBRE_FIELD) ' NombreEle
                'AddHandler lnkeliminar.ChangeContenido, AddressOf Me.EliminaContenido
            End If
            If ModeView <> ViewMode.Edit Then
                If Not (LnkCambiar Is Nothing) Then
                    LnkCambiar.Visible = False
                End If
                If Not lnkeliminar Is Nothing Then
                    lnkeliminar.Visible = False
                End If
            End If
            If Not lnkAgregar Is Nothing Then
                lnkAgregar.Visible = True
                If ModeView <> ViewMode.Edit Then
                    lnkAgregar.Visible = False
                End If
                lnkAgregar.idElemento = ele
                'lnkAgregar.text = dr.Item(ComunElementos.NOMBRE_FIELD) 'NombreEle
                lnkAgregar.RHeight = hg
                lnkAgregar.RHRef = url
                lnkAgregar.Rwidth = wt
                lnkAgregar.RArch = img
                lnkAgregar.RConte = cnt
                lnkAgregar.elemento = ctrl
                lnkAgregar.ididioma = Me.IdIdioma
                If Me.IsPortal Then
                    lnkAgregar.IdEmpresa = 0
                Else
                    lnkAgregar.IdEmpresa = Me.IdEmpresa
                End If
                AddHandler lnkAgregar.ChangeContenido, AddressOf Me.SeleccionaContenido
            End If
        End If
    End Sub
    Public Sub CargaContenido(ByVal NombreEle As String, ByRef ctrl As Object, ByVal tipo As TipoControl, ByRef LnkCambiar As CambiarContenido, ByVal lnkEliminar As EliminarContenido, ByRef lnkAgregar As AgregarContenido, ByVal img As Boolean, ByVal url As Boolean, ByVal cnt As Boolean, ByVal wt As Boolean, ByVal hg As Boolean)
        Dim dr() As DataRow
        dr = Contenido(NombreEle)
        'E_MODIFICADO:
        If Not dr Is Nothing AndAlso dr.Length > 0 Then
            CargaContenido(dr(0), ctrl, tipo, LnkCambiar, lnkEliminar, lnkAgregar, img, url, cnt, wt, hg)
        End If

    End Sub
    Public Sub CargaContenido(ByVal NombreEle As String, ByVal tipo As TipoControl, _
        ByRef panelSecc As Panel, ByVal img As Boolean, ByVal url As Boolean, ByVal cnt As Boolean, ByVal wt As Boolean, ByVal hg As Boolean)
        Dim con As Long
        Dim ele As Long
        Dim dr() As DataRow
        Dim ren As DataRow
        Dim lnkCambiar As CambiarContenido
        Dim lnkAgregar As AgregarContenido
        Dim lnkEliminar As EliminarContenido
        Dim ctrl As Control
        dr = Contenido(NombreEle)
        If Not dr Is Nothing AndAlso dr.Length > 0 Then
            For Each ren In dr
                ele = 0
                con = 0
                If Not ren.IsNull(ComunContenido.PKIDCONTENIDO_FIELD) Then
                    con = ren.Item(ComunContenido.PKIDCONTENIDO_FIELD)
                End If
                If Not ren.IsNull(ComunContenido.IDELEMENTO_FIELD) Then
                    ele = ren.Item(ComunContenido.IDELEMENTO_FIELD)
                End If
                If con > 0 Then
                    Select Case tipo
                        Case TipoControl.TextBox
                            Dim txt As TextBox = New TextBox
                            txt.ID = "Con" & con
                            If cnt AndAlso Not ren.IsNull(ComunContenido.DESCRIPCION_FIELD) Then
                                txt.Text = ren.Item(ComunContenido.DESCRIPCION_FIELD)
                            End If
                            If cnt AndAlso Not ren.IsNull(ComunContenido.WIDTH_FIELD) Then
                                txt.Width = System.Web.UI.WebControls.Unit.Parse(ren.Item(ComunContenido.WIDTH_FIELD))
                            End If
                            If cnt AndAlso Not ren.IsNull(ComunContenido.WIDTH_FIELD) Then
                                txt.Height = System.Web.UI.WebControls.Unit.Parse(ren.Item(ComunContenido.HEIGHT_FIELD))
                            End If
                            panelSecc.Controls.Add(txt)
                        Case TipoControl.Label
                            Dim lbl As Label = New Label
                            lbl.ID = "Con" & con
                            If cnt AndAlso Not ren.IsNull(ComunContenido.DESCRIPCION_FIELD) Then
                                lbl.Text = ren.Item(ComunContenido.DESCRIPCION_FIELD)
                            End If
                            If wt AndAlso Not ren.IsNull(ComunContenido.WIDTH_FIELD) Then
                                lbl.Width = System.Web.UI.WebControls.Unit.Parse(ren.Item(ComunContenido.WIDTH_FIELD))
                            End If
                            If hg AndAlso Not ren.IsNull(ComunContenido.WIDTH_FIELD) Then
                                lbl.Height = System.Web.UI.WebControls.Unit.Parse(ren.Item(ComunContenido.HEIGHT_FIELD))
                            End If
                            panelSecc.Controls.Add(lbl)
                        Case TipoControl.Link
                            Dim lnk As HyperLink = New HyperLink
                            lnk.ID = "Con" & con
                            If img AndAlso Not ren.IsNull(ComunContenido.ARCHIVO_FIELD) Then
                                lnk.ImageUrl = PathApli & "/" & Replace(ren.Item(ComunContenido.ARCHIVO_FIELD).ToString, "\", "/")
                            End If
                            If url AndAlso Not ren.IsNull(ComunContenido.URL_FIELD) Then
                                lnk.NavigateUrl = ren.Item(ComunContenido.URL_FIELD)
                            End If
                            If cnt AndAlso Not ren.IsNull(ComunContenido.DESCRIPCION_FIELD) Then
                                lnk.Text = ren.Item(ComunContenido.DESCRIPCION_FIELD)
                            End If
                            If wt AndAlso Not ren.IsNull(ComunContenido.WIDTH_FIELD) Then
                                lnk.Width = System.Web.UI.WebControls.Unit.Parse(ren.Item(ComunContenido.WIDTH_FIELD))
                            End If
                            If hg AndAlso Not ren.IsNull(ComunContenido.WIDTH_FIELD) Then
                                lnk.Height = System.Web.UI.WebControls.Unit.Parse(ren.Item(ComunContenido.HEIGHT_FIELD))
                            End If
                            panelSecc.Controls.Add(lnk)
                        Case TipoControl.Imagen
                            Dim ima As WebControls.Image
                            ima = New WebControls.Image
                            ima.ID = "Con" & con
                            If img AndAlso Not ren.IsNull(ComunContenido.ARCHIVO_FIELD) Then
                                ima.ImageUrl = AppSettings("Albums") & "/" & Replace(ren.Item(ComunContenido.ARCHIVO_FIELD).ToString, "\", "/")
                                If IO.File.Exists(Server.MapPath(ima.ImageUrl & AppSettings("Img_Prefix_Module"))) Then
                                    ima.ImageUrl &= AppSettings("Img_Prefix_Module")
                                Else
                                    ima.ImageUrl = GeRequestApplicationPath(PortalCulture.GetString("00683"))
                                End If
                            End If
                            If ima.ImageUrl.Trim = vbNullString Then
                                ima.ImageUrl = GeRequestApplicationPath(PortalCulture.GetString("00683"))
                            End If

                            If wt AndAlso Not ren.IsNull(ComunContenido.WIDTH_FIELD) Then
                                ima.Width = System.Web.UI.WebControls.Unit.Parse(ren.Item(ComunContenido.WIDTH_FIELD))
                            End If
                            If hg AndAlso Not ren.IsNull(ComunContenido.WIDTH_FIELD) Then
                                ima.Height = System.Web.UI.WebControls.Unit.Parse(ren.Item(ComunContenido.HEIGHT_FIELD))
                            End If
                            panelSecc.Controls.Add(ima)
                        Case TipoControl.Flash
                            Dim cad As String = ""
                            Dim W As String = ""
                            Dim H As String = ""
                            Dim lc As Literal = New Literal
                            If Not ren.IsNull(ComunContenido.ARCHIVO_FIELD) Then
                                cad = PathApli & "/" & Replace(ren.Item(ComunContenido.ARCHIVO_FIELD).ToString, "\", "/")
                            End If
                            If wt AndAlso Not ren.IsNull(ComunContenido.WIDTH_FIELD) Then
                                Dim ctrlWith As Unit = System.Web.UI.WebControls.Unit.Parse(ren.Item(ComunContenido.WIDTH_FIELD))
                                If ctrlWith.Value > 0 Then
                                    W = ctrlWith.Value           'System.Web.UI.WebControls.Unit.Parse(dr.Item(ComunContenido.WIDTH_FIELD))
                                End If
                            End If
                            If hg AndAlso Not ren.IsNull(ComunContenido.WIDTH_FIELD) Then
                                Dim ctrlHeigth As Unit = System.Web.UI.WebControls.Unit.Parse(ren.Item(ComunContenido.HEIGHT_FIELD))
                                If ctrlHeigth.Value > 0 Then
                                    H = ctrlHeigth.Value           ' System.Web.UI.WebControls.Unit.Parse(dr.Item(ComunContenido.HEIGHT_FIELD))
                                End If
                            End If
                            lc.Text = Cargaflash(cad, W, H)
                            panelSecc.Controls.Add(lc)
                    End Select
                    If ModeView = ViewMode.Edit Then
                        lnkCambiar = New CambiarContenido
                        lnkCambiar = LoadControl(GeRequestApplicationPath("/Portal/Modules/Contenido/CambiarContenido.ascx"))
                        lnkCambiar.Visible = True
                        lnkCambiar.idElemento = ele
                        lnkCambiar.idContenido = con
                        'lnkCambiar.text = NombreEle
                        lnkCambiar.ID = "Cam" & con
                        lnkCambiar.IdIdioma = Me.IdIdioma
                        If Me.IsPortal Then
                            lnkCambiar.IdEmpresa = 0
                        Else
                            lnkCambiar.IdEmpresa = Me.IdEmpresa
                        End If
                        lnkCambiar.RHeight = hg
                        lnkCambiar.RHRef = url
                        lnkCambiar.Rwidth = wt
                        lnkCambiar.RArch = img
                        lnkCambiar.RConte = cnt
                        lnkCambiar.elemento = ctrl
                        AddHandler lnkCambiar.ChangeContenido, AddressOf Me.SeleccionaContenido
                        panelSecc.Controls.Add(lnkCambiar)
                        'lnkEliminar = New EliminarContenido
                        'lnkEliminar = LoadControl( "/EliminarContenido.ascx")
                        'lnkEliminar.Visible = True
                        'lnkEliminar.idElemento = ele
                        'lnkEliminar.idContenido = con
                        'lnkEliminar.text = NombreEle
                        'lnkEliminar.ID = "Eli" & con
                        'AddHandler lnkEliminar.ChangeContenido, AddressOf Me.EliminaContenido
                        'AddHandler lnkCambiar.ChangeContenido, Evento
                        panelSecc.Controls.Add(lnkEliminar)
                    End If
                End If
                panelSecc.Controls.Add(New LiteralControl("<BR>"))
            Next
            If ModeView = ViewMode.Edit And ele > 0 Then
                lnkAgregar = New AgregarContenido
                lnkAgregar.RHeight = hg
                lnkAgregar.RHRef = url
                lnkAgregar.Rwidth = wt
                lnkAgregar.RArch = img
                lnkAgregar.RConte = cnt
                lnkAgregar.ididioma = Me.IdIdioma
                If Me.IsPortal Then
                    lnkAgregar.IdEmpresa = 0
                Else
                    lnkAgregar.IdEmpresa = Me.IdEmpresa
                End If
                lnkAgregar.elemento = panelSecc

                lnkAgregar = LoadControl(GeRequestApplicationPath("/Portal/modules/Contenido/AgregarContenido.ascx"))
                lnkAgregar.Visible = True
                lnkAgregar.idElemento = ele
                lnkAgregar.text = NombreEle
                AddHandler lnkAgregar.ChangeContenido, AddressOf Me.SeleccionaContenido
                panelSecc.Controls.Add(New LiteralControl("<BR>"))
                panelSecc.Controls.Add(lnkAgregar)
            End If
        End If
    End Sub

    Public Function Cargaflash(ByVal src As String, ByVal wt As String, ByVal hg As String) As String
        Dim cad As String = ""
        cad &= "<object classid='clsid:D27CDB6E-AE6D-11cf-96B8-444553540000' codebase='http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,29,0'"
        If Val(wt) > 0 Then
            cad &= " width='" & wt & "' "
        End If
        If Val(wt) > 0 Then
            cad &= " height='" & hg & "' "
        End If
        cad &= "><param name='movie' value='" & src & "'>"
        cad &= "<param name='quality' value='high'>"
        cad &= "<embed src='" & src & "' quality='high' pluginspage='http://www.macromedia.com/go/getflashplayer' type='application/x-shockwave-flash' "
        If Val(wt) > 0 Then
            cad &= " width='" & wt & "' "
        End If
        If Val(wt) > 0 Then
            cad &= " height='" & hg & "' "
        End If
        cad &= "></embed></object>"
        Return cad
    End Function

#End Region

#Region "Complete Avaliability"
    '''''''' propiedad para cargar la disponibilidad completa '''''''''''''''''''''''
    Public Property DsCompAvail() As DataSet
        Get
            Return dsCA
        End Get
        Set(ByVal Value As DataSet)
            dsCA = Value
        End Set
    End Property

#End Region

    ''funcion para seleccionar un elemento y contenido de un modulo
    Public Sub SeleccionaContenido(ByVal sender As Object, ByVal e As ArgsConte)
        Me.IdContenidoSelected = e.IdContenido
        Me.IdElementoSelected = e.IdElemento
        'Me.IdIdioma = Me.IdIdioma
        If Not dt Is Nothing Then
            dt.Dispose()
            dt = Nothing
        End If
        RaiseEvent ChangeContenido(Me, e)
    End Sub

    Public Sub EliminaContenido(ByVal sender As Object, ByVal e As ArgsConte)
        Me.IdContenidoSelected = e.IdContenido
        Me.IdElementoSelected = e.IdElemento
        RaiseEvent DeleteContenido(Me, e)
    End Sub

    Public Sub SeleccionaElemento(ByVal sender As Object, ByVal e As ArgsConte)
        Me.IdElementoSelected = e.IdElemento
        RaiseEvent AddContenido(Me, e)
    End Sub

    'Public Sub changeTypeSearch(ByVal ts As enumtypesearch)
    '    RaiseEvent onChangeTypeSearch(ts)
    'End Sub

    Public Sub ChangePage(ByVal path As String)
        RaiseEvent onRedirectInfo(path)
    End Sub

    'Public Sub OnLoadedKeyWords(ByVal gResponse As bkHotelesGWS.HotelDescription_Response)
    '    RaiseEvent LoadedKeyWords(gResponse)
    'End Sub

    'Public Sub OnLoadedImages(ByVal aList As ArrayList)
    '    RaiseEvent LoadedImages(aList)
    'End Sub

    '//funcion para agregar el contenido de un elemento dependiendo de la empresa
    ''''''''''''''''''''''''''''''''elemento''''''''''''''''''control''''''''''''''''''''tipo de control''''''''''ascx cambiar contenido'''''''''''ascx agregar Contenido'''''quiere imagen''''''''''''quiere url'''''''''''contenido''''''''''''width'''''''''''''''''height''''''

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If Me.HasContent Or Me.ModeView = ViewMode.Edit Or Me.IsRender Then
            MyBase.Render(writer)
        End If
    End Sub

#Region "  Funciones para Lanzar los Eventos  Onsel...,OnSel..,OnNew...,OnChang...,OnChange... "

    ''''''''''funcion agregada para la seleccion de GWS rates types room'''''''''''
    'Public Sub onSelRoomControlGWS(ByVal Room As String, ByVal Rate As String, ByVal tarifa As Double, ByVal moneda As String, ByVal nameroom As String, ByVal MonedaReal As String, ByVal THOR As String, ByVal TaxInc As Boolean)
    '    RaiseEvent onSelroomGWS(Room, Rate, tarifa, moneda, nameroom, MonedaReal, THOR, TaxInc)
    'End Sub
    'Public Sub onSelroomControl(ByVal sender As Object)
    '    RaiseEvent onSelroom(sender, System.EventArgs.Empty)
    'End Sub
    'Public Sub onSelEmp(ByVal sender As Object)
    '    RaiseEvent OnSelectedEmpresa(sender, EventArgs.Empty)
    'End Sub
    'Public Sub onNewChSearch(ByVal sender As Object)
    '    RaiseEvent onNewSearch(sender, EventArgs.Empty)
    'End Sub
    Protected Overridable Sub OnChangeAll(ByVal e As EventArgs)
        RaiseEvent SetAll(Me, e)
    End Sub
    'Public Sub OnChangeRoom(ByVal sender As Object, ByVal e As EventArgs)
    '    RaiseEvent OnChangeRoomsPersons(sender, e.Empty)
    'End Sub

#End Region

    'NUEVA FUNCION
    Public Sub Update(ByVal dn() As DataRow)
        Dim conte As ComunContenido
        'Dim ArchAnt As String
        Dim archivo As String = ""
        Dim dr As DataRow
        Dim dr2 As DataRow
        'Dim Extarch As String = ""
        With New presentacionContenido
            If dn(0)(ComunContenido.PKIDCONTENIDO_FIELD) > 0 Then
                'verifica si existe el contenido en la bd
                conte = .GetContenidoById(dn(0)(ComunContenido.PKIDCONTENIDO_FIELD), Me.IdIdioma)
                If conte.Tables(ComunContenido.CONTENIDOS_TABLA).Rows.Count > 0 Then
                    dr = conte.Tables(ComunContenido.CONTENIDOS_TABLA).Rows(0)
                    dr.Item(ComunContenido.WIDTH_FIELD) = Val(dn(0)(ComunContenido.WIDTH_FIELD))
                    dr.Item(ComunContenido.HEIGHT_FIELD) = Val(dn(0)(ComunContenido.HEIGHT_FIELD))
                    'existe el idioma difernete al default??
                    If conte.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows.Count > 0 Then
                        dr2 = conte.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows(0)
                    Else
                        dr2 = conte.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).NewRow
                        dr2.SetParentRow(dr)
                        conte.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows.Add(dr2)
                        dr2.Item(ComunContenido.ARCHIVO_FIELD) = ""
                    End If
                    dr2.Item(ComunContenido.DESCRIPCION_FIELD) = dn(0)(ComunContenido.DESCRIPCION_FIELD)
                    dr2.Item(ComunContenido.URL_FIELD) = dn(0)(ComunContenido.URL_FIELD)
                    dr2.Item(ComunContenido.ARCHIVO_FIELD) = dn(0)(ComunContenido.ARCHIVO_FIELD)
                    dr2.Item(ComunContenido.IDIDIOMA_FIELD) = Me.IdIdioma
                    .UpdateContenido(conte)
                End If
            Else
                archivo = dn(0)(ComunContenido.ARCHIVO_FIELD)
                If .CreateContenido(dn(0)(ComunContenido.IDELEMENTO_FIELD), IdEmpresa, _
                  IdIdioma, dn(0)(ComunContenido.WIDTH_FIELD), dn(0)(ComunContenido.HEIGHT_FIELD), _
                  dn(0)(ComunContenido.DESCRIPCION_FIELD), dn(0)(ComunContenido.URL_FIELD), dn(0)(ComunContenido.ARCHIVO_FIELD), conte) Then
                End If
            End If
        End With
    End Sub

    Public Function GetProperCase(ByVal InputString As String) As String

        Dim InputArray() As String = Regex.Split("" & InputString, "\\b")
        Dim WordCount As Integer = InputArray.GetUpperBound(0)
        Dim Prefix As String = ""
        Dim Suffix As String = ""
        Dim ReturnString As String = ""

        For iloop As Integer = 0 To WordCount
            If InputArray(iloop).Trim().Length > 0 Then
                Prefix = InputArray(iloop).Substring(0, 1)
                Prefix = Prefix.ToUpper()
                Suffix = InputArray(iloop).Substring(1)
                Suffix = Suffix.ToLower()
                ReturnString = ReturnString + Prefix + Suffix
            Else
                If InputArray(iloop) = " " Then
                    ReturnString = ReturnString + " "
                End If
            End If
        Next
        Return ReturnString

    End Function

    Public Sub CargaContenidoCCT(ByRef ctrl As Object, ByVal categoria As Integer, ByRef LnkCambiar As CambiarContenido, ByVal tipo As TipoControl, ByVal idDelHotel As Integer, ByVal img As Boolean, ByVal url As Boolean, ByVal cnt As Boolean, ByVal wt As Boolean, ByVal hg As Boolean)
        Dim conection As New SqlConnection(AppSettings("HotelConnectionString"))

        Dim spname As String = "spGetInfoToCallCenterByIdHotel"
        Dim command As New SqlCommand(spname, conection)
        'Dim idUsuario = CType(Me.Page, PaginaBase).Usuario
        'Dim idAsociacionHotel As Integer = CType(Me.Page, PaginaBase).GetIdAsociation

        With command
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@IdHotel", idDelHotel))
            .Parameters.Add(New SqlParameter("@Categoria", categoria))
        End With
        Dim adapter As New SqlDataAdapter(command)
        Dim dRes As New DataSet
        adapter.Fill(dRes)
        If dRes IsNot Nothing AndAlso dRes.Tables(0).Rows.Count > 0 Then
            If tipo = TipoControl.Label Then
                Dim lbl As Label = CType(ctrl, Label)
                If IdIdioma = 1 Then
                    lbl.Text = dRes.Tables(0).Rows(0)("Descripcion_ES").ToString()
                Else
                    lbl.Text = dRes.Tables(0).Rows(0)("Descripcion_EN").ToString()
                End If
            ElseIf tipo = TipoControl.Imagen Then
                Dim imgagecct As Image = CType(ctrl, Image) 'PathApli & IIf(IsImgHeader, "", AppSettings("Img_Prefix_Module")) & "?" & Now.ToString
                If Not String.IsNullOrEmpty(dRes.Tables(0).Rows(0)("UrlImagen").ToString) Then
                    PathApli = AppSettings("Albums_url").ToString.ToUpper.Replace("ALBUMS", "") + Replace(dRes.Tables(0).Rows(0)("UrlImagen").ToString(), "\", "/").Replace("//", "/")
                    imgagecct.ImageUrl = PathApli & IIf(IsImgHeader, "", AppSettings("Img_Prefix_Module")) & "?" & Now.ToString
                Else
                    imgagecct.ImageUrl = ""
                End If
            End If
        End If
        If tipo = TipoControl.Imagen Then
            LnkCambiar.ShowMode = CambiarContenido.ShowModeType.ViewImage
        End If
        If Not LnkCambiar Is Nothing Then
            LnkCambiar.Visible = True
            LnkCambiar.RHeight = hg
            LnkCambiar.RHRef = url
            LnkCambiar.Rwidth = wt
            LnkCambiar.RArch = img
            LnkCambiar.RConte = cnt
            LnkCambiar.elemento = ctrl
            LnkCambiar.IdIdioma = Me.IdIdioma
            LnkCambiar.IdHotelCCT = idDelHotel
            LnkCambiar.CategoriaCCT = categoria
            LnkCambiar.ISCCTContent = True
            If Me.IsPortal Then
                LnkCambiar.IdEmpresa = 0
            Else
                LnkCambiar.IdEmpresa = Me.IdEmpresa
            End If
            'LnkCambiar.text = dr.Item(ComunElementos.NOMBRE_FIELD) ' NombreEle
            AddHandler LnkCambiar.ChangeContenido, AddressOf Me.SeleccionaContenido
        End If

    End Sub

    'Public Function getLessThanEqual(ByVal size As String, ByVal pi As bkHotelesGWS.PictureInfo, ByRef aList As ArrayList) As String
    '    Dim sizes As String = "ELMSIT"

    '    'size E
    '    If sizes.IndexOf(size) <= 0 Then

    '        For i As Integer = 0 To aList.Count - 1
    '            Dim img As bkHotelesGWS.PictureInfo = aList.Item(i)
    '            If img.Url = pi.Url.Replace(pi.Size & ".", "E.") Then
    '                Return img.Url
    '            End If
    '        Next
    '    End If

    '    'size L 
    '    If sizes.IndexOf(size) <= 1 Then

    '        For i As Integer = 0 To aList.Count - 1
    '            Dim img As bkHotelesGWS.PictureInfo = aList.Item(i)
    '            If img.Url = pi.Url.Replace(pi.Size & ".", "L.") Then
    '                Return img.Url
    '            End If
    '        Next
    '    End If

    '    'size M
    '    If sizes.IndexOf(size) <= 2 Then

    '        For i As Integer = 0 To aList.Count - 1
    '            Dim img As bkHotelesGWS.PictureInfo = aList.Item(i)
    '            If img.Url = pi.Url.Replace(pi.Size & ".", "M.") Then
    '                Return img.Url
    '            End If
    '        Next
    '    End If

    '    'size S
    '    If sizes.IndexOf(size) <= 3 Then

    '        For i As Integer = 0 To aList.Count - 1
    '            Dim img As bkHotelesGWS.PictureInfo = aList.Item(i)
    '            If img.Url = pi.Url.Replace(pi.Size & ".", "S.") Then
    '                Return img.Url
    '            End If
    '        Next
    '    End If

    '    'size I
    '    If sizes.IndexOf(size) <= 4 Then

    '        For i As Integer = 0 To aList.Count - 1
    '            Dim img As bkHotelesGWS.PictureInfo = aList.Item(i)
    '            If img.Url = pi.Url.Replace(pi.Size & ".", "I.") Then
    '                Return img.Url
    '            End If
    '        Next
    '    End If

    '    'size T
    '    If sizes.IndexOf(size) <= 5 Then

    '        For i As Integer = 0 To aList.Count - 1
    '            Dim img As bkHotelesGWS.PictureInfo = aList.Item(i)
    '            If img.Url = pi.Url.Replace(pi.Size & ".", "T.") Then
    '                Return img.Url
    '            End If
    '        Next
    '    End If


    'End Function

End Class