<%@ Register TagPrefix="uc1" TagName="CambiarContenido" Src="CambiarContenido.ascx" %>
<%@ Control Language="vb" AutoEventWireup="false" Codebehind="AreaAttraction.ascx.vb" Inherits="RateManager.AreaAttraction" targetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<asp:Panel id="lPanel" runat="server">
	<TABLE id="Table2" cellSpacing="2" cellPadding="2" width="100%">
	    <tr><td>
	    <asp:label id="Label1" runat="server" CssClass="clsdarklabel"><%= RateManager.PortalCulture.GetString("00404") %></asp:label>
	    </td></tr>
		<TR>
			<TD vAlign="top" align="left" width="160" >
				<uc1:cambiarcontenido id="ctrlTitle" runat="server" Visible="False"></uc1:cambiarcontenido>
				<asp:label id="lblTitle" runat="server" CssClass="clslabel">Lugares de atracción</asp:label></TD>
		</TR>
		<TR>
			<TD vAlign="top" align="left" width="160">
				<uc1:cambiarcontenido id="CambiarContenidoAttractionImage" runat="server" DESIGNTIMEDRAGDROP="38"></uc1:cambiarcontenido>
				<asp:Image id="imgArea" runat="server"></asp:Image></TD>
		
		</TR>
		<tr>
			<TD vAlign="top">
				<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="100%" border="0">
					<TR>
						<TD>
							<uc1:CambiarContenido id="CambiarContenidoAttractionText" runat="server"></uc1:CambiarContenido></TD>
					</TR>
					<TR>
						<TD>
							<asp:label id="lblAttractionText" runat="server" CssClass="clslabel"></asp:label></TD>
					</TR>
				</TABLE>
			</TD>
		</tr>
	</TABLE>
</asp:Panel>
<asp:Panel id="gPanel" runat="server">
	<TABLE id="Table4" cellSpacing="1" cellPadding="1" width="100%" border="0">
		<TR>
			<TD colSpan="2">
				<asp:label id="lblGTitle" runat="server" CssClass="clsdarklabel">Recreación</asp:label></TD>
		</TR>
		<TR>
			<TD width="10"></TD>
			<TD>
				<asp:label id="lblGDescription" runat="server" CssClass="clslabel"></asp:label></TD>
		</TR>
	</TABLE>
</asp:Panel>
<TABLE id="Table3" height="10" cellSpacing="0" cellPadding="0" width="100%" border="0">
	<TR>
		<TD></TD>
	</TR>
</TABLE>
