<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ctrRateAplication.ascx.vb" Inherits="RateManager.ctrRateAplication" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Import Namespace="RateManager" %>

<%@ Register TagPrefix="uc1" TagName="CtrlIdioma" Src="CtrlIdioma.ascx" %>

<script src="../Pages/Scripts/jquery.min.js" type="text/javascript"></script>

<style type="text/css">
    input.currency {
        font-family: Arial, Helvetica, sans-serif;
        font-size: 12px;
        text-align: right;
        width: 80px;
    }
</style>

<script type="text/javascript">

    $(document).ready(function () {
        $('#<%= Me.ddlrateplans.ClientId%>').change(function () {
            rate = $(this).find('option:selected').val();
            $(ratesCurrencies).each(function () {
                if (this.ratePlan === rate) { $('span.currency').html(this.currency); }
            });
        });
        $('#<%= Me.ddlrateplans.ClientId%>').change();

        var list = $('#<%= Me.ddlShowRates.ClientId %>');

        list.change(function () {
            var validators = new Array();
            validators.push({ id: '<%= Me.reqAdultFare.ClientID %>', hide: true });
            validators.push({ id: '<%= Me.reqChildFare.ClientID %>', hide: true });
            validators.push({ id: '<%= Me.reqTeenFare.ClientID %>', hide: true });
            validators.push({ id: '<%= Me.reqExtraAdultPrice.ClientID %>', hide: false });
            validators.push({ id: '<%= Me.reqExtraChildPrice.ClientID %>', hide: false });
            validators.push({ id: '<%= Me.reqExtraTeenPrice.ClientID %>', hide: false });

            FireShowSelectRates(this, 'pnlTarifas', 'DivRates', validators);
        });
        list.find('option:selected').removeAttr('selected');
        list.find('option:eq(' + rateModeView.toString() + ')').attr('selected', 'selected');
        list.change();
    });


    function DeleteDate(lst, tD) {
        var e = document.getElementById(lst);
        var txtDates = document.getElementById(tD);
        if (e && e.selectedIndex > 0 && txtDates) {
            var Dates = txtDates.value.split("$");
            txtDates.value = "";
            for (i = 1; i <= Dates.length - 1; i++) {
                if (i != e.selectedIndex) {
                    txtDates.value = txtDates.value + "$" + Dates[i];
                }
            }
            e.options[e.selectedIndex] = null;
        }
    }

    function AddDate(t1, t2, lst, tD, msg, OverlapMsg) {
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
    function OcultarRules(v, id, hS, hH) {
        var e = document.getElementById(id);
        var s = document.getElementById(hS);
        var o = document.getElementById(hH);
        if (v == '1') {
            e.style.display = '';
            o.style.display = '';
            s.style.display = "none";
        }
        else {
            e.style.display = "none";
            s.style.display = '';
            o.style.display = "none";
        }
    }



    function onddlRateplansChanged(cmb, lbl) {
        var c = document.getElementById(cmb);
        var l = document.getElementById(lbl);
        l.innerHTML = "";
        l.value = "";
        for (var i = 0; i < AccountsArray.length; i++) {

            if (AccountsArray[i][0] == c.value) {
                l.innerHTML = AccountsArray[i][1];
                l.value = AccountsArray[i][1];
                break;
            }
        }
    }

    /*function ShowDivRules(id,chk)
    {
    var e = document.getElementById(id);
    var c = document.getElementById(chk);
    if (c.checked)
    {
    e.style.display = "none"  								
    }
    else
    {
    e.style.display = ''  
    } 
  
 }*/

    function onCheckBoxClick(chk, td) {
        var c = document.getElementById(chk);
        var t = document.getElementById(td);
        if (c && t) {
            t.style.display = c.checked ? "" : "none";
        }
    }

    // Convierte items separadores en <optgroup> de Activos / Inactivos
    function convertirGruposRatePlans() {
        var sel = document.getElementById('<%= Me.ddlrateplans.ClientID %>');
        if (!sel) return;
        var opciones = Array.prototype.slice.call(sel.options);
        sel.innerHTML = '';
        var grupoActual = null;
        opciones.forEach(function (opt) {
            if (opt.value === '__GRP_ACTIVOS__' || opt.value === '__GRP_INACTIVOS__') {
                grupoActual = document.createElement('optgroup');
                grupoActual.label = opt.text.replace(/[─]/g, '').trim();
                sel.appendChild(grupoActual);
            } else {
                var newOpt = document.createElement('option');
                newOpt.value = opt.value;
                newOpt.text  = opt.text;
                if (opt.selected) newOpt.selected = true;
                (grupoActual || sel).appendChild(newOpt);
            }
        });
    }

    $(document).ready(function () { convertirGruposRatePlans(); });
    if (typeof Sys !== 'undefined') {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            convertirGruposRatePlans();
        });
    }
</script>


<span id="spArrayScriptContainer" runat="server"></span>
<table id="Table1" border="0" cellspacing="1" cellpadding="1" width="100%">
    <tr>
        <td style="HEIGHT: 49px" valign="middle" rowspan="2" colspan="2">
            <asp:TextBox Style="DISPLAY: none" ID="txtFechas" runat="server" Width="34px"></asp:TextBox>
            <table id="Table3" border="0" cellspacing="1" cellpadding="0" width="100%">
                <tr>
                    <td>
                        <asp:Label ID="lblRatePlan" runat="server" CssClass="clslabel" EnableViewState="False">Rate Plan</asp:Label></td>
                    <td>
                        <asp:DropDownList ID="ddlrateplans" runat="server"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td colspan="2"></td>
                </tr>
            </table>
        </td>
        <td style="HEIGHT: 49px" valign="middle" rowspan="2" colspan="2" align="left">
            <table border="0" cellspacing="1" cellpadding="1" width="100%">
                <tr>
                    <td>
                        <asp:Label ID="lblStartDate" runat="server" CssClass="clslabel" EnableViewState="False">Desde:</asp:Label></td>
                    <td>
                        <asp:TextBox ID="txtDateFrom" runat="server" Width="84px" CssClass="textbox" Columns="10" MaxLength="10"></asp:TextBox>
                        <a hidefocus 
                            onclick="if(self.gfPop)gfPop.fPopCalendar(document.getElementById('<%Response.Write(txtDateTo.ClientID)%>'),document.getElementById('<%Response.Write(txtDateFrom.ClientID)%>'));return false;" 
                            href="javascript:void(0)">
                            <img class="PopcalTrigger" border="0" alt="" align="absMiddle" src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>'>
                        </a>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblEndDate" runat="server" CssClass="clsLabel" EnableViewState="False">Hasta:</asp:Label></td>
                    <td>
                        <asp:TextBox ID="txtDateTo" runat="server" Width="84px" CssClass="textbox" Columns="10" MaxLength="10"></asp:TextBox><a hidefocus onclick="if(self.gfPop)gfPop.fPopCalendar1(document.getElementById('<%Response.Write(txtDateTo.ClientID)%>'));return false;" href="javascript:void(0)"><img class="PopcalTrigger" border="0"
                            alt="" align="absMiddle"
                            src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>'>
                        </a>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblDateErrorSign" runat="server" CssClass="Validators" EnableViewState="False"
                            Visible="False">Fecha Invalida</asp:Label></td>
                </tr>
            </table>
        </td>
        <td>
            <asp:Image Style="CURSOR: pointer" ID="imgAddDate" runat="server" ImageUrl="../Images/agregafecha.jpg"></asp:Image></td>
        <td style="HEIGHT: 49px" valign="top" rowspan="2" width="20%" align="left">
            <asp:ListBox ID="lstDates" runat="server" CssClass="clslabel" EnableViewState="True" Style="height: 80px; width: 160px;"></asp:ListBox></td>
    </tr>
    <tr>
        <td style="HEIGHT: 13px">
            <asp:Image Style="CURSOR: pointer" ID="imgDeleteDate" runat="server" ImageUrl="../Images/eliminarfecha.jpg"></asp:Image></td>
    </tr>
    <tr>
        <td colspan="6" align="center">
            <asp:Label ID="lblDateError" runat="server" CssClass="Validators" EnableViewState="False" Visible="False">El rango de fecha especificado se traslapa con una tarifa existente en el plan seleccionado, verifique las fechas</asp:Label>
            <asp:Label ID="lblError" runat="server" CssClass="Validators" EnableViewState="False" Visible="false"></asp:Label>
        </td>
    </tr>
    <!-- INICIO DE PROMOCIONES Y VENTANA DE RESERVACION -->
    <tr>
        <td class="dgItem" colspan="6" align="center">
            <asp:Label ID="lblProWin" runat="server" CssClass="clsLabel" EnableViewState="False">Promociones</asp:Label></td>
    </tr>
    <tr>
        <td colspan="6" align="right">
            <asp:HyperLink ID="hplShowProWin" runat="server" CssClass="showOptions">Mostrar</asp:HyperLink><asp:HyperLink ID="hplHideProWin" runat="server" CssClass="hideOptions">Ocultar</asp:HyperLink></td>
    </tr>
    <tr>
        <td valign="top" colspan="6" align="center">
            <div id="DivPromotionAndWindow" align="center" runat="server">

                <!--Promicion-->
                <table border="0" cellspacing="0" cellpadding="0" width="100%">
                    <tr>
                        <td align="right" style="width: 30%">
                            <asp:Label ID="lblPromotion" runat="server" EnableViewState="False" CssClass="clsLabel">Promotion : </asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDescProm" runat="server" Width="48px" CssClass="textbox" MaxLength="5"></asp:TextBox>
                            <asp:Label ID="lblDesc" runat="server" EnableViewState="False" CssClass="clsLabel">%</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:RangeValidator Style="Z-INDEX: 0" ID="RVPromotion" runat="server" CssClass="validators" MinimumValue="0"
                                MaximumValue="100" Type="Double" ControlToValidate="txtDescProm" ErrorMessage="Descuento debe ser numerico" Display="Dynamic"></asp:RangeValidator>
                            <asp:RangeValidator Style="Z-INDEX: 0" ID="Rangevalidator4" runat="server" CssClass="Validators" MinimumValue="0" Display="Dynamic"
                                MaximumValue="100" Type="Double" ControlToValidate="txtDescProm" ErrorMessage="0-100"></asp:RangeValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="clsLabel"><%=RateManager.PortalCulture.GetString("01399", True)%></td>
                        <td>
                            <uc1:CtrlIdioma IsMultiline="false" MaxLength="80" RequiredText="false" ID="txtPromoDescription" runat="server"></uc1:CtrlIdioma>
                        </td>
                    </tr>
                    <%If Me.HasData AndAlso (Not Me.txtPromoDescription.Published) Then%>
                    <tr>
                        <td colspan="2">
                            <span class="validators"><%=RateManager.PortalCulture.GetString(If(CType(Me.Page, RateManager.PaginaBase).IsSupervisor, "01365", "01392"))%></span>
                        </td>
                    </tr>
                    <% End If%>
                </table>
            </div>
        </td>
    </tr>
    <tr>
        <td class="dgItem" colspan="6" align="center">
            <asp:Label ID="lblWinReserva" runat="server" CssClass="clsLabel" EnableViewState="False">Ventana de Reserva</asp:Label></td>
        </td>
    </tr>
    <tr>
        <td colspan="6" align="right">
            <asp:HyperLink ID="hplShowVentanaReserva" runat="server" CssClass="showOptions">Mostrar</asp:HyperLink><asp:HyperLink ID="hplHideVentanaReserva" runat="server" CssClass="hideOptions">Ocultar</asp:HyperLink></td>
    </tr>
    <tr>
        <td align="center" colspan="6">
            <div id="DivWindowBooking" align="center" runat="server">
                <table border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td align="left">
                            <asp:CheckBox Style="Z-INDEX: 0" ID="chkBookingWindow" runat="server" CssClass="clsLabel" Text="Ventana de Reservacion"></asp:CheckBox></td>
                    </tr>
                    <tr>
                        <td align="left">
                            <table id="tbBookingWindow" border="0" cellspacing="1" cellpadding="1" width="100%" runat="server">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblBookWindowStartDate" runat="server" CssClass="clslabel" EnableViewState="False">Desde:</asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtBookWindowDateFrom" runat="server" Width="75px" CssClass="textbox" Columns="10"
                                            MaxLength="10"></asp:TextBox><a hidefocus onclick="if(self.gfPop)gfPop.fPopCalendar(document.getElementById('<%Response.Write(txtBookWindowDateTo.ClientID)%>'),document.getElementById('<%Response.Write(txtBookWindowDateFrom.ClientID)%>'));return false;" href="javascript:void(0)"><img
                                                class="PopcalTrigger" border="0" alt="" align="absMiddle"
                                                src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>'></a></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblBookWindowEndDate" runat="server" CssClass="clsLabel" EnableViewState="False">Hasta:</asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtBookWindowDateTo" runat="server" Width="75px" CssClass="textbox" Columns="10"
                                            MaxLength="10"></asp:TextBox><a hidefocus onclick="if(self.gfPop)gfPop.fPopCalendar1(document.getElementById('<%Response.Write(txtBookWindowDateTo.ClientID)%>'));return false;" href="javascript:void(0)"><img
                                                class="PopcalTrigger" border="0" alt="" align="absMiddle"
                                                src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>'>
                                            </a>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblBookWindowDateErrorSign" runat="server" CssClass="Validators" EnableViewState="False"
                                            Visible="False">Fecha Invalida</asp:Label></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>


        </td>
    </tr>
    <!-- FIN DE PROMOCIONES Y VENTANA DE RESERVACION -->
    <!-- INICIO OcUPACION POR HABITACION -->
    <tr>
        <td class="dgItem" colspan="6" align="center">
            <asp:Label ID="lblOccupation" runat="server" CssClass="clsLabel" EnableViewState="False">Ocupaci�n por habitaci�n</asp:Label></td>
    </tr>
    <tr>
        <td colspan="6" align="right">
            <asp:HyperLink ID="hplShowOccupation" runat="server" CssClass="showOptions">Mostrar</asp:HyperLink><asp:HyperLink ID="hplHideOccupation" runat="server" CssClass="hideOptions">Ocultar</asp:HyperLink></td>
    </tr>
    <tr>
        <td valign="top" colspan="6" align="center">
            <div id="DivOccupation" align="center" runat="server">
                <!--OCUPACION-->
                <table border="0" cellspacing="5" cellpadding="0">
                    <tr>
                        <td align="right">
                            <asp:Label ID="lbPersonas" runat="server" CssClass="clslabel" EnableViewState="False">Personas:</asp:Label></td>
                        <td align="left">
                            <asp:DropDownList ID="lstPeoplesInRoom" runat="server">
                                <asp:ListItem Value="-1">&nbsp;</asp:ListItem>
                                <asp:ListItem Value="1">1</asp:ListItem>
                                <asp:ListItem Value="2">2</asp:ListItem>
                                <asp:ListItem Value="3">3</asp:ListItem>
                                <asp:ListItem Value="4">4</asp:ListItem>
                                <asp:ListItem Value="5">5</asp:ListItem>
                                <asp:ListItem Value="6">6</asp:ListItem>
                                <asp:ListItem Value="7">7</asp:ListItem>
                                <asp:ListItem Value="8">8</asp:ListItem>
                                <asp:ListItem Value="9">9</asp:ListItem>
                                <asp:ListItem Value="10">10</asp:ListItem>
                                <asp:ListItem Value="11">11</asp:ListItem>
                                <asp:ListItem Value="12">12</asp:ListItem>
                                <asp:ListItem Value="13">13</asp:ListItem>
                                <asp:ListItem Value="14">14</asp:ListItem>
                                <asp:ListItem Value="15">15</asp:ListItem>
                            </asp:DropDownList></td>
                        <td align="right">
                            <asp:Label ID="lblMinNumberAdults" runat="server" CssClass="clslabel" EnableViewState="False">Minimo Adultos:</asp:Label></td>
                        <td>
                            <asp:DropDownList ID="lstMinNumberAdults" runat="server">
                                <asp:ListItem Value="-1">&#160;</asp:ListItem>
                                <asp:ListItem Value="1">1</asp:ListItem>
                                <asp:ListItem Value="2">2</asp:ListItem>
                                <asp:ListItem Value="3">3</asp:ListItem>
                                <asp:ListItem Value="4">4</asp:ListItem>
                                <asp:ListItem Value="5">5</asp:ListItem>
                                <asp:ListItem Value="6">6</asp:ListItem>
                                <asp:ListItem Value="7">7</asp:ListItem>
                                <asp:ListItem Value="8">8</asp:ListItem>
                                <asp:ListItem Value="9">9</asp:ListItem>
                                <asp:ListItem Value="10">10</asp:ListItem>
                                <asp:ListItem Value="11">11</asp:ListItem>
                                <asp:ListItem Value="12">12</asp:ListItem>
                                <asp:ListItem Value="13">13</asp:ListItem>
                                <asp:ListItem Value="14">14</asp:ListItem>
                                <asp:ListItem Value="15">15</asp:ListItem>
                            </asp:DropDownList></td>
                        <td align="right">
                            <asp:Label ID="lblNumberAdults" runat="server" CssClass="clslabel" EnableViewState="False">Maximo Adultos:</asp:Label></td>
                        <td>
                            <asp:DropDownList ID="lstNumberAdults" runat="server">
                                <asp:ListItem Value="-1">&nbsp;</asp:ListItem>
                                <asp:ListItem Value="1">1</asp:ListItem>
                                <asp:ListItem Value="2">2</asp:ListItem>
                                <asp:ListItem Value="3">3</asp:ListItem>
                                <asp:ListItem Value="4">4</asp:ListItem>
                                <asp:ListItem Value="5">5</asp:ListItem>
                                <asp:ListItem Value="6">6</asp:ListItem>
                                <asp:ListItem Value="7">7</asp:ListItem>
                                <asp:ListItem Value="8">8</asp:ListItem>
                                <asp:ListItem Value="9">9</asp:ListItem>
                                <asp:ListItem Value="10">10</asp:ListItem>
                                <asp:ListItem Value="11">11</asp:ListItem>
                                <asp:ListItem Value="12">12</asp:ListItem>
                                <asp:ListItem Value="13">13</asp:ListItem>
                                <asp:ListItem Value="14">14</asp:ListItem>
                                <asp:ListItem Value="15">15</asp:ListItem>
                            </asp:DropDownList></td>
                        <td align="right">
                            <asp:Label ID="lblNumberChildrens" runat="server" CssClass="clslabel" EnableViewState="False"> Ni�os:</asp:Label></td>
                        <td>
                            <asp:DropDownList ID="lstNumberChildrens" runat="server">
                                <asp:ListItem Value="-1">&nbsp;</asp:ListItem>
                                <asp:ListItem Value="0">0</asp:ListItem>
                                <asp:ListItem Value="1">1</asp:ListItem>
                                <asp:ListItem Value="2">2</asp:ListItem>
                                <asp:ListItem Value="3">3</asp:ListItem>
                                <asp:ListItem Value="4">4</asp:ListItem>
                                <asp:ListItem Value="5">5</asp:ListItem>
                                <asp:ListItem Value="6">6</asp:ListItem>
                                <asp:ListItem Value="7">7</asp:ListItem>
                                <asp:ListItem Value="8">8</asp:ListItem>
                                <asp:ListItem Value="9">9</asp:ListItem>
                                <asp:ListItem Value="10">10</asp:ListItem>
                                <asp:ListItem Value="11">11</asp:ListItem>
                                <asp:ListItem Value="12">12</asp:ListItem>
                                <asp:ListItem Value="13">13</asp:ListItem>
                                <asp:ListItem Value="14">14</asp:ListItem>
                            </asp:DropDownList></td>
                        <td>
                            <asp:Label ID="lblPeoplesExtras" runat="server" CssClass="clslabel" EnableViewState="False"> Extras:</asp:Label></td>
                        <td>
                            <asp:DropDownList ID="lstPeoplesExtras" runat="server">
                                <asp:ListItem Value="-1">&nbsp;</asp:ListItem>
                                <asp:ListItem Value="0">0</asp:ListItem>
                                <asp:ListItem Value="1">1</asp:ListItem>
                                <asp:ListItem Value="2">2</asp:ListItem>
                                <asp:ListItem Value="3">3</asp:ListItem>
                                <asp:ListItem Value="4">4</asp:ListItem>
                                <asp:ListItem Value="5">5</asp:ListItem>
                                <asp:ListItem Value="6">6</asp:ListItem>
                                <asp:ListItem Value="7">7</asp:ListItem>
                                <asp:ListItem Value="8">8</asp:ListItem>
                                <asp:ListItem Value="9">9</asp:ListItem>
                                <asp:ListItem Value="10">10</asp:ListItem>
                                <asp:ListItem Value="11">11</asp:ListItem>
                                <asp:ListItem Value="12">12</asp:ListItem>
                                <asp:ListItem Value="13">13</asp:ListItem>
                                <asp:ListItem Value="14">14</asp:ListItem>
                                <asp:ListItem Value="15">15</asp:ListItem>
                            </asp:DropDownList></td>
                    </tr>
                </table>
            </div>
        </td>
    </tr>
    <!-- FIN OcUPACION POR HABITACION -->
    <tr>
        <td class="dgItem" colspan="6" align="center">
            <asp:Label ID="lblReservationRules" runat="server" CssClass="clsLabel" EnableViewState="False">Precios de tarifa</asp:Label></td>
    </tr>
    <tr>
        <td colspan="5" align="left">
            <asp:CheckBox ID="chkRules" runat="server" CssClass="clsLabel"></asp:CheckBox></td>
        <td align="right">
            <asp:HyperLink ID="hplShowRules" runat="server" CssClass="showOptions">Show Rules</asp:HyperLink><asp:HyperLink ID="hplHideRules" runat="server" CssClass="hideOptions">Hide Rules</asp:HyperLink></td>
    </tr>
    <tr>
        <td colspan="6">
            <div id="DivRules" runat="server">
                <table border="0" cellspacing="1" cellpadding="1" width="100%">
                    <tr>
                        <td width="30%" align="right">
                            <asp:Label ID="lblAdvBooking" runat="server" CssClass="clsLabel" EnableViewState="False">Advanced Booking:</asp:Label></td>
                        <td width="20%">
                            <span style="margin-left: 10px">Min: </span>
                            <asp:TextBox ID="txtAdvBooking" runat="server" Width="40px" CssClass="TextBox" MaxLength="3"></asp:TextBox>
                            <asp:RangeValidator ID="RangeValidator1" runat="server" CssClass="Validators" MinimumValue="0" MaximumValue="255" Type="Integer" ControlToValidate="txtAdvBooking" ErrorMessage="0-255" ForeColor="Black" Display="Dynamic"></asp:RangeValidator>
                            <span style="margin-left: 10px">Max: </span>
                            <asp:TextBox ID="txtMaxAdvBooking" runat="server" Width="40px" CssClass="TextBox" MaxLength="3"></asp:TextBox>
                            <asp:RangeValidator ID="RangeValidator5" runat="server" CssClass="Validators" MinimumValue="0" MaximumValue="255" Type="Integer" ControlToValidate="txtMaxAdvBooking" ErrorMessage="0-255" ForeColor="Black" Display="Dynamic"></asp:RangeValidator>
                        </td>
                        <td width="20%" align="right">
                            <asp:Label ID="lblNoArrivos" runat="server" CssClass="clsLabel" EnableViewState="False">No Arrivos:</asp:Label></td>
                        <td width="30%">
                            <table id="Table2" border="0" cellspacing="0" cellpadding="0" width="100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbldomingo" runat="server" CssClass="clsLabel" EnableViewState="False">Dom</asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblLunes" runat="server" CssClass="clsLabel" EnableViewState="False">Lu</asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblMartes" runat="server" CssClass="clsLabel" EnableViewState="False">Mar</asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblMiercoles" runat="server" CssClass="clsLabel" EnableViewState="False">Mier</asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblJueves" runat="server" CssClass="clsLabel" EnableViewState="False">Jue</asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblViernes" runat="server" CssClass="clsLabel" EnableViewState="False">Vi</asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblSabado" runat="server" CssClass="clsLabel" EnableViewState="False">Sab</asp:Label></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="Chk7" runat="server"></asp:CheckBox></td>
                                    <td>
                                        <asp:CheckBox ID="Chk1" runat="server"></asp:CheckBox></td>
                                    <td>
                                        <asp:CheckBox ID="Chk2" runat="server"></asp:CheckBox></td>
                                    <td>
                                        <asp:CheckBox ID="Chk3" runat="server"></asp:CheckBox></td>
                                    <td>
                                        <asp:CheckBox ID="Chk4" runat="server"></asp:CheckBox></td>
                                    <td>
                                        <asp:CheckBox ID="Chk5" runat="server"></asp:CheckBox></td>
                                    <td>
                                        <asp:CheckBox ID="Chk6" runat="server"></asp:CheckBox></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblMinDias" runat="server" CssClass="clsLabel" EnableViewState="False">Minimo de noches para reservar:</asp:Label></td>
                        <td>
                            <asp:TextBox Style="margin-left: 10px" ID="txtMinDias" runat="server" Width="40px" CssClass="TextBox" MaxLength="3"></asp:TextBox><asp:RangeValidator ID="RangeValidator2" runat="server" CssClass="Validators" MinimumValue="1" MaximumValue="255"
                                Type="Integer" ControlToValidate="txtMinDias" ErrorMessage="1-255" ForeColor="Black" Display="Dynamic"></asp:RangeValidator></td>
                        <td align="right">
                            <asp:Label ID="lblMaxDias" runat="server" CssClass="clsLabel" EnableViewState="False">Maximo  de noches para reservar</asp:Label></td>
                        <td>
                            <asp:TextBox ID="txtMaxDias" runat="server" Width="32px" CssClass="TextBox" MaxLength="3"></asp:TextBox><asp:RangeValidator ID="RangeValidator3" runat="server" CssClass="Validators" MinimumValue="0" MaximumValue="255"
                                Type="Integer" ControlToValidate="txtMaxDias" ErrorMessage="0-255" ForeColor="Black" Display="Dynamic"></asp:RangeValidator></td>
                    </tr>
                    <tr>
                        <td align="right"></td>
                        <td></td>
                        <td align="right"></td>
                        <td></td>
                    </tr>
                </table>
            </div>
        </td>
    </tr>
    <tr>
        <td class="dgItem" colspan="6" align="center">
            <%=PortalCulture.GetString("00131")%>&nbsp;-&nbsp;<b><%=PortalCulture.GetString("M000263")%>&nbsp;<span class="currency"></span></b>&nbsp;
		    <%--<asp:label id="lblPreciosTarifa" runat="server" CssClass="clsLabel" EnableViewState="False">Precios de tarifa</asp:label>&nbsp;-&nbsp;
			<asp:label CssClass="" style="Z-INDEX: 0" id="lblCurrency" runat="server" Font-Bold="True"></asp:label>&nbsp;--%>
            <asp:Label ID="lblMonTar" runat="server" Font-Bold="True"></asp:Label>
        </td>
    </tr>
    <tr>
        <td colspan="6">
            <asp:DropDownList ID="ddlShowRates" runat="server">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td colspan="6">
            <div id="pnlTarifas" style="display: inline; width: 49%; vertical-align: top;">
                <table width="100%" style="vertical-align: top;">
                    <tr>
                        <td align="left" width="33%">
                            <asp:Label ID="lblPrecio" runat="server" CssClass="clsLabel" EnableViewState="False">Adulto:</asp:Label></td>
                        <td align="left" width="33%">
                            <asp:Label ID="LblChildPrice" runat="server" CssClass="clsLabel" EnableViewState="False">Ni�o</asp:Label></td>
                        <td align="left" width="33%">
                            <asp:Label ID="lblAdolescentePrice" runat="server" CssClass="clsLabel" EnableViewState="False">Adolescente</asp:Label></td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <asp:TextBox ID="txtAdultFare" runat="server" CssClass="TextBox currency" Columns="10" MaxLength="10">0</asp:TextBox>
                            <span class="currency"></span>
                            <asp:RequiredFieldValidator ID="reqAdultFare" runat="server" CssClass="Validators" ControlToValidate="txtAdultFare"
                                ErrorMessage="*" ForeColor=" " Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RVPrecio" runat="server" CssClass="Validators" ControlToValidate="txtAdultFare"
                                ErrorMessage="Precio Inv�lido" ForeColor=" " Display="Dynamic" ValidationExpression="^([0-9]*|\d*\.\d{1}?\d*)$"></asp:RegularExpressionValidator></td>
                        <td align="left" valign="top">
                            <asp:TextBox ID="txtChildFare" runat="server" CssClass="TextBox currency" Columns="10" MaxLength="10">0</asp:TextBox>
                            <span class="currency"></span>
                            <asp:RequiredFieldValidator ID="reqChildFare" runat="server" CssClass="Validators" ControlToValidate="txtChildFare"
                                ErrorMessage="*" ForeColor=" " Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="Regularexpressionvalidator1" runat="server" CssClass="Validators" ControlToValidate="txtChildFare"
                                ErrorMessage="Precio Inv�lido" ForeColor=" " Display="Dynamic" ValidationExpression="^([0-9]*|\d*\.\d{1}?\d*)$"></asp:RegularExpressionValidator></td>
                        <td align="left" valign="top">
                            <asp:TextBox ID="txtTeenFare" runat="server" CssClass="TextBox currency" Columns="10" MaxLength="10">0</asp:TextBox>
                            <% If txtTeenFare.Visible Then%>
                            <span class="currency"></span>
                            <asp:RequiredFieldValidator ID="reqTeenFare" runat="server" CssClass="Validators" ControlToValidate="txtTeenFare"
                                ErrorMessage="*" ForeColor=" " Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="rxvAdoslecenteFare" runat="server" CssClass="Validators" ControlToValidate="txtTeenFare"
                                ErrorMessage="Precio Inv�lido" ForeColor=" " Display="Dynamic" ValidationExpression="^([0-9]*|\d*\.\d{1}?\d*)$"></asp:RegularExpressionValidator></td>

                        <% End If%>
                    </tr>

                </table>
            </div>
            <div style="display: inline; width: 49%; vertical-align: top;">
                <table width="100%" style="vertical-align: top;">
                    <tr>
                        <td align="left" width="33%">
                            <asp:Label ID="lblExtraAdult" runat="server" CssClass="clsLabel" EnableViewState="False">Adulto extra:</asp:Label></td>
                        <td align="left" width="33%">
                            <asp:Label ID="lblExtraChildPrice" runat="server" CssClass="clsLabel" EnableViewState="False">Ni�o extra:</asp:Label></td>
                        <td align="left" width="33%">
                            <asp:Label ID="lblExtraAdolescente" runat="server" CssClass="clsLabel" EnableViewState="False">Adolescente extra:</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <asp:TextBox ID="txtExtraAdultPrice" runat="server" CssClass="TextBox currency" Columns="10" MaxLength="10"></asp:TextBox>
                            <span class="currency"></span>
                            <asp:RequiredFieldValidator ID="reqExtraAdultPrice" runat="server" CssClass="Validators" ControlToValidate="txtExtraAdultPrice"
                                ErrorMessage="*" ForeColor=" " Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="valAdultExtraPrice" runat="server" CssClass="Validators" ControlToValidate="txtExtraAdultPrice"
                                ErrorMessage="Precio Inv�lido" ForeColor=" " Display="Dynamic" ValidationExpression="^([0-9]*|\d*\.\d{1}?\d*)$"></asp:RegularExpressionValidator>
                        </td>
                        <td align="left" valign="top">
                            <asp:TextBox ID="txtExtraChildPrice" runat="server" CssClass="TextBox currency" Columns="10" MaxLength="10"></asp:TextBox>
                            <span class="currency"></span>
                            <asp:RequiredFieldValidator ID="reqExtraChildPrice" runat="server" CssClass="Validators" ControlToValidate="txtExtraChildPrice"
                                ErrorMessage="*" ForeColor=" " Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="valExtraChildPrice" runat="server" CssClass="Validators" ControlToValidate="txtExtraChildPrice"
                                ErrorMessage="Precio Inv�lido" ForeColor=" " Display="Dynamic" ValidationExpression="^([0-9]*|\d*\.\d{1}?\d*)$"></asp:RegularExpressionValidator>
                        </td>
                        <td align="left" valign="top">
                            <asp:TextBox ID="txtExtraTeenPrice" runat="server" CssClass="TextBox currency" Columns="10" MaxLength="10">0</asp:TextBox>
                            <% If txtExtraTeenPrice.Visible Then%>
                            <span class="currency"></span>
                            <asp:RequiredFieldValidator ID="reqExtraTeenPrice" runat="server" CssClass="Validators" ControlToValidate="txtExtraTeenPrice"
                                ErrorMessage="*" ForeColor=" " Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="rxvExtraTeenprice" runat="server" CssClass="Validators" ControlToValidate="txtExtraTeenPrice"
                                ErrorMessage="Precio Inv�lido" ForeColor=" " Display="Dynamic" ValidationExpression="^([0-9]*|\d*\.\d{1}?\d*)$"></asp:RegularExpressionValidator>
                            <% End If%>          
                        </td>
                    </tr>
                </table>
            </div>
        </td>
    </tr>
</table>

<%--<script>
    
    /*try
    {
		onddlRateplansChanged('<%=ddlrateplans.ClientID%>','<%=lblCurrency.ClientID%>');

    }

    catch(ex){}*/
    
  

</script>--%>