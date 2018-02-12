<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ctrlLogon.ascx.vb" Inherits="RateManager.ctrlLogon" targetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<TABLE id="Table2" cellSpacing="1" cellPadding="1" width="100%" border="0">
	<TR>
		<TD align="center" width="40%" colSpan="2">
			<asp:requiredfieldvalidator id="rfvEmailRequired" CssClass="Validators" runat="server" ErrorMessage="Correo electrónico requerido"
				ControlToValidate="txtEmail" Display="Dynamic" ForeColor=" "></asp:requiredfieldvalidator></TD>
	</TR>
	<TR>
		<TD align="right" width="40%"><asp:label id="lblEmail" runat="server" CssClass="bookingNormalLabel" EnableViewState="False">Correo electrónico</asp:label></TD>
		<TD vAlign="top" align="left" width="50%"><asp:textbox id="txtEmail" CssClass="txtDataBold" runat="server" MaxLength="80"></asp:textbox></TD>
	</TR>
	<TR>
		<TD align="center" colSpan="2">
			<asp:requiredfieldvalidator id="rfvPasswordRequired" Display="Dynamic" ControlToValidate="txtPassword" ErrorMessage="Contraseña requerida"
				runat="server" CssClass="Validators" ForeColor=" "></asp:requiredfieldvalidator></TD>
	</TR>
	<TR>
		<TD align="right"><asp:label id="lblPassword" runat="server" CssClass="bookingNormalLabel" EnableViewState="False">Contraseña</asp:label></TD>
		<TD vAlign="top" align="left"><asp:textbox id="txtPassword" CssClass="TextBox" runat="server" MaxLength="12" TextMode="Password"></asp:textbox></TD>
	</TR>
	<TR>
		<TD align="center" colSpan="2"><asp:customvalidator id="cvInvalid" CssClass="Validators" runat="server" ErrorMessage="Correo electrónico o contraseña invalida"
				Height="8px" Display="Dynamic" ForeColor=" "></asp:customvalidator></TD>
	</TR>
</TABLE>
<%
response.write("<script>document.getElementById('" & txtemail.clientId & "').focus(); </script>")
%>
