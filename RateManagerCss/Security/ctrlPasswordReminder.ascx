<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ctrlPasswordReminder.ascx.vb" Inherits="RateManager.ctrlPasswordReminder" TargetSchema="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" %>
<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="100%" border="0">
	<TR>
		<TD align="center" width="45%" colSpan="2">
			<asp:requiredfieldvalidator id="rfvEmailRequired" runat="server" CssClass="Validators" ErrorMessage="Correo electrónico es requerido"
				ControlToValidate="txtEmail" Display="Dynamic" ForeColor=" "></asp:requiredfieldvalidator></TD>
	</TR>
	<TR>
		<TD align="center" width="45%" colSpan="2">
			<asp:regularexpressionvalidator id="revEmailExpresion" runat="server" CssClass="Validators" ControlToValidate="txtEmail"
				ErrorMessage="Correo electrónico contiene caracteres invalidos" ValidationExpression="^\w+((-\w+)|(\.\w+))*\@\w+((\.|-)\w+)*\.\w+$"
				Display="Dynamic" ForeColor=" " DESIGNTIMEDRAGDROP="7"></asp:regularexpressionvalidator></TD>
	</TR>
	<TR>
		<TD align="right" width="45%">
			<asp:Label id="lblEmail" CssClass="clsDarkLabel" runat="server" EnableViewState="False">Correo electrónico:</asp:Label></TD>
		<TD align="left" width="55%">
			<asp:textbox id="txtEmail" runat="server" CssClass="TextBox" Width="168px" MaxLength="80"></asp:textbox></TD>
	</TR>
</TABLE>
