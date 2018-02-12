<%@ Page Language="vb" AutoEventWireup="false" Codebehind="exportSubscribers.aspx.vb" Inherits="RateManager.exportSubscribers"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>exportSubscribers</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0">
		<form id="Form1" method="post" runat="server">
			<asp:datagrid id="dgEmailList" runat="server" AutoGenerateColumns="False" ShowFooter="True">
				<SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
				<AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
				<ItemStyle CssClass="dgItem"></ItemStyle>
				<HeaderStyle HorizontalAlign="Center" CssClass="dgHeader" VerticalAlign="Middle"></HeaderStyle>
				<Columns>
					<asp:BoundColumn DataField="Email" HeaderText="Correo Electronico"></asp:BoundColumn>
					<asp:BoundColumn DataField="Fecha" HeaderText="Fecha de Registro"></asp:BoundColumn>
				</Columns>
			</asp:datagrid>
		</form>
	</body>
</HTML>
