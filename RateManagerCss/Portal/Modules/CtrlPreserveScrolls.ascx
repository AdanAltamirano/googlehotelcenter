<%@ Control Language="vb" AutoEventWireup="false" Codebehind="CtrlPreserveScrolls.ascx.vb" Inherits="RateManager.CtrlPreserveScrolls" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<asp:datagrid id="DG" AutoGenerateColumns="False" runat="server">
	<Columns>
		<asp:TemplateColumn HeaderText="StaticPostBackScrollVerticalPosition">
			<ItemTemplate>
				<asp:TextBox id=txtStaticPostBackScrollVerticalPosition runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"StaticPostBackScrollVerticalPosition") %>'>
				</asp:TextBox>
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="StaticPostBackScrollHorizontalPosition">
			<ItemTemplate>
				<asp:TextBox id=txtStaticPostBackScrollHorizontalPosition runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"StaticPostBackScrollHorizontalPosition") %>'>
				</asp:TextBox>
			</ItemTemplate>
		</asp:TemplateColumn>
	</Columns>
</asp:datagrid>
<script>
	var Commandseen=false;
	function SaveScrollPositions() 
	{   
	   if (Commandseen==true)
	   {
			var theControl;
			var theControlInVertical;
			var theControlInHorizontal;
			for (i=0; i<max; i++)
			{
				theControlInVertical = document.getElementById(NamesInVerticalPosition[i]);
				theControlInHorizontal = document.getElementById(NamesInHorizontalPosition[i]);			
			    if (ModesNamesControls[i]=="THEWINDOW")
			    {
					theControlInVertical.value = document.body.scrollTop; //(navigator.appName == 'Netscape') ? document.pageYOffset : document.body.scrollTop;
					theControlInHorizontal.value = document.body.scrollLeft; //(navigator.appName == 'Netscape') ? document.pageXOffset : document.body.scrollLeft;
			    }
			    else
			    {
   					theControl = document.getElementById(controlNames[i]);
					theControlInVertical.value = theControl.scrollLeft;
					theControlInHorizontal.value = theControl.scrollTop;
			    }
			}	    
			setTimeout('SaveScrollPositions()', 10);
	   }
    }
  
function RestoreScrollPosition() 
{   
		Commandseen=false;
		var theControl;
		var theControlInVertical;
		var theControlInHorizontal;
	    for (i=0; i<max; i++)
	    {
			theControlInVertical = document.getElementById(NamesInVerticalPosition[i]);
			theControlInHorizontal = document.getElementById(NamesInHorizontalPosition[i]);	    
			if (ModesNamesControls[i]=="THEWINDOW")
			{
				scrollTo(theControlInHorizontal.value, theControlInVertical.value); 
			}	    
			else
			{
				theControl = document.getElementById(controlNames[i]);	    
				theControl.scrollTop = theControlInHorizontal.value;
				theControl.scrollLeft = theControlInVertical.value;
			}
		}
		Commandseen=true;
		SaveScrollPositions();
}  
window.onload = RestoreScrollPosition; 
</script>
