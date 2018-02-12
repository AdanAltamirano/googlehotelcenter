<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../Modules/ctrlHeader.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Home.aspx.vb" Inherits="RateManager.Home"%>
<%@ Register TagPrefix="uc1" TagName="CtrMenu" Src="../Modules/CtrMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../Modules/ctrlFooter.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Home</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="bookingcontainer" cellSpacing="0" cellPadding="0" width="500" border="0">
				<TR>
					<td>
						<TABLE style="WIDTH: 375px" cellSpacing="1" cellPadding="1" width="375" align="center"
							border="0">
							<TR>
								<TD class="Titulo" align="center" colSpan="2"><asp:label id="lblTitulo" runat="server">Administrador de hotel</asp:label></TD>
							</TR>
							<tr>
								<td colSpan="2"><asp:dropdownlist id="cmbHotels" runat="server" AutoPostBack="True"></asp:dropdownlist><asp:label id="lblNameHotel" runat="server" CssClass="bookingNormalLabel" Font-Italic="True">Hotel puntita dorada</asp:label></td>
							</tr>
							<TR>
								<TD align="left" colSpan="2"></TD>
							</TR>
							<tr>
								<td colSpan="2">
									<TABLE id="tblPortal" cellSpacing="0" cellPadding="1" width="100%" border="0" runat="server">
										<TR>
											<TD colspan="2"><asp:label id="lblPortal" runat="server" CssClass="bookingNormalLabel" EnableViewState="False">Portal</asp:label></TD>
										</TR>
										<TR>
											<TD></TD>
											<TD align="left"><asp:hyperlink id="lnkSearch" runat="server" CssClass="dglink">Search</asp:hyperlink></TD>
										</TR>
										<TR>
											<TD></TD>
											<TD align="left"><asp:hyperlink id="lnkUsuarios" runat="server" CssClass="dglink">Usuarios</asp:hyperlink></TD>
										</TR>
										<TR>
											<TD></TD>
											<TD align="left"><asp:hyperlink id="hplRoomTypes" runat="server" CssClass="dglink" EnableViewState="False">Tipos de Habitación</asp:hyperlink></TD>
										</TR>
										<TR>
											<TD></TD>
											<TD align="left"><asp:hyperlink id="hplInfGral" runat="server" CssClass="dglink" EnableViewState="False">Información General</asp:hyperlink></TD>
										</TR>
									</TABLE>
								</td>
							</tr>
							<TR>
								<TD></TD>
								<TD align="left"></TD>
							</TR>
							<tr>
								<td colSpan="2">
									<table id="tblListReservation" cellSpacing="0" cellPadding="1" width="100%" border="0"
										runat="server">
										<TR>
											<TD colSpan="2"><asp:label id="lblReservaciones" runat="server" CssClass="bookingNormalLabel" EnableViewState="False">Reservaciones</asp:label></TD>
										</TR>
										<TR>
											<TD></TD>
											<TD align="left"><asp:hyperlink id="hplListado" runat="server" CssClass="dglink" EnableViewState="False">Listado</asp:hyperlink></TD>
										</TR>
										<TR>
											<TD></TD>
											<TD align="left"><asp:hyperlink id="hplConfirmRes" runat="server" CssClass="dglink" EnableViewState="False">Confirmar Reservaciones</asp:hyperlink></TD>
										</TR>
										<TR>
											<TD></TD>
											<TD align="left"><asp:hyperlink id="HplNoConfRes" runat="server" CssClass="dglink" EnableViewState="False">Reservaciones No confirmadas</asp:hyperlink></TD>
										</TR>
										<TR>
											<TD></TD>
											<TD align="left"><asp:hyperlink id="hplByChanel" runat="server" CssClass="dglink" EnableViewState="False">Resevaciones por canal de distribución</asp:hyperlink></TD>
										</TR>
										<TR>
											<TD></TD>
											<TD align="left"><asp:hyperlink id="hplInvoicing" runat="server" CssClass="dglink">Conciliación</asp:hyperlink></TD>
										</TR>
									</table>
								</td>
							</tr>
							<TR>
								<TD align="left" colSpan="2"><asp:label id="lblRooms" runat="server" CssClass="bookingNormalLabel" EnableViewState="False"> Rooms</asp:label></TD>
							</TR>
							<TR>
								<TD></TD>
								<TD align="left"><asp:hyperlink id="lnkRooms" runat="server" CssClass="dglink">Rooms</asp:hyperlink></TD>
							</TR>
							<TR>
								<TD></TD>
								<TD align="left"><asp:hyperlink id="lnkRoomslinks" runat="server" CssClass="dglink">Links</asp:hyperlink></TD>
							</TR>
							<TR>
								<TD align="left" colSpan="2"><asp:label id="lblRatesPlans" runat="server" CssClass="bookingNormalLabel" EnableViewState="False">Rates Plans</asp:label></TD>
							</TR>
							<TR>
								<TD></TD>
								<TD align="left"><asp:hyperlink id="lnkRateplanRules" runat="server" CssClass="dglink">Rules</asp:hyperlink></TD>
							</TR>
							<TR>
								<TD></TD>
								<TD align="left"><asp:hyperlink id="lnkRatePlans" runat="server" CssClass="dglink">rate plans</asp:hyperlink></TD>
							</TR>
							<TR>
								<TD></TD>
								<TD align="left"><asp:hyperlink id="lnkRatePlanLinks" runat="server" CssClass="dglink">links</asp:hyperlink></TD>
							</TR>
							<TR>
								<TD align="left" colSpan="2"><asp:label id="lblCatalogo" runat="server" CssClass="bookingNormalLabel" EnableViewState="False"> Fares</asp:label></TD>
							</TR>
							<TR>
								<TD></TD>
								<TD align="left"><asp:hyperlink id="lnkfaresCatalogue" runat="server" CssClass="dglink">Fares Catalogue</asp:hyperlink></TD>
							</TR>
							<TR>
								<TD></TD>
								<TD align="left"><asp:hyperlink id="hplFaresCopy" runat="server" CssClass="dglink">Fares Copy</asp:hyperlink></TD>
							</TR>
							<TR>
								<TD></TD>
								<TD align="left"><asp:hyperlink id="hplCreditCard" runat="server" CssClass="dglink">Credit Card</asp:hyperlink></TD>
							</TR>
							<TR>
								<TD align="left" colSpan="2"><asp:label id="lblInventory" runat="server" CssClass="bookingNormalLabel" EnableViewState="False">Inventory</asp:label></TD>
							</TR>
							<TR>
								<TD></TD>
								<TD align="left"><asp:hyperlink id="lnkHomeInventory" runat="server" CssClass="dglink">Home inventory</asp:hyperlink></TD>
							</TR>
							<TR>
								<TD></TD>
								<TD align="left"><asp:hyperlink id="lnkRatePlanInventory" runat="server" CssClass="dglink">By rate plan</asp:hyperlink></TD>
							</TR>
							<TR>
								<TD></TD>
								<TD align="left"><asp:hyperlink id="hplSoldOut" runat="server" CssClass="dglink" EnableViewState="False">Sold Out</asp:hyperlink></TD>
							</TR>
							<TR>
								<TD align="left" colSpan="2"><asp:label id="lblReports" runat="server" CssClass="bookingNormalLabel" EnableViewState="False">Reports</asp:label></TD>
							</TR>
							<TR>
								<TD></TD>
								<TD align="left"><asp:hyperlink id="lnkHomeStatus" runat="server" CssClass="dglink">Home Status</asp:hyperlink></TD>
							</TR>
							<TR>
								<TD></TD>
								<TD align="left"><asp:hyperlink id="lnkRatesChart" runat="server" CssClass="dglink">Rates Chart</asp:hyperlink></TD>
							</TR>

							<TR>
								<TD></TD>
								<TD align="left"><asp:hyperlink id="lnkNoDefinedRates" runat="server" CssClass="dglink">No Defined Rates</asp:hyperlink></TD>
							</TR>
							<TR>
								<TD style="HEIGHT: 10px"></TD>
								<TD style="HEIGHT: 10px" align="left"><asp:hyperlink id="hplLog" runat="server" CssClass="dglink">Log</asp:hyperlink></TD>
							</TR>
							<TR>
								<TD noWrap align="left" colSpan="2"><asp:label id="lblRegistro" runat="server" CssClass="bookingNormalLabel" EnableViewState="False"
										Visible="true">Registro:</asp:label></TD>
							</TR>
							<tr>
								<td></td>
								<td align="left">
									<TABLE id="TblRegistro" cellSpacing="0" cellPadding="1" width="100%" border="0" runat="server">
										<TR>
											<TD><asp:hyperlink id="hplChangeLogo" runat="server" CssClass="dglink" EnableViewState="False">Cambiar logo</asp:hyperlink></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD><asp:hyperlink id="lnkInfProp" runat="server" CssClass="dglink"> Información de la propiedad</asp:hyperlink></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD><asp:hyperlink id="lnkInfAmenities" runat="server" CssClass="dglink">Informacion de Amenidades</asp:hyperlink></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD><asp:hyperlink id="lnkActivities" runat="server" CssClass="dglink">Informacion de actividades</asp:hyperlink></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD><asp:hyperlink id="lnkAtraction" runat="server" CssClass="dglink">Informacion de recreaciones</asp:hyperlink></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD><asp:hyperlink id="hplPolicies" runat="server" CssClass="dglink" EnableViewState="False">Información de políticas</asp:hyperlink></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD><asp:hyperlink id="hplFood" runat="server" CssClass="dglink" EnableViewState="False">Información de Alimentos</asp:hyperlink></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD><asp:hyperlink id="hplRoomAmenities" runat="server" CssClass="dglink" EnableViewState="False">Información de Amenidades de cuartos</asp:hyperlink></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD><asp:hyperlink id="hplRooms" runat="server" CssClass="dglink" EnableViewState="False">Información de habitación</asp:hyperlink></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD><asp:hyperlink id="hplMapLocation" runat="server" CssClass="dglink" EnableViewState="False">Mapa de Ubicación</asp:hyperlink></TD>
											<TD></TD>
										</TR>
										<TR>
											<TD><asp:hyperlink id="hplphotos" runat="server" CssClass="dglink" EnableViewState="False">Photos</asp:hyperlink></TD>
											<TD></TD>
										</TR>
									</TABLE>
								</td>
							</tr>
							<TR>
								<TD colSpan="2"><asp:label id="lblCallCenter" runat="server" CssClass="bookingNormalLabel" EnableViewState="False">Call Center</asp:label></TD>
							</TR>
							<TR>
								<TD></TD>
								<TD align="left"><asp:hyperlink id="hplBooking" runat="server" CssClass="dglink" EnableViewState="False">Booking</asp:hyperlink></TD>
							</TR>
							<TR>
								<TD colSpan="2"><asp:label id="lblUsuarios" runat="server" CssClass="bookingNormalLabel" EnableViewState="False">Perfil de Usuario</asp:label></TD>
							</TR>
							<TR>
								<TD></TD>
								<TD align="left"><asp:hyperlink id="hplChangePassword" runat="server" CssClass="dglink" EnableViewState="False">Cambiar contraseña</asp:hyperlink></TD>
							</TR>
						</TABLE>
					</td>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
