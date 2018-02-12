<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ctrlReservationsNetRateQuery.ascx.vb" Inherits="RateManager.ctrlReservationsNetRate" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="uc1" TagName="Date" Src="../../ListDate/Date.ascx" %>
<script type="text/javascript" src='<%= (request.applicationpath & "/pages/Scripts/jquery.min.js").Replace("//","/") %>'></script>

<script type="text/javascript">
     $().ready(function() {
         // alert(document.getElementById('hplShow').style.display);
         var officeSelected = $('#<%= Me.hdnSearch.ClientId %>').val();
         if (officeSelected == 1) {
             $('#<%= Me.hplShow.ClientId %>').hide();
             $('#<%= Me.hplHide.ClientId %>').show();
             $('#<%= Me.pnlSearch.ClientId %>').show();
         }
     });  
</script>

<table style="text-align:right; width:100%"><tr><td> <asp:hyperlink id="hplShow" runat="server" CssClass="showOptions">Mostrar</asp:hyperlink>
<asp:hyperlink id="hplHide" runat="server" CssClass="hideOptions">Ocultar</asp:hyperlink>
<asp:HiddenField ID="hdnSearch" runat="server" Value="0" />
</td></tr>
<tr><td>   
<asp:Panel ID= "pnlSearch" runat = server  style= "display:none;">
<TABLE id="Table22" border="0" cellSpacing="1" cellPadding="1" width="100%">
	<tr>
		<td align="center">
			<div id="divSpecificSearch" runat="server">
				<TABLE id="Table1" border="0" cellSpacing="1" cellPadding="1" style ="padding-bottom:20px; width:100%; ">
					<TR>
						<TD class="dgitem" align="center"><asp:label id="lblSearchSpecific" runat="server" EnableViewState="False">Búsqueda Específica</asp:label></TD>
					</TR>
					<TR>
						<TD>
							<TABLE border="0" cellSpacing="0" cellPadding="0" width="100%">
								<TR>
									<TD width="14%" align="center"><asp:label id="lblNoconfirmacion" runat="server" EnableViewState="False" CssClass="clsLabel">No Confirmación:</asp:label></TD>
									<TD width="20%" align="left"><asp:textbox id="txtnoConfirmacion" runat="server" CssClass="textbox" Width="177px" MaxLength="24"></asp:textbox></TD>
									<TD width="40%" align="left"><asp:button id="btnBuscar" runat="server" EnableViewState="False" CssClass="button" Text="Buscar"></asp:button></TD>
								</TR>
							</TABLE>
						</TD>
					</TR>
				</TABLE>
			</div>
		</td>
	</tr>
	<tr>
		<TD class="dgitem" align="center"><asp:label id="lblSearchData" runat="server" EnableViewState="False">Búsqueda Filtrada</asp:label></TD>
	</tr>
	<tr>
		<td align="center">
			<table border="0" cellSpacing="0" cellPadding="0">
				<tr>
					<td vAlign="top">
						<table border="0" cellSpacing="0" cellPadding="0" width="100%">
							<TR>
								<td vAlign="top"><asp:checkbox id="chkDates" runat="server" Text="Incluir Fechas" Checked="True"></asp:checkbox></td>
								<td style="WIDTH: 10px"></td>
								<TD vAlign="top" align="left">
									<div id="divContainerddl">
										<table border="0" cellSpacing="1" cellPadding="1">
											<tr>
												<td><asp:label style="Z-INDEX: 0" id="lblSearchby" runat="server" EnableViewState="False" CssClass="clsLabel">Buscar por:</asp:label></td>
												<td><asp:dropdownlist style="Z-INDEX: 0" id="ddlFilterby" runat="server"></asp:dropdownlist></td>
											</tr>
											<tr>
												<td colSpan="2"><asp:label id="lblDesde" runat="server" EnableViewState="False" CssClass="clsLabel">Desde:</asp:label><uc1:date id="Date1" runat="server"></uc1:date></td>
											</tr>
											<tr>
												<td colSpan="2"><asp:label id="lblHasta" runat="server" EnableViewState="False" CssClass="clsLabel">Hasta:</asp:label><uc1:date id="Date2" runat="server"></uc1:date></td>
											</tr>
										</table>
									</div>
								</TD>
							</TR>
						</table>
					</td>
					<td style="WIDTH: 10px"></td>
					<td vAlign="top">
						<table border="0" cellSpacing="0" cellPadding="0" width="100%">
							<tr>
								<td><asp:label style="Z-INDEX: 0" id="lblCanal" runat="server">Canal</asp:label></td>
								<td><asp:dropdownlist style="Z-INDEX: 0" id="ddlCanal" runat="server"></asp:dropdownlist></td>
								<td style="WIDTH: 5px"></td>
								<td><asp:label style="Z-INDEX: 0" id="lblStatus" runat="server" EnableViewState="False" CssClass="clsLabel">Estatus:</asp:label></td>
								<td><asp:dropdownlist style="Z-INDEX: 0" id="lstStatus" runat="server" Width="93px"></asp:dropdownlist></td>
								<td style="WIDTH: 5px"></td>
								<td vAlign="top"></td>
								<TD vAlign="top"></TD>
							</tr>
						</table>
						<p></p>
						<asp:label style="Z-INDEX: 0" id="lblTotales" runat="server">Totales en:</asp:label><asp:radiobutton id="rbMXN" runat="server" Text="MXN" Checked="True" style="Z-INDEX: 0" GroupName="x"></asp:radiobutton><asp:radiobutton id="rbUSD" runat="server" Text="USD" style="Z-INDEX: 0" GroupName="x"></asp:radiobutton>
					</td>
				</tr>
			</table>
		</td>
	</tr>
	<TR>
		<TD align="right">
			<TABLE id="Table2" border="0" cellSpacing="0" cellPadding="0" width="200">
				<TR>
					<TD align="left">
						<asp:button id="BtnSpecificSearch" runat="server" EnableViewState="False" CssClass="button"
							Text="Send Consultation"></asp:button></TD>
					<TD align="right"></TD>
				</TR>
			</TABLE>
		</TD>
	</TR>
	
</TABLE>
</asp:Panel> 
</td></tr>
<tr style="padding-top:4px; padding-bottom:2px;"><td align=center >
	    <asp:Label ID="lblFiltro" runat="server" Text="" CssClass="clsHelpLabel" ></asp:Label>
	</td></tr>

</table>
<SCRIPT>
function ShowOrHide(e)
 {
   var chk = document.getElementById(e);
   //var dv = document.getElementById('divContainerlbl');
   var dv2 = document.getElementById('divContainerddl');
   
   if (chk.checked)
    {	
      //dv.style.display = '';
      dv2.style.display = 'block';
    }
   else
   {
      //dv.style.display = 'none';
      dv2.style.display = 'none';
   }



}

function FireShowOrHidden(ID, IDlnk, IHDlnk, hdn, show) {
    var e = document.getElementById(ID);
    var l = document.getElementById(IDlnk);
    var h = document.getElementById(IHDlnk);
    var hd = document.getElementById(hdn);
    if (e) {
        e.style.display = show ? 'block' : 'none';
    }
    if (l) {
        l.style.display = show ? 'block' : 'none';
    }
    if (h) {
        h.style.display = !show ? 'block' : 'none';
    }
    if (hd) {
        hd.value = show ? 1 : 0;
    }
    onResizeIframe();
}

</SCRIPT>
