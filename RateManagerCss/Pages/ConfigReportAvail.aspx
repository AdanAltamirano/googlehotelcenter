<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ConfigReportAvail.aspx.vb" Inherits="RateManager.ConfigReportAvail"%>
<HTML>
	<HEAD>
		<title>ConfigReportAvail</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
		</LINK>
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0">
		<form id="Form1" method="post" runat="server">
			<TABLE id="bookingcontainer" cellSpacing="0" cellPadding="2" width="650" border="0">
				<tr>
					<td class="titulo" align="center" colspan="4">
						<asp:Label id="lblTitulo" runat="server">Configuración de Reportes de disponibilidad</asp:Label></td>
				</tr>
				<tr>
					<td>
						<asp:Label id="lblEmails" runat="server">Emails</asp:Label></td>
					<td colspan="3"><asp:TextBox id="txtEmails" runat="server" TextMode="MultiLine" Width="497px"></asp:TextBox></td>
				</tr>
				<TR>
					<TD>
						<asp:Label id="lblGenerar" runat="server">Generar Cada</asp:Label></TD>
					<TD>
						<asp:DropDownList id="ddlGenerarTime" runat="server">
							<asp:ListItem>1</asp:ListItem>
							<asp:ListItem>2</asp:ListItem>
							<asp:ListItem>3</asp:ListItem>
							<asp:ListItem>4</asp:ListItem>
							<asp:ListItem>5</asp:ListItem>
							<asp:ListItem>6</asp:ListItem>
							<asp:ListItem>7</asp:ListItem>
							<asp:ListItem>8</asp:ListItem>
							<asp:ListItem>9</asp:ListItem>
							<asp:ListItem>10</asp:ListItem>
							<asp:ListItem>11</asp:ListItem>
							<asp:ListItem>12</asp:ListItem>
							<asp:ListItem>13</asp:ListItem>
							<asp:ListItem>14</asp:ListItem>
							<asp:ListItem>15</asp:ListItem>
							<asp:ListItem>16</asp:ListItem>
							<asp:ListItem>17</asp:ListItem>
							<asp:ListItem>18</asp:ListItem>
							<asp:ListItem>19</asp:ListItem>
							<asp:ListItem>20</asp:ListItem>
						</asp:DropDownList>
						<asp:DropDownList id="ddlGenerarType" runat="server">
							<asp:ListItem Value="D">D</asp:ListItem>
							<asp:ListItem Value="W">W</asp:ListItem>
							<asp:ListItem Value="M">M</asp:ListItem>
							<asp:ListItem value="Y">Y</asp:ListItem>
						</asp:DropDownList></TD>
					<TD><asp:Label id="lblDisplay" runat="server">Mostrar en Reporte</asp:Label></TD>
					<td><asp:DropDownList id="ddlDisplayTime" runat="server">
							<asp:ListItem>1</asp:ListItem>
							<asp:ListItem>2</asp:ListItem>
							<asp:ListItem>3</asp:ListItem>
							<asp:ListItem>4</asp:ListItem>
							<asp:ListItem>5</asp:ListItem>
							<asp:ListItem>6</asp:ListItem>
							<asp:ListItem>7</asp:ListItem>
							<asp:ListItem>8</asp:ListItem>
							<asp:ListItem>9</asp:ListItem>
							<asp:ListItem>10</asp:ListItem>
							<asp:ListItem>11</asp:ListItem>
							<asp:ListItem>12</asp:ListItem>
							<asp:ListItem>13</asp:ListItem>
							<asp:ListItem>14</asp:ListItem>
							<asp:ListItem>15</asp:ListItem>
							<asp:ListItem>16</asp:ListItem>
							<asp:ListItem>17</asp:ListItem>
							<asp:ListItem>18</asp:ListItem>
							<asp:ListItem>19</asp:ListItem>
							<asp:ListItem>20</asp:ListItem>
						</asp:DropDownList>
						<asp:DropDownList id="ddlDisplayType" runat="server">
							<asp:ListItem Value="D">D</asp:ListItem>
							<asp:ListItem Value="W">W</asp:ListItem>
							<asp:ListItem Value="M">M</asp:ListItem>
							<asp:ListItem value="Y">Y</asp:ListItem>
						</asp:DropDownList></td>
				</TR>
				<TR>
					<TD colspan="4">
						<asp:CheckBox id="chkSendMail" runat="server" Text="Send Mail to Hotel"></asp:CheckBox></TD>
				</TR>
				<TR>
					<TD colspan="4" align="center">
						<asp:Button id="btnSave" runat="server" Text="Guardar" CssClass="button"></asp:Button>&nbsp;
						<asp:Button id="btnEjecutar" runat="server" Text="Ejecutar" CssClass="button"></asp:Button></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
