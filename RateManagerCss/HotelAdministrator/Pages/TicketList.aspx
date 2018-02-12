<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TicketList.aspx.vb" Inherits="RateManager.TicketList" %>
<%@ Import Namespace="Portal.Hotel.Common.Data" %>
<%@ Import Namespace="RateManager" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head id="Head1" runat="server">
    <title>Tickets</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
      <div class="clear">		    
		        <div class="mDiv"></div>
		        <div>
		            <asp:label id="lblTitle" runat="server" EnableViewState="False" CssClass=tituloSeccion ><%=Me.GetLabel("01292")%></asp:label> 
		        </div>
		    </div> 
    <table id="bookingcontainer" cellspacing="0" cellpadding="2" width="750" border="0">
      
        <tr>
            <td style="padding:15px;">
                <p><%=String.Format(Me.GetLabel("01293"), Me.cInfoActual.HotelName)%></p>
            </td>
        </tr>
        <tr class="dgHeader">
            <td align="center" colspan="2" class="dgitem">
                <span>Busqueda Especifica</span>
            </td>
        </tr>
        <tr>
            <td style="padding-left: 15px; padding-right: 15px;">      
                <table width="100%">
                    <tbody><tr>
                        <td align="left">
                            <%=Me.GetLabel("01286")%>&nbsp;:&nbsp;
                            <asp:TextBox ID="txtSubject" runat="server"  style="width: 200px; vertical-align:middle;"></asp:TextBox>
                        </td>
                        <td align="left">
                            <%=Me.GetLabel("00695")%>&nbsp;:&nbsp;
                            <asp:TextBox ID="txtReferencia" runat="server"  style="width: 100px; vertical-align:middle;"></asp:TextBox>
                        </td>       
                    </tr>
                    <tr>
                        <td align="left" colspan="2">
                            <%=Me.GetLabel("01316")%>&nbsp;:&nbsp; 
                            <input id="chkInProcess" type="checkbox" style="vertical-align:middle;" checked="checked" runat="server">En Proceso 
                            <input id="chkClosed" type="checkbox" style="vertical-align:middle;" runat="server">Cerrada
                        </td>
                    </tr>
                    <tr><td align="right" colspan="4">
                        <%
                                Me.btnBuscar.Text = Me.GetLabel("M000637")
                        %>
                        <asp:Button ID="btnBuscar" runat="server" CssClass="Button" />
                    </td></tr>
                </tbody></table>
            </td>
        </tr>
        <tr>
            <td align="center">  
                <br />
                <%
                    Dim sysCulture As System.Globalization.CultureInfo
                    sysCulture = System.Threading.Thread.CurrentThread.CurrentCulture
                    System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(PortalCulture.GetCulture.ToString())
                %>    
                <asp:DataGrid ID="lstTickets" runat="server" CssClass="DataGrid" 
                    ShowFooter="True" AutoGenerateColumns="False" AllowPaging="True" Width="95%" 
                    PageSize="20" GridLines="None">                    
                    <SelectedItemStyle CssClass="dgSelected" Font-Bold="False" Font-Italic="False" 
                        Font-Overline="False" Font-Strikeout="False" Font-Underline="False" 
                        HorizontalAlign="Left"></SelectedItemStyle>
                    <AlternatingItemStyle CssClass="dgAlternate" Font-Bold="False" 
                        Font-Italic="False" Font-Overline="False" Font-Strikeout="False" 
                        Font-Underline="False" HorizontalAlign="Left"></AlternatingItemStyle>
                    <ItemStyle CssClass="dgItem" Font-Bold="False" Font-Italic="False" 
                        Font-Overline="False" Font-Strikeout="False" Font-Underline="False" 
                        HorizontalAlign="Left"></ItemStyle>
                    <HeaderStyle CssClass="dgHeader" Font-Bold="False" Font-Italic="False" 
                        Font-Overline="False" Font-Strikeout="False" Font-Underline="False" 
                        HorizontalAlign="Center"></HeaderStyle>
                    <FooterStyle HorizontalAlign="Right"></FooterStyle>
                    <Columns>
                        <asp:TemplateColumn>
                            <ItemTemplate>
                                <a href="TicketDetail.aspx?id=<%#Eval("Id") %>"><%#Eval("Id").ToString().PadLeft(10, "0")%></a>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center"  Width="60px"/>
                        </asp:TemplateColumn>           
                        <asp:BoundColumn DataFormatString="{0:dd/MMM/yyyy HH:mm}" DataField="StartDate">                            
                            <ItemStyle HorizontalAlign="center" />
                        </asp:BoundColumn> 
                        <asp:TemplateColumn>
                            <ItemTemplate>
                                <a href="TicketDetail.aspx?id=<%#Eval("Id") %>"><%#Eval("Subject")%></a>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateColumn> 
                        <asp:TemplateColumn>
                            <ItemTemplate>
                                <%#If(Convert.ChangeType(Eval("Status"), GetType(TicketData.TicketStatus)) = TicketData.TicketStatus.InProcess, Me.GetLabel("01294"), Me.GetLabel("01295"))%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center"  Width="80px"/>
                        </asp:TemplateColumn> 
                    </Columns>
                    <PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
                        Position="Top" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
                    </asp:DataGrid>
                    <% System.Threading.Thread.CurrentThread.CurrentCulture = sysCulture%>
                <br />
                <br />
            </td> 
        </tr> 
    </table>
    </form>
</body>
</html>
