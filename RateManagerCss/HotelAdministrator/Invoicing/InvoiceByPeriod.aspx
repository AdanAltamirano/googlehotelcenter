<%@ Import NameSpace = "RateManager" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="InvoiceByPeriod.aspx.vb" Inherits="RateManager.InvoiceByPeriod"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>InvoiceByPeriod</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body MS_POSITIONING="flowLayout">
		<form id="Form1" method="post" runat="server">
		<div class="mDiv">
            </div>
            <div class="title">
                <asp:Label ID="lblTitle" runat="server" EnableViewState="False" 
                    CssClass="tituloSeccion">Facturas Pendientes de Conciliar</asp:Label>
            </div>
        </div>
			<TABLE id="bookingcontainer" cellSpacing="0" cellPadding="0" width="100%" align="center"
				border="0">
				<TR>
					<TD>
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">							
							<TR>
								<TD align="center" colSpan="3"><asp:dropdownlist id="ddlperiodo" runat="server">
										<asp:ListItem>enero</asp:ListItem>
										<asp:ListItem>febrero</asp:ListItem>
										<asp:ListItem>marzo</asp:ListItem>
										<asp:ListItem>abril</asp:ListItem>
										<asp:ListItem>Mayo</asp:ListItem>
										<asp:ListItem>Junio</asp:ListItem>
										<asp:ListItem>Julio</asp:ListItem>
										<asp:ListItem>Agosto</asp:ListItem>
										<asp:ListItem>Septiembre</asp:ListItem>
										<asp:ListItem>Octubre</asp:ListItem>
										<asp:ListItem>Noviembre</asp:ListItem>
										<asp:ListItem>Diciembre</asp:ListItem>
									</asp:dropdownlist><asp:dropdownlist id="ddlyear" runat="server">
										<asp:ListItem Value="2006"></asp:ListItem>
										<asp:ListItem Value="2006"></asp:ListItem>
										<asp:ListItem Value="2007"></asp:ListItem>
									</asp:dropdownlist><asp:button id="btnload" runat="server" Text="Mostrar" CssClass="button"></asp:button></TD>
							</TR>
							<TR>
								<TD width="15"></TD>
								<TD align="center" height="15"><asp:label id="lblPendingInfo" runat="server" EnableViewState="False" CssClass="Validators">No hay Facturas en ese periodo</asp:label></TD>
								<TD width="15"></TD>
							</TR>
							<TR>
								<TD width="15"></TD>
								<TD align="center"><asp:datagrid id="dgSummary" runat="server" CssClass="DataGrid" BorderColor="WhiteSmoke" PageSize="12"
										AllowPaging="True" Width="100%" AutoGenerateColumns="False">
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
												<HeaderStyle ></HeaderStyle>
												<ItemTemplate>
													<asp:HyperLink id="lnkPeriodo" runat="server" CssClass="DGLink"></asp:HyperLink>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="Concepto">
												<HeaderStyle ></HeaderStyle>
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
												<HeaderStyle ></HeaderStyle>
												<ItemTemplate>
													<asp:Label id="lblRegistrationDate" runat="server"></asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn Visible="False" DataField="RegistrationDate"></asp:BoundColumn>
											<asp:TemplateColumn HeaderText="L&#237;mite para Conciliar">
												<HeaderStyle ></HeaderStyle>
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
											<TD><asp:hyperlink id="hypMainConciliate" runat="server" EnableViewState="False" CssClass="dgLink" Text='<%# PortalCulture.GetString("M0BT0000148") %>' NavigateUrl='<%# String.Format("{0}MainInvoicing.aspx?cid={1}", GeRequestApplicationPath("/HotelAdministrator/Invoicing/"), CompanyID) %>'></asp:hyperlink></TD>
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
