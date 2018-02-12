<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CtrRatePlanContain.ascx.vb"
    Inherits="RateManager.CtrRatePlanContain" %>
<%@ Register Src="CtrlIdiomaPortalFCK.ascx" TagName="CtrlIdiomaPortalFCK" TagPrefix="uc1" %>

<script>

    function ReceiveServerData(arg, context) {
        var e = document.getElementById('divContainer');
        if (e) e.innerHTML = arg;
        //alert('ReceiveServerData: ' + arg);
        //        var e = $('divContent');
        //        if (e) e.innerHTML = arg;
        //        alert(arg);
    }
    
</script>
<table style = "width:100%" >
<tr><td>

    <asp:DataGrid ID="DG_Datos" runat="server" Width="100%" AutoGenerateColumns="False" Height =100%
        CellPadding="0" GridLines="None" ShowHeader="False" CssClass="DataGrid">
        <SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
        <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
        <ItemStyle CssClass="dgItem"></ItemStyle>
        <HeaderStyle CssClass="dgHeader"></HeaderStyle>
        <Columns>
            <asp:BoundColumn ReadOnly="True">
                <ItemStyle Font-Size="10px" VerticalAlign="Top"></ItemStyle>
                <FooterStyle VerticalAlign="Top"></FooterStyle>
            </asp:BoundColumn>
            <asp:TemplateColumn>
                <ItemStyle VerticalAlign="Top" HorizontalAlign="Center" Width="24px"></ItemStyle>
                <ItemTemplate>                   
                    <asp:ImageButton ID="ImageButton3" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"idportal") %>'
                        CommandName="Edit" ToolTip='' Visible="true" ImageUrl="../Images/edit.gif"
                        CausesValidation="False"></asp:ImageButton>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:ImageButton ID="ImageButton1" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"idportal") %>'
                        CommandName="Update" ToolTip='00021' Visible="false" ImageUrl="~/Images/edit.gif">                        
                    </asp:ImageButton>
                    <asp:Label id="LlbEdit" Text ="Editing" runat=server CssClass="Label" style="margin-top: 8px;" ></asp:Label>
                </EditItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <ItemStyle VerticalAlign="Top" Width="90%"></ItemStyle>
                <ItemTemplate>
                    <table id="Table1" cellspacing="0" cellpadding="0" width="" border="0" style="margin-top: 4px;">
                        <tr>
                            <td>
                                <asp:Label ID="lblTitulo" runat="server" CssClass="bookingnormallabel" Text='<%# DataBinder.Eval(Container.DataItem,"portal") %>'>
                                </asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label5" runat="server" CssClass="Label" Text='<%# GetShortContainer(DataBinder.Eval(Container.DataItem,"texto"),20) %>'>
                                </asp:Label>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
                <EditItemTemplate>
                    <table id="Table2" cellspacing="0" cellpadding="0" border="0" width="100%" style="margin-top: 8px;">
                        <tr>
                            <td>
                                <asp:Label ID="lblTitulo" runat="server" CssClass="bookingnormallabel" Text='<%# DataBinder.Eval(Container.DataItem,"portal") %>'>
                                </asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <uc1:CtrlIdiomaPortalFCK ID="CtrlIdiomaPortalFCK1" runat="server" />
                            </td>
                        </tr>                        
	                    <tr id="pnlUnpublishedContain" runat="server">
	                        <td>
	                            <span class="validators"><%=RateManager.PortalCulture.GetString(If(CType(Me.Page, RateManager.PaginaBase).IsSupervisor, "01365", "01392"))%></span>
	                        </td>
	                    </tr>	
                        <tr>
                            <td align="right" style="padding-right: 18px; padding-top: 2px; padding-bottom: 4px;">
                            
                                <asp:Button ID="Button1" Width="" runat="server" CssClass="button" CommandName="Update" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"idportal") %>' Text="Update" Visible="true" CausesValidation="False"></asp:Button>
                                <asp:Button ID="BtnUpdate2" Width="" runat="server" CssClass="button" CommandName="Cancel" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"idportal") %>' Text="Cerrar" Visible="true" CausesValidation="False"></asp:Button>                                  
                                <% If CType(Me.Page, RateManager.PaginaBase).IsSupervisor Then%>
                                <asp:Button ID="BtnPublish" Width="" runat="server" CssClass="button" CommandName="Publish" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"idportal") %>' Text="Publicar" Visible="true" CausesValidation="False"></asp:Button>         
                                <% End If%>                         
                            </td>
                        </tr>
                    </table>
                </EditItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <ItemStyle VerticalAlign="Top"></ItemStyle>
                <ItemTemplate>
                    <asp:LinkButton ID="ImgBtnDelete" Width="15px" runat="server" ToolTip='03094' CausesValidation="False"
                        CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"idportal") %>'
                        CssClass="BotonDelete" Height="15px" Visible="False">
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn Visible="False" DataField="idportal"></asp:BoundColumn>
            <asp:BoundColumn Visible="False" DataField="idIdioma"></asp:BoundColumn>
        </Columns>
    </asp:DataGrid>

</td></tr>
</table>