<%--@ Page Language="vb" AutoEventWireup="false" CodeBehind="PaidOnlineReservations.aspx.vb" Inherits="RateManager.PaidOnlineReservations" --%>

<%@ Page Language="vb" AutoEventWireup="false" Inherits="RateManager.PaginaBase" %>

<%@ Import Namespace="RateManager" %>
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="uc1" TagName="ctrlReservationsQuery" Src="../Modules/ctrlReservationsQuery.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../../Portal/Modules/ctrlFooter.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrMenu" Src="../../Portal/Modules/CtrMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../../Portal/Modules/ctrlHeader.ascx" %>

<script runat="server">
    
    Public Enum Columns As Integer
        Itinerario
        Id
        Autorizacion
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
        
        Me.CtrlReservationsQuery1.QueryType = ctrlReservations.QueryTypes.PaidOnline
        
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
            Case Columns.Autorizacion
                Return PortalCulture.GetString("01402")
            Case Columns.Cantidad
                Return PortalCulture.GetString("00062")
            Case Columns.StatusNew
                Return PortalCulture.GetString("M000126")
            Case Else
                Return String.Empty
        End Select
    End Function


    Private Sub Grid_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles Grid.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim status() As String = {"undefined", PortalCulture.GetString("M000331"), PortalCulture.GetString("M000592"), PortalCulture.GetString("M000333"), PortalCulture.GetString("00719")}
            'e.Item.Cells(Columns.Status).Text = status(CType(e.Item.Cells(8).Text, Integer))
            Dim lb As Label
            lb = e.Item.Cells(Columns.StatusNew).FindControl("lblStatus")
            lb.Text = status(CType(e.Item.Cells(Columns.Status).Text, Integer))
            If e.Item.Cells(Columns.Status).Text = "1" And e.Item.Cells(Columns.Guaranteed).Text = "0" Then lb.Text = lb.Text & " " & PortalCulture.GetString("1529")
            
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
        Me.lblTitle.Text = PortalCulture.GetString("01403")
        Grid.PagerStyle.PrevPageText = "<< " & PortalCulture.GetString("M000056")
        Grid.PagerStyle.NextPageText = PortalCulture.GetString("M000057") & " >>"
        Me.Grid.Columns(Columns.Itinerario).HeaderText = ColumunsName(Columns.Itinerario)
        Me.Grid.Columns(Columns.Fecha).HeaderText = ColumunsName(Columns.Fecha)
        Me.Grid.Columns(Columns.Autorizacion).HeaderText = ColumunsName(Columns.Autorizacion)
        Me.Grid.Columns(Columns.Cliente).HeaderText = ColumunsName(Columns.Cliente)
        Me.Grid.Columns(Columns.Llegada).HeaderText = ColumunsName(Columns.Llegada)
        Me.Grid.Columns(Columns.Salida).HeaderText = ColumunsName(Columns.Salida)
        Me.Grid.Columns(Columns.TipoHab).HeaderText = ColumunsName(Columns.TipoHab)
        Me.Grid.Columns(Columns.Cantidad).HeaderText = ColumunsName(Columns.Cantidad)
        Me.Grid.Columns(Columns.Status).HeaderText = ColumunsName(Columns.Status)

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
        MyBase.redirectTo(PaginaBase.pages.ReservaDetails, "?qs=" & id & "&returnUrl=" & HttpUtility.UrlEncode(Me.Request.Url.ToString()))
    End Sub

    Private Sub CtrlReservationsQuery1_onSendReservation(ByVal idReservacion As Integer) Handles CtrlReservationsQuery1.onSendReservation
        RedirectDetails(idReservacion)
    End Sub
</script>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>Reservations</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
</head>
<body bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0" ms_positioning="FlowLayout">
    <form id="FormX" method="post" runat="server">
    <div class="mDiv">
    </div>
    <div class="title">
        <asp:Label ID="lblTitle" runat="server" EnableViewState="False" CssClass="tituloSeccion">Consulta de reservaciones</asp:Label>
    </div>
    </div>
    <table id="bookingcontainer" cellspacing="0" cellpadding="2" width="650" border="0">
        <tr>
            <td>
                <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                    <tr>
                        <td>
                            <uc1:ctrlReservationsQuery ID="CtrlReservationsQuery1" runat="server"></uc1:ctrlReservationsQuery>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                        </td>
                    </tr>
                    <tr height="5">
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:DataGrid ID="Grid" runat="server" CssClass="DataGrid" ShowFooter="True" CellSpacing="0"
                                AutoGenerateColumns="False" AllowPaging="True" Width="95%" PageSize="15">
                                <SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
                                <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
                                <ItemStyle CssClass="dgItem"></ItemStyle>
                                <HeaderStyle CssClass="dgHeader"></HeaderStyle>
                                <FooterStyle HorizontalAlign="Right"></FooterStyle>
                                <Columns>
                                    <asp:TemplateColumn HeaderText="Itinerario">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkItinerario" runat="server" CausesValidation="false" CommandName="DetalleReserva"
                                                CssClass="dgLink">
														<%# DataBinder.Eval(Container, "DataItem.NoReservacion") %>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="AuthorizationNumber" HeaderText="No. Autorización"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="FechaReservacion" HeaderText="Fecha"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="TravelerName" HeaderText="Cliente"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="CheckIn" HeaderText="Llegada"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="CheckOut" HeaderText="Salida"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="NombreHabitacion" HeaderText="Tipo de habitaci&#243;n">                                  </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Cantidad" HeaderText="Hab.">
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="status" HeaderText="Status"></asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="Status">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn Visible="False" DataField="StatusConf" HeaderText="StatusConf">                                    </asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="WizcomPassOn" HeaderText="WizcomPassOn">                                    </asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="Guaranteed"></asp:BoundColumn>
                                </Columns>
                                <PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
                                    Position="Bottom" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
                            </asp:DataGrid>
                        </td>
                    </tr>
                    <tr height="5">
                        <td>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
