<%@ Import NameSpace = "RateManager" %>
<%@ Register TagPrefix="uc1" TagName="CtrMenu" Src="../../Portal/Modules/CtrMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../../Portal/Modules/ctrlHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../../Portal/Modules/ctrlFooter.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="NoConfirmReservas.aspx.vb" Inherits="RateManager.NoConfirmReservas" %>
<%@ Register TagPrefix="uc1" TagName="ctrlReservationsQuery" Src="../Modules/ctrlReservationsQuery.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>NoConfirmReservas</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
		<script>
			function showReservas(idgrid,idtd)
			 { 
			  var dg=document.getElementById(idgrid);			  
			  if (dg)
			   {  if (dg.style.display=='none')
			        { dg.style.display = '';
			          document.getElementById(idtd).innerHTML = "-"; 
			        }
			      else
			      {
			        dg.style.display ='none';   
			        document.getElementById(idtd).innerHTML = "+";
			       }
			   }
			 }
		</script>
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
        <iframe id="gToday:normal:agenda.js" style="Z-INDEX: 999; LEFT: -500px; POSITION: absolute; TOP: -500px" 
            name="gToday:normal:agenda.js"
            src='<%=GeRequestApplicationPath(string.concat("/Calendar/",portalculture.getculture().Name.Substring(0, 2).ToLower(),"/ipopeng2.htm" )) %>'
            frameborder="0" width="174" scrolling="no" height="189">
        </iframe>		
		<form id="Form1" method="post" runat="server">
		
			<div class="clear">		    
		      <div class="mDiv"></div>
		        <div>
		            <asp:label id="lbltitle" runat="server" EnableViewState="False"  
                        Text="REPORTE DE RESERVACIONES NO CONFIRMADAS" CssClass=tituloSeccion ></asp:label> 
		        </div>
		    </div>  
			<table id="bookingcontainer" cellSpacing="0" cellPadding="2" width="650" border="0">
				<tr>
					<td>
						<TABLE cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">						
							<tr>
								<td></td>
								<td align="right"></td>
								<td align="left">
									<A hideFocus onclick="if(self.gfPop)gfPop.fPopCalendar(txtInicio);return false;" href="javascript:void(0)">
									</A>&nbsp;
								</td>
								<td align="left"></td>
								<TD align="right"><asp:checkbox id="chkAllHotels" runat="server" CssClass="clslabel" Text="Show all hotels"></asp:checkbox></TD>
							</tr>
							<TR>
								<TD colspan="5">
									<uc1:ctrlReservationsQuery id="CtrlReservationsQuery1" runat="server"></uc1:ctrlReservationsQuery></TD>
							</TR>
							<tr height="5">
								<td colspan="5"></td>
							</tr>
							<tr>
								<td width="90%" colSpan="5"><asp:datalist id="dlHoteles" runat="server" EnableViewState="true" Width="100%">
										<ItemTemplate>
											<TABLE id='tshow' runat="server" width="100%" Class="dgHeader" border="0">
												<TR class="dgHeader">
													<TD id='tdshow' runat="server" align="center" style="CURSOR: pointer" width="10">-</TD>
													<TD align="center"><%#databinder.eval(container,"DataItem.nombre")%></TD>
													<TD width="150" align="right"></TD>
												</TR>
											</TABLE>
											<%-- IMPORTANTE: NO PONER LA PAGINACION AL GRID --%>
											<asp:DataGrid id="dgReservas" runat="server"  GridLines="None" AutoGenerateColumns="False" Width="100%" EnableViewState="false"
												AllowPaging="false" PageSize="20" CssClass="DataGrid" >
												<AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
												<ItemStyle CssClass="dgItem"></ItemStyle>
												<HeaderStyle CssClass="dgHeader"></HeaderStyle>
												<Columns>
													<asp:BoundColumn HeaderText="Itinerary" DataField="NoReservacion"></asp:BoundColumn>
													<asp:BoundColumn HeaderText="Date" DataField="FechaReservacion" Visible="False"></asp:BoundColumn>
													<asp:BoundColumn HeaderText="Customer" DataField="TravelerName"></asp:BoundColumn>
													<asp:BoundColumn HeaderText="Arrival" DataField="CheckIn"></asp:BoundColumn>
													<asp:BoundColumn HeaderText="Departure" DataField="CheckOut"></asp:BoundColumn>
													<asp:BoundColumn HeaderText="Quantity" DataField="Cantidad" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
													<asp:BoundColumn Visible="False" HeaderText="Status" DataField="status"></asp:BoundColumn>
													<asp:TemplateColumn HeaderText="Status">
														<ItemTemplate>
															<asp:Label id="lblStatus" runat="server" EnableViewState="False"></asp:Label>
														</ItemTemplate>
													</asp:TemplateColumn>
													<asp:BoundColumn Visible="False" DataField="StatusConf" HeaderText="StatusConf"></asp:BoundColumn>
													<asp:BoundColumn Visible="False" DataField="WizcomPassOn" HeaderText="WizcomPassOn"></asp:BoundColumn>
													<asp:BoundColumn Visible="False" DataField="Idhotel"></asp:BoundColumn>
													<asp:BoundColumn Visible="False" DataField="Guaranteed"></asp:BoundColumn>
												</Columns>
												<PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
											      Position="Bottom" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
											</asp:DataGrid>
											<BR>
										</ItemTemplate>
									</asp:datalist></td>
							</tr>
						</TABLE>
					</td>
				</tr>
				<tr height="5">
					<td colspan="5"></td>
				</tr>
			</table>
		</form>
	</body>
	
</HTML>
