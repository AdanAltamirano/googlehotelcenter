<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ctrHotelsUserChain.ascx.vb" Inherits="RateManager.ctrHotelsUserChain" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<table border="0" cellSpacing="2" cellPadding="2">
	<tr>
		<td><asp:label id="lblEmpresas" CssClass="clslabel" runat="server">Empresas</asp:label></td>
		<TD><asp:dropdownlist id="ddlHoteles" runat="server" Width="232px" AutoPostBack="True"></asp:dropdownlist></TD>
		
	</tr>
	<tr>
		<td colspan="2" align="center">
			<asp:label id="lblCurrentHotel" runat="server" CssClass="bookingnormallabel"></asp:label>
		</td>
	</tr>
</table>
