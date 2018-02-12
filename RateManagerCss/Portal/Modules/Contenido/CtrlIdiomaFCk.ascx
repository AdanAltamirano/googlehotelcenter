<%@ Control Language="vb" AutoEventWireup="false" Codebehind="CtrlIdiomaFCk.ascx.vb" Inherits="RateManager.CtrlIdiomaFCk" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="fckeditorv2" Namespace="FredCK.FCKeditorV2" Assembly="FredCK.FCKeditorV2" %>
<script lang="text/javascript">

    function SizeDiv(w, h) {
        var ClientID = <%= divEditor.ClientID %>;
		var id = document.getElementById(ClientID).style.height 
		if (id) {
			id.style.width=w; 
			id.style.height=h;
		}
    }
    
    function GetInnerText(idCtrl,idHCtrl) {
		// This functions shows that you can interact directly with the editor area
		// DOM. In this way you have the freedom to do anything you want with it.

		// Get the editor instance that we want to interact with.
		var oEditor = FCKeditorAPI.GetInstance(idCtrl) ;

		// Get the Editor Area DOM (Document object).
		var oDOM = oEditor.EditorDocument ;

		var iText ;

		// The are two diffent ways to get the text (without HTML markups).
		// It is browser specific.
		if ( document.all )      // If Internet Explorer.
		{
			iText = oDOM.body.innerText ;
		}
		else               // If Gecko.
		{
			var r = oDOM.createRange() ;
			r.selectNodeContents( oDOM.body ) ;
			iText = r.toString() ;
		}		
			
		if (document.getElementById(idHCtrl))
			document.getElementById(idHCtrl).value=iText;				
	}		
		 		   		       
	function SelectIdioma(item,v) {
      if( document.getElementById(item) )
      {
          document.getElementById(item).value=v
      }      
    }
        
    function SelectTabFCk(from,to,sel,upl,trsel,trupl) {
		document.getElementById(from).className='TabSelected';
		document.getElementById(to).className='Tab';
		document.getElementById(upl).style.display='none';
		document.getElementById(sel).style.display='';
		
		document.getElementById(trupl).style.display='none';
		document.getElementById(trupl).style.height='0px';
		document.getElementById(trsel).style.display='';
		document.getElementById(trsel).style.height='100%';
		document.getElementById(sel).focus();
    }    
</script>
<div id="divEditor" style="WIDTH: 100%; overflow: hidden; min-height:280px;" runat="server">
	<TABLE id="Table1"  cellSpacing="0" cellPadding="0" width="100%" border="0" style="clear:both">
		<TR style="clear:both">
			<td vAlign="top">
				<TABLE id="Table111"  cellSpacing="1" cellPadding="0" width="100%" style="clear:both">
					<tr>
						<TD vAlign="middle" align="center" width="50%">
							<DIV class="TabSelected" id="DivSelect" style="DISPLAY: inline; WIDTH: 100%; CURSOR: pointer"
								runat="server"><asp:label id="lblEn" runat="server">Ingles</asp:label></DIV>
						</TD>
						<TD vAlign="middle" align="center" width="50%">
							<DIV class="Tab" id="DivUpload" style="DISPLAY: inline; WIDTH: 100%; CURSOR: pointer"
								runat="server"><asp:label id="lblEs" runat="server">Spanish</asp:label></DIV>
						</TD>
					</tr>
				</TABLE>
			</td>
		</TR>
		<TR id="trlenglish" runat="server">
			<TD vAlign="top" width="100%" >
				<table id="tblenglish" cellSpacing="0" cellPadding="0" width="100%" border="0" runat="server">
					<tr>
						<td vAlign="top" width="100%" ><asp:textbox id="txtenglish" style="DISPLAY: none" runat="server" Rows="5" TextMode="MultiLine"
								CssClass="TextBox" Width="100%" Height="80px"></asp:textbox><FCKEDITORV2:FCKEDITOR id="FCKeditorEN" runat="server" ToolbarSet="OZ"></FCKEDITORV2:FCKEDITOR></td>
					</tr>
				</table>
			</TD>
		</TR>
		<TR id="trlspanish" runat="server">
			<TD vAlign="top" width="100%" >
				<table id="tblspanish" style="DISPLAY: none" cellSpacing="0" cellPadding="0" width="100%"
					border="0" runat="server">
					<tr>
						<td vAlign="top" width="100%" >
						<asp:textbox id="txtspanish" style="DISPLAY: none" runat="server" Rows="5" TextMode="MultiLine" CssClass="TextBox" Width="100%" Height="80px"></asp:textbox>
						<fckeditorv2:fckeditor id="FCKeditorES" runat="server" ToolbarSet="OZ"></fckeditorv2:fckeditor>
						</td>
					</tr>
				</table>
			</TD>
		</TR>
		<TR>
			<TD vAlign="top" width="100%" ></TD>
		</TR>
	</TABLE>
	<INPUT id="TextditorEN" type="hidden" name="TextditorEN" runat="server"> <INPUT id="TextditorES" type="hidden" name="TextditorES" runat="server">
</div>
