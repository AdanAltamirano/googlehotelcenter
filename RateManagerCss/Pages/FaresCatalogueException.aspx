<%@ Register TagPrefix="uc1" TagName="ctrlPlanFares" Src="../Modulos/ctrlPlanFares.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrlPlanFaresExc" Src="../Modulos/CtrlPlanFaresExc.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctlMensajes" Src="../Modulos/ctlMensajes.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtlReservaMsj" Src="../Modulos/CtlReservaMsj.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="FaresCatalogueException.aspx.vb"
    Inherits="RateManager.FaresCatalogueException" %>

<%@ Register TagPrefix="uc1" TagName="ctrlFaresExc" Src="../Modulos/ctrlFaresExc.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>FaresCatalogueException</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defau">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet"></link>
    <script type="text/javascript" src="../Includes/Script/jquery-1.4.2.min.js"></script>
    
    <style>
        #tblControles TD
        {
            border-right: silver 1px solid;
            border-top: silver 1px solid;
            border-left: silver 1px solid;
            border-bottom: silver 1px solid;
            vertical-align:top; 
        }
        #tblCtrlHomePage TD
        {
            border-right: 0px;
            border-top: 0px;
            border-left: 0px;
            border-bottom: 0px;
        }
    </style>
    <script type="text/javascript">

        function FireValRoomsPlan(room, plan) {
            var e = document.getElementById(room);
            var p = document.getElementById(plan);
            var vr = document.getElementById('lblNoroomSelected')
            var vp = document.getElementById('lblNoPlanRateSelected')
            var hr = true;
            if (e && vr) {
                vr.style.display = (e.selectedIndex == 0) ? 'inline' : 'none';
                if (e.selectedIndex == 0) hr= false;
            }
            if (p && vp) {
                vp.style.display = (p.selectedIndex == 0) ? 'inline' : 'none';
                if (p.selectedIndex == 0) hr= false;
            }
            if (hr) $('#loadingProcess').show();
            return hr;
        }


    </script>
</head>
<body bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <div class="clear">
        <div class="mDiv">
        </div>
        <div class="title">
            <asp:Label ID="lblTitle" runat="server" EnableViewState="False" Text="Rooms setup"
                CssClass="tituloSeccion"></asp:Label>
        </div>
        
    </div>
     <div id="loadingProcess" >
    </div>
    <div id="modalPage2" style="display: none; left: 0px; width: 100%; position: absolute;
        top: 0px; height: 100%" align="left">
        <div style="filter: Alpha(Opacity=5); width: 100%; position: absolute; height: 134.96%;">
        </div>
        <div class="datagrid" id="modalPage" style="border-right: black 1px solid; padding-right: 0px;
            border-top: black 1px solid; display: none; padding-left: 0px; left: 16%; border-left: black 1px solid;
            border-bottom: black 1px solid; position: absolute; top: 24%; background-color: #f5f5f5"
            align="left" runat="server">
            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                <tr class="" style="background-color: #7eaecb; text-align: center;">
                    <td colspan="2">
                        <asp:Label ID="lblPreciosTarifa" runat="server" Font-Bold="true" CssClass="clsLabel"
                            EnableViewState="False">Tarifas <br>  Defina el precio de tarifa en ocupacion por adulto y el precio para niños, el campo total de tarifas mostrara el precio calculado por noche en habitación.</asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2" nclass="DataGridAlternatedItem">
                        <p>
                            <uc1:ctrlPlanFares ID="Ctrlplanfares1" runat="server"></uc1:ctrlPlanFares>
                        </p>
                        <input id="currentprices" type="hidden">
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: center; height: 14px;">
                        <asp:Label ID="lblValidPriceRes" runat="server" CssClass="validators" EnableViewState="false">Select room and load data</asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; width: 50%">
                        <asp:HyperLink ID="lnkCloseShowRates" runat="server" CssClass="dglink" ImageUrl="~/Includes/imagenes/ok_.jpg"
                            Style="padding-right: 12px;">[Cerrar]</asp:HyperLink>
                    </td>
                    <td style="width: 50%;">
                        <asp:HyperLink ID="lnkCancel" runat="server" CssClass="dglink" ImageUrl="~/Includes/imagenes/Close.ico"
                            Style="padding-left: 12px;">[Cancelar]</asp:HyperLink>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <table id="bookingcontainer" cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td>
                <asp:Label ID="lbltitleDate" runat="server" CssClass="bookingnormallabel" EnableViewState="False">Select date</asp:Label>
            </td>
            <td>
                <asp:Label ID="lblEName" runat="server" CssClass="bookingnormallabel" EnableViewState="False">Name:</asp:Label>
            </td>
            <td>
                <asp:Label ID="lblERatesPlans" runat="server" CssClass="bookingnormallabel" EnableViewState="False">Rate Plan:</asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblMonth" runat="server" CssClass="clslabel" EnableViewState="False" style= " padding-left: 20px; ">Month</asp:Label>                
                <asp:Label ID="lblYear" runat="server" CssClass="clslabel" EnableViewState="False" style= " padding-left: 64px; ">Year</asp:Label>
            </td>
            <td>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td>
                <asp:DropDownList ID="ddlMonth" runat="server">
                </asp:DropDownList>
                <asp:DropDownList ID="ddlyear" runat="server" style= " margin-left:30px; ">
                </asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList ID="ddlRooms" runat="server">
                </asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList ID="ddlratesplans" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td style=" height:16px; ">
                <asp:Label ID="lblNoroomSelected" runat="server" CssClass="validators" style="Display:none;">Select room and load data</asp:Label>
            </td>
            <td>
                <asp:Label ID="lblNoPlanRateSelected" runat="server" CssClass="validators" style="Display:none;">Select Plan Rate and load data</asp:Label>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
            </td>
            <td>
                <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                    <tr class="dgitem" valign="top">
                        <td colspan="3" style=" text-align: left; height:18px; padding-left:4px; " >
                            <asp:Label ID="lblEstatus" runat="server" CssClass="bookingnormallabel" EnableViewState="False">Estatus</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="text-align: left;">
                            <asp:Label ID="Label1" runat="server" CssClass="clslabel" EnableViewState="False">NA (No available)</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="" align="left" width="33.33%">
                            <asp:Label ID="lblA" runat="server" CssClass="clslabel" EnableViewState="False">A (Adult)</asp:Label>
                        </td>
                        <td align="left" width="33.33%">
                            <asp:Label ID="lblN" runat="server" CssClass="clslabel" EnableViewState="False">c (Child)</asp:Label>
                        </td>
                        <td width="33.33%">
                            <asp:Label ID="lblJ" runat="server" CssClass="clslabel" EnableViewState="False">J (Junior)</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="" align="left" width="33.33%">
                            <asp:Label ID="lblAE" runat="server" CssClass="clslabel" EnableViewState="False">EA (Extra Adult)</asp:Label>
                        </td>
                        <td align="left" width="33.33%">
                            <asp:Label ID="lblNE" runat="server" CssClass="clslabel" EnableViewState="False">EC Extra Child</asp:Label>
                        </td>
                        <td align="left" width="33.33%">
                            <asp:Label ID="lblJE" runat="server" CssClass="clslabel" EnableViewState="False">EJ Extra Junior</asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnLoad" runat="server" CssClass="button" Width="103px" Text="Load"
                    CausesValidation="False"></asp:Button>
            </td>
            <td colspan="2" style="padding-left: 120px;">
                <asp:Label ID="lblValidPrice" runat="server" CssClass="Validators" EnableViewState="false">Select room and load data</asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <table id="tblControles" cellspacing="0" cellpadding="0" width="100%" align="center"
                    border="0" style="vertical-align: top;">
                    <tr>
                        <td class="dgitem" align="center" colspan="7">
                            <asp:Label ID="lbltitletable" runat="server" CssClass="bookingnormallabel" EnableViewState="False">Label</asp:Label>
                        </td>
                    </tr>
                    <tr class="Titulo">
                        <td style="height: 14px" align="center" width="14.29%">
                            <asp:Label ID="lblDomingo" runat="server" EnableViewState="False">domingo</asp:Label>
                        </td>
                        <td style="height: 14px" align="center" width="14.29%">
                            <asp:Label ID="lblLunes" runat="server" EnableViewState="False">lunes</asp:Label>
                        </td>
                        <td style="height: 14px" align="center" width="14.29%">
                            <asp:Label ID="lblMartes" runat="server" EnableViewState="False">martes</asp:Label>
                        </td>
                        <td style="height: 14px" align="center" width="14.29%">
                            <asp:Label ID="lblMiercoles" runat="server" EnableViewState="False">miercoles</asp:Label>
                        </td>
                        <td style="height: 14px" align="center" width="14.29%">
                            <asp:Label ID="lblJueves" runat="server" EnableViewState="False">jueves</asp:Label>
                        </td>
                        <td style="height: 14px" align="center" width="14.29%">
                            <asp:Label ID="lblViernes" runat="server" EnableViewState="False">viernes</asp:Label>
                        </td>
                        <td style="height: 14px" align="center" width="14.29%">
                            <asp:Label ID="lblSabado" runat="server" EnableViewState="False">sabado</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <uc1:ctrlFaresExc ID="CtrlFaresExc1" runat="server"></uc1:ctrlFaresExc>
                        </td>
                        <td>
                            <uc1:ctrlFaresExc ID="CtrlFaresExc2" runat="server"></uc1:ctrlFaresExc>
                        </td>
                        <td>
                            <uc1:ctrlFaresExc ID="CtrlFaresExc3" runat="server"></uc1:ctrlFaresExc>
                        </td>
                        <td>
                            <uc1:ctrlFaresExc ID="CtrlFaresExc4" runat="server"></uc1:ctrlFaresExc>
                        </td>
                        <td>
                            <uc1:ctrlFaresExc ID="CtrlFaresExc5" runat="server"></uc1:ctrlFaresExc>
                        </td>
                        <td>
                            <uc1:ctrlFaresExc ID="CtrlFaresExc6" runat="server"></uc1:ctrlFaresExc>
                        </td>
                        <td>
                            <uc1:ctrlFaresExc ID="CtrlFaresExc7" runat="server"></uc1:ctrlFaresExc>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <uc1:ctrlFaresExc ID="CtrlFaresExc8" runat="server"></uc1:ctrlFaresExc>
                        </td>
                        <td>
                            <uc1:ctrlFaresExc ID="CtrlFaresExc9" runat="server"></uc1:ctrlFaresExc>
                        </td>
                        <td>
                            <uc1:ctrlFaresExc ID="CtrlFaresExc10" runat="server"></uc1:ctrlFaresExc>
                        </td>
                        <td>
                            <uc1:ctrlFaresExc ID="CtrlFaresExc11" runat="server"></uc1:ctrlFaresExc>
                        </td>
                        <td>
                            <uc1:ctrlFaresExc ID="CtrlFaresExc12" runat="server"></uc1:ctrlFaresExc>
                        </td>
                        <td>
                            <uc1:ctrlFaresExc ID="CtrlFaresExc13" runat="server"></uc1:ctrlFaresExc>
                        </td>
                        <td>
                            <uc1:ctrlFaresExc ID="CtrlFaresExc14" runat="server"></uc1:ctrlFaresExc>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <uc1:ctrlFaresExc ID="CtrlFaresExc15" runat="server"></uc1:ctrlFaresExc>
                        </td>
                        <td>
                            <uc1:ctrlFaresExc ID="CtrlFaresExc16" runat="server"></uc1:ctrlFaresExc>
                        </td>
                        <td>
                            <uc1:ctrlFaresExc ID="CtrlFaresExc17" runat="server"></uc1:ctrlFaresExc>
                        </td>
                        <td>
                            <uc1:ctrlFaresExc ID="CtrlFaresExc18" runat="server"></uc1:ctrlFaresExc>
                        </td>
                        <td>
                            <uc1:ctrlFaresExc ID="CtrlFaresExc19" runat="server"></uc1:ctrlFaresExc>
                        </td>
                        <td>
                            <uc1:ctrlFaresExc ID="CtrlFaresExc20" runat="server"></uc1:ctrlFaresExc>
                        </td>
                        <td>
                            <uc1:ctrlFaresExc ID="CtrlFaresExc21" runat="server"></uc1:ctrlFaresExc>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <uc1:ctrlFaresExc ID="CtrlFaresExc22" runat="server"></uc1:ctrlFaresExc>
                        </td>
                        <td>
                            <uc1:ctrlFaresExc ID="CtrlFaresExc23" runat="server"></uc1:ctrlFaresExc>
                        </td>
                        <td>
                            <uc1:ctrlFaresExc ID="CtrlFaresExc24" runat="server"></uc1:ctrlFaresExc>
                        </td>
                        <td>
                            <uc1:ctrlFaresExc ID="CtrlFaresExc25" runat="server"></uc1:ctrlFaresExc>
                        </td>
                        <td>
                            <uc1:ctrlFaresExc ID="CtrlFaresExc26" runat="server"></uc1:ctrlFaresExc>
                        </td>
                        <td>
                            <uc1:ctrlFaresExc ID="CtrlFaresExc27" runat="server"></uc1:ctrlFaresExc>
                        </td>
                        <td>
                            <uc1:ctrlFaresExc ID="CtrlFaresExc28" runat="server"></uc1:ctrlFaresExc>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <uc1:ctrlFaresExc ID="CtrlFaresExc29" runat="server"></uc1:ctrlFaresExc>
                        </td>
                        <td>
                            <uc1:ctrlFaresExc ID="CtrlFaresExc30" runat="server"></uc1:ctrlFaresExc>
                        </td>
                        <td>
                            <uc1:ctrlFaresExc ID="CtrlFaresExc31" runat="server"></uc1:ctrlFaresExc>
                        </td>
                        <td>
                            <uc1:ctrlFaresExc ID="CtrlFaresExc32" runat="server"></uc1:ctrlFaresExc>
                        </td>
                        <td>
                            <uc1:ctrlFaresExc ID="CtrlFaresExc33" runat="server"></uc1:ctrlFaresExc>
                        </td>
                        <td>
                            <uc1:ctrlFaresExc ID="CtrlFaresExc34" runat="server"></uc1:ctrlFaresExc>
                        </td>
                        <td>
                            <uc1:ctrlFaresExc ID="CtrlFaresExc35" runat="server"></uc1:ctrlFaresExc>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <uc1:ctrlFaresExc ID="CtrlFaresExc36" runat="server"></uc1:ctrlFaresExc>
                        </td>
                        <td>
                            <uc1:ctrlFaresExc ID="CtrlFaresExc37" runat="server"></uc1:ctrlFaresExc>
                        </td>
                        <td>
                            <uc1:ctrlFaresExc ID="CtrlFaresExc38" runat="server"></uc1:ctrlFaresExc>
                        </td>
                        <td>
                            <uc1:ctrlFaresExc ID="CtrlFaresExc39" runat="server"></uc1:ctrlFaresExc>
                        </td>
                        <td>
                            <uc1:ctrlFaresExc ID="CtrlFaresExc40" runat="server"></uc1:ctrlFaresExc>
                        </td>
                        <td>
                            <uc1:ctrlFaresExc ID="CtrlFaresExc41" runat="server"></uc1:ctrlFaresExc>
                        </td>
                        <td>
                            <uc1:ctrlFaresExc ID="CtrlFaresExc42" runat="server"></uc1:ctrlFaresExc>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:Button ID="btnSave" runat="server" CssClass="Button" EnableViewState="False"
                    Width="85px" Text="Guardar" Enabled="False"></asp:Button>
                <asp:TextBox ID="txtDivP" Style="display: none" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    </form>

    <script type="text/javascript">
        var defaultEmptyOK = false;
        var resource01471 = '<%= RateManager.PortalCulture.GetString("01471") %>'
        var resource00116 = '<%= RateManager.PortalCulture.GetString("00116") %>'

        function DisabledSelect(Disabled) {
            var ddlrooms = document.getElementById('ddlRooms')
            var ddlplanrates = document.getElementById('ddlratesplans')
            var ddlmonth = document.getElementById('ddlMonth')
            var ddlyears = document.getElementById('ddlyear')

            ddlrooms.disabled = Disabled;
            ddlplanrates.disabled = Disabled;
            ddlmonth.disabled = Disabled;
            ddlyears.disabled = Disabled;
        }

        function FireHide() {
            var r = document.getElementById('modalPage2');
            r.style.display = 'none';
            var e = document.getElementById('modalPage');
            e.style.display = 'none';
            DisabledSelect(false);
        }

        function ShowTxt(txt, lnk) {
            var l = document.getElementById(txt);
            l.style.display = ''
            var t = document.getElementById(lnk);
            t.style.display = "none"
        }
        function ShowTxt2(lnk, txt1, txt2, txt3, txt4, txt5, txt6, lb1, lb2, lb3, lb4, lb5, lb6, lnk2, hper, isJunior) {
            var hprs = document.getElementById(hper);
            var l = document.getElementById(lnk);
            l.style.display = "none"
            var t1 = document.getElementById(txt1);
            t1.style.display = ''
            if (hprs.value != '0') {
                var t2 = document.getElementById(txt2);
                t2.style.display = ''
                var l2 = document.getElementById(lb2);
                l2.style.display = ''
            }            
            var t3 = document.getElementById(txt3);
            t3.style.display = ''
            if (hprs.value != '0') {
                var t4 = document.getElementById(txt4);
                t4.style.display = ''
                var l4 = document.getElementById(lb4);
                l4.style.display = ''
            }            
            var l1 = document.getElementById(lb1);
            l1.style.display = ''
            
            var l3 = document.getElementById(lb3);
            l3.style.display = ''
          
            var ln = document.getElementById(lnk2);
            ln.style.display = ''
                        
            if (isJunior) {
                var l5 = document.getElementById(lb5);
                l5.style.display = ''               
                var t5 = document.getElementById(txt5);
                t5.style.display = ''
                if (hprs.value != '0') {
                    var t6 = document.getElementById(txt6);
                    t6.style.display = ''
                    var l6 = document.getElementById(lb6);
                    l6.style.display = ''
                }
            }
        }

        function CambiaTxt(txt) {
            document.getElementById(txt).value = "1";
        }

        function FillPrices(Dg, typeFare, Price, pr) {

            var P = document.getElementById(Price);
            var grid = document.getElementById(Dg);
            var item = grid.getElementsByTagName("tr");
            var pr2 = document.getElementById(pr);
            var prices = pr2.value;
            var arrprices = prices.split("|");
            var arrdultprices = arrprices[0];
            var arrchildprices = arrprices[1];
            var arrjuniorprices = arrprices[2];
            var tempPrice = "";
            var p1;

            for (var i = 1; i < item.length; i++) {
                var txt = item[i].getElementsByTagName("input");
                var lbl = item[i].getElementsByTagName("span");
                for (var j = 0; j < lbl.length; j++) {
                    for (var t = 0; t < txt.length; t++) {
                        if (txt[t].id.indexOf(typeFare) != -1) {
                            txt[t].value = P.value;
                            p1 = P.value;
                        }
                    }

                    if (lbl[j].id.indexOf("lblTotal") != -1) {
                        if (txt.length > 1) {
                            sP(txt[0].id, txt[1].id, lbl[j].id);
                        }
                        else {
                            sP(txt[0].id, '', lbl[j].id);
                        }
                    }
                }
            }

            if (Dg == 'Ctrlplanfares1_dgAdult') {
                for (i = 0; i < arrdultprices.split("$").length; i++) {
                    if (i + 1 == arrdultprices.split("$").length)
                        tempPrice += p1;
                    else
                        tempPrice += p1 + "$";
                }
                arrdultprices = tempPrice
            }
            else if (Dg == 'Ctrlplanfares1_dgChild') {
                for (i = 0; i < arrchildprices.split("$").length; i++) {
                    if (i + 1 == arrchildprices.split("$").length)
                        tempPrice += p1;
                    else
                        tempPrice += p1 + "$";
                }
                arrchildprices = tempPrice;
            }
            else if (Dg == 'Ctrlplanfares1_dgTeen') {
                for (i = 0; i < arrjuniorprices.split("$").length; i++) {
                    if (i + 1 == arrjuniorprices.split("$").length)
                        tempPrice += p1;
                    else
                        tempPrice += p1 + "$";
                }
                arrjuniorprices = tempPrice;
            }

            pr2.value = arrdultprices + "|" + arrchildprices + "|" + arrjuniorprices;
        }

        /*****************************************************************************************/

        /*****************************************************************************************/
        function FillPrices2(Dg, typeFare, Price) {
            var P = document.getElementById(Price);
            var pr = P.value;
            var grid = document.getElementById(Dg);
            var item;
            var arrayprices;
            var arrayadultprice;
            var arraychildprice;
            var arrayjuniorprice;

            if (!grid) return;
            item = grid.getElementsByTagName("tr");
            arrayprices = pr.split("|");
            arrayadultprice = arrayprices[0];
            arraychildprice = arrayprices[1];
            arrayjuniorprice = arrayprices[2];

            arrayadultprice = arrayadultprice.split("$");
            arraychildprice = arraychildprice.split("$");
            arrayjuniorprice = arrayjuniorprice.split("$");

            for (var i = 1; i < item.length; i++) {
                var txt = item[i].getElementsByTagName("input");
                var lbl = item[i].getElementsByTagName("span");

                for (var j = 0; j < lbl.length; j++) {
                    for (var t = 0; t < txt.length; t++) {
                        if (txt[t].id.indexOf(typeFare) != -1) {
                            if (Dg == 'Ctrlplanfares1_dgAdult') {
                                if (arrayadultprice[i - 1] != undefined) {
                                    txt[t].value = arrayadultprice[i - 1];
                                }
                            }
                            else if (Dg == 'Ctrlplanfares1_dgChild') {
                                if (arraychildprice[i - 1] != undefined) {
                                    txt[t].value = arraychildprice[i - 1];
                                }
                            }
                            else if (Dg == 'Ctrlplanfares1_dgTeen') {
                                if (arrayjuniorprice[i - 1] != undefined) {
                                    txt[t].value = arrayjuniorprice[i - 1];
                                }
                            }
                        }
                    }

                    if (lbl[j].id.indexOf("lblTotal") != -1) {
                        if (txt.length > 1) {
                            sP(txt[0].id, txt[1].id, lbl[j].id);
                        }
                        else {
                            sP(txt[0].id, '', lbl[j].id);
                        }
                    }
                }
            }
        }

        function GetMinRate(item) {
            var mintar= 99999999;
            for (var i = 0; i < item.length; i++) {
                if (eval(item[i]) < mintar) {
                    mintar = eval(item[i]);
                }
            }
            return ((mintar== 99999999) ? item[0] : mintar);
        }
        
        function FillPrices3(Dg, typeFare) {
            var grid = document.getElementById(Dg);
            var item;
            var p1;
            var tempPrice = "";

            if (!grid) return true;

            item = grid.getElementsByTagName("tr");
            for (var i = 1; i < item.length; i++) {
                var txt = item[i].getElementsByTagName("input");
                var lbl = item[i].getElementsByTagName("span");
                for (var j = 0; j < lbl.length; j++) {
                    for (var t = 0; t < txt.length; t++) {
                        if (txt[t].id.indexOf(typeFare) != -1) {
                            p1 = txt[t].value;
                        }
                    }
                    if (lbl[j].id.indexOf("lblTotal") != -1) {
                        if (txt.length > 1) {
                            sP(txt[0].id, txt[1].id, lbl[j].id);
                        }
                        else {
                            sP(txt[0].id, '', lbl[j].id);
                        }
                    }
                }
                if (i + 1 == item.length)
                    tempPrice += p1;
                else
                    tempPrice += p1 + "$";
            }
            var val = tempPrice.split("$")
            var lblvpres = document.getElementById('lblValidPriceRes');
            if (tempPrice.length > 0) {
                for (var i = 0; i < tempPrice.split("$").length; i++) {
                    if (isNumber(val[i]) == false) {
                        lblvpres.style.display = '';
                        return false;
                    }
                    else if (i == 0 && parseFloat(val[i]) == 0.0 && Dg == 'Ctrlplanfares1_dgAdult') {
                        lblvpres.style.display = '';
                        return false;
                    }
                }
            }

            lblvpres.style.display = 'none';
            var ptprecios = document.getElementById('currentprices');
            var prc = ptprecios.value;
            prc = prc.split("|");
            var precios = document.getElementById(prc[0]);
            var pr = precios.value;
            pr = pr.split("|");
            var pradult = pr[0];
            var prchild = pr[1];
            var prjunior = pr[2];
            var txa = document.getElementById(prc[1]);
            var txc = document.getElementById(prc[2]);
            var txj = document.getElementById(prc[3]);

            if (Dg == 'Ctrlplanfares1_dgAdult') {
                pradult = tempPrice;
                tempPrice = tempPrice.split("$");
                if (pradult.length > 0) {
                    txa.value = tempPrice[0];
                    txa.value = GetMinRate(tempPrice);
                }
                else {
                    //txa.value = "0";
                }
            }
            else if (Dg == 'Ctrlplanfares1_dgChild') {
                prchild = tempPrice;
                tempPrice = tempPrice.split("$");
                if (prchild.length > 0) {
                    txc.value = tempPrice[0];
                    txc.value = GetMinRate(tempPrice);
                }
                else {
                    //txc.value = "0";
                }
            }
            else if (Dg == 'Ctrlplanfares1_dgTeen') {
                prjunior = tempPrice;
                tempPrice = tempPrice.split("$");
                if (prjunior.length > 0) {
                    txj.value = tempPrice[0];
                    txj.value = GetMinRate(tempPrice);
                }
                else {
                    //txc.value = "0";
                }
            }
            precios.value = pradult + "|" + prchild + "|" + prjunior;
            return true;
        }

        function Ocultar(v, con, prices, txtadult, txtchild, txtjunior) {
            var e = document.getElementById(con);
            var r = document.getElementById('modalPage2');
            var lblvpres = document.getElementById('lblValidPriceRes');
            //            var ddlrooms = document.getElementById('ddlRooms')
            //            var ddlplanrates = document.getElementById('ddlratesplans')
            //            var ddlmonth = document.getElementById('ddlMonth')
            //            var ddlyears = document.getElementById('ddlyear')
            if (v == '1') {
                lblvpres.style.display = 'none';
                var cp = document.getElementById('currentprices');
                cp.value = prices + "|" + txtadult + "|" + txtchild + "|" + txtjunior;
                FillPrices2('Ctrlplanfares1_dgAdult', 'txtAdult', prices);
                FillPrices2('Ctrlplanfares1_dgChild', 'txtChild', prices);
                FillPrices2('Ctrlplanfares1_dgTeen', 'txtTeenFare', prices);
                r.style.display = 'block';
                e.style.display = 'block';
                DisabledSelect(true);
            }
            else {
                if (FillPrices3('Ctrlplanfares1_dgAdult', 'txtAdult') && FillPrices3('Ctrlplanfares1_dgChild', 'txtChild') &&
				            FillPrices3('Ctrlplanfares1_dgTeen', 'txtTeenFare')) {
                    r.style.display = "none";
                    e.style.display = "none";
                    DisabledSelect(false);
                }
            }
        }

        function ValidPrice(field, iszero) {
            var price = document.getElementById(field);
            var lab = document.getElementById('lblValidPrice');
            if (isNumber(price.value) == false) {
                price.focus();
                lab.style.display = '';
                lab.innerHTML = resource00116;
            }
            else {

                if (iszero && price.value == 0) {
                    price.focus();
                    lab.innerHTML = resource01471;
                    lab.style.display = '';
                    //price.focus();			
                }
                else {
                    lab.style.display = "none";
                }
            }
        }

        function isNumber(s) {
            var i;
            var dotAppeared;
            dotAppeared = false;
            if (isEmpty(s))
                if (isNumber.arguments.length == 1) return defaultEmptyOK;
            else return (isNumber.arguments[1] == true);

            for (i = 0; i < s.length; i++) {
                var c = s.charAt(i);
                if (i != 0) {
                    if (c == ".") {
                        if (!dotAppeared)
                            dotAppeared = true;
                        else
                            return false;
                    } else
                        if (!isDigit(c)) return false;
                } else {
                    if (c == ".") {
                        if (!dotAppeared)
                            dotAppeared = true;
                        else
                            return false;
                    } else
                        if (!isDigit(c) && (c != "-") || (c == "+")) return false;
                }
            }
            return true;
        }

        function isLetterOrDigit(c) {
            return (isLetter(c) || isDigit(c));
        }

        function isDigit(c) {
            return ((c >= "0") && (c <= "9"));
        }

        function isEmpty(s) {
            return ((s == null) || (s.length == 0));
        }
		    																		
    </script>

    
    <script type="text/javascript">
          $().ready(function() {
              $('#loadingProcess').hide();
          });  
    </script>

</body>
</html>
