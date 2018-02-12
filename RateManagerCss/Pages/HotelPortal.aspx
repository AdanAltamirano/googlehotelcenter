<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="HotelPortal.aspx.vb" Inherits="RateManager.HotelPortal" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
     <link href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet" />
     <script type= "text/javascript" >
         var resource = '<%= Ratemanager.PortalCulture.GetString("01419") %>'

         function FireIdioma(dg) {
             var dg = document.getElementById(dg);
             var list = dg.getElementsByTagName("input");
             var listSel = dg.getElementsByTagName("select");
             var ck;
             var value = true;

             for (var i = 0; i <= list.length - 1; i++) {
                 if (list[i].type == 'checkbox') {
                     ck = list[i].checked;
                 }
                 if (listSel[i]) {
                     if (ck && listSel[i].selectedIndex == 0) {
                         value = false;
                         alert(resource);
                         break; 
                     }
                 }
             }
             return value;
         }        
         
     </script>
</head>
<body>
     <form id="Form1" method="post" runat="server" submitdisabledcontrols="True">
  <div class="clear">		    
		        <div class="mDiv"></div>
		        <div>
		            <asp:Label ID="lblTitle" runat="server" class="tituloSeccion" style= "margin-left:32px;">Configuracion Portal Hotel Contenido Web</asp:Label>         
		        </div>
		    </div>     
    <table id="bookingcontainer" cellspacing="0" cellpadding="2" width="600" border="0"
        align="center" style="border: solid 1px #003399">
        <tr>
            <td>
                <table id="Table2" cellspacing="0" cellpadding="0" width="90%" align="center" border="0">
                    <tr>
                        <td style="height: 167px" align="center" colspan="2">
                            <div style="overflow-x: hidden; overflow-y: auto; width: 99%; padding-left: 2px;"
                                align="left">
                                <asp:DataGrid ID="dgPortals" runat="server" Width="100%" AllowPaging="true" PageSize="20"
                                    AutoGenerateColumns="False" CssClass=DataGrid ShowFooter="True" >
                                    <FooterStyle HorizontalAlign="Right"></FooterStyle>
                                    <SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
                                    <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
                                    <ItemStyle CssClass="dgItem"></ItemStyle>
                                    <HeaderStyle CssClass="dgHeader"></HeaderStyle>
                                    <Columns>
                                     <asp:TemplateColumn HeaderText="" >
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkPortal" runat="server"   />
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:BoundColumn Visible="False" DataField="idaplicacionportal"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="IdPortal"></asp:BoundColumn>
                                        <asp:BoundColumn Visible="False" DataField="idIdioma"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="Nombre" HeaderText="Nombre">
                                            <HeaderStyle Width="60%" HorizontalAlign="Left"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundColumn>
                                        <asp:TemplateColumn HeaderText="Idioma Portal" >
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlIdioma" runat="server" CssClass="textbox"  >
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                       
                                    </Columns>
                                    <PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
                                        Position="Top" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
                                </asp:DataGrid>                             
                            </div>
                        </td>
                    </tr>
                    <tr style="height: 4px;">
                        <td colspan="2">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <asp:Label ID="lblError" runat="server" CssClass="Validators" Visible="False">Error</asp:Label>
                        </td>
                    </tr>                    
                    <tr>
                        <td align="center" colspan="2">
                            <asp:Button ID="btnSave" runat="server" CssClass="Button" Text="Guardar"></asp:Button>&nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
