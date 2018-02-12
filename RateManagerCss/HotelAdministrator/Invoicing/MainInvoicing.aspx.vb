Partial Class MainInvoicing
    Inherits PaginaBase

#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label

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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)
        Me.HeaderPanel = ctrlHeader.Usuario.uHotelAdministrador

        If Not IsNothing(Request.QueryString("cid")) AndAlso IsNumeric(Request.QueryString("cid")) Then
            CompanyID = Integer.Parse(Request.QueryString("cid"))
        End If
        DataBind()
        
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender

        lblMainTitle.Text = PortalCulture.GetString("M000644")
        'lblTitle.Text = PortalCulture.GetString("M0BT0000145")
        lblConciliationTitle.Text = PortalCulture.GetString("M0BT0000213")
        hypInvoicesToConciliate.Text = PortalCulture.GetString("M0BT0000146")
        lblPaymentTitle.Text = PortalCulture.GetString("M0BT0000214")
        hypInvoicesPendingToPay.Text = PortalCulture.GetString("M0BT0000305")
        hypPaymentRegistration.Text = PortalCulture.GetString("M0BT0000212")
        hypPaymentRegistered.Text = PortalCulture.GetString("M0BT0000302")
        lblInvoicesTitle.Text = PortalCulture.GetString("M0BT0000306")
        hypPrintInvoices.Text = PortalCulture.GetString("M0BT0000307")

        hypInvoicesToConciliate.NavigateUrl = GeRequestApplicationPath("/HotelAdministrator/Invoicing/InvoicesToConciliate.aspx")
        hypInvoicesPendingToPay.NavigateUrl = GeRequestApplicationPath("/HotelAdministrator/Invoicing/Payment/PaymentInvoicesSummary.aspx")
        hypPaymentRegistration.NavigateUrl = GeRequestApplicationPath("/HotelAdministrator/Invoicing/Payment/PaymentRegistration.aspx")
        hypPaymentRegistered.NavigateUrl = GeRequestApplicationPath("/HotelAdministrator/Invoicing/Payment/PaymentsRegisteredByHotel.aspx")
        hypPrintInvoices.NavigateUrl = GeRequestApplicationPath("/HotelAdministrator/Invoicing/Invoice/PrintInvoice.aspx")

    End Sub
End Class
