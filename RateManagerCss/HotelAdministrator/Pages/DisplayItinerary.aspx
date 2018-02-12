<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DisplayItinerary.aspx.vb" Inherits="RateManager.DisplayItinerary" %>

<%@ Register src="../Modules/CtrlActivityItinerary.ascx" tagname="CtrlActivityItinerary" tagprefix="uc1" %>

<%@ Register src="../Modules/CtrlHotelItinerary.ascx" tagname="CtrlHotelItinerary" tagprefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
        <link href="../../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
        <script>
    function ShowDetails(divId, display) {
        var div = document.getElementById(divId);
        var pos = findPos(div); if (div) {div.style.display = display;}
        onResizeIframe();
        if (pos) { parent.scrollTo(0, pos); } else { parent.scrollTo(0, findPos(div)); }
    }
    function findPos(obj) {var curtop = 0;if (obj.offsetParent) {do {curtop += obj.offsetTop;} while (obj = obj.offsetParent);return [curtop]; } }
		    </script>
		    <style>
.litletitle,.litletitle td
{
    font-size:10px;
    font-weight:bold;
}
.littletext,.littletext td
{
    font-size:10px;
}
.MiddleText
{
       font-size:11px;
}
.MiddleTextBold
{
       font-size:11px; font-weight:bold;
}
</style>
</head>
<body>
    <form id="form1" runat="server">
     <table cellspacing="0" cellpadding="2" width="99%" border="0">
     <tr><td colspan="2"> <asp:Label ID="lblConfirm" runat="server" EnableViewState="False" Text="Confirm Reservation"
                CssClass="tituloSeccion"></asp:Label></td></tr>
     <tr>
        <td colspan=2 align=right>
        <div><asp:label id="lblItinerary" runat="server" EnableViewState="False" CssClass="labelBold">Itinerario :</asp:label><asp:label id="lblItiNoItinerary" runat="server" CssClass="bookingNormalLabel">LBA9829SDWW</asp:label></div>
        <div><span id="lbCancelationNumber" runat="server" class="labelBold">Número de Cancelación</span> <span id="lblCancelationNumber" runat="server" Class="bookingNormalLabel">Número de Cancelación</span></div>
        </td>
     </tr>
     <tr>
        <td colspan=2 align=right>
        <div><asp:label Visible="false" id="lblCancelDeadLine" runat="server" EnableViewState="False" CssClass="labelBold">Cancel With DeadLine</asp:label></div>
        <div><asp:label Visible="false" id="lblEPenaltyAmount" runat="server" CssClass="bookingNormalLabel">Amount: </asp:label>&nbsp;<span Visible="false" id="lblPenaltyAmount" runat="server" class="labelBold">Número de Cancelación</span></div>
        </td>
     </tr>
     <tr>
        <td colspan="2"><uc2:CtrlHotelItinerary ID="CtrlHotelItinerary1" runat="server" /></td>
     </tr>
     <tr>
        <td colspan="2"><uc1:CtrlActivityItinerary ID="CtrlActivityItinerary1" runat="server"/></td>
     </tr>
     <tr>
       
     </tr>
     </table>
    </form>
</body>
</html>