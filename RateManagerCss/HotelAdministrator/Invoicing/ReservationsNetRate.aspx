<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../../Portal/Modules/ctrlHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrMenu" Src="../../Portal/Modules/CtrMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../../Portal/Modules/ctrlFooter.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlReservationsNetRateQuery" Src="../Modules/ctrlReservationsNetRateQuery.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ReservationsNetRate.aspx.vb" Inherits="RateManager.ReservationsNetRate" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Reservations</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK rel="stylesheet" type="text/css" href="../../StyleSheets/Styles.css">
	</HEAD>
	<body bottomMargin="0" leftMargin="0" rightMargin="0" topMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
		 <div class="clear">
        <div class="mDiv">
            </div>
            <div class="title">
                <asp:Label ID="lblTitle" runat="server" EnableViewState="False" 
                    CssClass="tituloSeccion">Consulta de reservaciones</asp:Label>
            </div>
        </div>
			<table id="bookingcontainer" border="0" cellSpacing="0" cellPadding="2" width="800">
				<tr>
					<td>
						<TABLE border="0" cellSpacing="0" cellPadding="0" width="100%" align="center">							
							<TR>
								<TD><uc1:ctrlreservationsnetratequery id="CtrlReservationsNetRateQuery1" runat="server"></uc1:ctrlreservationsnetratequery></TD>
							</TR>
							<TR>
								<TD style="HEIGHT: 13px" align="center"></TD>
							</TR>
							<tr height="5">
								<td></td>
							</tr>
							<TR>
								<TD align="center"><asp:datagrid id="Grid" runat="server" CssClass="DataGrid" PageSize="15" Width="95%" AllowPaging="True"
										AutoGenerateColumns="False" ShowFooter="True">
										<FooterStyle HorizontalAlign="Right"></FooterStyle>
										<SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
										<AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
										<ItemStyle CssClass="dgItem"></ItemStyle>
										<HeaderStyle CssClass="dgHeader"></HeaderStyle>
										<Columns>
											<asp:TemplateColumn HeaderText="Itinerario">
												<ItemTemplate>
													<asp:LinkButton id="lnkItinerario" runat="server" CssClass="dgLink" CommandName="DetalleReserva"
														CausesValidation="false">
														<%# DataBinder.Eval(Container, "DataItem.ReservacionNumber") %>
													</asp:LinkButton>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn DataField="ResevationDate" HeaderText="Fecha" DataFormatString="{0:MMM/dd/yyyy}"></asp:BoundColumn>
											<asp:BoundColumn DataField="Customer" HeaderText="Cliente"></asp:BoundColumn>
											<asp:BoundColumn DataField="CheckIn" HeaderText="Llegada" DataFormatString="{0:MMM/dd/yyyy}"></asp:BoundColumn>
											<asp:BoundColumn DataField="CheckOut" HeaderText="Salida" DataFormatString="{0:MMM/dd/yyyy}"></asp:BoundColumn>
											<asp:TemplateColumn HeaderText="Total">
												<ItemStyle HorizontalAlign="Right"></ItemStyle>
												<ItemTemplate>
													<asp:Label id="glblTotal" runat="server">-</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="Estatus">
												<ItemTemplate>
													<asp:Label id="glblEstatus" runat="server">-</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
										</Columns>
										<PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
											Position="Bottom" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
									</asp:datagrid></TD>
							</TR>
							<tr>
								<TD></TD>
							</tr>
							<tr height="5">
								<td align="center">
									<table id="TablaTotales" border="0" cellSpacing="0" cellPadding="0" width="95%" runat="server">
										<tr>
											<td align="right">
												<table border="0" cellPadding="2">
													<tr>
														<td><B><asp:label style="Z-INDEX: 0" id="lblNumeroReservacionesTitulo" Runat="server">Numero de reservaciones</asp:label></B></td>
														<td align="right"><asp:label id="lblNumeroReservaciones" Runat="server">0</asp:label></td>
													</tr>
													<tr>
														<td><b><asp:label style="Z-INDEX: 0" id="lblTotalTitulo" Runat="server">Total:</asp:label></b></td>
														<td align="right"><asp:label id="lblTotal" Runat="server">$0.00</asp:label></td>
													</tr>
												</table>
											</td>
										</tr>
									</table>
								</td>
							</tr>
						</TABLE>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
