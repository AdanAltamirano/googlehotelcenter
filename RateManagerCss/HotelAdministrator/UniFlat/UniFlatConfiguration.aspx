<%@ Register TagPrefix="uc1" TagName="CtrlIdioma" Src="../../Modulos/CtrlIdioma.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" validaterequest="false" Codebehind="UniFlatConfiguration.aspx.vb" Inherits="RateManager.UniFlatConfiguration"%>
<%@ Register TagPrefix="uc1" TagName="CtrlIdiomaRFCK" Src="../../Modulos/CtrlIdiomaRFCk.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>UniFlatConfiguration</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK href="../../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
		<script type="text/javascript">
			function openOnePageUI(code) {				    		    
	    	    var URLStr = "https://crs.univisit.com/OnePageCrs/OnePageUI.aspx?Code=";
				var ie = document.all != undefined;   
				if (ie){				
						var width  = window.screen.width-9;
						var height = window.screen.height-65;
						var popUpWin=0;
				}else{
						var width  = window.screen.width;
						var height = window.screen.height;
						var popUpWin=0;
				}
				    
				if(popUpWin){
					if(!popUpWin.closed) 
						popUpWin.close();
				}
				
				popUpWin = open(URLStr + code, 'popUpWin', 'toolbar=no,location=no,directories=no,status=no,menub ar=ye,scrollbar=no,resizable=no,copyhistory=yes,width='+width+',height='+height+',left=0, top=0');
			}
		</script>
	</HEAD>
	<body bottomMargin="0" leftMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
		 <div class="clear">
            <div class="mDiv">
            </div>
            <div class="title">
                <asp:Label ID="lbltitle" runat="server" EnableViewState="False" Text="Configuracion unipantalla" CssClass="tituloSeccion"></asp:Label>
            </div>
         </div>		
			<table id="bookingcontainer" cellSpacing="0" cellPadding="2" width="650" align="center"
				border="0">
				<tr>
					<td width="20%" align="right"><asp:Label id="lblUrlWebSite" runat="server">url Web Site</asp:Label></td>
					<td><uc1:CtrlIdioma id="txtUrlWebSite" runat="server"></uc1:CtrlIdioma></td>
				</tr>
				<tr>
					<td align="right"><asp:Label id="lblmailComments" runat="server">Emai Comments</asp:Label></td>
					<td><uc1:CtrlIdiomaRFCK id="txtEmailComments" runat="server"></uc1:CtrlIdiomaRFCK></td>					
				</tr>
				<tr>
					<td colspan="2"><asp:HyperLink id="hplUniFlatEn" runat="server">Uniflat</asp:HyperLink></td>
				</tr>
				<tr>
					<td colspan="2"><asp:HyperLink id="hplUniFlatEs" runat="server">Unipantalla</asp:HyperLink></td>
				</tr>
				<TR>
					<TD align="center" colspan="2" align="center">
						<asp:Button id="btnSave" runat="server" Text="Save" CssClass="button"></asp:Button></TD>
				</TR>
			</table>
		</form>
	</body>
</HTML>
