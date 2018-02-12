<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ctrlActividades.ascx.vb" Inherits="RateManager.ctrlActividades" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<div>
<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
	<TR>
		<TD>
			<div id="serviciosedit" style="DISPLAY: none"><asp:checkboxlist id="chkServicios" CssClass="clslabel" Width="100%" RepeatColumns="3" CellSpacing="1"
					runat="server"></asp:checkboxlist><asp:linkbutton id="lnkGuardar" CssClass="dgLink" runat="server">Guardar Servicios</asp:linkbutton></div>
			<div id="servicios"><asp:datalist id="dlstServicios" CssClass="clslabel" Width="100%" RepeatColumns="2" runat="server"
					cellSpacing="1">
					<ItemTemplate>
						<asp:Label id="lblAmen" runat="server" CssClass = "clslabel" text='<%# "<li>" & container.dataitem("texto") & "</li>" %>'>
						</asp:Label>
					</ItemTemplate>
				</asp:datalist><asp:linkbutton id="lnkEditar" CssClass="dgLink" runat="server">Editar Servicios</asp:linkbutton></div>
		</TD>
	</TR>
	<tr>
	<td>
                	    
	</td>
	</tr>
	<TR>
		<TD><asp:label id="lblMensaje" CssClass="clslabel" runat="server"></asp:label></TD>
	</TR>
</TABLE>

</div>

