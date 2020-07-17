<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UserPermissionCards.aspx.vb" Inherits="RateManager.UserPermissionCards" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>TarjetasHotel</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link id="LINK1" href="../../StyleSheets/Styles.css" type="text/css" rel="stylesheet" runat="server"></link>
</head>
<body bottommargin="0" leftmargin="0" topmargin="5" rightmargin="0">
    <form id="Form1" method="post" runat="server">
        <div class="clear">
            <div class="mDiv">
            </div>
            <div class="title">
                <asp:Label ID="lblTitulo" runat="server" EnableViewState="False" Text="Tarjetas Aceptadas" CssClass="tituloSeccion"></asp:Label>
            </div>
        </div>
        <table id="Table1" cellspacing="0" cellpadding="0" width="770" border="0">
            <tr height="300">
                <td align="center">
                    <table id="Bookingcontainer" cellspacing="0" cellpadding="2" width="300" border="0">
                        <tr>
                            <td align="left" colspan="5" height="5"></td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="lblNombreUsuario" EnableViewState="False" CssClass="clsLabel" runat="server">Nombre de Usuario:</asp:Label>&nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmailSearch" CssClass="textbox" runat="server"
                                    MaxLength="80" Width="200px"></asp:TextBox>
                            </td>
                            <td align="right">
                                <asp:Label ID="lblNombreHotel" EnableViewState="False" CssClass="clsLabel" runat="server">Nombre del Hotel:</asp:Label>&nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtHotelSearch" CssClass="textbox" runat="server"
                                    MaxLength="80" Width="200px"></asp:TextBox>
                            </td>
                            <td align="left">
                                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="Button" EnableViewState="False" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="5">
                                <asp:Literal ID="ltlUserName" runat="server" Visible="false"></asp:Literal>
                                <asp:DataGrid ID="Grid" GridLines="None" runat="server" Width="99%" CssClass="DataGrid" PageSize="15"
                                    AllowPaging="True" AutoGenerateColumns="False" Border="0" CellSpacing="1" ShowFooter="True" DataKeyField="UserID">
                                    <FooterStyle HorizontalAlign="Right"></FooterStyle>
                                    <SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
                                    <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
                                    <ItemStyle CssClass="dgItem"></ItemStyle>
                                    <HeaderStyle CssClass="dgHeader"></HeaderStyle>
                                    <Columns>
                                        <asp:BoundColumn DataField="UserID" HeaderText="UserID" Visible="false"></asp:BoundColumn>                                        
                                        <asp:BoundColumn DataField="Email" HeaderText="Email"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="CompanyName" HeaderText="Nombre"></asp:BoundColumn>
                                        <asp:TemplateColumn Visible="true" HeaderText="Permission">
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkAdd" runat="server"></asp:CheckBox>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:BoundColumn DataField="CompanyId" HeaderText="CompanyId" Visible="false"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="AllowSeeCC" HeaderText="AllowSeeCC" Visible="false"></asp:BoundColumn>
                                    </Columns>
                                    <PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
                                        Position="Bottom" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
                                </asp:DataGrid>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" height="5" align="center">
                                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="Button" EnableViewState="False" Visible="false"></asp:Button>
                                <asp:Label ID="lblMensajes" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5"></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
