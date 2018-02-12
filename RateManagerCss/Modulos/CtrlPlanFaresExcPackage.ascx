<%@ Control Language="vb" AutoEventWireup="false" Codebehind="CtrlPlanFaresExcPackage.ascx.vb" Inherits="RateManager.CtrlPlanFaresExcPackage" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<LINK href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
	<tr>
		<td>
			<table class="clsbackground" id="Table2" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR class="DgAlternate">
					<TD>
						<asp:label id="lblDomingo" EnableViewState="False" CssClass="clsLabel" runat="server" DESIGNTIMEDRAGDROP="20"></asp:label></TD>
					<TD><asp:label id="lblLunes" runat="server" CssClass="clsLabel" EnableViewState="False"></asp:label></TD>
					<TD><asp:label id="lblMartes" runat="server" CssClass="clsLabel" EnableViewState="False"></asp:label></TD>
					<TD><asp:label id="lblMiercoles" runat="server" CssClass="clsLabel" EnableViewState="False"></asp:label></TD>
					<TD><asp:label id="lblJueves" runat="server" CssClass="clsLabel" EnableViewState="False"></asp:label></TD>
					<TD><asp:label id="lblViernes" runat="server" CssClass="clsLabel" EnableViewState="False"></asp:label></TD>
					<TD><asp:label id="lblSabado" runat="server" CssClass="clsLabel" EnableViewState="False"></asp:label></TD>
				</TR>
				<TR>
					<TD><asp:checkbox id="chk7" runat="server"></asp:checkbox></TD>
					<TD><asp:checkbox id="chk1" runat="server"></asp:checkbox></TD>
					<TD><asp:checkbox id="chk2" runat="server"></asp:checkbox></TD>
					<TD><asp:checkbox id="chk3" runat="server"></asp:checkbox></TD>
					<TD><asp:checkbox id="chk4" runat="server"></asp:checkbox></TD>
					<TD><asp:checkbox id="chk5" runat="server"></asp:checkbox></TD>
					<TD><asp:checkbox id="chk6" runat="server"></asp:checkbox></TD>
				</TR>
			</table>
		</td>
	</tr>
</TABLE>
<asp:customvalidator id="cvErrMsg" runat="server" CssClass="Validators" Display="Dynamic">El valor de la tarifa para adulto y niño no puede ser cero  para esta combinación la habitación seria gratiuta</asp:customvalidator>
<TABLE id="Table3" cellSpacing="2" cellPadding="0" width="100%" border="0">
	<TR>
		<TD></TD>
		<TD></TD>
	</TR>
	<TR>
		<TD vAlign="top" align="center" width="50%"><asp:datagrid id="dgAdult" runat="server" CssClass="DataGrid" AutoGenerateColumns="False" Width="100%">
				<SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
				<AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
				<ItemStyle CssClass="dgItem"></ItemStyle>
				<HeaderStyle CssClass="dgHeader"></HeaderStyle>
				<Columns>
					<asp:TemplateColumn HeaderText="Adultos">
						<ItemTemplate>
							<asp:Label id=lblAdults runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Adults") %>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Tarifa de adultos">
						<HeaderTemplate>
							<asp:Label id="lblHeader2" runat="server">Tarifa de adultos</asp:Label>
						</HeaderTemplate>
						<ItemTemplate>
							<asp:TextBox id=txtAdultFare style="TEXT-ALIGN: right" CssClass="TextBox" runat="server" MaxLength="10" Columns="10" Text='<%# DataBinder.Eval(Container, "DataItem.price") %>'>
							</asp:TextBox>
							<asp:CustomValidator id="cvErrAdults" Display="Dynamic" CssClass="Validators" runat="server" ControlToValidate="txtAdultFare">*</asp:CustomValidator><BR>
							<asp:RegularExpressionValidator id="valAdultExtraPrice" Display="Dynamic" CssClass="Validators" runat="server" ControlToValidate="txtAdultFare"
								ValidationExpression="^([0-9]*|\d*\.\d{1}?\d*)$" ErrorMessage="* Precio Inválido">* Tarifa Inválida</asp:RegularExpressionValidator>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn Visible="False" DataField="idRestriccion" HeaderText="idRestriccion"></asp:BoundColumn>
					<asp:TemplateColumn Visible="False" HeaderText="Tarifa Activa">
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:CheckBox id="chkActive" onclick="javascript:HighlightRow(this);" runat="server"></asp:CheckBox>
						</ItemTemplate>
					</asp:TemplateColumn>
				</Columns>
				<PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
					Position="Top" CssClass="DgPage"></PagerStyle>
			</asp:datagrid></TD>
		<TD vAlign="top" align="center" width="50%"><asp:datagrid id="dgChild" runat="server" CssClass="DataGrid" AutoGenerateColumns="False" Width="100%">
				<SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
				<AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
				<ItemStyle CssClass="dgItem"></ItemStyle>
				<HeaderStyle CssClass="dgHeader"></HeaderStyle>
				<Columns>
					<asp:TemplateColumn HeaderText="Ni&#241;os">
						<HeaderTemplate>
							<asp:Label id="lblHeader3" runat="server">Niños</asp:Label>
						</HeaderTemplate>
						<ItemTemplate>
							<asp:Label id=lblChildren runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Children") %>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Tarifa de ni&#241;os">
						<HeaderTemplate>
							<asp:Label id="lblHeader4" runat="server">Tarifa de niños</asp:Label>
						</HeaderTemplate>
						<ItemTemplate>
							<asp:TextBox id=txtChildrenFare style="TEXT-ALIGN: right" CssClass="TextBox" runat="server" MaxLength="10" Columns="10" Text='<%# DataBinder.Eval(Container, "DataItem.Price") %>'>
							</asp:TextBox>
							<asp:CustomValidator id="cvErrChilds" Display="Dynamic" runat="server" ControlToValidate="txtChildrenFare">*</asp:CustomValidator><BR>
							<asp:RegularExpressionValidator id="valChildrenExtraPrice" Display="Dynamic" CssClass="Validators" runat="server"
								ControlToValidate="txtChildrenFare" ValidationExpression="^([0-9]*|\d*\.\d{1}?\d*)$" ErrorMessage="* Precio Inválido">* Tarifa Inválida</asp:RegularExpressionValidator>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn Visible="False" DataField="idRestriccion" HeaderText="idRestriccion"></asp:BoundColumn>
					<asp:TemplateColumn Visible="False" HeaderText="Tarifa Activa">
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:CheckBox id="Checkbox1" onclick="javascript:HighlightRow(this);" runat="server"></asp:CheckBox>
						</ItemTemplate>
					</asp:TemplateColumn>
				</Columns>
				<PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
					Position="Top" CssClass="DgPage"></PagerStyle>
			</asp:datagrid></TD>
	</TR>
</TABLE>
