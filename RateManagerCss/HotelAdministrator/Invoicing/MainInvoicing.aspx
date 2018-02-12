<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../../Portal/Modules/ctrlFooter.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../../Portal/Modules/ctrlHeader.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="MainInvoicing.aspx.vb" Inherits="RateManager.MainInvoicing" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>MainInvoicing</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="5" rightMargin="0">
		<form id="Form1" method="post" runat="server">
		    <div class="clear">
        <div id="tituloSeccion" style=" text-align:center !important;  ">
            <img class="bgplus" src="../Includes/imagenes/icono-big-plus.png" width="25" height="25" />
            <asp:Label ID="lblMainTitle" class="tituloSeccion" runat="server" EnableViewState="False">Pagos  y Facturación</asp:Label></div>

        </div> 
			<TABLE id="bookingcontainer" cellSpacing="0" cellPadding="0" width="500" border="0" align="center">
				<TR>
					<td>
						<TABLE style="WIDTH: 375px" cellSpacing="1" cellPadding="1" width="375" align="center"
							border="0">
							
							<TR>
								<TD align="left" colSpan="2"><asp:label id="lblConciliationTitle" runat="server" EnableViewState="False" CssClass="bookingNormalLabel">Conciliación:</asp:label></TD>
							</TR>
							<tr>
								<td colSpan="2">
									<TABLE id="tblPortal" cellSpacing="0" cellPadding="1" width="100%" border="0" runat="server">
										<TR>
											<TD></TD>
											<TD align="left"><asp:hyperlink id="hypInvoicesToConciliate" runat="server" CssClass="dgLink" EnableViewState="False"> Conciliar Facturas</asp:hyperlink></TD>
										</TR>
									</TABLE>
								</td>
							</tr>
							<TR>
								<TD align="left" colSpan="2"><asp:label id="lblPaymentTitle" runat="server" CssClass="bookingNormalLabel" EnableViewState="False">Pagos:</asp:label></TD>
							</TR>
							<tr>
								<td colSpan="2">
									<TABLE id="Table1" cellSpacing="0" cellPadding="1" width="100%" border="0" runat="server">
										<TR>
											<TD></TD>
											<TD align="left"><asp:hyperlink id="hypInvoicesPendingToPay" runat="server" CssClass="dgLink" EnableViewState="False">Facturas Pendientes de Pago</asp:hyperlink></TD>
										</TR>
										<tr>
											<TD></TD>
											<TD align="left"><asp:hyperlink id="hypPaymentRegistration" runat="server" CssClass="dgLink" EnableViewState="False"> Registrar Pagos</asp:hyperlink></TD>
										</tr>
										<tr>
											<TD></TD>
											<TD align="left"><asp:hyperlink id="hypPaymentRegistered" runat="server" CssClass="dgLink" EnableViewState="False">Ver Resumen de Pagos Registrados</asp:hyperlink></TD>
										</tr>
									</TABLE>
								</td>
							</tr>
							<TR>
								<TD align="left" colSpan="2"><asp:label id="lblInvoicesTitle" runat="server" CssClass="bookingNormalLabel" EnableViewState="False">Facturación:</asp:label></TD>
							</TR>
							<tr>
								<td colSpan="2">
									<TABLE id="Table2" cellSpacing="0" cellPadding="1" width="100%" border="0" runat="server">
										<TR>
											<TD></TD>
											<TD align="left"><asp:hyperlink id="hypPrintInvoices" runat="server" CssClass="dgLink" EnableViewState="False">Imprimir Facturas</asp:hyperlink></TD>
										</TR>
									</TABLE>
								</td>
							</tr>
						</TABLE>
					</td>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
