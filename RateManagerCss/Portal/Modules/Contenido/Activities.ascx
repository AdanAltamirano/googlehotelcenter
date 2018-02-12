<%@ Register TagPrefix="uc1" TagName="CambiarContenido" Src="CambiarContenido.ascx" %>
<%@ Control Language="vb" AutoEventWireup="false" Codebehind="Activities.ascx.vb" Inherits="RateManager.Activities" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<asp:Panel id="lPanel" runat="server" DESIGNTIMEDRAGDROP="1">
	<TABLE id="Table4" cellSpacing="2" cellPadding="2" width="100%" border="0">
		<TR>
			<TD vAlign="top" align="left" width="160" colSpan="2">
				<uc1:cambiarcontenido id="ctrlTitle" runat="server" Visible="False"></uc1:cambiarcontenido>
				<asp:label id="lblTitle" runat="server" EnableViewState="False" CssClass="clsdarklabel">Recreación</asp:label></TD>
		</TR>
		<TR>
			<TD vAlign="top" align="left" width="160">
				<uc1:cambiarcontenido id="ctrlImgRecreation" runat="server"></uc1:cambiarcontenido>
				<asp:image id="imgRecreation" runat="server"></asp:image></TD>

		</TR>
		<tr>
					<TD vAlign="top">
				<TABLE id="Table5" cellSpacing="1" cellPadding="1" width="100%" border="0">
					<TR>
						<TD>
							<uc1:cambiarcontenido id="ctrlTextRecreation" runat="server"></uc1:cambiarcontenido></TD>
					</TR>
					<TR>
						<TD>
							<asp:label id="lblRecreationText" runat="server" CssClass="clslabel" Width="100%"></asp:label></TD>
					</TR>
				</TABLE>
			</TD>
		
		</tr>
	</TABLE>
</asp:Panel>
<asp:Panel id="gPanel" runat="server">
	<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="100%" border="0">
		<TR>
			<TD colSpan="2">
				<asp:label id="lblGTitle" runat="server" EnableViewState="False" CssClass="clsdarklabel">Servicios</asp:label></TD>
		</TR>
		<TR>
			<TD width="10"></TD>
			<TD>
				<asp:label id="lblGDescription" runat="server" CssClass="clslabel"></asp:label></TD>
		</TR>
	</TABLE>
</asp:Panel>
