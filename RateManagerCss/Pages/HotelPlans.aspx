<%@ Page Language="vb" AutoEventWireup="false"  validaterequest="false"  Codebehind="HotelPlans.aspx.vb" Inherits="RateManager.HotelPlans" %>
<%@ Register TagPrefix="uc2" TagName="ctrlHeader" Src="../Portal/Modules/ctrlHeader.ascx" %>
<%@ Register TagPrefix="uc2" TagName="ctrlFooter" Src="../Portal/Modules/ctrlFooter.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrMenu" Src="../Portal/Modules/CtrMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlHotelPlans" Src="../Modulos/ctrlHotelPlans.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Planes de Hotel</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="5" rightMargin="0">
		<script>
			function ShowToolDescription(txt)
			{
				var l = document.getElementById("lblTool");
				l.innerHTML= txt;
			}
			
			function HideToolDescription()
			{	var l = document.getElementById("lblTool");
				l.innerHTML= "";
			}		
		</script>
		<form id="Form1" method="post" runat="server">
			<TABLE id="bookingcontainer" width="650" cellSpacing="0" cellPadding="0" border="0">			
				<tr>			
					<td>
						<TABLE  id="Table2" cellSpacing="1" cellPadding="1" width="100%" border="0"
							align="center">
							<TR>
								<TD class="Titulo">
									<asp:label id="lbltitle" runat="server">Planes</asp:label></TD>
							</TR>
							<TR>
								<TD></TD>
							</TR>
							<TR>
								<TD align="center">
									<asp:datagrid id="dtgPlans" PageSize="5" runat="server" CssClass="DataGrid" Width="100%" AllowPaging="True"
										AutoGenerateColumns="False" BorderWidth="0px" CellSpacing="1">
										<SelectedItemStyle Cssclass="dgSelected"></SelectedItemStyle>
										<AlternatingItemStyle Cssclass="dgAlternate"></AlternatingItemStyle>
										<ItemStyle Cssclass="dgItem"></ItemStyle>
										<HeaderStyle Cssclass="dgHeader"></HeaderStyle>
										<Columns>
											<asp:BoundColumn HeaderText="Plan">
												<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn Visible="False" HeaderText="idPlan">
												<HeaderStyle Width="50px"></HeaderStyle>
											</asp:BoundColumn>
											<asp:TemplateColumn>
												<HeaderStyle HorizontalAlign="Center" Width="50px"></HeaderStyle>
												<ItemStyle Width="50px"></ItemStyle>
												<HeaderTemplate>
													<DIV id="lblTool" style="DISPLAY: inline; WIDTH: 80px" ms_positioning="FlowLayout"></DIV>
												</HeaderTemplate>
												<ItemTemplate>
													<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
														<TR>
															<TD align="center">
																<asp:ImageButton id="ibtnEdit" onmouseover="ShowToolDescription(this.title)" onmouseout="HideToolDescription()"
																	runat="server" CausesValidation="False" CommandName="Select" ImageUrl="Images/edit.gif" ToolTip="Editar"></asp:ImageButton></TD>
														</TR>
													</TABLE>
												</ItemTemplate>
											</asp:TemplateColumn>
										</Columns>
										<PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
											Position="Top" Cssclass="dgPager"></PagerStyle>
									</asp:datagrid></TD>
							</TR>
							<TR>
								<TD align="center">
									<uc1:ctrlHotelPlans id="CtrlHotelPlans1" runat="server"></uc1:ctrlHotelPlans></TD>
							</TR>
							<TR>
								<TD align="center">
									<asp:Label id="lblError" runat="server" CssClass="Validators">No se pudo eliminar el plan</asp:Label></TD>
							</TR>
							<TR>
								<TD align="center">
									<HR width="100%" SIZE="1">
									<asp:button id="btnNew" runat="server" CssClass="Button" CausesValidation="False" Text="Nuevo"
										Width="85px"></asp:button>
									<asp:button id="btnDelete" runat="server" CssClass="Button" CausesValidation="False" Text="Eliminar"
										Enabled="False" Width="85px"></asp:button>
									<asp:button id="btnSave" runat="server" CssClass="Button" Text="Guardar" Width="85px"></asp:button>
								</TD>
							</TR>
						</TABLE>
					</td>
				</tr>				
			</TABLE>
			&nbsp;
		</form>
	</body>
</HTML>
