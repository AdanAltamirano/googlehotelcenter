<%@ Control Language="vb" EnableViewState="true" AutoEventWireup="false" CodeBehind="ctrlHP.ascx.vb"
    Inherits="RateManager.ctrlHP" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<table id="tblCtrlHomePage" cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
    <tr>
        <td style="padding-left: 4px;">
            <asp:HyperLink ID="lnkDay" EnableViewState="False" runat="server" Enabled="False">HyperLink</asp:HyperLink>
            <asp:TextBox ID="txtCambia" Style="display: none" runat="server" Width="34px" CssClass="TextBox"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td style="text-align: center; height: 19px">
            <asp:Label ID="lblRackRate" runat="server" EnableViewState="False" CssClass="clslabel">$100</asp:Label>
        </td>
    </tr>
    <tr>
        <td align="center" style="height: 27px;">
            <asp:Label ID="lblAvailability" runat="server" EnableViewState="False" CssClass="clslabel">1/</asp:Label>
            <asp:HyperLink ID="lnkRooms" runat="server" EnableViewState="False" CssClass="link">0</asp:HyperLink>
            <asp:TextBox ID="txtRooms" runat="server" Width="40px" CssClass="textbox" Visible="true"
                MaxLength="2"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td align="center">
            <asp:Label ID="lblMenor" runat="server" EnableViewState="False" CssClass="clslabel">$10</asp:Label>
        </td>
    </tr>
</table>
