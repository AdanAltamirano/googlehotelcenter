<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RatesPlansDineroMail.aspx.vb"
    Inherits="RateManager.RatesPlansDineroMail" %>

<%@ Register Src="../Modulos/CtrRatePlanDineroMail.ascx" TagName="CtrRatePlanDineroMail"
    TagPrefix="uc1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Rates plans DineroMail</title>
    <link href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet" />

    <script type="text/javascript">

        function FireShow(ID, IDcmd, show) {
            var e = document.getElementById(ID);
            var c = document.getElementById(IDcmd);
            if (e) {
                e.style.display = show ? 'block' : 'none';
            }
            if (c) {
                c.style.display = !show ? 'block' : 'none';
            }
            onResizeIframe();
        }
    </script>

</head>
<body ms_positioning="FlowLayout" bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0">
    <form id="Form1" method="post" runat="server">
    <div class="clear">
        <input type="button" id="cmdNew" class="ButtonNew" runat="server" causesvalidation="false"
            value="New" visible =false  />
        <div class="mDiv">
        </div>
        <div class="title">
            <asp:Label ID="lblMsg" runat="server" EnableViewState="False" Text="Rates Plans DineroMail"
                CssClass="tituloSeccion"></asp:Label>
        </div>
    </div>
    <asp:Panel ID="pnlData" runat="server" Style="display: none;">
        <table id="bookingcontainer" border="0" cellspacing="0" cellpadding="2" width="740">
            <tr>
                <td>
                    <table id="Table2" border="0" cellspacing="0" cellpadding="0" width="100%" align="center">
                        <tr>
                            <td style="height: 16px" class="Titulo" align="center">
                                <asp:Label ID="lblTitle" runat="server" EnableViewState="False">Rates Plans DineroMail</asp:Label>
                            </td>
                        </tr>                      
                        <tr>
                            <td>
                                <asp:Label ID="lblContPortal" EnableViewState="False" CssClass="bookingnormallabel"
                                    runat="server" Visible="false">DineroMail</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblRateplan" EnableViewState="False" CssClass="clsLabel" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <uc1:CtrRatePlanDineroMail ID="CtrRatePlanDineroMail1" runat="server" />
                                <br>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="lblError" runat="server" CssClass="Validators" Visible="False">Error</asp:Label><asp:Label
                                    ID="lblErrorSource" runat="server" CssClass="Validators" Visible="False">*</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 20px" align="center">
                                <asp:Button ID="cmdEliminar" runat="server" EnableViewState="False" CssClass="button"
                                    Text="Eliminar" CausesValidation="false" Enabled="false"></asp:Button>
                                <asp:Button ID="btnSave" runat="server" EnableViewState="False" CssClass="button"
                                    Text="Guardar"></asp:Button>
                                <asp:Button ID="btncancel" runat="server" EnableViewState="False" CssClass="button"
                                    Text="Cancelar" CausesValidation="False"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <div class="clear">
        <asp:DataGrid ID="dgRatePlans" runat="server" Width="99%" AllowPaging="True" PageSize="20" 
            AutoGenerateColumns="False" CssClass="DataGrid" ShowFooter="True">
            <SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
            <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
            <ItemStyle CssClass="dgitem"></ItemStyle>
            <HeaderStyle CssClass="dgHeader"></HeaderStyle>
            <FooterStyle HorizontalAlign="Right"></FooterStyle>
            <Columns>
                <asp:BoundColumn Visible="False" DataField="idrateplan"></asp:BoundColumn>
                <asp:BoundColumn DataField="codigotarifa" HeaderText="C&#243;digo">
                    <HeaderStyle Width="9%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundColumn>
                <asp:BoundColumn DataField="Name" HeaderText="Nombre">
                    <HeaderStyle></HeaderStyle>
                </asp:BoundColumn>
                <asp:TemplateColumn>
                    <HeaderStyle ></HeaderStyle>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkedit" runat="server" CssClass="dgLink" CausesValidation="False"
                            CommandName="Select">editar</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn Visible="False" DataField="codigotarifa"></asp:BoundColumn>
            </Columns>
            <PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
                Position="Bottom" CssClass="dgPager" Mode="NumericPages"></PagerStyle>                
        </asp:DataGrid>
    </div>
    </form>
</body>
</html>
