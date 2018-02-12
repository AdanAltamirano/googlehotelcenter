<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ctrlLanguageDictionary.ascx.vb" Inherits="RateManager.ctrlLanguageDictionary" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<table width="100%">
	<tr>
		<td align="center" colSpan="2"><asp:label id="lblTitle" runat="server" CssClass="TituloTabla">Idiomas</asp:label></td>
	</tr>
	<TR>
		<TD align="center" colSpan="2"><asp:datagrid id="dtgStrings" runat="server" CssClass="DataGrid" AutoGenerateColumns="False" PageSize="5"
				Width="100%">
				<SelectedItemStyle CssClass="DataGridSelectedItem"></SelectedItemStyle>
				<AlternatingItemStyle CssClass="DataGridAlternatedItem"></AlternatingItemStyle>
				<ItemStyle CssClass="DataGridItem"></ItemStyle>
				<HeaderStyle CssClass="DataGridHeader"></HeaderStyle>
				<Columns>
					<asp:BoundColumn ReadOnly="True" HeaderText="Idioma"></asp:BoundColumn>
					<asp:TemplateColumn HeaderText="Texto">
						<ItemTemplate>
							<asp:Label id=Label1 runat="server" Text="<%# DataBinder.Eval(Container.DataItem, Portal.Common.DictionaryData.DictionaryTableFields.Texto) %>">
							</asp:Label>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox id=txtString runat="server" CssClass="TextBox" TextMode="MultiLine" Width="176px" Text="<%# DataBinder.Eval(Container.DataItem, Portal.Common.DictionaryData.DictionaryTableFields.Texto) %>">
							</asp:TextBox>
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn>
						<ItemTemplate>
							<asp:LinkButton id="LinkButton1" runat="server" CssClass="dgLink" Text="Editar" CausesValidation="false"
								CommandName="Edit">Editar</asp:LinkButton>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:LinkButton id="LinkButton3" runat="server" CssClass="dgLink" Text="Actualizar" CommandName="Update">Actualizar</asp:LinkButton>&nbsp;<BR>
							<asp:LinkButton id="LinkButton2" runat="server" CssClass="dgLink" Text="Cancelar" CausesValidation="false"
								CommandName="Cancel">Cancelar</asp:LinkButton>
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn Visible="False" ReadOnly="True" HeaderText="idDictionary"></asp:BoundColumn>
					<asp:BoundColumn Visible="False" ReadOnly="True" HeaderText="idLanguage"></asp:BoundColumn>
				</Columns>
			</asp:datagrid></TD>
	</TR>
</table>
