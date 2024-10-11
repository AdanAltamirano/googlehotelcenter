<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="HotelItem.aspx.vb" Inherits="RateManager.HotelItem" %>


<%@ Register Src="~/HotelAdministrator/Modules/ctrlHotelItem.ascx" TagName="ctrlHotelItem" TagPrefix="uc1" %>
<%@ Register TagPrefix="uc1" TagName="ctlMensajes" Src="~/Modulos/ctlMensajes.ascx" %>
<%@ Register Src="~/Modulos/ctrlAutoComplete.ascx" TagName="ctrlAutoComplete" TagPrefix="uc2" %>


    
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    
    <link rel="stylesheet" type="text/css" href="../../StyleSheets/Styles.css">
    
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
        CtrHotelItem_Clear();
        FireShow('divCtrlContent', 'cmdNew', false)

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
            <asp:Label ID="lblTitle" runat="server" EnableViewState="False" class="tituloSeccion">Items</asp:Label>
        </div>
    </div>
    <div runat="server" id="divContenedor">
        <table id="bookingcontainer clear" border="0" cellspacing="0" cellpadding="2" width="100%">
            <tr>
                <td>
                    <div id="divCtrlContent" style="<%  If Not ctrlHotelItem1.IsEdit Then %>  display: none; <% End if %>">
                        <table id="Table2" border="0" cellspacing="0" cellpadding="0" width="100%" align="center">
                         <tr class="trTitle rounded-corners" id="dvContent2">
                              <td class="dgitem" align="left">
                                <asp:Label ID="lblMsg" runat="server" DESIGNTIMEDRAGDROP="987"></asp:Label>
                              </td>
                         </tr>
                            <tr class="trContent rounded-corners" id="dvContent">
                                <td class="tdContent" colspan="2">
                                    
                                        <br />
                                        
                                        <uc1:ctrlHotelItem ID="ctrlHotelItem1" runat="server"></uc1:ctrlHotelItem>
                                        
                                        <br />
                                        <asp:Button ID="btnSave" runat="server" EnableViewState="False" CssClass="Button"
                                            Text="Guardar" OnClientClick="return"  CausesValidation="true">
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
         <div style="float:left; margin-bottom:15px;" >
           <asp:Label ID="lblFilter" runat="server" Text="Filtro:" ></asp:Label>
           <asp:DropDownList ID="ddlFilter" runat="server" AutoPostBack="True">
            <asp:ListItem Text="Solo activos" Value="1"></asp:ListItem>
            <asp:ListItem Text="Solo no activos" Value="0"></asp:ListItem>
            <asp:ListItem Text="Activos y no activos" Value="-1"></asp:ListItem>
           </asp:DropDownList>
        </div>
        <asp:DataGrid ID="grid" runat="server" Width="99%" AllowPaging="True" PageSize="20"
            GridLines="None" DataKeyField="idHotelItem" AutoGenerateColumns="False"
            CssClass="datagrid" ShowFooter="True">
            <SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
            <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
            <ItemStyle CssClass="dgItem"></ItemStyle>
            <HeaderStyle CssClass="dgHeader"></HeaderStyle>
            <FooterStyle HorizontalAlign="Right"></FooterStyle>
            <Columns>
                <asp:BoundColumn Visible="False" DataField="idHotelItem"></asp:BoundColumn>
                <asp:BoundColumn DataField="Name" HeaderText="Name"></asp:BoundColumn>
                <asp:BoundColumn HeaderText="Price" DataField="Price"></asp:BoundColumn>
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
