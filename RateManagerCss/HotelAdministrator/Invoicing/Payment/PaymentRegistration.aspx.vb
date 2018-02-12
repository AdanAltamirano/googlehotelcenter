Imports System.Configuration.ConfigurationManager
Imports System.Text
Imports System.Globalization
Imports Oz.UniBilling.Common.Hotels
Imports Oz.UniBilling.Common.Business
Imports Oz.UniBilling.DataSchemas.Currency

Imports Oz.UniBilling.Hotels.Business
Imports Oz.UniBilling.DataSchemas.Hotels

Imports Oz.UniBilling.Banking.Business
Imports Oz.UniBilling.DataSchemas.Banking

Imports ReferencesSystem


Partial Class PaymentRegistration
    Inherits PaginaBase

#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label

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

    Public Property CompanyID() As Integer
        Get
            Return ViewState("CompanyID")
        End Get
        Set(ByVal Value As Integer)
            ViewState("CompanyID") = Value
        End Set
    End Property

    Public Property UserID() As Integer
        Get
            Return ViewState("UserID")
        End Get
        Set(ByVal Value As Integer)
            ViewState("UserID") = Value
        End Set
    End Property

    Public Property CurrencyCode() As String
        Get
            Return ViewState("CurrencyCode")
        End Get
        Set(ByVal Value As String)
            ViewState("CurrencyCode") = Value
        End Set
    End Property

    Public Property dsCurrency() As CurrencyDataSet
        Get
            Return ViewState("dsCurrency")
        End Get
        Set(ByVal Value As CurrencyDataSet)
            ViewState("dsCurrency") = Value
        End Set
    End Property

#End Region

    Protected WithEvents ctrHotelsUserChain1 As ctrHotelsUserChain
    Private AccountsScript As New StringBuilder
    Private ReferencesScript As New StringBuilder
    Private dsBilling As New BillingStatementDataSet
    Private dsBankAccount As New BankAccountDataSet
    Private dsPayment As New PaymentDataSet
    Private HotelIdentity As New HotelIdentifier



    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ctrHotelsUserChain1.Visible = False
        Dim x As Integer = 1
        If MyBase.isUserChain AndAlso x = 2 Then 'Especial para user chain
            ctrHotelsUserChain1.Visible = True
            Me.HeaderPanel = ctrlHeader.Usuario.uHotelAdministrador
            UserID = MyBase.Usuario

            ctrHotelsUserChain1.LoadHotels()
            HotelIdentity.CompanyID = ctrHotelsUserChain1.SelectedCompanyID


            Dim BillingMng As New BillingManager(Oz.UniBilling.Common.Security.cSecurity.GetIsser(1), AppSettings("UserID"), "ratemanager@univisit.com")
            Dim dsBS As BillingStatementDataSet = New BillingStatementDataSet
            BillingMng.GetBillingStatements(HotelIdentity, dsBS)
            fillReference(dsBS)

            Dim HotelMng As New HotelManager
            HotelMng.SearchHotelByCompanyID(HotelIdentity.CompanyID, dsBilling)
            If dsBilling.Hotels.Count > 0 Then
                If Not dsBilling.Hotels(0).IsUniPantallaHotelIDNull Then HotelIdentity.UniPantallaID = dsBilling.Hotels(0).UniPantallaHotelID
                If Not dsBilling.Hotels(0).IsPortalHotelIDNull Then HotelIdentity.PortalID = dsBilling.Hotels(0).PortalHotelID
            End If


            If Not IsPostBack Or dsBankAccount Is Nothing Then
                Dim BankingMng As New BankingManager
                BankingMng.GetAllBankAccounts(dsBankAccount, True)

                GetCurrencyCode()

                If dsBankAccount.Banks.Rows.Count > 0 Then
                    loadPaymentType()
                    ddlBankName.DataSource = dsBankAccount.Banks
                    ddlBankName.DataTextField = "BankName"
                    ddlBankName.DataValueField = "BankID"
                    ddlBankName.DataBind()
                    If dsBankAccount.BankAccounts.Rows.Count > 0 Then
                        fillArrayData(dsBankAccount)
                    Else
                        'no hay cuentas para ese banco
                        fillArrayData(dsBankAccount)
                        ddlAccountNumber.Items.Clear()
                        ddlAccountNumber.Items.Add(New ListItem(PortalCulture.GetString("M0BT0000181"), -1))
                    End If
                Else
                    'no hay bancos disponibles
                    fillArrayData(dsBankAccount)
                    ddlBankName.Items.Clear()
                    ddlBankName.Items.Add(New ListItem(PortalCulture.GetString("M0BT0000180"), -1))
                End If

                ddlSourceBank.Items.Clear()
                ddlSourceBank.Items.Add(New ListItem("Banamex", "Banamex"))
                ddlSourceBank.Items.Add(New ListItem("Bancomer", "Bancomer"))
                ddlSourceBank.Items.Add(New ListItem("Banorte", "Banorte"))
                ddlSourceBank.Items.Add(New ListItem("HSBC", "HSBC"))
                ddlSourceBank.Items.Add(New ListItem("Santander Serfin", "Santander Serfin"))
                ddlSourceBank.Items.Add(New ListItem("Scotia-Bank", "Scotia-Bank"))
                ddlSourceBank.Items.Add(New ListItem(PortalCulture.GetString("M0BT0000335"), "Otro"))
            Else
                fillArrayData(dsBankAccount)
            End If


        Else ' normal
            If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)

            Me.HeaderPanel = ctrlHeader.Usuario.uHotelAdministrador
            UserID = MyBase.Usuario
            CompanyID = MyBase.cInfoActual.Empresa
            HotelIdentity.CompanyID = MyBase.cInfoActual.Empresa

            Dim BillingMng As New BillingManager(Oz.UniBilling.Common.Security.cSecurity.GetIsser(1), AppSettings("UserID"), "ratemanager@univisit.com")
            Dim dsBS As BillingStatementDataSet = New BillingStatementDataSet
            BillingMng.GetBillingStatements(HotelIdentity, dsBS)
            fillReference(dsBS)

            Dim HotelMng As New HotelManager
            HotelMng.SearchHotelByCompanyID(HotelIdentity.CompanyID, dsBilling)
            If dsBilling.Hotels.Count > 0 Then
                If Not dsBilling.Hotels(0).IsUniPantallaHotelIDNull Then HotelIdentity.UniPantallaID = dsBilling.Hotels(0).UniPantallaHotelID
                If Not dsBilling.Hotels(0).IsPortalHotelIDNull Then HotelIdentity.PortalID = dsBilling.Hotels(0).PortalHotelID
            End If



            If Not IsPostBack Or dsBankAccount Is Nothing Then
                Dim BankingMng As New BankingManager
                BankingMng.GetAllBankAccounts(dsBankAccount, True)

                GetCurrencyCode()

                If dsBankAccount.Banks.Rows.Count > 0 Then
                    loadPaymentType()
                    ddlBankName.DataSource = dsBankAccount.Banks
                    ddlBankName.DataTextField = "BankName"
                    ddlBankName.DataValueField = "BankID"
                    ddlBankName.DataBind()
                    If dsBankAccount.BankAccounts.Rows.Count > 0 Then
                        fillArrayData(dsBankAccount)
                    Else
                        'no hay cuentas para ese banco
                        fillArrayData(dsBankAccount)
                        ddlAccountNumber.Items.Clear()
                        ddlAccountNumber.Items.Add(New ListItem(PortalCulture.GetString("M0BT0000181"), -1))
                    End If
                Else
                    'no hay bancos disponibles
                    fillArrayData(dsBankAccount)
                    ddlBankName.Items.Clear()
                    ddlBankName.Items.Add(New ListItem(PortalCulture.GetString("M0BT0000180"), -1))
                End If

                ddlSourceBank.Items.Clear()
                ddlSourceBank.Items.Add(New ListItem("Banamex", "Banamex"))
                ddlSourceBank.Items.Add(New ListItem("Bancomer", "Bancomer"))
                ddlSourceBank.Items.Add(New ListItem("Banorte", "Banorte"))
                ddlSourceBank.Items.Add(New ListItem("HSBC", "HSBC"))
                ddlSourceBank.Items.Add(New ListItem("Santander Serfin", "Santander Serfin"))
                ddlSourceBank.Items.Add(New ListItem("Scotia-Bank", "Scotia-Bank"))
                ddlSourceBank.Items.Add(New ListItem(PortalCulture.GetString("M0BT0000335"), "Otro"))
            Else
                fillArrayData(dsBankAccount)
            End If
        End If



     

    End Sub
    Private Sub fillReference(ByVal ds As BillingStatementDataSet)
        If Not ds Is Nothing Then
            Dim c As Integer = ds.BillingStatements.Count
            ReferencesScript.Append("<script language='javascript'>")
            ReferencesScript.Append("var ReferencesArray = new Array(" & c & ");")
            Dim i As Integer = 0
            For Each dr As BillingStatementDataSet.BillingStatementsRow In ds.BillingStatements
                Dim payment As Boolean = True
                Dim FixedCharges() As BillingStatementDataSet.FixedChargesRow
                Dim ExtraCharges() As BillingStatementDataSet.ExtraChargesRow
                Dim ReservationCharges() As BillingStatementDataSet.ReservationChargesRow

                FixedCharges = dr.GetFixedChargesRows
                ExtraCharges = dr.GetExtraChargesRows
                ReservationCharges = dr.GetReservationChargesRows



                If Not FixedCharges Is Nothing Then
                    For Each drF As BillingStatementDataSet.FixedChargesRow In FixedCharges
                        If drF.Status < 2 Then ' pagada
                            payment = False
                        End If
                    Next
                End If

                If Not ExtraCharges Is Nothing Then
                    For Each drE As BillingStatementDataSet.ExtraChargesRow In ExtraCharges
                        If drE.Status < 2 Then ' pagada
                            payment = False
                        End If
                    Next
                End If

                If Not ReservationCharges Is Nothing Then
                    For Each drR As BillingStatementDataSet.ReservationChargesRow In ReservationCharges
                        If drR.Status < 2 Then ' pagada
                            payment = False
                        End If
                    Next
                End If








                If Not payment Then
                    ReferencesScript.AppendFormat("ReferencesArray[" & i & "]= '" & BBVA_A36.BuildReferenceBillingStatement(dr.RegistrationDate, dr.CompanyID, dr.BillingStatementID) & "';")
                    i = i + 1
                End If
            Next



            ReferencesScript.Append("</script>")
        End If
    End Sub



    Private Sub fillArrayData(ByVal ds As BankAccountDataSet)
        Dim c As Integer = ds.BankAccounts.Rows.Count

        AccountsScript.Append("<script language='javascript'>")
        AccountsScript.Append("var AccountsArray = new Array(" & c & ");")

        For i As Integer = 0 To ds.BankAccounts.Rows.Count - 1
            AccountsScript.AppendFormat("AccountsArray[" & i & "]= new Array(3);")
            AccountsScript.AppendFormat("AccountsArray[" & i & "][0] = '" & ds.BankAccounts.Rows(i).Item("BankID") & "';")
            AccountsScript.AppendFormat("AccountsArray[" & i & "][1] = '" & ds.BankAccounts.Rows(i).Item("BankAccountID") & "';")
            AccountsScript.AppendFormat("AccountsArray[" & i & "][2] = '" & CType(ds.BankAccounts.Rows(i).Item("AccountNumber"), String) & "';")
            AccountsScript.AppendFormat("AccountsArray[" & i & "][3] = '" & ds.BankAccounts.Rows(i).Item("AccountOwner") & "';")
            AccountsScript.AppendFormat("AccountsArray[" & i & "][4] = '" & ds.BankAccounts.Rows(i).Item("CLABE") & "';")

            Dim Curr As String = ds.BankAccounts.Rows(i).Item("CurrencyCode")
            Dim rows() As CurrencyDataSet.CurrenciesRow
            rows = dsCurrency.Currencies.Select("CurrencyCode = '" & ds.BankAccounts.Rows(i).Item("CurrencyCode") & "'")
            If rows.Length > 0 Then
                Curr = rows(0).ShortName
            End If
            AccountsScript.AppendFormat("AccountsArray[" & i & "][5] = '" & PortalCulture.GetString("M0BT0000310") & " " & Curr & "';")
            AccountsScript.AppendFormat("AccountsArray[" & i & "][6] = '" & ds.BankAccounts.Rows(i).Item("CurrencyCode") & "';")
        Next

        AccountsScript.Append("</script>")

    End Sub

    Private Sub loadPaymentType()
        ddlPaymentType.Items.Clear()
        ddlPaymentType.Items.Add(New ListItem(PortalCulture.GetString("M0BT0000178"), Oz.BillingSystem.Common.TransactionType.Deposit))
        ddlPaymentType.Items.Add(New ListItem(PortalCulture.GetString("M0BT0000179"), Oz.BillingSystem.Common.TransactionType.Transference))

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        lblTitle.Text = PortalCulture.GetString("M0BT0000177")

        lblBankName.Text = PortalCulture.GetString("M0BT0000200", True)
        lblAccountNumber.Text = PortalCulture.GetString("M0BT0000192", True)
        lblsAccountName.Text = PortalCulture.GetString("M0BT0000309", True)
        lblsClabe.Text = PortalCulture.GetString("M0BT0000193", True)
        lblSourceBank.Text = PortalCulture.GetString("M0BT0000311", True)
        lblTransactionNumber.Text = PortalCulture.GetString("M0BT0000201", True)
        lblsDateTime.Text = PortalCulture.GetString("M0BT0000312", True)
        lblAmount.Text = PortalCulture.GetString("M0BT0000203", True) & "  "
        btnRegister.Value = PortalCulture.GetString("M0BT0000204")

        cvAmountFormat.Text = "[" & PortalCulture.GetString("M0BT0000227") & "]"

        lblConfirmationTitle.Text = PortalCulture.GetString("M0BT0000177")
        lblConfirmationPrompt.Text = PortalCulture.GetString("M0BT0000262")
        lnkYes.Text = PortalCulture.GetString("M0BT0000077")
        hypNo.Text = PortalCulture.GetString("M0BT0000078")

        hplGoToTaskList.Text = PortalCulture.GetString("M0BT0000148")
        hplGoToTaskList.NavigateUrl = String.Format("{0}MainInvoicing.aspx?cid={1}", GeRequestApplicationPath("/HotelAdministrator/Invoicing/"), HotelIdentity.CompanyID)

        txtDate.Text = Oz.BillingSystem.Common.Formatting.FormatDate(Now, "MM/dd/yyyy")

        lblMsg.Text = PortalCulture.GetString("M0BT0000329")
        lblMsg2.Text = PortalCulture.GetString("M0BT0000331")

        lblReferenceBank.Text = PortalCulture.GetString("00793", True)
        cvReferenceBank.ErrorMessage = PortalCulture.GetString("00794")

        RegisterStartupScript("Accounts", AccountsScript.ToString)
        RegisterStartupScript("References", ReferencesScript.ToString)

        ddlBankName.Attributes("onChange") = String.Format("javascript:ddlChangeBank('{0}');", PortalCulture.GetString("M0BT0000181"))
        ddlAccountNumber.Attributes("onChange") = String.Format("javascript:ddlChangeAccounts();")
        litCalendar.Text = String.Format("<a hideFocus onclick=""gfPop.fPopCalendar1(document.getElementById('{0}'),getCalendarDateRange({1}, {2}));""href=""javascript:void(0)""><img src=""{3}"" border=0></a>", txtDate.ClientID, Oz.BillingSystem.Common.Formatting.GetJavaScriptNewDate(DateTime.Now.Subtract(New TimeSpan(1825, 0, 0, 0))), Oz.BillingSystem.Common.Formatting.GetJavaScriptNewDate(DateTime.Now), GeRequestApplicationPath("/Images/calendar.gif"))
        scriptContainer.InnerHtml = "<script language=""javascript"">ddlChangeBank('" & PortalCulture.GetString("M0BT0000181") & "');"
        scriptContainer.InnerHtml &= "ddlChangeAccounts();</script>"

    End Sub

    Private Function getHour(ByVal hour, ByVal min, ByVal time) As String
        Select Case time
            Case "a.m."
                If hour = 12 Then
                    Return "00:" & min & ":00"
                End If
                Return hour & ":" & min & ":00"
            Case "p.m."
                If hour = 12 Then
                    Return hour & ":" & min & ":00"
                End If
                Return hour + 12 & ":" & min & ":00"
        End Select
    End Function

    Protected Overrides Sub OnPreRender(ByVal e As System.EventArgs)
        MyBase.OnPreRender(e)
        hypNo.Attributes("onclick") = "javascript:document.getElementById('divConfirmation').style.display = 'none';"

        txtSourceBank.Text = ddlSourceBank.SelectedValue
        txtSourceBank.Style("Display") = "none"
        ddlSourceBank.Attributes("onChange") = "javascript:onTransactionBankChanged('" & ddlSourceBank.ClientID & "', '" & txtSourceBank.ClientID & "');"

    End Sub

    Private Sub fillDs()
        Try


            Dim paymentDate As String = txtDate.Text & " " & getHour(txtHour.Text, txtMinutes.Text, ddlTime.SelectedValue)
            Dim row As PaymentDataSet.PaymentsRow
            row = dsPayment.Payments.NewRow

            Dim BankingMng As New BankingManager
            BankingMng.GetAllBankAccounts(dsPayment, True)

            With row
                .RegistrationDate = Now
                .CompanyID = HotelIdentity.CompanyID
                If Not (HotelIdentity.PortalID = -1) Then .PortalHotelID = HotelIdentity.PortalID
                If Not (HotelIdentity.UniPantallaID = -1) Then .UniPantallaHotelID = HotelIdentity.UniPantallaID
                .CompanyName = dsBilling.Hotels(0).CompanyName
                .BillingName = dsBilling.Hotels(0).BillingName
                .UserID = UserID
                .BankAccountID = hdnAccountNumber.Value
                .TransactionBankName = txtSourceBank.Text
                .TransactionReference = txtTransactionNumber.Text
                .TransactionType = ddlPaymentType.SelectedValue
                .TransactionDate = DateTime.Parse(paymentDate, New CultureInfo("en-US"))
                .CurrencyCode = ddlCurrency.SelectedValue
                .ReferenceBank = txtReferenceBank.Text.ToUpper() 'referencia bancaria
                Dim dv As New DataView(dsCurrency.MoneyExchangeHistory)
                dv.RowFilter = "CurrencyCode = '" & ddlCurrency.SelectedValue & "'"
                .MoneyExchangeRate = dv(0).Item("Rate")
                .Amount = Decimal.Parse(txtAmount.Text.Trim)
                .AmountUsd = Decimal.Divide(.Amount, .MoneyExchangeRate)
                .Verified = False
            End With

            dsPayment.Payments.AddPaymentsRow(row)
        Catch ex As Exception

        End Try

    End Sub

    Private Function GetCurrencyCode() As Boolean
        Dim MoneyMng As New MoneyManager
        dsCurrency = MoneyMng.GetMoneyExchangeHistory(DateTime.Now)
        ddlCurrency.DataSource = dsCurrency.MoneyExchangeHistory
        ddlCurrency.DataTextField = "CurrencyCode"
        ddlCurrency.DataValueField = "CurrencyCode"
        ddlCurrency.DataBind()

        If Not ddlCurrency.Items.FindByValue("MXN") Is Nothing Then
            ddlCurrency.SelectedValue = "MXN"
        End If

    End Function

    Private Sub lnkYes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkYes.Click
        Dim PaymentMng As New PaymentManager
        If Page.IsValid Then
            fillDs()
            PaymentMng.RegisterPayments(dsPayment)
            Dim id As Long = dsPayment.Payments(0).PaymentID
            Response.Redirect(GeRequestApplicationPath(String.Concat("/HotelAdministrator/Invoicing/Payment/PaymentRegistered.aspx?id=", id)))
        End If
    End Sub



    Private Sub cvReferenceBank_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvReferenceBank.ServerValidate
        args.IsValid = BBVA_A36.ValidateReference(args.Value.ToString())
    End Sub

    Private Sub ctrHotelsUserChain1_onSelected(ByVal CompanyID As Integer) Handles ctrHotelsUserChain1.onSelected



        If Not IsPostBack Or dsBankAccount Is Nothing Then
            Dim BankingMng As New BankingManager
            BankingMng.GetAllBankAccounts(dsBankAccount, True)

            GetCurrencyCode()

            If dsBankAccount.Banks.Rows.Count > 0 Then
                loadPaymentType()
                ddlBankName.DataSource = dsBankAccount.Banks
                ddlBankName.DataTextField = "BankName"
                ddlBankName.DataValueField = "BankID"
                ddlBankName.DataBind()
                If dsBankAccount.BankAccounts.Rows.Count > 0 Then
                    fillArrayData(dsBankAccount)
                Else
                    'no hay cuentas para ese banco
                    fillArrayData(dsBankAccount)
                    ddlAccountNumber.Items.Clear()
                    ddlAccountNumber.Items.Add(New ListItem(PortalCulture.GetString("M0BT0000181"), -1))
                End If
            Else
                'no hay bancos disponibles
                fillArrayData(dsBankAccount)
                ddlBankName.Items.Clear()
                ddlBankName.Items.Add(New ListItem(PortalCulture.GetString("M0BT0000180"), -1))
            End If

            ddlSourceBank.Items.Clear()
            ddlSourceBank.Items.Add(New ListItem("Banamex", "Banamex"))
            ddlSourceBank.Items.Add(New ListItem("Bancomer", "Bancomer"))
            ddlSourceBank.Items.Add(New ListItem("Banorte", "Banorte"))
            ddlSourceBank.Items.Add(New ListItem("HSBC", "HSBC"))
            ddlSourceBank.Items.Add(New ListItem("Santander Serfin", "Santander Serfin"))
            ddlSourceBank.Items.Add(New ListItem("Scotia-Bank", "Scotia-Bank"))
            ddlSourceBank.Items.Add(New ListItem(PortalCulture.GetString("M0BT0000335"), "Otro"))
        Else
            fillArrayData(dsBankAccount)
        End If

    End Sub
End Class
