<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ctrRateAplicationNR.ascx.vb"
    Inherits="RateManager.ctrRateAplicationNR" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Import Namespace="RateManager" %>
<%@ Register TagPrefix="uc1" TagName="CtrlIdioma" Src="CtrlIdioma.ascx" %>

<script runat="server">
    Protected Sub AlCargar(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        Me.reqExtraAdultPriceNR.Enabled = Me.txtExtraAdultPriceNR.Visible AndAlso Me.txtExtraAdultPriceNR.Enabled
        Me.reqExtraChildPriceNR.Enabled = Me.txtExtraChildPriceNR.Visible AndAlso Me.txtExtraChildPriceNR.Enabled
        Me.reqExtraTeenPriceNR.Enabled = Me.txtExtraTeenPriceNR.Visible AndAlso Me.txtExtraTeenPriceNR.Enabled
        Me.reqExtraAdultPrice.Enabled = Me.IsSupervisor AndAlso Me.txtExtraAdultPrice.Enabled
        Me.reqExtraChildPrice.Enabled = Me.IsSupervisor AndAlso Me.txtExtraChildPrice.Enabled
        Me.reqExtraTeenPrice.Enabled = Me.IsSupervisor AndAlso Me.txtExtraTeenPrice.Enabled
    End Sub
</script>

<style type="text/css">
    input.currency
    {
        width: 75px;
    }
</style>

<script type="text/javascript">

    $(document).ready(function() {
        $('#<%= Me.ddlrateplans.ClientId%>').change(function() {
            rate = $(this).find('option:selected').val();
            $(rateInfo).each(function() {
                if (this.ratePlan === rate) {
                    $('span.currency').html(this.currency);
                    $('<%=Me.MaxPercentControlId%>').val(this.maxPercent);
                    $('<%=Me.MinPercentControlId%>').val(this.minPercent);
                }
            });
        });
        
        $('#<%= Me.ddlrateplans.ClientId%>').change();
        
        var list = $('#<%= Me.ddlShowRates.ClientId %>');
        
        list.change(function(){            
            var validators = new Array();
            validators.push({ id: '<%= Me.reqAdultFare.ClientID %>', hide:true });
            validators.push({ id: '<%= Me.reqChildFare.ClientID %>', hide:true });
            validators.push({ id: '<%= Me.reqTeenFare.ClientID %>', hide: true });
            validators.push({ id: '<%= Me.reqAdultFareNR.ClientId %>', hide: true });
            validators.push({ id: '<%= Me.reqChildFareNR.ClientId %>', hide: true });
            validators.push({ id: '<%= Me.reqTeenFareNR.ClientId %>', hide: true });
            
            validators.push({ id: '<%= Me.reqExtraAdultPrice.ClientID %>', hide: false });
            validators.push({ id: '<%= Me.reqExtraChildPrice.ClientID %>', hide: false });
            validators.push({ id: '<%= Me.reqExtraTeenPrice.ClientID %>', hide: false });
            validators.push({ id: '<%= Me.reqExtraAdultPriceNR.ClientId %>', hide: false });
            validators.push({ id: '<%= Me.reqExtraChildPriceNR.ClientId %>', hide: false });
            validators.push({ id: '<%= Me.reqExtraTeenPriceNR.ClientId %>', hide: false });
            
            FireShowSelectRates(this,'pnlTarifas','DivRates', validators);
        });        
        list.find('option:selected').removeAttr('selected');
        list.find('option:eq('+rateModeView.toString()+')').attr('selected','selected');
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

    function <%= Me.id %>_ValidateRates() {
                
        var minPercent = 0;

        //Obtiene el porcentaje minimo
        minPercent = Number($('#<%= TextBoxPorcMin.ClientId %>').val());
        if (isNaN(minPercent))
            minPercent = 0;
            
        var isValid = true;
        
        $('#pnlNetRates input.netRate').each(function(){
            
            if(isValid){
                var parent = $(this).closest('td');
                var netRate = Number($(this).val());
                var rate = Number(parent.find('input.rate').val());               
                if (isNaN(netRate)) netRate = 0;
                if (isNaN(rate)) rate = 0;
                if (rate <= 0 || Math.round(((rate - netRate) * 100) / netRate) < minPercent){
                    isValid = false;
                    $('#<%= Me.lblNetRateExceded.ClientId %>').show();                         
                    $('#<%= Me.lblNetRateExceded.ClientId %>').html('<%= PortalCulture.GetString("01355") %>'.replace('{0}', parent.find('span.label').html().replace(':','')));
                }
            } 
            return isValid;
            
        });
        
        return isValid;
    }
    
</script>

<span id="spArrayScriptContainer" runat="server"></span>
<table id="Table1" cellspacing="1" cellpadding="1" width="100%" border="0">
    <tr>
        <td valign="middle" colspan="2" rowspan="2" style="height: 49px">
            <asp:TextBox ID="txtFechas" Style="display: none" runat="server" Width="34px"></asp:TextBox>
            <table id="Table3" cellspacing="1" cellpadding="0" width="100%" border="0">
                <tr>
                    <td>
                        <asp:Label ID="lblRatePlan" EnableViewState="False" CssClass="clslabel" runat="server">Rate Plan</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlrateplans" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </td>
        <td valign="middle" align="left" colspan="2" rowspan="2" style="height: 49px">
            <table cellspacing="1" cellpadding="1" width="100%" border="0">
                <tr>
                    <td>
                        <asp:Label ID="lblStartDate" EnableViewState="False" CssClass="clslabel" runat="server">Desde:</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDateFrom" CssClass="textbox" runat="server" Width="84px" MaxLength="10"
                            Columns="10"></asp:TextBox>
                        <a hidefocus onclick="if(self.gfPop)gfPop.fPopCalendar(document.getElementById('<%response.write(txtDateTo.ClientId)%>'),document.getElementById('<%response.write(txtDateFrom.ClientId)%>'));return false;"
                            href="javascript:void(0)">
                            <img class="PopcalTrigger" alt="" src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>'
                                align="absMiddle" border="0"></a>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblEndDate" EnableViewState="False" CssClass="clsLabel" runat="server">Hasta:</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDateTo" CssClass="textbox" runat="server" Width="84px" MaxLength="10"
                            Columns="10"></asp:TextBox><a hidefocus onclick="if(self.gfPop)gfPop.fPopCalendar1(document.getElementById('<%response.write(txtDateTo.ClientId)%>'));return false;"
                                href="javascript:void(0)"><img class="PopcalTrigger" alt="" src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>'
                                    align="absMiddle" border="0">
                            </a>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblDateErrorSign" EnableViewState="False" CssClass="Validators" runat="server"
                            Visible="False">Fecha Invalida</asp:Label>
                    </td>
                </tr>
            </table>
        </td>
        <td>
            <asp:Image ID="imgAddDate" Style="cursor: pointer" runat="server" ImageUrl="../Images/agregafecha.jpg">
            </asp:Image>
        </td>
        <td valign="top" align="left" width="20%" rowspan="2" style="height: 49px">
            <asp:ListBox ID="lstDates" EnableViewState="True" CssClass="clslabel" runat="server"
                Style="height: 80px; width: 160px;"></asp:ListBox>
        </td>
    </tr>
    <tr>
        <td style="height: 13px">
            <asp:Image ID="imgDeleteDate" Style="cursor: pointer" runat="server" ImageUrl="../Images/eliminarfecha.jpg">
            </asp:Image>
        </td>
    </tr>
    <tr>
        <td align="center" colspan="6">
            <asp:Label ID="lblDateError" EnableViewState="False" CssClass="Validators" runat="server"
                Visible="False">El rango de fecha especificado se traslapa con una tarifa existente en el plan seleccionado, verifique las fechas</asp:Label>
        </td>
    </tr>
    <!-- INICIO DE PROMOCIONES Y VENTANA DE RESERVACION -->
    <tr>
        <td class="dgItem" align="center" colspan="6">
            <asp:Label ID="lblProWin" EnableViewState="False" CssClass="clsLabel" runat="server">Promociones y Ventana de Reserva</asp:Label>
        </td>
    </tr>
    <tr>
        <td align="right" colspan="6">
            <asp:HyperLink ID="hplShowProWin" CssClass="showOptions" runat="server">Mostrar</asp:HyperLink>
            <asp:HyperLink ID="hplHideProWin" CssClass="hideOptions" runat="server">Ocultar</asp:HyperLink>
        </td>
    </tr>
    <tr>
        <td colspan="6" align="center" valign="top">
            <div id="DivPromotionAndWindow" runat="server" align="center">
                <table cellpadding="2" cellspacing="2" border="0" width="100%">
                    <tr>
                        <!--Promicion-->
                        <td style="width: 25%; padding-top: 5px;" align="right" valign="top">
                            <asp:Label ID="lblPromotion" EnableViewState="False" runat="server" CssClass="clsLabel">Promotion : </asp:Label>
                        </td>
                        <td style="width: 25%" valign="top">
                            <div style="vertical-align: top;">
                                <asp:TextBox Style="vertical-align: middle;" ID="txtDescProm" CssClass="textbox"
                                    runat="server" MaxLength="5" Width="48px"></asp:TextBox>
                                <asp:Label ID="lblDesc" EnableViewState="False" runat="server">%</asp:Label>
                            </div>
                        </td>
                        <td>
                            <!--Venta-->
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td align="left">
                                        <asp:CheckBox Style="z-index: 0" ID="chkBookingWindow" runat="server" CssClass="clsLabel"
                                            Text="Ventana de Reservacion"></asp:CheckBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <table cellspacing="1" cellpadding="1" width="100%" border="0" id="tbBookingWindow"
                                            runat="server">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblBookWindowStartDate" EnableViewState="False" CssClass="clslabel"
                                                        runat="server">Desde:</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtBookWindowDateFrom" CssClass="textbox" runat="server" Width="75px"
                                                        MaxLength="10" Columns="10"></asp:TextBox>
                                                    <a hidefocus onclick="if(self.gfPop)gfPop.fPopCalendar(document.getElementById('<%response.write(txtBookWindowDateTo.ClientId)%>'),document.getElementById('<%response.write(txtBookWindowDateFrom.ClientId)%>'));return false;"
                                                        href="javascript:void(0)">
                                                        <img class="PopcalTrigger" alt="" src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>'
                                                            align="absMiddle" border="0"></a>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblBookWindowEndDate" EnableViewState="False" CssClass="clsLabel"
                                                        runat="server">Hasta:</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtBookWindowDateTo" CssClass="textbox" runat="server" Width="75px"
                                                        MaxLength="10" Columns="10"></asp:TextBox><a hidefocus onclick="if(self.gfPop)gfPop.fPopCalendar1(document.getElementById('<%response.write(txtBookWindowDateTo.ClientId)%>'));return false;"
                                                            href="javascript:void(0)"><img class="PopcalTrigger" alt="" src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>'
                                                                align="absMiddle" border="0">
                                                        </a>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Label ID="lblBookWindowDateErrorSign" EnableViewState="False" CssClass="Validators"
                                                        runat="server" Visible="False">Fecha Invalida</asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:RangeValidator Style="z-index: 0" ID="RVPromotion" runat="server" CssClass="validators"
                                ErrorMessage="Descuento debe ser numerico" ControlToValidate="txtDescProm" Type="Double"
                                MaximumValue="100" MinimumValue="0" Display="Dynamic"></asp:RangeValidator>
                            &nbsp;
                            <asp:RangeValidator Style="z-index: 0" ID="Rangevalidator4" runat="server" CssClass="Validators"
                                ErrorMessage="0-100" ControlToValidate="txtDescProm" Type="Double" MaximumValue="100"
                                MinimumValue="0" Display="Dynamic"></asp:RangeValidator>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="clsLabel">
                            <%=RateManager.PortalCulture.GetString("01399", True)%>
                        </td>
                        <td colspan="2">
                            <uc1:CtrlIdioma IsMultiline="false" MaxLength="80" RequiredText="false" ID="txtPromoDescription"
                                runat="server"></uc1:CtrlIdioma>
                        </td>
                    </tr>
                    <%If Me.HasData AndAlso (Not Me.txtPromoDescription.Published) Then%>
                    <tr>
                        <td colspan="3">
                            <span class="validators">
                                <%=RateManager.PortalCulture.GetString(If(CType(Me.Page, RateManager.PaginaBase).IsSupervisor, "01365", "01392"))%></span>
                        </td>
                    </tr>
                    <% End If%>
                </table>
            </div>
        </td>
    </tr>
    <!-- FIN DE PROMOCIONES Y VENTANA DE RESERVACION -->
    <!-- INICIO OcUPACION POR HABITACION -->
    <tr>
        <td class="dgItem" align="center" colspan="6">
            <asp:Label ID="lblOccupation" EnableViewState="False" CssClass="clsLabel" runat="server">Ocupación por habitación</asp:Label>
        </td>
    </tr>
    <tr>
        <td align="right" colspan="6">
            <asp:HyperLink ID="hplShowOccupation" CssClass="showOptions" runat="server">Mostrar</asp:HyperLink>
            <asp:HyperLink ID="hplHideOccupation" CssClass="hideOptions" runat="server">Ocultar</asp:HyperLink>
        </td>
    </tr>
    <tr>
        <td colspan="6" align="center" valign="top">
            <div id="DivOccupation" runat="server" align="center">
                <!--OCUPACION-->
                <table cellspacing="5" cellpadding="0" border="0">
                    <tr>
                        <td align="right">
                            <asp:Label ID="lbPersonas" runat="server" EnableViewState="False" CssClass="clslabel">Personas:</asp:Label>
                        </td>
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
                            </asp:DropDownList>
                        </td>
                        <td align="right">
                            <asp:Label ID="lblMinNumberAdults" runat="server" CssClass="clslabel" EnableViewState="False">Minimo Adultos:</asp:Label>
                        </td>
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
                            </asp:DropDownList>
                        </td>
                        <td align="right">
                            <asp:Label ID="lblNumberAdults" runat="server" CssClass="clslabel" EnableViewState="False">Adultos:</asp:Label>
                        </td>
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
                            </asp:DropDownList>
                        </td>
                        <td align="right">
                            <asp:Label ID="lblNumberChildrens" runat="server" CssClass="clslabel" EnableViewState="False">Niños:</asp:Label>
                        </td>
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
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblPeoplesExtras" runat="server" CssClass="clslabel" EnableViewState="False"> Extras:</asp:Label>
                        </td>
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
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </div>
        </td>
    </tr>
    <!-- FIN OcUPACION POR HABITACION -->
    <tr>
        <td class="dgItem" align="center" colspan="6">
            <asp:Label ID="lblReservationRules" EnableViewState="False" CssClass="clsLabel" runat="server">Precios de tarifa</asp:Label>
        </td>
    </tr>
    <tr>
        <td align="left" colspan="5">
            <asp:CheckBox ID="chkRules" CssClass="clsLabel" runat="server"></asp:CheckBox>
        </td>
        <td align="right">
            <asp:HyperLink ID="hplShowRules" CssClass="showOptions" runat="server">Show Rules</asp:HyperLink><asp:HyperLink
                ID="hplHideRules" CssClass="hideOptions" runat="server">Hide Rules</asp:HyperLink>
        </td>
    </tr>
    <tr>
        <td colspan="6">
            <div id="DivRules" runat="server">
                <table cellspacing="1" cellpadding="1" width="100%" border="0">
                    <tr>
                        <td align="right" width="30%">
                            <asp:Label ID="lblAdvBooking" EnableViewState="False" CssClass="clsLabel" runat="server">Advanced Booking:</asp:Label>
                        </td>
                        <td width="20%">
                            <span style="margin-left:10px">Min: </span>
                            <asp:TextBox ID="txtAdvBooking" CssClass="TextBox" runat="server" Width="40px" MaxLength="3"></asp:TextBox><asp:RangeValidator
                                ID="RangeValidator1" CssClass="Validators" runat="server" MinimumValue="0" MaximumValue="255"
                                Type="Integer" ControlToValidate="txtAdvBooking" ErrorMessage="0-255" Display="Dynamic"
                                ForeColor="Black"></asp:RangeValidator>
                            <span style="margin-left:10px">Max: </span>
                            <asp:TextBox ID="txtMaxAdvBooking" runat="server" Width="40px" CssClass="TextBox" MaxLength="3"></asp:TextBox>
                            <asp:RangeValidator ID="RangeValidator5" runat="server" CssClass="Validators" MinimumValue="0" MaximumValue="255"
                                Type="Integer" ControlToValidate="txtMaxAdvBooking" ErrorMessage="0-255" ForeColor="Black"
                                Display="Dynamic"></asp:RangeValidator>
                        </td>
                        <td align="right" width="20%">
                            <asp:Label ID="lblNoArrivos" EnableViewState="False" CssClass="clsLabel" runat="server">No Arrivos:</asp:Label>
                        </td>
                        <td width="30%">
                            <table id="Table2" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbldomingo" runat="server" CssClass="clsLabel" EnableViewState="False">Dom</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblLunes" runat="server" CssClass="clsLabel" EnableViewState="False">Lu</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblMartes" EnableViewState="False" CssClass="clsLabel" runat="server">Mar</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblMiercoles" EnableViewState="False" CssClass="clsLabel" runat="server">Mier</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblJueves" EnableViewState="False" CssClass="clsLabel" runat="server">Jue</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblViernes" EnableViewState="False" CssClass="clsLabel" runat="server">Vi</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblSabado" EnableViewState="False" CssClass="clsLabel" runat="server">Sab</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="Chk7" runat="server"></asp:CheckBox>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="Chk1" runat="server"></asp:CheckBox>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="Chk2" runat="server"></asp:CheckBox>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="Chk3" runat="server"></asp:CheckBox>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="Chk4" runat="server"></asp:CheckBox>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="Chk5" runat="server"></asp:CheckBox>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="Chk6" runat="server"></asp:CheckBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblMinDias" EnableViewState="False" CssClass="clsLabel" runat="server">Minimo de noches para reservar:</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMinDias" CssClass="TextBox" runat="server" Width="40px" MaxLength="3"></asp:TextBox><asp:RangeValidator
                                ID="RangeValidator2" CssClass="Validators" runat="server" MinimumValue="1" MaximumValue="255"
                                Type="Integer" ControlToValidate="txtMinDias" ErrorMessage="1-255" Display="Dynamic"
                                ForeColor="Black"></asp:RangeValidator>
                        </td>
                        <td align="right">
                            <asp:Label ID="lblMaxDias" EnableViewState="False" CssClass="clsLabel" runat="server">Maximo  de noches para reservar</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMaxDias" CssClass="TextBox" runat="server" Width="32px" MaxLength="3"></asp:TextBox><asp:RangeValidator
                                ID="RangeValidator3" CssClass="Validators" runat="server" MinimumValue="0" MaximumValue="255"
                                Type="Integer" ControlToValidate="txtMaxDias" ErrorMessage="0-255" Display="Dynamic"
                                ForeColor="Black"></asp:RangeValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                        </td>
                        <td>
                            <asp:TextBox ID="TextBoxPorcMax" runat="server"></asp:TextBox>
                        </td>
                        <td align="right">
                            <asp:TextBox ID="TextBoxPorcMin" runat="server"></asp:TextBox>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </div>
        </td>
    </tr>
    <tr>
        <td class="dgItem" align="center" colspan="6">
            <%=PortalCulture.GetString("00131")%>&nbsp;-&nbsp;<b><%=PortalCulture.GetString("M000263")%>&nbsp;<span
                class="currency"></span></b>&nbsp;
            <%--<asp:label id="lblPreciosTarifa" runat="server" CssClass="clsLabel" EnableViewState="False">Precios de tarifa</asp:label>&nbsp;-&nbsp;
			<asp:label CssClass="" style="Z-INDEX: 0" id="lblCurrency" runat="server" Font-Bold="True"></asp:label>&nbsp;--%>
            <asp:Label ID="lblMonTar" runat="server" Font-Bold="True"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="dgItem" colspan="6" align="left">
            <asp:DropDownList ID="ddlShowRates" runat="server">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td colspan="6">
            <div style="display: inline; width: 49%; vertical-align: top;" id="pnlTarifas">
                <table width="100%" style="vertical-align: top;">
                    <tr id="pnlNetRates">
                        <td align="left" width="33%" valign="top">
                            <asp:Label ID="lblPrecioNR" runat="server" CssClass="clsLabel label" EnableViewState="False">Adulto NR:</asp:Label><br />
                            <asp:TextBox ID="txtAdultFareNR" runat="server" CssClass="TextBox currency netRate"
                                Columns="10" MaxLength="10">0</asp:TextBox>
                            <input type="hidden" id="varAdultFareNR" runat="server" class="rate" />
                            <span class="currency"></span>
                            <asp:RequiredFieldValidator ID="reqAdultFareNR" ForeColor=" " runat="server" CssClass="Validators"
                                Display="Dynamic" ErrorMessage="*" ControlToValidate="txtAdultFareNR"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="Regularexpressionvalidator2" runat="server" CssClass="Validators"
                                ForeColor=" " Display="Dynamic" ErrorMessage="Precio Inválido" ControlToValidate="txtAdultFareNR"
                                ValidationExpression="^([0-9]*|\d*\.\d{1}?\d*)$"></asp:RegularExpressionValidator>
                            <asp:RangeValidator runat="server" ControlToValidate="txtAdultFareNR" MinimumValue="1" MaximumValue="99999"
                                Display="Dynamic" CssClass="Validators" Type="Double">1-99999</asp:RangeValidator>
                        </td>
                        <td align="left" width="33%" valign="top">
                            <asp:Label ID="LblChildPriceNR" runat="server" CssClass="clsLabel label" EnableViewState="False">Niño NR:</asp:Label><br />
                            <asp:TextBox ID="txtChildFareNR" runat="server" CssClass="TextBox currency" Columns="10"
                                MaxLength="10">0</asp:TextBox>
                            <input type="hidden" id="varChildFareNR" runat="server" class="rate" />
                            <span class="currency"></span>
                            <asp:RequiredFieldValidator ID="reqChildFareNR" ForeColor=" " runat="server" CssClass="Validators"
                                Display="Dynamic" ErrorMessage="*" ControlToValidate="txtChildFareNR"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="Regularexpressionvalidator3" runat="server" CssClass="Validators"
                                ForeColor=" " Display="Dynamic" ErrorMessage="Precio Inválido" ControlToValidate="txtChildFareNR"
                                ValidationExpression="^([0-9]*|\d*\.\d{1}?\d*)$"></asp:RegularExpressionValidator>
                        </td>
                        <td align="left" width="33%" valign="top">
                            <asp:Label ID="lblTeenPriceNr" runat="server" CssClass="clsLabel label" EnableViewState="False">Adolescente NR:</asp:Label><br />
                            <asp:TextBox ID="txtTeenFareNR" runat="server" CssClass="TextBox currency" Columns="10"
                                MaxLength="10">0</asp:TextBox>
                            <% If txtTeenFareNR.Visible Then%>
                            <input type="hidden" id="varTeenFareNR" runat="server" class="rate" />
                            <span class="currency"></span>
                            <asp:RequiredFieldValidator ID="reqTeenFareNR" ForeColor=" " runat="server" CssClass="Validators"
                                Display="Dynamic" ErrorMessage="*" ControlToValidate="txtTeenFareNR"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="rxvTeenFareNR" runat="server" CssClass="Validators"
                                ForeColor=" " Display="Dynamic" ErrorMessage="Precio Inválido" ControlToValidate="txtTeenFareNR"
                                ValidationExpression="^([0-9]*|\d*\.\d{1}?\d*)$"></asp:RegularExpressionValidator>
                            <% End If%>
                        </td>
                    </tr>
                    <% If Me.IsSupervisor Then%>
                    <tr>
                        <td>
                            <asp:Label ID="lblPrecio" EnableViewState="False" CssClass="clsLabel" runat="server">Adulto UV:</asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="LblChildPrice" EnableViewState="False" CssClass="clsLabel" runat="server">Niño UV:</asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblTeenPrice" runat="server" CssClass="clsLabel" EnableViewState="False">Adolescente UV:</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <asp:TextBox ID="txtAdultFare" CssClass="TextBox currency" runat="server" MaxLength="10"
                                Columns="10">0</asp:TextBox>
                            <span class="currency"></span>
                            <asp:RequiredFieldValidator ID="reqAdultFare" CssClass="Validators" ForeColor=" "
                                runat="server" ControlToValidate="txtAdultFare" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RVPrecio" CssClass="Validators" runat="server"
                                ControlToValidate="txtAdultFare" ErrorMessage="Precio Inválido" Display="Dynamic"
                                ForeColor=" " ValidationExpression="^([0-9]*|\d*\.\d{1}?\d*)$"></asp:RegularExpressionValidator>
                            <%--<asp:CompareValidator ID="cmpvAdults" runat="server" CssClass="Validators" ErrorMessage="CompareValidator" ControlToValidate="txtAdultFare"  ControlToCompare="txtAdultFareNR"  Type="Double" Operator="GreaterThanEqual" Display=Dynamic style= "width:120px;" ><%= RateManager.PortalCulture.GetString("01506") %></asp:CompareValidator>--%>
                        </td>
                        <td align="left" valign="top">
                            <asp:TextBox ID="txtChildFare" CssClass="TextBox currency" runat="server" MaxLength="10"
                                Columns="10">0</asp:TextBox>
                            <span class="currency"></span>
                            <asp:RequiredFieldValidator ID="reqChildFare" CssClass="Validators" ForeColor=" "
                                runat="server" ControlToValidate="txtChildFare" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="Regularexpressionvalidator1" CssClass="Validators"
                                runat="server" ControlToValidate="txtChildFare" ErrorMessage="Precio Inválido"
                                Display="Dynamic" ForeColor=" " ValidationExpression="^([0-9]*|\d*\.\d{1}?\d*)$"></asp:RegularExpressionValidator>
                            <%--<asp:CompareValidator ID="cmpvChilds" runat="server" CssClass="Validators" ErrorMessage="CompareValidator" ControlToValidate="txtChildFare"  ControlToCompare="txtChildFareNR"  Type="Double" Operator="GreaterThanEqual" Display=Dynamic style= "width:120px;" ><%= RateManager.PortalCulture.GetString("01506") %></asp:CompareValidator>--%>
                        </td>
                        <td align="left" valign="top">
                            <asp:TextBox ID="txtTeenFare" CssClass="TextBox currency" runat="server" MaxLength="10"
                                Columns="10">0</asp:TextBox>
                            <% If txtTeenFare.Visible Then%>
                            <span class="currency"></span>
                            <asp:RequiredFieldValidator ID="reqTeenFare" CssClass="Validators" ForeColor=" "
                                runat="server" ControlToValidate="txtTeenFare" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="rxvTeenPrice" CssClass="Validators" runat="server"
                                ControlToValidate="txtTeenFare" ErrorMessage="Precio Inválido" Display="Dynamic"
                                ForeColor=" " ValidationExpression="^([0-9]*|\d*\.\d{1}?\d*)$"></asp:RegularExpressionValidator>
                            <%--<asp:CompareValidator ID="cmpvJuniors" runat="server" CssClass="Validators" ErrorMessage="CompareValidator" ControlToValidate="txtTeenFare"  ControlToCompare="txtTeenFareNR"  Type="Double" Operator="GreaterThanEqual"  Display=Dynamic  style= "width:120px;"><%= RateManager.PortalCulture.GetString("01506") %></asp:CompareValidator>--%>
                            <% End If%>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="lblAdultValMax" runat="server" CssClass="Validators">--</asp:Label>
                            <asp:Label ID="lblAdultValMin" runat="server" CssClass="Validators">--</asp:Label>
                        </td>
                        <td align="left">
                            <asp:Label ID="lblChildValMax" runat="server" CssClass="Validators">Label</asp:Label>
                            <asp:Label ID="lblChildValMin" runat="server" CssClass="Validators">Label</asp:Label>
                        </td>
                    </tr>
                    <%End If%>
                </table>
            </div>
            <div style="display: inline; width: 49%; vertical-align: top;">
                <table width="100%" style="vertical-align: top;">
                    <tr>
                        <td align="left" width="33%" valign="top">
                            <asp:Label ID="lblExtraAdultNR" runat="server" CssClass="clsLabel label" EnableViewState="False">Adulto extra NR:</asp:Label><br />
                            <asp:TextBox ID="txtExtraAdultPriceNR" runat="server" CssClass="TextBox currency netRate"
                                Columns="10" MaxLength="10"></asp:TextBox>
                            <input type="hidden" id="varExtraAdultPriceNR" runat="server" class="rate" />
                            <span class="currency"></span>
                            <asp:RequiredFieldValidator ID="reqExtraAdultPriceNR" CssClass="Validators" ForeColor=" "
                                runat="server" ControlToValidate="txtExtraAdultPriceNR" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="Regularexpressionvalidator4" runat="server" CssClass="Validators"
                                ForeColor=" " Display="Dynamic" ErrorMessage="Precio Inválido" ControlToValidate="txtExtraAdultPriceNR"
                                ValidationExpression="^([0-9]*|\d*\.\d{1}?\d*)$"></asp:RegularExpressionValidator>
                        </td>
                        <td align="left" width="33%" valign="top">
                            <asp:Label ID="lblExtraChildPriceNR" runat="server" CssClass="clsLabel label" EnableViewState="False">Niño extra NR:</asp:Label><br />
                            <asp:TextBox ID="txtExtraChildPriceNR" runat="server" CssClass="TextBox currency netRate"
                                Columns="10" MaxLength="10"></asp:TextBox>
                            <input type="hidden" id="varExtraChildPriceNR" runat="server" class="rate" />
                            <span class="currency"></span>
                            <asp:RequiredFieldValidator ID="reqExtraChildPriceNR" CssClass="Validators" ForeColor=" "
                                runat="server" ControlToValidate="txtExtraChildPriceNR" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="Regularexpressionvalidator5" runat="server" CssClass="Validators"
                                ForeColor=" " Display="Dynamic" ErrorMessage="Precio Inválido" ControlToValidate="txtExtraChildPriceNR"
                                ValidationExpression="^([0-9]*|\d*\.\d{1}?\d*)$"></asp:RegularExpressionValidator>
                        </td>
                        <td width="33%" valign="top">
                            <asp:Label ID="lblExtraAdolescenteNR" runat="server" CssClass="clsLabel label" EnableViewState="False">Junior Extra NR:</asp:Label><br />
                            <asp:TextBox ID="txtExtraTeenPriceNR" CssClass="TextBox currency netRate" Columns="10"
                                MaxLength="10" runat="server">0</asp:TextBox>
                            <% If txtExtraTeenPriceNR.Visible Then%>
                            <input type="hidden" id="varExtraTeenPriceNR" runat="server" class="rate" />
                            <span class="currency"></span>
                            <asp:RequiredFieldValidator ID="reqExtraTeenPriceNR" CssClass="Validators" ForeColor=" "
                                runat="server" ControlToValidate="txtExtraTeenPriceNR" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="valExtraTeenPriceNR" runat="server" CssClass="Validators"
                                ForeColor=" " Display="Dynamic" ErrorMessage="Precio Inválido" ControlToValidate="txtExtraTeenPriceNR"
                                ValidationExpression="^([0-9]*|\d*\.\d{1}?\d*)$"></asp:RegularExpressionValidator>
                            <% End If%>
                        </td>
                    </tr>
                    <% If Me.IsSupervisor Then%>
                    <tr>
                        <td width="33%">
                            <asp:Label ID="lblExtraAdultUv" runat="server" CssClass="clsLabel" EnableViewState="False">Adulto extra UV:</asp:Label>
                        </td>
                        <td width="33%">
                            <asp:Label ID="lblExtraChildPriceUV" runat="server" CssClass="clsLabel" EnableViewState="False">Niño. extra UV:</asp:Label>
                        </td>
                        <td width="33%">
                            <asp:Label ID="lblExtraAdolescenteUV" runat="server" CssClass="clsLabel" EnableViewState="False">Junior extra UV:</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <asp:TextBox ID="txtExtraAdultPrice" runat="server" CssClass="TextBox currency" Columns="10"
                                MaxLength="10"></asp:TextBox>
                            <span class="currency"></span>
                            <asp:RequiredFieldValidator ID="reqExtraAdultPrice" CssClass="Validators" ForeColor=" "
                                runat="server" ControlToValidate="txtExtraAdultPrice" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="valAdultExtraPrice" runat="server" CssClass="Validators"
                                ForeColor=" " Display="Dynamic" ErrorMessage="Precio Inválido" ControlToValidate="txtExtraAdultPrice"
                                ValidationExpression="^([0-9]*|\d*\.\d{1}?\d*)$"></asp:RegularExpressionValidator>
                            <asp:CompareValidator ID="compExtraAdultPrice" runat="server" CssClass="Validators"
                                ErrorMessage="CompareValidator" ControlToValidate="txtExtraAdultPrice" ControlToCompare="txtExtraAdultPriceNR"
                                Type="Double" Operator="GreaterThanEqual" Display="Dynamic" Style="width: 120px;"><%= RateManager.PortalCulture.GetString("01506") %></asp:CompareValidator>
                        </td>
                        <td align="left" valign="top">
                            <asp:TextBox ID="txtExtraChildPrice" runat="server" CssClass="TextBox currency" Columns="10"
                                MaxLength="10"></asp:TextBox>
                            <span class="currency"></span>
                            <asp:RequiredFieldValidator ID="reqExtraChildPrice" CssClass="Validators" ForeColor=" "
                                runat="server" ControlToValidate="txtExtraChildPrice" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="valExtraChildPrice" runat="server" CssClass="Validators"
                                ForeColor=" " Display="Dynamic" ErrorMessage="Precio Inválido" ControlToValidate="txtExtraChildPrice"
                                ValidationExpression="^([0-9]*|\d*\.\d{1}?\d*)$"></asp:RegularExpressionValidator>
                            <asp:CompareValidator ID="compExtraChildPrice" runat="server" CssClass="Validators"
                                ErrorMessage="CompareValidator" ControlToValidate="txtExtraChildPrice" ControlToCompare="txtExtraChildPriceNR"
                                Type="Double" Operator="GreaterThanEqual" Display="Dynamic" Style="width: 120px;"><%= RateManager.PortalCulture.GetString("01506") %></asp:CompareValidator>
                        </td>
                        <td valign="top">
                            <asp:TextBox ID="txtExtraTeenPrice" CssClass="TextBox currency" Columns="10" MaxLength="10"
                                runat="server">0</asp:TextBox>
                            <% If txtExtraTeenPrice.Visible Then%>
                            <span class="currency"></span>
                            <asp:RequiredFieldValidator ID="reqExtraTeenPrice" CssClass="Validators" ForeColor=" "
                                runat="server" ControlToValidate="txtExtraTeenPrice" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="valExtraTeenPrice" runat="server" CssClass="Validators"
                                ForeColor=" " Display="Dynamic" ErrorMessage="Precio Inválido" ControlToValidate="txtExtraTeenPrice"
                                ValidationExpression="^([0-9]*|\d*\.\d{1}?\d*)$"></asp:RegularExpressionValidator>
                            <asp:CompareValidator ID="compExtraTeenPrice" runat="server" CssClass="Validators"
                                ErrorMessage="CompareValidator" ControlToValidate="txtExtraTeenPrice" ControlToCompare="txtExtraTeenPriceNR"
                                Type="Double" Operator="GreaterThanEqual" Display="Dynamic" Style="width: 120px;"><%= RateManager.PortalCulture.GetString("01506") %></asp:CompareValidator>
                            <% End If%>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="lblAdultExtValMax" runat="server" CssClass="Validators">Label</asp:Label>
                            <asp:Label ID="lblAdultExtValMin" runat="server" CssClass="Validators">Label</asp:Label>
                        </td>
                        <td align="left">
                            <asp:Label ID="lblChildExtValMax" runat="server" CssClass="Validators">Label</asp:Label>
                            <asp:Label ID="lblChildExtValMin" runat="server" CssClass="Validators">Label</asp:Label>
                        </td>
                    </tr>
                    <%End If%>
                </table>
            </div>
            <% If Me.IsSupervisor Then%>
            <div>
                <span class="validators" id="lblNetRateExceded" runat="server" style="display: none;">
                </span>
            </div>
            <%End If%>
        </td>
    </tr>
</table>
