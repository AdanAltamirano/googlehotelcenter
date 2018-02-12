<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CtrlIdiomaPortalFCK.ascx.vb"
    Inherits="RateManager.CtrlIdiomaPortalFCK" %>
<%@ Register Src="../Portal/Modules/Contenido/CtrlIdiomaFCk.ascx" TagName="CtrlIdiomaFCk"
    TagPrefix="uc1" %>
<table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
    <tr>
        <td style=" padding-top:2px;">
            <uc1:CtrlIdiomaFCk ID="CtrlIdiomaFCk1" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:RequiredFieldValidator ID="rfvDefaultText" runat="server" Display="Dynamic"
                CssClass="validators" ControlToValidate="TextBox1" Enabled="False">El texto en ingles es requerido</asp:RequiredFieldValidator>
            <asp:TextBox ID="TextBox1" runat="server" Width="32px" Visible="False"></asp:TextBox>
        </td>
    </tr>
</table>
