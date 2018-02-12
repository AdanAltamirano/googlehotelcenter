<%@ Register TagPrefix="uc1" TagName="ctrlLogon" Src="ctrlLogon.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../Portal/Modules/ctrlHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../Portal/Modules/ctrlFooter.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="logon.aspx.vb" Inherits="RateManager.logon" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>logon</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
		</LINK>
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table2" style="WIDTH: 670px; HEIGHT: 360px" cellSpacing="0" cellPadding="0"
				width="721" border="0">
				<TR>
					<TD vAlign="middle" align="left"></TD>
				</TR>
				<TR>
					<TD vAlign="middle" align="center" height="300"><TABLE id="Table1" width="400" border="0">
							<TR>
								<TD class="Titulo" align="center">
									<asp:label id="lblTituloLog" runat="server" CssClass="TituloForma" EnableViewState="False">Univisit - Ingresar</asp:label></TD>
							</TR>
							<TR>
								<TD align="center">
									<asp:label id="lblDenyAccess1" runat="server" CssClass="Validators" Width="100%" EnableViewState="False">Acceso denegado</asp:label>
									<asp:label id="lblDenyAccess2" runat="server" CssClass="Validators" Width="100%" Height="48px"
										EnableViewState="False">No cuenta con la autorizaci&oacute;n adecuada para explorar la pagina deseada, identifiquese con otra cuenta de usuario o solicite ayuda con el administrador.</asp:label></TD>
							</TR>
							<TR>
								<TD align="center">
									<uc1:ctrllogon id="CtrlLogon1" runat="server"></uc1:ctrllogon></TD>
							</TR>
							<TR>
								<TD align="center">
									<asp:button id="btnSignIn" runat="server" CssClass="Button" Width="69px" Text="Ingresar"></asp:button></TD>
							</TR>
							<TR>
								<TD vAlign="middle" align="center">
									<asp:hyperlink id="HyperLink1" runat="server" CssClass="dgLink" NavigateUrl="passwordReminder.aspx"
										EnableViewState="False"> ¿Olvid&oacute; su contraseña?</asp:hyperlink></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<tr>
					<td id="TDes" runat="server">Si requiere de asistencia, contáctese con nosotros al 
						Departamento de Capacitación Y Soporte:
						[LABELGROUPS]
						<BR>
						<strong>Correo electrónico:</strong> <A href="mailto:atencionaclientes@univisit.com">
							atencionaclientes@univisit.com</A>
						<BR>
						<strong>Desde México:</strong> 01-(612) 123-8740 Ext. 121 y 122
						<BR>
						<strong>Desde Estados Unidos y Canadá:</strong> +52 612-1238740 Ext. 121 y 122
						<BR>
						<strong>Skype:</strong> uvlearning Skype: uvlearning_anabel</td>
				</tr>
				<tr>
					<td id="TDen" runat="server">
						If you requiere assistance please contact support:
						[LABELGROUPS]
						<BR>
						<strong>By email:</strong> <A href="mailto:custsrvc@univisit.com">custsrvc@univisit.com</A>
						<BR>
						<strong>By telephone from Mexico:</strong> +52 (800) 590-4887 or +52 (612) 
						123-8740 ext 121 y 122
						<BR>
						<strong>By telephone from United States and Canada:</strong> 1-800-485-4959
						<BR>
						<strong>By telephone from Other Countries:</strong> 1-559-431-7300
					</td>
				</tr>
			</TABLE>
		</form>
	</body>
</HTML>
