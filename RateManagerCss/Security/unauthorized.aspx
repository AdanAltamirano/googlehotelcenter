<%@ Page Language="vb" AutoEventWireup="false" Codebehind="unauthorized.aspx.vb" Inherits="RateManager.unauthorized" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>unauthorized</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
		</LINK>
	</HEAD>
	<body MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE style="WIDTH: 650px; HEIGHT: 24px" cellSpacing="1" cellPadding="1" width="721" align="center"
				border="0">
				<TR>
				</TR>
				<TR>
					<TD colSpan="4" height="6"></TD>
				</TR>
				<TR>
					<TD colSpan="4" height="6">&nbsp;</TD>
				</TR>
				<TR>
					<TD colSpan="4" height="10"></TD>
				</TR>
				<TR>
					<TD width="20%"></TD>
					<TD align="center" width="60%">
						<asp:Panel id="panelPswChange" runat="server" Width="100%">
							<TABLE id="bookingcontainer" cellSpacing="1" cellPadding="1" width="100%" border="0">
								<TR>
									<TD class="TituloForma" align="center" colSpan="3" height="21">
										<DIV align="center"><FONT face="Arial, Helvetica, sans-serif" color="#ffffff" size="3"><SPAN id="Label1" style="FONT-WEIGHT: bold">
													<asp:Label id="Label3" runat="server" EnableViewState="False">Error de autorización</asp:Label></SPAN></FONT></DIV>
									</TD>
								</TR>
								<TR>
									<TD></TD>
									<TD align="center">&nbsp;</TD>
									<TD></TD>
								</TR>
								<TR>
									<TD height="50"></TD>
									<TD align="center" height="50">
										<asp:Label id="Label4" runat="server" CssClass="darklabel" EnableViewState="False">No cuenta con la autorización adecuada para explorar la pagina deseada, identifiquese con otra cuenta de usuario o solicite ayuda con el administrador</asp:Label>
										<asp:LinkButton id="lbLogon" runat="server" CssClass="labelBoldLink">Iniciar sesión con otra cuenta</asp:LinkButton></TD>
									<TD height="50"></TD>
								</TR>
								<TR>
									<TD></TD>
									<TD align="center">
										<TABLE id="Table4" height="23" cellSpacing="1" cellPadding="1" width="182" border="0">
											<TR>
												<TD vAlign="middle" align="center" colSpan="1" rowSpan="1"></TD>
												<TD align="center" width="112">
													<asp:button id="btnContinue" runat="server" CssClass="Button" CausesValidation="False" DESIGNTIMEDRAGDROP="176"
														Text="Pagina de inicio"></asp:button></TD>
												<TD align="center"></TD>
											</TR>
										</TABLE>
									</TD>
									<TD></TD>
								</TR>
								<TR>
									<TD height="36"></TD>
									<TD align="center" height="36">
										<HR width="100%" SIZE="1">
										<asp:Label id="Label2" runat="server" CssClass="labelHelp" DESIGNTIMEDRAGDROP="47" EnableViewState="False">Acceso denegado</asp:Label></TD>
									<TD height="36"></TD>
								</TR>
								<TR>
									<TD></TD>
									<TD vAlign="middle" align="center"></TD>
									<TD></TD>
								</TR>
							</TABLE>
						</asp:Panel></TD>
					<TD width="20%"></TD>
				</TR>
				<TR>
					<TD colSpan="4" height="22"></TD>
				</TR>
				<TR>
					<TD colSpan="4"></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
