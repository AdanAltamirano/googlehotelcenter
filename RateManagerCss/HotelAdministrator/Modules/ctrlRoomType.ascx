<%@ Register TagPrefix="uc1" TagName="CtrlIdioma" Src="../../Modulos/CtrlIdioma.ascx" %>
<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ctrlRoomType.ascx.vb" Inherits="RateManager.ctrlRoomType" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="uc1" TagName="ctrlLanguageButton" Src="../../Modulos/ctrlLanguageButton.ascx" %>
<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="100%" border="0">
	<TR>
		<TD align="center" colSpan="3" class="dgitem">
			<asp:label id="lblEdit" runat="server" CssClass="bookingNormalLabel">Nuevo Tipo Habitación</asp:label></TD>
	</TR>
	<TR>
		<TD align="right"></TD>
		<TD align="right" style="WIDTH: 150px">
			<asp:label id="lblCode" runat="server" EnableViewState="False">Codigo Habitación:</asp:label></TD>
		<TD>
			<asp:textbox id="txtCodeRoom" CssClass="TextBox" runat="server" MaxLength="4" Columns="4"></asp:textbox></TD>
	</TR>
	<TR>
		<TD align="right"></TD>
		<TD vAlign="bottom" align="right" style="WIDTH: 150px"><asp:label id="lblNameDefault" runat="server" EnableViewState="False">Nombre de habitación:</asp:label></TD>
		<TD>
			<uc1:CtrlIdioma id="mlNameRoom" runat="server"></uc1:CtrlIdioma></TD>
	</TR>
	<TR>
		<TD align="right" width="20"></TD>
		<TD width="150" style="WIDTH: 150px"></TD>
		<TD></TD>
	</TR>
	<TR>
		<TD align="left" colSpan="3"></TD>
	</TR>
</TABLE>
