<%@ Register TagPrefix="uc1" TagName="CtrlPackageRubros" Src="CtrlPackageRubros.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctlMensajes" Src="ctlMensajes.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrRateAplicationPackage" Src="ctrRateAplicationPackage.ascx" %>
<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ctrlPackage.ascx.vb" Inherits="RateManager.ctrlPackage" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="uc1" TagName="ctrlMensajesControl" Src="ctrlMensajesControl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrlPlanFaresExcPackage" Src="CtrlPlanFaresExcPackage.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrRateAplication" Src="ctrRateAplication.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlPlanFares" Src="ctrlPlanFares.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrlPlanFaresExc" Src="CtrlPlanFaresExc.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrPortal" Src="ctrPortal.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrlIdioma" Src="CtrlIdioma.ascx" %>
<%@ Import NameSpace = "RateManager" %>
<%
	response.write(LoadVariables)
%>

<script type="text/javascript" >

    function AddDatesLocal(t1, t2, lst, tD, msg, OverlapMsg) {
        var txt1 = document.getElementById(t1);
        var txt2 = document.getElementById(t2);
        var txtDates = document.getElementById(tD);
        var e = document.getElementById(lst);
        var xDia, xMes, xYear, i, sw;

        if (txt1 && txt2 && e && txtDates) {

            xDia = txt1.value.substring(3, 5);
            xMes = txt1.value.substring(0, 2);
            xMes = xMes - 1;
            xYear = txt1.value.substring(6, 10);
            //xYear = xYear - 1;	
            var NewDt1 = new Date(xYear, xMes, xDia);



            xDia = txt2.value.substring(3, 5);
            xMes = txt2.value.substring(0, 2);
            xMes = xMes - 1;
            xYear = txt2.value.substring(6, 10);
            //xYear = xYear - 1;	
            var NewDt2 = new Date(xYear, xMes, xDia);

            sw = false;

            //Set 1 day in milliseconds
            var one_day = 1000 * 60 * 60 * 24;

            //Calculate difference btw the two dates, and convert to days		
            one_day = (NewDt2.getTime() - NewDt1.getTime()) / (one_day);

            NewDt1 = Date.parse(NewDt1);
            NewDt2 = Date.parse(NewDt2);
            var conf;
            if (eval(one_day) <= 3) {
                conf = window.confirm(updateSeasson);
                //alert(conf);
                if (!conf) {
                    sw = true;
                    return;
                }
            }

            if (NewDt2 < NewDt1) {
                alert(msg);
                sw = true;
                return;
            }
            for (i = 1; i <= e.options.length - 1 && sw == false; i++) {
                xDia = e.options[i].value.substring(3, 5);
                xMes = e.options[i].value.substring(0, 2);
                xMes = xMes - 1;
                xYear = e.options[i].value.substring(6, 10);
                xYear = xYear;
                var f1 = new Date(xYear, xMes, xDia);

                f1 = Date.parse(f1);

                xDia = e.options[i].value.substring(14, 16);
                xMes = e.options[i].value.substring(11, 13);
                xMes = xMes - 1;
                xYear = e.options[i].value.substring(17, 21);
                xYear = xYear;
                var f2 = new Date(xYear, xMes, xDia);
                f2 = Date.parse(f2);
                if (((NewDt1 >= f1 && NewDt1 <= f2) || ((NewDt2 >= f1) && NewDt2 <= f2)) || ((f1 >= NewDt1 && f1 <= NewDt2) || ((f2 >= NewDt1) && f2 <= NewDt2))) {
                    sw = true;
                }
            }

            if (!sw) {
                var texto = txt1.value + "-" + txt2.value;
                var optionObject = new Option(texto, texto);
                var optionRank = e.options.length;
                e.options[optionRank] = optionObject;
                txtDates.value = txtDates.value + "$" + texto;
            }
            else {
                alert(OverlapMsg);
            }


        }
    }
</script>

        <iframe id="gToday:normal:agenda.js" style="Z-INDEX: 999; LEFT: -500px; POSITION: absolute; TOP: -500px" 
            name="gToday:normal:agenda.js"
            src='<%=GeRequestApplicationPath(string.concat("/Calendar/",portalculture.getculture().Name.Substring(0, 2).ToLower(),"/ipopeng2.htm" )) %>'
            frameborder="0" width="174" scrolling="no" height="189">
        </iframe>

<TABLE class="Form" width="99%" border="0" class="Form" >
	
	<TR>
		<TD align="right"><asp:label id="lblRatecode" EnableViewState="False" CssClass="clsLabel" runat="server">Package Code</asp:label></TD>
		<TD><asp:textbox id="txtRateCode" runat="server" MaxLength="4" Width="56px"></asp:textbox><asp:requiredfieldvalidator id="rfvCodePack" runat="server" ErrorMessage="Campo Requerido" ControlToValidate="txtRateCode"
				Display="Dynamic"></asp:requiredfieldvalidator></TD>
	</TR>
	<TR>
		<TD align="right"><asp:label id="lblname" EnableViewState="False" CssClass="clslabel" runat="server">Nombre</asp:label></TD>
		<TD><uc1:ctrlidioma id="txtShortDescription" runat="server" IsMultiline="false"></uc1:ctrlidioma></TD>
	</TR>
	<TR>
		<TD align="right"><asp:label id="lblDescripcion" EnableViewState="False" CssClass="clsLabel" runat="server">Descripción</asp:label></TD>
		<TD><uc1:ctrlidioma id="txtDescripcion" runat="server"></uc1:ctrlidioma></TD>
	</TR>
	<TR>
		<TD vAlign="top" align="right"><asp:label id="lblImagen" EnableViewState="False" CssClass="clsLabel" runat="server">Imagen</asp:label></TD>
		<TD vAlign="top">
			<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
				<TR>
					<TD vAlign="top"><INPUT id="fileImagen" type="file" runat="server"></TD>
					<TD><asp:image id="ImagenHabitacion" CssClass="clsBackGround" runat="server" Width="75px" Visible="False"
							Height="75px"></asp:image></TD>
				</TR>
			</TABLE>
		</TD>
	</TR>
	<TR>
		<TD align="right"><asp:label id="lblStartDate" EnableViewState="False" CssClass="clsLabel" runat="server">Start Date</asp:label></TD>
		<TD><asp:textbox id="txtStart" runat="server" MaxLength="10" Width="96px" Columns="10"></asp:textbox><A hideFocus onclick="if(self.gfPop)gfPop.fPopCalendar(document.getElementById('<%=txtend.ClientId%>'), document.getElementById('<%=txtStart.ClientId%>'),IniDate());return false;" href="javascript:void(0)" ><IMG class=PopcalTrigger alt="" 
      src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>' align=absMiddle 
      border=0> </A>
		</TD>
	</TR>
	<TR>
		<TD align="right"><asp:label id="lblEndDate" EnableViewState="False" CssClass="clsLabel" runat="server">End Date</asp:label></TD>
		<TD><asp:textbox id="txtEnd" runat="server" MaxLength="10" Width="96px" Columns="10"></asp:textbox><A hideFocus onclick="if(self.gfPop)gfPop.fPopCalendar1(document.getElementById('<%=txtend.ClientId%>'),IniDate());return false;" href="javascript:void(0)" ><IMG class=PopcalTrigger alt="" 
      src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>' align=absMiddle 
      border=0> </A>
		</TD>
	</TR>
	<TR>
		<TD align="right"><asp:label id="lblRule" EnableViewState="False" CssClass="clsLabel" runat="server">Rule</asp:label></TD>
		<TD><asp:dropdownlist id="ddlRules" runat="server"></asp:dropdownlist></TD>
	</TR>
	<TR>
		<TD align="right"><asp:label id="lblMaxAdultos" EnableViewState="False" CssClass="clsLabel" runat="server">Max Adultos</asp:label></TD>
		<TD><asp:textbox id="txtMaxAd" runat="server" MaxLength="4" Width="48px"></asp:textbox><asp:requiredfieldvalidator id="rfvMaxAdultos" runat="server" ErrorMessage="Campo Requerido" ControlToValidate="txtMaxAd"
				Display="Dynamic"></asp:requiredfieldvalidator></TD>
	</TR>
	<TR>
		<TD align="right"><asp:label id="lblMaxChildren" EnableViewState="False" CssClass="clsLabel" runat="server">Max Children</asp:label></TD>
		<TD><asp:textbox id="txtMaxChild" runat="server" MaxLength="4" Width="48px"></asp:textbox><asp:requiredfieldvalidator id="rfvMaxChild" runat="server" ErrorMessage="Campo Requerido" ControlToValidate="txtMaxChild"
				Display="Dynamic"></asp:requiredfieldvalidator></TD>
	</TR>
	<TR id="trHeadProm1">
		<TD align="right"><asp:label id="lblNoches" EnableViewState="False" runat="server">Noches</asp:label></TD>
		<TD colSpan="1"><asp:textbox id="txtDaysFree" CssClass="textbox" runat="server" MaxLength="2" Width="48px">1</asp:textbox><asp:requiredfieldvalidator id="rfvNoches" runat="server" ErrorMessage="Campo Requerido" ControlToValidate="txtDaysFree"
				Display="Dynamic"></asp:requiredfieldvalidator></TD>
	</TR>
	<tr>
		<td align="right" colspan="2"><A class="showOptions" id="aPaqueteRubro" href="javascript:;" runat="server">Paquete 
				Armado</A>
		</td>
	</tr>
	<tr>
		<td colspan="2">
			<DIV id="dvPaqueteRubro" style="padding-right: 3px; display: none; padding-left: 3px; padding-bottom: 3px; padding-top: 3px; position: relative"
				ms_positioning="FlowLayout">
				<uc1:CtrlPackageRubros id="PackageRubros" runat="server"></uc1:CtrlPackageRubros>
			</DIV>
		</td>
	</tr>
	<TR>
		<TD align="right"><asp:label id="lblRateApply" EnableViewState="False" CssClass="clslabel" runat="server">Disponible en:</asp:label></TD>
		<TD colSpan="1"><asp:checkbox id="chkGDS" CssClass="clslabel" runat="server" Checked="True" Text="GDS"></asp:checkbox><asp:checkbox id="chkPortal" CssClass="clslabel" runat="server" Text="Portal"></asp:checkbox><asp:checkbox id="chkUnipantalla" CssClass="clslabel" runat="server" Text="Unipantalla"></asp:checkbox><asp:checkbox id="chkADS" CssClass="clslabel" runat="server" Text="ADS"></asp:checkbox></TD>
	</TR>
	<tr id="trApplyGDS">
	    <td align="right"><asp:label id="lblAplicaGDS" EnableViewState="False" CssClass="clslabel" runat="server">Aplica Para GDS</asp:label></td>
		<td align="center">
			<TABLE class="datagrid" cellSpacing="1" cellPadding="1" width="100%" border="0">
				<tr>
					<td width="50%"><asp:checkbox id="chkGDSAmadeus" runat="server" Checked="True" Text="Amadeus"></asp:checkbox>&nbsp;
						<asp:checkbox id="chkGDSGalileo" runat="server" Checked="True" Text="Galileo"></asp:checkbox>&nbsp;
						<asp:checkbox id="chkGDSSabre" runat="server" Checked="True" Text="Sabre"></asp:checkbox>&nbsp;
						<asp:checkbox id="chkGDSWorldSpan" runat="server" Checked="True" Text="WorldSpan"></asp:checkbox></td>
					<td></td>
				</tr>
			</TABLE>
		</td>
	</tr>	
	
	<TR id="trPortal" runat="server">	    
		<TD colspan="2">
		    <table width=100%>
		    <tr><td class="dgitem"><asp:label id="lblPortal" runat="server">Portales</asp:label>:</td></tr>
		    <tr><td><uc1:ctrportal id="ctrPortal1" runat="server"></uc1:ctrportal></td></tr>
		    </TABLE>
		    
			
		</TD>
	</TR>
	<TR>
		<TD colSpan="2"><asp:rangevalidator id="RVNochesgratis" CssClass="validators" runat="server" ErrorMessage="Noche gratis debe ser Numerico"
				ControlToValidate="txtDaysFree" Display="Dynamic" Type="Integer" MaximumValue="99" MinimumValue="1"></asp:rangevalidator><asp:rangevalidator id="rvAdultos" CssClass="validators" runat="server" ErrorMessage="Adultos deben ser Numerico"
				ControlToValidate="txtMaxAd" Display="Dynamic" Type="Integer" MaximumValue="99" MinimumValue="0"></asp:rangevalidator><asp:rangevalidator id="rvalNinios" CssClass="validators" runat="server" ErrorMessage="Niños deben ser Numerico"
				ControlToValidate="txtMaxChild" Display="Dynamic" Type="Integer" MaximumValue="99" MinimumValue="0"></asp:rangevalidator><asp:customvalidator id="cvMaxPeople" runat="server" ErrorMessage="La Habitación {0}, Acepta Como Máximo {1} Adultos, {2} Niños y Como Maximo de Personas {3}"
				Display="Dynamic"></asp:customvalidator></TD>
	</TR>
	<% response.write("<script>  onCheckBoxesClick('" & chkPortal.Clientid & "','" & trPortal.clientid & "');</script>")%>
	<% response.write("<script>  onCheckBoxesClick('" & chkGDS.Clientid & "','trApplyGDS');</script>")%>
</TABLE>
<table width="100%" border="0" cellpadding="0" cellspacing="0"  class="Form">
	<tr>
		<td  class="dgitem" align="center" colSpan="3"><span id="lblSeasons" runat="server">Seasons</span></td>
	</tr>
	<tr>
		<td colSpan="3">
		<div id="dgPager" align="right"></div>
		<asp:datagrid id="dgRooms" CssClass="datagrid" runat="server" Width="99%" ShowFooter="True" AutoGenerateColumns="False">
				<FooterStyle HorizontalAlign="Right"></FooterStyle>
				<SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
				<AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
				<ItemStyle CssClass="dgItem"></ItemStyle>
				<HeaderStyle CssClass="dgHeader"></HeaderStyle>
				<Columns>
					<asp:BoundColumn DataField="Codigohabitacion"></asp:BoundColumn>
					<asp:BoundColumn Visible="False" DataField="idtipohabitacion_hotel"></asp:BoundColumn>
					<asp:BoundColumn DataField="FechaInicia" HeaderText="Inicio"></asp:BoundColumn>
					<asp:BoundColumn DataField="FechaFinaliza" HeaderText="Fin"></asp:BoundColumn>
					<asp:BoundColumn DataField="tarifaAdulto" HeaderText="Tarifa 1 Adulto" ItemStyle-HorizontalAlign="Right"></asp:BoundColumn>
					<asp:TemplateColumn HeaderText="Eliminar">
						<ItemTemplate>
							<asp:CheckBox ID="chkDelete" Runat="server" Text=""></asp:CheckBox>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn Visible="False" DataField="idTarifa"></asp:BoundColumn>
					<asp:TemplateColumn HeaderText="Editar Tarifa">
						<ItemTemplate>
							<asp:CheckBox ID="chkEditOcc" Runat="server" Text=""></asp:CheckBox>
							<asp:TextBox Runat="server" id="tPrices" style="DISPLAY: none"></asp:TextBox>
							<asp:TextBox Runat="server" id="tPricesE" style="DISPLAY: none"></asp:TextBox>
							<asp:TextBox Runat="server" id="txtPriceModified" style="DISPLAY: none"></asp:TextBox>
							<asp:TextBox Runat="server" id="txtPriceEModified" style="DISPLAY: none"></asp:TextBox>
							<asp:TextBox Runat="server" id="txtTipoPrecio" style="DISPLAY: none"></asp:TextBox>
							<asp:TextBox Runat="server" id="txtTipoPrecioModified" style="DISPLAY: none"></asp:TextBox>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn DataField="Excepciones" Visible="False"></asp:BoundColumn>
					<asp:BoundColumn DataField="packageType" Visible="False"></asp:BoundColumn>
					<asp:BoundColumn DataField="Precio" Visible="False"></asp:BoundColumn>
				</Columns>
				<PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
					Position="Top" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
			</asp:datagrid>
			
		</td>
	</tr>
	<tr>
		<td align="right" colSpan="3"></td>
	</tr>
	<TR>
		<TD align="right" colSpan="3"><asp:label id="lblmoney" CssClass="bookingnormallabel" runat="server"></asp:label></TD>
	</TR>
	<TR>
		<TD colSpan="3"><asp:checkboxlist id="chkHabitaciones" runat="server" RepeatDirection="Horizontal"></asp:checkboxlist></TD>
	</TR>
	<tr>
		<td align="right" colSpan="3"><A class="showOptions" id="ADetails" href="javascript:;" runat="server">Details</A></td>
	</tr>
	<tr>
		<td colSpan="3">
			<DIV id="divRoomsDetails" style="padding-right: 3px; display: none; padding-left: 3px; padding-bottom: 3px; padding-top: 3px; position: relative"
				ms_positioning="FlowLayout">
				<table id="bookingcontainer" align="right" >
					<tr class="trTitle">
						<td width="500"><asp:label id="lblRoomsNames" EnableViewState="False" Runat="server">Habitaciones:</asp:label></td>
						<td align="right" width="10" align=center><img id="imgclose" style="CURSOR: hand" src="../Images/close.png" align="right" runat="server"></td>
					</tr>
					<tr>
						<td colSpan="2">
							<div id="dvRooms" runat="server"></div>
						</td>
					</tr>
				</table>
			</DIV>
		</td>
	</tr>
	<TR>
		<TD><asp:label id="lblPrice" EnableViewState="False" CssClass="clsLabel" runat="server"> Price</asp:label></TD>
		<TD colSpan="2"><asp:textbox id="txtPrecio" runat="server" MaxLength="8" Columns="8"></asp:textbox><asp:dropdownlist id="ddlTypePrice" runat="server">
				<asp:ListItem Value="By Night">By Night</asp:ListItem>
				<asp:ListItem Value="Total">Total</asp:ListItem>
			</asp:dropdownlist><asp:rangevalidator id="Rangevalidator1" CssClass="validators" runat="server" ErrorMessage="Precio debe ser Numerico"
				ControlToValidate="txtPrecio" Display="Dynamic" Type="Currency" MaximumValue="99999.99" MinimumValue="0.01"></asp:rangevalidator>
			<asp:radiobutton id="RdbPaquete" runat="server" Checked="True" Text="Por Paquete" GroupName="TipoPaquete"></asp:radiobutton><asp:radiobutton id="RdbPersona" runat="server" Text="Por Persona" GroupName="TipoPaquete"></asp:radiobutton><asp:radiobutton id="RdbOcupacion" runat="server" Text="Por Ocupación" GroupName="TipoPaquete"></asp:radiobutton></TD>
	</TR>
	<tr>
		<td colSpan="3">
			<div class="bookingnormallabel" id="dvtitleEdit"></div>
		</td>
	</tr>
</table>
<TABLE cellSpacing="0" cellPadding="0" width="100%" border="0" class="Form">
	<tr>
		<td vAlign="top">
			<TABLE cellSpacing="1" cellPadding="1" width="100%" border="0">
				<TR>
					<TD align="center">
						<div id="dvPrices" style="DISPLAY: none">
							<TABLE id="tblTAB2" cellSpacing="0" cellPadding="0" width="100%" border="0">
								<TR>
									<TD align="center" colSpan="2"></TD>
								</TR>
								<TR>
									<TD class="tabselected" id="TdPricing" onmouseover="javascript:this.style.cursor='pointer';"
										onclick="javascript:optionSw('1P');" align="center" width="33.33%"><asp:label id="lblPricingNE" EnableViewState="False" runat="server" Font-Size="XX-Small">[Pricing]</asp:label></TD>
									<TD class="tab" id="TdPricingE" onmouseover="javascript:this.style.cursor='pointer';"
										onclick="javascript:optionSw('1E');" align="center" width="33.33%"><asp:label id="lblPricingExc" EnableViewState="False" runat="server" Font-Size="XX-Small">[Pricing Exception]</asp:label></TD>
								</TR>
							</TABLE>
							<DIV id="divA2" style="MIN-HEIGHT: 100px; WIDTH: 100%" align="center" ms_positioning="FlowLayout">
								<div id="tblPrices"></div>
							</DIV>
							<DIV id="divB2" style="DISPLAY: none; MIN-HEIGHT: 100px; WIDTH: 100%" align="center"
								ms_positioning="FlowLayout"><asp:checkbox id="chk7" CssClass="txtSmallText" runat="server" Text="Dom"></asp:checkbox><asp:checkbox id="chk1" CssClass="txtSmallText" runat="server" Text="Lun"></asp:checkbox><asp:checkbox id="chk2" CssClass="txtSmallText" runat="server" Text="Mar"></asp:checkbox><asp:checkbox id="chk3" CssClass="txtSmallText" runat="server" Text="Mie"></asp:checkbox><asp:checkbox id="chk4" CssClass="txtSmallText" runat="server" text="Jue"></asp:checkbox><asp:checkbox id="chk5" CssClass="txtSmallText" runat="server" Text="Vie"></asp:checkbox><asp:checkbox id="chk6" CssClass="txtSmallText" runat="server" Text="Sab"></asp:checkbox>
								<div id="tblPricesExc"></div>
							</DIV>
						</div>
						<input id="btnAddOccRate" style="DISPLAY: none" type="button" class ="ButtonNew"  value="Agregar Ocupación"
							name="btnAddOccRate" runat="server"> <input id="btnCancelOccRate" class="ButtonNew"  style="DISPLAY: none" type="button" value="Cancelar" name="btnCancelOccRate"
							runat="server">
					</TD>
				</TR>
			</TABLE>
		</td>
		<td vAlign="top" align="right" width="25%">
			<div>
			    <table cellpadding=0 cellspacing=0 border=0 style="width:100%;" >
			        <tr><td style="text-align:right; padding-right:4px; ">
			<asp:label id="lblODesde" EnableViewState="False" CssClass="clslabel" runat="server">Desde:</asp:label></td><td><asp:textbox id="txtDateFrom" CssClass="textbox" runat="server" MaxLength="10" Width="84px" Columns="10"></asp:textbox><A hideFocus onclick="if(self.gfPop)gfPop.fPopCalendar(document.getElementById('<%response.write(txtDateTo.ClientId)%>'),document.getElementById('<%response.write(txtDateFrom.ClientId)%>'));return false;" href="javascript:void(0)" ><IMG class=PopcalTrigger alt="" 
      src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>' align=absMiddle 
      border=0></A></td><td><asp:image style="CURSOR: pointer" id="imgAddDate" runat="server" 
                                ImageUrl="../Images/agregafecha.jpg" Width="34px"></asp:image></td></tr>
			        <tr><td style="text-align:right; padding-right:4px;">
				<asp:label id="lblOHasta" EnableViewState="False" CssClass="clsLabel" runat="server">Hasta:</asp:label></td><td><asp:textbox id="txtDateTo" CssClass="textbox" runat="server" MaxLength="10" Width="84px" Columns="10"></asp:textbox><A hideFocus onclick="if(self.gfPop)gfPop.fPopCalendar1(document.getElementById('<%response.write(txtDateTo.ClientId)%>'));return false;" href="javascript:void(0)" ><IMG class=PopcalTrigger alt="" 
      src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>' align=absMiddle 
      border=0></A></td><td style=" vertical-align:bottom; height:25px;">
			                &nbsp;</td></tr>
			<tr><td ></td><td></td><td>
			<asp:image id="imgDeleteDate" style="CURSOR: pointer" runat="server" ImageUrl="../Images/eliminarfecha.jpg"></asp:image></td></tr>
			    </table>
			<br>
				<A hideFocus onclick="if(self.gfPop)gfPop.fPopCalendar(document.getElementById('<%response.write(txtDateTo.ClientId)%>'),document.getElementById('<%response.write(txtDateFrom.ClientId)%>'));return false;" href="javascript:void(0)" ></A><br>
			</div>
			</td>
		<td vAlign="top" width="25%">
			<div><asp:listbox id="lstDates" EnableViewState="True" CssClass="clslabel" runat="server" Width="190px"
					Height="150px" Rows="3"></asp:listbox></div>
		</td>
	</tr>
	<tr>
		<td colSpan="3"><asp:label id="lblDateErrorSign" EnableViewState="False" CssClass="Validators" runat="server"
				Visible="False">Fecha Invalida</asp:label></td>
	</tr>			
    <% If Me.edicion AndAlso (Not Me.txtDescripcion.Published OrElse Not Me.txtShortDescription.Published) Then%>
    <tr>
        <td colspan="3">
            <span class="validators"><%=RateManager.PortalCulture.GetString(If(CType(Me.Page, RateManager.PaginaBase).IsSupervisor, "01365", "01392"))%></span>
        </td>
    </tr>
    <% end if %>
	
</TABLE>
<asp:label id="lblDateError" EnableViewState="False" CssClass="Validators" runat="server" Visible="False">El rango de fecha especificado se traslapa con una tarifa existente en el plan seleccionado, verifique las fechas</asp:label><asp:textbox id="txtFechas" style="DISPLAY: none" runat="server"></asp:textbox><asp:textbox id="txtPrices" style="DISPLAY: none" runat="server"></asp:textbox><asp:textbox id="txtPricesE" style="DISPLAY: none" runat="server"></asp:textbox><input id="iChkEditId" style="DISPLAY: none" type="text" name="iChkEditId">
<input id="itxtPriceEdit" style="DISPLAY: none" type="text" name="itxtPriceEdit">
<input id="itxtPriceEEdit" style="DISPLAY: none" type="text" name="itxtPriceEEdit">
<input id="itxtCodigoEdit" style="DISPLAY: none" type="text" name="itxtCodigoEdit">
<input id="itxtTipoPaqueteEdit" style="DISPLAY: none" type="text" name="itxtTipoPaqueteEdit">
<input id="itxtTipoPrecio" type="text" name="itxtTipoPrecio" style="DISPLAY: none" runat="server">
<script>

</script>
