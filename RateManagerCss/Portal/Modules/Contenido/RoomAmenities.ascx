<%@ Control Language="vb" AutoEventWireup="false" Codebehind="RoomAmenities.ascx.vb" Inherits="RateManager.RoomAmenities" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="uc1" TagName="CambiarContenido" Src="CambiarContenido.ascx" %>
<TABLE id="Table" cellSpacing="1" cellPadding="1" width="100%" border="0" runat="server">
	<TR>
		<TD>
			<asp:label id="lblTitle" runat="server" CssClass="clsdarklabel">Rooms amenities</asp:label></TD>
	</TR>
	<TR>
		<TD vAlign="middle">
			<TABLE id="Table2" cellSpacing="1" cellPadding="1" width="100%" border="0">
			</TABLE>
			<TABLE id="Table3" cellSpacing="4" cellPadding="1" width="100%" border="0">
				<TR>
					<TD valign="top">
						<uc1:CambiarContenido id="ctrlLinkDescription" runat="server"></uc1:CambiarContenido>
						<asp:label id="lblDescription" runat="server" Width="100%" CssClass="clslabel"></asp:label>
					</TD>
				</TR>
			</TABLE>
		</TD>
	</TR>
</TABLE>
