Partial Class HotelInvoiceDetails
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

    Protected HotelInvoiceDetailViewer1 As HotelInvoiceDetailViewer

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        Dim x As Integer = 1
        If isUserChain AndAlso x = 2 Then

        Else
            'Normal
            If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)
        End If

        Me.HeaderPanel = ctrlHeader.Usuario.uHotelAdministrador

        If Not IsPostBack Then
            If (Not Request.QueryString("id") Is Nothing) AndAlso IsNumeric(Request.QueryString("id")) Then
                HotelInvoiceDetailViewer1.InvoiceID = Convert.ToInt32(Request.QueryString("id"))
            End If
        End If

    End Sub

    Private Sub lnkToConciliate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkToConciliate.Click
        Response.Redirect("../Invoices.aspx")
    End Sub

    Private Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        lblTitle.Text = PortalCulture.GetString("M0BT0000170")
        'chkPrintable.Text = PortalCulture.GetString("M0BT0000169")

        hypTaskList.Text = PortalCulture.GetString("M0BT0000148")
        hypTaskList.NavigateUrl = String.Format("{0}MainInvoicing.aspx?cid={1}", GeRequestApplicationPath("/HotelAdministrator/Invoicing/"), MyBase.cInfoActual.Empresa)

        hypGoBack.Text = PortalCulture.GetString("M0BT0000328")
        hypGoBack.NavigateUrl = String.Format("{0}PaymentInvoicesSummary.aspx", GeRequestApplicationPath("/HotelAdministrator/Invoicing/Payment/"))
        lnkToConciliate.Text = PortalCulture.GetString("01344")
    End Sub

    'Private Sub chkPrintable_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    'CtrlHeader1.Visible = Not chkPrintable.Checked
    '    CtrlFooter1.Visible = Not chkPrintable.Checked
    '    tdTitle.Style("DISPLAY") = IIf(Not chkPrintable.Checked, "block", "none")
    '    hypGoBack.Visible = Not chkPrintable.Checked
    '    hypTaskList.Visible = Not chkPrintable.Checked
    'End Sub

End Class
