Imports System.Configuration.ConfigurationManager
Imports System.Text
Imports Contenido.Comun
Imports contenido.presentacion

Partial Class CambiarContenido
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
    Protected WithEvents ctlConstructorGramatical1 As ctlConstructorGramatical
    ' Delegate Sub Click(ByVal sender As Object)
    ' Public Event ClickEdit As Click

    Delegate Sub ChangeConte(ByVal sender As Object, ByVal e As ArgsConte)
    Public Event ChangeContenido As ChangeConte

    Public elemento As Control

    Public Enum ShowModeType
        ViewImage
        ViewContenido
    End Enum


#Region "Propiedades"
    Public Property Nota() As String
        Get
            Return EditConte.Nota
        End Get
        Set(ByVal Value As String)
            EditConte.Nota = Value
        End Set
    End Property
    Public Property Pagina() As String
        Get
            Return EditConte.Pagina
        End Get
        Set(ByVal Value As String)
            EditConte.Pagina = Value
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
    Public WriteOnly Property EliminaContenido() As Boolean
        Set(ByVal Value As Boolean)
            ImageButton2.Visible = Value
        End Set
    End Property
    Public Property IdIdioma() As Long
        Get
            Return viewstate("IdIdioma")
        End Get
        Set(ByVal Value As Long)
            viewstate("IdIdioma") = Value
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

    Public Property RArch() As Boolean
        Get
            Return viewstate("RArch")
        End Get
        Set(ByVal Value As Boolean)
            viewstate("RArch") = Value
        End Set
    End Property

    Public Property idContenido() As Long
        Get
            Return viewstate("idContenido")
        End Get
        Set(ByVal Value As Long)
            viewstate("idContenido") = Value
        End Set
    End Property

    Public Property idElemento() As Long
        Get
            Return viewstate("idElemento")
        End Get
        Set(ByVal Value As Long)
            viewstate("idElemento") = Value
            Call CargaElemento(Value)
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

    Public Property IsImgHeader() As Boolean
        Get
            Return EditConte.IsImgHeader
        End Get
        Set(ByVal Value As Boolean)
            EditConte.IsImgHeader = Value

        End Set
    End Property

    Public Property IdHotelCCT() As Long
        Get
            Return ViewState("IdHotelCCT")
        End Get
        Set(ByVal Value As Long)
            ViewState("IdHotelCCT") = Value
        End Set
    End Property

    Public Property CategoriaCCT() As Long
        Get
            Return ViewState("CategoriaCCT")
        End Get
        Set(ByVal Value As Long)
            ViewState("CategoriaCCT") = Value
        End Set
    End Property

    Public Property ISCCTContent() As Long
        Get
            Return ViewState("ISCCTContent")
        End Get
        Set(ByVal Value As Long)
            ViewState("ISCCTContent") = Value
        End Set
    End Property

    Public ReadOnly Property EditButton() As ImageButton
        Get
            Return Me.ImageButton1
        End Get
    End Property

    Public ReadOnly Property DeleteButtonAllContent() As ImageButton
        Get
            Return Me.ImageButton2
        End Get
    End Property

    Public ReadOnly Property DeleteButtonByIdContent() As ImageButton
        Get
            Return Me.ImageButton3
        End Get
    End Property

    Public Property IdTypeRoomHotelChangeContent() As Integer
        Get
            Return ViewState("IdTypeRoomHotelChangeContent")
        End Get
        Set(value As Integer)
            ViewState("IdTypeRoomHotelChangeContent") = value
        End Set
    End Property

    Public Property CodeTypeRoomHotelChangeContent() As String
        Get
            Return ViewState("CodeTypeRoomHotelChangeContent")
        End Get
        Set(value As String)
            ViewState("CodeTypeRoomHotelChangeContent") = value
        End Set
    End Property

#End Region

    Public Sub OnChangeContenido(ByVal idcon As Long, ByVal idele As Long)
        Dim e As New ArgsConte(idcon, idele, Me.RConte, Me.RHRef, Me.RArch, Me.Rwidth, Me.RHeight)
        RaiseEvent ChangeContenido(Me, e)
    End Sub 'ActivateFireAlarm


    'Protected Overridable Sub OnChangeContenido(ByVal e As EventArgs)
    '    RaiseEvent ChangeContenido(Me, e)
    'End Sub

    Private Sub CargaElemento(ByVal idElemento As Integer)
        Dim data As ComunElementos
        data = (New presentacionelementos).GetelementoById(idElemento)
        viewstate("IDGRUPO") = Nothing
        viewstate("GRUPOTIPO") = Nothing
        viewstate("GRUPOMAX") = Nothing
        If Not IsNothing(data) Then
            If Not IsDBNull(data.Tables(ComunElementos.elementos_TABLA).Rows(0).Item(ComunElementos.FIELD_IDGRUPO)) Then
                ViewState("IDGRUPO") = data.Tables(ComunElementos.elementos_TABLA).Rows(0).Item(ComunElementos.FIELD_IDGRUPO)
                ViewState("GRUPOTIPO") = data.Tables(ComunElementos.elementos_TABLA).Rows(0).Item(ComunElementos.FIELD_GRUPOTIPO)
                If Not IsDBNull(data.Tables(ComunElementos.elementos_TABLA).Rows(0).Item(ComunElementos.FIELD_GRUPOMAX)) Then
                    ViewState("GRUPOMAX") = data.Tables(ComunElementos.elementos_TABLA).Rows(0).Item(ComunElementos.FIELD_GRUPOMAX)
                Else
                    ViewState("GRUPOMAX") = 1
                End If
            End If
        End If
    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página

        If cmdGramatical.Visible = False Then
            If Not IsNothing(viewstate("IDGRUPO")) Then
                ctlConstructorGramatical1.Grupo = CInt(viewstate("IDGRUPO"))
                ctlConstructorGramatical1.Idioma = Me.IdIdioma '0
                ctlConstructorGramatical1.Tipo = CInt(viewstate("GRUPOTIPO")) 'ctlConstructorGramatical.TipoGrupo.Check
                ctlConstructorGramatical1.GrupoMax = CInt(viewstate("GRUPOMAX"))
            End If
        End If
    End Sub

    Private Sub lnkCambiar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        pEdit.Visible = True
        EditConte.RArch = Me.RArch
        EditConte.RConte = Me.RConte
        EditConte.RHRef = Me.RHRef
        EditConte.Rwidth = Me.Rwidth
        EditConte.RHeight = Me.RHeight
        '  EditConte.IsMultilanguage = Me.IsMultilanguage
        EditConte.IdEmp = Me.IdEmpresa
        EditConte.IdCon = Me.idContenido
        EditConte.IdEle = Me.idElemento
        EditConte.Ididioma = Me.IdIdioma
        'Me.elemento.ClientID()
        EditConte.CargaDatos()
        EditConte.Visible = True
        ctlConstructorGramatical1.Visible = False
        cmdGramatical.Visible = False
        Me.cmdGramatical.Text = PortalCulture.GetString("A00678")

        ctlConstructorGramatical1.Titulo = PortalCulture.GetString("A00678")
        If Me.ShowMode = ShowModeType.ViewContenido And Not IsNothing(viewstate("IDGRUPO")) Then
            cmdGramatical.Visible = True
            cmdFreeForm.Visible = False
        End If
        '        elemento.Visible = False
    End Sub

    Private Sub cmdGuardar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdGuardar.Click, cmdPublicar.Click
        'If Page.IsValid Then

        If ISCCTContent Then
            If EditConte.GuardarCCTInfo(IdHotelCCT, CategoriaCCT) Then
                CType(Me.Page, PaginaBase).guardalog(Me.Pagina, PaginaBase.acciones.Modificar, "Se modificó el contenido. " & Nota)
                CType(Me.Page, PaginaBase).NotifyContentModification(Me.Nota)
            Else
                CType(Me.Page, PaginaBase).guardalog(Me.Pagina, PaginaBase.acciones.Modificar, "Error al modificar el contenido. " & Nota)
            End If
            Dim ds As DataSet = GetInfoToCallCenter()
            If ds IsNot Nothing AndAlso ds.Tables(0).Rows.Count > 0 Then
                If ShowMode = ShowModeType.ViewContenido Then
                    If IdIdioma = 1 Then
                        CType(elemento, Label).Text = ds.Tables(0).Rows(0)("Descripcion_ES").ToString
                    Else
                        CType(elemento, Label).Text = ds.Tables(0).Rows(0)("Descripcion_EN").ToString
                    End If
                ElseIf ShowMode = ShowModeType.ViewImage Then
                    Dim PathApli As String = AppSettings("Albums_url").ToString.ToUpper.Replace("ALBUMS", "") + Replace(ds.Tables(0).Rows(0)("UrlImagen").ToString, "\", "/").Replace("//", "/")
                    CType(elemento, Image).ImageUrl = PathApli & IIf(IsImgHeader, "", AppSettings("Img_Prefix_Module")) & "?" & Now.ToString
                End If
            End If
                If Not elemento Is Nothing Then elemento.Visible = True
                pEdit.Visible = False
            Else
                Dim publish As Boolean = (CType(sender, Button).ID = Me.cmdPublicar.ID)
                Dim action As PaginaBase.acciones = If(publish, PaginaBase.acciones.Publicar, PaginaBase.acciones.Modificar)
                Dim hasChanged As Boolean = False

                If Me.ShowMode = ShowModeType.ViewContenido Then
                    If ctlConstructorGramatical1.Visible = False Then
                        Call Guarda(publish, hasChanged)
                        CType(Me.Page, PaginaBase).guardalog(Me.Pagina, action, "Se modificó el contenido. " & Nota)
                    Else
                        'EditConte.Visible = True
                        'aqui pasar el valor al editconte
                        EditConte.Contenido = ctlConstructorGramatical1.Valor
                        'ctlConstructorGramatical1.Visible = False
                        cmdGramatical.Visible = True
                        CType(Me.Page, PaginaBase).guardalog(Me.Pagina, action, "Se modificó el contenido. " & Nota)
                    End If
                Else
                    Call Guarda(publish, hasChanged)
                    If EditConte.GetUrlArchivo = "" Then
                        CType(Me.Page, PaginaBase).guardalog(Me.Pagina, action, "Se modificó la imagen para mostrar: No hay imagen para mostrar. " & Nota)
                    Else
                        CType(Me.Page, PaginaBase).guardalog(Me.Pagina, action, "Se modificó la imagen para mostrar: " & AppSettings("Albums_url").ToString.ToUpper.Replace("ALBUMS", "") & EditConte.GetUrlArchivo & ". " & Nota)
                    End If

                End If
                If hasChanged Then CType(Me.Page, PaginaBase).NotifyContentModification(Me.Nota)
            End If
    End Sub

    Private Sub Guarda(ByVal publish As Boolean, ByRef hasChanged As Boolean)
        EditConte.IdEmp = Me.IdEmpresa
        EditConte.IdCon = Me.idContenido
        EditConte.IdEle = Me.idElemento
        EditConte.Ididioma = Me.IdIdioma
        EditConte.Update(publish, hasChanged)
        If Not elemento Is Nothing Then elemento.Visible = True
        OnChangeContenido(Me.idContenido, Me.idElemento)
        pEdit.Visible = False

    End Sub
    Private Sub cmdFreeForm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFreeForm.Click

        'If ctlConstructorGramatical1.Visible = True Then
        EditConte.Visible = True
        EditConte.heigthctrlidioma = 150
        ctlConstructorGramatical1.Visible = False
        If Not IsNothing(viewstate("IDGRUPO")) Then
            cmdGramatical.Visible = True
            cmdFreeForm.Visible = False
        End If
        'End If
    End Sub

    Private Sub cmdCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancelar.Click
        If Me.ShowMode = ShowModeType.ViewContenido Then
            '            If ctlConstructorGramatical1.Visible = True Then
            '            EditConte.Visible = True
            '            ctlConstructorGramatical1.Visible = False
            '            If Not IsNothing(viewstate("IDGRUPO")) Then
            '            cmdGramatical.Visible = True
            '            cmdFreeForm.Visible = False
            '        End If
            '        Else
            pEdit.Visible = False
            If Not elemento Is Nothing Then elemento.Visible = True
            '        End If
        Else
            pEdit.Visible = False
            If Not elemento Is Nothing Then elemento.Visible = True
        End If

    End Sub

    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        pEdit.Visible = True
        EditConte.RArch = Me.RArch
        EditConte.RConte = Me.RConte
        EditConte.RHRef = Me.RHRef
        EditConte.Rwidth = Me.Rwidth
        EditConte.RHeight = Me.RHeight
        'EditConte.IsMultilanguage = Me.IsMultilanguage
        EditConte.IdEmp = Me.IdEmpresa
        EditConte.ShowMode = Me.ShowMode
        EditConte.EditMode = Me.EditMode
        EditConte.IdCon = Me.idContenido
        EditConte.IdEle = Me.idElemento
        EditConte.Ididioma = Me.IdIdioma

        If Not elemento Is Nothing Then elemento.Visible = False
        If ISCCTContent Then
            Dim ds As DataSet = GetInfoToCallCenter()
            EditConte.cargarDtosCCT(ds)
        Else
            EditConte.CargaDatos()
        End If
        EditConte.Visible = True
        ctlConstructorGramatical1.Visible = False
        cmdGramatical.Visible = False
        cmdFreeForm.Visible = False

        Me.cmdGramatical.Text = PortalCulture.GetString("A00685")
        ctlConstructorGramatical1.Titulo = PortalCulture.GetString("A00685")
        If Me.ShowMode = ShowModeType.ViewContenido And Not IsNothing(ViewState("IDGRUPO")) Then
            'cmdGramatical.Visible = True
            'cmdGramatical_Click(sender, e)
            EditConte.Visible = True
            cmdFreeForm_Click(sender, e)
        End If
    End Sub

    Public Function GetInfoToCallCenter() As DataSet
        Dim conection As New System.Data.SqlClient.SqlConnection(AppSettings("HotelConnectionString"))

        Dim spname As String = "spGetInfoToCallCenterByIdHotel"
        Dim command As New System.Data.SqlClient.SqlCommand(spname, conection)
        'Dim idUsuario = CType(Me.Page, PaginaBase).Usuario
        'Dim idAsociacionHotel As Integer = CType(Me.Page, PaginaBase).GetIdAsociation

        With command
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New System.Data.SqlClient.SqlParameter("@IdHotel", IdHotelCCT))
            .Parameters.Add(New System.Data.SqlClient.SqlParameter("@Categoria", CategoriaCCT))
        End With
        Dim adapter As New System.Data.SqlClient.SqlDataAdapter(command)
        Dim dRes As New DataSet
        adapter.Fill(dRes)
        Return dRes
    End Function

    Private Sub ImageButton2_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton2.Click
        If Me.idContenido > 0 Then
            EditConte.IdCon = Me.idContenido
            EditConte.IdEle = Me.idElemento
            EditConte.DeleteContenidoAll()
            pEdit.Visible = False
            'elemento.Visible = True
            OnChangeContenido(Me.idContenido, Me.idElemento)
        End If
    End Sub

    Private Sub ImageButton3_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton3.Click
        If Me.idContenido > 0 Then
            EditConte.IdCon = Me.idContenido
            EditConte.IdEle = Me.idElemento
            EditConte.DeleteContenidoByIdContenido()

            Dim queryString As String = String.Format("?idroomTypeHotel={0}&codeRoomTypeHotel={1}", Me.IdTypeRoomHotelChangeContent, Me.CodeTypeRoomHotelChangeContent) ' Que siempre elimine el querysTring y revisar los parametros

            Dim redirectUrl As String = Request.Path & queryString

            Response.Redirect(redirectUrl)
        End If
    End Sub

    Public Property EditMode() As ShowModeType
        Get
            Return EditConte.ShowMode
        End Get
        Set(ByVal Value As ShowModeType)
            EditConte.ShowMode = Value
        End Set
    End Property

    Public _ShowMode As ShowModeType
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

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        MyBase.Render(writer)
        ''optener el objeto yo
        ''posicionar el x y el y del yo donde el elemento
        'Dim str As StringBuilder = New StringBuilder("<script>")
        'Dim yo As String = Me.elemento.ClientID
        'str.Append(" var x=document.getelementById('" & yo & "');")
        'str.Append(" var y=document.getelementById('" & pEdit.ClientID & "');")
        'str.Append(" var y.style.left=findposx(x);")
        'str.Append(" var y.style.top=findposy(x);")
        'str.Append("</script>")
        'writer.Write(str.ToString)
    End Sub

    Private Sub EditConte_HideComponent(ByVal Visible As Boolean) Handles EditConte.HideComponent
        pEdit.Visible = Visible
        If Not elemento Is Nothing Then elemento.Visible = True
    End Sub

    Private Sub cmdGramatical_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdGramatical.Click
        EditConte.Visible = False
        ctlConstructorGramatical1.Visible = True
        cmdGramatical.Visible = False
        cmdFreeForm.Visible = True

        Dim str As StringBuilder = New StringBuilder("<script>")
        '        Dim yo As String = Me.elemento.ClientID
        str.Append(" var y=document.getElementById('" & pEdit.ClientID & "');")
        str.Append(" y.style.left=160;")
        str.Append("</script>")
        'Page.RegisterStartupScript("posit", str.ToString)
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "posit", str.ToString)
        ctlConstructorGramatical1.idElemento = Me.idElemento


    End Sub

    Private Sub ctlConstructorGramatical1_Valores(ByVal Valor As String, ByVal ValorAlt As String) Handles ctlConstructorGramatical1.Valores

        EditConte.IdEmp = Me.IdEmpresa
        EditConte.IdCon = Me.idContenido
        EditConte.IdEle = Me.idElemento
        EditConte.Contenido = Valor
        EditConte.Ididioma = Me.IdIdioma
        If Me.IdIdioma = 2 Then
            EditConte.ContenidoEspañol = ValorAlt
            EditConte.ContenidoIngles = Valor
        Else
            EditConte.ContenidoIngles = ValorAlt
            EditConte.ContenidoEspañol = Valor
        End If


        EditConte.Update()

        EditConte.Contenido = ValorAlt
        If Me.IdIdioma = 1 Then
            EditConte.Ididioma = 2
        Else
            EditConte.Ididioma = 1
        End If
        EditConte.Update()
        If Not elemento Is Nothing Then elemento.Visible = True
        OnChangeContenido(Me.idContenido, Me.idElemento)
        pEdit.Visible = False

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Me.lblTitle.Text = PortalCulture.GetString("A00666")
        ImageButton1.ToolTip = PortalCulture.GetString("A00666")
        ImageButton2.ToolTip = PortalCulture.GetString("A00667")

        If ctlConstructorGramatical1.Visible Then
            Dim str As StringBuilder = New StringBuilder("<script>")
            str.Append(" var y=document.getElementById('" & pEdit.ClientID & "');")
            str.Append(" y.style.left=160;")
            str.Append("</script>")
            'Page.RegisterStartupScript("posit", str.ToString)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "posit", str.ToString)
            ctlConstructorGramatical1.GuardaScript()
        End If

        Me.cmdGuardar.Text = PortalCulture.GetString("A00153")
        Me.cmdCancelar.Text = PortalCulture.GetString("A00670")
    End Sub

End Class
