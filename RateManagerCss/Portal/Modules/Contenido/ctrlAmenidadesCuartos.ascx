<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ctrlAmenidadesCuartos.ascx.vb" Inherits="RateManager.ctrlAmenidadesCuartos" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="uc1" TagName="ctrlAmenidadesCuarto" Src="ctrlAmenidadesCuarto.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CambiarContenido" Src="CambiarContenido.ascx" %>
<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="100%" border="0">
	<TR>
		<TD>
			<uc1:cambiarcontenido id="ctrlTitle" Visible="False" runat="server"></uc1:cambiarcontenido>
			<asp:label id="lblTitle" runat="server" CssClass="clsdarklabel" EnableViewState="False">Amenidades de Cuartos</asp:label></TD>
	</TR>
	<TR>
		<TD>
			<uc1:ctrlAmenidadesCuarto id="CtrlAmenidadesCuarto1" runat="server"></uc1:ctrlAmenidadesCuarto></TD>
	</TR>
</TABLE>
