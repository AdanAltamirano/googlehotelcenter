<%@ Import Namespace="RateManager" %>
<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../Portal/Modules/ctrlFooter.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../Portal/Modules/ctrlHeader.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" EnableEventValidation="true" CodeBehind="SegmentRoomsAvailability.aspx.vb"
    Inherits="RateManager.SegmentRoomsAvailability" %>

<%@ Register TagPrefix="uc1" TagName="ctrRoomRatePlan" Src="../Modulos/ctrRoomRatePlan.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrMenu" Src="../Portal/Modules/CtrMenu.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>SegmentRoomsAvailability</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet" />

    <script type="text/javascript" src="../Includes/Script/jquery-1.4.2.min.js"></script>

    <script type="text/javascript">

        function FireUpdateStatus() {
            $('#loadingProcess').show();
            return true;
        }
        
    </script>

    <style type="text/css" >
        #tblControles TD
        {
            border-right: silver 1px solid;
            border-top: silver 1px solid;
            border-left: silver 1px solid;
            border-bottom: silver 1px solid;
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
<body bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <iframe id="gToday:normal:agenda.js" style="z-index: 999; left: -500px; position: absolute;
        top: -500px" name="gToday:normal:agenda.js" src='<%=GeRequestApplicationPath(string.concat("/Calendar/",portalculture.getculture().Name.Substring(0, 2).ToLower(),"/ipopeng2.htm" )) %>'
        frameborder="0" width="174" scrolling="no" height="189"></iframe>
    <asp:TextBox ID="txtCambia" Style="display: none" runat="server" Width="34px" CssClass="TextBox"></asp:TextBox><asp:TextBox
        ID="txtCambiaCalendar" Style="display: none" runat="server" Width="34px" CssClass="TextBox"></asp:TextBox>
    <img class="bgplus" src="../Includes/imagenes/icono-big-plus.png" width="25" height="25" />
    <asp:Label ID="lblTitle" runat="server" EnableViewState="False" class="tituloSeccion">Inventory managment</asp:Label>
    <div class="clear">
        <div id="loadingProcess">
        </div>
        <table id="bookingcontainer" cellspacing="0" cellpadding="2" width="100%" border="0">
            <tr class="">
                <td>
                </td>
            </tr>
            <tr class="trContent rounded-corners">
                <td class="tdContent">
                    <table id="Table1" cellspacing="0" cellpadding="0" width="100%" align="center">
                        <tr>
                            <td>
                                <table id="Table2" cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td class="" align="center">
                                            <asp:TextBox ID="txtDivVisible" Style="display: none" runat="server" Width="34px"
                                                CssClass="TextBox"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="5">
                                            <asp:HyperLink ID="hplShow" runat="server" EnableViewState="False" CssClass="showOptions">Show</asp:HyperLink><asp:HyperLink
                                                ID="hplOcultar" Style="display: none" runat="server" CssClass="hideOptions">Hide</asp:HyperLink>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <div id="Div1" style="display: block" runat="server" width="100%">
                                                <table id="Table5" cellspacing="0" cellpadding="0" width="100%" border="0">
                                                    <tr>
                                                        <td valign="top" align="center">
                                                            <table id="Table4" style="height: 61px" cellspacing="0" cellpadding="0">
                                                                <tr>
                                                                    <td class="dgitem" align="center" colspan="2">
                                                                        <asp:Label ID="lbltitleDate" runat="server" EnableViewState="False" CssClass="bookingnormallabel">Select date</asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="bottom" align="center">
                                                                        <asp:Label ID="lblMonth" runat="server" EnableViewState="False" CssClass="clslabel">Month</asp:Label>
                                                                    </td>
                                                                    <td style="height: 17px" valign="bottom" align="center">
                                                                        <asp:Label ID="lblYear" runat="server" EnableViewState="False" CssClass="clslabel">Year</asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top" align="center">
                                                                        <asp:DropDownList ID="ddlMonth" runat="server">
                                                                        </asp:DropDownList>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td valign="top" align="center">
                                                                        &nbsp;
                                                                        <asp:DropDownList ID="ddlyear" runat="server">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="height: 12px" align="center" colspan="2">
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td valign="top" align="center">
                                                            <table id="Table8" cellspacing="0" cellpadding="0" align="center" border="0">
                                                                <tr>
                                                                    <td class="dgitem" align="center">
                                                                        <asp:Label ID="lblSelectType" runat="server" EnableViewState="False" CssClass="bookingnormallabel"> Select</asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="height: 17px" valign="bottom" align="center">
                                                                        <asp:Label ID="lblRoomType" runat="server" EnableViewState="False" CssClass="clslabel">Room Type</asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top" align="center">
                                                                        <asp:DropDownList ID="ddlRoomType" runat="server">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center">
                                                                        <asp:CustomValidator ID="cvMissingRoomTypes" runat="server" CssClass="Validators"
                                                                            ErrorMessage="No se encontraron tipos de habitación" ControlToValidate="ddlRoomtype"
                                                                            Display="Dynamic" ForeColor=" " Height="16px"></asp:CustomValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center">
                                                                        <asp:Label ID="lblRatePlanType" runat="server">Rate Plan Type</asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top" align="center">
                                                                        <asp:DropDownList ID="ddlRatePlanType" runat="server">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:CustomValidator ID="cvMissingRatePlanTypes" runat="server" CssClass="Validators"
                                                                            ErrorMessage="No se encontraron tipos de Plan Tarifario" ControlToValidate="ddlRatePlanType"
                                                                            Display="Dynamic" ForeColor=" " Height="16px"></asp:CustomValidator>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td valign="top" align="center">
                                                            <table id="Table6" style="width: 239px; height: 69px" cellspacing="0" cellpadding="0"
                                                                border="0">
                                                                <tr>
                                                                    <td class="dgitem" align="center" colspan="2">
                                                                        <asp:Label ID="lbltitleintervals" runat="server" EnableViewState="False" CssClass="bookingnormallabel">Change Data by intervals</asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center">
                                                                        <asp:Label ID="lblFrom" runat="server" EnableViewState="False" CssClass="clslabel">From</asp:Label>
                                                                    </td>
                                                                    <td align="center">
                                                                        <asp:Label ID="lblTo" runat="server" EnableViewState="False" CssClass="clslabel">to</asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top" align="center">
                                                                        <a hidefocus onclick="if(self.gfPop)gfPop.fPopCalendar(document.getElementById('txtFinal'),document.getElementById('txtInicio'));return false;"
                                                                            href="javascript:void(0)">
                                                                            <asp:TextBox ID="txtInicio" runat="server" Width="84px" CssClass="textbox" MaxLength="10"
                                                                                Columns="12"></asp:TextBox></a><a hidefocus onclick="if(self.gfPop)gfPop.fPopCalendar(document.getElementById('txtFinal'),document.getElementById('txtInicio'));return false;"
                                                                                    href="javascript:void(0)"><img class="PopcalTrigger" alt="" src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>'
                                                                                        align="absMiddle" border="0"></a>
                                                                    </td>
                                                                    <td valign="top" align="center">
                                                                        <a hidefocus onclick="if(self.gfPop)gfPop.fPopCalendar1(document.getElementById('txtFinal'));return false;"
                                                                            href="javascript:void(0)">
                                                                            <asp:TextBox ID="txtFinal" runat="server" Width="84px" CssClass="textbox" MaxLength="10"
                                                                                Columns="12"></asp:TextBox></a><a hidefocus onclick="if(self.gfPop)gfPop.fPopCalendar1(document.getElementById('txtFinal'));return false;"
                                                                                    href="javascript:void(0)"><img class="PopcalTrigger" alt="" src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>'
                                                                                        align="absMiddle" border="0"></a>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                    </td>
                                                                    <td>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right">
                                                                        <asp:Label ID="lblRooms" runat="server" EnableViewState="False" CssClass="clslabel">Rooms</asp:Label>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:TextBox ID="TxtRooms" runat="server" Width="40px" EnableViewState="False" CssClass="Textbox"
                                                                            MaxLength="3"></asp:TextBox>
                                                                        <asp:RegularExpressionValidator ID="valAdultExtraPrice" Display="Dynamic" CssClass="Validators"
                                                                            runat="server" ControlToValidate="TxtRooms" ValidationExpression="^([0-9]*|\d*\.\d{1}?\d*)$"
                                                                            ErrorMessage="* Precio Inválido">*</asp:RegularExpressionValidator>
                                                                        <asp:RangeValidator ID="rvRooms" runat="server" ControlToValidate="TxtRooms" ErrorMessage="RangeValidator"
                                                                            MaximumValue="999" MinimumValue="0" Type="integer" Display="Dynamic" CssClass="clsValidators"></asp:RangeValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center" colspan="2">
                                                                        <asp:Label ID="lblRoomsError" runat="server" Width="165px" EnableViewState="False"
                                                                            CssClass="Validators" Visible="False">Datos Inválidos</asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center" colspan="2">
                                                                        <asp:Button ID="btnSaveIntervals" runat="server" CssClass="button" Text="Save" CausesValidation="true">
                                                                        </asp:Button>
                                                                        <input type="hidden" id="hidMaxInventario" value="9999" runat="server" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" align="center">
                                                        </td>
                                                        <td align="center">
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" align="center" colspan="3">
                                                            <asp:Button ID="btnLoad" runat="server" EnableViewState="False" CssClass="button"
                                                                Text="Load" CausesValidation="False"></asp:Button>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: center;">
                                            <asp:Label ID="lblSugerencia" runat="server" EnableViewState="False" CssClass="Validators"
                                                Visible="False">Sugerencias</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:Label ID="lblInfoCalendar" runat="server" EnableViewState="False" CssClass="clslabel">(Reservación/Disponible)</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblRoomRatePlan" runat="server" CssClass="clsHelpLabel"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>                                            
                                            
                                            <table id="tblControles" cellspacing="0" cellpadding="0" width="100%" align="center"
                                                border="0">
                                                <tr>
                                                    <td class="dgitem" style="height: 14px" align="center" colspan="7">
                                                        <asp:Label ID="lbltitletable" runat="server" EnableViewState="False" CssClass="bookingnormallabel">Label</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr class="Titulo">
                                                    <td align="center" width="14.29%" style="height: 14px">
                                                        <asp:Label ID="lblDomingo" runat="server" EnableViewState="False">domingo</asp:Label>
                                                    </td>
                                                    <td align="center" width="14.29%">
                                                        <asp:Label ID="lblLunes" runat="server" EnableViewState="False">lunes</asp:Label>
                                                    </td>
                                                    <td align="center" width="14.29%">
                                                        <asp:Label ID="lblMartes" runat="server" EnableViewState="False">martes</asp:Label>
                                                    </td>
                                                    <td align="center" width="14.29%">
                                                        <asp:Label ID="lblMiercoles" runat="server" EnableViewState="False">miercoles</asp:Label>
                                                    </td>
                                                    <td align="center" width="14.29%">
                                                        <asp:Label ID="lblJueves" runat="server" EnableViewState="False">jueves</asp:Label>
                                                    </td>
                                                    <td align="center" width="14.29%">
                                                        <asp:Label ID="lblViernes" runat="server" EnableViewState="False">viernes</asp:Label>
                                                    </td>
                                                    <td align="center" width="14.29%">
                                                        <asp:Label ID="lblSabado" runat="server" EnableViewState="False">sabado</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <uc1:ctrRoomRatePlan ID="ctrRoomRatePlan1" runat="server"></uc1:ctrRoomRatePlan>
                                                    </td>
                                                    <td>
                                                        <uc1:ctrRoomRatePlan ID="ctrRoomRatePlan2" runat="server"></uc1:ctrRoomRatePlan>
                                                    </td>
                                                    <td>
                                                        <uc1:ctrRoomRatePlan ID="ctrRoomRatePlan3" runat="server"></uc1:ctrRoomRatePlan>
                                                    </td>
                                                    <td>
                                                        <uc1:ctrRoomRatePlan ID="ctrRoomRatePlan4" runat="server"></uc1:ctrRoomRatePlan>
                                                    </td>
                                                    <td>
                                                        <uc1:ctrRoomRatePlan ID="ctrRoomRatePlan5" runat="server"></uc1:ctrRoomRatePlan>
                                                    </td>
                                                    <td>
                                                        <uc1:ctrRoomRatePlan ID="ctrRoomRatePlan6" runat="server"></uc1:ctrRoomRatePlan>
                                                    </td>
                                                    <td>
                                                        <uc1:ctrRoomRatePlan ID="ctrRoomRatePlan7" runat="server"></uc1:ctrRoomRatePlan>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <uc1:ctrRoomRatePlan ID="ctrRoomRatePlan8" runat="server"></uc1:ctrRoomRatePlan>
                                                    </td>
                                                    <td>
                                                        <uc1:ctrRoomRatePlan ID="ctrRoomRatePlan9" runat="server"></uc1:ctrRoomRatePlan>
                                                    </td>
                                                    <td>
                                                        <uc1:ctrRoomRatePlan ID="ctrRoomRatePlan10" runat="server"></uc1:ctrRoomRatePlan>
                                                    </td>
                                                    <td>
                                                        <uc1:ctrRoomRatePlan ID="ctrRoomRatePlan11" runat="server"></uc1:ctrRoomRatePlan>
                                                    </td>
                                                    <td>
                                                        <uc1:ctrRoomRatePlan ID="ctrRoomRatePlan12" runat="server"></uc1:ctrRoomRatePlan>
                                                    </td>
                                                    <td>
                                                        <uc1:ctrRoomRatePlan ID="ctrRoomRatePlan13" runat="server"></uc1:ctrRoomRatePlan>
                                                    </td>
                                                    <td>
                                                        <uc1:ctrRoomRatePlan ID="ctrRoomRatePlan14" runat="server"></uc1:ctrRoomRatePlan>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <uc1:ctrRoomRatePlan ID="ctrRoomRatePlan15" runat="server"></uc1:ctrRoomRatePlan>
                                                    </td>
                                                    <td>
                                                        <uc1:ctrRoomRatePlan ID="ctrRoomRatePlan16" runat="server"></uc1:ctrRoomRatePlan>
                                                    </td>
                                                    <td>
                                                        <uc1:ctrRoomRatePlan ID="ctrRoomRatePlan17" runat="server"></uc1:ctrRoomRatePlan>
                                                    </td>
                                                    <td>
                                                        <uc1:ctrRoomRatePlan ID="ctrRoomRatePlan18" runat="server"></uc1:ctrRoomRatePlan>
                                                    </td>
                                                    <td>
                                                        <uc1:ctrRoomRatePlan ID="ctrRoomRatePlan19" runat="server"></uc1:ctrRoomRatePlan>
                                                    </td>
                                                    <td>
                                                        <uc1:ctrRoomRatePlan ID="ctrRoomRatePlan20" runat="server"></uc1:ctrRoomRatePlan>
                                                    </td>
                                                    <td>
                                                        <uc1:ctrRoomRatePlan ID="ctrRoomRatePlan21" runat="server"></uc1:ctrRoomRatePlan>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <uc1:ctrRoomRatePlan ID="ctrRoomRatePlan22" runat="server"></uc1:ctrRoomRatePlan>
                                                    </td>
                                                    <td>
                                                        <uc1:ctrRoomRatePlan ID="ctrRoomRatePlan23" runat="server"></uc1:ctrRoomRatePlan>
                                                    </td>
                                                    <td>
                                                        <uc1:ctrRoomRatePlan ID="ctrRoomRatePlan24" runat="server"></uc1:ctrRoomRatePlan>
                                                    </td>
                                                    <td>
                                                        <uc1:ctrRoomRatePlan ID="ctrRoomRatePlan25" runat="server"></uc1:ctrRoomRatePlan>
                                                    </td>
                                                    <td>
                                                        <uc1:ctrRoomRatePlan ID="ctrRoomRatePlan26" runat="server"></uc1:ctrRoomRatePlan>
                                                    </td>
                                                    <td>
                                                        <uc1:ctrRoomRatePlan ID="ctrRoomRatePlan27" runat="server"></uc1:ctrRoomRatePlan>
                                                    </td>
                                                    <td>
                                                        <uc1:ctrRoomRatePlan ID="ctrRoomRatePlan28" runat="server"></uc1:ctrRoomRatePlan>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <uc1:ctrRoomRatePlan ID="ctrRoomRatePlan29" runat="server"></uc1:ctrRoomRatePlan>
                                                    </td>
                                                    <td>
                                                        <uc1:ctrRoomRatePlan ID="ctrRoomRatePlan30" runat="server"></uc1:ctrRoomRatePlan>
                                                    </td>
                                                    <td>
                                                        <uc1:ctrRoomRatePlan ID="ctrRoomRatePlan31" runat="server"></uc1:ctrRoomRatePlan>
                                                    </td>
                                                    <td>
                                                        <uc1:ctrRoomRatePlan ID="ctrRoomRatePlan32" runat="server"></uc1:ctrRoomRatePlan>
                                                    </td>
                                                    <td>
                                                        <uc1:ctrRoomRatePlan ID="ctrRoomRatePlan33" runat="server"></uc1:ctrRoomRatePlan>
                                                    </td>
                                                    <td>
                                                        <uc1:ctrRoomRatePlan ID="ctrRoomRatePlan34" runat="server"></uc1:ctrRoomRatePlan>
                                                    </td>
                                                    <td>
                                                        <uc1:ctrRoomRatePlan ID="ctrRoomRatePlan35" runat="server"></uc1:ctrRoomRatePlan>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <uc1:ctrRoomRatePlan ID="ctrRoomRatePlan36" runat="server"></uc1:ctrRoomRatePlan>
                                                    </td>
                                                    <td>
                                                        <uc1:ctrRoomRatePlan ID="ctrRoomRatePlan37" runat="server"></uc1:ctrRoomRatePlan>
                                                    </td>
                                                    <td>
                                                        <uc1:ctrRoomRatePlan ID="ctrRoomRatePlan38" runat="server"></uc1:ctrRoomRatePlan>
                                                    </td>
                                                    <td>
                                                        <uc1:ctrRoomRatePlan ID="ctrRoomRatePlan39" runat="server"></uc1:ctrRoomRatePlan>
                                                    </td>
                                                    <td>
                                                        <uc1:ctrRoomRatePlan ID="ctrRoomRatePlan40" runat="server"></uc1:ctrRoomRatePlan>
                                                    </td>
                                                    <td>
                                                        <uc1:ctrRoomRatePlan ID="ctrRoomRatePlan41" runat="server"></uc1:ctrRoomRatePlan>
                                                    </td>
                                                    <td>
                                                        <uc1:ctrRoomRatePlan ID="ctrRoomRatePlan42" runat="server"></uc1:ctrRoomRatePlan>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            &nbsp;
                                            <asp:Button ID="cmdSave" runat="server" EnableViewState="False" CssClass="button"
                                                Text="Save"></asp:Button>&nbsp;
                                            <asp:Button ID="btnCancel" runat="server" EnableViewState="False" CssClass="button"
                                                Text="Cancel"></asp:Button>
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
    </form>

    <script type="text/javascript">
	function SaveAva(Room,RatePlan,txt,CodeRate,CodeRoom)
	 {
		var e = document.getElementById(txt);
	
		if (isnumber(e.value))
		{
			var e1 = document.getElementById('txtCambia');
			e1.value = e1.value  + Room + "/" + RatePlan + "/" + e.value + "/" + CodeRate + "/" + CodeRoom + "," ;
		}
		else
		{
			e.value =0;
		}		  
	}	
	
	function isnumber(valor)	 
	{		
		var ValidChars = "0123456789";
   		var IsNumber=true;
   		var Char;
  		
  		for (i = 0; i < valor.length && IsNumber == true; i++) 
      	{ 
      		Char = valor.charAt(i); 
      		if (ValidChars.indexOf(Char) == -1) 
        	{
    			IsNumber = false;
	    	}
		}
   		return IsNumber;
	}
		
	function ShowTxt(txt,lnk,Room,RatePlan,txt,CodeRate,CodeRoom)
	{ 
		var l = document.getElementById(txt);
		l.style.display = ''
		var t = document.getElementById(lnk);  
		t.style.display = "none"  
		SaveAva(Room,RatePlan,txt,CodeRate,CodeRoom)  
	}
			
	function ShowTxtCalendario(txt,lnk,d)
	{ 
		var l = document.getElementById(txt);
		l.style.display = ''
		l.focus();
		var t = document.getElementById(lnk);  
		t.style.display = "none"  
	    
		SaveAvaCalendar(txt,d)	 
	}
 
	function CambiaTxt(txt)
	{ 
		document.getElementById(txt).value="1"; 
		var e1 = document.getElementById('txtCambia');
	}

	function SaveAvaCalendar(txt,d)	 
	{		 
		var e = document.getElementById(txt);
		if (isnumber(e.value))
		{
			var e1 = document.getElementById('txtCambiaCalendar');
			var room = document.getElementById('ddlRoomType');
			var ratePlan = document.getElementById('ddlRatePlanType');
			var codeRate = document.getElementById('ddlRatePlanType');
			var codeRoom = document.getElementById('ddlRoomType');
			var a = document.getElementById('ddlyear');
			var m = document.getElementById('ddlMonth');			

			e1.value = e1.value  + room.value + "/ID" + ratePlan.value + "/" + e.value + "/" + codeRate.value + "/" + codeRoom.options[codeRoom.selectedIndex].innerText + "~" + (m.selectedIndex + 1).toString() + "/" + d + "/" + a.value + ",";
		}
		else
		{
			e.value =0;
		}		  
	}	

	function Ocultar(v)
	{
		var e= document.getElementById('Div1');
		var s = document.getElementById('hplShow');
		var o = document.getElementById('hplOcultar');
		var txtDiv = document.getElementById('txtDivVisible');
		
		if (v == '1')
		{
			e.style.display = '';     
			o.style.display = '';     
			s.style.display = "none";  
			txtDiv.value = 0;
		}
		else
		{
			e.style.display = "none";  
			s.style.display = '';     
			o.style.display = "none";  
			txtDiv.value = 1;
		} 
	}			
	
	function SaveAvaIntervals(txt) 
	{		 
		var e = document.getElementById(txt);		
		if (isnumber(e.value))
		{
			var e1 = document.getElementById('txtCambia');
			var room = document.getElementById('ddlRoomType');
			var ratePlan = document.getElementById('ddlRatePlanType');
			var codeRate = document.getElementById('ddlRatePlanType');
			var codeRoom = document.getElementById('ddlRoomType');						

			e1.value = e1.value  + room.value + "/ID" + ratePlan.value + "/" + e.value + "/" + codeRate.value + "/" + codeRoom.options[codeRoom.selectedIndex].innerText  + ",";		
		}		
	}	
	
	function validaAvaIntervals(txt) 
	{		 
		var e = document.getElementById(txt);
		var e1 = document.getElementById('txtCambia');	
		if (isnumber(e.value) )
		{			
			e1.value = "" ;
		}		
		else
		{
			e1.value = "error" ;
		}
	}	
	
	
var resource= '<%= PortalCulture.GetString("01352") %>'
    
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
        var eroom = document.getElementById('ddlRoomType');
        var eplan= document.getElementById('ddlRatePlanType');
        var ecant= document.getElementById('TxtRooms');
        var edfr = document.getElementById('txtInicio');
        var edto = document.getElementById('txtFinal');
            
        if (eroom && eplan && ecant) {       
            var msg = '';            
            msg = msg.format(resource, ecant.value, eroom.options[eroom.selectedIndex].text, eplan.options[eplan.selectedIndex].text, edfr.value, edto.value);
            if (!isDigit(ecant.value)) return true;
            return confirm (msg);
        }
        return confirm ("se modifico inventario");
    }
        
		 
    </script>

    <script type="text/javascript">
        $().ready(function() {
            $('#loadingProcess').hide();
        });  
    </script>

</body>
</html>
