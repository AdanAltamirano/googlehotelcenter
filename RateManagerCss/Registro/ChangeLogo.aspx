<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ChangeLogo.aspx.vb" Inherits="RateManager.ChangeLogo" %>
<%@ Register TagPrefix="uc1" TagName="CtrMenu" Src="../Portal/Modules/CtrMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../Portal/Modules/ctrlHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../Portal/Modules/ctrlFooter.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>ChangeLogo</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body leftMargin="0" topMargin="5">
		<form id="Form1" method="post" runat="server">
        <div id="tituloSeccion">
            <img src="../Includes/imagenes/icono-big-plus.png" width="25" height="25" />            
            <asp:Label ID="lbltitle" class="tituloSeccion" runat="server" EnableViewState="False">Cambiar logo</asp:Label>
        </div>
        <table id="bookingcontainer" cellspacing="0" cellpadding="2" width="100%" border="0">
            <tr>
                <td>
                    <table cellpadding="0" width="100%" border="0">
                        <tr>
                            <td>
                                <table class="BordeTabla" id="Table3" cellspacing="0" cellpadding="0" width="500"
                                    align="" border="0">
                                    <tr>
                                        <td height="10" align="center">
                                            <asp:Image ID="img" CssClass="images" runat="server"></asp:Image>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TituloForma" height="10">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:Label ID="lbllogo" runat="server" CssClass="clsLabel" EnableViewState="False">Nuevo Logo :</asp:Label>
                                            <input class="textbox" id="ImgFileOpen" style="width: 100%; height: auto" type="file"
                                                name="ImgFileOpen" runat="server">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" height="5">
                                            <asp:Label ID="lblNoSave" runat="server" CssClass="Validators">No se Pudo Guardar el Archivo</asp:Label>
                                            <asp:Label ID="lblError" runat="server" CssClass="Validators" Visible="False">Imagen No Válida</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 32px" align="center">
                                            <asp:Button ID="cmdCambiar" runat="server" CssClass="Button" Text="Cambiar" EnableViewState="False">
                                            </asp:Button>
                                            <asp:Button ID="btnCancelar" runat="server" CssClass="Button" Text="Cancelar" EnableViewState="False">
                                            </asp:Button>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" height="5">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
		</form>
		<script language="javascript">
	function loadimage()
	{
	 var img=document.getElementById("imgNewLogo");	
	 var fil=document.getElementById("File1");	 
	 img.style.display=''	 
	 img.src="file://" + fil.value;	 
	}
	function saveImg(lbl)
   {
   	var imgFile = document.getElementById("ImgFileOpen");
   	imgFile.disabled=false;
	imgFile.click();	
	var RutaFile = document.getElementById(lbl);
	RutaFile.value=imgFile.value;
	imgFile.disabled=true;
   }
   
   function validaInputFile()
		{ 
		  var objs = document.getElementsByTagName("INPUT");		 	 
		  for(i=0; i<objs.length; i++)
			{
			 if (objs[i].type=='file')
			 	{objs[i].disabled=true;
			    }
			}		 
		}
		</script>
	</body>
</HTML>
