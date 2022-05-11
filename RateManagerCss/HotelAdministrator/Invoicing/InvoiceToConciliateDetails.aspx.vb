Imports System.Configuration.ConfigurationManager
Imports System.Globalization
Imports Oz.UniBilling.Hotels.Business
Imports Oz.UniBilling.DataSchemas.Hotels
Imports Oz.UniBilling.Common.Hotels
Imports Oz.UniBilling.Common
'Imports Oz.BillingSystem.Common
Imports Oz.UniBilling.Common.Security

Imports ReferencesSystem
Imports System.IO

Partial Class InvoiceToConciliateDetails
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

    Private Property IsCorporate() As Boolean
        Get
            Try
                Return ViewState("_IsCorporate")
            Catch
                Return False
            End Try
        End Get
        Set(ByVal Value As Boolean)
            ViewState("_IsCorporate") = Value
        End Set
    End Property

    Private Property InvoiceID() As Long
        Get
            Return ViewState("InvoiceID")
        End Get
        Set(ByVal Value As Long)
            ViewState("InvoiceID") = Value
        End Set
    End Property

    Private Property PrincipalSecure() As System.Security.Principal.IPrincipal
        Get
            Return ViewState("PrincipalSecure")
        End Get
        Set(ByVal Value As System.Security.Principal.IPrincipal)
            ViewState("PrincipalSecure") = Value
        End Set
    End Property

    Private Property dsData() As Object
        Get
            Return Session("dsData")
        End Get
        Set(ByVal Value As Object)
            Session("dsData") = Value
        End Set
    End Property

    Private Property SortOrder(ByVal key As String) As String
        Get
            If ViewState(key) Is Nothing Then
                Return String.Empty
            End If

            Return ViewState(key)
        End Get
        Set(ByVal Value As String)
            ViewState(key) = Value
        End Set
    End Property

#End Region


    Protected HotelIdentity As New HotelIdentifier

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        Dim x As Integer = 1
        If isUserChain AndAlso x = 2 Then
        Else 'Normal
            If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)
        End If

        Me.HeaderPanel = ctrlHeader.Usuario.uHotelAdministrador

        If Not IsPostBack Then
            If Not IsNothing(Request.QueryString("id")) AndAlso IsNumeric(Request.QueryString("id")) Then
                InvoiceID = Long.Parse(Request.QueryString("id"))

                Dim ds As New BillingStatementDataSet
                Dim BillingMng As New BillingManager(Oz.UniBilling.Common.Security.cSecurity.GetIsser(1), AppSettings("UserID"), "ratemanager@univisit.com")

                BillingMng.GetBillingStatements(InvoiceID, ds)

                If ds.BillingStatements.Count > 0 Then
                    Dim Period As New MonthlyPeriod(ds.BillingStatements(0).Month, ds.BillingStatements(0).Year)
                    Dim statement As BillingStatementDataSet.BillingStatementsRow = ds.BillingStatements(0)
                    IsCorporate = False
                    If Not statement.HotelsRow.IsCorporateIDNull AndAlso statement.HotelsRow.CorporateID <> -1 Then
                        IsCorporate = True
                    End If


                    HotelIdentity.CompanyID = ds.BillingStatements(0).CompanyID
                    If Not ds.BillingStatements(0).IsPortalHotelIDNull() Then HotelIdentity.PortalID = ds.BillingStatements(0).PortalHotelID
                    If Not ds.BillingStatements(0).IsUniPantallaHotelIDNull() Then HotelIdentity.UniPantallaID = ds.BillingStatements(0).UniPantallaHotelID

                    dsData = ds
                    If Not dsData Is Nothing Then
                        ShowInfo(dsData)
                    End If

                Else
                    NoInvoice(PortalCulture.GetString("M0BT0000048")) 'Sorry, the Invoice was not found
                End If

            Else
                NoInvoice(PortalCulture.GetString("M0BT0000050")) 'Sorry, an error has happened
            End If
        End If
        Dim dsAux As BillingStatementDataSet

        If TypeOf (dsData) Is DataView Then
            Dim dvw As DataView = dsData
            dsAux = dvw.Table.DataSet
        Else
            dsAux = dsData
        End If
        btnUpdateConciliation.Visible = False
        btnFinalizeConciliation.Visible = False
        If Not dsAux Is Nothing Then

            If dsAux.ReservationCharges.Count > 0 AndAlso Now.Date <= dsAux.ReservationCharges(0).LimitToReconcile AndAlso Not GetIsInvoiceReservation() Then
                btnUpdateConciliation.Visible = True
                btnFinalizeConciliation.Visible = True
            End If
        End If
        hplinvoicebyperiod.NavigateUrl = Me.UrlPage(PaginaBase.pages.InvoiceByPeriod)
        If Not IsPostBack Then
            LoadPaymentMethod()
        End If
    End Sub
    Private Sub LoadPaymentMethod()

        Dim dt As DataTable = New Oz.UniBilling.Hotels.Business.PaymentMethod().SelectByCompanyID(MyBase.cInfoActual.Empresa, PortalCulture.GetIDCulture())
        dt.Columns.Add("Method_AccountNumber", GetType(String), "Method+ ' '+isnull(AccountNumber,'')")
        dt.Columns.Add("MethodAccountNumber", GetType(String), "Method+ ','+isnull(AccountNumber,'')")
        ddlPaymentPref.DataSource = dt
        ddlPaymentPref.DataTextField = "Method_AccountNumber"
        ddlPaymentPref.DataValueField = "MethodAccountNumber"

        Dim rowdef As DataRow() = dt.Select("Default=1")
        If rowdef.Length > 0 Then
            ddlPaymentPref.SelectedValue = rowdef(0)("Method").ToString() + "," + rowdef(0)("AccountNumber").ToString()
        End If


        ddlPaymentPref.DataBind()

        If dt.Rows.Count = 0 Then

            rbtnPaymentPref.Checked = False
            rbtnPaymentList.Checked = True

            rbtnPaymentList.Visible = False
            rbtnPaymentPref.Visible = False

            divPaymentPref.Style("display") = "none"
            divPaymentList.Style("display") = "inline-block"

        Else
            rbtnPaymentPref.Checked = True
            rbtnPaymentList.Checked = False

            rbtnPaymentPref.Visible = True
            rbtnPaymentList.Visible = True

            divPaymentPref.Style("display") = "inline-block"
            divPaymentList.Style("display") = "none"
        End If

        ddlPaymentList.SelectedIndex = 0
        txtAccountNumber.Enabled = False
        txtOther.Enabled = False
        txtAccountNumber.Text = ""
        txtOther.Text = ""
        txtAccountNumber.Style("background-color") = "#bbb"
        txtOther.Style("background-color") = "#bbb"

        dt = New Oz.UniBilling.Hotels.Business.PaymentMethod().GetAll()
        Dim newrow As DataRow = dt.NewRow()
        newrow(0) = 0
        newrow(1) = "No Identificado"
        newrow(2) = "Unidentified"
        dt.Rows.InsertAt(newrow, 0)
        ddlPaymentList.DataSource = dt
        If PortalCulture.GetIDCulture() = 1 Then
            ddlPaymentList.DataTextField = "MethodESP"
        Else
            ddlPaymentList.DataTextField = "MethodING"
        End If

        ddlPaymentList.DataValueField = "PaymentMethodID"
        ddlPaymentList.DataBind()

    End Sub

    Private Function GetIsInvoiceReservation() As Boolean
        Dim dsAux As BillingStatementDataSet
        If TypeOf (dsData) Is DataView Then
            Dim dvw As DataView = dsData
            dsAux = dvw.Table.DataSet
        Else
            dsAux = dsData
        End If

        If Not dsAux Is Nothing AndAlso dsAux.ReservationCharges.Count > 0 Then
            For Each dr As BillingStatementDataSet.BilledReservationChargesRow In dsAux.ReservationCharges(0).GetBilledReservationChargesRows
                If dr.Active Then
                    Return True
                End If
            Next
        End If
        Return False
    End Function

    Private Sub NoInvoice(ByVal msg As String)

        lblNoRvas.Visible = True
        lblNoRvas.Text = msg
        lblReservationsList.Visible = False

        lblActualDate.Text = Now.ToString("dddd, dd MMMM yyyy", PortalCulture.GetCulture)
        lblsInvoiceNumber.Visible = False
        lblsReferenceBank.Visible = False
        lblReferenceBank.Visible = False
        lblPeriod.Visible = False
        lblLimit.Visible = False
        lblExchange.Visible = False
        lblReservationsList.Visible = False
        lblsTotalRva.Visible = False
        lblOthers.Visible = False
        lblsGenerated.Visible = False
        lblsTotalCommissions.Visible = False
        lblsTotalOthers.Visible = False
        lblsSubtotal.Visible = False
        lblsTaxes.Visible = False
        lblsTotalToPay.Visible = False
        btnUpdateConciliation.Visible = False
        btnFinalizeConciliation.Visible = False

    End Sub

    Private Sub InvoiceConciliated(ByVal msg As String)
        lblInvoiceConciliated.Visible = True
        lblInvoiceConciliated.Text = msg
        btnUpdateConciliation.Enabled = False
        btnFinalizeConciliation.Disabled = True
    End Sub

    Private Sub ShowInfo(ByVal ds As BillingStatementDataSet)
        Dim ci As CultureInfo = PortalCulture.GetCulture
        Dim CompanyID = ds.BillingStatements(0).CompanyID
        Dim curr As String = "MXN"
        'If Not IsDBNull(ds.BillingStatements(0).BillingCurrencyCode) Then
        '    'curr = ds.BillingStatements(0).BillingCurrencyCode
        '    curr = "USD"
        'End If
        Dim days As Byte = DateTime.DaysInMonth(ds.BillingStatements(0).Year, ds.BillingStatements(0).Month)
        Dim StDate As New DateTime(ds.BillingStatements(0).Year, ds.BillingStatements(0).Month, 1)
        Dim EndDate As New DateTime(ds.BillingStatements(0).Year, ds.BillingStatements(0).Month, days)

        If ds.BillingStatements(0).IsReferenceNumberNull Then
            lblsInvoiceNumber.Visible = False
            lblInvoiceNumber.Visible = False
        Else
            lblInvoiceNumber.Text = ds.BillingStatements(0).ReferenceNumber
        End If
        lblReferenceBank.Text = BBVA_A36.BuildReferenceBillingStatement(ds.BillingStatements(0).RegistrationDate, ds.BillingStatements(0).CompanyID, ds.BillingStatements(0).BillingStatementID)
        Dim mes As New DateTime(ds.BillingStatements(0).Year, ds.BillingStatements(0).Month, 1)
        lblPeriodDate.Text = mes.ToString("y", ci)
        lblPeriodRange.Text = "[ " & StDate.ToString("dd MMMM yyyy", ci) & " - " & EndDate.ToString("dd MMMM yyyy", ci) & "]"

        lblActualDate.Text = Now.ToString("dddd, dd MMMM yyyy", PortalCulture.GetCulture)
        lblGenerated.Text = ds.BillingStatements(0).RegistrationDate.ToString("dd MMMM yyyy", ci)

        Dim drHot As Oz.UniBilling.DataSchemas.Hotels.BillingStatementDataSet.HotelsRow  '.BillingStatementDataSet.HotelsRow

        drHot = ds.Hotels.FindByCompanyID(CompanyID)

        lblHotelName.Text = drHot.CompanyName
        If Not drHot.IsStreetAddressNull() Then lblHotelAddress.Text = drHot.StreetAddress
        lblHotelCityState.Text = drHot.City
        If Not drHot.IsStateNull() Then lblHotelCityState.Text &= ", " & drHot.State
        lblHotelPhone.Text = drHot.PhoneNumber

        If ds.BillingStatements(0).BillingCurrencyCode = "USD" Then
            lblExchange.Visible = False
            lblMoneyExchange.Visible = False
        Else
            lblExchange.Visible = True
            lblMoneyExchange.Visible = True
        End If

        Dim subTotalOther As Decimal = 0
        Dim subTotalReservation As Decimal = 0
        Dim taxOther As Decimal = 0
        Dim taxReservation As Decimal = 0

        Dim Tax As Decimal = 0
        Dim SubTotal As Decimal = 0
        Dim Total As Decimal = 0

        '********************************************************
        If ds.FixedCharges.Count > 0 Or ds.ExtraCharges.Count > 0 Then
            If ds.FixedCharges.Count > 0 Then
                lblMoneyExchange.Text = FCurrency(CDec(ds.FixedCharges(0).MoneyExchangeRateMxn), 2)
                For i As Integer = 0 To ds.FixedCharges.Count - 1
                    subTotalOther += (ds.FixedCharges(i).Subtotal * ds.FixedCharges(i).MoneyExchangeRateMxn)
                    taxOther += (ds.FixedCharges(i).Taxes * ds.FixedCharges(i).MoneyExchangeRateMxn)
                Next

            End If
            If ds.ExtraCharges.Count > 0 Then
                lblMoneyExchange.Text = FCurrency(CDec(ds.ExtraCharges(0).MoneyExchangeRateMxn), 2)
                For i As Integer = 0 To ds.ExtraCharges.Count - 1
                    subTotalOther += (ds.ExtraCharges(i).Subtotal * ds.ExtraCharges(i).MoneyExchangeRateMxn)
                    taxOther += (ds.ExtraCharges(i).Taxes * ds.ExtraCharges(i).MoneyExchangeRateMxn)
                Next
            End If
            lblTotalOthers.Text = FCurrency(subTotalOther, 2) & " " & curr
            dgOthers.DataSource = CreateDsOthers(ds)
            dgOthers.DataBind()
            lblReservationsList.Visible = True
            lblsTotalRva.Visible = False
            lblTotalRva.Visible = False
            lblNoRvas.Visible = False
            btnUpdateConciliation.Visible = False
            btnFinalizeConciliation.Visible = False
        Else
            lblReservationsList.Visible = False
            lblOthers.Visible = False
            lblsTotalOthers.Visible = False
            lblTotalOthers.Visible = False
        End If

        '********************************************************
        If ds.ReservationCharges.Count > 0 Then

            lblMoneyExchange.Text = FCurrency(CDec(ds.ReservationCharges(0).MoneyExchangeRateMxn), 2)
            subTotalReservation = (ds.ReservationCharges(0).Subtotal * ds.ReservationCharges(0).MoneyExchangeRateMxn)
            taxReservation = (ds.ReservationCharges(0).Taxes * ds.ReservationCharges(0).MoneyExchangeRateMxn)

            lblTotalCommissions.Text = FCurrency(subTotalReservation, 2) & " " & curr

            lblLimitDate.Text = ds.ReservationCharges(0).LimitToReconcile.ToString("dd MMMM yyyy", ci)
            If ds.CommissionDetails.Count > 0 Then
                lblTotalRva.Text = ds.CommissionDetails.Count
                dgReservations.DataSource = ds
                dgReservations.DataMember = "CommissionDetails"
                dgReservations.Columns(2).HeaderText = PortalCulture.GetString("M0BT0000332") 'reservacion
                dgReservations.Columns(4).HeaderText = PortalCulture.GetString("M0BT0000042") 'fuente
                dgReservations.Columns(5).HeaderText = PortalCulture.GetString("M0BT0000069") 'tarifa
                dgReservations.Columns(6).HeaderText = PortalCulture.GetString("M0BT0000043") 'llegada


                dgReservations.Columns(8).Visible = Not IsCorporate
                dgReservations.Columns(9).Visible = Not IsCorporate
                dgReservations.Columns(10).Visible = Not IsCorporate
                dgReservations.Columns(11).Visible = Not IsCorporate


                dgReservations.DataBind()
            End If
            'If ds.ReservationCharges(0).Status = BillingStatementStatus.Generated Then
            '    lblMoneyExchange.Text = CDec(ds.ReservationCharges(0).MoneyExchangeRate).ToString("$ ###,###,###,##0.00")
            '    lblTotalCommissions.Text = FCurrency(ds.ReservationCharges(0).Subtotal) & " " & curr
            '    lblLimitDate.Text = ds.ReservationCharges(0).LimitToReconcile.ToString("dd MMMM yyyy", ci)
            '    If ds.CommissionDetails.Count > 0 Then
            '        lblTotalRva.Text = ds.CommissionDetails.Count
            '        dgReservations.DataSource = ds
            '        dgReservations.DataMember = "CommissionDetails"
            '        dgReservations.Columns(2).HeaderText = PortalCulture.GetString("M0BT0000332") 'reservacion
            '        dgReservations.Columns(4).HeaderText = PortalCulture.GetString("M0BT0000042") 'fuente
            '        dgReservations.Columns(5).HeaderText = PortalCulture.GetString("M0BT0000069") 'tarifa
            '        dgReservations.Columns(6).HeaderText = PortalCulture.GetString("M0BT0000043") 'llegada
            '        dgReservations.DataBind()
            '    End If
            '    Generated(True)
            'Else
            '    Generated(False)
            '    Tax -= ds.ReservationCharges(0).Taxes
            '    SubTotal -= ds.ReservationCharges(0).Subtotal
            '    Total -= ds.ReservationCharges(0).Total
            'End If
            btnUpdateConciliation.Visible = True
            btnFinalizeConciliation.Visible = True
        Else
            Generated(False)
        End If

        SubTotal = (subTotalOther + subTotalReservation)
        Tax = (taxOther + taxReservation)

        Total = (SubTotal + Tax)



        lblTotalToPay.Text = FCurrency(Total, 2) & " " & curr
        If ds.ReservationCharges.Count > 0 AndAlso ds.ReservationCharges(0).Status = Oz.BillingSystem.Common.BillingStatementStatus.Paid Then
            lblTotalToPay.Text &= " *" & PortalCulture.GetString("00585")
        End If


        lblSubtotal.Text = FCurrency(SubTotal, 2) & " " & curr
        lblTaxes.Text = FCurrency(Tax, 2) & " " & curr

    End Sub

    Private Function CreateDsOthers(ByVal ds As BillingStatementDataSet) As DataSet
        Dim dsOther As New DataSet
        Dim table As New DataTable
        Dim row As DataRow
        With table.Columns
            .Add("BillingStatementTransactionCode", GetType(System.Int32))
            .Add("SubTotal", GetType(System.Decimal))
            .Add("Taxes", GetType(System.Decimal))
            .Add("Total", GetType(System.Decimal))
        End With
        dsOther.Tables.Add(table)
        For i As Integer = 0 To ds.FixedCharges.Count - 1
            row = dsOther.Tables(0).NewRow
            row("BillingStatementTransactionCode") = ds.FixedCharges(i).BillingStatementTransactionCode
            row("Subtotal") = (ds.FixedCharges(i).Subtotal * ds.FixedCharges(i).MoneyExchangeRateMxn)
            row("Taxes") = (ds.FixedCharges(i).Taxes * ds.FixedCharges(i).MoneyExchangeRateMxn)
            row("Total") = (ds.FixedCharges(i).Total * ds.FixedCharges(i).MoneyExchangeRateMxn)
            dsOther.Tables(0).Rows.Add(row)
        Next
        For i As Integer = 0 To ds.ExtraCharges.Count - 1
            row = dsOther.Tables(0).NewRow
            row("BillingStatementTransactionCode") = ds.ExtraCharges(i).BillingStatementTransactionCode
            row("Subtotal") = (ds.ExtraCharges(i).Subtotal * ds.ExtraCharges(i).MoneyExchangeRateMxn)
            row("Taxes") = (ds.ExtraCharges(i).Taxes * ds.ExtraCharges(i).MoneyExchangeRateMxn)
            row("Total") = (ds.ExtraCharges(i).Total * ds.ExtraCharges(i).MoneyExchangeRateMxn)
            dsOther.Tables(0).Rows.Add(row)
        Next

        Return dsOther
    End Function

    Private Sub Generated(ByVal value As Boolean)

        lblsTotalCommissions.Visible = value
        lblTotalCommissions.Visible = value


        lblLimitDate.Visible = value
        lblLimit.Visible = value
        lblsTotalRva.Visible = value
        lblTotalRva.Visible = value
        lblReservationsList.Visible = value
        btnUpdateConciliation.Visible = value
        btnFinalizeConciliation.Visible = value
        lblNoRvas.Visible = Not value
    End Sub

    Private Sub btnUpdateConciliation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdateConciliation.Click
        Dim ds As BillingStatementDataSet

        If TypeOf (dsData) Is DataView Then
            Dim dvw As DataView = dsData
            ds = dvw.Table.DataSet
        Else
            ds = dsData
        End If

        If Not ds Is Nothing Then
            If ds.ReservationCharges.Count > 0 Then
                If Not ds.ReservationCharges(0).Status = Oz.BillingSystem.Common.BillingStatementStatus.Reconciled Then
                    Dim BillingMng As New BillingManager(Oz.UniBilling.Common.Security.cSecurity.GetIsser(1), AppSettings("UserID"), "ratemanager@univisit.com")
                    BillingMng.UpdateBillingStatements(ds)
                    Response.Redirect(GeRequestApplicationPath(String.Concat("/HotelAdministrator/Invoicing/ConciliationSummary.aspx?id=", ds.BillingStatements(0).BillingStatementID, "&bp=0")))
                Else
                    InvoiceConciliated(PortalCulture.GetString("M0BT0000054")) 'The Invoice Cannot Be Updated Because This Invoice Already Has Been Conciliated
                End If
            Else
                InvoiceConciliated(PortalCulture.GetString("M0BT0000054")) 'The Invoice Cannot Be Updated Because This Invoice Already Has Been Conciliated
            End If
        Else
            'Response.Redirect(String.Format("{0}Pages/SessionTimeout.aspx", PathUtil.VirtualCommonPath))
        End If
    End Sub

    Private Sub dgReservations_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgReservations.ItemDataBound
        Dim dsAux As BillingStatementDataSet

        If TypeOf (dsData) Is DataView Then
            Dim dvw As DataView = dsData
            dsAux = dvw.Table.DataSet
        Else
            dsAux = dsData
        End If

        Select Case e.Item.ItemType
            Case ListItemType.Header
                With e.Item
                    .Cells(3).Text = Server.HtmlDecode(PortalCulture.GetString("M0BT0000068")) 'tipo tarifa
                    .Cells(7).Text = Server.HtmlDecode(PortalCulture.GetString("M0BT0000071")) 'tarifa
                    .Cells(8).Text = Server.HtmlDecode(PortalCulture.GetString("M0BT0000072")) 'cargo
                    .Cells(9).Text = Server.HtmlDecode(PortalCulture.GetString("M0BT0000073")) 'ag
                    .Cells(10).Text = Server.HtmlDecode(PortalCulture.GetString("M0BT0000074")) 'com/ag
                    .Cells(11).Text = Server.HtmlDecode(PortalCulture.GetString("M0BT0000045")) 'total
                    .Cells(12).Text = Server.HtmlDecode(PortalCulture.GetString("M0BT0000046")) 'estado
                End With

            Case ListItemType.Item, ListItemType.AlternatingItem
                Dim ds As BillingStatementDataSet
                If TypeOf dgReservations.DataSource Is BillingStatementDataSet Then
                    ds = dgReservations.DataSource
                Else
                    ds = CType(CType(dgReservations.DataSource, DataView).Table.DataSet, BillingStatementDataSet)
                End If

                Dim table As BillingStatementDataSet.CommissionDetailsDataTable
                Dim rows() As BillingStatementDataSet.CommissionDetailsRow

                '*Dim moneyExchange As Decimal = ds.BillingStatements(0).MoneyExchangeRate
                Dim moneyExchangeMxn As Decimal = ds.ReservationCharges(0).MoneyExchangeRateMxn

                Dim ddl As DropDownList = e.Item.FindControl("ddlDgCorrectionOptions")
                If Not ddl Is Nothing Then
                    Dim prefijo As String = ddl.ClientID.Substring(0, ddl.ClientID.LastIndexOf("_") + 1)
                    ddl.Attributes("onChange") = "javascript:toggleChangeNights('" & prefijo & "','ddlDgCorrectionOptions','TrCheckIn','TrNights','txtDgNewCheckIn');"
                End If

                Dim txt As TextBox = e.Item.FindControl("txtDgNewCheckIn")
                Dim lit As Literal = e.Item.FindControl("litCalendar")
                If (Not lit Is Nothing) And (Not txt Is Nothing) Then
                    lit.Text = String.Format("<a hideFocus onclick=""gfPop.fPopCalendar1(document.getElementById('{0}'),getCalendarDateRange({1}, {2}));""href=""javascript:void(0)""><img src=""{3}"" border=0></a>", txt.ClientID, Formatting.GetJavaScriptNewDate(DateTime.Now.Subtract(New TimeSpan(1825, 0, 0, 0))), Formatting.GetJavaScriptNewDate(DateTime.Now), GeRequestApplicationPath("/Images/calendar.gif"))
                End If

                table = ds.Tables(dgReservations.DataMember)
                rows = table.Select("CommissionDetailID = " & DataBinder.Eval(e.Item.DataItem, "CommissionDetailID"))

                If rows.Length > 0 Then

                    Dim lblIdx As Label
                    lblIdx = e.Item.FindControl("Label4")
                    If Not lblIdx Is Nothing Then
                        lblIdx.Text = (dgReservations.CurrentPageIndex * dgReservations.PageSize) + (e.Item.ItemIndex + 1)
                    End If

                    Select Case rows(0).ReservationSourceCode

                        Case Hotels.ReservationSource.Portal, Hotels.ReservationSource.CallCenter
                            Dim lbl As Label = e.Item.FindControl("lblNumRvaDg")

                            lbl.Text = rows(0).ReservationNumber
                            lbl = e.Item.FindControl("lblNameDg")
                            lbl.Text = rows(0).CustomerName
                            lbl = e.Item.FindControl("lblRateTypeDg")
                            lbl.Text = clsFormat.GetDisplayName(CType(rows(0).RateType, Hotels.RateType))
                            lbl = e.Item.FindControl("lblSourceDg")
                            lbl.Text = IIf(rows(0).ReservationSourceCode = Hotels.ReservationSource.CallCenter, "Call Center", "Portal") '"Portal" 
                            lbl = e.Item.FindControl("lblAvRateDg")
                            '*lbl.Text = FCurrency(CDec(rows(0).AverageRate), ,2 )
                            lbl.Text = FCurrency((CDec(rows(0).AverageRate) * moneyExchangeMxn), 2)
                            lbl = e.Item.FindControl("lblCheckInDg")
                            lbl.Text = rows(0).CheckIn.ToString("dd/ MMM/ yy", PortalCulture.GetCulture)
                            lbl = e.Item.FindControl("lblNightsDg")
                            lbl.Text = IIf(CType(rows(0).Nights, Integer) > 1, rows(0).Nights & " " & PortalCulture.GetString("M0BT0000070"), rows(0).Nights & " " & PortalCulture.GetString("M0BT0000330"))
                            lbl = e.Item.FindControl("lblRvaTotalDg")
                            '*lbl.Text = FCurrency(CDec(rows(0).Total), , 2)
                            lbl.Text = FCurrency((CDec(rows(0).ReservationFare) * moneyExchangeMxn), 2)
                            lbl = e.Item.FindControl("lblOurChargeDg")
                            '*lbl.Text = FCurrency(CDec(rows(0).Amount), , )
                            lbl.Text = FCurrency((CDec(rows(0).Amount) * moneyExchangeMxn), 2)
                            lbl = e.Item.FindControl("lblTADg")
                            '*lbl.Text = FCurrency(CDec(rows(0).TACommission), , )
                            lbl.Text = FCurrency((CDec(rows(0).TACommission) * moneyExchangeMxn), 2)
                            lbl = e.Item.FindControl("lblComTADg")
                            '*lbl.Text = FCurrency(CDec(rows(0).ChargeOverTA), , )
                            lbl.Text = FCurrency((CDec(rows(0).ChargeOverTA) * moneyExchangeMxn), 2)
                            lbl = e.Item.FindControl("lblTotalDg")
                            '*lbl.Text = FCurrency(CDec(rows(0).Total), , )
                            lbl.Text = FCurrency((CDec(rows(0).Total) * moneyExchangeMxn), 2)
                            lbl = e.Item.FindControl("lblStatusDg")
                            lbl.Text = clsFormat.GetDisplayName(CType(rows(0).Status, Hotels.CommissionDetailStatus))

                            Dim btnHtml As HtmlInputButton = e.Item.FindControl("BTnConciliarDg")
                            btnHtml.Visible = False

                            If Not btnHtml Is Nothing Then

                                If Now.Date <= dsAux.ReservationCharges(0).LimitToReconcile And Not GetIsInvoiceReservation() Then
                                    btnHtml.Visible = True
                                    Dim prefijo As String = btnHtml.ClientID.Substring(0, btnHtml.ClientID.LastIndexOf("_") + 1)
                                    btnHtml.Attributes("onClick") = "javascript:Conciliation('" & prefijo & "','" & rows(0).ReservationNumber & "','" & Formatting.FormatDate(CDate(rows(0).CheckIn).Date, "MM/dd/yyyy") & "','" & rows(0).CheckIn.ToString("dd MMMM yyyy", PortalCulture.GetCulture) & "','" & rows(0).CheckIn.AddDays(rows(0).Nights).ToString("dd MMMM yyyy", PortalCulture.GetCulture) & "','" & rows(0).Nights & "','" & rows(0).CommissionDetailID & "','" & rows(0).Status & "','divToCorrectData', 'block');"
                                End If

                            End If
                            Dim btnHtmlCancel As HtmlInputButton = e.Item.FindControl("BTnHDgCancelar")
                            If Not btnHtmlCancel Is Nothing Then
                                Dim prefijo As String = btnHtmlCancel.ClientID.Substring(0, btnHtmlCancel.ClientID.LastIndexOf("_") + 1)
                                btnHtmlCancel.Attributes("onclick") = "javascript:toggleConciliate('" & prefijo & "','divToCorrectData', 'none','" & rows(0).Status & "');"
                            End If

                        Case Hotels.ReservationSource.Gds, Hotels.ReservationSource.Ads, Hotels.ReservationSource.Ids
                            Dim lbl As Label = e.Item.FindControl("lblNumRvaDg")
                            If rows(0).ReservationSourceCode = Hotels.ReservationSource.Ids Then
                                Dim sma As String = rows(0).ReservationNumber
                            End If
                            lbl.Text = rows(0).ReservationNumber
                            lbl = e.Item.FindControl("lblNameDg")
                            lbl.Text = rows(0).CustomerName
                            lbl = e.Item.FindControl("lblRateTypeDg")
                            lbl.Text = clsFormat.GetDisplayName(CType(rows(0).RateType, Hotels.RateType))
                            lbl = e.Item.FindControl("lblSourceDg")
                            lbl.Text = IIf(rows(0).ReservationSourceCode = Hotels.ReservationSource.Gds, "Gds", If(rows(0).ReservationSourceCode = Hotels.ReservationSource.Ads, "Ads", "Ids"))
                            lbl = e.Item.FindControl("lblAvRateDg")
                            '*lbl.Text = FCurrency(CDec(rows(0).AverageRate), , )
                            lbl.Text = FCurrency((CDec(rows(0).AverageRate) * moneyExchangeMxn), 2)
                            lbl = e.Item.FindControl("lblCheckInDg")
                            lbl.Text = rows(0).CheckIn.ToString("dd/ MMM/ yy", PortalCulture.GetCulture)
                            lbl = e.Item.FindControl("lblNightsDg")
                            lbl.Text = IIf(CType(rows(0).Nights, Byte) > 1, rows(0).Nights & " " & PortalCulture.GetString("M0BT0000070"), rows(0).Nights & " " & PortalCulture.GetString("M0BT0000330"))
                            lbl = e.Item.FindControl("lblRvaTotalDg")
                            '*lbl.Text = FCurrency(CDec(rows(0).Total), , )
                            lbl.Text = FCurrency((CDec(rows(0).Total) * moneyExchangeMxn), 2)
                            lbl = e.Item.FindControl("lblOurChargeDg")
                            '*lbl.Text = FCurrency(CDec(rows(0).Amount), , )
                            lbl.Text = FCurrency((CDec(rows(0).Amount) * moneyExchangeMxn), 2)
                            lbl = e.Item.FindControl("lblTADg")
                            '*lbl.Text = FCurrency(CDec(rows(0).TACommission), , )
                            lbl.Text = FCurrency((CDec(rows(0).TACommission) * moneyExchangeMxn), 2)
                            lbl = e.Item.FindControl("lblComTADg")
                            '*lbl.Text = FCurrency(CDec(rows(0).ChargeOverTA), , )
                            lbl.Text = FCurrency((CDec(rows(0).ChargeOverTA) * moneyExchangeMxn), 2)
                            lbl = e.Item.FindControl("lblTotalDg")
                            '*lbl.Text = FCurrency(CDec(rows(0).Total), , )
                            lbl.Text = FCurrency((CDec(rows(0).Total) * moneyExchangeMxn), 2)
                            lbl = e.Item.FindControl("lblStatusDg")
                            lbl.Text = clsFormat.GetDisplayName(CType(rows(0).Status, Hotels.CommissionDetailStatus))

                            Dim btnHtml As HtmlInputButton = e.Item.FindControl("BTnConciliarDg")

                            btnHtml.Visible = False

                            If Not btnHtml Is Nothing Then
                                If Now.Date <= dsAux.ReservationCharges(0).LimitToReconcile AndAlso Not GetIsInvoiceReservation() Then
                                    btnHtml.Visible = True
                                    Dim prefijo As String = btnHtml.ClientID.Substring(0, btnHtml.ClientID.LastIndexOf("_") + 1)
                                    btnHtml.Attributes("onClick") = "javascript:Conciliation('" & prefijo & "','" & rows(0).ReservationNumber & "','" & Formatting.FormatDate(CDate(rows(0).CheckIn).Date, "MM/dd/yyyy") & "','" & rows(0).CheckIn.ToString("dd MMMM yyyy", PortalCulture.GetCulture) & "','" & rows(0).CheckIn.AddDays(rows(0).Nights).ToString("dd MMMM yyyy", PortalCulture.GetCulture) & "','" & rows(0).Nights & "','" & rows(0).CommissionDetailID & "','" & rows(0).Status & "','divToCorrectData', 'block');"
                                End If

                            End If
                            Dim btnHtmlCancel As HtmlInputButton = e.Item.FindControl("BTnHDgCancelar")
                            If Not btnHtmlCancel Is Nothing Then
                                Dim prefijo As String = btnHtmlCancel.ClientID.Substring(0, btnHtmlCancel.ClientID.LastIndexOf("_") + 1)
                                btnHtmlCancel.Attributes("onclick") = "javascript:toggleConciliate('" & prefijo & "','divToCorrectData', 'none','" & rows(0).Status & "');"
                            End If

                        Case Hotels.ReservationSource.UniPantalla
                            Dim lbl As Label = e.Item.FindControl("lblNumRvaDg")

                            If Not rows(0).IsNull("ReservationNumber") Then lbl.Text = rows(0).ReservationNumber Else lbl.Text = ""

                            lbl = e.Item.FindControl("lblNameDg")
                            lbl.Text = rows(0).CustomerName
                            lbl = e.Item.FindControl("lblRateTypeDg")
                            lbl.Text = clsFormat.GetDisplayName(CType(rows(0).RateType, Hotels.RateType))
                            lbl = e.Item.FindControl("lblSourceDg")
                            lbl.Text = "UniPantalla"
                            lbl = e.Item.FindControl("lblAvRateDg")
                            '*lbl.Text = FCurrency(CDec(rows(0).AverageRate), , )
                            lbl.Text = FCurrency((CDec(rows(0).AverageRate) * moneyExchangeMxn), 2)
                            lbl = e.Item.FindControl("lblCheckInDg")
                            lbl.Text = rows(0).CheckIn.ToString("dd/ MMM/ yy", PortalCulture.GetCulture)
                            lbl = e.Item.FindControl("lblNightsDg")
                            lbl.Text = IIf(CType(rows(0).Nights, Byte) > 1, rows(0).Nights & " " & PortalCulture.GetString("M0BT0000070"), rows(0).Nights & " " & PortalCulture.GetString("M0BT0000330"))
                            lbl = e.Item.FindControl("lblRvaTotalDg")
                            '*lbl.Text = FCurrency(CDec(rows(0).Total), , )
                            lbl.Text = FCurrency((CDec(rows(0).Total) * moneyExchangeMxn), 2)
                            lbl = e.Item.FindControl("lblOurChargeDg")
                            '*lbl.Text = FCurrency(CDec(rows(0).Amount), , )
                            lbl.Text = FCurrency((CDec(rows(0).Amount) * moneyExchangeMxn), 2)
                            lbl = e.Item.FindControl("lblTADg")
                            '*lbl.Text = FCurrency(CDec(rows(0).TACommission), , )
                            lbl.Text = FCurrency((CDec(rows(0).TACommission) * moneyExchangeMxn), 2)
                            lbl = e.Item.FindControl("lblComTADg")
                            '*lbl.Text = FCurrency(CDec(rows(0).ChargeOverTA), , )
                            lbl.Text = FCurrency((CDec(rows(0).ChargeOverTA) * moneyExchangeMxn), 2)
                            lbl = e.Item.FindControl("lblTotalDg")
                            '*lbl.Text = FCurrency(CDec(rows(0).Total), , )
                            lbl.Text = FCurrency((CDec(rows(0).Total) * moneyExchangeMxn), 2)
                            lbl = e.Item.FindControl("lblStatusDg")
                            lbl.Text = clsFormat.GetDisplayName(CType(rows(0).Status, Hotels.CommissionDetailStatus))

                            Dim btnHtml As HtmlInputButton = e.Item.FindControl("BTnConciliarDg")
                            btnHtml.Visible = False

                            If Not btnHtml Is Nothing Then
                                If Now.Date <= dsAux.ReservationCharges(0).LimitToReconcile AndAlso Not GetIsInvoiceReservation() Then
                                    btnHtml.Visible = True
                                    Dim prefijo As String = btnHtml.ClientID.Substring(0, btnHtml.ClientID.LastIndexOf("_") + 1)
                                    btnHtml.Attributes("onClick") = "javascript:Conciliation('" & prefijo & "','" & rows(0).ReservationID & "','" & Formatting.FormatDate(CDate(rows(0).CheckIn).Date, "MM/dd/yyyy") & "','" & rows(0).CheckIn.ToString("dd MMMM yyyy", PortalCulture.GetCulture) & "','" & rows(0).CheckIn.AddDays(rows(0).Nights).ToString("dd MMMM yyyy", PortalCulture.GetCulture) & "','" & rows(0).Nights & "','" & rows(0).CommissionDetailID & "','" & rows(0).Status & "','divToCorrectData', 'block');"
                                End If
                            End If
                            Dim btnHtmlCancel As HtmlInputButton = e.Item.FindControl("BTnHDgCancelar")
                            If Not btnHtmlCancel Is Nothing Then
                                Dim prefijo As String = btnHtmlCancel.ClientID.Substring(0, btnHtmlCancel.ClientID.LastIndexOf("_") + 1)
                                btnHtmlCancel.Attributes("onclick") = "javascript:toggleConciliate('" & prefijo & "','divToCorrectData', 'none','" & rows(0).Status & "');"
                            End If

                    End Select
                End If

        End Select

    End Sub

    Private Sub dgReservations_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgReservations.ItemCommand
        Select Case e.CommandName

            Case "UpdateRvaInfo"
                Dim hdn As HtmlInputHidden = e.Item.FindControl("hdnDgCommissionDetailID")
                Dim CommissionDetailID As Integer = hdn.Value
                Dim table As BillingStatementDataSet.CommissionDetailsDataTable
                Dim rows() As BillingStatementDataSet.CommissionDetailsRow
                '*agregada:
                Dim BillingMng As New BillingManager(Oz.UniBilling.Common.Security.cSecurity.GetIsser(1), AppSettings("UserID"), "ratemanager@univisit.com")

                If Not dsData Is Nothing Then
                    If TypeOf (dsData) Is DataView Then
                        Dim dvw As DataView = dsData
                        If dvw.Count > 0 Then
                            table = dvw.Table
                            rows = table.Select("CommissionDetailID = " & CommissionDetailID)

                            Dim ddl As DropDownList = e.Item.FindControl("ddlDgCorrectionOptions")
                            Dim txtCheckIn As TextBox = e.Item.FindControl("txtDgNewCheckIn")
                            Dim txtNights As TextBox = e.Item.FindControl("txtDgNewNights")

                            If rows.Length > 0 Then
                                rows(0).Status = ddl.SelectedValue
                                rows(0).CheckIn = IIf(txtCheckIn.Text.Trim = String.Empty, rows(0).CheckIn, DateTime.Parse(txtCheckIn.Text, New CultureInfo("en-US")))
                                rows(0).Nights = IIf(txtNights.Text.Trim = String.Empty, rows(0).Nights, txtNights.Text)
                                '*agregada:
                                rows(0).UserID = (New AuthUser).Usuario 'Page.User.Identity.Name

                                'actualiza la tabla CommissionDetails con los valores conciliados
                                '*BillingManager.ProcessConciliation(rows(0), CType(Page.User.Identity.Name, Integer), Nothing)
                                BillingMng.CalculateCharges(rows(0))

                                ShowInfo(dvw.Table.DataSet)
                                dgReservations.DataSource = dsData
                                dgReservations.DataMember = "CommissionDetails"
                                dgReservations.DataBind()
                            Else
                                InvoiceConciliated(PortalCulture.GetString("M0BT0000054")) 'The Invoice Cannot Be Updated Because This Invoice Already Has Been Conciliated
                            End If

                            txtCheckIn.Text = ""
                            txtNights.Text = ""
                            ddl.SelectedIndex = 0

                        Else
                            InvoiceConciliated(PortalCulture.GetString("M0BT0000055")) 'An Error Has Happened, please Go Back to Pending Invoices to Conciliate and Select a Correct Period to Conciliate
                        End If
                    Else
                        If dsData.CommissionDetails.Count > 0 Then
                            table = dsData.Tables("CommissionDetails")
                            rows = table.Select("CommissionDetailID = " & CommissionDetailID)

                            Dim ddl As DropDownList = e.Item.FindControl("ddlDgCorrectionOptions")
                            Dim txtCheckIn As TextBox = e.Item.FindControl("txtDgNewCheckIn")
                            Dim txtNights As TextBox = e.Item.FindControl("txtDgNewNights")

                            If rows.Length > 0 Then

                                rows(0).Status = ddl.SelectedValue
                                rows(0).CheckIn = IIf(txtCheckIn.Text.Trim = String.Empty, rows(0).CheckIn, DateTime.Parse(txtCheckIn.Text, New CultureInfo("en-US")))
                                rows(0).Nights = IIf(txtNights.Text.Trim = String.Empty, rows(0).Nights, txtNights.Text)
                                'agregada:
                                'rows(0).UserID = Page.User.Identity.Name
                                rows(0).UserID = (New AuthUser).Usuario

                                'actualiza la tabla CommissionDetails con los valores conciliados
                                '*BillingManager.ProcessConciliation(rows(0), CType(Page.User.Identity.Name, Integer), Nothing)
                                BillingMng.CalculateCharges(rows(0))

                                ShowInfo(dsData)
                                dgReservations.DataSource = dsData
                                dgReservations.DataMember = "CommissionDetails"
                                dgReservations.DataBind()
                            Else
                                InvoiceConciliated(PortalCulture.GetString("M0BT0000054")) 'The Invoice Cannot Be Updated Because This Invoice Already Has Been Conciliated
                            End If

                            txtCheckIn.Text = ""
                            txtNights.Text = ""
                            ddl.SelectedIndex = 0

                        Else
                            InvoiceConciliated(PortalCulture.GetString("M0BT0000055")) 'An Error Has Happened, please Go Back to Pending Invoices to Conciliate and Select a Correct Period to Conciliate
                        End If
                    End If
                Else
                    'Response.Redirect(String.Format("{0}Pages/SessionTimeout.aspx", PathUtil.VirtualCommonPath))
                End If

        End Select

    End Sub

    Private Sub dgReservations_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgReservations.PageIndexChanged
        If Not dsData Is Nothing Then
            dgReservations.CurrentPageIndex = e.NewPageIndex
            dgReservations.DataSource = dsData
            If TypeOf (dsData) Is BillingStatementDataSet Then
                dgReservations.DataMember = "CommissionDetails"
            End If
            dgReservations.DataBind()
        Else
            'Response.Redirect(String.Format("{0}Pages/SessionTimeout.aspx", PathUtil.VirtualCommonPath))
        End If
    End Sub

    Private Sub lnkToConciliate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkToConciliate.Click
        Response.Redirect("Invoices.aspx")
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        lblTitle.Text = PortalCulture.GetString("00586")
        lblsInvoiceNumber.Text = PortalCulture.GetString("M0BT0000027", True)
        lblsReferenceBank.Text = PortalCulture.GetString("00793", True)
        lblPeriod.Text = PortalCulture.GetString("M0BT0000008", True)
        lblsGenerated.Text = PortalCulture.GetString("M0BT0000010", True)
        lblLimit.Text = PortalCulture.GetString("M0BT0000011", True)
        lblExchange.Text = PortalCulture.GetString("M0BT0000030", True)
        lblReservationsList.Text = PortalCulture.GetString("M0BT0000031")
        lblsTotalRva.Text = PortalCulture.GetString("M0BT0000032", True)
        lblInvoiceConciliated.Text = PortalCulture.GetString("M0BT0000033")
        lblNoRvas.Text = PortalCulture.GetString("M0BT0000034")
        lblOthers.Text = PortalCulture.GetString("M0BT0000035")
        lblsTotalCommissions.Text = PortalCulture.GetString("M0BT0000036", True)
        lblsTotalOthers.Text = PortalCulture.GetString("M0BT0000037", True)
        lblsSubtotal.Text = PortalCulture.GetString("M0BT0000038", True)
        lblsTaxes.Text = PortalCulture.GetString("M0BT0000039", True)
        lblsTotalToPay.Text = PortalCulture.GetString("M0BT0000040", True)
        btnUpdateConciliation.Text = PortalCulture.GetString("M0BT0000028")
        btnUpdateConciliation.ToolTip = PortalCulture.GetString("M0BT0000079")
        btnFinalizeConciliation.Value = PortalCulture.GetString("M0BT0000029")
        lnkToConciliate.Text = PortalCulture.GetString("01344")


        hypTaskList.Text = PortalCulture.GetString("M0BT0000148")
        '*hypTaskList.NavigateUrl = String.Format("{0}MainInvoicing.aspx?cid={1}",  "/Hotel_Administrator/Invoicing/", CompanyID)
        hypTaskList.NavigateUrl = String.Format("{0}MainInvoicing.aspx?cid={1}", GeRequestApplicationPath("/HotelAdministrator/Invoicing/"), HotelIdentity.CompanyID)

        lblConfirmTitle.Text = PortalCulture.GetString("M0BT0000075")
        lblConfirmMsg.Text = String.Format(PortalCulture.GetString("M0BT0000076"), "<br>")
        hplYes.Text = PortalCulture.GetString("M0BT0000077")
        hplNo.Text = PortalCulture.GetString("M0BT0000078")
        hplinvoicebyperiod.Text = PortalCulture.GetString("00584")
        If IsCorporate Then
            tbTotales.Visible = False
        End If
    End Sub

    Private Sub dgReservations_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgReservations.ItemCreated

        Select Case e.Item.ItemType
            Case ListItemType.Item, ListItemType.AlternatingItem
                Dim btnHtml As HtmlInputButton = e.Item.FindControl("btnConciliarDg")
                btnHtml.Value = PortalCulture.GetString("M0BT0000056")
                Dim lbl As Label = e.Item.FindControl("lblsDgNumRva")
                lbl.Text = PortalCulture.GetString("M0BT0000057", True)
                lbl = e.Item.FindControl("lblsDgCheckIn")
                lbl.Text = PortalCulture.GetString("M0BT0000043", True)
                lbl = e.Item.FindControl("lblsDgCheckOut")
                lbl.Text = PortalCulture.GetString("M0BT0000044", True)
                lbl = e.Item.FindControl("lblhDgStatus")
                lbl.Text = PortalCulture.GetString("M0BT0000046", True)

                lbl = e.Item.FindControl("lblTitleData")
                lbl.Text = PortalCulture.GetString("01511")

                Dim ddl As DropDownList = e.Item.FindControl("ddlDgCorrectionOptions")
                ddl.Items(0).Text = PortalCulture.GetString("M0BT0000052")
                ddl.Items(1).Text = PortalCulture.GetString("M0BT0000053")
                'ddl.Items(2).Text = PortalCulture.GetString("M0BT0000304")'duplicado
                lbl = e.Item.FindControl("lblDgNewCheckIn")
                lbl.Text = PortalCulture.GetString("M0BT0000043", True)
                lbl = e.Item.FindControl("lblDgNewNights")
                lbl.Text = PortalCulture.GetString("M0BT0000058", True)
                Dim btn As Button = e.Item.FindControl("btnDgUpdate")
                btn.Text = PortalCulture.GetString("M0BT0000059")
                btnHtml = e.Item.FindControl("btnHDgCancelar")
                btnHtml.Value = PortalCulture.GetString("M0BT0000060")
        End Select

    End Sub

    Private Sub dgOthers_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgOthers.ItemCreated
        If e.Item.ItemType = ListItemType.Header Then
            With e.Item
                .Cells(1).Text = Server.HtmlDecode(PortalCulture.GetString("M0BT0000009"))
                .Cells(2).Text = Server.HtmlDecode(PortalCulture.GetString("M0BT0000038"))
                .Cells(3).Text = Server.HtmlDecode(PortalCulture.GetString("M0BT0000039"))
                .Cells(4).Text = Server.HtmlDecode(PortalCulture.GetString("M0BT0000045"))
            End With

        End If
    End Sub

    Private Sub HeaderStyle(ByVal SortExpression As String)

        dgReservations.Columns(2).HeaderStyle.ForeColor = dgReservations.Columns(2).HeaderStyle.ForeColor.White
        dgReservations.Columns(4).HeaderStyle.ForeColor = dgReservations.Columns(4).HeaderStyle.ForeColor.White
        dgReservations.Columns(5).HeaderStyle.ForeColor = dgReservations.Columns(5).HeaderStyle.ForeColor.White
        dgReservations.Columns(6).HeaderStyle.ForeColor = dgReservations.Columns(6).HeaderStyle.ForeColor.White



        Select Case SortExpression
            Case "CustomerName"
                dgReservations.HeaderStyle.CssClass = "Button"
                dgReservations.Columns(2).HeaderStyle.Font.Bold = True
                dgReservations.Columns(2).HeaderStyle.ForeColor = dgReservations.Columns(2).HeaderStyle.ForeColor.Orange

            Case "ReservationSourceCode"
                dgReservations.HeaderStyle.CssClass = "Button"
                dgReservations.Columns(4).HeaderStyle.Font.Bold = True
                dgReservations.Columns(4).HeaderStyle.ForeColor = dgReservations.Columns(4).HeaderStyle.ForeColor.Orange

            Case "AverageRate"
                dgReservations.HeaderStyle.CssClass = "Button"
                dgReservations.Columns(5).HeaderStyle.Font.Bold = True
                dgReservations.Columns(5).HeaderStyle.ForeColor = dgReservations.Columns(5).HeaderStyle.ForeColor.Orange

            Case "CheckIn"
                dgReservations.HeaderStyle.CssClass = "Button"
                dgReservations.Columns(6).HeaderStyle.Font.Bold = True
                dgReservations.Columns(6).HeaderStyle.ForeColor = dgReservations.Columns(6).HeaderStyle.ForeColor.Orange

        End Select

    End Sub

    Private Sub dgReservations_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgReservations.SortCommand
        Dim dv As DataView

        If Not dsData Is Nothing Then
            If TypeOf (dsData) Is DataView Then
                dv = dsData
            Else
                dv = New DataView(CType(dsData, BillingStatementDataSet).CommissionDetails)
                dsData = dv
            End If

            If SortOrder(e.SortExpression) = String.Empty Then
                SortOrder(e.SortExpression) = "ASC"
            End If

            Dim order As String = SortOrder(e.SortExpression)
            dv.Sort = e.SortExpression & " " & order
            dgReservations.DataSource = dv
            HeaderStyle(e.SortExpression)
            dgReservations.DataBind()

            If order = "ASC" Then
                SortOrder(e.SortExpression) = "DESC"
            Else
                SortOrder(e.SortExpression) = "ASC"
            End If
        End If

    End Sub

    Private Sub hplYes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hplYes.Click
        Dim ds As BillingStatementDataSet

        If TypeOf (dsData) Is DataView Then
            Dim dvw As DataView = dsData
            ds = dvw.Table.DataSet
        Else
            ds = dsData
        End If

        If Not ds Is Nothing Then
            If ds.BillingStatements.Count > 0 Then
                If ds.ReservationCharges.Count > 0 Then
                    If Not ds.ReservationCharges(0).Status = Oz.BillingSystem.Common.BillingStatementStatus.Reconciled Then
                        Dim BillingMng As New BillingManager(Oz.UniBilling.Common.Security.cSecurity.GetIsser(1), AppSettings("UserID"), "ratemanager@univisit.com")

                        Dim strErr As String
                        Dim fn As String

                        Dim dsInvoices As Oz.UniBilling.DataSchemas.Sat.CfdDataSet()
                        'If Not IsCorporate Then
                        dsInvoices = BillingMng.FinishReconciliation(ds, True, strErr, IIf(ds.BillingStatements(0).BillingCurrencyCode.Trim() = "", "MXN", ds.BillingStatements(0).BillingCurrencyCode), getDialogMethod(), getDialogAccountNumber())
                        'End If
                        If (strErr IsNot Nothing AndAlso strErr <> String.Empty) Then
                            WriteLog(strErr)
                        End If
                        'System.Threading.Thread.CurrentPrincipal = PrincipalSecure


                        If Not dsInvoices Is Nothing AndAlso dsInvoices.Length > 0 AndAlso dsInvoices(0).Comprobante.Count > 0 Then
                            fn = String.Format("{0}-{1}", dsInvoices(0).Comprobante(0).serie, dsInvoices(0).Comprobante(0).folio)
                            Response.Redirect(GeRequestApplicationPath(String.Concat("/HotelAdministrator/Invoicing/ConciliationSummary.aspx?id=", ds.BillingStatements(0).BillingStatementID, "&bp=1", "&fn=", fn)))

                        Else
                            Response.Redirect(GeRequestApplicationPath(String.Concat("/HotelAdministrator/Invoicing/ConciliationSummary.aspx?id=", ds.BillingStatements(0).BillingStatementID, "&bp=1")))
                        End If

                    Else
                        lblInvoiceConciliated.Visible = True
                        btnUpdateConciliation.Enabled = False
                        btnFinalizeConciliation.Disabled = True
                    End If
                End If
            Else
                lblInvoiceConciliated.Visible = True
                btnUpdateConciliation.Enabled = False
                btnFinalizeConciliation.Disabled = True
            End If
        End If
    End Sub

    Private Sub WriteLog(ByVal strError As String)
        Try


            If strError <> "" Then
                Dim PathFile As String = HttpContext.Current.Request.PhysicalApplicationPath & "Portal\Logs\"
                Dim FileName As String = PathFile & Now.Day.ToString("00") & Now.Month.ToString("00") & Now.Year.ToString("0000") & ".log"
                Dim FileError As New System.IO.FileInfo(FileName)

                If Not Directory.Exists(PathFile) Then
                    Directory.CreateDirectory(PathFile)
                End If
                If Not FileError.Exists Then
                    Dim fs As FileStream = File.Create(FileName)
                    fs.Close()
                End If
                FileError = New System.IO.FileInfo(FileName)
                If FileError.Exists Then
                    FileOpen(1, FileName, OpenMode.Append)
                    Print(1, Now.Hour & ":" & Now.Minute & ":" & Now.Second & ": " & strError)
                    FileClose(1)
                End If
            End If
        Catch

        End Try
    End Sub

    Private Sub dgOthers_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgOthers.ItemDataBound
        Select Case e.Item.ItemType
            Case ListItemType.Item, ListItemType.AlternatingItem
                Dim drTransaction As BillingStatementDataSet.BillingStatementTransactionsRow

                drTransaction = dsData.BillingStatementTransactions.FindByBillingStatementTransactionCode(CInt(DataBinder.Eval(e.Item.DataItem, "BillingStatementTransactionCode")))
                If Not drTransaction Is Nothing Then
                    Dim lbl As Label = e.Item.FindControl("lblConcept")
                    If Not lbl Is Nothing Then
                        lbl.Text = drTransaction.TransactionName 'clsFormat.GetDisplayName(CType(drTransaction.BillingStatementTransactionCode, BillingStatementTransaction))
                    End If
                End If

        End Select
    End Sub

    Public Function getDialogMethod() As String
        If rbtnPaymentPref.Checked Then
            Return ddlPaymentPref.SelectedValue.Split(",")(0)
        ElseIf ddlPaymentList.SelectedValue <> "1" Then
            Return ddlPaymentList.SelectedItem.Text
        Else
            Return txtOther.Text
        End If
    End Function

    Public Function getDialogAccountNumber() As String
        If rbtnPaymentPref.Checked Then
            Return ddlPaymentPref.SelectedValue.Split(",")(1)
        ElseIf ddlPaymentList.SelectedValue <> "0" And ddlPaymentList.SelectedValue <> "2" Then
            Return txtAccountNumber.Text
        Else
            Return ""
        End If
    End Function
End Class
