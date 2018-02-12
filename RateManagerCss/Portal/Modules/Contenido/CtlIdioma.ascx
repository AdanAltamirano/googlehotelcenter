<%@ Control Language="vb" AutoEventWireup="false" Codebehind="CtlIdioma.ascx.vb" Inherits="RateManager.CtlIdioma" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<% 
    Response.Write("<script>  _editor_url =""" & GeRequestApplicationPath("/Editor/") & """;</script>")
%>
<script language="Javascript1.2"><!-- // load htmlarea
		//_editor_url = "/doradoweb/editor/";                     // URL to htmlarea files
		var win_ie_ver = parseFloat(navigator.appVersion.split("MSIE")[1]);
		if (navigator.userAgent.indexOf('Mac')        >= 0) { win_ie_ver = 0; }
		if (navigator.userAgent.indexOf('Windows CE') >= 0) { win_ie_ver = 0; }
		if (navigator.userAgent.indexOf('Opera')      >= 0) { win_ie_ver = 0; }
		if (win_ie_ver >= 5.5) {
			document.write('<scr' + 'ipt src="' +_editor_url+ 'editor.js"');
			document.write(' language="Javascript1.2"></scr' + 'ipt>');  
		} else { document.write('<scr'+'ipt>function editor_generate() { return false; }</scr'+'ipt>'); }
// --></script>
<script lang="text/javascript">
    function SelectIdioma(item,v)
    {
      if( document.getElementById(item) )
      {
          document.getElementById(item).value=v
      }      
    }
</script>
<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" width="100%" height="100%">
	<TR>
		<td valign="top">
			<TABLE id="Table111" cellpadding="0" cellspacing="1" width="100%">
				<tr>
					<TD vAlign="middle" align="center" width="50%">
						<DIV class="TabSelected" id="DivSelect" style="DISPLAY: inline; WIDTH: 100%; CURSOR: pointer"
							runat="server">English</DIV>
					</TD>
					<TD vAlign="middle" align="center" width="50%">
						<DIV class="Tab" id="DivUpload" style="DISPLAY: inline; WIDTH: 100%; CURSOR: pointer"
							runat="server">Español</DIV>
					</TD>
				</tr>
			</TABLE>
		</td>
	</TR>
	<TR>
		<TD valign="top" height="100%">
			<table id="tblspanish" runat="server" height="100%" width="100%" style="DISPLAY: none"
				border="0" cellpadding="0" cellspacing="0">
				<tr>
					<td vAlign="top" height="100%">
						<asp:textbox id="txtspanish" runat="server" Height="80px" Width="100%" CssClass="TextBox" TextMode="MultiLine"
							Rows="5"></asp:textbox>
					</td>
				</tr>
			</table>
			<table id="tblenglish" runat="server" height="100%" style="DISPLAY: block" width="100%"
				border="0" cellpadding="0" cellspacing="0">
				<tr>
					<td valign="top" height="100%">
						<asp:textbox id="txtenglish" runat="server" Height="100%" Width="100%" TextMode="MultiLine" CssClass="TextBox"
							Rows="5"></asp:textbox>
					</td>
				</tr>
			</table>
			<INPUT Id="IdiomaSelected" type="text" runat="server" value="0" style="DISPLAY: none" NAME="IdiomaSelected"
				maxLength="1" size="1"></TD>
	</TR>
</TABLE>
<%  if ishtml then
		response.write("<script> editor_generate('" + txtspanish.clientID + "');"+vbcrlf) 
		response.write("editor_generate('" + txtenglish.clientID + "'); </script>")
    end if
%>
