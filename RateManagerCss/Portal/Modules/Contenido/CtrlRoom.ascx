<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CtrlRoom.ascx.vb" Inherits="RateManager.CtrlRoom"
    TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="uc1" TagName="CambiarContenido" Src="CambiarContenido.ascx" %>
<table id="Table1" cellspacing="1" cellpadding="1" width="100%" border="0">
    <tr>
        <td>
            <uc1:CambiarContenido ID="ctrlTitle" runat="server" Visible="False"></uc1:CambiarContenido>
            <asp:Label ID="lblTitle" runat="server" CssClass="clsdarklabel" EnableViewState="False">Rooms</asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <table id="Table2" cellspacing="1" cellpadding="1" width="100%" border="0" height="100%">
                <tr>
                    <td align="left" valign="top" width="160">
                        <uc1:CambiarContenido ID="ctrlImage" runat="server" DESIGNTIMEDRAGDROP="142"></uc1:CambiarContenido>
                        <asp:Image ID="imgRoom" runat="server"></asp:Image>
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="center">
                        <table id="Table3" height="100%" cellspacing="2" cellpadding="2" width="100%" border="0">
                            <tr>
                                <td height="100%" valign="top">
                                    <uc1:CambiarContenido ID="ctrlLinkDescription" runat="server"></uc1:CambiarContenido>
                                    <asp:Label ID="lblDescription" runat="server" Width="100%" CssClass="clslabel"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
