<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ctrRoomLink.ascx.vb"
    Inherits="RateManager.ctrRoomLink" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<table id="Table1" cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
    <tr>
        <td align="center" colspan="7">
            <asp:TextBox ID="txtDivVisible" Style="display: none" CssClass="TextBox" Width="34px"
                runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr class="trTitle rounded-corners">
        <td class="dgitem" align="center" colspan="7">
            <asp:Label ID="lblTitle" CssClass="bookingNormalLabel" runat="server" EnableViewState="False">Add linked Rate Plan</asp:Label>
        </td>
    </tr>
    <tr>
        <td colspan="7" height="5">
        </td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label ID="lblERoomSource" runat="server" EnableViewState="False" CssClass="clsLabel"> Room Source</asp:Label>
        </td>
        <td align="center" colspan="3">
            <asp:DropDownList ID="ddlRoomSource" runat="server">
            </asp:DropDownList>
        </td>
        <td align="right">
            <asp:Label ID="lblERoomTarget" runat="server" EnableViewState="False" CssClass="clsLabel"> Room Target</asp:Label>
        </td>
        <td align="left" colspan="2">
            <asp:DropDownList ID="ddlRoomTarget" runat="server">
            </asp:DropDownList>
            <asp:TextBox ID="txtTarget" runat="server" CssClass="textbox" Enabled="False" Visible="False"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td colspan="7" height="5">
        </td>
    </tr>
    <tr>
        <td class="dgitem" align="center" colspan="7">
            <asp:Label ID="lbllinkdata" runat="server" EnableViewState="False">link data</asp:Label>
        </td>
    </tr>
    <tr>
        <td colspan="7">
            &nbsp;
        </td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label ID="lbl_Ratio" runat="server" CssClass="clsLabel" EnableViewState="False">Ratio:</asp:Label>
        </td>
        <td>
            <div id="divratio" runat="server">
                <asp:TextBox ID="txtRatio" Width="64px" runat="server" CssClass="textbox" MaxLength="5"></asp:TextBox><asp:RangeValidator
                    ID="RvRatio" runat="server" CssClass="validators" Display="Dynamic" ErrorMessage="0-10"
                    Type="Double" MinimumValue="0" MaximumValue="10" ControlToValidate="txtRatio"></asp:RangeValidator></div>
        </td>
        <td>
        </td>
        <td>
        </td>
        <td align="right">
            <asp:Label ID="lbl_Offset" runat="server" CssClass="clsLabel" EnableViewState="False">Offset:</asp:Label>
        </td>
        <td>
            <div id="divoffset" runat="server">
                <asp:TextBox ID="txtoffset" Width="64px" runat="server" CssClass="textbox" MaxLength="7"></asp:TextBox><asp:RangeValidator
                    ID="RVOffset" runat="server" CssClass="validators" Display="Dynamic" ErrorMessage="-999 to 999"
                    Type="Double" MinimumValue="-999" MaximumValue="999" ControlToValidate="txtoffset"></asp:RangeValidator></div>
        </td>
        <td>
        </td>
    </tr>
    <tr>
        <td colspan="2">
        </td>
        <td align="left" colspan="2">
        </td>
        <td align="right">
        </td>
        <td align="left">
        </td>
        <td>
        </td>
    </tr>
    <tr>
        <td align="right" colspan="7">
            <asp:HyperLink ID="hplhide" runat="server" CssClass="hideOptions">Hide Ratio/offset by occupancy</asp:HyperLink><asp:HyperLink
                ID="hplShow" runat="server" CssClass="showOptions">Show Ratio/offset by occupancy</asp:HyperLink>
        </td>
    </tr>
    <tr>
        <td align="center" colspan="7">
            <asp:Label ID="lblNoLink" runat="server" EnableViewState="False" CssClass="validators"
                Visible="False">No puede hacerse un link sobre sí mismo</asp:Label>
        </td>
    </tr>
    <tr>
        <td align="center" colspan="7">
            <div id="divDatos" runat="server">
                <table cellspacing="0" cellpadding="0" width="95%" align="center" border="0">
                    <tr>
                        <td width="25%">
                        </td>
                        <td width="10%">
                            <asp:Label ID="lblRatio" runat="server" EnableViewState="False" CssClass="clsLabel">Ratio</asp:Label>
                        </td>
                        <td width="15%">
                            <asp:Label ID="lblOffset" runat="server" EnableViewState="False" CssClass="clsLabel">Offset</asp:Label>
                        </td>
                        <td width="25%">
                        </td>
                        <td width="10%">
                            <asp:Label ID="lblRatio2" runat="server" EnableViewState="False" CssClass="clsLabel">Ratio</asp:Label>
                        </td>
                        <td width="15%">
                            <asp:Label ID="lblOffset2" runat="server" EnableViewState="False" CssClass="clsLabel">Offset</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblEOnePersonRate" runat="server" EnableViewState="False" CssClass="clsLabel">One Person Rate</asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtOnePersonRatio" Width="64px" runat="server" CssClass="textbox"
                                MaxLength="5"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtOnePersonOffset" Width="64px" runat="server" CssClass="textbox"
                                MaxLength="7"></asp:TextBox>
                        </td>
                        <td align="right">
                            <asp:Label ID="lblEExtraAdultRate" runat="server" EnableViewState="False" CssClass="clsLabel">Extra Adult Rate</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtExtraAdultRatio" Width="64px" runat="server" CssClass="textbox"
                                MaxLength="5"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtExtraAdultOffset" runat="server" Width="64px" CssClass="textbox"
                                MaxLength="7"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                        </td>
                        <td align="left">
                            <asp:RangeValidator ID="RvOnePersonRatio" runat="server" CssClass="validators" Display="Dynamic"
                                ErrorMessage="0-10" Type="Double" MinimumValue="0" MaximumValue="10" ControlToValidate="txtOnePersonRatio"></asp:RangeValidator>
                        </td>
                        <td>
                            <asp:RangeValidator ID="RVOnePersonOffset" runat="server" CssClass="validators" Display="Dynamic"
                                ErrorMessage="-999 a 999" Type="Double" MinimumValue="-999" MaximumValue="999"
                                ControlToValidate="txtOnePersonOffset"></asp:RangeValidator>
                        </td>
                        <td align="right">
                        </td>
                        <td>
                            <asp:RangeValidator ID="RvExtraAdultRatio" runat="server" CssClass="validators" ControlToValidate="txtExtraAdultRatio"
                                MaximumValue="10" MinimumValue="0" Type="Double" ErrorMessage="0-10" Display="Dynamic"></asp:RangeValidator>
                        </td>
                        <td>
                            <asp:RangeValidator ID="RVExtraAdultOffset" runat="server" CssClass="validators"
                                ControlToValidate="txtExtraAdultOffset" MaximumValue="999" MinimumValue="-999"
                                Type="Double" ErrorMessage="-999 a 999" Display="Dynamic"></asp:RangeValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblETwoPersonRAte" runat="server" EnableViewState="False" CssClass="clsLabel">Two Person Rate</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTwoPersonRatio" Width="64px" runat="server" CssClass="textbox"
                                MaxLength="5"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTwoPersonOffset" Width="64px" runat="server" CssClass="textbox"
                                MaxLength="7"></asp:TextBox>
                        </td>
                        <td align="right">
                            <asp:Label ID="lblEExtraChildRate" runat="server" EnableViewState="False" CssClass="clsLabel">Extra Child Rate</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtExtraChildRatio" Width="64px" runat="server" CssClass="textbox"
                                MaxLength="5"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtExtraChildOffset" Width="64px" runat="server" CssClass="textbox"
                                MaxLength="7"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                        </td>
                        <td>
                            <asp:RangeValidator ID="RvTwoPersonRatio" runat="server" CssClass="validators" Display="Dynamic"
                                ErrorMessage="0-10" Type="Double" MinimumValue="0" MaximumValue="10" ControlToValidate="txtTwoPersonRatio"></asp:RangeValidator>
                        </td>
                        <td>
                            <asp:RangeValidator ID="RVTwoPersonOffset" runat="server" CssClass="validators" Display="Dynamic"
                                ErrorMessage="-999 a 999" Type="Double" MinimumValue="-999" MaximumValue="999"
                                ControlToValidate="txtTwoPersonOffset"></asp:RangeValidator>
                        </td>
                        <td align="right">
                        </td>
                        <td>
                            <asp:RangeValidator ID="RvExtraChildRatio" runat="server" CssClass="validators" Display="Dynamic"
                                ErrorMessage="0-10" Type="Double" MinimumValue="0" MaximumValue="10" ControlToValidate="txtExtraChildRatio"></asp:RangeValidator>
                        </td>
                        <td>
                            <asp:RangeValidator ID="RVExtraChildOffset" runat="server" CssClass="validators"
                                Display="Dynamic" ErrorMessage="-999 a 999" Type="Double" MinimumValue="-999"
                                MaximumValue="999" ControlToValidate="txtExtraChildOffset"></asp:RangeValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblOthersOcupation" runat="server" EnableViewState="False" CssClass="clsLabel">Others Occupation</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtOthersRatio" Width="64px" runat="server" CssClass="textbox" MaxLength="5"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtOthersOffset" runat="server" Width="64px" CssClass="textbox"
                                MaxLength="7"></asp:TextBox>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                        </td>
                        <td align="center">
                            <asp:RangeValidator ID="RvOthersRatio" runat="server" CssClass="validators" Display="Dynamic"
                                ErrorMessage="0-10" Type="Double" MinimumValue="0" MaximumValue="10" ControlToValidate="txtOthersRatio"></asp:RangeValidator>
                        </td>
                        <td>
                            <asp:RangeValidator ID="RVOthersOffset" runat="server" CssClass="validators" ControlToValidate="txtOthersOffset"
                                MaximumValue="999" MinimumValue="-999" Type="Double" ErrorMessage="-999 a 999"
                                Display="Dynamic"></asp:RangeValidator>
                        </td>
                        <td align="center">
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </div>
        </td>
    </tr>
    
</table>


<script>
function GName(ddlRoom,lblRoom,array)
{
var e = document.getElementById(ddlRoom);
var lbl = document.getElementById(lblRoom);
if (lbl.firstChild)
{
if (array.split("//")[e.selectedIndex +1]=="")
{lbl.firstChild.nodeValue ="-";}
else
{lbl.firstChild.nodeValue =array.split("//")[e.selectedIndex +1];}
}
}


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
								e.style.display = '';     
								o.style.display = '';     
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
								s.style.display = '';     
								o.style.display = "none";  
								divr.style.display = '';  
								divo.style.display = '';  
								lblr.style.display = '';  
								lblo.style.display = '';  
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

