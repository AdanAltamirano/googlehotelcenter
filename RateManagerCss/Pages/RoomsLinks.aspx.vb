Imports Portal.General.Facade
Imports Portal.General.Common.Data
Imports Portal.Hotel.Common.Data
Imports Portal.Hotel.Facade
Partial Class RoomsLinks
    Inherits PaginaBase
    Protected WithEvents CtlMensajes1 As ctlMensajes
    Private Enum dgColumns
        sourceDescription
        targetDescription
        ratio
        offset
        roundamount
        edit
        delete
        targetId
        sourceId
        TargetCode
        NameTarget
        NameSource
    End Enum
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
    Protected CtrRoomLink1 As ctrRoomLink
    Private Property idroom() As Integer
        Get
            Return viewstate("_idroom")
        End Get
        Set(ByVal Value As Integer)
            viewstate("_idroom") = Value
        End Set
    End Property

    Sub MostrarCmdNew(ByVal show As Boolean)
        cmdNew.Style.Add("display", IIf(show, "block", "none"))
        divContenedor.Style.Add("display", IIf(show, "none", "block"))
    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)
        If Not IsPostBack Then
            Me.idroom = 0
            If Not Request.QueryString("Room") Is Nothing Then
                Me.idroom = Request.QueryString("Room")
            End If
            CtrRoomLink1.Editar = False
            CtrRoomLink1.m_iHotelId = cInfoActual.Hotel
            Me.lblError.Visible = False
            loadLinks()
            MostrarCmdNew(True)
        End If
        cmdNew.Attributes.Add("onclick", String.Format("javascript:FireShow('{0}','{1}',{2});", divContenedor.ClientID, cmdNew.ClientID, "true"))
        'divContenedor.Style("display") = "none"
        'If (MostarDivContenedor) Then divContenedor.Style("display") = ""
        'btnMostarDivContenedor.Visible = Not MostarDivContenedor

        'Me.ResizefrmPrincipal()
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        cmdNew.Value = PortalCulture.GetString("M000058")
        Me.lblTitle.Text = PortalCulture.GetString("00266")
        Me.btnAceptar.Text = PortalCulture.GetString("00008")
        Me.btnCancel.Text = PortalCulture.GetString("00009")
        Me.dgLinks.PagerStyle.PrevPageText = "<< " & PortalCulture.GetString("00010")
        Me.dgLinks.PagerStyle.NextPageText = PortalCulture.GetString("00011") & " >>"
        Me.lblError.Text = PortalCulture.GetString("00125")
        dgLinks.Columns(dgColumns.offset).HeaderText = PortalCulture.GetString("00422")
        dgLinks.Columns(dgColumns.ratio).HeaderText = PortalCulture.GetString("00421")
        dgLinks.Columns(dgColumns.roundamount).HeaderText = PortalCulture.GetString("00042")
        dgLinks.Columns(dgColumns.sourceDescription).HeaderText = PortalCulture.GetString("00124")
        dgLinks.Columns(dgColumns.targetDescription).HeaderText = PortalCulture.GetString("00123")
        If CtrRoomLink1.Editar = False Then
            CType(Me.Page, PaginaBase).Habilitaboton(permisos.RoomsLinks, Me.btnAceptar, "A")
        End If
        Dim LK As LinkButton
        Dim hpl As HyperLink
        For Each i As DataGridItem In Me.dgLinks.Items
            If i.ItemType = ListItemType.AlternatingItem Or i.ItemType = ListItemType.Item Then
                hpl = i.Cells(dgColumns.delete).FindControl("lnkEliminar")
                CType(Me.Page, PaginaBase).Habilitaboton(permisos.RoomsLinks, hpl, "D")
                LK = i.Cells(dgColumns.targetDescription).FindControl("lnkEdit")
                CType(Me.Page, PaginaBase).Habilitaboton(permisos.RoomsLinks, LK, "M")
            End If
        Next

    End Sub

    Private Sub loadLinks()
        Dim links As New LinkRoomTypeData
        With New LinkRoomsFacade
            links = .getList(Me.cInfoActual.Hotel, PortalCulture.GetIDCulture)
        End With

        dgLinks.DataSource = links
        dgLinks.DataBind()
        Dim ds As RoomsHotelData
        With New RoomFacade
            ds = .getRooms(MyBase.cInfoActual.Hotel, PortalCulture.GetIDCulture)
        End With
        ds.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Columns.Add("texto", System.Type.GetType("System.String"), "substring(" & RoomsHotelData.FLD_ROOM_CODE & "+ ' ' + '--' + ' ' +" & RoomsHotelData.FLD_NOMBRE & ",1,25)")
        Dim dv As DataView

        ''eliminar los rooms que ya tienen links
        For Each r As DataRow In ds.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows
            dv = links.Tables(LinkRoomTypeData.TABLE_LINKROOM).DefaultView
            dv.RowFilter = LinkRoomTypeData.FIELD_TargetRoom & "=" & r(RoomsHotelData.FLD_ID_ROOM_HOTEL)
            If dv.Count > 0 Then
                r.Delete()
            End If

        Next
        ds.Tables(RoomsHotelData.TBL_ROOM_HOTEL).AcceptChanges()

        CtrRoomLink1.SourceDataSet = ds

        For Each r As DataRow In ds.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Rows
            dv = links.Tables(LinkRoomTypeData.TABLE_LINKROOM).DefaultView
            dv.RowFilter = LinkRoomTypeData.FIELD_SourceRoom & "=" & r(RoomsHotelData.FLD_ID_ROOM_HOTEL)
            If dv.Count > 0 Then
                r.Delete()
            End If
        Next
        ds.Tables(RoomsHotelData.TBL_ROOM_HOTEL).AcceptChanges()

        If Me.idroom <> 0 Then
            With New RoomFacade
                ds = .getRoomByID(Me.idroom, PortalCulture.GetIDCulture)
            End With
            ds.Tables(RoomsHotelData.TBL_ROOM_HOTEL).Columns.Add("texto", System.Type.GetType("System.String"), "substring(" & RoomsHotelData.FLD_ROOM_CODE & "+ ' ' + '--' + ' ' +" & RoomsHotelData.FLD_NOMBRE & ",1,25)")
        End If
        CtrRoomLink1.TargetDataSet = ds
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        If Not Page.IsValid Then Return
        lblError.Visible = False
        If CtrRoomLink1.Editar OrElse Not CtrRoomLink1.samelink Then
            If CtrRoomLink1.SaveLinks() Then
                Me.dgLinks.SelectedIndex = -1
                CtrRoomLink1.cleardata()
                loadLinks()
                MostrarCmdNew(True)
            Else
                Me.lblError.Visible = True
            End If
        Else
            MostrarCmdNew(False)
        End If
    End Sub

    Private Sub dgLinks_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgLinks.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.SelectedItem Then
            If e.Item.Cells(dgColumns.targetId).Text = "&nbsp;" Then
                e.Item.Cells(dgColumns.targetId).Text = ""
            End If
            If e.Item.Cells(dgColumns.sourceId).Text = "&nbsp;" Then
                e.Item.Cells(dgColumns.sourceId).Text = ""
            End If
            e.Item.Cells(dgColumns.targetDescription).Text &= " - " & e.Item.Cells(dgColumns.NameTarget).Text
            e.Item.Cells(dgColumns.sourceDescription).Text &= " - " & e.Item.Cells(dgColumns.NameSource).Text
            Dim LK As LinkButton
            LK = e.Item.Cells(dgColumns.edit).FindControl("lnkEdit")
            LK.Text = PortalCulture.GetString("00093")
            LK = e.Item.Cells(dgColumns.delete).FindControl("lnkEliminar2")
            Dim hpl As HyperLink
            hpl = e.Item.Cells(dgColumns.delete).FindControl("lnkEliminar")
            hpl.Text = PortalCulture.GetString("00103")
            hpl.NavigateUrl = CtlMensajes1.getShow(LK.ClientID, PortalCulture.GetString("00266"), PortalCulture.GetString("00462"))

        End If
        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(dgColumns.offset).Text = PortalCulture.GetString("00422")
            e.Item.Cells(dgColumns.ratio).Text = PortalCulture.GetString("00421")
            e.Item.Cells(dgColumns.roundamount).Text = PortalCulture.GetString("00042")
            e.Item.Cells(dgColumns.sourceDescription).Text = PortalCulture.GetString("00124")
            e.Item.Cells(dgColumns.targetDescription).Text = PortalCulture.GetString("00123")
        ElseIf e.Item.ItemType = ListItemType.Footer Then
            e.Item.Cells(dgColumns.delete).Text = CType(Me.dgLinks.DataSource, LinkRoomTypeData).Tables(LinkRoomTypeData.TABLE_LINKROOM).Rows.Count & " " & PortalCulture.GetString("00475")
        End If
    End Sub

    Private Sub dgLinks_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgLinks.ItemCommand
        If e.CommandName = "Eliminar" Then
            With New LinkRoomsFacade
                If .Delete(dgLinks.Items(e.Item.ItemIndex).Cells(dgColumns.targetId).Text, dgLinks.Items(e.Item.ItemIndex).Cells(dgColumns.sourceId).Text) Then
                    Me.guardalog("/Pages/RoomsLinks.aspx", PaginaBase.acciones.Eliminar, "Se eliminó el linkeo de " & dgLinks.Items(e.Item.ItemIndex).Cells(dgColumns.TargetCode).Text & " con " & dgLinks.Items(e.Item.ItemIndex).Cells(dgColumns.sourceDescription).Text)
                    CtrRoomLink1.cleardata()
                    loadLinks()
                    If dgLinks.CurrentPageIndex > 0 And dgLinks.Items.Count = 1 Then
                        dgLinks.CurrentPageIndex = (((dgLinks.Items.Count - 1) * dgLinks.PageSize) - 1) \ dgLinks.PageSize
                    End If
                    dgLinks.SelectedIndex = -1
                End If
                MostrarCmdNew(True)
            End With
        ElseIf e.CommandName = "Select" Then
            MostrarCmdNew(False)
            Me.btnAceptar.Enabled = True
            CtrRoomLink1.cleardata()
            CtrRoomLink1.Editar = True
            CtrRoomLink1.Sourcecode = e.Item.Cells(dgColumns.sourceDescription).Text
            CtrRoomLink1.Targetcode = e.Item.Cells(dgColumns.TargetCode).Text
            CtrRoomLink1.loadlink(CInt(e.Item.Cells(dgColumns.targetId).Text))
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        lblError.Visible = False
        CtrRoomLink1.cleardata()
        dgLinks.SelectedIndex = -1
        loadLinks()

        'MostarDivContenedor = False
        divContenedor.Style("display") = "none"
        MostrarCmdNew(True)
    End Sub

    Private Sub dgLinks_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgLinks.PageIndexChanged
        dgLinks.CurrentPageIndex = e.NewPageIndex
        dgLinks.SelectedIndex = -1
        CtrRoomLink1.cleardata()
        loadLinks()
    End Sub

    Private Sub dgLinks_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgLinks.ItemCreated
        If e.Item.ItemType = ListItemType.Pager Then
            If Me.dgLinks.CurrentPageIndex > 0 Then
                Dim prev As New System.Web.UI.WebControls.LinkButton
                prev.CommandArgument = "Prev"
                prev.CommandName = "Page"
                prev.Text = "<&nbsp;" & PortalCulture.GetString("00010")
                prev.CausesValidation = False
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(0, New System.Web.UI.LiteralControl("&nbsp;"))
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(0, prev)
            End If
            If Me.dgLinks.CurrentPageIndex < Me.dgLinks.PageCount - 1 Then
                Dim _next As New System.Web.UI.WebControls.LinkButton
                _next.CommandArgument = "Next"
                _next.CommandName = "Page"
                _next.Text = PortalCulture.GetString("00011") & "&nbsp;>"
                _next.CausesValidation = False

                CType(e.Item.Controls(0), TableCell).Controls.AddAt(CType(e.Item.Controls(0), TableCell).Controls.Count, New System.Web.UI.LiteralControl("&nbsp;"))
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(CType(e.Item.Controls(0), TableCell).Controls.Count, _next)
            End If
        End If
    End Sub


#Region "Mostrar Oculatar Formulario"


    'Protected Property MostarDivContenedor() As Boolean
    '    Get
    '        If (ViewState(String.Format("{0}MostarDivContenedor", Me.ClientID)) Is Nothing) Then
    '            Return False
    '        End If
    '        Return CType(ViewState(String.Format("{0}MostarDivContenedor", Me.ClientID)), Boolean)
    '    End Get
    '    Set(ByVal value As Boolean)

    '        ViewState(String.Format("{0}MostarDivContenedor", Me.ClientID)) = value
    '    End Set
    'End Property


    'Protected Sub btnMostarDivContenedor_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnMostarDivContenedor.Click
    '    MostarDivContenedor = True
    '    divContenedor.Style("display") = ""
    '    btnMostarDivContenedor.Visible = Not MostarDivContenedor
    '    'btnNuevo_Click(sender, e)
    'End Sub
    Private Sub PrepararParaEditar()
        'MostarDivContenedor = True
        divContenedor.Style("display") = ""
        ' btnMostarDivContenedor.Visible = Not MostarDivContenedor
    End Sub

    'Protected Sub btnOcultarDivContenedor_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnOcultarDivContenedor.Click
    '    MostarDivContenedor = False
    '    divContenedor.Style("display") = "none"
    '    btnMostarDivContenedor.Visible = Not MostarDivContenedor
    'End Sub
#End Region
End Class
