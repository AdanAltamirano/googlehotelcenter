<%@ Import NameSpace = "RateManager"%>
<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../../../Portal/Modules/ctrlHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../../../Portal/Modules/ctrlFooter.ascx" %>
<%@ Register TagPrefix="uc1" TagName="HotelInvoiceViewer" Src="../Modules/HotelInvoiceViewer.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="PaymentInvoicesSummary.aspx.vb" Inherits="RateManager.PaymentInvoicesSummary" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title></title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../../../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="5" rightMargin="0">
		<form id="Form1" method="post" runat="server">
		 <div class="mDiv">
            </div>
            <div class="title">
                <asp:Label ID="lblTitle" runat="server" EnableViewState="False" 
                    CssClass="tituloSeccion">>Resumen de Facturas Por Pagar</asp:Label>
            </div>
        </div>
			<TABLE id="bookingcontainer" cellSpacing="0" cellPadding="0" width="650" border="0" align="center">				
				<TR>
					<TD height="30"></TD>
					<TD height="30"></TD>
					<TD height="30"></TD>
				</TR>
				<TR>
					<TD align="center" colSpan="3"><uc1:hotelinvoiceviewer id="HotelInvoiceViewer1" runat="server"></uc1:hotelinvoiceviewer></TD>
				</TR>
				<TR>
					<TD height="15"></TD>
					<TD height="15"></TD>
					<TD height="15"></TD>
				</TR>
				<TR>
					<TD align="left" colSpan="3"><br>
						<TABLE id="Table2" cellSpacing="1" cellPadding="5" border="0">
							<TR>
								<TD width="15"></TD>
								<TD><asp:hyperlink id=hypTaskList runat="server" CssClass="dgLink"  EnableViewState="False">Regresar a Lista de Tareas</asp:hyperlink></TD>
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
