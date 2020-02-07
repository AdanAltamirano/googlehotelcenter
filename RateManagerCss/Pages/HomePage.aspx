<%@ Import Namespace="RateManager" %>
<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../Portal/Modules/ctrlFooter.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../Portal/Modules/ctrlHeader.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="HomePage.aspx.vb" Inherits="RateManager.HomePage" %>

<%@ Register TagPrefix="uc1" TagName="ctlMensajes" Src="../Modulos/ctlMensajes.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrMenu" Src="../Portal/Modules/CtrMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlHP" Src="../Modulos/ctrlHP.ascx" %>
<%--<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
<html>
<head>
    <title>HomePage</title>
    <meta http-equiv="Content-Type" content="text/html; charset=windows-1252" />
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <link href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet" />

    <script type="text/javascript" src="../Includes/Script/jquery-1.4.2.min.js"></script>

    <style type="text/css" >
        #tblControles TD
        {
            border-right: silver 1px solid;
            border-top: silver 1px solid;
            border-left: silver 1px solid;
            border-bottom: silver 1px solid;
            vertical-align: top; 
        }
        #tblCtrlHomePage TD
        {
            border-right: 0px;
            border-top: 0px;
            border-left: 0px;
            border-bottom: 0px;
        }
    </style>
</head>
<body bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0" ms_positioning="flowlayout">

    <script>
        function IniDate() {
            var fecha = new Date();
            var fecha2 = new Date(2030, 12, 31);
            var arr = new Array(3);
            arr[0] = [fecha.getFullYear(), fecha.getMonth() + 1, fecha.getDate()];
            arr[1] = [fecha2.getFullYear(), fecha2.getMonth() + 1, fecha2.getDate()];
            return arr;

        }
        function FireUpdateStatus() {
            $('#loadingProcess').show();
            return true;
        }
		             
    </script>

    <form id="Form" method="post" runat="server">
    <asp:HyperLink ID="hplSubmit" runat="server"></asp:HyperLink>
    <div class="clear">        
        <div class="title">
            <asp:Label ID="lblMsg" runat="server" EnableViewState="False" Text="Inventario de Habitacion"
                CssClass="tituloSeccion"></asp:Label>
        </div>
        <div id="loadingProcess">            
        </div>
    </div>
    <table id="bookingcontainer" cellspacing="0" cellpadding="0" width="100%" border="0">
        <tbody>
            <tr>
                <td>
                    <table id="Table1" cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                        <tr>
                            <td align="center" colspan="4">
                                <asp:TextBox ID="txtDivVisible" Style="display: none" runat="server" CssClass="TextBox"
                                    Width="34px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="Titulo" align="center" colspan="4">
                                <asp:Label ID="lblTitle" runat="server" EnableViewState="False">A1K ROOM INVENTORY</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:HyperLink ID="hplShow" runat="server" EnableViewState="False" CssClass="showOptions">Show</asp:HyperLink><asp:HyperLink
                                    ID="hplOcultar" Style="display: none" runat="server" CssClass="hideOptions">Hide</asp:HyperLink>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <div id="Div1" style="display: block" runat="server" width="100%">
                                    <table width="100%">
                                        <tr>
                                            <td valign="top" align="center">
                                                <table height="110" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td class="dgitem" align="center" colspan="2">
                                                            <asp:Label ID="lbltitleDate" runat="server" EnableViewState="False" CssClass="bookingnormallabel">Select date</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <asp:Label ID="lblMonth" runat="server" EnableViewState="False" CssClass="clslabel">Month</asp:Label>
                                                        </td>
                                                        <td align="center">
                                                            <asp:Label ID="lblYear" runat="server" EnableViewState="False" CssClass="clslabel">Year</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <asp:DropDownList ID="ddlMonth" runat="server" Style="text-transform: capitalize;">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td align="center">
                                                            <asp:DropDownList ID="ddlyear" runat="server">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" colspan="2">
                                                            <asp:Label ID="lblRoomType" runat="server" EnableViewState="False" CssClass="clslabel">Room Type:</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" colspan="2">
                                                            <asp:DropDownList ID="ddlRoomtype" runat="server">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" colspan="2">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:CustomValidator ID="cvMissingRoomTypes" runat="server" CssClass="Validators"
                                                                Height="16px" ForeColor=" " Display="Dynamic" ControlToValidate="ddlRoomtype"
                                                                ErrorMessage="No se encontraron tipos de habitación"></asp:CustomValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="bottom" align="center" colspan="2">
                                                            <asp:Button ID="cmdShowInventory" runat="server" EnableViewState="False" CssClass="button"
                                                                CausesValidation="False" Text="Show Inventory"></asp:Button>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td valign="top" align="center">
                                                <table height="110" cellspacing="0" cellpadding="0" align="center">
                                                    <tr>
                                                        <td class="dgitem" align="center" colspan="3">
                                                            <asp:Label ID="lbltitleintervals" runat="server" EnableViewState="False" CssClass="bookingnormallabel">Change Data by intervals</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr align="center">
                                                        <td align="center" colspan="3">
                                                            <asp:Label ID="lblFrom" runat="server" EnableViewState="False" CssClass="clslabel">From</asp:Label><a
                                                                hidefocus onclick="if(self.gfPop)gfPop.fPopCalendar(document.getElementById('txtFinal'),document.getElementById('txtInicio'),IniDate());return false;"
                                                                href="javascript:void(0)"><asp:TextBox ID="txtInicio" runat="server" Width="84px"
                                                                    CssClass="textbox" MaxLength="12"></asp:TextBox></a><a hidefocus onclick="if(self.gfPop)gfPop.fPopCalendar(document.getElementById('txtFinal'),document.getElementById('txtInicio'),IniDate());return false;"
                                                                        href="javascript:void(0)"><img class="PopcalTrigger" alt="" src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>'
                                                                            align="absMiddle" border="0"></a><asp:Label ID="lblTo" runat="server" EnableViewState="False"
                                                                                CssClass="clslabel" Style="padding-left: 8px;">to</asp:Label><a hidefocus onclick="if(self.gfPop)gfPop.fPopCalendar1(document.getElementById('txtFinal'),IniDate());return false;"
                                                                                    href="javascript:void(0)"><asp:TextBox ID="txtFinal" runat="server" Width="84px"
                                                                                        CssClass="textbox" MaxLength="12"></asp:TextBox></a><a hidefocus onclick="if(self.gfPop)gfPop.fPopCalendar1(document.getElementById('txtFinal'),IniDate());return false;"
                                                                                            href="javascript:void(0)"><img class="PopcalTrigger" alt="" src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>'
                                                                                                align="absMiddle" border="0"></a><asp:Label ID="lblError" runat="server" EnableViewState="False"
                                                                                                    CssClass="Validators" Visible="False">error</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" colspan="3" style="padding-top: 4px;">
                                                            <table id="Table2" cellspacing="0" cellpadding="0" width="100%" border="0">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="LblDom" runat="server" EnableViewState="False" CssClass="clsLabel">L</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="LblLun" runat="server" EnableViewState="False" CssClass="clsLabel">L</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblmar" runat="server" EnableViewState="False" CssClass="clsLabel">M</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblmie" runat="server" EnableViewState="False" CssClass="clsLabel">M</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lbljue" runat="server" EnableViewState="False" CssClass="clsLabel">J</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblvie" runat="server" EnableViewState="False" CssClass="clsLabel">V</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblsab" runat="server" EnableViewState="False" CssClass="clsLabel">S</asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBox ID="Chk7" runat="server" Checked="True"></asp:CheckBox>
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox ID="Chk1" runat="server" Checked="True"></asp:CheckBox>
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox ID="Chk2" runat="server" Checked="True"></asp:CheckBox>
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox ID="Chk3" runat="server" Checked="True"></asp:CheckBox>
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox ID="Chk4" runat="server" Checked="True"></asp:CheckBox>
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox ID="Chk5" runat="server" Checked="True"></asp:CheckBox>
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox ID="Chk6" runat="server" Checked="True"></asp:CheckBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 21px" align="center" colspan="3">
                                                            <asp:Label ID="lblRooms" runat="server" EnableViewState="False" CssClass="clslabel">Rooms</asp:Label><asp:TextBox
                                                                ID="TxtRooms" runat="server" Width="40px" EnableViewState="False" CssClass="Textbox"
                                                                MaxLength="3"></asp:TextBox>
                                                            <asp:RegularExpressionValidator ID="valTotalRooms" Display="Dynamic" CssClass="Validators"
                                                                runat="server" ControlToValidate="TxtRooms" ValidationExpression="^([0-9]*|\d*\.\d{1}?\d*)$"
                                                                ErrorMessage="" Enabled="false">*</asp:RegularExpressionValidator>
                                                            <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ControlToValidate="TxtRooms"
                                                                CssClass="clsValidators" ErrorMessage="*"><%=RateManager.PortalCulture.GetString("00071")%></asp:RequiredFieldValidator>
                                                            <asp:RangeValidator ID="rvRooms" runat="server" ControlToValidate="TxtRooms" ErrorMessage="RangeValidator"
                                                                MaximumValue="999" MinimumValue="0" Type="integer" Display="Dynamic" CssClass="Validators"></asp:RangeValidator>
                                                            <asp:Label ID="lblRoomsError" runat="server" EnableViewState="False" CssClass="Validators"
                                                                Visible="False">Label</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr align="center">
                                                        <td valign="bottom" align="center" colspan="3">
                                                            <asp:Button ID="btnSaveIntervals" runat="server" CssClass="button" Text='<%=PortalCulture.GetString("00008") %>'>
                                                            </asp:Button>
                                                            <asp:Button ID="btnSingleImgInv" runat="server" CssClass="button" CausesValidation="false" Visible="false" Text='Sinc Channel Manager'>
                                                            </asp:Button>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label CssClass="Validators" Visible="false" runat="server" ID="lblIsHouse">Esta propiedad es una casa. Solo se permite la cantidad de 1 en inventario.</asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td valign="top" align="center">
                                                <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                                                    <tr class="dgitem" valign="top">
                                                        <td align="center" colspan="2">
                                                            <asp:Label ID="lblEstatus" runat="server" EnableViewState="False" CssClass="bookingnormallabel">Estatus</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="10%" bgcolor="red" style="border-bottom: solid 2px #fff;">
                                                        </td>
                                                        <td align="left" width="90%">
                                                            <asp:Label ID="lblClose" runat="server" EnableViewState="False" CssClass="clslabel"
                                                                Style="margin-left: 4px;">Close</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="10%" bgcolor="green" style="border-bottom: solid 2px #fff;">
                                                        </td>
                                                        <td align="left" width="90%">
                                                            <asp:Label ID="lblOpen" runat="server" EnableViewState="False" CssClass="clslabel"
                                                                Style="margin-left: 4px;">Open</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="10%" bgcolor="blueviolet">
                                                        </td>
                                                        <td align="left" width="90%">
                                                            <asp:Label ID="lblNoArrivals" runat="server" EnableViewState="False" CssClass="clslabel"
                                                                Style="margin-left: 4px;">No arrivals</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" style="padding-top: 4px;">
                                                            <style>
                                                                #tblDescripciones
                                                                {
                                                                    border-right: silver 1px solid;
                                                                    border-top: silver 1px solid;
                                                                    border-left: silver 1px solid;
                                                                    border-bottom: silver 1px solid;
                                                                }
                                                            </style>
                                                            <table id="tblDescripciones" cellspacing="0" cellpadding="0" width="100%">
                                                                <tr>
                                                                    <td class="dgAlternate" align="left" style=" padding-left:4px; ">
                                                                        <asp:Label ID="lblDayOfMonth" runat="server" EnableViewState="False" CssClass="clslabel">day of de month</asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center">
                                                                        <asp:Label ID="lblRackRate" runat="server" EnableViewState="False" CssClass="clslabel">Rack rate</asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center">
                                                                        <asp:Label ID="lblAvailRes" runat="server" EnableViewState="False" CssClass="clslabel">Res/Avail</asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center">
                                                                        <asp:Label ID="lbllowestRate" runat="server" EnableViewState="False" CssClass="clslabel">lowest 
																				Rate</asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
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
                            <td align="center" colspan="4">
                                
                                <table id="tblControles" cellspacing="0" cellpadding="0" width="100%" align="center"
                                    border="0" style="padding-bottom:6px; ">
                                    <tr>
                                        <td class="dgitem" align="center" colspan="7">
                                            <asp:Label ID="lbltitletable" runat="server" EnableViewState="False" CssClass="bookingnormallabel">Label</asp:Label>
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
                                            <uc1:ctrlHP ID="CtrlHP1" runat="server"></uc1:ctrlHP>
                                        </td>
                                        <td>
                                            <uc1:ctrlHP ID="CtrlHP2" runat="server"></uc1:ctrlHP>
                                        </td>
                                        <td>
                                            <uc1:ctrlHP ID="CtrlHP3" runat="server"></uc1:ctrlHP>
                                        </td>
                                        <td>
                                            <uc1:ctrlHP ID="CtrlHP4" runat="server"></uc1:ctrlHP>
                                        </td>
                                        <td>
                                            <uc1:ctrlHP ID="CtrlHP5" runat="server"></uc1:ctrlHP>
                                        </td>
                                        <td>
                                            <uc1:ctrlHP ID="CtrlHP6" runat="server"></uc1:ctrlHP>
                                        </td>
                                        <td>
                                            <uc1:ctrlHP ID="CtrlHP7" runat="server"></uc1:ctrlHP>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <uc1:ctrlHP ID="CtrlHP8" runat="server"></uc1:ctrlHP>
                                        </td>
                                        <td>
                                            <uc1:ctrlHP ID="CtrlHP9" runat="server"></uc1:ctrlHP>
                                        </td>
                                        <td>
                                            <uc1:ctrlHP ID="CtrlHP10" runat="server"></uc1:ctrlHP>
                                        </td>
                                        <td>
                                            <uc1:ctrlHP ID="CtrlHP11" runat="server"></uc1:ctrlHP>
                                        </td>
                                        <td>
                                            <uc1:ctrlHP ID="CtrlHP12" runat="server"></uc1:ctrlHP>
                                        </td>
                                        <td>
                                            <uc1:ctrlHP ID="CtrlHP13" runat="server"></uc1:ctrlHP>
                                        </td>
                                        <td>
                                            <uc1:ctrlHP ID="CtrlHP14" runat="server"></uc1:ctrlHP>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <uc1:ctrlHP ID="CtrlHP15" runat="server"></uc1:ctrlHP>
                                        </td>
                                        <td>
                                            <uc1:ctrlHP ID="CtrlHP16" runat="server"></uc1:ctrlHP>
                                        </td>
                                        <td>
                                            <uc1:ctrlHP ID="CtrlHP17" runat="server"></uc1:ctrlHP>
                                        </td>
                                        <td>
                                            <uc1:ctrlHP ID="CtrlHP18" runat="server"></uc1:ctrlHP>
                                        </td>
                                        <td>
                                            <uc1:ctrlHP ID="CtrlHP19" runat="server"></uc1:ctrlHP>
                                        </td>
                                        <td>
                                            <uc1:ctrlHP ID="CtrlHP20" runat="server"></uc1:ctrlHP>
                                        </td>
                                        <td>
                                            <uc1:ctrlHP ID="CtrlHP21" runat="server"></uc1:ctrlHP>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <uc1:ctrlHP ID="CtrlHP22" runat="server"></uc1:ctrlHP>
                                        </td>
                                        <td>
                                            <uc1:ctrlHP ID="CtrlHP23" runat="server"></uc1:ctrlHP>
                                        </td>
                                        <td>
                                            <uc1:ctrlHP ID="CtrlHP24" runat="server"></uc1:ctrlHP>
                                        </td>
                                        <td>
                                            <uc1:ctrlHP ID="CtrlHP25" runat="server"></uc1:ctrlHP>
                                        </td>
                                        <td>
                                            <uc1:ctrlHP ID="CtrlHP26" runat="server"></uc1:ctrlHP>
                                        </td>
                                        <td>
                                            <uc1:ctrlHP ID="CtrlHP27" runat="server"></uc1:ctrlHP>
                                        </td>
                                        <td>
                                            <uc1:ctrlHP ID="CtrlHP28" runat="server"></uc1:ctrlHP>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <uc1:ctrlHP ID="CtrlHP29" runat="server"></uc1:ctrlHP>
                                        </td>
                                        <td>
                                            <uc1:ctrlHP ID="CtrlHP30" runat="server"></uc1:ctrlHP>
                                        </td>
                                        <td>
                                            <uc1:ctrlHP ID="CtrlHP31" runat="server"></uc1:ctrlHP>
                                        </td>
                                        <td>
                                            <uc1:ctrlHP ID="CtrlHP32" runat="server"></uc1:ctrlHP>
                                        </td>
                                        <td>
                                            <uc1:ctrlHP ID="CtrlHP33" runat="server"></uc1:ctrlHP>
                                        </td>
                                        <td>
                                            <uc1:ctrlHP ID="CtrlHP34" runat="server"></uc1:ctrlHP>
                                        </td>
                                        <td>
                                            <uc1:ctrlHP ID="CtrlHP35" runat="server"></uc1:ctrlHP>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <uc1:ctrlHP ID="CtrlHP36" runat="server"></uc1:ctrlHP>
                                        </td>
                                        <td>
                                            <uc1:ctrlHP ID="CtrlHP37" runat="server"></uc1:ctrlHP>
                                        </td>
                                        <td>
                                            <uc1:ctrlHP ID="CtrlHP38" runat="server"></uc1:ctrlHP>
                                        </td>
                                        <td>
                                            <uc1:ctrlHP ID="CtrlHP39" runat="server"></uc1:ctrlHP>
                                        </td>
                                        <td>
                                            <uc1:ctrlHP ID="CtrlHP40" runat="server"></uc1:ctrlHP>
                                        </td>
                                        <td>
                                            <uc1:ctrlHP ID="CtrlHP41" runat="server"></uc1:ctrlHP>
                                        </td>
                                        <td>
                                            <uc1:ctrlHP ID="CtrlHP42" runat="server"></uc1:ctrlHP>
                                        </td>
                                    </tr>
                                </table>
                                <asp:Button ID="btnSave" runat="server" EnableViewState="False" CssClass="button"
                                    Text="Button" CausesValidation="false"></asp:Button>
                            </td>
                        </tr>
                    </table>                 
                </td>
            </tr>
        </tbody>
    </table>
    <iframe id="gToday:normal:agenda.js" style="z-index: 999; left: -500px; position: absolute;
        top: -500px" name="gToday:normal:agenda.js" src='<%=GeRequestApplicationPath(string.concat("/Calendar/",portalculture.getculture().Name.Substring(0, 2).ToLower(),"/ipopeng2.htm" )) %>'
        frameborder="0" width="174" scrolling="no" height="189"></iframe>
    <uc1:ctlMensajes ID="CtlMensajes1" runat="server"></uc1:ctlMensajes>
    </form>

    <script type="text/javascript">

        function ShowTxt(txt, lnk) {
            var l = document.getElementById(txt);
            l.style.display = '';
            l.focus();
            var t = document.getElementById(lnk);
            t.style.display = "none";

        }
        function Ocultar(v) {
            var e = document.getElementById('Div1');
            var s = document.getElementById('hplShow');
            var o = document.getElementById('hplOcultar');
            var txtDiv = document.getElementById('txtDivVisible');
            if (v == '1') {
                e.style.display = 'block';
                o.style.display = 'block';
                s.style.display = "none";
                txtDiv.value = 0;
            }
            else {
                e.style.display = "none";
                s.style.display = 'block';
                o.style.display = "none";
                txtDiv.value = 1;
            }
        }
        function CambiaTxt(txt) {
            document.getElementById(txt).value = "1";
        }
        function showRoomType(ddl, array) {
            var e = document.getElementById(ddl);
            var lbl = document.getElementById('lblRoomName');
            if (e.selectedIndex != 0) {
                lbl.firstChild.nodeValue = array.split("//")[e.selectedIndex];
            }
            else {
                lbl.firstChild.nodeValue = '-';
            }

        }

        var resource = '<%= PortalCulture.GetString("01354") %>'

        String.prototype.format = function() {
            var elements = new Array();
            var strfrmt = arguments[0];
            var item = '';
            for (var i = 1; i < arguments.length; i++) {
                item = "{" + (i - 1) + "}";
                strfrmt = strfrmt.replace(item, arguments[i]);
            }
            return strfrmt;
        }

        function isDigit(src) {
            var spattern = /^\d+$/g;
            return spattern.exec(src) == null ? false : true;
        }

        function AlertMessage() {
            var eroom = document.getElementById('ddlRoomtype');
            var ecant = document.getElementById('TxtRooms');
            var edfr = document.getElementById('txtInicio');
            var edto = document.getElementById('txtFinal');
            if (eroom && ecant) {
                var msg = '';
                msg = msg.format(resource, ecant.value, eroom.options[eroom.selectedIndex].text, edfr.value, edto.value);
                if (!isDigit(ecant.value)) return false;
                return confirm(msg);
            }
            return confirm("se modificara inventario");
        }
        
        
    </script>

    <script type="text/javascript">
        $().ready(function() {
            $('#loadingProcess').hide();
        });  
    </script>

</body>
</html>
