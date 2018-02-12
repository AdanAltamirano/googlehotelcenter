<%@ Import NameSpace="Oz.BillingSystem.Common"%>
<%@ Import NameSpace="RateManager"%>
<%@ Import NameSpace="Oz.BillingSystem.HotelBillingEngine"%>
<%@ Control Language="vb" AutoEventWireup="false" Codebehind="HotelInvoiceDetailViewer.ascx.vb" Inherits="RateManager.HotelInvoiceDetailViewer" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<meta name="vs_snapToGrid" content="False">
<TABLE id="Table1" border="0" cellSpacing="1" cellPadding="1" width="100%">
	<TR>
		<TD>
			<TABLE id="Table6" border="0" cellSpacing="1" cellPadding="1" width="100%">
				<TR>
					<TD class="textBox"><asp:label id="lblInvoiceIDCaption" EnableViewState="False" CssClass="bookingNormalLabel" runat="server">No. Folio:</asp:label>&nbsp;
						<asp:label id="lblInvoiceID" EnableViewState="False" CssClass="bookingNormalLabel" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD>
						<TABLE id="Table7" border="0" cellSpacing="1" cellPadding="1" width="100%">
							<TR>
								<TD vAlign="top">
									<TABLE id="Table8" border="0" cellSpacing="1" cellPadding="1" width="100%">
										<TR>
											<TD align="right"><asp:label id="lblInvoiceNumberCaption" EnableViewState="False" CssClass="bookingNormalLabel"
													runat="server">No. Factura:</asp:label></TD>
											<TD><asp:label id="lblInvoiceNumber" CssClass="clsLabel" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD align="right"><asp:label id="lblPeriodCaption" EnableViewState="False" CssClass="bookingNormalLabel" runat="server">Período:</asp:label></TD>
											<TD><asp:label id="lblPeriod" CssClass="clsLabel" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD align="right"><asp:label id="lblStatusCaption" CssClass="bookingNormalLabel" runat="server">Estado:</asp:label></TD>
											<TD><asp:label id="lblStatus" CssClass="clsLabel" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD align="right"><asp:label id="lblCurrencyCodeCaption" EnableViewState="False" CssClass="bookingNormalLabel"
													runat="server">Código de moneda:</asp:label></TD>
											<TD><asp:label id="lblCurrencyCode" CssClass="clsLabel" runat="server"></asp:label></TD>
										</TR>
										<TR id="trMoneyExchange" runat="server">
											<TD align="right"><asp:label id="lblMoneyExchangeCaption" EnableViewState="False" CssClass="bookingNormalLabel"
													runat="server">Tipo de cambio:</asp:label></TD>
											<TD><asp:label id="lblMoneyExchange" CssClass="clsLabel" runat="server"></asp:label></TD>
										</TR>
									</TABLE>
								</TD>
								<TD vAlign="top">
									<TABLE id="Table9" border="0" cellSpacing="1" cellPadding="1" width="100%">
										<TR>
											<TD align="right"><asp:label id="lblRegistrationDateCaption" EnableViewState="False" CssClass="bookingNormalLabel"
													runat="server">Fecha de generación:</asp:label></TD>
											<TD><asp:label id="lblRegistrationDate" CssClass="clsLabel" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="HEIGHT: 6px" align="right"><asp:label id="lblLimitToConciliateCaption" EnableViewState="False" CssClass="bookingNormalLabel"
													runat="server">Límite para conciliar:</asp:label></TD>
											<TD style="HEIGHT: 6px"><asp:label id="lblLimitToConciliate" CssClass="clsLabel" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD align="right"><asp:label id="lblLimitToPayCaption" EnableViewState="False" CssClass="bookingNormalLabel"
													runat="server">Límite para pagar:</asp:label></TD>
											<TD><asp:label id="lblLimitToPay" CssClass="clsLabel" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD align="right"><asp:label id="lblConciliationDateCaption" EnableViewState="False" CssClass="bookingNormalLabel"
													runat="server">Fecha de conciliación:</asp:label></TD>
											<TD><asp:label id="lblConciliationDate" CssClass="clsLabel" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD align="right"><asp:label id="lblPaymentDateCaption" EnableViewState="False" CssClass="bookingNormalLabel"
													runat="server">Fecha de pago:</asp:label></TD>
											<TD><asp:label id="lblPaymentDate" CssClass="clsLabel" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD align="right"><asp:label id="lblReferenceBankCapion" EnableViewState="False" CssClass="bookingNormalLabel"
													runat="server">Fecha de pago:</asp:label></TD>
											<TD><asp:label id="lblReferenceBank" CssClass="clsLabel" runat="server"></asp:label></TD>
										</TR>
									</TABLE>
								</TD>
								<TD vAlign="top">
									<TABLE id="Table10" border="0" cellSpacing="1" cellPadding="1" width="100%">
										<TR>
											<TD align="right"><asp:label id="lblHotelNameCaption" EnableViewState="False" CssClass="bookingNormalLabel" runat="server">Hotel:</asp:label></TD>
											<TD><asp:label id="lblHotelName" CssClass="clsLabel" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD align="right"><asp:label id="lblStreetAddressCaption" EnableViewState="False" CssClass="bookingNormalLabel"
													runat="server">Street address:</asp:label></TD>
											<TD><asp:label id="lblStreetAddress" CssClass="clsLabel" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD align="right"><asp:label id="lblPostalCodeCaption" EnableViewState="False" CssClass="bookingNormalLabel"
													runat="server">Código postal:</asp:label></TD>
											<TD><asp:label id="lblPostalCode" CssClass="clsLabel" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD style="HEIGHT: 4px" align="right"><asp:label id="lblZoneCaption" EnableViewState="False" CssClass="bookingNormalLabel" runat="server">Zona:</asp:label></TD>
											<TD style="HEIGHT: 4px"><asp:label id="lblZone" CssClass="clsLabel" runat="server"></asp:label></TD>
										</TR>
										<TR>
											<TD align="right"><asp:label id="lblLocationCaption" EnableViewState="False" CssClass="bookingNormalLabel" runat="server">Ubicación:</asp:label></TD>
											<TD><asp:label id="lblLocation" CssClass="clsLabel" runat="server"></asp:label></TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
		</TD>
	</TR>
	<TR>
		<TD>
			<TABLE id="Table2" border="0" cellSpacing="1" cellPadding="1" width="100%">
				<TR id="trChargesExpander" runat="server">
					<TD style="WIDTH: 20px; VERTICAL-ALIGN: middle" class="dgitem" align="center"><asp:image id="imgToggleCharges" runat="server" ImageUrl="~/Includes/imagenes/menos.png"></asp:image></TD>
					<TD style="PADDING-BOTTOM: 5px; PADDING-LEFT: 5px; PADDING-RIGHT: 3px; VERTICAL-ALIGN: middle; PADDING-TOP: 5px"
						class="dgitem">
						<TABLE id="Table3" border="0" cellSpacing="1" cellPadding="1" width="100%">
							<TR>
								<TD class="txtDataBold" width="50%"><asp:label id="lblChargesCaption" EnableViewState="False" CssClass="bookingNormalLabel" runat="server">Cargos</asp:label></TD>
								<TD width="50%" align="right"><asp:label id="lblCharges" EnableViewState="False" CssClass="clsLabel" runat="server"></asp:label></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR id="trCharges" runat="server">
					<TD style="WIDTH: 20px" align="center"></TD>
					<TD><asp:datagrid id="grdCharges" EnableViewState="False" CssClass="DataGrid" runat="server" GridLines="None"
							AutoGenerateColumns="False" Width="100%" AllowSorting="True">
							<AlternatingItemStyle CssClass="CalendarBackground"></AlternatingItemStyle>
							<HeaderStyle CssClass="dgHeader"></HeaderStyle>
							<Columns>
								<asp:TemplateColumn SortExpression="BillingStatementTransactionCode" HeaderText="Descripci&#243;n">
									<HeaderStyle CssClass="txtPlanDetailHotelRatesList"></HeaderStyle>
									<ItemTemplate>
										<asp:Label id="lblDescription" CssClass="txtPlanDetailHotelRatesList" runat="server" Text=""></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="Quantity" SortExpression="Quantity" HeaderText="Quantity">
									<HeaderStyle HorizontalAlign="Right"  CssClass="txtPlanDetailHotelRatesList"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn SortExpression="Amount" HeaderText="Amount">
									<HeaderStyle HorizontalAlign="Right"  CssClass="txtPlanDetailHotelRatesList"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lblDgAmount" CssClass="txtPlanDetailHotelRatesList" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="Subtotal" HeaderText="Subtotal">
									<HeaderStyle HorizontalAlign="Right" CssClass="txtPlanDetailHotelRatesList"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lblDgSubtotal" CssClass="txtPlanDetailHotelRatesList" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR id="trCommissionsExpander" runat="server">
					<TD style="WIDTH: 20px; VERTICAL-ALIGN: middle" class="DataGridAlternatedItem" align="center"><asp:image id="imgToggleCommissions" runat="server" ImageUrl="~/Includes/imagenes/menos.png"></asp:image></TD>
					<TD style="PADDING-BOTTOM: 3px; PADDING-LEFT: 3px; PADDING-RIGHT: 3px; VERTICAL-ALIGN: middle; PADDING-TOP: 3px"
						class="DataGridAlternatedItem">
						<TABLE id="Table4" border="0" cellSpacing="1" cellPadding="1" width="100%">
							<TR>
								<TD class="txtDataBold" width="50%"><asp:label id="lblCommissionsCaption" EnableViewState="False" CssClass="bookingNormalLabel"
										runat="server">Comisiones</asp:label></TD>
								<TD width="50%" align="right"><asp:label id="lblCommissions" EnableViewState="False" CssClass="clsLabel" runat="server"></asp:label></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR id="trCommissions" runat="server">
					<TD style="WIDTH: 20px" align="center"></TD>
					<TD style="VERTICAL-ALIGN: top"><asp:datagrid id="grdCommissions" EnableViewState="False" CssClass="DataGrid" runat="server" GridLines="None"
							AutoGenerateColumns="False" Width="100%" AllowSorting="True" CellSpacing="1">
							<AlternatingItemStyle CssClass="CalendarBackground" VerticalAlign="Top"></AlternatingItemStyle>
							<ItemStyle VerticalAlign="Top"></ItemStyle>
							<HeaderStyle CssClass="dgHeader"></HeaderStyle>
							<Columns>
								<asp:TemplateColumn>
									<HeaderStyle CssClass="txtPlanDetailHotelRatesList"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<TABLE cellSpacing="0" cellPadding="0" border="0">
											<TR>
												<TD vAlign="top">
													<asp:Label id=lblItemIndex runat="server" CssClass="clsLabel" Text='<%# CInt(DataBinder.Eval(Container, "DataSetIndex")) + 1 %>'>
													</asp:Label></TD>
											</TR>
										</TABLE>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="ReservationNumber" HeaderText="Reservaci&#243;n">
									<HeaderStyle ></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<ItemTemplate>
										<asp:Label id=lblRvaNumber runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ReservationNumber") %>'>
										</asp:Label><BR>
										<asp:Label id=lblCustomerName runat="server" CssClass="clsLabelDataGrid" Text='<%# DataBinder.Eval(Container.DataItem, "CustomerName") %>'>
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="ReservationSourceName" HeaderText="Fuente">
									<HeaderStyle ></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<ItemTemplate>
										<asp:Label id=lblReservationSource runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ReservationSourceName") %>'>
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="AverageRate" HeaderText="Tarifa">
									<HeaderStyle ></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lblDgTarifa" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Tipo">
									<HeaderStyle ></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lblRateType" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="CheckIn" HeaderText="Llegada">
									<HeaderStyle ></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<ItemTemplate>
										<asp:Label id=lblCheckIn runat="server" Text='<%# Formatting.FormatDate(DataBinder.Eval(Container, "DataItem.CheckIn"), "MM/dd/yy", PortalCulture.GetCulture.Name) %>'>
										</asp:Label><BR>
										<asp:Label id="lblNights" runat="server" CssClass="clsLabelDataGrid"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn Visible="False" SortExpression="Total" HeaderText="Importe">
									<HeaderStyle ></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lblDgImporte" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="Amount" HeaderText="Cargo">
									<HeaderStyle ></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lblDgComAmount" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="TACommission" HeaderText="TA">
									<HeaderStyle ></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lblDgTA" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="ChargeOverTA" HeaderText="Cargo/TA">
									<HeaderStyle ></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lblDgChargeTA" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="Total" HeaderText="Total">
									<HeaderStyle ></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lblDgTotal" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn SortExpression="Status" HeaderText="Estado">
									<HeaderStyle ></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lblDgStatus" CssClass="txtPlanDetailHotelRatesList" runat="server" Text=""></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
			</TABLE>
		</TD>
	</TR>
	<TR>
		<TD class="tablaSumary" align="right">
			<TABLE id="Table5" border="0" cellSpacing="1" cellPadding="1">
				<TR id="trAmountCharges" runat="server">
					<TD align="right"><asp:label id="lblAmountChargesCaption" EnableViewState="False" CssClass="bookingNormalLabel"
							runat="server">Cargos:</asp:label></TD>
					<TD align="right"><asp:label id="lblAmountCharges" CssClass="clsLabel" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD align="right"><asp:label id="lblAmountCommissionsCaption" EnableViewState="False" CssClass="bookingNormalLabel"
							runat="server">Comisiones:</asp:label></TD>
					<TD align="right"><asp:label id="lblAmountCommissions" CssClass="clsLabel" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD colSpan="2">
						<HR class="SeparaBoton" color="#ff0000" SIZE="1" width="100%">
					</TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 17px" align="right"><asp:label id="lblSubtotalCaption" EnableViewState="False" CssClass="bookingNormalLabel" runat="server">Subtotal:</asp:label></TD>
					<TD style="HEIGHT: 17px" class="BigNameOfItem" align="right"><asp:label id="lblSubtotal" CssClass="clsLabel" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD align="right"><asp:label id="lblTaxesCaption" EnableViewState="False" CssClass="bookingNormalLabel" runat="server">Impuestos:</asp:label></TD>
					<TD class="BigNameOfItem" align="right"><asp:label id="lblTaxes" CssClass="clsLabel" runat="server"></asp:label></TD>
				</TR>
				<TR>
					<TD style="VERTICAL-ALIGN: middle" class="DataGridSelectedItem" align="right"><asp:label id="lblGrandTotalCaption" EnableViewState="False" CssClass="bookingNormalLabel"
							runat="server">Total:</asp:label></TD>
					<TD style="VERTICAL-ALIGN: middle" class="DataGridSelectedItem" align="right"><span class="BigNameOfItem"><asp:label id="lblGrandTotal" CssClass="clsLabel" runat="server"></asp:label></span></TD>
				</TR>
				<TR>
					<TD style="VERTICAL-ALIGN: middle" class="DataGridSelectedItem" align="right"><asp:label id="lblBalanceCaption" EnableViewState="False" CssClass="bookingNormalLabel" runat="server">Saldo:</asp:label></TD>
					<TD style="VERTICAL-ALIGN: middle" class="DataGridSelectedItem" align="right"><span class="BigNameOfItem"><asp:label id="lblBalance" CssClass="clsLabel" runat="server"></asp:label></span></TD>
				</TR>
			</TABLE>
		</TD>
	</TR>
</TABLE>
