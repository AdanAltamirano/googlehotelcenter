<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ConfirmReservas.aspx.vb" Inherits="RateManager.ConfirmReservas" %>
<%@ Register TagPrefix="uc1" TagName="ctrlReservationsQuery" Src="../Modules/ctrlReservationsQuery.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../../Portal/Modules/ctrlFooter.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../../Portal/Modules/ctrlHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrMenu" Src="../../Portal/Modules/CtrMenu.ascx" %>
<%@ Import NameSpace = "RateManager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>ConfirmReservas</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK rel="stylesheet" type="text/css" href="../../StyleSheets/Styles.css">
	</HEAD>
	<body bottomMargin="0" leftMargin="0" rightMargin="0" topMargin="0" MS_POSITIONING="FlowLayout">

        <iframe id="gToday:normal:agenda.js" style="Z-INDEX: 999; LEFT: -500px; POSITION: absolute; TOP: -500px" 
            name="gToday:normal:agenda.js"
            src='<%=GeRequestApplicationPath(string.concat("/Calendar/",portalculture.getculture().Name.Substring(0, 2).ToLower(),"/ipopeng2.htm" )) %>'
            frameborder="0" width="174" scrolling="no" height="189">
        </iframe>

		<form id="Form1" method="post" runat="server">
		    <div class="clear">		    
		        <div class="mDiv"></div>
		        <div>
		            <asp:label id="lblTitle" runat="server" EnableViewState="False"  Text="Confirmación de reservas" CssClass=tituloSeccion ></asp:label> 
		        </div>
		    </div>  
			<table id="bookingcontainer" border="0" cellSpacing="0" cellPadding="2" width="750">
				<tr>
					<td>
						<TABLE border="0" cellSpacing="0" cellPadding="0" width="100%" align="center">
							<tr height="5">
								<td colSpan="2"><uc1:ctrlreservationsquery id="CtrlReservationsQuery1" runat="server"></uc1:ctrlreservationsquery></td>
							</tr>
							<tr>
								<td align="center"><A hideFocus onclick="if(self.gfPop)gfPop.fPopCalendar(txtInicio);return false;" href="javascript:void(0)"></A>&nbsp;
								</td>
								<td align="left"></td>
							</tr>
							<tr height="5">
								<td colSpan="2"></td>
							</tr>
							<tr>
								<td colSpan="2" align="center"><asp:datagrid id="dgReservas" runat="server" CssClass="datagrid" Width="99%" ShowFooter="True"
										AllowPaging="True" PageSize="20"  GridLines="None" AutoGenerateColumns="False">
										<FooterStyle HorizontalAlign="Right"></FooterStyle>
										<AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
										<ItemStyle CssClass="dgItem"></ItemStyle>
										<HeaderStyle CssClass="dgHeader"></HeaderStyle>
										<Columns>
											<asp:TemplateColumn HeaderText="Itinerary">
												<ItemStyle Width="20%"></ItemStyle>
												<ItemTemplate>
													<asp:HyperLink id="Itinerary" runat="server" CssClass="dglink">
														<%# DataBinder.Eval(Container, "DataItem.NoReservacion")%>
													</asp:HyperLink>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn DataField="Nombre" HeaderText="Hotel">
												<ItemStyle Width="25%"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="FechaReservacion" HeaderText="Date"></asp:BoundColumn>
											<asp:BoundColumn DataField="TravelerName" HeaderText="Customer">
												<ItemStyle Width="25%"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="CheckIn" HeaderText="Arrival">
												<ItemStyle Width="10%"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="CheckOut" HeaderText="Departure">
												<ItemStyle Width="10%"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="Cantidad" HeaderText="Rooms">
												<ItemStyle HorizontalAlign="Center" Width="10%"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="pmsACT" HeaderText="Movement"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="pmsStatus"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="status" HeaderText="Status"></asp:BoundColumn>
											<asp:TemplateColumn Visible="False" HeaderText="Status">
												<ItemTemplate>
													<asp:Label id="lblStatus" runat="server"></asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn Visible="False" DataField="StatusConf" HeaderText="StatusConf"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="WizcomPassOn" HeaderText="WizcomPassOn"></asp:BoundColumn>
											<asp:TemplateColumn HeaderText="confirm">
												<ItemStyle Width="25%"></ItemStyle>
												<ItemTemplate>
													<TABLE>
														<TR>
															<TD>
																<asp:checkbox id="chkPmsCode" runat="server"></asp:checkbox></TD>
															<TD>
																<asp:Label id="lblPmsCode" runat="server" CssClass="clslabel">
																	<%#databinder.eval(container,"DataItem.pmscode")%>
																</asp:Label></TD>
															<TD>
																<asp:textbox id="txtPmsCode" runat="server" CssClass="textbox" Width="118px" size="12" MaxLength="20"
																	Columns="12"></asp:textbox></TD>
														</TR>
													</TABLE>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn Visible="False" DataField="pmscode"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="ID"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="NoReservacion"></asp:BoundColumn>
										</Columns>
										<PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
											Position=Bottom  CssClass="dgPager" Mode="NumericPages"></PagerStyle>
									</asp:datagrid></td>
							</tr>
							<tr height="5">
								<td colSpan="2"></td>
							</tr>
							<TR>
								<TD colSpan="2" align="center"><asp:button id="btnSave" runat="server" EnableViewState="False" CssClass="button" Text="Save"></asp:button></TD>
							</TR>
							<TR>
								<TD colSpan="2" align="center"><asp:label id="lblHelp2" runat="server" EnableViewState="False">Label</asp:label></TD>
							</TR>
						</TABLE>
					</td>
				</tr>
			</table>
		</form>
		<script>
		function ShowControls(ck,txt,lbl,pmsCode)
		 {
		   var e = document.getElementById(ck);
		   var l = document.getElementById(lbl);
		   var t = document.getElementById(txt);
		   if (pmsCode!="&nbsp;")
		   {
			l.firstChild.nodeValue = pmsCode;
			t.value = pmsCode;
		   }		   
		   if (e.checked == false )
		    {
		    if (l.firstChild)
		    {
		    l.firstChild.nodeValue ="";	
		    }
			 
			 
		     l.style.display='block';
		     t.style.display='none';
		     
		    }
		   else
		    {
		        t.style.display = 'block';
		     l.style.display='none';
		    }		   
		 }
		</script>
	</body>
</HTML>
