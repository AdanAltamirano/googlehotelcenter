<%@ Page Language="vb" AutoEventWireup="false" Codebehind="PaymentRegistered.aspx.vb" Inherits="RateManager.PaymentRegistered" %>
<%@ Register TagPrefix="uc1" TagName="HotelBalanceViewer" Src="../Modules/HotelBalanceViewer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../../../Portal/Modules/ctrlHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../../../Portal/Modules/ctrlFooter.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>PaymentRegistered</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK href="../../../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="5" rightMargin="0">
		<form id="Form1" method="post" runat="server">
		 <div class="mDiv">
            </div>
            <div class="title">
                <asp:Label ID="lblTitle" runat="server" EnableViewState="False" 
                    CssClass="tituloSeccion">Pago Registrado</asp:Label>
            </div>
        </div>
			<TABLE id="bookingcontainer" cellSpacing="0" cellPadding="0" width="650" border="0" align="center">
				<TR>
					<TD width="15" height="20"></TD>
					<TD height="20"></TD>
					<TD width="15" height="20"></TD>
				</TR>
				<TR>
					<TD width="15" height="20"></TD>
					<TD height="20">
						<TABLE id="Table2" cellSpacing="1" cellPadding="3" width="60%" align="center" border="0">
							<TR>
								<TD class="menuitem" align="center" colSpan="2">
									<asp:label id="lblMsg" runat="server" CssClass="txtDataBold">Su Pago Ha Sido Registrado</asp:label></TD>
							</TR>
							<TR>
								<TD colSpan="2" height="20"></TD>
							</TR>
							<TR>
								<TD colSpan="2">
									<asp:label id="lblTitle2" runat="server" CssClass="bookingNormalLabel" EnableViewState="False">Información Registrada</asp:label></TD>
							</TR>
							<TR>
								<TD width="25"></TD>
								<TD>
									<TABLE id="Table3" cellSpacing="1" cellPadding="3" border="0" runat="server">
										<TR>
											<TD align="right" class="dgalternate">
												<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0" height="100%">
													<TR>
														<TD align="right">
															<asp:label id="lblsBankName" runat="server" CssClass="clsLabel" EnableViewState="False">Banco:</asp:label></TD>
													</TR>
												</TABLE>
											</TD>
											<TD class="textBox" height="20" bgColor="#ffffff">
												<asp:label id="lblBankName" runat="server" CssClass="bookingNormalLabel" EnableViewState="False"></asp:label></TD>
										</TR>
										<TR>
											<TD class="dgalternate" align="right">
												<TABLE id="Table7" height="100%" cellSpacing="0" cellPadding="0" width="100%" border="0">
													<TR>
														<TD align="right">
															<asp:label id="lblsAccountNumber" runat="server" CssClass="clsLabel" EnableViewState="False">Número de Cuenta:</asp:label></TD>
													</TR>
												</TABLE>
											</TD>
											<TD class="textBox" height="20" bgColor="#ffffff">
												<asp:label id="lblAccountNumber" runat="server" CssClass="bookingNormalLabel" EnableViewState="False"></asp:label></TD>
										</TR>
										<TR>
											<TD class="dgalternate" align="right">
												<TABLE id="Table8" height="100%" cellSpacing="0" cellPadding="0" width="100%" border="0">
													<TR>
														<TD align="right">
															<asp:Label id="lblsAccountName" runat="server" CssClass="clsLabel" EnableViewState="False">Nombre de Cuenta:</asp:Label></TD>
													</TR>
												</TABLE>
											</TD>
											<TD class="textBox" height="20" bgColor="#ffffff">
												<asp:Label id="lblAccountName" runat="server" CssClass="bookingNormalLabel" EnableViewState="False"></asp:Label></TD>
										</TR>
										<TR>
											<TD height="15"></TD>
											<TD height="15"></TD>
										</TR>
										<TR>
											<TD class="dgalternate" align="right">
												<TABLE id="Table9" height="100%" cellSpacing="0" cellPadding="0" width="100%" border="0">
													<TR>
														<TD align="right">
															<asp:Label id="lblsSourceBank" runat="server" CssClass="clsLabel" EnableViewState="False">Banco donde realizó <BR>el pago:</asp:Label></TD>
													</TR>
												</TABLE>
											</TD>
											<TD class="textBox" height="20" bgColor="#ffffff">
												<asp:Label id="lblSourceBank" runat="server" CssClass="bookingNormalLabel" EnableViewState="False"></asp:Label></TD>
										</TR>
										<TR>
											<TD class="dgalternate" align="right">
												<TABLE id="Table10" height="100%" cellSpacing="0" cellPadding="0" width="100%" border="0">
													<TR>
														<TD align="right">
															<asp:label id="lblsTransactionNumber" runat="server" CssClass="clsLabel" EnableViewState="False">No. Transacción:</asp:label></TD>
													</TR>
												</TABLE>
											</TD>
											<TD class="textBox" height="20" bgColor="#ffffff">
												<asp:label id="lblTransactionNumber" runat="server" CssClass="bookingNormalLabel" EnableViewState="False"></asp:label></TD>
										</TR>
										<TR>
											<TD class="dgalternate" align="right">
												<TABLE id="Table11" height="100%" cellSpacing="0" cellPadding="0" width="100%" border="0">
													<TR>
														<TD align="right">
															<asp:label id="lblsPaymentType" runat="server" CssClass="clsLabel" EnableViewState="False">Tipo de Pago:</asp:label></TD>
													</TR>
												</TABLE>
											</TD>
											<TD class="textBox" height="20" bgColor="#ffffff">
												<asp:label id="lblPaymentType" runat="server" CssClass="bookingNormalLabel" EnableViewState="False"></asp:label></TD>
										</TR>
										<TR>
											<TD class="dgalternate" align="right">
												<TABLE id="Table15" height="100%" cellSpacing="0" cellPadding="0" width="100%" border="0">
													<TR>
														<TD align="right">
															<asp:label id="lblsReferenceBank" runat="server" CssClass="clsLabel" EnableViewState="False">Tipo de Pago:</asp:label></TD>
													</TR>
												</TABLE>
											</TD>
											<TD class="textBox" height="20" bgColor="#ffffff">
												<asp:label id="lblReferenceBank" runat="server" CssClass="bookingNormalLabel" EnableViewState="False"></asp:label></TD>
										</TR>
										<TR>
											<TD height="15"></TD>
											<TD height="15"></TD>
										</TR>
										<TR>
											<TD class="dgalternate" align="right">
												<TABLE id="Table12" height="100%" cellSpacing="0" cellPadding="0" width="100%" border="0">
													<TR>
														<TD align="right">
															<asp:label id="lblsAmount" runat="server" CssClass="clsLabel" EnableViewState="False">Cantidad Pagada:</asp:label></TD>
													</TR>
												</TABLE>
											</TD>
											<TD class="textBox" height="20" bgColor="#ffffff">
												<asp:label id="lblAmount" runat="server" CssClass="bookingNormalLabel" EnableViewState="False"></asp:label></TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
						</TABLE>
					</TD>
					<TD width="15" height="20"></TD>
				</TR>
				<TR>
					<TD></TD>
					<TD></TD>
					<TD></TD>
				</TR>
				<TR>
					<TD height="20"></TD>
					<TD align="left">
						<TABLE id="Table5" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD><asp:hyperlink id="hplGoTo" runat="server" CssClass="dgLink" EnableViewState="False">Ir a Lista de Tareas</asp:hyperlink></TD>
							</TR>
						</TABLE>
					</TD>
					<TD height="20"></TD>
				</TR>
				<TR>
					<TD height="30" colspan="3"></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
