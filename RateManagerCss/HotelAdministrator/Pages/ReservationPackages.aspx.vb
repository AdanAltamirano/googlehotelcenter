Public Partial Class ReservationPackages
    Inherits PaginaBase



    Public Enum Columns As Integer
        Itinerario
        Id
        Fecha
        TravelerName
        Llegada
        Salida
        Status
        StatusNew
        fee
    End Enum

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)

        If Not MyBase.IsSupervisor AndAlso Not MyBase.isUserChain AndAlso MyBase.cInfoActual.Hotel <> 0 AndAlso MyBase.cInfoActual.EsMoroso Then
            MyBase.redirectTo(PaginaBase.pages.IsDefaulter)
        End If
        'CtrlHeader1.Panel = ctrlHeader.Usuario.uHotelAdministrador
        'Introducir aquí el código de usuario para inicializar la página
        CtrlReservationsQuery1.idHotel = MyBase.cInfoActual.Hotel
        CtrlReservationsQuery1.Supervisor = Me.IsSupervisor
        Me.DefaultButton(CtrlReservationsQuery1.getTxtName, CtrlReservationsQuery1.getbtnName)
    End Sub

    Public Function ColumunsName(ByVal col As Columns) As String
        Select Case col
            Case Columns.Itinerario
                Return PortalCulture.GetString("M0BT0000117") 'PortalCulture.GetString("M000119")
            Case Columns.Fecha
                Return PortalCulture.GetString("M000120")
            Case Columns.TravelerName
                Return PortalCulture.GetString("M000121")
            Case Columns.Llegada
                Return PortalCulture.GetString("M000122")
            Case Columns.Salida
                Return PortalCulture.GetString("M000123")
            Case Columns.fee
                Return PortalCulture.GetString("M0BT0000322").Substring(0, 8)
        End Select
    End Function


    Private Sub Grid_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles Grid.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim status() As String = {"undefined", PortalCulture.GetString("M000331"), PortalCulture.GetString("M000592"), PortalCulture.GetString("M000333"), PortalCulture.GetString("00719")}
            e.Item.Cells(Columns.StatusNew).Text = status(CType(e.Item.Cells(Columns.Status).Text, Integer))
            
            
            'If CBool(e.Item.Cells(Columns.EarlyCheckOut).Text) = True Then
            '    lb.Text &= " / " & PortalCulture.GetString("01586")
            'End If
            'If CBool(e.Item.Cells(Columns.NoShow).Text) = True Then
            '    lb.Text &= " / No Show"
            'End If

            If Not e.Item.Cells(Columns.Llegada).Text = "" Then
                e.Item.Cells(Columns.Llegada).Text = CDate(e.Item.Cells(Columns.Llegada).Text).ToString("MMM/dd/yyyy")
            End If
            If Not e.Item.Cells(Columns.Salida).Text = "" Then
                e.Item.Cells(Columns.Salida).Text = CDate(e.Item.Cells(Columns.Salida).Text).ToString("MMM/dd/yyyy")
            End If
            If Not e.Item.Cells(Columns.Fecha).Text = "" Then
                e.Item.Cells(Columns.Fecha).Text = CDate(e.Item.Cells(Columns.Fecha).Text).ToString("MMM/dd/yyyy")
            End If

            If Not e.Item.Cells(Columns.fee).Text = "" Then
                e.Item.Cells(Columns.fee).Text = Format(CDbl(e.Item.Cells(Columns.fee).Text), "##,##00.00")
            End If

        ElseIf e.Item.ItemType = ListItemType.Footer Then

        End If
    End Sub

    Private Sub Grid_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles Grid.ItemCreated
        If e.Item.ItemType = ListItemType.Pager Then
            If Me.Grid.CurrentPageIndex > 0 Then
                Dim prev As New System.Web.UI.WebControls.LinkButton
                prev.CommandArgument = "Prev"
                prev.CommandName = "Page"
                prev.Text = "<&nbsp;" & PortalCulture.GetString("00010")
                prev.CausesValidation = False
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(0, New System.Web.UI.LiteralControl("&nbsp;"))
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(0, prev)
            End If
            If Me.Grid.CurrentPageIndex < Me.Grid.PageCount - 1 Then
                Dim _next As New System.Web.UI.WebControls.LinkButton
                _next.CommandArgument = "Next"
                _next.CommandName = "Page"
                _next.Text = PortalCulture.GetString("00011") & "&nbsp;>"
                _next.CausesValidation = False

                CType(e.Item.Controls(0), TableCell).Controls.AddAt(CType(e.Item.Controls(0), TableCell).Controls.Count, New System.Web.UI.LiteralControl("&nbsp;"))
                CType(e.Item.Controls(0), TableCell).Controls.AddAt(CType(e.Item.Controls(0), TableCell).Controls.Count, _next)
            End If
        End If
    End Sub

    Private Sub Grid_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles Grid.PageIndexChanged
        loadData(e.NewPageIndex)
    End Sub

    Private Sub loadData(ByVal GridCurrentPageIndex As Integer) Handles CtrlReservationsQuery1.loadData

        Grid.CurrentPageIndex = GridCurrentPageIndex
        Me.Grid.DataKeyField = "Itinerary"
        Dim ci As System.Globalization.CultureInfo
        ci = System.Threading.Thread.CurrentThread.CurrentCulture
        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString)
        Me.Grid.DataSource = CtrlReservationsQuery1.searchReservations()
        Me.Grid.DataBind()
        System.Threading.Thread.CurrentThread.CurrentCulture = ci
    End Sub

    Private Sub loadResources()
        Me.lblTitle.Text = PortalCulture.GetString("M000117")
        Grid.PagerStyle.PrevPageText = "<< " & PortalCulture.GetString("M000056")
        Grid.PagerStyle.NextPageText = PortalCulture.GetString("M000057") & " >>"
        Me.Grid.Columns(Columns.Itinerario).HeaderText = ColumunsName(Columns.Itinerario)
        Me.Grid.Columns(Columns.Fecha).HeaderText = ColumunsName(Columns.Fecha)
        Me.Grid.Columns(Columns.TravelerName).HeaderText = ColumunsName(Columns.TravelerName)

        Me.Grid.Columns(Columns.Llegada).HeaderText = ColumunsName(Columns.Llegada)
        Me.Grid.Columns(Columns.Salida).HeaderText = ColumunsName(Columns.Salida)
        Me.Grid.Columns(Columns.Status).HeaderText = ColumunsName(Columns.Status)
        Me.Grid.Columns(Columns.fee).HeaderText = ColumunsName(Columns.fee)


    End Sub

    Private Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        loadResources()
        If Not Me.IsPostBack Then
            loadData(0)
        End If
    End Sub

    Private Sub Grid_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles Grid.ItemCommand
        If e.CommandName = "DetalleReserva" Then
            RedirectDetails(Grid.DataKeys(e.Item.ItemIndex))
        End If
    End Sub

    Private Sub RedirectDetails(ByVal id As String)
        MyBase.redirectTo(PaginaBase.pages.ItineraryDetails, "?itinerary=" & id)
    End Sub

    Private Sub CtrlReservationsQuery1_onSendReservation(ByVal res As DataView) Handles CtrlReservationsQuery1.onSendReservation
        Grid.CurrentPageIndex = 0
        Me.Grid.DataKeyField = "Itinerary"
        Dim ci As System.Globalization.CultureInfo
        ci = System.Threading.Thread.CurrentThread.CurrentCulture
        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString)
        Me.Grid.DataSource = res
        Me.Grid.DataBind()
        System.Threading.Thread.CurrentThread.CurrentCulture = ci
    End Sub

End Class