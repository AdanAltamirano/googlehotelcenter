<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DataClientPermission.aspx.vb" Inherits="RateManager.DataClientPermission" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../Includes/estilos.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label runat="server" ID="lblName" CssClass="clsDarkLabel">Hotel :</asp:Label>
            <asp:TextBox Height="16px" runat="server" ID="txtName" CssClasºs="textbox"></asp:TextBox>
            <asp:Button runat="server" ID="btnSearch" CssClass="button" Text="Buscar" />
        </div>
        <div style="width:500px; height:525px;">
            <asp:DataGrid runat="server" ID="dgHotels" GridLines="None" runat="server" Width="30%" CssClass="DataGrid" PageSize="15"
                AllowPaging="True" AutoGenerateColumns="False" Border="0" CellSpacing="1" ShowFooter="True">
                <FooterStyle HorizontalAlign="Right"></FooterStyle>
                <SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
                <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
                <ItemStyle CssClass="dgItem"></ItemStyle>
                <HeaderStyle CssClass="dgHeader"></HeaderStyle>
                <Columns>
                    <asp:BoundColumn DataField="ID" Visible="false"></asp:BoundColumn>
                    <asp:BoundColumn DataField="NombreEmpresa" HeaderText="Hotel" HeaderStyle-HorizontalAlign="Center"></asp:BoundColumn>
                    <asp:TemplateColumn HeaderText="Ver Datos Del Cliente" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:CheckBox runat="server" id="chkPermission"/>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:BoundColumn DataField="Permission" Visible="false"></asp:BoundColumn>
                </Columns>
                <PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
                                    Position="Bottom" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
            </asp:DataGrid>
        </div>
        <div>
            <asp:Button runat="server" ID="btnSave" CssClass="button" Text="Guardar" visible="false"/>
        </div>
    </form>
</body>
</html>
