<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../Portal/Modules/ctrlFooter.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../Portal/Modules/ctrlHeader.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Booking.aspx.vb" Inherits="RateManager.Booking" %>

<%@ Register TagPrefix="uc1" TagName="CtrMenu" Src="../Portal/Modules/CtrMenu.ascx" %>
<%@ Import Namespace="RateManager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>Booking</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet"></link>

    <script type="text/javascript" src="../Includes/Script/jquery-1.4.2.min.js"></script>

    <script type="text/javascript">
        function FireUpdateStatus() {
            if (isValidAges()) {
                $('#loadingProcess').show();
                return true;
            }
            return false;
        }

        function FireChildOld() {
            var index = $("#<%= Me.cmbChildren1.clientID%> option:selected").attr("value");
            if (index > 0) {

            }
        }
        
    
    </script>

</head>
<body bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0" ms_positioning="FlowLayout">
    <iframe id="gToday:normal:agenda.js" style="z-index: 999; left: -500px; position: absolute;
        top: -500px" name="gToday:normal:agenda.js" src='<%=GeRequestApplicationPath(string.concat("/Calendar/",portalculture.getculture().Name.Substring(0, 2).ToLower(),"/ipopeng2.htm" )) %>'
        frameborder="0" width="174" scrolling="no" height="189"></iframe>
    <form id="Form1" method="post" runat="server">
    <div class="clear">
        <div class="mDiv">
        </div>
        <div class="title">
            <asp:Label ID="lbltitle" runat="server" EnableViewState="False" Text="Modulo de reservación"
                CssClass="tituloSeccion"></asp:Label>
        </div>
    </div>
    <div id="loadingProcess">
    </div>
    <table id="bookingcontainer" cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
            <td>
                <asp:Panel ID="panelPasos" runat="server">
                    <table border="0" cellspacing="0" cellpadding="0" width="100%" align="center">
                        <tr>
                            <td>
                                <table border="0" cellspacing="0" cellpadding="0" width="100%" align="center">
                                    <tr>
                                        <td colspan="4" align="left">
                                            <asp:Label ID="lblPaso1" runat="server" CssClass="bookingNormalLabel">Seleccione Fechas</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblCheckin" class="clslabel" runat="server">Check In date:</asp:Label>
                                        </td>
                                        <td align="left">
                                            <a hidefocus onclick="if(self.gfPop)gfPop.fPopCalendar(document.getElementById('txtFinal'),document.getElementById('txtInicio'),IniDate());return false;"
                                                href="javascript:void(0)">
                                                <asp:TextBox ID="txtInicio" runat="server" CssClass="textbox" Width="84px" MaxLength="10"></asp:TextBox><img
                                                    class="PopcalTrigger" border="0" alt="" align="absMiddle" src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>'>
                                            </a>&nbsp;&nbsp;
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="lblCheckOut" runat="server" CssClass="clslabel">Check Out date:</asp:Label>
                                        </td>
                                        <td align="left">
                                            <a hidefocus onclick="if(self.gfPop)gfPop.fPopCalendar1(document.getElementById('txtFinal'),IniDate());return false;"
                                                href="javascript:void(0)">
                                                <asp:TextBox ID="txtFinal" runat="server" CssClass="textbox" Width="84px" MaxLength="10"></asp:TextBox><img
                                                    class="PopcalTrigger" border="0" alt="" align="absMiddle" src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>'>
                                            </a>&nbsp;&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" style="text-align: center; padding: 8px;">
                                            <asp:Label ID="Label1" runat="server" CssClass="clsHelpLabel"><%=PortalCulture.GetString("01446")%></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblAccessCode" runat="server" Text="Corporativo/Codigo Promoción:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAccessCode" runat="server" CssClass="Textbox" MaxLength="6" Columns="10"></asp:TextBox>
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="lblConvenio" runat="server" Text="Convenio:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtConvenio" runat="server" CssClass="Textbox" MaxLength="6" Columns="10"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" align="left">
                                            <asp:Label ID="lblpaso2" runat="server" CssClass="bookingNormalLabel">Indique Ocupación de habitación(es)</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <table id="tblRooms" border="0" cellspacing="0" cellpadding="0" width="100%" align="left"
                                                runat="server">
                                                <tr>
                                                    <td valign="top" align="center">
                                                        <asp:Label ID="lblRooms" runat="server" CssClass="clslabel" EnableViewState="False">Rooms:</asp:Label>
                                                    </td>
                                                    <td id="TC0" valign="top" align="right">
                                                    </td>
                                                    <td valign="top" align="center">
                                                        <asp:Label ID="lblAdultos" runat="server" CssClass="clslabel" EnableViewState="False">Adults:</asp:Label>
                                                    </td>
                                                    <td valign="top" align="center">
                                                        <asp:Label ID="lblChild" runat="server" CssClass="clslabel" EnableViewState="False">Children:</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" align="center">
                                                        <asp:DropDownList ID="cmbRooms" runat="server" Width="60px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td id="TC1" valign="top" align="right">
                                                        <asp:Label ID="lblRoom1" runat="server" EnableViewState="False">Room&nbsp;1:</asp:Label>
                                                    </td>
                                                    <td valign="top" align="center">
                                                        <asp:DropDownList ID="CmbAdultos1" runat="server">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td valign="top" align="center">
                                                        <asp:DropDownList ID="cmbChildren1" runat="server">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" style="padding-left: 12px;">
                                            <table id="tblChilds" border="0" cellspacing="0" cellpadding="0" align="left" runat="server"
                                                style="display: none;">
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" align="left">
                                            <asp:Label ID="lblpaso3" runat="server" CssClass="bookingNormalLabel">Busque disponibilidad</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" align="center">
                                            <asp:Button ID="btnSearch" runat="server" CssClass="button" CausesValidation="False"
                                                Text="Search"></asp:Button>
                                            <asp:Label ID="lblErrorEdades" runat="server" CssClass="validators" Text="Edad de Menor Inválida(1-18)"
                                                Style="display: none;"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" align="center">
                                            <asp:Label ID="lblError" runat="server" CssClass="validators">Label</asp:Label>
                                        </td>
                                    </tr>
                                    <tr height="5">
                                        <td colspan="4">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:DataGrid ID="dgRooms" runat="server" CssClass="DataGrid" Width="99%" HorizontalAlign="Center"
                                                AutoGenerateColumns="False">
                                                <SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
                                                <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
                                                <ItemStyle CssClass="dgItem"></ItemStyle>
                                                <HeaderStyle CssClass="dgHeader"></HeaderStyle>
                                                <Columns>
                                                    <asp:BoundColumn DataField="RoomName" HeaderText="Room" ItemStyle-Width="30%"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="RatePlan" HeaderText="RatePlan" ItemStyle-Width="30%">
                                                    </asp:BoundColumn>
                                                    <asp:TemplateColumn HeaderText="Lowest Rate" ItemStyle-Width="15%">
                                                        <ItemTemplate>
                                                            <table width="100%">
                                                                <tr>
                                                                    <td align="center">
                                                                        <asp:Label ID="lblPrice1" runat="server" CssClass="clslabel"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center">
                                                                        <asp:Label ID="lblPrice2" runat="server" CssClass="clslabel"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center">
                                                                        <asp:Label ID="lblPrice3" runat="server" CssClass="clslabel"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn>
                                                        <ItemTemplate>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <a runat="server" class="clslabel" id="AnclaTarifas" href="#">Show Details</a>
                                                                    </td>
                                                                    <td>
                                                                        <div runat="server" class="tablaSumary" id="divtarifas" style="padding-right: 3px;
                                                                            display: none; padding-left: 3px; padding-bottom: 3px; padding-top: 3px; position: absolute;
                                                                            background-color: white">
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Button ID="btnBookit" runat="server" CssClass="Button" CausesValidation="False"
                                                                Text="Book it"></asp:Button>
                                                            <asp:Label ID="lblNA" runat="server">NA</asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:BoundColumn Visible="False" DataField="RatePlan"></asp:BoundColumn>
                                                    <asp:BoundColumn Visible="False" DataField="Availability"></asp:BoundColumn>
                                                    <asp:BoundColumn Visible="False" DataField="Money"></asp:BoundColumn>
                                                    <asp:BoundColumn Visible="False" DataField="Message"></asp:BoundColumn>
                                                </Columns>
                                            </asp:DataGrid>
                                        </td>
                                    </tr>
                                    <tr height="5">
                                        <td colspan="4">
                                            <div id="HotelRatesList" class="HotelRatesList" runat="server">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="tdDetalles" colspan="4">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="PanelRules" runat="server">
                </asp:Panel>
                </td>
                </tr>
    </table>
    </form>

    <script>
        function cambiocuartos(obj) {

            var oculta = 0;
            var c = obj.value;
            var idobj = obj.id;
            var nid = idobj.substring(0, idobj.length - 8);

            var o;
            var or;
            o = getObj(nid + "TC1");

            if (eval(c) > 1) {
                o.style.display = "block";
                o = getObj(nid + "TC0");
                o.style.display = "block";
            }
            else {
                o.style.display = "none";
                o = getObj(nid + "TC0");
                o.style.display = "none";
            }

            if (c) {
                for (var j = 2; ; j++) {
                    o = getObj(nid + "TR" + j);
                    if (o) {
                        o.style.display = "none";
                    }
                    else break;
                }

                for (var i = 1; eval(i) <= eval(c); i++) {
                    o = getObj(nid + "TR" + i);

                    if (o) o.style.display = '';
                }
            }
            FireShowAgesBk();
        }


        function Cambio() {
            var ocuartos = getObj("cmbRooms");
            cambiocuartos(ocuartos);
            var cuartos = ocuartos.value;
        }

        function getObj(objID) {
            if (document.getElementById) { return document.getElementById(objID); }
            else if (document.all) { return document.all[objID]; }
            else if (document.layers) { return document.layers[objID]; }
        }

        function IniDate() {
            var fecha = new Date();
            var fecha2 = new Date(2030, 12, 31);
            var arr = new Array(3);
            arr[0] = [fecha.getFullYear(), fecha.getMonth() + 1, fecha.getDate()]
            arr[1] = [fecha2.getFullYear(), fecha2.getMonth() + 1, fecha2.getDate()];
            return arr;

        }
        function toggleTaxAndFees(divId, display) {
            var div = document.getElementById(divId);
            if (div) {
                div.style.display = display;
            }
        }

        Cambio();

        function ShowDivDetails(dv) {
            var e = document.getElementById('tdDetalles');
            var d = document.getElementById(dv);
            e.innerHTML = d.innerHTML;
            e.style.display = "block";
        }
        function NoShowDivDetails() {
            var e = document.getElementById('tdDetalles');
            e.style.display = "none";
        }

        /////////////////////////////////////
        function FireShowChildAges(Id) {
            var index = $("#cmbChildren" + Id + " option:selected").attr("value");
            if (index != undefined) {
                for (var x = 1; x <= 10; x++) {
                    if (x <= index) {
                        $("#lblMenor" + Id + '' + x).show();
                        $("#txtMenor" + Id + '' + x).show();
                    }
                    else {
                        $("#lblMenor" + Id + '' + x).hide();
                        $("#txtMenor" + Id + '' + x).hide();
                    }
                }

                if (index == 0) {
                    $("#TRTOLD" + Id).hide();
                    $("#TRHOLD" + Id).hide();
                    $("#TRCOLD" + Id).hide();
                    return false;
                }
                $("#TRTOLD" + Id).show();
                $("#TRHOLD" + Id).show();
                $("#TRCOLD" + Id).show();
            }
            return true;
        }

        function CtrlShowChildAges(index) {
            var hr;
            for (var j = 1; j <= 10; j++) {
                hr = FireShowChildAges(j);
            }
        }

        function FireShowAgesBk() {
            var index = $("#cmbRooms option:selected").attr("value");
            if (index != undefined) {
                CtrlShowChildAges(index);

            }
            var indx = eval(index); indx++;
            for (var x = indx; x <= 10; x++) {
                $("#TRTOLD" + x).hide();
                $("#TRHOLD" + x).hide();
                $("#TRCOLD" + x).hide();
            }
        }


        function validAgesByRooms(Id) {
            var index = $("#cmbChildren" + Id + " option:selected").attr("value");
            var hr = true;
            if (index != undefined) {
                var indx = eval(index);
                for (var x = 1; x <= indx; x++) {
                    var valor = document.getElementById("txtMenor" + Id + '' + x).value;   //$("#txtMenor" + Id + '' + x).text();
                    var spattern = /^\d+$/g;
                    hr = spattern.exec(valor) == null ? false : true;
                    if (eval(valor) > 18) hr = false;
                    if (!hr) break;
                }
                if (!hr) { $('#lblErrorEdades').show(); }
                else { $('#lblErrorEdades').hide(); }
                return hr;
            }
            return true;
        }

        function isValidAges() {
            var index = $("#cmbRooms option:selected").attr("value");
            var hr;
            if (index != undefined) {
                var indx = eval(index);
                for (var x = 1; x <= indx; x++) {
                    hr = validAgesByRooms(x);
                    if (!hr) return false;
                }
            }
            return true;
        }

    </script>

    <script type="text/javascript">
        $().ready(function() {
            $('#loadingProcess').hide();
            FireShowAgesBk();
            $('#tblChilds').show();
        });  
    </script>

</body>
</html>
