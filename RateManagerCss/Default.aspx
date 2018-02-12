<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Default.aspx.vb" Inherits="RateManager.Index"%>
<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="Portal/Modules/ctrlFooter.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrMenu" Src="Portal/Modules/CtrMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="Portal/Modules/ctrlHeader.ascx" %>
<%@ Import NameSpace="RateManager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>
			<%=PortalCulture.GetString("00157") %>
		</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
		<meta content="JavaScript" name="vs_defaultClientScript" />
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
        <%--<link href='<%= GeRequestApplicationPath("StyleSheets/Styles.css")%>' type="text/css" rel="stylesheet" />--%>
		<link href="StyleSheets/Styles.css" type="text/css" rel="stylesheet" />
		
		<script type="text/javascript" >
		function btnReload()
		{ document.getElementById("btnreload").click();  
		      
		}		
    	function UpdateMe()
          { //alert(1);                   
           // self.location.href =  "/default.aspx"            
          }
          
		</script>
	</HEAD>
	<body bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0" ms_positioning="flowlayout">
		<form id="Form1" method="post" runat="server">
			<asp:button id="btnreload" style="DISPLAY: none" Runat="server"></asp:button>
			<table height="100%" cellspacing="0" cellpadding="0" width="100%" border="0">
				<TR>
					<TD style='BACKGROUND-IMAGE: url(<%= GeRequestApplicationPath("/Images/bgshadowleft.JPG") %>); WIDTH: 54px; BACKGROUND-REPEAT: no-repeat'></TD>
					<TD style='BACKGROUND-IMAGE: url(<%= GeRequestApplicationPath(string.concat("/Images/ChainCodeGroup/MainHeader", Response.Cookies("groupid").value,".jpg")) %>); BACKGROUND-REPEAT: no-repeat'
						height="100%">
						<TABLE height="100%" cellSpacing="1" cellPadding="1" width="100%" align="left" border="0">
							<TR vAlign="top" height="120">
								<TD vAlign="bottom" height="130"><uc1:ctrlheader id="CtrlHeader1" runat="server"></uc1:ctrlheader></TD>
							</TR>
							<TR height="88%" width="100%">
								<td style="HEIGHT: 100%" vAlign="top" align="center" width="100%">
								<IFRAME id="frmPrincipal" style="BORDER-RIGHT: 0px solid; BORDER-TOP: 0px solid; BORDER-LEFT: 0px solid; WIDTH: 100%; BORDER-BOTTOM: 0px solid; HEIGHT: 100%; BACKGROUND-COLOR: transparent"
										name="frmPrincipal" src='Portal/Pages/Welcome.aspx' frameBorder="0" width="100%" height="100%" runat="server">
								</IFRAME>
								</td>
							</TR>
						</TABLE>
					</TD>
					<TD style='BACKGROUND-IMAGE: url(<%= GeRequestApplicationPath("/Images/bgshadowleft.JPG") %>); BACKGROUND-REPEAT: no-repeat'
						width="54"></TD>
				</TR>
			</table>
		</form>
	</body>
</html>
