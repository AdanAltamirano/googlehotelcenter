<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ReservationsReport.aspx.vb" Inherits="RateManager.ReservationsReport" %>
<%@ Import NameSpace = "RateManager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>ReservationsReport</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
		<script type="text/javascript" src='<%= (request.applicationpath & "/pages/Scripts/jquery.min.js").Replace("//","/") %>'></script>
		<script>
			function showReservas(idgrid,idtd)
			 { 
			  var dg=document.getElementById(idgrid);			  
			  if (dg)
			   {  if (dg.style.display=='none')
			        { dg.style.display = '';
			          document.getElementById(idtd).innerHTML = "-"; 
			        }
			      else
			      {
			        dg.style.display ='none';   
			        document.getElementById(idtd).innerHTML = "+";
			       }
			   }
		}

		function FireShowImg() {
		    $('#loadingProcess').show();
		}
		
		    
		</script>
	</HEAD>
	<iframe id="gToday:normal:agenda.js" style="Z-INDEX: 999; LEFT: -500px; POSITION: absolute; TOP: -500px" 
            name="gToday:normal:agenda.js"
            src='<%=GeRequestApplicationPath(string.concat("/Calendar/",portalculture.getculture().Name.Substring(0, 2).ToLower(),"/ipopeng2.htm" )) %>'
            frameborder="0" width="174" scrolling="no" height="189">
        </iframe>
        
	<body bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0" ms_positioning="flowlayout">
		
		<form id="Form1" method="post" runat="server">
		  <div id="loadingProcess">
        </div>
  		 <div class="clear">		    
		      <div class="mDiv"></div>
		        <div>
		            <asp:label id="lbltitle" runat="server" EnableViewState="False"  
                        Text="REPORTE DE RESERVACIONES" CssClass=tituloSeccion ></asp:label> 
		        </div>
		    </div>  
			<table id="bookingcontainer" cellspacing="0" cellpadding="2" width="650" border="0">
				<tr>
					<td>
						<table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">							
							<tr>
								<td>
									<asp:RadioButton id="RdCheckIn" runat="server" Text="Checkin" GroupName="Search" Checked="True"></asp:RadioButton></td>
								<td rowspan="2"><asp:label id="lblFecha" runat="server" CssClass="clslabel" EnableViewState="False">A partir de:</asp:label>
								    <A hideFocus="" onclick="if(self.gfPop)gfPop.fPopCalendar(document.getElementById('<%response.write(txtFinal.ClientId)%>'),document.getElementById('<%response.write(txtInicio.ClientId)%>'));return false;" href="javascript:void(0)"
										><asp:textbox id="txtInicio" runat="server" CssClass="textbox" MaxLength="10" Columns="12" Width=84px></asp:textbox><IMG 
            class=PopcalTrigger alt="" 
            src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>'
            align=absMiddle border=0></A></td>
								<td rowspan="2"><asp:label id="lblHasta" runat="server" CssClass="clslabel" EnableViewState="False">Hasta:</asp:label><A hideFocus onclick="if(self.gfPop)gfPop.fPopCalendar1(document.getElementById('<%response.write(txtFinal.ClientId)%>'));return false;" href="javascript:void(0)"><asp:textbox id="txtFinal" runat="server" CssClass="textbox" MaxLength="10" Columns="12" Width=84px></asp:textbox><IMG 
            class=PopcalTrigger alt="" 
            src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>'
            align=absMiddle border=0></A></td>
								<td rowspan="2">
									<asp:Label id="lblStatus" runat="server" EnableViewState="False">Status</asp:Label>
									<asp:DropDownList id="ddlStatus" runat="server"></asp:DropDownList></td>
								<td rowspan="2"><asp:button id="btnSearch" runat="server" Text="Search" CssClass="button" EnableViewState="False" ></asp:button></td>
							</TR>
							<TR>
								<td>
									<asp:RadioButton id="RdResDate" runat="server" Text="Fecha de reservación" GroupName="Search"></asp:RadioButton></td>
							</TR>
							<TR>
								<td align="center" colSpan="5">
									<asp:checkbox id="chkAllHotels" runat="server" CssClass="clslabel" Text="Show all hotels"></asp:checkbox></td>
							</TR>
							<tr style=" padding-top:4px; padding-bottom:4px;">
							<td colspan = 5 align=center>
							<asp:Label ID="lblFiltro" runat="server" Text="" CssClass="clsHelpLabel" ></asp:Label>
							</td></tr>
							<tr>
								<td width="100%" colSpan="5"><asp:datalist id="dlHoteles"  GridLines="None" CssClass="datagrid" runat="server" EnableViewState="False" Width="100%">
										<ItemTemplate>
											<table id='tshow' runat="server" width="100%" class="dgHeader" border="0">
												<tr class="dgHeader">
													<td id='tdshow' runat="server" align="center" style="CURSOR: pointer" width="10">-</td>
													<td align="center">
													    <%#databinder.eval(container,"DataItem.nombre")%>
													</td>
													<td width="150" align="right"></td>
												</TR>
											</table>
											<asp:DataGrid id="dgReservas" CssClass="datagrid"  GridLines="None"  runat="server" AutoGenerateColumns="False" Width="100%" EnableViewState="false"
												AllowPaging="false">
												<AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
												<ItemStyle CssClass="dgItem"></ItemStyle>
												<HeaderStyle CssClass="dgHeader"></HeaderStyle>
												<Columns>
													<asp:BoundColumn HeaderText="Quantity" DataField="cant"></asp:BoundColumn>
													<asp:BoundColumn DataField="source" HeaderText="Source"></asp:BoundColumn>
													<asp:BoundColumn DataField="sourceName"></asp:BoundColumn>
													<asp:BoundColumn DataField="nombrehotel" visible="False"></asp:BoundColumn>
												</Columns>
											</asp:DataGrid>
											<BR>
										</ItemTemplate>
									</asp:datalist></td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
		</form>
		
		    <script type="text/javascript">
		        $().ready(function() {
		            $('#loadingProcess').hide();
		        });  
    </script>
	</body>
</HTML>
