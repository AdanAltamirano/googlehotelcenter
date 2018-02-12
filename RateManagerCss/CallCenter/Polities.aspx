<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../Portal/Modules/ctrlFooter.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrMenu" Src="../Portal/Modules/CtrMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../Portal/Modules/ctrlHeader.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Polities.aspx.vb" Inherits="RateManager.Polities" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>Polities</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
		</link>

    <script type="text/javascript">
        function ShowDataCC( show ) {
            var cc1 = document.getElementById('txtNumber');
            var cc2 = document.getElementById('txtVerify');
            var cc3 = document.getElementById('txtHolder');
            var cc4 = document.getElementById('txtMonth');
            var cc5 = document.getElementById('txtYear');
            var ddl = document.getElementById('ddlTarjetas');

            if (show == false) {
                if (cc1) cc1.value = "4242424242424242"
                if (cc2) cc2.value = "123"
                if (cc3) cc3.value = "x"
                if (cc4) cc4.value = "12"
                if (cc5) cc5.value = "2000"                
            }
            else {
                if (ddl) ddl.selectedIndex = 0;

                if (cc1) cc1.value = ""
                if (cc2) cc2.value = ""
                if (cc3) cc3.value = ""
                if (cc4) cc4.value = ""
                if (cc5) cc5.value = ""
            }
        }

        function FireDeposito(pnlTc, pnlDep, rb, show) {
	        var etc = document.getElementById(pnlTc);
	        var edp = document.getElementById(pnlDep);
	        var erb = document.getElementById(rb);

	        if (etc) { etc.style.display = erb.checked ? '' : 'none'; }
	        if (edp) { edp.style.display = !erb.checked ? '' : 'none'; }
	        ShowDataCC(show);
	    }

	    function FireValidaFechaDeposito(rb, msg) {
	        var e = document.getElementById(rb);
	        if (e && e.checked) {
	            return confirm(msg);
	        }	        
	    }
	    
    </script>

</head>
<body bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <asp:Panel ID="pnlPolities" runat =server >
    <table id="bookingcontainer" cellspacing="0" cellpadding="2" width="650" border="0">
        <tr>
            <td>
                <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                    <tr>
                        <td>
                            <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                                <tr>
                                    <td class="dgitem" style="height: 12px" colspan="4">
                                        <asp:Label ID="lblDatosReserva" runat="server" CssClass="bookingnormallabel">Datos de la reservacion</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:Label ID="lblCheckIn" runat="server" CssClass="clslabel" EnableViewState="False">CheckIn</asp:Label><asp:Label
                                            ID="lblFechaInicio" runat="server" CssClass="bookingNormalLabel" EnableViewState="False"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="lblCheckOut" runat="server" CssClass="clslabel" EnableViewState="False">CheckOut</asp:Label><asp:Label
                                            ID="lblFechasalida" runat="server" CssClass="bookingNormalLabel" EnableViewState="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblCuartos" runat="server" CssClass="clslabel" EnableViewState="False">Habitaciones:</asp:Label>
                                    </td>
                                    <td id="tddetalles" colspan="3" runat="server">
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblcodigotarifa" runat="server" CssClass="clslabel">RateCode:</asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:Label ID="lblratecode" runat="server" CssClass="bookingNormalLabel"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblConvenio" runat="server" CssClass="clslabel">Convenio:</asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:Label ID="lblConvenioEmpresa" runat="server" CssClass="bookingNormalLabel"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblCodigo" runat="server" CssClass="clslabel">Corporativo/Código Promoción:</asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:Label ID="lblAccessCode" runat="server" CssClass="bookingNormalLabel"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="dgitem" colspan="4">
                                        <asp:Label ID="lblRateDetails" runat="server">Detalle de tarifas</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4"> <asp:datagrid id="dgDetail" runat="server" Width="90%" AutoGenerateColumns="False" GridLines="None"
															CellPadding="0" BorderWidth="0px" ShowHeader="False"><ITEMSTYLE HorizontalAlign="Right"></ITEMSTYLE>
															<COLUMNS>
																<ASP:BOUNDCOLUMN></ASP:BOUNDCOLUMN>
																<ASP:BOUNDCOLUMN>
																	<ITEMSTYLE HorizontalAlign="Right"></ITEMSTYLE>
																</ASP:BOUNDCOLUMN>
															</COLUMNS>
														</asp:datagrid></td>
                                </tr>                               
                              
                                <tr>
                                    <td colspan="4">
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 13px; padding-right:4px;" align="right" width="50%" colspan="2">
                                        <asp:Label ID="lblTotalRate" runat="server" CssClass="clslabel" EnableViewState="False"></asp:Label>
                                    </td>
                                    <td style="height: 13px" align="left" colspan="2">
                                        <asp:Label ID="lbTotalRate" runat="server" CssClass="clslabel" EnableViewState="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="2" style="padding-right:4px;">
                                        <asp:Label ID="lblTotalExtra" runat="server" CssClass="clslabel" EnableViewState="False"></asp:Label>
                                    </td>
                                    <td align="left" colspan="2">
                                        <asp:Label ID="lbTotalExtra" runat="server" CssClass="clslabel" EnableViewState="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="2" style="padding-right:4px;">
                                        <asp:Label ID="lblTotalTaxes" runat="server" CssClass="clslabel" EnableViewState="False"></asp:Label>
                                    </td>
                                    <td align="left" colspan="2">
                                        <asp:Label ID="lbTotalTaxes" runat="server" CssClass="clslabel" EnableViewState="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="2" style="padding-right:4px;">
                                        <asp:Label ID="lblTotal" runat="server" CssClass="bookingNormalLabel" EnableViewState="False"></asp:Label>
                                    </td>
                                    <td align="left" colspan="2">
                                        <asp:Label ID="lbTotal" runat="server" CssClass="clslabel" EnableViewState="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr><td align="right" colspan="2" style="padding-right:4px;">
                                    <asp:Label ID="lblEdadMenor"  CssClass= clsLabel runat="server" Text="Edad menores"></asp:Label> </td><td align="left" colspan="2">
                                        <asp:Label ID="txtEdadMenor" CssClass="clsLabel" runat="server" Text="1,2"></asp:Label>
                                </td></tr>
                                <tr>
                                    <td class="dgitem" colspan="4">
                                        <asp:Label ID="lblInformes" runat="server" CssClass="bookingnormallabel">Politicas de la reservacion</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lblPoliticas" runat="server" CssClass="clslabel" EnableViewState="False">Politicas:</asp:Label>
                                    </td>
                                    <td align="left" colspan="3">
                                        <asp:Label ID="lblPoliticies" runat="server" CssClass="bookingNormalLabel" EnableViewState="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lblPoliticasCancelacion" runat="server" CssClass="clslabel" EnableViewState="False">Politicas de cancelacion:</asp:Label>
                                    </td>
                                    <td align="left" colspan="3">
                                        <asp:Label ID="lblCancelationPoliticies" runat="server" CssClass="bookingNormalLabel"
                                            EnableViewState="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lblPoliticasGarantia" runat="server" CssClass="clslabel" EnableViewState="False">Politicas de garantia</asp:Label>
                                    </td>
                                    <td align="left" colspan="3">
                                        <asp:Label ID="lblGuarantyPoliticies" runat="server" CssClass="bookingNormalLabel"
                                            EnableViewState="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lblpoliticastarjeta" runat="server" CssClass="clslabel" EnableViewState="False">Politicas de tarjeta de credito</asp:Label>
                                    </td>
                                    <td align="left" colspan="3">
                                        <asp:Label ID="lblCreditCardPoliticies" runat="server" CssClass="bookingNormalLabel"
                                            EnableViewState="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lblCargosExtras" runat="server" CssClass="clslabel" EnableViewState="False">Cargos Extras</asp:Label>
                                    </td>
                                    <td align="left" colspan="3">
                                        <asp:Label ID="lblExtraCharges" runat="server" CssClass="bookingNormalLabel" EnableViewState="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lblMinimodiasCancelacion" runat="server" CssClass="clslabel" EnableViewState="False">Minimo dias para cancelar</asp:Label>
                                    </td>
                                    <td align="left" colspan="3">
                                        <asp:Label ID="lblMinDaysToCancel" runat="server" CssClass="bookingNormalLabel" EnableViewState="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lblExtrapersons" runat="server" CssClass="clslabel" EnableViewState="False">Extra Persons</asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblextrapeople" runat="server" CssClass="bookingNormalLabel" EnableViewState="False"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblEdadMaximaninio" runat="server" CssClass="clslabel">Edad maxima de niño</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblMaxAgeOfChildren" runat="server" CssClass="bookingNormalLabel"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lblMaximoAdultos" runat="server" CssClass="clslabel" EnableViewState="False">Maximo Adultos</asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblMaxAdults" runat="server" CssClass="bookingNormalLabel" EnableViewState="False"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblMaximoNinios" runat="server" CssClass="clslabel" EnableViewState="False">Maximo niños</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblmaxninios" runat="server" CssClass="bookingNormalLabel" EnableViewState="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lblmaximodias" runat="server" CssClass="clslabel" EnableViewState="False">Maximo de dias</asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblMaxdays" runat="server" CssClass="bookingNormalLabel" EnableViewState="False"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblMinimodias" runat="server" CssClass="clslabel" EnableViewState="False">Minimo de dias</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblMindays" runat="server" CssClass="bookingNormalLabel"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lblenpeticion" runat="server" CssClass="clslabel" EnableViewState="False">On request</asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblOnRequest" runat="server" CssClass="bookingNormalLabel" EnableViewState="False"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblGarantiaDep" runat="server" CssClass="clslabel" EnableViewState="False">Garantia o deposito</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblGuardep" runat="server" CssClass="bookingNormalLabel" EnableViewState="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:Literal ID="lblNetRatePol" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="dgitem" style="height: 13px" colspan="4">
                                        <asp:Label ID="lbltitlePet" runat="server" CssClass="bookingnormallabel" EnableViewState="False">Petición especial de habitación</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <table id="divpeticiones" width="100%" runat="server">
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="dgitem" colspan="4">
                                        <asp:Label ID="lblDatosviajero" runat="server" CssClass="bookingnormallabel" EnableViewState="False">Datos del viajero principal</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lblName" runat="server" CssClass="clslabel" EnableViewState="False">Nombre</asp:Label>
                                    </td>
                                    <td align="left" colspan="3">
                                        <p>
                                            <asp:TextBox ID="txtNombre" runat="server" CssClass="textbox" Columns="50" MaxLength="80"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvName" runat="server" CssClass="Validators" Display="Dynamic"
                                                ErrorMessage="*" ControlToValidate="txtNombre"></asp:RequiredFieldValidator></p>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lblLastName" runat="server" CssClass="clslabel" EnableViewState="False">Apellido</asp:Label>
                                    </td>
                                    <td align="left" colspan="3">
                                        <p>
                                            <asp:TextBox ID="txtApellido" runat="server" CssClass="textbox" Columns="50" MaxLength="80"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvApellido" runat="server" CssClass="Validators"
                                                Display="Dynamic" ErrorMessage="*" ControlToValidate="txtApellido"></asp:RequiredFieldValidator></p>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblHomePhone" runat="server" CssClass="clslabel" EnableViewState="False">Telefono de casa</asp:Label>
                                    </td>
                                    <td align="left" colspan="3">
                                        <asp:TextBox ID="txtHomePhone" runat="server" CssClass="textbox" Columns="30" MaxLength="28"></asp:TextBox><asp:RequiredFieldValidator
                                            ID="RfvWorkPhone" runat="server" CssClass="Validators" ControlToValidate="txtHomePhone"
                                            ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblWorkHome" runat="server" CssClass="clslabel" EnableViewState="False">Telefono de trabajo</asp:Label>
                                    </td>
                                    <td align="left" colspan="3">
                                        <asp:TextBox ID="txtWorkHome" runat="server" CssClass="textbox" Columns="30" MaxLength="28"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblAddress" runat="server" CssClass="clslabel" EnableViewState="False">Direccion</asp:Label>
                                    </td>
                                    <td align="left" colspan="3">
                                        <asp:TextBox ID="txtAddress" runat="server" CssClass="textbox" Columns="40" MaxLength="50"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblCiudad" runat="server" CssClass="clslabel" EnableViewState="False">Ciudad</asp:Label>
                                    </td>
                                    <td align="left" colspan="3">
                                        <asp:TextBox ID="txtCiudad" runat="server" CssClass="textbox" Columns="40" MaxLength="30"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblEstado" runat="server" CssClass="clslabel" EnableViewState="False">Estado</asp:Label>
                                    </td>
                                    <td align="left" colspan="3">
                                        <asp:TextBox ID="txtEstado" runat="server" CssClass="textbox" Columns="40" MaxLength="30"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblemail" runat="server" CssClass="clslabel" EnableViewState="False">Correo Electronico</asp:Label>
                                    </td>
                                    <td align="left" colspan="3">
                                        <p>
                                            <asp:TextBox ID="txtEmail" runat="server" CssClass="textbox" Columns="50" MaxLength="80"></asp:TextBox><asp:RequiredFieldValidator
                                                ID="rfvMail" runat="server" CssClass="Validators" ControlToValidate="txtEmail"
                                                ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                                                    ID="REMail" runat="server" CssClass="Validators" ControlToValidate="txtEmail"
                                                    ErrorMessage="Invalid Mail" Display="Dynamic" ValidationExpression="^\w+((-\w+)|(\.\w+))*\@\w+((\.|-)\w+)*\.\w+$"></asp:RegularExpressionValidator></p>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblPortal" runat="server" CssClass="clslabel" EnableViewState="False">Portal Original:</asp:Label>
                                    </td>
                                    <td align="left" colspan="3">
                                        <asp:DropDownList ID="ddlPortal" runat="server" Width="272px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="4">
                                        <asp:Label ID="lblError" runat="server" CssClass="Validators" Visible="False">Email o nombre es requerido</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="dgitem" colspan="4">
                                        <asp:Label ID="lblPago" runat="server" CssClass="bookingnormallabel" EnableViewState="False" Text= "Informacion del Pago" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style= "padding-left: 4px;">
                                        <asp:RadioButton ID="rbTc" Checked="true" runat="server" Text="Con Tarjeta de Crédito"
                                            GroupName="x" />
                                    </td>
                                    <td colspan="3">
                                    </td>
                                </tr>
                                <tr>
                                    <td style= "padding-left: 4px;">
                                        <asp:RadioButton ID="rbDeposito" runat="server" Text="Con deposito Bancario" GroupName="x" />
                                    </td>
                                    <td colspan="3">
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td colspan="3">
                                        <asp:Panel ID="pnlCredito" runat="server">
                                            <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                                                <tr>
                                                    <td class="dgitem" colspan="4">
                                                        <asp:Label ID="lbldatostarjeta" runat="server" CssClass="bookingnormallabel" EnableViewState="False">Datos de la tarjeta de credito</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblType" runat="server" CssClass="clslabel" EnableViewState="False">Tipo</asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:DropDownList ID="ddlTarjetas" runat="server">
                                                            <asp:ListItem Value="-1">-- Select credit card --</asp:ListItem>
                                                            <asp:ListItem Value="CA">Master Card</asp:ListItem>
                                                            <asp:ListItem Value="VI">Visa</asp:ListItem>
                                                            <asp:ListItem Value="AX">American Express</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblNumber" runat="server" CssClass="clslabel" EnableViewState="False">Numero de tarjeta</asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:TextBox ID="txtNumber" runat="server" CssClass="textbox" Columns="16" MaxLength="16"></asp:TextBox><asp:RequiredFieldValidator
                                                            ID="RfvCardNumber" runat="server" CssClass="Validators" ControlToValidate="txtNumber"
                                                            ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                                                                ID="RevCardNumber" runat="server" CssClass="Validators" ControlToValidate="txtNumber"
                                                                ErrorMessage="Invalid Card Number" Display="Dynamic" ValidationExpression="(^\d{15,16})|(^\d{13})"></asp:RegularExpressionValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblVerify" runat="server" CssClass="clslabel" EnableViewState="False">Numero de verificacion</asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:TextBox ID="txtVerify" runat="server" CssClass="textbox" Columns="4" MaxLength="4"></asp:TextBox><asp:RequiredFieldValidator
                                                            ID="RfvNumberVer" runat="server" CssClass="Validators" ControlToValidate="txtVerify"
                                                            ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblHolder" runat="server" CssClass="clslabel" EnableViewState="False">Holder</asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:TextBox ID="txtHolder" runat="server" CssClass="textbox" Columns="40" MaxLength="80"></asp:TextBox><asp:RequiredFieldValidator
                                                            ID="RfvNombre" runat="server" CssClass="Validators" ControlToValidate="txtHolder"
                                                            ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblExpirationDate" runat="server" CssClass="clslabel" EnableViewState="False">Expiracion (MM/YYYY)</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtMonth" runat="server" CssClass="textbox" Columns="2" MaxLength="2"></asp:TextBox><asp:RequiredFieldValidator
                                                            ID="RfvMonth" runat="server" CssClass="Validators" ControlToValidate="txtMonth"
                                                            ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator><asp:RangeValidator
                                                                ID="RVMonth" runat="server" CssClass="Validators" ControlToValidate="txtMonth"
                                                                ErrorMessage="Range Invalid (MM)" Display="Dynamic" MinimumValue="01" MaximumValue="12"></asp:RangeValidator>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtYear" runat="server" CssClass="textbox" Columns="4" MaxLength="4"></asp:TextBox><asp:RequiredFieldValidator
                                                            ID="RfvYear" runat="server" CssClass="Validators" ControlToValidate="txtYear"
                                                            ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator><asp:Label ID="lblExpirationError"
                                                                runat="server" CssClass="Validators" Visible="False">Invalid Expiration Date</asp:Label><asp:RegularExpressionValidator
                                                                    ID="REVYear" runat="server" CssClass="Validators" ControlToValidate="txtYear"
                                                                    ErrorMessage="Year Invalid(Only numbers please(YYYY))" Display="Dynamic" ValidationExpression="^\d{4}"></asp:RegularExpressionValidator>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlDeposito" runat="server">
                                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                <tr>
                                                    <td class="dgitem" colspan="2">
                                                        <asp:Label ID="lblDatosDep" runat="server" CssClass="bookingnormallabel" EnableViewState="False">Datos del Depósito Bancario</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblDeposito" runat="server" Text="Monto:"></asp:Label>
                                                        <asp:Label ID="lblMontoDeposito" runat="server" Text="1.00 MXN"></asp:Label>
                                                    </td>
                                                    <td>
                                                        
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblDepositInfo" runat="server" Text="pago a Univisit"></asp:Label>
                                                    </td>
                                                    <td>
                                                        
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlPagoLinea" runat="server">
                                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                <tr>
                                                    <td class="dgitem">
                                                        <asp:Label ID="lblTitleLine" runat="server" CssClass="bookingnormallabel" EnableViewState="False">Pago Linea</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr><td>
                                                 <asp:Label ID="lblPrepagoLine" runat="server" Text="Prepago 1.00 MXN"></asp:Label>
                                                <br />
                                                </td></tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblMsgLinea" runat="server" Text="Automáticamente será re direccionado a una página donde podrá finalizar su reservación al efectuar su pago en línea, este es una vía fácil y segura. "></asp:Label>
                                                    </td>                                                    
                                                </tr>                                                
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr >
                                    <td style="padding-top:6px;">
                                    </td>
                                    <td colspan="3">
                                    </td>
                                </tr>
                                <tr style="padding-top:6px;">
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnReservar" runat="server" CssClass="button" EnableViewState="False"
                                            Text="Reservar"></asp:Button>
                                            <asp:Button ID="cmdCancelaReserva" runat="server" CssClass="button" EnableViewState="False"
                                            Text="Cancelar reservacion" CausesValidation="False" Visible =false style=" margin-right: 6px;"  ></asp:Button>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnCancel" runat="server" CssClass="button" EnableViewState="False"
                                            Text="Cancelar" CausesValidation="False"></asp:Button>
                                    </td>
                                     <td>
                                     
                                        <asp:Button ID="cmdPayment" runat="server" CssClass="button" EnableViewState="False"
                                            Text="Payment" style="display:none" CausesValidation=false ></asp:Button>
                                    </td>
                                    <td>
                                                                           
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="4">
                                        <asp:Label ID="lblErrorReserva" runat="server" CssClass="Validators" Visible="False">No se pudo hacer la reservacion</asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </asp:Panel>
    <IFRAME id="frmOnLine" style="BORDER-RIGHT: 0px solid; BORDER-TOP: 0px solid; BORDER-LEFT: 0px solid; WIDTH: 100%; BORDER-BOTTOM: 0px solid; HEIGHT: 100%; BACKGROUND-COLOR: transparent; Z-INDEX: 999; LEFT: 0px; top: 0px; POSITION: absolute; display: none; "
										name="frmOnLinea" src="OnlineConfirmed.aspx" frameBorder="0" width="100%" height="100" runat="server" ></IFRAME>
    </form>
</body>
</html>
