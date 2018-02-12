<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ctrPortal.ascx.vb" Inherits="RateManager.ctrPortal" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script>
	function mostrarPortal(e, c){
		var chk = document.getElementById(e);
		var cbl = document.getElementById(c); 
		
	/*	var list = c.getElementById(c); */
		 					 
		if (chk && cbl){
			alert("1");
			alert(cbl);	
			alert(cbl.type);	
			alert(cbl.SelectedItem[1].checked);	
			cbl.SelectedItem[1].checked = true;
			
	/*			alert(cbl.length);
		for(var i=0;i<=list.length - 1;i++) 
			{
				/*cbl.items(i).checked = true ;			 '* /
				list[i].checked=true;
				alert("2");
			}
	*/		
	/*	alert(i);			*/
		}
	}
	
	function portalCheckAll(chk,grid,button) 
	{
		var e = document.getElementById(chk) 
		var dg= document.getElementById(grid);
		var list = dg.getElementsByTagName("input"); 
		var btn = document.getElementById(button); 
	
		//if(btn)btn.disabled = !e.checked 
	
		for (var i=0;i<=list.length-1;i++)
		{             
			if (list[i].type=='checkbox')
			{
				list[i].checked=e.checked 
			}
		}
		ShowHidePortals(1);
	}
	
	
	function checkAll(chk,grid) 
	{
		var e = document.getElementById(chk) 
		var dg= document.getElementById(grid);
		var list = dg.getElementsByTagName("input"); 
		
		for (var i=0;i<=list.length-1;i++)
		{             
			if (list[i].type=='checkbox')
			{
				list[i].checked=e.checked 
			}
		}
		ShowHidePortals(1);
	}
	function UncheckAll(chk,grid) 
	{
		var e = document.getElementById(chk) 
		var dg= document.getElementById(grid);
		var list = dg.getElementsByTagName("input"); 
		var AllChecked=1;
		for (var i=0;i<=list.length-1;i++)
		{             
			if (list[i].type=='checkbox')
			{
				if (!list[i].checked)
				 {
				 AllChecked=0;				 
				 }
			}
		}
		if (AllChecked==0)
		 {
			e.checked=false;
		 }
		 else
		 {
			e.checked=true;
		 }
		 
		 
	}

	/*
	function checkAll(chk,grid,button) 

{              var e = document.getElementById(chk) 

var dg= document.getElementById(grid);

var list = dg.getElementsByTagName("input"); 

var btn = document.getElementById(button); 

if(btn)btn.disabled = !e.checked 

for (var i=0;i<=list.length-1;i++)

 {             

if (list[i].type=='checkbox')

 {

 list[i].checked=e.checked 

}
*/
function ShowHidePortals(show)
 {
  var e=document.getElementById("divPortal");
  e.style.display='none';  
  if (show)e.style.display='';
  
 }

</script>
<TABLE cellSpacing="0" cellPadding="0" width="100%" border="0">
	<tr>
		<td  align="center"><asp:label id="lblPortal" visible="false" CssClass="bookingnormallabel" runat="server">Portales</asp:label></td>
	</tr>
	<TR>
		<TD><asp:checkbox id="chkPortalTodo" runat="server" Text="Todos"></asp:checkbox></TD>
	</TR>
	<TR>
		<TD align="left">
			<DIV id="divPortal" style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 120px" align="left" ms_positioning="FlowLayout"><asp:checkboxlist id="cblPortal" runat="server" RepeatColumns="3"></asp:checkboxlist></DIV>
		</TD>
	</TR>
</TABLE>
