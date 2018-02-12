Imports System.Configuration.ConfigurationManager
Imports System.Text
Imports Oz.UniBilling.DataSchemas.Hotels
Imports Oz.UniBilling.Hotels.Business
Imports Oz.UniBilling.Common.Hotels
Imports Oz.UniBilling.Common

Imports ReferencesSystem

Partial Class HotelInvoiceViewer
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

    Private Property MoneyExRate() As Decimal
        Get
            Return ViewState("MoneyExRate")
        End Get
        Set(ByVal Value As Decimal)
            ViewState("MoneyExRate") = Value
        End Set
    End Property

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
    End Sub

    Private Sub dlsInvoices_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles dlsInvoices.ItemDataBound
        Select Case e.Item.ItemType
            Case ListItemType.Item, ListItemType.AlternatingItem
                Dim ds As BillingStatementDataSet = dlsInvoices.DataSource
                Dim statement As BillingStatementDataSet.BillingStatementsRow
                If Not DataBinder.Eval(e.Item.DataItem, "BillingStatementID") Is DBNull.Value Then
                    statement = ds.BillingStatements.FindByBillingStatementID(DataBinder.Eval(e.Item.DataItem, "BillingStatementID"))
                    If Not (statement Is Nothing) Then
                        Dim lblRef As Label = e.Item.FindControl("lblReferenceBank")
                        If Not lblRef Is Nothing Then
                            lblRef.Text = BBVA_A36.BuildReferenceBillingStatement(statement.RegistrationDate, statement.CompanyID, statement.BillingStatementID)
                        End If
                    End If
                End If

                'ds.BilledExtraCharges(0).a()

                ' Retrieve hotel name
                Dim lnk As HyperLink = e.Item.FindControl("lnkHotel")
                Dim lbl As Label = e.Item.FindControl("lblHotel")
                Dim div As HtmlGenericControl = e.Item.FindControl("divHotelData")
                Dim header As Label = e.Item.FindControl("lblHotelDataHeader")
                Dim img As WebControls.Image = e.Item.FindControl("imgCloseHotelData")
                If (Not lnk Is Nothing) AndAlso (Not lbl Is Nothing) AndAlso (Not div Is Nothing) AndAlso (Not header Is Nothing) AndAlso (Not img Is Nothing) Then
                    Dim hotelName As String = String.Empty
                    Dim hotelLocation As String = String.Empty
                    Dim hotelData As New StringBuilder
                    Dim script As String = String.Empty

                    If ds.Hotels.Count > 0 Then
                        hotelName = ds.Hotels(0).CompanyName
                        If Not ds.Hotels(0).IsStreetAddressNull() Then
                            hotelData.AppendFormat("<BR>{0}", ds.Hotels(0).StreetAddress)
                        End If
                        If Not ds.Hotels(0).IsPostalCodeNull() Then
                            hotelData.AppendFormat("<BR>C.P. {0}", ds.Hotels(0).PostalCode)
                        End If
                        hotelLocation = ds.Hotels(0).City
                        hotelData.AppendFormat("<BR>{0}", ds.Hotels(0).City)
                        If Not ds.Hotels(0).IsStateNull() Then
                            hotelData.AppendFormat("<BR>{0}", ds.Hotels(0).State)
                        End If
                        hotelLocation &= String.Format(", {0}", ds.Hotels(0).CountryCode)
                        hotelData.AppendFormat("<BR>{0}", ds.Hotels(0).CountryCode)
                    End If

                    lnk.Text = String.Format("{0}, {1}", hotelName, hotelLocation)
                    header.Text = hotelName
                    lbl.Text = hotelData.ToString()

                    div.Style("display") = "none"

                    script = String.Format("javascript:expandOrCollapse('{0}', '{1}');", lnk.ClientID, div.ClientID)
                    lnk.NavigateUrl = script
                    img.Attributes("onClick") = script
                    img.Style("cursor") = "pointer"
                End If

                ' Image attributes for toggling details
                Dim td As HtmlTableCell = e.Item.FindControl("tdDetails")
                img = e.Item.FindControl("imgToggleDetails")
                If (Not img Is Nothing) AndAlso (Not td Is Nothing) Then
                    img.Attributes("onClick") = String.Format("javascript:expandOrCollapse('{0}', '{1}');", img.ClientID, td.ClientID)
                    img.Style("cursor") = "pointer"
                    td.Style("display") = "none"
                End If

                'nuevo
                Dim dvFixed As New DataView(ds.FixedCharges)
                dvFixed.RowFilter = "BillingStatementID = " & DataBinder.Eval(e.Item.DataItem, "BillingStatementID")
                If dvFixed.Count > 0 Then
                    lbl = e.Item.FindControl("lblStatus")
                    If Not lbl Is Nothing Then lbl.Text = Formatting.GetDisplayName(CType(dvFixed(0).Item("Status"), BillingStatus))

                    lbl = e.Item.FindControl("lblLastConciliationDate")
                    If Not lbl Is Nothing Then lbl.Text = PortalCulture.GetString("M0BT0000314")

                    lbl = e.Item.FindControl("lblLastPaymentDate")
                    If Not lbl Is Nothing Then
                        If Not IsDBNull(dvFixed(0).Item("DueDate")) Then lbl.Text = Formatting.FormatDate(dvFixed(0).Item("DueDate"), "D", PortalCulture.GetCulture.Name)
                    End If

                    MoneyExRate = dvFixed(0).Item("MoneyExchangeRate")

                    '//'If Not grd Is Nothing Then
                    '//'    MoneyExRate = dvFixed(0).Item("MoneyExchangeRate")

                    '//'    AddHandler grd.ItemDataBound, AddressOf grdDetails_ItemDataBound

                    '//'    BindList(grd, dvFixed)
                    '//'End If

                End If

                Dim dvExtra As New DataView(ds.ExtraCharges)
                dvExtra.RowFilter = "BillingStatementID = " & DataBinder.Eval(e.Item.DataItem, "BillingStatementID")
                If dvExtra.Count > 0 Then
                    lbl = e.Item.FindControl("lblStatus")
                    If Not lbl Is Nothing Then lbl.Text = Formatting.GetDisplayName(CType(dvExtra(0).Item("Status"), BillingStatus))

                    lbl = e.Item.FindControl("lblLastConciliationDate")
                    If Not lbl Is Nothing Then lbl.Text = PortalCulture.GetString("M0BT0000314")

                    lbl = e.Item.FindControl("lblLastPaymentDate")
                    If Not lbl Is Nothing Then
                        If Not IsDBNull(dvExtra(0).Item("DueDate")) Then lbl.Text = Formatting.FormatDate(dvExtra(0).Item("DueDate"), "D", PortalCulture.GetCulture.Name)
                    End If

                    MoneyExRate = dvExtra(0).Item("MoneyExchangeRate")

                End If


                Dim dvReservation As New DataView(ds.ReservationCharges)
                dvReservation.RowFilter = "BillingStatementID = " & DataBinder.Eval(e.Item.DataItem, "BillingStatementID")
                If dvReservation.Count > 0 Then
                    lbl = e.Item.FindControl("lblStatus")
                    If Not lbl Is Nothing Then lbl.Text = Formatting.GetDisplayName(CType(dvReservation(0).Item("Status"), BillingStatus))

                    lbl = e.Item.FindControl("lblLastConciliationDate")
                    If Not lbl Is Nothing Then lbl.Text = Formatting.FormatDate(dvReservation(0).Item("LimitToReconcile"), "D", PortalCulture.GetCulture.Name)

                    lbl = e.Item.FindControl("lblLastPaymentDate")
                    If Not lbl Is Nothing Then
                        If Not IsDBNull(dvReservation(0).Item("DueDate")) Then lbl.Text = Formatting.FormatDate(dvReservation(0).Item("DueDate"), "D", PortalCulture.GetCulture.Name)
                    End If

                    MoneyExRate = dvReservation(0).Item("MoneyExchangeRate")

                    '//'If Not grd Is Nothing Then
                    '//'    MoneyExRate = dvReservation(0).Item("MoneyExchangeRate")

                    '//'    AddHandler grd.ItemDataBound, AddressOf grdDetails_ItemDataBound

                    '//'    BindList(grd, dvReservation)
                    '//'End If

                End If

                'Obenter Totales en Pesos 
                Dim total As Decimal = 0
                Dim subTotal As Decimal = 0
                Dim tax As Decimal = 0

                If Not statement Is Nothing Then
                    'Reservation
                    For Each rRes As BillingStatementDataSet.ReservationChargesRow In statement.GetReservationChargesRows()
                        subTotal += rRes.Subtotal * rRes.MoneyExchangeRateMxn
                        tax += rRes.Taxes * rRes.MoneyExchangeRateMxn
                    Next
                    'Extra
                    For Each rExt As BillingStatementDataSet.ExtraChargesRow In statement.GetExtraChargesRows()
                        subTotal += rExt.Subtotal * rExt.MoneyExchangeRateMxn
                        tax += rExt.Taxes * rExt.MoneyExchangeRateMxn
                    Next
                    'Fixed
                    For Each rFix As BillingStatementDataSet.FixedChargesRow In statement.GetFixedChargesRows()
                        subTotal += rFix.Subtotal * rFix.MoneyExchangeRateMxn
                        tax += rFix.Taxes * rFix.MoneyExchangeRateMxn
                    Next
                End If
                total = (subTotal + tax)


                lbl = e.Item.FindControl("lblSubtotal")
                lbl.Text = String.Format("{0} MXN", FCurrency(subTotal, 2))

                lbl = e.Item.FindControl("lblTaxes")
                lbl.Text = String.Format("{0} MXN", FCurrency(tax, 2))

                lbl = e.Item.FindControl("lblTotal")
                lbl.Text = String.Format("{0} MXN", FCurrency(total, 2))

                lbl = e.Item.FindControl("lblGrandTotal")
                lbl.Text = String.Format("{0} MXN", FCurrency(total, 2))










                ' Bind details grid
                Dim grd As DataGrid = e.Item.FindControl("grdDetails")
                If Not grd Is Nothing Then
                    grd.Columns(0).HeaderText = PortalCulture.GetString("M0BT0000126")
                    grd.Columns(1).HeaderText = PortalCulture.GetString("M0BT0000129")
                    grd.Columns(2).HeaderText = PortalCulture.GetString("M0BT0000130")
                    grd.Columns(3).HeaderText = PortalCulture.GetString("M0BT0000133")

                    AddHandler grd.ItemDataBound, AddressOf grdDetails_ItemDataBound

                    Dim dsDetails As New DataSet
                    dsDetails = CreateTableDetails(ds, DataBinder.Eval(e.Item.DataItem, "BillingStatementID"))
                    BindList(grd, dsDetails)
                End If
                '//' fin nuevo '''
                lnk = e.Item.FindControl("hLnkViewDetail")
                If Not lnk Is Nothing Then
                    lnk.Text = PortalCulture.GetString("00376")
                    lnk.NavigateUrl = "../InvoiceToConciliateDetails.aspx?id=" & DataBinder.Eval(e.Item.DataItem, "BillingStatementID")
                End If

        End Select

    End Sub

    Private Function CreateTableDetails(ByVal ds As BillingStatementDataSet, ByVal BillingStatementID As Long) As DataSet
        Dim dtSet As New DataSet
        Dim table As DataTable = New DataTable("Details")
        Dim dr As DataRow

        With table.Columns
            .Add("BillingStatementTransactionCode", GetType(System.Int32))
            .Add("Subtotal", GetType(System.Decimal))
            .Add("Taxes", GetType(System.Decimal))
            .Add("Total", GetType(System.Decimal))
        End With
        dtSet.Tables.Add(table)

        Dim dv As New DataView(ds.ReservationCharges)
        dv.RowFilter = "BillingStatementID = " & BillingStatementID
        For i As Integer = 0 To dv.Count - 1
            dr = dtSet.Tables("Details").NewRow()
            With dr
                .Item("BillingStatementTransactionCode") = BillingStatementTransaction.ReservationCommission
                .Item("Subtotal") = dv(i).item("Subtotal") * dv(i).item("MoneyExchangeRateMxn")
                .Item("Taxes") = dv(i).item("Taxes") * dv(i).item("MoneyExchangeRateMxn")
                .Item("Total") = dv(i).item("Total") * dv(i).item("MoneyExchangeRateMxn")
            End With
            dtSet.Tables("Details").Rows.Add(dr)
        Next

        Dim dvFixed As New DataView(ds.FixedCharges)
        dvFixed.RowFilter = "BillingStatementID = " & BillingStatementID
        For i As Integer = 0 To dvFixed.Count - 1
            dr = dtSet.Tables("Details").NewRow()
            With dr
                .Item("BillingStatementTransactionCode") = dvFixed(i).item("BillingStatementTransactionCode")
                .Item("Subtotal") = dvFixed(i).item("Subtotal") * dvFixed(i).item("MoneyExchangeRateMxn")
                .Item("Taxes") = dvFixed(i).item("Taxes") * dvFixed(i).item("MoneyExchangeRateMxn")
                .Item("Total") = dvFixed(i).Item("Total") * dvFixed(i).item("MoneyExchangeRateMxn")
            End With
            dtSet.Tables("Details").Rows.Add(dr)
        Next

        Dim dvExtra As New DataView(ds.ExtraCharges)
        dvExtra.RowFilter = "BillingStatementID = " & BillingStatementID
        For i As Integer = 0 To dvExtra.Count - 1
            dr = dtSet.Tables("Details").NewRow()
            With dr
                .Item("BillingStatementTransactionCode") = dvExtra(i).item("BillingStatementTransactionCode")
                .Item("Subtotal") = dvExtra(i).item("Subtotal") * dvExtra(i).item("MoneyExchangeRateMxn")
                .Item("Taxes") = dvExtra(i).item("Taxes") * dvExtra(i).item("MoneyExchangeRateMxn")
                .Item("Total") = dvExtra(i).Item("Total") * dvExtra(i).item("MoneyExchangeRateMxn")
            End With
            dtSet.Tables("Details").Rows.Add(dr)
        Next

        Return dtSet

    End Function

    Private Sub grdDetails_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)
        Select Case e.Item.ItemType
            Case ListItemType.Item, ListItemType.AlternatingItem
                Dim lbl As Label = e.Item.FindControl("lblDescription")
                Dim lnk As HyperLink = e.Item.FindControl("lnkDescription")
                Dim manager As New BillingManager(Oz.UniBilling.Common.Security.cSecurity.GetIsser(1), AppSettings("UserID"), "ratemanager@univisit.com")
                Dim transactionsData As New BillingStatementTransactionsDataset
                Dim transactions() As BillingStatementTransactionsDataset.BillingStatementTransactionsRow

                manager.GetBillingStatementTransactions(transactionsData)

                If (Not lbl Is Nothing) And (Not lnk Is Nothing) Then
                    lbl.Visible = True
                    lnk.Visible = False
                    '' Link para ver detalles de comisiones desactivado
                    ''If Not IsDBNull(DataBinder.Eval(e.Item.DataItem, "BillingConceptCode")) Then
                    ''    lbl.Visible = CType(DataBinder.Eval(e.Item.DataItem, "BillingConceptCode"), BillingConcept) <> BillingConcept.ReservationCommission
                    ''    lnk.Visible = CType(DataBinder.Eval(e.Item.DataItem, "BillingConceptCode"), BillingConcept) = BillingConcept.ReservationCommission
                    ''End If
                    ''lbl.Text = Formatting.GetDisplayName(CType(DataBinder.Eval(e.Item.DataItem, "BillingStatementTransactionCode"), BillingStatementTransaction))
                    If (Not transactionsData Is Nothing) Then
                        transactions = transactionsData.BillingStatementTransactions.Select(String.Format("BillingStatementTransactionCode={0}", DataBinder.Eval(e.Item.DataItem, "BillingStatementTransactionCode")))
                        lbl.Text = IIf(transactions.Length <> 0, transactions(0).TransactionName, "")
                    End If



                End If
                lbl = e.Item.FindControl("lblDgSubtotal")
                lbl.Text = FCurrency(DataBinder.Eval(e.Item.DataItem, "Subtotal"), 2)
                lbl = e.Item.FindControl("lblDgTaxes")
                lbl.Text = FCurrency(DataBinder.Eval(e.Item.DataItem, "Taxes"), 2)
                lbl = e.Item.FindControl("lblDgTotal")
                lbl.Text = FCurrency(DataBinder.Eval(e.Item.DataItem, "Total"), 2)



                ''Dim ds As BillingStatementDataSet = dlsInvoices.DataSource

                ''Dim dvFixed As New DataView(ds.FixedCharges)
                ''dvFixed.RowFilter = "BillingStatementID = " & DataBinder.Eval(e.Item.DataItem, "BillingStatementID")
                ''If dvFixed.Count > 0 Then
                ''    Dim lbl As Label = e.Item.FindControl("lblDescription")
                ''    Dim lnk As HyperLink = e.Item.FindControl("lnkDescription")
                ''    If (Not lbl Is Nothing) And (Not lnk Is Nothing) Then
                ''        lbl.Visible = True
                ''        lnk.Visible = False
                ''        '' Link para ver detalles de comisiones desactivado
                ''        ''If Not IsDBNull(DataBinder.Eval(e.Item.DataItem, "BillingConceptCode")) Then
                ''        ''    lbl.Visible = CType(DataBinder.Eval(e.Item.DataItem, "BillingConceptCode"), BillingConcept) <> BillingConcept.ReservationCommission
                ''        ''    lnk.Visible = CType(DataBinder.Eval(e.Item.DataItem, "BillingConceptCode"), BillingConcept) = BillingConcept.ReservationCommission
                ''        ''End If

                ''        Try
                ''            lbl.Text = Formatting.GetDisplayName(CType(dvFixed(0).Item("BillingStatementTransactionCode"), BillingStatementTransaction))
                ''        Catch ex As Exception
                ''            lbl.Text = PortalCulture.GetString("M0BT0000322") 'Comisiones
                ''        End Try

                ''    End If
                ''    lbl = e.Item.FindControl("lblDgSubtotal")
                ''    lbl.Text = FCurrency(dvFixed(0).Item("Subtotal"))
                ''    lbl = e.Item.FindControl("lblDgTaxes")
                ''    lbl.Text = FCurrency(dvFixed(0).Item("Taxes"))
                ''    lbl = e.Item.FindControl("lblDgTotal")
                ''    lbl.Text = FCurrency(dvFixed(0).Item("Total"))
                ''End If

                ''Dim dvReservation As New DataView(ds.ReservationCharges)
                ''dvReservation.RowFilter = "BillingStatementID = " & DataBinder.Eval(e.Item.DataItem, "BillingStatementID")
                ''If dvReservation.Count > 0 Then
                ''    Dim lbl As Label = e.Item.FindControl("lblDescription")
                ''    Dim lnk As HyperLink = e.Item.FindControl("lnkDescription")
                ''    If (Not lbl Is Nothing) And (Not lnk Is Nothing) Then
                ''        lbl.Visible = True
                ''        lnk.Visible = False
                ''        '' Link para ver detalles de comisiones desactivado
                ''        ''If Not IsDBNull(DataBinder.Eval(e.Item.DataItem, "BillingConceptCode")) Then
                ''        ''    lbl.Visible = CType(DataBinder.Eval(e.Item.DataItem, "BillingConceptCode"), BillingConcept) <> BillingConcept.ReservationCommission
                ''        ''    lnk.Visible = CType(DataBinder.Eval(e.Item.DataItem, "BillingConceptCode"), BillingConcept) = BillingConcept.ReservationCommission
                ''        ''End If

                ''        Try
                ''            '/lbl.Text = Formatting.GetDisplayName(CType(DataBinder.Eval(e.Item.DataItem, "BillingStatementTransactionCode"), BillingStatementTransaction))
                ''            lbl.Text = Formatting.GetDisplayName(CType(dvReservation(0).Item("BillingStatementTransactionCode"), BillingStatementTransaction))
                ''        Catch ex As Exception
                ''            lbl.Text = PortalCulture.GetString("M0BT0000322") 'Comisiones
                ''        End Try

                ''    End If
                ''    lbl = e.Item.FindControl("lblDgSubtotal")
                ''    '/lbl.Text = FCurrency(DataBinder.Eval(e.Item.DataItem, "Subtotal"))
                ''    lbl.Text = FCurrency(dvReservation(0).Item("Subtotal"))
                ''    lbl = e.Item.FindControl("lblDgTaxes")
                ''    '/lbl.Text = FCurrency(DataBinder.Eval(e.Item.DataItem, "Taxes"))
                ''    lbl.Text = FCurrency(dvReservation(0).Item("Taxes"))
                ''    lbl = e.Item.FindControl("lblDgTotal")
                ''    '/lbl.Text = FCurrency(DataBinder.Eval(e.Item.DataItem, "Total"))
                ''    lbl.Text = FCurrency(dvReservation(0).Item("Total"))
                ''End If

        End Select
    End Sub

    Protected Overrides Sub OnDataBinding(ByVal e As System.EventArgs)
        Dim hasInvoices As Boolean = False
        If (Not DataSource Is Nothing) AndAlso (TypeOf (DataSource) Is BillingStatementDataSet) Then
            Dim billingData As BillingStatementDataSet = DataSource
            'Show only active invoices
            Dim statement As BillingStatementDataSet.BillingStatementsRow
            Dim statementsAmount As Integer
            For statementsAmount = billingData.BillingStatements.Count - 1 To 0 Step -1
                statement = billingData.BillingStatements(statementsAmount)
                If (statement.GetExtraChargesRows().Length > 0) AndAlso (statement.GetExtraChargesRows()(0).GetBilledExtraChargesRows().Length > 0) AndAlso (statement.GetExtraChargesRows()(0).GetBilledExtraChargesRows()(0).Active = False) Then
                    billingData.BillingStatements.RemoveBillingStatementsRow(statement)
                ElseIf (statement.GetFixedChargesRows().Length > 0) AndAlso (statement.GetFixedChargesRows()(0).GetBilledFixedChargesRows().Length > 0) AndAlso (statement.GetFixedChargesRows()(0).GetBilledFixedChargesRows()(0).Active = False) Then
                    billingData.BillingStatements.RemoveBillingStatementsRow(statement)
                ElseIf (statement.GetReservationChargesRows().Length > 0) AndAlso (statement.GetReservationChargesRows()(0).GetBilledReservationChargesRows().Length > 0) AndAlso (statement.GetReservationChargesRows()(0).GetBilledReservationChargesRows()(0).Active = False) Then
                    billingData.BillingStatements.RemoveBillingStatementsRow(statement)
                End If
            Next

            BindList(dlsInvoices, billingData, billingData.BillingStatements.TableName)

            hasInvoices = billingData.BillingStatements.Count > 0
        Else
            hasInvoices = False
        End If

        dlsInvoices.Style("display") = IIf(hasInvoices, "block", "none")
        lblNoInvoices.Style("display") = IIf(Not hasInvoices, "block", "none")
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        lblNoInvoices.Text = PortalCulture.GetString("M0BT0000116")
        '//Page.RegisterStartupScript("Util.js", String.Format("<script language=""javascript"" src=""{0}Script/Util.js""></script>", Request.ApplicationPat & "/HotelAdministrator/Invoicing/"))

        Dim scriptUrl As String
        scriptUrl = String.Format("<script language=""javascript"" src=""{0}Script/Util.js""></script>", GeRequestApplicationPath("/HotelAdministrator/Invoicing/"))
        Page.ClientScript.RegisterStartupScript(Me.GetType(), Me.ClientID, scriptUrl)

    End Sub

    Private Sub dlsInvoices_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs) Handles dlsInvoices.DataBinding

    End Sub

    Private Sub dlsInvoices_DeleteCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs) Handles dlsInvoices.DeleteCommand

    End Sub
End Class
