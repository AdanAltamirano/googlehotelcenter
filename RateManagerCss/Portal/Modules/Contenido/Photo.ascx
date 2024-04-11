<%@ Register TagPrefix="uc1" TagName="AgregarContenido" Src="AgregarContenido.ascx" %>
<%@ Control Language="vb" AutoEventWireup="false" Codebehind="Photo.ascx.vb" Inherits="RateManager.Photo" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
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
	<tr>
		<td colSpan="2"></td>
	</tr>
	<TR>
		<TD width="100%" colSpan="2">
<%--			<uc1:cambiarcontenido id="lnkChangeTitle" Visible="False" runat="server"></uc1:cambiarcontenido>--%>
			<asp:label id="lblTitle" runat="server" EnableViewState="False" CssClass="clsdarklabel">Hotel photos</asp:label>
		</TD>
	</TR>
	<TR width="100%">
		<TD><IMG class="label" id="ZoomImage" onerror="javascript:this.style.display='none'" src=""
				onload="javascript:this.style.display='block'" name="ZoomImage"></TD>
		<TD vAlign="middle" align="left"></TD>
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
					<td width="80%"></td>
					<td vAlign="middle"></td>
				</tr>
			</TABLE>
		</td>
	</tr>
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
		<TD align="left" colSpan="2">
			<asp:label id="lblMoreImages" Visible="False" runat="server" CssClass="clslabel">Agregar imagenes:</asp:label>
			<%--<uc1:agregarcontenido id="lnkAdd" runat="server"></uc1:agregarcontenido>--%>
			<asp:label id="lblMensajeImagen" runat="server">Maximo de Imagenes</asp:label>
			<asp:label id="lblImagesTips" Visible="False" runat="server" CssClass="clslabel" Width="100%"></asp:label>
			<asp:table id="tblThumbnails" runat="server" CssClass="link"></asp:table>

		</TD>
	</TR>
	<asp:linkbutton CssClass="dgLink" runat=server></asp:linkbutton></TABLE>
