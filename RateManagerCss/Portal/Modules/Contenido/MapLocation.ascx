<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="MapLocation.ascx.vb"
    Inherits="RateManager.MapLocation" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="uc1" TagName="CambiarContenido" Src="CambiarContenido.ascx" %>
<asp:Panel ID="lPanel" runat="server">
    <table id="Table1" cellspacing="1" cellpadding="1" width="100%" border="0">
        <tr>
            <td>
                <uc1:CambiarContenido ID="ctrlTitle" runat="server" Visible="False"></uc1:CambiarContenido>
                <asp:Label ID="lblTitle" runat="server" CssClass="clsdarklabel" EnableViewState="False">Map location</asp:Label>
            </td>
        </tr>
        <tr>
            <td valign="middle" align="left" width="160">
                <uc1:CambiarContenido ID="ctrlLinkImage" runat="server"></uc1:CambiarContenido>
                <asp:Image ID="imgMap" runat="server"></asp:Image>
            </td>
        </tr>
        <tr>
            <td valign="middle">
                <uc1:CambiarContenido ID="ctrlLinkDescription" runat="server" DESIGNTIMEDRAGDROP="10">
                </uc1:CambiarContenido>
                <asp:Label ID="lblDescription" runat="server" CssClass="clslabel"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="gPanel" runat="server">
    <table id="Table4" cellspacing="1" cellpadding="1" width="100%" border="0">
        <tr>
            <td colspan="2">
                <asp:Label ID="lblGTitle" runat="server" CssClass="clsdarklabel" EnableViewState="False">Location</asp:Label>
            </td>
        </tr>
        <tr>
            <td width="10">
            </td>
            <td>
                <asp:Label ID="lblGTitle1" runat="server" CssClass="clsdarklabel" DESIGNTIMEDRAGDROP="107">Hotel Location</asp:Label>
            </td>
        </tr>
        <tr>
            <td width="10">
            </td>
            <td>
                <asp:Label ID="lblGDescription" runat="server" CssClass="clslabel" DESIGNTIMEDRAGDROP="110"></asp:Label>
            </td>
        </tr>
        <tr>
            <td width="10">
            </td>
            <td>
                <asp:Label ID="lblGTitle2" runat="server" CssClass="clsdarklabel">Directions to Hotel</asp:Label>
            </td>
        </tr>
        <tr>
            <td width="10">
            </td>
            <td>
                <asp:Label ID="lblGDescription1" runat="server" CssClass="clslabel"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Panel>
<table id="Table2" height="10" cellspacing="0" cellpadding="0" width="100%" border="0">
    <tr>
        <td>
        </td>
    </tr>
</table>
