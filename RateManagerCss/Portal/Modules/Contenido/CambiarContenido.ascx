<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CambiarContenido.ascx.vb"
    Inherits="RateManager.CambiarContenido" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="uc1" TagName="ctlConstructorGramatical" Src="ctlConstructorGramatical.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Contenido" Src="ContenidoFCk.ascx" %>
<%--<style>
    .ImgTitulo
    {
        font-weight: bold;
        font-size: 11px;
        color: white;
        font-family: Verdana;
        background-color: #336699;
    }
    .BordeWin
    {
        border-right: steelblue 2px solid;
        border-top: royalblue 1px solid;
        border-left: royalblue 1px solid;
        border-bottom: steelblue 2px solid;
    }
</style>--%>
<asp:Label ID="lblPaso" runat="server" CssClass="clsdarklabel"></asp:Label><asp:ImageButton
    ID="ImageButton1" runat="server" ImageUrl="../../../Images/edit.gif" ToolTip="Edit Content">
</asp:ImageButton>
<asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="../../../Images/delete.gif"
    ToolTip="Delete Content" Visible="False"></asp:ImageButton><asp:Label ID="lblTexto"
        runat="server" CssClass="clslabel"></asp:Label><br />
<asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="../../../Images/delete.gif"
    ToolTip="Delete Content" Visible="False"></asp:ImageButton><asp:Label ID="lblTexto1"
        runat="server" CssClass="clslabel"></asp:Label><br />
<asp:Panel ID="pEdit" Style="position: absolute; float:left; left:2%" runat="server" CssClass="BordeWin"
    Visible="False" BackColor="white" Width="700px">
    <table id="Separa" cellspacing="1" cellpadding="1" width="100%">
        <tr>
            <td valign="top" width="100%">
                <table class="ImgTitulo" id="Table1" cellspacing="0" cellpadding="1" width="100%"
                    border="0">
                    <tr class="trTitle">
                        <td class="dgitem">
                            <asp:Label ID="lblTitle" CssClass="clsdarklabel" runat="server">Editar contenido</asp:Label>
                        </td>
                    </tr>
                    <tr class="trContent">
                        <td class="tdContent">
                            <uc1:Contenido ID="EditConte" runat="server" Visible="true"></uc1:Contenido>
                            <uc1:ctlConstructorGramatical ID="CtlConstructorGramatical1" runat="server" Visible="false">
                            </uc1:ctlConstructorGramatical>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <span class="Validators">
                    <%=RateManager.PortalCulture.GetString(If(CType(Me.Page, RateManager.PaginaBase).IsSupervisor, "01365", "01392"))%></span>
            </td>
        </tr>
        <%--  <% end if %>--%>
        <tr>
            <td align="left">
                <asp:Button ID="cmdGramatical" CssClass="Button" runat="server" Visible="False" Width="120px"
                    Text="Ayuda gramatical"></asp:Button>
                <asp:Button ID="cmdFreeForm" CssClass="Button" runat="server" Visible="true" Width="120px"
                    Text="Free Form"></asp:Button>
                <% If CType(Me.Page, RateManager.PaginaBase).IsSupervisor Then%>
                <asp:Button ID="cmdPublicar" CssClass="Button" runat="server" Width="70px" Text="Publicar">
                </asp:Button>
                <% End If%>
                <asp:Button ID="cmdGuardar" CssClass="Button" runat="server" Width="70px" Text="Aceptar">
                </asp:Button>
                <asp:Button ID="cmdCancelar" CssClass="Button" runat="server" Width="70px" Text="Cancelar"
                    CausesValidation="False"></asp:Button>
            </td>
        </tr>
    </table>
</asp:Panel>
