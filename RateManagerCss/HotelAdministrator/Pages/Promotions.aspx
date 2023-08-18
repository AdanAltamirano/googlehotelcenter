<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Promotions.aspx.vb" Inherits="RateManager.Promotions" %>

<%@ Import Namespace="RateManager" %>

<%@ Register Src="~/Modulos/CtrlIdioma.ascx" TagPrefix="uc2" TagName="CtrlIdioma" %>
<%@ Register TagPrefix="uc1" TagName="ctlMensajes" Src="../../Modulos/ctlMensajes.ascx" %>
<%@ Register Src="../../Modulos/ctrlAutoComplete.ascx" TagName="ctrlAutoComplete" TagPrefix="uc2" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="../../StyleSheets/Styles.css" rel="stylesheet" />
    <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <link rel="stylesheet" href="/resources/demos/style.css">
    <script type="text/javascript" src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script type="text/javascript" src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.5/jquery.min.js"></script>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js"></script>
    <script type="text/jscript" src="../../Includes/Script/JsSearch-1.0.js"></script>

    <style type="text/css">
        div {
            overflow: hidden;
        }

        .promoName {
            width: 510px;
            height: 85px;
            margin: 2px;
        }

        .promoDescription {
            width: 510px;
            height: 200px;
            margin: 2px;
        }

        .NoCancelable {
            padding-top: 15px;
            padding-left: 200px;
        }

        .SpecificHour {
            padding-top: 15px;
            padding-left: 15px;
        }

            .SpecificHour div.left {
                margin-left: 15px;
                padding-top: 5px;
                display: inline;
            }

            .SpecificHour div.right {
                display: inline;
            }

        .cancelPolicyDescription {
            padding-top: 15px;
        }

            .cancelPolicyDescription div.left {
                float: left;
                width: 200px;
            }

            .cancelPolicyDescription div.right {
                padding: 2px;
            }

        .travelWindowDates {
            padding-bottom: 2px;
        }

        .cancelPolicyPreview div.left {
            float: left;
            padding-top: 45px;
            width: 200px;
        }

        .cancelPolicyPreview div.right {
            margin: 2px;
            padding-bottom: 10px;
            padding-left: 20px;
        }

        .cancelPolicyDetail div.left {
            float: left;
            padding-top: 100px;
            width: 200px;
        }

        .cancelPolicyDetail div.right {
            margin: 2px;
            padding-bottom: 10px;
            padding-left: 20px;
        }

        .blackoutDays .blackoutDetails ul img {
            float: right;
            cursor: pointer;
        }

        .blackoutDays .blackoutDetails ul li {
            display: block;
            margin: 5px;
            border-bottom: 1px solid #CCC;
        }

        .rangeSelected span {
            font-size: 16px;
        }

        .cancelPolicy div {
            display: inline;
            margin-left: 5px;
            margin-top: 15px;
        }

        .cancelPolicy {
            margin-top: 15px;
            padding: 2px;
        }

        .promoNights {
            margin-top: 15px;
        }

        .divPromoNights {
            margin-left: 30px;
            margin-bottom: 20px;
            margin-top: 20px;
        }

        .dateShow {
            margin-top: 15px;
        }

        .advBookingSection span {
            padding-left: 15px;
        }

        .advBookingSection #txtMaxAdvBooking {
        }

        .title2 {
            margin-bottom: 10px;
        }

            .title2 span {
                font-size: 16px;
                font-weight: bold;
            }

        .upperLabel {
            display: block;
        }

        .weekDays label {
            display: block;
        }

        .toggle {
            display: none;
        }

        .weekdays td {
            padding-left: 6px;
        }

        .weekDays {
            margin-left: 15px;
            display: none;
            padding: 10px;
            border: 1px solid #CCC;
        }

        .blackoutDays {
            margin-left: 20px;
            margin-top: 15px;
            width: 90%;
        }

        .blackoutDetails {
            border: 1px solid #CCC;
            padding: 15px;
            margin-left: 15px;
            margin-top: 15px;
        }

        .roomList {
            border: 1px solid #CCC;
            padding: 10px;
            height: 160px;
            overflow-y: scroll;
            margin: 0 2px 0 2px;
        }

            .roomList td {
                border-bottom: 1px solid #CCC;
            }

        .contractList {
            border: 1px solid #CCC;
            padding: 10px;
            height: 160px;
            overflow-y: scroll;
            margin: 0 2px 0 2px;
        }

            .contractList td {
                border-bottom: 1px solid #CCC;
            }

        .dateRight {
            display: inline;
            margin-left: 15px;
        }

        .dateLeft {
            display: inline;
            margin-left: 15px;
        }

        .sectionTitle {
            display: block;
        }

        #travelWindowFrom, #travelWindowTo, #bookingWindowTo, #bookingWindowFrom, #blackoutTo, #blackoutFrom {
            width: 90px;
        }

        .promoNights div {
            display: inline;
            padding: 2px;
        }

        .promoNights input {
            width: 50px;
        }

        .sectionTitle {
            font-size: 20px;
            font-weight: bold;
        }

        .boxContractSelection {
            float: left;
            margin-right: 35px;
            height: 200px;
            width: 300px;
        }

        .boxRoomSelection {
            margin-right: 35px;
            height: 200px;
            width: 300px;
        }

        .hr {
            border-bottom: 1px solid #CCC;
            margin-bottom: 15px;
        }

        .resumeCode {
            padding: 20px;
            border: 1px solid #CCC;
            /*width: 650px;*/
            width:750px;
            height: 270px;
            margin-bottom: 15px;
        }

        .resumeBlock {
            margin-bottom: 15px;
        }

        .sectionWindow {
            padding: 20px;
            border: 1px solid #CCC;
            /*width: 650px;*/
            width:750px;
            margin-bottom: 15px;
            margin-left: 2px;
        }

        .exclusiveDate, .exclusiveArrivals {
            width: 40%;
            margin-top: 15px;
            margin-bottom: 15px;
        }

        .exclusiveDate {
            float: left;
            margin-left: 20px;
            margin-right: 60px;
        }

        .incorrect {
            box-shadow: 0 0 4px #f00;
        }

        .promoNights {
            padding-left: 200px;
        }

        .CodeNameConteiner div {
            padding: 3px;
        }
    </style>
    <style type="text/css">
        /* Style the tab */
        div.typePromoTab {
            overflow: hidden;
            border: 1px solid #ccc;
            background-color: #f1f1f1;
        }

            /* Style the buttons inside the tab */
            div.typePromoTab div {
                background-color: inherit;
                float: left;
                border: none;
                outline: none;
                cursor: pointer;
                padding: 14px 16px;
                transition: 0.3s;
                font-size: 17px;
            }

                /* Change background color of buttons on hover */
                div.typePromoTab div:hover {
                    background-color: #ddd;
                }

                /* Create an active/current tablink class */
                div.typePromoTab div.active {
                    background-color: #ccc;
                }

        /* Style the tab content */

        .tabcontent {
            display: none;
            padding: 6px 12px;
            border: 1px solid #ccc;
            border-top: none;
        }

        .tabDiscount div {
            display: inline-block;
        }

        .tabDiscount span {
            margin-top: 10px;
            margin-left: 20px;
        }

        .tabcontent {
            -webkit-animation: fadeEffect 1s;
            animation: fadeEffect 1s; /* Fading effect takes 1 second */
        }

        .tooltip-header {
            padding: 2px 16px;
            background-color: #3E7BAC;
            color: white;
            height: 10%;
        }

        .tooltip-body {
            padding: 2px 16px;
            height: 80%;
        }

        .tooltip-footer {
            padding: 2px 16px;
            background-color: #3E7BAC;
            color: white;
            height: 10%;
        }

        .tooltip-content {
            position: relative;
            background-color: #fefefe;
            margin: auto;
            padding: 0;
            border: 1px solid #888;
            height: 100%;
            box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2),0 6px 20px 0 rgba(0,0,0,0.19);
            -webkit-animation-name: animatetop;
            -webkit-animation-duration: 0.4s;
            animation-name: animatetop;
            animation-duration: 0.4s;
        }


        .toolTip-Discount {
            display: none; /* Hidden by default */
            position: fixed; /* Stay in place */
            z-index: 1; /* Sit on top */
            padding: 30px; /* Location of the box */
            left: 50%;
            right: 40%;
            width: 400px; /* Full width */
            height: 400px; /* Full height */
            background-color: rgb(0,0,0); /* Fallback color */
            background-color: rgba(0,0,0,0.4); /* Black w/ opacity */
        }

        @-webkit-keyframes fadeEffect {
            from {
                opacity: 0;
            }

            to {
                opacity: 1;
            }
        }

        @keyframes fadeEffect {
            from {
                opacity: 0;
            }

            to {
                opacity: 1;
            }
        }
    </style>
    <style>
        /* The Modal (background) */
        .modal {
            display: none; /* Hidden by default */
            position: fixed; /* Stay in place */
            z-index: 1; /* Sit on top */
            padding-top: 100px; /* Location of the box */
            left: 0;
            top: 0;
            width: 100%; /* Full width */
            height: 100%; /* Full height */
            overflow: auto; /* Enable scroll if needed */
            background-color: rgb(0,0,0); /* Fallback color */
            background-color: rgba(0,0,0,0.4); /* Black w/ opacity */
        }

        /* Modal Content */
        .modal-content {
            position: relative;
            background-color: #fefefe;
            margin: auto;
            padding: 0;
            border: 1px solid #888;
            width: 50%;
            box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2),0 6px 20px 0 rgba(0,0,0,0.19);
            -webkit-animation-name: animatetop;
            -webkit-animation-duration: 0.4s;
            animation-name: animatetop;
            animation-duration: 0.4s;
        }

        /* Add Animation */
        @-webkit-keyframes animatetop {
            from {
                top: -300px;
                opacity: 0;
            }

            to {
                top: 0;
                opacity: 1;
            }
        }

        @keyframes animatetop {
            from {
                top: -300px;
                opacity: 0;
            }

            to {
                top: 0;
                opacity: 1;
            }
        }

        /* The Close Button */
        .close {
            color: white;
            float: right;
            font-size: 28px;
            font-weight: bold;
        }

            .close:hover,
            .close:focus {
                color: #000;
                text-decoration: none;
                cursor: pointer;
            }

        .modal-header {
            padding: 2px 16px;
            background-color: #3E7BAC;
            color: white;
        }

        .modal-body {
            padding: 2px 16px;
        }

        .modal-footer {
            padding: 2px 16px;
            background-color: #3E7BAC;
            color: white;
        }
    </style>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <input type="hidden" runat="server" id="txtObjDelete" />
        <div class="clear">
            <input type="button" id="cmdNew" class="ButtonNew" runat="server" causesvalidation="false"
                value="New" style="width: 85px;" />
            <div class="mDiv">
            </div>
            <div class="title">
                <asp:Label ID="lblTitle" runat="server" EnableViewState="False" class="tituloSeccion">PROMOCIONES</asp:Label>
            </div>
        </div>
        <table id="bookingcontainer" cellspacing="0" width="650px" align="center" border="0">
            <tr>
                <td>
                    <div id="divContenedor" style="display: none;" runat="server">
                        <div class="CodeNameConteiner">
                            <div>
                                <div>
                                    <asp:Label Style="margin-right: 20px;" runat="server" CssClass="" ID="lblPromotionCode">Código de la promoción: </asp:Label>
                                    <asp:TextBox runat="server" ID="txtPromotionCode" Width="100px" MaxLength="4"></asp:TextBox>
                                </div>
                                <div style="display:inline;">
                                    <asp:Label Style="margin-right: 20px;" runat="server" CssClass="" ID="lblIsCombinable" Visible="False">Promoción combinable: </asp:Label>
                                    <asp:CheckBox ID="chkIscombinable" runat="server"  Visible="False"/>
                                </div>
                            </div>
                            <div>
                                <div style="float: left; margin-right: 45px; /*height: 30px;*/ width: 200px; padding-top: 48px">
                                    <asp:Label runat="server" CssClass="lblName" ID="lblPromotionName">Nombre de la promoción: </asp:Label>
                                </div>
                                <div class="promoName">
                                    <uc2:CtrlIdioma IsMultiline="false" Width="500" MaxLength="200" RequiredText="true" ID="txtPromoName" runat="server"></uc2:CtrlIdioma>
                                </div>
                            </div>
                            <div>
                                <div style="float: left; margin-right: 45px; /*height: 30px;*/ width: 200px; padding-top: 48px">
                                    <asp:Label runat="server" CssClass="lblName" ID="lblPromoDescription">Descripcion de la promoción: </asp:Label>
                                </div>
                                <div class="promoDescription">
                                    <uc2:CtrlIdioma IsMultiline="true" Width="500" Height="50" MaxLength="500" RequiredText="true" ID="txtPromoDescription" runat="server"></uc2:CtrlIdioma>
                                </div>
                            </div>
                        </div>
                        <div class="resumeCode">
                            <div class="promoResume">
                                <div class="resumeBlock">
                                    <span class="sectionTitle">Planes Tarifarios y Habitaciones</span>
                                </div>
                                <div class="hr"></div>
                            </div>
                            <div class="boxContractSelection">
                                <div>
                                    <input type="checkbox" class="selectAll" id="AllContract" onchange="selectAllRatePlans();" />
                                    <label for="AllContract" style="font-size: 12px;">Seleccionar todos los planes tarifarios</label>
                                </div>
                                <div class="contractList">
                                    <asp:CheckBoxList runat="server" ID="chlListContract" CssClass="" RepeatDirection="Vertical"></asp:CheckBoxList>
                                </div>
                            </div>
                            <div class="boxRoomSelection">
                                <div>
                                    <input type="checkbox" class="selectAll" id="AllRooms" onchange="selectAllRooms();" />
                                    <label for="AllRooms" style="font-size: 12px;">Seleccionar todas las habitaciones</label>
                                </div>
                                <div class="roomList">
                                    <asp:CheckBoxList runat="server" ID="chkListRoom" CssClass="" RepeatDirection="Vertical"></asp:CheckBoxList>
                                </div>
                            </div>
                        </div>
                        <div class="loadedContent">
                            <div class="sectionWindow travelWindow">
                                <div class="travelWindowDates">
                                    <span class="sectionTitle">Travel Window</span>
                                    <div class="dateLeft">
                                        <label for="from">Fecha inicial del viaje</label>
                                        <asp:TextBox runat="server" ID="travelWindowFrom"></asp:TextBox>
                                        <!--<input type="text" id="travelWindowFrom" name="travelWindowFrom" />-->
                                    </div>
                                    <div class="dateRight">
                                        <label for="to">Fecha final del viaje</label>
                                        <asp:TextBox runat="server" ID="travelWindowTo"></asp:TextBox>
                                        <!--<input type="text" id="travelWindowTo" name="travelWindowTo" />-->
                                    </div>
                                </div>
                                <div class="exclusiveDate">
                                    <asp:CheckBox ID="open_SpecificDate" runat="server" CssClass="toggleSection" AutoPostBack="false" Text="Promo válida para días específicos." />
                                    <!--<input id="open_SpecificDate" name="open_SpecificDate" type="checkbox" value="false" onchange="toggle('exclusiveDate');" />
                                <label for="open_SpecificDate">Promo válida para días específicos.</label>-->
                                    <div class="weekDays toggle">
                                        <asp:CheckBoxList runat="server" ID="ckhlSpecificDay" RepeatDirection="Horizontal" TextAlign="Left">
                                            <%--<asp:ListItem Text="Dom" Value="1" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Lun" Value="2" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Mar" Value="3" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Mie" Value="4" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Jue" Value="5" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Vie" Value="6" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Sab" Value="7" Selected="True"></asp:ListItem>--%>

                                            <asp:ListItem Text="Lun" Value="1" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Mar" Value="2" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Mie" Value="3" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Jue" Value="4" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Vie" Value="5" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Sab" Value="6" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Dom" Value="7" Selected="True"></asp:ListItem>

                                        </asp:CheckBoxList>
                                    </div>
                                </div>
                                <div class="exclusiveArrivals">
                                    <asp:CheckBox ID="open_SpecificArrivals" AutoPostBack="false" CssClass="toggleSection" runat="server" Text="No llegadas." />
                                    <!--<input id="open_SpecificArrivals" name="open_SpecificArrivals" type="checkbox" value="false" onchange="toggle('exclusiveArrivals');" />
                                <label for="open_SpecificArrivals">Promo aplica días de llegada específicos.</label>-->
                                    <div class="weekDays toggle">
                                        <asp:CheckBoxList runat="server" ID="chklSpecificArrivals" RepeatDirection="Horizontal" TextAlign="Left">
                                           <%-- <asp:ListItem Text="Dom" Value="1" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Lun" Value="2" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Mar" Value="3" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Mie" Value="4" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Jue" Value="5" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Vie" Value="6" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Sab" Value="7" Selected="True"></asp:ListItem>--%>
                                            
                                            <asp:ListItem Text="Lun" Value="1" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Mar" Value="2" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Mie" Value="3" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Jue" Value="4" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Vie" Value="5" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Sab" Value="6" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Dom" Value="7" Selected="True"></asp:ListItem>

                                        </asp:CheckBoxList>
                                    </div>
                                </div>
                                <div class="blackoutDays">
                                    <div style="display: block" class="">
                                        <asp:CheckBox ID="open_blackout" runat="server" CssClass="toggleSection" AutoPostBack="false" Text="Excluir promoción en los días." />
                                        <!--<input id="open_blackout" name="open_blackout" class="toggleSection" type="checkbox" value="false" onchange="toggle('blackoutDays');" />
                                    <label for="open_blackout">Excluir promoción en los días.</label>-->
                                    </div>
                                    <div class="blackoutDetails toggle">
                                        <div class="dateLeft">
                                            <label for="blackoutFrom">Entre: </label>
                                            <asp:TextBox runat="server" ID="blackoutFrom"></asp:TextBox>
                                            <!--<input type="text" id="blackoutFrom" name="blckoutFrom" />-->
                                        </div>
                                        <div class="dateRight">
                                            <label for="blackoutTo">Y :</label>
                                            <asp:TextBox runat="server" ID="blackoutTo"></asp:TextBox>
                                            <!--<input type="text" id="blackoutTo" name="blckoutTo" />-->
                                        </div>
                                        <input type="button" class="buttonNew" id="btnAddDateBlackout" value="Agregar Cierre" onclick="addDateBlackout(document.getElementById('blackoutFrom').value, document.getElementById('blackoutTo').value);" />
                                        <div class="dateShow">
                                            <span>Rangos de fechas seleccionado:</span>
                                            <div class="hr"></div>
                                            <div class="rangeSelected">
                                                <ul id="Dates_Blackout">
                                                </ul>
                                            </div>
                                        </div>
                                        <asp:HiddenField runat="server" ID="txtDiasBlackout" />
                                    </div>
                                </div>
                            </div>
                            <div class="sectionWindow bookingWindow">
                                <span class="sectionTitle">Booking Window</span>
                                <div class="blackoutDetails">
                                    <div class="title2">
                                        <span>Periodo de venta</span>
                                    </div>
                                    <div class="dateLeft">
                                        <label for="bookingWindowFrom">Fecha inicial: </label>
                                        <asp:TextBox runat="server" ID="bookingWindowFrom"></asp:TextBox>
                                        <!--<input type="text" id="bookingWindowFrom" name="bookingWindowFrom" />-->
                                    </div>
                                    <div class="dateRight">
                                        <label for="bookingWindowTo">Fecha final: </label>
                                        <asp:TextBox runat="server" ID="bookingWindowTo"></asp:TextBox>
                                        <!--<input type="text" id="bookingWindowTo" name="bookingWindowTo" />-->
                                    </div>
                                    <div class="SpecificHour">
                                        <asp:CheckBox ID="CheckBoxDefHora" runat="server" Text="Especificar Hora" />
                                        <div class="left">
                                            <asp:Label Style="z-index: 0" ID="lblFromHora" runat="server" CssClass="clslabel" EnableViewState="False">Desde</asp:Label>
                                            <asp:DropDownList ID="HoraInicio" runat="server" Enabled="False">
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
							<asp:DropDownList Style="z-index: 0" ID="MinutoInicio" runat="server" Enabled="False">
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
                                        </div>
                                        <div class="right">
                                            <asp:Label Style="z-index: 0" ID="lblToHora" runat="server" CssClass="clslabel" EnableViewState="False">Hasta</asp:Label><asp:DropDownList Style="z-index: 0" ID="HoraFin" runat="server" Enabled="False">
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
							<asp:DropDownList Style="z-index: 0" ID="MinutoFin" runat="server" Enabled="False">
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
                                        </div>
                                    </div>
                                </div>
                                <div class="blackoutDetails">
                                    <div class="title2">
                                        <span>Días Anticipados</span>
                                    </div>
                                    <div class="advBookingSection">
                                        <span>Días Min</span>
                                        <asp:TextBox runat="server" ID="txtMinAdvBooking" Width="40px" MaxLength="3"></asp:TextBox>
                                        <span>Días Max</span>
                                        <asp:TextBox runat="server" ID="txtMaxAdvBooking" Width="40px" MaxLength="3"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="sectionWindow promoType">
                                <span class="sectionTitle">Tipo de la promoción</span>
                                <div class="typePromoTab">
                                    <div class="tabLinks" onclick="openPromo(event, 'divPromoAdd')" id="defaultOpen">
                                        <asp:CheckBox ID="chkPromoAdd" Checked="false" runat="server" Text=" " />
                                        <label>Valor Agregado</label>
                                    </div>
                                    <div class="tabLinks" onclick="openPromo(event, 'divPromoNights')">
                                        <asp:CheckBox ID="chkPromoNights" Checked="false" runat="server" Text=" " />
                                        <label>Noches Gratis</label>
                                    </div>
                                    <div class="tabLinks" onclick="openPromo(event, 'divPromoDiscount')">
                                        <asp:CheckBox ID="chkPromoDiscount" Checked="false" runat="server" Text=" " />
                                        <label>Descuento</label>
                                    </div>
                                </div>
                                <div id="divPromoAdd" class="tabContent">
                                    <div class="title2">
                                        <span>Descripción: </span>
                                    </div>
                                    <div style="margin-left: 80px;">
                                        <uc2:CtrlIdioma IsMultiline="true" Width="450" MaxLength="200" Height="100" RequiredText="false" ID="txtAddValueDescription" runat="server"></uc2:CtrlIdioma>
                                    </div>
                                </div>
                                <div id="divPromoNights" class="tabContent">
                                    <div class="divPromoNights">
                                        <asp:DropDownList runat="server" ID="ddlFreeNight">
                                            <asp:ListItem Text="Cada" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Solo" Value="1"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:TextBox runat="server" ID="txtFreeNight" Width="50px" MaxLength="3"></asp:TextBox>
                                        <span style="font-size: 16px;">&#170;&nbsp;&nbsp;</span>
                                        <span>Noche será gratis</span>
                                    </div>
                                </div>
                                <div id="divPromoDiscount" class="tabContent tabDiscount">
                                    <div class="discountOptions">
                                        <asp:RadioButtonList runat="server" ID="rblDiscountOptions" RepeatDirection="Horizontal">
                                            <asp:ListItem Selected="True" Text="% (Porcentaje)" Value="1"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                    <div class="discountDetails">
                                        <asp:TextBox runat="server" ID="txtPromoDiscount" Width="50px" MaxLength="6"></asp:TextBox>
                                    </div>
                                    <div>
                                        <asp:Label runat="server" ID="lblApplicationMode">Modo de aplicación :</asp:Label>
                                    </div>
                                    <div>
                                        <asp:DropDownList runat="server" ID="ddlApplicationMode">
                                            <asp:ListItem Selected="True" Text="Prioridad a descunto en Tarifa" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Suma porcentajes de descuento" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Descuento adicional" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div>
                                        <!--<asp:LinkButton runat="server" ID="lknDiscountTooltip" Text="Ayuda" CssClass="dglink" href="#" OnClientClick="showToolTip(); return false;"></asp:LinkButton>-->
                                        <span class="dglink" onclick="showToolTip();">Ayuda</span>
                                    </div>
                                </div>
                                <div id="Div1" class="modal">
                                    <!-- Modal content -->
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <span class="close" onclick="closeModal()">&times;</span>
                                            <h2>Desactivar Promoción</h2>
                                        </div>
                                        <div class="modal-body">
                                            <h4 class="msg">¿Está seguro que desea desactivar esta promoción?</h4>
                                        </div>
                                        <div class="modal-footer">
                                            <input type="button" value="Sí" onclick="deactivatePromo();" />
                                            <input type="button" value="No" onclick="closeModal();" />
                                        </div>
                                    </div>
                                </div>
                                <div class="toolTip-Discount" id="TooltipDiscount">
                                    <div class="tooltip-content">
                                        <div class="tooltip-header">
                                            <h2>Modo de aplicacón del descuento</h2>
                                        </div>
                                        <div class="tooltip-body">
                                            <p>El <b>Modo de Aplicación</b> solo afectará cuando haya un descuento a nivel de tarifa. En caso contrario se aplicará el porcetaje de descuento que aquí se configure.</p>
                                            <p>Suponiendo que en la promoción se configuré con un 40% de descuento y en la tarifa se configure con un 20% de descuento, quedaría de la siguiente forma: </p>
                                            <h3>Prioridad a descuento en Tarifa</h3>
                                            <!--<p>Si la tarifa creada tiene un descuento, el descuento de esta promoción será reemplazado.</p>-->
                                            <p>Solo se aplicará el 20% de descuento.</p>

                                            <h3>Suma porcentajes de descuento</h3>
                                            <!--<p>Si la tarifa creada tiene un descuento, el descuento de esta promoción será reemplazado.</p>-->
                                            <p>Se aplicará un 60% de descuento.</p>

                                            <h3>Descuento adicional</h3>
                                            <!--<p>Si la tarifa creada tiene un descuento, el descuento de esta promoción será reemplazado.</p>-->
                                            <p>Primero se aplicará el 40% de descuento y al resultado de eso se le aplicará el 20% de descuento.</p>
                                        </div>
                                        <div class="tooltip-footer">
                                            <input type="button" value="OK" onclick="closeTooltip();" />
                                        </div>

                                    </div>
                                </div>
                            </div>
                            <div class="sectionWindow conditions">
                                <span class="sectionTitle">Restricciones</span>
                                <div class="cancelPolicyDescription">
                                    <div class="left">
                                        <span style="display: none;">Descripción: </span>
                                    </div>
                                    <div class="right">
                                        <asp:TextBox Visible="false" ID="txtCancelPolicyDescription" runat="server" CssClass="TextBox" MaxLength="100" Width="100px"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="promoNights">
                                    <div>
                                        <span>Noches Min: </span>
                                        <asp:TextBox runat="server" ID="txtMinNights"></asp:TextBox>
                                    </div>
                                    <div>
                                        <span>Noches Max: </span>
                                        <asp:TextBox runat="server" ID="txtMaxNights"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="NoCancelable">
                                    <asp:CheckBox runat="server" ID="chkNonCancelable" Text="No Cancelable" />
                                </div>
                                <div class="cancelPolicy">
                                    <div>
                                        <span>Politica de cancelación: </span>
                                        <asp:DropDownList ID="ddlCancelationPolicy" runat="server"></asp:DropDownList>
                                    </div>
                                    <div>
                                        <asp:Label ID="lblAux" runat="server"></asp:Label>
                                    </div>
                                    <div>
                                        <asp:TextBox ID="txtCancellationPolicy" runat="server" CssClass="textbox" MaxLength="3" Columns="3" Width="56px"></asp:TextBox>
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
                                        </asp:DropDownList><asp:Label ID="lblSep" runat="server">:</asp:Label><asp:DropDownList ID="ddlMinutes" runat="server">
                                            <asp:ListItem Value="00">00</asp:ListItem>
                                            <asp:ListItem Value="15">15</asp:ListItem>
                                            <asp:ListItem Value="30">30</asp:ListItem>
                                            <asp:ListItem Value="45">45</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div>
                                        <input type="hidden" id="varCancelationTime" runat="server" value="" />
                                        <asp:Label ID="lblEDaysHour" runat="server" CssClass="clslabel">Days / Hours</asp:Label>
                                        <asp:Label ID="lblErrorHours" runat="server" CssClass="validators" Visible="False"></asp:Label>
                                        <asp:RangeValidator ID="RVCancelation" runat="server" CssClass="validators" Display="Dynamic" ControlToValidate="txtCancellationPolicy"
                                            ErrorMessage="0-999" MaximumValue="9999" MinimumValue="0"></asp:RangeValidator>
                                    </div>
                                </div>
                                <div class="cancelPolicyPreview">
                                    <div class="left">
                                        <asp:Label ID="lblCancelPoliciesPreview" runat="server" CssClass="lbl">Política de cancelación previa: </asp:Label>
                                    </div>
                                    <div class="right">
                                        <uc2:CtrlIdioma IsMultiline="false" Width="400" MaxLength="200" RequiredText="false" ID="txtCancelPoliciesPreview" runat="server"></uc2:CtrlIdioma>
                                    </div>
                                </div>
                                <div class="cancelPolicyDetail">
                                    <div class="left">
                                        <asp:Label ID="lblCancelPoliciesFull" runat="server" CssClass="clslabel">Política de cancelación detallada: </asp:Label>
                                    </div>
                                    <div class="right">
                                        <uc2:CtrlIdioma IsMultiline="true" Width="400" MaxLength="200" Height="80" RequiredText="false" ID="txtCancelPoliciesFull" runat="server"></uc2:CtrlIdioma>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div>
                            <asp:Button runat="server" ID="btnSave" Text="Guardar" Style="display: none" />
                            <input type="button" value="Guardar" class="button" onclick="Save();" />
                            <asp:Button ID="btncancel" runat="server" EnableViewState="False" CssClass="Button" Text="Cancelar" CausesValidation="False"></asp:Button>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <asp:Label ID="lblError" runat="server" CssClass="validators" Visible="false"></asp:Label>
                    </div>
                    <uc2:ctrlAutoComplete ID="ctrlAutoComplete1" runat="server" />
                    <div style="float: left; margin-bottom: 15px;">
                        <asp:Label ID="lblFilter" runat="server" Text="Filtro:"></asp:Label>
                        <asp:DropDownList ID="ddlDeletedFilter" runat="server" AutoPostBack="True">
                            <asp:ListItem Text="Solo activos" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Solo no activos" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Activos y no activos" Value="-1"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:CheckBox id="oldPromosCheckbox" runat="server" Text="Incluir promociones anteriores" TextAlign="Right" AutoPostBack="True"/>
                    </div>
                    <asp:DataGrid ID="grid" runat="server" Width="99%" AllowPaging="True" PageSize="20"
                        GridLines="None" AutoGenerateColumns="False" CssClass="datagrid" ShowFooter="True">
                        <SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
                        <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
                        <ItemStyle CssClass="dgItem"></ItemStyle>
                        <HeaderStyle CssClass="dgHeader"></HeaderStyle>
                        <FooterStyle HorizontalAlign="Right"></FooterStyle>
                        <Columns>
                            <asp:BoundColumn Visible="False" DataField="idrateplan"></asp:BoundColumn>
                            <asp:BoundColumn Visible="false" DataField="orden" HeaderText="Orden">
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="codigotarifa" HeaderText="C&#243;digo">
                                <HeaderStyle Width="9%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Name" HeaderText="Nombre">
                                <HeaderStyle Width="40%"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="PromoStartDate" HeaderText="Inicio">
                                <HeaderStyle Width="20%"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="PromoEndDate" HeaderText="Fin">
                                <HeaderStyle Width="20%"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn Visible="false" DataField="Segment" HeaderText="Segmento">
                                <HeaderStyle Width="23%"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle Width="10%"></HeaderStyle>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkedit" runat="server" CssClass="dgLink" CausesValidation="False"
                                        CommandName="Select">editar</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle Width="10%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEliminar2" Style="display: none" runat="server" CssClass="dgLink"
                                        CausesValidation="False" CommandName="Delete">-</asp:LinkButton>
                                    <asp:HyperLink ID="lnkEliminar" runat="server" CssClass="dgLink"></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle Width="10%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkActivar2" Style="display: none" runat="server" CssClass="dgLink"
                                        CausesValidation="False" CommandName="Active">-</asp:LinkButton>
                                    <asp:HyperLink ID="lnkActivar" runat="server" CssClass="dgLink"></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn Visible="False" DataField="SegmentRacPrinc"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="codigotarifa"></asp:BoundColumn>
                            <asp:BoundColumn Visible="False" DataField="Deleted"></asp:BoundColumn>
                        </Columns>
                        <PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
                            Position="Bottom" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="ratesPlanGrid">
                    </div>
                </td>
            </tr>
        </table>

        <!--<uc1:ctlmensajes id="CtlMensajes1" runat="server"></uc1:ctlmensajes>
        <uc1:ctlmensajes id="CtlMensajes2" runat="server"></uc1:ctlmensajes>
        <uc1:ctlmensajes id="CtlMensajes3" runat="server"></uc1:ctlmensajes>-->
    </form>
    <!-- The Modal -->
    <div id="myModal" class="modal">

        <!-- Modal content -->
        <div class="modal-content">
            <div class="modal-header">
                <span class="close" onclick="closeModal()">&times;</span>
                <h2>Desactivar Promoción</h2>
            </div>
            <div class="modal-body">
                <h4 class="msg">¿Está seguro que desea desactivar esta promoción?</h4>
            </div>
            <div class="modal-footer">
                <input type="button" value="Sí" onclick="deactivatePromo();" />
                <input type="button" value="No" onclick="closeModal();" />
            </div>
        </div>

    </div>
    <script type="text/javascript">

        SearchStart.AddParam
            (
                {
                    searchitems: [
                        { Item: 'RatePlans', IDSearch: 'IdRatePlan', nameSearch: 'CodigoTarifa', isdefault: true },
                        { Item: 'RatePlans', IDSearch: 'IdRatePlan', nameSearch: 'name', isdefault: false }
                    ],
                    colModel: [
                        { display: '<%= RateManager.PortalCulture.GetString("00001") %>' },
                        { display: '<%= RateManager.PortalCulture.GetString("00073") %>' },
                    ],
                    Data: [{
                        catalogo: 'RatesPlans', idHotel: '<%= MyBase.cInfoActual.Hotel %>',
            idIdioma: '<%= RateManager.PortalCulture.GetIDCulture %>',
            IsSupervisor: '<%= MyBase.IsSupervisor %>'
                    }],
                    id: 'RatePlan',
                    index: 1
                }
            );

        var hideCancelPolicies = function () {
            if ($("#chkNonCancelable")[0].checked) {
                $(".cancelPolicy").attr("style", "display: none");
            } else {
                $(".cancelPolicy").attr("style", "display: block");
            }
        }

        var discountTooltip = document.getElementById('TooltipDiscount');

        function showToolTip() {
            var t = document.getElementById('TooltipDiscount');
            t.style.display = 'block';
        }

        function closeTooltip() {
            discountTooltip.style.display = 'none';
        }

        function FireShow(ID, IDcmd, show) {
            var e = document.getElementById(ID);
            var c = document.getElementById(IDcmd);
            if (e) {
                e.style.display = show ? 'block' : 'none';
            }
            if (c) {
                c.style.display = !show ? 'block' : 'none';
            }
            onResizeIframe();
        }

        function openPromo(evt, promoName) {
            var i, tabcontent, tablinks;
            tabcontent = document.getElementsByClassName("tabcontent");
            for (i = 0; i < tabcontent.length; i++) {
                tabcontent[i].style.display = "none";
            }
            tablinks = document.getElementsByClassName("tablinks");
            for (i = 0; i < tablinks.length; i++) {
                tablinks[i].className = tablinks[i].className.replace(" active", "");
            }
            document.getElementById(promoName).style.display = "block";
            evt.currentTarget.className += " active";
        }
    </script>
    <script type="text/javascript">
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

        var showTimeChangedNotify = true

        function CacellationTimeHasChanged() {

            var result = false;
            if ($('#<%= Me.varCancelationTime.ClientId %>').val().length > 0) {
                var original = eval('(' + $('#<%= Me.varCancelationTime.ClientId %>').val() + ')');
                var current = new Object();
                current.type = parseInt($('#<%= Me.ddlCancelationPolicy.ClientId %>').attr("selectedIndex"));
                if (current.type == 3) {
                    current.value = $('#<%= Me.ddlHour.ClientId %> option:selected').val() + ':' + $('#<%= Me.ddlMinutes.ClientId %> option:selected').val();
                } else {
                    current.value = $('#<%= Me.txtCancellationPolicy.ClientId %>').val();
                }
                result = !(original.type === current.type && original.value === current.value);
            }
            return result;
        }

        $(document).ready(function () {
            $('#Dates_Blackout').empty();
            hideCancelPolicies();

            if ($("#<%= Me.open_SpecificArrivals.ClientID%>")[0].checked) {
                toggle("exclusiveArrivals");
            }

            if ($("#<%= Me.open_blackout.ClientID%>")[0].checked) {
                toggle("blackoutDays");
                loadBlackoutDays();
            }

            if ($("#<%= Me.open_SpecificDate.ClientID%>")[0].checked) {
                toggle("exclusiveDate");
            }

            notAllRatePlans();
            notAllRooms();

            $('#<%= Me.ddlHour.ClientId %>, #<%= Me.ddlMinutes.ClientId %>, #<%= Me.txtCancellationPolicy.ClientId %>').change(function () {
                showTimeChangedNotify = true;
            });

            $('#<%= Me.ddlCancelationPolicy.ClientId %>').change(function () {
                showTimeChangedNotify = true;

                $('#<%= Me.ddlHour.ClientId %>, #<%= Me.lblSep.ClientId %>, #<%= Me.ddlMinutes.ClientId %>, #<%= Me.txtCancellationPolicy.ClientId %>, #<%= Me.lblAux.ClientId %>, #<%= Me.lblEDaysHour.ClientId %>').hide();
                if ($(this)[0].selectedIndex > 0) {
                    $('#<%= Me.lblAux.ClientId %>, #<%= Me.lblEDaysHour.ClientId %>').show();

                    if ($(this)[0].selectedIndex == 1 || $(this)[0].selectedIndex == 2) {
                        $('#<%= Me.txtCancellationPolicy.ClientId %>').show();
                        $('#<%= Me.lblAux.ClientId %>').html('<%= PortalCulture.GetString("00413") %>');
                        if ($(this)[0].selectedIndex == 1) {
                            $('#<%= Me.lblEDaysHour.ClientId %>').html('<%= PortalCulture.GetString("00410") %>');
                        } else {
                            $('#<%= Me.lblEDaysHour.ClientId %>').html('<%= PortalCulture.GetString("00409") %>');
                        }
                    } else if ($(this)[0].selectedIndex == 3) {
                        $('#<%= Me.ddlHour.ClientId %>, #<%= Me.ddlMinutes.ClientId %>, #<%= Me.lblSep.ClientId %>').show();
                        $('#<%= Me.lblAux.ClientId %>').html('<%= PortalCulture.GetString("00412") %>');
                        $('#<%= Me.lblEDaysHour.ClientId %>').html('<%= PortalCulture.GetString("00411") %>');
                    }
                }
            });
        });

    </script>
    <script type="text/javascript">
        $(function () {
            var dateFormat = 'dd/mm/yy',
                from = $("#travelWindowFrom")
                    .datepicker({
                        showOn: "button",
                        buttonImage: "/Calendar/calbtn.gif", ///RateManager/Calendar/calbtn.gif Aplicacion de Ratemanager
                        buttonImageOnly: true,
                        buttonText: "Select date",
                        defaultDate: "+1w",
                        changeMonth: true,
                        numberOfMonths: 2,
                        dateFormat: dateFormat
                    })
                    .on("change", function () {
                        to.datepicker("option", "minDate", getDate(this));
                    }),
                to = $("#travelWindowTo").datepicker({
                    showOn: "button",
                    buttonImage: "/Calendar/calbtn.gif",
                    buttonImageOnly: true,
                    buttonText: "Select date",
                    defaultDate: "+1w",
                    changeMonth: true,
                    numberOfMonths: 2,
                    dateFormat: dateFormat
                })
                    .on("change", function () {
                        from.datepicker("option", "maxDate", getDate(this));
                    });

            function getDate(element) {
                var date;
                try {
                    date = $.datepicker.parseDate(dateFormat, element.value);
                } catch (error) {
                    date = null;
                }

                return date;
            }
        }); //DATEPICKER

        $(function () {
            var dateFormat = "dd/mm/yy",
                from = $("#bookingWindowFrom")
                    .datepicker({
                        showOn: "button",
                        buttonImage: "/Calendar/calbtn.gif",
                        buttonImageOnly: true,
                        buttonText: "Select date",
                        defaultDate: "+1w",
                        changeMonth: true,
                        numberOfMonths: 2,
                        dateFormat: dateFormat
                    })
                    .on("change", function () {
                        to.datepicker("option", "minDate", getDate(this));
                    }),
                to = $("#bookingWindowTo").datepicker({
                    showOn: "button",
                    buttonImage: "/Calendar/calbtn.gif",
                    buttonImageOnly: true,
                    buttonText: "Select date",
                    defaultDate: "+1w",
                    changeMonth: true,
                    numberOfMonths: 2,
                    dateFormat: dateFormat
                })
                    .on("change", function () {
                        from.datepicker("option", "maxDate", getDate(this));
                    });

            function getDate(element) {
                var date;
                try {
                    date = $.datepicker.parseDate(dateFormat, element.value);
                } catch (error) {
                    date = null;
                }

                return date;
            }
        }); //DATEPICKER

        $(function () {
            var dateFormat = "dd/mm/yy",
                from = $("#blackoutFrom")
                    .datepicker({
                        showOn: "button",
                        buttonImage: "/Calendar/calbtn.gif",
                        buttonImageOnly: true,
                        buttonText: "Select date",
                        defaultDate: "+1w",
                        changeMonth: true,
                        numberOfMonths: 2,
                        dateFormat: dateFormat
                    })
                    .on("change", function () {
                        to.datepicker("option", "minDate", getDate(this));
                    }),
                to = $("#blackoutTo").datepicker({
                    showOn: "button",
                    buttonImage: "/Calendar/calbtn.gif",
                    buttonImageOnly: true,
                    buttonText: "Select date",
                    defaultDate: "+1w",
                    changeMonth: true,
                    numberOfMonths: 2,
                    dateFormat: dateFormat
                })
                    .on("change", function () {
                        from.datepicker("option", "maxDate", getDate(this));
                    });

            function getDate(element) {
                var date;
                try {
                    date = $.datepicker.parseDate(dateFormat, element.value);
                } catch (error) {
                    date = null;
                }

                return date;
            }
        }); //DATEPICKER

        function selectAllRatePlans() {
            //var ratePlans = $(".contractList :input")
            var chkRates = $("#AllContract")

            if (chkRates[0].checked) {
                $(".contractList :input").each(function (index) {
                    this.checked = true;
                });
            } else {
                $(".contractList :input").each(function (index) {
                    this.checked = false;
                });
            }
            //console.log(ratePlans);
        }

        function isRatePlanSelected() {
            var isSelected = false;
            $(".contractList :input").each(function (index) {
                if (this.checked) {
                    isSelected = true;
                }
            });
            return isSelected;
        }

        function isRoomSelected() {
            var isSelected = false;
            $(".roomList :input").each(function (index) {
                if (this.checked) {
                    isSelected = true;
                }
            });
            return isSelected;
        }

        function notAllRatePlans() {
            $("#AllContract")[0].checked = true
            $(".contractList :input").each(function (index) {
                if (!this.checked) {
                    $("#AllContract")[0].checked = false;
                }
            });
        }

        function selectAllRooms() {
            var chkRooms = $("#AllRooms")

            if (chkRooms[0].checked) {
                $(".roomList :input").each(function (index) {
                    this.checked = true;
                });
            } else {
                $(".roomList :input").each(function (index) {
                    this.checked = false;
                });
            }
        }

        function notAllRooms() {
            $("#AllRooms")[0].checked = true
            $(".roomList :input").each(function (index) {
                if (!this.checked) {
                    $("#AllRooms")[0].checked = false;
                }
            });
        }

        function toggle(id) {
            $("." + id + " .toggle").toggle("fast");
        }

        function addDateBlackout(from, to) {
            //var from = $('#blackoutFrom').val();
            //var to = $('#blackoutTo').val();
            if (isValidDates(from, to)) {
                var dateRange = "<span class='blackFrom' id='" + from + "'>" + from + "</span> - <span class='blackTo' id='" + to + "'>" + to + "</span>";
                var imgRemoveRange = "<img class='removeRange' onclick='removeDate(this)' src='../../Images/remove.jpg'/>";
                var div = "<div class='hr'></div>";
                if (dateRange.length) {
                    if (!isOverlappedDates()) {
                        $('<li />', {
                            "class": "willAdd",
                            html: dateRange + imgRemoveRange
                        }).appendTo('#Dates_Blackout')

                        $(".blackoutDays :input[type=text]").removeClass('incorrect');
                    } else {
                        $(".blackoutDays :input[type=text]").addClass('incorrect');
                    }
                }
            } else {
                $(".blackoutDays :input[type=text]").addClass('incorrect');
            }

        }

        var isValidDates = function (from, to) {
            var valid = false;
            var auxDate;
            var auxFrom;
            var auxTo;

            var newFrom;
            var newTo;

            if (from.length && to.length) {
                auxDate = from.split('/');
                auxFrom = new Date(auxDate[2], +auxDate[1] - 1, auxDate[0]);

                auxDate = to.split('/');
                auxTo = new Date(auxDate[2], auxDate[1] - 1, auxDate[0]);

                if (auxFrom.valueOf() <= auxTo.valueOf()) {

                    valid = true;
                }
            }

            return valid;
        }

        function removeDate(elem) {
            $(elem).parent().remove();
        }

        function Save() {
            if (isValidData()) {
                var selectedToDates = $('#Dates_Blackout li')
                selectedToDates.each(function (index) {
                    var from = $(this).children(".blackFrom")[0].id;
                    var to = $(this).children(".blackTo")[0].id;

                    $("#<%= txtDiasBlackout.ClientID%>").val(function () {
                        return $(this).val() + from + "-" + to + "|";
                    });
                });
                $('#Dates_Blackout').empty();
                $("#btnSave").click();
            }
        }

        var isValidData = function () {
            $(".incorrect").removeClass('incorrect');
            var isValid = true
            if ($("#travelWindowFrom").val() === "" || $("#travelWindowTo").val() === "" || !isValidDates($("#travelWindowFrom").val(), $("#travelWindowTo").val())) {
                $(".travelWindowDates :input[type=text]").addClass('incorrect');
                isValid = false;
            }

            if ($('#<%= txtPromoDescription.ReturnNameTxtEn()%>').val() === '' || $('#<%= txtPromoDescription.ReturnNameTxtEs()%>').val() === '') {
                $(".promoDescription").addClass("incorrect");
                //alert('Es necesario una descipción de Valor Agregado.');
                isValid = false;
            }

            if ($('#<%= txtPromoName.ReturnNameTxtEn()%>').val() === '' || $('#<%= txtPromoName.ReturnNameTxtEs()%>').val() === '') {
                $(".promoName").addClass("incorrect");
                //alert('Es necesario una descipción de Valor Agregado.');
                isValid = false;
            }

            if ($('#<%= txtCancelPoliciesFull.ReturnNameTxtEn()%>').val() === '' || $('#<%= txtCancelPoliciesFull.ReturnNameTxtEs()%>').val() === '') {
                $(".cancelPolicyDetail div.right").addClass("incorrect");
                //alert('Es necesario una descipción de Valor Agregado.');
                isValid = false;
            }

            if ($('#<%= txtCancelPoliciesPreview.ReturnNameTxtEn()%>').val() === '' || $('#<%= txtCancelPoliciesPreview.ReturnNameTxtEs()%>').val() === '') {
                $(".cancelPolicyPreview div.right").addClass("incorrect");
                //alert('Es necesario una descipción de Valor Agregado.');
                isValid = false;
            }

            if ($("#txtPromotionCode").val() == "") {
                $("#txtPromotionCode").addClass('incorrect');
                //alert('Debe definir un Código para la promoción.');
                isValid = false;
            }

            if (!isRatePlanSelected()) {
                $(".contractList").addClass('incorrect');
                //alert('Debe elegir al menos un Plan Tarifario.');
                isValid = false;
            }

            if (!isRoomSelected()) {
                $(".roomList").addClass('incorrect');
                //alert('Debe elegir al menos una Habitación.');
                isValid = false;
            }

            if (!$("#chkPromoAdd")[0].checked && !$("#chkPromoNights")[0].checked && !$("#chkPromoDiscount")[0].checked) {
                $(".promoType").addClass('incorrect');
                //alert("Debe elegir al menos un tipo de promoción.");
                isValid = false;
            } else {
                if ($("#chkPromoDiscount")[0].checked) {
                    if ($("#<%= txtPromoDiscount.ClientID%>").val() == "") {
                        $("#<%= txtPromoDiscount.ClientID%>").addClass("incorrect");
                        $("#divPromoDiscount").addClass("incorrect");
                        //alert('Es necesario definir el valor del Descuento.');
                        isValid = false;
                    }
                }

                if ($("#chkPromoAdd")[0].checked) {
                    if ($('#<%= txtAddValueDescription.ReturnNameTxtEn()%>').val() === '' || $('#<%= txtAddValueDescription.ReturnNameTxtEs()%>').val() === '') {
                        $("#divPromoAdd").addClass("incorrect");
                        //alert('Es necesario una descipción de Valor Agregado.');
                        isValid = false;
                    }
                } else {
                    $('#<%= txtAddValueDescription.ReturnNameTxtEn()%>').val("");
                    $('#<%= txtAddValueDescription.ReturnNameTxtEs()%>').val("");
                }

                if ($("#chkPromoNights")[0].checked) {
                    if ($("#<%= txtFreeNight.ClientID%>").val() == "") {
                        $("#<%= txtFreeNight.ClientID%>").addClass("incorrect");
                        $("#divPromoNights").addClass("incorrect");
                        //alert('Es necesario definir Noches Gratis ');
                        isValid = false;
                    }
                }
            }

            if ($("#txtCancelPolicyDescription").val() === "") {
                $("#txtCancelPolicyDescription").addClass("incorrect");
            }

            if ($("#ddlCancelationPolicy")[0].selectedIndex == 0 || $("#txtCancellationPolicy").val() === "") {
                $(".cancelPolicy").addClass("incorrect");

            }
            return isValid;
        }

        var loadBlackoutDays = function () {
            var blackoutDays = $("#<%= txtDiasBlackout.ClientID%>").val().split('|');
            if (blackoutDays.length) {
                blackoutDays.forEach(function (index) {
                    var from = index.split('-')[0];
                    var to = index.split('-')[1];

                    addDateBlackout(from, to);
                });
                $("#<%= txtDiasBlackout.ClientID%>").val("");
            }
        }

        var isOverlappedDates = function () {
            var selectedToDates = $('#Dates_Blackout li')
            var isOverlapped = false;

            var newFrom = $("#blackoutFrom").val()
            var newTo = $("#blackoutTo").val()

            if (newFrom === "" || newTo === "")
                return false;

            var dayNewFrom = newFrom.split("/")[0]
            var monthNewFrom = newFrom.split("/")[1]
            var yearNewFrom = newFrom.split("/")[2]

            var dayNewTo = newTo.split("/")[0]
            var monthNewTo = newTo.split("/")[1]
            var yearNewTo = newTo.split("/")[2]

            newFrom = new Date(yearNewFrom, monthNewFrom - 1, dayNewFrom);
            newTo = new Date(yearNewTo, monthNewTo - 1, dayNewTo);

            selectedToDates.each(function (index) {
                var from = $(this).children(".blackFrom")[0].id;
                var to = $(this).children(".blackTo")[0].id;

                var dayFrom = from.split("/")[0]
                var monthFrom = from.split("/")[1]
                var yearFrom = from.split("/")[2]

                var dayTo = to.split("/")[0]
                var monthTo = to.split("/")[1]
                var yearTo = to.split("/")[2]

                from = new Date(yearFrom, monthFrom - 1, dayFrom);
                to = new Date(yearTo, monthTo - 1, dayTo);

                if (!(from > newFrom && from > newTo) && !(to < newFrom && to < newTo)) {
                    isOverlapped = true;
                    alert("El periodo que trata de introducir se traslapa con fechas existentes.");
                    return 0;
                }
            });
            return isOverlapped;
        }

    </script>
    <script type="text/javascript">
        document.getElementById("defaultOpen").click();
    </script>
    <script type="text/javascript">
        // Get the modal
        var modal = document.getElementById('myModal');

        // Get the <span> element that closes the modal
        var span = document.getElementsByClassName("close")[0];

        // When the user clicks the button, open the modal 
        var openModal = function (id, msg, title) {
            modal.style.display = "block";
            document.getElementById('txtObjDelete').value = id;
            $(".modal-body .msg").text(msg);
            $(".modal-header h2").text(title);
        }

        // When the user clicks on <span> (x), close the modal
        var closeModal = function () {
            modal.style.display = "none";
        }

        // When the user clicks anywhere outside of the modal, close it
        window.onclick = function (event) {
            if (event.target == modal) {
                modal.style.display = "none";
            }
        }

        var deactivatePromo = function () {
            var id = document.getElementById('txtObjDelete').value;
            document.getElementById(id).click();
        }
    </script>
</body>
</html>
