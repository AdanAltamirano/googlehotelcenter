<%@ Page Language="vb" AutoEventWireup="false" Codebehind="UserAsociate.aspx.vb" Inherits="RateManager.UserAsociate"%>
<%@ Register TagPrefix="uc1" TagName="ctlMensajes" Src="../Modulos/ctlMensajes.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>UserAsociate</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
		    <div class="clear">		    
		        <div class="mDiv"></div>
		        <div>
		            <asp:Label ID="lblTitleForm" runat="server" class="tituloSeccion" style= "margin-left:32px;">Asociar Usuario</asp:Label>         
		        </div>
		    </div>
		    
			<TABLE id="bookingcontainer" cellSpacing="0" width="650" align="center" border="0">
				
				<tr>
					<td colspan="3" class="dgitem" align=center><span id="lblagregar" runat="server">Agregar Usuario</span></td>
				</tr>
				<tr>
					<td width="15%" align=center><span id="lblemail" runat="server">Email:</span></td>
					<td><asp:TextBox ID="txtEmail" Runat="server" MaxLength="80" Columns="50"></asp:TextBox></td>
					<td width="20%"><asp:Button ID="btnSave" Runat="server" Text="Add" CssClass="button"></asp:Button></td>
				</tr>
				<tr>
					<td height=5px></td>
				</tr>
				<tr>
					<td colspan="3" align=center>
						<asp:datagrid id="dgUsuarios" runat="server" Width="90%" CssClass="DataGrid" AllowPaging="True"
							PageSize="20" AutoGenerateColumns="False">
							<AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
							<ItemStyle CssClass="dgItem"></ItemStyle>
							<HeaderStyle CssClass="dgHeader"></HeaderStyle>
							<Columns>
								<asp:TemplateColumn>
									<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
									<ItemTemplate>
										<asp:LinkButton id="lnkEliminar2" style="DISPLAY: none" runat="server" CssClass="dgLink" CausesValidation="False"
											CommandName="Delete">-</asp:LinkButton>
										<asp:LinkButton id="lnkEliminar" runat="server" CssClass="dgLink" CommandName="Delete">Eliminar</asp:LinkButton>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn Visible="False" DataField="IdUsuario"></asp:BoundColumn>
								<asp:BoundColumn DataField="email" HeaderText="Correo electr&#243;nico"></asp:BoundColumn>
							</Columns>
							 <PagerStyle CssClass="dgPager" HorizontalAlign="Right" Mode="NumericPages" Position="Top"
                                PrevPageText="<< Anterior" NextPageText="Siguiente >>"></PagerStyle>
							
						</asp:datagrid>
						<asp:Label id="lblError" runat="server" CssClass="validators">Error</asp:Label>
					</td>
				</tr>
			</TABLE>
			<uc1:ctlMensajes id="CtlMensajes1" runat="server" Visible =false ></uc1:ctlMensajes>
		</form>
	</body>
</HTML>
