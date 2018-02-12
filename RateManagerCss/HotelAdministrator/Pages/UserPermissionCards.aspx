<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UserPermissionCards.aspx.vb" Inherits="RateManager.UserPermissionCards" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>TarjetasHotel</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK id="LINK1" href="../../StyleSheets/Styles.css" type="text/css" rel="stylesheet" runat="server">
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
							    <td align="right">
                                    <asp:Label ID="lblNombreUsuario" EnableViewState="False" CssClass="clsLabel" runat="server">Nombre de Usuario:</asp:Label>&nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEmailSearch" CssClass="textbox" runat="server" 
                                        MaxLength="80" Width="200px"></asp:TextBox>
                                </td>
                                <td align="left">
                                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="Button" EnableViewState="False" />
                                </td>
							</TR>
							<TR>
								<TD>
                                    <td align="center">
                                        <asp:Literal ID="ltlUserName" runat="server" Visible="false"></asp:Literal>
                                        <asp:DataGrid ID="Grid"  GridLines="None" runat="server" Width="99%" CssClass="DataGrid" PageSize="15"
                                            AllowPaging="True" AutoGenerateColumns="False" Border="0" CellSpacing="1" ShowFooter="True" DataKeyField="idusuario">
                                            <FooterStyle HorizontalAlign="Right"></FooterStyle>
                                            <SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
                                            <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
                                            <ItemStyle CssClass="dgItem"></ItemStyle>
                                            <HeaderStyle CssClass="dgHeader"></HeaderStyle>
                                            <Columns>
                                                <asp:BoundColumn DataField="idusuario" HeaderText="idusuario" Visible="false"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="Email" HeaderText="Email"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="Nombre" HeaderText="Nombre"></asp:BoundColumn>
                                                <asp:TemplateColumn Visible="true" HeaderText="Permission">
													<ItemStyle HorizontalAlign="Center"></ItemStyle>
													<ItemTemplate>
														<asp:CheckBox id="chkAdd" runat="server"></asp:CheckBox>
													</ItemTemplate>
												</asp:TemplateColumn>
                                            </Columns>
                                            <PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
                                                Position="Bottom" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
                                        </asp:DataGrid>
                                    </td>
								</TD>
							</TR>
							<TR>
								<TD colSpan="2" height="5" align="center">
									<asp:button id="btnGuardar" runat="server" Text="Guardar" CssClass="Button" EnableViewState="False" Visible="false"></asp:button>
									<asp:Label ID="lblMensajes" runat="server" ForeColor="Red"></asp:Label>
								</TD>
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
