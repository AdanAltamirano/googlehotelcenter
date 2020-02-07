<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="EmpresaModulo.ascx.vb"
    Inherits="RateManager.EmpresaModulo" %>

<%@ Register Src="../CtrlPreserveScrolls.ascx" TagName="CtrlPreserveScrolls" TagPrefix="uc2" %>
<%@ Register Src="~/Modulos/CtrlIdioma.ascx" TagPrefix="uc2" TagName="CtrlIdioma" %>

<table cellspacing="0" cellpadding="2" width="700" align="left" border="0">
    <tr>
        <td class="Titulo" align="center" colspan="4">
            <asp:Label ID="lblTitulo" runat="server">Solicitudes de Registro</asp:Label>
        </td>
    </tr>
    <tr>
        <td class="dgItem" align="center" colspan="4">
            <asp:Label ID="lblInformacion" runat="server">Informacion Empresa</asp:Label>
        </td>
    </tr>
    <tr>
        <td align="right" width="25%">
            <asp:Label ID="lblNombre" runat="server" EnableViewState="False">Nombre :</asp:Label>
        </td>
        <td width="25%">
            <asp:TextBox ID="txtNombre" runat="server" MaxLength="80"></asp:TextBox><asp:RequiredFieldValidator
                ID="rfvNombre" runat="server" CssClass="validators" ForeColor=" " ErrorMessage="Nombre de la Empresa es requerido"
                ControlToValidate="txtNombre" Display="Dynamic">*</asp:RequiredFieldValidator>
        </td>
        <td align="right" width="25%">
            <asp:Label ID="lblDomicilio" runat="server" EnableViewState="False">Domicilio :</asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtDomicilio" runat="server" MaxLength="120" Width="150px"></asp:TextBox><asp:RequiredFieldValidator
                ID="rfvDomicilio" runat="server" CssClass="validators" ForeColor=" " ErrorMessage="El Domicilio de la Empresa es requerido"
                ControlToValidate="txtDomicilio" Display="Dynamic">*</asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td align="right" style="height: 21px">
            <asp:Label ID="lblPais" runat="server" EnableViewState="False">Pais :</asp:Label>
        </td>
        <td style="height: 21px">
            <asp:DropDownList ID="cmbPaises" runat="server" Width="150px" AutoPostBack="true"></asp:DropDownList>
            <%-- <select id="ddlCountries" runat="server" style="width:150px;" class="country"></select>
            <input id="selectedCountry" type="hidden" runat="server" value="" />--%>
        </td>
        <td align="right" style="height: 21px">
            <asp:Label ID="lblEstado" runat="server" EnableViewState="False">Estado :</asp:Label>
        </td>
        <td style="height: 21px">

            <asp:DropDownList ID="cmbEstados" runat="server" Width="150px" AutoPostBack="true"></asp:DropDownList>

            <%--<select id="ddlStates" runat="server" style="width:150px;" class="state"></select>
            <input id="selectedState" type="hidden" runat="server" value="" />--%>
        </td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label ID="lblMunicipio" runat="server" EnableViewState="False">Municipio :</asp:Label>
        </td>
        <td>
            <asp:DropDownList ID="cmbMunicipio" runat="server" Width="150px" AutoPostBack="true"></asp:DropDownList>
            <%--<select id="ddlDistricts" runat="server" style="width:150px;" class="district"></select>
            <input id="selectedDistrict" type="hidden" runat="server" value="" />--%>
        </td>
        <td align="right">
            <asp:Label ID="lblCiudad" runat="server" EnableViewState="False">Ciudad :</asp:Label>
        </td>
        <td>

            <asp:DropDownList ID="cmbCiudades" runat="server" Width="150px" AutoPostBack="true"></asp:DropDownList>
            <asp:RequiredFieldValidator ID="rfvCiudad" runat="server" CssClass="validators" ForeColor=" "
                ErrorMessage="Ciudad de la Empresa es requerida" ControlToValidate="txtCiudad"
                Display="Dynamic">*</asp:RequiredFieldValidator>

            <%--<select id="ddlCities" runat="server" style="width:250px;" class="city"></select>
            <asp:CustomValidator Display="Dynamic" ID="rfvCiudad" runat="server" ErrorMessage="Ciudad de la Empresa es requerida" EnableViewState="false" ClientValidationFunction="ValidateCity"></asp:CustomValidator>
            <input id="selectedCity" type="hidden" runat="server" value="" />--%>
        </td>
    </tr>
    <tr>
        <td align="right"></td>
        <td></td>
        <td align="right"></td>
        <td>
            <anthem:TextBox ID="txtCiudad" runat="server" MaxLength="30" Width="150px" AutoUpdateAfterCallBack="false" AutoCallBack="true"></anthem:TextBox>
        </td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label ID="Label2" runat="server" EnableViewState="False">Area :</asp:Label>
        </td>
        <td>
            <%--<select id="ddlAreas" runat="server" style="width:150px;" class="area"></select>
            <input id="selectedArea" type="hidden" runat="server" value="" />--%>
            <asp:DropDownList ID="cmbArea" runat="server" Width="150px" AutoPostBack="true"></asp:DropDownList>
            <asp:RequiredFieldValidator ID="rfvArea" runat="server" CssClass="validators" ForeColor=" "
                ErrorMessage="Area es requerida" ControlToValidate="txtArea" Display="Dynamic">*</asp:RequiredFieldValidator>

        </td>
        <td align="right">
            <asp:Label ID="lblCP" runat="server" EnableViewState="False">Cod. Postal :</asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtCP" runat="server" MaxLength="11" Width="150px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td align="right"></td>
        <td>
            <anthem:TextBox ID="txtArea" runat="server" MaxLength="30" Width="150px"></anthem:TextBox>
        </td>
        <td align="right"></td>
        <td></td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label ID="lblFax" runat="server" CssClass="label" EnableViewState="False">Fax :</asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtFax" runat="server" MaxLength="20" CssClass="textbox" Width="150px"></asp:TextBox>
        </td>
        <td align="right">
            <asp:Label ID="lblTel" runat="server" EnableViewState="False">Telefono :</asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtTel" runat="server" MaxLength="20" Width="150px"></asp:TextBox><asp:RequiredFieldValidator
                ID="rfvTel" runat="server" CssClass="validators" ForeColor=" " ErrorMessage="Telefono de la Empresa es requerido"
                ControlToValidate="txtTel" Display="Dynamic">*</asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label ID="Label8" runat="server" CssClass="label" EnableViewState="False">Pagina Web :</asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtPaginaWeb" runat="server" MaxLength="80" CssClass="textbox" Width="150px"></asp:TextBox>
        </td>
        <td align="right">
            <asp:Label ID="lblGerente" runat="server" CssClass="DarkLabel" EnableViewState="False">Gerente General :</asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtGerente" runat="server" MaxLength="80" CssClass="textbox" Width="150px"></asp:TextBox><asp:RequiredFieldValidator
                ID="rfvGerente" runat="server" CssClass="validators" ForeColor=" " ControlToValidate="txtGerente"
                Display="Dynamic">*</asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label ID="lblCorreoG" runat="server" EnableViewState="False">Correo :</asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtCorreoG" runat="server" MaxLength="80" CssClass="textbox" Width="150px"></asp:TextBox><asp:RequiredFieldValidator
                ID="rfvCorreoG" runat="server" CssClass="validators" ForeColor=" " ControlToValidate="txtCorreoG"
                Display="Dynamic">*</asp:RequiredFieldValidator>
        </td>
        <td align="right">
            <asp:Label ID="lblTelG" runat="server" CssClass="DarkLabel" EnableViewState="False">Telefono  Gerente:</asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtTelG" runat="server" MaxLength="20" CssClass="textbox" Width="150px"></asp:TextBox><asp:RequiredFieldValidator
                ID="rfvTelG" runat="server" CssClass="validators" ForeColor=" " ControlToValidate="txtTelG"
                Display="Dynamic">*</asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label ID="lblCompanyType" runat="server" EnableViewState="False">Tipo de Empresa :</asp:Label>
        </td>
        <td>
            <asp:DropDownList runat="server" ID="ddlCompanyType" Width="150px">
                <asp:ListItem Value="0" Text="Hotel" Selected="True"></asp:ListItem>
                <asp:ListItem Value="1" Text="Casa"></asp:ListItem>
            </asp:DropDownList>
        </td>
        <td>
        </td>
        <td>
        </td>
    </tr>
    <tr>
        <td align="center" colspan="4" class="dgItem">
            <asp:Label ID="lblHotelInformation" runat="server" EnableViewState="False">Informacion de Hotel</asp:Label>
        </td>
    </tr>
    <tr>
        <td align="right" style="height: 21px">
            <asp:Label ID="lblCategoria" runat="server" CssClass="Darklabel" EnableViewState="False">Categoria :</asp:Label>
        </td>
        <td style="height: 21px">
            <asp:DropDownList ID="cmbCategoria" runat="server" Width="154px">
                <asp:ListItem Value="1">1 Estrella</asp:ListItem>
                <asp:ListItem Value="2">2 Estrellas</asp:ListItem>
                <asp:ListItem Value="3">3 Estrellas</asp:ListItem>
                <asp:ListItem Value="4">4 Estrellas</asp:ListItem>
                <asp:ListItem Value="5">5 Estrellas</asp:ListItem>
                <asp:ListItem Value="0">Especial</asp:ListItem>
            </asp:DropDownList>
        </td>
        <td align="right" style="height: 21px">
            <asp:Label ID="lblMoneda" runat="server" CssClass="Darklabel" EnableViewState="False">Moneda :</asp:Label>
        </td>
        <td style="height: 21px">
            <asp:DropDownList ID="cmbMonedas" runat="server" Width="180px">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label ID="lblInventario" runat="server" EnableViewState="False">Num. de Cuartos :</asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtInventario" runat="server" MaxLength="4" Width="150px"></asp:TextBox><asp:RequiredFieldValidator
                ID="rfvInventario" runat="server" CssClass="validators" ForeColor=" " ErrorMessage="Inventario es requerido"
                ControlToValidate="txtInventario" Display="Dynamic">*</asp:RequiredFieldValidator><asp:RangeValidator
                    ID="RangeValidator1" runat="server" CssClass="validators" ForeColor=" " ErrorMessage="Valor de inventario invalido"
                    ControlToValidate="txtInventario" Display="Dynamic" MaximumValue="9999" MinimumValue="0"
                    Type="Integer">*</asp:RangeValidator>
        </td>
        <td align="right">
            <asp:Label ID="lblChain" runat="server" EnableViewState="False">Label</asp:Label>
        </td>
        <td>
            <asp:DropDownList ID="ddlCorporativos" runat="server">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td class="dgItem" align="center" colspan="4">
            <asp:Label ID="lblInformacionFact" runat="server" EnableViewState="False">Informacion Facturacion</asp:Label>
        </td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label ID="lblRazonSocial" runat="server" EnableViewState="False">Razon Social :</asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtRazonSocial" runat="server" MaxLength="80" Width="150px"></asp:TextBox><asp:RequiredFieldValidator
                ID="rfvRazonSocial" runat="server" CssClass="validators" ForeColor=" " ErrorMessage="Razon Social es un campo requerido"
                ControlToValidate="txtRazonSocial" Display="Dynamic">*</asp:RequiredFieldValidator>
        </td>
        <td align="right">
            <asp:Label ID="lblRFC" runat="server" EnableViewState="False">R.F.C. :</asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtRFC" runat="server" MaxLength="15" Width="150px"></asp:TextBox><asp:RequiredFieldValidator
                ID="rfvRFC" runat="server" CssClass="validators" ForeColor=" " ErrorMessage="R.F.C. es un campo requerido"
                ControlToValidate="txtRFC" Display="Dynamic">*</asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td style="height: 35px" align="right">
            <asp:Label ID="lblFiscalDom" runat="server" EnableViewState="False">Domicilio Fiscal :</asp:Label>
        </td>
        <td style="height: 35px">
            <asp:TextBox ID="txtFiscalDom" runat="server" MaxLength="120" CssClass="textbox"
                Width="150px"></asp:TextBox><asp:RequiredFieldValidator ID="rfvDomFiscal" runat="server"
                    CssClass="validators" ForeColor=" " ErrorMessage="Dom. Fiscal es un campo requerido"
                    ControlToValidate="txtFiscalDom" Display="Dynamic">*</asp:RequiredFieldValidator>
        </td>
        <td style="height: 35px" align="right">
            <asp:Label ID="lblFiscalEstado" runat="server" EnableViewState="False">Estado :</asp:Label>
        </td>
        <td style="height: 35px">

            <asp:DropDownList ID="cmbFiscalEstados" runat="server" Width="150px" AutoPostBack="true">
            </asp:DropDownList>


        </td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label ID="lblMunicipioFiscal" runat="server" EnableViewState="False">Municipio :</asp:Label>
        </td>
        <td>

            <asp:DropDownList ID="cmbMunicipioFiscal" runat="server" Width="150px" AutoPostBack="true">
            </asp:DropDownList>

        </td>
        <td align="right">
            <asp:Label ID="lblFiscalCiudad" runat="server" EnableViewState="False">Ciudad :</asp:Label>
        </td>
        <td>
            <asp:DropDownList ID="cmbFiscalCiudades" runat="server" Width="150px" AutoPostBack="true">
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="rfvFiscalCiudad" runat="server" Display="Dynamic"
                ControlToValidate="txtCiudadFiscal" ErrorMessage="Ciudad de Facturacion es un campo requerido"
                ForeColor=" " CssClass="validators">*</asp:RequiredFieldValidator>

        </td>
    </tr>
    <tr>
        <td align="right"></td>
        <td></td>
        <td align="right"></td>
        <td>
            <anthem:TextBox ID="txtCiudadFiscal" runat="server" MaxLength="30" Width="150px"></anthem:TextBox>
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
        <td align="right">
            <asp:Label ID="lblContactoNombre2" runat="server" EnableViewState="False">Contacto 2:</asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtContacto2" runat="server" MaxLength="80" CssClass="textbox" Width="150px"></asp:TextBox>
        </td>
        <td align="right">
            <asp:Label ID="lblContactoPuesto2" runat="server" EnableViewState="False">Puesto :</asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtPuesto2" runat="server" MaxLength="50" CssClass="textbox" Width="150px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label ID="lblTel2" runat="server" EnableViewState="False">Telefono :</asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtTel2" runat="server" MaxLength="20" CssClass="textbox" Width="150px"></asp:TextBox>
        </td>
        <td align="right">
            <asp:Label ID="lblCorreo2" runat="server" Width="100%" EnableViewState="False">Correo :</asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtCorreo2" runat="server" MaxLength="80" CssClass="textbox" Width="150px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label ID="lblContactoNombre3" runat="server" EnableViewState="False">Contacto 3:</asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtContacto3" runat="server" MaxLength="80" CssClass="textbox" Width="150px"></asp:TextBox>
        </td>
        <td align="right">
            <asp:Label ID="lblContactoPuesto3" runat="server" EnableViewState="False">Puesto :</asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtPuesto3" runat="server" MaxLength="50" CssClass="textbox" Width="150px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label ID="lblTel3" runat="server" EnableViewState="False">Telefono :</asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtTel3" runat="server" MaxLength="20" CssClass="textbox" Width="150px"></asp:TextBox>
        </td>
        <td align="right">
            <asp:Label ID="lblCorreo3" runat="server" Width="100%" EnableViewState="False">Correo :</asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtCorreo3" runat="server" MaxLength="80" CssClass="textbox" Width="150px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label ID="lblAdmin" runat="server" CssClass="Label" Visible="False">Administrador:</asp:Label>
        </td>
        <td>
            <asp:DropDownList ID="cmbAdmin" runat="server" Visible="False">
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="rfv_Admin" runat="server" CssClass="validators" ForeColor=" "
                ControlToValidate="cmbAdmin" Display="Dynamic" InitialValue="0" Enabled="false">*</asp:RequiredFieldValidator>
        </td>
        <td colspan="2"></td>
    </tr>
    <tr>
        <td align="right"></td>
        <td></td>
        <td></td>
        <td></td>
    </tr>
</table>
<asp:Label ID="lblError" runat="server"></asp:Label>

<uc2:CtrlPreserveScrolls ID="CtrlPreserveScrolls1" runat="server" />



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

