<%@ Page Language="vb" AutoEventWireup="false" Codebehind="TarjetasHotel.aspx.vb" Inherits="RateManager.TarjetasHotel" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>TarjetasHotel</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../../StyleSheets/Styles.css" type="text/css" rel="stylesheet" runat="server">
		</LINK>
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="5" rightMargin="0">
		<form id="Form1" method="post" runat="server">
		 <div class="clear">
            <div class="mDiv">
            </div>
            <div class="title">
                <asp:Label ID="lblTitulo" runat="server" EnableViewState="False" Text="Tarjetas Aceptadas" CssClass="tituloSeccion"></asp:Label>
            </div>
         </div>
			<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="770" border="0">
				<TR height="300">
					<TD align="center">
						<TABLE id="Bookingcontainer" cellSpacing="0" cellPadding="2" width="300" border="0">						
							<TR>
								<TD align="left" colSpan="2" height="5"></TD>
							</TR>
							<TR>
								<TD colSpan="2" align="center"><asp:datagrid id="dgTarjetas" runat="server" CssClass="DataGrid" AutoGenerateColumns="False" Width="250px">
										<AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
										<ItemStyle CssClass="dgitem"></ItemStyle>
										<HeaderStyle CssClass="dgHeader"></HeaderStyle>
										<Columns>
											<asp:TemplateColumn>
												<ItemTemplate>
													<asp:CheckBox id="cbTarjeta" runat="server"></asp:CheckBox>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn DataField="Codigo" HeaderText="Codigo"></asp:BoundColumn>
											<asp:BoundColumn DataField="Nombre" HeaderText="Descripcion"></asp:BoundColumn>
										</Columns>
										<PagerStyle CssClass="dgpager"></PagerStyle>
									</asp:datagrid></TD>
							</TR>
							<TR>
								<TD colSpan="2" height="5"></TD>
							</TR>
							<TR>
								<TD colSpan="2" height="5" align="center">
									<asp:button id="btnSave" runat="server" Text="Guardar" CssClass="Button" EnableViewState="False"></asp:button></TD>
							</TR>
							<TR>
								<TD colSpan="2"></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
