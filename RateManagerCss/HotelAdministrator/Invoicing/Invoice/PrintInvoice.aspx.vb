Imports System.Configuration.ConfigurationManager
Imports Oz.UniBilling.DataSchemas.Sat
Imports Oz.UniBilling.DigitalInvoicing.Business
Imports Oz.UniBilling.DataSchemas.Hotels
Imports Oz.UniBilling.Hotels.Business
Imports Oz.UniBilling.Common.Hotels
Imports Oz.UniBilling.Common


Partial Class PrintInvoice
    Inherits PaginaBase

#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents Button1 As System.Web.UI.WebControls.Button
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

#End Region

    Private ds As New CfdDataSet
    Private HotelIdentity As New HotelIdentifier
    Dim FileName As String = ""

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)
        Me.HeaderPanel = ctrlHeader.Usuario.uHotelAdministrador

        CompanyID = MyBase.cInfoActual.Empresa
        HotelIdentity.CompanyID = CompanyID

        Dim dsHotel As New HotelDataSet
        Dim HotelMng As New HotelManager
        HotelMng.SearchHotelByCompanyID(HotelIdentity.CompanyID, dsHotel)
        If Not dsHotel.Hotels(0).IsPortalHotelIDNull Then HotelIdentity.PortalID = dsHotel.Hotels(0).PortalHotelID
        If Not dsHotel.Hotels(0).IsUniPantallaHotelIDNull Then HotelIdentity.UniPantallaID = dsHotel.Hotels(0).UniPantallaHotelID
        If Not IsPostBack Then
            loadfacturas()
        End If

       

    End Sub
    Private Sub loadfacturas()
        ds = New CfdDataSet
        Dim DigitalInvoiceMng As New DigitalInvoiceManager(Oz.UniBilling.Common.Security.cSecurity.GetIsser(1), AppSettings("UserID"), "ratemanager@univisit.com")
        If Me.ddlfiltro.SelectedValue = 0 Then
            DigitalInvoiceMng.GetDigitalInvoices(HotelIdentity, ds)
        ElseIf Me.ddlfiltro.SelectedValue = 1 Then
            DigitalInvoiceMng.GetDigitalInvoices(Nothing, Nothing, Nothing, 0, 0, 0, -1, -1, Nothing, Nothing, Nothing, Nothing, Nothing, DigitalInvoiceType.Undefined, Nothing, DbBit.True, Nothing, HotelIdentity, ds)
        Else
            DigitalInvoiceMng.GetDigitalInvoices(Nothing, Nothing, Nothing, 0, 0, 0, -1, -1, Nothing, Nothing, Nothing, Nothing, Nothing, DigitalInvoiceType.Undefined, Nothing, DbBit.False, Nothing, HotelIdentity, ds)
        End If


        If Not ds Is Nothing AndAlso ds.Comprobante.Rows.Count > 0 Then
            lblMsg.Visible = False
            dgInvoices.Visible = True
            dgInvoices.DataSource = ds
            dgInvoices.DataBind()
        Else
            lblMsg.Visible = True
            dgInvoices.Visible = False
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        lblTitle.Text = PortalCulture.GetString("M0BT0000336")
        lblMsg.Text = PortalCulture.GetString("M0BT0000337")

        hplGoToTaskList.Text = PortalCulture.GetString("M0BT0000148")
        hplGoToTaskList.NavigateUrl = String.Format("{0}MainInvoicing.aspx?cid={1}", GeRequestApplicationPath("/HotelAdministrator/Invoicing/"), CompanyID)
        Me.ddlfiltro.Items(0).Text = PortalCulture.GetString("00576")
        Me.ddlfiltro.Items(1).Text = PortalCulture.GetString("00577")
        Me.ddlfiltro.Items(2).Text = PortalCulture.GetString("00578")
        Me.lblshow.Text = PortalCulture.GetString("00579", True)
        Me.btnshow.Text = PortalCulture.GetString("00580")
    End Sub

    Private Sub dgInvoices_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgInvoices.ItemDataBound
        Select Case e.Item.ItemType
            Case ListItemType.Header
                With e.Item
                    .Cells(1).Text = Server.HtmlDecode(PortalCulture.GetString("M0BT0000338"))
                    .Cells(2).Text = Server.HtmlDecode(PortalCulture.GetString("M0BT0000342"))
                End With

            Case ListItemType.Item, ListItemType.AlternatingItem
                Dim lbl As Label = e.Item.FindControl("lblReferenceNumber")
                lbl.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" & DataBinder.Eval(e.Item.DataItem, "Serie") & " - " & DataBinder.Eval(e.Item.DataItem, "Folio")

                Dim lnk As HyperLink = e.Item.FindControl("hypPdf")
                FileName = Cryptography.EncryptUnivisitString(DataBinder.Eval(e.Item.DataItem, "Serie") & "-" & DataBinder.Eval(e.Item.DataItem, "Folio"))
                lnk.NavigateUrl = String.Concat(AppSettings("DIR_INVOICE_PRINTING"), "DigitalInvoiceHelper.aspx") & "?action=Download&Invoice=" & Server.UrlEncode(FileName) & "&format=" & DigitalInvoiceFormat.Pdf.ToString

                lnk = e.Item.FindControl("hypXml")
                lnk.NavigateUrl = String.Concat(AppSettings("DIR_INVOICE_PRINTING"), "DigitalInvoiceHelper.aspx") & "?action=Download&Invoice=" & Server.UrlEncode(FileName) & "&format=" & DigitalInvoiceFormat.Xml.ToString

        End Select
    End Sub

    

    Private Sub btnshow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnshow.Click
        loadfacturas()
    End Sub
End Class
