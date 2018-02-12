<%@ Register TagPrefix="uc1" TagName="CtrlAmenidades" Src="CtrlAmenidades.ascx" %>
<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ctrlAmenities.ascx.vb" Inherits="RateManager.ctrlAmenities" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="uc1" TagName="CambiarContenido" Src="CambiarContenido.ascx" %>
<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="100%" border="0">
	<TR>
		<TD>
			<uc1:cambiarcontenido id="ctrlTitle" runat="server" Visible="False"></uc1:cambiarcontenido>
			<asp:label id="lblTitle" runat="server" CssClass="clsdarklabel" EnableViewState="False">Amenidades</asp:label></TD>
	</TR>
	<TR>
		<TD>
			<uc1:CtrlAmenidades id="CtrlAmenidades1" runat="server"></uc1:CtrlAmenidades></TD>
	</TR>
</TABLE>
