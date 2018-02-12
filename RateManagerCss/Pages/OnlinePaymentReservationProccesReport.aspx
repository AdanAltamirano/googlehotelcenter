<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="OnlinePaymentReservationProccesReport.aspx.vb" Inherits="RateManager.OnlinePaymentReservationProccesReport" %>

<!DOCTYPE html>
<html>
<head>
    <title>Reservations</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet">


    <script type="text/javascript">
           
    </script>

</head>
<body bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
        <div class="clear">
            <div class="mDiv"></div>
            <div>
                <asp:Label ID="lblTitle" runat="server" EnableViewState="False"
                    Text="Consulta de reservaciones" CssClass="tituloSeccion"></asp:Label>
            </div>
        </div>
        <table id="bookingcontainer" cellspacing="0" cellpadding="2" width="650" border="0">
            <tr>
                <td>
                    <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                        <tr>
                            <td><asp:Label runat="server" CssClass="bookingLabel" ID="lblCheckin">Fecha Inicial:</asp:Label></td>
                            <td><asp:TextBox runat="server" CssClass="textbox" ID="txtCheckin"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td align="center"></td>
                        </tr>
                        <tr height="5">
                            <td></td>
                        </tr>
                        <tr>
                            <td align="center"></td>
                        </tr>
                        <tr height="5">
                            <td></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table style="width: 100%;">
            <tr>
                <td>
                    <asp:DataGrid ID="Grid" GridLines="None" runat="server" CssClass="DataGrid" ShowFooter="True" CellSpacing="0" AutoGenerateColumns="False"
                        AllowPaging="True" Width="95%" PageSize="20">
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
                            <asp:BoundColumn DataField="FechaReservacion" HeaderText="Fecha"></asp:BoundColumn>
                            <asp:BoundColumn DataField="TravelerName" HeaderText="Cliente"></asp:BoundColumn>
                            <asp:BoundColumn DataField="CheckIn" HeaderText="Llegada"></asp:BoundColumn>
                            <asp:BoundColumn DataField="CheckOut" HeaderText="Salida"></asp:BoundColumn>
                            <asp:BoundColumn DataField="NombreHabitacion" HeaderText="Tipo de habitaci&#243;n"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Cantidad" HeaderText="Hab.">
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="status" HeaderText="Status"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Status">
                                <ItemTemplate>
                                    <asp:Label ID="lblStatus" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn Visible="False" DataField="StatusConf" HeaderText="StatusConf"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="WizcomPassOn" HeaderText="WizcomPassOn"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="Guaranteed" HeaderText="Guaranteed"></asp:BoundColumn>
                            <asp:BoundColumn Visible="false" DataField="PorcPor" HeaderText="Fee"></asp:BoundColumn>
                            <asp:BoundColumn Visible="false" DataField="OnlinePayment" HeaderText="OnlinePayment"></asp:BoundColumn>
                        </Columns>
                        <PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
                            Position="Bottom" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
                    </asp:DataGrid>

                </td>
            </tr>
        </table>
    </form>
</body>
</html>
