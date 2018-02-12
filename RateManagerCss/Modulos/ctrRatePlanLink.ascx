<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ctrRatePlanLink.ascx.vb" Inherits="RateManager.ctrRatePlanLink" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script>
	/*	function showRatePlan2(ddl,array,lbl)
		 { 
		  var e = document.getElementById(ddl);
		  var lbl = document.getElementById(lbl);		  		  	  
		  lbl.firstChild.nodeValue =array.split("//")[e.selectedIndex + 1];		  		  
		  }*/
</script>
<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">
	<tr>
		<TD align="center" colspan="6"><asp:textbox id="txtDivVisible" style="DISPLAY: none" runat="server" Width="34px" cssclass="TextBox"></asp:textbox></TD>
	</tr>
	
	<tr>
		<TD align="right"><asp:label id="lblERatePlanSource" runat="server" CssClass="clsLabel" EnableViewState="False">Rate Plan Source</asp:label></TD>
		<TD colspan="2"><asp:dropdownlist id="ddlRatePlanSource" runat="server"></asp:dropdownlist></TD>
		<TD align="right"><asp:label id="lblERatePlanTarget" runat="server" CssClass="clsLabel" EnableViewState="False">Rate Plan Target</asp:label></TD>
		<TD align="left" colspan="2"><asp:dropdownlist id="ddlRatePlanTarget" runat="server"></asp:dropdownlist>
            <asp:textbox id="txtTarget" runat="server" CssClass="textbox" Enabled="False" 
                Width="202px" Visible="False"></asp:textbox></TD>
	</tr>
	
	<tr>
		<td align="right">
			<asp:label id="lblSoldOut" runat="server" CssClass="clsLabel" EnableViewState="False">Sold out percent</asp:label></td>
		<td colspan="5"><asp:textbox id="txtSoldOut" runat="server" CssClass="textbox" Width="64px" MaxLength="5"></asp:textbox>
			<asp:rangevalidator id="RvSoldOut" runat="server" CssClass="validators" ControlToValidate="txtSoldOut"
				MaximumValue="100" MinimumValue="0" Type="Double" ErrorMessage="0-100" Display="Dynamic"></asp:rangevalidator>
            <asp:RequiredFieldValidator ID="rfvSoldPercent" runat="server" CssClass="validators"
                ErrorMessage="RequiredFieldValidator" ControlToValidate=txtSoldOut Display=Dynamic ></asp:RequiredFieldValidator>
        </td>
	</tr>
	<tr>
		<TD align="center" colSpan="6"><asp:label id="lblNoLink" runat="server" CssClass="validators" Visible="False">No puede hacerse un link sobre sí mismo</asp:label></TD>
	</tr>
	<TR>
		<TD class="dgitem" align="center" colSpan="6">
			<asp:Label id="lbllinkdata" runat="server" EnableViewState="False">link data</asp:Label></TD>
	</TR>
	<TR>
		<TD align="right"><asp:label id="lbl_Ratio" runat="server" CssClass="clsLabel" EnableViewState="False">Ratio</asp:label></TD>
		<TD><div runat="server" id="divratio"><asp:textbox id="txtRatio" runat="server" CssClass="textbox" Width="64px" MaxLength="4"></asp:textbox><asp:rangevalidator id="RvRatio" runat="server" CssClass="validators" Display="Dynamic" ErrorMessage="0-10"
					Type="Double" MinimumValue="0" MaximumValue="10" ControlToValidate="txtRatio"></asp:rangevalidator></div>
		</TD>
		<TD align="right"><asp:label id="lbl_Offset" runat="server" CssClass="clsLabel" EnableViewState="False">Offset</asp:label></TD>
		<TD><div runat="server" id="divoffset"><asp:textbox id="txtoffset" runat="server" CssClass="textbox" Width="64px" MaxLength="4"></asp:textbox><asp:rangevalidator id="RVOffset" runat="server" CssClass="validators" Display="Dynamic" ErrorMessage="-999 a 999"
					Type="Double" MinimumValue="-999" MaximumValue="999" ControlToValidate="txtoffset"></asp:rangevalidator></div>
		</TD>
		<TD colspan="2">
			<asp:hyperlink id="hplhide" runat="server" CssClass="hideOptions">Hide Ratio/offset by occupancy</asp:hyperlink>
			<asp:hyperlink id="hplShow" runat="server" CssClass="showOptions">Show Ratio/offset by occupancy</asp:hyperlink></TD>
	</TR>
	<TR>
		<TD align="center" colSpan="6">
		</TD>
	</TR>
	<TR>
		<TD align="center" colSpan="6">
			<div id="divDatos" runat="server">
				<TABLE>
					<TR>
						<TD align="right" colSpan="4"></TD>
						<TD align="right"></TD>
						<TD align="right"></TD>
					</TR>
					<TR>
						<TD width="25%"></TD>
						<TD width="10%"><asp:label id="lblRatio" runat="server" CssClass="clsLabel" EnableViewState="False">Ratio</asp:label></TD>
						<TD width="15%"><asp:label id="lblOffset" runat="server" CssClass="clsLabel" EnableViewState="False">Offset</asp:label></TD>
						<TD width="25%"></TD>
						<TD width="10%"><asp:label id="lbRatio" runat="server" CssClass="clsLabel" EnableViewState="False">Ratio</asp:label></TD>
						<TD width="15%"><asp:label id="lbOffset" runat="server" CssClass="clsLabel" EnableViewState="False">Offset</asp:label></TD>
					</TR>
					<TR>
						<TD align="right"><asp:label id="lblEOnePersonRate" runat="server" CssClass="clsLabel" EnableViewState="False">One Person Rate</asp:label></TD>
						<TD><asp:textbox id="txtOnePersonRatio" runat="server" CssClass="textbox" Width="64px" MaxLength="2"></asp:textbox></TD>
						<TD><asp:textbox id="txtOnePersonOffset" runat="server" CssClass="textbox" Width="64px" MaxLength="4"></asp:textbox></TD>
						<TD align="right"><asp:label id="lblEExtraAdultRate" runat="server" CssClass="clsLabel" EnableViewState="False">Extra Adult Rate</asp:label></TD>
						<TD><asp:textbox id="txtExtraAdultRatio" runat="server" CssClass="textbox" Width="64px" MaxLength="2"></asp:textbox></TD>
						<TD><asp:textbox id="txtExtraAdultOffset" runat="server" CssClass="textbox" Width="64px" MaxLength="4"></asp:textbox></TD>
					</TR>
					<TR>
						<TD align="right"></TD>
						<TD><asp:rangevalidator id="RVOnePersonRatio" runat="server" CssClass="validators" Display="Dynamic" ErrorMessage="0-10"
								Type="Double" MinimumValue="0" MaximumValue="10" ControlToValidate="txtOnePersonRatio"></asp:rangevalidator></TD>
						<TD><asp:rangevalidator id="RVOnePersonOffset" runat="server" CssClass="validators" Display="Dynamic" ErrorMessage="-999 a 999"
								Type="Double" MinimumValue="-999" MaximumValue="999" ControlToValidate="txtOnePersonOffset"></asp:rangevalidator></TD>
						<TD align="right"></TD>
						<TD><asp:rangevalidator id="RVExtraAdultRatio" runat="server" CssClass="validators" Display="Dynamic" ErrorMessage="0-10"
								Type="Double" MinimumValue="0" MaximumValue="10" ControlToValidate="txtExtraAdultRatio"></asp:rangevalidator></TD>
						<TD><asp:rangevalidator id="RVExtraAdultOffset" runat="server" CssClass="validators" Display="Dynamic" ErrorMessage="-999 a 999"
								Type="Double" MinimumValue="-999" MaximumValue="999" ControlToValidate="txtExtraAdultOffset"></asp:rangevalidator></TD>
					</TR>
					<TR>
						<TD align="right"><asp:label id="lblETwoPersonRAte" runat="server" CssClass="clsLabel" EnableViewState="False">Two Person Rate</asp:label></TD>
						<td><asp:textbox id="txtTwoPersonRatio" runat="server" CssClass="textbox" Width="64px" MaxLength="2"></asp:textbox></td>
						<TD><asp:textbox id="txtTwoPersonOffset" runat="server" CssClass="textbox" Width="64px" MaxLength="4"></asp:textbox></TD>
						<TD align="right"><asp:label id="lblEExtraChildRate" runat="server" CssClass="clsLabel" EnableViewState="False">Extra Child Rate</asp:label></TD>
						<TD style="HEIGHT: 43px"><asp:textbox id="txtExtraChildRatio" runat="server" CssClass="textbox" Width="64px" MaxLength="2"></asp:textbox></TD>
						<TD><asp:textbox id="txtExtraChildOffset" runat="server" CssClass="textbox" Width="64px" MaxLength="4"></asp:textbox></TD>
					</TR>
					<TR>
						<TD align="right"></TD>
						<TD>
							<asp:rangevalidator id="RVTwoPersonRatio" runat="server" CssClass="validators" Display="Dynamic" ErrorMessage="0-10"
								Type="Double" MinimumValue="0" MaximumValue="10" ControlToValidate="txtTwoPersonRatio"></asp:rangevalidator></TD>
						<TD>
							<asp:rangevalidator id="RVTwoPersonOffset" runat="server" CssClass="validators" Display="Dynamic" ErrorMessage="-999 a 999"
								Type="Double" MinimumValue="-999" MaximumValue="999" ControlToValidate="txtTwoPersonOffset"></asp:rangevalidator></TD>
						<TD align="right"></TD>
						<TD>
							<asp:rangevalidator id="RVExtraChildRatio" runat="server" CssClass="validators" Display="Dynamic" ErrorMessage="0-10"
								Type="Double" MinimumValue="0" MaximumValue="10" ControlToValidate="txtExtraChildRatio"></asp:rangevalidator></TD>
						<TD>
							<asp:rangevalidator id="RVExtraChildOffset" runat="server" CssClass="validators" Display="Dynamic" ErrorMessage="-999 a 999"
								Type="Double" MinimumValue="-999" MaximumValue="999" ControlToValidate="txtExtraChildOffset"></asp:rangevalidator></TD>
					</TR>
					<TR>
						<TD align="right"><asp:label id="lblOthersOcupation" runat="server" CssClass="clsLabel" EnableViewState="False">Others Occupation</asp:label></TD>
						<td><asp:textbox id="txtOthersRatio" runat="server" CssClass="textbox" Width="64px" MaxLength="2"></asp:textbox></td>
						<TD>
							<asp:textbox id="txtOthersOffset" Width="64px" runat="server" CssClass="textbox" MaxLength="4"></asp:textbox></TD>
						<TD></TD>
						<TD></TD>
						<TD></TD>
					</TR>
					<TR>
						<TD align="right"></TD>
						<TD>
							<asp:rangevalidator id="RVOthersRatio" runat="server" CssClass="validators" Display="Dynamic" ErrorMessage="0-10"
								Type="Double" MinimumValue="0" MaximumValue="10" ControlToValidate="txtOthersRatio"></asp:rangevalidator></TD>
						<TD>
							<asp:rangevalidator id="RVOthersOffset" runat="server" CssClass="validators" Display="Dynamic" ErrorMessage="-999 a 999"
								Type="Double" MinimumValue="-999" MaximumValue="999" ControlToValidate="txtOthersOffset"></asp:rangevalidator></TD>
						<TD></TD>
						<TD></TD>
						<TD></TD>
					</TR>
				</TABLE>
			</div>
		</TD>
	</TR>
</TABLE>
<script>
 function Ocultar(v,show,hide,div,txtR,txtO,Dv,DR,DO,LR,LO,txtDivV)
					{
				
				
						var e= document.getElementById(div);
						var s = document.getElementById(show);
						var o = document.getElementById(hide);
						var t1 = document.getElementById(txtR);	
						var t2 = document.getElementById(txtO);	
						var divr = document.getElementById(DR);	
						var divo = document.getElementById(DO);	
						var lblr = document.getElementById(LR);	
						var lblo = document.getElementById(LO);	
						var txtDiv = document.getElementById(txtDivV);							
						if (v == '1')
							{
							if ((t1.value=="" || isnumber(t1.value,0)) && (t2.value=="" || isnumber(t2.value,1)) && t1.value<=10 && t2.value<=999 && t2.value>=-999)
							 {
								e.style.display = 'block';
								o.style.display = 'block';     
								s.style.display = "none";  							
								divr.style.display = "none";  
								divo.style.display = "none";  
								lblr.style.display = "none";  
								lblo.style.display = "none";  
								txtDiv.value='0';	
							 }
							}
						else
							{
								if (checkFares(Dv))
								 {
								e.style.display = "none";
								s.style.display = 'block';     
								o.style.display = "none";
								divr.style.display = 'block';
								divo.style.display = 'block';
								lblr.style.display = 'block';
								lblo.style.display = 'block';  
								t1.value="";
								t2.value="";		
								txtDiv.value='1';						 
								}
							} 
					}
					
				function checkFares(Dv)
				  {
				  var div = document.getElementById(Dv);
					var e =div.getElementsByTagName("input");	
					for (var i = 0; i < e.length; i++)
					 {
						if (e[i].id.indexOf('Ratio') != -1)
						{
							if (!isnumber(e[i].value,0))
							 {
							  return false;
							 }
							 else if (e[i].value>10)
							 {
							  return false;
							 }
						}	
					   else if (e[i].id.indexOf('Offset') != -1)
					    {
					        if (!isnumber(e[i].value,1))
					         {
					          return false;
					         }
					         else if (e[i].value>999 || e[i].value<-999)
							 {
							  return false;
							 }
					         
					    }				
					 }					
					return true;			
							
				  }
					
				function FillPrices(Dv,typeFare,Price)
				{					
					var P = document.getElementById(Price);					
					var div = document.getElementById(Dv);
					var e =div.getElementsByTagName("input");				
					for (var i = 0; i < e.length; i++)
					 {						
						if (e[i].id.indexOf(typeFare) != -1)
						{
							e[i].value = P.value;
						}		
										
					 }			
					
				}
				
				
				function isnumber(valor,negativo)	 
				{	
					var ValidChars;
					var punto = false;
					if (negativo==1){ValidChars = "0123456789.-";}
					else{ValidChars = "0123456789.";}
					 
   					var IsNumber=true;
   					var Char;
  					for (i = 0; i < valor.length && IsNumber == true; i++) 
      				{ 
      					Char = valor.charAt(i); 
      					if (ValidChars.indexOf(Char) == -1) 
         					{
    	     					return false;
	         				}
	         			if (i!=0 && Char=='-')
	         				{
	         					return false;
	         				}	         					         			
	         			
	         			if (Char=='.')
	         				{
	         				 if (punto == true)
	         				   {
	         					return false;
	         				   }
	         				  punto = true;
	         				}
	         			
      					}
      				if (valor.length == 1 && (valor.charAt(0)=='.' || valor.charAt(0)=='-'))
      				 {
      				 		return false;
      				 }
      				 if(valor.charAt(valor.length -1)=='.')
      				  {
      				   return false;
      				  }
      				 
   					return IsNumber;
				}
</script>
