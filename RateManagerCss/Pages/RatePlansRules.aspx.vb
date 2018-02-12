Imports Portal.General.Facade
Imports Portal.General.Common.Data

Partial Class RatePlansRules
    Inherits PaginaBase
    Protected CtrRatePlanRules1 As ctrRatePlanRules
    Protected WithEvents CtlMensajes1 As ctlMensajes
    Private Enum dgcolumns
        Code
        Description
        GuarDep
        ReqVerification
        editar
        Eliminar
        ruledescription
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

    Sub MostrarCmdNew(ByVal show As Boolean)
        cmdNew.Style.Add("display", IIf(show, "block", "none"))
        divContenedor.Style.Add("display", IIf(show, "none", "block"))
    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)
        If Not IsPostBack Then
            loadrules()
            CtrRatePlanRules1.edicion = False
            CtrRatePlanRules1.m_iHotelId = cInfoActual.Hotel
            lblError.Visible = False
            lblErrorDelete.Visible = False
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowInfo", "ShowNewInfo(0);", True)

            MostrarCmdNew(True)
        End If
        cmdNew.Attributes.Add("onclick", String.Format("javascript:FireShow('{0}','{1}',{2});", divContenedor.ClientID, cmdNew.ClientID, "true"))
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender

        If CtrRatePlanRules1.edicion = False Then
            CType(Me.Page, PaginaBase).Habilitaboton(permisos.RatePlanRules, Me.btnAceptar, "A")
        End If
        Dim LK As LinkButton
        Dim hpl As HyperLink
        For Each i As DataGridItem In Me.grid.Items
            If i.ItemType = ListItemType.AlternatingItem Or i.ItemType = ListItemType.Item Then
                hpl = i.Cells(Me.dgcolumns.Eliminar).FindControl("lnkEliminar")
                CType(Me.Page, PaginaBase).Habilitaboton(permisos.RatePlanRules, hpl, "D")
                LK = i.Cells(Me.dgcolumns.Description).FindControl("lnkEdit")
                CType(Me.Page, PaginaBase).Habilitaboton(permisos.RatePlanRules, LK, "M")
            End If
        Next
        loadResources()
    End Sub
    Private Sub loadResources()
        cmdNew.Value = PortalCulture.GetString("00102")
        Me.btnAceptar.Text = PortalCulture.GetString("00008")
        Me.btnCancel.Text = PortalCulture.GetString("00009")
        grid.Columns(dgcolumns.Code).HeaderText = PortalCulture.GetString("00001")
        grid.Columns(dgcolumns.Description).HeaderText = PortalCulture.GetString("00002")
        grid.Columns(dgcolumns.ReqVerification).HeaderText = PortalCulture.GetString("00297")
        grid.PagerStyle.PrevPageText = "<< " & PortalCulture.GetString("00010")
        grid.PagerStyle.NextPageText = PortalCulture.GetString("00011") & " >>"
        lblError.Text = PortalCulture.GetString("00130")
        lblErrorDelete.Text = PortalCulture.GetString("00289")
        Me.lbltitle.Text = PortalCulture.GetString("00456")
        lblCtrlTitle.Text = PortalCulture.GetString("00024")
        If Me.CtrRatePlanRules1.edicion = True Then
            lblCtrlTitle.Text = PortalCulture.GetString("00025")
        End If
    End Sub

    Private Sub loadrules()
        Dim Rules As New RatesPlanRulesData
        With New RatesPlanRulesFacade
            Rules = .getList(Me.cInfoActual.Hotel)
        End With
        Me.grid.DataKeyField = RatesPlanRulesData.FIELD_IDRULE
        Me.grid.DataSource = Rules
        Me.grid.DataBind()
        Dim ds As RatePlanData
        Me.btnAceptar.Enabled = True
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        lblError.Visible = False
        lblErrorDelete.Visible = False
        grid.SelectedIndex = -1
        CtrRatePlanRules1.clearData()
        loadrules()
        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowInfo", "ShowNewInfo(0);", True)
        MostrarCmdNew(True)
    End Sub

    'Private Sub btnCancel2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
    '    lblError.Visible = False
    '    lblErrorDelete.Visible = False
    '    grid.SelectedIndex = -1
    '    CtrRatePlanRules1.clearData()
    '    loadrules()
    '    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowInfo", "ShowNewInfo(1);", True)
    'End Sub

    Private Sub dgRatesPlans_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grid.ItemCommand

        If e.CommandName = "Select" Then
            CtrRatePlanRules1.loadRule(grid.DataKeys(e.Item.ItemIndex))
            CtrRatePlanRules1.edicion = True
            Me.btnAceptar.Enabled = True
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowInfo", "ShowNewInfo(1);", True)
            MostrarCmdNew(False)
        ElseIf e.CommandName = "Eliminar" Then
            With New RatesPlanRulesFacade
                If .Delete(grid.DataKeys(e.Item.ItemIndex)) Then
                    Me.guardalog("/Pages/RatePlansRules.aspx", PaginaBase.acciones.Eliminar, "Eliminó la regla " & Me.grid.Items(e.Item.ItemIndex).Cells(dgcolumns.ruledescription).Text)
                    CtrRatePlanRules1.clearData()
                    If grid.CurrentPageIndex > 0 And grid.Items.Count = 1 Then
                        grid.CurrentPageIndex = ((grid.CurrentPageIndex * grid.PageSize) \ grid.PageSize) - 1
                    End If
                    loadrules()
                    grid.SelectedIndex = -1
                    MostrarCmdNew(True)
                Else
                    lblErrorDelete.Visible = True
                End If
            End With
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowInfo", "ShowNewInfo(0);", True)
        End If
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click, btnPublicar.Click
        If Not Page.IsValid Then Return

        Dim publish As Boolean = (CType(sender, Button).ID = Me.btnPublicar.ID)

        lblError.Visible = False
        If CtrRatePlanRules1.validaHours() Then
            If CtrRatePlanRules1.SaveRules(publish) Then
                grid.SelectedIndex = -1
                CtrRatePlanRules1.clearData()
                loadrules()
                'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowInfo", "ShowNewInfo(0);", True)

                MostrarCmdNew(True)
            Else
                lblError.Visible = True
                'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowInfo", "ShowNewInfo(1);", True)
            End If
        Else
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowInfo", "ShowNewInfo(1);", True)
        End If
    End Sub

    Private Sub dgRatesPlans_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles grid.PageIndexChanged
        grid.CurrentPageIndex = e.NewPageIndex
        grid.SelectedIndex = -1
        loadrules()
        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowInfo", "ShowNewInfo(0);", True)
    End Sub

    Private Sub dgRatesPlans_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grid.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.SelectedItem Or e.Item.ItemType = ListItemType.Item Then
            Dim LK As LinkButton


            LK = e.Item.Cells(Me.dgcolumns.editar).FindControl("lnkEdit")
            LK.Text = PortalCulture.GetString("00093")
            LK = e.Item.Cells(Me.dgcolumns.Description).FindControl("Lnkruledescription")

            LK = e.Item.Cells(Me.dgcolumns.Eliminar).FindControl("lnkEliminar2")
            Dim hpl As HyperLink
            hpl = e.Item.Cells(Me.dgcolumns.Eliminar).FindControl("lnkEliminar")
            hpl.Text = PortalCulture.GetString("00103")
            hpl.NavigateUrl = CtlMensajes1.getShow(LK.ClientID, PortalCulture.GetString("00456"), PortalCulture.GetString("00463"))
            If e.Item.Cells(dgcolumns.ReqVerification).Text.ToUpper = "TRUE" Then
                e.Item.Cells(dgcolumns.ReqVerification).Text = PortalCulture.GetString("00030")
            Else
                e.Item.Cells(dgcolumns.ReqVerification).Text = PortalCulture.GetString("00031")
            End If
            If e.Item.Cells(dgcolumns.GuarDep).Text.ToUpper = "G" Then
                e.Item.Cells(dgcolumns.GuarDep).Text = PortalCulture.GetString("00028")
            ElseIf e.Item.Cells(dgcolumns.GuarDep).Text.ToUpper = "D" Then
                e.Item.Cells(dgcolumns.GuarDep).Text = PortalCulture.GetString("00029")
            Else
                e.Item.Cells(dgcolumns.GuarDep).Text = PortalCulture.GetString("00027")
            End If
        ElseIf e.Item.ItemType = ListItemType.Footer Then
            e.Item.Cells(dgcolumns.Eliminar).Text = CType(grid.DataSource, RatesPlanRulesData).Tables(RatesPlanRulesData.TABLE_RATEPLANRULES).Rows.Count & " " & PortalCulture.GetString("00015")
        ElseIf e.Item.ItemType = ListItemType.Header Then
            grid.Columns(dgcolumns.Code).HeaderText = PortalCulture.GetString("00001")
            grid.Columns(dgcolumns.Description).HeaderText = PortalCulture.GetString("00002")
            grid.Columns(dgcolumns.ReqVerification).HeaderText = PortalCulture.GetString("00297")
        End If
    End Sub

    Private Sub dgRatesPlans_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grid.ItemCreated
        If e.Item.ItemType = ListItemType.Pager Then
            If grid.CurrentPageIndex > 0 Then
                Dim prev As New System.Web.UI.WebControls.LinkButton
                prev.CommandArgument = "Prev"
                prev.CommandName = "Page"
                prev.Text = "<&nbsp;" & PortalCulture.GetString("00010")
                prev.CausesValidation = False
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(0, New System.Web.UI.LiteralControl("&nbsp;"))
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(0, prev)
            End If
            If grid.CurrentPageIndex < grid.PageCount - 1 Then
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

