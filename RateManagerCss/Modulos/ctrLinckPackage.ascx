<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ctrLinckPackage.ascx.vb" Inherits="RateManager.ctrLinckPackage" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script>

var Height=600;
var Width=900;
function ShowReservation(lnk)
	{
	
		window.abrir_ventanaIidoma(lnk);
	}

     function abrir_ventanaIidoma(url) {
              var window_width = Width;
              var window_height = Height;
              var newfeatures= 'scrollbars=1,resizable=1';
              var window_top = (screen.height-window_height)/2;
              var window_left =(screen.width-window_width)/2;
              newWindow=window.open(url, 'titulo','width=' + window_width + ',height=' + window_height + ',top=' + window_top + ',left=' + window_left + ',scrollbars=1,resizable=1');
}

</script>
<div>
	<p id="MenssajePackages" runat="server">Esta reservacion pertenece a un paquete
	</p>
	<a Target="_blank" id="lnkCarDetail" Class="dglink" runat="server"  >Detalle 
		reservacion Auto</a><br>
	<br>
	<asp:hyperlink id="lnkActDetail" CssClass="dglink" runat="server">Detalle reservacion Actividad</asp:hyperlink></div>
<br>
<asp:hyperlink id="lnkFlDetail" CssClass="dglink" runat="server">Detalle reservacion Vuelo</asp:hyperlink>
<DIV></DIV>
