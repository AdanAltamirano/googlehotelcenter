<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../Portal/Modules/ctrlFooter.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../Portal/Modules/ctrlHeader.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Users.aspx.vb" Inherits="RateManager.Users"%>
<%@ Register TagPrefix="uc1" TagName="ctlMensajes" Src="../Modulos/ctlMensajes.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrMenu" Src="../Portal/Modules/CtrMenu.ascx" %>
<%@ Import NameSpace = "RateManager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Users</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
		</LINK>
		
		<script type="text/javascript" >
		    var res01412 = '<%=RateManager.PortalCulture.GetString("01412")%>'

		    function HotelSeleccionado(id) {
		        var dg = document.getElementById(id);
		        var list = dg.getElementsByTagName("input");
		        var val = false;
		        for (var i = 0; i <= list.length - 1; i++) {
		            if (list[i].type == 'checkbox') {
		                val = list[i].checked;
		                if (val) break;
		            }
		        }
		        if (!val) {
		            alert(res01412);
		        }
		        return (val) 
		    }
		    
		</script>
		
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="2" rightMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
        <div class="clear">
            <div class="mDiv">
            </div>
            <div class="title">
                <asp:Label ID="lbltitle" runat="server" EnableViewState="False" Text="Agregar Usuario" CssClass="tituloSeccion"></asp:Label>
            </div>
         </div>		
			<TABLE id="bookingcontainer" cellSpacing="0" cellPadding="2" width="600" border="0">
				<tr>
					<td>
						<TABLE cellSpacing="0" cellPadding="0" width="100%" border="0">
                            <tr>
                                <td width="50%">
                                    <asp:label id="lblSearch" runat="server" EnableViewState="False" CssClass="clslabel">Search User:</asp:label>
                                    <asp:textbox id="txtSearch" runat="server" CssClass="textbox" MaxLength="80"></asp:textbox>
                                    <asp:button id="btnSearch" runat="server" EnableViewState="False" CssClass="button" Text="Search" CausesValidation="False" style="float:none;"></asp:button>
                                </td>
                            </tr>
                            <TR>
								<TD align="center" width="50%">
								<asp:datagrid id="dgUsuarios" runat="server" CssClass=DataGrid  AutoGenerateColumns="False" Width="70%" AllowPaging="True"
										PageSize="5">
										<SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
										<AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
										<ItemStyle CssClass="dgItem"></ItemStyle>
										<HeaderStyle CssClass="dgHeader"></HeaderStyle>
										<Columns>
											<asp:BoundColumn Visible="False" DataField="iduser"></asp:BoundColumn>
											<asp:BoundColumn DataField="Name" HeaderText="User Name">
												<ItemStyle Width="50%"></ItemStyle>
											</asp:BoundColumn>
											<asp:TemplateColumn>
												<ItemStyle HorizontalAlign="Center" Width="25%"></ItemStyle>
												<ItemTemplate>
													<asp:LinkButton id="lnkEdit" runat="server" CssClass="dgLink" CausesValidation="False" CommandName="Select">Edit</asp:LinkButton>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn>
												<ItemStyle HorizontalAlign="Center"></ItemStyle>
												<ItemTemplate>
													<asp:LinkButton id="lnkdelete2" style="DISPLAY: none" runat="server" CssClass="dgLink" CausesValidation="False"
														CommandName="Delete"></asp:LinkButton>
													<asp:HyperLink id="lnkdelete" runat="server" CssClass="dglink">Delete</asp:HyperLink>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn Visible="False" DataField="verDatosTarjeta"></asp:BoundColumn>
										</Columns>
										<PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
											Position="Top" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
									</asp:datagrid></TD>
								<td colSpan="2" valign=top >
									<table>
										<tr>
											<td><asp:label id="lblName" runat="server" EnableViewState="False" CssClass="clslabel">Name:</asp:label></td>
											<td><asp:textbox id="txtName" runat="server" CssClass="textbox" Columns="15" 
                                                    MaxLength="80"></asp:textbox><asp:requiredfieldvalidator id="RfvName" runat="server" CssClass="Validators" ErrorMessage="*" ControlToValidate="txtName"
													Display="Dynamic"></asp:requiredfieldvalidator></td>
											<td></td>
										</tr>
										<TR>
											<TD style="HEIGHT: 19px"><asp:label id="lblPass" runat="server" EnableViewState="False" CssClass="clslabel">Password:</asp:label></TD>
											<TD style="HEIGHT: 19px"><asp:textbox id="txtPassword" runat="server" CssClass="textbox" Columns="20" MaxLength="20" TextMode="Password"></asp:textbox></TD>
											<TD style="HEIGHT: 19px"></TD>
										</TR>
										<TR>
											<TD><asp:label id="lblConfPass" runat="server" EnableViewState="False" CssClass="clslabel">Confirm Password:</asp:label></TD>
											<TD><asp:textbox id="txtConfirmPass" runat="server" CssClass="textbox" Columns="20" MaxLength="20"
													TextMode="Password"></asp:textbox><asp:comparevalidator id="CVPass" runat="server" CssClass="Validators" ErrorMessage="*" ControlToValidate="txtConfirmPass"
													Display="Dynamic" ControlToCompare="txtPassword"></asp:comparevalidator></TD>
											<TD></TD>
										</TR>
										<tr><td></td><td colspan =2>
										<asp:CheckBox id="chkInfoTC" runat="server" Text="Restringir Información Tarjeta Credito"></asp:CheckBox>
										</td></tr>
									</table>
								</td>
							</TR>
							<TR>
								<TD class="dgItem" align="center" colSpan="3">
									<table cellSpacing="0" cellPadding="0" width="100%" border="0">
										<tr>
											<td align="center" width="50%"></td>
											<td align="center" width="50%"></td>
										</tr>
									</table>
								</TD>
							</TR>
							<TR>
								<TD align="center" colSpan="3">
									<table cellSpacing="4" cellPadding="0" width="100%" border="0">
										<tr>
											<td vAlign="top" align="center" width="50%"><asp:datagrid id="dgPermisos" runat="server" AutoGenerateColumns="False" Width="90%" CssClass=DataGrid >
													<SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
													<AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
													<ItemStyle CssClass="dgItem"></ItemStyle>
													<HeaderStyle CssClass="dgHeader"></HeaderStyle>
													<Columns>
														<asp:BoundColumn DataField="Nombre" HeaderText="Acceso"></asp:BoundColumn>
														<asp:TemplateColumn>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:CheckBox id="chkRead" runat="server"></asp:CheckBox>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn Visible="False" HeaderText="ADD">
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:CheckBox id="chkAdd" runat="server"></asp:CheckBox>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn Visible="False" HeaderText="MODIFY">
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:CheckBox id="chkModify" runat="server"></asp:CheckBox>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:TemplateColumn Visible="False" HeaderText="DELETE">
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:CheckBox id="chkDelete" runat="server"></asp:CheckBox>
															</ItemTemplate>
														</asp:TemplateColumn>
														<asp:BoundColumn Visible="False" DataField="Url"></asp:BoundColumn>
														<asp:BoundColumn Visible="False" DataField="EsEncabezado"></asp:BoundColumn>
													</Columns>
												</asp:datagrid></td>
											<td vAlign="top" align="center" width="50%"><asp:datagrid id="dgHoteles" runat="server" AutoGenerateColumns="False" Width="90%" PageSize="15" CssClass=DataGrid >
													<SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
													<AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
													<ItemStyle CssClass="dgItem"></ItemStyle>
													<HeaderStyle CssClass="dgHeader"></HeaderStyle>
													<Columns>
														<asp:BoundColumn Visible="False" DataField="idHotel"></asp:BoundColumn>
														<asp:BoundColumn DataField="NombreEmpresa" HeaderText="Hoteles"></asp:BoundColumn>
														<asp:TemplateColumn>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<ItemTemplate>
																<asp:CheckBox id="chkAdministrar" runat="server"></asp:CheckBox>
															</ItemTemplate>
														</asp:TemplateColumn>
													</Columns>
													<PagerStyle HorizontalAlign="Right" Position="Top" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
												</asp:datagrid></td>
										</tr>
									</table>
								</TD>
							</TR>
							<TR>
								<TD align="center" colSpan="4"><asp:button id="btnNew" runat="server" EnableViewState="False" CssClass="button" Text="New"
										CausesValidation="False"></asp:button><asp:button id="btnModify" runat="server" EnableViewState="False" CssClass="button" Text="Guardar"></asp:button></TD>
							</TR>
							<TR>
								<TD align="center" colSpan="4"><asp:label id="lblError" runat="server" CssClass="Validators" Visible="False">Ya existe un usuario con ese nombre</asp:label></TD>
							</TR>
						</TABLE>
					</td>
				</tr>
			</TABLE>
			<uc1:ctlmensajes id="CtlMensajes1" runat="server"></uc1:ctlmensajes></form>
	</body>
</HTML>
