<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../../Portal/Modules/ctrlFooter.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrMenu" Src="../../Portal/Modules/CtrMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../../Portal/Modules/ctrlHeader.ascx" %>
<%@ Import Namespace="RateManager" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ReservationDetails.aspx.vb"
    Inherits="RateManager.ReservationDetails" %>

<%@ Register TagPrefix="uc1" TagName="ctrLinckPackage" Src="../../Modulos/ctrLinckPackage.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>ReservationDetails</title>

    <script>
        function ShowError(dateAux) {

            document.getElementById('txtCheckOut').value = dateAux;
            document.getElementById('lblErrorPop').style.display = 'block';
        }

        function showOutCalendar() {
            document.getElementById('txtCheckOut').disabled = false;
            document.getElementById('PopcalTrigger').style.visibility = 'visible';
        }

        function HideOutCalendar() {
            document.getElementById('txtCheckOut').disabled = true;
            document.getElementById('PopcalTrigger').style.visibility = 'hidden';
        }

        function ShowResStatus() {

            var element = document.getElementById('btnCancel');
            if (typeof (element) != 'undefined' && element != null) {
                document.getElementById('btnCancel').style.display = 'none'
            }
            document.getElementById('StatusPopup').style.display = 'block';
            document.getElementById('backgroundPopup').style.display = 'block';
        }

        function HideResStatus() {
            var element = document.getElementById('btnCancel');
            if (typeof (element) != 'undefined' && element != null) {
                document.getElementById('btnCancel').style.display = 'block'
            }
            document.getElementById('backgroundPopup').style.display = 'none';
            document.getElementById('StatusPopup').style.display = 'none';
        }

        function ShowResConfirm() {

            var element = document.getElementById('btnConfirmCancel');
            if (typeof (element) != 'undefined' && element != null) {
                document.getElementById('btnConfirmCancel').style.display = 'none'
            }
            document.getElementById('ConfirmPopup').style.display = 'block';
            document.getElementById('backgroundPopup').style.display = 'block';
        }

        function HideResConfirm() {
            var element = document.getElementById('btnConfirmCancel');
            if (typeof (element) != 'undefined' && element != null) {
                document.getElementById('btnConfirmCancel').style.display = 'block'
            }
            document.getElementById('backgroundPopup').style.display = 'none';
            document.getElementById('ConfirmPopup').style.display = 'none';
        }

        function ShowConfirm() {
            document.getElementById('ConfirmCancel').style.display = 'block';
        }

        function HideConfirm() {
            document.getElementById('ConfirmCancel').style.display = 'none';
        }
        function ShowDetails(divId, display) {
            var div = document.getElementById(divId);
            var pos = findPos(div);
            if (div) {
                div.style.display = display;
            }
            onResizeIframe();

            //Scroll to location of SupportDiv on load
            // window.scroll(0, 1000);
            //div.scrollTop = 0;
            //var myIframe = document.getElementById('frmPrincipal');
            if (pos)
                parent.scrollTo(0, pos);
            else
                parent.scrollTo(0, findPos(div));
        }
        //Finds y value of given object
        function findPos(obj) {
            var curtop = 0;
            if (obj.offsetParent) {
                do {
                    curtop += obj.offsetTop;
                } while (obj = obj.offsetParent);
                return [curtop];
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
    </script>

    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <link href="../../StyleSheets/Styles.css" type="text/css" rel="stylesheet">

    <script type="text/javascript" src='<%= (request.applicationpath & "/pages/Scripts/jquery.min.js").Replace("//","/") %>'></script>

    <style type="text/css">
        #boxPayConfirmation1 {
            width: 500px;
            margin-left: 0px;
            margin-top: 50px;
        }

            #boxPayConfirmation1 .row {
                text-align: left;
                margin-bottom: 10px;
            }

                #boxPayConfirmation1 .row .label {
                    width: 150px;
                    display: inline;
                    text-align: right;
                }

                #boxPayConfirmation1 .row.actions {
                    text-align: right;
                }

                #boxPayConfirmation1 .row.title {
                    text-align: center;
                }

        @media Print {
            .lbltc {
                display: none;
            }
        }
    </style>

    <script type="text/javascript">    
            <% If Me.CanShowConfirmButton Then%>
        $(document).ready(function () {

            if ($('.popup').length > 0) {
                $('#<%= Me.form1.ClientId %>').append('<div class="backgrounder" style="display:none;"></div>');
                /*var closer = $('<a class="closer" href="javascript:">X</a>').click(function() {
                    ShowPopup($(this).closest('.popup').attr('id'), false);
                });
                $('.popup .title').append(closer);*/
            }

            $('#btnShowPayConfirm').click(function () {
                $(this).find('input.field:text').val('');
                ShowPopup('boxPayConfirmation', true);
                $(this).find('input.field:text:first').focus();
            });

            $('#btnPayCancel').click(function () {
                ShowPopup('boxPayConfirmation', false);
            });

        });


        function ShowPopup(id, status) {
            if (status) {
                $('#' + id + ', .backgrounder').show();
            } else {
                $('#' + id + ', .backgrounder').hide();
            }
        }
            <%End If%>

        function FireReactive(msg) {
            return confirm(msg);
        }

    </script>

</head>
<body bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
        <iframe id="gToday:normal:agenda.js" style="Z-INDEX: 999; LEFT: -500px; POSITION: absolute; TOP: -500px"
            name="gToday:normal:agenda.js"
            src='<%=GeRequestApplicationPath(string.concat("/Calendar/",portalculture.getculture().Name.Substring(0, 2).ToLower(),"/ipopeng2.htm" )) %>'
            frameborder="0" width="174" scrolling="no" height="189"></iframe>
        <div class="clear">
            <div class="mDiv">
            </div>
            <div>
                <asp:Label ID="lblConfirm" runat="server" EnableViewState="False" Text="Confirm Reservation"
                    CssClass="tituloSeccion"></asp:Label>
            </div>
        </div>
        <table id="bookingcontainer" cellspacing="0" cellpadding="2" width="650" border="0">
            <tr>
                <td>
                    <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                        <tr>
                            <td>
                                <asp:HyperLink ID="hplListRes" runat="server" EnableViewState="False" CssClass="dglink">Go to Reservation list</asp:HyperLink>
                            </td>
                            <td align="right" colspan="2">
                                <asp:Label ID="lblWizcom" runat="server" EnableViewState="False" CssClass="bookingNormalLabel">Esta reservación fue hecha por el switch</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 13px" align="left"></td>
                            <td style="height: 13px" align="left"></td>
                            <td style="height: 13px" align="right">
                                <asp:Label ID="lblEStatus" runat="server" CssClass="bookingNormalLabel">Status</asp:Label>
                                <asp:Label ID="lblStatus" runat="server" EnableViewState="False" CssClass="bookingNormalLabel">Reserved</asp:Label>
                                &nbsp; <a id="ADetails" runat="server" href="javascript:;" class="dglink">Details</a>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td>
                                <asp:Literal ID="lblpay" runat="server"></asp:Literal>
                                <a id="detailObserv" runat="server" href="javascript:;" class="dglink" visible="FALSE">Observaciones</a>
                                <div id="divObservaciones" style="padding-right: 3px; display: none; padding-left: 3px; padding-bottom: 3px; padding-top: 3px; position: absolute"
                                    ms_positioning="FlowLayout">
                                    <table id="Table1" align="right">
                                        <tr>
                                            <td width="300" class="titulo">
                                                <asp:Label ID="Label2" runat="server">Observaciones:</asp:Label>
                                            </td>
                                            <td width="10" align="right">
                                                <img align="right" runat="server" id="imgCloseObserv" src="../../Images/close.png"
                                                    style="cursor: hand;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Label ID="lblObservaciones" runat="server">Observaciones del deposito</asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <div id="divDetails" class="boxMsgCan" ms_positioning="FlowLayout" style="display: none;">
                                    <table id="bookingcontainer">
                                        <tr>
                                            <td width="300" class="titulo" valign="top">
                                                <asp:Label ID="lblEMotivocancelacion" runat="server">Motivo de Cancelación:</asp:Label>
                                            </td>
                                            <td width="32" align="right" valign="top">
                                                <img align="right" runat="server" id="imgclose" src="~/Includes/imagenes/close.ico"
                                                    style="cursor: hand">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="center">
                                                <asp:Label ID="lblMotivoCancelacion" runat="server">Hay sobrecarga de venta de habitaciones y no tenemos donde hubicar al cliente por lo que lo trasladamos a paradise hotel</asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>

                                <div id="divItems" class="boxMsgCan" ms_positioning="FlowLayout" style="display: none;">
                                    <table style="background: #FFF; width: 100%;">
                                        <tr>
                                            <td width="300" class="titulo" valign="top">
                                                <asp:Label ID="lblItems" runat="server">Items:</asp:Label>
                                            </td>
                                            <td width="32" align="right" valign="top">
                                                <img align="right" runat="server" id="imgCloseItems" src="~/Includes/imagenes/close.ico"
                                                    style="cursor: hand">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="center">
                                                <asp:DataGrid ID="dgItems" runat="server" EnableViewState="False" AutoGenerateColumns="False"
                                                    HorizontalAlign="Center" Width="100%" CssClass="DataGrid">
                                                    <Columns>
                                                        <asp:BoundColumn DataField="Name"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Price"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Currency"></asp:BoundColumn>
                                                    </Columns>
                                                </asp:DataGrid>
                                                Total:
                                                <asp:Label ID="lblTotalItems" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="center"></td>
                            <td align="left"></td>
                            <td align="right">
                                <asp:Label ID="lblNoCanc" runat="server" EnableViewState="False" CssClass="clslabel">No. Cancel</asp:Label>
                                <asp:Label ID="lblNoCancelacion" runat="server" EnableViewState="False" CssClass="bookingNormalLabel"></asp:Label>
                                <%If Me.CanShowConfirmButton Then%>
                                <br />
                                <a class="dglink" style="display: inline; margin-top: 5px;" href="javascript:" id="btnShowPayConfirm">
                                    <%=RateManager.PortalCulture.GetString("01407")%></a>&nbsp;
                            <%End If%>
                                <br />
                                <asp:HyperLink ID="hplModify" runat="server" CssClass="dglink">HyperLink</asp:HyperLink>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <%If Me.CanShowConfirmButton Then%>
                                <div id="boxPayConfirmation" class="boxMsgCan" style="display: none; background-color: #f9fcff;">
                                    <table id="Table4" style="width: 100%;">
                                        <tr>
                                            <td class="modulo tituloModulo" colspan="2">
                                                <%=RateManager.PortalCulture.GetString("01404")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <span class="label">
                                                    <%=RateManager.PortalCulture.GetString("01281")%>:</span>
                                            </td>
                                            <td>
                                                <asp:TextBox class="field" ID="txtPayAutorization" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <span class="label">
                                                    <%=RateManager.PortalCulture.GetString("01405")%>:</span>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="lstPayMode" runat="server">
                                                    <asp:ListItem Text="Banamex - UV" Value="1" />
                                                    <asp:ListItem Text="Bancomer - HeM" Value="4" />
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <%
                                                    Me.btnPayConfirm.OnClientClick = "if($('#" + Me.txtPayAutorization.ClientID + "').val().length > 0) {return true;} else {$('#" + Me.txtPayAutorization.ClientID + "').focus(); return false;}"
                                                    Me.btnPayConfirm.Text = RateManager.PortalCulture.GetString("01406")
                                                %>
                                                <asp:Button CssClass="Button" ID="btnPayConfirm" runat="server" Text="Confirmar" />
                                                <input class="Button" type="button" value='<%=RateManager.PortalCulture.GetString("A00143")%>'
                                                    id="btnPayCancel" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <%End If%>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblNHotel" runat="server" EnableViewState="False" CssClass="bookingNormalLabel"
                                    Font-Size="9pt">Hotel Marina</asp:Label><br />
                                <asp:Label ID="lblDirHotel" runat="server" EnableViewState="False" CssClass="clslabel">Carretera a Pichilingue km. 2.5</asp:Label><br />
                                <asp:Label ID="lblCdHotel" runat="server" EnableViewState="False" CssClass="clsLabel">La Paz B.C.S.</asp:Label><br />
                            </td>
                            <td align="left">
                                <br />
                                <asp:Label ID="lblEIn" runat="server" EnableViewState="False" CssClass="clsLabel"> Check-In Date:</asp:Label><asp:Label
                                    ID="lblIn" runat="server" EnableViewState="False" Font-Bold="True" CssClass="clsLabel">21 Aug 05</asp:Label><br>
                                <asp:Label ID="lblEOut" runat="server" EnableViewState="False" CssClass="clsLabel">Check-Out Date :</asp:Label><asp:Label
                                    ID="lblOut" runat="server" EnableViewState="False" Font-Bold="True" CssClass="clsLabel">26 Aug 05</asp:Label><br>
                                <asp:Label ID="lblEResDate" runat="server" EnableViewState="False" CssClass="clsLabel"> Reservation Date:</asp:Label>
                                <asp:Label ID="lblResDate" runat="server" EnableViewState="False" Font-Bold="True"
                                    CssClass="clsLabel">26 Aug 05</asp:Label>
                                <br />
                                <%--<asp:label id="lblNAdultos" runat="server" EnableViewState="False" CssClass="clsLabel">2</asp:label>&nbsp;<asp:label id="lblEAdultos" runat="server" EnableViewState="False" CssClass="clsLabel">Adulto(s), </asp:label><asp:label id="lblNchildren" runat="server" EnableViewState="False" CssClass="clsLabel">0</asp:label>&nbsp;<asp:label id="lblEchildren" runat="server" EnableViewState="False" CssClass="clsLabel">Niño(s)</asp:label><br>
									<asp:label id="LblNAdultosExt" runat="server" EnableViewState="False" CssClass="clsLabel">2</asp:label>&nbsp;<asp:label id="lblEAdultosExt" runat="server" EnableViewState="False" CssClass="clsLabel">Adulto(s) Extra(s) </asp:label><BR>
									<asp:label id="lblNchildrenExt" runat="server" EnableViewState="False" CssClass="clsLabel">0</asp:label>&nbsp;<asp:label id="lblEchildrenExt" runat="server" EnableViewState="False" CssClass="clsLabel">Niño(s) Extra(s)</asp:label><br>--%>
                                <%--<asp:label id="lbelChildAges" runat="server" EnableViewState="False" 
                                        CssClass="clsLabel" Text="Edades Niños:" />
									<asp:label id="lblChildAges" runat="server" EnableViewState="False" 
                                        CssClass="clsLabel" Text="10,12" />--%>
                            </td>
                            <td align="right">
                                <br>
                                <table id="Table8" cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblESystemCode" runat="server" EnableViewState="False" CssClass="clsLabel">System Code :</asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="lblSystemCode" runat="server" EnableViewState="False" Font-Bold="True"
                                                CssClass="clsLabel"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblEBookingSource" runat="server" EnableViewState="False" CssClass="clsLabel">BookingSource</asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="lblBookingSource" runat="server" EnableViewState="False" Font-Bold="True"
                                                CssClass="clsLabel"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblERecordLocator" runat="server" EnableViewState="False" CssClass="clsLabel">RecordLocator:</asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="lblRecordLocator" runat="server" EnableViewState="False" Font-Bold="True"
                                                CssClass="clsLabel"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblERF" runat="server" EnableViewState="False" CssClass="clsLabel">RF</asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="lblRF" runat="server" EnableViewState="False" Font-Bold="True" CssClass="clsLabel"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblESource" runat="server" EnableViewState="False" CssClass="clsLabel">Source : </asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="lblSource" runat="server" EnableViewState="False" Font-Bold="True"
                                                CssClass="clsLabel">Univisit/Galileo</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblEIdBooking" runat="server" EnableViewState="False" CssClass="clsLabel">ID booking : </asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="lblIdBooking" runat="server" EnableViewState="False" Font-Bold="True"
                                                CssClass="clsLabel">000000</asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <uc1:ctrLinckPackage ID="CtrLinckPackage1" runat="server"></uc1:ctrLinckPackage>
                                <br />
                                <a id="ancItems" runat="server" href="javascript:;" class="dglink">Items</a>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="center">
                                <asp:Label ID="lblRaG" runat="server" EnableViewState="False" CssClass="bookingNormalLabel">Rooms and Guest :</asp:Label><br>
                                <asp:Literal ID="LitRooms" runat="server"></asp:Literal>
                                <asp:Label ID="lblNCuartos" runat="server" EnableViewState="False" CssClass="clsLabel">1</asp:Label>&nbsp;<asp:Label
                                    ID="lblECuartos" runat="server" EnableViewState="False" CssClass="clsLabel">Cuartos,</asp:Label><asp:Label
                                        ID="lblNNoches" runat="server" EnableViewState="False" CssClass="clsLabel">5</asp:Label>&nbsp;<asp:Label
                                            ID="lblENoches" runat="server" EnableViewState="False" CssClass="clsLabel">Noches</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="center" colspan="2">
                                <br>
                                <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td align="left" colspan="2">
                                            <asp:Label ID="lblReservationData" runat="server" EnableViewState="False" CssClass="bookingNormalLabel">Reservation data</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblEID" runat="server" EnableViewState="False" CssClass="clsLabel">Reservation ID :</asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblID" runat="server" EnableViewState="False" Font-Bold="True" CssClass="bookingNormalLabel">1520124585</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblNoConf" runat="server" EnableViewState="False" CssClass="clslabel">No. Confirmacion:</asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="lblConfirmationNumber" runat="server" EnableViewState="False" CssClass="bookingNormalLabel">123456</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblRecLoc" runat="server" EnableViewState="False" CssClass="clslabel">RecLoc:</asp:Label>
                                        </td>
                                        <td align="right">
                                            <p align="left">
                                                <asp:Label ID="lblreclocnumber" runat="server" EnableViewState="False" CssClass="bookingNormalLabel">gds123</asp:Label>
                                            </p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblETipoReservacion" runat="server" EnableViewState="False" CssClass="clslabel">Tipo:</asp:Label>
                                        </td>
                                        <td align="right">
                                            <p align="left">
                                                <asp:Label ID="lblTipoReservacion" runat="server" EnableViewState="False" CssClass="bookingNormalLabel">Commisionable</asp:Label>
                                            </p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblEResCurrency" Visible="false" runat="server" EnableViewState="False"
                                                CssClass="clslabel">Moneda Elegida al Reservar:</asp:Label>
                                        </td>
                                        <td align="right">
                                            <p align="left">
                                                <asp:Label ID="lblResCurrency" Visible="false" runat="server" EnableViewState="False"
                                                    CssClass="bookingNormalLabel">MXN</asp:Label>
                                            </p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblEMoneyChange" Visible="false" runat="server" EnableViewState="False"
                                                CssClass="clslabel">Tipo de Cambio:</asp:Label>
                                        </td>
                                        <td align="right">
                                            <p align="left">
                                                <asp:Label ID="lblMoneyChange" Visible="false" runat="server" EnableViewState="False"
                                                    CssClass="bookingNormalLabel">1</asp:Label>
                                            </p>
                                        </td>
                                    </tr>
                                    <tr id="pnlConvenios" runat="server">
                                        <td align="right">
                                            <%=PortalCulture.GetString("M0BT0000195")%>:
                                        </td>
                                        <td align="right">
                                            <p align="left">
                                                <asp:Label ID="lblNoConvenio" runat="server" EnableViewState="False" CssClass="bookingNormalLabel">Commisionable</asp:Label>
                                            </p>
                                        </td>
                                    </tr>
                                    <tr id="pnlConvenios2" runat="server">
                                        <td align="right">
                                            <%=PortalCulture.GetString("01220")%>:
                                        </td>
                                        <td align="right">
                                            <p align="left">
                                                <asp:Label ID="lblEmpresaConvenio" runat="server" EnableViewState="False" CssClass="bookingNormalLabel">Commisionable</asp:Label>
                                            </p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblCompanySegmentTitle" Visible="true" runat="server" EnableViewState="False"
                                                CssClass="clslabel">Empresa de Segmento</asp:Label>
                                        </td>
                                        <td align="right">
                                            <p align="left">
                                                <asp:Label ID="lblCompanySegment" Visible="true" runat="server" EnableViewState="False"
                                                    CssClass="bookingNormalLabel">1</asp:Label>
                                            </p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblSegmentTitle" Visible="true" runat="server" EnableViewState="False"
                                                CssClass="clslabel">Segmento</asp:Label>
                                        </td>
                                        <td align="right">
                                            <p align="left">
                                                <asp:Label ID="lblSegment" Visible="true" runat="server" EnableViewState="False"
                                                    CssClass="bookingNormalLabel">1</asp:Label>
                                            </p>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td valign="top">
                                <br>
                                <div id="DivRollAwayData" runat="server">
                                    <table width="100%" align="center">
                                        <tr>
                                            <td align="left" colspan="4">
                                                <asp:Label ID="lblTitleRollAway" runat="server" EnableViewState="False" CssClass="bookingNormalLabel">Roll Away Data:</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td>
                                                <asp:Label ID="lblRollAwayAdult" runat="server" EnableViewState="False" CssClass="clsLabel">Adult</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblRollAwayChild" runat="server" EnableViewState="False" CssClass="clsLabel">Child</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCrib" runat="server" EnableViewState="False" CssClass="clsLabel">Crib</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:Label ID="lblECantidad" runat="server" EnableViewState="False" CssClass="clsLabel">Cantidad</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCantAdults" runat="server" EnableViewState="False" CssClass="bookingNormalLabel">0</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCantChild" runat="server" EnableViewState="False" CssClass="bookingNormalLabel">0</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCantCrib" runat="server" EnableViewState="False" CssClass="bookingNormalLabel">0</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:Label ID="lblPrice" runat="server" EnableViewState="False" CssClass="clsLabel">Costo</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblPriceAdult" runat="server" EnableViewState="False" CssClass="bookingNormalLabel">$0.0</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblPriceChild" runat="server" EnableViewState="False" CssClass="bookingNormalLabel">$0.0</asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblPriceCrib" runat="server" EnableViewState="False" CssClass="bookingNormalLabel">$0.0</asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" height="5"></td>
                        </tr>
                        <tr>
                            <td valign="top" colspan="2">
                                <table id="Table5" cellspacing="0" cellpadding="0" width="99%" border="0">
                                    <tr>
                                        <td align="left" colspan="2">
                                            <asp:Label ID="lblCustomerData" runat="server" EnableViewState="False" CssClass="bookingNormalLabel">Customer Data</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblEContacto" runat="server" EnableViewState="False" CssClass="clslabel">Main Contact :</asp:Label>
                                        </td>
                                        <td style="width: 145px" align="left">
                                            <asp:Label ID="lblContacto" runat="server" EnableViewState="False" Font-Bold="True"
                                                CssClass="bookingNormalLabel"> Contacto</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblEmail" runat="server" EnableViewState="False" CssClass="clsLabel">E-mail Address :</asp:Label>
                                        </td>
                                        <td style="width: 145px">
                                            <asp:Label ID="lblMail" runat="server" EnableViewState="False" Font-Bold="True" CssClass="bookingNormalLabel">E-mail</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblEPhoneH" runat="server" EnableViewState="False" CssClass="clsLabel">Home Phone :</asp:Label>
                                        </td>
                                        <td style="width: 145px">
                                            <asp:Label ID="lblPhoneH" runat="server" EnableViewState="False" Font-Bold="True"
                                                CssClass="bookingNormalLabel">Phone Home</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 13px" align="right">
                                            <asp:Label ID="lblEPhoneW" runat="server" EnableViewState="False" CssClass="clsLabel">Work Phone :</asp:Label>
                                        </td>
                                        <td style="width: 145px; height: 13px">
                                            <asp:Label ID="lblPhoneW" runat="server" EnableViewState="False" Font-Bold="True"
                                                CssClass="bookingNormalLabel">Phone Work</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblEAddress" runat="server" EnableViewState="False" CssClass="clsLabel">Address :</asp:Label>
                                        </td>
                                        <td style="width: 145px">
                                            <asp:Label ID="lblAddress" runat="server" EnableViewState="False" Font-Bold="True"
                                                CssClass="bookingNormalLabel">Callejón A</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblECode" runat="server" EnableViewState="False" CssClass="clsLabel">Zip Code :</asp:Label>
                                        </td>
                                        <td style="width: 145px">
                                            <asp:Label ID="lblZipCode" runat="server" EnableViewState="False" Font-Bold="True"
                                                CssClass="bookingNormalLabel">23000</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblCiudad" runat="server" EnableViewState="False" CssClass="clsLabel">City :</asp:Label>
                                        </td>
                                        <td style="width: 145px">
                                            <asp:Label ID="lblCustCity" runat="server" EnableViewState="False" Font-Bold="True"
                                                CssClass="bookingNormalLabel">City A</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblEstado" runat="server" EnableViewState="False" CssClass="clsLabel">County :</asp:Label>
                                        </td>
                                        <td style="width: 145px">
                                            <asp:Label ID="lblCustCounty" runat="server" EnableViewState="False" Font-Bold="True"
                                                CssClass="bookingNormalLabel">County A</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblPais" runat="server" EnableViewState="False" CssClass="clsLabel">Country :</asp:Label>
                                        </td>
                                        <td style="width: 145px">
                                            <asp:Label ID="lblCustCountry" runat="server" EnableViewState="False" Font-Bold="True"
                                                CssClass="bookingNormalLabel">Country A</asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <div id="divClientportal" runat="server">
                                    <span id="AgencyInfo" runat="server"></span><span id="AgencyRecepcion" runat="server"></span>
                                </div>
                            </td>
                            <td valign="top">
                                <asp:Panel ID="pnlTC" runat="server">
                                    <table id="Table3" cellspacing="0" cellpadding="0" width="300" border="0">
                                        <tr>
                                            <td align="left" colspan="2">
                                                <asp:Label ID="lblCCTitle" runat="server" EnableViewState="False" CssClass="bookingNormalLabel">Credit Card Data</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:Label ID="lblCCTipo" runat="server" EnableViewState="False" CssClass="clslabel" Visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCCType" runat="server" EnableViewState="False" CssClass="bookingNormalLabel" Visible="false"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:Label ID="lblCCNa" runat="server" EnableViewState="False" CssClass="clslabel" Visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCCName" runat="server" EnableViewState="False" CssClass="bookingNormalLabel" Visible="false"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:Label ID="lblCCNu" runat="server" EnableViewState="False" CssClass="clslabel"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCCNumber" runat="server" EnableViewState="False" CssClass="bookingNormalLabel"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:Label ID="lblCCcv" runat="server" EnableViewState="False" CssClass="clslabel" Visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCCcvNumber" runat="server" EnableViewState="False" CssClass="bookingNormalLabel" Visible="false"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:Label ID="lblCCExp" runat="server" CssClass="clslabel" Visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCCExpDate" runat="server" EnableViewState="False" CssClass="bookingNormalLabel" Visible="false"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <center>
                                                    <asp:LinkButton ID="lnkShowCC" runat="server">Mostrar datos</asp:LinkButton></center>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3"></td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:DataGrid ID="dgReservas" runat="server" EnableViewState="False" AutoGenerateColumns="False"
                                    HorizontalAlign="Center" Width="100%" CssClass="DataGrid">
                                    <SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
                                    <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
                                    <ItemStyle CssClass="dgItem"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Center" CssClass="dgHeader" VerticalAlign="Middle"></HeaderStyle>
                                    <Columns>
                                        <asp:TemplateColumn>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblNumCuartos" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:BoundColumn DataField="viajero">
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="Adultos" Visible="false">
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="NombreHabitacion">
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                        </asp:BoundColumn>
                                        <%--<asp:BoundColumn DataField="rateplan">
												<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
											</asp:BoundColumn>--%>
                                        <asp:TemplateColumn HeaderText="rateplan">
                                            <ItemTemplate>
                                                <asp:Label runat="server">													
														<%#DataBinder.Eval(Container, "DataItem.rateplan")%>   
                                                </asp:Label>&nbsp;
                                            <asp:Label ID="Label3" runat="server">													
														<%#DataBinder.Eval(Container, "DataItem.rateplanName")%>
                                            </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:BoundColumn DataField="Preferencia">
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                        </asp:BoundColumn>
                                    </Columns>
                                </asp:DataGrid>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="3">
                                <input class="button" id="btnPrint" onclick="window.print();" type="button" value='<%=PortalCulture.GetString("00606") %>'>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 14px" colspan="3" height="14"></td>
                        </tr>
                        <tr>
                            <td colspan="2"></td>
                            <td>
                                <a id="aNRPolicies" name="NRPoliticies" runat="server" href="#NRPoliticies" class="showOptions" style="display: none;">Details</a><br />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <div id="dvNRPolicies" style="padding-right: 3px; display: none; padding-left: 3px; border: solid 1px #ccc; padding-bottom: 3px; padding-top: 3px; -moz-border-radius: 7px; -webkit-border-radius: 7px; -khtml-border-radius: 7px; border-radius: 7px;">
                                    <table id="bookingcontainer" align="right" style="padding: 0;">
                                        <tr>
                                            <td width="95%"></td>
                                            <td width="5%" align="right">
                                                <img align="right" runat="server" id="imgNRclose" src="~/Includes/imagenes/Close.ico"
                                                    style="cursor: hand">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Literal ID="lblNRPolicies" runat="server"></asp:Literal>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2"></td>
                            <td>
                                <a id="aPolices" runat="server" name="Politicie" href="#Politicie" class="showOptions" style="display: inline-block;">Details</a>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <div id="divPolicies" style="padding-right: 3px; display: none; padding-left: 3px; border: solid 1px #ccc; padding-bottom: 3px; padding-top: 3px; -moz-border-radius: 7px; -webkit-border-radius: 7px; -khtml-border-radius: 7px; border-radius: 7px;">
                                    <table id="bookingcontainer" align="right" style="padding: 0;">
                                        <tr>
                                            <td width="95%"></td>
                                            <td width="5%" align="right">
                                                <img align="right" runat="server" id="imgPoliceClose" src="~/Includes/imagenes/Close.ico"
                                                    style="cursor: hand">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <table>
                                                    <tr id="trCancelPol" runat="server">
                                                        <td colspan="3">
                                                            <p>
                                                                <b>
                                                                    <asp:Label ID="lblCancelPolTitle" runat="server">Cancelacion:</asp:Label></b>
                                                            </p>
                                                            <p>
                                                                <asp:Label ID="lblCancelationPolices" runat="server"></asp:Label>
                                                            </p>
                                                            <br />
                                                        </td>
                                                    </tr>
                                                    <tr id="trCardPol" runat="server">
                                                        <td colspan="3">
                                                            <p>
                                                                <b>
                                                                    <asp:Label ID="lblCardPolTitle" runat="server">Tarjeta de credito:</asp:Label></b>
                                                            </p>
                                                            <p>
                                                                <asp:Label ID="lblCreditCardPolices" runat="server"></asp:Label>
                                                            </p>
                                                            <br />
                                                        </td>
                                                    </tr>
                                                    <tr id="trGuarPol" runat="server">
                                                        <td colspan="3">
                                                            <p>
                                                                <b>
                                                                    <asp:Label ID="lblGuarPolTitle" runat="server">Garantias:</asp:Label></b>
                                                            </p>
                                                            <p>
                                                                <asp:Label ID="lblGuaranteePolices" runat="server"></asp:Label>
                                                            </p>
                                                            <br />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="3">
                                <asp:Label ID="lblCostos" runat="server" EnableViewState="False" CssClass="bookingNormalLabel"
                                    Font-Size="8pt">Cost and Travel Summary</asp:Label>
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td runat="server" id="tdCostoHotel">
                                            <table id="Table2" cellspacing="0" cellpadding="0" border="0">
                                                <tr>
                                                    <td colspan="3" id="tdTextHotel" align="center" runat="server">
                                                        <b>HOTEL</b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        <asp:Label ID="lblECosto" runat="server" EnableViewState="False" CssClass="clsLabel"
                                                            Visible="False">Costo por noche</asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lblCosto" runat="server" EnableViewState="False" CssClass="clslabel"
                                                            Visible="False">$ X.00</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl1" runat="server" EnableViewState="False" CssClass="clslabel" Visible="false"> /noche</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblEFees" runat="server" EnableViewState="False" CssClass="clsLabel">Procesing Fee(-):</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblFees" runat="server" EnableViewState="False" CssClass="clslabel"
                                                            Visible="false">$ X.00</asp:Label>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblEComision" runat="server" EnableViewState="False" CssClass="clsLabel"
                                                            Visible="false">Comision</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblComision" runat="server" EnableViewState="False" CssClass="clslabel">$ X.00</asp:Label>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td></td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        <asp:Label ID="lblETotal" runat="server" EnableViewState="False" CssClass="clsLabel">Total :</asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lblTotal" runat="server" EnableViewState="False" CssClass="clslabel">$ X.00</asp:Label>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        <asp:Label ID="lblMsgImpuesto" runat="server" EnableViewState="False" CssClass="clsLabel">Impuestos incluidos</asp:Label>
                                                    </td>
                                                    <td></td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        <asp:Label ID="lblEImpuesto" runat="server" EnableViewState="False" CssClass="clsLabel"
                                                            Visible="False">Impuestos</asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lblImpuestos" runat="server" EnableViewState="False" CssClass="clslabel"
                                                            Visible="False">$ X.00 </asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl2" runat="server" EnableViewState="False" CssClass="clslabel"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        <asp:Label ID="lblETotalH" runat="server" EnableViewState="False" CssClass="clslabel">Total a cobrar al cliente :</asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lblTotalH" runat="server" EnableViewState="False" CssClass="bookingNormalLabel">$ X.00</asp:Label>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td align="right">&nbsp;
                                                    </td>
                                                    <td align="right">&nbsp;
                                                    </td>
                                                    <td></td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="width: 10px"></td>
                                        <td runat="server" id="tdCostoUV">
                                            <table id="TableNR" cellspacing="0" cellpadding="0" border="0">
                                                <tr>
                                                    <td colspan="3" id="tdTextUnivisit" align="center" runat="server">
                                                        <b>UNIVISIT</b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        <asp:Label ID="lblECostoUV" runat="server" EnableViewState="False" CssClass="clsLabel"
                                                            Visible="false">Costo por noche</asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lblCostoUV" runat="server" EnableViewState="False" CssClass="clslabel"
                                                            Visible="False">$ X.00</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl1UV" runat="server" EnableViewState="False" CssClass="clslabel"
                                                            Visible="False"> /noche</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblEFeesUV" runat="server" EnableViewState="False" CssClass="clsLabel"
                                                            Visible="False">Procesing Fee(-):</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblFeesUV" runat="server" EnableViewState="False" CssClass="clslabel"
                                                            Visible="False">$ X.00</asp:Label>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblEComisionUV" runat="server" EnableViewState="False" CssClass="clsLabel"
                                                            Visible="False">Comision</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblComisionUV" runat="server" EnableViewState="False" CssClass="clslabel"
                                                            Visible="False">$ X.00</asp:Label>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td></td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        <asp:Label ID="lblETotalUV" runat="server" EnableViewState="False" CssClass="clsLabel">Total :</asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lblTotalUV" runat="server" EnableViewState="False" CssClass="clslabel">$ X.00</asp:Label>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        <asp:Label ID="lblMsgImpuestoUV" runat="server" EnableViewState="False" CssClass="clsLabel">Impuestos incluidos</asp:Label>
                                                    </td>
                                                    <td></td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        <asp:Label ID="lblEImpuestoUV" runat="server" EnableViewState="False" CssClass="clsLabel"
                                                            Visible="False">Impuestos</asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lblImpuestosUV" runat="server" EnableViewState="False" CssClass="clslabel"
                                                            Visible="False">$ X.00 </asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl2UV" runat="server" EnableViewState="False" CssClass="clslabel"
                                                            Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        <asp:Label ID="lblETotalHUV" runat="server" EnableViewState="False" CssClass="clslabel">Total a cobrar al cliente :</asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lblTotalHUV" runat="server" EnableViewState="False" CssClass="bookingNormalLabel">$ X.00</asp:Label>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td align="right">&nbsp;
                                                    </td>
                                                    <td align="right">&nbsp;
                                                    </td>
                                                    <td></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="3" style="padding-top: 12px;">
                                <asp:Label ID="Label1" runat="server" EnableViewState="False" CssClass="bookingNormalLabel"
                                    Font-Size="8pt"><%=PortalCulture.GetString("01226")%></asp:Label>
                                <asp:Literal ID="litDetalleTarifa" runat="server" EnableViewState="False"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3"></td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="lblDeposito" runat="server" CssClass="dgpager">Label</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="3">
                                <input class="button" id="btnCancel" style="" onclick="javascript: ShowConfirm();"
                                    type="button" value="Cancel " name="btnCancel" runat="server">&nbsp;
                            <asp:Button ID="btnReactive" CssClass="button" runat="server" Text="Reactive" />
                                <asp:Button ID="btnStatusPMS" CssClass="button" runat="server" Text="Cambiar Status PMS"
                                    Visible="False" />
                                <% If IsSupervisor Then%>
                                <input class="button" id="Modify" style="" onclick="javascript: ShowResStatus();"
                                    type="button" value="Modify" name="btnResStatus" runat="server">
                                <input class="button" id="btnResConfirm" style="" onclick="javascript: ShowResConfirm();"
                                    type="button" value="Confirm" name="btnResConfirm" runat="server" visible="false">
                                <%End If%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox runat="server" ID="txtMultiCancel" Width="300px" Visible="false"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button ID="btnMultiCancel" CssClass="button" runat="server" Text="Cancelación Multiple" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3"></td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="lblError" runat="server" CssClass="validators" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="lblErrorMotivo" runat="server" CssClass="validators" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <div id="ConfirmCancel" style="display: none">
                                    <table id="Table7" style="border-right: #cbdced 1px solid; border-top: #cbdced 1px solid; border-left: #cbdced 1px solid; border-bottom: #cbdced 1px solid"
                                        cellspacing="1"
                                        cellpadding="1" width="60%" align="center" bgcolor="#bcc9d8" border="0">
                                        <tr>
                                            <td align="center" bgcolor="#cbdced" colspan="3">
                                                <asp:Label ID="lblConfirmCancel" runat="server" EnableViewState="False" Font-Bold="True"
                                                    CssClass="bookingNormalLabel" ForeColor="SteelBlue" BackColor="Transparent">Confirm Cancellation</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" style="text-align: left">
                                                <asp:Label ID="lblMotivo" runat="server" EnableViewState="False" CssClass="clslabel">Motivo</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="3" style="vertical-align: top;">
                                                <asp:TextBox ID="txtMotivo" runat="server" Width="600px" TextMode="MultiLine" MaxLength="250"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="3">
                                                <asp:Label ID="lblSure" runat="server" EnableViewState="False" CssClass="clslabel">Sure??</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" width="50%">
                                                <asp:LinkButton ID="hplCancelRes" runat="server" EnableViewState="False" CssClass="dgLink">Yes</asp:LinkButton>
                                            </td>
                                            <td align="right"></td>
                                            <td width="50%">
                                                <asp:HyperLink ID="HyperLink2" runat="server" EnableViewState="False" CssClass="Link"
                                                    NavigateUrl="javascript:HideConfirm();">No</asp:HyperLink>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <div id="backgroundPopup" style="display: none;">
        </div>
        <div id="StatusPopup" style="display: none;">
            <table id="Table9" style="border-right: #cbdced 1px solid; border-top: #cbdced 1px solid; border-left: #cbdced 1px solid; border-bottom: #cbdced 1px solid; margin: 50px;"
                cellspacing="1"
                cellpadding="1" width="700px" align="center" bgcolor="#bcc9d8" border="0" height="200px">
                <tr>
                    <td align="center" bgcolor="#cbdced" colspan="3">
                        <asp:Label ID="lblConfirmModify" runat="server" EnableViewState="False" Font-Bold="True"
                            CssClass="bookingNormalLabel" ForeColor="SteelBlue" BackColor="Transparent">Confirm modification</asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="vertical-align: top;" width="32%">
                        <asp:Label ID="lblNombreCL" runat="server" EnableViewState="False" CssClass="clslabel">Nombre:</asp:Label>
                        <asp:TextBox ID="txtNombreCL" runat="server" />
                        <asp:RequiredFieldValidator ID="RFNombre" runat="server" ErrorMessage="Campo Requerido" ValidationGroup="ModifyRes" ControlToValidate="txtNombreCL"></asp:RequiredFieldValidator>
                    </td>
                    <td align="left" style="vertical-align: top;" width="33%">
                        <asp:Label ID="lblApelldoCL" runat="server" EnableViewState="False" CssClass="clslabel">Apellido:</asp:Label>
                        <asp:TextBox ID="txtApelldoCL" runat="server" />
                        <asp:RequiredFieldValidator ID="RFApellido" runat="server" ErrorMessage="Campo Requerido" ValidationGroup="ModifyRes" ControlToValidate="txtApelldoCL"></asp:RequiredFieldValidator>
                    </td>
                    <td align="left" style="vertical-align: top;" width="35%">
                        <asp:Label ID="lblCheckIn" runat="server" EnableViewState="False" CssClass="clslabel">Check In:</asp:Label>
                        <asp:TextBox ID="txtCheckIn" Columns="10" MaxLength="10" runat="server"></asp:TextBox><a hidefocus onclick="if(self.gfPop)gfPop.fPopCalendar1(document.getElementById('txtCheckIn'),document.getElementById('txtCheckOut'));return false;"
                            href="javascript:void(0)"><img class="PopcalTrigger" id="PopcalTrigger" alt="" src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>' align="absMiddle" border="0"></a><br>
                        <asp:Label ID="lblErrorCheckin" runat="server" EnableViewState="True" Style="display: none;" CssClass="validators">Invalid Date</asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="vertical-align: top;" width="29%">
                        <asp:Label ID="lblTotalModify" runat="server" EnableViewState="False" CssClass="clslabel">Total:</asp:Label>
                        <asp:TextBox ID="txtTotalModify" Style="TEXT-ALIGN: right" CssClass="TextBox currency" runat="server" MaxLength="10" Columns="10">
                        </asp:TextBox>
                        <span id="spnCurrencyTotal" runat="server" class="currency"></span>
                        <asp:RequiredFieldValidator ID="RFTotal" runat="server" ErrorMessage="Campo Requerido" ValidationGroup="ModifyRes" ControlToValidate="txtTotalModify"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="valTotalMidfy" ValidationGroup="ModifyRes" Display="Dynamic" CssClass="Validators" runat="server" ControlToValidate="txtTotalModify"
                            ValidationExpression="^([0-9]*|\d*\.\d{1}?\d*)$" ErrorMessage="* Precio Inválido">* Tarifa Inválida</asp:RegularExpressionValidator>
                    </td>
                    <td align="left" style="vertical-align: top;" width="29%">
                        <asp:Label ID="lblTotalNRModify" runat="server" EnableViewState="False" CssClass="clslabel">TotalNR:</asp:Label>
                        <asp:TextBox ID="txtTotalNRModify" Style="TEXT-ALIGN: right" CssClass="TextBox currency" runat="server" MaxLength="10" Columns="10">
                        </asp:TextBox>
                        <span id="spnCurrencyTotalNR" runat="server" class="currency"></span>
                        <asp:RequiredFieldValidator ID="RFTotalNR" runat="server" ErrorMessage="Campo Requerido" ValidationGroup="ModifyRes" ControlToValidate="txtTotalNRModify"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="valTotalMidfyNR" ValidationGroup="ModifyRes" Display="Dynamic" CssClass="Validators" runat="server" ControlToValidate="txtTotalNRModify"
                            ValidationExpression="^([0-9]*|\d*\.\d{1}?\d*)$" ErrorMessage="* Precio Inválido">* Tarifa Inválida</asp:RegularExpressionValidator>
                    </td>
                    <td align="left" style="vertical-align: top;" width="42%">
                        <asp:Label ID="lblCheckOut" runat="server" EnableViewState="False" CssClass="clslabel">Check Out:</asp:Label>
                        <asp:TextBox ID="txtCheckOut" Columns="10" MaxLength="10" runat="server"></asp:TextBox><a hidefocus onclick="if(self.gfPop)gfPop.fPopCalendar1(document.getElementById('txtCheckOut'));return false;"
                            href="javascript:void(0)"><img class="PopcalTrigger" id="IMG1" alt="" src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>' align="absMiddle" border="0"></a><br>
                        <asp:Label ID="lblErrorCheckOut" runat="server" EnableViewState="True" Style="display: none;" CssClass="validators">Invalid Date</asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="text-align: left;">
                        <asp:Label ID="lbErrorTotals" runat="server" EnableViewState="True" Style="display: none;" CssClass="validators"></asp:Label><br />
                        <asp:Label ID="lberrorTotalsZero" runat="server" EnableViewState="True" Style="display: none;" CssClass="validators"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="text-align: center;" colspan="3">
                        <asp:LinkButton ID="hplModifyRes" runat="server" EnableViewState="False" CssClass="dgLink" ValidationGroup="ModifyRes" OnClientClick="javascript:return validateData();">Save</asp:LinkButton>

                        <asp:HyperLink ID="HyperLink3" runat="server" EnableViewState="False" CssClass="Link"
                            NavigateUrl="javascript:HideResStatus();">Cancel</asp:HyperLink>
                    </td>
                </tr>
            </table>
            <script type="text/javascript">
                function validateData() {
                    document.getElementById("lbErrorTotals").style.display = "none";
                    document.getElementById("lblErrorCheckin").style.display = "none";
                    var isValid = false;
                    var total = parseFloat(document.getElementById("txtTotalModify").value);
                    var totalNR = parseFloat(document.getElementById("txtTotalNRModify").value);
                    if (total >= totalNR) {
                        isValid = true;
                    } else if (total <= 0 || totalNR <= 0) {
                        document.getElementById("lberrorTotalsZero").style.display = "block";
                        isValid = false;
                    } else {
                        document.getElementById("lbErrorTotals").style.display = "block";
                        isValid = false;
                    }

                    var CheckIn = new Date(document.getElementById("txtCheckIn").value.split("/")[2], document.getElementById("txtCheckIn").value.split("/")[0], document.getElementById("txtCheckIn").value.split("/")[1], 0, 0, 0, 0);
                    var CheckOut = new Date(document.getElementById("txtCheckOut").value.split("/")[2], document.getElementById("txtCheckOut").value.split("/")[0], document.getElementById("txtCheckOut").value.split("/")[1], 0, 0, 0, 0);
                    if (isValid == true) {
                        if (CheckIn < CheckOut) {
                            isValid = true;
                        } else {
                            document.getElementById("lblErrorCheckin").style.display = "block";
                            isValid = false;
                        }
                    }

                    return isValid;
                }
            </script>
        </div>
        <div id="ConfirmPopup" style="display: none;">
            <table id="tblConfirm" style="border-right: #cbdced 1px solid; border-top: #cbdced 1px solid; border-left: #cbdced 1px solid; border-bottom: #cbdced 1px solid; margin: 50px;"
                cellspacing="1"
                cellpadding="1" width="700px" align="center" bgcolor="#bcc9d8" border="0" height="200px">
                <tr>
                    <td></td>
                </tr>
                <tr>
                    <td align="left" style="vertical-align: top;" width="32%">
                        <asp:Label ID="lblConfirmNumber" runat="server" EnableViewState="False" CssClass="clslabel">Número de Autorización: </asp:Label>
                        <asp:TextBox ID="txtConfirmNumber" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvResConfirm" runat="server" ErrorMessage="Campo Requerido" ValidationGroup="ResConfirm" ControlToValidate="txtConfirmNumber"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="text-align: center;" colspan="3">
                        <asp:LinkButton ID="hplSaveConfirm" ValidationGroup="ResConfirm" runat="server" EnableViewState="False" CssClass="dgLink" OnClientClick="">Save</asp:LinkButton>

                        <asp:HyperLink ID="hplConfirmCancel" runat="server" EnableViewState="False" CssClass="Link"
                            NavigateUrl="javascript:HideResConfirm();">Cancel</asp:HyperLink>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
