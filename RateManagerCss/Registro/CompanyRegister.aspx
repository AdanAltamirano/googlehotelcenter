<%@ Register TagPrefix="uc1" TagName="EmpresaModulo" Src="../Portal/Modules/Contenido/EmpresaModulo.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="CompanyRegister.aspx.vb" Inherits="RateManager.CompanyRegister"%>
<%@import namespace="Ratemanager"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>EditRegistro</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
		 <div class="mDiv">
            </div>
            <div class="title">
                <asp:Label ID="lblTitulo" runat="server" EnableViewState="False" Text="Registro de Empresa" CssClass="tituloSeccion"></asp:Label>
            </div>
         </div>	
			<TABLE id="bookingcontainer" cellSpacing="0" width="650" align="center" border="0">
				<tr>
					<td><uc1:EmpresaModulo id="EmpresaModulo1" runat="server"></uc1:EmpresaModulo>
					</td>
				</tr>
				<tr>
					<td align=center>
						<asp:button id="cmdCancelar" runat="server" CssClass="Button" Text="Cancelar" Visible="False"
							CausesValidation="False"></asp:button>
						<asp:Button id="BtnNuevo" runat="server" Text="Nuevo" CssClass="Button" CausesValidation="False"
							Width="64px"></asp:Button>
						<asp:Button id="cmdAceptar" runat="server" Text="Guardar" CssClass="Button"></asp:Button>
						<asp:Button id="cmbPublish" runat="server" Text="Guardar y Publicar" CssClass="Button"></asp:Button>
                        <asp:Button id="cmdSaveHouse" ValidationGroup="Houses" runat="server" Text="Guardar" CssClass="Button" Visible="false"></asp:Button>
					</td>
				</tr>
			</TABLE>
		</form>
	</body>
</HTML>
