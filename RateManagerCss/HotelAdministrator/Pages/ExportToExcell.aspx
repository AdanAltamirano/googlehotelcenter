<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ExportToExcell.aspx.vb" Inherits="RateManager.ExportToExcell" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <asp:DataGrid ID="Grid" Style="z-index: 101; left: 8px; position: absolute; top: 8px" runat="server"
        AutoGenerateColumns="False" Width="95%" PageSize="15" CssClass="DataGrid">
        <FooterStyle HorizontalAlign="Right"></FooterStyle>
        <SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
        <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
        <ItemStyle CssClass="dgItem"></ItemStyle>
        <HeaderStyle CssClass="dgHeader"></HeaderStyle>
        <Columns>
        </Columns>
        <PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
            Position="Top" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
    </asp:DataGrid>
    </form>
</body>
</html>
