<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ctrlHotelPlans.ascx.vb" Inherits="RateManager.ctrlHotelPlans" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="uc1" TagName="CtrlIdioma" Src="CtrlIdioma.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlLanguageButton" Src="ctrlLanguageButton.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlLanguageDictionary" Src="ctrlLanguageDictionary.ascx" %>
<TABLE id="Table2" cellSpacing="1" cellPadding="1" width="100%" border="0">
	<TR>
		<TD align="center" colSpan="2">
			<asp:label id="lblEditTitle" runat="server" CssClass="clsHelpLabel"></asp:label></TD>
	</TR>
	<TR>
		<TD align="left" colSpan="2">
			<asp:label id="lblPlanType" runat="server" CssClass="clsDarkLabel" EnableViewState="False">Tipo de plan:</asp:label></TD>
	</TR>
	<TR>
		<TD align="left" colSpan="2">
			<asp:dropdownlist id="lstPlanType" CssClass="TextBox" runat="server" Width="208px"></asp:dropdownlist></TD>
	</TR>
	<TR>
		<TD align="left" colSpan="2">
			<asp:label id="lblPlanName" runat="server" CssClass="clsDarkLabel" EnableViewState="False">Descripción de plan:</asp:label></TD>
	</TR>
	<TR>
		<TD align="left" colSpan="2">
			<uc1:CtrlIdioma id="mlDescriptionPlan" runat="server"></uc1:CtrlIdioma></TD>
	</TR>
</TABLE>
<script>
	function ShowMsgB()
		{	
		document.getElementById("divResource").style.visibility='visible';
		var e=document.getElementById("spanResource");
            e.style.visibility='visible';	       	            
            w=300;
            h=70;
            
            l=Math.round((screen.availWidth-w)/2);
	        t=Math.round((screen.availHeight-h)/2);
	        e.style.top=t-100;
	        e.style.left=l;		          
		}
		
		function HideMsgB()
		{	document.getElementById("spanResource").style.visibility='hidden';
		    document.getElementById("divResource").style.visibility='hidden';
		}		
</script>
