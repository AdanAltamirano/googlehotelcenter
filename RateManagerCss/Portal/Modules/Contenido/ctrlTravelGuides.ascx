<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ctrlTravelGuides.ascx.vb"
    Inherits="RateManager.ctrlTravelGuides" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="uc1" TagName="CambiarContenido" Src="CambiarContenido.ascx" %>
<table id="Table3" cellspacing="1" cellpadding="1" width="100%" border="0">
    <tr>
        <td colspan="2">
            <br>
            <uc1:CambiarContenido ID="ctrlTitle" runat="server" Visible="False"></uc1:CambiarContenido>
            <asp:Label ID="lblTitle" runat="server" EnableViewState="False" CssClass="clsdarklabel">Meeting And Events Information</asp:Label>
        </td>
    </tr>
    <tr>
        <td valign="top" align="left" width="160">
            <uc1:CambiarContenido ID="lnkPropiedad" runat="server"></uc1:CambiarContenido>
            <asp:Image ID="imgPropiedad" runat="server"></asp:Image>
        </td>
       
    </tr>
    <tr>
     <td valign="top">
            <p>
                <uc1:CambiarContenido ID="lnkInfo1" runat="server" DESIGNTIMEDRAGDROP="40"></uc1:CambiarContenido>
                <asp:Label ID="lblInfo1" runat="server" CssClass="clslabel"></asp:Label><br>
                <br>
            </p>
        </td>
    </tr>
</table>
<table id="Table1" height="10" cellspacing="0" cellpadding="0" width="100%" border="0">
    <tr>
        <td>
        </td>
    </tr>
</table>
