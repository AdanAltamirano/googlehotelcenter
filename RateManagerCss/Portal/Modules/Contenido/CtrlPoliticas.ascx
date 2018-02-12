<%@ Control Language="vb" AutoEventWireup="false" Codebehind="CtrlPoliticas.ascx.vb" Inherits="RateManager.CtrlPoliticas" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
	<TR>
		<TD><asp:checkboxlist id="chkPoliticas" runat="server" CellSpacing="1" RepeatColumns="3" Width="100%"
				CssClass="clslabel" Visible="False"></asp:checkboxlist><asp:datalist id="dlstPoliticas" runat="server" RepeatColumns="2" Width="100%" CssClass="clslabel"
				cellSpacing="1">
				<ItemTemplate>
					<asp:Label id="lblAmen" runat="server" CssClass = "clslabel" text='<%# "<li>" & container.dataitem(RateManager.DataCtrlPoliticas.Descripcion_Field) & "</li>" %>'>
					</asp:Label>
				</ItemTemplate>
			</asp:datalist></TD>
	</TR>
	<TR>
		<TD><asp:label id="lblMensaje" runat="server" CssClass="clslabel"></asp:label></TD>
	</TR>
	<TR>
		<TD><asp:linkbutton id="lnkGuardar" runat="server" CssClass="dgLink" Visible="False">Guardar Politicas</asp:linkbutton><asp:linkbutton id="lnkEditar" runat="server" CssClass="link" Visible="False">Editar Politicas</asp:linkbutton></TD>
	</TR>
</TABLE>
