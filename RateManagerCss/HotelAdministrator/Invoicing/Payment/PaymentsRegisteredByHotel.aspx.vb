Imports Oz.UniBilling.Common
Imports Oz.UniBilling.Common.Hotels
Imports Oz.UniBilling.Hotels.Business
Imports Oz.UniBilling.DataSchemas.Hotels

Partial Class PaymentsRegisteredByHotel
    Inherits PaginaBase

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

#Region " Properties.. "

    Private Property CompanyID() As Integer
        Get
            Return ViewState("CompanyID")
        End Get
        Set(ByVal Value As Integer)
            ViewState("CompanyID") = Value
        End Set
    End Property

#End Region

    Private dsPayment As New PaymentDataSet
    Private dsBilling As New BillingStatementDataSet
    Private HotelIdentity As New HotelIdentifier
    Private All As String = "0"
    Private Period As String = "1"
    Dim countAccepted As Integer = 0

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)
        Me.HeaderPanel = ctrlHeader.Usuario.uHotelAdministrador

        CompanyID = MyBase.cInfoActual.Empresa
        HotelIdentity.CompanyID = CompanyID

        Dim HotelMng As New HotelManager
        HotelMng.SearchHotelByCompanyID(HotelIdentity.CompanyID, dsBilling)
        If dsBilling.Hotels.Count > 0 Then
            If Not dsBilling.Hotels(0).IsUniPantallaHotelIDNull Then HotelIdentity.UniPantallaID = dsBilling.Hotels(0).UniPantallaHotelID
            If Not dsBilling.Hotels(0).IsPortalHotelIDNull Then HotelIdentity.PortalID = dsBilling.Hotels(0).PortalHotelID
        End If

        If Not IsPostBack Then
            loadOptions()
        End If

        preparePage()

    End Sub

    Private Sub loadOptions()
        ddlShowBy.Items.Clear()
        ddlShowBy.Items.Add(New ListItem(PortalCulture.GetString("M0BT0000292"), All))
        ddlShowBy.Items.Add(New ListItem(PortalCulture.GetString("M0BT0000293"), Period))
    End Sub

    Private Sub preparePage()
        If ddlShowBy.SelectedValue = All Then
            trMonth.Style("DISPLAY") = "none"
            trYear.Style("DISPLAY") = "none"
        ElseIf ddlShowBy.SelectedValue = Period Then
            trMonth.Style("DISPLAY") = "block"
            trYear.Style("DISPLAY") = "block"
            IIf(trGridPayments.Style("DISPLAY") = "block", trGridPayments.Style("DISPLAY") = "none", trGridPayments.Style("DISPLAY") = "none")
            LoadMonths()
            LoadYears()
            If ddlMonth.SelectedIndex = -1 Then
                ddlMonth.SelectedIndex = Now.Month - 1
            End If
        End If
        If trGridPayments.Style("DISPLAY") = "block" Then trGridPayments.Style("DISPLAY") = "none"
        If ltlNote.Visible Then ltlNote.Visible = False

        If lblTotal.Visible Then lblTotal.Visible = False
        If lblTotalAccepted.Visible Then lblTotalAccepted.Visible = False
        If lblMsg.Visible Then lblMsg.Visible = False
    End Sub

    Private Sub LoadMonths()
        Dim idx As Integer = ddlMonth.SelectedIndex

        ddlMonth.Items.Clear()
        For m As Byte = 1 To 12
            ddlMonth.Items.Add(New ListItem(Formatting.GetMonthName(m, False, PortalCulture.GetCulture.Name), m.ToString()))
        Next

        ddlMonth.SelectedIndex = idx
    End Sub

    Private Sub LoadYears()
        Dim y, y1 As Integer
        y = DateTime.Now.Year
        y1 = 0
        If ddlYear.Items.Count > 0 Then
            y1 = ddlYear.SelectedIndex
        End If
        ddlYear.Items.Clear()
        ddlYear.Items.Add(y.ToString())
        ddlYear.Items.Add((y - 1).ToString())
        ddlYear.Items.Add((y - 2).ToString())
        If y1 <> 0 Then
            ddlYear.SelectedIndex = y1
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        lblTitle.Text = PortalCulture.GetString("M0BT0000291")
        lblShowBy.Text = PortalCulture.GetString("M0BT0000294", True)
        lblMonth.Text = PortalCulture.GetString("M0BT0000003", True)
        lblYear.Text = PortalCulture.GetString("M0BT0000004", True)
        btnShow.Text = PortalCulture.GetString("M0BT0000294")
        lblMsg.Text = PortalCulture.GetString("M0BT0000297")
        ltlNote.Text = PortalCulture.GetString("M0BT0000303")

        lblTotalAccepted.Text = IIf(countAccepted > 0, PortalCulture.GetString("M0BT0000300") & " (" & countAccepted & ") : ", PortalCulture.GetString("M0BT0000300", True))

        dgPayments.Columns(0).HeaderText = PortalCulture.GetString("M0BT0000210") 'banco
        dgPayments.Columns(1).HeaderText = PortalCulture.GetString("M0BT0000235") 'fecha reg
        dgPayments.Columns(2).HeaderText = PortalCulture.GetString("M0BT0000237") 'no trans
        dgPayments.Columns(3).HeaderText = PortalCulture.GetString("M0BT0000315") 'fecha dep
        dgPayments.Columns(4).HeaderText = PortalCulture.GetString("M0BT0000345") 'tipo de cambio respecto al dolar
        dgPayments.Columns(5).HeaderText = PortalCulture.GetString("M0BT0000295") 'Amount
        dgPayments.Columns(6).HeaderText = PortalCulture.GetString("M0BT0000296") 'estado

        hplGoToTaskList.Text = PortalCulture.GetString("M0BT0000148")
        hplGoToTaskList.NavigateUrl = String.Format("{0}MainInvoicing.aspx?cid={1}", GeRequestApplicationPath("/HotelAdministrator/Invoicing/"), CompanyID)
        Dim sFiltro As String
        sFiltro = PortalCulture.GetString("M0BT0000113") & ","
        If ddlMonth.Items.Count > 0 And ddlShowBy.SelectedIndex = 1 Then
            sFiltro &= ", " & PortalCulture.GetString("M000120") & " " & ddlMonth.SelectedItem.Text & "/" & Me.ddlYear.SelectedItem.Text
        End If
        sFiltro &= String.Format(", {0}", ddlShowBy.SelectedItem.Text)
       
        lblHelpFiltro.Text = sFiltro.Replace(",,", "").Replace(", ,", "")
        If Not Me.IsPostBack Then
            btnShow_Click(Me, Nothing)
        End If
    End Sub

    Private Sub btnShow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShow.Click
        Dim PeriodPayment As New MonthlyPeriod
        Dim PaymentMng As New PaymentManager
        dsPayment.Payments.Clear()

        If ddlShowBy.SelectedValue = All Then
            PaymentMng.GetPaymentInfoByHotelAndPeriod(HotelIdentity, MonthlyPeriod.MinValue, dsPayment)
        ElseIf ddlShowBy.SelectedValue = Period Then
            Dim month As Byte = ddlMonth.SelectedValue
            Dim year As Integer = ddlYear.SelectedValue
            PeriodPayment = New MonthlyPeriod(month, year)
            PaymentMng.GetPaymentInfoByHotelAndPeriod(HotelIdentity, PeriodPayment, dsPayment)
        End If

        If dsPayment.Payments.Count > 0 Then
            trGridPayments.Style("DISPLAY") = "block"
            ltlNote.Visible = True

            lblMsg.Visible = False
            lblTotalAccepted.Visible = True
            lblTotal.Visible = True
            dgPayments.DataSource = dsPayment
            dgPayments.DataMember = "Payments"
            dgPayments.DataBind()

            Dim rows() As PaymentDataSet.PaymentsRow
            rows = dsPayment.Payments.Select(" Verified = " & True)
            Dim total As Decimal = Decimal.Zero
            For i As Integer = 0 To rows.Length - 1
                total += rows(i).Amount
            Next
            countAccepted = rows.Length
            lblTotal.Text = FCurrency(total, 2) & " MXN"
        Else
            lblMsg.Visible = True
            trGridPayments.Style("DISPLAY") = "none"
            ltlNote.Visible = False

            lblTotalAccepted.Visible = False
            lblTotal.Visible = False
        End If

    End Sub

    Private Sub dgPayments_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgPayments.ItemDataBound
        Select Case e.Item.ItemType
            Case ListItemType.Item, ListItemType.AlternatingItem

                Dim dv As New DataView(dsPayment.BankAccounts)
                dv.RowFilter = "BankAccountID = " & DataBinder.Eval(e.Item.DataItem, "BankAccountID")

                Dim lbl As Label = e.Item.FindControl("lblAccountNumber")
                If dv.Count > 0 Then lbl.Text = dv(0).Item("AccountNumber") Else lbl.Text = "--"

                Dim dvB As New DataView(dsPayment.Banks)

                If dvB.Count > 0 Then dvB.RowFilter = "BankID = " & dv(0).Item("BankID") Else lbl.Text = ""

                lbl = e.Item.FindControl("lblBankName")
                If dvB.Count > 0 Then lbl.Text = dvB(0).Item("BankName") Else lbl.Text = "--"

                lbl = e.Item.FindControl("lblRegistrationDate")
                lbl.Text = Formatting.FormatDate(DataBinder.Eval(e.Item.DataItem, "RegistrationDate"), "dd/ MMMM/ yy")

                lbl = e.Item.FindControl("lblType")
                Select Case DataBinder.Eval(e.Item.DataItem, "TransactionType")
                    Case TransactionType.Deposit
                        lbl.Text = PortalCulture.GetString("M0BT0000178")
                    Case TransactionType.Transference
                        lbl.Text = PortalCulture.GetString("M0BT0000179")
                End Select
                lbl = e.Item.FindControl("lblReferenceBank")
                If Not DataBinder.Eval(e.Item.DataItem, "ReferenceBank") Is DBNull.Value Then
                    lbl.Text = DataBinder.Eval(e.Item.DataItem, "ReferenceBank")


                End If

                lbl = e.Item.FindControl("lblPaymentDate")
                lbl.Text = Formatting.FormatDate(DataBinder.Eval(e.Item.DataItem, "TransactionDate"), "dd/ MMMM/ yy")

                lbl = e.Item.FindControl("lblMoneyExchange")
                If DataBinder.Eval(e.Item.DataItem, "CurrencyCode").ToString.Trim = "USD" Then
                    lbl.Text = "--"
                Else
                    lbl.Text = FCurrency(DataBinder.Eval(e.Item.DataItem, "MoneyExchangeRate"), 2)
                End If

                lbl = e.Item.FindControl("lblAmount")

                lbl.Text = FCurrency(DataBinder.Eval(e.Item.DataItem, "Amount"), 2) & " " & DataBinder.Eval(e.Item.DataItem, "CurrencyCode")

                lbl = e.Item.FindControl("lblStatus")
                Select Case DataBinder.Eval(e.Item.DataItem, "Verified")
                    Case True
                        lbl.Text = PortalCulture.GetString("M0BT0000298")
                    Case False
                        lbl.Text = PortalCulture.GetString("M0BT0000299")
                End Select

        End Select
    End Sub

End Class
