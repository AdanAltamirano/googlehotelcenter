<%@ Register TagPrefix="uc1" TagName="CambiarContenido" Src="CambiarContenido.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrlAmenidades" Src="CtrlAmenidades.ascx" %>
<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="InfoAmenidades.ascx.vb"
    Inherits="RateManager.InfoAmenidades" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<asp:Panel ID="lPanel" runat="server">
    <table id="Table1" cellspacing="1" cellpadding="1" width="100%" border="0">
        <tr class="">
            <td valign="top" align="left" colspan="2">
                <uc1:CambiarContenido ID="ctrlTitle" runat="server" Visible="False"></uc1:CambiarContenido>
                <asp:Label ID="lblTitle" runat="server" CssClass="clsdarklabel" EnableViewState="False" >Amenidades de la Propiedad</asp:Label>
            </td>
        </tr>
        <tr>
            <table>
                <tr>
                    <td align="left" valign="top">
                        <uc1:CambiarContenido ID="lnkImagPropiedades" runat="server" />
                        <asp:Image ID="imgPropAmenidades" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td valign="middle">
                        <table id="Tablasde2" border="0" cellpadding="2" cellspacing="2" width="100%">
                        </table>
                        <uc1:CambiarContenido ID="lnkInfoPropiedad" runat="server" DESIGNTIMEDRAGDROP="24" />
                        <asp:Label ID="lblInfoPropiedad" runat="server" CssClass="clslabel" EnableViewState="False">Amenidades de la Propiedad</asp:Label>
                    </td>
                </tr>
            </table>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="gPanel" runat="server">
    <table id="Table2" cellspacing="1" cellpadding="1" width="100%" border="0">
        <tr>
            <td colspan="2">
                <asp:Label ID="lblGTitle" runat="server" CssClass="clsdarklabel" DESIGNTIMEDRAGDROP="128"
                    EnableViewState="False">Facilities</asp:Label>
            </td>
        </tr>
        <tr>
            <td width="10">
            </td>
            <td>
                <asp:Label ID="lblGDescription" runat="server" CssClass="clslabel"></asp:Label>
            </td>
        </tr>
        <tr>
            <td width="10" colspan="2">
            </td>
        </tr>
    </table>
</asp:Panel>
<uc1:CtrlAmenidades ID="CtrlAmen" runat="server"></uc1:CtrlAmenidades>
