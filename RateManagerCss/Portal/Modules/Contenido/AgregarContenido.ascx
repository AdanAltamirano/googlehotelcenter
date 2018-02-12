<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="AgregarContenido.ascx.vb"
    Inherits="RateManager.AgregarContenido" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="uc1" TagName="Contenido" Src="ContenidoFCk.ascx" %>
<%--style><.ImgTitulo { FONT-WEIGHT: bold; FONT-SIZE: 11px; COLOR: white; FONT-FAMILY: Verdana; BACKGROUND-COLOR: #336699 }
	.BordeWin { BORDER-RIGHT: steelblue 2px solid; BORDER-TOP: royalblue 1px solid; BORDER-LEFT: royalblue 1px solid; BORDER-BOTTOM: steelblue 2px solid }
</style>--%>
<asp:Label ID="lblPaso" runat="server" CssClass="clsdarklabel"></asp:Label><asp:ImageButton
    ID="ImageButton1" runat="server" ImageUrl="../../../Images/add.gif" ToolTip="Add Content">
</asp:ImageButton>
<asp:Panel ID="pEdit" runat="server" Visible="False" Style="position: absolute" BackColor="white"
    CssClass="BordeWin" Width="700px">
    <table class="caja" id="Table2" cellspacing="1" cellpadding="1" width="100%" align="center"
        border="0">
        <tr>
            <td align="center">
                <table id="Table1" cellspacing="0" cellpadding="1" width="100%" border="0">
                    <tr class="trTitle">
                        <td>
                            <asp:Label ID="lblTitle" runat="server" EnableViewState="False">Agregar contenido</asp:Label>
                        </td>
                    </tr>
                </table>
                <uc1:Contenido ID="EditConte" runat="server"></uc1:Contenido>
            </td>
        </tr>
        <tr>
            <td align="right">
                <table cellspacing="2" cellpadding="0" border="0">
                    <tr>
                        <td align="center">
                        </td>
                        <td align="center">
                            <asp:Button ID="cmdGuardar" CssClass="Button" runat="server" Width="70px" Text="Aceptar">
                            </asp:Button>
                        </td>
                        <td align="center">
                            <asp:Button ID="cmdCancelar" CssClass="Button" runat="server" Width="70px" Text="Cancelar"
                                CausesValidation="False"></asp:Button>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Label ID="lblTexto" runat="server" CssClass="clslabel"></asp:Label>
