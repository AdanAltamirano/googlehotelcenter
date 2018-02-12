<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ctrlSearchCompany.ascx.vb"
    Inherits="RateManager.ctrlSearchCompany" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<table class="bordeTabla" id="Table1" cellspacing="1" cellpadding="1" width="100%"
    border="0">
    <tr>
        <td align="right">
            <asp:Label ID="Label1" EnableViewState="False" CssClass="clsLabel" runat="server">Nombre de empresa:</asp:Label>&nbsp;
        </td>
        <td>
            <asp:TextBox ID="txtName" CssClass="textbox" runat="server" MaxLength="80" Width="200px"></asp:TextBox>
            <asp:LinkButton ID="lnkAvancedSearch" CssClass="dgLink" runat="server">Busqueda avanzada</asp:LinkButton>
        </td>
    </tr>
    <tr>
        <td align="right" colspan="2">
            <asp:Panel ID="PanelAvancedSearch" runat="server" Visible="False">
                <table id="Table3" cellspacing="1" cellpadding="1" width="100%" border="0">
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblRubro" runat="server" CssClass="clsLabel" EnableViewState="False">Rubro:</asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="cmbRubro" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td valign="bottom" align="center" colspan="2">
                            <table id="Table2" cellspacing="0" cellpadding="0" align="center" border="0">
                                <tr>
                                    <td style="height: 14px" align="center">
                                        <asp:Label ID="Label4" runat="server" CssClass="clsLabel" EnableViewState="False"
                                            Visible="False">Pais:</asp:Label>
                                    </td>
                                    <td style="height: 14px" align="center">
                                        <asp:Label ID="Label7" runat="server" CssClass="clsLabel" EnableViewState="False">Estado:</asp:Label>
                                    </td>
                                    <td style="height: 14px" align="center">
                                        <asp:Label ID="lblMunicipio" runat="server" CssClass="clsLabel" Visible="False">Municipio:</asp:Label>
                                    </td>
                                    <td style="height: 14px" align="center">
                                        <asp:Label ID="lblCiudad" runat="server" CssClass="clsLabel" Visible="False">Ciudad:</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="Label2" runat="server" CssClass="clsLabel" EnableViewState="False">Lugar:&nbsp;</asp:Label>
                                        <asp:DropDownList ID="cmbPais" runat="server" Visible="False">
                                        </asp:DropDownList>
                                    </td>
                                    <td align="right">
                                        <asp:DropDownList ID="cmbEstado" runat="server" AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cmbMunicipio" runat="server" Visible="False" DESIGNTIMEDRAGDROP="835"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                    <td align="right">
                                        <asp:DropDownList ID="cmbCiudad" runat="server" Visible="False">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </td>
    </tr>
</table>
<%
    Me.Page.RegisterStartupScript("NameFocus", "<script>document.getElementById('" & txtName.ClientID & "').focus(); </script>")
%>
