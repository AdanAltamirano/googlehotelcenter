<%@ Import NameSpace="Oz.BillingSystem.Common"%>
<%@ Import NameSpace="Oz.BillingSystem.HotelBillingEngine"%>
<%@ Control Language="vb" AutoEventWireup="false" Codebehind="HotelInvoiceViewer.ascx.vb" Inherits="RateManager.HotelInvoiceViewer" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Import NameSpace = "RateManager"%>
<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="100%" border="0">
	<TR>
		<TD align="center"><asp:label id="lblNoInvoices" CssClass="bookingNormalLabel" Visible="True" runat="server">No se encontraron facturas que mostrar.</asp:label></TD>
	</TR>
	<TR>
		<TD><asp:datalist id="dlsInvoices" Visible="True" runat="server" Width="100%">
				<ItemTemplate>
					<TABLE id="Table4" cellSpacing="1" cellPadding="1" width="100%" border="0">
						<TR>
							<TD class="captionBackgroundWithBorder" style="VERTICAL-ALIGN: middle" align="left"
								colSpan="2">
								<TABLE id="Table16" cellSpacing="1" cellPadding="1" width="100%" border="0">
									<TR>
										<TD class="" style="VERTICAL-ALIGN: top" vAlign="top">
											<asp:Label id=lblInvoiceIDCaption runat="server" CssClass="clsLabel" DESIGNTIMEDRAGDROP="10" Text='<%# PortalCulture.GetString("M0BT0000117", True) %>'>
											</asp:Label>&nbsp;
											<asp:Label id=lblInvoiceID runat="server" Visible="False" CssClass="clsLabel" Text='<%# DataBinder.Eval(Container.DataItem, "BillingStatementID") %>'>
											</asp:Label>
											<asp:HyperLink id=hypInvoiceID runat="server" CssClass="Link" Text='<%# DataBinder.Eval(Container.DataItem, "ReferenceNumber") %>' NavigateUrl='<%# String.Format("{0}/HotelAdministrator/Invoicing/Payment/HotelInvoiceDetails.aspx?id={1}", Request.ApplicationPath , DataBinder.Eval(Container.DataItem, "BillingStatementID")) %>'>
											</asp:HyperLink></TD>
										<TD class="" style="VERTICAL-ALIGN: top" align="right">
											<asp:Label id=lblGrandTotalCaption runat="server" CssClass="clsLabel" Text='<%# PortalCulture.GetString("M0BT0000118", True) %>'>
											</asp:Label>
											<asp:Label id=lblGrandTotal runat="server" CssClass="bookingNormalLabel" Text=''>
											</asp:Label></TD>
									</TR>
								</TABLE>
							</TD>
						</TR>
						<TR>
							<TD class="dgitem" style="VERTICAL-ALIGN: top" align="left">
								<TABLE id="Table5" cellSpacing="1" cellPadding="1" border="0">
									<TR>
										<TD align="right">
											<asp:Label id=lblPeriodCaption runat="server" CssClass="bookingNormalLabel" Text='<%# PortalCulture.GetString("M0BT0000119", True) %>'>
											</asp:Label></TD>
										<TD>
											<asp:Label id=lblPeriod runat="server" CssClass="clsHelpLabel" Text='<%# Formatting.GetPeriodName(CByte(DataBinder.Eval(Container.DataItem, "Month")), CInt(DataBinder.Eval(Container.DataItem, "Year")), , PortalCulture.GetCulture.Name) %>'>
											</asp:Label></TD>
									</TR>
									<TR>
										<TD align="right">
											<asp:Label id=lblStatusCaption runat="server" CssClass="bookingNormalLabel" Text='<%# PortalCulture.GetString("M0BT0000120", True) %>'>
											</asp:Label></TD>
										<TD>
											<asp:Label id="lblStatus" runat="server" CssClass="clsLabel" Text=""></asp:Label></TD>
									</TR>
									<TR>
										<TD align="right">
											<asp:Label id=lblHotelCaption runat="server" CssClass="bookingNormalLabel" Text='<%# PortalCulture.GetString("M0BT0000121", True) %>'>
											</asp:Label></TD>
										<TD>
											<asp:HyperLink id="lnkHotel" runat="server" CssClass="Link"></asp:HyperLink></TD>
									</TR>
									<TR>
										<TD align="right"></TD>
										<TD id="tdHotelData">
											<DIV class="textBox" id="divHotelData" style="WIDTH: 300px; POSITION: absolute; BACKGROUND-COLOR: white"
												runat="server" ms_positioning="FlowLayout">
												<TABLE id="Table19" cellSpacing="1" cellPadding="1" width="100%" border="0">
													<TR>
														<TD class="divTitleStep" style="VERTICAL-ALIGN: middle">
															<TABLE id="Table20" cellSpacing="1" cellPadding="1" width="100%" border="0">
																<TR class="dgalternate">
																	<TD>
																		<asp:Label id="lblHotelDataHeader" runat="server" CssClass="bookingNormalLabel"></asp:Label></TD>
																	<TD align="right">
																		<asp:Image id="imgCloseHotelData" runat="server" ImageUrl="../../../Images/close.png"></asp:Image></TD>
																</TR>
															</TABLE>
														</TD>
													</TR>
													<TR>
														<TD>
															<asp:Label id="lblHotel" runat="server" CssClass="clsLabel"></asp:Label></TD>
													</TR>
												</TABLE>
											</DIV>
										</TD>
									</TR>
								</TABLE>
							</TD>
							<TD class="dgitem" style="VERTICAL-ALIGN: top" align="right">
								<TABLE id="Table8" cellSpacing="1" cellPadding="1" border="0">
									<TR>
										<TD align="right">
											<asp:Label id=lblUserCaption runat="server" CssClass="bookingNormalLabel" Text='<%# PortalCulture.GetString("M0BT0000122", True) %>'>
											</asp:Label></TD>
										<TD>
											<asp:Label id=lblUser runat="server" CssClass="clsHelpLabel" Text='<%# DataBinder.Eval(Container.DataItem, "UserName") %>'>
											</asp:Label></TD>
									</TR>
									<TR>
										<TD align="right">
											<asp:Label id=lblGenerationDateCaption runat="server" CssClass="bookingNormalLabel" Text='<%# PortalCulture.GetString("M0BT0000123", True) %>'>
											</asp:Label></TD>
										<TD>
											<asp:Label id=lblRegistrationDate runat="server" CssClass="clsHelpLabel" Text='<%# Formatting.FormatDate(DataBinder.Eval(Container.DataItem, "RegistrationDate"), "D", PortalCulture.GetCulture.Name) %>'>
											</asp:Label></TD>
									</TR>
									<TR>
										<TD style="HEIGHT: 17px" align="right">
											<asp:Label id=lblLastConciliationDateCaption runat="server" CssClass="bookingNormalLabel" Text='<%# PortalCulture.GetString("M0BT0000124", True) %>'>
											</asp:Label></TD>
										<TD style="HEIGHT: 17px">
											<asp:Label id="lblLastConciliationDate" runat="server" CssClass="clsHelpLabel" Text=""></asp:Label></TD>
									</TR>
									<TR>
										<TD align="right">
											<asp:Label id=lblLastPaymentDateCaption runat="server" CssClass="bookingNormalLabel" Text='<%# PortalCulture.GetString("M0BT0000125", True) %>'>
											</asp:Label></TD>
										<TD>
											<asp:Label id="lblLastPaymentDate" runat="server" CssClass="clsHelpLabel" Text=""></asp:Label></TD>
									</TR>
									<TR>
										<TD align="right">
											<asp:Label id=lblReferenceNumberCaption runat="server" CssClass="bookingNormalLabel" Text='<%# PortalCulture.GetString("M0BT0000313", True) %>'>
											</asp:Label></TD>
										<TD>
											<asp:Label id=lblReferenceNumber runat="server" CssClass="clsHelpLabel" Text='<%# DataBinder.Eval(Container.DataItem, "CompanyID") &amp; "-" &amp; DataBinder.Eval(Container.DataItem, "BillingStatementID") %>'>
											</asp:Label></TD>
									</TR>
									<TR>
										<TD align="right">
											<asp:Label id=lblReferenceBankCaption runat="server" CssClass="bookingNormalLabel" Text='<%# PortalCulture.GetString("00793", True) %>'>
											</asp:Label></TD>
										<TD>
											<asp:Label id="lblReferenceBank" runat="server" CssClass="clsHelpLabel"></asp:Label></TD>
									</TR>
								</TABLE>
								<asp:HyperLink id="hLnkViewDetail" runat="server">Ver Detalle</asp:HyperLink></TD>
						</TR>
						<TR>
							<TD align="left" colSpan="2">
								<TABLE id="Table14" cellSpacing="0" cellPadding="3" width="100%" border="0">
									<TR>
										<TD class="boxBestRatesinBooking" style="VERTICAL-ALIGN: middle" align="center" width="30">
											<asp:Image id="imgToggleDetails" runat="server" ImageUrl="~/Includes/imagenes/mas.png"></asp:Image></TD>
										<TD class="dgheader" style="VERTICAL-ALIGN: middle">
											<asp:Label id=lblDetailsTitle runat="server" Text='<%# PortalCulture.GetString("M0BT0000132") %>'>
											</asp:Label></TD>
									</TR>
									<TR>
										<TD></TD>
										<TD id="tdDetails" runat="server">
											<asp:DataGrid id="grdDetails" runat="server" CssClass="DataGrid" Width="100%" AutoGenerateColumns="False"
												GridLines="None" HeaderStyle-CssClass="HeaderTableFillCell">
												<HeaderStyle CssClass="editItemBackGround"></HeaderStyle>
												<Columns>
													<asp:TemplateColumn>
														<ItemTemplate>
															<asp:Label id="lblDescription" runat="server"></asp:Label>
															<asp:HyperLink id="lnkDescription" runat="server"></asp:HyperLink>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<ItemTemplate>
															<asp:Label id="lblDgSubtotal" runat="server"></asp:Label>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<ItemTemplate>
															<asp:Label id="lblDgTaxes" runat="server"></asp:Label>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:TemplateColumn>
														<ItemTemplate>
															<asp:Label id="lblDgTotal" runat="server"></asp:Label>
														</ItemTemplate>
													</asp:TemplateColumn>
												</Columns>
											</asp:DataGrid></TD>
									</TR>
								</TABLE>
							</TD>
						</TR>
						<TR>
							<TD style="VERTICAL-ALIGN: top" align="right" colSpan="2">
								<TABLE id="Table22" cellSpacing="1" cellPadding="1" border="0">
									<TR>
										<TD align="right">
											<asp:Label id=lblSubtotalCaption runat="server" CssClass="bookingNormalLabel" Text='<%# PortalCulture.GetString("M0BT0000129", True) %>'>
											</asp:Label></TD>
										<TD align="right">
											<asp:Label id=lblSubtotal runat="server" CssClass="clsHelpLabel" Text=''>
											</asp:Label></TD>
									</TR>
									<TR>
										<TD align="right">
											<asp:Label id=lblTaxesCaption runat="server" CssClass="bookingNormalLabel" Text='<%# PortalCulture.GetString("M0BT0000130", True) %>'>
											</asp:Label></TD>
										<TD align="right">
											<asp:Label id=lblTaxes runat="server" CssClass="clsHelpLabel" Text=''>
											</asp:Label></TD>
									</TR>
									<TR>
										<TD align="right">
											<asp:Label id=lblTotalCaption runat="server" CssClass="bookingNormalLabel" Text='<%# PortalCulture.GetString("M0BT0000131", True) %>'>
											</asp:Label></TD>
										<TD align="right">
											<asp:Label id=lblTotal runat="server" CssClass="clsLabel" Text=''>
											</asp:Label></TD>
									</TR>
									<TR>
										<TD align="right">
											<asp:Label id=lblBalanceCaption runat="server" Visible="False" CssClass="bookingNormalLabel" Text='<%# PortalCulture.GetString("M0BT0000250", True) %>'>
											</asp:Label></TD>
										<TD align="right">
											<asp:Label id="lblBalance" runat="server" Visible="False" CssClass="clsLabel" Text=""></asp:Label></TD>
									</TR>
								</TABLE>
							</TD>
						</TR>
					</TABLE>
				</ItemTemplate>
			</asp:datalist></TD>
	</TR>
</TABLE>
