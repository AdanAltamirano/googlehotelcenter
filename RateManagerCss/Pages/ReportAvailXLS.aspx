<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ReportAvailXLS.aspx.vb"
    Inherits="RateManager.ReportAvailXLS" %>

<%@ Import Namespace="RateManager" %>
<html>
<head>
    <title>ConfigReportAvail</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
    </link>
    <script type="text/javascript" src="../Includes/Script/jquery-1.4.2.min.js"></script>

    
    
    <script type="text/javascript" >
        function FireUpdateStatus(msg) {
            $('#loadingProcess').show();
            return true;
        }
    
    </script>
</head>
<body bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0">
    <iframe id="gToday:normal:agenda.js" style="z-index: 999; left: -500px; position: absolute;
        top: -500px" name="gToday:normal:agenda.js" src='<%=GeRequestApplicationPath(string.concat("/Calendar/",portalculture.getculture().Name.Substring(0, 2).ToLower(),"/ipopeng2.htm" )) %>'
        frameborder="0" width="174" scrolling="no" height="189"></iframe>
    <form id="Form1" method="post" runat="server">
    <div class="clear">
        <div class="mDiv">
        </div>
        <div class="title">
            <asp:Label ID="lblTitulo" runat="server" EnableViewState="False" Text="Reportes de Disponibilidad"
                CssClass="tituloSeccion"></asp:Label>
        </div>
    </div>
     <div id="loadingProcess" >
     </div>
    <table id="bookingcontainer" cellspacing="0" cellpadding="2" width="100%" border="0">
        <tr>
            <td>
                <asp:Label ID="lblDesde" runat="server">Desde</asp:Label>
            </td>
            <td>
                <a hidefocus onclick="if(self.gfPop)gfPop.fPopCalendar1(document.getElementById('txtInicio'));return false;"
                    href="javascript:void(0)">
                    <asp:TextBox ID="txtInicio" runat="server" Width="84px" CssClass="textbox" MaxLength="10"
                        Columns="10"></asp:TextBox><img class="PopcalTrigger" alt="" src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>'
                            align="absMiddle" border="0">
                </a>
            </td>
            <td>
                <asp:Label ID="lblDisplay" runat="server">Mostrar en Reporte</asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlDisplayTime" runat="server">
                    <asp:ListItem>1</asp:ListItem>
                    <asp:ListItem>2</asp:ListItem>
                    <asp:ListItem>3</asp:ListItem>
                    <asp:ListItem>4</asp:ListItem>
                    <asp:ListItem>5</asp:ListItem>
                    <asp:ListItem>6</asp:ListItem>
                    <asp:ListItem>7</asp:ListItem>
                    <asp:ListItem>8</asp:ListItem>
                    <asp:ListItem>9</asp:ListItem>
                    <asp:ListItem>10</asp:ListItem>
                    <asp:ListItem>11</asp:ListItem>
                    <asp:ListItem>12</asp:ListItem>
                    <asp:ListItem>13</asp:ListItem>
                    <asp:ListItem>14</asp:ListItem>
                    <asp:ListItem Selected>15</asp:ListItem>
                    <asp:ListItem>16</asp:ListItem>
                    <asp:ListItem>17</asp:ListItem>
                    <asp:ListItem>18</asp:ListItem>
                    <asp:ListItem>19</asp:ListItem>
                    <asp:ListItem>20</asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="ddlDisplayType" runat="server">
                    <asp:ListItem Value="D">D</asp:ListItem>
                    <asp:ListItem Value="W">W</asp:ListItem>
                    <asp:ListItem Value="M">M</asp:ListItem>
                    <asp:ListItem Value="Y">Y</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style=" width:20%; ">
                <asp:Button ID="btnEjecutar" runat="server" Text="Ejecutar" CssClass="button"></asp:Button>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
            </td>
            <td>
            </td>
            <td>
            </td>
            <td>
                <asp:HyperLink ID="lnkRepXLS" runat="server" Visible="False" Target="_blank">Ver Reporte Excel</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="4">
                &nbsp;
            </td>
            <td align="center">
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
