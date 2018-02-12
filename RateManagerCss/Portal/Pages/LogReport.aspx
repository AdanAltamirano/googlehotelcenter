<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LogReport.aspx.vb" Inherits="RateManager.LogReport" %>

<%@ Import Namespace="RateManager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>LogReport</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
    <script type="text/javascript" src="../../Includes/Script/jquery-1.4.2.min.js"></script>
    <script>
        function showHotel(idgrid, idtd) {
            var dg = document.getElementById(idgrid);
            if (dg) {
                if (dg.style.display == 'none') {
                    dg.style.display = '';
                    document.getElementById(idtd).innerHTML = "-";
                }
                else {
                    dg.style.display = 'none';
                    document.getElementById(idtd).innerHTML = "+";
                }
            }
        }

        function FireUpdateStatus() {
            $('#loadingProcess').show();
            return true;
        }

        /*function IniDate()
        {		 		                     
        var fecha = new Date();
        var fecha2 = new Date(2030, 12, 31);
        var arr = new Array(3);
        arr[0] = [fecha.getFullYear(), fecha.getMonth()+1, fecha.getDate()]
        arr[1] = [fecha2.getFullYear(), fecha2.getMonth()+1, fecha2.getDate()];
        return arr;
		                
        }*/
    </script>

</head>
<body bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0" ms_positioning="FlowLayout">
    <iframe id="gToday:normal:agenda.js" style="z-index: 999; left: -500px; position: absolute;
        top: -500px" name="gToday:normal:agenda.js" src='<%=GeRequestApplicationPath(string.concat("/Calendar/",portalculture.getculture().Name.Substring(0, 2).ToLower(),"/ipopeng2.htm" )) %>'
        frameborder="0" width="174" scrolling="no" height="189"></iframe>
    <form id="Form1" method="post" runat="server">
    <div class="clear">
        <div class="title">
            <asp:Label ID="lbltitle" runat="server" EnableViewState="False" Text="Log" CssClass="tituloSeccion"></asp:Label>
        </div>
        <div id="loadingProcess"></div>
    </div>
    <div class =clear >
        <table id="bookingcontainer" cellspacing="0" cellpadding="2" width="700" border="0">
        <tr>
            <td>
                <asp:Label ID="lblInicio" runat="server" EnableViewState="False">Desde</asp:Label>
            </td>
            <td>
                <a hidefocus onclick="if(self.gfPop)gfPop.fPopCalendar(document.getElementById('txtFinal'),document.getElementById('txtInicio'));return false;"
                    href="javascript:void(0)">
                    <asp:TextBox ID="txtInicio" runat="server" CssClass="textbox" MaxLength="10" Width="84px"
                        Columns="10"></asp:TextBox><img class="PopcalTrigger" alt="" src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>'
                            align="absMiddle" border="0">
                </a>
            </td>
            <td>
                <asp:Label ID="lblFinal" runat="server" EnableViewState="False">Hasta</asp:Label>
            </td>
            <td>
                <a hidefocus onclick="if(self.gfPop)gfPop.fPopCalendar1(document.getElementById('txtFinal'));return false;"
                    href="javascript:void(0)">
                    <asp:TextBox ID="txtFinal" runat="server" CssClass="textbox" MaxLength="10" Width="84px"
                        Columns="10"></asp:TextBox><img class="PopcalTrigger" alt="" src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>'
                            align="absMiddle" border="0">
                </a>
            </td>
            <td>
                <asp:Label ID="lblHotel" runat="server" EnableViewState="False">Hotel :</asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlHoteles" runat="server">
                </asp:DropDownList>
            </td>
            <td style=" width:15% ">
                <asp:Button ID="btnSearch" runat="server" EnableViewState="False" CssClass="button"
                    Text="Buscar"></asp:Button>
            </td>
        </tr>
        <tr>
            <td colspan="7" align="left" style="padding-top: 20px;">
                <asp:DataList ID="dlHoteles" runat="server" EnableViewState="true" Width="100%" HorizontalAlign="LEFT">
                    <ItemTemplate>
                        <table class="dgHeader" id="tshow" width="650px" border="0" runat="server">
                            <tr>
                                <td id="tdshow" style="cursor: pointer" align="center" width="10" runat="server">
                                    -
                                </td>
                                <td align="center" colspan="2" class="clsHelpLabel">
                                    <%# databinder.eval(container,"DataItem.Nombre") %>
                                </td>
                            </tr>
                        </table>
                        <asp:DataGrid ID="dgLog" runat="server" EnableViewState="false" Width="720px" AllowPaging="false"
                            AutoGenerateColumns="False" CssClass="DataGrid">
                            <AlternatingItemStyle CssClass="dgAlternate" Width="720"></AlternatingItemStyle>
                            <ItemStyle CssClass="dgItem" Width="720"></ItemStyle>
                            <HeaderStyle CssClass="dgHeader" Width="720"></HeaderStyle>
                            <Columns>
                                <asp:BoundColumn HeaderText="Usuario" DataField="Usuario"></asp:BoundColumn>
                                <asp:TemplateColumn>
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hplPagina" runat="server"></asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:BoundColumn HeaderText="Pagina" DataField="Pagina" Visible="False"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Accion" DataField="Accion"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Fecha" DataField="Fecha"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Hora" DataField="Fecha"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Nota" DataField="Nota"></asp:BoundColumn>
                                <asp:TemplateColumn HeaderText="Itinerario">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="lnkDetalle" runat="server"></asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:BoundColumn HeaderText="idlog" DataField="idlog" Visible="false"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="link" DataField="link" Visible="False"></asp:BoundColumn>
                            </Columns>
                        </asp:DataGrid>
                    </ItemTemplate>
                </asp:DataList>
            </td>
        </tr>
    </table>
    </div>
    </form>

    <script type="text/javascript">
        $().ready(function() {
            $('#loadingProcess').hide();
        });  
    </script>

</body>
</html>
