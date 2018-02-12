<%@ Page Language="vb" AutoEventWireup="false" Codebehind="seamless.aspx.vb" Inherits="RateManager.seamless" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Log Exporter (Seamless)</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script language="javascript" src="wzSeamless.js"></script>
		<LINK href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<TABLE id="bookingContainer" cellSpacing="0" cellPadding="2" border="0" width="300" align="center">
				<TR>
					<TD align="center" colSpan="2">
						<asp:Label id="Label3" runat="server" CssClass="Titulo" EnableViewState="False">Exportación de logs de peticiones.</asp:Label></TD>
				</TR>
				<TR>
					<TD><asp:label id="Label1" runat="server" EnableViewState="False">Fecha Inicial</asp:label></TD>
					<TD><asp:label id="Label2" runat="server" EnableViewState="False">Fecha Final</asp:label></TD>
				</TR>
				<TR>
					<TD><INPUT id="txtInicio" type="text" size="10" runat="server">
						<asp:literal id="ltInicio" runat="server"></asp:literal></TD>
					<TD><INPUT id="txtFin" type="text" size="10" name="Text1" runat="server">
						<asp:literal id="ltFin" runat="server"></asp:literal></TD>
				</TR>
				<TR>
					<TD align="left" colspan="2" height="14" class="progressBarContainer"><div id="progreso" class="progressBar">0 
							%</div>
					</TD>
				</TR>
				<TR>
					<TD></TD>
					<TD align="right"><INPUT id="btnExportar" runat="server" type="button" value="Exportar" onclick="procesarLogs();"
							class="button"></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
