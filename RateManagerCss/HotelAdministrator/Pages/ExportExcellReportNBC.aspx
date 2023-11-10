<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ExportExcellReportNBC.aspx.vb" Inherits="RateManager.ExportExcellReportNBC" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>ExportExcel - Report Nights By Channel</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
</head>
<body ms_positioning="GridLayout">

    <form id="Form1" method="post" runat="server">

        <asp:Table runat="server" ID="tblTitle" HorizontalAlign="center" BorderColor="Black" BorderStyle="Solid" >
            <asp:TableRow>
                <asp:TableCell><asp:Label runat="server" ID="lblHotel">Title</asp:Label></asp:TableCell>
            </asp:TableRow>
        </asp:Table>

        <asp:Table runat="server" ID="tblHeaders" >
            <asp:TableRow>
                <asp:TableCell><asp:Label runat="server" ID="lblReportName">Noches por dia por canal</asp:Label></asp:TableCell>
                <asp:TableCell><asp:Label runat="server" ID="thWhiteSpace1"></asp:Label></asp:TableCell>
                <asp:TableCell><asp:Label runat="server" ID="thWhiteSpace2"></asp:Label></asp:TableCell>
                <asp:TableCell><asp:Label runat="server" ID="lblGenerated"></asp:Label></asp:TableCell>
            </asp:TableRow>
        </asp:Table>

        <asp:DataGrid ID="Grid" Style="z-index: 101; left: 8px; position: absolute; top: 8px" runat="server" AutoGenerateColumns="True" HorizontalAlign="Center" Width="95%" CssClass="DataGrid">
            <FooterStyle HorizontalAlign="Center"></FooterStyle>

            <SelectedItemStyle HorizontalAlign="Center" CssClass="dgSelected"></SelectedItemStyle>

            <AlternatingItemStyle HorizontalAlign="Center" CssClass="dgAlternate"></AlternatingItemStyle>

            <ItemStyle HorizontalAlign="Center" CssClass="dgItem"></ItemStyle>

            <HeaderStyle HorizontalAlign="Center" CssClass="dgHeader"></HeaderStyle>

        </asp:DataGrid>

    </form>

</body>
</html>
