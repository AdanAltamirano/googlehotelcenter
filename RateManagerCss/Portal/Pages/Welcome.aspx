<%@ Import Namespace="RateManager" %>
<%@ Import Namespace="Club100" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Welcome.aspx.vb" Inherits="RateManager.Welcome1" %>

<script runat="server">

    Private Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        loadCorporateAndAsociateHotelInfo()
    End Sub
    Private Sub loadCorporateAndAsociateHotelInfo()
        If Usuario = 0 Then Exit Sub
        Dim reader As System.Data.DataSet
        With New Portal.General.Facade.HotelSistema
            reader = .GetHotelsCompanyByUser(Usuario)
        End With
        Dim cInfo As New companyInfo
        If Not reader.Tables(0) Is Nothing Then
            If reader.Tables(0).Rows.Count = 1 Then
                If Not Me.cInfoActual Is Nothing Then
                    With reader.Tables(0).Rows(0)
                        If Not .IsNull("idcorporativo") Then
                            Me.cInfoActual.IdCorporate = .Item("idcorporativo")
                        End If
                        If Not .IsNull("idasociacion") Then
                            Me.cInfoActual.IdAsociation = .Item("idasociacion")
                        End If
                    End With
                End If
            End If
        End If

    End Sub

    Private Const CLUB100URL As String = ""
    Private _club100Info As Data.DataRow

    Protected Enum Club100StatusEntitie
        Unknow = 0
        Review = 2
        Active = 1
        Unregistred = 3
    End Enum

    Private ReadOnly Property Club100Info() As Data.DataRow
        Get
            If Me._club100Info Is Nothing Then
                Dim data As Data.DataSet = Club100.CapaDatos.sql.get_cuestionario(Me.cInfoActual.Hotel)
                If data IsNot Nothing AndAlso data.Tables.Contains("cuestionario") AndAlso data.Tables("cuestionario").Rows.Count > 0 Then
                    Me._club100Info = data.Tables("cuestionario").Rows(0)
                End If
            End If
            Return Me._club100Info
        End Get
    End Property

    Protected ReadOnly Property IsClub100Enabled() As Boolean
        Get
            Return If(Me.cInfoActual.IdPais Is Nothing, False, (Me.cInfoActual.IdPais.ToUpper() = "MX"))
        End Get
    End Property

    Protected ReadOnly Property Club100Status() As Club100StatusEntitie
        Get
            Dim temp As Club100StatusEntitie = Club100StatusEntitie.Unregistred

            If Me.Club100Info IsNot Nothing Then
                If Me.Club100Info.Table.Columns.Contains("estado") Then
                    Try
                        If Me.Club100Info("estado") = 0 Then
                            temp = Club100StatusEntitie.Review
                        ElseIf Me.Club100Info("estado") = 1 Then
                            temp = Club100StatusEntitie.Active
                        End If
                    Catch ex As Exception
                    End Try
                End If
            End If

            Return temp
        End Get
    End Property

    Protected ReadOnly Property Club100Points() As Integer
        Get
            Dim temp As Integer = 0
            If Me.Club100Info IsNot Nothing AndAlso Me.Club100Info.Table.Columns.Contains("puntos") AndAlso Not IsDBNull(Me.Club100Info("puntos")) Then
                Integer.TryParse(Me.Club100Info("puntos"), temp)
            End If
            Return temp
        End Get
    End Property

    Protected Function GetLabel(ByVal key As String) As String
        Return PortalCulture.GetString(key)
    End Function


    Protected Sub LoadStatusHotelUniBilling(ByVal companyID As Integer, ByRef showCancel As Boolean, ByRef showWithDebit As Boolean)

        showCancel = False
        showWithDebit = False
        Try

            Dim da As New Data.SqlClient.SqlDataAdapter
            Dim tb As New Data.DataTable
            Dim cnn As Data.SqlClient.SqlConnection = New Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.AppSettings("HotelConnection"))
            Dim cmm As Data.SqlClient.SqlCommand = New Data.SqlClient.SqlCommand("spHotelInformationUniBilling", cnn)
            cmm.CommandType = Data.CommandType.StoredProcedure
            cmm.Parameters.Add(New Data.SqlClient.SqlParameter("@companyID", companyID))
            da.SelectCommand = cmm
            da.Fill(tb)
            If (tb IsNot Nothing AndAlso tb.Rows.Count > 0) Then
                Dim paymentRequeriment As Integer = tb.Rows(0)("PaymentRequirement")
                Dim Active As Boolean = tb.Rows(0)("Active")
                If Not (Active) Then
                    showCancel = True
                End If
                If paymentRequeriment = 3 Then
                    showWithDebit = True
                End If
            End If
        Catch ex As Exception
        End Try

    End Sub

</script>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>Welcome</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">

    <script src="<%= Me.ResolveUrl("~/Pages/Scripts/jquery.min.js") %>" type="text/javascript"></script>

    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../../StyleSheets/Styles.css" type="text/css" rel="stylesheet">

    <script language="javascript" src="../../xmlHttp.js"></script>

    <script language="javascript" src="../../Logic.js"></script>

    <style>
        .Club100Box
        {
            margin: 4px;
            padding: 4px;
        }
        .Club100Box img
        {
            width: 137px;
            border: 0 none;
        }
        .Club100Box a.Logo:visited, .Club100Box a.Logo:hover, .Club100Box a.Logo
        {
            border: none;
            text-decoration: none;
        }
        .Club100Box div
        {
            padding: 10px;
        }
    </style>

    <script type="text/javascript" language="javascript">
        (function($) {
            $.fn.blink = function(options) {

                var defaults = {
                    //highlightClass: "highlight",
                    //blinkCount: 3,
                    fadeDownSpeed: "slow",
                    fadeUpSpeed: "slow",
                    fadeToOpacity: 0.33
                };
                var options = $.extend(defaults, options);

                return this.each(function() {
                    var obj = $(this);
                    //var blinkCount = 0;

                    //obj.addClass(options.highlightClass);
                    doBlink();

                    function doBlink() {
                        //if (!options.stop) {
                            //if (blinkCount < options.blinkCount) {
                            obj.fadeTo(options.fadeDownSpeed, options.fadeToOpacity, function() {
                                obj.fadeTo(options.fadeUpSpeed, 1.0, doBlink);
                            });
                            //} else {
                            //  obj.removeClass(options.highlightClass);
                            //}
                            //blinkCount++;
//                        }
//                        else {
//                            obj.removeClass(options.highlightClass);
//                           
//                        }
                    }
                });

            };
        })(jQuery);

        $(document).ready(function() {
            var countDepositos = 0;
            var countReservations = 0;
            var countCryptoDeposits = 0;
            countDepositos = $('#<%=dgDepositos.ClientID%> tr').length - 2;
            countReservations = $('#<%=dgReservations.ClientID%> tr').length - 2;
            countCryptoDeposits = $('#<%=dgCryptoDeposits.ClientID%> tr').length - 2;

            $('#<%=lblLastResevations.ClientID%>').append("<strong> (" + countReservations + ")</strong>");
            $('#<%=lblLastResevationsDep.ClientID%>').append("<strong> (" + countDepositos + ")</strong>");
            $('#<%=lblCryptoDeposits.ClientID%>').append("<strong> (" + countCryptoDeposits + ")</strong>");

            $("#tabs div").click(function() {
                if ($(this).attr("id") == "reservaciones") {
                    $("#dvReservations").show();
                    $('#dvCryptoDeposits').hide();
                    $("#dvDeposits").hide();
                    $("#activeTab").attr("value", "1");
                    $('#reservaciones').stop().css({ 'opacity': '1' });
                }
                else if ($(this).attr('id') == 'cryptoDeposits') {
                    $('#dvCryptoDeposits').show();
                    $('#dvReservations').hide();
                    $('#dvDeposits').hide();
                    $("#activeTab").attr("value", "3");
                    $('#cryptoDeposits').stop().css({ 'opacity': '1' });
                }
                else {
                    $("#dvDeposits").show();
                    $("#dvReservations").hide();
                    $('#dvCryptoDeposits').hide();
                    $("#activeTab").attr("value", "2");
                    $('#depositos').stop().css({ 'opacity': '1' });
                }
                $("#tabs div").removeClass("TabSelected");
                $("#tabs div").removeClass("tab");
                $("#tabs div").addClass("tab");
                $(this).addClass("TabSelected");
                $(this).removeClass("tab");
                onResizeIframe();
            });

            if ($("#activeTab").attr("value") != "") {
                $("#tabs div").removeClass("TabSelected");
                $("#tabs div").removeClass("tab");
                $("#tabs div").addClass("tab");
                if ($("#activeTab").attr("value") == "1") {
                    $("#dvReservations").show();
                    $("#dvDeposits").hide();
                    $("#activeTab").attr("value", "1");


                    $("#tabRes").addClass("TabSelected");
                    $("#tabRes").removeClass("tab");
                }
                else {
                    $("#dvDeposits").show();
                    $("#dvReservations").hide();
                    $("#activeTab").attr("value", "2");

                    $("#tabDep").addClass("TabSelected");
                    $("#tabDep").removeClass("tab");

                }
                onResizeIframe();
            }

            if (countReservations > 0) {
                $('#reservaciones').blink();
            }
            if (countDepositos > 0) {
                $('#depositos').blink();
            }



            //            function effectFadeInd(classname) {
            //                if (d)
            //                    $("#" + classname).fadeOut(800).fadeIn(800, effectFadeOutd(classname));
            //            }

            //            function effectFadeOutd(classname) {
            //                if (d)
            //                    $("#" + classname).fadeIn(800).fadeOut(800, effectFadeInd(classname));
            //            }

            //            function effectFadeInr(classname) {
            //                if (r) {
            //                    alert(r);
            //                    $("#" + classname).fadeOut(800).fadeIn(800, effectFadeOutr(classname));
            //                }
            //            }

            //            function effectFadeOutr(classname) {
            //                if (r)
            //                    $("#" + classname).fadeIn(800).fadeOut(800, effectFadeInr(classname));
            //            }


            //            $('#<%=lblLastResevationsDep.ClientID%>').click(function() {
            //                d = false;
            //            });

            //            $('#<%=lblLastResevations.ClientID%>').click(function() {
            //                r = false;
            //            });
        });

       
    </script>

</head>
<body ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <div id="tituloSeccion">
        <img class="bgplus" src="../../Includes/imagenes/icono-big-plus.png" width="25" height="25" />
        <span class="tituloSeccion"><%=PortalCulture.GetString("00191")%></span>
    </div>
    <div style="clear: both">
        <table id="bookingcontainer" height="400" cellspacing="0" cellpadding="0" width="760"
            border="0">
            <tr>
                <td align="right" colspan="3">
                    <asp:Label ID="lblLastAccess" runat="server" EnableViewState="False" CssClass="bookingNormalLabel">Ultimo acceso al sistema</asp:Label>
                    <asp:Label ID="lblFechaLastAccess" runat="server" EnableViewState="False">01/01/2007</asp:Label>
                </td>
            </tr>
            <tr>
                <td style="height: 209px" valign="top" align="center" width="15%">
                    <table cellspacing="1" cellpadding="1" width="95%" border="0">
                        <tr align="left">
                            <td align="center">
                                <asp:Image ID="imgLogo" CssClass="images" runat="server" EnableViewState="False" ImageUrl="../../Includes/imagenes/noavaliable.jpg" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblNombre" runat="server" EnableViewState="False">Test Hotel</asp:Label><br />
                                <asp:Label ID="lblCity" runat="server" EnableViewState="False">La Paz, BCS</asp:Label><br />
                                <asp:Label ID="lblAdress" runat="server" EnableViewState="False">Abasolo y Serdan</asp:Label><br />
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <a id="aInfo" target="_blank" runat="server">Ver Status de la empresa</a>
                            </td>
                        </tr>
                    </table>
                    <%-- Club 100 Info --%>
                   <%-- <div>
                        <%If Me.IsClub100Enabled Then%>
                        <div class="Club100Box">
                            <a target="_blank" href="https://crs.univisit.com/club100">
                                <img alt="" src="../../Images/logosClub100.png" /></a>
                            <%Select Case Me.Club100Status%>
                            <%Case Club100StatusEntitie.Active%>
                            <div style="text-align: right">
                                <%=PortalCulture.GetString("01242")%>:</br><b><%=Me.Club100Points.ToString()%>
                                    <%=PortalCulture.GetString("M000135").ToLower%>
                                    100</b></div>
                            <%Case Club100StatusEntitie.Review%>
                            <div class="DarkRedLabel" style="text-align: right">
                                <%=Me.GetLabel("01243")%></div>
                            <%Case Club100StatusEntitie.Unregistred%>
                            <div style="text-align: right">
                                <a target="_blank" href="https://crs.univisit.com/club100">
                                    <%=Me.GetLabel("01244")%></a></div>
                            <%End Select%>
                        </div>
                        <%Else%>
                        &nbsp;
                        <%End If%>
                    </div>--%>
                    <%-- ************* --%>
                    <asp:HyperLink ID="hplLogoCertificacion" runat="server" ImageUrl="../../Images/logoCertificado.JPG"></asp:HyperLink>
                </td>
                <td style="height: 209px" valign="top" align="center" width="50%">
                    <table cellspacing="2" cellpadding="1" width="95%" align="center" border="0">
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:Label ID="lblLIsting" runat="server" EnableViewState="False">*</asp:Label><asp:HyperLink
                                    ID="hplLIsting" runat="server" EnableViewState="False" CssClass="bookingNormalLabel">Consultar mis reservaciones</asp:HyperLink>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td class="dgAlternate">
                                <asp:Label ID="lblVerifyRes" runat="server">*</asp:Label><asp:HyperLink ID="hplVerifyRes"
                                    runat="server" EnableViewState="False" CssClass="bookingNormalLabel">Revisar y relacionar reservaciones con mi sistema</asp:HyperLink>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:Label ID="lblResFrontDesk" runat="server" EnableViewState="False">*</asp:Label><asp:HyperLink
                                    ID="hplResFrontDesk" runat="server" EnableViewState="False" CssClass="bookingNormalLabel">Realizar mi conciliación de reservaciones</asp:HyperLink><sup
                                        class="dgpager" id="lblNew2" runat="server">Nuevo</sup>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:Label ID="lblStatus" runat="server" EnableViewState="False">*</asp:Label><asp:HyperLink
                                    ID="hplStatus" runat="server" EnableViewState="False" CssClass="bookingNormalLabel">Cerrar fechas y ver la situación de ocupación. </asp:HyperLink>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td class="dgAlternate">
                                <asp:Label ID="lblInventory" runat="server">*</asp:Label><asp:HyperLink ID="hplInventory"
                                    runat="server" EnableViewState="False" CssClass="bookingNormalLabel">Actualizar la disponibilidad de habitaciones. </asp:HyperLink>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:Label ID="lblFares" runat="server" EnableViewState="False">*</asp:Label><asp:HyperLink
                                    ID="hplFares" runat="server" EnableViewState="False" CssClass="bookingNormalLabel">Actualizar mis tarifas. </asp:HyperLink>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td class="dgAlternate">
                                <asp:Label ID="lblDeposits" runat="server" EnableViewState="False">*</asp:Label><asp:HyperLink
                                    ID="hplDeposits" runat="server" EnableViewState="False" CssClass="bookingNormalLabel">Depositos. </asp:HyperLink><sup
                                        class="dgpager" id="lblNew3" runat="server">Nuevo</sup>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:Label ID="lblFacturacion" runat="server" EnableViewState="False">*</asp:Label><asp:HyperLink
                                    ID="hplFacturacion" runat="server" EnableViewState="False" CssClass="bookingNormalLabel">Realizar mi conciliación de reservaciones</asp:HyperLink>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:Label ID="lblCertificacion" runat="server" EnableViewState="False">*</asp:Label><asp:HyperLink
                                    ID="hplCertificacion" runat="server" EnableViewState="False" CssClass="bookingNormalLabel"
                                    Target="_blank">4 Acciones para Incrementar mis reservaciones</asp:HyperLink><sup
                                        class="dgpager" id="lblNew4" runat="server">Nuevo</sup>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                            </td>
                        </tr>
                    </table>
                    &nbsp;&nbsp;
                    <br />
                    <br />
                    <table id="tblEstadoCuenta" cellspacing="0" cellpadding="0" width="300" border="0"
                        runat="server">
                        <tr class="trTitle">
                            <td align="center" colspan="2">
                                <asp:Label ID="lblEstadoCuenta" runat="server" EnableViewState="False" Style="color: #3E7BAC;
                                    font-weight: bold;">Estado de Cuenta</asp:Label>
                            </td>
                        </tr>
                        <tr class="trContent">
                            <td align="right" width="40%">
                                <asp:Label ID="lblSinAplicar" runat="server" Style="display: none;">Pagos sin Aplicar:</asp:Label>
                            </td>
                            <td align="right">
                                <asp:Label ID="lblSinAplicar_" runat="server" Style="display: none;"></asp:Label>
                            </td>
                        </tr>
                        <tr class="trContent">
                            <td align="right" width="40%">
                                <asp:Label ID="lblSaldo" runat="server">Saldo</asp:Label>
                            </td>
                            <td align="right">
                                <asp:Label ID="lblSaldo_" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr class="trContent">
                            <td align="right">
                                <asp:Label ID="lblTipoSaldo" runat="server">Tipo Saldo</asp:Label>
                            </td>
                            <td align="right">
                                <asp:Label ID="lblTipoSaldo_" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="height: 209px; width: 20%;" valign="top" align="center">
                    <div class="clear" style="text-align: center; margin: 10px;">
                        <%
                            If (Me.IsSupervisor) Then

                                If (Not Me.cInfoActual Is Nothing) Then
                                    Dim showCancel As Boolean = False
                                    Dim showWithDebit As Boolean = False
                                    Dim lenguaje As Integer = PortalCulture.GetIDCulture()
                                    LoadStatusHotelUniBilling(Me.cInfoActual.Empresa, showCancel, showWithDebit)
                                    If lenguaje = 1 Then
                                        If showCancel Then
                        %>
                        <br />
                        <span class="alertBox">HOTEL CANCELADO</span>
                        <%
                            End If
                            If showWithDebit Then
                        %>
                        <br />
                        <span class="alertBox">HOTEL CON ADEUDO</span>
                        <%                            
                            End If
                        Else
                            If showCancel Then
                        %>
                        <br />
                        <span class="alertBox">HOTEL CANCELED</span>
                        <%
                            End If
                            If showWithDebit Then
                        %>
                        <br />
                        <span class="alertBox">HOTEL WITH DEBT</span>
                        <%                            
                            End If                            
                        End If

                    End If
                End If
                
                        %>
                    </div>
                    <table cellspacing="0" cellpadding="0" width="95%" border="0">
                        <tr class="trTitle">
                            <td>
                                <asp:Label ID="lblRecorda" runat="server" EnableViewState="False" CssClass="dgHeader">Recordatorios</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <ul>
                                    <li id="liConciliar" runat="server">
                                        <asp:Label ID="lblConciliar" runat="server" EnableViewState="False" CssClass="bookingNormalLabel"></asp:Label>
                                    </li>
                                </ul>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <ul>
                                    <li id="liTarifas" runat="server">
                                        <asp:Label ID="lblTarifas" runat="server" EnableViewState="False" CssClass="bookingNormalLabel"></asp:Label></li>
                                </ul>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <ul>
                                    <li id="liVeririfar" runat="server">
                                        <asp:Label ID="lblVerificar" runat="server" EnableViewState="False" CssClass="bookingNormalLabel"></asp:Label></li>
                                </ul>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <ul>
                                    <li id="liInprocess" runat="server">
                                        <asp:Label ID="lblInprocess" runat="server" EnableViewState="False" CssClass="bookingNormalLabel"></asp:Label></li>
                                </ul>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <ul>
                                    <li id="LiFactura" runat="server">
                                        <asp:Label ID="lblFactura" runat="server" EnableViewState="False" CssClass="bookingNormalLabel"></asp:Label></li>
                                </ul>
                            </td>
                        </tr>
                        <tr>
                            <td style="display: none;">
                                <ul>
                                    <li id="liContrato" runat="server">
                                        <asp:Label ID="lblContrato" runat="server" EnableViewState="False" CssClass="DarkRedLabel"></asp:Label></li>
                                </ul>
                            </td>
                        </tr>
                    </table>
                    <p>
                        <table id="Table1" cellspacing="1" cellpadding="1" width="95%" border="0">
                            <tr class="trTitle">
                                <td class="tdContent">
                                    <asp:Label ID="lblNoticias" runat="server" EnableViewState="False" CssClass="dgHeader">Noticias</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:HyperLink style="display:none" ID="hplNew3" runat="server" Target="_blank" NavigateUrl="../../Documentacion/ConciliacionYPagos.pdf">Ayuda para Conciliación y Pagos</asp:HyperLink>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:HyperLink ID="hplNew1" runat="server" Target="_blank" NavigateUrl="../../Documentacion/PDF/Guia de promociones y paquetes.pdf">Promociones y paquetes</asp:HyperLink>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:HyperLink ID="hplNew2" runat="server" Target="_blank" NavigateUrl="../../Documentacion/reservasPorDeposito.pdf">Reservaciones por depósito</asp:HyperLink>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:HyperLink ID="hplFrontDeskRes" runat="server" Target="_blank" NavigateUrl="../../Documentacion/PDF/FrontDeskreservations_EN.pdf">Reservaciones por recepción</asp:HyperLink>
                                </td>
                            </tr>
                            <tr>
                                <td class="dgAlternate" align="right">
                                    <asp:HyperLink ID="hplOthersNews" runat="server" NavigateUrl="../../Documentacion/Noticias.aspx">Otras...</asp:HyperLink>
                                </td>
                            </tr>
                        </table>
                    </p>
                </td>
            </tr>           
            <tr>
                <td colspan="3">
                <asp:Panel ID="pnlReservas" runat =server >
                
                    <input type="hidden" id="activeTab" value="" runat="server" />
                    <div style="float: left; display: inline; width: 100%; margin-top: 15px;">
                        <div class="clear" id="tabs">
                            <table><tr><td> <div class="TabSelected" id="reservaciones">
                                <asp:Label ID="lblLastResevations" CssClass="clsDarkLabel" runat="server" EnableViewState="False">Últimas Reservaciones No Verificadas</asp:Label>
                            </div></td><td><div class="tab" id="depositos">
                                <asp:Label ID="lblLastResevationsDep" CssClass="clsDarkLabel" runat="server" EnableViewState="False">Ultimas Reservaciones No Depositadas</asp:Label>
                            </div></td>
                                <td><div class="tab" id="cryptoDeposits">
                                    <asp:Label ID="lblCryptoDeposits" CssClass="clsDarkLabel" runat="server" EnabledViewState="False">Reservaciones pendientes de confirmar dep&oacute;sito BTC</asp:Label>
                                    </div></td>

                                   </tr></table>
                           
                            
                        </div>
                    </div>
                        <div class="clear" id="dvReservations">
                            <asp:DataGrid ID="dgReservations" runat="server" EnableViewState="true" CssClass="DataGrid"
                               GridLines="None"  Width="100%" ShowFooter="false" AutoGenerateColumns="False" CellPadding="0" AllowPaging="True"
                                PageSize="20">
                                <SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
                                <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
                                <ItemStyle CssClass="dgItem"></ItemStyle>
                                <HeaderStyle CssClass="dgHeader"></HeaderStyle>
                                <Columns>
                                    <asp:BoundColumn DataField="NoReservacion" HeaderText="NoReservacion"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="Nombre" HeaderText="Hotel"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="Cliente" HeaderText="Cliente"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="checkIn" HeaderText="Llegada"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="cantidad" HeaderText="Habitaciones">
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="ID"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="pmsACT" HeaderText="Movement"></asp:BoundColumn>
                                    <asp:TemplateColumn>
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkItinerario" runat="server" CssClass="dgLink" CommandName="DetalleReserva"
                                                CausesValidation="false">
											<%# DataBinder.Eval(Container, "DataItem.NoReservacion") %>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn>
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkVerificar" runat="server" CssClass="dgLink" CommandName="Select"
                                                CausesValidation="false">
											<% Response.Write(PortalCulture.GetString("M000654"))%>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn Visible="False" DataField="noReservacion"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="idHotel"></asp:BoundColumn>
                                </Columns>
                                <PagerStyle CssClass="dgPager" HorizontalAlign="Right" Mode="NumericPages" Position="Bottom"
                                    PrevPageText="<< Anterior" NextPageText="Siguiente >>"></PagerStyle>
                            </asp:DataGrid>
                        </div>
                        <div class="" id="dvDeposits" style="display: none">
                            <asp:DataGrid ID="dgDepositos" GridLines="None" runat="server" EnableViewState="true" CssClass="DataGrid"
                                Width="100%" ShowFooter="false" AutoGenerateColumns="False" CellPadding="0" AllowPaging="true"
                                PageSize="20">
                                <SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
                                <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
                                <ItemStyle CssClass="dgItem"></ItemStyle>
                                <HeaderStyle CssClass="dgHeader"></HeaderStyle>
                                <Columns>
                                    <asp:BoundColumn DataField="NoReservacion" HeaderText="NoReservacion"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="Nombre" HeaderText="Hotel"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="Cliente" HeaderText="Cliente"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="checkIn" HeaderText="Fecha"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="cantidad" HeaderText="Hab.">
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="ID"></asp:BoundColumn>
                                    <asp:TemplateColumn>
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkItinerario" runat="server" CssClass="dgLink" CommandName="DetalleReserva"
                                                CausesValidation="false">
											<%# DataBinder.Eval(Container, "DataItem.NoReservacion") %>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn HeaderText="pmsACT" Visible="false"></asp:BoundColumn>
                                    <asp:TemplateColumn>
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkVerificar" runat="server" CssClass="dgLink" CommandName="Select"
                                                CausesValidation="false">
											<% Response.Write(PortalCulture.GetString("01342"))%>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn Visible="False" DataField="noReservacion"></asp:BoundColumn>
                                    <%--<asp:BoundColumn Visible="False" DataField="idHotel"></asp:BoundColumn>--%>
                                    <asp:BoundColumn Visible="False" DataField="noReservacion"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="IsNetRateUv"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="deposittarget"></asp:BoundColumn>
                                </Columns>
                                <PagerStyle CssClass="dgPager" HorizontalAlign="Right" Mode="NumericPages" Position="Bottom"
                                    PrevPageText="<< Anterior" NextPageText="Siguiente >>"></PagerStyle>
                            </asp:DataGrid>
                        </div>       
                    
                    <div id="dvCryptoDeposits" style="display:none">
                        <asp:DataGrid ID="dgCryptoDeposits" GridLines="None" runat="server" 
                            EnableViewState="true" CssClass="DataGrid" Width="100%" ShowFooter="false" 
                            AutoGenerateColumns="false" CellPadding="0" AllowPaging="true" PageSize="20">
                            <SelectedItemStyle CssClass="dgSelected" />
                            <AlternatingItemStyle CssClass="dgAlternate" />
                            <ItemStyle CssClass="dgItem" />
                            <HeaderStyle CssClass="dgHeader" />

                            <Columns>
                                    <asp:BoundColumn DataField="NoReservacion" HeaderText="NoReservacion"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="Nombre" HeaderText="Hotel"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="Cliente" HeaderText="Cliente"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="checkIn" HeaderText="Fecha"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="cantidad" HeaderText="Hab.">
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="ID"></asp:BoundColumn>
                                    <asp:TemplateColumn>
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkItinerario" runat="server" CssClass="dgLink" CommandName="DetalleReserva"
                                                CausesValidation="false">
											<%# DataBinder.Eval(Container, "DataItem.NoReservacion") %>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn HeaderText="pmsACT" Visible="false"></asp:BoundColumn>
                                    <asp:TemplateColumn>
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkVerificar" runat="server" CssClass="dgLink" CommandName="Select"
                                                CausesValidation="false">
											<% Response.Write(PortalCulture.GetString("01342"))%>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn Visible="False" DataField="noReservacion"></asp:BoundColumn>
                                    <%--<asp:BoundColumn Visible="False" DataField="idHotel"></asp:BoundColumn>--%>
                                    <asp:BoundColumn Visible="False" DataField="noReservacion"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="IsNetRateUv"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="deposittarget"></asp:BoundColumn>
                                </Columns>
                            <PagerStyle CssClass="dgPager" HorizontalAlign="Right" Mode="NumericPages" Position="Bottom"
                                    PrevPageText="<< Anterior" NextPageText="Siguiente >>"></PagerStyle>
                        </asp:DataGrid>
                    </div>
                        </asp:Panel>
                </td>                
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
