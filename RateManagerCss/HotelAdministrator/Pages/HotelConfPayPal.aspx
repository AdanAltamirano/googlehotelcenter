<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../../Portal/Modules/ctrlFooter.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../../Portal/Modules/ctrlHeader.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="HotelConfPayPal.aspx.vb" Inherits="RateManager.HotelConfPayPal"%>
<%@ Register TagPrefix="uc1" TagName="ctlMensajes" Src="../../Modulos/ctlMensajes.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrMenu" Src="../../Portal/Modules/CtrMenu.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>PayPal Settings</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK rel="stylesheet" type="text/css" href="../../StyleSheets/Styles.css">
	</HEAD>
	<body bottomMargin="0" leftMargin="0" rightMargin="0" topMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
		  <div class="clear1">		    
		        <div class="mDiv"></div>
		        <div>
		            <asp:label id="lblTitle" runat="server" EnableViewState="False" CssClass=tituloSeccion style= "margin-left:32px;">Configuracion del Hotel PayPal</asp:label> 
		        </div>
		    </div>
			<TABLE id="bookingcontainer" border="0" cellSpacing="0" cellPadding="2" width="650">
				<tr>
					<td>
						<TABLE id="Table1" border="0" cellSpacing="0" cellPadding="0" width="100%" align="center">
							
							<tr>
								<td height="5" colSpan="3"></td>
							</tr>
							<TR>
								<TD style="HEIGHT: 12px" colSpan="3" align="center"></TD>
							</TR>
							<TR>
								<TD align="left"><asp:label style="Z-INDEX: 0" id="lblBuscar" runat="server" EnableViewState="False" CssClass="clslabel">Buscar:</asp:label></TD>
								<TD style="WIDTH: 1px"><asp:textbox style="Z-INDEX: 0" id="txtBuscarHotel" runat="server" Width="216px"></asp:textbox></TD>
								<td><asp:button style="Z-INDEX: 0" id="btnBuscar" runat="server" EnableViewState="False" CssClass="button"
										CausesValidation="False" Text="Buscar"></asp:button></td>
							</TR>
							<TR>
								<TD align="left"><asp:label style="Z-INDEX: 0" id="Label1" runat="server" EnableViewState="False" CssClass="clslabel">Hotels:</asp:label></TD>
								<td style="WIDTH: 1px"><asp:dropdownlist id="ddlHotels" runat="server" Width="249px"></asp:dropdownlist></td>
								<td></td>
							</TR>
							<TR>
								<TD colSpan="3" align="center"><asp:label style="Z-INDEX: 0" id="msgErrorCargar" runat="server" CssClass="Validators" Width="273px"
										Visible="False">Debe seleccionar un hotel</asp:label></TD>
							</TR>
							<TR>
								<TD align="left"><asp:label style="Z-INDEX: 0" id="lblChannel" runat="server" EnableViewState="False" CssClass="clslabel"> Canal:</asp:label></TD>
								<td style="WIDTH: 1px"><asp:dropdownlist id="ddlChannel" runat="server" Width="249px"></asp:dropdownlist></td>
								<td><asp:button style="Z-INDEX: 0" id="btnLoad" runat="server" EnableViewState="False" CssClass="button"
										CausesValidation="False" Text="Cargar"></asp:button></td>
							</TR>
							<TR>
								<TD colSpan="3" align="center"><asp:label style="Z-INDEX: 0" id="msgErrorCargarCanal" runat="server" CssClass="Validators"
										Width="273px" Visible="False">Debe seleccionar un hotel</asp:label></TD>
							</TR>
							<TR>
								<TD style="HEIGHT: 24px" colSpan="3" align="center"></TD>
							</TR>
							<TR>
								<TD align="left"><asp:label id="lblNombre" runat="server" EnableViewState="False" CssClass="clslabel">Email Bussiness:</asp:label></TD>
								<TD colSpan="2" align="left"><asp:textbox id="txtbussinesEmail" runat="server" Width="288px"></asp:textbox><asp:requiredfieldvalidator id="RequiredFieldValidatorEmail" runat="server" ControlToValidate="txtbussinesEmail"
										ErrorMessage="**"></asp:requiredfieldvalidator></TD>
							</TR>
							<TR>
								<TD align="left"></TD>
								<TD colSpan="2" align="center"><asp:regularexpressionvalidator style="Z-INDEX: 0" id="RegularExpressionValidatorEmail" runat="server" CssClass="Validators"
										ControlToValidate="txtbussinesEmail" ErrorMessage="El Email debe ser valido" Display="Dynamic" ValidationExpression='^(([^<;>;()[\]\\.,;:\s@\""]+(\.[^<;>;()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$'></asp:regularexpressionvalidator></TD>
							</TR>
							<TR>
								<TD align="left"><asp:label id="lblPorcMinimo" runat="server" EnableViewState="False" CssClass="clslabel">Url Pago Existoso:</asp:label></TD>
								<TD width="65%" colSpan="2" align="left"><asp:textbox style="Z-INDEX: 0" id="txtsuccessReturn" runat="server" Width="392px"></asp:textbox><asp:requiredfieldvalidator style="Z-INDEX: 0" id="RequiredfieldvalidatorReturnUrl" runat="server" ControlToValidate="txtsuccessReturn"
										ErrorMessage="**"></asp:requiredfieldvalidator></TD>
							</TR>
							<TR>
								<TD align="left"></TD>
								<TD width="65%" colSpan="2" align="center"><asp:label style="Z-INDEX: 0" id="msgErrorReturnUrl" runat="server" CssClass="Validators" Visible="False">La url debe ser valida</asp:label></TD>
							</TR>
							<TR>
								<TD align="left"><asp:label id="lblPorcMaximo" runat="server" EnableViewState="False" CssClass="clslabel">Url Cancelacion Pago:</asp:label></TD>
								<TD colSpan="2" align="left"><asp:textbox style="Z-INDEX: 0" id="txtcancelReturn" runat="server" Width="392px"></asp:textbox><asp:requiredfieldvalidator style="Z-INDEX: 0" id="RequiredfieldvalidatorCancel" runat="server" ControlToValidate="txtcancelReturn"
										ErrorMessage="**"></asp:requiredfieldvalidator></TD>
							</TR>
							<TR>
								<TD align="left"></TD>
								<TD colSpan="2" align="center"><asp:label style="Z-INDEX: 0" id="msgErrorCancelUrl" runat="server" CssClass="Validators" Visible="False">La url debe ser valida</asp:label></TD>
							</TR>
							<TR>
								<TD align="left"></TD>
								<TD colSpan="2" align="left">&nbsp;</TD>
							</TR>
							<TR>
								<TD align="left"></TD>
								<TD colSpan="2" align="left"></TD>
							</TR>
							<TR>
								<TD colSpan="3" align="center"></TD>
							</TR>
							<TR>
								<TD colSpan="3" align="center"><asp:label style="Z-INDEX: 0" id="lblMsgActualizacion" runat="server" CssClass="Validators"
										Visible="False"> Operacion Exitosa</asp:label></TD>
							</TR>
							<TR>
								<TD style="HEIGHT: 45px" colSpan="3" align="center"><asp:button id="btnAceptar" runat="server" EnableViewState="False" CssClass="button" Text="Guardar"></asp:button>
									<asp:button style="Z-INDEX: 0" id="btnEliminar" runat="server" EnableViewState="False" CssClass="button"
										CausesValidation="False" Text="Eliminar" Visible="False"></asp:button><asp:button style="Z-INDEX: 0" id="btnEliminar2" runat="server" EnableViewState="False" CssClass="button"
										CausesValidation="False" Text="Eliminar"></asp:button>&nbsp;&nbsp;<asp:button id="btnCancel" runat="server" EnableViewState="False" CssClass="button" CausesValidation="False"
										Text="Cancelar"></asp:button></TD>
							</TR>
						</TABLE>
					</td>
				</tr>
			</TABLE>
			<uc1:ctlmensajes id="CtlMensajes1" runat="server"></uc1:ctlmensajes></form>
	</body>
</HTML>
