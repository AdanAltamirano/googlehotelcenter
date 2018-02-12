<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ExcelWaitListReport.aspx.vb" Inherits="RateManager.ExcelWaitListReport" %>

<!DOCTYPE html>

<html>
<head>
    <title>ExportExcel</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
</head>
<body ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
        <asp:DataGrid ID="Grid" Style="LEFT: 8px; POSITION: absolute; TOP: 8px" runat="server" AutoGenerateColumns="False" Width="95%">
            <FooterStyle HorizontalAlign="Right"></FooterStyle>
            <AlternatingItemStyle BackColor="#ccffff"></AlternatingItemStyle>
            <Columns>
                <asp:TemplateColumn HeaderText="Referencia">
                    <ItemTemplate>
                        <%#Eval("id").ToString().PadLeft(8, "0")%>
                    </ItemTemplate>
                    <HeaderStyle Width="40px" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateColumn>
                <asp:BoundColumn DataField="hotelName" HeaderText="Hotel"></asp:BoundColumn>
                <asp:BoundColumn DataFormatString="{0:dd/MMM/yyyy}" DataField="date" HeaderText="Fecha de registro">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundColumn>
                <asp:TemplateColumn HeaderText="Nombre">
                    <ItemTemplate>
                        <%#Eval("firstName") + " " + Eval("lastName")%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn DataFormatString="{0:dd/MMM/yyyy}" DataField="checkIn" HeaderText="Fecha Llegada">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundColumn>
                <asp:TemplateColumn HeaderText="Noches">
                    <ItemTemplate>
                        <%#Convert.ToDateTime(Eval("checkOut")).Subtract(Convert.ToDateTime(Eval("checkIn"))).TotalDays.ToString()%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateColumn>
                <asp:BoundColumn DataField="Rooms" HeaderText="Rooms">
                    <HeaderStyle Width="80px" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundColumn>
                <asp:BoundColumn DataField="RateCode" HeaderText="Rate Code">
                    <HeaderStyle Width="100px" />
                </asp:BoundColumn>
                <asp:BoundColumn DataField="Status" HeaderText="Status">
                    <HeaderStyle Width="100px" />
                </asp:BoundColumn>
            </Columns>
            <PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right" Position="Top" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
        </asp:DataGrid>
    </form>
</body>
</html>
