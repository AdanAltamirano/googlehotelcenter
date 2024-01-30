<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ctrRatePlan.ascx.vb" Inherits="RateManager.ctrRatePlan" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="uc1" TagName="ctrPortal" Src="ctrPortal.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrlIdioma" Src="CtrlIdioma.ascx" %>

<script src="../Pages/Scripts/jquery.min.js" type="text/javascript"></script>

<script type="text/javascript">
    // Get the modal
    var initGoogleChecks = function () {

        $('#<%= Me.portalMovil.ClientId %>').change(function () {
            document.getElementById('msgGoogleHC').style.display = "block";
        });

        $('#<%= Me.onlyCC.ClientId %>').change(function () {
            document.getElementById('msgGoogleHC').style.display = "block";
        });

    }

    // Get the <span> element that closes the modal
    var span = document.getElementsByClassName("close")[0];

    // When the user clicks on <span> (x), close the modal
    var closeModal = function () {
        document.getElementById('msgGoogleHC').style.display = "none";
    }

    var showModal = function () {
        document.getElementById('msgGoogleHC').style.display = "block";
    }

    // When the user clicks anywhere outside of the modal, close it
    window.onclick = function (event) {
        if (event.target == document.getElementById('onlyCC') || event.target == document.getElementById('portalMovil')) {
            showModal();
        }

        if (event.target == document.getElementById('msgGoogleHC')) {
            closeModal();
        }
    }

</script>

<script type="text/javascript">
    
    var descriptionWasChanged = false;

    function ShowWarnings() {
        var result = true;
        <%--if (descriptionWasChanged) {
            alert($('#<%= Me.lblhelp.clientId %>').html());
        }--%>
        
        result=<%=validaHora() %>
        if (!result)
        return false;    
        if ($('#<%= Me.ddlRules.clientId %> option:selected').val() == 0) {
                result = confirm('<%= RateManager.PortalCulture.GetString("01351") %>');
        }
        return result;        
    }  

    function showRowPromotion(c1, c2, trComUni) {
        var e = document.getElementById(c1);
        var e2 = document.getElementById(c2);
        var e3 = document.getElementById('<%=chkGDS.clientid%>');
        var comUni = document.getElementById(trComUni);

        if (e.checked == true || e2.checked == true || e3.checked == true) {
            document.getElementById("trHeadProm").style.display = "block";
            document.getElementById("trHeadProm1").style.display = "inline";
            document.getElementById("trHeadProm2").style.display = "block";
        }
        else {
            document.getElementById("trHeadProm").style.display = "none";
            document.getElementById("trHeadProm1").style.display = "none";
            document.getElementById("trHeadProm2").style.display = "none";
        }
        if (e2.checked == true) {
            comUni.style.display = "block";
        }
        else {
            comUni.style.display = "none";
        }
        showcomisiones();
    }

    function showcomisiones() {
        var gds = document.getElementById('<%=chkGDS.clientid%>');
        var portal = document.getElementById('<%=chkPortal.clientid%>');
        var unipantalla = document.getElementById('<%=chkUnipantalla.clientid%>');
        var ads = document.getElementById('<%=chkADS.clientid%>');
        var renCom = document.getElementById('<%=trComisiones.clientid%>');
        if (gds.checked || portal.checked || unipantalla.checked || ads.checked) {
            renCom.style.display = "block";
        }
        else {
            renCom.style.display = "none";
        }
    }

    function onCheckBoxADS(chk, td) {
        var c = document.getElementById(chk);
        var t = document.getElementById(td);

        if (c && t) {
            t.style.display = c.checked ? "" : "none";
        }
        showcomisiones();
    }

    function showRowPortal(c1, c2, trComPor) {
        var e = document.getElementById(c1);
        var e2 = document.getElementById(c2);
        var comPor = document.getElementById(trComPor);
        var e3 = document.getElementById('<%=chkGDS.clientid%>');

        if (e.checked == true || e2.checked == true || e3.checked == true) {
            document.getElementById("trHeadProm").style.display = "block";
            document.getElementById("trHeadProm1").style.display = "inline";
            document.getElementById("trHeadProm2").style.display = "block";
        }
        else {
            document.getElementById("trHeadProm").style.display = "none";
            document.getElementById("trHeadProm1").style.display = "none";
            document.getElementById("trHeadProm2").style.display = "none";
        }

        if (e2.checked == true) {
            document.getElementById("trPortal").style.display = "";
            comPor.style.display = "";
        }
        else {
            document.getElementById("trPortal").style.display = "none";
            comPor.style.display = "none";
        }
        showcomisiones();
    }

    function onCheckBoxesClick(chk, td, trComGDS) {
        var c = document.getElementById(chk);
        var t = document.getElementById(td);
        var cGDS = document.getElementById(trComGDS);
        if (c && t) {
            t.style.display = c.checked ? "" : "none";
            cGDS.style.display = c.checked ? "" : "none";
        }
        showcomisiones();
        showRowPromotion('<%=chkPortal.ClientID%>','<%=chkUnipantalla.ClientID%>','<%=Me.trPorcUni.ClientID%>');
        
    }


    function showRowPortal99(chk, chkU, td) {
        var c = document.getElementById(chk);
        var u = document.getElementById(chkU);
        var t = document.getElementById(td);

        if (c.checked == true || u.checked == true) {
            document.getElementById("trHeadProm").style.display = "block";
            document.getElementById("trHeadProm1").style.display = "inline";
            document.getElementById("trHeadProm2").style.display = "block";
        }
        else {
            document.getElementById("trHeadProm").style.display = "none";
            document.getElementById("trHeadProm1").style.display = "none";
            document.getElementById("trHeadProm2").style.display = "none";
        }

        if (c && t)
            //		{			
            //			t.style.display = c.checked ? "" : "none";			
            //		}
        {
            document.getElementById("trPortal").style.display = "";
        }
        else {
            document.getElementById("trPortal").style.display = "none";
        }

    }

    function IniDate() {
        var fecha = new Date();
        var fecha2 = new Date(2030, 12, 31);
        var arr = new Array(3);
        arr[0] = [fecha.getFullYear(), fecha.getMonth() + 1, fecha.getDate()]
        arr[1] = [fecha2.getFullYear(), fecha2.getMonth() + 1, fecha2.getDate()];
        return arr;
    }
    function VerifyDate() {
        var txt1 = document.getElementById("txtInicio").value;
        var txt2 = document.getElementById("txtFinal").value;
    }

    function MostrarOcultarConf(table, check) {
        if (document.getElementById(check).checked == false) {
            document.getElementById(table).style.display = "none";
        }
        else {
            document.getElementById(table).style.display = ""
        }
    }
    function DesabilitarHabilitarHora(check, horainicio, minutoinicio, horafin, minutofin) {
        if (document.getElementById(check).checked == false) {
            document.getElementById(horainicio).disabled = true;
            document.getElementById(minutoinicio).disabled = true;
            document.getElementById(horafin).disabled = true;
            document.getElementById(minutofin).disabled = true;
        }
        else {
            document.getElementById(horainicio).disabled = false;
            document.getElementById(minutoinicio).disabled = false;
            document.getElementById(horafin).disabled = false;
            document.getElementById(minutofin).disabled = false;
        }
    }

    function validaHora(chkDeal, chKTime, horaIni, minutoIni, horaFin, minutoFin, msg) {
        
        var banTime = false;
        var hini = parseInt(document.getElementById(horaIni).value, 10);
        var hfin = parseInt(document.getElementById(horaFin).value, 10);
        var mini = parseInt(document.getElementById(minutoIni).value, 10);
        var mfin = parseInt(document.getElementById(minutoFin).value, 10);
        
        if (document.getElementById(chkDeal).checked == true) {
            if (document.getElementById(chKTime).checked == true) {

                if (hfin < hini) {
                    banTime = true;
                }
                else {
                    if (hfin == hini) {
                        if (mfin <= mini) {
                            banTime = true;
                        }
                    }
                }
                if (banTime == true) {
                    document.getElementById(msg).style.display = "block";
                    return false;
                }                
            }
        }        
        return true;
    }

    function ShowContPortals(id, pnl) {

        var e = document.getElementById(id);
        var p = document.getElementById(pnl);
        if (e && p) {
            p.style.display = !e.checked ? '' : 'none';
        }
    }

    function FireHiddeConfigDineroMail(v, id, hS, hH) {
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

    function FireNoneNetRate(lbl, pnl, sel) {
        var e = document.getElementById(lbl);
        var p = document.getElementById(pnl);
        var s = document.getElementById(sel);
        if (e && p && s) {
            e.style.display = p.style.display = (s.selectedIndex == 0) ? "none":"";
        }
    }

    function HideHotelPayment(spanHotelPaymentId) {

        var spanHotelPayment = $('#' + spanHotelPaymentId);

        var contractList = $('#<%= Me.ddlContratosNR.ClientId %>');

        var value = contractList.val();

       <%-- console.log(spanHotelPayment);
        console.log(value);

        if (value == '0') {

            spanHotelPayment.attr("style", "display:block");

            //$('#<%= Me.hotelPayment.ClientId %>').attr('checked', false);

            }
            else {

            spanHotelPayment.attr("style", "display:none");

            $('#<%= Me.hotelPayment.ClientId %>').attr('checked', false);

        } --%>
    }

    <%--$(document).ready(function() {
        $('#<%= Me.txtDescripcion.ClientId %> textarea').change(function() {
            descriptionWasChanged = true;
        });
    });--%>
        
    $(document).ready( function() {
        var options = new Array();
        var list = $('#<%= Me.lstRatePlans.ClientId %>');
        var contractList = $('#<%= Me.ddlContratosNR.ClientId %>');
        var counterHotelPayment = 0;
        var isEdit = false;
        
        list.find('option:gt(0)').each(function() {
            var current = $(this);
            var value = eval('(' + current.val() + ')');
            current.html(value.code + ' -- ' + value.name);
            options.push(current);            
        });
        list.data('options', options);
        
        
        list.bind('filter', function(e, contract) {
        
            var selected = null;
            if(list.find('option:gt(0):selected').length > 0) 
                selected = eval('(' + list.find('option:selected').val() + ')');
            
            list.find('option:selected').removeAttr('selected');
            list.find('option:gt(0)').remove();
            
            $.each(list.data('options'), function() {
                var current = $(this);
                current.removeAttr('selected');
                var value = eval('(' + current.val() + ')');
                //if(value.contract == contract){
                if( (value.contract > 0) == (contract > 0) ){
                    list.append(this);
                    if(selected != null && value.id == selected.id)
                        current.attr('selected','selected');
                }
            });
            if(list.find('option:selected').length == 0)
                list.find('option:eq(0)').attr('selected', 'selected');
                
        });   
        
        contractList.change(function() {
            list.trigger('filter', [(this.selectedIndex > 0)]);

            console.log('Primer Evento')

            console.log(counterHotelPayment);


            if (counterHotelPayment == 0) {

                var isChecked = $('#<%= Me.hotelPayment.ClientId %>').is(':checked');
                var value = $(this).val();

                //Saber si es Nuevo o se esta Editando
                if (value != '0' && isChecked) {

                    $('#spanHotelPayment').attr("style", "display:block");
                    isEdit = true;
                }
                else if (value != '0' && !isChecked) {
                    $('#spanHotelPayment').attr("style", "display:none");
                    isEdit = true;
                }

                counterHotelPayment++;

                console.log(counterHotelPayment);


            }
            else {

                console.log('despues')

                var value = $(this).val();

                var spanHotelPayment = $('#spanHotelPayment');

                if (value == '0') {

                    spanHotelPayment.attr("style", "display:block");

                    //$('#<%= Me.hotelPayment.ClientId %>').attr('checked', false);

                }
                else {

                    spanHotelPayment.attr("style", "display:none");

                    $('#<%= Me.hotelPayment.ClientId %>').attr('checked', false);

                }

                counterHotelPayment++;

            }

                
        });
        
        contractList.change();

        $('#<%= Me.hotelPayment.ClientId %>').change(function () {

            console.log(counterHotelPayment);

            if (isEdit && counterHotelPayment == 1) {

                $('#spanHotelPayment').attr("style", "display:none");
            }
            
        });

    });
    
    

</script>
<asp:HiddenField ID="strError" runat="server" />

<div id="msgGoogleHC" class="modal">
    <!-- Modal content -->
    <div class="modal-content">
        <div class="modal-header">
            <span class="close" onclick="closeModal()">&times;</span>
            <h2 id="msgGHC_Title">Seleccion de opcion para Google Hotel Center</h2>
        </div>
        <div class="modal-body">
            <h4 class="msg">Actualmente esta característica solo aplicará para el envío de información hacía Google Hotel Center. Posteriormente se implementarán validaciones que aplicarán en Call Center y Motor de reservaciones de Internet Power Hotel.</h4>
        </div>
        <div class="modal-footer">
            <input type="button" value="Aceptar" onclick="closeModal();" />
        </div>
    </div>
</div>

<table id="Table1" class="Form" cellspacing="1" cellpadding="1" width="100%" border="0">
    <tbody>

        <tr>
            <td width="18%" align="right">
                <asp:Label ID="lblRatecode" EnableViewState="False" CssClass="clsLabel" runat="server">Rate Code</asp:Label></td>
            <td width="34%">
                <asp:TextBox ID="txtRateCode" runat="server" MaxLength="4" Columns="4"></asp:TextBox><asp:RequiredFieldValidator ID="RfvRateCode" CssClass="clsvalidators" runat="server" Display="Dynamic" ErrorMessage="*"
                    ControlToValidate="txtRateCode">*</asp:RequiredFieldValidator></td>
            <td width="155" align="right" valign="top">
                <asp:Label ID="lblMoneda" runat="server" EnableViewState="False" CssClass="clsLabel" Visible="False">Moneda :</asp:Label></td>
            <td width="35%" align="left" valign="top">
                <asp:DropDownList ID="cmbMonedas" runat="server" Width="155px" Visible="False"></asp:DropDownList></td>
        </tr>

        <tr>
            <td colspan="4" align="center">
                <asp:Label Style="display: none;" ID="lblhelp" runat="server" Text="Se puede agregar contenido" CssClass="clsHelpLabel"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="lblname" EnableViewState="False" CssClass="clslabel" runat="server">Nombre</asp:Label></td>
            <td>
                <uc1:CtrlIdioma ID="txtShortDescription" runat="server"></uc1:CtrlIdioma>
            </td>
            <td align="right">
                <asp:Label ID="lblDescripcion" EnableViewState="False" CssClass="clsLabel" runat="server">Descripción</asp:Label><br />
            </td>
            <td align="left">
                <uc1:CtrlIdioma ID="txtDescripcion" runat="server"></uc1:CtrlIdioma>
            </td>
        </tr>
        <tr>
            <td></td>
            <td colspan="3" align="center"></td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="lblSegmento" EnableViewState="False" CssClass="clsLabel" runat="server">Segmento</asp:Label></td>
            <td>
                <asp:DropDownList ID="ddlSegmentos" runat="server" Width="140px"></asp:DropDownList></td>
            <td align="right">
                <asp:Label ID="lblRule" EnableViewState="False" CssClass="clsLabel" runat="server">Rule</asp:Label></td>
            <td align="left">
                <asp:DropDownList ID="ddlRules" runat="server"></asp:DropDownList></td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="lblAsignarPais" EnableViewState="False" CssClass="clsLabel" runat="server">Asignar País</asp:Label>
            </td>
            <td>
                <asp:CheckBoxList runat="server" ID="chklCountries" RepeatDirection="Horizontal">
                    <asp:ListItem Value="CA" Selected="True">CAN</asp:ListItem>
                    <asp:ListItem Value="US" Selected="True">EUA</asp:ListItem>
                    <asp:ListItem Value="MX" Selected="True">MEX</asp:ListItem>
                    <asp:ListItem Value="ALL" Selected="True">Resto del mundo</asp:ListItem>
                </asp:CheckBoxList>
            </td>
            <td>
            </td>
            <td>

                <span id="spanHotelPayment">
                    <asp:CheckBox ID="hotelPayment" CssClass="clslabel" runat="server" Text="Hotel Payment"></asp:CheckBox>
                    <asp:CheckBox ID="portalMovil" CssClass="clslabel" runat="server" Text="Its movil" ></asp:CheckBox>
                    <asp:CheckBox ID="onlyCC" CssClass="clslabel" runat="server" Text="Call Center Exclusive" ></asp:CheckBox>
                </span>
            </td>
        </tr>
        <%If Me.HasData AndAlso (Not Me.txtDescripcion.Published OrElse Not Me.txtShortDescription.Published OrElse Not Me.txtPromoDescription.Published) Then%>
        <tr>
            <td>&nbsp;</td>
            <td colspan="3" align="center"></td>
        </tr>
        <tr>
            <td align="right"><%=RateManager.PortalCulture.GetString(If(CType(Me.Page, RateManager.PaginaBase).IsSupervisor, "01365", "01392"))%>:</td>
            <td colspan="3">
                <asp:DropDownList ID="lstRatePlans" runat="server" Style="width: 300px;"></asp:DropDownList></td>
        </tr>
        <% End If%>
        <tr>
            <td>&nbsp;</td>
            <td colspan="3" align="center"></td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="lblAccessCode" CssClass="clsLabel" runat="server">Access Code</asp:Label></td>
            <td align="left">
                <asp:TextBox ID="txtAccessCode" CssClass="textbox" runat="server" MaxLength="10" Columns="10"></asp:TextBox></td>
            <td align="right">
                <asp:Label Style="Z-INDEX: 0" ID="lblCD" CssClass="clsLabel" runat="server">CD:</asp:Label></td>
            <td align="left">
                <asp:TextBox ID="txtCD" CssClass="textbox" runat="server" MaxLength="25" Width="180px"></asp:TextBox></td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="lblRateApply" EnableViewState="False" CssClass="clslabel" runat="server">aplicar rate plan para:</asp:Label></td>
            <td>
                <asp:CheckBox ID="chkGDS" CssClass="clslabel" runat="server" Text="GDS"></asp:CheckBox><asp:CheckBox ID="chkPortal" CssClass="clslabel" runat="server" Text="Portal"></asp:CheckBox><asp:CheckBox ID="chkUnipantalla" CssClass="clslabel" runat="server" Text="Unipantalla"></asp:CheckBox><asp:CheckBox ID="chkADS" runat="server" Text="ADS"></asp:CheckBox></td>
            <td align="right">
                <asp:Label Style="Z-INDEX: 0" ID="lblOrden" runat="server">Orden</asp:Label></td>
            <td valign="middle" align="left">
                <table id="Table2" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td>
                            <asp:DropDownList ID="ddlOrden" runat="server"></asp:DropDownList></td>
                        <td>
                            <asp:RangeValidator ID="rvOrden" CssClass="validators" runat="server" ErrorMessage="Sólo Números" ControlToValidate="ddlOrden"
                                MinimumValue="1" MaximumValue="999" Type="Integer"></asp:RangeValidator></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td></td>
            <td colspan="3">
                <asp:CheckBox ID="chkWaitListAvailable" runat="server" Visible="True" />
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label Style="Z-INDEX: 0" ID="lblComision" runat="server">Comisiones:</asp:Label></td>
            <td></td>
            <td align="right">
                <asp:Label Style="Z-INDEX: 0" ID="lblContratos" runat="server">Contrato Tarifa Neta:</asp:Label></td>
            <td valign="middle" align="left">
                <asp:DropDownList Style="Z-INDEX: 0" ID="ddlContratosNR" runat="server" Width="176px"></asp:DropDownList><asp:TextBox Style="Z-INDEX: 0" ID="TextBoxTarifaComisionable" runat="server" Width="49px">0</asp:TextBox></td>
        </tr>
        <asp:Panel ID="tipoPago" runat="server" Visible="false">
        <tr>
             <td align="right">
            <asp:Label ID="lblTipoPago" runat="server" EnableViewState="False">Tipo de Pago :</asp:Label>
        </td>
        <td>
            <asp:DropDownList ID="ddlTipoPago" runat="server" Width="150px"></asp:DropDownList>
        </td>
        </tr>
        </asp:Panel>
        <tr>
            <td id="trComisiones" runat="server" valign="top"></td>
            <td>
                <table width="100%" style="">
                    <tr id="trPorcGDS" runat="server">
                        <td width="25%">
                            <asp:Label ID="lblPorcGDS" runat="server">GDS</asp:Label></td>
                        <td>
                            <asp:TextBox ID="txtPorcGDS" runat="server" MaxLength="3" Width="32px"></asp:TextBox>%</td>
                    </tr>
                    <tr id="trPorcUni" runat="server">
                        <td width="25%">
                            <asp:Label ID="lblPorcUni" runat="server">UniPantalla</asp:Label></td>
                        <td>
                            <asp:TextBox ID="txtPorcUNI" runat="server" MaxLength="3" Width="32px"></asp:TextBox>%</td>
                    </tr>
                    <tr id="trPorcPortal" runat="server">
                        <td width="25%">
                            <asp:Label ID="lblPorcPortal" runat="server">Portal</asp:Label></td>
                        <td>
                            <asp:TextBox ID="txtPorcPOR" runat="server" MaxLength="3" Width="32px"></asp:TextBox>%</td>
                    </tr>
                    <tr id="trPorcADS" runat="server">
                        <td width="25%">
                            <asp:Label ID="lblPorcADS" runat="server">ADS</asp:Label></td>
                        <td>
                            <asp:TextBox ID="txtPorcADS" runat="server" MaxLength="3" Width="32px"></asp:TextBox>%</td>
                    </tr>
                </table>

            </td>
            <td valign="top" colspan="2">
                <asp:CheckBox Style="Z-INDEX: 0" ID="CheckBoxDeal" runat="server" Text="Oferta"></asp:CheckBox>
                <br />

                <table id="tableConfDeal" runat="server">
                    <tr>
                        <td align="left">
                            <asp:Label ID="lblFrom" EnableViewState="False" CssClass="clslabel" runat="server">Desde</asp:Label><a hidefocus onclick="if(self.gfPop)gfPop.fPopCalendar(<%=txtFinal.clientId%>,<%=txtInicio.clientId%>,IniDate());return false;" href="javascript:void(0)"><asp:TextBox ID="txtInicio" CssClass="textbox" runat="server" MaxLength="10" Width="75px" Columns="10"></asp:TextBox></a><a hidefocus onclick="if(self.gfPop)gfPop.fPopCalendar(<%=txtFinal.clientId%>,<%=txtInicio.clientId%>,IniDate());return false;" href="javascript:void(0)"><img class="PopcalTrigger" border="0"
                                alt="" align="absMiddle"
                                src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>'></a>
                        </td>
                        <td align="left">
                            <asp:Label Style="Z-INDEX: 0" ID="lblTo" EnableViewState="False" CssClass="clslabel" runat="server">Hasta</asp:Label><a hidefocus onclick="if(self.gfPop)gfPop.fPopCalendar1(<%=txtFinal.clientId%>,IniDate());return false;" href="javascript:void(0)"><asp:TextBox Style="Z-INDEX: 0" ID="txtFinal" CssClass="textbox" runat="server" MaxLength="10"
                                Width="75px" Columns="10"></asp:TextBox></a><a hidefocus onclick="if(self.gfPop)gfPop.fPopCalendar1(<%=txtFinal.clientId%>,IniDate());return false;" href="javascript:void(0)"><img style="Z-INDEX: 0"
                                    class="PopcalTrigger" border="0" alt="" align="absMiddle"
                                    src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>'></a></td>
                    </tr>
                    <tr>
                        <td style="height: 21px" align="left" colspan="2">
                            <asp:CheckBox Style="Z-INDEX: 0" ID="CheckBoxDefHora" runat="server" Text="Definir Hora"></asp:CheckBox></td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label Style="Z-INDEX: 0" ID="lblFromHora" runat="server" CssClass="clslabel" EnableViewState="False">Desde</asp:Label><asp:DropDownList ID="HoraInicio" runat="server" Enabled="False">
                                <asp:ListItem Value="00">00</asp:ListItem>
                                <asp:ListItem Value="01">01</asp:ListItem>
                                <asp:ListItem Value="02">02</asp:ListItem>
                                <asp:ListItem Value="03">03</asp:ListItem>
                                <asp:ListItem Value="04">04</asp:ListItem>
                                <asp:ListItem Value="05">05</asp:ListItem>
                                <asp:ListItem Value="06">06</asp:ListItem>
                                <asp:ListItem Value="07">07</asp:ListItem>
                                <asp:ListItem Value="08">08</asp:ListItem>
                                <asp:ListItem Value="09">09</asp:ListItem>
                                <asp:ListItem Value="10">10</asp:ListItem>
                                <asp:ListItem Value="11">11</asp:ListItem>
                                <asp:ListItem Value="12">12</asp:ListItem>
                                <asp:ListItem Value="13">13</asp:ListItem>
                                <asp:ListItem Value="14">14</asp:ListItem>
                                <asp:ListItem Value="15">15</asp:ListItem>
                                <asp:ListItem Value="16">16</asp:ListItem>
                                <asp:ListItem Value="17">17</asp:ListItem>
                                <asp:ListItem Value="18">18</asp:ListItem>
                                <asp:ListItem Value="19">19</asp:ListItem>
                                <asp:ListItem Value="20">20</asp:ListItem>
                                <asp:ListItem Value="21">21</asp:ListItem>
                                <asp:ListItem Value="22">22</asp:ListItem>
                                <asp:ListItem Value="23">23</asp:ListItem>
                            </asp:DropDownList>:
							<asp:DropDownList Style="Z-INDEX: 0" ID="MinutoInicio" runat="server" Enabled="False">
                                <asp:ListItem Value="00">00</asp:ListItem>
                                <asp:ListItem Value="01">01</asp:ListItem>
                                <asp:ListItem Value="02">02</asp:ListItem>
                                <asp:ListItem Value="03">03</asp:ListItem>
                                <asp:ListItem Value="04">04</asp:ListItem>
                                <asp:ListItem Value="05">05</asp:ListItem>
                                <asp:ListItem Value="06">06</asp:ListItem>
                                <asp:ListItem Value="07">07</asp:ListItem>
                                <asp:ListItem Value="08">08</asp:ListItem>
                                <asp:ListItem Value="09">09</asp:ListItem>
                                <asp:ListItem Value="10">10</asp:ListItem>
                                <asp:ListItem Value="11">11</asp:ListItem>
                                <asp:ListItem Value="12">12</asp:ListItem>
                                <asp:ListItem Value="13">13</asp:ListItem>
                                <asp:ListItem Value="14">14</asp:ListItem>
                                <asp:ListItem Value="15">15</asp:ListItem>
                                <asp:ListItem Value="16">16</asp:ListItem>
                                <asp:ListItem Value="17">17</asp:ListItem>
                                <asp:ListItem Value="18">18</asp:ListItem>
                                <asp:ListItem Value="19">19</asp:ListItem>
                                <asp:ListItem Value="20">20</asp:ListItem>
                                <asp:ListItem Value="21">21</asp:ListItem>
                                <asp:ListItem Value="22">22</asp:ListItem>
                                <asp:ListItem Value="23">23</asp:ListItem>
                                <asp:ListItem Value="24">24</asp:ListItem>
                                <asp:ListItem Value="25">25</asp:ListItem>
                                <asp:ListItem Value="26">26</asp:ListItem>
                                <asp:ListItem Value="27">27</asp:ListItem>
                                <asp:ListItem Value="28">28</asp:ListItem>
                                <asp:ListItem Value="29">29</asp:ListItem>
                                <asp:ListItem Value="30">30</asp:ListItem>
                                <asp:ListItem Value="31">31</asp:ListItem>
                                <asp:ListItem Value="32">32</asp:ListItem>
                                <asp:ListItem Value="33">33</asp:ListItem>
                                <asp:ListItem Value="34">34</asp:ListItem>
                                <asp:ListItem Value="35">35</asp:ListItem>
                                <asp:ListItem Value="36">36</asp:ListItem>
                                <asp:ListItem Value="37">37</asp:ListItem>
                                <asp:ListItem Value="38">38</asp:ListItem>
                                <asp:ListItem Value="39">39</asp:ListItem>
                                <asp:ListItem Value="40">40</asp:ListItem>
                                <asp:ListItem Value="41">41</asp:ListItem>
                                <asp:ListItem Value="42">42</asp:ListItem>
                                <asp:ListItem Value="43">43</asp:ListItem>
                                <asp:ListItem Value="44">44</asp:ListItem>
                                <asp:ListItem Value="45">45</asp:ListItem>
                                <asp:ListItem Value="46">46</asp:ListItem>
                                <asp:ListItem Value="47">47</asp:ListItem>
                                <asp:ListItem Value="48">48</asp:ListItem>
                                <asp:ListItem Value="49">49</asp:ListItem>
                                <asp:ListItem Value="50">50</asp:ListItem>
                                <asp:ListItem Value="51">51</asp:ListItem>
                                <asp:ListItem Value="52">52</asp:ListItem>
                                <asp:ListItem Value="53">53</asp:ListItem>
                                <asp:ListItem Value="54">54</asp:ListItem>
                                <asp:ListItem Value="55">55</asp:ListItem>
                                <asp:ListItem Value="56">56</asp:ListItem>
                                <asp:ListItem Value="57">57</asp:ListItem>
                                <asp:ListItem Value="58">58</asp:ListItem>
                                <asp:ListItem Value="59">59</asp:ListItem>
                            </asp:DropDownList></td>
                        <td align="left">
                            <asp:Label Style="Z-INDEX: 0" ID="lblToHora" runat="server" CssClass="clslabel" EnableViewState="False">Hasta</asp:Label><asp:DropDownList Style="Z-INDEX: 0" ID="HoraFin" runat="server" Enabled="False">
                                <asp:ListItem Value="00">00</asp:ListItem>
                                <asp:ListItem Value="01">01</asp:ListItem>
                                <asp:ListItem Value="02">02</asp:ListItem>
                                <asp:ListItem Value="03">03</asp:ListItem>
                                <asp:ListItem Value="04">04</asp:ListItem>
                                <asp:ListItem Value="05">05</asp:ListItem>
                                <asp:ListItem Value="06">06</asp:ListItem>
                                <asp:ListItem Value="07">07</asp:ListItem>
                                <asp:ListItem Value="08">08</asp:ListItem>
                                <asp:ListItem Value="09">09</asp:ListItem>
                                <asp:ListItem Value="10">10</asp:ListItem>
                                <asp:ListItem Value="11">11</asp:ListItem>
                                <asp:ListItem Value="12">12</asp:ListItem>
                                <asp:ListItem Value="13">13</asp:ListItem>
                                <asp:ListItem Value="14">14</asp:ListItem>
                                <asp:ListItem Value="15">15</asp:ListItem>
                                <asp:ListItem Value="16">16</asp:ListItem>
                                <asp:ListItem Value="17">17</asp:ListItem>
                                <asp:ListItem Value="18">18</asp:ListItem>
                                <asp:ListItem Value="19">19</asp:ListItem>
                                <asp:ListItem Value="20">20</asp:ListItem>
                                <asp:ListItem Value="21">21</asp:ListItem>
                                <asp:ListItem Value="22">22</asp:ListItem>
                                <asp:ListItem Value="23">23</asp:ListItem>
                            </asp:DropDownList>:
							<asp:DropDownList Style="Z-INDEX: 0" ID="MinutoFin" runat="server" Enabled="False">
                                <asp:ListItem Value="00">00</asp:ListItem>
                                <asp:ListItem Value="01">01</asp:ListItem>
                                <asp:ListItem Value="02">02</asp:ListItem>
                                <asp:ListItem Value="03">03</asp:ListItem>
                                <asp:ListItem Value="04">04</asp:ListItem>
                                <asp:ListItem Value="05">05</asp:ListItem>
                                <asp:ListItem Value="06">06</asp:ListItem>
                                <asp:ListItem Value="07">07</asp:ListItem>
                                <asp:ListItem Value="08">08</asp:ListItem>
                                <asp:ListItem Value="09">09</asp:ListItem>
                                <asp:ListItem Value="10">10</asp:ListItem>
                                <asp:ListItem Value="11">11</asp:ListItem>
                                <asp:ListItem Value="12">12</asp:ListItem>
                                <asp:ListItem Value="13">13</asp:ListItem>
                                <asp:ListItem Value="14">14</asp:ListItem>
                                <asp:ListItem Value="15">15</asp:ListItem>
                                <asp:ListItem Value="16">16</asp:ListItem>
                                <asp:ListItem Value="17">17</asp:ListItem>
                                <asp:ListItem Value="18">18</asp:ListItem>
                                <asp:ListItem Value="19">19</asp:ListItem>
                                <asp:ListItem Value="20">20</asp:ListItem>
                                <asp:ListItem Value="21">21</asp:ListItem>
                                <asp:ListItem Value="22">22</asp:ListItem>
                                <asp:ListItem Value="23">23</asp:ListItem>
                                <asp:ListItem Value="24">24</asp:ListItem>
                                <asp:ListItem Value="25">25</asp:ListItem>
                                <asp:ListItem Value="26">26</asp:ListItem>
                                <asp:ListItem Value="27">27</asp:ListItem>
                                <asp:ListItem Value="28">28</asp:ListItem>
                                <asp:ListItem Value="29">29</asp:ListItem>
                                <asp:ListItem Value="30">30</asp:ListItem>
                                <asp:ListItem Value="31">31</asp:ListItem>
                                <asp:ListItem Value="32">32</asp:ListItem>
                                <asp:ListItem Value="33">33</asp:ListItem>
                                <asp:ListItem Value="34">34</asp:ListItem>
                                <asp:ListItem Value="35">35</asp:ListItem>
                                <asp:ListItem Value="36">36</asp:ListItem>
                                <asp:ListItem Value="37">37</asp:ListItem>
                                <asp:ListItem Value="38">38</asp:ListItem>
                                <asp:ListItem Value="39">39</asp:ListItem>
                                <asp:ListItem Value="40">40</asp:ListItem>
                                <asp:ListItem Value="41">41</asp:ListItem>
                                <asp:ListItem Value="42">42</asp:ListItem>
                                <asp:ListItem Value="43">43</asp:ListItem>
                                <asp:ListItem Value="44">44</asp:ListItem>
                                <asp:ListItem Value="45">45</asp:ListItem>
                                <asp:ListItem Value="46">46</asp:ListItem>
                                <asp:ListItem Value="47">47</asp:ListItem>
                                <asp:ListItem Value="48">48</asp:ListItem>
                                <asp:ListItem Value="49">49</asp:ListItem>
                                <asp:ListItem Value="50">50</asp:ListItem>
                                <asp:ListItem Value="51">51</asp:ListItem>
                                <asp:ListItem Value="52">52</asp:ListItem>
                                <asp:ListItem Value="53">53</asp:ListItem>
                                <asp:ListItem Value="54">54</asp:ListItem>
                                <asp:ListItem Value="55">55</asp:ListItem>
                                <asp:ListItem Value="56">56</asp:ListItem>
                                <asp:ListItem Value="57">57</asp:ListItem>
                                <asp:ListItem Value="58">58</asp:ListItem>
                                <asp:ListItem Value="59">59</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="HEIGHT: 21px" colspan="2">
                            <asp:Label Style="Z-INDEX: 0" ID="timevalidator" runat="server" CssClass="Validators" EnableViewState="False">Hora Invalida</asp:Label></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td align="left"></td>
                    </tr>
                </table>
            </td>
        </tr>

        <tr id="trGDSApply" runat="server">
            <td align="right">
                <asp:Label ID="lblAplicaGDS" EnableViewState="False" runat="server">Aplica Para GDS</asp:Label>:</td>
            <td id="tdApplyGDS" valign="top" colspan="2" runat="server">
                <br>
                <table style="WIDTH: 100%;" border="0" cellspacing="1" cellpadding="1" width="240">
                    <tr>
                        <td style="WIDTH: 25%">
                            <asp:CheckBox ID="chkGDSAmadeus" runat="server" Text="Amadeus"></asp:CheckBox></td>
                        <td style="WIDTH: 25%">
                            <asp:CheckBox ID="chkGDSGalileo" runat="server" Text="Galileo"></asp:CheckBox></td>
                        <td style="WIDTH: 25%">
                            <asp:CheckBox ID="chkGDSSabre" runat="server" Text="Sabre"></asp:CheckBox></td>
                        <td style="WIDTH: 25%">
                            <asp:CheckBox ID="chkGDSWorldSpan" runat="server" Text="WorldSpan"></asp:CheckBox></td>
                    </tr>
                </table>
            </td>
            <td style="HEIGHT: 60px" valign="top" align="left"></td>
        </tr>
        <tr id="trPortal">
            <td id="" align="right" valign="top">
                <asp:Label ID="lblPortal" runat="server">Portales</asp:Label>:</td>
            <td colspan="3">
                <uc1:ctrPortal ID="CtrPortal1" runat="server"></uc1:ctrPortal>
            </td>
        </tr>


        <tr>
            <td align="right">
                <asp:Label ID="lblDeposittitle" runat="server">Si acepta depósito defina el tipo</asp:Label></td>
            <td colspan="3">
                <asp:RadioButton ID="RdbNone" runat="server" Checked="True" GroupName="deposittype"></asp:RadioButton>&nbsp;&nbsp;
				<asp:RadioButton ID="RdbOneNigth" runat="server" GroupName="deposittype"></asp:RadioButton>&nbsp;&nbsp;
				<asp:RadioButton ID="rdbAlltotal" runat="server" GroupName="deposittype"></asp:RadioButton></td>
        </tr>
        <tr>
            <td></td>
            <td class="dgItem" colspan="3" align="center">
                <div id="trHeadProm">
                    <asp:Label ID="lblPromotiontitle" CssClass="clslabel" runat="server">Configuración de promociones para portales</asp:Label>
                </div>
            </td>
        </tr>

        <tr>
            <td></td>
            <td colspan="3">
                <table id="trHeadProm1">
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblGratis" EnableViewState="False" runat="server">Noches Gratis </asp:Label></td>
                        <td colspan="2" align="left">
                            <asp:TextBox ID="txtDaysFree" CssClass="textbox" runat="server" MaxLength="2" Width="48px"></asp:TextBox><asp:Label ID="lblFreeNights" EnableViewState="False" runat="server">Noche</asp:Label></td>
                        <td></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td></td>
            <td colspan="3">
                <table id="trHeadProm2">
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblPromotion" EnableViewState="False" runat="server">Promotion </asp:Label></td>
                        <td colspan="2" align="left">
                            <asp:TextBox ID="txtDescProm" CssClass="textbox" runat="server" MaxLength="5" Width="48px"></asp:TextBox><asp:Label ID="lblDesc" EnableViewState="False" runat="server">%</asp:Label></td>
                        <td></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="right"><%  Response.Write("<script>  showRowPromotion('" & chkPortal.ClientID & "','" & chkUnipantalla.ClientID & "','" & trPorcUni.ClientID & "');</script>")%></td>
            <td colspan="3">
                <uc1:CtrlIdioma IsMultiline="false" MaxLength="80" RequiredText="false" ID="txtPromoDescription" runat="server"></uc1:CtrlIdioma>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <asp:RangeValidator ID="RVNochesgratis" CssClass="validators" runat="server" ErrorMessage="Noche gratis debe ser numerico"
                    ControlToValidate="txtDaysFree" MinimumValue="0" MaximumValue="99" Type="Integer"></asp:RangeValidator><asp:RangeValidator ID="RVPromotion" CssClass="validators" runat="server" ErrorMessage="Descuento debe ser numerico"
                        ControlToValidate="txtDescProm" MinimumValue="0" MaximumValue="100" Type="Double"></asp:RangeValidator></td>
        </tr>
        <%If Me.HasData AndAlso (Not Me.txtDescripcion.Published OrElse Not Me.txtShortDescription.Published OrElse Not Me.txtPromoDescription.Published) Then%>
        <tr>
            <td colspan="4">
                <span class="validators"><%=RateManager.PortalCulture.GetString(If(CType(Me.Page, RateManager.PaginaBase).IsSupervisor, "01365", "01392"))%></span>
            </td>
        </tr>
        <% End If%>
    </tbody>
</table>

<script>

    function ShowDivNegotiates(Segment, txt, codes, el2, el3, el4, el5, load) {
        var S = (document.getElementById(Segment)).value;

        var e2 = document.getElementById(el2);
        var e3 = document.getElementById(el3);
        var e4 = document.getElementById(el4);
        var e5 = document.getElementById(el5);
        if (S != "N" && S != "C") {

            e2.style.display = 'none';
            e3.style.display = 'none';
            e4.style.display = 'none';
            e5.style.display = 'none';
        }
        else {

            e2.style.display = '';
            e3.style.display = '';
            e4.style.display = '';
            e5.style.display = '';
        }
        if (load != 1) {
            Name(Segment, txt, codes);
        }
    }
    function Name(ddl, txt, array) {
        var e = document.getElementById(ddl);
        var lbl = document.getElementById(txt);
        if (e != null && lbl != null && array != null) {
            lbl.value = array.split("//")[e.selectedIndex + 1];
        }
    }

	
</script>
<%  Response.Write("<script>  initGoogleChecks();</script>")%>
<%  Response.Write("<script>  showRowPromotion('" & chkPortal.ClientID & "','" & chkUnipantalla.ClientID & "','" & trPorcUni.ClientID & "');</script>")%>
<%  Response.Write("<script>  onCheckBoxesClick('" & chkGDS.ClientID & "','" & trGDSApply.ClientID & "','" & trPorcGDS.ClientID & "');</script>")%>
<%  Response.Write("<script>  showRowPortal('" & chkUnipantalla.ClientID & "','" & chkPortal.ClientID & "','" & trPorcPortal.ClientID & "');</script>")%>


<script>
    function ShowNegotiates() {
        var text = document.getElementById('<%=txtAccessCode.ClientID%>');
        var lbl = document.getElementById('<%=lblAccessCode.ClientID%>');
        var S = (document.getElementById('<%=ddlSegmentos.ClientID%>')).value;

        if (S == "N" || S == "C") {
            text.style.display = '';
            lbl.style.display = '';
        }
        else {
            text.style.display = 'none';
            lbl.style.display = 'none';
        }
    }
    try {
        ShowNegotiates();

    }

    catch (ex) { }

</script>

