<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../../Portal/Modules/ctrlFooter.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../../Portal/Modules/ctrlHeader.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ConciliationSummary.aspx.vb" Inherits="RateManager.ConciliationSummary" %>
<%@ Register TagPrefix="uc1" TagName="HotelBalanceViewer" Src="Modules/HotelBalanceViewer.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>ConciliationSummary</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="5" rightMargin="0">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Tablex" cellSpacing="0" cellPadding="0" width="650" align="center" border="0">
				<TR>
					<TD>
						<TABLE id="bookingcontainer" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD width="15"></TD>
								<TD height="60"></TD>
								<TD width="15"></TD>
							</TR>
							<TR>
								<TD width="15"></TD>
								<TD align="center"><asp:label id="lblMsg" runat="server" EnableViewState="False" CssClass="bookingNormalLabel">Error</asp:label></TD>
								<TD width="15"></TD>
							</TR>
							<TR>
								<TD width="15"></TD>
								<TD>
									<TABLE id="tblUpdated" cellSpacing="0" cellPadding="0" width="95%" align="center" border="0"
										runat="server">
										<TR>
											<TD class="dgheader" style="HEIGHT: 16px" colSpan="3"><asp:label id="lblUpdated" runat="server" EnableViewState="False">Información Actualizada</asp:label></TD>
										</TR>
										<TR>
											<TD height="40"></TD>
											<TD height="40"></TD>
											<TD height="40"></TD>
										</TR>
										<TR>
											<TD style="HEIGHT: 12px" align="center" colSpan="3"><asp:label id="lblUpdate" runat="server" EnableViewState="False" CssClass="bookingNormalLabel"></asp:label></TD>
										</TR>
										<TR>
											<TD align="center" colSpan="3" height="100"></TD>
										</TR>
										<TR>
											<TD colSpan="3">
												<TABLE id="Table2" cellSpacing="0" cellPadding="0" width="100%" border="0">
													<TR>
														<TD width="150"></TD>
														<TD class="clsLabel"><asp:literal id="ltlUpdateInfo" runat="server" EnableViewState="False"></asp:literal></TD>
													</TR>
												</TABLE>
											</TD>
										</TR>
										<TR>
											<TD></TD>
											<TD></TD>
											<TD></TD>
										</TR>
									</TABLE>
								</TD>
								<TD width="15"></TD>
							</TR>
							<TR>
								<TD width="15"></TD>
								<TD>
									<TABLE id="tblFinished" cellSpacing="0" cellPadding="0" width="95%" align="center" border="0"
										runat="server">
										<TR>
											<TD class="dgheader" style="HEIGHT: 16px" align="left" colSpan="3"><asp:label id="lblFinished" runat="server" EnableViewState="False">Conciliación Terminada</asp:label></TD>
										</TR>
										<TR>
											<TD height="40"></TD>
											<TD height="40"></TD>
											<TD height="40"></TD>
										</TR>
										<TR>
											<TD align="center" colSpan="3"><asp:label id="lblFinish" runat="server" EnableViewState="False" CssClass="bookingNormalLabel"></asp:label></TD>
										</TR>
										<tr>
											<td colspan="3" style="HEIGHT: 20px"></td>
										</tr>
										<TR>
											<TD align="center" colSpan="3">
												<asp:label id="lblInvoice" runat="server" EnableViewState="False" CssClass="bookingNormalLabel"></asp:label>&nbsp;<asp:hyperlink id="lnkInvoice" runat="server"></asp:hyperlink></TD>
										</TR>
										<TR>
											<TD align="center" colSpan="3" height="100"><BR>
												<uc1:hotelbalanceviewer id="balanceViewer" runat="server"></uc1:hotelbalanceviewer><BR>
											</TD>
										</TR>
										<TR>
											<TD colSpan="3">
												<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
													<TR>
														<TD width="150"></TD>
														<TD class="clsLabel"><asp:literal id="ltlFinishInfo" runat="server" EnableViewState="False"></asp:literal></TD>
													</TR>
												</TABLE>
											</TD>
										</TR>
										<TR>
											<TD></TD>
											<TD></TD>
											<TD></TD>
										</TR>
									</TABLE>
								</TD>
								<TD width="15"></TD>
							</TR>
							<TR>
								<TD width="15"></TD>
								<TD height="60"></TD>
								<TD width="15"></TD>
							</TR>
							<TR>
								<TD width="15" height="20"></TD>
								<TD height="20"></TD>
								<TD width="15" height="20"></TD>
							</TR>
							<TR>
								<TD width="15"></TD>
								<TD align="left">
									<TABLE id="Table4" cellSpacing="1" cellPadding="5" border="0">
										<TR>
											<TD width="20"></TD>
											<TD><asp:linkbutton id="lnkInvoiceDetails" runat="server" EnableViewState="False" CssClass="dgLink">Regresar a los Detalles de Fatura</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD width="20"></TD>
											<TD><asp:linkbutton id="lnkToConciliate" runat="server" EnableViewState="False" CssClass="dgLink">Regresar a Facturas Pendientes de Conciliar</asp:linkbutton></TD>
										</TR>
										<TR>
											<TD width="20"></TD>
											<TD><asp:hyperlink id="hypTaskList" runat="server" EnableViewState="False" CssClass="dgLink">Ir a Lista de Tareas</asp:hyperlink></TD>
										</TR>
									</TABLE>
								</TD>
								<TD width="15"></TD>
							</TR>
							<TR>
								<TD width="15" height="20"></TD>
								<TD height="20"></TD>
								<TD width="15" height="20"></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
