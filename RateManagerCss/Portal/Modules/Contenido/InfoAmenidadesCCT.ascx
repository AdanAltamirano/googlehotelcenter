<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="InfoAmenidadesCCT.ascx.vb"
    Inherits="RateManager.InfoAmenidadesCCT" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
    <%@ Register TagPrefix="uc1" TagName="CambiarContenido" Src="CambiarContenido.ascx" %>
    <%@ Register TagPrefix="uc1" TagName="CtrlAmenidades" Src="CtrlAmenidades.ascx" %>
<asp:Panel ID="lPanel" runat="server">
<table id="Table3" cellspacing="1" cellpadding="1" width="100%" border="0">
    <tr>
        <td colspan="2" class="dgitem">
            <uc1:CambiarContenido ID="ctrlTitle" Visible="False" runat="server"></uc1:CambiarContenido>
            <asp:Label ID="lblTitle" runat="server" EnableViewState="False">Property Information</asp:Label>
        </td>
    </tr>
    <tr>
        <td valign="top">
            <table width="100%">
                <%--<tr>
                    <td valign="top" colspan="2">
                        <uc1:CambiarContenido ID="lnkInfo1" runat="server" DESIGNTIMEDRAGDROP="40"></uc1:CambiarContenido>
                        <asp:Label ID="lblInfoHotel" runat="server" CssClass="clslabel"></asp:Label>
                    </td>
                </tr>--%>
                <tr>
                    <td valign="middle" align="left" width="160">
                        <uc1:CambiarContenido ID="ctrlLinkImagecct" runat="server"></uc1:CambiarContenido>
                        <asp:Image ID="imgMapcct" runat="server"></asp:Image>
                    </td>
                </tr>
                <tr>
                    <td valign="top" colspan="2">
                        <uc1:CambiarContenido ID="lnkInfo2" runat="server"></uc1:CambiarContenido>
                        <asp:Label ID="lblInfoMapa" runat="server" CssClass="clslabel" Height="100%"></asp:Label>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
</asp:Panel>
<uc1:CtrlAmenidades ID="CtrlAmen" runat="server"></uc1:CtrlAmenidades>