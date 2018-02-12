<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../../../Portal/Modules/ctrlHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../../../Portal/Modules/ctrlFooter.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="HotelInvoiceDetails.aspx.vb" Inherits="RateManager.HotelInvoiceDetails" %>
<%@ Register TagPrefix="uc1" TagName="HotelInvoiceDetailViewer" Src="../Modules/HotelInvoiceDetailViewer.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>HotelInvoiceDetails</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK href="../../../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="5" rightMargin="0">
		<form id="Form1" method="post" runat="server">
		<div>
		    <div class="mDiv"></div>
            <div class="title">
                <asp:Label ID="lblTitle" runat="server" EnableViewState="False" 
                    CssClass="tituloSeccion">Detalles de la factura</asp:Label>
            </div>
        </div>
			<TABLE id="bookingcontainer" cellSpacing="1" cellPadding="1" width="650" align="center"
				border="0">
				
				<TR>
					<TD align="left"><BR>
						<TABLE id="Table1" cellSpacing="1" cellPadding="5" width="100%" align="center" border="0">
							<TR>
								<TD align="center">
									<uc1:HotelInvoiceDetailViewer id="HotelInvoiceDetailViewer1" runat="server"></uc1:HotelInvoiceDetailViewer></TD>
							</TR>
							<TR>
								<TD><BR>
								</TD>
							</TR>
						</TABLE>
						<br>
						<TABLE id="Table3" cellSpacing="1" cellPadding="5" width="300" border="0">
							<TR>
								<TD width="15"></TD>
								<TD>
									<asp:HyperLink id="hypGoBack" runat="server" CssClass="dgLink" EnableViewState="False"  Visible="false">Regresar a Resumen de Facturas por Pagar </asp:HyperLink></TD>
							</TR>
							<TR>
								<TD width="15"></TD>
								<TD>
								<asp:linkbutton id="lnkToConciliate" runat="server" CssClass="dgLink" EnableViewState="False">Regresar a Facturas Pendientes de Conciliar</asp:linkbutton>
									<asp:HyperLink id="hypTaskList" runat="server" CssClass="dgLink" EnableViewState="False"  Visible="false">Ir a Lista de Tareas</asp:HyperLink>
									</TD>
							</TR>
							<TR>
								<TD width="15" height="15"></TD>
								<TD height="15"></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
