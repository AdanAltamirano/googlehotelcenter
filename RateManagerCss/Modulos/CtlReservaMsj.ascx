<%@ Control Language="vb" AutoEventWireup="false" Codebehind="CtlReservaMsj.ascx.vb" Inherits="RateManager.CtlReservaMsj" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Import NameSpace = "RateManager" %>
<script>
	function SetTotal(iTax,iRooms,clntId,iTotal,InvalidRt,idtxt)
	 {
	 //si no es numerico el valor poner 0
	  var e=document.getElementById(iTax);
	  var Rooms=document.getElementById(iRooms);
	  var total=0;	  
	  var sw=0;	 
	  for (var i=1;i<=eval(Rooms.value);i++)
	   {	   
		var txt = document.getElementById(clntId + "_txtPriceRoom" + i);
		if (!isnumber(txt.value,0)){txt.value=0;if (sw==0 && clntId + "_txtPriceRoom" + i == idtxt){alert(InvalidRt);sw=1;}}					
		if (eval(txt.value)>=1){ total += eval(txt.value);}
	   }
	   
	  if (eval(e.value)>=1)
	   {		
	     total += eval(total)*eval(e.value)/100;
	   }
	   
	  var t= document.getElementById(iTotal);
	  //redondear
	  t.innerHTML=total;
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
	         			if (i==0 && Char=='0')
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
<asp:panel id="TblPnl" style="DISPLAY: none; Z-INDEX: 997; FILTER: alpha(opacity=30); LEFT: 0px; POSITION: absolute; TOP: 0px; BACKGROUND-COLOR: whitesmoke; moz-opacity: .25; opacity: .25"
	Height="99%" Width="99%" runat="server"></asp:panel>
	
	<asp:panel id="PnlBox" style="DISPLAY: none; Z-INDEX: 998; LEFT: 0px; POSITION: absolute; TOP: 0px;"
	Height="" Width="100%" runat="server"><INPUT id="txtTax" type="hidden" name="txtTax" runat="server">
	<INPUT id="iTotRooms" type="hidden" name="iTotRooms" runat="server">
	<div class="boxMsg">
    	<TABLE height="" cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">
		<TR>
			<TD vAlign="middle">
				<TABLE id="bookingcontainer" height="100" cellSpacing="1" cellPadding="0" width="450" align="center"
					border="0">
					<TR>
						<TD class="titulo" align="center">
							<asp:label id="lblTitle" runat="server"></asp:label></TD>
					</TR>
					<TR>
						<TD vAlign="middle" align="right" height="50">
							<asp:Label id="lblImpuesto" runat="server">Impuesto: Incluido</asp:Label>
							<TABLE id="TblMai" cellSpacing="0" cellPadding="5" width="300" align="center" border="0">
								<TR>
									<TD vAlign="middle" align="center" width="5">
										<asp:image id="img" runat="server" Visible="False"></asp:image></TD>
									<TD align="center">
										<asp:label id="lblPrompt" runat="server" CssClass="clsDarkLabel"></asp:label></TD>
								</TR>
							</TABLE>
					<TR>
						<TD colSpan="5">
							<TABLE cellSpacing="2" cellPadding="2" width="90%" border="0">
								<TR>
									<td width=10%><%response.write(portalculture.getstring("00170",true))%></td>
									<TD id="CtrldivRoom1" style="CURSOR: pointer" onclick="SelectRoom(1,'CtrldivRoom','CtrlDataRoom');">
										<DIV style="DISPLAY: inline; WIDTH: 100%"> 1</DIV>
									</TD>
									<TD id="CtrldivRoom2" style="CURSOR: pointer" onclick="SelectRoom(2,'CtrldivRoom','CtrlDataRoom');">
										<DIV style="DISPLAY: inline; WIDTH: 100%"> 2</DIV>
									</TD>
									<TD id="CtrldivRoom3" style="CURSOR: pointer" onclick="SelectRoom(3,'CtrldivRoom','CtrlDataRoom');">
										<DIV style="DISPLAY: inline; WIDTH: 100%"> 3</DIV>
									</TD>
									<TD id="CtrldivRoom4" style="CURSOR: pointer" onclick="SelectRoom(4,'CtrldivRoom','CtrlDataRoom');">
										<DIV style="DISPLAY: inline; WIDTH: 100%"> 4</DIV>
									</TD>
									<TD id="CtrldivRoom5" style="CURSOR: pointer" onclick="SelectRoom(5,'CtrldivRoom','CtrlDataRoom');">
										<DIV style="DISPLAY: inline; WIDTH: 100%"> 5</DIV>
									</TD>
									<TD id="CtrldivRoom6" style="CURSOR: pointer" onclick="SelectRoom(6,'CtrldivRoom','CtrlDataRoom');">
										<DIV style="DISPLAY: inline; WIDTH: 100%"> 6</DIV>
									</TD>
									<TD id="CtrldivRoom7" style="CURSOR: pointer" onclick="SelectRoom(7,'CtrldivRoom','CtrlDataRoom');">
										<DIV style="DISPLAY: inline; WIDTH: 100%"> 7</DIV>
									</TD>
									<TD id="CtrldivRoom8" style="CURSOR: pointer" onclick="SelectRoom(8,'CtrldivRoom','CtrlDataRoom');">
										<DIV style="DISPLAY: inline; WIDTH: 100%"> 8</DIV>
									</TD>
									<TD id="CtrldivRoom9" style="CURSOR: pointer" onclick="SelectRoom(9,'CtrldivRoom','CtrlDataRoom');">
										<DIV style="DISPLAY: inline; WIDTH: 100%"> 9</DIV>
									</TD>
									<td></td>
								</TR>
								<tr>
									<td width=10%></td>
									<td colspan=9>
									<DIV id="CtrlDataRoom1">Total:
								<asp:TextBox id="txtPriceRoom1" runat="server" Columns="5" MaxLength="7"></asp:TextBox><SPAN id="lblMonedaRoom1" runat="server"></SPAN></DIV>
							<DIV id="CtrlDataRoom2">Total: 
								<asp:TextBox id="txtPriceRoom2" runat="server" Columns="5" MaxLength="7"></asp:TextBox><SPAN id="lblMonedaRoom2" runat="server"></SPAN></DIV>
							<DIV id="CtrlDataRoom3">Total: 
								<asp:TextBox id="txtPriceRoom3" runat="server" Columns="5" MaxLength="7"></asp:TextBox><SPAN id="lblMonedaRoom3" runat="server"></SPAN></DIV>
							<DIV id="CtrlDataRoom4">Total: 
								<asp:TextBox id="txtPriceRoom4" runat="server" Columns="5" MaxLength="7"></asp:TextBox><SPAN id="lblMonedaRoom4" runat="server"></SPAN></DIV>
							<DIV id="CtrlDataRoom5">Total: 
								<asp:TextBox id="txtPriceRoom5" runat="server" Columns="5" MaxLength="7"></asp:TextBox><SPAN id="lblMonedaRoom5" runat="server"></SPAN></DIV>
							<DIV id="CtrlDataRoom6">Total: 
								<asp:TextBox id="txtPriceRoom6" runat="server" Columns="5" MaxLength="7"></asp:TextBox><SPAN id="lblMonedaRoom6" runat="server"></SPAN></DIV>
							<DIV id="CtrlDataRoom7">Total: 
								<asp:TextBox id="txtPriceRoom7" runat="server" Columns="5" MaxLength="7"></asp:TextBox><SPAN id="lblMonedaRoom7" runat="server"></SPAN></DIV>
							<DIV id="CtrlDataRoom8">Total: 
								<asp:TextBox id="txtPriceRoom8" runat="server" Columns="5" MaxLength="7"></asp:TextBox><SPAN id="lblMonedaRoom8" runat="server"></SPAN></DIV>
							<DIV id="CtrlDataRoom9">Total: 
								<asp:TextBox id="txtPriceRoom9" runat="server" Columns="5" MaxLength="7"></asp:TextBox><SPAN id="lblMonedaRoom9" runat="server"></SPAN></DIV>
									</td>
								</tr>
							</TABLE>
						</TD>
					</TR>
					
					<TR>
						<TD align="center" colSpan="5"><SPAN class="bookingNormalLabel">Total: </SPAN><SPAN class="bookingNormalLabel" id="lblTotal" runat="server">0.0</SPAN>&nbsp;<SPAN class="bookingNormalLabel" id="lblmoneda" runat="server"></SPAN></TD>
					</TR>
					<TR>
						<TD align="center">
							<asp:panel id="pnlPromptInput" runat="server">
								<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="100%" border="0">
									<TR>
										<TD align="right" width="50%"><INPUT class="button" id="btnOk" type="button" value="Aceptar" name="btnOk" runat="server"
												Width="60px"></TD>
										<TD align="left" width="50%"><INPUT class="button" id="btnCancel" type="button" value="Cancelar" name="btnCancel" runat="server"
												Width="60px"></TD>
									</TR>
								</TABLE>
							</asp:panel></TD>
					</TR>
				</TABLE>
			</TD>
		</TR>
	</TABLE>
	</div>
</asp:panel>
