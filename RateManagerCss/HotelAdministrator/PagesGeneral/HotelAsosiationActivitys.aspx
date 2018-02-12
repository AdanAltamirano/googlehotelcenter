<%@ Page Language="vb" AutoEventWireup="false" Codebehind="HotelAsosiationActivitys.aspx.vb" Inherits="RateManager.HotelAsosiationActivitys"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>HotelAsosiationCars</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK href="../../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
        <div id="tituloSeccion">
            <img  class="bgplus"  src="../../Includes/imagenes/icono-big-plus.png" width="25" height="25" />
            <asp:Label ID="lblTitle" class="tituloSeccion" runat="server" EnableViewState="False">Hotel Configuration</asp:Label>
        </div>
        <table id="bookingcontainer" cellspacing="0" cellpadding="2" width="650" align="center"
            border="0">
            <tr>
                <td>
                    <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                        <tr>
                            <td align="center">
                                <asp:Label ID="lblCategoria" runat="server" EnableViewState="False" CssClass="clsLabel">Cadena :</asp:Label>
                                <asp:DropDownList ID="ddlCorporativos" runat="server" Style="z-index: 0">
                                </asp:DropDownList>
                                <asp:Button Style="z-index: 0" ID="cmdCargar" runat="server" CssClass="Button" Text="Cargar">
                                </asp:Button>
                            </td>
                        </tr>
                        <tr height="5">
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="dgitem" align="center" colspan="4">
                                <h3>
                                    <asp:Label ID="lbltitleCar" runat="server" EnableViewState="False" CssClass="clsdarklabel">Autorentas</asp:Label>
                                </h3>
                                <a style="cursor: pointer" id="LnkSelectAll" runat="server" class="dgLink" onclick="javascript:Franquicias(true);return false;">
                                    Seleccionar Todos</a> <a style="cursor: pointer" id="LnkUnSelectAll" runat="server"
                                        class="dgLink" onclick="javascript:Franquicias(false);return false;">Quitar Todos</a>
                            </td>
                        </tr>
                        <tr height="5">
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <div style="width: 590px; overflow: auto; top: 1px">
                                    <table id="TblFranquicias" cellpadding="0" cellspacing="0" style="z-index: 0">
                                        <tr>
                                            <td valign="top">
                                                <asp:CheckBoxList ID="chkFranquicias1" runat="server" CssClass="clsLabel">
                                                </asp:CheckBoxList>
                                            </td>
                                            <td valign="top">
                                                <asp:CheckBoxList ID="chkFranquicias2" runat="server" CssClass="clsLabel">
                                                </asp:CheckBoxList>
                                            </td>
                                            <td valign="top">
                                                <asp:CheckBoxList ID="chkFranquicias3" runat="server" CssClass="clsLabel">
                                                </asp:CheckBoxList>
                                            </td>
                                            <td valign="top">
                                                <asp:CheckBoxList ID="chkFranquicias4" runat="server" CssClass="clsLabel">
                                                </asp:CheckBoxList>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="4">
                                <asp:Button ID="CmdAceptar" runat="server" Text="Aceptar" CssClass="Button" Style="z-index: 0">
                                </asp:Button>&nbsp;
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
			<script>
			function Franquicias(valor)
	 {
		try
		{
		var tbl= document.getElementById("TblFranquicias");		
		if (tbl)
		 {
			var trs = tbl.getElementsByTagName("tr");
			
					for(var i=0;i<trs.length-1;i++)
					 {
						var chk = trs[i].getElementsByTagName("input");
						if(chk)
						{
						for(var x=0;i<chk.length-1;x++)
						{
							chk[x].checked=valor;
						}
						}
							  
					 }
		 }	 
		 }
		 catch(e)
		 {
		 }
	 }
			</script>
		</form>
	</body>
</HTML>
