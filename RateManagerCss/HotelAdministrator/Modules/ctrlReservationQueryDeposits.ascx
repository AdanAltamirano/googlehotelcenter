<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ctrlReservationQueryDeposits.ascx.vb" Inherits="RateManager.ctrlReservationQueryDeposits" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
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
<TABLE id="Table1" border="0" cellSpacing="1" cellPadding="1" width="100%">
	<TR>
		<TD class="dgitem" colSpan="4" align="center"><asp:label id="lblSearchSpecific" runat="server" EnableViewState="False">Búsqueda Específica</asp:label></TD>
	</TR>
	<TR>
		<TD colSpan="4">
			<TABLE id="Table3" border="0" cellSpacing="0" cellPadding="0" width="100%" style ="padding-bottom:20px;">
				<TR>
					<TD width="16%" align="right"><asp:label id="lblNoconfirmacion" runat="server" EnableViewState="False" CssClass="clsLabel">No Confirmación:</asp:label></TD>
					<TD width="20%" align="left"><asp:textbox id="txtnoConfirmacion" runat="server" CssClass="textbox" MaxLength="24" Width="177px"></asp:textbox></TD>
					<TD width="55%" align="left"><asp:button id="btnBuscar" runat="server" EnableViewState="False" CssClass="button" Text="Buscar"></asp:button><asp:customvalidator id="cvlNoConfirmacion" runat="server" CssClass="validators" ErrorMessage="Reservación no Entrontrada"
							Display="Dynamic" ControlToValidate="txtnoConfirmacion" ForeColor=" "></asp:customvalidator></TD>
				</TR>
			</TABLE>
		</TD>
	</TR>
	<TR>
		<TD class="dgitem" colSpan="4" align="center"><asp:label id="lblSearchData" runat="server" EnableViewState="False">Búsqueda Filtrada</asp:label></TD>
	</TR>
	<TR>
		<TD align="right"></TD>
		<TD align="left"><asp:checkbox id="chkDates" runat="server" Text="Incluir Fechas" Checked="True"></asp:checkbox></TD>
		<TD align=right >
			<DIV id="divContainerlbl"><asp:label id="lblSearchby" runat="server" EnableViewState="False" CssClass="clsLabel">Buscar por:</asp:label></DIV>
		</TD>
		<TD align="left">
			<DIV id="divContainerddl"><asp:dropdownlist id="ddlFilterby" runat="server"></asp:dropdownlist></DIV>
		</TD>
	</TR>
	<TR>
		<TD align="right"></TD>
		<TD align="left"><asp:checkbox style="Z-INDEX: 0" id="chkHotels" runat="server" Text="Hotel" Checked="True"></asp:checkbox></TD>
		<TD align="right"><asp:label style="Z-INDEX: 0" id="lblHoteles" runat="server" EnableViewState="False" CssClass="clsLabel">Hoteles:</asp:label></TD>
		<TD align="left"><asp:dropdownlist style="Z-INDEX: 0" id="ddlHoteles" runat="server"></asp:dropdownlist></TD>
	</TR>
	<TR>
		<TD align="right"><asp:label id="lblDesde" runat="server" EnableViewState="False" CssClass="clsLabel">Desde:</asp:label></TD>
		<TD align="left"><uc1:date id="Date1" runat="server"></uc1:date></TD>
		<TD align="right"><asp:label id="lblHasta" runat="server" EnableViewState="False" CssClass="clsLabel">Hasta:</asp:label></TD>
		<TD align="left"><uc1:date id="Date2" runat="server"></uc1:date></TD>
	</TR>
	<TR>
		<TD align="right"><asp:label id="lblNombreCliente" runat="server" EnableViewState="False" CssClass="clsLabel">Nombre del cliente:</asp:label></TD>
		<TD colSpan="3" align="left"><asp:textbox id="txtNameCustomer" runat="server" CssClass="textbox" MaxLength="80" Width="288px"></asp:textbox></TD>
	</TR>
	<TR>
		<TD align="right"></TD>
		<TD align="left"></TD>
		<TD align="left">
            <asp:Button ID="BtnSpecificSearch" runat="server" CssClass="button" 
                EnableViewState="False" Text="Send Consultation" />
        </TD>
		<TD align="center">
			<TABLE id="Table2" border="0" cellSpacing="0" cellPadding="0" width="300">
				<TR>
					<TD align="left">&nbsp;</TD>
				</TR>
			</TABLE>
		</TD>
	</TR>
	<tr><td colspan=4 align="center" >
       
        </td></tr>
</TABLE>

</asp:Panel>
</td></tr>
<tr><td align="center">
 <asp:Label ID="lblFiltro" runat="server" Text="" CssClass="clsHelpLabel" ></asp:Label>
</td></tr>

</table>

<script type="text/javascript">  
function ShowOrHideHotels(chkHotel,lblHotel,ddlHoteles)
 {
   var chk = document.getElementById(chkHotel);
   var lblh = document.getElementById(lblHotel);
   var ddlh = document.getElementById(ddlHoteles);   
   if (chk.checked)
    {	
      lblh.style.display = 'block';
      ddlh.style.display = 'block';
    }
   else
   {
      lblh.style.display = 'none';
      ddlh.style.display = 'none';
   }
}

function FireShowOrHidden(ID, IDlnk, IHDlnk, hdn, show) {
    var e = document.getElementById(ID);
    var l = document.getElementById(IDlnk);
    var h = document.getElementById(IHDlnk);
    var hd = document.getElementById(hdn);

    if (e) {
        e.style.display = show ? '' : 'none';
    }
    if (l) {
        l.style.display = show ? '' : 'none';
    }
    if (h) {
        h.style.display = !show ? '' : 'none';
    }
    if (hd) {
        hd.value = show ? 1 : 0;
    }
    onResizeIframe();
}
 
</script>
