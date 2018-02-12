<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ReservationsV2.aspx.vb" Inherits="RateManager.ReservationsV2" %>
<%@ Register TagPrefix="uc1" TagName="ctrlReservationsQuery" Src="../Modules/ctrlReservationsQueryV2.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../../Portal/Modules/ctrlFooter.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrMenu" Src="../../Portal/Modules/CtrMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../../Portal/Modules/ctrlHeader.ascx" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Reservations</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
		
		
        <script type="text/javascript">
           
        </script>

	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
		    <div class="clear">		    
		        <div class="mDiv"></div>
		        <div>
		            <asp:label id="lblTitle" runat="server" EnableViewState="False"  
                        Text="Consulta de reservaciones" CssClass=tituloSeccion ></asp:label> 
		        </div>
		    </div>  
			<table id="bookingcontainer" cellSpacing="0" cellPadding="2" width="650" border="0">
				<tr>
					<td>
						<TABLE cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">
							<TR>
								<TD><uc1:ctrlreservationsquery id="CtrlReservationsQuery1" runat="server"></uc1:ctrlreservationsquery></TD>
							</TR>
							<TR>
								<TD align="center"></TD>
							</TR>
							<tr height="5">
								<td></td>
							</tr>
							<TR>
								<TD align="center"></TD>
							</TR>
							<tr height="5">
								<td></td>
							</tr>
						</TABLE>
					</td>
				</tr>
			</table>
			<table style="width:100%;"><tr><td>
			<asp:datagrid id="Grid"  GridLines="None" runat="server" CssClass="DataGrid" ShowFooter="True" CellSpacing="0" AutoGenerateColumns="False"
										AllowPaging="True" Width="95%" PageSize="20">
										<SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
										<AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
										<ItemStyle CssClass="dgItem"></ItemStyle>
										<HeaderStyle CssClass="dgHeader"></HeaderStyle>
										<FooterStyle HorizontalAlign="Right"></FooterStyle>
										<Columns>
											<asp:TemplateColumn HeaderText="Itinerario">
												<ItemTemplate>
													<asp:LinkButton id="lnkItinerario" runat="server" CausesValidation="false" CommandName="DetalleReserva"
														CssClass="dgLink">
														<%# DataBinder.Eval(Container, "DataItem.NoReservacion") %>
													</asp:LinkButton>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
											<asp:BoundColumn DataField="FechaReservacion" HeaderText="Fecha"></asp:BoundColumn>
											<asp:BoundColumn DataField="TravelerName" HeaderText="Cliente"></asp:BoundColumn>
											<asp:BoundColumn DataField="CheckIn" HeaderText="Llegada"></asp:BoundColumn>
											<asp:BoundColumn DataField="CheckOut" HeaderText="Salida"></asp:BoundColumn>
											<asp:BoundColumn DataField="NombreHabitacion" HeaderText="Tipo de habitaci&#243;n"></asp:BoundColumn>
											<asp:BoundColumn DataField="Cantidad" HeaderText="Hab.">
												<ItemStyle HorizontalAlign="Center"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="status" HeaderText="Status"></asp:BoundColumn>
											<asp:TemplateColumn HeaderText="Status">
												<ItemTemplate>
													<asp:Label id="lblStatus" runat="server"></asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn Visible="False" DataField="StatusConf" HeaderText="StatusConf"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="WizcomPassOn" HeaderText="WizcomPassOn"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="Guaranteed" HeaderText="Guaranteed"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="earlyOut" HeaderText="EarlyCheckOut"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="noshow" HeaderText="NoShow"></asp:BoundColumn>
										</Columns>
										<PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
											Position=Bottom  CssClass="dgPager" Mode="NumericPages"></PagerStyle>
									</asp:datagrid>
			
			</td></tr></table>
		</form>
	</body>
</HTML>
