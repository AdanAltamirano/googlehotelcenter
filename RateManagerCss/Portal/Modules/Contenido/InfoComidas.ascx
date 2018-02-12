<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="InfoComidas.ascx.vb"
    Inherits="RateManager.InfoComidas" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="uc1" TagName="CambiarContenido" Src="CambiarContenido.ascx" %>
<%@ Register TagPrefix="uc2" TagName="AgregarContenido" Src="AgregarContenido.ascx" %>
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
<asp:Panel ID="lPanel" runat="server">
    <table id="Table1" cellspacing="2" cellpadding="2" width="100%" border="0">
        <tr>
            <td valign="top" align="left" width="160">
                <uc1:cambiarcontenido id="ctrlTitle" runat="server" Visible="False">
                </uc1:cambiarcontenido>
                <asp:Label ID="lblTitle" runat="server" CssClass="clsdarklabel" EnableViewState="False">Dining</asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <table id="Table2" cellspacing="1" cellpadding="1" width="100%" border="0">
                    <tr>
                        <td>
                            <uc1:CambiarContenido id="lnkListadoRestaurants" runat="server" DESIGNTIMEDRAGDROP="63">
                            </uc1:CambiarContenido>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblListadoRestaurants" runat="server" CssClass="clslabel"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblMoreImages" Visible="False" runat="server" CssClass="clslabel">Agregar imagenes:</asp:Label>
                <uc2:AgregarContenido id="lnkAdd" runat="server" RConte="true" IsMultilanguage="True" ShowMode="ViewImage" IsAlbumEnable="True">
                </uc2:AgregarContenido>
                <asp:Label ID="lblMensajeImagen" runat="server"></asp:Label>
                <asp:Label ID="lblImagesTips" Visible="False" runat="server" CssClass="clslabel"
                    Width="100%"></asp:Label>
                <asp:Table ID="tblThumbnails" runat="server" CssClass="link">
                </asp:Table>
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="gPanel" runat="server">
    <table id="Table4" cellspacing="1" cellpadding="1" width="100%" border="0">
        <tr>
            <td style="height: 18px" colspan="2">
                <asp:Label ID="lblGTitle" runat="server" CssClass="clsdarklabel" EnableViewState="False">Meal Plans</asp:Label>
            </td>
        </tr>
        <tr>
            <td width="10">
            </td>
            <td>
                <asp:Label ID="lblGDescription" runat="server" CssClass="clslabel"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Panel>
<table id="Table3" height="10" cellspacing="0" cellpadding="0" width="100%" border="0">
    <tr>
        <td>
        </td>
    </tr>
</table>
