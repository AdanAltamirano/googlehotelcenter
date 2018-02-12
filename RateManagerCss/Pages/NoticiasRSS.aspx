<%@ Register TagPrefix="uc1" TagName="CtrlIdiomaFCk" Src="../Portal/Modules/Contenido/CtrlIdiomaFCk.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="NoticiasRSS.aspx.vb" validateRequest="false" Inherits="RateManager.NoticiasRSS"%>
<%@ Register TagPrefix="uc1" TagName="CtrlIdioma" Src="../Modulos/CtrlIdioma.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlPlanFares" Src="../Modulos/ctrlPlanFares.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrlPlanFaresExc" Src="../Modulos/CtrlPlanFaresExc.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctlMensajes" Src="../Modulos/ctlMensajes.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrMenu" Src="../Portal/Modules/CtrMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../Portal/Modules/ctrlHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../Portal/Modules/ctrlFooter.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrRateAplication" Src="../Modulos/ctrRateAplication.ascx" %>
<%@ Import NameSpace = "RateManager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>FaresCatalogue</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
		</LINK>
		<script language="javascript">
<!--
  function HighlightRow(chkB){
    var oItem = chkB;
    xState=oItem.checked;    
    if(xState){
		chkB.parentElement.parentElement.className="DataGridSelectedItem"; //.style.backgroundColor='#CE5D5A';
        //chkB.parentElement.parentElement.style.color='white'; 
    }
    else{
        var itemType = chkB.parentElement.parentElement.getAttribute("itemType")//.className="";//style.backgroundColor='#F7F7DE';         
        if(itemType=="AlternatingItem")
			chkB.parentElement.parentElement.className="DataGridAlternatedItem";
        else
			chkB.parentElement.parentElement.className="DataGrid";
        
        //chkB.parentElement.parentElement.style.color='black'; 
    }    
  }
  
  function hP(ele)
  {
	var p = document.getElementById(ele);
	if(p) p.style.display='none';
  }
  
  function sP(ele,ele1,ele2)
  {
	try{
		var p = document.getElementById(ele);
		var p1= document.getElementById(ele1);
		var p2= document.getElementById(ele2);
		if (p && p2){
		    if(p.value=='') p.value=0;
		    if(p1){
		    	if(p1.value=='') p1.value=0;
		    	p2.innerHTML= '$' +  (formatAsMoney(eval(p.value) + eval(p1.value)));	
		    }else{
				p2.innerHTML= '$' +  formatAsMoney(eval(p.value))
		    }				
			p2.style.display='block';
		}
	}catch(e){
		p2.style.display='none';
	}
  }
  

  function formatAsMoney(mnt) {
    mnt -= 0;
    mnt = (Math.round(mnt*100))/100;
    return (mnt == Math.floor(mnt)) ? mnt + '.00' 
              : ( (mnt*10 == Math.floor(mnt*10)) ? 
                       mnt + '0' : mnt);
  }
 
function MostrarFiltro(chkFiltro)
{				    
	chkF = document.getElementById(chkFiltro);			    
	var tblFiltro = document.getElementById('TableFiltro');
	tblFiltro.style.display = (chkF.checked == true) ? 'block' : 'none';                
} 


//-->
		</script>
		<script>
		/*function showRatePlan2(ddl,array,lbl)
		 { 
		  var e = document.getElementById(ddl);
		  var lbl = document.getElementById(lbl);		  		  	  
		  lbl.firstChild.nodeValue =array.split("//")[e.selectedIndex];
		  		  
		  }
		  
	  	 function showRatePlan(ddl,array,lbl)
		 { 
		  var e = document.getElementById(ddl);
		  var lbl = document.getElementById(lbl);		  
		  if (e.selectedIndex!=0) 
		  {		  
		  lbl.firstChild.nodeValue =array.split("//")[e.selectedIndex-1];
		  }
		  else
		  {
		   lbl.firstChild.nodeValue ='-';
		   }
		  
		  }*/
		  		 function Ocultar(v)
					{
						var e= document.getElementById('DivRates');
						var s = document.getElementById('hplShowRates');
						var o = document.getElementById('hplHideRates');
						if (v == '1')
							{
							    e.style.display = 'block';
							    o.style.display = 'block';     
								s.style.display = "none";  
							}
						else
							{
								e.style.display = "none";
								s.style.display = 'block';     
								o.style.display = "none";  
							} 
					}
					
						function optionSw(e)		
			{
			   var div1A;
			   var div2B;	   
			   var td1A;
			   var td2B;

					var txt;
					txt = document.getElementById('txtDivP');
					txt.value = e;

					div1A= document.getElementById('divA2');
					div2B= document.getElementById('divB2');
					td1A= document.getElementById('TdPricing');
					td1B= document.getElementById('TdPricingE');
					 switch (e)
					  {
						case '1P':

						    div1A.style.display = 'block';
							div2B.style.display="none";		
							
							td1A.className= 'tabselected';
							td1B.className= 'tab';
							break;
						case '1E':
							
							div1A.style.display="none";
							div2B.style.display = 'block';							
							
							
							td1A.className= 'tab';
							td1B.className= 'tabselected';
							break;														
					  }    
					  return true;			
			}
			
		</script>
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<iframe id="gToday:normal:agenda.js" style="Z-INDEX: 999; LEFT: -500px; POSITION: absolute; TOP: -500px" 
            name="gToday:normal:agenda.js"
            src='<%=GeRequestApplicationPath(string.concat("/Calendar/",portalculture.getculture().Name.Substring(0, 2).ToLower(),"/ipopeng2.htm" )) %>'
            frameborder="0" width="174" scrolling="no" height="189">
            </iframe>
         <div class="clear">
            <div class="mDiv">
            </div>
            <div class="title">
                <asp:Label ID="lbltitle" runat="server" EnableViewState="False" Text="Noticias RSS" CssClass="tituloSeccion"></asp:Label>
            </div>
         </div>
			<TABLE id="bookingcontainer" cellSpacing="0" cellPadding="2" width="700" border="0">
				<tr>
					<td>
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">
							<TR>
								<TD colSpan="6" height="5"></TD>
							</TR>
							<TR>
								<TD vAlign="top" align="center" colSpan="6"></TD>
							</TR>
							<TR>
								<TD vAlign="top" width="12%" align="left"><asp:label id="lblEName" runat="server" EnableViewState="False" CssClass="clslabel">Canal:</asp:label></TD>
								<TD vAlign="top" width="16%" align="left"><asp:dropdownlist id="ddlCanal" runat="server"></asp:dropdownlist></TD>
								<TD vAlign="top" align="left">
									<TABLE id="Table2" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD style="HEIGHT: 18px" align="left"></TD>
											<TD style="HEIGHT: 18px" align="left"><asp:checkbox id="CheckBoxFiltro" runat="server" Text="Filtro"  CssClass="clslabel"></asp:checkbox></TD>
										</TR>
										<TR>
											<TD></TD>
											<TD>
												<TABLE id="TableFiltro" cellSpacing="0" cellPadding="0" width="100%" border="0">
													<TR>
														<TD><asp:label id="LabelTituloFiltro" runat="server" CssClass="clslabel">Titulo</asp:label></TD>
														<TD colSpan="3"><asp:textbox id="TextboxTituloFiltro" runat="server" Width="198px"></asp:textbox></TD>
													</TR>
													<TR>
														<TD><asp:label id="LabelDescripcionFiltro" runat="server" CssClass="clslabel">Descripcion</asp:label></TD>
														<TD colSpan="3"><asp:textbox id="TextBoxDescripcionFiltro" runat="server" Width="355px"></asp:textbox></TD>
													</TR>
													<TR>
														<TD><asp:label id="LabelDesdeFiltro" runat="server" CssClass="clslabel">Desde:</asp:label></TD>
														<TD><asp:textbox id="txtDateFromFiltro" runat="server" CssClass="textbox" Width="75px" Columns="10"
																MaxLength="10"></asp:textbox><A hideFocus onclick="if(self.gfPop)gfPop.fPopCalendar(document.getElementById('<%response.write(txtDateToFiltro.ClientId)%>'),document.getElementById('<%response.write(txtDateFromFiltro.ClientId)%>'));return false;" href="javascript:void(0)" ><IMG 
                        class=PopcalTrigger alt="" 
                        src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>' 
                        align=absMiddle border=0></A></TD>
														<TD><asp:label id="LabelHastaFiltro" runat="server" CssClass="clslabel">Hasta:</asp:label></TD>
														<TD><asp:textbox id="txtDateToFiltro" runat="server" CssClass="textbox" Width="75px" Columns="10"
																MaxLength="10"></asp:textbox><A hideFocus onclick="if(self.gfPop)gfPop.fPopCalendar1(document.getElementById('<%response.write(txtDateToFiltro.ClientId)%>'));return false;" href="javascript:void(0)" ><IMG 
                        class=PopcalTrigger alt="" 
                        src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>'
                        align=absMiddle border=0></A></TD>
													</TR>
													<TR>
														<TD colSpan="4"><asp:checkbox id="CheckBoxIncluirFechas" runat="server" Text="Incluir fechas"  CssClass="clslabel"></asp:checkbox></TD>
													</TR>
												</TABLE>
											</TD>
										</TR>
									</TABLE>
								</TD>
								<TD vAlign="top" align="right"><asp:button id="btnLoad" runat="server" CssClass="button" Text="Load" CausesValidation="False"></asp:button></TD>
							</TR>
							<TR>
								<TD vAlign="top" align="center" colSpan="9"><asp:label id="LabelMsgCanalLoad" runat="server" CssClass="validators" Visible="False"> Seleccione un Canal</asp:label></TD>
							</TR>
							<TR>
								<TD vAlign="top" align="center" colSpan="9"><asp:label id="lblNoNotesFound" runat="server" CssClass="validators" Visible="False">No existen Notas para el Canal</asp:label></TD>
							</TR>
							<TR>
								<TD align="center" colSpan="6"><asp:datagrid id="dgNotasRSS" runat="server" CssClass="datagrid" Width="99%" ShowFooter="True"
										AllowPaging="True" AutoGenerateColumns="False" PageSize="6">
										<FooterStyle HorizontalAlign="Right"></FooterStyle>
										<SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
										<AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
										<ItemStyle CssClass="dgItem"></ItemStyle>
										<HeaderStyle CssClass="dgHeader"></HeaderStyle>
										<Columns>
											<asp:BoundColumn DataField="Title" HeaderText="Titulo"></asp:BoundColumn>
											<asp:BoundColumn Visible="False" DataField="idNota"></asp:BoundColumn>
											<asp:BoundColumn DataField="pubDate" HeaderText="Fecha de Publicacion"></asp:BoundColumn>
											<asp:TemplateColumn>
												<ItemTemplate>
													<asp:LinkButton id="lnkEdit" runat="server" CausesValidation="False" CssClass="dgLink" CommandName="Edit"></asp:LinkButton>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn>
												<ItemTemplate>
													<asp:LinkButton id="lnkDelete2" style="display:none" runat="server" CssClass="dgLink" CausesValidation="False"
														CommandName="Delete"></asp:LinkButton>
													<asp:HyperLink id="lnkDelete" runat="server" CssClass="dglink">Delete</asp:HyperLink>
												</ItemTemplate>
											</asp:TemplateColumn>
										</Columns>
										<PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
											Position="Top" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
									</asp:datagrid></TD>
							</TR>
							<TR>
								<TD align="center" colSpan="6">
									<TABLE cellSpacing="1" cellPadding="1" width="100%" border="0">
										<TR>
											<TD class="dgitem" style="HEIGHT: 14px" align="center" colSpan="3"><asp:label id="lblFaresTitle" runat="server" EnableViewState="False" CssClass="bookingnormallabel">Noticia RSS</asp:label></TD>
										</TR>
										<TR>
											<TD align="center" colSpan="3"><asp:label id="lblMsgUpdateNote" runat="server" CssClass="validators" Visible="False">Se agrego la nota al canal</asp:label></TD>
										</TR>
										<TR>
											<TD align="left" colSpan="2"><asp:label id="LabelCanalNuevo" runat="server" EnableViewState="False" CssClass="clslabel">Canal:</asp:label></TD>
											<TD align="left"><asp:dropdownlist id="ddlCanalNuevo" runat="server"></asp:dropdownlist><asp:label id="MsgCanal" runat="server" CssClass="validators" Visible="False">Seleccione un Canal</asp:label></TD>
										</TR>
										<TR>
											<TD align="left" colSpan="2"><asp:label id="lblFechaNoticia" runat="server" EnableViewState="False" CssClass="clslabel">Fecha:</asp:label></TD>
											<TD align="left"><A hideFocus onclick="if(self.gfPop)gfPop.fPopCalendar(document.getElementById('<%response.write(txtDateTo.ClientId)%>'),document.getElementById('<%response.write(txtDateFrom.ClientId)%>'));return false;" href="javascript:void(0)" ><asp:textbox id="txtDateFrom" runat="server" CssClass="textbox" Width="75px" Columns="10" MaxLength="10"></asp:textbox></A><A hideFocus onclick="if(self.gfPop)gfPop.fPopCalendar(document.getElementById('<%response.write(txtDateTo.ClientId)%>'),document.getElementById('<%response.write(txtDateFrom.ClientId)%>'));return false;" href="javascript:void(0)" ><IMG class=PopcalTrigger 
                  alt="" src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>' 
                  align=absMiddle border=0></A>
												<asp:textbox id="txtDateTo" runat="server" CssClass="textbox" Width="75px" Columns="10" MaxLength="10"></asp:textbox><asp:label id="lblMsgFecha" runat="server" EnableViewState="false" CssClass="validators" Visible="False">Proporcione una fecha valida</asp:label></TD>
										</TR>
										<TR>
											<TD align="left" colSpan="2"><asp:label id="LabelTituloNuevo" runat="server" EnableViewState="False" CssClass="clslabel">Titulo:</asp:label></TD>
											<TD align="left"><uc1:ctrlidioma id="CtrlIdiomaFCk_Titulo" runat="server"></uc1:ctrlidioma><asp:label id="lblMsgTitulo" runat="server" EnableViewState="false" CssClass="validators" Visible="False">Proporcione el titulo en ambos Idiomas</asp:label></TD>
										</TR>
										<TR>
											<TD align="left" colSpan="2"><asp:label id="LabelLinkNuevo" runat="server" EnableViewState="False" CssClass="clslabel">Link:</asp:label></TD>
											<TD align="left"><asp:textbox id="TextBoxLink" runat="server" Width="463px"></asp:textbox></TD>
										</TR>
										<TR>
											<TD align="left" colSpan="2"><asp:label id="LabelDescripcionNuevo" runat="server" EnableViewState="False" CssClass="clslabel">Descripcion:</asp:label></TD>
											<TD align="left"><uc1:ctrlidiomafck id="CtrlIdiomaFCk_Descripcion" runat="server"></uc1:ctrlidiomafck></TD>
										</TR>
										<TR>
											<TD style="HEIGHT: 25px" align="left" colSpan="2"><asp:label id="LabelAutorNuevo" runat="server" EnableViewState="False" CssClass="clslabel">Autor:</asp:label></TD>
											<TD style="HEIGHT: 25px" align="left"><asp:textbox id="TextBoxAutor" runat="server"></asp:textbox></TD>
										</TR>
										<TR>
											<TD align="center" colSpan="3">
												<HR width="100%" SIZE="1">
												<asp:button id="btnNew" runat="server" EnableViewState="False" CssClass="Button" Text="Nuevo"
													Width="85px" CausesValidation="False"></asp:button><asp:button id="btnSave" runat="server" EnableViewState="False" CssClass="Button" Text="Guardar"
													Width="85px"></asp:button></TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
						</TABLE>
					</td>
				</tr>
			</TABLE>
			<uc1:ctlmensajes id="CtlMensajes1" runat="server"></uc1:ctlmensajes></form>
		<script>
		/* function showRoomType(ddl)
		  {
		   var Code = document.getElementById(ddl);
		   var indice = Code.selectedIndex;		   		   
		   var lbl = document.getElementById('lblRoomType');
		   lbl.innerText= Code[indice].value;
		  }*/
		  /*function showRoomType(ddl,array)
		  {
		  var e = document.getElementById(ddl);
		  var lbl = document.getElementById('lblRoomType');		  
		  if (e.selectedIndex!=0) 
		  {		  
		  lbl.firstChild.nodeValue =array.split("//")[e.selectedIndex];
		  }
		 else
		 {
		 lbl.firstChild.nodeValue ='-';
		 }
		  
		  }*/
		

		  
		
		
		
			
			function IniDate()
		             {		 		                     
		                var fecha = new Date();
		                var fecha2 = new Date(2030, 12, 31);
		                var arr = new Array(3);
		                arr[0] = [fecha.getFullYear(), fecha.getMonth()+1, fecha.getDate()]
		                arr[1] = [fecha2.getFullYear(), fecha2.getMonth()+1, fecha2.getDate()];
		                return arr;
		                
		             }
		             
		  
				
				  function FillPrices(Dg,typeFare,Price)
				{					
					var P = document.getElementById(Price);					
					var grid = document.getElementById(Dg);
					var item = grid.getElementsByTagName("tr");					
					
					for (var i = 1; i < item.length; i++)
					 {						
						var txt = item[i].getElementsByTagName("input");
						var lbl = item[i].getElementsByTagName("span");
						
						for (var j = 0; j < lbl.length; j++)
						{				
							for (var t = 0; t < txt.length; t++)
							{							
								if (txt[t].id.indexOf(typeFare) != -1)
								{
								txt[t].value = P.value;
								}					
							}
									
							if (lbl[j].id.indexOf("lblTotal") != -1)
								{	
								if (txt.length>1)
								{
									sP(txt[0].id,txt[1].id,lbl[j].id);
								}
								else
								{
								sP(txt[0].id,'',lbl[j].id);
								}								
								}					
						}					
						
						
					 }
								
				}
				
		
		</script>
	</body>
</HTML>
