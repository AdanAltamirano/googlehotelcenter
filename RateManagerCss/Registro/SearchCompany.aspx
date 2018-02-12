<%@ Page Language="vb" AutoEventWireup="false" Codebehind="SearchCompany.aspx.vb" Inherits="RateManager.SearchCompany"%>
<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../portal/Modules/ctrlHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../portal/Modules/ctrlFooter.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrMenu" Src="../portal/Modules/CtrMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlSearchCompany" Src="../Portal/Modules/ctrlSearchCompany.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>SearchCompany</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK rel="stylesheet" type="text/css" href="../StyleSheets/Styles.css">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<TABLE id="bookingcontainer" cellSpacing="0" cellPadding="2" width="650" border="0">
				<TR>
					<td>
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" align="center">
							<TR>
								<TD class="Titulo" align="center">
									<asp:Label id="lblTitulo" runat="server" EnableViewState="False">Buscar hotel</asp:Label></TD>
							</TR>
							<TR>
								<TD>
									<uc1:ctrlSearchCompany id="CtrlSearchCompany1" runat="server"></uc1:ctrlSearchCompany></TD>
							</TR>
							<TR>
								<TD align="center">
									<asp:Button id="btnSearch" runat="server" Text="Actualizar lista" CssClass="Button" EnableViewState="False"></asp:Button></TD>
							</TR>
							<tr height="5">
								<td></td>
							</tr>
							<TR>
								<TD align="center">
									<asp:datagrid id="Grid" runat="server" Width="99%" CssClass="DataGrid" PageSize="15" AllowPaging="True"
										AutoGenerateColumns="False" Border="0" CellSpacing="1" ShowFooter="True">
										<FooterStyle HorizontalAlign="Right"></FooterStyle>
										<SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
										<AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
										<ItemStyle CssClass="dgItem"></ItemStyle>
										<HeaderStyle CssClass="dgHeader"></HeaderStyle>
										<Columns>
											<asp:TemplateColumn HeaderText="Nombre&#160;Empresa">
												<ItemStyle Width="20%"></ItemStyle>
												<ItemTemplate>
													<asp:LinkButton id="lnkSelect" runat="server" CssClass="dgLink" CommandName="Select" CausesValidation="false">
														<%# databinder.eval(container.dataitem,"NombreEmpresa") %>
													</asp:LinkButton>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn Visible="False" DataField="idEmpresa"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="NombreEmpresa"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="Domicilio"></asp:BoundColumn>
											<asp:BoundColumn DataField="Estado" HeaderText="Estado">
												<ItemStyle Width="15%"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="Municipio" HeaderText="Municipio">
												<ItemStyle Width="10%"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="Ciudad" HeaderText="Ciudad">
												<ItemStyle Width="10%"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="Contacto_Nombre" HeaderText="Nombre contacto">
												<ItemStyle Width="10%"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="Contacto_Email" HeaderText="Email contacto">
												<ItemStyle Width="25%"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="Telefono" HeaderText="Telefono">
												<ItemStyle Width="10%"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="GUID_Registro" HeaderText="GUID_Registro"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="ID" HeaderText="id"></asp:BoundColumn>
										</Columns>
										<PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
											Position="Top" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
									</asp:datagrid></TD>
							</TR>
							<tr height="5">
								<td></td>
							</tr>
						</TABLE>
					</td>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
