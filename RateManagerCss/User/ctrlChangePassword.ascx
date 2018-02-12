<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ctrlChangePassword.ascx.vb" Inherits="RateManager.ctrlChangePassword" targetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<TABLE id="Table1" cellSpacing="1" cellPadding="1" border="0" width="100%">
	<TR>
		<TD class="TituloTabla" colSpan="2" align="center">
			<asp:Label id="lblMsg" runat="server" CssClass="clsHelpLabel" EnableViewState="False"> Para cambiar por la nueva contraseña, llene la siguiente información</asp:Label></TD>
	</TR>
	<TR>
		<TD align="right" width="50%">
			<asp:Label id="lblEmail" runat="server" CssClass="clsDarkLabel" EnableViewState="False">Correo electrónico:</asp:Label></TD>
		<TD align="left" width="50%">
			<asp:Label id="lblEmailUser" runat="server" CssClass="clsDarkLabel" EnableViewState="False">Email de usuario</asp:Label></TD>
	</TR>
	<TR>
		<TD align="center" colSpan="2">
			<asp:RequiredFieldValidator id="rfvPasswordRequired" CssClass="Validators" runat="server" Display="Dynamic"
				ErrorMessage="Contraseña anterior es requerida" ControlToValidate="txtPassword" ForeColor=" "></asp:RequiredFieldValidator></TD>
	</TR>
	<TR>
		<TD align="right">
			<asp:Label id="lbloldpass" runat="server" CssClass="clsDarkLabel" EnableViewState="False">Contraseña anterior:</asp:Label></TD>
		<TD align="left">
			<asp:TextBox id="txtPassword" runat="server" CssClass="TextBox" MaxLength="12" TextMode="Password"></asp:TextBox></TD>
	</TR>
	<TR>
		<TD align="center" colSpan="2">
			<asp:CustomValidator id="cvPassValid" runat="server" CssClass="Validators" Display="Dynamic" ErrorMessage="Contraseña anterior no coincide con la cantraseña almacenada"
				ForeColor=" "></asp:CustomValidator></TD>
	</TR>
	<TR>
		<TD align="center" colSpan="2">
			<asp:CompareValidator id="cvPasswordConfirmCompare" runat="server" CssClass="Validators" Display="Dynamic"
				ErrorMessage="Nueva contraseña y confirmación deben de coincidir" ControlToValidate="txtPasswordConfirm"
				ControlToCompare="txtNewPassword" ForeColor=" "></asp:CompareValidator></TD>
	</TR>
	<TR>
		<TD align="center" colSpan="2">
			<asp:RequiredFieldValidator id="rfvNewPass" runat="server" CssClass="Validators" Display="Dynamic" ErrorMessage="Nueva contraseña es requerida"
				ControlToValidate="txtNewPassword" ForeColor=" "></asp:RequiredFieldValidator></TD>
	</TR>
	<TR>
		<TD align="right">
			<asp:Label id="lblnuevopass" runat="server" CssClass="clsDarkLabel" EnableViewState="False">Contraseña Nueva:</asp:Label></TD>
		<TD align="left">
			<asp:TextBox id="txtNewPassword" runat="server" CssClass="TextBox" MaxLength="12" TextMode="Password"></asp:TextBox></TD>
	</TR>
	<TR>
		<TD align="center" colSpan="2">
			<asp:RequiredFieldValidator id="rfvPasswordConfirmRequired" runat="server" CssClass="Validators" Display="Dynamic"
				ErrorMessage="Debe confirmar la nueva contraseña" ControlToValidate="txtPasswordConfirm" ForeColor=" "></asp:RequiredFieldValidator></TD>
	</TR>
	<TR>
		<TD align="right">
			<asp:Label id="lblPasswordConfirm" runat="server" CssClass="clsDarkLabel" EnableViewState="False">Confirmar contraseña:</asp:Label></TD>
		<TD align="left">
			<asp:TextBox id="txtPasswordConfirm" runat="server" CssClass="TextBox" TextMode="Password" MaxLength="12"></asp:TextBox></TD>
	</TR>
</TABLE>
