<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PaymentMethod.aspx.vb"
    Inherits="RateManager.PaymentMethod" %>

<%@ Register Src="~/HotelAdministrator/Invoicing/Modules/CtrPaymentMethod.ascx" TagName="ctrlPaymentMethod"
    TagPrefix="uc1" %>
<%@ Register TagPrefix="uc1" TagName="ctlMensajes" Src="~/Modulos/ctlMensajes.ascx" %>
<%@ Register Src="~/Modulos/ctrlAutoComplete.ascx" TagName="ctrlAutoComplete" TagPrefix="uc2" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>Payment Methods</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link rel="stylesheet" type="text/css" href="../../../StyleSheets/Styles.css">

    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.5/jquery.min.js"></script>

    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js"></script>

    <script type="text/javascript" src="../Includes/Script/JsSearch-1.0.js"></script>
    
    <script language="javascript" type="text/javascript">

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

        function Cancel() {
            CtrPaymentMethod_Clear();
            FireShow('divCtrlContent', 'cmdNew', false)
        }

        function validarkeyCode(e) { // 1
            tecla = (document.all) ? e.keyCode : e.which; // 2
            if (tecla == 8) return true; // 3
            patron = /[A-Za-z0-9\s]/; // 4
            te = String.fromCharCode(tecla); // 5
            return patron.test(te); // 6
        } 
</script>

</head>



<body bottommargin="0" leftmargin="0" rightmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <div class="clear">
        <input type="button" id="cmdNew" class="ButtonNew" runat="server" causesvalidation="false"
            value="New" style="width: 85px;" onclick="FireShow('divCtrlContent', 'cmdNew', true);" />
        <div class="mDiv">
        </div>
        <div class="title">
            <asp:Label ID="lblTitle" runat="server" EnableViewState="False" class="tituloSeccion">Metodos de pago</asp:Label>
        </div>
    </div>
    <div runat="server" id="divContenedor">
        <table id="bookingcontainer clear" border="0" cellspacing="0" cellpadding="2" width="100%">
            <tr>
                <td>
                    <div id="divCtrlContent" style="<%  If Not CtrlPaymentMethod1.IsEdit Then %>  display: none; <% End if %>">
                        <table id="Table2" border="0" cellspacing="0" cellpadding="0" width="100%" align="center">
                         <tr class="trTitle rounded-corners" id="dvContent2">
                              <td class="dgitem" align="left">
                                <asp:Label ID="lblMsg" runat="server" DESIGNTIMEDRAGDROP="987"></asp:Label>
                              </td>
                         </tr>
                            <tr class="trContent rounded-corners" id="dvContent">
                                <td class="tdContent" colspan="2">
                                    
                                        <br />
                                        
                                        <uc1:ctrlPaymentMethod ID="CtrlPaymentMethod1" runat="server"></uc1:ctrlPaymentMethod>
                                        <br />
                                        <asp:Button ID="btnSave" runat="server" EnableViewState="False" CssClass="Button"
                                            Text="Guardar" OnClientClick="return CtrPaymentMethod_Val()"  CausesValidation="true">
                                        </asp:Button>
                                        <asp:Button ID="btncancel" runat="server" EnableViewState="False" CssClass="Button" 
                                            Text="Cancelar" OnClientClick="Cancel();return false;" CausesValidation="false"  >
                                        </asp:Button>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <br />
                    <asp:Label ID="lblError" runat="server" CssClass="Validators" Visible="False">Error</asp:Label><asp:Label
                        ID="lblErrorSource" runat="server" CssClass="Validators" Visible="False">*</asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <div class="clear">
        <asp:DataGrid ID="grid" runat="server" Width="99%" AllowPaging="True" PageSize="20"
            GridLines="None" DataKeyField="CompanyPaymentMethodID" AutoGenerateColumns="False"
            CssClass="datagrid" ShowFooter="True">
            <SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
            <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
            <ItemStyle CssClass="dgItem"></ItemStyle>
            <HeaderStyle CssClass="dgHeader"></HeaderStyle>
            <FooterStyle HorizontalAlign="Right"></FooterStyle>
            <Columns>
                <asp:BoundColumn Visible="False" DataField="CompanyPaymentMethodID"></asp:BoundColumn>
                <asp:BoundColumn DataField="Method" HeaderText="Method">
                    <ItemStyle ></ItemStyle>
                </asp:BoundColumn>
                <asp:TemplateColumn HeaderText="Default">
                    <HeaderStyle ></HeaderStyle>
                    <ItemTemplate >
                        <asp:CheckBox  ID="chkStatus" runat="server" onclick="return false;" Checked='<%# Convert.ToBoolean(Eval("Default")) %>' />
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn HeaderText="AccountNumber" DataField="AccountNumber"></asp:BoundColumn>
                <asp:TemplateColumn HeaderText="Editar">
                    <HeaderStyle ></HeaderStyle>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkEdit" runat="server" CssClass="dgLink" CausesValidation="False"
                            CommandName="Edit">Editar</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Eliminar">
                    <HeaderStyle ></HeaderStyle>
                    <ItemStyle></ItemStyle>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkDelete2" Style="display: none"  runat="server" CssClass="dgLink" CausesValidation="False"
                            CommandName="Delete"></asp:LinkButton>
                            <asp:HyperLink ID="lnkDelete" runat="server" CssClass="dgLink"></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
            <PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
                Position="Bottom" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
        </asp:DataGrid>
    </div>
    <uc1:ctlMensajes ID="CtlMensajes1" runat="server"></uc1:ctlMensajes>
    </form>
</body>
</html>
