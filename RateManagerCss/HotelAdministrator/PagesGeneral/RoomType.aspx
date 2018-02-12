<%@ Register TagPrefix="uc1" TagName="ctrlRoomType" Src="../Modules/ctrlRoomType.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RoomType.aspx.vb" Inherits="RateManager.RoomType" %>
<%@ Register TagPrefix="uc1" TagName="ctlMensajes" Src="../../Modulos/ctlMensajes.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../../Portal/Modules/ctrlFooter.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>RoomType</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
		
		<script type="text/javascript" >

		    function validarkeyCode(e) { // 1
		        tecla = (document.all) ? e.keyCode : e.which; // 2
		        if (tecla == 8) return true; // 3
		        patron = /[A-Za-z0-9\s]/; // 4
		        te = String.fromCharCode(tecla); // 5
		        return patron.test(te); // 6
		    } 
		    
		</script>
	</HEAD>
	
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
		 <div class="clear">
            <div class="mDiv">
            </div>
            <div class="title">
                <asp:Label ID="lblTitulo" runat="server" EnableViewState="False" Text="Tipos de habitación" CssClass="tituloSeccion"></asp:Label>
            </div>
         </div>	
			<TABLE id="bookingcontainer" cellSpacing="0" cellPadding="0" width="600" align="center"
				border="0">
				<tr>
					<td>
						<table cellSpacing="0" cellPadding="0" width="100%" border="0">
							<tr>
								<td width="20%"><asp:label id="lblRoom" runat="server" EnableViewState="False">Label</asp:label></td>
								<td><asp:dropdownlist id="lstRoomTypes" runat="server"></asp:dropdownlist></td>
								<td width="10%"><asp:linkbutton id="lnkEdit" runat="server" CausesValidation="False" CssClass="dglink" EnableViewState="False">Editar</asp:linkbutton></td>
								<td width="10%"><asp:linkbutton id="lnkDelete2" runat="server" CausesValidation="False" CssClass="dglink" style="DISPLAY: none">-</asp:linkbutton>
									<asp:HyperLink id="lnkDelete" runat="server" CssClass="dglink" EnableViewState="False">Delete</asp:HyperLink></td>
							</tr>
						</table>
					</td>
				</tr>
				<TR>
					<TD><uc1:ctrlroomtype id="CtrlRoomType1" runat="server"></uc1:ctrlroomtype></TD>
				</TR>
				<TR>
					<TD align="center">
						<HR width="100%" SIZE="1">
						<asp:button id="btnNuevo" runat="server" CausesValidation="False" CssClass="Button" Text="Nuevo"
							Width="85px" EnableViewState="False"></asp:button>
						<asp:button id="btnGuardar" runat="server" CssClass="Button" Text="Guardar" Width="85px" EnableViewState="False"></asp:button></TD>
				</TR>
				<TR>
					<TD align="center">
						<asp:Label id="lblError" runat="server" CssClass="validators"></asp:Label></TD>
				</TR>
			</TABLE>
			<uc1:ctlMensajes id="CtlMensajes1" runat="server"></uc1:ctlMensajes>
		</form>
	</body>
</HTML>
