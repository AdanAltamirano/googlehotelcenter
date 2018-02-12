<%@ Register TagPrefix="uc1" TagName="CambiarContenido" Src="CambiarContenido.ascx" %>
<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ctrlImages.ascx.vb"
    Inherits="RateManager.ctrlImages1" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="uc1" TagName="AgregarContenido" Src="AgregarContenido.ascx" %>

<script language="javascript">
<!--
    //-->
    function LI(img, des) {/* Function load image and text description */
        var i = document.getElementById('ZoomImage');
        var d = document.getElementById('textDes');
        if (null != img && null != i) i.src = img;
        if (null != des && null != d) d.innerHTML = des;
    }
</script>

<table id="Table3" cellspacing="1" cellpadding="1" width="100%" border="0">
    <tr>
        <td colspan="2">
            <br>
            <uc1:CambiarContenido ID="ctrlTitle" runat="server" Visible="False"></uc1:CambiarContenido>
            <asp:Label ID="lblTitle" runat="server" EnableViewState="False" CssClass="clsdarklabel">Imagenes de Header</asp:Label>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <p>
                <img class="label" id="ZoomImage" onerror="javascript:this.style.display='none'"
                    src="" onload="javascript:this.style.display='block'" name="ZoomImage" style=" width: 400px; "><br />
            </p>
            <uc1:CambiarContenido ID="lnkPropiedad" runat="server" Visible="False"></uc1:CambiarContenido>
        </td>
    </tr>
    <tr>
        <td valign="top" align="left" width="160">
            <div class="clslabel" id="textDes">
            </div>
        </td>
        <td valign="top">
        </td>
    </tr>
    <tr>
        <td valign="top" align="left" width="160">
            <asp:Label ID="lblMoreImages" Visible="False" runat="server" CssClass="">Agregar imagenes:</asp:Label>
            <uc1:AgregarContenido ID="lnkAdd" runat="server"></uc1:AgregarContenido>
            <asp:Label ID="lblMensajeImagen" runat="server">Maximo de Imagenes</asp:Label><asp:Label
                ID="lblImagesTips" Visible="False" runat="server" CssClass="clslabel" Width="100%"></asp:Label>
            <asp:Table ID="tblThumbnails" runat="server" CssClass="link">
            </asp:Table>
        </td>
    </tr>
</table>
<table id="Table1" height="10" cellspacing="0" cellpadding="0" width="100%" border="0">
    <tr>
        <td>
        </td>
    </tr>
</table>
