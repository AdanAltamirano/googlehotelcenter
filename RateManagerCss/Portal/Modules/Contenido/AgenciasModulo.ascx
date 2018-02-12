<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="AgenciasModulo.ascx.vb"
    Inherits="RateManager.AgenciasModulo" %>
<%@ Register Src="../CtrlPreserveScrolls.ascx" TagName="CtrlPreserveScrolls" TagPrefix="uc2" %>
<%@ Register Assembly="Anthem" Namespace="Anthem" TagPrefix="anthem" %>
<table cellspacing="0" cellpadding="2" width="100%" align="left" border="0">
    <tr class="trTitle rounded-corners">
        <td class="Titulo" align="center" colspan="4">
            <asp:Label ID="lblTitulo" runat="server">Solicitudes de Registro</asp:Label>
        </td>
    </tr>
    <tr class="trContent rounded-corners">
        <td class="tdContent">
            <center>
                <table cellspacing="0" cellpadding="0" width="700" align="center" border="0">
                    <tr class="trTitle rounded-corners">
                        <td class="dgItem" align="center" colspan="4">
                            <asp:Label ID="lblInformacion" runat="server">Informacion Empresa</asp:Label>
                        </td>
                    </tr>
                    <tr class="trContent rounded-corners">
                        <td class="tdContent">
                            <table cellspacing="0" cellpadding="0" border="0">
                                <tr>
                                    <td align="right" width="25%">
                                        <asp:Label ID="lblNombre" runat="server" EnableViewState="False">Nombre :</asp:Label>
                                    </td>
                                    <td width="25%">
                                        <asp:TextBox ID="txtNombre" runat="server" MaxLength="80" Width="320px"></asp:TextBox><asp:RequiredFieldValidator
                                            ID="rfvNombre" runat="server" CssClass="validators" ForeColor=" " ErrorMessage="Nombre de la Empresa es requerido"
                                            ControlToValidate="txtNombre" Display="Dynamic">*</asp:RequiredFieldValidator>
                                    </td>
                                    <td align="right" width="25%">
                                        <asp:Label ID="lblDomicilio0" runat="server" EnableViewState="False" >IATA :</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIATA" runat="server" MaxLength="11" Width="150px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="height: 21px">
                                    </td>
                                    <td style="height: 21px">
                                        <%-- <select id="ddlCountries" runat="server" style="width:150px;" class="country"></select>
                    <input id="selectedCountry" type="hidden" runat="server" value="" />--%>
                                    </td>
                                    <td align="right" style="height: 21px">
                                    </td>
                                    <td style="height: 21px">
                                        <%--<select id="ddlStates" runat="server" style="width:150px;" class="state"></select>
                    <input id="selectedState" type="hidden" runat="server" value="" />--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblDomicilio" runat="server" EnableViewState="False">Domicilio :</asp:Label>
                                    </td>
                                    <td>
                                        <%--<select id="ddlDistricts" runat="server" style="width:150px;" class="district"></select>
                    <input id="selectedDistrict" type="hidden" runat="server" value="" />--%>
                                        <asp:TextBox ID="txtDomicilio" runat="server" MaxLength="120" Width="320px"></asp:TextBox><asp:RequiredFieldValidator
                                            ID="rfvDomicilio" runat="server" CssClass="validators" ForeColor=" " ErrorMessage="El Domicilio de la Empresa es requerido"
                                            ControlToValidate="txtDomicilio" Display="Dynamic">*</asp:RequiredFieldValidator>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblCP" runat="server" EnableViewState="False">Cod. Postal :</asp:Label>
                                    </td>
                                    <td>
                                        <%--<select id="ddlCities" runat="server" style="width:250px;" class="city"></select>
                    <asp:CustomValidator Display="Dynamic" ID="rfvCiudad" runat="server" ErrorMessage="Ciudad de la Empresa es requerida" EnableViewState="false" ClientValidationFunction="ValidateCity"></asp:CustomValidator>
                    <input id="selectedCity" type="hidden" runat="server" value="" />--%>
                                        <asp:TextBox ID="txtCP" runat="server" MaxLength="11" Width="150px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblCiudad" runat="server" EnableViewState="False">Ciudad :</asp:Label>
                                    </td>
                                    <td>
                                        <anthem:TextBox ID="txtCiudad" runat="server" MaxLength="30" Width="150px"></anthem:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvCiudad" runat="server" CssClass="validators" ForeColor=" "
                                            ErrorMessage="Ciudad de la Empresa es requerida" ControlToValidate="txtCiudad"
                                            Display="Dynamic">*</asp:RequiredFieldValidator>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblPais" runat="server" EnableViewState="False">Pais :</asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cmbPaises" runat="server" Width="150px" AutoPostBack="false">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="Label2" runat="server" EnableViewState="False">Area :</asp:Label>
                                    </td>
                                    <td>
                                        <%--<select id="ddlAreas" runat="server" style="width:150px;" class="area"></select>
                    <input id="selectedArea" type="hidden" runat="server" value="" />--%><anthem:TextBox
                        ID="txtArea" runat="server" MaxLength="30" Width="150px"></anthem:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvArea" runat="server" CssClass="validators" ForeColor=" "
                                            ErrorMessage="Area es requerida" ControlToValidate="txtArea" Display="Dynamic">*</asp:RequiredFieldValidator>
                                    </td>
                                    <td align="right">
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblTel" runat="server" EnableViewState="False">Telefono :</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTel" runat="server" MaxLength="20" Width="150px"></asp:TextBox><asp:RequiredFieldValidator
                                            ID="rfvTel" runat="server" CssClass="validators" ForeColor=" " ErrorMessage="Telefono de la Empresa es requerido"
                                            ControlToValidate="txtTel" Display="Dynamic">*</asp:RequiredFieldValidator>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblFax" runat="server" CssClass="label" EnableViewState="False">Fax :</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFax" runat="server" MaxLength="20" CssClass="textbox" Width="150px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                    </td>
                                    <td>
                                    </td>
                                    <td align="right">
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="Label8" runat="server" CssClass="label" EnableViewState="False">Pagina Web :</asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtPaginaWeb" runat="server" MaxLength="80" CssClass="textbox" Width="329px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblComentarios" runat="server" CssClass="label" EnableViewState="False">Comentarios :</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtComentarios" runat="server" Height="55px" TextMode="MultiLine"
                                            Width="325px" MaxLength="256" Columns="40" Rows="8"></asp:TextBox>
                                    </td>
                                    <td align="right">
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="dgItem" align="center" colspan="4">
                                        <asp:Label ID="lblInformacionContacto" runat="server" EnableViewState="False">Informacion de Contacto</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblContactoNombre" runat="server" EnableViewState="False"> Contacto :</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtContactoNombre" runat="server" MaxLength="80" CssClass="textbox"
                                            Width="150px"></asp:TextBox><asp:RequiredFieldValidator ID="rfvContactoNombre" runat="server"
                                                CssClass="validators" ForeColor=" " ErrorMessage="Nombre de la persona de contacto es requerido"
                                                ControlToValidate="txtContactoNombre" Display="Dynamic">*</asp:RequiredFieldValidator>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblContactoPuesto" runat="server" EnableViewState="False">Puesto :</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtContactoPuesto" runat="server" MaxLength="50" CssClass="textbox"
                                            Width="150px"></asp:TextBox><asp:RequiredFieldValidator ID="rfvPuesto" runat="server"
                                                CssClass="validators" ForeColor=" " ErrorMessage="Puesto de la persona de contacto es requerido"
                                                ControlToValidate="txtContactoPuesto" Display="Dynamic">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblTel1" runat="server" EnableViewState="False">Telefono :</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtContactoTel" runat="server" MaxLength="20" CssClass="textbox"
                                            Width="150px"></asp:TextBox><asp:RequiredFieldValidator ID="rfvContactoTel" runat="server"
                                                CssClass="validators" ForeColor=" " ControlToValidate="txtContactoTel" Display="Dynamic">*</asp:RequiredFieldValidator>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblContactoCorreo" runat="server" EnableViewState="False">Correo :</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtContactoCorreo" runat="server" MaxLength="80" CssClass="textbox"
                                            Width="150px"></asp:TextBox><asp:RequiredFieldValidator ID="rfvContactoCorreo" runat="server"
                                                CssClass="validators" ForeColor=" " ErrorMessage="Formato de Correo Electronico no valido"
                                                ControlToValidate="txtContactoCorreo" Display="Dynamic">*</asp:RequiredFieldValidator><asp:RegularExpressionValidator
                                                    ID="RegularExpressionValidator1" runat="server" CssClass="validators" ForeColor=" "
                                                    ErrorMessage="Correo de la Persona de Contacto es Requerido" ControlToValidate="txtContactoCorreo"
                                                    Display="Dynamic" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">*</asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:Label ID="lblError" runat="server" CssClass="validators"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </center>
        </td>
    </tr>
</table>

<script type='text/javascript'>

    function CopyData(chk, from, to, option) {
        var c = document.getElementById(chk);
        var f = document.getElementById(from);
        var t = document.getElementById(to);

        if (c.checked) {
            t.value = f.value;
        }
        else if (option == 0) {
            t.value = '';
        }
    }
</script>

<script runat="server">
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        If Me.txtArea.Text.Length > 30 Then
            Me.txtArea.Text = Me.txtArea.Text.Substring(0, 30)
        End If
    End Sub
</script>

