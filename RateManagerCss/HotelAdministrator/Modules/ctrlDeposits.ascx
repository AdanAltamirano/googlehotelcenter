<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ctrlDeposits.ascx.vb" Inherits="RateManager.ctrlDeposits" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="uc1" TagName="Date" Src="../../ListDate/Date.ascx" %>
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
		<TD>
			<asp:label id="txtReferencia" runat="server" CssClass="clslabel" Font-Bold="true">Label</asp:label></TD>
	</TR>
	<TR>
		<TD><asp:label id="lblCuenta" CssClass="clslabel" runat="server">No. Cuenta :</asp:label></TD>
		<TD><asp:textbox id="txtCuenta" CssClass="TextBox" runat="server" MaxLength="20"></asp:textbox><asp:requiredfieldvalidator id="rfvCuenta" CssClass="validators" runat="server" ErrorMessage="RequiredFieldValidator"
				Display="Dynamic" ControlToValidate="txtCuenta"></asp:requiredfieldvalidator></TD>
	</TR>
	<TR>
		<TD><asp:label id="lblFecha" CssClass="clslabel" runat="server">Fecha :</asp:label></TD>
		<TD><uc1:date id="Fecha" runat="server"></uc1:date></TD>
	</TR>
	<TR>
		<TD><asp:label id="lblBanco" CssClass="clslabel" runat="server">Banco :</asp:label></TD>
		<TD><asp:textbox id="txtBanco" CssClass="TextBox" runat="server" MaxLength="20"></asp:textbox><asp:requiredfieldvalidator id="rfvBanco" CssClass="validators" runat="server" ErrorMessage="RequiredFieldValidator"
				Display="Dynamic" ControlToValidate="txtBanco"></asp:requiredfieldvalidator></TD>
	</TR>
	<TR>
		<TD><asp:label id="lblAmountDep" CssClass="clslabel" runat="server">Monto</asp:label></TD>
		<TD><asp:textbox id="txtAmountDep" CssClass="TextBox" runat="server" MaxLength="20"></asp:textbox>
			<asp:DropDownList id="ddlMoneda" runat="server"></asp:DropDownList><asp:requiredfieldvalidator id="rfvMonto" CssClass="validators" runat="server" ErrorMessage="RequiredFieldValidator"
				Display="Dynamic" ControlToValidate="txtAmountDep"></asp:requiredfieldvalidator><asp:rangevalidator id="rngvMonto" runat="server" ErrorMessage="RangeValidator" Display="Dynamic" ControlToValidate="txtAmountDep"
				Type="Currency" MaximumValue="99999999" MinimumValue="0"></asp:rangevalidator></TD>
	</TR>
	<TR>
		<TD vAlign="top"><asp:label id="lblObservacion" CssClass="clslabel" runat="server">Observacion :</asp:label></TD>
		<TD><asp:textbox id="txtObservacion" CssClass="TextBox" runat="server" MaxLength="50" Height="88px"
				Width="288px"></asp:textbox></TD>
	</TR>
</TABLE>
<asp:label id="lblMens" CssClass="validators" runat="server" Visible="False">Label</asp:label><asp:label id="lblEmailCli" CssClass="validators" runat="server" Visible="False">Label</asp:label>
