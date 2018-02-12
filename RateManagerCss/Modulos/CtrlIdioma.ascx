<%@ Control Language="vb" AutoEventWireup="false" Codebehind="CtrlIdioma.ascx.vb" Inherits="RateManager.CtrlIdioma" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
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
    
    function EnableValidators_<%= Me.Id %>(enabled){ 
       
        var spanish = document.getElementById('<%= Me.txtspanish.ClientId %>');
        var english = document.getElementById('<%= Me.txtenglish.ClientId %>');        
        var spanishVal = document.getElementById('<%= Me.rfvDefaultText.ClientId %>');
        var englishVal = document.getElementById('<%= Me.rfvDefaultText2.ClientId %>');
        
        if (spanishVal != null) {
       
            ValidatorEnable(spanishVal, enabled);
            if(spanish != null && spanish.value.length == 0){
                spanishVal.style.display = 'none';    
            }
        }
        
        if (englishVal != null) {
            ValidatorEnable(englishVal, enabled);
            if(english != null && english.value.length == 0){
                englishVal.style.display = 'none';    
            }
        }
    }
</script>


<TABLE id="tblGen" cellSpacing="0" cellPadding="0" border="0" width="100%" runat="server">
<tbody style="width:100px">
    <tr>
    <td>
        <table  cellSpacing="0" cellPadding="0" border="0" style="width:100%"><tr>
        <td style="width:50%"><asp:RequiredFieldValidator Enabled="true" id="rfvDefaultText" runat="server" Display="Dynamic" CssClass="validators">El texto en ingles es requerido</asp:RequiredFieldValidator></td>
        <td style="width:50%"><asp:RequiredFieldValidator Enabled="true" id="rfvDefaultText2" runat="server" Display="Dynamic" CssClass="validators">El texto en ingles es requerido</asp:RequiredFieldValidator></td></tr></table>
    
    </td>
    </tr>
	<TR>
		<td vAlign="bottom">
			<TABLE id="Table111" cellpadding="0" cellspacing="1" width="100%">
				<tr>
					<TD vAlign="middle" align="center" width="50%">
						<DIV class="TabSelected" id="DivSelect" style="DISPLAY: inline; WIDTH: 100%; CURSOR: pointer"
							runat="server"><%=Ratemanager.PortalCulture.GetString("M0BT0000081") %> </DIV>
					</TD>
					<TD vAlign="middle" align="center" width="50%">
						<DIV class="Tab" id="DivUpload" style="DISPLAY: inline; WIDTH: 100%; CURSOR: pointer"
							runat="server"><%=Ratemanager.PortalCulture.GetString("M0BT0000080") %></DIV>
					</TD>
				</tr>
			</TABLE>
		</td>
	</TR>
	<TR>
		<TD>
			<table id="tblspanish" runat="server" width="100%" style="DISPLAY: none" border="0" cellpadding="0"
				cellspacing="0">
				<tbody style="width:100px">
				<tr>
					<td>
						<asp:textbox id="txtspanish" runat="server" Width="100%" CssClass="TextBox" TextMode="MultiLine"></asp:textbox>
					</td>
				</tr>
				</tbody>
			</table>
			<table id="tblenglish" runat="server" style="DISPLAY: block" width="100%" border="0" cellpadding="0"
				cellspacing="0">
				<tr>
					<td>
						<asp:textbox id="txtenglish" runat="server" Width="100%" TextMode="MultiLine" CssClass="TextBox"></asp:textbox>
					</td>
				</tr>
			</table>
			<INPUT Id="IdiomaSelected" type="text" runat="server" value="0" style="DISPLAY: none" NAME="IdiomaSelected"
				maxLength="1" size="1">
			<asp:TextBox id="txtAux" runat="server" Width="32px" Visible="False">aux</asp:TextBox></TD>
	</TR>
	</tbody>
</TABLE>
<%  if ishtml then
		response.write("<script> editor_generate('" + txtspanish.clientID + "');"+vbcrlf) 
		response.write("editor_generate('" + txtenglish.clientID + "'); </script>")
    end if
%>
