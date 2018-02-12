Imports System.Globalization
Imports Oz.UniBilling.DataSchemas
Imports Oz.UniBilling.Hotels.Business
Imports Oz.UniBilling.DataSchemas.Hotels
Imports Oz.UniBilling.Common.Hotels
Imports Oz.UniBilling.Common
Imports Oz.UniBilling.DataSchemas.Sat
Imports Oz.UniBilling.DigitalInvoicing.Business
Imports System.Configuration.ConfigurationManager

Partial Class Invoices
    Inherits PaginaBase

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region " Properties.. "
    Public Property CompanyID() As Integer
        Get
            Return Session(String.Format("{0}CompanyID", Me.ID))
        End Get
        Set(ByVal Value As Integer)
            Session(String.Format("{0}CompanyID", Me.ID)) = Value
        End Set
    End Property

    Private Property dsDataFacturas() As HotelBillingStatementsInvoceDataSet
        Get
            Return Session("dsDataFacturas")
        End Get
        Set(ByVal Value As HotelBillingStatementsInvoceDataSet)
            Session("dsDataFacturas") = Value
        End Set
    End Property

    Private Property dsDataEstadoCuenta() As HotelBillingStatementsInvoceDataSet
        Get
            Return Session("dsDataEstadoCuenta")
        End Get
        Set(ByVal Value As HotelBillingStatementsInvoceDataSet)
            Session("dsDataEstadoCuenta") = Value
        End Set
    End Property

    Private Property dsImprimir() As CfdDataSet
        Get
            Return Session("dsImprimir")
        End Get
        Set(ByVal Value As CfdDataSet)
            Session("dsImprimir") = Value
        End Set
    End Property

    Private Property filtroPeriodo() As String
        Get
            Return Session("filtroPeriodo")
        End Get
        Set(ByVal Value As String)
            Session("filtroPeriodo") = Value
        End Set
    End Property

    Private Property filtroPagada() As String
        Get
            Return Session("filtroPagada")
        End Get
        Set(ByVal Value As String)
            Session("filtroPagada") = Value
        End Set
    End Property

    Private Property totalEC() As String
        Get
            Return Session("totalEC")
        End Get
        Set(ByVal Value As String)
            Session("totalEC") = Value
        End Set
    End Property

    Private Property totalFD() As String
        Get
            Return Session("totalFD")
        End Get
        Set(ByVal Value As String)
            Session("totalFD") = Value
        End Set
    End Property

    Private Property totalDebeFD() As String
        Get
            Return Session("totalDebeFD")
        End Get
        Set(ByVal Value As String)
            Session("totalDebeFD") = Value
        End Set
    End Property

    Private Property totalFM() As String
        Get
            Return Session("totalFM")
        End Get
        Set(ByVal Value As String)
            Session("totalFM") = Value
        End Set
    End Property

    Private Property totalDebeFM() As String
        Get
            Return Session("totalDebeFM")
        End Get
        Set(ByVal Value As String)
            Session("totalDebeFM") = Value
        End Set
    End Property

#End Region

    Protected WithEvents ctrHotelsUserChain1 As ctrHotelsUserChain

    Dim totalFacturaDigital As Decimal = Decimal.Zero
    Dim totalFacturaManual As Decimal = Decimal.Zero
    Dim totalCuentaConciliar As Decimal = Decimal.Zero
    Dim totalFacturas As Decimal = 0
    Private ds As New CfdDataSet
    Private HotelIdentity As New HotelIdentifier

    Dim FileName As String = ""

    Private Sub loadfacturas()
        ds = New CfdDataSet
        Dim DigitalInvoiceMng As New DigitalInvoiceManager(Oz.UniBilling.Common.Security.cSecurity.GetIsser(1), AppSettings("UserID"), "ratemanager@univisit.com")
        DigitalInvoiceMng.GetDigitalInvoices(HotelIdentity, ds)

        If Not ds Is Nothing AndAlso ds.Comprobante.Rows.Count > 0 Then
            dsImprimir = ds
        End If
    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ctrHotelsUserChain1.Visible = False

        Dim x As Integer = 1
        If MyBase.isUserChain AndAlso x = 2 Then
            'Comportamiento UserChain
            ctrHotelsUserChain1.Visible = True

            If Not IsPostBack Then
                ctrHotelsUserChain1.LoadHotels()
                CompanyID = ctrHotelsUserChain1.SelectedCompanyID
                LoadDataEstadoCuenta()
                LoadDataFactura()
            Else
                CompanyID = ctrHotelsUserChain1.SelectedCompanyID
            End If

        Else
            'Comportamiento normal
            If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)
            Me.HeaderPanel = ctrlHeader.Usuario.uHotelAdministrador

            CompanyID = MyBase.cInfoActual.Empresa

            If Not IsPostBack Then
                LoadDataEstadoCuenta()
                LoadDataFactura()
            End If


        End If






        ckbPeriodo.Attributes.Add("onclick", "javascript:ShowOrHide('" & Me.ckbPeriodo.ClientID & "');")
        trPeriodo.Style.Item("display") = IIf(ckbPeriodo.Checked, "", "none")

        imgEC.Attributes.Add("onClick", "javascript:expandOrCollapse('" & Me.imgEC.ClientID & "','" & Me.trMostrarEC.ClientID & "','" & ckbEC.ClientID & "');")
        imgEC.Style("CURSOR") = "pointer"
        ckbEC.Style.Item("display") = "none"
        trMostrarEC.Style.Item("display") = IIf(Not ckbEC.Checked, "", "none")
        imgEC.ImageUrl = IIf(Not ckbEC.Checked, imgEC.ImageUrl.Replace("mas.png", "menos.png"), imgEC.ImageUrl.Replace("menos.png", "mas.png"))

        imgF.Attributes.Add("onClick", "javascript:expandOrCollapse('" & Me.imgF.ClientID & "','" & Me.trMostrarF.ClientID & "','" & ckbF.ClientID & "');")
        imgF.Style("CURSOR") = "pointer"
        ckbF.Style.Item("display") = "none"
        trMostrarF.Style.Item("display") = IIf(Not ckbF.Checked, "", "none")
        imgF.ImageUrl = IIf(Not ckbF.Checked, imgF.ImageUrl.Replace("mas.png", "menos.png"), imgF.ImageUrl.Replace("menos.png", "mas.png"))
    End Sub



    Private Sub LoadDataEstadoCuenta(Optional ByVal opcionEC As Boolean = True, Optional ByVal opcionF As Boolean = True)
        HotelIdentity.CompanyID = CompanyID

        If Not IsNothing(CompanyID) AndAlso IsNumeric(CompanyID) AndAlso CompanyID > -1 Then
            If (CompanyID > 0) Then
                Dim manager As HotelManager = New HotelManager
                Dim i As Integer
                Dim codigoMoneda As String = String.Empty
                Dim debe As Decimal = 0

                With manager
                    Dim data As HotelBillingStatementsInvoceDataSet = New HotelBillingStatementsInvoceDataSet
                    manager.GetBillingStatementInvoice(CompanyID, data)
                    dsDataEstadoCuenta = data

                    'ESTADO DE CUENTAS POR CONCILIAR
                    Dim a As Date
                    Dim fechaInicio As String = ddlyear.SelectedValue.ToString & "/" & ddlperiodo.SelectedValue & "/01"
                    Dim fechaFin As String = ddlyear.SelectedValue.ToString & "/" & ddlperiodo.SelectedValue & "/" & a.DaysInMonth(ddlyear.SelectedValue, ddlperiodo.SelectedValue).ToString
                    Dim dvC As New DataView(dsDataEstadoCuenta.CuentasConciliables)

                    If ckbPeriodo.Checked = True Then
                        filtroPeriodo = "fechaReferencia >='" & fechaInicio & "' and fechaReferencia <='" & fechaFin & "'"
                    Else
                        filtroPeriodo = String.Empty
                    End If

                    dvC.RowFilter = filtroPeriodo
                    If (dvC.Count > 0) Then
                        lblInfoEstadoCuentaConciliar.Visible = False
                        dgCuentasConciliables.Visible = True
                        lblTotalEC.Visible = True

                        dgCuentasConciliables.CurrentPageIndex = 0
                        dgCuentasConciliables.DataSource = dvC
                        dgCuentasConciliables.DataBind()

                        totalCuentaConciliar = 0
                        For i = 0 To dvC.Count - 1
                            totalCuentaConciliar += dvC.Item(i).Item("total")
                            codigoMoneda = dvC.Item(i).Item("codigoMoneda")
                        Next

                        totalEC = PortalCulture.GetString("00897") + Me.FCurrency(totalCuentaConciliar, 2) + String.Format(" {0}", codigoMoneda)
                        lblTotalEC.Text = totalEC
                    Else
                        lblInfoEstadoCuentaConciliar.Visible = True
                        dgCuentasConciliables.Visible = False
                        lblTotalEC.Visible = False
                    End If


                    Dim td As HtmlTableRow = FindControl("trMostrarEC")
                    Dim img As WebControls.Image = FindControl("imgEC")
                    If opcionEC = False Then
                        td.Style("display") = "none"
                    End If

                End With
            End If
        End If
    End Sub

    Private Sub LoadDataFactura(Optional ByVal opcionEC As Boolean = True, Optional ByVal opcionF As Boolean = True, Optional ByVal mostrar As Integer = 0)
        HotelIdentity.CompanyID = CompanyID
        Dim dsHotel As New HotelDataSet
        Dim HotelMng As New HotelManager
        HotelMng.SearchHotelByCompanyID(HotelIdentity.CompanyID, dsHotel)
        If Not dsHotel.Hotels(0).IsPortalHotelIDNull Then HotelIdentity.PortalID = dsHotel.Hotels(0).PortalHotelID
        If Not dsHotel.Hotels(0).IsUniPantallaHotelIDNull Then HotelIdentity.UniPantallaID = dsHotel.Hotels(0).UniPantallaHotelID

        loadfacturas()

        If Not IsNothing(CompanyID) AndAlso IsNumeric(CompanyID) AndAlso CompanyID > -1 Then
            If (CompanyID > 0) Then
                Dim manager As HotelManager = New HotelManager
                Dim i As Integer
                Dim codigoMoneda As String = String.Empty
                Dim debe As Decimal = 0

                With manager
                    Dim data As HotelBillingStatementsInvoceDataSet = New HotelBillingStatementsInvoceDataSet
                    manager.GetBillingStatementInvoice(CompanyID, data)
                    dsDataFacturas = data

                    trFDInfo.Style.Item("display") = "none"
                    trFDTitulo.Style.Item("display") = "none"
                    trFDDatos.Style.Item("display") = "none"
                    trFDTotal.Style.Item("display") = "none"
                    trFDTotalDebe.Style.Item("display") = "none"
                    trFMInfo.Style.Item("display") = "none"
                    trFMTitulo.Style.Item("display") = "none"
                    trFMDatos.Style.Item("display") = "none"
                    trFMTotal.Style.Item("display") = "none"
                    trFMTotalDebe.Style.Item("display") = "none"

                    If ddlTipo.SelectedValue <> 3 Then
                        If ddlTipo.SelectedValue = 2 Then
                            filtroPagada = "pagada = false"
                        Else
                            filtroPagada = ""
                        End If

                        'FACTURAS DIGITALES
                        Dim dvFD As New DataView(dsDataFacturas.FacturasDigitales)
                        dvFD.RowFilter = filtroPagada

                        If (dvFD.Count > 0) Then
                            trFDTitulo.Style.Item("display") = ""
                            trFDDatos.Style.Item("display") = ""
                            trFDTotal.Style.Item("display") = ""
                            trFDTotalDebe.Style.Item("display") = ""

                            dgFacturasDigitales.CurrentPageIndex = 0
                            dgFacturasDigitales.DataSource = dvFD
                            dgFacturasDigitales.DataBind()

                            totalFacturaDigital = 0
                            codigoMoneda = dvFD.Item(0).Item("codigoMoneda")
                            debe = 0
                            For i = 0 To dvFD.Count - 1
                                totalFacturaDigital += dvFD.Item(i).Item("total")
                                If dvFD.Item(i).Item("pagada") = False Then
                                    debe += dvFD.Item(i).Item("debe")
                                End If
                            Next
                            totalFD = PortalCulture.GetString("00897") + Me.FCurrency(totalFacturaDigital, 2) + String.Format(" {0}", codigoMoneda)
                            lblTotalFD.Text = totalFD

                            totalDebeFD = PortalCulture.GetString("00916") + Me.FCurrency(debe, 2) + String.Format(" {0}", codigoMoneda)
                            lblTotalDebeFD.Text = totalDebeFD
                        Else
                            trFDInfo.Style.Item("display") = ""
                        End If

                        'FACTURAS MANUALES
                        Dim dvFM As New DataView(dsDataFacturas.FacturasManuales)
                        dvFM.RowFilter = filtroPagada

                        If (dvFM.Count > 0) Then

                            trFMTitulo.Style.Item("display") = ""
                            trFMDatos.Style.Item("display") = ""
                            trFMTotal.Style.Item("display") = ""
                            trFMTotalDebe.Style.Item("display") = ""

                            dgFacturasManuales.CurrentPageIndex = 0
                            dgFacturasManuales.DataSource = dvFM
                            dgFacturasManuales.DataBind()

                            totalFacturaManual = 0
                            codigoMoneda = dvFM.Item(0).Item("codigoMoneda")
                            debe = 0
                            For i = 0 To dvFM.Count - 1
                                totalFacturaManual += dvFM.Item(i).Item("total")
                                If dvFM.Item(i).Item("pagada") = False Then
                                    debe += dvFM.Item(i).Item("debe")
                                End If
                            Next
                            totalFM = PortalCulture.GetString("00897") + Me.FCurrency(totalFacturaManual, 2) + String.Format(" {0}", codigoMoneda)
                            lblTotalFM.Text = totalFM

                            totalDebeFM = PortalCulture.GetString("00916") + Me.FCurrency(debe, 2) + String.Format(" {0}", codigoMoneda)
                            lblTotalDebeFM.Text = totalDebeFM
                        End If
                    End If

                    'FACTURAS CANCELADAS
                    trFCInfo.Style.Item("display") = "none"
                    trFCTitulo.Style.Item("display") = "none"
                    trFCDatos.Style.Item("display") = "none"
                    If ddlTipo.SelectedValue = 3 Or ddlTipo.SelectedValue = 0 Then
                        If ddlTipo.SelectedValue = 0 Or ddlTipo.SelectedValue = 3 Then
                            Dim dvFC As New DataView(dsDataFacturas.FacturasCanceladas)
                            If (dvFC.Count > 0) Then
                                trFCTitulo.Style.Item("display") = ""
                                trFCDatos.Style.Item("display") = ""
                                dgFacturasCanceladas.CurrentPageIndex = 0
                                dgFacturasCanceladas.DataSource = dvFC
                                dgFacturasCanceladas.DataBind()
                            Else
                                trFCInfo.Style.Item("display") = ""
                            End If
                        End If
                    End If

                    Dim td As HtmlTableRow
                    Dim img As WebControls.Image

                    td = FindControl("trMostrarF")
                    img = FindControl("imgF")
                    If opcionF = False Then
                        td.Style("display") = "none"
                    End If
                End With
            End If
        End If
    End Sub

    Private Sub dgFacturasDigitales_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgFacturasDigitales.ItemCreated
        If e.Item.ItemType = ListItemType.Header Then
            With e.Item
                .Cells(1).Text = PortalCulture.GetString("00891")
                .Cells(2).Text = PortalCulture.GetString("00892")
                .Cells(3).Text = PortalCulture.GetString("00900")
                .Cells(4).Text = PortalCulture.GetString("00901")
                .Cells(5).Text = PortalCulture.GetString("00893")
                .Cells(6).Text = PortalCulture.GetString("00902")
                .Cells(7).Text = PortalCulture.GetString("00905")
                .Cells(8).Text = PortalCulture.GetString("00904")
                .Cells(9).Text = PortalCulture.GetString("00896")
            End With
        End If
    End Sub

    Private Sub dgFacturasDigitales_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgFacturasDigitales.ItemDataBound
        Select Case e.Item.ItemType
            Case ListItemType.Item, ListItemType.AlternatingItem
                Dim lbl As Label
                Dim lnk As HyperLink
                Dim estadoCuenta As String = DataBinder.Eval(e.Item.DataItem, "estadoCuenta")
                Dim status As Boolean = DataBinder.Eval(e.Item.DataItem, "pagada")
                Dim version As Single = DataBinder.Eval(e.Item.DataItem, "version")
                Dim SelloSat As Boolean = DataBinder.Eval(e.Item.DataItem, "SelloSat")
                Dim faltaSelloSat As Boolean = IIf(version >= 3.0F, Not SelloSat, False)

                lnk = e.Item.FindControl("lnkCuenta")
                If Not lnk Is Nothing Then
                    lnk.Text = DataBinder.Eval(e.Item.DataItem, "estadoCuenta")
                    If estadoCuenta.Split("-").Length = 2 Then
                        lnk.NavigateUrl = "../Invoicing/Payment/HotelInvoiceDetails.aspx?id=" & estadoCuenta.Split("-")(1)
                    End If

                End If

                lbl = e.Item.FindControl("lblFechaCuenta")
                If Not lbl Is Nothing Then
                    lbl.Text = CDate(DataBinder.Eval(e.Item.DataItem, "fechaReferencia")).ToString("dd MMMM yyyy", PortalCulture.GetCulture)
                End If

                lbl = e.Item.FindControl("lblFactura")
                If Not lbl Is Nothing Then
                    lbl.Text = DataBinder.Eval(e.Item.DataItem, "folioFactura")
                End If

                lbl = e.Item.FindControl("lblFechaFactura")
                If Not lbl Is Nothing Then
                    lbl.Text = CDate(DataBinder.Eval(e.Item.DataItem, "fechaFactura")).ToString("dd MMMM yyyy", PortalCulture.GetCulture)
                End If

                lbl = e.Item.FindControl("lblReferenciaBancaria")
                If Not lbl Is Nothing Then
                    lbl.Text = DataBinder.Eval(e.Item.DataItem, "referenciaBancaria")
                End If

                lnk = e.Item.FindControl("lnkVerDetalle")
                lnk.Text = PortalCulture.GetString("00903")
                lnk.Visible = False
                If Not lnk Is Nothing Then
                    If status = False Then
                        If estadoCuenta.Split("-").Length = 2 Then
                            lnk.NavigateUrl = "InvoiceToConciliateDetails.aspx?id=" & estadoCuenta.Split("-")(1)
                            lnk.Visible = True
                        End If
                    End If
                End If

                Dim codigoDeMoneda As String = CType(DataBinder.Eval(e.Item.DataItem, "codigoMoneda"), String)
                Dim total As Decimal = CType(DataBinder.Eval(e.Item.DataItem, "total"), Decimal)
                lbl = e.Item.FindControl("lblImporte")
                If Not lbl Is Nothing Then
                    lbl.Text = Me.FCurrency(total, 2) + String.Format(" {0}", codigoDeMoneda)
                    totalFacturaDigital += total
                    totalFacturas += total
                End If

                Dim debe As Decimal = CType(DataBinder.Eval(e.Item.DataItem, "debe"), Decimal)
                lbl = e.Item.FindControl("lblStatus")
                If Not lbl Is Nothing Then
                    lbl.Text = Me.FCurrency(debe, 2) + String.Format(" {0}", codigoDeMoneda)
                End If

                lbl = e.Item.FindControl("lblNumeracion")
                If Not lbl Is Nothing Then
                    lbl.Text = (dgFacturasDigitales.CurrentPageIndex * dgFacturasDigitales.PageSize) + (e.Item.ItemIndex + 1)
                End If


                Dim lnkDescargar As HyperLink = e.Item.FindControl("hypPdf")
                If faltaSelloSat Then
                    lnkDescargar.Visible = False
                    CType(e.Item.FindControl("lblMsgSelloSat"), Label).Text = PortalCulture.GetString("00439")
                Else
                    FileName = Cryptography.EncryptUnivisitString(DataBinder.Eval(e.Item.DataItem, "folioFactura"))
                    lnkDescargar.NavigateUrl = String.Concat(AppSettings("DIR_INVOICE_PRINTING"), "DigitalInvoiceHelper.aspx") & "?action=Download&Invoice=" & Server.UrlEncode(FileName) & "&format=" & DigitalInvoiceFormat.Pdf.ToString
                    lnkDescargar.ToolTip = PortalCulture.GetString("00906")
                    lnkDescargar.Visible = True
                    CType(e.Item.FindControl("lblMsgSelloSat"), Label).Text = String.Empty
                End If

                lnkDescargar = e.Item.FindControl("hypXml")
                If faltaSelloSat Then
                    lnkDescargar.Visible = False
                Else
                    lnkDescargar.NavigateUrl = String.Concat(AppSettings("DIR_INVOICE_PRINTING"), "DigitalInvoiceHelper.aspx") & "?action=Download&Invoice=" & Server.UrlEncode(FileName) & "&format=" & DigitalInvoiceFormat.Xml.ToString
                    lnkDescargar.ToolTip = PortalCulture.GetString("00907")
                    lnkDescargar.Visible = True
                End If

                lnkDescargar = e.Item.FindControl("hypExc")
                lnkDescargar.NavigateUrl = String.Format("{0}BillingStatementHelper.aspx?action={1}&BillingStatement={2}", _
                               AppSettings("DIR_INVOICE_PRINTING"), _
                               "Download", _
                               Server.UrlEncode(Cryptography.EncryptUnivisitString(String.Format("{0}", estadoCuenta _
                               ))))
                lnkDescargar.ToolTip = PortalCulture.GetString("01340")
        End Select
    End Sub

    Private Sub dgFacturasDigitales_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgFacturasDigitales.PageIndexChanged
        Try
            If Not dsDataFacturas Is Nothing Then

                Dim dvFD As New DataView(dsDataFacturas.FacturasDigitales)
                dvFD.RowFilter = filtroPagada
                dgFacturasDigitales.DataSource = dvFD
                dgFacturasDigitales.CurrentPageIndex = e.NewPageIndex
                dgFacturasDigitales.DataBind()
                lblTotalFD.Text = totalFD
                lblTotalDebeFD.Text = totalDebeFD
            End If
        Catch ex As Exception
            trFDTotal.Style.Item("display") = "none"
            trFDTotalDebe.Style.Item("display") = "none"
        End Try
    End Sub

    Private Sub dgFacturasManuales_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgFacturasManuales.ItemCreated
        If e.Item.ItemType = ListItemType.Header Then
            With e.Item
                .Cells(1).Text = PortalCulture.GetString("00891")
                .Cells(2).Text = PortalCulture.GetString("00892")
                .Cells(3).Text = PortalCulture.GetString("00900")
                .Cells(4).Text = PortalCulture.GetString("00901")
                .Cells(5).Text = PortalCulture.GetString("00893")
                .Cells(6).Text = PortalCulture.GetString("00902")
                .Cells(7).Text = PortalCulture.GetString("00904")
                .Cells(8).Text = PortalCulture.GetString("00896")
            End With
        End If
    End Sub

    Private Sub dgFacturasManuales_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgFacturasManuales.PageIndexChanged
        Try
            If Not dsDataFacturas Is Nothing Then
                Dim dvFM As New DataView(dsDataFacturas.FacturasManuales)
                dvFM.RowFilter = filtroPagada
                dgFacturasManuales.CurrentPageIndex = e.NewPageIndex
                dgFacturasManuales.DataSource = dvFM
                dgFacturasManuales.DataBind()
                lblTotalFM.Text = totalFM
                lblTotalDebeFM.Text = totalDebeFM
            End If
        Catch ex As Exception
            trFMTotal.Style.Item("display") = "none"
            trFMTotalDebe.Style.Item("display") = "none"
        End Try
    End Sub

    Private Sub dgFacturasManuales_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgFacturasManuales.ItemDataBound
        Select Case e.Item.ItemType
            Case ListItemType.Item, ListItemType.AlternatingItem
                Dim lbl As Label
                Dim lnk As HyperLink
                Dim referencia As String = DataBinder.Eval(e.Item.DataItem, "referencia")
                Dim status As Boolean = DataBinder.Eval(e.Item.DataItem, "pagada")

                lnk = e.Item.FindControl("lnkCuentaFM")
                If Not lnk Is Nothing Then
                    lnk.Text = DataBinder.Eval(e.Item.DataItem, "referencia")
                    lnk.NavigateUrl = "../Invoicing/Payment/HotelInvoiceDetails.aspx?id=" & referencia.Split("-")(1)
                    lnk.Visible = True
                End If

                lbl = e.Item.FindControl("lblFechaCuentaFM")
                If Not lbl Is Nothing Then
                    lbl.Text = CDate(DataBinder.Eval(e.Item.DataItem, "fechaReferencia")).ToString("dd MMMM yyyy", PortalCulture.GetCulture)
                End If

                lbl = e.Item.FindControl("lblFacturaFM")
                If Not lbl Is Nothing Then
                    lbl.Text = DataBinder.Eval(e.Item.DataItem, "serieFolio")
                End If

                lbl = e.Item.FindControl("lblFechaFacturaFM")
                If Not lbl Is Nothing Then
                    lbl.Text = CDate(DataBinder.Eval(e.Item.DataItem, "fechaFactura")).ToString("dd MMMM yyyy", PortalCulture.GetCulture)
                End If

                lbl = e.Item.FindControl("lblReferenciaBancariaFM")
                If Not lbl Is Nothing Then
                    lbl.Text = DataBinder.Eval(e.Item.DataItem, "referenciaBancaria")
                End If

                lnk = e.Item.FindControl("lnkVerDetalleFM")
                lnk.Text = PortalCulture.GetString("00903")
                lnk.Visible = False
                If Not lnk Is Nothing Then
                    If status = False Then
                        lnk.NavigateUrl = "InvoiceToConciliateDetails.aspx?id=" & referencia.Split("-")(1)
                        lnk.Visible = True
                    End If
                End If

                Dim codigoDeMoneda As String = CType(DataBinder.Eval(e.Item.DataItem, "codigoMoneda"), String)
                Dim total As Decimal = CType(DataBinder.Eval(e.Item.DataItem, "total"), Decimal)
                lbl = e.Item.FindControl("lblImporteFM")
                If Not lbl Is Nothing Then
                    lbl.Text = Me.FCurrency(total, 2) + String.Format(" {0}", codigoDeMoneda)
                    totalFacturaManual += total
                    totalFacturas += total
                End If

                Dim debe As Decimal = CType(DataBinder.Eval(e.Item.DataItem, "debe"), Decimal)
                lbl = e.Item.FindControl("lblStatusFM")
                If Not lbl Is Nothing Then
                    lbl.Text = Me.FCurrency(debe, 2) + String.Format(" {0}", codigoDeMoneda)
                End If

                lbl = e.Item.FindControl("lblNumeracionFD")
                If Not lbl Is Nothing Then
                    lbl.Text = (dgFacturasManuales.CurrentPageIndex * dgFacturasManuales.PageSize) + (e.Item.ItemIndex + 1)
                End If
        End Select
    End Sub

    Private Sub dgCuentasConciliables_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgCuentasConciliables.ItemDataBound
        Select Case e.Item.ItemType
            Case ListItemType.Item, ListItemType.AlternatingItem
                Dim lbl As Label
                Dim lnk As HyperLink
                Dim referencia As String = DataBinder.Eval(e.Item.DataItem, "referencia")

                lnk = e.Item.FindControl("lnkCuentaEC")
                If Not lnk Is Nothing Then
                    lnk.Text = DataBinder.Eval(e.Item.DataItem, "referencia")
                    lnk.NavigateUrl = "../Invoicing/Payment/HotelInvoiceDetails.aspx?id=" & referencia.Split("-")(1)
                End If

                lbl = e.Item.FindControl("lblFechaCuentaEC")
                If Not lbl Is Nothing Then
                    lbl.Text = CDate(DataBinder.Eval(e.Item.DataItem, "fechaReferencia")).ToString("dd MMMM yyyy", PortalCulture.GetCulture)
                End If

                lbl = e.Item.FindControl("lblReferenciaBancariaEC")
                If Not lbl Is Nothing Then
                    lbl.Text = DataBinder.Eval(e.Item.DataItem, "referenciaBancaria")
                End If

                lbl = e.Item.FindControl("lblFechaLimiteConciliarEC")
                If Not lbl Is Nothing Then
                    lbl.Text = CDate(DataBinder.Eval(e.Item.DataItem, "limiteParaConciliar")).ToString("dd MMMM yyyy", PortalCulture.GetCulture)
                End If

                Dim codigoDeMoneda As String = CType(DataBinder.Eval(e.Item.DataItem, "codigoMoneda"), String)
                Dim total As Decimal = CType(DataBinder.Eval(e.Item.DataItem, "total"), Decimal)
                lbl = e.Item.FindControl("lblImporteEC")
                If Not lbl Is Nothing Then
                    lbl.Text = Me.FCurrency(total, 2) + String.Format(" {0}", codigoDeMoneda)
                    totalCuentaConciliar += total
                    totalFacturas += total
                End If


                lnk = e.Item.FindControl("lnkConciliarEC")
                lnk.Text = PortalCulture.GetString("00895")
                If Not lnk Is Nothing Then
                    lnk.NavigateUrl = "InvoiceToConciliateDetails.aspx?id=" & referencia.Split("-")(1)
                End If

                lbl = e.Item.FindControl("lblNumeracionEC")
                If Not lbl Is Nothing Then
                    lbl.Text = (dgCuentasConciliables.CurrentPageIndex * dgCuentasConciliables.PageSize) + (e.Item.ItemIndex + 1)
                End If
        End Select

    End Sub

    Private Sub dgCuentasConciliables_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgCuentasConciliables.ItemCreated
        If e.Item.ItemType = ListItemType.Header Then
            With e.Item
                .Cells(1).Text = PortalCulture.GetString("00891")
                .Cells(2).Text = PortalCulture.GetString("00892")
                .Cells(3).Text = PortalCulture.GetString("00893")
                .Cells(4).Text = PortalCulture.GetString("00894")
                .Cells(5).Text = PortalCulture.GetString("00895")
                .Cells(6).Text = PortalCulture.GetString("00896")
            End With
        End If
    End Sub

    Private Sub dgCuentasConciliables_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgCuentasConciliables.PageIndexChanged
        If Not dsDataFacturas Is Nothing Then
            Dim dvC As New DataView(dsDataEstadoCuenta.CuentasConciliables)
            dvC.RowFilter = filtroPeriodo
            dgCuentasConciliables.CurrentPageIndex = e.NewPageIndex
            dgCuentasConciliables.DataSource = dvC
            dgCuentasConciliables.DataBind()
            lblTotalEC.Text = totalEC
        End If
    End Sub

    Private Sub btnload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnload.Click
        LoadDataEstadoCuenta(Not ckbEC.Checked, Not ckbF.Checked)
    End Sub

    Private Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        Dim ci As System.Globalization.CultureInfo
        ci = System.Threading.Thread.CurrentThread.CurrentCulture
        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString)
        For i As Integer = 0 To 11
            Me.ddlperiodo.Items(i).Text = MonthName(i + 1)
        Next
        For i As Integer = 0 To 2
            Me.ddlyear.Items(i).Text = Now.Date.Year + i - 2
            Me.ddlyear.Items(i).Value = Now.Date.Year + i - 2
        Next
        Me.ddlyear.SelectedValue = Now.Date.Year
        System.Threading.Thread.CurrentThread.CurrentCulture = ci

        lblTitle.Text = PortalCulture.GetString("00881")
        lblOpcion1.Text = PortalCulture.GetString("00917", True)
        ckbPeriodo.Text = PortalCulture.GetString("00883")
        lblOpcion2.Text = PortalCulture.GetString("00884", True)
        ddlTipo.Items(0).Text = PortalCulture.GetString("00885")
        ddlTipo.Items(1).Text = PortalCulture.GetString("00886")
        ddlTipo.Items(2).Text = PortalCulture.GetString("00887")
        ddlTipo.Items(3).Text = PortalCulture.GetString("00888")
        btnload.Text = PortalCulture.GetString("00889")
        btnMostrarFactura.Text = PortalCulture.GetString("00889")
        lblInfoEstadoCuentaConciliar.Text = PortalCulture.GetString("00890")
        lblTituloEstadoCuentaConciliar.Text = PortalCulture.GetString("00882")
        lblInfoFacturaDigital.Text = PortalCulture.GetString("00898")
        lblTituloFacturaDigital.Text = PortalCulture.GetString("00899")
        lblInfoFacturaManual.Text = PortalCulture.GetString("00908")
        lblTituloFacturaManual.Text = PortalCulture.GetString("00909")
        lblInfoFacturaCancelada.Text = PortalCulture.GetString("00910")
        lblTituloFacturaCancelada.Text = PortalCulture.GetString("00911")
    End Sub

    Private Sub dgFacturasCanceladas_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgFacturasCanceladas.ItemCreated
        If e.Item.ItemType = ListItemType.Header Then
            With e.Item
                .Cells(1).Text = PortalCulture.GetString("00891")
                .Cells(2).Text = PortalCulture.GetString("00892")
                .Cells(3).Text = PortalCulture.GetString("00900")
                .Cells(4).Text = PortalCulture.GetString("00901")
                .Cells(5).Text = PortalCulture.GetString("00893")
                .Cells(6).Text = PortalCulture.GetString("00912")
                .Cells(7).Text = PortalCulture.GetString("00913")
                .Cells(8).Text = PortalCulture.GetString("00905")
            End With
        End If
    End Sub

    Private Sub dgFacturasCanceladas_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgFacturasCanceladas.PageIndexChanged
        If Not dsDataFacturas Is Nothing Then
            Dim dvfC As New DataView(dsDataFacturas.FacturasCanceladas)
            dgFacturasCanceladas.CurrentPageIndex = e.NewPageIndex
            dgFacturasCanceladas.DataSource = dvfC
            dgFacturasCanceladas.DataBind()
        End If
    End Sub

    Private Sub dgFacturasCanceladas_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgFacturasCanceladas.ItemDataBound
        Select Case e.Item.ItemType
            Case ListItemType.Item, ListItemType.AlternatingItem
                Dim lbl As Label
                Dim lnk As HyperLink
                Dim estadoCuenta As String = DataBinder.Eval(e.Item.DataItem, "estadoCuenta")

                If DataBinder.Eval(e.Item.DataItem, "estadoCuenta") = "S/E" Then
                    lbl = e.Item.FindControl("lblCuentaFC")
                    If Not lbl Is Nothing Then
                        lbl.Text = DataBinder.Eval(e.Item.DataItem, "estadoCuenta")
                    End If
                Else
                    lnk = e.Item.FindControl("lnkCuentaFC")
                    If Not lnk Is Nothing Then
                        lnk.Text = DataBinder.Eval(e.Item.DataItem, "estadoCuenta")
                        If estadoCuenta.Split("-").Length = 2 Then
                            lnk.NavigateUrl = "../Invoicing/Payment/HotelInvoiceDetails.aspx?id=" & estadoCuenta.Split("-")(1)
                        End If
                    End If
                    End If

                    lbl = e.Item.FindControl("lblFechaCuentaFC")
                    If Not lbl Is Nothing Then
                        lbl.Text = CDate(DataBinder.Eval(e.Item.DataItem, "fechaReferencia")).ToString("dd MMMM yyyy", PortalCulture.GetCulture)
                    End If

                    lbl = e.Item.FindControl("lblFacturaFC")
                    If Not lbl Is Nothing Then
                        lbl.Text = DataBinder.Eval(e.Item.DataItem, "folioFactura")
                    End If

                    lbl = e.Item.FindControl("lblFechaFacturaFC")
                    If Not lbl Is Nothing Then
                        lbl.Text = CDate(DataBinder.Eval(e.Item.DataItem, "fechaFactura")).ToString("dd MMMM yyyy", PortalCulture.GetCulture)
                    End If

                    lbl = e.Item.FindControl("lblReferenciaBancariaFC")
                    If Not lbl Is Nothing Then
                        lbl.Text = DataBinder.Eval(e.Item.DataItem, "referenciaBancaria")
                    End If

                    lbl = e.Item.FindControl("lblFechaCancelacionFC")
                    If Not lbl Is Nothing Then
                        If Not DataBinder.Eval(e.Item.DataItem, "fechaCancelacion") Is DBNull.Value Then
                            lbl.Text = CDate(DataBinder.Eval(e.Item.DataItem, "fechaCancelacion")).ToString("dd MMMM yyyy", PortalCulture.GetCulture)
                        Else
                            lbl.Text = "-----"
                        End If
                    End If

                    lbl = e.Item.FindControl("lblMotivoCancelacionFC")
                    If Not lbl Is Nothing Then
                        If Not DataBinder.Eval(e.Item.DataItem, "motivoCancelacion") Is DBNull.Value Then
                            lbl.Text = DataBinder.Eval(e.Item.DataItem, "motivoCancelacion")
                        Else
                            lbl.Text = PortalCulture.GetString("00918")
                        End If
                    End If

                    lbl = e.Item.FindControl("lblNumeracionFC")
                    If Not lbl Is Nothing Then
                        lbl.Text = (dgFacturasCanceladas.CurrentPageIndex * dgFacturasCanceladas.PageSize) + (e.Item.ItemIndex + 1)
                    End If

                    Dim lnkDescargar As HyperLink = e.Item.FindControl("hypPdfFC")
                    FileName = Cryptography.EncryptUnivisitString(DataBinder.Eval(e.Item.DataItem, "folioFactura"))
                lnkDescargar.NavigateUrl = String.Concat(AppSettings("DIR_INVOICE_PRINTING"), "DigitalInvoiceHelper.aspx") & "?action=Download&Invoice=" & Server.UrlEncode(FileName) & "&format=" & DigitalInvoiceFormat.Pdf.ToString
                    lnkDescargar.ToolTip = PortalCulture.GetString("00906")

                    lnkDescargar = e.Item.FindControl("hypXmlFC")
                lnkDescargar.NavigateUrl = String.Concat(AppSettings("DIR_INVOICE_PRINTING"), "DigitalInvoiceHelper.aspx") & "?action=Download&Invoice=" & Server.UrlEncode(FileName) & "&format=" & DigitalInvoiceFormat.Xml.ToString
                lnkDescargar.ToolTip = PortalCulture.GetString("00907")

                lnkDescargar = e.Item.FindControl("hypExcFC")
                lnkDescargar.NavigateUrl = String.Format("{0}BillingStatementHelper.aspx?action={1}&BillingStatement={2}", _
                               AppSettings("DIR_INVOICE_PRINTING"), _
                               "Download", _
                               Server.UrlEncode(Cryptography.EncryptUnivisitString(String.Format("{0}", estadoCuenta _
                               ))))
                lnkDescargar.ToolTip = PortalCulture.GetString("01340")
        End Select

    End Sub

    Private Sub btnMostrarFactura_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMostrarFactura.Click
        LoadDataFactura(Not ckbEC.Checked, Not ckbF.Checked)
    End Sub

    Private Sub ctrHotelsUserChain1_onSelected(ByVal CompanyID As Integer) Handles ctrHotelsUserChain1.onSelected
        CompanyID = ctrHotelsUserChain1.CurrentSelectedCompanyID
        LoadDataEstadoCuenta()
        LoadDataFactura()

    End Sub
End Class
