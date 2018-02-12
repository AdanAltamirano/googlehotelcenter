Imports System.Configuration.ConfigurationManager
Imports System.Globalization
Imports Oz.UniBilling.Hotels.Business
Imports Oz.UniBilling.DataSchemas.Hotels
Imports Oz.UniBilling.Common.Hotels


Partial Class InvoicesToConciliate
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

    Private Property dsData() As BillingStatementDataSet
        Get
            Return Session("dsData")
        End Get
        Set(ByVal Value As BillingStatementDataSet)
            Session("dsData") = Value
        End Set
    End Property

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)
        Me.HeaderPanel = ctrlHeader.Usuario.uHotelAdministrador
       
        CompanyID = MyBase.cInfoActual.Empresa

        lblPendingInfo.Text = PortalCulture.GetString("M0BT0000012")

        If Not IsNothing(CompanyID) AndAlso IsNumeric(CompanyID) AndAlso CompanyID > -1 Then

            Dim ds As New BillingStatementDataSet
            Dim HotelIdentity As New HotelIdentifier
            HotelIdentity.CompanyID = CompanyID
            HotelIdentity.PortalID = -1
            HotelIdentity.UniPantallaID = -1

            Dim HotelMng As New HotelManager
            HotelMng.SearchHotelByCompanyID(HotelIdentity.CompanyID, ds)

            If ds.Hotels.Count > 0 Then
                If Not ds.Hotels(0).IsUniPantallaHotelIDNull Then HotelIdentity.UniPantallaID = ds.Hotels(0).UniPantallaHotelID
                If Not ds.Hotels(0).IsPortalHotelIDNull Then HotelIdentity.PortalID = ds.Hotels(0).PortalHotelID

                Dim BillingMng As New BillingManager(Oz.UniBilling.Common.Security.cSecurity.GetIsser(1), AppSettings("UserID"), "ratemanager@univisit.com")
                BillingMng.GetBillingStatements(HotelIdentity, Oz.UniBilling.Common.BillingStatus.Generated, Oz.UniBilling.Common.BillingStatus.Generated, Oz.UniBilling.Common.BillingStatus.Generated, ds)

                RemoveDeleted(ds)

                dsData = ds
                If Not dsData Is Nothing Then
                    If dsData.BillingStatements.Count > 0 Then
                        lblPendingInfo.Visible = False
                        dgSummary.DataSource = dsData
                        dgSummary.DataMember = dsData.BillingStatements.TableName
                        dgSummary.DataBind()
                    Else
                        lblPendingInfo.Text = PortalCulture.GetString("00890")
                        lblPendingInfo.Visible = True
                    End If
                Else
                    lblPendingInfo.Text = PortalCulture.GetString("M0BT0000012")
                    lblPendingInfo.Visible = True
                End If
            Else
                lblPendingInfo.Text = PortalCulture.GetString("M0BT0000012")
                lblPendingInfo.Visible = True
            End If
        Else
            lblPendingInfo.Text = PortalCulture.GetString("M0BT0000012")
            lblPendingInfo.Visible = True
        End If
        DataBind()

    End Sub
    Private Sub RemoveDeleted(ByRef ds As BillingStatementDataSet)
        If Not ds Is Nothing Then
            For Each dr As BillingStatementDataSet.BillingStatementsRow In ds.BillingStatements
                If Not dr.IsActiveNull() Then
                    If Not dr.Active Then
                        dr.Delete()

                    End If
                End If
            Next
            ds.AcceptChanges()
        End If
    End Sub
    Public Function FormatPeriod(ByVal month As Integer, ByVal year As Integer) As String
        Dim fecha As New DateTime(year, month, 1)
        Return fecha.ToString("y", PortalCulture.GetCulture)
    End Function

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        lblTitle.Text = PortalCulture.GetString("M0BT0000026")
        hypMainConciliate.Text = PortalCulture.GetString("M0BT0000148")
        hypMainConciliate.NavigateUrl = String.Format("{0}MainInvoicing.aspx?cid={1}", GeRequestApplicationPath("/HotelAdministrator/Invoicing/"), CompanyID)
    End Sub

    Private Sub dgSummary_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgSummary.ItemCreated
        If e.Item.ItemType = ListItemType.Header Then
            With e.Item
                .Cells(1).Text = Server.HtmlDecode(PortalCulture.GetString("M0BT0000008"))
                .Cells(2).Text = Server.HtmlDecode(PortalCulture.GetString("M0BT0000009"))
                .Cells(3).Text = Server.HtmlDecode(PortalCulture.GetString("M0BT0000010"))
                .Cells(5).Text = Server.HtmlDecode(PortalCulture.GetString("M0BT0000011"))
            End With
        End If
    End Sub

    Private Sub dgConcept_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)
        Select Case e.Item.ItemType
            Case ListItemType.Item, ListItemType.AlternatingItem
                Dim drTransaction As BillingStatementDataSet.BillingStatementTransactionsRow

                drTransaction = dsData.BillingStatementTransactions.FindByBillingStatementTransactionCode(CInt(DataBinder.Eval(e.Item.DataItem, "BillingStatementTransactionCode")))
                If Not drTransaction Is Nothing Then
                    Dim lbl As Label = e.Item.FindControl("lblConcept")
                    If Not lbl Is Nothing Then


                        lbl.Text = "- " & drTransaction.TransactionName 'clsFormat.GetDisplayName(CType(drTransaction.BillingStatementTransactionCode, BillingStatementTransaction))
                    End If
                End If
        End Select
    End Sub

    Private Sub dgExtraCharges_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)
        Select Case e.Item.ItemType
            Case ListItemType.Item, ListItemType.AlternatingItem
                Dim drTransaction As BillingStatementDataSet.BillingStatementTransactionsRow

                drTransaction = dsData.BillingStatementTransactions.FindByBillingStatementTransactionCode(CInt(DataBinder.Eval(e.Item.DataItem, "BillingStatementTransactionCode")))
                If Not drTransaction Is Nothing Then
                    Dim lbl As Label = e.Item.FindControl("lblExtraCharge")
                    If Not lbl Is Nothing Then
                        lbl.Text = "- " & drTransaction.TransactionName  'clsFormat.GetDisplayName(CType(drTransaction.BillingStatementTransactionCode, BillingStatementTransaction))
                        If lbl.Text.Trim = String.Empty Then
                            lbl.Text = "- " & drTransaction.TransactionName
                        End If
                    End If
                End If
        End Select
    End Sub

    Private Sub dgSummary_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgSummary.ItemDataBound
        Select Case e.Item.ItemType
            Case ListItemType.Item, ListItemType.AlternatingItem
                Dim ds As BillingStatementDataSet = dgSummary.DataSource

                Dim dv As New DataView(ds.FixedCharges)
                dv.RowFilter = "BillingStatementID = " & DataBinder.Eval(e.Item.DataItem, "BillingStatementID")

                Dim dg As DataGrid = e.Item.FindControl("dgConcept")
                AddHandler dg.ItemDataBound, AddressOf dgConcept_ItemDataBound
                dg.DataSource = dv
                dg.DataBind()

                Dim dvExtra As New DataView(ds.ExtraCharges)
                dvExtra.RowFilter = "BillingStatementID = " & DataBinder.Eval(e.Item.DataItem, "BillingStatementID")

                dg = e.Item.FindControl("dgExtraCharges")
                AddHandler dg.ItemDataBound, AddressOf dgExtraCharges_ItemDataBound
                dg.DataSource = dvExtra
                dg.DataBind()

                Dim lbl As Label = e.Item.FindControl("lblRegistrationDate")
                If Not lbl Is Nothing Then
                    lbl.Text = CDate(DataBinder.Eval(e.Item.DataItem, "RegistrationDate")).ToString("dd MMMM yyyy", PortalCulture.GetCulture)
                End If

                Dim lnk As HyperLink = e.Item.FindControl("lnkPeriodo")
                If Not lnk Is Nothing Then
                    lnk.Text = "  " & FormatPeriod(DataBinder.Eval(e.Item.DataItem, "Month"), DataBinder.Eval(e.Item.DataItem, "Year"))
                    lnk.NavigateUrl = "InvoiceToConciliateDetails.aspx?id=" & DataBinder.Eval(e.Item.DataItem, "BillingStatementID")
                End If

                Dim dvRva As New DataView(ds.ReservationCharges)
                dvRva.RowFilter = "BillingStatementID = " & DataBinder.Eval(e.Item.DataItem, "BillingStatementID") & " And Status= " & Oz.BillingSystem.Common.BillingStatementStatus.Generated
                lbl = e.Item.FindControl("lblLastConciliationDate")
                If Not lbl Is Nothing Then
                    Dim lblComm As Label = e.Item.FindControl("lblCommissionConcept")
                    'If DataBinder.Eval(e.Item.DataItem, "LimitToReconcile") < Now Then
                    If dvRva.Count > 0 Then
                        If Not lblComm Is Nothing Then
                            lblComm.Text = "- " & PortalCulture.GetString("M0BT0000322")
                            lblComm.Visible = True
                        End If
                        If CDate(dvRva(0).Item("LimitToReconcile")).Date < Now.Date Then
                            lbl.CssClass = "labelRed"
                            'lnk.Enabled = False'
                        End If

                        lbl.Text = CDate(dvRva(0).Item("LimitToReconcile")).ToString("dd MMMM yyyy", PortalCulture.GetCulture)
                    Else
                        If Not lblComm Is Nothing Then lblComm.Visible = False
                        lbl.Font.Italic = True
                        lbl.Text = PortalCulture.GetString("M0BT0000314")
                    End If
                End If

                lbl = e.Item.FindControl("Label1")
                If Not lbl Is Nothing Then
                    lbl.Text = (dgSummary.CurrentPageIndex * dgSummary.PageSize) + (e.Item.ItemIndex + 1)
                End If
        End Select

    End Sub

    Private Sub dgSummary_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgSummary.PageIndexChanged
        If Not dsData Is Nothing Then
            dgSummary.CurrentPageIndex = e.NewPageIndex
            dgSummary.DataSource = dsData
            dgSummary.DataMember = dsData.BillingStatements.TableName
            dgSummary.DataBind()
        End If
    End Sub

End Class
