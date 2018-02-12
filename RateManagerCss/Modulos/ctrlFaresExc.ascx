<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ctrlFaresExc.ascx.vb"
    Inherits="RateManager.ctrlFaresExc" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<style type="text/css">
    #tblCtrlHomePage
    {
        vertical-align: top;
    }
    #tblCtrlHomePage tr
    {
        height: 14px;
    }
</style>
<table id="tblCtrlHomePage" cellpadding="0" cellspacing="0" border="0" style="text-align: center;
    width: 100%; padding-bottom: 4px;">
    <tr>
        <td colspan="3" style="text-align: left;">
            <asp:TextBox ID="txtCambia" Style="display: none" CssClass="TextBox" Width="34px"
                runat="server"></asp:TextBox>
            <asp:HyperLink ID="lnkDay" runat="server" Enabled="False" EnableViewState="False"
                Style="padding-left: 4px;">HyperLink</asp:HyperLink>
            <input type= "hidden" id="hPersonasExtras" runat="server" />
        </td>
    </tr>
    <tr>
        <td colspan="3" class="DataGridAlternatedItem">
            <asp:HyperLink ID="hplShowRates" runat="server" EnableViewState="False" CssClass="dglink">ShowRates</asp:HyperLink>
            <asp:TextBox ID="Textbox" Style="display: none" CssClass="TextBox" Width="34px" runat="server"></asp:TextBox>
            <input id="PriceAdultChildExc" type="hidden" value="99$99|99$99" name="PriceAdultChildExc"
                runat="server">
            <asp:HyperLink ID="lnkViewRate" runat="server" EnableViewState="False" CssClass="link">NA
            </asp:HyperLink>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblAdult" runat="server" EnableViewState="False" CssClass="clslabel">A</asp:Label>
        </td>
        <td>
            <asp:Label ID="lblChild" runat="server" EnableViewState="False" CssClass="clslabel">C</asp:Label>
        </td>
        <td>
            <asp:Label ID="lbljunior" runat="server" EnableViewState="False" CssClass="clslabel">J</asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:TextBox ID="txtAdult" Width="38px" runat="server" EnableViewState="False" CssClass="textbox"
                MaxLength="6" Columns="3"></asp:TextBox>
        </td>
        <td>
            <asp:TextBox ID="txtChild" Width="38px" runat="server" EnableViewState="False" CssClass="textbox"
                MaxLength="6" Columns="3"></asp:TextBox>
        </td>
        <td>
            <asp:TextBox ID="txtJunior" Width="38px" runat="server" EnableViewState="False" CssClass="textbox"
                MaxLength="6" Columns="3"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblAdultExtra" runat="server" EnableViewState="False" CssClass="clslabel">EA</asp:Label>
        </td>
        <td>
            <asp:Label ID="lblChildExtra" runat="server" EnableViewState="False" CssClass="clslabel">EC</asp:Label>
        </td>
        <td>
            <asp:Label ID="lblJuniorExtra" runat="server" EnableViewState="False" CssClass="clslabel">EJ</asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:TextBox ID="txtAdultExtra" Width="38px" runat="server" EnableViewState="False"
                CssClass="textbox" MaxLength="6" Columns="3" Style="background: #eee3d0;"></asp:TextBox>
        </td>
        <td>
            <asp:TextBox ID="txtChildExtra" Width="38px" runat="server" EnableViewState="False"
                CssClass="textbox" MaxLength="6" Columns="3" Style="background: #eee3d0;"></asp:TextBox>
        </td>
        <td>
            <asp:TextBox ID="txtJuniorExtra" Width="38px" runat="server" EnableViewState="False"
                CssClass="textbox" MaxLength="6" Columns="3" Style="background: #eee3d0;"></asp:TextBox>
        </td>
    </tr>
</table>
<%--<table id="tblCtrlHomePage1" cellspacing="0" cellpadding="0" width="100%" border="1">
    <tbody>
        <tr>
            <td colspan="2">
            </td>
        </tr>
        <tr>
            <td class="DataGridAlternatedItem" colspan="2">
            </td>
        </tr>
        <tr>
            <td style="height: 19px" align="center">
                &nbsp;
            </td>
            <td style="height: 19px">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td style="height: 19px">
                &nbsp;
            </td>
            <td style="height: 19px">
                &nbsp;
            </td>
        </tr>
    </tbody>
</table>
--%>