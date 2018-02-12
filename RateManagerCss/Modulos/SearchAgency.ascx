<%@ Control Language="vb" AutoEventWireup="false" Codebehind="SearchAgency.ascx.vb" Inherits="RateManager.SearchAgency" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script language="JavaScript" type="text/javascript">		
		<!--
				function onCheckBoxClickOcultarMostrar(chk, filtro, btn, lst, lbl, ren)
				{
					var c = document.getElementById(chk);
					var f = document.getElementById(filtro);
					var b = document.getElementById(btn);
					var l = document.getElementById(lst);
					var lb = document.getElementById(lbl);
					var renglon = document.getElementById(ren);
					
					
					f.style.display = c.checked ? "" : "none";
					b.style.display = c.checked ? "" : "none";
					l.style.display = c.checked ? "" : "none";
					renglon.style.display = c.checked ? "" : "none";
					
					if (c.checked == false)
					{
						lb.style.display = "none";
					}				
				}
		//-->
</script>
<table>
	<TR>
		<TD align="left" colSpan="5"><asp:textbox id="txtFiltro" runat="server" CssClass="textBox" Width="176px"></asp:textbox>&nbsp;
			<asp:linkbutton id="uxSearchLink" runat="server" CssClass="Link" CausesValidation="False"> Buscar</asp:linkbutton></TD>
	</TR>
	<TR>
		<TD align="left" colSpan="5">
			<asp:DropDownList id="ddlBoxAgencies" runat="server" Width="232px"></asp:DropDownList></TD>
	</TR>
	<TR>
		<TD align="left" colSpan="5"><asp:label id="lblNotFound" runat="server" CssClass="clsvalidators">Label</asp:label></TD>
	</TR>
</table>