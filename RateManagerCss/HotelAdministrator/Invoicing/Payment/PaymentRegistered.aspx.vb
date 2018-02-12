Imports Oz.UniBilling.DataSchemas.Hotels
Imports Oz.UniBilling.Hotels.Business
Imports Oz.UniBilling.Banking.Business
Imports Oz.UniBilling.Common.Hotels
Imports Oz.BillingSystem.Common

Partial Class PaymentRegistered
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

    Public Property CompanyID() As Integer
        Get
            Return ViewState("CompanyID")
        End Get
        Set(ByVal Value As Integer)
            ViewState("CompanyID") = Value
        End Set
    End Property

    Public Property PaymentID() As Long
        Get
            Return ViewState("PaymentID")
        End Get
        Set(ByVal Value As Long)
            ViewState("PaymentID") = Value
        End Set
    End Property

#End Region

    Private ds As New PaymentDataSet
    Dim HotelIdentity As New HotelIdentifier

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)
        Me.HeaderPanel = ctrlHeader.Usuario.uHotelAdministrador

        If Not IsNothing(Request.QueryString("id")) AndAlso IsNumeric(Request.QueryString("id")) Then
            PaymentID = Long.Parse(Request.QueryString("id"))
            If Not PaymentID = -1 AndAlso Not PaymentID = 0 Then
                CompanyID = MyBase.cInfoActual.Empresa
                HotelIdentity.CompanyID = CompanyID

                Dim PaymentMng As New PaymentManager
                PaymentMng.GetPaymentInfoByPaymentID(HotelIdentity, PaymentID, ds)

                If ds.Payments.Count > 0 Then
                    lblMsg.Text = PortalCulture.GetString("M0BT0000209")
                    PaymentInfo()
                Else
                    NoPayment()
                End If

            Else
                NoPayment()
            End If
        Else
            NoPayment()
        End If

    End Sub

    Private Sub NoPayment()
        lblMsg.Text = PortalCulture.GetString("M0BT0000207")
        lblTitle2.Visible = False
        Table3.Visible = False
    End Sub

    Private Sub PaymentInfo()
        Dim rowsAccount() As PaymentDataSet.BankAccountsRow
        rowsAccount = ds.BankAccounts.Select("BankAccountID = '" & ds.Payments(0).BankAccountID & "'")
        If rowsAccount.Length > 0 Then
            lblAccountNumber.Text = rowsAccount(0).AccountNumber
            lblAccountName.Text = rowsAccount(0).AccountOwner
        Else
            lblAccountNumber.Text = "---"
            lblAccountName.Text = "---"
        End If
        Dim rowsBank() As PaymentDataSet.BanksRow
        rowsBank = ds.Banks.Select("BankID = '" & rowsAccount(0).BankID & "'")
        If rowsBank.Length > 0 Then
            lblBankName.Text = rowsBank(0).BankName
        Else
            lblBankName.Text = "---"
        End If
        If ds.Payments(0).TransactionType = Oz.BillingSystem.Common.TransactionType.Deposit Then
            lblPaymentType.Text = PortalCulture.GetString("M0BT0000178")
        ElseIf ds.Payments(0).TransactionType = Oz.BillingSystem.Common.TransactionType.Transference Then
            lblPaymentType.Text = PortalCulture.GetString("M0BT0000179")
        End If
        If Not ds.Payments(0).IsReferenceBankNull() Then
            lblReferenceBank.Text = ds.Payments(0).ReferenceBank
        Else
            lblReferenceBank.Text = String.Empty

        End If


        lblTransactionNumber.Text = ds.Payments(0).TransactionReference
        lblSourceBank.Text = ds.Payments(0).TransactionBankName
        lblAmount.Text = FCurrency(ds.Payments(0).Amount, 2) & " " & ds.Payments(0).CurrencyCode

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        lblTitle.Text = PortalCulture.GetString("M0BT0000205")

        lblTitle2.Text = PortalCulture.GetString("M0BT0000208")
        lblsBankName.Text = PortalCulture.GetString("M0BT0000200", True)
        lblsAccountNumber.Text = PortalCulture.GetString("M0BT0000192", True)
        lblsAccountName.Text = PortalCulture.GetString("M0BT0000309", True)
        lblsPaymentType.Text = PortalCulture.GetString("M0BT0000202", True)
        lblsTransactionNumber.Text = PortalCulture.GetString("M0BT0000201", True)
        lblsSourceBank.Text = PortalCulture.GetString("M0BT0000311", True)
        lblsAmount.Text = PortalCulture.GetString("M0BT0000203", True)
        lblsReferenceBank.Text = PortalCulture.GetString("00793", True)


        hplGoTo.Text = PortalCulture.GetString("M0BT0000148")
        hplGoTo.NavigateUrl = String.Format("{0}MainInvoicing.aspx?cid={1}", GeRequestApplicationPath("/HotelAdministrator/Invoicing/"), CompanyID)

    End Sub

End Class
