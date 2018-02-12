<%@ Import NameSpace = "RateManager" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="InvoicesToConciliate.aspx.vb" Inherits="RateManager.InvoicesToConciliate" %>
<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../../Portal/Modules/ctrlFooter.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../../Portal/Modules/ctrlHeader.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>InvoicesToConciliate</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="5" rightMargin="0">
		<form id="Form1" method="post" runat="server">
		<div class="clear">
        <div class="mDiv">
            </div>
            <div class="title">
                <asp:Label ID="lblTitle" runat="server" EnableViewState="False" 
                    CssClass="tituloSeccion">Facturas Pendientes de Conciliar</asp:Label>
            </div>
        </div>
			<TABLE id="bookingcontainer" cellSpacing="0" cellPadding="0" width="650" border="0" align="center">
				<TR>
					<TD>
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
							
							<TR>
								<TD height="15"></TD>
								<TD height="15"></TD>
								<TD height="15"></TD>
							</TR>
							<TR>
								<TD height="15"></TD>
								<TD height="15"></TD>
								<TD height="15"></TD>
							</TR>
							<TR>
								<TD width="15"></TD>
								<TD align="center" height="15"><asp:label id="lblPendingInfo" runat="server" CssClass="bookingNormalLabel" EnableViewState="False">No hay Facturas Pendientes de Conciliar</asp:label></TD>
								<TD width="15"></TD>
							</TR>
							<TR>
								<TD width="15"></TD>
								<TD align="center"><asp:datagrid id="dgSummary" runat="server" CssClass="DataGrid" AutoGenerateColumns="False" Width="100%"
										AllowPaging="True" PageSize="12" BorderColor="WhiteSmoke">
										<AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
										<ItemStyle CssClass="dgitem"></ItemStyle>
										<HeaderStyle CssClass="DGHeader"></HeaderStyle>
										<Columns>
											<asp:TemplateColumn>
												<ItemTemplate>
													<asp:Label id="Label1" runat="server" CssClass="bookingNormalLabel"></asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="Periodo">
												
												<ItemTemplate>
													<asp:HyperLink id="lnkPeriodo" runat="server" CssClass="DGLink"></asp:HyperLink>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="Concepto">
												
												<ItemTemplate>
													<asp:DataGrid id="dgConcept" runat="server" CssClass="DataGrid" AutoGenerateColumns="False" GridLines="None"
														ShowHeader="False">
														<Columns>
															<asp:TemplateColumn>
																<ItemTemplate>
																	<asp:Label id="lblConcept" runat="server">Concept</asp:Label>
																</ItemTemplate>
															</asp:TemplateColumn>
														</Columns>
													</asp:DataGrid>
													<asp:DataGrid id="dgExtraCharges" runat="server" CssClass="DataGrid" AutoGenerateColumns="False"
														GridLines="None" ShowHeader="False">
														<AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
														<ItemStyle CssClass="dgitem"></ItemStyle>
														<Columns>
															<asp:TemplateColumn>
																<ItemTemplate>
																	<asp:Label id="lblExtraCharge" runat="server">Extra</asp:Label>
																</ItemTemplate>
															</asp:TemplateColumn>
														</Columns>
													</asp:DataGrid>
													<asp:Label id="lblCommissionConcept" runat="server"></asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="Generado">
												
												<ItemTemplate>
													<asp:Label id="lblRegistrationDate" runat="server"></asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn Visible="False" DataField="RegistrationDate"></asp:BoundColumn>
											<asp:TemplateColumn HeaderText="L&#237;mite para Conciliar">
												
												<ItemTemplate>
													<asp:Label id="lblLastConciliationDate" runat="server"></asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn Visible="False" DataField="BillingStatementID"></asp:BoundColumn>
										</Columns>
										<PagerStyle HorizontalAlign="Right" CssClass="dgpager" Mode="NumericPages"></PagerStyle>
									</asp:datagrid></TD>
								<TD width="15"></TD>
							</TR>
							<TR>
								<TD width="15" height="15"></TD>
								<TD align="center" height="15"></TD>
								<TD width="15" height="15"></TD>
							</TR>
							<TR>
								<TD width="15" height="15"></TD>
								<TD align="left">
									<TABLE id="Table2" cellSpacing="1" cellPadding="5" border="0">
										<TR>
											<TD><asp:hyperlink id=hypMainConciliate runat="server" CssClass="dgLink" Text='<%# PortalCulture.GetString("M0BT0000148") %>' NavigateUrl='<%# String.Format("{0}MainInvoicing.aspx?cid={1}", GeRequestApplicationPath("/HotelAdministrator/Invoicing/"), CompanyID) %>' EnableViewState="False"></asp:hyperlink></TD>
										</TR>
									</TABLE>
								</TD>
								<TD width="15" height="15"></TD>
							</TR>
							<TR>
								<TD width="15" height="20"></TD>
								<TD align="center" height="20"></TD>
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
