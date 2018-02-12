<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MonedasHotel.aspx.vb"
    Inherits="RateManager.MonedasHotel" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<link href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet" />
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Money Exchange by Hotel</title>
</head>

<script type="text/javascript">

    function FireClearMsg() {
        var lblError = document.getElementById("<% =lblMessage.ClientID %>");
        if (lblError) {
            lblError.style["visibility"] = "hidden";
        }
    }
    
</script>

<body>
    <form id="form1" runat="server">
    <div>
     <div class="clear">		    
		        <div class="mDiv"></div>
		        <div>
		            <asp:Label ID="lblTitulo" runat="server" class="tituloSeccion" style= "margin-left:32px;">Tipo de Cambio</asp:Label>         
		        </div>
		    </div>
        <table id="BookingContainer" cellspacing="0" cellpadding="2" width="500" align="center"
            border="0">         
            <tr>
                <td align="center">
                    <!--Dado que no hay una interfase para los tipos de cambio en monedas, las monedas estaran fijas. -->
                    <asp:DataGrid ID="dgMonedas" runat="server" AutoGenerateColumns="False" CssClass="datagrid">
                        <HeaderStyle CssClass="dgHeader"></HeaderStyle>
                        <ItemStyle CssClass="dgItem"></ItemStyle>
                        <Columns>
                            <asp:BoundColumn DataField="IdMoneda" Visible="False"></asp:BoundColumn>
                            <asp:BoundColumn DataField="IdHotel" Visible="False"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Codigo" ReadOnly="True" HeaderText="Codigo" ItemStyle-HorizontalAlign="Left">
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Nombre" ReadOnly="True" HeaderText="Nombre" ItemStyle-HorizontalAlign="Left">
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="TipoCambio" ReadOnly="True" Visible="false"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="= 1 USD">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtTipoCambio" runat="server" Width="69px" CssClass="textbox" Style="margin: 2px;"
                                        MaxLength="8" Text='<%# databinder.eval(container.dataitem, "tipoCambio")%>'>
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfv" runat="server" Display="Dynamic" ControlToValidate="txtTipoCambio"
                                        Visible="false">*</asp:RequiredFieldValidator>
                                    <asp:RangeValidator ID="rgv" runat="server" Display="Dynamic" MaximumValue="9999.999"
                                        MinimumValue="0.0001" ControlToValidate="txtTipoCambio" Style="display: none;">
											<%#RateManager.PortalCulture.GetString("M000105", False)%>
                                    </asp:RangeValidator>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                           
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr style="height: 18px;">
                <td align="center">
                    <asp:Label ID="lblMessage" CssClass="Validators" runat="server" Style="display: none;"></asp:Label>
                </td>
            </tr>            
            <tr>
                <td align="center">
                    <asp:Button ID="btnSave" runat="server" EnableViewState="False" CssClass="Button"
                        Text="Guardar" Width="85px"></asp:Button>
                </td>
            </tr>

        </table>
    </div>
    </form>
</body>
</html>
