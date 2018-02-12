<%@ Page Language="vb" AutoEventWireup="false" Codebehind="rsReport.aspx.vb" Inherits="RateManager.rsReport" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>rsReport</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
		<meta content="JavaScript" name="vs_defaultClientScript" />
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
		<script language="javascript" type="text/javascript">
			function SetState(obj_checkbox, obj_disable)
			{  
				if(obj_checkbox.checked){
					obj_disable.disabled = false;
				}
				else{
					obj_disable.disabled = true;
				}
			}

			function SetState_1(obj_checkbox)
			{  
				var obj_disable1 = document.getElementById('txtInicio');
				var obj_disable2 = document.getElementById('txtFin');
				if(obj_checkbox.checked){
					obj_disable1.disabled = false;
					obj_disable2.disabled = false;
				}
				else{
					obj_disable1.disabled = true;
					obj_disable2.disabled = true;
				}
			}

			function Validar()
			{
				var Ini = new Date(document.getElementById('txtInicio').value);
				var Fin = new Date(document.getElementById('txtFin').value);
				alert('Validando ...');
				if (Fin<Ini){
					document.getElementById('txtFin').value=document.getElementById('txtInicio').value;
				}
			}
		</script>
		<LINK href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body>
		<form id="Form1" runat="server" metdod="post">
			<TABLE id="bookingcontainer" cellSpacing="0" cellPadding="2" width="700" border="0" widtd="500"
				style="MARGIN: auto">
				<TR>
					<TD align="center"><asp:label id="Label1" runat="server" CssClass="Titulo" EnableViewState="False">Reporte de logs de peticiones.</asp:label></TD>
				</TR>
				<TR>
					<TD align="center">
						<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="370" border="0">
							<TR>
								<TD align="right"><asp:label id="Label2" runat="server" CssClass="bookingNormalLabel" EnableViewState="False">Filtrar por :</asp:label></TD>
								<TD colSpan="3"></TD>
							</TR>
							<TR>
								<TD align="right"><asp:checkbox id="cbStatus" onclick="SetState(this, this.form.ddlStatus);" runat="server" Text="Status"
										TextAlign="Left"></asp:checkbox></TD>
								<TD><asp:dropdownlist id="ddlStatus" runat="server" Enabled="False">
										<asp:ListItem Value="0">Todos</asp:ListItem>
										<asp:ListItem Value="1">Correcto</asp:ListItem>
										<asp:ListItem Value="2">Error</asp:ListItem>
									</asp:dropdownlist></TD>
								<TD align="right"><asp:checkbox id="cbGDS" onclick="SetState(this, this.form.ddlGds);" runat="server" Text="GDS"
										TextAlign="Left"></asp:checkbox></TD>
								<TD><asp:dropdownlist id="ddlGds" runat="server" Enabled="False">
										<asp:ListItem Value="XX">Todos</asp:ListItem>
										<asp:ListItem Value="1A">Amadeus</asp:ListItem>
										<asp:ListItem Value="UA">Galileo</asp:ListItem>
										<asp:ListItem Value="AA">Sabre</asp:ListItem>
										<asp:ListItem Value="TW">Worldspan</asp:ListItem>
									</asp:dropdownlist></TD>
							</TR>
							<TR>
								<TD align="right"><asp:checkbox id="cbFechas" onclick="SetState_1(this);" runat="server" Text="Fecha/Rango" TextAlign="Left"></asp:checkbox></TD>
								<TD><INPUT id="txtInicio" type="text" size="10" runat="server"><asp:literal id="ltInicio" runat="server"></asp:literal></TD>
								<TD></TD>
								<TD><INPUT id="txtFin" type="text" size="10" runat="server"><asp:literal id="ltFin" runat="server"></asp:literal></TD>
							</TR>
							<TR>
								<TD></TD>
								<TD></TD>
								<TD></TD>
								<TD align="right"><asp:button id="Button1" runat="server" CssClass="button" Text="Aceptar" EnableViewState="False"></asp:button></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD class="titulo"></TD>
				</TR>
				<TR>
					<TD align="center"><asp:datagrid id="dgLogs" runat="server" CssClass="datagrid" Width="650px" AutoGenerateColumns="False"
							AllowPaging="True" BackColor="White">
							<AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
							<ItemStyle CssClass="dgItem"></ItemStyle>
							<HeaderStyle HorizontalAlign="Center" CssClass="dgHeader"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="PN" HeaderText="No Prop.">
									<HeaderStyle Width="80px"></HeaderStyle>
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Fecha">
									<HeaderStyle Width="80px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:Label id=Label3 runat="server" Text='<%# Format(Container.DataItem("DS"),"dd/MMM/yyyy").ToUpper & "<br />" & Format(Container.DataItem("TS"),"hh:mm:ss") %>'>
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Check-In&lt;br /&gt;Check-Out">
									<HeaderStyle Width="80px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
									<ItemTemplate>
										<asp:Label id=Label4 runat="server" Text='<%# Format(Container.DataItem("IN"),"dd/MMM/yyyy").ToUpper & "<br />" & Format(Container.DataItem("OT"),"dd/MMM/yyyy").ToUpper %>'>
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Ad/Ni">
									<HeaderStyle Width="50px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<ItemTemplate>
										<asp:Label runat="server" Text='<%# Container.DataItem("NA") & "/" & Container.DataItem("NC") %>'>
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="MSG" HeaderText="Mensaje"></asp:BoundColumn>
								<asp:BoundColumn DataField="GDS" HeaderText="GDS">
									<HeaderStyle Width="50px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="AGY" HeaderText="Agencia">
									<HeaderStyle Width="80px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
								</asp:BoundColumn>
							</Columns>
							<PagerStyle HorizontalAlign="Right" Position="Top" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD align="center" height="15"></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
