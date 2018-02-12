<%@ Control Language="vb" AutoEventWireup="false" Codebehind="CtrlPackageRubros.ascx.vb" Inherits="RateManager.CtrlPackageRubros" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<table id="bookingcontainer" width="100%" border="0" cellpadding="0" cellspacing="0">	
	<tr  class="trTitle">
		<td width="50%">
			<asp:CheckBox id="chkFlight" runat="server" Text="Avión" CssClass="bookingnormallabel"></asp:CheckBox></td>
		<td>
			<asp:CheckBox id="chkCar" runat="server" Text="Auto" CssClass="bookingnormallabel"></asp:CheckBox></td>
	</tr>
	<tr>
		<td><asp:DropDownList ID="ddlAerolinea" Runat="server"></asp:DropDownList></td>
		<td><asp:DropDownList ID="ddlFranquicia" Runat="server"></asp:DropDownList></td>
	</tr>
	<tr>
		<td></td>
		<td><span runat="server" id="lblSipp">Sipp Code:</span> &nbsp;<asp:TextBox ID="txtSipp" Runat="server" MaxLength="10" Columns="10"></asp:TextBox><br>
			<span runat="server" id="lblCarProm">Código Promoción</span> &nbsp;<asp:TextBox ID="txtCarProm" Runat="server" MaxLength="10" Columns="10"></asp:TextBox>
		</td>
	</tr>
	<tr  class="trTitle">
		<td colspan="2">
			<asp:CheckBox id="chkActivity" runat="server" Text="Actividad" CssClass="bookingnormallabel"></asp:CheckBox></td>
	</tr>
	<tr>
		<td colspan="2">
			<span runat="server" id="lblActProm">Código Promoción</span> &nbsp;<asp:TextBox ID="txtActProm" Runat="server" MaxLength="10" Columns="10"></asp:TextBox><br>
			<asp:CheckBoxList Runat="server" ID="lstActividades" RepeatColumns="3"></asp:CheckBoxList>
		</td>
	</tr>
	<tr>
		<td colspan="3" align=center>
			<input type="button" runat="server" class="Button" id="imgRubroClose" value="Cerrar Ventana" NAME="imgRubroClose">
		</td>
	</tr>
</table>
