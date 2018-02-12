<%@ Register TagPrefix="uc1" TagName="ctlMensajes" Src="../Modulos/ctlMensajes.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ConventionsMeetings.aspx.vb" Inherits="RateManager.ConventionsMeetings"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Rooms</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK rel="stylesheet" type="text/css" href="../StyleSheets/Styles.css">
		<script language="javascript">
	<!-- /* Messaje script */
		function hideElms(elmTag) 
		{
			for (i=0; i<document.all.tags(elmTag).length; i++){
				obj = document.all.tags(elmTag)[i];
				if (!obj || !obj.offsetParent) continue;
				obj.style.visibility = "hidden";
			}
		}

		function wMsgShow()
		{
			var ns4 = (document.layers)? true:false;
			var ie4 = (document.all)? true:false;

			var winW = (ns4)? window.innerWidth-16:document.body.offsetWidth-20;
			var winH = (ns4)? window.innerHeight:document.body.offsetHeight;
			
			if(ie4) hideElms('SELECT');

			var div = document.getElementById("divAlpha")		
			div.style.width="100%";
			div.style.height="100%";
			div.style.filter="alpha(opacity=30)";
			div.style.MozOpacity=0.3;
			div.style.position="absolute";
			div.style.top=0;
			div.style.left=0;
			div.style.zIndex=998;
			div.style.display='block';
					
			var wMsg = document.getElementById("TableMsg");		
			wMsg.style.display="";
			wMsg.style.position="absolute";		
			wMsg.style.left = (winW - parseInt(wMsg.style.width))/2;			
			wMsg.style.top = (winH - parseInt(wMsg.style.height))/2;			
			wMsg.style.zIndex=999;					
		}
		
		function wMsgHide()
		{
			var div = document.getElementById("divAlpha");			
			div.style.display="none";
			var wMsg = document.getElementById("TableMsg");
			wMsg.style.display="none";
		}

	//-->
		function validaInputFile()
		{ 
		  var objs = document.getElementsByTagName("INPUT");		 	 
		  for(i=0; i<objs.length; i++)
			{
			 if (objs[i].type=='file')
			 	{objs[i].disabled=true;
			    }
			}		 
		}
	
		</script>
	</HEAD>
	<body bottomMargin="0" leftMargin="0" rightMargin="0" topMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
		    <div class="clear">		    
		      <div class="mDiv"></div>
		        <div>
		            <asp:label id="lblTitleForm" runat="server" EnableViewState="False"  
                        Text="Convenciones y Reuniones" CssClass=tituloSeccion ></asp:label> 
		        </div>
		    </div>  
			<TABLE id="bookingcontainer" border="0" cellSpacing="0" cellPadding="2" width="750">
				<TR>
					<td colSpan="2"><asp:textbox style="DISPLAY: none" id="txtDiv1" runat="server" Width="34px" cssclass="TextBox">0</asp:textbox><asp:textbox style="DISPLAY: none" id="txtDiv2" runat="server" Width="34px" cssclass="TextBox">0</asp:textbox><asp:textbox style="DISPLAY: none" id="txtDiv3" runat="server" Width="34px" cssclass="TextBox">0</asp:textbox></td>
				</TR>
				<TR>
					<TD><asp:panel id="AcountPanel" runat="server" BorderStyle="None">
							<TABLE id="Table1" border="0" cellSpacing="0" cellPadding="0" width="100%" align="center">								
								<TR height="5">
									<TD></TD>
								</TR>
								<TR>
									<TD>
										<TABLE border="0" cellSpacing="0" cellPadding="0">
											<TR>
												<TD>
													<asp:Label style="Z-INDEX: 0" id="lblFiltro" runat="server" DESIGNTIMEDRAGDROP="987" CssClass="bookingNormalLabel">Estatus :</asp:Label>
													<asp:DropDownList id="ddlEstatus" runat="server"></asp:DropDownList></TD>
												<TD>&nbsp;
													<asp:button style="Z-INDEX: 0" id="btnLoadStatus" runat="server" Width="85px" CssClass="Button"
														Text="Load"></asp:button></TD>
											</TR>
											<TR>
												<TD colSpan="2">
													<P>&nbsp;</P>
												</TD>
											</TR>
										</TABLE>
									</TD>
								</TR>
								<TR>
									<TD align="center">
										<asp:datagrid id="grid"  GridLines="None" runat="server" Width="99%" CssClass="DataGrid" AutoGenerateColumns="False"
											AllowPaging="True" PageSize="4" ShowFooter="True" Borde="0">
											<FooterStyle HorizontalAlign="Right"></FooterStyle>
											<SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
											<AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
											<ItemStyle CssClass="dgItem"></ItemStyle>
											<HeaderStyle CssClass="dgHeader"></HeaderStyle>
											<Columns>
												<asp:BoundColumn Visible="False" DataField="idConvenciones" ReadOnly="True" HeaderText="idConvenciones"></asp:BoundColumn>
												<asp:BoundColumn DataField="ContactoNombre" ReadOnly="True" HeaderText="ContactoNombre"></asp:BoundColumn>
												<asp:BoundColumn DataField="ContactoEmail" ReadOnly="True" HeaderText="ContactoEmail"></asp:BoundColumn>
												<asp:BoundColumn DataField="Llegada" ReadOnly="True" HeaderText="Llegada" DataFormatString="{0:dd-MMM-yyyy}"></asp:BoundColumn>
												<asp:BoundColumn DataField="Salida" ReadOnly="True" HeaderText="Salida" DataFormatString="{0:dd-MMM-yyyy}"></asp:BoundColumn>
												<asp:BoundColumn Visible="False" DataField="Estatus" ReadOnly="True" HeaderText="Estatus"></asp:BoundColumn>
												<asp:TemplateColumn HeaderText="Estatus">
													<ItemTemplate>
														<asp:Label id="lblEstatusEdit" runat="server"></asp:Label>
														<asp:LinkButton style="Z-INDEX: 0" id="lnkStatus" runat="server" CausesValidation="False" CommandName="Edit">Editar</asp:LinkButton>
													</ItemTemplate>
													<EditItemTemplate>
														<TABLE style="Z-INDEX: 0" id="Table8" border="0" cellSpacing="3" cellPadding="1" width="100%">
															<TR>
																<TD vAlign="top" align="center">
																	<asp:DropDownList id="ddlEstatusEdit" runat="server" Font-Size="10px"></asp:DropDownList></TD>
															</TR>
															<TR>
																<TD vAlign="top" align="center">
																	<TABLE id="Table10" border="0" cellSpacing="2" cellPadding="1">
																		<TR>
																			<TD>
																				<asp:LinkButton id="lnkUpdate" runat="server" CausesValidation="False" CommandName="Update">Actualizar</asp:LinkButton></TD>
																			<TD>
																				<asp:LinkButton id="lnkCancel" runat="server" CausesValidation="False" CommandName="Cancel">Cancelar</asp:LinkButton></TD>
																		</TR>
																	</TABLE>
																</TD>
															</TR>
														</TABLE>
													</EditItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn>
													<ItemTemplate>
														<asp:LinkButton id="lnkSeleccionar" runat="server" CommandName="Select">Seleccionar</asp:LinkButton>
													</ItemTemplate>
												</asp:TemplateColumn>
											</Columns>
											<PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
												Position="Bottom" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
										</asp:datagrid></TD>
								</TR>
								<TR height="5">
									<TD></TD>
								</TR>
								<TR>
									<TD>
										<DIV align="center">
											<TABLE style="BORDER-BOTTOM: #3366ff 1px solid; BORDER-LEFT: #3366ff 1px solid; BACKGROUND-COLOR: white; BORDER-TOP: #3366ff 1px solid; BORDER-RIGHT: #3366ff 1px solid"
												id="TablaPincipal" border="0" cellSpacing="0" cellPadding="0" width="99%" runat="server">
												<TR>
													<TD style="HEIGHT: 19px" colSpan="2" align="left"><B>
															<asp:Label id="lbl_NombreEmpresa" runat="server" Text="Empresa y/o Grupo :"></asp:Label></B></TD>
												</TR> <!--Lledado-->
												<TR>
													<TD style="WIDTH: 10px">&nbsp;</TD>
													<TD style="HEIGHT: 19px" align="left">&nbsp;
														<asp:Label id="lblNombreEmpresa" runat="server"></asp:Label></TD>
												</TR> <!--Espacio en Blanco-->
												<TR>
													<TD colSpan="2">&nbsp;</TD>
												</TR> <!--Preguta-->
												<TR>
													<TD style="HEIGHT: 19px" colSpan="2" align="left"><B>
															<asp:Label id="lbl_Noches" runat="server" Text="Número de Noches que durará su evento :"></asp:Label></B></TD>
												</TR> <!--Lledado-->
												<TR>
													<TD style="WIDTH: 10px">&nbsp;</TD>
													<TD style="HEIGHT: 19px" align="left">&nbsp;
														<asp:Label id="lblNoches" runat="server"></asp:Label></TD>
												</TR> <!--Espacio en Blanco-->
												<TR>
													<TD style="HEIGHT: 19px" colSpan="2">&nbsp;</TD>
												</TR> <!--Preguta-->
												<TR>
													<TD style="HEIGHT: 17px" colSpan="2" align="left"><B>
															<asp:Label id="lbl_TipoEvento" runat="server" Text="¿Cual es el tipo de evento que va a realizar?"></asp:Label></B></TD>
												</TR> <!--Lledado-->
												<TR>
													<TD style="WIDTH: 10px; HEIGHT: 19px">&nbsp;</TD>
													<TD style="HEIGHT: 19px" vAlign="top" align="left">
														<asp:Label id="lblTipoEvento" runat="server"></asp:Label>
														<TABLE id="TableOtroTipoEvento" border="0" cellSpacing="0" cellPadding="0" runat="server">
															<TR>
																<TD></TD>
																<TD></TD>
															</TR>
														</TABLE>
													</TD>
												</TR> <!--Espacio en Blanco-->
												<TR>
													<TD style="HEIGHT: 19px" colSpan="2"></TD>
												</TR> <!--Preguta-->
												<TR>
													<TD colSpan="2" align="left"><B>
															<asp:Label id="lbl_Fechas" runat="server" Text="Fechas :"></asp:Label></B></TD>
												</TR> <!--Lledado-->
												<TR>
													<TD style="WIDTH: 10px; HEIGHT: 19px">&nbsp;</TD>
													<TD align="left"><!--Llegada y Salida-->
														<TABLE border="0" cellSpacing="1" cellPadding="1">
															<TR>
																<TD>&nbsp;</TD>
																<TD>
																	<asp:Label id="lbl_Llegada" runat="server" Text="Llegada&nbsp;"></asp:Label></TD>
																<TD>
																	<asp:Label id="lblLlegada" runat="server"></asp:Label>&nbsp;<A id="aIframeLlegada" href="javascript:void(0)" runat="server">
																	</A>
																</TD>
																<TD>&nbsp;
																</TD>
																<TD>
																	<asp:Label id="lbl_Salida" runat="server" Text="Salida&nbsp;"></asp:Label></TD>
																<TD>
																	<asp:Label id="lblSalida" runat="server"></asp:Label>&nbsp;<A id="aIframeSalida" href="javascript:void(0)" runat="server"></A>
																</TD>
															</TR>
														</TABLE>
													</TD>
												</TR> <!--Espacio en Blanco--> <!--Preguta--> <!--Lledado--> <!--Espacio en Blanco-->
												<TR>
													<TD style="HEIGHT: 19px" colSpan="2">&nbsp;</TD>
												</TR> <!--Preguta-->
												<TR>
													<TD colSpan="2" align="left"><B>
															<asp:Label id="lbl_Flexibilidad" runat="server" Text="¿Su evento y/o grupo tiene flexibilidad para cambiar de fechas?"></asp:Label></B></TD>
												</TR> <!--Lledado-->
												<TR>
													<TD style="WIDTH: 10px">&nbsp;</TD>
													<TD style="HEIGHT: 19px" align="left">
														<TABLE border="0" cellSpacing="1" cellPadding="1">
															<TR>
																<TD>&nbsp;
																	<asp:Label id="lblFlexibilidad" runat="server"></asp:Label></TD>
															</TR>
															<TR>
																<TD><!--Llegada y Salida-->
																	<TABLE id="TablaOtraFechas" border="0" cellSpacing="1" cellPadding="1" runat="server">
																		<TR>
																			<TD colSpan="6"><B>
																					<asp:Label id="lbl_FlexibilidadFechas" runat="server" Text="¿Que otras fechas?"></asp:Label></B></TD>
																		</TR>
																		<TR>
																			<TD>&nbsp;</TD>
																			<TD>
																				<asp:Label id="lbl_FlexibilidadLlegada" runat="server" Text="Llegada&nbsp;"></asp:Label></TD>
																			<TD>
																				<asp:Label id="lblFlexibilidadLlegada" runat="server"></asp:Label>&nbsp;<A id="aIframeFlexibilidadLlegada" href="javascript:void(0)" runat="server"></A>
																			</TD>
																			<TD style="WIDTH: 7px">&nbsp;
																			</TD>
																			<TD>
																				<asp:Label id="lbl_FlexibilidadSalida" runat="server" Text="Salida&nbsp;"></asp:Label></TD>
																			<TD>
																				<asp:Label id="lblFlexibilidadSalida" runat="server"></asp:Label>&nbsp;<A id="aIframeFlexibilidadSalida" href="javascript:void(0)" runat="server"></A>
																			</TD>
																		</TR>
																	</TABLE>
																</TD>
															</TR>
														</TABLE>
													</TD>
												</TR> <!--Espacio en Blanco-->
												<TR>
													<TD colSpan="2">&nbsp;</TD>
												</TR> <!--Preguta-->
												<TR>
													<TD style="HEIGHT: 19px" colSpan="2" align="left"><B>
															<asp:Label id="lbl_NumeroPersonas" runat="server" Text="Número de personas :"></asp:Label></B></TD>
												</TR> <!--Lledado-->
												<TR>
													<TD style="WIDTH: 10px; HEIGHT: 19px">&nbsp;</TD>
													<TD style="HEIGHT: 19px" align="left">&nbsp;
														<asp:Label id="lblNumeroPersonas" runat="server"></asp:Label></TD>
												</TR> <!--Espacio en Blanco-->
												<TR>
													<TD colSpan="2">&nbsp;</TD>
												</TR> <!--Preguta-->
												<TR>
													<TD colSpan="2" align="left"><B>
															<asp:Label id="lbl_NumeroHabitaciones" runat="server" Text="Número Total de Habitaciones solicitadas :"></asp:Label></B></TD>
												</TR> <!--Lledado-->
												<TR>
													<TD style="WIDTH: 10px; HEIGHT: 24px">&nbsp;</TD>
													<TD style="HEIGHT: 24px" align="left">
														<TABLE border="0" cellSpacing="1" cellPadding="1">
															<TR>
																<TD>&nbsp;
																	<asp:Label id="lblNumeroHabitaciones" runat="server"></asp:Label></TD>
															</TR>
															<TR>
																<TD><B>
																		<asp:Label id="lbl_HabitacionesSon" runat="server" Text="De las cuales son :"></asp:Label></B></TD>
															</TR>
															<TR>
																<TD><!--Llegada y Salida-->
																	<TABLE border="0" cellSpacing="1" cellPadding="1">
																		<TR>
																			<TD>&nbsp;</TD>
																			<TD>
																				<asp:Label id="lbl_NumeroHabitacionesSencillas" runat="server" Text="Sencilla "></asp:Label></TD>
																			<TD style="WIDTH: 7px">&nbsp;
																				<asp:Label id="lblNumeroHabitacionesSencillas" runat="server"></asp:Label></TD>
																		</TR>
																		<TR>
																			<TD>&nbsp;</TD>
																			<TD>
																				<asp:Label id="lbl_NumeroHabitacionesDobles" runat="server" Text="Doble"></asp:Label></TD>
																			<TD style="WIDTH: 7px">&nbsp;
																				<asp:Label id="lblNumeroHabitacionesDobles" runat="server"></asp:Label></TD>
																		</TR>
																		<TR>
																			<TD>&nbsp;</TD>
																			<TD>
																				<asp:Label id="lbl_NumeroHabitacionesTriples" runat="server" Text="Triple"></asp:Label></TD>
																			<TD style="WIDTH: 7px">&nbsp;
																				<asp:Label id="lblNumeroHabitacionesTriples" runat="server"></asp:Label></TD>
																		</TR>
																		<TR>
																			<TD>&nbsp;</TD>
																			<TD>
																				<asp:Label id="lbl_NumeroHabitacionesJrSuites" runat="server" Text="Jr.Suites"></asp:Label></TD>
																			<TD style="WIDTH: 7px">&nbsp;
																				<asp:Label id="lblNumeroHabitacionesJrSuites" runat="server"></asp:Label></TD>
																		</TR>
																		<TR>
																			<TD>&nbsp;</TD>
																			<TD>
																				<asp:Label id="lbl_NumeroHabitacionesMasterSuites" runat="server" Text="Master Suites"></asp:Label></TD>
																			<TD style="WIDTH: 7px">&nbsp;
																				<asp:Label id="lblNumeroHabitacionesMasterSuites" runat="server"></asp:Label></TD>
																		</TR>
																		<TR>
																			<TD>&nbsp;</TD>
																			<TD>
																				<asp:Label id="lbl_NumeroHabitacionesSuitePresidente" runat="server" Text="Suite Presidencial"></asp:Label></TD>
																			<TD style="WIDTH: 7px">&nbsp;
																				<asp:Label id="lblNumeroHabitacionesSuitePresidente" runat="server"></asp:Label></TD>
																		</TR>
																	</TABLE>
																</TD>
															</TR>
														</TABLE>
													</TD>
												</TR> <!--Espacio en Blanco-->
												<TR>
													<TD colSpan="2">&nbsp;</TD>
												</TR> <!--Preguta-->
												<TR>
													<TD style="HEIGHT: 19px" colSpan="2" align="left"><B>
															<asp:Label id="lbl_Hoteles" runat="server" Text="¿Que hotel(es) en especial de Hoteles Misión?"></asp:Label></B></TD>
												</TR> <!--Lledado-->
												<TR>
													<TD style="WIDTH: 10px">&nbsp;</TD>
													<TD align="left"><BR>
														&nbsp;
														<asp:Label id="lblHoteles" runat="server"></asp:Label></TD>
												</TR> <!--Espacio en Blanco-->
												<TR>
													<TD style="HEIGHT: 19px" colSpan="2">&nbsp;</TD>
												</TR> <!--Preguta-->
												<TR>
													<TD colSpan="2" align="left"><B>
															<asp:Label id="lbl_Salones" runat="server" Text="¿Requiere Salones para Sesionar?"></asp:Label></B></TD>
												</TR> <!--Lledado-->
												<TR>
													<TD style="WIDTH: 10px">&nbsp;</TD>
													<TD style="HEIGHT: 19px" align="left">
														<TABLE border="0" cellSpacing="1" cellPadding="1">
															<TR>
																<TD style="HEIGHT: 21px">
																	<asp:Label id="lbl_Salon" runat="server" Text="Salones"></asp:Label></TD>
																<TD style="WIDTH: 100px; HEIGHT: 21px">
																	<asp:Label id="lbl_Salon1" runat="server" Text="Salon 1"></asp:Label></TD>
																<TD style="WIDTH: 100px; HEIGHT: 21px">
																	<asp:Label id="lbl_Salon2" runat="server" Text="Salon 2"></asp:Label></TD>
																<TD style="WIDTH: 100px; HEIGHT: 21px">
																	<asp:Label id="lbl_Salon3" runat="server" Text="Salon 3"></asp:Label></TD>
															</TR>
															<TR>
																<TD style="HEIGHT: 21px">
																	<asp:Label id="lbl_TipoMontaje" runat="server" Text="Tipo de Montaje"></asp:Label></TD>
																<TD style="WIDTH: 100px; HEIGHT: 21px">
																	<asp:Label id="lblTipoMontaje1" runat="server"></asp:Label></TD>
																<TD style="WIDTH: 100px; HEIGHT: 21px">
																	<asp:Label id="lblTipoMontaje2" runat="server"></asp:Label></TD>
																<TD style="WIDTH: 100px; HEIGHT: 21px">
																	<asp:Label id="lblTipoMontaje3" runat="server"></asp:Label></TD>
															</TR>
															<TR>
																<TD>
																	<asp:Label id="lbl_NumeroSalonPersona" runat="server" Text="Número de Personas&nbsp;"></asp:Label></TD>
																<TD style="WIDTH: 100px">
																	<asp:Label id="lblNumeroSalonPersona1" runat="server"></asp:Label></TD>
																<TD style="WIDTH: 100px">
																	<asp:Label id="lblNumeroSalonPersona2" runat="server"></asp:Label></TD>
																<TD style="WIDTH: 100px">
																	<asp:Label id="lblNumeroSalonPersona3" runat="server"></asp:Label></TD>
															</TR>
														</TABLE>
													</TD>
												</TR> <!--Espacio en Blanco-->
												<TR>
													<TD colSpan="2">&nbsp;</TD>
												</TR> <!--Preguta-->
												<TR>
													<TD colSpan="2" align="left"><B>
															<asp:Label id="lbl_AudioVisual" runat="server" Text="¿Requiere Equipo Audiovisual?  (Ingrese el número de equipos que requiere)"></asp:Label></B></TD>
												</TR> <!--Lledado-->
												<TR>
													<TD style="WIDTH: 10px">&nbsp;</TD>
													<TD align="left">
														<TABLE border="0" cellSpacing="1" cellPadding="1">
															<TR>
																<TD>
																	<asp:Label id="lbl_AudioVisualPantalla" runat="server" Text="Pantalla"></asp:Label></TD>
																<TD>
																	<asp:Label id="lblAudioVisualPantalla" runat="server"></asp:Label></TD>
															</TR>
															<TR>
																<TD>
																	<asp:Label id="lbl_AudioVisualEquipoSonido" runat="server" Text="Equipo de Sonido con micrófonos"></asp:Label></TD>
																<TD>
																	<asp:Label id="lblAudioVisualEquipoSonido" runat="server"></asp:Label></TD>
															</TR>
															<TR>
																<TD>
																	<asp:Label id="lbl_AudioVisualAcetatos" runat="server" Text="Proyector de acetatos"></asp:Label></TD>
																<TD>
																	<asp:Label id="lblAudioVisualAcetatos" runat="server"></asp:Label></TD>
															</TR>
															<TR>
																<TD>
																	<asp:Label id="lbl_AudioVisualProyectorTransparencias" runat="server" Text="Proyector de transparencias"></asp:Label></TD>
																<TD>
																	<asp:Label id="lblAudioVisualProyectorTransparencias" runat="server"></asp:Label></TD>
															</TR>
															<TR>
																<TD>
																	<asp:Label id="lbl_AudioVisualCanion" runat="server" Text="Cañón"></asp:Label></TD>
																<TD>
																	<asp:Label id="lblAudioVisualCanion" runat="server"></asp:Label></TD>
															</TR>
															<TR>
																<TD>
																	<asp:Label id="lbl_AudioVisualLapTop" runat="server" Text="Lap Top"></asp:Label></TD>
																<TD>
																	<asp:Label id="lblAudioVisualLapTop" runat="server"></asp:Label></TD>
															</TR>
															<TR>
																<TD>
																	<asp:Label id="lbl_AudioVisualCopiadora" runat="server" Text="Copiadora"></asp:Label></TD>
																<TD>
																	<asp:Label id="lblAudioVisualCopiadora" runat="server"></asp:Label></TD>
															</TR>
															<TR>
																<TD>
																	<asp:Label id="lbl_AudioVisualOtro" runat="server" Text="Otro"></asp:Label></TD>
																<TD>&nbsp;
																	<asp:Label id="lblAudioVisualOtro" runat="server"></asp:Label></TD>
															</TR>
														</TABLE>
													</TD>
												</TR> <!--Espacio en Blanco-->
												<TR>
													<TD colSpan="2"></TD>
												</TR> <!--Preguta-->
												<TR>
													<TD colSpan="2" align="left"><B>
															<asp:Label id="lbl_Alimentos" runat="server" Text="Número de Alimentos :"></asp:Label></B></TD>
												</TR> <!--Lledado-->
												<TR>
													<TD style="WIDTH: 10px">&nbsp;</TD>
													<TD align="left">
														<TABLE border="0" cellSpacing="0" cellPadding="0">
															<TR>
																<TD>
																	<asp:Label id="lbl_AlimentosDesayunos" runat="server" Text="Desayunos&nbsp;"></asp:Label></TD>
																<TD>
																	<asp:Label id="lblAlimentosDesayunos" runat="server"></asp:Label></TD>
																<TD>
																	<asp:Label id="lbl_AlimentosComidas" runat="server" Text="&nbsp;Comidas&nbsp;"></asp:Label></TD>
																<TD>
																	<asp:Label id="lblAlimentosComidas" runat="server"></asp:Label></TD>
																<TD>
																	<asp:Label id="lbl_AlimentosCenas" runat="server" Text="&nbsp;Cenas&nbsp;"></asp:Label></TD>
																<TD>
																	<asp:Label id="lblAlimentosCenas" runat="server"></asp:Label>&nbsp;</TD>
															</TR>
														</TABLE>
													</TD>
												</TR> <!--Espacio en Blanco--> <!--Preguta--> <!--Lledado--> <!--Espacio en Blanco-->
												<TR>
													<TD colSpan="2">&nbsp;</TD>
												</TR> <!--Preguta-->
												<TR>
													<TD colSpan="2" align="left"><B>
															<asp:Label id="lbl_AlimentosRequiere" runat="server" Text="Los alimentos los requiere :"></asp:Label></B></TD>
												</TR> <!--Lledado-->
												<TR>
													<TD style="WIDTH: 10px">&nbsp;</TD>
													<TD style="HEIGHT: 19px" align="left">
														<asp:Label id="lblAlimentosRequiere" runat="server"></asp:Label></TD>
												</TR> <!--Espacio en Blanco--> <!--Preguta--> <!--Lledado--> <!--Espacio en Blanco-->
												<TR>
													<TD colSpan="2">&nbsp;</TD>
												</TR> <!--Preguta-->
												<TR>
													<TD colSpan="2" align="left"><B>
															<asp:Label id="lbl_Bebidas" runat="server" Text="Sus bebidas las requiere por :"></asp:Label></B></TD>
												</TR> <!--Lledado-->
												<TR>
													<TD style="WIDTH: 10px; HEIGHT: 66px">&nbsp;</TD>
													<TD style="HEIGHT: 66px" align="left">
														<TABLE border="0" cellSpacing="1" cellPadding="1">
															<TR>
																<TD>
																	<asp:CheckBox id="chkBebidasRefresco" runat="server" Text="" Enabled="False"></asp:CheckBox>
																	<asp:Label id="lblBebidasRefresco" Text="Refrescos" Runat="server"></asp:Label></TD>
																<TD>
																	<asp:CheckBox id="chkBebidasCervezas" runat="server" Text="" Enabled="False"></asp:CheckBox>
																	<asp:Label id="lblBebidasCervezas" Text="Cervezas" Runat="server"></asp:Label></TD>
															</TR>
															<TR>
																<TD>
																	<asp:CheckBox id="chkBebidasBotella" runat="server" Text="" Enabled="False"></asp:CheckBox>
																	<asp:Label id="lblBebidasBotella" Text="Botella" Runat="server"></asp:Label></TD>
																<TD>
																	<asp:CheckBox id="chkBebidasCopeo" runat="server" Text="" Enabled="False"></asp:CheckBox>
																	<asp:Label id="lblBebidasCopeo" Text="Copeo" Runat="server"></asp:Label></TD>
															</TR>
															<TR>
																<TD>
																	<asp:CheckBox id="chkBebidasBarraNacional" runat="server" Text="" Enabled="False"></asp:CheckBox>
																	<asp:Label id="lblBebidasBarraNacional" Text="Barra Importada" Runat="server"></asp:Label></TD>
																<TD>
																	<asp:CheckBox id="chkBebidasAguasFrutas" runat="server" Text="" Enabled="False"></asp:CheckBox>
																	<asp:Label id="lblBebidasAguasFrutas" Text="Aguas de Frutas" Runat="server"></asp:Label></TD>
															</TR>
															<TR>
																<TD>
																	<asp:CheckBox id="chkBebidasDescorcheBotella" runat="server" Text="" Enabled="False"></asp:CheckBox>
																	<asp:Label id="lblBebidasDescorcheBotella" Text="Descorche por Botella" Runat="server"></asp:Label></TD>
																<TD>
																	<asp:CheckBox id="chkBebidasBarraImportada" runat="server" Text="" Enabled="False"></asp:CheckBox>
																	<asp:Label id="lblBebidasBarraImportada" Text="Barra Nacional" Runat="server"></asp:Label></TD>
															</TR>
															<TR>
																<TD>
																	<asp:CheckBox id="chkBebidasDescorchePersona" runat="server" Text="" Enabled="False"></asp:CheckBox>
																	<asp:Label id="lblBebidasDescorchePersona" Text="Descorche por Persona" Runat="server"></asp:Label></TD>
																<TD></TD>
															</TR>
														</TABLE>
													</TD>
												</TR> <!--Espacio en Blanco-->
												<TR>
													<TD colSpan="2">&nbsp;</TD>
												</TR> <!--Preguta-->
												<TR>
													<TD colSpan="2" align="left"><B>
															<asp:Label id="lbl_BebidasEN" runat="server" Text="Las bebidas alcohólicas solo las desea en :"></asp:Label></B></TD>
												</TR> <!--Lledado-->
												<TR>
													<TD style="WIDTH: 10px; HEIGHT: 64px">&nbsp;</TD>
													<TD style="HEIGHT: 64px" align="left">
														<TABLE>
															<TR>
																<TD vAlign="top">
																	<asp:CheckBox id="chkBebidasEnComidas" runat="server" Text="" Enabled="False"></asp:CheckBox>
																	<asp:Label id="lblBebidasEnComidas" Text="Comidas" Runat="server"></asp:Label></TD>
																<TD vAlign="top">
																	<asp:CheckBox id="chkBebidasEnCenas" runat="server" Text="" Enabled="False"></asp:CheckBox>
																	<asp:Label id="lblBebidasEnCenas" Text="Cenas" Runat="server"></asp:Label></TD>
																<TD vAlign="top">
																	<TABLE border="0" cellSpacing="0" cellPadding="0">
																		<TR>
																			<TD>
																				<asp:CheckBox id="chkBebidasEnOtro" runat="server" Text="" Enabled="False"></asp:CheckBox>
																				<asp:Label id="lblBebidasEnOtro" Text="Otro" Runat="server"></asp:Label>&nbsp;
																				<asp:Label id="lblBebidasENOtroCual" runat="server"></asp:Label></TD>
																		</TR>
																	</TABLE>
																</TD>
															</TR>
														</TABLE>
													</TD>
												</TR> <!--Espacio en Blanco-->
												<TR>
													<TD colSpan="2">&nbsp;</TD>
												</TR> <!--Preguta-->
												<TR>
													<TD style="HEIGHT: 19px" colSpan="2" align="left"><B>
															<asp:Label id="lbl_NochesTema" runat="server" Text="¿Requiere de Noches Tema?"></asp:Label></B></TD>
												</TR> <!--Lledado-->
												<TR>
													<TD style="WIDTH: 10px; HEIGHT: 19px">&nbsp;</TD>
													<TD style="HEIGHT: 19px" align="left">
														<TABLE border="0" cellSpacing="1" cellPadding="1">
															<TR>
																<TD>
																	<asp:CheckBox id="chkNochesTemaPalenque" runat="server" Text="" Enabled="False"></asp:CheckBox>
																	<asp:Label id="lblNochesTemaPalenque" Text="Palenque" Runat="server"></asp:Label></TD>
															</TR>
															<TR>
																<TD>
																	<asp:CheckBox id="chkNochesTemaCasino" runat="server" Text="" Enabled="False"></asp:CheckBox>
																	<asp:Label id="lblNochesTemaCasino" Text="Casino" Runat="server"></asp:Label></TD>
															</TR>
															<TR>
																<TD>
																	<TABLE border="0" cellSpacing="0" cellPadding="0">
																		<TR>
																			<TD>
																				<asp:CheckBox id="chkNochesTemaOtras" runat="server" Text="" Enabled="False"></asp:CheckBox>
																				<asp:Label id="lblNochesTemaOtras" Text="Otros" Runat="server"></asp:Label></TD>
																			<TD>
																				<TABLE id="TableNochesTemaOtras" border="0" cellSpacing="0" cellPadding="0" runat="server">
																					<TR>
																						<TD>
																							<asp:Label id="lbl_NochesTemaOtrasCuales" runat="server" Text="&nbsp;¿Cuales?&nbsp;"></asp:Label></TD>
																						<TD>
																							<asp:Label id="lblNochesTemaOtrasCuales" runat="server"></asp:Label></TD>
																					</TR>
																				</TABLE>
																			</TD>
																		</TR>
																	</TABLE>
																</TD>
															</TR>
														</TABLE>
													</TD>
												</TR> <!--Espacio en Blanco-->
												<TR>
													<TD colSpan="2">&nbsp;</TD>
												</TR> <!--Preguta-->
												<TR>
													<TD colSpan="2" align="left"><B>
															<asp:Label id="lbl_ServiciosExternos" runat="server" Text="Requiere de Servicios Externos :"></asp:Label></B></TD>
												</TR> <!--Lledado-->
												<TR>
													<TD style="WIDTH: 10px">&nbsp;</TD>
													<TD style="HEIGHT: 19px" align="left">
														<TABLE border="0" cellSpacing="1" cellPadding="1">
															<TR>
																<TD>
																	<asp:CheckBox id="chkServiciosExternosPeleaGallos" runat="server" Text="" Enabled="False"></asp:CheckBox>
																	<asp:Label id="lblServiciosExternosPeleaGallos" Text="Pelea de Gallos" Runat="server"></asp:Label></TD>
															</TR>
															<TR>
																<TD>
																	<asp:CheckBox id="chkServiciosExternosMariachi" runat="server" Text="" Enabled="False"></asp:CheckBox>
																	<asp:Label id="lblServiciosExternosMariachi" Text="Mariachi" Runat="server"></asp:Label></TD>
															</TR>
															<TR>
																<TD>
																	<asp:CheckBox id="chkServiciosExternosTrio" runat="server" Text="" Enabled="False"></asp:CheckBox>
																	<asp:Label id="lblServiciosExternosTrio" Text="Trio" Runat="server"></asp:Label></TD>
															</TR>
															<TR>
																<TD>
																	<asp:CheckBox id="chkServiciosExternosCarpas" runat="server" Text="" Enabled="False"></asp:CheckBox>
																	<asp:Label id="lblServiciosExternosCarpas" Text="Carpas" Runat="server"></asp:Label></TD>
															</TR>
															<TR>
																<TD>
																	<asp:CheckBox id="chkServiciosExternosManteleriaEspecial" runat="server" Text="" Enabled="False"></asp:CheckBox>
																	<asp:Label id="lblServiciosExternosManteleriaEspecial" Text="Mantelería Especial" Runat="server"></asp:Label></TD>
															</TR>
															<TR>
																<TD>
																	<asp:CheckBox id="chkServiciosExternosCentroMesa" runat="server" Text="" Enabled="False"></asp:CheckBox>
																	<asp:Label id="lblServiciosExternosCentroMesa" Text="Centros de Mesa" Runat="server"></asp:Label></TD>
															</TR>
															<TR>
																<TD>
																	<asp:CheckBox id="chkServiciosExternosEquipoAudioVisual" runat="server" Text="" Enabled="False"></asp:CheckBox>
																	<asp:Label id="lblServiciosExternosEquipoAudioVisual" Text="Equipo Audiovisual Especial" Runat="server"></asp:Label></TD>
															</TR>
															<TR>
																<TD>
																	<asp:CheckBox id="chkServiciosExternosRecorridoPorCiudad" runat="server" Text="" Enabled="False"></asp:CheckBox>
																	<asp:Label id="lblServiciosExternosRecorridoPorCiudad" Text="Recorrido por Ciudad" Runat="server"></asp:Label></TD>
															</TR>
															<TR>
																<TD>
																	<TABLE border="0" cellSpacing="0" cellPadding="0">
																		<TR>
																			<TD>
																				<asp:CheckBox id="chkServiciosExternosOtros" runat="server" Text="" Enabled="False"></asp:CheckBox>
																				<asp:Label id="lblServiciosExternosOtros" Text="Otros" Runat="server"></asp:Label></TD>
																			<TD></TD>
																		</TR>
																	</TABLE>
																</TD>
															</TR>
														</TABLE>
													</TD>
												</TR> <!--Espacio en Blanco-->
												<TR>
													<TD colSpan="2">&nbsp;</TD>
												</TR> <!--Preguta-->
												<TR>
													<TD colSpan="2" align="left"><B>
															<asp:Label id="lbl_EventoPorAgenciaViajes" runat="server" Text="Su evento será manejado por una agencia de Viajes :"></asp:Label></B></TD>
												</TR> <!--Lledado-->
												<TR>
													<TD style="WIDTH: 10px">&nbsp;</TD>
													<TD style="HEIGHT: 19px" align="left">
														<TABLE border="0" cellSpacing="1" cellPadding="1">
															<TR>
																<TD>
																	<TABLE border="0" cellSpacing="0" cellPadding="0">
																		<TR>
																			<TD>
																				<asp:RadioButton id="rbEventoPorAgenciaViajesSi" runat="server" Text="" Enabled="False" GroupName="A2"
																					Checked="true"></asp:RadioButton>
																				<asp:Label id="lblEventoPorAgenciaViajesSi" Text="Si" Runat="server"></asp:Label></TD>
																			<TD>
																				<TABLE id="TablaAgenciaViajesSi" border="0" cellSpacing="0" cellPadding="0" runat="server">
																					<TR>
																						<TD></TD>
																						<TD>&nbsp;
																						</TD>
																					</TR>
																				</TABLE>
																			</TD>
																		</TR>
																	</TABLE>
																</TD>
															</TR>
															<TR>
																<TD>
																	<asp:RadioButton id="rbEventoPorAgenciaViajesNo" runat="server" Text="" Enabled="False" GroupName="A2"></asp:RadioButton>
																	<asp:Label id="lblEventoPorAgenciaViajesNo" Text="No" Runat="server"></asp:Label></TD>
															</TR>
														</TABLE>
													</TD>
												</TR> <!--Espacio en Blanco-->
												<TR>
													<TD colSpan="2">&nbsp;</TD>
												</TR> <!--Preguta-->
												<TR>
													<TD style="HEIGHT: 19px" colSpan="2" align="left"><B>
															<asp:Label id="lbl_CotizadoEn" runat="server" Text="Su presupuesto lo requiere cotizado en :"></asp:Label></B></TD>
												</TR> <!--Lledado-->
												<TR>
													<TD style="WIDTH: 10px">&nbsp;</TD>
													<TD style="HEIGHT: 19px" align="left">
														<TABLE border="0" cellSpacing="1" cellPadding="1">
															<TR>
																<TD>
																	<asp:RadioButton id="rbCotizadoEnPaquete" runat="server" Text="" Enabled="False" GroupName="A3" Checked="true"></asp:RadioButton>
																	<asp:Label id="lblCotizadoEnPaquete" Text="Paquete" Runat="server"></asp:Label></TD>
															</TR>
															<TR>
																<TD>
																	<asp:RadioButton id="rbCotizadoEnHabitacionesAlimentos" runat="server" Text="" Enabled="False" GroupName="A3"></asp:RadioButton>
																	<asp:Label id="lblCotizadoEnHabitacionesAlimentos" Text="Habitaciones y Alimentos por separado"
																		Runat="server"></asp:Label></TD>
															</TR>
															<TR>
																<TD>
																	<TABLE border="0" cellSpacing="0" cellPadding="0">
																		<TR>
																			<TD>
																				<asp:RadioButton id="rbCotizadoEnOtro" runat="server" Text="" Enabled="False" GroupName="A3"></asp:RadioButton>
																				<asp:Label id="lblCotizadoEnOtro" Text="Otro" Runat="server"></asp:Label></TD>
																			<TD>
																				<TABLE id="TableCotizadoEnOtro" border="0" cellSpacing="0" cellPadding="0" runat="server">
																					<TR>
																						<TD></TD>
																						<TD>&nbsp;
																						</TD>
																					</TR>
																				</TABLE>
																			</TD>
																		</TR>
																	</TABLE>
																</TD>
															</TR>
														</TABLE>
													</TD>
												</TR> <!--Espacio en Blanco-->
												<TR>
													<TD colSpan="2">&nbsp;</TD>
												</TR> <!--Preguta-->
												<TR>
													<TD colSpan="2" align="left"><B>
															<asp:Label id="lbl_RequerimiestosEspeciales" runat="server" Text="Requerimientos Especiales :"></asp:Label></B></TD>
												</TR> <!--Lledado-->
												<TR>
													<TD style="WIDTH: 10px">&nbsp;</TD>
													<TD style="HEIGHT: 19px" align="left">&nbsp;
														<asp:Label id="lblRequerimiestosEspeciales" runat="server"></asp:Label></TD>
												</TR> <!--Espacio en Blanco-->
												<TR>
													<TD colSpan="2">&nbsp;</TD>
												</TR> <!--Preguta-->
												<TR>
													<TD colSpan="2" align="left"><B>
															<asp:Label id="lbl_Contacto" runat="server" Text="Por favor proporcionenos sus datos para poder contactarlo"></asp:Label></B></TD>
												</TR> <!--Lledado-->
												<TR>
													<TD style="WIDTH: 10px">&nbsp;</TD>
													<TD style="HEIGHT: 19px" align="left">
														<TABLE border="0" cellSpacing="1" cellPadding="1">
															<TR>
																<TD align="right">
																	<asp:Label id="lbl_ContactoNombre" runat="server" Text="Nombre :"></asp:Label></TD>
																<TD>
																	<asp:Label id="lblContactoNombre" runat="server"></asp:Label></TD>
																<TD></TD>
																<TD align="right">
																	<asp:Label id="lbl_ContactoPuesto" runat="server" Text="Puesto :"></asp:Label></TD>
																<TD>
																	<asp:Label id="lblContactoPuesto" runat="server"></asp:Label></TD>
															</TR>
															<TR>
																<TD align="right">
																	<asp:Label id="lbl_ContactoEmpresa" runat="server" Text="Empresa : "></asp:Label></TD>
																<TD>
																	<asp:Label id="lblContactoEmpresa" runat="server"></asp:Label></TD>
																<TD></TD>
																<TD align="right">
																	<asp:Label id="lbl_ContactoCiudad" runat="server" Text="Ciudad : "></asp:Label></TD>
																<TD>
																	<asp:Label id="lblContactoCiudad" runat="server"></asp:Label></TD>
															</TR>
															<TR>
																<TD align="right">
																	<asp:Label id="lbl_ContactoCalle" runat="server" Text="Calle : "></asp:Label></TD>
																<TD>
																	<asp:Label id="lblContactoCalle" runat="server"></asp:Label></TD>
																<TD></TD>
																<TD align="right">
																	<asp:Label id="lbl_ContactoColonia" runat="server" Text="Colonia :"></asp:Label></TD>
																<TD>
																	<asp:Label id="lblContactoColonia" runat="server"></asp:Label></TD>
															</TR>
															<TR>
																<TD align="right">
																	<asp:Label id="lbl_ContactoTelefono" runat="server" Text="Teléfono :"></asp:Label></TD>
																<TD>
																	<asp:Label id="lblContactoTelefono" runat="server"></asp:Label></TD>
																<TD></TD>
																<TD align="right">
																	<asp:Label id="lbl_ContactoCodigoPostal" runat="server" Text="C.P :"></asp:Label></TD>
																<TD>
																	<asp:Label id="lblContactoCodigoPostal" runat="server"></asp:Label></TD>
															</TR>
															<TR>
																<TD align="right">
																	<asp:Label id="lbl_ContactoFax" runat="server" Text="Fax :"></asp:Label></TD>
																<TD>
																	<asp:Label id="lblContactoFax" runat="server"></asp:Label></TD>
																<TD></TD>
																<TD align="right">
																	<asp:Label id="lbl_ContactoEmail" runat="server" Text="E-mail :"></asp:Label></TD>
																<TD>
																	<asp:Label id="lblContactoEmail" runat="server"></asp:Label></TD>
															</TR>
														</TABLE>
													</TD>
												</TR>
											</TABLE>
										</DIV>
									</TD>
								</TR>
							</TABLE>
						</asp:panel></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
