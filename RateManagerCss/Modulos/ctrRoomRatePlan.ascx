<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ctrRoomRatePlan.ascx.vb" Inherits="RateManager.ctrRoomRatePlan"
    TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<table id="tblCtrlHomePage" cellspacing="0" cellpadding="0" width="100%" border="0">
    <tr>
        <td colspan="2">
            <asp:TextBox ID="txtCambia" Style="display: none" runat="server" Width="34px" CssClass="TextBox"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="DataGridAlternatedItem" colspan="2" style="padding-left: 4px;">
            <asp:Label ID="lblDay" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <td style="height: 5px" align="center" colspan="2">
        </td>
    </tr>
    <tr>
        <td nowrap align="center" colspan="2" style="height: 27px">
            <asp:Label ID="lblAvailability" runat="server" EnableViewState="False" CssClass="clslabel">1/</asp:Label>
            <asp:HyperLink ID="lnkRooms" runat="server" EnableViewState="False" CssClass="link">0</asp:HyperLink>            
            <asp:TextBox ID="txtRooms" runat="server" Width="40px" CssClass="textbox" Visible="true" MaxLength="2"></asp:TextBox>
            <asp:Label ID="lblAvailabilityByRoom"  CssClass =PromotionSpecialRate runat="server" EnableViewState="False" style="display:none;">/1</asp:Label>
        </td>
    </tr>
    <tr>
        <td style="height: 5px" nowrap align="center" colspan="2">
        </td>
    </tr>
</table>
