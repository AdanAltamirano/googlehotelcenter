<%@ Page Language="vb" AutoEventWireup="false" Codebehind="InvoiceToConciliateDetails.aspx.vb" Inherits="RateManager.InvoiceToConciliateDetails" %>
<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../../Portal/Modules/ctrlHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../../Portal/Modules/ctrlFooter.ascx" %>
<%@ Import NameSpace = "RateManager"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>InvoiceToConciliateDetails</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
		<script src="Script/Util.js"></script>
		<script language="javascript" type="text/javascript">
			
			function Conciliation(prefijo, NumRva, CheckIn, CheckInFormat, CheckOut, Nights, CommissionDetailID, status, divId, display){
				var div = document.getElementById(prefijo + divId);				
				if (div){				
					div.style.display = display;
					reStart(prefijo,status);
				}
				var lbl = document.getElementById(prefijo + 'lblDgReservationID');				
				lbl.innerHTML = NumRva;
				lbl = document.getElementById(prefijo + 'lblDgCheckIn');
				lbl.innerHTML = CheckInFormat;
				lbl = document.getElementById(prefijo + 'lblDgCheckOut');
				lbl.innerHTML = CheckOut;
				var txt = document.getElementById(prefijo + 'txtDgNewCheckIn');
				txt.value = CheckIn;
				txt = document.getElementById(prefijo + 'txtDgNewNights');
				txt.value = Nights;
				var hdn = document.getElementById(prefijo + 'hdnDgCommissionDetailID');
				hdn.value = CommissionDetailID;
			}
			
			function toggleConciliate(prefijo, divId, display, status){
				var div = document.getElementById(prefijo + divId);
				if (div){
					div.style.display = display;
					reStart(prefijo,status);
				}
			}
								
			function toggleChangeNights(prefijo, ddlId, trCheckIn, trNights, txtId){
				var ddl = document.getElementById(prefijo + ddlId);
				var checkIn = document.getElementById(prefijo + trCheckIn);
				var Nights = document.getElementById(prefijo + trNights);
				var txt = document.getElementById(prefijo + txtId);
				if (checkIn && Nights && ddl && txt){
					if (ddl.selectedIndex == 1){			
						checkIn.style.display = '';
						Nights.style.display = '';
						txt.focus();
					}else{
						checkIn.style.display = 'none';
						Nights.style.display = 'none';
					}
				}
			}
			
			function reStart(prefijo,status){
				var txtCheckIn = document.getElementById(prefijo + 'txtDgNewCheckIn');
				var txtNights = document.getElementById(prefijo + 'txtDgNewNights');
				var ddl = document.getElementById(prefijo + 'ddlDgCorrectionOptions');
				var checkIn = document.getElementById(prefijo + 'TrCheckIn');
				var Nights = document.getElementById(prefijo + 'TrNights');
				if (txtCheckIn && txtNights){
					txtCheckIn.value = '';
					txtNights.value = '';
					if (ddl && checkIn && Nights){
						if (status != 0){
							/*if (status == 4){
								ddl.selectedIndex = 2;
							}else{*/
								ddl.selectedIndex = status - 1;
							//}
						}else{
							ddl.selectedIndex = 0;
						}
						if (status == 2){
							checkIn.style.display = '';
							Nights.style.display = '';							
						}else{
							checkIn.style.display = 'none';
							Nights.style.display = 'none';
						}
					}
				}
			}
			
			function showConfirm(){
				document.getElementById('FinishConciliation').style.display='';
			}
			
			function hideConfirm(){
				document.getElementById('FinishConciliation').style.display='none';
			}

			function switch_visible_ddlP(inv) {
			    var id1 = '<%=divPaymentPref.ClientID %>';
			    var id2 = '<%=divPaymentList.ClientID %>';
			    if (inv) {
			        document.getElementById(id2).style.display = "inline-block";
			        document.getElementById(id1).style.display = "none";
			    }
			    else {
			        document.getElementById(id1).style.display = "inline-block";
			        document.getElementById(id2).style.display = "none";
			    }
			}

			function val_ddlPaymentPref() {
			    if (document.getElementById("<%=ddlPaymentList.ClientID%>").value == "1") {
			        document.getElementById("<%=txtOther.ClientID%>").style.backgroundColor = '#ffffff';
			        document.getElementById('<%=txtOther.ClientID%>').disabled = '';
			    }
			    else {
			        document.getElementById("<%=txtOther.ClientID%>").style.backgroundColor = '#bbbbbb';
			        document.getElementById('<%=txtOther.ClientID%>').disabled = 'true';
			        document.getElementById('<%=txtOther.ClientID%>').value = ''
			    }

			    if (document.getElementById("<%=ddlPaymentList.ClientID%>").value == "2" || document.getElementById("<%=ddlPaymentList.ClientID%>").value == 0) {
			        document.getElementById("<%=txtAccountNumber.ClientID%>").style.backgroundColor = '#bbbbbb';
			        document.getElementById('<%=txtAccountNumber.ClientID%>').disabled = 'true';
			        document.getElementById('<%=txtAccountNumber.ClientID%>').value = ''
			    }
			    else {
			        document.getElementById('<%=txtAccountNumber.ClientID%>').disabled = '';
			        document.getElementById("<%=txtAccountNumber.ClientID%>").style.backgroundColor = '#ffffff';
			    }
			}

			function valPaymentMethod() {
			    var res = true;
			    var txtaccount = document.getElementById("<%=txtAccountNumber.ClientID %>");
			    var txtother = document.getElementById("<%=txtOther.ClientID %>");
			    var PaymentListValue = document.getElementById("<%=ddlPaymentList.ClientID %>").value;

			    if (PaymentListValue == 1 && txtother.value.trim() == "") {
			        document.getElementById("rqfvOther").style.display = "inline-block";
			        res = false;
			    }
			    else
			        document.getElementById("rqfvOther").style.display = "none";


			    if (PaymentListValue != 0 && PaymentListValue != 2 && txtaccount.value.length < 4) {
			        document.getElementById("rqfvAccount").style.display = "inline-block";
			        res = false;
			    }
			    else
			        document.getElementById("rqfvAccount").style.display = "none";

			    return res;
			}
			
		</script>
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="5" rightMargin="0">
		<form id="Form1" method="post" runat="server">
			<iframe id="gToday:normal:agenda.js" style="Z-INDEX: 999; LEFT: -500px; POSITION: absolute; TOP: -500px" 
                name="gToday:normal:agenda.js"
                src='<%=GeRequestApplicationPath(string.concat("/Calendar/",portalculture.getculture().Name.Substring(0, 2).ToLower(),"/ipopeng2.htm" )) %>'
                frameborder="0" width="174" scrolling="no" height="189">
            </iframe>
		    <div>
		        <div class="mDiv"></div>
                <div class="title">
                    <asp:Label ID="lblTitle" runat="server" EnableViewState="False" 
                        CssClass="tituloSeccion">Facturas Pendientes de Conciliar</asp:Label>
                </div>
            </div>
            
			<TABLE id="bookingcontainer" cellSpacing="0" cellPadding="0" width="700" border="0" align="center">
				
				<TR>
					<TD align="right" colSpan="4" height="15"><asp:label id="lblActualDate" runat="server" CssClass="bookingNormalLabel">[Actual Date]</asp:label></TD>
				</TR>
				<TR>
					<TD colSpan="4" height="15">
						<TABLE id="Table2" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD style="WIDTH: 231px" align="left" colSpan="3"><asp:label id="lblHotelName" runat="server" CssClass="bookingNormalLabel"></asp:label></TD>
								<TD align="left"></TD>
								<TD width="5"></TD>
								<TD align="left"></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 231px" align="left" colSpan="3" height="10"><asp:label id="lblHotelAddress" runat="server" CssClass="clsLabel"></asp:label></TD>
								<TD align="right" height="10"><asp:label id="lblsInvoiceNumber" runat="server" CssClass="bookingNormalLabel">Número de Factura:</asp:label></TD>
								<TD width="5" height="10"></TD>
								<TD height="10"><asp:label id="lblInvoiceNumber" runat="server" CssClass="clsLabel"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 231px; HEIGHT: 13px" colSpan="3"><asp:label id="lblHotelCityState" runat="server" CssClass="clsLabel"></asp:label></TD>
								<TD style="HEIGHT: 13px" align="right">
									<asp:label id="lblsReferenceBank" runat="server" CssClass="bookingNormalLabel">Referencia Bancaria :</asp:label></TD>
								<TD style="HEIGHT: 13px" width="5"></TD>
								<TD style="HEIGHT: 13px">
									<asp:label id="lblReferenceBank" runat="server" CssClass="clsLabel"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 231px" colSpan="3"><asp:label id="lblHotelPhone" runat="server" CssClass="clsLabel"></asp:label></TD>
								<TD align="right"><asp:label id="lblPeriod" runat="server" CssClass="bookingNormalLabel">Período :</asp:label></TD>
								<TD width="5"></TD>
								<TD><asp:label id="lblPeriodDate" runat="server" CssClass="clsLabel"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 231px" colSpan="3"></TD>
								<TD style="HEIGHT: 17px" align="right"></TD>
								<TD style="HEIGHT: 17px" width="5"></TD>
								<TD style="HEIGHT: 17px"><asp:label id="lblPeriodRange" runat="server" CssClass="clsLabel"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 231px" colSpan="3"></TD>
								<TD align="right"><asp:label id="lblsGenerated" runat="server" CssClass="bookingNormalLabel">Generado:</asp:label></TD>
								<TD width="5"></TD>
								<TD><asp:label id="lblGenerated" runat="server" CssClass="clsLabel"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 231px" colSpan="3"></TD>
								<TD align="right"><asp:label id="lblLimit" runat="server" CssClass="bookingNormalLabel">Límite para Conciliar:</asp:label></TD>
								<TD width="5"></TD>
								<TD><asp:label id="lblLimitDate" runat="server" CssClass="clsLabel"></asp:label></TD>
							</TR>
							<TR>
								<TD style="WIDTH: 231px" colSpan="3"></TD>
								<TD align="right"><asp:label id="lblExchange" runat="server" CssClass="bookingNormalLabel"> Tipo de Cambio:</asp:label></TD>
								<TD width="5"></TD>
								<TD><asp:label id="lblMoneyExchange" runat="server" CssClass="clsLabel"></asp:label></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD colSpan="4" height="30"></TD>
				</TR>
				<TR>
					<TD colSpan="4">
						<TABLE id="Table3" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<TR>
								<TD colSpan="2">
									<TABLE id="Table6" cellSpacing="0" cellPadding="0" width="100%" border="0">
										<TR>
											<TD><asp:label id="lblReservationsList" runat="server" CssClass="bookingNormalLabel">Reservaciones por Conciliar</asp:label></TD>
											<TD align="right"><asp:label id="lblsTotalRva" runat="server" CssClass="clsLabel">Número de Reservaciones : </asp:label></TD>
											<TD width="5"></TD>
											<TD align="left"><asp:label id="lblTotalRva" runat="server" CssClass="bookingNormalLabel"></asp:label></TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
							<TR>
								<TD align="center" colSpan="2"><asp:label id="lblInvoiceConciliated" runat="server" CssClass="Validators" Height="100%" Visible="False">El Proceso De Conciliación No Puede Ser Finalizado Porque Esta Factura Ya Ha Sido Conciliada</asp:label></TD>
							</TR>
							<TR>
								<TD align="center" colSpan="2"><asp:label id="lblNoRvas" runat="server" CssClass="bookingNormalLabel">No hay facturas por Conciliar</asp:label></TD>
							</TR>
							<TR>
								<TD align="right" colSpan="2"></TD>
							</TR>
							<TR>
								<TD colSpan="2"><asp:datagrid id="dgReservations" runat="server" CssClass="DataGrid" BorderColor="WhiteSmoke"
										PageSize="15" AllowSorting="True" AutoGenerateColumns="False" Width="100%">
										<ItemStyle Font-Size="9px"></ItemStyle>
										<HeaderStyle CssClass="dgHeader"></HeaderStyle>
										<Columns>
											<asp:TemplateColumn>
												<ItemTemplate>
													<TABLE id="Table4" border="0" cellSpacing="0" cellPadding="0" width="100%">
														<TR>
															<TD><INPUT id="btnConciliarDg" class="btnShowItems" value="Corregir" type="button" name="btnConciliarDg"
																	runat="server"></TD>
														</TR>
														<TR>
															<TD>
																<DIV style="Z-INDEX: 996; POSITION: absolute; PADDING-BOTTOM: 3px; BACKGROUND-COLOR: white; PADDING-LEFT: 3px; PADDING-RIGHT: 3px;  PADDING-TOP: 3px; display:none;"
																	id="divToCorrectData" class="boxMsgleft" runat="server">
																	<TABLE id="tblDiv" border="0" cellSpacing="0" cellPadding="0" width="400">
																	    <tr><td class="modulo tituloModulo" colspan =4>
                                                                            <asp:Label ID="lblTitleData" runat="server" Text="Correccion de datos"></asp:Label></td></tr>
																		<TR class="ReservationsFillCell">
																			<TD >
																				<asp:label id="lblsDgNumRva" runat="server" CssClass="clsLabel">Reservación:</asp:label></TD>
																			<TD  width="5"></TD>
																			<TD >
																				<asp:label id="lblsDgCheckIn" runat="server" CssClass="clsLabel">Llegada:</asp:label></TD>
																			<TD >
																				<asp:label id="lblsDgCheckOut" runat="server" CssClass="clsLabel">Salida:</asp:label></TD>
																		</TR>
																		<TR class="ReservationsFillCell">
																			<TD class="dgAlternate">
																				<asp:label id="lblDgReservationID" runat="server" CssClass="bookingNormalLabel">NumRva</asp:label></TD>
																			<TD class="dgAlternate" width="5"></TD>
																			<TD class="dgAlternate">
																				<asp:label id="lblDgCheckIn" runat="server" CssClass="bookingNormalLabel">CheckIn</asp:label></TD>
																			<TD class="dgAlternate">
																				<asp:label id="lblDgCheckOut" runat="server" CssClass="bookingNormalLabel">CheckOut</asp:label></TD>
																		</TR>
																		<TR>
																			<TD></TD>
																			<TD style="HEIGHT: 19px" width="5"></TD>
																			<TD></TD>
																			<TD></TD>
																		</TR>
																		<TR>
																			<TD style="HEIGHT: 49px" align="right">
																				<asp:label id="lblhDgStatus" runat="server" CssClass="clsLabel">Estado :</asp:label></TD>
																			<TD style="HEIGHT: 49px" width="5"></TD>
																			<TD style="HEIGHT: 49px">
																				<asp:dropdownlist id="ddlDgCorrectionOptions" runat="server">
																					<asp:ListItem Selected="True" Value="1">No Llegó</asp:ListItem>
																					<asp:ListItem Value="2">Modificar Noches</asp:ListItem>
																				</asp:dropdownlist></TD>
																			<TD style="HEIGHT: 49px"></TD>
																		</TR>
																		<TR>
																			<TD height="10" colSpan="4" align="center"></TD>
																		</TR>
																		<TR id="TrCheckIn" runat="server">
																			<TD align="right">
																				<asp:label id="lblDgNewCheckIn" runat="server" CssClass="clsLabel">Llegada:</asp:label></TD>
																			<TD width="5"></TD>
																			<TD>
																				<asp:textbox id="txtDgNewCheckIn" runat="server" CssClass="textBox" Width="96px">mm/dd/yyyy</asp:textbox>
																				<asp:Literal id="litCalendar" runat="server"></asp:Literal></TD>
																			<TD></TD>
																		</TR>
																		<TR id="TrNights" runat="server">
																			<TD align="right">
																				<asp:label id="lblDgNewNights" runat="server" CssClass="clsLabel">Número de Noches:</asp:label></TD>
																			<TD width="5"></TD>
																			<TD>
																				<asp:textbox id="txtDgNewNights" runat="server" CssClass="textBox" Width="39px" MaxLength="3"></asp:textbox>
																				<asp:RangeValidator id="rvNights" runat="server" CssClass="Validators" Height="100%" Type="Integer"
																					MaximumValue="100" MinimumValue="1" ErrorMessage="[*]" ControlToValidate="txtDgNewNights"></asp:RangeValidator></TD>
																			<TD></TD>
																		</TR>
																		<TR>
																			<TD height="15" colSpan="4" align="left"><INPUT id="hdnDgCommissionDetailID" size="1" type="hidden" name="hdnDgCommissionDetailID"
																					runat="server"></TD>
																		</TR>
																		<TR>
																			<TD colSpan="4" align="center">
																				<TABLE id="Table5" border="0" cellSpacing="0" cellPadding="0" width="50%">
																					<TR>
																						<TD style="WIDTH: 150px">
																							<asp:button id="btnDgUpdate" runat="server" CssClass="buttonNew" Width="80px" Text="Actualizar"
																								CommandName="UpdateRvaInfo"></asp:button></TD>
																						<TD style="WIDTH: 150px"><INPUT style="WIDTH: 80px" id="btnHDgCancelar" class="buttonNew" value="Cancelar" type="button"
																								name="btnHDgCancelar" runat="server"></TD>
																					</TR>
																				</TABLE>
																			</TD>
																		</TR>
																	</TABLE>
																</DIV>
															</TD>
														</TR>
													</TABLE>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn>
												<ItemStyle HorizontalAlign="Center"></ItemStyle>
												<ItemTemplate>
													<asp:Label id=Label4 runat="server" CssClass="bookingNormalLabel" Text='<%# cInt(DataBinder.Eval(Container, "ItemIndex"))+1 %>'>
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn SortExpression="CustomerName" HeaderText="Reservaci&#243;n">
												<HeaderStyle ></HeaderStyle>
												<ItemStyle HorizontalAlign="Center"></ItemStyle>
												<ItemTemplate>
													<asp:Label id="lblNumRvaDg" runat="server"></asp:Label><BR>
													<asp:Label id="lblNameDg" runat="server" CssClass="clsLabelDataGrid"></asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="Tipo Tarifa">
												<ItemTemplate>
													<asp:Label id="lblRateTypeDg" runat="server"></asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn SortExpression="ReservationSourceCode" HeaderText="Fuente">
												<HeaderStyle></HeaderStyle>
												<ItemStyle HorizontalAlign="Center"></ItemStyle>
												<ItemTemplate>
													<asp:Label id="lblSourceDg" runat="server"></asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn SortExpression="AverageRate" HeaderText="Tarifa">
												<HeaderStyle ></HeaderStyle>
												<ItemStyle HorizontalAlign="Right"></ItemStyle>
												<ItemTemplate>
													<asp:Label id="lblAvRateDg" runat="server"></asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn SortExpression="CheckIn" HeaderText="Llegada">
												<HeaderStyle ></HeaderStyle>
												<ItemStyle HorizontalAlign="Center"></ItemStyle>
												<ItemTemplate>
													<asp:Label id="lblCheckInDg" runat="server"></asp:Label><br>
													<asp:Label CssClass="clsLabelDataGrid" id="lblNightsDg" runat="server"></asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn Visible="False" HeaderText="Total Reservaci&#243;n">
												<ItemStyle HorizontalAlign="Right"></ItemStyle>
												<ItemTemplate>
													<asp:Label id="lblRvaTotalDg" runat="server"></asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="Cargo">
												<ItemStyle HorizontalAlign="Right"></ItemStyle>
												<ItemTemplate>
													<asp:Label id=lblOurChargeDg runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.Status") <> Oz.BillingSystem.Common.Hotels.CommissionDetailStatus.Duplicated %>'>
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="TA">
												<ItemStyle HorizontalAlign="Right"></ItemStyle>
												<ItemTemplate>
													<asp:Label id=lblTADg runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.Status") <> Oz.BillingSystem.Common.Hotels.CommissionDetailStatus.Duplicated %>'>
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="com TA">
												<ItemStyle HorizontalAlign="Right"></ItemStyle>
												<ItemTemplate>
													<asp:Label id=lblComTADg runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.Status") <> Oz.BillingSystem.Common.Hotels.CommissionDetailStatus.Duplicated %>'>
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="Total">
												<ItemStyle HorizontalAlign="Right"></ItemStyle>
												<ItemTemplate>
													<asp:Label id=lblTotalDg runat="server" Visible='<%# DataBinder.Eval(Container, "DataItem.Status") <> Oz.BillingSystem.Common.Hotels.CommissionDetailStatus.Duplicated %>'>
													</asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn HeaderText="Estado">
												<ItemTemplate>
													<asp:Label id="lblStatusDg" runat="server"></asp:Label>
												</ItemTemplate>
											</asp:TemplateColumn>
										</Columns>
										<PagerStyle HorizontalAlign="Right" CssClass="labelBold" Mode="NumericPages"></PagerStyle>
									</asp:datagrid></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD colSpan="4" height="15"></TD>
				</TR>
				<TR>
					<TD align="left" colSpan="4"><asp:label id="lblOthers" runat="server" CssClass="bookingNormalLabel">Otros</asp:label></TD>
				</TR>
				<TR>
					<TD colSpan="4"><asp:datagrid id="dgOthers" runat="server" CssClass="DataGrid" BorderColor="WhiteSmoke" AutoGenerateColumns="False"
							Width="100%" BackColor="White">
							<HeaderStyle CssClass="dgHeader"></HeaderStyle>
							<Columns>
								<asp:TemplateColumn>
									<ItemStyle CssClass="txtPlanDetailHotelRatesList"></ItemStyle>
									<ItemTemplate>
										<asp:Label id=Label1 runat="server" CssClass="bookingNormalLabel" Text='<%# cInt(DataBinder.Eval(Container, "ItemIndex"))+1 %>'>
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Concepto">
									<ItemTemplate>
										<asp:Label id="lblConcept" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn DataField="Subtotal" HeaderText="SubTotal" DataFormatString="{0:C}">
									<HeaderStyle Font-Bold="True" HorizontalAlign="Right" CssClass="txtPlanDetailHotelRatesList"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" CssClass="txtPlanDetailHotelRatesList"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="Taxes" HeaderText="Impuestos" DataFormatString="{0:C}">
									<HeaderStyle Font-Bold="True" HorizontalAlign="Right"  CssClass="txtPlanDetailHotelRatesList"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" CssClass="txtPlanDetailHotelRatesList"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="Total" HeaderText="Total" DataFormatString="{0:C}">
									<HeaderStyle Font-Bold="True" HorizontalAlign="Right" CssClass="txtPlanDetailHotelRatesList"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right" CssClass="txtPlanDetailHotelRatesList"></ItemStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD align="center" colSpan="4" height="15"></TD>
				</TR>
				<TR>
					<TD align="right" colSpan="4">
						<TABLE id="tbTotales" cellSpacing="0" cellPadding="0" border="0" runat="server">
							<TR>
								<TD align="right" style="HEIGHT: 11px"><asp:label id="lblsTotalCommissions" runat="server" CssClass="clsLabel">Total de Comisiones :</asp:label></TD>
								<TD width="5" style="HEIGHT: 11px"></TD>
								<TD align="right" style="HEIGHT: 11px"><asp:label id="lblTotalCommissions" runat="server" CssClass="bookingNormalLabel"></asp:label></TD>
							</TR>
							<TR>
								<TD align="right"><asp:label id="lblsTotalOthers" runat="server" CssClass="clsLabel">Total de Otros :</asp:label></TD>
								<TD width="5"></TD>
								<TD align="right"><asp:label id="lblTotalOthers" runat="server" CssClass="bookingNormalLabel"></asp:label></TD>
							</TR>
							<TR>
								<TD></TD>
								<TD width="5" height="15"></TD>
								<TD height="15"></TD>
							</TR>
							<TR>
								<TD align="right"><asp:label id="lblsSubtotal" runat="server" CssClass="clsLabel">Subtotal :</asp:label></TD>
								<TD width="5" height="15"></TD>
								<TD align="right"><asp:label id="lblSubtotal" runat="server" CssClass="bookingNormalLabel"></asp:label></TD>
							</TR>
							<TR>
								<TD align="right"><asp:label id="lblsTaxes" runat="server" CssClass="clsLabel">Impuesto :</asp:label></TD>
								<TD width="5" height="15"></TD>
								<TD align="right"><asp:label id="lblTaxes" runat="server" CssClass="bookingNormalLabel"></asp:label></TD>
							</TR>
							<TR>
								<TD align="right" height="15"></TD>
								<TD width="5" height="15"></TD>
								<TD align="right" height="15"></TD>
							</TR>
							<TR>
								<TD align="right"><asp:label id="lblsTotalToPay" runat="server" CssClass="clsLabel">Total a Pagar :</asp:label></TD>
								<TD width="5"></TD>
								<TD align="right"><asp:label id="lblTotalToPay" runat="server" CssClass="bookingNormalLabel"></asp:label></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD align="right" colSpan="4" height="60"><INPUT type="button" value="Imprimir" onclick="window.print();" class=buttonNew></TD>
				</TR>
				<TR>
					<TD align="right" colSpan="4">
						<TABLE id="Table7" cellSpacing="0" cellPadding="0" width="300" border="0">
							<TR>
								<TD><asp:button id="btnUpdateConciliation" runat="server" CssClass="Button" Width="197px" Text="Guardar Conciliación"
										ToolTip="Este botón no finaliza la conciliación, solo guarda las actualizaciones que se han hecho hasta el momento."></asp:button></TD>
								<TD><INPUT class="Button" id="btnFinalizeConciliation" visible="false" style="WIDTH: 197px"
										onclick="javascript:showConfirm();" type="button" value="Terminar Conciliación" name="btnFinalizeConciliationHtml"
										runat="server"></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD align="right" colSpan="4" height="20">
						<TABLE id="Table9" cellSpacing="0" cellPadding="0" width="70%" border="0">
							<TR>
								<TD>
									<DIV id="FinishConciliation" style="POSITION: absolute;display:none; " class="dgAlternate">
										<TABLE style="BORDER-BOTTOM: #cbdced 1px solid; BORDER-LEFT: #cbdced 1px solid; BORDER-TOP: #cbdced 1px solid; BORDER-RIGHT: #cbdced 1px solid"
											cellSpacing="0" cellPadding="1" width="100%" border="0">
											<TBODY>
												<TR>
													<TD class="dgheader" align="center" colSpan="3" style="HEIGHT: 13px"><asp:label id="lblConfirmTitle" runat="server" BackColor="Transparent" Font-Bold="True" EnableViewState="False">Confirmar Fin de Conciliación</asp:label></TD>
												</TR>
												<TR>
													<TD class="dgAlternate" style="HEIGHT: 31px" align="center" colSpan="3"><asp:label id="lblConfirmMsg" runat="server" CssClass="bookingNormalLabel" EnableViewState="False">¿Está seguro que desea terminar la conciliación?<br>Ya no podrá hacer cambios.</asp:label></TD>
												</TR>
												  <tr>
                                                    <td align="center" colspan="3">
                                                         <asp:RadioButton id="rbtnPaymentPref" runat="server" Text="Metodos de pago del cliente:" Checked="true" GroupName="PaymentGroup" onclick="switch_visible_ddlP(false)" style="margin-right:22px;" />
                                                    
                                                        <asp:RadioButton id="rbtnPaymentList" runat="server" Text="Otros metodos de pago:" GroupName="PaymentGroup" onclick="switch_visible_ddlP(true)" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3" align="center" style="width:420px;">
                                                        <div id="divPaymentPref" runat="server"  style="display:inline-block; text-align:left;" >
                                                            <br />
                                                            Métodos de pago: <asp:DropDownList ID="ddlPaymentPref" runat="server" ></asp:DropDownList>
                                                        </div>
                                                        <div id="divPaymentList" runat="server" style="display:none; text-align:left;">
                                                            <br />
                                                            <table>
                                                            <tr>
                                                                <td align="right">Métodos de pago:</td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlPaymentList" runat="server" onchange="val_ddlPaymentPref()"></asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right">Otro:</td>
                                                                <td><asp:TextBox ID="txtOther" runat="server" Enabled="false" style="background-color:#bbbbbb"></asp:TextBox><div id="rqfvOther" style="display:none;color:#FF0000;">*</div></td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right">Número de cuenta:</td>
                                                                <td><asp:TextBox ID="txtAccountNumber" runat="server" Enabled="false" style="background-color:#bbbbbb" ></asp:TextBox><div id="rqfvAccount" style="display:none;color:#FF0000;">*</div></td>
                                                            </tr>
                                                            </table>
                                                        </div>
                                                    </td>
                                                </tr>
												<TR>
													<TD class="textBox" align="center" width="50%"><asp:linkbutton id="hplYes" OnClientClick="return valPaymentMethod()" runat="server" CssClass="dgLink">Si</asp:linkbutton></TD>
													<TD align="right"></TD>
													<TD class="textBox" align="center" width="50%"><asp:hyperlink id="hplNo" runat="server" CssClass="dgLink" NavigateUrl="javascript:hideConfirm();">No</asp:hyperlink></TD>
												</TR>
											</TBODY>
										</TABLE>
									</DIV>
								</TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD colSpan="4" height="20"></TD>
				</TR>
				<TR>
					<TD align="left" colSpan="4">
						<TABLE id="Table10" cellSpacing="1" cellPadding="5" width="300" border="0">
							<TR>
								<TD width="15"></TD>
								<TD><asp:linkbutton id="lnkToConciliate" runat="server" CssClass="dgLink" EnableViewState="False">Regresar a Facturas Pendientes de Conciliar</asp:linkbutton></TD>
							</TR>
							<TR>
								<TD width="15"></TD>
								<TD>
									<asp:HyperLink id="hplinvoicebyperiod" runat="server" CssClass="dglink" Visible="false">Ir a Facturas por periodo</asp:HyperLink></TD>
							</TR>
							<TR>
								<TD width="15"></TD>
								<TD>
									<asp:HyperLink id="hypTaskList" runat="server" CssClass="dgLink" EnableViewState="False" Visible =false>
												Ir a Lista de Tareas
												</asp:HyperLink></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD></TD>
					<TD align="right"></TD>
				</TR>
				<TR>
					<TD align="center" colSpan="4" height="15"></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
