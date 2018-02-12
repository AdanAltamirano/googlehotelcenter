<%@ Import Namespace="RateManager" %>
<%@ Register TagPrefix="uc1" TagName="ctlMensajes" Src="../Modulos/ctlMensajes.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../Portal/Modules/ctrlFooter.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../Portal/Modules/ctrlHeader.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" EnableEventValidation="true" CodeBehind="AvailabilityRestrictions.aspx.vb"
    Inherits="RateManager.AvailabilityRestrictions" %>

<%@ Register TagPrefix="uc1" TagName="ctlMensajeRuleConf" Src="../Modulos/ctlMensajeRuleConf.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrMenu" Src="../Portal/Modules/CtrMenu.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>AvailabilityRestrictions</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet"></link>

    <script type="text/javascript" src='<%= (request.applicationpath & "/pages/Scripts/jquery.min.js").Replace("//","/") %>'></script>

    <script type="text/javascript" src='<%= (request.applicationpath & "/pages/Scripts/jquery.bgiframe.min.js").Replace("//","/")%>'></script>

    <script type="text/javascript" src='<%= (request.applicationpath & "/pages/Scripts/jquery.autocomplete.min.js").Replace("//","/")%>'></script>

    <script language="javascript">
        jQuery.noConflict();           
    </script>

    <%--    <script language='javascript' src='../Portal/Scripts/base.js'></script>
    <script language='javascript' src='../Portal/Scripts/Ajax.js'></script>--%>

    <script language='javascript' src='Scripts/RulesSearch.js'></script>

</head>
<body bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0" ms_positioning="FlowLayout">

    <script>

        function IniDate() {
            var fecha = new Date();
            var fecha2 = new Date(2030, 12, 31);
            var arr = new Array(3);
            arr[0] = [fecha.getFullYear(), fecha.getMonth() + 1, fecha.getDate()]
            arr[1] = [fecha2.getFullYear(), fecha2.getMonth() + 1, fecha2.getDate()];
            return arr;
        }
        function submitpage(more) {

            var iM = document.getElementById("imonth");
            var iY = document.getElementById("iyear");

            //iM.value=cmb1.selectedIndex;      
            if (more > 0) {
                var cmb1 = document.getElementById("ddlMonth");
                var cmb2 = document.getElementById("ddlyear");
                if (eval(iM.value) + 3 < 12) {
                    cmb1.selectedIndex = eval(iM.value) + 3;
                }
                else {
                    if (eval(iY.value) < 5) {
                        cmb2.selectedIndex = eval(iY.value) + 1;
                        cmb1.selectedIndex = (eval(iM.value) + 3) % 12;
                    }
                }
            }
            else {
                var cmb1 = document.getElementById("ddlMonth");
                var cmb2 = document.getElementById("ddlyear");
                if (eval(iM.value) - 3 >= 0) {
                    cmb1.selectedIndex = eval(iM.value) - 3;
                }
                else {
                    if (eval(iY.value) > 0) {
                        cmb2.selectedIndex = eval(iY.value) - 1;
                        cmb1.selectedIndex = (12 + eval(iM.value) - 3);
                    }
                }
            } 
            document.getElementById('Form1').submit();
        }

        function VerifyDate() {
            var txt1 = document.getElementById("txtInicio").value;
            var txt2 = document.getElementById("txtFinal").value;
        }
        function evalDates() {
            var cmb = document.getElementById("ddlStatus"); //combo
            var txt1 = document.getElementById("txtInicio"); //inicio
            var txt2 = document.getElementById("txtFinal"); //fin
            var xDia, xMes, xYear;

            if (cmb.selectedIndex >= 1) {
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

                //Set 1 day in milliseconds
                var one_day = 1000 * 60 * 60 * 24;

                //Calculate difference btw the two dates, and convert to days		
                one_day = (NewDt2.getTime() - NewDt1.getTime()) / (one_day);

                NewDt1 = Date.parse(NewDt1);
                NewDt2 = Date.parse(NewDt2);
                var conf;
                if (eval(one_day) >= 15) {
                    return true;
                }
                return false;
            }
        }

        function LoadRules(fecha) {
            hotelCtlInit();
            searchRulesHotel(fecha, 1);

            hotelLockCtlInit();
            searchHotelLockRules(fecha, 4);

            RPCtlInit();
            RPCtlInit2();
            searchRatePlan(fecha, 2);

            RPHCtlInit();
            RPHCtlInit2();
            searchRatePlanHotel(5);

            THCtlInit(1);

            var tbl = document.getElementById("bookingcontainer");
            tbl.style.display = 'none';

            showRule('CtlMensajeRuleConf1_PnlBox', 'CtlMensajeRuleConf1_TblPnl', '');
            return false;
        }

        function FireUpdateStatus(msg) {

            var hr = true;
            if (evalDates()) {
                hr = confirm(msg);
            }
            if (hr) $('#loadingProcess').show();
            return hr;
        }

        function valCheckbox() {
            
            if ($("#<%= chkMindays.ClientID%>").is(':checked') ||
                $("#<%= chkMaxDays.ClientID%>").is(':checked') ||
                $("#<%= chkAdvBook.ClientID%>").is(':checked') ||
                $("#<%= chkEstatus.ClientID%>").is(':checked')) {
                    $(".valcheck").css("display", "none");
                    return true;
                }
                else {
                    $(".valcheck").css("display", "block");
                    return false;
            }
        }

        jQuery(document).ready(function() {
            var chkMindays = $("#<%= chkMindays.ClientID%>").click(function() {

            if ($(this).is(':checked'))
                $("#<%= txtmindays.ClientID %>").removeAttr('disabled');
            else
                $("#<%= txtmindays.ClientID %>").attr('disabled', 'disabled');
                
            });
            var chkMaxDays = $("#<%= chkMaxDays.ClientID %>").click(function() {

                if ($(this).is(':checked'))
                    $("#<%= txtMaxdays.ClientID %>").removeAttr('disabled');
                else
                    $("#<%= txtMaxdays.ClientID %>").attr('disabled', 'disabled');

            });
            var chkAdvBook = $("#<%= chkAdvBook.ClientID %>").click(function() {

                if ($(this).is(':checked'))
                    $("#<%= txtAdvBook.ClientID %>").removeAttr('disabled');
                else
                    $("#<%= txtAdvBook.ClientID %>").attr('disabled', 'disabled');

            });
        });
    
    </script>

    <iframe id="gToday:normal:agenda.js" style="z-index: 999; left: -500px; position: absolute;
        top: -500px" name="gToday:normal:agenda.js" src='<%=GeRequestApplicationPath(string.concat("/Calendar/",portalculture.getculture().Name.Substring(0, 2).ToLower(),"/ipopeng2.htm" )) %>'
        frameborder="0" width="174" scrolling="no" height="189"></iframe>
    <form id="Form1" method="post" runat="server">
    <input id="imonth" type="hidden" runat="server">
    <input id="iyear" type="hidden" runat="server">
    <div class="clear">
        <div class="mDiv">
        </div>
        <div class="title">
            <asp:Label ID="lblTitle" runat="server" EnableViewState="False" Text="Status" CssClass="tituloSeccion"></asp:Label>
        </div>
        <div id="loadingProcess">
        </div>
    </div>
    <table id="bookingcontainer" cellspacing="0" cellpadding="2" width="650" border="0">
        <tr>
            <td colspan="5">
                <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                    <tr>
                        <td>
                            <!--Tabla para seleccionar hotel ó RP-->
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <!--Tabla para Cargar datos-->
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td colspan="7">
                                                    <asp:Label ID="lblShowAvail" runat="server" EnableViewState="False" CssClass="clslabel">Show Availability</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="10%">
                                                    <div id="divRdHotel" runat="server">
                                                        <asp:RadioButton ID="RdHotel" runat="server" CssClass="clslabel" Text="Hotel" GroupName="rd"
                                                            Checked="True"></asp:RadioButton></div>
                                                </td>
                                                <td width="10%">
                                                    <asp:RadioButton ID="RdRatePlan" runat="server" CssClass="clslabel" Text="Rate Plan"
                                                        GroupName="rd"></asp:RadioButton>
                                                </td>
                                                <td width="20%">
                                                    <div id="divRdRatePlan" runat="server">
                                                        <asp:DropDownList ID="DlRatePlan" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblSegment" runat="server" EnableViewState="False" CssClass="clslabel">Segmento</asp:Label>
                                                </td>
                                                <td width="20%">
                                                    <asp:DropDownList ID="DlSegment" runat="server">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblStart" runat="server" EnableViewState="False" CssClass="clslabel">Inicio</asp:Label>
                                                </td>
                                                <td align="right" width="15%">
                                                    <asp:DropDownList ID="ddlMonth" runat="server">
                                                    </asp:DropDownList>
                                                </td>
                                                <td align="left" width="15%">
                                                    <asp:DropDownList ID="ddlyear" runat="server">
                                                    </asp:DropDownList>
                                                </td>
                                                <td align="right" width="15%">
                                                    <asp:Button ID="btnLoad" runat="server" CssClass="button" Text="Button" CausesValidation=false ></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table id="Table1" cellspacing="0" cellpadding="0" width="100%" align="center" border="0" style=" padding-top: 4px; "> 
                    <tbody>
                        <tr>
                            <td class="dgitem" align="center" width="30%" colspan="5" >
                                <table id="Table3" cellspacing="1" cellpadding="1" width="95%" border="0">
                                    <tr>
                                        <td width="10" bgcolor="green">
                                        </td>
                                        <td>
                                            <asp:Label ID="lblOpen" runat="server" EnableViewState="False" CssClass="clslabel">Open</asp:Label>
                                        </td>
                                        <td width="10" bgcolor="red">
                                        </td>
                                        <td>
                                            <asp:Label ID="lblClose" runat="server" EnableViewState="False" CssClass="clslabel">Close</asp:Label>
                                        </td>
                                        <td width="10" bgcolor="#b0c4de">
                                        </td>
                                        <td>
                                            <asp:Label ID="lblNoArrivals" runat="server" EnableViewState="False" CssClass="clslabel">No arrivals</asp:Label>
                                        </td>
                                        <td width="10" bgcolor="lightsalmon">
                                        </td>
                                        <td>
                                            <asp:Label ID="lblAllSold" runat="server" EnableViewState="False" CssClass="clslabel">All sold</asp:Label>
                                        </td>
                                        <td width="10" bgcolor="#b8860b">
                                        </td>
                                        <td>
                                            <asp:Label ID="lblnorates" runat="server" CssClass="clslabel">No Tarifas</asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 15px" align="right" width="30%" colspan="5">
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center" width="30%">
                                <table id="tblMonth1" cellspacing="0" cellpadding="0" width="100%" align="center"
                                    border="0" runat="server" enableviewstate="False" class="clslabel">
                                    <tr>
                                        <td colspan="7">
                                            <asp:Label ID="lblMonth1" runat="server" EnableViewState="False" CssClass="clsDarkLabel">Label</asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="Titulo">
                                        <td align="center" width="14.29%">
                                            D
                                        </td>
                                        <td align="center" width="14.29%">
                                            L
                                        </td>
                                        <td align="center" width="14.29%">
                                            M
                                        </td>
                                        <td align="center" width="14.29%">
                                            M
                                        </td>
                                        <td align="center" width="14.29%">
                                            J
                                        </td>
                                        <td align="center" width="14.29%">
                                            V
                                        </td>
                                        <td align="center" width="14.29%">
                                            S
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr class="clslabel">
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr class="clslabel">
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr class="clslabel">
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr class="clslabel">
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr class="clslabel">
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td width="5%">
                            </td>
                            <td style="text-align: center" width="30%">
                                <table id="tblMonth2" cellspacing="0" cellpadding="0" width="100%" align="center"
                                    border="0" runat="server" enableviewstate="False" class="clslabel">
                                    <tr>
                                        <td colspan="7">
                                            <asp:Label ID="lblMonth2" runat="server" EnableViewState="False" CssClass="clsDarkLabel">Label</asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="Titulo">
                                        <td align="center" width="14.29%">
                                            D
                                        </td>
                                        <td style="width: 30px" align="center" width="30">
                                            L
                                        </td>
                                        <td align="center" width="14.29%">
                                            M
                                        </td>
                                        <td align="center" width="14.29%">
                                            M
                                        </td>
                                        <td align="center" width="14.29%">
                                            J
                                        </td>
                                        <td align="center" width="14.29%">
                                            V
                                        </td>
                                        <td align="center" width="14.29%">
                                            S
                                        </td>
                                    </tr>
                                    <tr class="clslabel">
                                        <td>
                                        </td>
                                        <td style="width: 30px">
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr class="clslabel">
                                        <td>
                                        </td>
                                        <td style="width: 30px">
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr class="clslabel">
                                        <td>
                                        </td>
                                        <td style="width: 30px">
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr class="clslabel">
                                        <td>
                                        </td>
                                        <td style="width: 30px">
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr class="clslabel">
                                        <td>
                                        </td>
                                        <td style="width: 30px">
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr class="clslabel">
                                        <td>
                                        </td>
                                        <td style="width: 30px">
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td width="5%">
                            </td>
                            <td style="text-align: center" align="right" width="30%">
                                <table id="tblMonth3" cellspacing="0" cellpadding="0" width="100%" align="center"
                                    border="0" runat="server" enableviewstate="False" class="clslabel">
                                    <tr>
                                        <td colspan="7">
                                            <asp:Label ID="lblMonth3" runat="server" EnableViewState="False" CssClass="clsDarkLabel">Label</asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="Titulo">
                                        <td align="center" width="14.29%">
                                            D
                                        </td>
                                        <td align="center" width="14.29%">
                                            L
                                        </td>
                                        <td align="center" width="14.29%">
                                            M
                                        </td>
                                        <td align="center" width="14.29%">
                                            M
                                        </td>
                                        <td align="center" width="14.29%">
                                            J
                                        </td>
                                        <td align="center" width="14.29%">
                                            V
                                        </td>
                                        <td align="center" width="14.29%">
                                            S
                                        </td>
                                    </tr>
                                    <tr class="clslabel">
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr class="clslabel">
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr class="clslabel">
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr class="clslabel">
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr class="clslabel">
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr class="clslabel">
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 30px" align="right" colspan="5" valign=middle  >
                                <asp:HyperLink ID="hplPrevius" runat="server">Previus</asp:HyperLink>&nbsp;&nbsp;<asp:HyperLink
                                    ID="hplNext" runat="server">Next</asp:HyperLink>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="5" style="text-align: center;">
                <asp:Label ID="lblErrorGeneral" runat="server" CssClass="validators" Visible="False">Without rate plan</asp:Label>
            </td>
        </tr>
        <tr>
            <td width="30%" colspan="5">
                <asp:HyperLink ID="hplHide" runat="server" CssClass="hideOptions">Hide configurations</asp:HyperLink><asp:HyperLink
                    ID="hplShow" runat="server" CssClass="showOptions">Show configurations</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td width="30%" colspan="5">
                <div id="divConf" style="display: none;" runat="server">
                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr>
                            <td>
                            </td>
                            <td>
                                <!--Tabla para Guardar los status availability-->
                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td align="left">
                                            <asp:RadioButton ID="RbdHotel" runat="server" CssClass="clslabel" Text="Hotel" GroupName="RD2"
                                                Checked="True" Font-Bold="True"></asp:RadioButton>
                                        </td>
                                        <td colspan="2">
                                            <asp:RadioButton ID="RbdRatePlan" runat="server" CssClass="clslabel" Text="Only Rate Plan"
                                                GroupName="RD2" Font-Bold="True" Checked="False"></asp:RadioButton>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblFrom" runat="server" EnableViewState="False" CssClass="clslabel">From</asp:Label><a
                                                hidefocus onclick="if(self.gfPop)gfPop.fPopCalendar(document.getElementById('txtFinal'),document.getElementById('txtInicio'),IniDate());return false;"
                                                href="javascript:void(0)"><asp:TextBox ID="txtInicio" runat="server" CssClass="textbox"
                                                    Width="84px" MaxLength="10" Columns="12"></asp:TextBox></a><a hidefocus onclick="if(self.gfPop)gfPop.fPopCalendar(document.getElementById('txtFinal'),document.getElementById('txtInicio'),IniDate());return false;"
                                                        href="javascript:void(0)"><img class="PopcalTrigger" alt="" src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>'
                                                            align="absMiddle" border="0"></a><asp:Label ID="lblTo" runat="server" EnableViewState="False"
                                                                CssClass="clslabel">to</asp:Label><a hidefocus onclick="if(self.gfPop)gfPop.fPopCalendar1(document.getElementById('txtFinal'),IniDate());return false;"
                                                                    href="javascript:void(0)"><asp:TextBox ID="txtFinal" runat="server" CssClass="textbox"
                                                                        Width="84px" MaxLength="10" Columns="12"></asp:TextBox></a><a hidefocus onclick="if(self.gfPop)gfPop.fPopCalendar1(document.getElementById('txtFinal'),IniDate());return false;"
                                                                            href="javascript:void(0)"><img class="PopcalTrigger" alt="" src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>'
                                                                                align="absMiddle" border="0"></a><asp:Label ID="lblError" runat="server" EnableViewState="False"
                                                                                    CssClass="Validators" Visible="False">error</asp:Label>
                                        </td>
                                        <td align="right">
                                            <span style="display:none;float:left;" class="Validators valcheck">*</span>
                                            <asp:CheckBox ID="chkMindays" runat="server" EnableViewState="False" CssClass="clslabel">
                                            </asp:CheckBox>
                                        </td>
                                        <td>
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="lblmindays" runat="server" EnableViewState="False" CssClass="clslabel">Min. days</asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtmindays" runat="server" EnableViewState="False" CssClass="textbox" Enabled="false"
                                                Width="40px" MaxLength="3" Columns="3"></asp:TextBox><asp:RangeValidator ID="RvMindays"
                                                    runat="server" CssClass="Validators" ErrorMessage="0-255" Display="Dynamic" ControlToValidate="txtmindays"
                                                    Type="Integer" MinimumValue="0" MaximumValue="255"></asp:RangeValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="3">
                                            <div id="divddl" align="center" runat="server">
                                                <asp:Label ID="lblRatePlan" CssClass="clslabel" runat="server">RatePlan:</asp:Label><asp:DropDownList
                                                    ID="ddlRateplans" runat="server" Width="130px">
                                                </asp:DropDownList>
                                                <br>
                                            </div>
                                            <div id="divApplyAllPlan" runat="server">
                                                <asp:CheckBox ID="chkApplyAllPlan" runat="server" EnableViewState="False" Text="Todos los Planes">
                                                </asp:CheckBox></div>
                                        </td>
                                        <td>
                                            <table id="Table2" cellspacing="0" cellpadding="0" width="100%" border="0">
                                                <tr>
                                                    <td align="center">
                                                        <asp:Label ID="lbldom" runat="server" EnableViewState="False" CssClass="clsLabel">D</asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="Lbllun" runat="server" EnableViewState="False" CssClass="clsLabel">L</asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblmar" runat="server" EnableViewState="False" CssClass="clsLabel">M</asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblmie" runat="server" EnableViewState="False" CssClass="clsLabel">M</asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lbljue" runat="server" EnableViewState="False" CssClass="clsLabel">J</asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblvie" runat="server" EnableViewState="False" CssClass="clsLabel">V</asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblsab" runat="server" EnableViewState="False" CssClass="clsLabel">S</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <asp:CheckBox ID="Chk7" runat="server" EnableViewState="False" Checked="True"></asp:CheckBox>
                                                    </td>
                                                    <td align="center">
                                                        <asp:CheckBox ID="Chk1" runat="server" EnableViewState="False" Checked="True"></asp:CheckBox>
                                                    </td>
                                                    <td align="center">
                                                        <asp:CheckBox ID="Chk2" runat="server" EnableViewState="False" Checked="True"></asp:CheckBox>
                                                    </td>
                                                    <td align="center">
                                                        <asp:CheckBox ID="Chk3" runat="server" EnableViewState="False" Checked="True"></asp:CheckBox>
                                                    </td>
                                                    <td align="center">
                                                        <asp:CheckBox ID="Chk4" runat="server" EnableViewState="False" Checked="True"></asp:CheckBox>
                                                    </td>
                                                    <td align="center">
                                                        <asp:CheckBox ID="Chk5" runat="server" EnableViewState="False" Checked="True"></asp:CheckBox>
                                                    </td>
                                                    <td align="center">
                                                        <asp:CheckBox ID="Chk6" runat="server" EnableViewState="False" Checked="True"></asp:CheckBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td align="right">
                                            <span style="display:none;float:left;" class="Validators valcheck">*</span>
                                            <asp:CheckBox ID="chkMaxDays" runat="server" EnableViewState="False"></asp:CheckBox>
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="lblMaxdays" runat="server" EnableViewState="False" CssClass="clslabel">Max. days</asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtMaxdays" runat="server" EnableViewState="False" CssClass="textbox" Enabled="false"
                                                Width="40px" MaxLength="3" Columns="3"></asp:TextBox><asp:RangeValidator ID="RvMaxDays"
                                                    runat="server" CssClass="Validators" ErrorMessage="0-255" Display="Dynamic" ControlToValidate="txtMaxdays"
                                                    Type="Integer" MinimumValue="0" MaximumValue="255"></asp:RangeValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" width="150" colspan="3">
                                            <div id="divOnRequest" align="center" runat="server">
                                                <asp:CheckBox ID="chkOnRequest" runat="server" CssClass="clslabel" Text="On Request">
                                                </asp:CheckBox></div>
                                        </td>
                                        <td align="left">
                                            <table id="Table4" cellspacing="0" cellpadding="0" width="300" border="0">
                                                <tr>
                                                    <td>
                                                        <span style="display:none;float:left;" class="Validators valcheck">*</span>
                                                        <asp:CheckBox ID="chkEstatus" runat="server" EnableViewState="False" Checked="False">
                                                        </asp:CheckBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label1" runat="server" EnableViewState="False" CssClass="clslabel"> Status</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlStatus" runat="server">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td align="right">
                                            <span style="display:none;float:left;" class="Validators valcheck">*</span>
                                            <asp:CheckBox ID="chkAdvBook" runat="server" EnableViewState="False"></asp:CheckBox>
                                        </td>
                                        <td align="left">
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="lblAdvBook" runat="server" EnableViewState="False" CssClass="clslabel">Advance booking</asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtAdvBook" runat="server" EnableViewState="False" CssClass="textbox" Enabled="false"
                                                Width="40px" MaxLength="3" Columns="3"></asp:TextBox><asp:RangeValidator ID="RVAdvBook"
                                                    runat="server" CssClass="Validators" ErrorMessage="0-255" Display="Dynamic" ControlToValidate="txtAdvBook"
                                                    Type="Integer" MinimumValue="0" MaximumValue="255"></asp:RangeValidator>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td align="left">
                                        </td>
                                        <td>
                                        </td>
                                        <td align="right" colspan="6">
                                            <br>
                                            <table width="100%">
                                                <tr>
                                                    <td align="left">
                                                        <asp:CheckBox ID="chkPriorCancel" runat="server" EnableViewState="False"></asp:CheckBox>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblPriorCancel" runat="server" EnableViewState="False" CssClass="clslabel">Advance booking</asp:Label>
                                                    </td>
                                                    <td nowrap align="left">
                                                        <asp:DropDownList ID="ddlCancelationPolicy" runat="server">
                                                        </asp:DropDownList>
                                                        <asp:DropDownList ID="ddlHour" runat="server">
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
                                                        </asp:DropDownList>
                                                        <asp:Label ID="lblSep" runat="server">:</asp:Label><asp:DropDownList ID="ddlMinutes"
                                                            runat="server">
                                                            <asp:ListItem Value="00">00</asp:ListItem>
                                                            <asp:ListItem Value="15">15</asp:ListItem>
                                                            <asp:ListItem Value="30">30</asp:ListItem>
                                                            <asp:ListItem Value="45">45</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:Label ID="lblAux" runat="server" CssClass="clslabel">Cancel</asp:Label><asp:TextBox
                                                            ID="txtCancellationPolicy" runat="server" CssClass="textbox" Width="30px" MaxLength="3"
                                                            Columns="3"></asp:TextBox><asp:Label ID="lblEDaysHour" runat="server" CssClass="clslabel">Days / Hours</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblErrorHours" runat="server" CssClass="validators" Visible="False"></asp:Label><asp:RangeValidator
                                                            ID="RVCancelation" runat="server" CssClass="validators" ErrorMessage="0-999"
                                                            Display="Dynamic" ControlToValidate="txtCancellationPolicy" MinimumValue="0"
                                                            MaximumValue="9999"></asp:RangeValidator>
                                                    </td>
                                                </tr>
                                            </table>
                                            <br>
                                        </td>
                                        <tr>
                                            <td align="center">
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <%--<input class="button" id="btnsaveAvail" onclick='<%= getFunctionShow() %>' type="button"
                                                    value='<%=PortalCulture.GetString("00008") %>' name='btnsaveAvail' style="display: none;">--%>
                                                <asp:Button ID="btnSave" runat="server" EnableViewState="False" CssClass="button"
                                                    Text="save" CausesValidation="true" Style="width: 80px;"></asp:Button>
                                                <%--
                                                <input id="btnsaveAvail" class="button" name='btnsaveAvail"' 
                                                    onclick="<%= getFunctionShow() %>" type="button" value='<%=PortalCulture.GetString("00008") %>'><asp:Button ID="btnSave"
                                                        Style="display: none" runat="server" EnableViewState="False" CssClass="button"
                                                        Text="save"></asp:Button>--%>
                                            </td>
                                        </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
    <span style="display:none;float:left;" class="Validators valcheck" id="spanMSG" runat="server">Seleccione al menos uno</span>
    <uc1:ctlMensajes ID="CtlMensajes1" runat="server"></uc1:ctlMensajes>
    <uc1:ctlMensajeRuleConf ID="CtlMensajeRuleConf1" runat="server"></uc1:ctlMensajeRuleConf>
    </form>

    <script>




        function Ocultar(v) {
            var e = document.getElementById('divConf');
            var s = document.getElementById('hplShow');
            var o = document.getElementById('hplHide');
            if (v == '1') {
                e.style.display = '';
                o.style.display = '';
                s.style.display = 'none';
            }
            else {
                e.style.display = 'none';
                s.style.display = '';
                o.style.display = 'none';
            }
            onResizeIframe();
        }

        var rd = document.getElementById('RbdRatePlan');
        var chkAp = document.getElementById('divApplyAllPlan');
        //var lblkAp = document.getElementById('lblAplyAllPlan');
        if (!rd.checked) {
            chkAp.style.display = '';
            //	lblkAp.style.display = '';
        }
        else {
            chkAp.style.display = 'none';
            //	lblkAp.style.display = 'none';			
        }


        function HideChk() {

            var rd = document.getElementById('RbdRatePlan');
            var chk = document.getElementById('divOnRequest');
            var ddl = document.getElementById('divddl');
            var chkAp = document.getElementById('divApplyAllPlan');
            var e1 = document.getElementById('chkMinPrice');
            var e2 = document.getElementById('lblMinPrice');
            var e3 = document.getElementById('txtMinPrice');
            var e4 = document.getElementById('chkMaxPrice');
            var e5 = document.getElementById('lblMaxPrice');
            var e6 = document.getElementById('txtMaxPrice');

            //var lblkAp = document.getElementById('lblAplyAllPlan');


            if (!rd.checked) {
                chk.style.display = '';
                ddl.style.display = 'none';
                chkAp.style.display = '';
                e1.style.display = '';
                e2.style.display = '';
                e3.style.display = '';
                e4.style.display = '';
                e5.style.display = '';
                e6.style.display = '';
                //	lblkAp.style.display = '';				
            }
            else {
                chk.style.display = 'none';
                ddl.style.display = '';
                chkAp.style.display = 'none';
                e1.style.display = 'none';
                e2.style.display = 'none';
                e3.style.display = 'none';
                e4.style.display = 'none';
                e5.style.display = 'none';
                e6.style.display = 'none';
                //lblkAp.style.display = 'none';							
            }
        }
        function HideChk2() {

            var htl = document.getElementById('RdHotel');
            var rp2 = document.getElementById('divRdRatePlan');
            if (!htl.checked) {
                rp2.style.display = '';
            }
            else {
                rp2.style.display = 'none';
            }
        }
        function showRatePlanName(ddl, array, lbl) {
            var e = document.getElementById(ddl);
            var lbl = document.getElementById(lbl);
            if (array != '') {
                lbl.firstChild.nodeValue = array.split("//")[e.selectedIndex];
            }
            else {
                lbl.firstChild.nodeValue = '-';
            }
        }
        function LoadMsg(ddl, lblAux, lblDay, dia, hora, specific, aux, canc) {
            var l1 = document.getElementById(lblDay);
            var l2 = document.getElementById(lblAux);
            var d = document.getElementById(ddl);
            var txt = document.getElementById('txtCancellationPolicy');
            var ddl1 = document.getElementById('ddlHour');
            var ddl2 = document.getElementById('ddlMinutes');
            var lbls = document.getElementById('lblSep');

            txt.style.display = '';
            ddl1.style.display = 'none';
            ddl2.style.display = 'none';
            lbls.style.display = 'none';
            switch (d.selectedIndex) {
                case 0:
                    l1.firstChild.nodeValue = dia;
                    l2.firstChild.nodeValue = canc;
                    break;
                case 1:
                    l1.firstChild.nodeValue = hora;
                    l2.firstChild.nodeValue = canc;
                    break;
                case 2:
                    l1.firstChild.nodeValue = specific;
                    l2.firstChild.nodeValue = aux;
                    txt.style.display = 'none';
                    ddl1.style.display = '';
                    ddl2.style.display = '';
                    lbls.style.display = '';
                    break;
            }
        }

    </script>

    <script type="text/javascript">          
        $().ready(function() {
          $('#loadingProcess').hide();
        });  
    </script>

</body>
</html>
