Imports System.Configuration.ConfigurationManager
Imports System.Text
Imports Oz.UniBilling.DataSchemas.Hotels
Imports Oz.UniBilling.Hotels.Business
Imports Oz.UniBilling.Common.Hotels
Imports Oz.UniBilling.Common
Imports ReferencesSystem

Partial Class HotelInvoiceDetailViewer
    Inherits UserControlBase

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

    Public Property InvoiceID() As Integer
        Get
            If ViewState("InvoiceID") Is Nothing Then
                Return -1
            End If

            Return Convert.ToInt32(ViewState("InvoiceID"))
        End Get
        Set(ByVal Value As Integer)
            ViewState("InvoiceID") = Value
            LoadData()
        End Set
    End Property


    Private Property ReservationMoneyExchangeRateMxn() As Decimal
        Get
            Return ViewState("ReservationMoneyExchangeRateMxn")
        End Get
        Set(ByVal Value As Decimal)
            ViewState("ReservationMoneyExchangeRateMxn") = Value
        End Set
    End Property

    Private Property MoneyExRate() As Decimal
        Get
            Return ViewState("MoneyExRate")
        End Get
        Set(ByVal Value As Decimal)
            ViewState("MoneyExRate") = Value
        End Set
    End Property

    Private Property CurrentInvoiceDetailsView() As DataView
        Get
            Return CType(Session(Me.GetType().Name & "CurrentInvoiceDetailsView"), DataView)
        End Get
        Set(ByVal Value As DataView)
            Session(Me.GetType().Name & "CurrentInvoiceDetailsView") = Value
        End Set
    End Property

    Private Property CurrentCommissionDetailsView() As DataView
        Get
            Return CType(Session(Me.GetType().Name & "CurrentCommissionDetailsView"), DataView)
        End Get
        Set(ByVal Value As DataView)
            Session(Me.GetType().Name & "CurrentCommissionDetailsView") = Value
        End Set
    End Property

#End Region



    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        If Not IsPostBack Then
            CurrentInvoiceDetailsView = Nothing
            CurrentCommissionDetailsView = Nothing
        End If
        DataBind()
    End Sub

    Private Sub LoadData()
        Dim ds As New BillingStatementDataSet

        Try
            '*BillingManager.GetBillingStatementByID(InvoiceID, ds)
            Dim BillingMng As New BillingManager(Oz.UniBilling.Common.Security.cSecurity.GetIsser(1), AppSettings("UserID"), "ratemanager@univisit.com")
            BillingMng.GetBillingStatements(InvoiceID, ds)

        Catch ex As Exception
            ds = Nothing
        Finally
            DataSource = ds
        End Try
    End Sub

    Private Sub ToggleCommissionsRows(ByVal show As Boolean)
        trCommissionsExpander.Style("DISPLAY") = IIf(show, "block", "none")
        trCommissions.Style("DISPLAY") = IIf(show, "block", "none")
    End Sub

    Private Sub ToggleChargesRows(ByVal show As Boolean)
        trChargesExpander.Style("DISPLAY") = IIf(show, "block", "none")
        trCharges.Style("DISPLAY") = IIf(show, "block", "none")
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Dim script As String = String.Format("<script language=""javascript"" src=""{0}Script/Util.js""></script>", GeRequestApplicationPath("/HotelAdministrator/Invoicing/"))
        'Page.RegisterStartupScript("Util.js", script)
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "Util.js", script)

        imgToggleCharges.Attributes("onClick") = String.Format("javascript:expandOrCollapse('{0}', '{1}');", imgToggleCharges.ClientID, trCharges.ClientID)
        imgToggleCharges.Style("CURSOR") = "pointer"
        imgToggleCommissions.Attributes("onClick") = String.Format("javascript:expandOrCollapse('{0}', '{1}');", imgToggleCommissions.ClientID, trCommissions.ClientID)
        imgToggleCommissions.Style("CURSOR") = "pointer"

        lblInvoiceIDCaption.Text = PortalCulture.GetString("M0BT0000117", True)
        lblInvoiceNumberCaption.Text = PortalCulture.GetString("M0BT0000141", True)
        lblPeriodCaption.Text = PortalCulture.GetString("M0BT0000119", True)
        lblStatusCaption.Text = PortalCulture.GetString("M0BT0000120", True)
        lblCurrencyCodeCaption.Text = PortalCulture.GetString("M0BT0000171", True)
        lblMoneyExchangeCaption.Text = PortalCulture.GetString("M0BT0000030", True)
        lblRegistrationDateCaption.Text = PortalCulture.GetString("M0BT0000123", True)
        lblLimitToConciliateCaption.Text = PortalCulture.GetString("M0BT0000124", True)
        lblLimitToPayCaption.Text = PortalCulture.GetString("M0BT0000125", True)
        lblConciliationDateCaption.Text = PortalCulture.GetString("M0BT0000161", True)
        lblPaymentDateCaption.Text = PortalCulture.GetString("M0BT0000162", True)
        lblHotelNameCaption.Text = PortalCulture.GetString("M0BT0000091", True)
        lblStreetAddressCaption.Text = PortalCulture.GetString("M0BT0000163", True)
        lblPostalCodeCaption.Text = PortalCulture.GetString("M0BT0000164", True)
        lblZoneCaption.Text = PortalCulture.GetString("M0BT0000165", True)
        lblLocationCaption.Text = PortalCulture.GetString("M0BT0000166", True)

        lblChargesCaption.Text = PortalCulture.GetString("M0BT0000167")
        lblCommissionsCaption.Text = PortalCulture.GetString("M0BT0000168")

        lblAmountChargesCaption.Text = PortalCulture.GetString("M0BT0000167", True)
        lblAmountCommissionsCaption.Text = PortalCulture.GetString("M0BT0000168", True)

        lblSubtotalCaption.Text = PortalCulture.GetString("M0BT0000129", True)
        lblTaxesCaption.Text = PortalCulture.GetString("M0BT0000130", True)
        lblGrandTotalCaption.Text = PortalCulture.GetString("M0BT0000131", True)
        lblBalanceCaption.Text = PortalCulture.GetString("M0BT0000250", True)
        lblReferenceBankCapion.Text = PortalCulture.GetString("00793", True)
    End Sub

    Public Function FormatPeriod(ByVal month As Integer, ByVal year As Integer) As String
        Dim fecha As New DateTime(year, month, 1)
        Return fecha.ToString("y", PortalCulture.GetCulture)
    End Function

    Protected Overrides Sub OnDataBinding(ByVal e As System.EventArgs)
        CreateDataSources(DataSource)

        If (Not DataSource Is Nothing) AndAlso (TypeOf DataSource Is BillingStatementDataSet) Then
            Dim ds As BillingStatementDataSet = DataSource
            Dim invoice As BillingStatementDataSet.BillingStatementsRow

            If ds.BillingStatements.Count > 0 Then
                Dim city As String = String.Empty
                Dim state As String = String.Empty
                Dim country As String = String.Empty
                Dim location As New StringBuilder
                Dim currencyCode As String = String.Empty
                'Dim compResult As Object

                invoice = ds.BillingStatements(0)

                lblReferenceBank.Text = BBVA_A36.BuildReferenceBillingStatement(invoice.RegistrationDate, invoice.CompanyID, invoice.BillingStatementID)

                If Not invoice.IsReferenceNumberNull Then
                    lblInvoiceNumber.Text = invoice.ReferenceNumber
                    lblInvoiceID.Text = invoice.ReferenceNumber
                Else
                    lblInvoiceNumber.Text = "---"
                    lblInvoiceID.Text = "---"
                End If

                'lblPeriod.Text = Formatting.GetPeriodName(invoice.Month, invoice.Year, , PortalCulture.GetCulture.Name)
                lblPeriod.Text = FormatPeriod(invoice.Month, invoice.Year)

                'currencyCode = invoice.BillingCurrencyCode
                currencyCode = "MXN"
                lblCurrencyCode.Text = currencyCode

                '*nuevo
                Dim dvExtra As New DataView(ds.ExtraCharges)
                dvExtra.RowFilter = "BillingStatementID = " & invoice.BillingStatementID
                If dvExtra.Count > 0 Then

                    lblStatus.Text = clsFormat.GetDisplayName(CType(dvExtra(0).Item("Status"), Oz.BillingSystem.Common.BillingStatementStatus))

                    MoneyExRate = dvExtra(0).Item("MoneyExchangeRate")
                    'lblMoneyExchange.Text = Formatting.FormatMoneyExchange(dvFixed(0).Item("MoneyExchangeRate"), currencyCode)
                    lblMoneyExchange.Text = Fcurrency(CDec(dvExtra(0).Item("MoneyExchangeRateMxn")), 2)

                    trMoneyExchange.Style("DISPLAY") = IIf(currencyCode.ToUpper() <> "MXN", "block", "none")

                    lblLimitToConciliate.Text = PortalCulture.GetString("M0BT0000314")

                    If Not IsDBNull(dvExtra(0).Item("DueDate")) Then
                        lblLimitToPay.Text = Formatting.FormatDate(dvExtra(0).Item("DueDate"), "dd MMMM yyyy", PortalCulture.GetCulture.Name)
                    Else
                        lblLimitToPay.Text = "---"
                    End If

                    'If Not IsDBNull(dvFixed(0).Item("ReconciliationDate")) Then
                    lblConciliationDate.Text = PortalCulture.GetString("M0BT0000314")
                    'Else
                    'lblConciliationDate.Text = "---"
                    'End If

                End If

                Dim dvReservation As New DataView(ds.ReservationCharges)
                dvReservation.RowFilter = "BillingStatementID = " & invoice.BillingStatementID
                If dvReservation.Count > 0 Then
                    lblStatus.Text = clsFormat.GetDisplayName(CType(dvReservation(0).Item("status"), Oz.BillingSystem.Common.BillingStatementStatus))

                    MoneyExRate = dvReservation(0).Item("MoneyExchangeRate")
                    'lblMoneyExchange.Text = Formatting.FormatMoneyExchange(dvReservation(0).Item("MoneyExchangeRate"), currencyCode)
                    lblMoneyExchange.Text = FCurrency(CDec(dvReservation(0).Item("MoneyExchangeRate")), 2)

                    trMoneyExchange.Style("DISPLAY") = IIf(currencyCode.ToUpper() <> "USD", "block", "none")

                    lblLimitToConciliate.Text = Formatting.FormatDate(dvReservation(0).Item("LimitToReconcile"), "dd MMMM yyyy", PortalCulture.GetCulture.Name)

                    If Not IsDBNull(dvReservation(0).Item("DueDate")) Then
                        lblLimitToPay.Text = Formatting.FormatDate(dvReservation(0).Item("DueDate"), "dd MMMM yyyy", PortalCulture.GetCulture.Name)
                    Else
                        lblLimitToPay.Text = "---"
                    End If

                    If Not IsDBNull(dvReservation(0).Item("ReconciliationDate")) Then
                        lblConciliationDate.Text = Formatting.FormatDate(dvReservation(0).Item("ReconciliationDate"), "dd MMMM yyyy", PortalCulture.GetCulture.Name)
                    Else
                        lblConciliationDate.Text = "---"
                    End If

                Else
                    Dim dvFixed As New DataView(ds.FixedCharges)
                    dvFixed.RowFilter = "BillingStatementID = " & invoice.BillingStatementID
                    If dvFixed.Count > 0 Then

                        lblStatus.Text = clsFormat.GetDisplayName(CType(dvFixed(0).Item("Status"), Oz.BillingSystem.Common.BillingStatementStatus))

                        MoneyExRate = dvFixed(0).Item("MoneyExchangeRate")
                        'lblMoneyExchange.Text = Formatting.FormatMoneyExchange(dvFixed(0).Item("MoneyExchangeRate"), currencyCode)
                        lblMoneyExchange.Text = FCurrency(CDec(dvFixed(0).Item("MoneyExchangeRate")), 2)

                        trMoneyExchange.Style("DISPLAY") = IIf(currencyCode.ToUpper() <> "USD", "block", "none")

                        lblLimitToConciliate.Text = PortalCulture.GetString("M0BT0000314")

                        If Not IsDBNull(dvFixed(0).Item("DueDate")) Then
                            lblLimitToPay.Text = Formatting.FormatDate(dvFixed(0).Item("DueDate"), "dd MMMM yyyy", PortalCulture.GetCulture.Name)
                        Else
                            lblLimitToPay.Text = "---"
                        End If

                        'If Not IsDBNull(dvFixed(0).Item("ReconciliationDate")) Then
                        lblConciliationDate.Text = PortalCulture.GetString("M0BT0000314")
                        'Else
                        'lblConciliationDate.Text = "---"
                        'End If

                    End If
                End If
                '*fin nuevo

                'lblStatus.Text = clsFormat.GetDisplayName(CType(invoice.Status, BillingStatementStatus))

                'MoneyExRate = invoice.MoneyExchangeRate
                'lblMoneyExchange.Text = Formatting.FormatMoneyExchange(invoice.MoneyExchangeRate, currencyCode)

                'trMoneyExchange.Style("DISPLAY") = IIf(currencyCode.ToUpper() <> "USD", "block", "none")

                lblRegistrationDate.Text = Formatting.FormatDate(invoice.RegistrationDate, "dd MMMM yyyy", PortalCulture.GetCulture.Name)
                'lblLimitToConciliate.Text = Formatting.FormatDate(invoice.LimitToReconcile, "D", PortalCulture.GetCulture.Name)

                'If Not invoice.IsDueDateNull Then
                '    lblLimitToPay.Text = Formatting.FormatDate(invoice.DueDate, "D", PortalCulture.GetCulture.Name)
                'Else
                '    lblLimitToPay.Text = "---"
                'End If

                'If Not invoice.IsReconciliationDateNull Then
                '    lblConciliationDate.Text = Formatting.FormatDate(invoice.ReconciliationDate, "D", PortalCulture.GetCulture.Name)
                'Else
                '    lblConciliationDate.Text = "---"
                'End If

                lblPaymentDate.Text = "---"

                Dim Hotel As BillingStatementDataSet.HotelsRow
                Hotel = ds.Hotels.FindByCompanyID(invoice.CompanyID)

                lblHotelName.Text = Hotel.CompanyName

                If Not Hotel.IsStreetAddressNull() Then
                    lblStreetAddress.Text = Hotel.StreetAddress
                Else
                    lblStreetAddress.Text = "---"
                End If

                If Not Hotel.IsPostalCodeNull() Then
                    lblPostalCode.Text = Hotel.PostalCode
                Else
                    lblPostalCode.Text = "---"
                End If

                If Not Hotel.IsAreaNull() Then
                    lblZone.Text = Hotel.Area
                Else
                    lblZone.Text = "---"
                End If

                city = Hotel.City

                If Not Hotel.IsStateNull() Then
                    state = Hotel.State
                Else
                    state = "---"
                End If

                country = Hotel.CountryCode

                If city <> String.Empty Then location.AppendFormat("{0}", city)
                If state <> String.Empty Then location.AppendFormat(", {0}", state)
                If country <> String.Empty Then location.AppendFormat(", {0}", country)

                lblLocation.Text = location.ToString()

                Dim subTotal As Decimal = 0
                Dim tax As Decimal = 0
                Dim total As Decimal = 0
                Dim subTotalCharges As Decimal = 0
                Dim subTotalReservation As Decimal = 0
                Dim taxCharges As Decimal = 0
                Dim taxReservation As Decimal = 0


                'Fiexed
                For Each rFixed As BillingStatementDataSet.FixedChargesRow In invoice.GetFixedChargesRows
                    subTotalCharges += (rFixed.Subtotal * rFixed.MoneyExchangeRateMxn)
                    taxCharges += (rFixed.Taxes * rFixed.MoneyExchangeRateMxn)
                Next
                'Extra
                For Each rExtra As BillingStatementDataSet.ExtraChargesRow In invoice.GetExtraChargesRows
                    subTotalCharges += (rExtra.Subtotal * rExtra.MoneyExchangeRateMxn)
                    taxCharges += (rExtra.Taxes * rExtra.MoneyExchangeRateMxn)
                Next

                'Reservation

                For Each rReservation As BillingStatementDataSet.ReservationChargesRow In invoice.GetReservationChargesRows
                    ReservationMoneyExchangeRateMxn = rReservation.MoneyExchangeRateMxn
                    subTotalReservation += (rReservation.Subtotal * rReservation.MoneyExchangeRateMxn)
                    taxReservation += (rReservation.Taxes * rReservation.MoneyExchangeRateMxn)
                Next
                subTotal = (subTotalCharges + subTotalReservation)
                tax = (taxCharges + taxReservation)
                total = subTotal + tax

                lblAmountCharges.Text = FCurrency(subTotalCharges, 2) & " " & currencyCode
                lblCharges.Text = lblAmountCharges.Text
                trAmountCharges.Style("DISPLAY") = IIf(subTotalCharges > 0, "block", "none")


                lblAmountCommissions.Text = FCurrency(subTotalReservation, 2) & " " & currencyCode
                lblCommissions.Text = lblAmountCommissions.Text

                If Not invoice.IsSubtotalNull() Then
                    lblSubtotal.Text = FCurrency(subTotal, 2) & " " & currencyCode
                Else
                    lblSubtotal.Text = "---"
                End If

                If Not invoice.IsTaxesNull() Then
                    lblTaxes.Text = FCurrency(tax, 2) & " " & currencyCode
                Else
                    lblTaxes.Text = "---"
                End If


                If Not invoice.IsTotalDueNull Then
                    lblGrandTotal.Text = FCurrency(total, 2) & " " & currencyCode
                Else
                    lblGrandTotal.Text = "---"
                End If

                lblBalance.Visible = False
                lblBalanceCaption.Visible = False

            End If
        End If

        BindGrids()
    End Sub

    Private Sub CreateDataSources(ByVal ds As BillingStatementDataSet)
        If (Not DataSource Is Nothing) AndAlso (TypeOf DataSource Is BillingStatementDataSet) Then
            If CurrentInvoiceDetailsView Is Nothing Then
                '*CurrentInvoiceDetailsView = ds.BillingStatementDetails.DefaultView
                CurrentInvoiceDetailsView = CreateDataView(ds)
            End If
            If CurrentCommissionDetailsView Is Nothing Then
                CurrentCommissionDetailsView = ds.CommissionDetails.DefaultView
            End If
            CurrentInvoiceDetailsView.RowFilter = String.Format("BillingStatementTransactionCode <> {0}", CInt(BillingStatementTransaction.ReservationCommission))

        End If
    End Sub

    Private Function CreateDataView(ByVal ds As BillingStatementDataSet) As DataView
        Dim dsOther As New DataSet
        Dim table As New DataTable
        Dim row As DataRow
        With table.Columns
            .Add("BillingStatementTransactionCode", GetType(System.Int32))
            .Add("Quantity", GetType(System.Int32))
            .Add("Amount", GetType(System.Decimal))
            .Add("SubTotal", GetType(System.Decimal))
        End With
        dsOther.Tables.Add(table)
        For i As Integer = 0 To ds.FixedCharges.Count - 1
            row = dsOther.Tables(0).NewRow
            row("BillingStatementTransactionCode") = ds.FixedCharges(i).BillingStatementTransactionCode
            row("Quantity") = ds.FixedCharges(i).Quantity
            row("Amount") = ds.FixedCharges(i).Amount * ds.FixedCharges(i).MoneyExchangeRateMxn
            row("SubTotal") = ds.FixedCharges(i).Subtotal * ds.FixedCharges(i).MoneyExchangeRateMxn
            dsOther.Tables(0).Rows.Add(row)
        Next
        For i As Integer = 0 To ds.ExtraCharges.Count - 1
            row = dsOther.Tables(0).NewRow
            row("BillingStatementTransactionCode") = ds.ExtraCharges(i).BillingStatementTransactionCode
            row("Quantity") = ds.ExtraCharges(i).Quantity
            row("Amount") = ds.ExtraCharges(i).Amount * ds.ExtraCharges(i).MoneyExchangeRateMxn
            row("SubTotal") = ds.ExtraCharges(i).Subtotal * ds.ExtraCharges(i).MoneyExchangeRateMxn
            dsOther.Tables(0).Rows.Add(row)
        Next

        Return dsOther.Tables(0).DefaultView
    End Function

    Private Sub BindGrids()
        grdCharges.Columns(0).HeaderText = PortalCulture.GetString("M0BT0000126")
        grdCharges.Columns(1).HeaderText = PortalCulture.GetString("M0BT0000127")
        grdCharges.Columns(2).HeaderText = PortalCulture.GetString("M0BT0000128")
        grdCharges.Columns(3).HeaderText = PortalCulture.GetString("M0BT0000129")

        grdCommissions.Columns(1).HeaderText = PortalCulture.GetString("M0BT0000332") 'reservacion
        grdCommissions.Columns(2).HeaderText = PortalCulture.GetString("M0BT0000042") 'fuente
        grdCommissions.Columns(3).HeaderText = PortalCulture.GetString("M0BT0000069") 'tarifa
        grdCommissions.Columns(4).HeaderText = PortalCulture.GetString("M0BT0000068") 'tipotarifa
        grdCommissions.Columns(5).HeaderText = PortalCulture.GetString("M0BT0000043") 'llegada
        grdCommissions.Columns(6).HeaderText = PortalCulture.GetString("M0BT0000071") 'importe
        grdCommissions.Columns(7).HeaderText = PortalCulture.GetString("M0BT0000072") 'cargo
        grdCommissions.Columns(8).HeaderText = PortalCulture.GetString("M0BT0000073") 'agencia
        grdCommissions.Columns(9).HeaderText = PortalCulture.GetString("M0BT0000074") 'comision ag
        grdCommissions.Columns(10).HeaderText = PortalCulture.GetString("M0BT0000045") 'total
        grdCommissions.Columns(11).HeaderText = PortalCulture.GetString("M0BT0000046") 'estado

       





        BindList(grdCharges, CurrentInvoiceDetailsView)
        ToggleChargesRows((Not CurrentInvoiceDetailsView Is Nothing) AndAlso (CurrentInvoiceDetailsView.Count > 0))

        BindList(grdCommissions, CurrentCommissionDetailsView)
        ToggleCommissionsRows((Not CurrentCommissionDetailsView Is Nothing) AndAlso (CurrentCommissionDetailsView.Count > 0))
    End Sub

    Private Function GetReservationMoneyExchangeRateMxn() As Decimal
        If (Not DataSource Is Nothing) AndAlso (TypeOf DataSource Is BillingStatementDataSet) Then
            Dim ds As BillingStatementDataSet = DataSource
            If ds.ReservationCharges.Count > 0 Then
                Return ds.ReservationCharges(0).MoneyExchangeRateMxn
            End If
        End If
        Return 1
    End Function

    Private Sub grdCharges_SortCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles grdCharges.SortCommand
        Dim order As String = String.Empty

        CreateDataSources(DataSource)
        If Not CurrentInvoiceDetailsView Is Nothing Then
            If CurrentOrder(CurrentInvoiceDetailsView.Table.TableName, e.SortExpression).ToUpper() = "ASC" Then
                order = "DESC"
            Else
                order = "ASC"
            End If

            CurrentInvoiceDetailsView.Sort = String.Format("{0} {1}", e.SortExpression, order)
            CurrentOrder(CurrentInvoiceDetailsView.Table.TableName, e.SortExpression) = order
        End If
        BindGrids()
    End Sub

    Private Sub grdCommissions_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles grdCommissions.SortCommand
        Dim order As String = String.Empty

        CreateDataSources(DataSource)
        If Not CurrentCommissionDetailsView Is Nothing Then
            If CurrentOrder(CurrentCommissionDetailsView.Table.TableName, e.SortExpression).ToUpper() = "ASC" Then
                order = "DESC"
            Else
                order = "ASC"
            End If

            CurrentCommissionDetailsView.Sort = String.Format("{0} {1}", e.SortExpression, order)
            CurrentOrder(CurrentCommissionDetailsView.Table.TableName, e.SortExpression) = order
        End If
        BindGrids()
    End Sub

    Private Sub grdCharges_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdCharges.ItemDataBound
        Select Case e.Item.ItemType
            Case ListItemType.Item, ListItemType.AlternatingItem
                Dim lbl As Label = e.Item.FindControl("lblDescription")
                lbl.Text = Formatting.GetDisplayName(CType(DataBinder.Eval(e.Item.DataItem, "BillingStatementTransactionCode"), BillingStatementTransaction))
                lbl = e.Item.FindControl("lblDgAmount")
                lbl.Text = FCurrency(DataBinder.Eval(e.Item.DataItem, "Amount"), 2)
                lbl = e.Item.FindControl("lblDgSubtotal")
                lbl.Text = FCurrency(DataBinder.Eval(e.Item.DataItem, "Subtotal"), 2)
        End Select
    End Sub

    Private Sub grdCommissions_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdCommissions.ItemDataBound
        Select Case e.Item.ItemType
            Case ListItemType.Item, ListItemType.AlternatingItem

                Dim lbl As Label = e.Item.FindControl("lblDgTarifa")
                lbl.Text = FCurrency((DataBinder.Eval(e.Item.DataItem, "AverageRate") * ReservationMoneyExchangeRateMxn), 2)
                lbl = e.Item.FindControl("lblRateType")
                lbl.Text = Formatting.GetDisplayName(CType(DataBinder.Eval(e.Item.DataItem, "RateType"), RateType))
                lbl = e.Item.FindControl("lblNights")
                lbl.Text = IIf(DataBinder.Eval(e.Item.DataItem, "Nights") > 1, DataBinder.Eval(e.Item.DataItem, "Nights") & " " & PortalCulture.GetString("M0BT0000070"), DataBinder.Eval(e.Item.DataItem, "Nights") & " " & PortalCulture.GetString("M0BT0000330"))
                lbl = e.Item.FindControl("lblDgImporte")
                lbl.Text = FCurrency((DataBinder.Eval(e.Item.DataItem, "Total") * ReservationMoneyExchangeRateMxn), 2)
                lbl = e.Item.FindControl("lblDgComAmount")
                lbl.Text = FCurrency((DataBinder.Eval(e.Item.DataItem, "Amount") * ReservationMoneyExchangeRateMxn), 2)
                lbl = e.Item.FindControl("lblDgTA")
                lbl.Text = FCurrency((DataBinder.Eval(e.Item.DataItem, "TACommission") * ReservationMoneyExchangeRateMxn), 2)
                lbl = e.Item.FindControl("lblDgChargeTA")
                lbl.Text = FCurrency((DataBinder.Eval(e.Item.DataItem, "ChargeOverTA") * ReservationMoneyExchangeRateMxn), 2)
                lbl = e.Item.FindControl("lblDgTotal")
                lbl.Text = FCurrency((DataBinder.Eval(e.Item.DataItem, "Total") * ReservationMoneyExchangeRateMxn), 2)
                lbl = e.Item.FindControl("lblDgStatus")
                lbl.Text = Formatting.GetDisplayName(CType(DataBinder.Eval(e.Item.DataItem, "Status"), CommissionDetailStatus))
        End Select
    End Sub

End Class
