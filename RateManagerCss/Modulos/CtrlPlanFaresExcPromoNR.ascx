<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CtrlPlanFaresExcPromoNR.ascx.vb" Inherits="RateManager.CtrlPlanFaresExcPromoNR" %>
<LINK href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
<input type="hidden" id="varMinPercent" runat="server" />
<input type="hidden" id="varMaxPercent" runat="server" />
<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
	<tr>
		<td>
			<table class="clsbackground" id="Table2" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR class="DgAlternate">
					<TD><asp:label id="lblDomingo" DESIGNTIMEDRAGDROP="20" runat="server" CssClass="clsLabel" EnableViewState="False"></asp:label></TD>
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
<asp:customvalidator id="cvErrMsg" runat="server" CssClass="Validators" Display="Dynamic">El valor de la tarifa para adulto y niño no puede ser cero  para esta combinación la habitación seria gratiuta</asp:customvalidator><!--<asp:datagrid id="dtgRestrictions" CssClass="DataGrid" runat="server" Width="100%" AutoGenerateColumns="False">
	<SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
	<AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
	<ItemStyle CssClass="dgItem"></ItemStyle>
	<HeaderStyle CssClass="dgHeader"></HeaderStyle>
	<Columns>
		<asp:TemplateColumn>
			<HeaderTemplate>
			</HeaderTemplate>
			<ItemTemplate>
				<asp:Label id=lblAdults runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Adultos") %>'>
				</asp:Label>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Tarifa de adultos">
			<HeaderTemplate>
				<asp:Label id="lblHeader2" runat="server">Tarifa de adultos</asp:Label>
			</HeaderTemplate>
			<ItemTemplate>
				<asp:TextBox id=txtAdultFare style="TEXT-ALIGN: right" CssClass="TextBox" runat="server" MaxLength="10" Columns="10" Text='<%# DataBinder.Eval(Container, "DataItem.TarifaAdultoExc") %>'>
				</asp:TextBox>
				<asp:CustomValidator id="cvErrAdults" Display="Dynamic" CssClass="Validators" runat="server" ControlToValidate="txtAdultFare">*</asp:CustomValidator><BR>
				<asp:RegularExpressionValidator id="valAdultExtraPrice" Display="Dynamic" CssClass="Validators" runat="server" ControlToValidate="txtAdultFare"
					ValidationExpression="^([0-9]*|\d*\.\d{1}?\d*)$" ErrorMessage="* Precio Inválido">* Tarifa Inválida</asp:RegularExpressionValidator>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Ni&#241;os">
			<HeaderTemplate>
				<asp:Label id="lblHeader3" runat="server">Niños</asp:Label>
			</HeaderTemplate>
			<ItemTemplate>
				<asp:Label id=lblChildren runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Ninios") %>'>
				</asp:Label>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Tarifa de ni&#241;os">
			<HeaderTemplate>
				<asp:Label id="lblHeader4" runat="server">Tarifa de niños</asp:Label>
			</HeaderTemplate>
			<ItemTemplate>
				<asp:TextBox id=txtChildrenFare style="TEXT-ALIGN: right" CssClass="TextBox" runat="server" MaxLength="10" Columns="10" Text='<%# DataBinder.Eval(Container, "DataItem.TarifaNinioExc") %>'>
				</asp:TextBox>
				<asp:CustomValidator id="cvErrChilds" Display="Dynamic" runat="server" ControlToValidate="txtChildrenFare">*</asp:CustomValidator><BR>
				<asp:RegularExpressionValidator id="valChildrenExtraPrice" Display="Dynamic" CssClass="Validators" runat="server"
					ControlToValidate="txtChildrenFare" ValidationExpression="^([0-9]*|\d*\.\d{1}?\d*)$" ErrorMessage="* Precio Inválido">* Tarifa Inválida</asp:RegularExpressionValidator>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Precio total">
			<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
			<ItemStyle HorizontalAlign="Right"></ItemStyle>
			<HeaderTemplate>
				<asp:Label id="lblHeader5" runat="server">Precio total</asp:Label>
			</HeaderTemplate>
			<ItemTemplate>
				<asp:Label id="lblTotal" runat="server"></asp:Label>
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
		Position="Top" CssClass="DataGridPage"></PagerStyle>
</asp:datagrid> -->
<TABLE id="Table3" cellSpacing="2" cellPadding="0" width="100%" border="0">
	<TR>
		<TD></TD>
		<TD></TD>
		<TD></TD>
	</TR>
	<TR>
		<TD vAlign="top" align="center" ><asp:datagrid id="dgAdult" runat="server" CssClass="DataGrid" Width="100%" AutoGenerateColumns="False">
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
					<asp:TemplateColumn HeaderText="Tarifa Neta">
						<HeaderTemplate>
							<asp:Label id="Label1" runat="server">Tarifa NETA</asp:Label>
						</HeaderTemplate>
						<ItemTemplate>
							<asp:TextBox id="txtAdultFareNR" style="TEXT-ALIGN: right" CssClass="TextBox currency" runat="server" MaxLength="10" Columns="10" Text='<%# DataBinder.Eval(Container, "DataItem.priceNR") %>'>
							</asp:TextBox>
							<span class="currency"></span>
							<input type="hidden" class="rate" id="varRate" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.price") %>' />							
							<asp:CustomValidator id="cvErrAdultsNR" Display="Dynamic" CssClass="Validators" runat="server" ControlToValidate="txtAdultFareNR">*</asp:CustomValidator>
							<asp:RegularExpressionValidator id="valAdultExtraPriceNR" Display="Dynamic" CssClass="Validators" runat="server"
								ControlToValidate="txtAdultFareNR" ValidationExpression="^([0-9]*|\d*\.\d{1}?\d*)$" ErrorMessage="* Precio Inválido">* Tarifa Inválida</asp:RegularExpressionValidator>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Tarifa UV">
						<HeaderTemplate>
							<asp:Label id="lblHeader2" runat="server">Tarifa UV</asp:Label>
						</HeaderTemplate>
						<ItemTemplate>
							<asp:TextBox id=txtAdultFare style="TEXT-ALIGN: right" CssClass="TextBox currency" runat="server" MaxLength="10" Columns="10" Text='<%# DataBinder.Eval(Container, "DataItem.price") %>'>
							</asp:TextBox>
							<span class="currency"></span>
							<asp:CompareValidator ID="cmpvAdults" runat="server" Enabled="false" CssClass="Validators" ErrorMessage="CompareValidator" ControlToValidate="txtAdultFare"  ControlToCompare="txtAdultFareNR"  Type="Double" Operator="GreaterThanEqual" Display=Dynamic  style= "width:120px;"><%= RateManager.PortalCulture.GetString("01506") %></asp:CompareValidator>
							<asp:CustomValidator id="cvErrAdults" Display="Dynamic" CssClass="Validators" runat="server" ControlToValidate="txtAdultFare">*</asp:CustomValidator>
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
					<asp:TemplateColumn Visible="True" HeaderText="">
						<HeaderTemplate>
						</HeaderTemplate>
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:Label id="lblAdultValMax" runat="server" CssClass="Validators">Msg</asp:Label>
							<asp:Label id="lblAdultValMin" runat="server" CssClass="Validators">Msg</asp:Label>
						</ItemTemplate>
					</asp:TemplateColumn>
				</Columns>
				<PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
					Position="Top" CssClass="DgPage"></PagerStyle>
			</asp:datagrid></TD>
		<TD vAlign="top" align="center" style="padding-left:6px;">
		<asp:datagrid id="dgChild" runat="server" CssClass="DataGrid" Width="100%" AutoGenerateColumns="False">
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
					<asp:TemplateColumn HeaderText="Tarifa Neta">
						<HeaderTemplate>
							<asp:Label id="Label2" runat="server">Tarifa Neta</asp:Label>
						</HeaderTemplate>
						<ItemTemplate>
							<asp:TextBox id="txtChildrenFareNR" style="TEXT-ALIGN: right" CssClass="TextBox currency" runat="server" MaxLength="10" Columns="10" Text='<%# DataBinder.Eval(Container, "DataItem.PriceNR") %>'>
							</asp:TextBox>
							<span class="currency"></span>
							<input type="hidden" class="rate" id="varRate" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.price") %>' />							
							<asp:CustomValidator id="cvErrChildsNR" Display="Dynamic" runat="server" ControlToValidate="txtChildrenFareNR">*</asp:CustomValidator>
							<asp:RegularExpressionValidator id="valChildrenExtraPriceNR" Display="Dynamic" CssClass="Validators" runat="server"
								ControlToValidate="txtChildrenFareNR" ValidationExpression="^([0-9]*|\d*\.\d{1}?\d*)$" ErrorMessage="* Precio Inválido">* Tarifa Inválida</asp:RegularExpressionValidator>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Tarifa Uv">
						<HeaderTemplate>
							<asp:Label id="lblHeader4" runat="server">Tarifa UV</asp:Label>
						</HeaderTemplate>
						<ItemTemplate>
							<asp:TextBox id=txtChildrenFare style="TEXT-ALIGN: right" CssClass="TextBox currency" runat="server" MaxLength="10" Columns="10" Text='<%# DataBinder.Eval(Container, "DataItem.Price") %>'>
							</asp:TextBox>
							<span class="currency"></span>
							<asp:CompareValidator ID="cmpvChilds" Enable="false" runat="server" CssClass="Validators" ErrorMessage="CompareValidator" ControlToValidate="txtChildrenFare"  ControlToCompare="txtChildrenFareNR"  Type="Double" Operator="GreaterThanEqual" Display=Dynamic style= "width:120px;" ><%= RateManager.PortalCulture.GetString("01506") %></asp:CompareValidator>
							<asp:CustomValidator id="cvErrChilds" Display="Dynamic" runat="server" ControlToValidate="txtChildrenFare">*</asp:CustomValidator>
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
					<asp:TemplateColumn Visible="True" HeaderText="">
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:Label id="lblChildValMax" runat="server" CssClass="Validators">Msg</asp:Label>
							<asp:Label id="lblChildValMin" runat="server" CssClass="Validators">Msg</asp:Label>
						</ItemTemplate>
					</asp:TemplateColumn>
				</Columns>
				<PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
					Position="Top" CssClass="DgPage"></PagerStyle>
			</asp:datagrid></TD>
			<td vAlign="top" align="center" style="padding-left:6px;">
			<asp:datagrid id="dgTeen" runat="server" CssClass="DataGrid" Width="100%" AutoGenerateColumns="False">
				<SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
				<AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
				<ItemStyle CssClass="dgItem"></ItemStyle>
				<HeaderStyle CssClass="dgHeader"></HeaderStyle>
				<Columns>
					<asp:TemplateColumn HeaderText="Ni&#241;os">
						<HeaderTemplate>
							<asp:Label id="lblHeader3" runat="server">Adolescentes</asp:Label>
						</HeaderTemplate>
						<ItemTemplate>
							<asp:Label id=lblChildren runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Teen") %>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Tarifa Neta">
						<HeaderTemplate>
							<asp:Label id="Label2" runat="server">Tarifa Neta</asp:Label>
						</HeaderTemplate>
						<ItemTemplate>
							<asp:TextBox id="txtTeenFareNR" style="TEXT-ALIGN: right" CssClass="TextBox currency" runat="server" MaxLength="10" Columns="10" Text='<%# DataBinder.Eval(Container, "DataItem.PriceNR") %>'>
							</asp:TextBox>
							<span class="currency"></span>
							<input type="hidden" class="rate" id="varRate" runat="server" value='<%# DataBinder.Eval(Container, "DataItem.price") %>' />							
							<asp:CustomValidator id="cvErrTeenNR" Display="Dynamic" runat="server" ControlToValidate="txtTeenFareNR">*</asp:CustomValidator>
							<asp:RegularExpressionValidator id="valTeenExtraPriceNR" Display="Dynamic" CssClass="Validators" runat="server"
								ControlToValidate="txtTeenFareNR" ValidationExpression="^([0-9]*|\d*\.\d{1}?\d*)$" ErrorMessage="* Precio Inválido">* Tarifa Inválida</asp:RegularExpressionValidator>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Tarifa Uv">
						<HeaderTemplate>
							<asp:Label id="lblHeader4" runat="server">Tarifa UV</asp:Label>
						</HeaderTemplate>
						<ItemTemplate>
							<asp:TextBox id=txtTeenFare style="TEXT-ALIGN: right" CssClass="TextBox currency" runat="server" MaxLength="10" Columns="10" Text='<%# DataBinder.Eval(Container, "DataItem.Price") %>'>
							</asp:TextBox>
							<span class="currency"></span>
							<asp:CompareValidator ID="cmpvJuniors" Enabled="false" runat="server" CssClass="Validators" ErrorMessage="CompareValidator" ControlToValidate="txtTeenFare"  ControlToCompare="txtTeenFareNR"  Type="Double" Operator="GreaterThanEqual"  Display=Dynamic style= "width:120px;" ><%= RateManager.PortalCulture.GetString("01506") %></asp:CompareValidator>
							<asp:CustomValidator id="cvErrTeen" Display="Dynamic" runat="server" ControlToValidate="txtTeenFare">*</asp:CustomValidator>
							<asp:RegularExpressionValidator id="valTeenExtraPrice" Display="Dynamic" CssClass="Validators" runat="server"
								ControlToValidate="txtTeenFare" ValidationExpression="^([0-9]*|\d*\.\d{1}?\d*)$" ErrorMessage="* Precio Inválido">* Tarifa Inválida</asp:RegularExpressionValidator>
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
					<asp:TemplateColumn Visible="True" HeaderText="">
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:Label id="lblTeenValMax" runat="server" CssClass="Validators">Msg</asp:Label>
							<asp:Label id="lblTeenValMin" runat="server" CssClass="Validators">Msg</asp:Label>
						</ItemTemplate>
					</asp:TemplateColumn>
				</Columns>
				<PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
					Position="Top" CssClass="DgPage"></PagerStyle>
			</asp:datagrid>
			</td>
	</TR>
</TABLE>
