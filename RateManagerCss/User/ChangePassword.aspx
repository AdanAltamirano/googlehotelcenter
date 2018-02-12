<%@ Register TagPrefix="uc1" TagName="CtrMenu" Src="../Portal/Modules/CtrMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../Portal/Modules/ctrlHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../Portal/Modules/ctrlFooter.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ChangePassword.aspx.vb" Inherits="RateManager.ChangePassword" %>
<%@ Register TagPrefix="uc1" TagName="ctrlChangePassword" Src="ctrlChangePassword.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>ChangePassword</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
		<div class="clear">
            <div class="mDiv">
            </div>
            <div class="title">
                <asp:Label ID="lbltitle" runat="server" EnableViewState="False" Text="Cambio de contraseña" CssClass="tituloSeccion"></asp:Label>
            </div>
         </div>
			<table cellSpacing="0" cellPadding="2" width="650" border="0">
				<TBODY>
					<tr>
						<td>
							<TABLE cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">
								<TR>
									<TD width="20%" align="center" height="300">
										<asp:panel id="AcountPanel" runat="server" BorderStyle="None" Width="400px">
											<TABLE id="bookingcontainer" cellSpacing="1" cellPadding="1" width="100%" border="0">
												
												<TR>
													<TD>
														<uc1:ctrlChangePassword id="CtrlChangePassword1" runat="server"></uc1:ctrlChangePassword></TD>
												</TR>
												<TR>
													<TD align="center">
														<asp:button id="btnChange" runat="server" Text="Cambiar" CssClass="Button"></asp:button></TD>
												</TR>
											</TABLE>
										</asp:panel></TD>
								</TR>
							</TABLE>
						</td>
					</tr>
				</TBODY>
			</table>
		</form>
	</body>
</HTML>
