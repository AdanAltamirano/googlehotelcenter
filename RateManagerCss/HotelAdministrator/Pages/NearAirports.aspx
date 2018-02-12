<%@ Page Language="vb" AutoEventWireup="false" Codebehind="NearAirports.aspx.vb" Inherits="RateManager.NearAirports"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>NearAirports</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
		<script language="javascript">		
				function show(pnl1,pnl2)
	{ document.getElementById(pnl1).style.display ="";
	  document.getElementById(pnl2).style.display ="";	  
	    var arrSelects=document.getElementsByTagName('SELECT');
        for (var i=0; i<arrSelects.length; i++)
        {arrSelects[i].style.display='None';}        
        //resizeIframe('frmPrincipal');
        onResizeIframe();
	}
			

  function HighlightRow(chkB){
    var oItem = chkB;
    xState=oItem.checked;    
    alert(chkB.parentElement.parentElement.getAttribute("itemType"));
    if(xState){		
		chkB.parentElement.parentElement.className="dgSelected";
    }
    else{
        var itemType = chkB.parentElement.parentElement.className;        

        
        if(itemType=="AlternatingItem")
			chkB.parentElement.parentElement.className="dgAlternate";
        else
			chkB.parentElement.parentElement.className="dgItem";    

    }    
  }
   function HighlightRow2(chkB,type){
    var oItem = document.getElementById(chkB);
    xState=oItem.checked;        
    if(xState){		
		oItem.parentElement.parentElement.className="dgSelected";
    }
    else{
      
        
        if(type=="AlternatingItem")
			oItem.parentElement.parentElement.className="dgAlternate";
        else
			oItem.parentElement.parentElement.className="dgItem";    

    }    
  }


		</script>
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
		 <div class="clear">
            <div class="mDiv">
            </div>
            <div class="title">
                <asp:Label ID="lbltitle" runat="server" EnableViewState="False" Text="Aeropuertos cercanos" CssClass="tituloSeccion"></asp:Label>
            </div>
         </div>
			<TABLE id="bookingcontainer" cellSpacing="0" cellPadding="0" width="500" border="0">
				<TR>
					<TD vAlign="middle" >
						<TABLE cellSpacing="1" cellPadding="1" width="500" border="0">
							<TR>
								<TD class="titulo" align="center"><asp:label id="lblTitle1" runat="server"><%=RateManager.PortalCulture.GetString("M000107")%></asp:label></TD>
							</TR>
							<tr>
								<td></td>
							</tr>
							<TR>
								<TD align="center"><BR>
									<table width="100%" border="0" cellpadding="0" cellspacing="0">
										<tr>
											<td><asp:label id="lblInfo" runat="server" CssClass="clsHelpLabel">Seleccione los aeropuertos cercanos a su hotel y establezca distancia y tiempo.</asp:label></td>
											<td><input type="button" id="btnSearchMoreAirports" runat="server" class="button"></td>
										</tr>
									</table>
									<br>
									<asp:datagrid id="grid" runat="server" CssClass="datagrid" CellSpacing="0" AutoGenerateColumns="False"
										Width="98%">
										<SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
										<AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
										<ItemStyle CssClass="dgitem"></ItemStyle>
										<HeaderStyle CssClass="dgHeader"></HeaderStyle>
										<Columns>
											<asp:TemplateColumn HeaderText="Incluir">
												<ItemStyle HorizontalAlign="Center"></ItemStyle>
												<ItemTemplate>
													<asp:CheckBox id="chkInclude" runat="server"></asp:CheckBox>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn DataField="Ciudad" HeaderText="Ciudad"></asp:BoundColumn>
											<asp:BoundColumn DataField="Aeropuerto" HeaderText="Aeropuerto">
												<ItemStyle Font-Italic="True"></ItemStyle>
											</asp:BoundColumn>
											<asp:TemplateColumn HeaderText="Distancia (Km)">
												<ItemStyle HorizontalAlign="Center"></ItemStyle>
												<ItemTemplate>
													<asp:TextBox id="txtDistance" runat="server" CssClass="textBox" Width="47px" MaxLength="6"></asp:TextBox><BR>
													<asp:RegularExpressionValidator id="valChildrenExtraPrice" runat="server" CssClass="validators" ControlToValidate="txtDistance"
														ValidationExpression="^([0-9]*|\d*\.\d{1}?\d*)$" Display="Dynamic" ErrorMessage="<NOBR>Valor Inválido</NOBR>"></asp:RegularExpressionValidator>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="Tiempo (Min)">
												<ItemStyle HorizontalAlign="Center"></ItemStyle>
												<ItemTemplate>
													<asp:TextBox id="txtTime" runat="server" CssClass="textBox" Width="47px" MaxLength="3"></asp:TextBox><BR>
													<asp:regularexpressionvalidator id="RegularExpressionValidator1" runat="server" CssClass="validators" ErrorMessage="<NOBR>Valor Inválido</NOBR>"
														Display="Dynamic" ValidationExpression="^\d+$" ControlToValidate="txtTime"></asp:regularexpressionvalidator>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn Visible="False" DataField="idAeropuerto"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="idMunicipio"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="idCiudad"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="Activo"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="Distancia"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="Tiempo"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="PK_idEmpresa"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="PK_idAeropuerto"></asp:BoundColumn>
										</Columns>
										<PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
											Position="Top" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
									</asp:datagrid></TD>
							</TR>
							<TR>
								<TD align="center"><asp:button id="btnSave" runat="server" CssClass="Button" Text="Guardar cambios"></asp:button></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<tr align="left">
					<td></td>
				</tr>
			</TABLE>
			<asp:panel id="TblPnl" style="BORDER-RIGHT: black 1px solid; BORDER-TOP: black 1px solid; DISPLAY: none; Z-INDEX: 997; FILTER: alpha(opacity=20); LEFT: 0px; BORDER-LEFT: black 1px solid; BORDER-BOTTOM: black 1px solid; POSITION: absolute; TOP: 0px; BACKGROUND-COLOR: #666666;"
				runat="server" Width="99%" Height="98%"></asp:panel><asp:panel id="PnlBox" style="DISPLAY: none; Z-INDEX: 998; LEFT: 0px; POSITION: absolute; TOP: 10px"
				runat="server" Width="99%" Height="98%">
				<TABLE id="bookingcontainer" cellSpacing="0" cellPadding="0" width="65%" border="0" runat="server" >
					<TR>
						<TD align="left" colSpan="3">
							<H2 id="lblAddairportNear" Runat="server">Agregue aeropuerto cercano</H2>
						</TD>
					</TR>
					<TR class="dgitem" height="35">
						<TD>
							<H3 id="lblSearhNewAirport" align="center" runat="server">Search airport</H3>
						</TD>
						<TD>
							<asp:textbox id="txtCityName" runat="server" MaxLength="35" Columns="35"></asp:textbox></TD>
						<TD align="left">
							<asp:button id="lnkSearch" runat="server" CssClass="Button" Text="Search"></asp:button></TD>
					</TR>
					</TR>
					<TR>
						<TD align="center" colSpan="3">
							<HR width="100%" SIZE="1">
							<asp:datagrid id="dgAirports" runat="server" CssClass="datagrid" Width="80%" AutoGenerateColumns="False">
								<AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
								<ItemStyle CssClass="dgitem"></ItemStyle>
								<HeaderStyle CssClass="dgHeader"></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="idAeropuerto" Visible="False"></asp:BoundColumn>
									<asp:BoundColumn DataField="CityName" ItemStyle-Width="40%"></asp:BoundColumn>
									<asp:BoundColumn DataField="AirportName" ItemStyle-Width="40%"></asp:BoundColumn>
									<asp:TemplateColumn ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center">
										<ItemTemplate>
											<asp:LinkButton id="lnkAddAirport" runat="server" CommandName="select" CssClass="dglink">Add</asp:LinkButton>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:BoundColumn DataField="CityCode" Visible="False"></asp:BoundColumn>
									<asp:BoundColumn DataField="idCiudad" Visible="False"></asp:BoundColumn>
								</Columns>
							</asp:datagrid></TD>
					</TR>
					<TR>
						<TD colSpan="3" height="5"></TD>
					</TR>
					<TR>
						<TD align="center" colSpan="3">
							<asp:button id="btnCancel" runat="server" CssClass="Button" Text="Cancel"></asp:button></TD>
					</TR>
				</TABLE>
			</asp:panel></form>
	</body>
</HTML>
