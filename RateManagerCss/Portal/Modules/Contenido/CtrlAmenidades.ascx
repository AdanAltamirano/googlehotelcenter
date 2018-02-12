<%@ Import Namespace="system.configuration.configurationsettings" %>
<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CtrlAmenidades.ascx.vb"
    Inherits="RateManager.CtrlAmenidades" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<div id="amenidades">
    <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
            <td>
                <asp:HyperLink ID="lnkEditar" NavigateUrl="editar" CssClass="dglink" runat="server">Editar Amenidades</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblMensaje" CssClass="Validators" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:DataList ID="dlAmenities" runat="server" RepeatLayout="Flow" CellSpacing="1"
                    CellPadding="1" RepeatDirection="Horizontal">
                    <ItemTemplate>
                        <asp:Image ID="Image" runat="server" ImageUrl='<%#AppSettings("ImagenesAmenidades") & container.dataitem(RateManager.DataCtrlAmenidades.IdAmenidad_Field)%>'
                            title='<%#container.dataitem(RateManager.DataCtrlAmenidades.Texto_Field)%>'>
                        </asp:Image>
                    </ItemTemplate>
                </asp:DataList>
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
    </table>
</div>
<div id="amenidadesedit" style="display: none">
    <table id="Table11" cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
            <td>
                <asp:LinkButton ID="lnkGuardar" CssClass="dgLink" runat="server">Guardar Amenidades</asp:LinkButton>
            </td>
            <td align="right">
                <asp:HyperLink ID="Hyperlink1" NavigateUrl="editar" CssClass="dglink" runat="server">Close</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:DataList ID="dlEditAmenities" runat="server" CellSpacing="1" CellPadding="1"
                    RepeatColumns="3">
                    <ItemTemplate>
                        <table id="Table2" style="border-right: #006699 1px solid; border-top: #006699 1px solid;
                            border-left: #006699 1px solid; border-bottom: #006699 1px solid" cellspacing="1"
                            cellpadding="1" width="100%" border="0">
                            <tr>
                                <td style="border-right: #006699 1px solid; height: 40px" valign="middle" align="center"
                                    width="60px">
                                    <asp:Image ID="Image1" title="<%#container.dataitem(RateManager.DataCtrlAmenidades.Texto_Field)%>"
                                        runat="server" ImageUrl='<%#AppSettings("ImagenesAmenidades") & container.dataitem(RateManager.DataCtrlAmenidades.IdAmenidad_Field)%>'>
                                    </asp:Image>
                                </td>
                                <td style="background-color: lightsteelblue" valign="middle">
                                    <asp:CheckBox ID="Check" CssClass="clslabel" runat="server"></asp:CheckBox>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:DataList>
            </td>
        </tr>
    </table>
    <table id="Table3" height="10" cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
            <td>
                <asp:LinkButton ID="lnkGuardar2" CssClass="dgLink" runat="server">Guardar Amenidades</asp:LinkButton>
            </td>
            <td align="right">
                <asp:HyperLink ID="Hyperlink2" NavigateUrl="editar" CssClass="dglink" runat="server">Close</asp:HyperLink>
            </td>
        </tr>
    </table>
</div>
