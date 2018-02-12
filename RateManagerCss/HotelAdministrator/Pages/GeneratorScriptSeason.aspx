<%@ Page Language="vb" AutoEventWireup="false" Codebehind="GeneratorScriptSeason.aspx.vb" Inherits="RateManager.GeneratorScriptSeason" ValidateRequest="false" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Generator Script Season</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../../StyleSheets/Styles.css" type="text/css" rel="stylesheet" runat="server"
			ID="Link1">
		</LINK>
		<script language="javascript"> 

			function copia_portapapeles(txt,msg)
			{ 
				//n=1 Español; n=2 Ingles;
				var t = document.getElementById(txt)
				t.select();
				window.clipboardData.setData("Text", t.value); 
				alert(msg);
	
			} 
		</script>
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="5" rightMargin="0">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="770" border="0">
				<TR height="300">
					<TD align="center" vAlign="top">
							 <div class="clear">
            <div class="mDiv">
            </div>
            <div class="title">
                <asp:Label ID="lblTitulo" runat="server" EnableViewState="False" Text="Script para Temporadas" CssClass="tituloSeccion"></asp:Label>
            </div>
         </div>	
						<TABLE id="Bookingcontainer" cellSpacing="0" cellPadding="2" width="300" border="0">
							<TR>
								<TD align="left" colSpan="2" height="5"></TD>
							</TR>
							<TR>
								<TD colSpan="2" align="center">
									<asp:DropDownList id="cmbHoteles" runat="server" Width="593px" AutoPostBack="True"></asp:DropDownList></TD>
							</TR>
							<TR>
								<TD colSpan="2" height="5">
									<table width="100%" id="tbScripts" runat="server">
										<tr>
											<td>
												<asp:label style="Z-INDEX: 0" id="lblEspañol" runat="server" EnableViewState="False" CssClass="clsDarkLabel">Español:</asp:label></td>
										</tr>
										<tr>
											<td><asp:TextBox id="txtEspañol" runat="server" Width="100%" TextMode="MultiLine" Height="170px"></asp:TextBox></td>
										</tr>
										<tr>
											<td align="center"><INPUT id="btnCopiarEspaniol" class="button" value="Copiar" type="button" runat="server"></td>
										</tr>
										<TR>
											<TD>
												<asp:label style="Z-INDEX: 0" id="lblIngles" runat="server" EnableViewState="False" CssClass="clsDarkLabel">Ingles:</asp:label></TD>
										</TR>
										<tr>
											<td>
												<asp:TextBox style="Z-INDEX: 0" id="txtIngles" runat="server" Width="100%" TextMode="MultiLine"
													Height="170px"></asp:TextBox></td>
										</tr>
										<TR>
											<TD align="center"><INPUT id="btnCopiarIngles" class="button" value="Copiar" type="button" runat="server"></TD>
										</TR>
									</table>
								</TD>
							</TR>
							<TR>
								<TD colSpan="2"></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
