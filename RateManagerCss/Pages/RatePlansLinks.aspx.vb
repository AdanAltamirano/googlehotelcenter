Imports Portal.General.Facade
Imports Portal.General.Common.Data
Partial Class RatePlansLinks
    Inherits PaginaBase
    Protected CtrRatePlanLink1 As ctrRatePlanLink
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
        TargetName
        sourceName
    End Enum

    Sub MostrarCmdNew(ByVal show As Boolean)
        cmdNew.Style.Add("display", IIf(show, "block", "none"))
        divContenedor.Style.Add("display", IIf(show, "none", "block"))
    End Sub

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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)
        Me.lblError.Visible = False
        If Not IsPostBack Then
            CtrRatePlanLink1.m_iHotelId = cInfoActual.Hotel
            loadLinks()
            CtrRatePlanLink1.Editar = False
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowInfo", "ShowNewInfo(0);", True)
            MostrarCmdNew(True)
        End If
        cmdNew.Attributes.Add("onclick", String.Format("javascript:FireShow('{0}','{1}',{2});", divContenedor.ClientID, cmdNew.ClientID, "true"))
        Me.ResizefrmPrincipal()
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        lblctrlTitle.Text = PortalCulture.GetString("00032")
        If Me.CtrRatePlanLink1.Editar = True Then
            lblctrlTitle.Text = PortalCulture.GetString("00033")
        End If
        cmdNew.Value = PortalCulture.GetString("00102")
        Me.lblTitle.Text = PortalCulture.GetString("00043")
        Me.btnAceptar.Text = PortalCulture.GetString("00008")
        Me.btnCancel.Text = PortalCulture.GetString("00009")
        Me.dglinks.PagerStyle.PrevPageText = "<< " & PortalCulture.GetString("00010")
        Me.dglinks.PagerStyle.NextPageText = PortalCulture.GetString("00011") & " >>"
        dglinks.Columns(dgColumns.offset).HeaderText = PortalCulture.GetString("00422")
        dglinks.Columns(dgColumns.ratio).HeaderText = PortalCulture.GetString("00421")
        dglinks.Columns(dgColumns.roundamount).HeaderText = PortalCulture.GetString("00042")
        dglinks.Columns(dgColumns.sourceDescription).HeaderText = PortalCulture.GetString("00035")
        dglinks.Columns(dgColumns.targetDescription).HeaderText = PortalCulture.GetString("00034")
        If CtrRatePlanLink1.Editar = False Then
            CType(Me.Page, PaginaBase).Habilitaboton(permisos.RatePlanLinks, Me.btnAceptar, "A")
        End If
        Dim LK As LinkButton
        Dim hpl As HyperLink
        For Each i As DataGridItem In Me.dglinks.Items
            If i.ItemType = ListItemType.AlternatingItem Or i.ItemType = ListItemType.Item Then
                hpl = i.Cells(Me.dgColumns.delete).FindControl("lnkEliminar")
                hpl.Attributes.Add("onmousedown", "DesabilidaValidadores();")
                CType(Me.Page, PaginaBase).Habilitaboton(permisos.RatePlanLinks, hpl, "D")
                LK = i.Cells(Me.dgColumns.targetDescription).FindControl("lnkEdit")
                CType(Me.Page, PaginaBase).Habilitaboton(permisos.RatePlanLinks, LK, "M")
            End If
        Next
    End Sub

    Private Sub loadLinks()
        Dim links As New LinkRatePlanData
        Dim idAsoc As Integer = Me.GetIdAsociation

        With New LinkRatePlanFacade
            links = .getList(Me.cInfoActual.Hotel, PortalCulture.GetIDCulture, idAsociacion:=idAsoc)
        End With
        dglinks.DataKeyField = links.FIELD_TargetRatePlan
        dglinks.DataSource = links
        dglinks.DataBind()
        Dim ds As RatePlanData

        With New RatePlanFacade
            ds = .GetRatePlanByIdHotel(MyBase.cInfoActual.Hotel, PortalCulture.GetIDCulture, incluirNetRatesPlan:=1, idAsociacion:=idAsoc, DeleteFilter:=1)
        End With
        ds.Tables(ds.RATEPLAN_TABLE).Columns.Add("texto", GetType(System.String), "substring(" & ds.FIELD_CODIGOTARIFA & "+' - '+" & ds.FIELD_NAME & ",1,30)")
        ''eliminar los ratesplan que ya tienen links
        Dim dv As DataView
        For Each r As DataRow In ds.Tables(ds.RATEPLAN_TABLE).Rows
            dv = links.Tables(links.TABLE_LINKRATEPLAN).DefaultView
            dv.RowFilter = links.FIELD_TargetRatePlan & "='" & r(ds.FIELD_IDRATEPLAN) & "'"
            If dv.Count > 0 OrElse r(ds.FIELD_SEGMENT) = "K" Then
                r.Delete()
            End If
        Next
        ds.Tables(ds.RATEPLAN_TABLE).AcceptChanges()
        CtrRatePlanLink1.SourceDataSet(ds)
        For Each r As DataRow In ds.Tables(ds.RATEPLAN_TABLE).Rows
            dv = links.Tables(links.TABLE_LINKRATEPLAN).DefaultView
            dv.RowFilter = links.FIELD_SourceRatePlan & "='" & r(ds.FIELD_IDRATEPLAN) & "'"
            If dv.Count > 0 OrElse r(ds.FIELD_SEGMENT) = "K" Then
                r.Delete()
            End If
        Next
        ds.Tables(ds.RATEPLAN_TABLE).AcceptChanges()

        'hay ke eliminar tambien la tarifa rack ya ke esta no puede linkearse a otra
        For Each r As DataRow In ds.Tables(ds.RATEPLAN_TABLE).Rows
            If r(ds.FIELD_SEGMENT).ToString.ToUpper = "R" AndAlso Not r(ds.FIELD_SegmentRacPrinc) Is System.DBNull.Value AndAlso r(ds.FIELD_SegmentRacPrinc) = True Then
                r.Delete()
            End If
        Next
        ds.Tables(ds.RATEPLAN_TABLE).AcceptChanges()
        CtrRatePlanLink1.TargetDataSet(ds)
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        If Not Page.IsValid Then Return
        Dim isNetRateDif As Boolean = False

        If CtrRatePlanLink1.Editar OrElse Not CtrRatePlanLink1.samelink Then

            If Not CtrRatePlanLink1.Editar Then
                If CtrRatePlanLink1.RatePlanTargetHasRates() Then
                    lblError.Text = PortalCulture.GetString("01674")
                    lblError.Visible = True
                Else
                    If CtrRatePlanLink1.SaveLinks(isNetRateDif) Then
                        CtrRatePlanLink1.cleardata()
                        dglinks.SelectedIndex = -1
                        loadLinks()
                        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowInfo", "ShowNewInfo(0);", True)

                        MostrarCmdNew(True)
                    Else
                        If isNetRateDif Then
                            lblError.Text = PortalCulture.GetString("01263")
                        Else
                            lblError.Text = PortalCulture.GetString("01174")
                        End If

                        Me.lblError.Visible = True
                        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowInfo", "ShowNewInfo(1);", True)
                    End If
                End If

            Else
                If CtrRatePlanLink1.SaveLinks(isNetRateDif) Then
                    CtrRatePlanLink1.cleardata()
                    dglinks.SelectedIndex = -1
                    loadLinks()
                    'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowInfo", "ShowNewInfo(0);", True)

                    MostrarCmdNew(True)
                Else
                    If isNetRateDif Then
                        lblError.Text = PortalCulture.GetString("01263")
                    Else
                        lblError.Text = PortalCulture.GetString("01174")
                    End If

                    Me.lblError.Visible = True
                    'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowInfo", "ShowNewInfo(1);", True)
                End If
            End If


            'If CtrRatePlanLink1.SaveLinks(isNetRateDif) Then
            '    CtrRatePlanLink1.cleardata()
            '    dglinks.SelectedIndex = -1
            '    loadLinks()
            '    'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowInfo", "ShowNewInfo(0);", True)

            '    MostrarCmdNew(True)
            'Else
            '    If isNetRateDif Then
            '        lblError.Text = PortalCulture.GetString("01263")
            '    Else
            '        lblError.Text = PortalCulture.GetString("01174")
            '    End If

            '    Me.lblError.Visible = True
            '    'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowInfo", "ShowNewInfo(1);", True)
            'End If

        Else
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowInfo", "ShowNewInfo(1);", True)
        End If
    End Sub

    Private Sub dgLinks_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dglinks.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.SelectedItem Then
            If e.Item.Cells(dgColumns.TargetName).Text <> "&nbsp;" Then
                e.Item.Cells(dgColumns.targetDescription).Text &= " - " & e.Item.Cells(dgColumns.TargetName).Text
            End If
            If e.Item.Cells(dgColumns.sourceName).Text <> "&nbsp;" Then
                e.Item.Cells(dgColumns.sourceDescription).Text &= " - " & e.Item.Cells(dgColumns.sourceName).Text
            End If
            If e.Item.Cells(dgColumns.targetId).Text = "&nbsp;" Then
                e.Item.Cells(dgColumns.targetId).Text = ""
            End If
            If e.Item.Cells(dgColumns.sourceId).Text = "&nbsp;" Then
                e.Item.Cells(dgColumns.sourceId).Text = ""
            End If
            Dim LK As LinkButton
            LK = e.Item.Cells(Me.dgColumns.edit).FindControl("lnkEdit")
            LK.Text = PortalCulture.GetString("00093")
            LK = e.Item.Cells(Me.dgColumns.delete).FindControl("lnkEliminar2")

            Dim hpl As HyperLink = e.Item.Cells(Me.dgColumns.delete).FindControl("lnkEliminar")
            hpl.Text = PortalCulture.GetString("00103")
            hpl.NavigateUrl = CtlMensajes1.getShow(LK.ClientID, PortalCulture.GetString("00043"), PortalCulture.GetString("00465"))

        End If
        If e.Item.ItemType = ListItemType.Header Then
            With e.Item
                .Cells(dgColumns.offset).Text = PortalCulture.GetString("00422")
                .Cells(dgColumns.ratio).Text = PortalCulture.GetString("00421")
                .Cells(dgColumns.roundamount).Text = PortalCulture.GetString("00042")
                .Cells(dgColumns.sourceDescription).Text = PortalCulture.GetString("00035")
                .Cells(dgColumns.targetDescription).Text = PortalCulture.GetString("00034")
            End With
        ElseIf e.Item.ItemType = ListItemType.Footer Then
            e.Item.Cells(dgColumns.delete).Text = CType(dglinks.DataSource, LinkRatePlanData).Tables(LinkRatePlanData.TABLE_LINKRATEPLAN).Rows.Count & " " & PortalCulture.GetString("00475")
        End If
    End Sub

    Private Sub dgLinks_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dglinks.ItemCommand

        If e.CommandName = "Eliminar" Then
            With New LinkRatePlanFacade
                If .Delete(Me.cInfoActual.Hotel, dglinks.Items(e.Item.ItemIndex).Cells(dgColumns.targetId).Text, dglinks.Items(e.Item.ItemIndex).Cells(dgColumns.sourceId).Text) Then
                    Me.guardalog("/Pages/RatePlansLinks.aspx", PaginaBase.acciones.Eliminar, "Se eliminó el linkeo de " & dglinks.Items(e.Item.ItemIndex).Cells(dgColumns.TargetCode).Text & " con " & dglinks.Items(e.Item.ItemIndex).Cells(dgColumns.sourceDescription).Text)
                    CtrRatePlanLink1.cleardata()
                    If dglinks.CurrentPageIndex > 0 And dglinks.Items.Count = 1 Then
                        dglinks.CurrentPageIndex = ((dglinks.CurrentPageIndex * dglinks.PageSize) \ dglinks.PageSize) - 1
                    End If
                    loadLinks()
                    dglinks.SelectedIndex = -1
                End If
                MostrarCmdNew(True)
            End With
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowInfo", "ShowNewInfo(0);", True)
        ElseIf e.CommandName = "Select" Then
            Me.btnAceptar.Enabled = True
            CtrRatePlanLink1.cleardata()
            CtrRatePlanLink1.Editar = True
            CtrRatePlanLink1.Target = e.Item.Cells(dgColumns.targetId).Text
            CtrRatePlanLink1.TargetNombre = e.Item.Cells(dgColumns.targetDescription).Text
            CtrRatePlanLink1.Sourcecode = e.Item.Cells(dgColumns.sourceDescription).Text
            CtrRatePlanLink1.Targetcode = e.Item.Cells(dgColumns.TargetCode).Text
            CtrRatePlanLink1.loadlink(dglinks.DataKeys(e.Item.ItemIndex))
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowInfo", "ShowNewInfo(1);", True)
            MostrarCmdNew(False)
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        CtrRatePlanLink1.cleardata()
        dglinks.SelectedIndex = -1
        loadLinks()
        '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowInfo", "ShowNewInfo(0);", True)
        MostrarCmdNew(True)
    End Sub
    'Private Sub btnCancel2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew.Click
    '    CtrRatePlanLink1.cleardata()
    '    dglinks.SelectedIndex = -1
    '    loadLinks()
    '    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowInfo", "ShowNewInfo(1);", True)
    '    btnNew.Visible = False
    'End Sub

    Private Sub dgLinks_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dglinks.PageIndexChanged
        dglinks.CurrentPageIndex = e.NewPageIndex
        dglinks.SelectedIndex = -1
        loadLinks()        
        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowInfo", "ShowNewInfo(0);", True)
    End Sub

    Private Sub dgLinks_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dglinks.ItemCreated
        If e.Item.ItemType = ListItemType.Pager Then
            If dglinks.CurrentPageIndex > 0 Then
                Dim prev As New System.Web.UI.WebControls.LinkButton
                prev.CommandArgument = "Prev"
                prev.CommandName = "Page"
                prev.Text = "<&nbsp;" & PortalCulture.GetString("00010")
                prev.CausesValidation = False
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(0, New System.Web.UI.LiteralControl("&nbsp;"))
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(0, prev)
            End If
            If dglinks.CurrentPageIndex < dglinks.PageCount - 1 Then
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
End Class
