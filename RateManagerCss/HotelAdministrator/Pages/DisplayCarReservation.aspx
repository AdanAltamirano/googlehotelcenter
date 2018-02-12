<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../../Portal/Modules/ctrlFooter.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrMenu" Src="../../Portal/Modules/CtrMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../../Portal/Modules/ctrlHeader.ascx" %>
<%@ Import NameSpace = "RateManager" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="DisplayCarReservation.aspx.vb" Inherits="RateManager.DisplayCarReservation"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>ReservationDetails</title>
		<script>
		
		function ShowConfirm()
		{
		 document.getElementById('ConfirmCancel').style.display='block';
		}
				
		function HideConfirm()
		{
		 document.getElementById('ConfirmCancel').style.display='none';
		}
		    
   

		</script>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
		<style> @media Print { .lbltc { DISPLAY: none }}
		</style>
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="5" rightMargin="0">
		<form id="Form1" method="post" runat="server">
			<table id="TblCarReservation" cellSpacing="1" cellPadding="0" width="710" border="0" runat="server">
				<tr>
					<TD align="center" colSpan="3"></TD>
					<td>
						<TABLE cellSpacing="0" cellPadding="0" width="100%" align="center" border="0" id="bookingcontainer">
							<TR>
								<TD class="titulo" align="center" colSpan="3"><asp:label id="lblConfirm" runat="server" Font-Bold="True" EnableViewState="False">Confirm Reservation</asp:label></TD>
							</TR>
							<TR>
								<td style="HEIGHT: 12px"><a class="dglink" onclick="window.close();return false;"><%=PortalCulture.GetString("00151") %></a></td>
								<TD style="HEIGHT: 12px" align="right" colSpan="2"><asp:label id="lblWizcom" runat="server" EnableViewState="False" CssClass="bookingNormalLabel"
										Visible="False">Esta reservación fue hecha por el switch</asp:label></TD>
							</TR>
							<TR>
								<TD style="HEIGHT: 13px" align="left"></TD>
								<TD style="HEIGHT: 13px" align="left"></TD>
								<TD style="HEIGHT: 13px" align="right"><asp:label id="lblEStatus" runat="server" CssClass="bookingNormalLabel" Font-Size="8pt">Status</asp:label><asp:label id="lblStatus" runat="server" EnableViewState="False" CssClass="bookingNormalLabel"
										Font-Size="8pt">Reserved</asp:label></TD>
							</TR>
							<TR>
								<TD align="center"></TD>
								<TD align="left"></TD>
								<TD align="right"><asp:label id="lblNoCanc" runat="server" EnableViewState="False" CssClass="clslabel" Visible="false">No. Cancel</asp:label><asp:label id="lblNoCancelacion" runat="server" EnableViewState="False" CssClass="bookingNormalLabel"></asp:label><BR>
								</TD>
							</TR>
							<TR>
								<TD align="left" valign="top">
									<b><asp:label id="CompanyName" runat="server" EnableViewState="False" CssClass="clslabel">Aga Renta a Car</asp:label></b>
									<asp:label id="lblNHotel" runat="server" EnableViewState="False" CssClass="bookingNormalLabel"
										Font-Size="9pt">Datos Autos</asp:label><br>
									<asp:label id="lblDetCar" runat="server" EnableViewState="False" CssClass="clslabel">Detalle auto</asp:label><br>
									<asp:label id="lblDetCarSure" runat="server" EnableViewState="False" CssClass="clsLabel">Detalle Seguro</asp:label></TD>
								<TD align="left" valign="top"><asp:label id="lblEIn" runat="server" EnableViewState="False" CssClass="clsLabel" Width="123px"> Check-In Date:</asp:label><asp:label id="lblIn" runat="server" Font-Bold="True" EnableViewState="False" CssClass="clsLabel"
										Width="96px">21 Aug 05</asp:label><br>
									<asp:label id="lblEOut" runat="server" EnableViewState="False" CssClass="clsLabel" Width="128px">Check-Out Date :</asp:label><asp:label id="lblOut" runat="server" Font-Bold="True" EnableViewState="False" CssClass="clsLabel"
										Width="97px">26 Aug 05</asp:label>
									</TD>
								<TD align="left" valign="top">
									<asp:label id="lblReservationData" runat="server" EnableViewState="False" CssClass="bookingNormalLabel">Reservation data</asp:label>
									<br />
									<asp:label id="lblEID" runat="server" EnableViewState="False" CssClass="clsLabel">Reservation ID :</asp:label>
									<asp:label id="lblID" runat="server" Font-Bold="True" EnableViewState="False" CssClass="bookingNormalLabel">1520124585</asp:label>
									<br />
									<asp:label id="lblERecordLocator" runat="server" EnableViewState="False" CssClass="clsLabel">RecLoc:</asp:label>
									<asp:label id="lblRecordLocator" runat="server" Font-Bold="True" EnableViewState="False" CssClass="clsLabel"></asp:label>
									
								</TD>
							</TR>
							<tr>
								<td>
								&nbsp;
								</td>
							</tr>
							<tr>
							<TD align="left" valign="top">
								<asp:label id="lblRaG" runat="server" EnableViewState="False" CssClass="bookingNormalLabel"> Conductor</asp:label><br>
									<asp:label id="lblEDriver" runat="server" EnableViewState="False" CssClass="clsLabel">Nombre conductor</asp:label>
							</td>							
							<TD align="left" valign="top">
								<asp:label id="lblAdress" runat="server" EnableViewState="False" CssClass="bookingNormalLabel">Dirreccion</asp:label>	
								<br />
								<asp:label id="lblAdressValue" runat="server" EnableViewState="False" >Roll Away Data:</asp:label>
							</TD>
							</tr>
							
							<tr>
								<td>
								&nbsp;
								</td>
							</tr>
							
							<TR>
								<TD align="center" colSpan="3"><asp:label id="lblCostos" runat="server" EnableViewState="False" CssClass="bookingNormalLabel"
										Font-Size="8pt"> Cost and Travel Summary</asp:label></TD>
							</TR>
							<TR>
								<TD align="center" colSpan="3">
									<TABLE id="Table2" cellSpacing="0" cellPadding="0" border="0">
										<TR>
											<TD align="left"><asp:label id="lblTarifBaseo" runat="server" EnableViewState="False" CssClass="clsLabel">Tarifa base</asp:label></TD>
											<TD align="right">&nbsp;<asp:label id="lblBase" runat="server" EnableViewState="False" CssClass="clslabel">$ X.00</asp:label></TD>
										</TR>
										<TR>
											<TD align="left"><asp:label id="lblSegurosText" runat="server" EnableViewState="False" CssClass="clsLabel">Seguros</asp:label></TD>
											<TD align="right">&nbsp;<asp:label id="lblSeguros" runat="server" EnableViewState="False" CssClass="clslabel">$ X.00 </asp:label></TD>
										</TR>
										<TR>
											<TD align="left"><asp:label id="lblETax" runat="server" EnableViewState="False" CssClass="clsLabel">Impuestos:</asp:label></TD>
											<TD align="right">&nbsp;<asp:label id="lblTax" runat="server" EnableViewState="False" CssClass="clslabel">$ 10.00</asp:label></TD>
										</TR>
										<TR>
											<TD align="left"><asp:label id="lblETotal" runat="server" EnableViewState="False" CssClass="clsLabel">Total :</asp:label></TD>
											<TD align="right">&nbsp;<asp:label id="lblTotal" runat="server" EnableViewState="False" CssClass="clslabel">$ X.00</asp:label></TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
						</TABLE>
					</td>
				</tr>
			</table>
			<table id="TblActivitieReservation" cellSpacing="1" cellPadding="0" width="600" border="0"
				runat="server">
				<tr>
					<td width="100%">
						<table id="bookingcontainer" width="100%">
							<TR>
								<TD class="titulo" align="center" colSpan="3"><asp:label id="lbltituloactividad" runat="server" Font-Bold="True" EnableViewState="False">Confirm Reservation</asp:label></TD>
							</TR>
							<TR>
								<td style="HEIGHT: 12px"><a class="dglink" onclick="window.close();return false;"><%=PortalCulture.GetString("00151") %></a></td>
							</TR>
							<tr>
								<td>
									<asp:datalist id="lstTravelersFliht" runat="server" Width="100%">
										<ItemTemplate>
											<TABLE class="BorderTable" id="Table4" cellSpacing="0" cellPadding="0" width="99%" border="0">
												<tr>
													<td width="100%">
														<TABLE class="boxBestRatesinBooking" align="center" width="98%" border="0" runat="server"
															ID="Table1">
															<TR>
																<td colspan="1">
																	<h2>
																		<asp:label id="PropertyName" runat="server" CssClass="LabelBold" EnableViewState="False">PropertyName :</asp:label></h2>
																</td>
																<TD align="center">
																	<asp:label id="lblRevNum" runat="server">Reservation Number :</asp:label>
																	<asp:label id="lblRvaNum" CssClass="labelBold" runat="server"></asp:label></TD>
																<td align="right" colspan="2">
																	<asp:label id="Label1" Visible="False" runat="server" ForeColor="Red" Font-Bold="True">Reservado</asp:label></td>
															</TR>
															<tr>
																<td>
																	<h3>
																		<asp:label id="lblEventoDescriptcion" runat="server" ForeColor="Red" Font-Bold="True">Evento(s)</asp:label></h3>
																</td>
															</tr>
														</TABLE>
													</td>
												</tr>
												<tr style="display:none">
													<td>
														<TABLE width="98%" border="0" runat="server" ID="TblCustomer">
															<tr>
																<td>
																	<B>
																		<asp:Label id="LblMainContact" runat="server" DESIGNTIMEDRAGDROP="350" Font-Bold="True" EnableViewState="False"
																			CssClass="textolibre">Conductor:</asp:Label></B>
																</td>
																<td>
																	<asp:Label id="MainContact" runat="server" DESIGNTIMEDRAGDROP="68" EnableViewState="False"
																		CssClass="textolibre"></asp:Label>
																</td>
															</tr>
															<tr>
																<td>
																	<B>
																		<asp:Label id="LblMainContactMail" runat="server" DESIGNTIMEDRAGDROP="350" Font-Bold="True"
																			EnableViewState="False" CssClass="textolibre">Conductor:</asp:Label></B>
																</td>
																<td>
																	<asp:Label id="MainContactMail" runat="server" DESIGNTIMEDRAGDROP="68" EnableViewState="False"
																		CssClass="textolibre"></asp:Label>
																</td>
															</tr>
															<tr>
																<td>
																	<B>
																		<asp:Label id="LblMainContactPhone" runat="server" DESIGNTIMEDRAGDROP="350" Font-Bold="True"
																			EnableViewState="False" CssClass="textolibre">Conductor:</asp:Label></B>
																</td>
																<td>
																	<asp:Label id="MainContactPhone" runat="server" DESIGNTIMEDRAGDROP="68" EnableViewState="False"
																		CssClass="textolibre"></asp:Label>
																</td>
															</tr>
														</TABLE>
													</td>
												</tr>
												<TR>
													<TD width="100%">
														<div id="contenedor" runat="server"></div>
												</TR>
											</TABLE>
										</ItemTemplate>
									</asp:datalist>
								</td>
							</tr>
							<TR>
								<TD align="center" colSpan="3"><asp:label id="lbltitulototalactividad" runat="server" EnableViewState="False" CssClass="bookingNormalLabel"
										Font-Size="8pt"> Cost and Travel Summary</asp:label></TD>
							</TR>
							<TR>
								<TD align="center" colSpan="3">
									<TABLE id="Table2" cellSpacing="0" cellPadding="0" border="0">
										<TR>
											<TD align="right"><asp:label id="lblSubtotalActivitie" runat="server" EnableViewState="False" CssClass="clsLabel">Tarifa base</asp:label></TD>
											<TD align="left"><asp:label id="SubtotalActivitie" runat="server" EnableViewState="False" CssClass="clslabel">$ X.00</asp:label></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD align="right"><asp:label id="lblTaxActivitie" runat="server" EnableViewState="False" CssClass="clsLabel">Impuestos:</asp:label></TD>
											<TD align="left"><asp:label id="TaxActivitie" runat="server" EnableViewState="False" CssClass="clslabel">$ 10.00</asp:label></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD align="right"><asp:label id="lblTotalActivitie" runat="server" EnableViewState="False" CssClass="clsLabel">Total :</asp:label></TD>
											<TD align="left"><asp:label id="TotalActivitie" runat="server" EnableViewState="False" CssClass="clslabel">$ X.00</asp:label></TD>
											<TD></TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
						</table>
					</td>
				</tr>
			</table>
			<br>
			<table cellSpacing="1" cellPadding="0" width="600" border="0">
				<TR>
					<TD align="center" colSpan="3"><INPUT class=button id=btnPrint onclick=window.print(); type=button value='<%=PortalCulture.GetString("00606") %>'></TD>
				</TR>
			</table>
			<asp:label id="lblCompanyName" runat="server" EnableViewState="False" CssClass="bookingNormalLabel"
										Visible="False" Font-Size="9pt">Nombre Autorenta</asp:label>
		</form>
	</body>
</HTML>
