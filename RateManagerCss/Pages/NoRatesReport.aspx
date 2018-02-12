<%@ Import Namespace="RateManager" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="NoRatesReport.aspx.vb"
    Inherits="RateManager.NoRatesReport" %>

<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../Portal/Modules/ctrlHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../Portal/Modules/ctrlFooter.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrMenu" Src="../Portal/Modules/CtrMenu.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>NoRatesReport</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet"></link>
    <script type="text/javascript" src="../Includes/Script/jquery-1.4.2.min.js"></script>

    <script>
        function showRatePlan(ddl, array, lbl) {
            var e = document.getElementById(ddl);
            var lbl = document.getElementById(lbl);
            if (e.selectedIndex != 0) {
                lbl.firstChild.nodeValue = array.split("//")[e.selectedIndex];
            }
            else {
                lbl.firstChild.nodeValue = '-';
            }

        }

        function showRoomType(ddl, array) {
            var e = document.getElementById(ddl);
            var lbl = document.getElementById('lblDescRoom');
            if (e.selectedIndex != 0) {
                lbl.firstChild.nodeValue = array.split("//")[e.selectedIndex];
            }
            else {
                lbl.firstChild.nodeValue = '-';
            }

        }

        function FireUpdateStatus() {
            $('#loadingProcess').show();
            return true;
        }
			
    </script>

</head>
<body ms_positioning="FlowLayout" bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0">
    <form id="Form1" method="post" runat="server">
    <iframe id="gToday:normal:agenda.js" style="z-index: 999; left: -500px; position: absolute;
        top: -500px" name="gToday:normal:agenda.js" src='<%=GeRequestApplicationPath(string.concat("/Calendar/",portalculture.getculture().Name.Substring(0, 2).ToLower(),"/ipopeng2.htm" )) %>'
        frameborder="0" width="174" scrolling="no" height="189"></iframe>
    <div class="clear">
        <div class="mDiv">
        </div>
        <div class="title">
            <asp:Label ID="lblTitle" runat="server" EnableViewState="False" Text="Tarifas no definidas"
                CssClass="tituloSeccion"></asp:Label>
        </div>
        <div id="loadingProcess">
        </div>
    </div>
    <table id="bookingcontainer" cellspacing="0" cellpadding="2" width="650" border="0">
        <tr>
            <td>
                <table id="Table1" cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblstart" runat="server" CssClass="clslabel" EnableViewState="False">desde:</asp:Label>
                        </td>
                        <td align="left" width="140">
                            <a hidefocus onclick="if(self.gfPop)gfPop.fPopCalendar(document.getElementById('<%response.write(txtDateTo.ClientId)%>'),document.getElementById('<%response.write(txtDateFrom.ClientId)%>'));return false;"
                                href="javascript:void(0)">
                                <asp:TextBox ID="txtDateFrom" runat="server" CssClass="textbox" MaxLength="10" Width="84px"
                                    Columns="10"></asp:TextBox><img class="PopcalTrigger" alt="" src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>'
                                        align="absMiddle" border="0"></a>
                        </td>
                        <td align="right">
                            <asp:Label ID="lblroom" runat="server" CssClass="clslabel" EnableViewState="False">Room:</asp:Label>
                        </td>
                        <td align="left" width="120">
                            <asp:DropDownList ID="ddlRooms" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td align="left" rowspan="2">
                            <asp:Button ID="btnload" runat="server" CssClass="button" Text="load"></asp:Button>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblEnd" runat="server" CssClass="clsLabel" EnableViewState="False">Hasta:</asp:Label>
                        </td>
                        <td align="left" width="140">
                            <a hidefocus onclick="if(self.gfPop)gfPop.fPopCalendar1(document.getElementById('<%response.write(txtDateTo.ClientId)%>'));return false;"
                                href="javascript:void(0)">
                                <asp:TextBox ID="txtDateTo" runat="server" CssClass="textbox" MaxLength="10" Width="84px"
                                    Columns="10"></asp:TextBox><img class="PopcalTrigger" alt="" src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>'
                                        align="absMiddle" border="0"></a>
                        </td>
                        <td align="right">
                            <asp:Label ID="lblrateplan" runat="server" CssClass="clslabel" EnableViewState="False">Rateplan:</asp:Label>
                        </td>
                        <td align="left" width="120">
                            <asp:DropDownList ID="ddlrateplans" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" align="center" style="padding-top: 8px; padding-bottom: 8px;">
                            <asp:Label ID="lblHelpFiltro" runat="server" CssClass="clsHelpLabel" EnableViewState="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" align="center">
                            <asp:DataGrid ID="dgnorates" EnableViewState="False" runat="server" Width="90%" AutoGenerateColumns="False"
                                CssClass="datagrid">
                                <SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
                                <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
                                <ItemStyle CssClass="dgItem"></ItemStyle>
                                <HeaderStyle CssClass="dgHeader"></HeaderStyle>
                                <Columns>
                                    <asp:BoundColumn DataField="codigohabitacion" HeaderText="Habitación"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="idrateplan" HeaderText="Plan tarifario"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="fechainicia" HeaderText="Desde"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="fechafinaliza" HeaderText="Hasta"></asp:BoundColumn>
                                </Columns>
                            </asp:DataGrid>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>

    <script type="text/javascript">
        $().ready(function() {
            $('#loadingProcess').hide();
        });  
    </script>

</body>
</html>
