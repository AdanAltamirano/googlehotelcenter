<%@ Register TagPrefix="uc1" TagName="ctrlLanguageDictionary" Src="ctrlLanguageDictionary.ascx" %>
<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ctrlLanguageButton.ascx.vb" Inherits="RateManager.ctrlLanguageButton" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script language="javascript">
<!--

function showResource(idDiv,idBuff){
var status;
var div;
	div=document.getElementById(idDiv);
	div.style.display='block';

	status=document.getElementById(idBuff);
	status.value="1";
}
function hideResource(idDiv,idBuff){
var status;
var div;

	status=document.getElementById(idBuff);
	status.value="0";

	div=document.getElementById(idDiv);
	div.style.display='none';
}
//-->
</script>
<DIV id="divResourceDescription" style="POSITION: absolute" runat="server">
	<TABLE class="clsBackGround" id="Table3" cellSpacing="1" cellPadding="1" width="200" border="0">
		<TR>
			<TD>
				<TABLE id="Table4" cellSpacing="1" cellPadding="1" width="100%" border="0">
					<TR>
						<TD class="clsHeadTitle" width="100%">
<asp:label id=lblResources runat="server">Recursos</asp:label></TD>
						<TD class="clsHeadTitle"><INPUT class="Button" id="btnHide" type="button" value="X" runat="server">
						</TD>
					</TR>
				</TABLE>
				<uc1:ctrllanguagedictionary id="CtrlLanguageDictionary1" runat="server"></uc1:ctrllanguagedictionary></TD>
		</TR>
	</TABLE>
</DIV>
<INPUT class="button" id="btnShow" type="button" value="Soporte multilenguaje" runat="server"
	style="WIDTH: 144px; HEIGHT: 24px">
<asp:textbox id="txtStatus" style="DISPLAY: none" runat="server" Width="1px">0</asp:textbox>
