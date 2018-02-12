<%@ Register TagPrefix="uc1" TagName="AgregarContenido" Src="AgregarContenido.ascx" %>
<%@ Control Language="vb" AutoEventWireup="false" Codebehind="PhotoRooms.ascx.vb" Inherits="RateManager.PhotoRooms" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="uc1" TagName="CambiarContenido" Src="CambiarContenido.ascx" %>
<P>
	<script language="javascript">
<!--

//-->
    function LI(img,des){/* Function load image and text description */
 		var i=document.getElementById('ZoomImage');
		var d=document.getElementById('textDes');	
		if(null!=img && null!=i ) i.src=img;
		if(null!=des && null!=d ) d.innerHTML=des;				
  }
  
 
  
  
	</script>
</P>
<P>&nbsp;</P>
<TABLE id="Table3" cellSpacing="1" cellPadding="1" width="100%" border="0">
	<tr>
		<td colSpan="2"></td>
	</tr>
	<TR width="100%">
		<TD>
			<asp:label id="lblTypeRoom" runat="server" CssClass="clsLabel" EnableViewState="False"> Tipo habitación:</asp:label>
			<asp:dropdownlist id="lstRoomType" runat="server" Width="220px"></asp:dropdownlist>
			<asp:Button id="ButtonMostrar" CssClass="button" runat="server" Text="Mostrar"></asp:Button></TD>
		<TD vAlign="middle" align="left"></TD>
	</TR>
	<TR>
		<TD style="HEIGHT: 10px"></TD>
		<TD style="HEIGHT: 10px" vAlign="middle" align="left"></TD>
	</TR>
	<tr>
		<td colSpan="2">
			<TABLE id="Table1" height="10" cellSpacing="1" cellPadding="1" width="100%" border="0">
				<tr class="dgitem">
					<td align="center" colSpan="2"><span class="bookingNormalLabel" id="lblfiles" runat="server">Add 
							files</span></td>
				</tr>
				<tr>
					<td><input id="attach1" type="file" name="attach1" runat="server"><br>
						<input id="attach2" type="file" name="attach2" runat="server"><br>
						<input id="attach3" type="file" name="attach3" runat="server"><br>
						<input id="attach4" type="file" name="attach4" runat="server"><br>
						<input id="attach5" type="file" name="attach5" runat="server"><br>
					</td>
					<td><input id="attach6" type="file" name="attach6" runat="server"><br>
						<input id="attach7" type="file" name="attach7" runat="server"><br>
						<input id="attach8" type="file" name="attach8" runat="server"><br>
						<input id="attach9" type="file" name="attach9" runat="server"><br>
						<input id="attach10" type="file" name="attach10" runat="server"><br>
					</td>
				</tr>
				<tr>
					<td align="center" colSpan="2"><asp:button id="btnAttach" runat="server" CssClass="button" Text="Attach"></asp:button></td>
				</tr>
				<tr>
					<td Style="width:80%"></td>
					<td vAlign="middle"></td>
				</tr>
			</TABLE>
		</td>
	</tr>
	<TR>
		<TD align="center" colSpan="2">
			<asp:Label id="lblError" CssClass="Validators" runat="server" Visible="False">Debe Seleccionar una Habitacion</asp:Label></TD>
	</TR>
	<TR>
		<TD align="left" colSpan="2">
			<TABLE id="Table4" cellSpacing="4" cellPadding="4" width="100%" border="0">
				<TR>
					<TD vAlign="middle">
						<DIV class="clslabel" id="textDes"></DIV>
						<asp:label id="lblDescTips" Visible="False" runat="server" CssClass="clslabel"></asp:label></TD>
				</TR>
			</TABLE>
		</TD>
	</TR>
	<TR>
		<TD align="center" colSpan="2">
			<asp:datalist id="dlRoomImages" runat="server" RepeatColumns="6" RepeatDirection="vertical">
				<HeaderTemplate>
				</HeaderTemplate>
				<ItemTemplate>
					<asp:LinkButton id="Linkbutton2" style="display:" runat="server" CssClass="dgLink" CausesValidation="true"
						CommandName="Editar">
							<img alt= "" border="0" src='<%# GetRootPath() & Container.DataItem("NombreImagen") & "_T" & "?" & Now.ToString() %>'></a></asp:LinkButton>
					<br>
					<asp:LinkButton id="Linkbutton1" style="display:" runat="server" CssClass="dgLink" CausesValidation="true"
						CommandName="Delete">
						<%# GetName()%>
					</asp:LinkButton>
					<asp:textbox id="NombreImagen" runat="server" Width="208px" Visible = "False" Text='<%# Container.DataItem("NombreImagen") %>'>
					</asp:textbox>
					<asp:textbox id="idImagen" runat="server" Width="208px" Visible = "False" Text='<%# Container.DataItem("idImagen") %>'>
					</asp:textbox>
				</ItemTemplate>
			</asp:datalist></TD>
	</TR>
	<asp:linkbutton CssClass="dgLink"></asp:linkbutton></TABLE>
