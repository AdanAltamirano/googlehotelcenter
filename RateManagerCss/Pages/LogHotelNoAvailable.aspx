<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LogHotelNoAvailable.aspx.vb"
    Inherits="RateManager.LogHotelNoAvailable" %>

<%@ Import Namespace="RateManager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>Hotel Requests Log</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet">

    <script type="text/javascript" src="../Includes/Script/jquery-1.4.2.min.js"></script>

    <script type="text/javascript">
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
        /*function IniDate()
        {		 		                     
        var fecha = new Date();
        var fecha2 = new Date(2030, 12, 31);
        var arr = new Array(3);
        arr[0] = [fecha.getFullYear(), fecha.getMonth()+1, fecha.getDate()]
        arr[1] = [fecha2.getFullYear(), fecha2.getMonth()+1, fecha2.getDate()];
        return arr;		                
        }*/
        function FireUpdateStatus() {
            $('#loadingProcess').show();
            return true;
        }
			 
    </script>

</head>
<body bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0" ms_positioning="FlowLayout">
    <iframe id="gToday:normal:agenda.js" style="z-index: 999; left: -500px; position: absolute;
        top: -500px" name="gToday:normal:agenda.js" src='<%=GeRequestApplicationPath(string.concat("/Calendar/",portalculture.getculture().Name.Substring(0, 2).ToLower(),"/ipopeng2.htm" )) %>'
        frameborder="0" width="174" scrolling="no" height="189"></iframe>
    <form id="Form1" method="post" runat="server">
    <div class="mDiv">
    </div>
    <div class="title">
        <asp:Label ID="lblTitle" runat="server" EnableViewState="False" Text="Reporte De No Disponible"
            CssClass="tituloSeccion"></asp:Label>
    </div>
    </div>
    <div id="loadingProcess">
    </div>
    <table id="bookingcontainer" cellspacing="0" cellpadding="2" width="100%" border="0">
        <tr>
            <td>
                <asp:Label ID="lblInicio" runat="server" EnableViewState="False">Desde</asp:Label>
            </td>
            <td>
                <a hidefocus onclick="if(self.gfPop)gfPop.fPopCalendar(document.getElementById('txtFinal'),document.getElementById('txtInicio'));return false;"
                    href="javascript:void(0)">
                    <asp:TextBox ID="txtInicio" runat="server" Columns="10" Width="84px" MaxLength="10"
                        CssClass="textbox"></asp:TextBox><img class="PopcalTrigger" alt="" src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>'
                            align="absMiddle" border="0">
                </a>
            </td>
            <td>
                <asp:Label ID="lblFinal" runat="server" EnableViewState="False">Hasta</asp:Label>
            </td>
            <td>
                <a hidefocus onclick="if(self.gfPop)gfPop.fPopCalendar1(document.getElementById('txtFinal'));return false;"
                    href="javascript:void(0)">
                    <asp:TextBox ID="txtFinal" runat="server" Columns="10" Width="84px" MaxLength="10"
                        CssClass="textbox"></asp:TextBox><img class="PopcalTrigger" alt="" src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>'
                            align="absMiddle" border="0">
                </a>
            </td>
            <td>
                <asp:Label ID="lblHotel" runat="server" EnableViewState="False">Hotel</asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtNombreHotel" runat="server"></asp:TextBox>
            </td>
            <td style=" width:20%;">
                <asp:Button ID="btnSearch" runat="server" EnableViewState="False" CssClass="button"
                    Text="Buscar"></asp:Button>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="7">
                <asp:DataGrid ID="dgSolicitudes" runat="server" AutoGenerateColumns="False" CssClass="DataGrid">
                    <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
                    <ItemStyle CssClass="dgItem"></ItemStyle>
                    <HeaderStyle CssClass="dgHeader"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="Nombre" HeaderText="Nombre Hotel"></asp:BoundColumn>
                        <asp:BoundColumn DataField="cnts" ItemStyle-HorizontalAlign="Center" HeaderText="Solicitudes No Disponibles">
                        </asp:BoundColumn>
                        <asp:ButtonColumn Text="Ver Detalle" CommandName="Detalle"></asp:ButtonColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td colspan="7">
                <br>
            </td>
        </tr>
        <tr>
            <td class="dgItem" id="TDDetalle" align="center" colspan="7" runat="server">
                <asp:Label ID="lblTitleDetalle" Font-Bold="True" runat="server" EnableViewState="False">Hotel La Concha - Detalle De Solicitudes No Disponible</asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="7">
                <asp:DataGrid ID="dgDetalle" runat="server" Visible="False" AutoGenerateColumns="False"
                    CssClass="DataGrid">
                    <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
                    <ItemStyle CssClass="dgItem"></ItemStyle>
                    <HeaderStyle CssClass="dgHeader"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:MMM/dd/yyyy}">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Checkin" HeaderText="Checkin" DataFormatString="{0:MMM/dd/yyyy}">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="CheckOut" HeaderText="CheckOut" DataFormatString="{0:MMM/dd/yyyy}">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Source" HeaderText="Origen"></asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
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
