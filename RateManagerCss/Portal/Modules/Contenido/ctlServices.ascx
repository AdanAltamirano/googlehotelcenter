<%@ Register TagPrefix="uc1" TagName="CambiarContenido" Src="CambiarContenido.ascx" %>
<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ctlServices.ascx.vb" Inherits="RateManager.ctlServices" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="uc1" TagName="ctrlActividades" Src="ctrlActividades.ascx" %>
<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="100%" border="0">
	<TR>
		<TD>
			<uc1:cambiarcontenido id="ctrlTitle" runat="server" Visible="False"></uc1:cambiarcontenido>
			<asp:label id="lblTitle" runat="server" CssClass="clsdarklabel" EnableViewState="False">Servicios</asp:label></TD>
	</TR>
	<TR>
		<TD>
			<uc1:ctrlActividades id="CtrlActividades1" runat="server"></uc1:ctrlActividades></TD>
	</TR>
</TABLE>
