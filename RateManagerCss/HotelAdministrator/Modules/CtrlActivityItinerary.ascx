<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CtrlActivityItinerary.ascx.vb" Inherits="RateManager.CtrlActivityItinerary" %>
 <div class="packagesBlockContainer">
	<div id="lblActivity"  Class="tituloSeccion" runat="server">Activity 
		Information summary</div>
	<br>
	<table cellSpacing="1" cellPadding="1" width="99%" align="center" border="0">
		<tr>
			<td colSpan="2">
				<table width="99%">
					<TR>
						<TD><asp:label id="lblTrip" runat="server" EnableViewState="False" CssClass="labelReMark"></asp:label></TD>
					</TR>
					<tr>
						<td align="center"><asp:datalist id="lstTravelersFliht" runat="server" Width="99%" >
						        <SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
                                <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
                                <ItemStyle CssClass="dgItem"></ItemStyle>
                                <HeaderStyle HorizontalAlign="Center" CssClass="dgHeader" VerticalAlign="Middle">
                                </HeaderStyle>
								<ItemTemplate>
									<TABLE class="BorderTable" id="Table4" cellSpacing="0" cellPadding="0" width="99%" border="0">
										<tr>
											<td width="100%">
												<TABLE class="boxBestRatesinBooking" align="center" width="98%" border="0" runat="server"
													ID="Table1">
													<TR>
														<TD align="center">
															<asp:label id="lblRevNum" runat="server" CssClass="MiddleText">Reservation Number :</asp:label>
															<asp:label id="lblRvaNum" CssClass="MiddleTextBold" runat="server"></asp:label></TD>
													</TR>
													<TR>
														<td colspan="1">
															<h2>
																<asp:label id="PropertyName" runat="server" CssClass="bookingNormalLabel" EnableViewState="False">PropertyName :</asp:label></h2>
														</td>
														<td align="right" colspan="2">
															<asp:label id="lblStatus" runat="server" ForeColor="Red" Font-Bold="True">Reservado</asp:label></td>
													</TR>
												</TABLE>
											</td>
										</tr>
										<tr>
											<td>
												<TABLE width="50%" border="0" runat="server" ID="TblCustomer">
													<tr>
														<td>
																<asp:Label id="LblMainContact" runat="server" EnableViewState="False"
																	CssClass="MiddleText">Conductor:</asp:Label>
														</td>
														<td>
															<asp:Label id="MainContact" runat="server" DESIGNTIMEDRAGDROP="68" EnableViewState="False"
																 CssClass="MiddleTextBold"></asp:Label>
														</td>
													</tr>
													<tr>
														<td>
																<asp:Label id="LblMainContactMail" runat="server"
																	EnableViewState="False" CssClass="MiddleText">Conductor:</asp:Label>
														</td>
														<td>
															<asp:Label id="MainContactMail" runat="server" DESIGNTIMEDRAGDROP="68" EnableViewState="False"
																 CssClass="MiddleTextBold"></asp:Label>
														</td>
													</tr>
													<tr>
														<td>
																<asp:Label id="LblMainContactPhone" runat="server"
																	EnableViewState="False" CssClass="MiddleText">Conductor:</asp:Label>
														</td>
														<td>
															<asp:Label id="MainContactPhone" runat="server" DESIGNTIMEDRAGDROP="68" EnableViewState="False"
																 CssClass="MiddleTextBold"></asp:Label>
														</td>
													</tr>
													<tr>
														<td>
																<asp:Label id="lblEComision" runat="server"
																	EnableViewState="False" CssClass="MiddleText" Visible="false">Comisión :</asp:Label>
														</td>
														<td>
															<asp:Label id="lblComision" runat="server" DESIGNTIMEDRAGDROP="68" EnableViewState="False"
																 CssClass="MiddleTextBold" Visible="false"></asp:Label>
														</td>
													</tr>
												</TABLE>
											</td>
										</tr>
										<TR>
											<TD width="100%">
												<div id="contenedor" runat="server" class="MiddleTextBold"></div>
										</TR>
									</TABLE>
								</ItemTemplate>
							</asp:datalist></td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
	<br class="clear:both;">
	<div class="packagePriceDetails">
		<TABLE id="Table5" width="98%" align="center">
			<TR>
				<TD align="right"><asp:label id="Label4" runat="server" CssClass="MiddleText" EnableViewState="False">SubTotal:</asp:label></TD>
				<TD width="150" align="right"><asp:label id="lblTotal" runat="server" CssClass="txtDataBold">$0.0</asp:label><asp:label id="lblMonedaSubtotal" runat="server" CssClass="txtDataBold"></asp:label></TD>
			</TR>
			<TR style="DISPLAY:none">
				<TD align="right"><asp:label id="lblImpGob" runat="server" CssClass="MiddleText" EnableViewState="False">Impuesto Gobierno:</asp:label></TD>
				<TD align="right"><asp:label id="lblImpuestoGob" runat="server" CssClass="txtDataBold">$0.0</asp:label><asp:label id="lblMonedaImpuesto" runat="server" CssClass="txtDataBold"></asp:label></TD>
			</TR>
			<TR>
				<TD align="right"><asp:label id="lblEFees" runat="server" CssClass="MiddleText">Fees:</asp:label></TD>
				<TD align="right"><asp:label id="lblFees" runat="server" CssClass="txtDataBold">$0.00</asp:label><asp:label id="lblMonedaFees" runat="server" CssClass="txtDataBold" Visible="False"></asp:label></TD>
			</TR>
			<TR>
				<TD align="right"><asp:label id="lblETotalReservation" runat="server" CssClass="MiddleText">Total: </asp:label></TD>
				<TD align="right"><asp:label id="lblTotalReservation" runat="server" Font-Bold="True" CssClass="txtDataBold">$0.0</asp:label><asp:label id="lblMonedaTotal" runat="server" CssClass="MiddleTextBold"></asp:label></TD>
			</TR>
		</TABLE>
		<center>
			<asp:label id="lblMsgMoneda" runat="server" Visible="False"></asp:label>
			<asp:label id="lblMsgTotalMoneda" runat="server" Visible="False"></asp:label>
		</center>
		<center>
			<asp:label id="lblmensajeReservated" runat="server" Visible="False"></asp:label>
		</center>
		<div class="policiesBox">
			<ul>
				<li>
					<asp:label id="lblRR1" runat="server">Read an overview of all the</asp:label>
					<A id="lnkBtnRulesRestrctions" href="javascript:;" CssClass="ChoosePackageLink" runat="server">
						rules and restrictions</A>
					<asp:label id="lblRR2" runat="server">applicable to this fare.</asp:label>
				</li>
			</ul>
		</div>
		<div align="right">
			<asp:panel id="PanelCancel" runat="server">
				<INPUT id="btnCancel" class="btnDefault" onclick="javascript:ActShowConfirm();" value="Cancel Reservation"
					type="button" name="btnCancel" runat="server" visible="false"></asp:panel><BR>
		</div>
		<DIV id="ActConfirmCancel" style="DISPLAY: none">
			<TABLE class="tblOrderListBox" id="Table7" align="center" width="98%" bgColor="#fffff1"
				border="0">
				<TR>
					<TD class="CalendarTitle" align="center" colSpan="3"><asp:label id="lblConfirmTitle" CssClass="CalendarTitle" runat="server" Font-Bold="True" BackColor="Transparent">Confirm cancellation</asp:label></TD>
				</TR>
				<TR>
					<TD class="tblOrderListBox" style="HEIGHT: 31px" align="center" colSpan="3"><asp:label id="lblConfirmPrompt" CssClass="txtDataBold" runat="server">Are you sure you want to cancel this reservation?<br>This action is irreversible.</asp:label></TD>
				</TR>
				<TR>
					<TD class="tblOrderListBox" align="center" width="50%"><asp:linkbutton id="hplCancelRes" runat="server" CssClass="claros">Yes, cancel this reservation </asp:linkbutton></TD>
					<TD align="right"></TD>
					<TD class="tblOrderListBox" align="center" width="50%"><asp:hyperlink id="lnkNo" runat="server" CssClass="claros" NavigateUrl="javascript:ActHideConfirm();">No, do not cancel this reservation </asp:hyperlink></TD>
				</TR>
			</TABLE>
			<BR>
		</DIV>
	</div>
</div>