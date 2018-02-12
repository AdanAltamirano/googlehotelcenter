Imports System.Configuration.ConfigurationManager
Imports Oz.UniBilling.Common.Hotels
Imports Oz.UniBilling.DataSchemas.Hotels
Imports Oz.UniBilling.Hotels.Business

Partial Class PaymentInvoicesSummary
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

    Protected HotelInvoiceViewer1 As HotelInvoiceViewer
    Protected HotelIdentity As New HotelIdentifier

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Introducir aquí el código de usuario para inicializar la página
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)
        Me.HeaderPanel = ctrlHeader.Usuario.uHotelAdministrador

        HotelIdentity.CompanyID = MyBase.cInfoActual.Empresa

        searchInvoices()
        ' DataBind()

    End Sub

    Private Sub searchInvoices()
        Dim ds As New BillingStatementDataSet
        Dim BillingMng As New BillingManager(Oz.UniBilling.Common.Security.cSecurity.GetIsser(1), AppSettings("UserID"), "ratemanager@univisit.com")

        BillingMng.GetBillingStatements(HotelIdentity, Oz.UniBilling.Common.BillingStatus.Generated, Oz.UniBilling.Common.BillingStatus.Reconciled, Oz.UniBilling.Common.BillingStatus.Generated, ds)

        If ds.Hotels.Count > 0 Then
            If Not ds.Hotels(0).IsPortalHotelIDNull Then HotelIdentity.PortalID = ds.Hotels(0).PortalHotelID
            If Not ds.Hotels(0).IsUniPantallaHotelIDNull Then HotelIdentity.UniPantallaID = ds.Hotels(0).UniPantallaHotelID
        End If
        RemoveDeleted(ds)
        If ds.BillingStatements.Count > 0 Then
            HotelInvoiceViewer1.DataSource = ds
            HotelInvoiceViewer1.DataBind()
        End If

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
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        lblTitle.Text = PortalCulture.GetString("M0BT0000305")
        hypTaskList.Text = PortalCulture.GetString("M0BT0000148")
        hypTaskList.NavigateUrl = String.Format("{0}MainInvoicing.aspx?cid={1}", GeRequestApplicationPath("/HotelAdministrator/Invoicing/"), cInfoActual.Empresa)
    End Sub

End Class
