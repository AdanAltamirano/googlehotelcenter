<%@ Import Namespace="RateManager" %>
<%@ Register TagPrefix="uc1" TagName="ctrRatePlan" Src="../Modulos/ctrRatePlan.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctlMensajes" Src="../Modulos/ctlMensajes.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlAgencyRates" Src="../Modulos/ctrlAgencyRates.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="AgencyRates.aspx.vb" Inherits="RateManager.AgencyRates"%>
<%@ Register src="../Modulos/ctrlAutoComplete.ascx" tagname="ctrlAutoComplete" tagprefix="uc2" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Agency Rates</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
		
        <script src="../Pages/Scripts/jquery.min.js" type="text/javascript"></script>
        <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js"></script>
        <script type="text/javascript" src="../Includes/Script/JsSearch-1.0.js"></script>

	</HEAD>
		<script>
	    function ShowNewInfo(value) {
	        if (value == 1) {
	            document.getElementById("dvContent").style.display = "";
	            document.getElementById("dvContent2").style.display = "";

	            document.getElementById("<%=btnSave.clientid %>").style.display = "";
	            document.getElementById("<%=btnNuevo.clientid %>").style.display = "";
	            var btn = document.getElementById("<%=btnPublish.clientid %>");
	            if (btn) {
	                btn.style.display = "";
	            }

	        } else {
	            document.getElementById("dvContent").style.display = "none";
	            document.getElementById("dvContent2").style.display = "none";
	            document.getElementById("<%=btnSave.clientid %>").style.display = "none";
	            document.getElementById("<%=btnNuevo.clientid %>").style.display = "none";
	            var btn = document.getElementById("<%=btnPublish.clientid %>");
	            if (btn) {
	                btn.style.display = "none";
	            }
	        }
	    }
	    
	    function FireShow(ID, IDcmd, show) {
	        var e = document.getElementById(ID);
	        var c = document.getElementById(IDcmd);
	        if (e) {
	            e.style.display = show ? '' : 'none';
	        }
	        if (c) {
	            c.style.display = !show ? '' : 'none';
	        }
	        onResizeIframe();
	    }

	    function validarkeyCode(e) { // 1
	        tecla = (document.all) ? e.keyCode : e.which; // 2
	        if (tecla == 8) return true; // 3
	        patron = /[A-Za-z0-9\s]/; // 4
	        te = String.fromCharCode(tecla); // 5
	        return patron.test(te); // 6
	    } 

	    SearchStart.AddParam
	    (
		    {
		    searchitems: [
			{ Item: 'agenciasRates', IDSearch: 'idAgenciasRate', nameSearch: 'rateCode', isdefault: true },
			{ Item: 'agenciasRates', IDSearch: 'idAgenciasRate', nameSearch: 'name', isdefault: true }
			],
		    colModel: [
			    { display: '<%= RateManager.PortalCulture.GetString("00001") %>' },
			    { display: '<%= RateManager.PortalCulture.GetString("00073") %>' }
			],
		    Data: [{ catalogo: 'AgencyRate', idHotel: '<%= MyBase.cInfoActual.Hotel %>',
		        idIdioma: '<%= RateManager.PortalCulture.GetIDCulture %>'
             }],
		        id: 'Agency',
		        index: 1
		    }
	     );
	
</script>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
		<div class="clear">
        <input type="button" id="cmdNew" class="ButtonNew" runat="server" causesvalidation="false" value="New" style=" width:85px;"  />
        <div class="mDiv">
        </div>
        <div class="title">
            <asp:label id="lblTitle" runat="server" EnableViewState="False"  class="tituloSeccion"> Agency Rates</asp:label>
        </div>
        </div>
		  <div runat="server" id="divContenedor">
			<table id="bookingcontainer clear" border="0" cellspacing="0" cellpadding="2" width="100%">
				<tr>
					<td>
						<TABLE id="Table2" cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">				
							 <tr class="trTitle rounded-corners"  id="dvContent2">
                                <td class="dgitem" align="left">
                                    <asp:label id="lblctrtitulo" EnableViewState="False" CssClass="bookingnormallabel" runat="server"> Nuevo Paquete</asp:label>
                                </td>
                            </tr>
							
							     <tr class="trContent rounded-corners"  id="dvContent">
                                <td class="tdContent">
                                
											<uc1:ctrlagencyrates id="CtrlAgencyRates1" runat="server"></uc1:ctrlagencyrates><br />
											
									<% 									    If Me.IsSupervisor Then
									        Me.btnPublish.Text = RateManager.PortalCulture.GetString("01364")
									%>
									<asp:Button id="btnPublish" runat="server" Text="Publicar" CssClass="button" EnableViewState="False"></asp:Button>
									<% End If%>
									<asp:button id="btnSave" runat="server" EnableViewState="False" CssClass="Button" Text="Guardar"></asp:button>
									<asp:button id="btnNuevo" runat="server" EnableViewState="False" CssClass="Button" Text=" Nuevo "
										CausesValidation="False"></asp:button>
											</TD>
							</TR>
							<TR>
								<TD align="center" colSpan="2"><asp:label id="lblError" runat="server" CssClass="Validators" Visible="False">Error</asp:label><asp:label id="lblErrorSource" runat="server" CssClass="Validators" Visible="False">*</asp:label></TD>
							</TR>
							
						</TABLE>
					    
					</td>
				</tr>
			</TABLE>
			</div>
			  <div class="clear">
			  
			  <uc2:ctrlAutoComplete ID="ctrlAutoComplete1" runat="server" />
			  <asp:datagrid id="dgRates"  GridLines="None" runat="server" ShowFooter="True" CssClass="datagrid" AutoGenerateColumns="False"
										AllowPaging="True" Width="99%" PageSize="20">
										<SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
										<AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
										<ItemStyle CssClass="dgItem"></ItemStyle>
										<HeaderStyle CssClass="dgHeader"></HeaderStyle>
										<FooterStyle HorizontalAlign="Right"></FooterStyle>
										<Columns>
											<asp:BoundColumn DataField="RateCode" HeaderText="C&#243;digo">
												<HeaderStyle Width="10%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="Name" HeaderText="Nombre">
												<HeaderStyle Width="40%"></HeaderStyle>
											</asp:BoundColumn>
											<asp:TemplateColumn>
												<HeaderStyle Width="15%"></HeaderStyle>
												<ItemTemplate>
													<asp:LinkButton id="lnkedit" runat="server" CssClass="dgLink" CausesValidation="False" CommandName="Select">editar</asp:LinkButton>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn>
												<HeaderStyle Width="15%"></HeaderStyle>
												<ItemStyle HorizontalAlign="Center"></ItemStyle>
												<ItemTemplate>
													<asp:LinkButton id="lnkEliminar2" style="DISPLAY: none" runat="server" CssClass="dgLink" CausesValidation="False"
														CommandName="Delete">-</asp:LinkButton>
													<asp:HyperLink id="lnkEliminar" runat="server" CssClass="dgLink">Eliminar</asp:HyperLink>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn Visible="False" DataField="idrateplan"></asp:BoundColumn>
										</Columns>
										<PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
											Position="Bottom" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
									</asp:datagrid>
			  </div>
			<uc1:ctlmensajes id="CtlMensajes1" runat="server"></uc1:ctlmensajes></form>
	</body>
</HTML>
