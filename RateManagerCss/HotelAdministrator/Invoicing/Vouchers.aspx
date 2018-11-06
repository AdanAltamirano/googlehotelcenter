<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Vouchers.aspx.vb" Inherits="RateManager.Vouchers" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" type="text/css" href="../../StyleSheets/Styles.css">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div class="title">
                <asp:Label ID="lblTitle" runat="server" EnableViewState="False"
                    CssClass="tituloSeccion"></asp:Label>
            </div>
        </div>
        <div>
            <asp:Label ID="lblError" CssClass="validators" runat="server" Visible="false">Error</asp:Label>
        </div>
        <div>
            <asp:DataGrid ID="dgComprobantes" runat="server" CssClass="DataGrid" AutoGenerateColumns="False"
                Width="100%" AllowPaging="True" BorderColor="WhiteSmoke">
                <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
                <ItemStyle CssClass="dgitem"></ItemStyle>
                <HeaderStyle CssClass="DGHeader" HorizontalAlign="Center"></HeaderStyle>
                <Columns>
                    <asp:BoundColumn DataField="Pago_Id" HeaderText="ID"></asp:BoundColumn>
                    <asp:BoundColumn DataField="FechaPago" HeaderText="FehcaPago"></asp:BoundColumn>
                    <asp:TemplateColumn HeaderText="Importe">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemTemplate>
                            <asp:Label ID="glblTotal2" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Descargar">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <div style="position: relative; float: left; width: 150px; display: inline; display: block;">
                                <asp:HyperLink ID="hypPdfComp" runat="server" ImageUrl="../../Images/invoice-receipt.jpg" Target="_blank"></asp:HyperLink>
                                <asp:HyperLink ID="hypXmlFC" runat="server" ImageUrl="../../Images/xml.gif" Target="_blank"></asp:HyperLink>
                                <br />
                                <asp:Label ID="lblMsgSelloSat" runat="server" Text=""></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                </Columns>
                <PagerStyle HorizontalAlign="Right" CssClass="dgpager" Mode="NumericPages"></PagerStyle>
            </asp:DataGrid>
        </div>
    </form>
</body>
</html>
