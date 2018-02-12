<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ctrlPayments.ascx.vb" Inherits="RateManager.ctrlPayments" %>
<TABLE id="Table1" cellSpacing="0" cellPadding="2" border="0">
	<TR>
		<TD><asp:label id="lblReservacion" CssClass="clslabel" runat="server">Reservacion :</asp:label></TD>
		<TD><asp:label id="txtReservacion" CssClass="clslabel" runat="server" Font-Bold="true">Label</asp:label></TD>
	</TR>
	<TR>
		<TD><asp:label id="lblFechaRes" CssClass="clslabel" runat="server">Fecha</asp:label></TD>
		<TD><asp:label id="txtFechaRes" CssClass="clslabel" runat="server" Font-Bold="true">Fecha</asp:label></TD>
	</TR>
	<TR>
		<TD><asp:label id="lblCliente" CssClass="clslabel" runat="server">Cliente</asp:label></TD>
		<TD><asp:label id="txtCliente" CssClass="clslabel" runat="server" Font-Bold="true">Label</asp:label></TD>
	</TR>
	<TR>
		<TD><asp:label id="lblHotel" CssClass="clslabel" runat="server">Hotel</asp:label></TD>
		<TD><asp:label id="txtHotel" CssClass="clslabel" runat="server" Font-Bold="true">Label</asp:label></TD>
	</TR>
	<TR>
		<TD><asp:label id="lblCiudad" CssClass="clslabel" runat="server">Ciudad</asp:label></TD>
		<TD><asp:label id="txtCiudad" CssClass="clslabel" runat="server" Font-Bold="true">Label</asp:label></TD>
	</TR>
	<TR>
		<TD><asp:label id="lblCheckin" CssClass="clslabel" runat="server">Checkin</asp:label></TD>
		<TD><asp:label id="txtCheckin" CssClass="clslabel" runat="server" Font-Bold="true">Label</asp:label></TD>
	</TR>
	<TR>
		<TD><asp:label id="lblCheckout" CssClass="clslabel" runat="server">Checkout</asp:label></TD>
		<TD><asp:label id="txtCheckout" CssClass="clslabel" runat="server" Font-Bold="true">Label</asp:label></TD>
	</TR>
	<TR>
		<TD style="HEIGHT: 24px"><asp:label id="lblAmount" CssClass="clslabel" runat="server">Monto</asp:label></TD>
		<TD style="HEIGHT: 24px"><asp:label id="txtAmount" CssClass="clslabel" runat="server" Font-Bold="true">Label</asp:label></TD>
	</TR>
	<TR>
		<TD><asp:label id="lblReferencia" CssClass="clslabel" runat="server">Referencia :</asp:label></TD>
		<TD><asp:textbox id="txtReferencia" CssClass="TextBox" runat="server" MaxLength="50"></asp:textbox></TD>
	</TR>
	<TR>
		<TD><asp:label id="lblCuenta" CssClass="clslabel" runat="server">No. de Autorizacion :</asp:label></TD>
		<TD><asp:textbox id="txtCuenta" CssClass="TextBox" runat="server" MaxLength="20"></asp:textbox></TD>
	</TR>
	<TR>
		<TD><asp:label id="lblFolio" CssClass="clslabel" runat="server">Folio :</asp:label></TD>
		<TD><asp:textbox id="txtFolio" CssClass="TextBox" runat="server" MaxLength="20"></asp:textbox></TD>
	</TR>
	<TR>
		<TD><asp:label id="lblMetodo" CssClass="clslabel" runat="server">Forma de pago :</asp:label></TD>
		<TD><asp:DropDownList id="lstMetodo" runat="server"></asp:DropDownList>
            <asp:rangevalidator id="rvLstMetodo" runat="server"  CssClass ="validators"
                ErrorMessage="*" Display="Dynamic" ControlToValidate="lstMetodo"
				Type="Currency" MaximumValue="5" MinimumValue="1"></asp:rangevalidator></TD>
	</TR>
	<TR>
		<TD><asp:label id="lblAmountDep" CssClass="clslabel" runat="server">Monto :</asp:label></TD>
		<TD><asp:textbox id="txtAmountDep" CssClass="TextBox" runat="server" MaxLength="20"></asp:textbox>
			<asp:DropDownList id="ddlMoneda" runat="server"></asp:DropDownList><asp:requiredfieldvalidator id="rfvMonto" CssClass="validators" runat="server" ErrorMessage="RequiredFieldValidator"
				Display="Dynamic" ControlToValidate="txtAmountDep"></asp:requiredfieldvalidator>
				<asp:rangevalidator id="rngvMonto" runat="server" ErrorMessage="RangeValidator" Display="Dynamic" ControlToValidate="txtAmountDep"
				Type="Currency" MaximumValue="99999999" MinimumValue="0"  CssClass="validators"></asp:rangevalidator></TD>
	</TR>
	<TR>
		<TD vAlign="top"><asp:label id="lblObservacion" CssClass="clslabel" runat="server" visible=false>Observacion :</asp:label></TD>
		<TD><asp:textbox id="txtObservacion" CssClass="TextBox" runat="server" MaxLength="50" Height="88px"
				Width="288px" visible=false></asp:textbox ></TD>
	</TR>
</TABLE>
<asp:label id="lblMens" CssClass="validators" runat="server" Visible="False">Label</asp:label><asp:label id="lblEmailCli" CssClass="validators" runat="server" Visible="False">Label</asp:label>