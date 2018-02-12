Imports Oz.UniBilling.Common.Hotels
Imports Oz.UniBilling.Hotels.Business
Imports Oz.UniBilling.DataSchemas.Hotels

Partial Class HotelBalanceViewer
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

    Public Property CompanyID() As Integer
        Get
            If (ViewState("CompanyID") Is Nothing) OrElse Not IsNumeric(ViewState("CompanyID")) Then
                Return -1
            End If

            Return Integer.Parse(ViewState("CompanyID").ToString())
        End Get
        Set(ByVal Value As Integer)
            ViewState("CompanyID") = Value
        End Set
    End Property

    Public Property ReferenceNumber() As String
        Get
            If ViewState("ReferenceNumber") Is Nothing Then
                Return ""
            End If
            Return ViewState("ReferenceNumber")
        End Get
        Set(ByVal Value As String)
            ViewState("ReferenceNumber") = Value
        End Set
    End Property

#End Region

    Private dsPayment As New PaymentDataSet
    Private HotelIdentity As New HotelIdentifier

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        lblTitle.Text = PortalCulture.GetString("M0BT0000218")
        lblReferenceNumberCaption.Text = PortalCulture.GetString("M0BT0000313", True)
        lblDueAmountCaption.Text = PortalCulture.GetString("M0BT0000334", True)
        lblPaymentsCaption.Text = PortalCulture.GetString("M0BT0000333", True)
        lblFinalAmountCaption.Text = PortalCulture.GetString("M0BT0000217", True)
    End Sub

    Public Sub LoadData(ByVal companyID As Integer, ByVal refNumber As String, Optional ByVal portalHotelID As Integer = -1, Optional ByVal uniPantallaHotelID As Integer = -1)
        Me.CompanyID = companyID
        HotelIdentity.CompanyID = companyID
        HotelIdentity.PortalID = portalHotelID
        HotelIdentity.UniPantallaID = uniPantallaHotelID
        Me.ReferenceNumber = refNumber
        LoadData()
    End Sub

    Public Sub LoadData()
        'Dim Debit As Decimal = 0
        'Dim Credit As Decimal = 0
        'Dim currCode As String = ""

        'currCode = BillingManager.GetBillingStatementCurrencyCode(ReferenceNumber)
        'dsPayment = PaymentManager.CreatePaymentDataSet()
        'PaymentManager.LoadPaymentTransactions(dsPayment, , ReferenceNumber, CompanyID, PortalHotelID, UniPantallaHotelID, , , , , )
        'If dsPayment.PaymentTransactions.Count > 0 Then
        '    For i As Integer = 0 To dsPayment.PaymentTransactions.Count - 1
        '        Debit += dsPayment.PaymentTransactions(i).Debit
        '        Credit += dsPayment.PaymentTransactions(i).Credit
        '    Next
        'End If

        'lblReferenceNumber.Text = ReferenceNumber
        'lblDueAmount.Text = FCurrency(Credit, , currCode)
        'lblPayments.Text = FCurrency(Debit, , currCode)
        'lblFinalAmount.Text = FCurrency(Credit - Debit, , currCode)

    End Sub

End Class
