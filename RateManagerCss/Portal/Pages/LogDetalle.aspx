<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LogDetalle.aspx.vb" Inherits="RateManager.LogDetalle" %>

<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../Modules/ctrlFooter.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrMenu" Src="../Modules/CtrMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../Modules/ctrlHeader.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<link href="../../StyleSheets/Styles.css" type="text/css" rel="stylesheet">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
	<form id="Form1" method="post" runat="server">
	 <div class="clear">
            <div class="mDiv">
            </div>
            <div class="title">
                <asp:Label ID="lblTitle" runat="server" EnableViewState="False" Text="Detalle log" CssClass="tituloSeccion"></asp:Label>
            </div>
         </div>
        <table id="bookingcontainer" cellSpacing="0" cellPadding="2" width="790" border="0">
		<tr><td>
		    <TABLE cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">				
				<TR>
					<td colspan=2>					
					<asp:LinkButton ID="LinkButton1" runat="server">Go to Log list</asp:LinkButton>
					</td>
				</TR>				
				
				</tr>
				<tr><td colspan =2 align=left >
                    <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                    </td>
				
				</tr>
			</TABLE> 
		</td></tr>
		</table> 

    </form>
</body>
</html>
