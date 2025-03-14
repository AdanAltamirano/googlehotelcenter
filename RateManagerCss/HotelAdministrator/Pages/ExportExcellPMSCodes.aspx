<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ExportExcellPMSCodes.aspx.vb" Inherits="RateManager.ExportExcellPMSCodes" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>ExportExcel - PMS Codes by Enterprise</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
</head>
<body ms_positioning="GridLayout">

    <form id="Form1" method="post" runat="server">

        <asp:Table runat="server" ID="generalTable">
            <asp:TableRow HorizontalAlign="center">
                <asp:TableCell BackColor="#3f85c5" ForeColor="White"><asp:Label runat="server"></asp:Label></asp:TableCell>
                <asp:TableCell BackColor="#3f85c5" ForeColor="White">
                    <asp:Label runat="server" ID="LabelHotel" Font-Bold="true">Hotel</asp:Label></asp:TableCell>
                <asp:TableCell BackColor="#3f85c5" ForeColor="White"><asp:Label runat="server" ></asp:Label></asp:TableCell>
                <asp:TableCell BackColor="#3f85c5" ForeColor="White"><asp:Label runat="server"></asp:Label></asp:TableCell>
                <asp:TableCell><asp:Label runat="server"></asp:Label></asp:TableCell>
                <asp:TableCell BackColor="#3f85c5" ForeColor="White"><asp:Label runat="server"></asp:Label></asp:TableCell>
                <asp:TableCell BackColor="#3f85c5" ForeColor="White">
                    <asp:Label runat="server" ID="LabelEndpoint" Font-Bold="true">EndPoint</asp:Label></asp:TableCell>
                <asp:TableCell BackColor="#3f85c5" ForeColor="White"><asp:Label runat="server"></asp:Label></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="center" BorderColor="Black" BorderStyle="Solid"><asp:Label runat="server" >Nombre</asp:Label></asp:TableCell>
                <asp:TableCell HorizontalAlign="center" BorderColor="Black" BorderStyle="Solid"><asp:Label runat="server" >Código</asp:Label></asp:TableCell>
                <asp:TableCell HorizontalAlign="center" BorderColor="Black" BorderStyle="Solid"><asp:Label runat="server" >Usuario</asp:Label></asp:TableCell>
                <asp:TableCell HorizontalAlign="center" BorderColor="Black" BorderStyle="Solid"><asp:Label runat="server" >Contraseña</asp:Label></asp:TableCell>
                <asp:TableCell><asp:Label runat="server" ></asp:Label></asp:TableCell>
                <asp:TableCell HorizontalAlign="Left" Wrap="false">
                    <asp:Label runat="server" ID="txtEndPoint">EndPoint</asp:Label></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow HorizontalAlign="center" BorderColor="Black" BorderStyle="Solid">
                <asp:TableCell>
                    <asp:Label runat="server" ID="txtHotelName">Nombre</asp:Label></asp:TableCell>
                <asp:TableCell>
                    <asp:Label runat="server" ID="txtCode">Código</asp:Label></asp:TableCell>
                <asp:TableCell>
                    <asp:Label runat="server" ID="txtUser">Usuario</asp:Label></asp:TableCell>
                <asp:TableCell>
                    <asp:Label runat="server" ID="txtPass">Contraseña</asp:Label></asp:TableCell>
            </asp:TableRow>
        </asp:Table>

        <asp:Table runat="server" ID="tblRooms">
            <asp:TableRow>
                <asp:TableCell>
                    <asp:Label runat="server" ID="Label1"> </asp:Label></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <asp:Label runat="server" ID="Label2"> </asp:Label></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell BackColor="#3f85c5" ForeColor="White"><asp:Label runat="server"></asp:Label></asp:TableCell>
                <asp:TableCell BackColor="#3f85c5" ForeColor="White">
                    <asp:Label runat="server" ID="Label3" Font-Bold="true">Habitaciones</asp:Label></asp:TableCell>
            </asp:TableRow>
        </asp:Table>

        <asp:DataGrid ID="dgRooms" Style="z-index: 101; left: 8px; position: absolute; top: 8px" runat="server" AutoGenerateColumns="True" HorizontalAlign="Center" Width="95%" CssClass="DataGrid" ShowFooter="false">
            <SelectedItemStyle HorizontalAlign="Center" CssClass="dgSelected"></SelectedItemStyle>
            <AlternatingItemStyle HorizontalAlign="Center" CssClass="dgAlternate" VerticalAlign="Middle"></AlternatingItemStyle>
            <ItemStyle HorizontalAlign="Center" CssClass="dgItem" VerticalAlign="Middle"></ItemStyle>
            <HeaderStyle HorizontalAlign="Center" CssClass="dgHeader" VerticalAlign="Middle"></HeaderStyle>
        </asp:DataGrid>

        <asp:Table runat="server" ID="tblPlans">
            <asp:TableRow>
                <asp:TableCell>
                    <asp:Label runat="server" ID="ws3"> </asp:Label></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <asp:Label runat="server" ID="ws4"> </asp:Label></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell BackColor="#3f85c5" ForeColor="White"><asp:Label runat="server"></asp:Label></asp:TableCell>
                <asp:TableCell BackColor="#3f85c5" ForeColor="White">
                    <asp:Label runat="server" ID="titleRP" Font-Bold="true">Planes Tarifarios</asp:Label></asp:TableCell>
            </asp:TableRow>
        </asp:Table>

        <asp:DataGrid ID="dgRatePlans" Style="z-index: 101; left: 8px; position: absolute; top: 8px" runat="server" AutoGenerateColumns="True" HorizontalAlign="Center" Width="95%" CssClass="DataGrid" ShowFooter="false">
            <SelectedItemStyle HorizontalAlign="Center" CssClass="dgSelected"></SelectedItemStyle>
            <AlternatingItemStyle HorizontalAlign="Center" CssClass="dgAlternate" VerticalAlign="Middle"></AlternatingItemStyle>
            <ItemStyle HorizontalAlign="Center" CssClass="dgItem" VerticalAlign="Middle"></ItemStyle>
            <HeaderStyle HorizontalAlign="Center" CssClass="dgHeader" VerticalAlign="Middle"></HeaderStyle>
        </asp:DataGrid>

    </form>

</body>
</html>
