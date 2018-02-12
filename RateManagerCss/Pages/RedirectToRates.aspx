<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../Portal/Modules/ctrlHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrMenu" Src="../Portal/Modules/CtrMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../Portal/Modules/ctrlFooter.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RedirectToRates.aspx.vb" Inherits="RateManager.RedirectToRates"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>RedirectToRates</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
		</LINK>
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<table id="bookingcontainer" cellSpacing="0" cellPadding="2" width="650" border="0">		
				<tr>			
					<td>
						<table cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD style="HEIGHT: 10px" align="center">
									<asp:LinkButton id="lnkRooms" runat="server" CssClass="dgLink">Continuar agregando cuartos</asp:LinkButton></TD>
							</TR>
							<TR>
								<TD align="center">
									<asp:Label id="lblOr" runat="server" CssClass="clslabel">Label</asp:Label></TD>
							</TR>
							<TR>
								<TD>
									<DIV id="ShowDiv" runat="server" align="center">
										<TABLE>
											<TR>
												<TD align="center" colSpan="2">
													<asp:label id="lblDivTitle" runat="server" CssClass="bookingNormalLabel">Definición de tarifas para la habitación</asp:label></TD>
											</TR>
											<TR>
												<TD>
													<asp:linkbutton id="lnkFaresCatalogue" runat="server" CssClass="dgLink" CausesValidation="False">Fares catalogue</asp:linkbutton></TD>
												<TD>
													<asp:linkbutton id="lnkLinkRooms" runat="server" CssClass="dgLink" CausesValidation="False">Link room types</asp:linkbutton></TD>
											</TR>
										</TABLE>
									</DIV>
								</TD>
							</TR>
							<TR>
								<TD>
									<DIV id="ModifDiv" runat="server" align="center">
										<TABLE>
											<TR>
												<TD align="center">
													<asp:label id="lblTitleChangeRates" runat="server" CssClass="bookingNormalLabel">Debe cambiar </asp:label></TD>
											</TR>
											<TR>
												<TD align="center">
													<asp:linkbutton id="lnkFares" runat="server" CssClass="dgLink" CausesValidation="False">Fares catalogue</asp:linkbutton></TD>
											</TR>
										</TABLE>
									</DIV>
								</TD>
							</TR>
						</table>
					</td>
				</tr>				
			</table>
		</form>
	</body>
</HTML>
