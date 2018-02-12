Imports Oz.UniBilling.DataSchemas.Hotels
Imports Oz.UniBilling.Common.Business
Imports Oz.UniBilling.DataSchemas.Currency

Partial Class ReservationsNetRate
    Inherits PaginaBase

#Region " Código generado por el Diseñador de Web Forms "

    'El Diseñador de Web Forms requiere esta llamada.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents btnConsult As System.Web.UI.WebControls.Button

    'NOTA: el Diseñador de Web Forms necesita la siguiente declaración del marcador de posición.
    'No se debe eliminar o mover.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: el Diseñador de Web Forms requiere esta llamada de método
        'No la modifique con el editor de código.
        InitializeComponent()
    End Sub

#End Region


    Protected WithEvents CtrlReservationsNetRateQuery1 As ctrlReservationsNetRate
    Public Enum Columns As Integer
        Itinerario
        Fecha
        Cliente
        Llegada
        Salida
        total
        Status
    End Enum

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)

        If Not MyBase.IsSupervisor AndAlso Not MyBase.isUserChain AndAlso MyBase.cInfoActual.Hotel <> 0 AndAlso MyBase.cInfoActual.EsMoroso Then
            MyBase.redirectTo(PaginaBase.pages.IsDefaulter)
        End If
        If Not IsPostBack Then
            TablaTotales.Visible = False
        End If



        CtrlReservationsNetRateQuery1.idHotel = MyBase.cInfoActual.Hotel
        CtrlReservationsNetRateQuery1.Supervisor = Me.IsSupervisor
        Me.DefaultButton(CtrlReservationsNetRateQuery1.getTxtName, CtrlReservationsNetRateQuery1.getbtnName)
    End Sub

    Public Function ColumunsName(ByVal col As Columns) As String
        Select Case col
            Case Columns.Itinerario
                Return PortalCulture.GetString("M000119")
            Case Columns.Fecha
                Return PortalCulture.GetString("M000120")
            Case Columns.Cliente
                Return PortalCulture.GetString("M000121")
            Case Columns.Llegada
                Return PortalCulture.GetString("M000122")
            Case Columns.Salida
                Return PortalCulture.GetString("M000123")
            Case Columns.total
                Return PortalCulture.GetString("M0BT0000045")
            Case Columns.Status
                Return PortalCulture.GetString("00283")


        End Select
    End Function


    Private Sub Grid_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles Grid.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim lbl As Label = Nothing
            lbl = e.Item.FindControl("glblTotal")

            Dim status As Integer = CType((DataBinder.Eval(e.Item.DataItem, "ConciliationStatus")), Integer)

            If (status = 0 AndAlso Not lbl Is Nothing AndAlso Not (DataBinder.Eval(e.Item.DataItem, "TotalNR")) Is DBNull.Value) Then
                lbl.Text = CType(DataBinder.Eval(e.Item.DataItem, "TotalNR"), Decimal).ToString("0.00") + (CType(DataBinder.Eval(e.Item.DataItem, "CurrencyCode"), String))
            ElseIf (status <> 0 AndAlso Not lbl Is Nothing AndAlso Not (DataBinder.Eval(e.Item.DataItem, "ConciliationAmount")) Is DBNull.Value) Then
                lbl.Text = CType(DataBinder.Eval(e.Item.DataItem, "ConciliationAmount"), Decimal).ToString("0.00") + CType(DataBinder.Eval(e.Item.DataItem, "ConciliationCurrencyCode"), String)
            End If

            lbl = e.Item.FindControl("glblEstatus")

            If status <> 3 Then
                lbl.Text = PortalCulture.GetString("01142")
            Else
                lbl.Text = PortalCulture.GetString("00585")

            End If
        End If
    End Sub



    Private Sub Grid_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles Grid.PageIndexChanged
        loadData(e.NewPageIndex)
    End Sub

    Private Sub CtrlReservationsNetRateQuery1_onSendReservation() Handles CtrlReservationsNetRateQuery1.onSendReservation
        Dim dsReservation As ReservationNetRateDataSet = CtrlReservationsNetRateQuery1.DataSource()
        Me.Grid.DataSource = dsReservation.Reservation
        Me.Grid.DataKeyField = "ReservationID"
        Grid.CurrentPageIndex = 0
        Me.Grid.DataBind()
        showTotals(dsReservation)

    End Sub


    Private Sub loadData(ByVal GridCurrentPageIndex As Integer)
        Dim dsReservation As ReservationNetRateDataSet = CtrlReservationsNetRateQuery1.DataSource()
        Me.Grid.DataSource = dsReservation.Reservation
        Me.Grid.DataKeyField = "ReservationID"
        Grid.CurrentPageIndex = GridCurrentPageIndex
        Me.Grid.DataBind()
        showTotals(dsReservation)
    End Sub



    Private Sub showTotals(ByVal dsNetRate As ReservationNetRateDataSet)
        Dim enDolares As Boolean = CtrlReservationsNetRateQuery1.EnDolares
        Dim codigoMoneda As String = "MXN"
        If enDolares Then codigoMoneda = "USD"
        TablaTotales.Visible = True

        lblNumeroReservaciones.Text = "0"
        lblTotal.Text = "0.00" & codigoMoneda

        If Not dsNetRate Is Nothing AndAlso dsNetRate.Reservation.Count > 0 Then
            Dim totalCostoHotel As Decimal = 0
            Dim manager As New MoneyManager
            Dim dsCurrency As CurrencyDataSet = manager.GetAllCurrencies()
            For Each drNetRate As ReservationNetRateDataSet.ReservationRow In dsNetRate.Reservation
                If drNetRate.ConciliationStatus <> 0 Then
                    totalCostoHotel += CovetirTipoCambioConciliado(drNetRate.ConciliationAmount, drNetRate.ConciliationCurrencyCode, drNetRate.ConciliationMoneyExchangeRateMxn, drNetRate.ConciliationMoneyExchangeRateUsd, enDolares)
                Else
                    totalCostoHotel += CovetirTipoCambio(drNetRate, drNetRate.TotalNR, drNetRate.CurrencyCode, dsCurrency, enDolares)
                End If
            Next
            lblNumeroReservaciones.Text = dsNetRate.Reservation.Count.ToString()
            lblTotal.Text = String.Format("{0}{1}", FCurrency(totalCostoHotel, 2), codigoMoneda)
        End If
    End Sub


    Private Function CovetirTipoCambioConciliado(ByVal total As Decimal, ByVal monedaOrigen As String, ByVal tipoCambioPesos As Decimal, ByVal tipoCambioDolares As Decimal, ByVal enDolares As Boolean) As Decimal
        If monedaOrigen.ToUpper().Trim() = "USD" AndAlso enDolares Then
            Return total
        ElseIf monedaOrigen.ToUpper().Trim() = "MXN" AndAlso Not enDolares Then
            Return total
        Else
            If enDolares Then
                Return total / tipoCambioDolares
            Else
                'Pesos 
                Return total * tipoCambioPesos
            End If
        End If
    End Function

    Private Function CovetirTipoCambio(ByVal drNetRate As ReservationNetRateDataSet.ReservationRow, ByVal total As Decimal, ByVal monedaOrigen As String, ByVal dsCurrency As CurrencyDataSet, ByVal enDolares As Boolean) As Decimal
        If monedaOrigen.ToUpper().Trim() = "USD" AndAlso enDolares Then
            Return total
        ElseIf monedaOrigen.ToUpper().Trim() = "MXN" AndAlso Not enDolares Then
            Return total
        Else
            If enDolares Then
                Dim drCurrencys As CurrencyDataSet.CurrenciesRow() = CType(dsCurrency.Currencies.[Select](String.Format("CurrencyCode = '{0}'", monedaOrigen.ToUpper().Trim())), CurrencyDataSet.CurrenciesRow())
                If drCurrencys.Length > 0 Then
                    Return total / drCurrencys(0).MoneyExchangeRate
                End If
            Else
                'Pesos 
                Dim pesos As Decimal = 1
                Dim drCurrencys As CurrencyDataSet.CurrenciesRow() = CType(dsCurrency.Currencies.[Select](String.Format("CurrencyCode = 'MXN'")), CurrencyDataSet.CurrenciesRow())
                If drCurrencys.Length > 0 Then
                    pesos = drCurrencys(0).MoneyExchangeRate
                End If

                drCurrencys = CType(dsCurrency.Currencies.[Select](String.Format("CurrencyCode = '{0}'", monedaOrigen.ToUpper().Trim())), CurrencyDataSet.CurrenciesRow())
                If drCurrencys.Length > 0 Then
                    Return (total / drCurrencys(0).MoneyExchangeRate) * pesos
                End If
            End If
        End If
        Return total
    End Function

    Private Sub loadResources()
        Me.lblTitle.Text = PortalCulture.GetString("01144")
        Grid.PagerStyle.PrevPageText = "<< " & PortalCulture.GetString("M000056")
        Grid.PagerStyle.NextPageText = PortalCulture.GetString("M000057") & " >>"
        Me.Grid.Columns(Columns.Itinerario).HeaderText = ColumunsName(Columns.Itinerario)
        Me.Grid.Columns(Columns.Fecha).HeaderText = ColumunsName(Columns.Fecha)
        Me.Grid.Columns(Columns.Cliente).HeaderText = ColumunsName(Columns.Cliente)
        Me.Grid.Columns(Columns.Llegada).HeaderText = ColumunsName(Columns.Llegada)
        Me.Grid.Columns(Columns.Salida).HeaderText = ColumunsName(Columns.Salida)
        Me.Grid.Columns(Columns.total).HeaderText = ColumunsName(Columns.total)
        Me.Grid.Columns(Columns.Status).HeaderText = ColumunsName(Columns.Status)
        Me.lblNumeroReservacionesTitulo.Text = PortalCulture.GetString("01141", True)
        Me.lblTotalTitulo.Text = PortalCulture.GetString("00380")


    End Sub

    Private Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        loadResources()
    End Sub

    Private Sub Grid_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles Grid.ItemCommand
        If e.CommandName = "DetalleReserva" Then
            RedirectDetails(Grid.DataKeys(e.Item.ItemIndex))
        End If
    End Sub

    Private Sub RedirectDetails(ByVal id As String)
        MyBase.redirectTo(PaginaBase.pages.ReservaDetailsV2, "?qs=" & id)
    End Sub

    Private Sub CtrlReservationsQuery1_onSendReservation(ByVal idReservacion As Integer)
        RedirectDetails(idReservacion)
    End Sub
End Class