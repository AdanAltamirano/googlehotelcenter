<%@ Control Language="vb" AutoEventWireup="false" Codebehind="HotelBalanceViewer.ascx.vb" Inherits="RateManager.HotelBalanceViewer" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0">
	<TR>
		<TD class="dgAlternate">
			<TABLE class="DataGrid, textBox" id="tblHotelBalanceViewer" cellSpacing="1" cellPadding="2"
				border="0" runat="server" width="100%">
				<TR>
					<TD class="titulo" style="PADDING-RIGHT: 0px; PADDING-LEFT: 0px; PADDING-BOTTOM: 0px; PADDING-TOP: 0px; HEIGHT: 16px"
						colSpan="2">
						<asp:label id="lblTitle" runat="server" EnableViewState="False">Información de Saldos</asp:label></TD>
				</TR>
				<TR>
					<TD align="left" colSpan="2">
						<asp:Label id="lblReferenceNumberCaption" runat="server" CssClass="bookingNormalLabel" EnableViewState="False"> No. de Referencia:</asp:Label>
						<asp:Label id="lblReferenceNumber" runat="server" CssClass="bookingNormalLabel"></asp:Label></TD>
				</TR>
				<TR>
					<TD align="right" height="3"></TD>
					<TD align="left" height="3"></TD>
				</TR>
				<TR id="trCurrentBalance" runat="server">
					<TD style="WIDTH: 136px; HEIGHT: 17px" align="right">
						<asp:label id="lblDueAmountCaption" runat="server" CssClass="clsLabel" EnableViewState="False">Saldo de la Factura:</asp:label></TD>
					<TD style="HEIGHT: 17px" align="right">
						<asp:label id="lblDueAmount" runat="server" CssClass="clsHelpLabel"></asp:label>&nbsp;</TD>
				</TR>
				<TR id="trAccumulatedBalance" runat="server">
					<TD style="WIDTH: 136px" align="right">
						<asp:label id="lblPaymentsCaption" runat="server" CssClass="clsLabel" EnableViewState="False">Pagos Realizados:</asp:label></TD>
					<TD align="right">
						<asp:label id="lblPayments" runat="server" CssClass="clsHelpLabel"></asp:label>&nbsp;</TD>
				</TR>
				<TR>
					<TD align="right" height="3"></TD>
					<TD align="left" height="3"></TD>
				</TR>
				<TR id="trFinalBalance" runat="server">
					<TD style="WIDTH: 136px" align="right">
						<asp:label id="lblFinalAmountCaption" runat="server" CssClass="clsLabel" EnableViewState="False">Saldo final:</asp:label></TD>
					<TD align="right">
						<asp:label id="lblFinalAmount" runat="server" CssClass="clsHelpLabel"></asp:label>&nbsp;</TD>
				</TR>
			</TABLE>
		</TD>
	</TR>
</TABLE>
