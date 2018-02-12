<%@ Page Language="vb" AutoEventWireup="false" Codebehind="passwordReminder.aspx.vb" Inherits="RateManager.passwordReminder" %>
<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../Portal/Modules/ctrlHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../Portal/Modules/ctrlFooter.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlPasswordReminder" Src="ctrlPasswordReminder.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>passwordReminder</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK rel="stylesheet" type="text/css" href="../StyleSheets/Styles.css">
		</LINK>
	</HEAD>
	<body MS_POSITIONING="FlowLayout" bottomMargin="0" leftMargin="0" topMargin="5" rightMargin="0">
		<form id="Form1" method="post" runat="server">
			<TABLE cellSpacing="0" cellPadding="0" width="670" border="0">
				<TR>
					<TD vAlign="middle" align="center" height="300">
						<asp:Panel id="panelPswChange" runat="server" Width="400px">
							<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="100%" border="0">
								<TR>
									<TD class="Titulo" align="center" height="21">
										<asp:Label id="lblRecordarContrasena" runat="server" EnableViewState="False">Recordar contraseña</asp:Label></TD>
								</TR>
								<TR>
									<TD align="center"></TD>
								</TR>
								<TR>
									<TD align="center">
										<uc1:ctrlPasswordReminder id="CtrlPasswordReminder1" runat="server"></uc1:ctrlPasswordReminder></TD>
								</TR>
								<TR>
									<TD align="center">
										<asp:LinkButton id="lnkReminderme" runat="server" CssClass="dglink">LinkButton</asp:LinkButton>&nbsp;
										<asp:HyperLink id="hplBack" runat="server" CssClass="dglink">Regresar</asp:HyperLink></TD>
								</TR>
								<TR>
									<TD align="center" height="36"></TD>
								</TR>
								<TR>
									<TD vAlign="middle" align="center"></TD>
								</TR>
							</TABLE>
						</asp:Panel>
						<asp:Panel id="PanelMsg" runat="server" Width="400px">
							<TABLE id="Table3" cellSpacing="1" cellPadding="1" width="100%" border="0">
								<TR>
									<TD align="center">
										<TABLE id="Table5" height="128" cellSpacing="1" cellPadding="1" width="100%" border="0">
											<TR>
												<TD class="Titulo" colSpan="3" height="10">
													<asp:label id="lblTitle" runat="server" EnableViewState="False">Title message</asp:label></TD>
											</TR>
											<TR>
												<TD vAlign="middle" align="center" colSpan="3" height="113">
													<asp:Label id="lblMessage" runat="server" CssClass="bookingNormalLabel">Message</asp:Label></TD>
											</TR>
										</TABLE>
									</TD>
								</TR>
								<TR>
									<TD align="center">
										<asp:button id="btnContinue" runat="server" Width="69px" CssClass="Button" Text="Continuar"
											CausesValidation="False"></asp:button></TD>
								</TR>
							</TABLE>
						</asp:Panel></TD>
				</TR>
				<tr>
					<td></td>
				</tr>
			</TABLE>
		</form>
	</body>
</HTML>
