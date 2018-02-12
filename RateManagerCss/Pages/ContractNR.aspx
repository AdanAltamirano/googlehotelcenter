<%@ Import Namespace="RateManager" %>
<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../Portal/Modules/ctrlFooter.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../Portal/Modules/ctrlHeader.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ContractNR.aspx.vb" Inherits="RateManager.ContractNR"%>
<%@ Register TagPrefix="uc1" TagName="ctlMensajes" Src="../Modulos/ctlMensajes.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrMenu" Src="../Portal/Modules/CtrMenu.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>	
		<title>Contracts Net Rates</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
	</HEAD>
	<script>
function ShowNewInfo(value) {
    if (value == 1) {
        document.getElementById("dvContent").style.display = "block";
        document.getElementById("dvContent2").style.display = "block";

        document.getElementById("<%=btnAceptar.clientid %>").style.display = "block";
        document.getElementById("<%=btncancel.clientid %>").style.display = "block";
        

    } else {
        document.getElementById("dvContent").style.display = "none";
        document.getElementById("dvContent2").style.display = "none";
        document.getElementById("<%=btnAceptar.clientid %>").style.display = "none";
        document.getElementById("<%=btncancel.clientid %>").style.display = "none";
        
        }
    }

    function FireShow(ID, IDcmd, show) {
        var e = document.getElementById(ID);
        var c = document.getElementById(IDcmd);
        if (e) {
            e.style.display = show ? 'block' : 'none';
        }
        if (c) {
            c.style.display = !show ? 'block' : 'none';
        }
        onResizeIframe();
    }
    
</script>
	<body MS_POSITIONING="FlowLayout" bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0">
		<form id="Form1" method="post" runat="server">
            <div class="clear">
                    <input type="button" id="cmdNew" class="ButtonNew" runat="server" causesvalidation="false" value="New" style=" width:85px;"/>
                    <div class="mDiv"></div>
                    <div class="title">
                        <asp:label id="lblTitle" runat="server" EnableViewState="False" class="tituloSeccion">Net Rates Contract</asp:label>
                    </div>
            </div>
		
		  <div runat="server" id="divContenedor">
		  <table id="bookingcontainer clear" border="0" cellspacing="0" cellpadding="2"  width="100%">           
            <tr class="trTitle rounded-corners" id="dvContent2">
                                <td class="dgitem" align="left">
                                    <asp:Label ID="lblMsg" runat="server" DESIGNTIMEDRAGDROP="987">Modificando tipo de habitación</asp:Label>
                                </td>
            </tr>
             <tr class="trContent rounded-corners" id="dvContent">
                                <td class="tdContent">
            	<TABLE id="Table1" class="Form" cellSpacing="0" cellPadding="0" width="100%" border="0" align="center">
												
							
							<TR>
								<TD align="right" colSpan="2">
									<asp:label id="lblNombre" runat="server" EnableViewState="False" CssClass="clslabel">Nombre:</asp:label></TD>
								<TD align="left">
									<asp:TextBox id="TextBoxNombre" runat="server" Width="392px"></asp:TextBox>
									<asp:Label id="lblMsgErrorNombre" runat="server" CssClass="Validators" Visible="False">El nombre debe ser valido</asp:Label></TD>
							</TR>
							<TR>
								<TD align="right" colSpan="2">
									<asp:label id="lblPorcMinimo" runat="server" EnableViewState="False" CssClass="clslabel">Porc. Mín. Ganancia UV:</asp:label></TD>
								<TD align="left" >
									<asp:TextBox id="TextBoxPorcMin" runat="server" Width="48px"></asp:TextBox>
									<asp:label id="Label3" runat="server" EnableViewState="False" CssClass="clslabel">%</asp:label>
									<asp:Label id="lblMsgErrorMinimo" runat="server" CssClass="Validators" Visible="False">El porcentaje Minimo debe ser Valido</asp:Label>
									<asp:requiredfieldvalidator id="Requiredfieldvalidator2" runat="server" CssClass="Validators" ControlToValidate="TextBoxPorcMin"
										ErrorMessage="*" Display="Dynamic" Enabled="False"></asp:requiredfieldvalidator></TD>
							</TR>
							<TR>
								<TD align="right" colSpan="2">
									<asp:label id="lblPorcMaximo" runat="server" EnableViewState="False" CssClass="clslabel">Porc. Máx. Ganancia UV:</asp:label></TD>
								<TD align="left">
									<asp:TextBox id="TextBoxPorcMax" runat="server" Width="48px"></asp:TextBox>
									<asp:label id="Label4" runat="server" EnableViewState="False" CssClass="clslabel">%</asp:label>
									<asp:Label id="lblMsgErrorMaximo" runat="server" CssClass="Validators" Visible="False">El porcentaje Maximo debe ser Valido</asp:Label>
									<asp:requiredfieldvalidator id="Requiredfieldvalidator3" runat="server" CssClass="Validators" ControlToValidate="TextBoxPorcMax"
										ErrorMessage="*" Display="Dynamic" Enabled="False"></asp:requiredfieldvalidator></TD>
							</TR>
							<TR>
								<TD align="center" colSpan="3"></TD>
							</TR>
							<TR>
								<TD align="center" colspan="3">
									<asp:button id="btnAceptar" runat="server" EnableViewState="False" CssClass="button" Text="Guardar" CausesValidation=true ></asp:button>
									<asp:button id="btnCancel" runat="server" EnableViewState="False" CssClass="button" Text="Cancelar"
										CausesValidation="False"></asp:button>
								</TD>
							</TR>
							
						</TABLE>
            </td>
            </tr>
            <TR>
								<TD align="center" >
									<asp:Label id="lblMsgActualizacion" runat="server" CssClass="Validators" Visible="False"> Operacion Exitosa</asp:Label></TD>
							</TR>
            </table>
		  </div>
		  
		   <div class="clear">
		   	<asp:datagrid id="dgcontract" GridLines="None" runat="server" ShowFooter="True" CssClass="datagrid" PageSize="20"
										AllowPaging="True" Width="99%" AutoGenerateColumns="False">
										<FooterStyle HorizontalAlign="Right"></FooterStyle>
										<SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
										<AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
										<ItemStyle CssClass="dgItem"></ItemStyle>
										<HeaderStyle CssClass="dgHeader"></HeaderStyle>
										<Columns>
											<asp:BoundColumn DataField="Nombre" HeaderText="Nombre">
												<ItemStyle Width="30%"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="idContrato">
												<ItemStyle Width="30%"></ItemStyle>
											</asp:BoundColumn>
											<asp:TemplateColumn>
												<ItemStyle Width="10%"></ItemStyle>
												<ItemTemplate>
													<asp:LinkButton id="lnkEdit" runat="server" CommandName="Select" CssClass="dgLink">Editar</asp:LinkButton>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn>
												<ItemStyle Width="10%"></ItemStyle>
												<ItemTemplate>
													<asp:LinkButton id="lnkEliminar2" style="display:none" runat="server" CssClass="dgLink" CommandName="Eliminar">Eliminar</asp:LinkButton>
													<asp:HyperLink id="lnkEliminar" runat="server" CssClass="dglink">Eliminar</asp:HyperLink>
												</ItemTemplate>
											</asp:TemplateColumn>
										</Columns>
										<PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
											Position="Bottom" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
									</asp:datagrid>
		   
		   </div>
			<TABLE id="bookingcontainer" cellSpacing="0" cellPadding="2" width="650" border="0">
				<tr>
					<td>
					
					</td>
				</tr>
			</TABLE>
			<uc1:ctlMensajes id="CtlMensajes1" runat="server"></uc1:ctlMensajes>
		</form>
	</body>
</HTML>
