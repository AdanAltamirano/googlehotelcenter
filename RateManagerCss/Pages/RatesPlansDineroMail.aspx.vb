Imports Portal.Hotel.Common.Data
Imports Portal.Hotel.Facade
Imports Portal.General.Common.Data
Imports Portal.General.DataAccess
Imports Portal.General.Facade


Partial Public Class RatesPlansDineroMail
    Inherits PaginaBase
    Private msError As String = ""

    Enum dgcolumns
        idrateplan
        code
        name
        edit
        codigotarifa
    End Enum

    Private Sub ShowError()
        Me.lblError.Visible = (msError.Trim.Length > 0)
        Me.lblError.Text = msError
    End Sub

    Property Editando() As Boolean
        Get
            Return ViewState("Editando")
        End Get
        Set(ByVal Value As Boolean)
            ViewState("Editando") = Value
        End Set
    End Property

    Sub loadResources()
        lblMsg.Text = PortalCulture.GetString("01274")
        If Editando Then
            'lblTitle.Text = String.Format("{0} {1}", PortalCulture.GetString("01250"), PortalCulture.GetString("01274"))
        Else
            lblTitle.Text = String.Format("{0} {1}", PortalCulture.GetString("00102"), PortalCulture.GetString("01274"))
        End If
        lblContPortal.Text = PortalCulture.GetString("01252")

        dgRatePlans.PagerStyle.PrevPageText = "<< " & PortalCulture.GetString("00010")
        dgRatePlans.PagerStyle.NextPageText = PortalCulture.GetString("00011") & " >>"
        dgRatePlans.Columns(dgcolumns.name).HeaderText = PortalCulture.GetString("00073")
        dgRatePlans.Columns(dgcolumns.code).HeaderText = PortalCulture.GetString("00001")
        cmdEliminar.Text = PortalCulture.GetString("00103")
        btnSave.Text = PortalCulture.GetString("M000060")
        btncancel.Text = PortalCulture.GetString("M000143")
    End Sub

    Private Sub loadrateplans()
        'Dim ds As RatePlanData

        'With New RatePlanFacade
        '    If MyBase.IsSupervisor Then
        '        'Mostramos todos los rateplans incluidos los de tarifas netas.
        '        ds = .GetRatePlanByIdHotel(MyBase.cInfoActual.Hotel, PortalCulture.GetIDCulture, 1, 1)
        '    Else
        '        'Mostramos solamente los ratesplans que no sean de tarifas netas.
        '        ds = .GetRatePlanByIdHotel(MyBase.cInfoActual.Hotel, PortalCulture.GetIDCulture, 1, 0)
        '    End If

        'End With

        'Dim dr As DataRow

        'For Each dr In ds.Tables(RatePlanData.RATEPLAN_TABLE).Rows            
        '    If dr(RatePlanData.FIELD_SEGMENT) = "I" Or dr.IsNull(RatePlanData.FIELD_IDCONTRATO) Then
        '        dr.Delete()
        '    End If
        'Next

        'ds.AcceptChanges()

        'With dgRatePlans
        '    'dsegmentos se utilizará en el databound            
        '    .DataKeyField = RatePlanData.FIELD_IDRATEPLAN
        '    .DataSource = ds
        '    .DataBind()
        'End With

        Try
            Dim dsa As AgencyRatesData

            dsa = (New AgencyRatesFacade).GetAgencyRatesByIdHotel(MyBase.cInfoActual.Hotel)
            With dgRatePlans
                .DataKeyField = AgencyRatesData.FIELD_rateCode
                .DataSource = dsa
                .DataBind()
            End With

        Catch ex As Exception
            Dim mensaje As String = ex.Message
        End Try

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)
        If Not IsPostBack Then
            loadrateplans()
            Editando = False
            pnlData.Style.Add("display", "none")
        End If
        'cmdNew.Attributes.Add("onclick", String.Format("javascript:FireShow('{0}','{1}',{2});", pnlData.ClientID, cmdNew.ClientID, "true"))
        Me.cmdEliminar.OnClientClick = String.Format("return confirm('{0}');", PortalCulture.GetString("01273"))
        'Me.ResizefrmPrincipal()
    End Sub

    Function LoadRatePlan(ByVal id As String) As Boolean
        Dim dsRatePlan As RatePlanData
        Dim dato As String = String.Empty
        Dim idDicc As Integer
        Dim isvalid As Boolean = False
        Dim idHotel As Integer
        Dim hr As Boolean = False

        idHotel = CType(Me.Page, PaginaBase).cInfoActual.Hotel
        With New RatePlanFacade
            dsRatePlan = .GetDataRatePlan(id, idHotel)
        End With
        If dsRatePlan.Tables(RatePlanData.RATEPLAN_TABLE).Rows.Count > 0 Then
            With dsRatePlan.Tables(RatePlanData.RATEPLAN_TABLE).Rows(0)
                CtrRatePlanDineroMail1.ClearCtlrs()
                lblTitle.Text = String.Format("{0} - {1}", PortalCulture.GetString("00294"), .Item(RatePlanData.FIELD_NAME))
                hr = CtrRatePlanDineroMail1.LoadDineroMailRatePlan(idHotel, .Item(RatePlanData.FIELD_IDRATEPLAN))
                cmdEliminar.Enabled = If(hr, True, False)
            End With

            'If Not hr Then
            '    lblError.Visible = True
            '    lblError.Text = PortalCulture.GetString("01264")
            'End If
        End If
        Return True
    End Function

    Private Sub dgRatePlans_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgRatePlans.ItemCommand

        Me.lblError.Visible = False
        Me.lblErrorSource.Visible = False
        If e.CommandName = "Select" Then
            LoadRatePlan(dgRatePlans.DataKeys(e.Item.ItemIndex))
            Me.btnSave.Enabled = True
            Editando = True
            '            cmdNew.Style.Add("display", "none")
            pnlData.Style.Add("display", "block")
        End If
    End Sub

    Private Sub dgRatePlans_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgRatePlans.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.SelectedItem Or e.Item.ItemType = ListItemType.Item Then
            
            Dim LK2 As LinkButton
            LK2 = e.Item.Cells(dgcolumns.edit).FindControl("lnkedit")
            If (Not LK2 Is Nothing) Then
                LK2.Text = PortalCulture.GetString("00093")
                LK2 = e.Item.Cells(dgcolumns.edit).FindControl("lnkEliminar2")
            End If
        End If
        If e.Item.ItemType = ListItemType.Header Then
            e.Item.Cells(dgcolumns.name).Text = PortalCulture.GetString("00073")
            e.Item.Cells(dgcolumns.code).Text = PortalCulture.GetString("00001")
        ElseIf e.Item.ItemType = ListItemType.Footer Then
            e.Item.Cells(dgcolumns.edit).Text = CType(dgRatePlans.DataSource, DataSet).Tables(AgencyRatesData.AgencyRates_TABLE).Rows.Count & " " & PortalCulture.GetString("00047")
        End If
    End Sub

    Private Sub dgRatePlans_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgRatePlans.ItemCreated
        If e.Item.ItemType = ListItemType.Pager Then
            If dgRatePlans.CurrentPageIndex > 0 Then
                Dim prev As New System.Web.UI.WebControls.LinkButton
                prev.CommandArgument = "Prev"
                prev.CommandName = "Page"
                prev.Text = "<&nbsp;" & PortalCulture.GetString("00010")
                prev.CausesValidation = False
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(0, New System.Web.UI.LiteralControl("&nbsp;"))
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(0, prev)
            End If
            If dgRatePlans.CurrentPageIndex < dgRatePlans.PageCount - 1 Then
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

    Private Sub dgRatePlans_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRatePlans.PageIndexChanged
        Me.dgRatePlans.CurrentPageIndex = e.NewPageIndex
        Me.dgRatePlans.SelectedIndex = -1
        loadrateplans()
    End Sub

    Private Sub RatePlanContentPortal_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        loadResources()
        ShowError()
    End Sub

    Private Sub btncancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncancel.Click
        CtrRatePlanDineroMail1.ClearCtlrs()
        dgRatePlans.SelectedIndex = -1
        loadrateplans()
        Editando = False
        'cmdNew.Style.Add("display", "block")
        pnlData.Style.Add("display", "none")
    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim hr As Boolean
        hr = CtrRatePlanDineroMail1.SaveRatePlanDineroMail()
        If Not hr Then
            Me.msError = PortalCulture.GetString("00844")
        Else
            Me.dgRatePlans.SelectedIndex = -1
        End If
        Editando = False
        'cmdNew.Style.Add("display", "block")
        pnlData.Style.Add("display", "none")

    End Sub

    Protected Sub cmdEliminar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdEliminar.Click
        Dim hr As Boolean
        hr = CtrRatePlanDineroMail1.DeleteRatesPlanPaymentMode()
        If Not hr Then
            Me.msError = PortalCulture.GetString("01275")
        End If
        Editando = False
    End Sub

End Class