<%@ Control Language="vb" AutoEventWireup="false" Codebehind="Date.ascx.vb" Inherits="RateManager._Date" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<TABLE id="Table1" cellSpacing="1" cellPadding="1" border="0">
	<TR>
		<TD><asp:dropdownlist id="lstDay" runat="server"></asp:dropdownlist></TD>
		<TD><asp:dropdownlist id="lstMonth" runat="server"></asp:dropdownlist></TD>
		<TD><asp:dropdownlist id="lstYear" runat="server"></asp:dropdownlist></TD>
		<TD><asp:imagebutton id="btnCalendar" runat="server" ImageUrl="cal.gif" Visible="False"></asp:imagebutton></TD>
	</TR>
</TABLE>
