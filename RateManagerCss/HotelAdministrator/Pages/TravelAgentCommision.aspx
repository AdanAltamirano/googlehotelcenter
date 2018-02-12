<%@ Page Language="vb" AutoEventWireup="false" Codebehind="TravelAgentCommision.aspx.vb" Inherits="RateManager.TravelAgentCommision"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>TravelAgentCommision</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK href="../../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<table id="bookingcontainer" cellSpacing="2" cellPadding="0" width="650" border="0">
				<tr>
					<td colspan="3" class="titulo"><h3 id="lblTitle" runat="server">Configuración de 
							comisión a las agencias de viajes</h3>
					</td>
				</tr>
				<tr>
					<td>
						<asp:RadioButton id="RbdAmount" runat="server" Text="Monto X Reserva" GroupName="Commisiontype"></asp:RadioButton></td>
					<td>
						<asp:TextBox id="txtMonto" runat="server" MaxLength="5" Columns="5"></asp:TextBox>
						<asp:Label id="lblmonedahotel" runat="server">USD</asp:Label>&nbsp;<asp:Label ID="lblperReservation" Runat="server">por reservacion</asp:Label></td>
					<td>
						<asp:RangeValidator id="RvMonto" runat="server" ErrorMessage="0-999" MaximumValue="999" MinimumValue="0"
							Type="Double" ControlToValidate="txtMonto"></asp:RangeValidator></td>
				</tr>
				<tr>
					<td>
						<asp:RadioButton id="RdbPercet" runat="server" Text="Percent of Reservation" GroupName="Commisiontype"></asp:RadioButton></td>
					<td>
						<asp:TextBox id="txtPorciento" runat="server" MaxLength="5" Columns="5"></asp:TextBox>
						<asp:Label id="lblPorciento" runat="server">% of total reservation</asp:Label></td>
					<td>
						<asp:RangeValidator id="RvPercent" runat="server" ErrorMessage="0-100" MaximumValue="100" MinimumValue="0"
							Type="Double" ControlToValidate="txtPorciento"></asp:RangeValidator></td>
				</tr>
				<TR>
					<TD align="center" colSpan="3">
						<asp:Button id="btnSave" runat="server" Text="Save" CssClass="button"></asp:Button></TD>
				</TR>
			</table>
		</form>
		<script>
		function SelectRadio(rdChecked,rdUnChecked,txt)
		 {
		 var t = document.getElementById(txt);
		  if (isnumber(t.value,0) && eval(t.value)>0)
		   {
		   var e = document.getElementById(rdChecked);
		   e.checked = true;
		   var e = document.getElementById(rdUnChecked);
		   e.checked = false;
		   }
		 }
		 
		 function isnumber(valor,negativo)	 
				{	
					var ValidChars;
					var punto = false;
					if (negativo==1){ValidChars = "0123456789.-";}
					else{ValidChars = "0123456789.";}
					 
   					var IsNumber=true;
   					var Char;
  					for (i = 0; i < valor.length && IsNumber == true; i++) 
      				{ 
      					Char = valor.charAt(i); 
      					if (ValidChars.indexOf(Char) == -1) 
         					{
    	     					return false;
	         				}
	         			if (i!=0 && Char=='-')
	         				{
	         					return false;
	         				}	         					         			
	         			
	         			if (Char=='.')
	         				{
	         				 if (punto == true)
	         				   {
	         					return false;
	         				   }
	         				  punto = true;
	         				}
	         			
      					}
      				if (valor.length == 1 && (valor.charAt(0)=='.' || valor.charAt(0)=='-'))
      				 {
      				 		return false;
      				 }
      				 if(valor.charAt(valor.length -1)=='.')
      				  {
      				   return false;
      				  }
      				 
   					return IsNumber;
				}

		</script>
	</body>
</HTML>
