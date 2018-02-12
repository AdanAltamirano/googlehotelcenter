<%@ Register TagPrefix="uc1" TagName="CambiarContenido" Src="CambiarContenido.ascx" %>
<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="InfoPropiedad.ascx.vb"
    Inherits="RateManager.InfoPropiedad" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<table id="Table3" cellspacing="1" cellpadding="1" width="100%" border="0">
    <tr>
        <td colspan="2" class="dgitem">
            <uc1:CambiarContenido ID="ctrlTitle" Visible="False" runat="server"></uc1:CambiarContenido>
            <asp:Label ID="lblTitle" runat="server" EnableViewState="False">Property Information</asp:Label>
        </td>
    </tr>
    <tr>
        <td valign="top">
            <table>
                <tr>
                    <td valign="top">
                        <uc1:CambiarContenido ID="lnkPropiedad" runat="server"></uc1:CambiarContenido>
                    </td>
                </tr>
                <tr>
                    <td  valign="top">
                        <asp:Image ID="imgPropiedad" runat="server" CssClass="images"></asp:Image>
                    </td>
                </tr>
                <tr>
                    <td valign="top" colspan="2">
                        <uc1:CambiarContenido ID="lnkInfo1" runat="server" DESIGNTIMEDRAGDROP="40"></uc1:CambiarContenido>
                        <asp:Label ID="lblInfo1" runat="server" CssClass="clslabel"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td valign="top" colspan="2">
                        <uc1:CambiarContenido ID="lnkInfo2" runat="server"></uc1:CambiarContenido>
                        <asp:Label ID="lblInfo2" runat="server" CssClass="clslabel" Height="100%"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td valign="top" colspan="2">
                        <uc1:CambiarContenido ID="lnkInfo3" runat="server"></uc1:CambiarContenido>
                        <asp:Label ID="lblInfo3" runat="server" CssClass="clslabel" Height="100%"></asp:Label>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<%--<table id="Table1" height="10" cellspacing="0" cellpadding="0" width="100%" border="0">
    <tr>
        <td>
        </td>
    </tr>
</table>--%>