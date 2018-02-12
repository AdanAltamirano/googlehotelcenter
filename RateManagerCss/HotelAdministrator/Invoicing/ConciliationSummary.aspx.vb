Imports Oz.UniBilling.Hotels.Business
Imports Oz.UniBilling.DataSchemas.Hotels
Imports Oz.UniBilling.Common.Hotels
Imports Oz.BillingSystem.Common
Imports System.Configuration.ConfigurationManager


Partial Class ConciliationSummary
    Inherits PaginaBase

#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblFinalBalanceCurrencyCode As System.Web.UI.WebControls.Label
    Protected WithEvents lblFinalBalance As System.Web.UI.WebControls.Label
    Protected WithEvents lblFinalBalanceCaption As System.Web.UI.WebControls.Label
    Protected WithEvents lblAccumulatedBalanceCurrencyCode As System.Web.UI.WebControls.Label
    Protected WithEvents lblAccumulatedBalance As System.Web.UI.WebControls.Label
    Protected WithEvents lblAccumulatedBalanceCaption As System.Web.UI.WebControls.Label
    Protected WithEvents lblCurrentBalanceCurrencyCode As System.Web.UI.WebControls.Label
    Protected WithEvents lblCurrentBalance As System.Web.UI.WebControls.Label
    Protected WithEvents lblCurrentBalanceCaption As System.Web.UI.WebControls.Label
    Protected WithEvents lblTitle As System.Web.UI.WebControls.Label
    Protected WithEvents tblHotelBalanceViewer As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents trCurrentBalance As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trAccumulatedBalance As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trFinalBalance As System.Web.UI.HtmlControls.HtmlTableRow

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

    Private Property InvoiceID() As Long
        Get
            Return ViewState("InvoiceID")
        End Get
        Set(ByVal Value As Long)
            ViewState("InvoiceID") = Value
        End Set
    End Property


    Private Property FileName() As String
        Get
            Return ViewState("FileName")
        End Get
        Set(ByVal Value As String)
            ViewState("FileName") = Value
        End Set
    End Property

#End Region

    Protected balanceViewer As HotelBalanceViewer
    Dim ds As New BillingStatementDataSet
    Protected HotelIdentity As New HotelIdentifier

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)
        Me.HeaderPanel = ctrlHeader.Usuario.uHotelAdministrador
        lnkInvoice.Visible = False
        lblInvoice.Visible = False

        If Not IsNothing(Request.QueryString("id")) AndAlso Not IsNothing(Request.QueryString("bp")) Then
            InvoiceID = Request.QueryString("id")
            If Not IsNothing(Request.QueryString("fn")) Then FileName = Request.QueryString("fn") Else FileName = ""

            Dim previousPage As Byte = Request.QueryString("bp")

            Dim BillingMng As New BillingManager(Oz.UniBilling.Common.Security.cSecurity.GetIsser(1), AppSettings("UserID"), "ratemanager@univisit.com")

            BillingMng.GetBillingStatements(InvoiceID, ds)
            If ds.BillingStatements.Count > 0 Then
                HotelIdentity.CompanyID = ds.BillingStatements(0).CompanyID
                If Not ds.BillingStatements(0).IsPortalHotelIDNull Then HotelIdentity.PortalID = ds.BillingStatements(0).PortalHotelID
                If Not ds.BillingStatements(0).IsUniPantallaHotelIDNull Then HotelIdentity.UniPantallaID = ds.BillingStatements(0).UniPantallaHotelID
                '**CHECAR SALDOS
                'balanceViewer.LoadData(HotelIdentity.CompanyID, HotelIdentity.CompanyID & "-" & InvoiceID, HotelIdentity.PortalID, HotelIdentity.UniPantallaID)
                balanceViewer.Visible = False
                '**
            Else
                HotelIdentity.CompanyID = -1
                HotelIdentity.PortalID = -1
                HotelIdentity.UniPantallaID = -1
            End If

            lblMsg.Visible = False

            If previousPage = 1 Then
                tblFinished.Visible = True
                tblUpdated.Visible = False
                lnkInvoiceDetails.Visible = False

                If ds.ReservationCharges.Count > 0 Then
                    If ds.ReservationCharges(0).IsDueDateNull() Then
                        ShowFinishData(String.Empty)
                    Else
                        ShowFinishData(ds.ReservationCharges(0).DueDate)
                    End If
                End If


                If FileName <> String.Empty Then
                    lnkInvoice.Visible = True
                    lblInvoice.Visible = True
                    lblInvoice.Text = PortalCulture.GetString("00803", True)
                    lnkInvoice.Text = FileName
                    FileName = Oz.UniBilling.Common.Cryptography.EncryptUnivisitString(FileName)
                    lnkInvoice.NavigateUrl = String.Concat(AppSettings("DIR_INVOICE_PRINTING"), "DigitalInvoiceHelper.aspx") & "?action=Download&Invoice=" & Server.UrlEncode(FileName) & "&format=" & DigitalInvoiceFormat.Pdf.ToString

                Else
                    lblInvoice.Visible = True
                    lblInvoice.Text = PortalCulture.GetString("00804")

                End If
            ElseIf previousPage = 0 Then
                tblFinished.Visible = False
                tblUpdated.Visible = True
                ShowUpdateData()
            End If

        Else
            tblFinished.Visible = False
            tblUpdated.Visible = False
            lnkInvoiceDetails.Visible = False
            lnkToConciliate.Visible = False
            lblMsg.Text = PortalCulture.GetString("M0BT0000050")
        End If

    End Sub

    Private Sub ShowUpdateData()
        lblUpdate.Text = String.Format(PortalCulture.GetString("M0BT0000017"), Formatting.GetMonthName(ds.BillingStatements(0).Month, , PortalCulture.GetCulture.Name) & " " & ds.BillingStatements(0).Year)
        ltlUpdateInfo.Text = "<b>" & PortalCulture.GetString("M0BT0000018", True) & " </b> " & PortalCulture.GetString("M0BT0000019") & "<br>" & _
                            PortalCulture.GetString("M0BT0000020") & "<br>" & "<br>" & PortalCulture.GetString("M0BT0000021") & " """ & PortalCulture.GetString("M0BT0000029") & """."
    End Sub

    Private Sub ShowFinishData(ByVal LastPaymentDate As String)
        String.Format(PortalCulture.GetString("M0BT0000022"), Formatting.GetMonthName(ds.BillingStatements(0).Month, , PortalCulture.GetCulture.Name) & " " & ds.BillingStatements(0).Year)
        lblFinish.Text = String.Format(PortalCulture.GetString("M0BT0000022"), Formatting.GetMonthName(ds.BillingStatements(0).Month, , PortalCulture.GetCulture.Name) & " " & ds.BillingStatements(0).Year)
        ltlFinishInfo.Text = "<b>" & PortalCulture.GetString("M0BT0000018", True) & " </b> " & PortalCulture.GetString("M0BT0000023") & "<br>" & _
                            PortalCulture.GetString("M0BT0000024") & "<br>"
        If Not LastPaymentDate = String.Empty Then
            ltlFinishInfo.Text &= PortalCulture.GetString("M0BT0000025") & " " & Formatting.FormatDate(CDate(LastPaymentDate), "D", PortalCulture.GetCulture.Name) & "."
        End If
    End Sub

    Private Sub lnkToConciliate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkToConciliate.Click
        Response.Redirect("InvoicesToConciliate.aspx")

    End Sub

    Private Sub lnkInvoiceDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkInvoiceDetails.Click
        Response.Redirect("InvoiceToConciliateDetails.aspx?id=" & InvoiceID)

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        lblUpdated.Text = PortalCulture.GetString("M0BT0000013")
        lblFinished.Text = PortalCulture.GetString("M0BT0000014")
        lnkInvoiceDetails.Text = PortalCulture.GetString("M0BT0000015")
        lnkToConciliate.Text = PortalCulture.GetString("M0BT0000016")

        hypTaskList.Text = PortalCulture.GetString("M0BT0000148")
        hypTaskList.NavigateUrl = String.Format("{0}MainInvoicing.aspx?cid={1}", GeRequestApplicationPath("/HotelAdministrator/Invoicing/"), HotelIdentity.CompanyID)

    End Sub

End Class