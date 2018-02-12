<%@ Page Language="vb" AutoEventWireup="false" Codebehind="FaresCopy.aspx.vb" Inherits="RateManager.FaresCopy"%>
<%@ Import NameSpace = "RateManager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>FaresCopy</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
		</LINK>
	</HEAD>
	<body>
	 <iframe id="gToday:normal:agenda.js" style="Z-INDEX: 999; LEFT: -500px; POSITION: absolute; TOP: -500px" 
            name="gToday:normal:agenda.js"
            src='<%=GeRequestApplicationPath(string.concat("/Calendar/",portalculture.getculture().Name.Substring(0, 2).ToLower(),"/ipopeng2.htm" )) %>'
            frameborder="0" width="174" scrolling="no" height="189">
        </iframe>
		<form id="Form1" method="post" runat="server">
           <div class="clear">
                <div class="mDiv">
                </div>
                <div class="title">
                    <asp:Label ID="lblTitle" runat="server" EnableViewState="False" Text="Extensión de Tarifas" CssClass="tituloSeccion"></asp:Label>
                </div>
            </div>
			<table id="bookingcontainer" cellSpacing="0" cellPadding="2" width="650" border="0">			
				<tr>
					<td><asp:label id="lblRatePlan" runat="server" EnableViewState="False">Rate Plan</asp:label><asp:dropdownlist id="ddlRatePlan" runat="server"></asp:dropdownlist></td>
					<td><asp:label id="lblDesde" runat="server" EnableViewState="False">Desde:</asp:label><asp:dropdownlist id="ddlMonth1" runat="server"></asp:dropdownlist>&nbsp;<asp:dropdownlist id="ddlYear1" runat="server"></asp:dropdownlist></td>
					<TD><asp:label id="lblHasta" runat="server" EnableViewState="False">Hasta</asp:label><asp:dropdownlist id="ddlMonth2" runat="server"></asp:dropdownlist>&nbsp;<asp:dropdownlist id="ddlYear2" runat="server"></asp:dropdownlist></TD>
					<td><asp:button id="btnLoad" runat="server" CssClass="button" Text="load" EnableViewState="False"></asp:button></td>
				</tr>
				<TR>
					<TD colSpan="4" height="5"></TD>
				</TR>
				<TR>
					<TD class="dgitem" colSpan="2"><asp:label id="lblSelect" runat="server" EnableViewState="False">Seleccione tarifas a copiar</asp:label></TD>
					<TD class="dgitem" align="right" colSpan="2"><asp:checkbox id="chkAll" runat="server" Text="change all"></asp:checkbox></TD>
				</TR>
				<TR>
					<TD colSpan="4" height="5"></TD>
				</TR>
				<TR>
					<TD align="center" colSpan="4"><asp:datagrid id="dgRates" GridLines="None"  runat="server" CssClass="datagrid" Width="100%" AutoGenerateColumns="False">
							<AlternatingItemStyle CssClass="dgalternate"></AlternatingItemStyle>
							<ItemStyle CssClass="dgitem"></ItemStyle>
							<HeaderStyle CssClass="dgheader"></HeaderStyle>
							<Columns>
								<asp:TemplateColumn>
									<ItemTemplate>
										<asp:CheckBox id="chkChange" runat="server"></asp:CheckBox>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="codigohabitacion" HeaderText="Room"></asp:BoundColumn>
								<asp:BoundColumn DataField="nombrehabitacion" visible="False"></asp:BoundColumn>
								<asp:BoundColumn DataField="fechainicia" HeaderText="Begin"></asp:BoundColumn>
								<asp:BoundColumn DataField="fechafinaliza" HeaderText="End"></asp:BoundColumn>
								<asp:BoundColumn DataField="tarifaAdulto" HeaderText="2(Ad)-0(Ch)">
								 <HeaderStyle HorizontalAlign="Right" />
                                        <ItemStyle HorizontalAlign="Right" />  
								</asp:BoundColumn>
								<asp:TemplateColumn HeaderText="New Start Date">
									<ItemStyle Width="20%"></ItemStyle>
									<ItemTemplate>
										<asp:textbox id="txtinicio" Runat="server" Columns="10" MaxLength="10"></asp:textbox>
										<img id="imgCalInicio" class="PopcalTrigger" alt="" src="../Calendar/calbtn.gif" align="absMiddle"
											border="0" runat="server" onmouseover="this.style.cursor='hand'">
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="New End Date">
									<ItemStyle Width="20%"></ItemStyle>
									<ItemTemplate>
										<asp:textbox id="txtfin" runat="server" Columns="10" MaxLength="10"></asp:textbox>
										<img id="imgCalFin" runat="server" class="PopcalTrigger" alt="" src="../Calendar/calbtn.gif"
											align="absMiddle" border="0" onmouseover="this.style.cursor='hand'">
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn Visible="False" DataField="idtarifa" HeaderText="IDTARIFA"></asp:BoundColumn>
							</Columns>
						</asp:datagrid><asp:label id="lblNoRates" runat="server">No hay tarifas en las fechas seleccionadas</asp:label></TD>
				</TR>
				<TR>
					<TD align="center" colSpan="4" height="5"><asp:button id="btnSave" runat="server" CssClass="button" Text="Save" EnableViewState="False"></asp:button></TD>
				</TR>
				<TR>
					<TD align="center" colSpan="4" height="5">
						<asp:Label id="lblAllSave" runat="server">Fueron copiadas con exito todas las tarifas</asp:Label></TD>
				</TR>
				<TR>
					<TD align="left" colSpan="4"><asp:label id="lblErrorOverlapped" runat="server" CssClass="validators" Visible="False">Error overlapped fares</asp:label></TD>
				</TR>
			</table>
		</form>
		<script>
	function CheckAll(chk,dg)
	 {
		var e = document.getElementById(chk);
		var grid = document.getElementById(dg);
		var item = grid.getElementsByTagName("tr");
		
		for (var i=0; i<item.length; i++)
		 {
			var box = item[i].getElementsByTagName("input"); 
			for (var j = 0; j< box.length; j++)
			 {
				if (box[j].id.indexOf("chkChange")!=-1)
				 {
				  box[j].checked = e.checked;
				 }
			 }			
		 }	 
    }
    function showRatePlanName(ddl,array,lbl)
	{	
		  var e = document.getElementById(ddl);
		  var lbl = document.getElementById(lbl);		  
		 if (array!='')
		 { 
		  lbl.firstChild.nodeValue =array.split("//")[e.selectedIndex];
		  }
		 else
		 {		 
		 lbl.firstChild.nodeValue ='-';
		 }		  
	}
		</script>
	</body>
</HTML>
