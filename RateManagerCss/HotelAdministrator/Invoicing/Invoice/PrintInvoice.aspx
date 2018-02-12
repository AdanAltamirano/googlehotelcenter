<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../../../Portal/Modules/ctrlFooter.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="PrintInvoice.aspx.vb" Inherits="RateManager.PrintInvoice" %>
<%@ Import NameSpace = "RateManager"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>PrintInvoice</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../../../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="5" rightMargin="0">
		<form id="Form1" method="post" runat="server">
		<div class="mDiv">
            </div>
            <div class="title">
                <asp:Label ID="lblTitle" runat="server" EnableViewState="False" 
                    CssClass="tituloSeccion">Impresion de Facturas Prueba</asp:Label>
            </div>
        </div>
			<TABLE id="bookingcontainer" cellSpacing="0" cellPadding="0" width="650" align="center"
				border="0">				
				<TR>
					<TD colSpan="3">
						<TABLE id="Table4" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD align="center">
									<TABLE id="Table3" cellSpacing="3" cellPadding="1" width="100%" border="0">
										<TR>
											<TD colspan="3" align="center"><asp:label id="lblshow" runat="server">Filtrar</asp:label>
												&nbsp;<asp:dropdownlist id="ddlfiltro" runat="server">
													<asp:ListItem Value="0">Todas</asp:ListItem>
													<asp:ListItem Value="1">Activas</asp:ListItem>
													<asp:ListItem Value="2">Canceladas</asp:ListItem>
												</asp:dropdownlist>&nbsp;
												<asp:button id="btnshow" runat="server" Text="Mostrar" CssClass="button"></asp:button></TD>
										</TR>
										<TR>
											<TD align="center" colSpan="3"><asp:label id="lblMsg" runat="server" CssClass="Validators" Height="100%" Visible="False">No hay facturas para mostrar</asp:label></TD>
										</TR>
										<TR>
											<TD align="center" colSpan="3"><asp:datagrid id="dgInvoices" runat="server" EnableViewState="False" CssClass="DataGrid" AutoGenerateColumns="False"
													BorderColor="WhiteSmoke" Width="100%">
													<AlternatingItemStyle CssClass="dgalternate"></AlternatingItemStyle>
													<ItemStyle CssClass="dgitem"></ItemStyle>
													<HeaderStyle CssClass="dgheader"></HeaderStyle>
													<Columns>
														<asp:TemplateColumn>
															<ItemTemplate>
																<asp:Label id=lblIndex runat="server" CssClass="clsLabel" Text='<%# cInt(DataBinder.Eval(Container, "ItemIndex"))+1 %>'>
																</asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn HeaderText="No. de Referencia de la Factura">
															<ItemTemplate>
																<asp:Label id="lblReferenceNumber" runat="server" CssClass="clsLabel"></asp:Label>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn HeaderText="Descargar">
															<ItemTemplate>
																<TABLE id="Table5" cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">
																	<TR>
																		<TD align="center" height="15">
																			<asp:HyperLink id=hypPdf runat="server" CssClass="Link" ImageUrl='<%# GeRequestApplicationPath("/Images/pdf.gif") %>' ToolTip='<%# PortalCulture.GetString("M0BT0000339") %>'>
																			</asp:HyperLink></TD>
																		<TD width="20"></TD>
																		<TD align="center" height="15">
																			<asp:HyperLink id=hypXml runat="server" CssClass="Link" ImageUrl='<%# GeRequestApplicationPath("/Images/xml.gif") %>' ToolTip='<%# PortalCulture.GetString("M0BT0000340") %>'>
																			</asp:HyperLink></TD>
																	</TR>
																</TABLE>
															</ItemTemplate>
														</asp:TemplateColumn>
													</Columns>
												</asp:datagrid></TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
							<TR>
								<TD><asp:hyperlink id="hplGoToTaskList" runat="server" EnableViewState="False" CssClass="dgLink">
												Ir a Lista de Tareas
												</asp:hyperlink></TD>
							</TR>
							<TR>
								<TD height="20"></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
