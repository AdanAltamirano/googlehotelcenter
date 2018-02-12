<%@ Import NameSpace = "RateManager" %>
<%@ Register TagPrefix="uc1" TagName="CtrlIdioma" Src="CtrlIdioma.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrPortal" Src="ctrPortal.ascx" %>
<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ctrlAgencyRates.ascx.vb" Inherits="RateManager.ctrlAgencyRates" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="uc1" TagName="SearchAgency" Src="SearchAgency.ascx" %>

<script src="../Pages/Scripts/jquery.min.js" type="text/javascript"></script>
<script type="text/javascript">

    $(document).ready(function() {
        var options = new Array();
        var list = $('#<%= Me.lstRatePlans.ClientId %>');
        var contractList = $('#<%= Me.ddlContratosNR.ClientId %>');

        list.find('option:gt(0)').each(function() {
            var current = $(this);
            var value = eval('(' + current.val() + ')');
            current.html(value.code + ' -- ' + value.name);
            options.push(current);
        });
        list.data('options', options);


        list.bind('filter', function(e, contract) {

            var selected = null;
            if (list.find('option:gt(0):selected').length > 0)
                selected = eval('(' + list.find('option:selected').val() + ')');

            list.find('option:selected').removeAttr('selected');
            list.find('option:gt(0)').remove();

            $.each(list.data('options'), function() {
                var current = $(this);
                current.removeAttr('selected');
                var value = eval('(' + current.val() + ')');
                //if(value.contract == contract){
                if ((value.contract > 0) == (contract > 0)) {
                    list.append(this);
                    if (selected != null && value.id == selected.id)
                        current.attr('selected', 'selected');
                }
            });
            if (list.find('option:selected').length == 0)
                list.find('option:eq(0)').attr('selected', 'selected');

        }); 

        contractList.change(function() {
            list.trigger('filter', [(this.selectedIndex > 0)]);
        });

        contractList.change();
    });
		
	function onCheckBoxAgencias(chk, td, txt){
		var chkA = document.getElementById(chk);
		var tdA = document.getElementById(td);
		var txtA = document.getElementById(txt);
		
		if (chkA && tdA && txtA)
		{
			if (chkA.checked)
			{
				txtA.value=""
			}
			//tdA.style.display = chkA.checked ? "none" : "";
			var rnb = document.getElementById("CtrlAgencyRates1_renglonBuscarPor");			
			var rni = document.getElementById("CtrlAgencyRates1_RenglonIATA");		
			var rna = document.getElementById("CtrlAgencyRates1_RenglonAgencia");		
			var rntragencias = document.getElementById("CtrlAgencyRates1_TrAgencias");		
			
		    rnb.style.display = chkA.checked ? "none" : "";								
		    if (chkA.checked==true)
		    {
				rni.style.display = "none";
				rna.style.display = "none";
		    }
		    else
		    {
				var rdbi = document.getElementById("CtrlAgencyRates1_rdbIATA");
				if (rdbi.checked==true)
				 {
					onrdbIATA("CtrlAgencyRates1_RenglonIATA", "CtrlAgencyRates1_RenglonAgencia", "CtrlAgencyRates1_rdbIATA");
				 }
				else				
				 {
					onrdbNombre("CtrlAgencyRates1_RenglonIATA", "CtrlAgencyRates1_RenglonAgencia", "CtrlAgencyRates1_rdbNombre");
				 }
		    }
		}
	  }

	function onrdbIATA(ri, ra, rbi)
	{
		var reni = document.getElementById(ri);
		var rena = document.getElementById(ra);
		//var rbiata = document.getElementById(rbi);

		reni.style.display = "";								
		rena.style.display = "none";								
		
	}
		
	function onrdbNombre(ri, ra, rbn)
	{
		var reni = document.getElementById(ri);
		var rena = document.getElementById(ra);
		//var rbnombre = document.getElementById(rbn);	
		
		reni.style.display = "none";
		rena.style.display = "";								
		
	}		
		
	function onCheckBoxesClick(chk, td, tdpor){
		var c = document.getElementById(chk);
		var t = document.getElementById(td);
		var tpc = document.getElementById(tdpor);
				 
		if (c && t && tpc){			
			t.style.display = c.checked ? "" : "none";
			tpc.style.display = c.checked ? "" : "none";			
		}
	}
	
	function onCheckBox(chk, td){
		var c = document.getElementById(chk);
		var t = document.getElementById(td);
		 
		if (c && t){
		    t.style.display = c.checked ? "" : "none";			
		}
	}	
	
	function showRowPortal(chk, td,rw){
		var c = document.getElementById(chk);
		var t = document.getElementById(td);
		var rp = document.getElementById(rw);
		 
		if (c && t && rp){
		    t.style.display = c.checked ? "" : "none";
		    rp.style.display = c.checked ? "" : "none";			
		}		
	}	
	
	function showRowPromotion(b)
	{	//alert(b);
	if (b==true)
		  {
			//document.getElementById("trHeadProm").style.display="block";
			//document.getElementById("trHeadProm1").style.display="block";
			//document.getElementById("trHeadProm2").style.display="block";
		  }
		else
		  {
		  
			//document.getElementById("trHeadProm").style.display="none";
			//document.getElementById("trHeadProm1").style.display="none";
			//document.getElementById("trHeadProm2").style.display="none";
		  }
	}	


function Name(ddl,txt,array)
{
var e = document.getElementById(ddl);
var lbl = document.getElementById(txt);
if (e!=null && lbl!=null && array!=null)
{
lbl.value =array.split("//")[e.selectedIndex +1];
}
}
</script>
 <iframe id="gToday:normal:agenda.js" style="Z-INDEX: 999; LEFT: -500px; POSITION: absolute; TOP: -500px" 
            name="gToday:normal:agenda.js"
            src='<%=GeRequestApplicationPath(string.concat("/Calendar/",portalculture.getculture().Name.Substring(0, 2).ToLower(),"/ipopeng2.htm" )) %>'
            frameborder="0" width="174" scrolling="no" height="189">
  </iframe>
<TABLE width="98%" class="Form" >
	<TBODY>
		
		<TR>
			<TD width="15%"  align="right"><asp:label id="lblRatecode" EnableViewState="False" CssClass="clsLabel" runat="server">Codigo Tarifa Agencia</asp:label></TD>
			<TD  colSpan="2"><asp:textbox id="txtRateCode" runat="server" MaxLength="4" Width="56px"></asp:textbox></TD>
		</TR>
		<TR>
			<TD  align="right"><asp:label id="lblname" EnableViewState="False" CssClass="clslabel" runat="server">Nombre</asp:label></TD>
			<TD colSpan="2"><uc1:ctrlidioma id="txtShortDescription" runat="server" IsMultiline="false"></uc1:ctrlidioma></TD>
		</TR>
		<TR>
			<TD  align="right"><asp:label id="lblDescripcion" EnableViewState="False" CssClass="clsLabel" runat="server">Descripción</asp:label></TD>
			<TD colSpan="2"><uc1:ctrlidioma id="txtDescripcion" runat="server"></uc1:ctrlidioma></TD>
		</TR>
		<TR>
			<TD  align="right"><asp:label id="lblRule" EnableViewState="False" CssClass="clsLabel" runat="server">Regla</asp:label></TD>
			<TD colSpan="2"><asp:dropdownlist id="ddlRules" runat="server"></asp:dropdownlist></TD>
		</TR>
		<TR>
			<TD  align="right"><asp:label style="Z-INDEX: 0" id="lblContratos" runat="server">Contrato Tarifa Neta:</asp:label></TD>
			<TD colSpan="2"><asp:dropdownlist style="Z-INDEX: 0" id="ddlContratosNR" runat="server" Width="176px"></asp:dropdownlist></TD>
		</TR>
		<TR>
			<TD  align="right"><asp:label id="lblRateApply" EnableViewState="False" CssClass="clslabel" runat="server">Disponible en:</asp:label></TD>
			<TD colSpan="2"><asp:checkbox id="chkGDS" CssClass="clslabel" runat="server" Checked="True" Text="GDS"></asp:checkbox>&nbsp;<asp:checkbox id="chkUnipantalla" CssClass="clslabel" runat="server" Text="UniPantalla"></asp:checkbox>&nbsp;
				<asp:checkbox id="chkPortal" onclick="showRowPromotion(this.checked);" CssClass="clslabel" runat="server"
					Text="Portal" onchange="showRowPromotion(this.checked);"></asp:checkbox><asp:checkbox id="chkADS" CssClass="clslabel" runat="server" Text="ADS"></asp:checkbox></TD>
		</TR>
		<TR id="trGDSApply" runat="server">
			<TD id="tdGDSApply1" style="WIDTH: 236px; HEIGHT: 38px" align="right" runat="server">
			<asp:label id="lblAplicaGDS" EnableViewState="False" runat="server">Aplica Para GDS</asp:label>
			</TD>
			<TD id="tdGDSApply2" style="HEIGHT: 38px" runat="server" colSpan="2">
				<TABLE id="Table1" style="WIDTH: 240px; HEIGHT: 24px" cellSpacing="1" cellPadding="1" width="240"
					border="0">
					<TR>
						<TD><asp:checkbox id="chkGDSAmadeus" runat="server" Text="Amadeus" CssClass="clslabel"></asp:checkbox></TD>
						<TD style="WIDTH: 92px"><asp:checkbox id="chkGDSGalileo" CssClass="clslabel" runat="server" Text="Galileo"></asp:checkbox></TD>
						<TD style="WIDTH: 95px"><asp:checkbox id="chkGDSabre" CssClass="clslabel"  runat="server" Text="Sabre"></asp:checkbox></TD>
						<TD><asp:checkbox id="chkGDSWorldSpan" CssClass="clslabel" runat="server" Text="WorldSpan"></asp:checkbox></TD>
					</TR>
				</TABLE>
			</TD>
		</TR>
		<TR id="trPortal" runat="server">
			<TD  vAlign="top" align="right"></TD>
			<TD colSpan="2">
				<uc1:ctrPortal id="CtrPortal1" runat="server"></uc1:ctrPortal></TD>
		</TR>
		<TR>
			<TD  vAlign="top" align="right"><asp:label id="lblComision" runat="server">Comisiones:</asp:label></TD>
			<TD colSpan="2">
				<TABLE id="Table4" style="WIDTH: 176px" cellSpacing="1" cellPadding="1" width="176" border="0">
					<TR id="trPorcGDS" runat="server">
						<TD style="WIDTH: 84px"><asp:label id="lblPorcGDS" runat="server">GDS</asp:label></TD>
						<TD><asp:textbox id="txtPorcGDS" runat="server" MaxLength="3" Width="32px">10</asp:textbox>%</TD>
					</TR>
					<TR id="trPorcUni" runat="server">
						<TD style="WIDTH: 84px"><asp:label id="lblPorcUni" runat="server">UniPantalla</asp:label></TD>
						<TD><asp:textbox id="txtPorcUNI" runat="server" MaxLength="3" Width="32px">10</asp:textbox>%</TD>
					</TR>
					<TR id="trPorcPortal" runat="server">
						<TD style="WIDTH: 84px">
							<P><asp:label id="lblPorcPortal" runat="server">Portal</asp:label></P>
						</TD>
						<TD><asp:textbox id="txtPorcPOR" runat="server" MaxLength="3" Width="32px">10</asp:textbox>%</TD>
					</TR>
					<TR id="trPorcADS" runat="server">
						<TD style="WIDTH: 84px">
							<P><asp:label id="lblPorcADS" runat="server">ADS</asp:label></P>
						</TD>
						<TD><asp:textbox id="txtPorcADS" runat="server" MaxLength="3" Width="32px">10</asp:textbox>%</TD>
					</TR>
				</TABLE>
			</TD>
		</TR>
		<tr>
		<td></td>		
		<td align ="left" colspan =2  class="dgItem"><asp:label id="lblPromotiontitle" CssClass="clslabel" runat="server">Configuración de promociones para portales</asp:label></td>
		
		</tr>
		<tr>
		    <TD align="right"   ><asp:label id="lblPromotion"  CssClass="clslabel" EnableViewState="False" runat="server">Promotion </asp:label></TD>
			<TD colSpan="2"><asp:textbox id="txtDescProm" CssClass="textbox" runat="server" MaxLength="5" Width="48px"></asp:textbox><asp:label id="lblDesc" EnableViewState="False" runat="server">%</asp:label></TD>
		</tr>
		<tr>
		    <td align="right"   ><span  CssClass="clslabel" ><%=RateManager.PortalCulture.GetString("01399", True)%></span></td>
		    <td colSpan="2">
		        <uc1:ctrlidioma IsMultiline="false" MaxLength="50" RequiredText="false" id="txtPromoDescription" runat="server"></uc1:ctrlidioma>
		    </td>
		</tr>
		<TR>
			<TD  vAlign="top" align="right"><asp:label id="lblAgencia" runat="server">Agencias:</asp:label></TD>
			<TD colSpan="2"><asp:checkbox id="chkTodasAgencias" runat="server" Text="Todas las agencias"></asp:checkbox></TD>
		</TR>
		<TR id="renglonBuscarPor" runat="server">
			<TD  vAlign="top" align="right"><asp:label id="lblBuscarPor" runat="server">Buscar Por</asp:label></TD>
			<TD colSpan="2"><asp:radiobutton id="rdbIATA" runat="server" Checked="True" Text="IATA" GroupName="1"></asp:radiobutton><asp:radiobutton id="rdbNombre" runat="server" Text="Nombre" GroupName="1"></asp:radiobutton></TD>
		</TR>
		<TR id="RenglonIATA" runat="server">
			<TD  vAlign="top" align="right"></TD>
			<TD colSpan="2"><asp:label id="lblIata" runat="server">IATA</asp:label><asp:textbox id="txtAgencia" runat="server" MaxLength="10" Width="96px"></asp:textbox><asp:button id="btnAgencia" runat="server" Text="Agregar" CausesValidation="False"></asp:button></TD>
		</TR>
		<TR id="RenglonAgencia" runat="server">
			<TD vAlign="top" align="right"></TD>
			<TD vAlign="baseline" style="WIDTH: 253px"><uc1:searchagency id="SearchAgency1" runat="server"></uc1:searchagency></TD>
			<td vAlign="top"><asp:button id="btnAgregarTourOp" runat="server" Text="Agregar" CausesValidation="False"></asp:button></td>
		</TR>
		<TR id="TrAgencias" runat="server">
			<TD id="TrAgencias1"  vAlign="top" align="right"></TD>
			<TD id="TrAgencias2" runat="server" colSpan="2">
				<P>&nbsp;&nbsp;</P>
				<P><asp:datagrid id="dgAgencias" runat="server" AutoGenerateColumns="False">
						<Columns>
							<asp:BoundColumn DataField="IATA" HeaderText="IATA"></asp:BoundColumn>
							<asp:BoundColumn DataField="Nombre" HeaderText="Nombre"></asp:BoundColumn>
							<asp:TemplateColumn>
								<ItemTemplate>
									<asp:LinkButton id="LinkButton1" runat="server" CommandName="Eliminar">Eliminar</asp:LinkButton>
								</ItemTemplate>
							</asp:TemplateColumn>
						</Columns>
					</asp:datagrid></P>
			</TD>
		</TR>
		<% If not Me.edicion then %>
		<tr><td align="center" colSpan="3">
		    <br /><span class="bookingnormallabel"><%=RateManager.PortalCulture.GetString("01413")%></span>
		</td></tr>
		<tr><td align="center" colSpan="3">
		    <br /><i><%=RateManager.PortalCulture.GetString("01414")%></i>
		</td></tr>
	    <tr><td align="right">
	        <%=RateManager.PortalCulture.GetString("01415")%>
	    </td><td colSpan="3">
		    <asp:DropDownList ID="lstRatePlans" runat="server" style="width:300px;"></asp:DropDownList>
	    </td></tr>
		<% End If %>
        <% If Me.edicion AndAlso (Not Me.txtDescripcion.Published OrElse Not Me.txtShortDescription.Published) Then%>
        <tr>
            <td colspan="3">
                <span class="validators"><%=RateManager.PortalCulture.GetString(If(CType(Me.Page, RateManager.PaginaBase).IsSupervisor, "01365", "01392"))%></span>
            </td>
        </tr>
        <% end if %>
	</TBODY>
	<% response.write("<script>  showRowPortal('" & chkPortal.Clientid & "','" & trPorcPortal.clientid & "','" & trPortal.clientid & "');</script>")%>
</TABLE>

