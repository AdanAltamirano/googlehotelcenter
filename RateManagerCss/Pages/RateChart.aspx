<%@ Import Namespace="RateManager" %>
<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../Portal/Modules/ctrlFooter.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../Portal/Modules/ctrlHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrMenu" Src="../Portal/Modules/CtrMenu.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RateChart.aspx.vb" Inherits="RateManager.RateChart" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>RateChart</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet">

    <script>
		function showRatePlan2(ddl,array,lbl)
		 { 
		  var e = document.getElementById(ddl);
		  var lbl = document.getElementById(lbl);		  		  	  
		  lbl.firstChild.nodeValue =array.split("//")[e.selectedIndex + 1];		  		  
		  }
    </script>

</head>
<body bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <iframe id="gToday:normal:agenda.js" style="z-index: 999; left: -500px; position: absolute;
        top: -500px" name="gToday:normal:agenda.js" src='<%=GeRequestApplicationPath(string.concat("/Calendar/",portalculture.getculture().Name.Substring(0, 2).ToLower(),"/ipopeng2.htm" )) %>'
        frameborder="0" width="174" scrolling="no" height="189"></iframe>
    <div class="clear">
        <div class="mDiv">
        </div>
        <div class="title">
            <asp:Label ID="lblTitle" runat="server" EnableViewState="False" Text="Rate Chart"
                CssClass="tituloSeccion"></asp:Label>
        </div>
    </div>
    <table id="bookingcontainer" cellspacing="0" cellpadding="2" width="700" border="0">
        <tr>
            <td>
                <table id="Table1" cellspacing="0" cellpadding="0" align="left" border = 0>
                    <tr>
                        <td align="left" width="50">
                            <asp:Label ID="lblstart" runat="server" EnableViewState="False" CssClass="clslabel">desde:</asp:Label>
                        </td>
                        <td align="left" width="140">
                            <a hidefocus onclick="if(self.gfPop)gfPop.fPopCalendar(document.getElementById('<%response.write(txtDateTo.ClientId)%>'),document.getElementById('<%response.write(txtDateFrom.ClientId)%>'));return false;"
                                href="javascript:void(0)">
                                <asp:TextBox ID="txtDateFrom" runat="server" CssClass="textbox" Columns="10" Width="84px"
                                    MaxLength="10"></asp:TextBox><img class="PopcalTrigger" alt="" src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>'
                                        align="absMiddle" border="0"></a>
                        </td>
                        <td align="right" width="120">
                            <asp:Label ID="lblrateplan" runat="server" EnableViewState="False" CssClass="clslabel">Rateplan:</asp:Label>
                        </td>
                        <td align="left" width="160">
                            <asp:DropDownList ID="ddlrateplans" runat="server" Width="106px">
                            </asp:DropDownList>                           
                        </td>
                        <td align="left" width="50" >
                            
                        </td>
                        <td align="left" rowspan="3">
                            <asp:Button ID="btnload" runat="server" CssClass="button" Text="load"></asp:Button>
                        </td>
                    </tr>
                    <tr><td colspan=3></td> <td colspan =2 style="padding-left:4px;"><asp:Label ID="lblDescRatePlan" runat="server" CssClass="clslabel">-</asp:Label></td></tr>                    
                    <tr>
                        <td align="left">
                            <asp:Label ID="lblEnd" runat="server" EnableViewState="False" CssClass="clsLabel">Hasta:</asp:Label>
                        </td>
                        <td align="left">
                            <a hidefocus onclick="if(self.gfPop)gfPop.fPopCalendar1(document.getElementById('<%response.write(txtDateTo.ClientId)%>'));return false;"
                                href="javascript:void(0)">
                                <asp:TextBox ID="txtDateTo" runat="server" CssClass="textbox" Columns="10" Width="84px"
                                    MaxLength="10"></asp:TextBox><img class="PopcalTrigger" alt="" src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>'
                                        align="absMiddle" border="0"></a>
                        </td>
                        <td align="right">
                            <asp:RadioButton ID="RdVertical" runat="server" Text="Vertical" Checked="True" GroupName="posicion"
                                CssClass="clslabel"></asp:RadioButton>
                        </td>
                        <td align="left">
                            <asp:RadioButton ID="RdHorizontal" runat="server" Text="Horizontal" GroupName="posicion"
                                CssClass="clslabel"></asp:RadioButton>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td align="center" colspan="6">
                            <table id="Table2" cellspacing="1" cellpadding="1" border="0" width=95% >
                                <tr>
                                    <td width="10" bgcolor="green">
                                    </td>
                                    <td>
                                        <asp:Label ID="lblAbierto" runat="server">Abierto</asp:Label>
                                    </td>
                                    <td width="10" bgcolor="red">
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCerrado" runat="server">Cerrado</asp:Label>
                                    </td>
                                    <td width="10" bgcolor="#b0c4de">
                                    </td>
                                    <td>
                                        <asp:Label ID="lblnoarrivos" runat="server">No Arrivos</asp:Label>
                                    </td>
                                    <td width="10" bgcolor="lightsalmon">
                                    </td>
                                    <td>
                                        <asp:Label ID="lblNodisponible" runat="server">No Disponible</asp:Label>
                                    </td>
                                    <td width="10" bgcolor="#b8860b">
                                    </td>
                                    <td>
                                        <asp:Label ID="lblNoRates" runat="server">No Tarifas</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblHelp" runat="server" EnableViewState="False"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="left">
                <table cellspacing="0" cellpadding="5" width="100%" border="0">
                    <tr>
                        <td align="left" colspan="5">
                        <div style=" overflow:auto; width:900px; padding-bottom:12px; " >
                            <asp:DataGrid ID="dgRatesVertical" GridLines="None" CssClass="DataGrid" runat="server"
                                EnableViewState="False" >
                                <SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
                                <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
                                <ItemStyle CssClass="dgitem"></ItemStyle>
                                <HeaderStyle CssClass="dgHeader"></HeaderStyle>
                            </asp:DataGrid>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="5">
                            <asp:DataGrid ID="dgRatesHorizontal" runat="server" CssClass="DataGrid" GridLines="None">
                                <SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
                                <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
                                <ItemStyle CssClass="dgitem"></ItemStyle>
                                <HeaderStyle CssClass="dgHeader"></HeaderStyle>
                            </asp:DataGrid>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
    <%
  response.write("<script>showRatePlan2('" & Me.ddlrateplans.ClientID & "','" & Me.SourceRateName & "','" & me.lblDescRatePlan.ClientID & "')</script>")  
    %>
</body>
</html>
