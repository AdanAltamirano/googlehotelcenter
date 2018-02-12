Imports Contenido.Comun
Imports Contenido.presentacion
Imports Contenido.Datos
Imports Contenido.config
Imports System.IO
Imports Portal.Facade
Imports Portal.Common
Imports System.Web.UI.WebControls
Imports System.Configuration.ConfigurationManager

Partial Class Contenidos
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

    Dim moduleeditmode As Configuracion.typemode
    Protected WithEvents ImgControl As Imagenes
    Protected WithEvents imgControlDef As Imagenes
    Private _showAlbum As Boolean = False

    Public Event HideComponent(ByVal Visible As Boolean)
    Private _width As Unit
    Private _height As Unit

    Public Property Nota() As String
        Get
            Return ImgControl.Nota
        End Get
        Set(ByVal Value As String)
            ImgControl.Nota = Value
            imgControlDef.Nota = Value
        End Set
    End Property
    Public Property Pagina() As String
        Get
            Return ImgControl.Pagina
        End Get
        Set(ByVal Value As String)
            ImgControl.Pagina = Value
            imgControlDef.Pagina = Value
        End Set
    End Property

    Public Property Width() As Unit
        Get
            Return _width
        End Get
        Set(ByVal Value As Unit)
            _width = Value
        End Set
    End Property

    Public Property Height() As Unit
        Get
            Return _height
        End Get
        Set(ByVal Value As Unit)
            _height = Value
        End Set
    End Property

    Public Enum ShowModeType
        ViewImage
        ViewContenido
        Link
    End Enum

    Public Property Contenido() As String
        Get
            Return txtcontenido.Text
        End Get
        Set(ByVal Value As String)
            txtcontenido.Text = Value
        End Set
    End Property
    Public Property ContenidoIngles() As String
        Get
            Dim temp As String = CtrlIdioma1.textoIngles
            If Me.CtrlIdiomaFCk1.Visible Then temp = CtrlIdiomaFCk1.textoIngles
            Return temp
        End Get
        Set(ByVal Value As String)
            Me.CtrlIdiomaFCk1.textoIngles = Value
            CtrlIdioma1.textoIngles = Value
        End Set
    End Property

    Public Property ContenidoEspañol() As String
        Get
            Dim temp As String = CtrlIdioma1.textoEspañol
            If Me.CtrlIdiomaFCk1.Visible Then temp = CtrlIdiomaFCk1.textoEspañol
            Return temp
        End Get
        Set(ByVal Value As String)
            CtrlIdioma1.textoEspañol = Value
            CtrlIdiomaFCk1.textoEspañol = Value
        End Set
    End Property
    Public WriteOnly Property heigthctrlidioma() As Long
        Set(ByVal Value As Long)
            CtrlIdioma1.Height = Value
        End Set
    End Property
    Public ReadOnly Property GetUrlArchivo() As String
        Get
            Return lblURLArchivo.Text
        End Get
    End Property

    Public Property IsImgHeader() As Boolean
        Get
            Return ImgControl.IsImgHeader
        End Get
        Set(ByVal Value As Boolean)
            ImgControl.IsImgHeader = Value

        End Set
    End Property

    Public Property IsAlbumEnable() As Boolean
        Get
            Return Me._showAlbum
        End Get
        Set(ByVal value As Boolean)
            Me._showAlbum = value
        End Set
    End Property

    '/**************************************************************/ 
    'Esta variable State, declarada de tipo enum puede tener 2 valores
    'ShowMode.ViewImage            Cuando se esta editando la imagen 
    'ShowMode.ViewContenido        Cuando se está editando el contenido...
    'por default esta para ver imagen...
    Public _ShowMode As ShowModeType = ShowModeType.ViewContenido
    Protected WithEvents CtrlIdioma1 As CtlIdioma
    Protected WithEvents CtrlIdiomaFCk1 As CtrlIdiomaFCk

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
            viewstate("SHOW") = _ShowMode
        End Set
    End Property

    Private Sub CargaControles()
        txtWidth.Visible = Me.Rwidth
        lblWidth.Visible = Me.Rwidth
        txtwidthDef.Visible = Me.Rwidth
        lblwidthDef.Visible = Me.Rwidth

        txtHeight.Visible = Me.RHeight
        lblHeight.Visible = Me.RHeight
        txtHeightDef.Visible = Me.RHeight
        lblHeightDef.Visible = Me.RHeight

        txtcontenido.Visible = (Not Me.IsMultilanguage AndAlso Me.RConte)
        lblContenido.Visible = Me.RConte
        txtcontenidodef.Visible = (Not Me.IsMultilanguage AndAlso Me.RConte)
        lblcontenidoDef.Visible = (Not Me.IsMultilanguage AndAlso Me.RConte)

        'Me.CtrlIdioma1.Visible = (Me.IsMultilanguage AndAlso Me.RConte)
        'Me.CtrlIdiomaFCk1.Visible = (Me.IsMultilanguage AndAlso Me.RConte)

        txtURL.Visible = Me.RHRef
        lblURL.Visible = Me.RHRef
        txturlDef.Visible = Me.RHRef
        lblURLDef.Visible = Me.RHRef

        'Me.ImgControl.Visible = Me.IsAlbumEnable

        lblURLArchivo.Visible = Me.RArch
        lblURLArchivoDef.Visible = Me.RArch

        'lnkContenidoDef.Visible = True
        'lnkContenido.Visible = True
        If Me.IdCon = 0 Then
            txtHeight.Visible = False
            lblHeight.Visible = False

            txtWidth.Visible = False
            lblWidth.Visible = False
            'lnkContenidoDef.Visible = False
            'lnkContenido.Visible = False
        End If

    End Sub

#Region "Propiedades"

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

    Public Property RHRef() As Boolean
        Get
            Return viewstate("RHRef")
        End Get
        Set(ByVal Value As Boolean)
            viewstate("RHRef") = Value
            If Value Then ShowMode = ShowModeType.Link
        End Set
    End Property

    Public Property RArch() As Boolean
        Get
            Return viewstate("RArch")
        End Get
        Set(ByVal Value As Boolean)
            viewstate("RArch") = Value
            If Value Then ShowMode = ShowModeType.ViewContenido
        End Set
    End Property

    Public Property EditMode() As Configuracion.typemode
        Get
            EditMode = moduleeditmode
        End Get
        Set(ByVal Value As Configuracion.typemode)
            moduleeditmode = Value
        End Set
    End Property

    Public Property IdEmp() As Long
        Get
            Return viewstate("IdEmp")
        End Get
        Set(ByVal Value As Long)
            viewstate("IdEmp") = Value
            CargaEmpresaRubro()
        End Set
    End Property

    Public Property IdCon() As Long
        Get
            Return viewstate("IdCon")
        End Get
        Set(ByVal Value As Long)
            viewstate("IdCon") = Value
        End Set
    End Property
    Public Property IdEle() As Long
        Get
            Return viewstate("IdEle")
        End Get
        Set(ByVal Value As Long)
            viewstate("IdEle") = Value

        End Set
    End Property

    Public Property Ididioma() As Long
        Get
            Return viewstate("Ididioma")
        End Get
        Set(ByVal Value As Long)
            viewstate("Ididioma") = Value

        End Set
    End Property
#End Region

    Private strUpdate As String = "Registro Actualizado"
    Private strEdit As String = "Editando"
    Private strNew As String = "Nuevo"
    Private strDelete As String = "Registro Elimado"

    Public Sub CargaDatos()
        If Me.IdCon > 0 Or Me.IdEle > 0 Then
            If Me.Ididioma > 0 Then
                Dim cc As ComunContenido
                Dim cc2 As ComunContenido
                'ShowCampos()
                If Me.IdCon = 0 Then
                    cc = (New presentacionContenido).GetContenidobyPagina(Me.IdEle, Me.IdEmp, Me.Ididioma)
                    If cc.Tables(ComunContenido.CONTENIDOS_TABLA).Rows.Count > 0 Then
                        If Me.Ididioma = 1 Then
                            cc2 = (New presentacionContenido).GetContenidobyPagina(Me.IdEle, Me.IdEmp, 2)
                        Else
                            cc2 = (New presentacionContenido).GetContenidobyPagina(Me.IdEle, Me.IdEmp, 1)
                        End If
                    End If
                Else
                    cc = (New presentacionContenido).GetContenidoById(Me.IdCon, Me.Ididioma)
                    If cc.Tables(ComunContenido.CONTENIDOS_TABLA).Rows.Count > 0 Then
                        If Me.Ididioma = 1 Then
                            cc2 = (New presentacionContenido).GetContenidoById(Me.IdCon, 2)
                        Else
                            cc2 = (New presentacionContenido).GetContenidoById(Me.IdCon, 1)
                        End If
                    End If
                End If

                If cc.Tables(ComunContenido.CONTENIDOS_TABLA).Rows.Count > 0 Then
                    If Not cc.Tables(ComunContenido.CONTENIDOS_TABLA).Rows(0).IsNull(ComunContenido.DESCRIPCION_FIELD) AndAlso Me.IdCon > 0 Then
                        txtcontenidodef.Text = "" & cc.Tables(ComunContenido.CONTENIDOS_TABLA).Rows(0).Item(ComunContenido.DESCRIPCION_FIELD)
                    End If
                    If Not cc.Tables(ComunContenido.CONTENIDOS_TABLA).Rows(0).IsNull(ComunContenido.URL_FIELD) AndAlso Me.IdCon > 0 Then
                        txturlDef.Text = "" & cc.Tables(ComunContenido.CONTENIDOS_TABLA).Rows(0).Item(ComunContenido.URL_FIELD)
                    End If
                    If Not cc.Tables(ComunContenido.CONTENIDOS_TABLA).Rows(0).IsNull(ComunContenido.ARCHIVO_FIELD) AndAlso Me.IdCon > 0 Then
                        lblURLArchivoDef.Text = "" & cc.Tables(ComunContenido.CONTENIDOS_TABLA).Rows(0).Item(ComunContenido.ARCHIVO_FIELD)
                    End If
                End If
                txtcontenido.Text = ""
                If cc.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows.Count > 0 AndAlso Me.IdCon > 0 Then
                    txtcontenido.Text = "" & cc.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows(0).Item(ComunContenido.DESCRIPCION_FIELD)
                    txtURL.Text = "" & cc.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows(0).Item(ComunContenido.URL_FIELD)
                    lblURLArchivo.Text = "" & cc.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows(0).Item(ComunContenido.ARCHIVO_FIELD)
                End If

                CtrlIdioma1.Limpia()
                Me.CtrlIdiomaFCk1.Limpia()

                Dim alter As String = ""
                Dim upd As Boolean = True
                If Not IsNothing(cc2) Then
                    If cc2.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows.Count > 0 Then
                        alter = "" & cc2.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows(0).Item(ComunContenido.DESCRIPCION_FIELD)
                    Else
                        upd = False
                    End If
                Else
                    upd = False
                End If

                If Me.Ididioma = 1 Then
                    CtrlIdioma1.textoEspañol = txtcontenido.Text
                    CtrlIdioma1.textoIngles = alter
                    CtrlIdioma1.textoInglesUpdate = upd

                    Me.CtrlIdiomaFCk1.textoEspañol = txtcontenido.Text
                    Me.CtrlIdiomaFCk1.textoIngles = alter
                    Me.CtrlIdiomaFCk1.textoInglesUpdate = upd

                Else
                    CtrlIdioma1.textoEspañol = alter
                    CtrlIdioma1.textoIngles = txtcontenido.Text
                    CtrlIdioma1.textoEspañolUpdate = upd

                    Me.CtrlIdiomaFCk1.textoEspañol = alter
                    Me.CtrlIdiomaFCk1.textoIngles = txtcontenido.Text
                    Me.CtrlIdiomaFCk1.textoEspañolUpdate = upd

                End If
                'CtrlIdioma1.textoEspañol = txtcontenido.Text
                'CtrlIdioma1.textoIngles = txtcontenido.Text
                CtrlIdioma1.Height = 150
                'Me.CtrlIdiomaFCk1.Height = 150


                ShowImg.Visible = False
                CtrlIdioma1.Visible = False
                btnQuitar.Visible = False
                CtrlIdiomaFCk1.Visible = False
                Select Case Me.ShowMode
                    Case ShowModeType.ViewContenido
                        'Si el modo es verimagen entonces oculta el contenido
                        ShowImgDef.Visible = False
                        ShowImg.Visible = False
                        lblURLArchivoDef.Visible = False
                        lblURLArchivo.Visible = False
                        ImgControl.Visible = False
                        imgControlDef.Visible = False
                        'CtrlIdioma1.Visible = True
                        'CtrlIdioma1.IsHTML = True
                        'CtrlIdioma1.IdIndice = 0
                        CtrlIdiomaFCk1.Visible = True

                        Me.ImgControl.Visible = False

                    Case ShowModeType.ViewImage
                        'sino, muestra la imagen 
                        Dim sPosfijo As String
                        If lblURLArchivo.Text.Trim <> "" Then
                            ShowImg.Visible = True
                            btnQuitar.Visible = True
                            sPosfijo = IIf(Me.IsImgHeader, "", Albums.Comun.General.getImgModulePrefix)
                            'ShowImg.ImageUrl = AppSettings("Albums_url").ToString.ToUpper.Replace("/ALBUMS", "/") & lblURLArchivo.Text & Albums.Comun.General.getImgModulePrefix & "?" & Now.ToString
                            ShowImg.ImageUrl = AppSettings("Albums_url").ToString.ToUpper.Replace("/ALBUMS", "/") & lblURLArchivo.Text.Replace("//", "/") & sPosfijo & "?" & Now.ToString
                        End If
                        ImgControl.Visible = True

                        lblURLArchivo.Visible = True
                        ShowImgDef.Visible = True
                        sPosfijo = IIf(Me.IsImgHeader, "", Albums.Comun.General.getImgModulePrefix)

                        ShowImgDef.ImageUrl = AppSettings("Albums_url").ToString.ToUpper.Replace("/ALBUMS", "/") & lblURLArchivoDef.Text.Replace("//", "/") & sPosfijo
                        imgControlDef.Visible = True
                        lblURLArchivoDef.Visible = True
                        Me.ImgControl.Visible = True

                    Case ShowModeType.Link
                        If lblURLArchivo.Text.Trim <> "" Then
                            ShowImg.Visible = True
                            btnQuitar.Visible = True
                            ShowImg.ImageUrl = AppSettings("Albums_url").ToString.ToUpper.Replace("/ALBUMS", "") & lblURLArchivo.Text.Replace("//", "/") & Albums.Comun.General.getImgModulePrefix
                        End If
                        ImgControl.Visible = True
                        lblURLArchivo.Visible = True

                        ShowImgDef.Visible = True
                        'TODO: Cambio imagenes
                        ShowImgDef.ImageUrl = AppSettings("Albums_url").ToString.ToUpper.Replace("/ALBUMS", "") & lblURLArchivoDef.Text.Replace("//", "/") & Albums.Comun.General.getImgModulePrefix

                        imgControlDef.Visible = True
                        lblURLArchivoDef.Visible = True

                End Select

                'TODO: El "../../" se deberá reemplazar por el valor real
                'se puede obtener de clase común de albums...

                If cc.Tables(ComunContenido.CONTENIDOS_TABLA).Rows.Count > 0 Then
                    txtwidthDef.Text = "" & cc.Tables(ComunContenido.CONTENIDOS_TABLA).Rows(0).Item(ComunContenido.WIDTH_FIELD)
                    txtHeightDef.Text = "" & cc.Tables(ComunContenido.CONTENIDOS_TABLA).Rows(0).Item(ComunContenido.HEIGHT_FIELD)
                End If
            End If
        End If
        CargaControles()
        If CtrlIdioma1.Visible Then
            txtcontenidodef.Visible = False
            txtcontenido.Visible = False
        End If
    End Sub

    Public Sub Update()
        Dim conte As ComunContenido
        ' Dim ArchAnt As String
        Dim archivo As String = ""
        Dim dr As DataRow
        Dim dr2 As DataRow
        Dim Extarch As String = ""
        If Me.IdEle > 0 Or Me.IdCon > 0 Then

            If Me.CtrlIdioma1.Visible = False Then
                If PortalCulture.GetIDCulture = 1 Then
                    Me.CtrlIdioma1.textoEspañol = Me.txtcontenido.Text
                Else
                    Me.CtrlIdioma1.textoIngles = Me.txtcontenido.Text
                End If
            End If

            If Me.CtrlIdiomaFCk1.Visible = False Then
                'If PortalCulture.GetIDCulture = 1 Then
                '    Me.CtrlIdiomaFCk1.textoEspañol = Me.txtcontenido.Text
                'Else
                '    Me.CtrlIdiomaFCk1.textoIngles = Me.txtcontenido.Text
                'End If
            End If

            With New presentacionContenido
                If Me.IdCon > 0 Then
                    conte = .GetContenidoById(Me.IdCon, Me.Ididioma)

                    If conte.Tables(ComunContenido.CONTENIDOS_TABLA).Rows.Count > 0 Then

                        dr = conte.Tables(ComunContenido.CONTENIDOS_TABLA).Rows(0)
                        dr.Item(ComunContenido.WIDTH_FIELD) = Val(txtwidthDef.Text)
                        dr.Item(ComunContenido.HEIGHT_FIELD) = Val(txtHeightDef.Text)
                        If (dr.IsNull(ComunContenido.DESCRIPCION_FIELD) AndAlso txtcontenidodef.Text.Trim <> "") OrElse _
                            (Not dr.IsNull(ComunContenido.DESCRIPCION_FIELD) AndAlso dr.Item(ComunContenido.DESCRIPCION_FIELD).trim <> txtcontenidodef.Text.Trim) Then
                            dr.Item(ComunContenido.DESCRIPCION_FIELD) = txtcontenidodef.Text.Trim
                        End If

                        If (dr.IsNull(ComunContenido.URL_FIELD) AndAlso txturlDef.Text.Trim <> "") OrElse _
                            (Not dr.IsNull(ComunContenido.URL_FIELD) AndAlso dr.Item(ComunContenido.URL_FIELD).trim <> txturlDef.Text.Trim) Then
                            dr.Item(ComunContenido.URL_FIELD) = txturlDef.Text.Trim
                        End If

                        If (dr.IsNull(ComunContenido.ARCHIVO_FIELD) AndAlso lblURLArchivoDef.Text.Trim <> "") OrElse _
                            (Not dr.IsNull(ComunContenido.ARCHIVO_FIELD) AndAlso dr.Item(ComunContenido.ARCHIVO_FIELD).trim <> lblURLArchivoDef.Text.Trim) Then
                            dr.Item(ComunContenido.ARCHIVO_FIELD) = lblURLArchivoDef.Text
                        End If

                        If conte.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows.Count > 0 Then
                            dr2 = conte.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows(0)
                        Else
                            dr2 = conte.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).NewRow
                            dr2.SetParentRow(dr)
                            conte.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows.Add(dr2)
                            dr2.Item(ComunContenido.ARCHIVO_FIELD) = ""
                        End If
                        If Me.Ididioma = AppSettings("DefaultLanguageId") Then
                            dr.Item(ComunContenido.DESCRIPCION_FIELD) = txtcontenido.Text
                            dr.Item(ComunContenido.URL_FIELD) = txtURL.Text
                            dr.Item(ComunContenido.ARCHIVO_FIELD) = lblURLArchivo.Text
                        End If
                        If Me.Ididioma = 1 Then
                            If CtrlIdioma1.Visible Then
                                dr2.Item(ComunContenido.DESCRIPCION_FIELD) = CtrlIdioma1.textoEspañol
                            ElseIf CtrlIdiomaFCk1.Visible Then
                                dr2.Item(ComunContenido.DESCRIPCION_FIELD) = CtrlIdiomaFCk1.textoEspañol
                            End If
                        Else
                            If CtrlIdioma1.Visible Then
                                dr2.Item(ComunContenido.DESCRIPCION_FIELD) = CtrlIdioma1.textoIngles
                            ElseIf CtrlIdiomaFCk1.Visible Then
                                dr2.Item(ComunContenido.DESCRIPCION_FIELD) = CtrlIdiomaFCk1.textoIngles
                            End If
                        End If
                            'dr2.Item(ComunContenido.DESCRIPCION_FIELD) = txtcontenido.Text
                            dr2.Item(ComunContenido.URL_FIELD) = txtURL.Text
                            dr2.Item(ComunContenido.ARCHIVO_FIELD) = lblURLArchivo.Text
                            dr2.Item(ComunContenido.IDIDIOMA_FIELD) = Me.Ididioma

                            .UpdateContenido(conte)

                        If Me.Ididioma = 1 Then

                            If CtrlIdioma1.textoInglesUpdate Then
                                'dr2.Item(ComunContenido.DESCRIPCION_FIELD) = If(CtrlIdioma1.Visible, CtrlIdioma1.textoIngles, CtrlIdiomaFCk1.textoIngles)
                                dr2.Item(ComunContenido.DESCRIPCION_FIELD) = CtrlIdioma1.textoIngles
                                dr2.Item(ComunContenido.IDIDIOMA_FIELD) = 2
                            Else
                                Dim dr3 As DataRow
                                dr3 = conte.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).NewRow
                                conte.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows.Add(dr3)
                                dr3.Item(ComunContenido.PKIDCONTENIDO_FIELD) = conte.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows(0).Item(ComunContenido.PKIDCONTENIDO_FIELD)
                                dr3.Item(ComunContenido.ARCHIVO_FIELD) = conte.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows(0).Item(ComunContenido.ARCHIVO_FIELD)
                                dr3.Item(ComunContenido.URL_FIELD) = conte.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows(0).Item(ComunContenido.URL_FIELD)
                                'dr3.Item(ComunContenido.DESCRIPCION_FIELD) = If(CtrlIdioma1.Visible, CtrlIdioma1.textoIngles, CtrlIdiomaFCk1.textoIngles)
                                dr3.Item(ComunContenido.DESCRIPCION_FIELD) = CtrlIdioma1.textoIngles
                                dr3.Item(ComunContenido.IDIDIOMA_FIELD) = 2
                            End If
                        Else
                            If CtrlIdioma1.textoEspañolUpdate Then
                                dr2.Item(ComunContenido.IDIDIOMA_FIELD) = 1
                                dr2.Item(ComunContenido.DESCRIPCION_FIELD) = CtrlIdioma1.textoEspañol
                                'dr2.Item(ComunContenido.DESCRIPCION_FIELD) = If(CtrlIdioma1.Visible, CtrlIdioma1.textoEspañol, CtrlIdiomaFCk1.textoEspañol)
                            Else
                                Dim dr3 As DataRow
                                dr3 = conte.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).NewRow
                                conte.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows.Add(dr3)
                                dr3.Item(ComunContenido.PKIDCONTENIDO_FIELD) = conte.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows(0).Item(ComunContenido.PKIDCONTENIDO_FIELD)
                                dr3.Item(ComunContenido.ARCHIVO_FIELD) = conte.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows(0).Item(ComunContenido.ARCHIVO_FIELD)
                                dr3.Item(ComunContenido.URL_FIELD) = conte.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows(0).Item(ComunContenido.URL_FIELD)
                                dr3.Item(ComunContenido.IDIDIOMA_FIELD) = 1
                                dr3.Item(ComunContenido.DESCRIPCION_FIELD) = CtrlIdioma1.textoEspañol
                                'dr3.Item(ComunContenido.DESCRIPCION_FIELD) = If(CtrlIdioma1.Visible, CtrlIdioma1.textoEspañol, CtrlIdiomaFCk1.textoEspañol)
                            End If
                        End If
                            .UpdateContenido(conte)
                        End If
                Else
                    archivo = lblURLArchivo.Text
                    'If Me.Ididioma = 1 Then
                    '    If Me.CtrlIdiomaFCk1.Visible Then
                    '        txtcontenido.Text = CtrlIdiomaFCk1.textoEspañol
                    '    Else
                    '        txtcontenido.Text = CtrlIdioma1.textoEspañol
                    '    End If
                    'Else
                    '    If Me.CtrlIdiomaFCk1.Visible Then
                    '        txtcontenido.Text = CtrlIdiomaFCk1.textoIngles
                    '    Else
                    '        txtcontenido.Text = CtrlIdioma1.textoIngles
                    '    End If
                    'End If
                    If Me.Ididioma = 1 Then
                        txtcontenido.Text = CtrlIdioma1.textoEspañol
                    Else
                        txtcontenido.Text = CtrlIdioma1.textoIngles
                    End If
                    If .CreateContenido(Me.IdEle, Me.IdEmp, Me.Ididioma, Val(txtWidth.Text), Val(txtHeight.Text), txtcontenido.Text, txtURL.Text, archivo, conte) Then
                        dr2 = conte.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).NewRow

                        conte.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows.Add(dr2)
                        dr2.Item(ComunContenido.PKIDCONTENIDO_FIELD) = conte.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows(0).Item(ComunContenido.PKIDCONTENIDO_FIELD)
                        dr2.Item(ComunContenido.ARCHIVO_FIELD) = conte.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows(0).Item(ComunContenido.ARCHIVO_FIELD)
                        dr2.Item(ComunContenido.URL_FIELD) = conte.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows(0).Item(ComunContenido.URL_FIELD)

                        With New presentacionContenido
                            'If Me.Ididioma = 1 Then
                            '    dr2.Item(ComunContenido.IDIDIOMA_FIELD) = 2
                            '    If Me.CtrlIdiomaFCk1.Visible Then
                            '        dr2.Item(ComunContenido.DESCRIPCION_FIELD) = CtrlIdiomaFCk1.textoIngles
                            '    Else
                            '        dr2.Item(ComunContenido.DESCRIPCION_FIELD) = CtrlIdioma1.textoIngles
                            '    End If
                            'Else
                            '    dr2.Item(ComunContenido.IDIDIOMA_FIELD) = 1
                            '    If Me.CtrlIdiomaFCk1.Visible Then
                            '        dr2.Item(ComunContenido.DESCRIPCION_FIELD) = CtrlIdiomaFCk1.textoEspañol
                            '    Else
                            '        dr2.Item(ComunContenido.DESCRIPCION_FIELD) = CtrlIdioma1.textoEspañol
                            '    End If
                            'End If
                            If Me.Ididioma = 1 Then
                                dr2.Item(ComunContenido.IDIDIOMA_FIELD) = 2
                                dr2.Item(ComunContenido.DESCRIPCION_FIELD) = CtrlIdioma1.textoIngles
                            Else
                                dr2.Item(ComunContenido.IDIDIOMA_FIELD) = 1
                                dr2.Item(ComunContenido.DESCRIPCION_FIELD) = CtrlIdioma1.textoEspañol
                            End If
                            If .UpdateContenido(conte) Then
                                'ok
                                Dim x As Integer = 30
                                x = x + 1
                            End If
                        End With
                    End If
                End If
            End With
        End If
    End Sub

    Private Sub cmbIdioma_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If Me.IdCon > 0 Or Me.IdEle > 0 Then
            limpia()
            CargaDatos()
        End If
    End Sub
    Private Sub limpia()
        lblMensaje.Visible = False
        txtcontenido.Text = ""
        txtURL.Text = ""
        txtWidth.Text = 0
        txtHeight.Text = 0

        lblmensajeDef.Visible = False
        txtcontenidodef.Text = ""
        txturlDef.Text = ""
        txtwidthDef.Text = 0
        txtHeightDef.Text = 0
    End Sub
    Public Sub DeleteContenido()
        If Me.IdCon > 0 And Me.Ididioma Then
            With New presentacionContenido
                If Not .Delete(Me.IdCon, Me.Ididioma, AppSettings("Albums_Dir")) Then
                    lblMensaje.Visible = False
                    lblMensaje.Text = "No se pudo Borrar el contenido"
                End If
            End With
        End If
    End Sub
    Public Sub DeleteContenidoAll()
        If Me.IdCon > 0 Then
            With New presentacionContenido
                If Not .Delete(Me.IdCon, AppSettings("Albums_Dir")) Then
                    lblMensaje.Visible = False
                    lblMensaje.Text = "No se pudo Borrar el contenido"
                End If
            End With
        End If
    End Sub
    Private Sub CargaEmpresaRubro()
        ImgControl.IdEmpresa = Me.IdEmp
        imgControlDef.IdEmpresa = Me.IdEmp
        '    Dim cr As ComunRubros
        '    With New presentacionRubro
        '        cr = .GetRubroEmp(Me.IdEmp)
        '        If Not cr Is Nothing AndAlso cr.Tables(cr.RUBROS_TABLA).Rows.Count > 0 Then
        '            ImgControl.IdRubro = cr.Tables(cr.RUBROS_TABLA).Rows(0).Item(cr.PKIDRUBRO_FIELD)
        '            imgControlDef.IdRubro = ImgControl.IdRubro
        '        End If
        '    End With
    End Sub

    Private Sub ImgControl_GetImgPath(ByVal strUrl As String) Handles ImgControl.GetImgPath
        lblURLArchivo.Text = strUrl
        ShowImg.Visible = True
        'TODO: Cambio Imagenes
        'ShowImg.ImageUrl = AppSettings("Albums_url").ToString.ToUpper.Replace("/ALBUMS", "") & "/" & strUrl & Albums.Comun.General.getImgModulePrefix
        ShowImg.ImageUrl = AppSettings("Albums_url").ToString.ToUpper.Replace("/ALBUMS", "") & "/" & strUrl.Replace("//", "/") & IIf(Me.IsImgHeader, "", Albums.Comun.General.getImgModulePrefix) ' Albums.Comun.General.getImgModulePrefix
        btnQuitar.Visible = True
    End Sub
    Private Sub ImgControl_HideIMGComponent(ByVal Visible As Boolean) Handles ImgControl.HideIMGComponent
        RaiseEvent HideComponent(Visible)
    End Sub

    Private Sub ImgControlDef_GetImgPath(ByVal strUrl As String) Handles imgControlDef.GetImgPath
        lblURLArchivoDef.Text = strUrl
        ShowImgDef.ImageUrl = AppSettings("Albums_url").ToString.ToUpper.Replace("/ALBUMS", "") & "/" & strUrl.Replace("//", "/") & Albums.Comun.General.getImgModulePrefix
    End Sub
    Private Sub ImgControlDef_HideIMGComponent(ByVal Visible As Boolean) Handles imgControlDef.HideIMGComponent
        RaiseEvent HideComponent(Visible)
    End Sub

    Private Sub lnkContenido_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkContenido.Click
        TContDef.Visible = False
        Tcont.Visible = True
        tcContenido.Attributes.Add("class", "")
        tcDefault.Attributes.Add("class", "cssSelected")
        lnkContenido.Style.Item("color") = "#000000"
        'lnkContenidoDef.Style.Item("color") = "#FFFFFF"

        lnkContenido.Font.Bold = True
        'lnkContenidoDef.Font.Bold = False

        CargaControles()
    End Sub

    Private Sub lnkContenidoDef_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkContenidoDef.Click
        Tcont.Visible = False
        TContDef.Visible = True
        tcContenido.Attributes.Add("class", "cssSelected")
        tcDefault.Attributes.Add("class", "")
        lnkContenidoDef.Style.Item("color") = "#000000"
        lnkContenido.Style.Item("color") = "#FFFFFF"

        lnkContenido.Font.Bold = False
        lnkContenidoDef.Font.Bold = True

        CargaControles()
    End Sub
    Private Sub VisibleControles(ByVal p As Panel)
        Dim c As Control
        For Each c In p.Controls
            c.Visible = True
        Next
    End Sub

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        lblContenido.Text = PortalCulture.GetString("A00660")
        lblWidth.Text = PortalCulture.GetString("A00661")
        REWwidth.Text = PortalCulture.GetString("A00662")
        lblHeight.Text = PortalCulture.GetString("A00663")
        REVHeight.Text = PortalCulture.GetString("A00664")
    End Sub
    Private Sub btnQuitar_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnQuitar.Click
        Me.lblURLArchivo.Text = ""
        Me.ShowImg.ImageUrl = ""
        ShowImg.Visible = False
        btnQuitar.Visible = False
    End Sub

    Public Function Resize(ByVal isResize As Boolean, ByVal arg1 As String) As String
        If IsImgHeader And isResize Then
            Return (String.Format("width: {0};", arg1))
        End If
        Return ""
    End Function


End Class
