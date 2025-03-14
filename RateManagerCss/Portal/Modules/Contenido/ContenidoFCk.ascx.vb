Imports Contenido.Comun
Imports Contenido.presentacion
'Imports Contenido.Datos
Imports Contenido.config
Imports System.IO
Imports Portal.Facade
Imports Portal.Common
Imports System.Web.UI.WebControls
Imports System.Configuration.ConfigurationManager

Partial Class ContenidoFCk
    Inherits System.Web.UI.UserControl

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

    Public ReadOnly Property IsPublished() As Boolean
        Get
            Dim flag As Boolean = True

            If Me.ViewState("published") IsNot Nothing Then flag = Me.ViewState("published")

            Return flag
        End Get
    End Property

    Dim moduleeditmode As Configuracion.typemode
    Protected WithEvents ImgControl As Imagenes
    Protected WithEvents imgControlDef As Imagenes

    Public Event HideComponent(ByVal Visible As Boolean)
    Private _width As Unit
    Private _height As Unit

    '// *****  Public Properties *****
#Region "Public Properties"

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
            If Value Then ShowMode = ShowModeType.Link
        End Set
    End Property

    Public Property RArch() As Boolean
        Get
            Return viewstate("RArch")
        End Get
        Set(ByVal Value As Boolean)
            viewstate("RArch") = Value
            If Not Value Then ShowMode = ShowModeType.ViewContenido
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
            Return CtrlIdiomaFCk1.textoIngles
        End Get
        Set(ByVal Value As String)
            CtrlIdiomaFCk1.textoIngles = Value
        End Set
    End Property

    Public Property ContenidoEspañol() As String
        Get
            Return CtrlIdiomaFCk1.textoEspañol
        End Get
        Set(ByVal Value As String)
            CtrlIdiomaFCk1.textoEspañol = Value
        End Set
    End Property

    Public WriteOnly Property heigthctrlidioma() As Long
        Set(ByVal Value As Long)
            CtrlIdiomaFCk1.Height = Value
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

    '/**************************************************************/ 
    'Esta variable State, declarada de tipo enum puede tener 2 valores
    'ShowMode.ViewImage            Cuando se esta editando la imagen 
    'ShowMode.ViewContenido        Cuando se está editando el contenido...
    'por default esta para ver imagen...
    Public _ShowMode As ShowModeType
    Protected WithEvents CtrlIdiomaFCk1 As CtrlIdiomaFCk

    Public Property ShowMode() As ShowModeType
        Get
            If Not IsNothing(viewstate("SHOW")) Then _ShowMode = viewstate("SHOW")
            Return _ShowMode
        End Get
        Set(ByVal Value As ShowModeType)
            _ShowMode = Value
            viewstate("SHOW") = _ShowMode
        End Set
    End Property

#End Region

    '//  *****  Var *****
    Private strUpdate As String = "Registro Actualizado"
    Private strEdit As String = "Editando"
    Private strNew As String = "Nuevo"
    Private strDelete As String = "Registro Elimado"

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

    Private Sub CargaControles()
        txtWidth.Visible = Me.Rwidth
        lblWidth.Visible = Me.Rwidth
        txtwidthDef.Visible = Me.Rwidth
        lblwidthDef.Visible = Me.Rwidth

        txtHeight.Visible = Me.RHeight
        lblHeight.Visible = Me.RHeight
        txtHeightDef.Visible = Me.RHeight
        lblHeightDef.Visible = Me.RHeight

        txtcontenido.Visible = Me.RConte
        lblContenido.Visible = Me.RConte
        txtcontenidodef.Visible = Me.RConte
        lblcontenidoDef.Visible = Me.RConte

        txtURL.Visible = Me.RHRef
        lblURL.Visible = Me.RHRef
        txturlDef.Visible = Me.RHRef
        lblURLDef.Visible = Me.RHRef

        lblURLArchivo.Visible = Me.RArch
        lblURLArchivoDef.Visible = Me.RArch

        If Me.IdCon = 0 Then
            txtHeight.Visible = False
            lblHeight.Visible = False
            txtWidth.Visible = False
            lblWidth.Visible = False
        End If

    End Sub

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

                    If Not cc.Tables(ComunContenido.CONTENIDOS_TABLA).Rows(0).IsNull(ComunContenido.DESCRIPCION_FIELD) Then
                        txtcontenidodef.Text = "" & cc.Tables(ComunContenido.CONTENIDOS_TABLA).Rows(0).Item(ComunContenido.DESCRIPCION_FIELD)
                    End If

                    If Not cc.Tables(ComunContenido.CONTENIDOS_TABLA).Rows(0).IsNull(ComunContenido.URL_FIELD) Then
                        txturlDef.Text = "" & cc.Tables(ComunContenido.CONTENIDOS_TABLA).Rows(0).Item(ComunContenido.URL_FIELD)
                    End If

                    If Not cc.Tables(ComunContenido.CONTENIDOS_TABLA).Rows(0).IsNull(ComunContenido.ARCHIVO_FIELD) Then
                        lblURLArchivoDef.Text = "" & cc.Tables(ComunContenido.CONTENIDOS_TABLA).Rows(0).Item(ComunContenido.ARCHIVO_FIELD)
                    End If

                End If
                txtcontenido.Text = ""

                Dim flag As Boolean = True
                If cc IsNot Nothing AndAlso cc.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA) IsNot Nothing AndAlso cc.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows.Count > 0 Then
                    flag = (CType(cc.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows(0)(ComunContenido.PUBLISHED_FIELD), Boolean))
                End If
                If flag AndAlso cc2 IsNot Nothing AndAlso cc2.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA) IsNot Nothing AndAlso cc2.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows.Count > 0 Then
                    flag = (CType(cc2.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows(0)(ComunContenido.PUBLISHED_FIELD), Boolean))
                End If

                Me.ViewState("published") = flag

                If cc.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows.Count > 0 Then
                    txtcontenido.Text = "" & cc.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows(0).Item(ComunContenido.DESCRIPCION_FIELD)
                    txtURL.Text = "" & cc.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows(0).Item(ComunContenido.URL_FIELD)
                    lblURLArchivo.Text = "" & cc.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows(0).Item(ComunContenido.ARCHIVO_FIELD)
                End If

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
                    Me.CtrlIdiomaFCk1.textoEspañol = txtcontenido.Text
                    Me.CtrlIdiomaFCk1.textoIngles = alter
                    Me.CtrlIdiomaFCk1.textoInglesUpdate = upd
                Else
                    Me.CtrlIdiomaFCk1.textoEspañol = alter
                    Me.CtrlIdiomaFCk1.textoEspañolUpdate = upd
                    Me.CtrlIdiomaFCk1.textoIngles = txtcontenido.Text
                End If
                Me.CtrlIdiomaFCk1.Height = 150


                ShowImg.Visible = False
                CtrlIdiomaFCk1.Visible = False
                btnQuitar.Visible = False
                Select Case Me.ShowMode
                    Case ShowModeType.ViewContenido
                        'Si el modo es verimagen entonces oculta el contenido
                        ShowImgDef.Visible = False
                        ShowImg.Visible = False
                        lblURLArchivoDef.Visible = False
                        lblURLArchivo.Visible = False
                        ImgControl.Visible = False
                        imgControlDef.Visible = False
                        CtrlIdiomaFCk1.Visible = True
                        CtrlIdiomaFCk1.isHtml = True
                        CtrlIdiomaFCk1.IdIndice = 0

                    Case ShowModeType.ViewImage
                        'sino, muestra la imagen 
                        If lblURLArchivo.Text.Trim <> "" Then
                            ShowImg.Visible = True
                            btnQuitar.Visible = True

                            Dim sPosfijo As String
                            sPosfijo = IIf(Me.IsImgHeader, "", Albums.Comun.General.getImgModulePrefix)
                            ShowImg.ImageUrl = AppSettings("Albums_url").ToString.ToUpper.Replace("/ALBUMS", "/") & lblURLArchivo.Text.Replace("//", "/") & sPosfijo & "?" & Now.ToString
                        End If
                        ImgControl.Visible = True
                        lblURLArchivo.Visible = True
                        ShowImgDef.Visible = True

                        ShowImgDef.ImageUrl = AppSettings("Albums_url").ToString.ToUpper.Replace("/ALBUMS", "/") & lblURLArchivoDef.Text.Replace("//", "/") & Albums.Comun.General.getImgModulePrefix
                        imgControlDef.Visible = True
                        lblURLArchivoDef.Visible = True
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
        If CtrlIdiomaFCk1.Visible Then
            txtcontenidodef.Visible = False
            txtcontenido.Visible = False
        End If
    End Sub

    Private isCallCenterInfo As Boolean = False
    Public Sub cargarDtosCCT(ByVal ds As DataSet)
        isCallCenterInfo = True
        Dim cc As String = String.Empty
        Dim cc2 As String = String.Empty
        Dim ccimage As String = String.Empty
        If ds IsNot Nothing And ds.Tables(0).Rows.Count > 0 Then
            If ShowMode = ShowModeType.ViewContenido Then
                If Me.Ididioma = 1 Then
                    If ds.Tables(0).Rows(0)("Descripcion_Es") IsNot Nothing Then
                        cc = ds.Tables(0).Rows(0)("Descripcion_Es").ToString
                    End If
                    If ds.Tables(0).Rows(0)("Descripcion_En") IsNot Nothing Then
                        cc2 = ds.Tables(0).Rows(0)("Descripcion_En").ToString
                    End If
                Else
                    If ds.Tables(0).Rows(0)("Descripcion_Es") IsNot Nothing Then
                        cc2 = ds.Tables(0).Rows(0)("Descripcion_Es").ToString
                    End If
                    If ds.Tables(0).Rows(0)("Descripcion_En") IsNot Nothing Then
                        cc = ds.Tables(0).Rows(0)("Descripcion_En").ToString
                    End If
                End If
            ElseIf ShowMode = ShowModeType.ViewImage Then
                If ds.Tables(0).Rows(0)("UrlImagen") IsNot Nothing Then
                    ccimage = ds.Tables(0).Rows(0)("UrlImagen").ToString
                    lblURLArchivoDef.Text = ccimage
                End If
            End If
        End If
        txtcontenidodef.Text = ""
        txtURL.Text = ""
        lblURLArchivo.Text = ccimage
        Me.CtrlIdiomaFCk1.Limpia()
        If Me.Ididioma = 1 Then
            Me.CtrlIdiomaFCk1.textoEspañol = cc
            Me.CtrlIdiomaFCk1.textoIngles = cc2
            Me.CtrlIdiomaFCk1.textoInglesUpdate = True
        Else
            Me.CtrlIdiomaFCk1.textoEspañol = cc2
            Me.CtrlIdiomaFCk1.textoEspañolUpdate = True
            Me.CtrlIdiomaFCk1.textoIngles = cc
        End If
        Me.CtrlIdiomaFCk1.Height = 150
        ShowImg.Visible = False
        CtrlIdiomaFCk1.Visible = False
        btnQuitar.Visible = False

        Select Case Me.ShowMode
            Case ShowModeType.ViewContenido
                ShowImgDef.Visible = False
                ShowImg.Visible = False
                lblURLArchivoDef.Visible = False
                lblURLArchivo.Visible = False
                ImgControl.Visible = False
                imgControlDef.Visible = False
                CtrlIdiomaFCk1.Visible = True
                CtrlIdiomaFCk1.isHtml = True
                CtrlIdiomaFCk1.IdIndice = 0
            Case ShowModeType.ViewImage
                'sino, muestra la imagen 
                If lblURLArchivo.Text.Trim <> "" Then
                    ShowImg.Visible = True
                    btnQuitar.Visible = True

                    Dim sPosfijo As String
                    sPosfijo = IIf(Me.IsImgHeader, "", Albums.Comun.General.getImgModulePrefix)
                    ShowImg.ImageUrl = AppSettings("Albums_url").ToString.ToUpper.Replace("/ALBUMS", "/") & lblURLArchivo.Text.Replace("//", "/") & sPosfijo & "?" & Now.ToString
                End If
                ImgControl.Visible = True
                lblURLArchivo.Visible = True
                ShowImgDef.Visible = True

                ShowImgDef.ImageUrl = AppSettings("Albums_url").ToString.ToUpper.Replace("/ALBUMS", "/") & lblURLArchivoDef.Text.Replace("//", "/") & Albums.Comun.General.getImgModulePrefix
                imgControlDef.Visible = True
                lblURLArchivoDef.Visible = True
        End Select
            txtwidthDef.Text = "0"
            txtHeightDef.Text = "0"


            CargaControles()
        If CtrlIdiomaFCk1.Visible Then
            txtcontenidodef.Visible = False
            txtcontenido.Visible = False
        End If
    End Sub

    Public Function GuardarCCTInfo(ByVal IdHotel As Integer, ByVal Categoria As Integer) As Boolean
        Dim conection As New System.Data.SqlClient.SqlConnection(AppSettings("HotelConnectionString"))

        Dim spname As String = String.Empty
        If Me.ShowMode = ShowModeType.ViewContenido Then
            spname = "spInsertUpdateInfoToCallCenterByIdHotel"
        ElseIf ShowMode = ShowModeType.ViewImage Then
            spname = "spInsertImageToCallCenterByIdHotel"
        End If
        Dim command As New System.Data.SqlClient.SqlCommand(spname, conection)
        Dim myTrans As System.Data.SqlClient.SqlTransaction
        conection.Open()
        myTrans = conection.BeginTransaction()
        Try
            command.CommandType = CommandType.StoredProcedure
            command.Transaction = myTrans
            command.Parameters.Add(New System.Data.SqlClient.SqlParameter("@IdHotel", IdHotel))
            command.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Categoria", Categoria))
            If Me.ShowMode = ShowModeType.ViewContenido Then
                command.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Descripcion_ES", Me.CtrlIdiomaFCk1.textoEspañol))
                command.Parameters.Add(New System.Data.SqlClient.SqlParameter("@Descripcion_EN", Me.CtrlIdiomaFCk1.textoIngles))
            ElseIf ShowMode = ShowModeType.ViewImage Then
                command.Parameters.Add(New System.Data.SqlClient.SqlParameter("@UrlImagen", Me.lblURLArchivo.Text))
            End If
            
            command.ExecuteNonQuery()
            myTrans.Commit()
            conection.Close()
            If Me.Ididioma = 1 Then
                txtcontenido.Text = CtrlIdiomaFCk1.textoEspañol
            Else
                txtcontenido.Text = CtrlIdiomaFCk1.textoIngles
            End If
            Return True
        Catch
            myTrans.Rollback()
            conection.Close()
            Return False
        End Try
    End Function

    Public Sub Update(Optional ByVal publish As Boolean = True, Optional ByRef hasChanged As Boolean = False)
        Dim conte As ComunContenido
        'Dim ArchAnt As String
        Dim archivo As String = ""
        Dim dr As DataRow
        Dim dr2 As DataRow
        Dim Extarch As String = ""

        If Me.IdEle > 0 Or Me.IdCon > 0 Then
            If Me.CtrlIdiomaFCk1.Visible = False Then
                If PortalCulture.GetIDCulture = 1 Then
                    Me.CtrlIdiomaFCk1.textoEspañol = Me.txtcontenido.Text
                Else
                    Me.CtrlIdiomaFCk1.textoIngles = Me.txtcontenido.Text
                End If

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
                            dr2.Item(ComunContenido.DESCRIPCION_FIELD) = CtrlIdiomaFCk1.textoEspañol
                        Else
                            dr2.Item(ComunContenido.DESCRIPCION_FIELD) = CtrlIdiomaFCk1.textoIngles
                        End If
                        'dr2.Item(ComunContenido.DESCRIPCION_FIELD) = txtcontenido.Text
                        dr2.Item(ComunContenido.URL_FIELD) = txtURL.Text
                        dr2.Item(ComunContenido.ARCHIVO_FIELD) = lblURLArchivo.Text
                        dr2.Item(ComunContenido.IDIDIOMA_FIELD) = Me.Ididioma


                        .UpdateContenido(conte, publish, hasChanged)

                        If Me.Ididioma = 1 Then
                            If CtrlIdiomaFCk1.textoInglesUpdate Then
                                dr2.Item(ComunContenido.DESCRIPCION_FIELD) = CtrlIdiomaFCk1.textoIngles
                                dr2.Item(ComunContenido.IDIDIOMA_FIELD) = 2
                            Else
                                Dim dr3 As DataRow
                                dr3 = conte.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).NewRow

                                conte.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows.Add(dr3)
                                dr3.Item(ComunContenido.PKIDCONTENIDO_FIELD) = conte.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows(0).Item(ComunContenido.PKIDCONTENIDO_FIELD)
                                dr3.Item(ComunContenido.ARCHIVO_FIELD) = conte.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows(0).Item(ComunContenido.ARCHIVO_FIELD)
                                dr3.Item(ComunContenido.URL_FIELD) = conte.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows(0).Item(ComunContenido.URL_FIELD)
                                dr3.Item(ComunContenido.DESCRIPCION_FIELD) = CtrlIdiomaFCk1.textoIngles
                                dr3.Item(ComunContenido.IDIDIOMA_FIELD) = 2
                            End If
                        Else
                            If CtrlIdiomaFCk1.textoEspañolUpdate Then
                                dr2.Item(ComunContenido.IDIDIOMA_FIELD) = 1
                                dr2.Item(ComunContenido.DESCRIPCION_FIELD) = CtrlIdiomaFCk1.textoEspañol
                            Else
                                Dim dr3 As DataRow
                                dr3 = conte.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).NewRow

                                conte.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows.Add(dr3)
                                dr3.Item(ComunContenido.PKIDCONTENIDO_FIELD) = conte.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows(0).Item(ComunContenido.PKIDCONTENIDO_FIELD)
                                dr3.Item(ComunContenido.ARCHIVO_FIELD) = conte.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows(0).Item(ComunContenido.ARCHIVO_FIELD)
                                dr3.Item(ComunContenido.URL_FIELD) = conte.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows(0).Item(ComunContenido.URL_FIELD)
                                dr3.Item(ComunContenido.IDIDIOMA_FIELD) = 1
                                dr3.Item(ComunContenido.DESCRIPCION_FIELD) = CtrlIdiomaFCk1.textoEspañol
                            End If
                        End If
                        .UpdateContenido(conte, publish, hasChanged)
                    End If
                Else
                    archivo = lblURLArchivo.Text
                    If Me.Ididioma = 1 Then
                        txtcontenido.Text = CtrlIdiomaFCk1.textoEspañol
                    Else
                        txtcontenido.Text = CtrlIdiomaFCk1.textoIngles
                    End If
                    If .CreateContenido(Me.IdEle, Me.IdEmp, Me.Ididioma, Val(txtWidth.Text), Val(txtHeight.Text), txtcontenido.Text, txtURL.Text, archivo, conte, publish, hasChanged) Then
                        dr2 = conte.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).NewRow

                        conte.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows.Add(dr2)
                        dr2.Item(ComunContenido.PKIDCONTENIDO_FIELD) = conte.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows(0).Item(ComunContenido.PKIDCONTENIDO_FIELD)
                        dr2.Item(ComunContenido.ARCHIVO_FIELD) = conte.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows(0).Item(ComunContenido.ARCHIVO_FIELD)
                        dr2.Item(ComunContenido.URL_FIELD) = conte.Tables(ComunContenido.CONTENIDO_IDIOMA_TABLA).Rows(0).Item(ComunContenido.URL_FIELD)
                        With New presentacionContenido
                            If Me.Ididioma = 1 Then
                                dr2.Item(ComunContenido.IDIDIOMA_FIELD) = 2
                                dr2.Item(ComunContenido.DESCRIPCION_FIELD) = CtrlIdiomaFCk1.textoIngles
                            Else
                                dr2.Item(ComunContenido.IDIDIOMA_FIELD) = 1
                                dr2.Item(ComunContenido.DESCRIPCION_FIELD) = CtrlIdiomaFCk1.textoEspañol
                            End If
                            dr2.AcceptChanges()
                            'dr2.BeginEdit()
                            'dr2.EndEdit()
                            dr2.SetModified()

                            If .UpdateContenido(conte, publish, hasChanged) Then
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

    Public Sub DeleteContenidoByIdContenido()
        If Me.IdCon > 0 Then
            With New presentacionContenido
                If Not .DeleteIdContenidoHotelRoom(Me.IdCon, AppSettings("Albums_Dir")) Then

                End If
            End With
        End If
    End Sub

    Public Sub DeleteContenidoHotelItem()
        If Me.IdCon > 0 Then
            With New presentacionContenido
                If Not .DeleteIdContenidoHotelItem(Me.IdCon, AppSettings("Albums_Dir")) Then

                End If
            End With
        End If
    End Sub


    Private Sub CargaEmpresaRubro()
        ImgControl.IdEmpresa = Me.IdEmp
        imgControlDef.IsImgHeader = Me.IsImgHeader
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
        lnkContenido.Font.Bold = True
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

End Class
