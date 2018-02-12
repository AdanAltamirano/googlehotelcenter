<%@ Register TagPrefix="uc1" TagName="CambiarContenido" Src="CambiarContenido.ascx" %>
<%@ Control Language="vb" AutoEventWireup="false" Codebehind="Policies.ascx.vb" Inherits="RateManager.Policies" targetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="uc1" TagName="CtrlPoliticas" Src="CtrlPoliticas.ascx" %>
<asp:Panel id="lPanel" runat="server">
	<TABLE width="100%">
		<TR>
			<TD>
				<asp:Label id="lblPoliciesTitle" runat="server" CssClass="clsdarklabel" EnableViewState="False">Policies</asp:Label></TD>
		</TR>
		<TR>
			<TD>
				<uc1:CambiarContenido id="CambiarContenidoPoliciesText" runat="server"></uc1:CambiarContenido>
				<asp:Label id="lblPoliciesText" runat="server" CssClass="clslabel" Width="100%">PoliciesText</asp:Label></TD>
		</TR>
		<TR>
			<TD>
				<uc1:CtrlPoliticas id="Poli" runat="server"></uc1:CtrlPoliticas></TD>
		</TR>
	</TABLE>
</asp:Panel>
<asp:Panel id="gPanel" runat="server">
	<TABLE id="Table4" cellSpacing="1" cellPadding="1" width="100%" border="0">
		<TR>
			<TD colSpan="2">
				<asp:label id="lblGTitle" runat="server" CssClass="clsdarklabel">Policies</asp:label></TD>
		</TR>
		<TR>
			<TD width="10"></TD>
			<TD>
				<asp:label id="lblGTitle1" runat="server" CssClass="clsdarklabel" DESIGNTIMEDRAGDROP="107">Guarantee Policy</asp:label></TD>
		</TR>
		<TR>
			<TD width="10"></TD>
			<TD>
				<asp:label id="lblGDescription" runat="server" CssClass="clslabel" DESIGNTIMEDRAGDROP="110"></asp:label></TD>
		</TR>
		<TR>
			<TD width="10"></TD>
			<TD>
				<asp:label id="lblGTitle2" runat="server" CssClass="clsdarklabel" DESIGNTIMEDRAGDROP="50">Cancellation Policy</asp:label></TD>
		</TR>
		<TR>
			<TD width="10"></TD>
			<TD>
				<asp:label id="lblGDescription1" runat="server" CssClass="clslabel" DESIGNTIMEDRAGDROP="54"></asp:label></TD>
		</TR>
		<TR>
			<TD width="10"></TD>
			<TD>
				<asp:label id="lblGTitle3" runat="server" CssClass="clsdarklabel" DESIGNTIMEDRAGDROP="3">Credit Card Policies</asp:label></TD>
		</TR>
		<TR>
			<TD width="10"></TD>
			<TD>
				<asp:label id="lblGDescription2" runat="server" CssClass="clslabel" DESIGNTIMEDRAGDROP="50"></asp:label></TD>
		</TR>
		<TR>
			<TD width="10"></TD>
			<TD>
				<asp:label id="lblGTitle4" runat="server" CssClass="clsdarklabel">Deposit Policy</asp:label></TD>
		</TR>
		<TR>
			<TD width="10"></TD>
			<TD>
				<asp:label id="lblGDescription3" runat="server" CssClass="clslabel"></asp:label></TD>
		</TR>
	</TABLE>
</asp:Panel>
