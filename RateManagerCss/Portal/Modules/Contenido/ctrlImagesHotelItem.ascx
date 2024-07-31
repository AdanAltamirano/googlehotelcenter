<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ctrlImagesHotelItem.ascx.vb" Inherits="RateManager.ctrlImagesHotelItem" %>
<TABLE id="Table3" cellSpacing="1" cellPadding="1" width="100%" border="0">
	<tr>
		<td colSpan="2"></td>
	</tr>
	<tr>
		<td colSpan="2"></td>
	</tr>
	<tr>
		<td width="100%" colSpan="2">
			<asp:label id="lblTitle" runat="server" EnableViewState="False" CssClass="clsdarklabel"></asp:label>
		</td>
	</tr>
	<tr width="100%">
		<td>
			<img class="label" id="ZoomImage" onerror="javascript:this.style.display='none'" src=""
				onload="javascript:this.style.display='block'" name="ZoomImage">
		</td>
		<td vAlign="middle" align="left"></td>
	</tr>
	<tr>
		<td colSpan="2">
			<table id="Table1" height="10" cellSpacing="1" cellPadding="1" width="100%" border="0">
				<tr class="">
					<td align="center" colSpan="2">
						<span class="bookingNormalLabel" id="lblfiles" runat="server"></span>
					</td>
				</tr>
				<tr>
					<td>
						<input id="attach1" type="file" name="attach1" runat="server"><br>
						<input id="attach2" type="file" name="attach2" runat="server"><br>
						<input id="attach3" type="file" name="attach3" runat="server"><br>
						<input id="attach4" type="file" name="attach4" runat="server"><br>
						<input id="attach5" type="file" name="attach5" runat="server"><br>
					</td>
					<td>
						<input id="attach6" type="file" name="attach6" runat="server"><br>
						<input id="attach7" type="file" name="attach7" runat="server"><br>
						<input id="attach8" type="file" name="attach8" runat="server"><br>
						<input id="attach9" type="file" name="attach9" runat="server"><br>
						<input id="attach10" type="file" name="attach10" runat="server"><br>
					</td>
				</tr>
				<tr>
					<td align="center" colSpan="2">
						<asp:button id="btnAttach" runat="server" CssClass="button" Text="Attach" Visible="false"></asp:button>
					</td>
				</tr>
				<tr>
					<td width="80%"></td>
					<td vAlign="middle"></td>
				</tr>
			</table>
		</td>
	</tr>
	<TR>
		<TD align="left" colSpan="2">
			<TABLE id="Table4" cellSpacing="4" cellPadding="4" width="100%" border="0">
				<TR>
					<TD vAlign="middle">
						<DIV class="clslabel" id="textDes"></DIV>
						<asp:label id="lblDescTips" Visible="False" runat="server" CssClass="clslabel"></asp:label>
					</TD>
				</TR>
			</TABLE>
		</TD>
	</TR>
	<TR>
		<TD align="left" colSpan="2">
			<asp:label id="lblMoreImages" Visible="False" runat="server" CssClass="clslabel">Agregar imagenes:</asp:label>
			<%--<asp:label id="lblMensajeImagen" runat="server">Maximo de Imagenes</asp:label>--%>
			<asp:label id="lblImagesTips" Visible="False" runat="server" CssClass="clslabel" Width="100%"></asp:label>
			<asp:table id="tblThumbnails" runat="server" CssClass="link"></asp:table>

		</TD>
	</TR>
	<asp:linkbutton CssClass="dgLink" runat=server></asp:linkbutton>
</TABLE>