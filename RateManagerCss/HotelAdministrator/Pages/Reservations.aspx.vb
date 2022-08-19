Partial Class Reservations
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

    Protected WithEvents CtrlReservationsQuery1 As ctrlReservations

    Public Enum Columns As Integer
        Itinerario
        Id
        Fecha
        Cliente
        Llegada
        Salida
        TipoHab
        Cantidad
        Status
        StatusNew
        StatusConf
        WizcomPassOn
        Guaranteed
        Fee
        OnlinePayment
    End Enum

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not MyBase.IsHotelSelected Then MyBase.redirectTo(PaginaBase.pages.Home)

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
                Return PortalCulture.GetString("M000119")
            Case Columns.Fecha
                Return PortalCulture.GetString("M000120")
            Case Columns.Cliente
                Return PortalCulture.GetString("M000121")
            Case Columns.Llegada
                Return PortalCulture.GetString("M000122")
            Case Columns.Salida
                Return PortalCulture.GetString("M000123")
            Case Columns.TipoHab
                Return PortalCulture.GetString("M000124")
            Case Columns.Cantidad
                Return PortalCulture.GetString("00062")
            Case Columns.StatusNew
                Return PortalCulture.GetString("M000126")
            Case Columns.Fee
                Return PortalCulture.GetString("M0BT0000322").Substring(0, 8)
        End Select
    End Function

    Private Sub btnConsult_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConsult.Click

    End Sub

    Private Sub Grid_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles Grid.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim status() As String = {"undefined", PortalCulture.GetString("M000331"), PortalCulture.GetString("M000592"), PortalCulture.GetString("M000333"), PortalCulture.GetString("00719")}
            'e.Item.Cells(Columns.Status).Text = status(CType(e.Item.Cells(8).Text, Integer))
            Dim lb As Label
            lb = e.Item.Cells(Columns.StatusNew).FindControl("lblStatus")
            lb.Text = status(CType(e.Item.Cells(Columns.Status).Text, Integer))

            If e.Item.Cells(Columns.Status).Text = "1" AndAlso e.Item.Cells(Columns.Guaranteed).Text = "0" AndAlso Not e.Item.Cells(Columns.OnlinePayment).Text = "1" Then
                lb.Text = lb.Text & " " & PortalCulture.GetString("01529")
            End If

            If e.Item.Cells(Columns.WizcomPassOn).Text <> "&nbsp;" AndAlso e.Item.Cells(Columns.Status).Text = "1" Then
                lb.Text = PortalCulture.GetString("00812")
                If e.Item.Cells(Columns.StatusConf).Text = "True" Then
                    lb.Text = status(CType(e.Item.Cells(Columns.Status).Text, Integer))
                    If e.Item.Cells(Columns.Status).Text = "1" And e.Item.Cells(Columns.Guaranteed).Text = "0" Then lb.Text = lb.Text & " " & PortalCulture.GetString("1529")
                End If
            End If
            e.Item.Cells(Columns.Llegada).Text = CDate(e.Item.Cells(Columns.Llegada).Text).ToString("MMM/dd/yyyy")
            e.Item.Cells(Columns.Salida).Text = CDate(e.Item.Cells(Columns.Salida).Text).ToString("MMM/dd/yyyy")
            e.Item.Cells(Columns.Fecha).Text = CDate(e.Item.Cells(Columns.Fecha).Text).ToString("MMM/dd/yyyy")
            If Not e.Item.Cells(Columns.Fee).Text = "" Then
                e.Item.Cells(Columns.Fee).Text = Format(CDbl(e.Item.Cells(Columns.Fee).Text), "##,##00.00")
            End If
        ElseIf e.Item.ItemType = ListItemType.Footer Then
            e.Item.Cells(Columns.StatusNew).Text = CType(Grid.DataSource, DataView).Count & " " & PortalCulture.GetString("M000028")
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
        Me.Grid.DataKeyField = "ID"
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
        Me.Grid.Columns(Columns.Cliente).HeaderText = ColumunsName(Columns.Cliente)
        Me.Grid.Columns(Columns.Llegada).HeaderText = ColumunsName(Columns.Llegada)
        Me.Grid.Columns(Columns.Salida).HeaderText = ColumunsName(Columns.Salida)
        Me.Grid.Columns(Columns.TipoHab).HeaderText = ColumunsName(Columns.TipoHab)
        Me.Grid.Columns(Columns.Cantidad).HeaderText = ColumunsName(Columns.Cantidad)
        Me.Grid.Columns(Columns.Status).HeaderText = ColumunsName(Columns.Status)
        Me.Grid.Columns(Columns.Fee).HeaderText = ColumunsName(Columns.Fee)

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
        MyBase.redirectTo(PaginaBase.pages.ReservaDetailsV2, "?qs=" & id)
    End Sub

    Private Sub CtrlReservationsQuery1_onSendReservation(ByVal idReservacion As Integer) Handles CtrlReservationsQuery1.onSendReservation
        RedirectDetails(idReservacion)
    End Sub
End Class