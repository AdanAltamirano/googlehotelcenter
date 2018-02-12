<%@ Control Language="vb" AutoEventWireup="false"   CodeBehind="ctlMensajeRuleConf.ascx.vb"
    Inherits="RateManager.ctlMensajeRuleConf"  TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<link href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet"></link>
<script type="text/javascript">
    function cancelRule(pnl1, pnl2) {
        document.getElementById(pnl1).style.display = 'none';
        document.getElementById(pnl2).style.display = 'none';
        //var arrSelects=document.getElementsByTagName('SELECT');
        //  for (var i=0; i<arrSelects.length; i++)
        //  {arrSelects[i].style.display='';}

        var tbl = document.getElementById('bookingcontainer');
        document.getElementById("CtlMensajeRuleConf1_ddlRatePlanConf").selectedIndex = 0;
        tbl.style.display = '';
    }

    function okRule(pnl1, pnl2) {
        var txt = document.getElementById('txtIdObj');
        document.getElementById(txt.Value).click();
    }

    function showRule(pnl1, pnl2, obj) {
        document.getElementById(pnl1).style.display = "none";
        document.getElementById(pnl2).style.display = "";
        document.getElementById("CtlMensajeRuleConf1_LoadImage").style.display = "";
        //var arrSelects=document.getElementsByTagName('SELECT');
        //for (var i=0; i<arrSelects.length; i++)
        //{arrSelects[i].style.display='None';}
//        var txt = document.getElementById('txtIdObj');
//        txt.Value = obj;
//        if (parent) {
//            //parent.resizeIframe(400);
//        }
    }

    function show2(pnlBox) {
        document.getElementById(pnlBox).style.display = "";
        document.getElementById("CtlMensajeRuleConf1_LoadImage").style.display = "none";
    }
    
</script>

<asp:Panel ID="TblPnl" Style="border: #ccc 1px solid; display: none; z-index: 997;
    left: 0px; position: absolute; top: 30px;" Width="99%" runat="server">
  <table id="tblLoading" cellspacing="0" cellpadding="0" width="100%" align="center"
        border="0">
        <tr>
            <td align="center">
                <input id="txtIdObj" type="hidden" name="txtIdObj">
                <asp:Image ID="LoadImage" runat="server" ImageUrl="../Images/HugeRotation.gif"></asp:Image>
                <asp:Panel ID="PnlBox" Style="display: block;" runat="server" Width="100%">
                    <table id="Table5" cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                        <tr><td>
                            <%--<asp:Label ID="lblDateSearch" runat="server" Text="Label" CssClass="clslabel" EnableViewState="False">01/01/2001</asp:Label>--%>
                        </td></tr>
                        <tr>
                            <td valign="top" align="center">
                                <table id="bookingcontainer" cellspacing="1" cellpadding="0" width="500" align="center"
                                    border="0">
                                    <tr>
                                        <td class="" align="center" colspan="2">
                                            <asp:Label ID="lblTitle" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="" align="center" colspan="2">
                                            <asp:Label ID="lblPrompt" runat="server" CssClass="clsDarkLabel" Visible="False"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" align="left" height="50">
                                            <table id="TblRuleHotel" cellspacing="0" cellpadding="0" border="0" width="99%">
                                                <tr>
                                                    <td valign="middle" align="center" colspan="4" class="dgitem">
                                                        <asp:Label ID="Label1" runat="server" CssClass="bookingNormalLabel">Hotel General</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="middle" align="left">
                                                        <asp:Label ID="LabelStatusH" runat="server" CssClass="clslabel" EnableViewState="False"> Status:</asp:Label>
                                                    </td>
                                                    <td style="width: 53px" valign="middle" align="left" colspan="2">
                                                        <asp:Label ID="LabelStatH" runat="server" CssClass="bookingNormalLabel" EnableViewState="False">Open</asp:Label>
                                                    </td>
                                                    <td valign="middle" align="left">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="middle" align="left">
                                                        <asp:Label ID="lblMinNightsH" runat="server" CssClass="clslabel" EnableViewState="False">Min. Nights</asp:Label>
                                                    </td>
                                                    <td style="width: 53px" valign="middle" align="left">
                                                        <asp:TextBox ID="txtMinNightsH" runat="server" Width="40px" CssClass="textbox" ReadOnly="True"
                                                            MaxLength="3"></asp:TextBox>
                                                    </td>
                                                    <td valign="middle" align="left">
                                                        <asp:Label ID="lblMaxNightsH" runat="server" CssClass="clslabel" EnableViewState="False">Max. Nights</asp:Label>
                                                    </td>
                                                    <td valign="middle" align="left">
                                                        <asp:TextBox ID="txtMaxNightsH" runat="server" Width="40px" CssClass="textbox" ReadOnly="True"
                                                            MaxLength="3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="middle" align="left">
                                                        <asp:Label ID="lblAdvanceddaysH" runat="server" CssClass="clslabel" EnableViewState="False">Advanced booking days:</asp:Label>
                                                    </td>
                                                    <td style="width: 53px" valign="middle" align="left">
                                                        <asp:TextBox ID="txtAdvancedDaysH" runat="server" Width="40px" CssClass="textbox"
                                                            ReadOnly="True"></asp:TextBox>
                                                    </td>
                                                    <td valign="middle" align="left">
                                                    </td>
                                                    <td valign="middle" align="left">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblNoArrivalsH" runat="server" CssClass="clslabel" EnableViewState="False">No Arrivals:</asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <table id="Table6" cellspacing="0" cellpadding="0" border="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lbldomingoH" runat="server" CssClass="clsLabel" EnableViewState="False">Dom</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblLunesH" runat="server" CssClass="clsLabel" EnableViewState="False">Lu</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblMartesH" runat="server" CssClass="clsLabel" EnableViewState="False">Mar</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblMiercolesH" runat="server" CssClass="clsLabel" EnableViewState="False">Mier</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblJuevesH" runat="server" CssClass="clsLabel" EnableViewState="False">Jue</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblViernesH" runat="server" CssClass="clsLabel" EnableViewState="False">Vi</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblSabadoH" runat="server" CssClass="clsLabel" EnableViewState="False">Sab</asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="Chk7H" runat="server" Enabled="False"></asp:CheckBox>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="Chk1H" runat="server" Enabled="False"></asp:CheckBox>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="Chk2H" runat="server" Enabled="False"></asp:CheckBox>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="Chk3H" runat="server" Enabled="False"></asp:CheckBox>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="Chk4H" runat="server" Enabled="False"></asp:CheckBox>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="Chk5H" runat="server" Enabled="False"></asp:CheckBox>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="Chk6H" runat="server" Enabled="False"></asp:CheckBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 29px" valign="middle" align="left">
                                                        <asp:Label ID="lblPrioridadCHotel" runat="server" CssClass="clslabel" EnableViewState="False">Prioridad de cancelacion:</asp:Label>
                                                    </td>
                                                    <td style="width: 53px; height: 29px" valign="middle" align="left">
                                                        <asp:Label ID="lblCancelacionH" runat="server" CssClass="clslabel" EnableViewState="False">Por Dias</asp:Label>
                                                    </td>
                                                    <td style="height: 29px" valign="middle" align="left">
                                                        <asp:TextBox ID="TextboxCH" runat="server" Width="40px" CssClass="textbox" ReadOnly="True"></asp:TextBox>
                                                    </td>
                                                    <td style="height: 29px" valign="middle" align="left">
                                                        <asp:Label ID="lblMsgCH" runat="server" CssClass="clslabel" EnableViewState="False">Dias antes del Check In</asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td valign="baseline" align="center" height="50">
                                            <table id="TblRuleRatePLanHotel" cellspacing="0" cellpadding="0" width="99%" border="0">
                                                <tr>
                                                    <td valign="middle" align="center" colspan="4" class="dgitem">
                                                        <asp:Label ID="Label18" runat="server" CssClass="bookingNormalLabel">Hotel RatePlan</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="middle" align="left">
                                                        <asp:Label ID="LabelHRP" runat="server" CssClass="clslabel" EnableViewState="False">Rate Plans</asp:Label>
                                                    </td>
                                                    <td valign="middle" align="left" colspan="3">                                                        
                                                        <asp:DropDownList ID="ddlRatePlanHotel" runat="server" Width="240px">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="middle" align="left">
                                                        <asp:Label ID="lblMinNightsHRP" runat="server" CssClass="clslabel" EnableViewState="False">Min. Nights</asp:Label>
                                                    </td>
                                                    <td valign="middle" align="left">
                                                        <asp:TextBox ID="txtMinNightsHRP" runat="server" Width="40px" CssClass="textbox"
                                                            ReadOnly="True" MaxLength="3"></asp:TextBox>
                                                    </td>
                                                    <td valign="middle" align="left">
                                                        <asp:Label ID="lblMaxNightsHRP" runat="server" CssClass="clslabel" EnableViewState="False">Max. Nights</asp:Label>
                                                    </td>
                                                    <td valign="middle" align="left">
                                                        <asp:TextBox ID="txtMaxNightsHRP" runat="server" Width="40px" CssClass="textbox"
                                                            ReadOnly="True" MaxLength="3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="middle" align="left">
                                                        <asp:Label ID="lblAdvanceddaysHRP" runat="server" CssClass="clslabel" EnableViewState="False">Advanced booking days:</asp:Label>
                                                    </td>
                                                    <td valign="middle" align="left">
                                                        <asp:TextBox ID="txtAdvancedDaysHRP" runat="server" Width="40px" CssClass="textbox"
                                                            ReadOnly="True"></asp:TextBox>
                                                    </td>
                                                    <td valign="middle" align="left">
                                                    </td>
                                                    <td valign="middle" align="left">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblNoArrivalsHRP" runat="server" CssClass="clslabel" EnableViewState="False">No Arrivals:</asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <table id="Table3" cellspacing="0" cellpadding="0" border="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lbldomingoHRP" runat="server" CssClass="clsLabel" EnableViewState="False">Dom</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblLunesHRP" runat="server" CssClass="clsLabel" EnableViewState="False">Lu</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblMartesHRP" runat="server" CssClass="clsLabel" EnableViewState="False">Mar</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblMiercolesHRP" runat="server" CssClass="clsLabel" EnableViewState="False">Mier</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblJuevesHRP" runat="server" CssClass="clsLabel" EnableViewState="False">Jue</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblViernesHRP" runat="server" CssClass="clsLabel" EnableViewState="False">Vi</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblSabadoHRP" runat="server" CssClass="clsLabel" EnableViewState="False">Sab</asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="Chk7HRP" runat="server" Enabled="False"></asp:CheckBox>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="Chk1HRP" runat="server" Enabled="False"></asp:CheckBox>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="Chk2HRP" runat="server" Enabled="False"></asp:CheckBox>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="Chk3HRP" runat="server" Enabled="False"></asp:CheckBox>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="Chk4HRP" runat="server" Enabled="False"></asp:CheckBox>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="Chk5HRP" runat="server" Enabled="False"></asp:CheckBox>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="Chk6HRP" runat="server" Enabled="False"></asp:CheckBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="middle" align="left">
                                                        <asp:Label ID="lblPrioridadCRPHotel" runat="server" CssClass="clslabel" EnableViewState="False">Prioridad de cancelacion:</asp:Label>
                                                    </td>
                                                    <td valign="middle" align="left">
                                                        <asp:Label ID="lblCancelacionHRP" runat="server" CssClass="clslabel" EnableViewState="False">Por Dias</asp:Label>
                                                    </td>
                                                    <td valign="middle" align="left">
                                                        <asp:TextBox ID="TextboxCHRP" runat="server" Width="40px" CssClass="textbox" ReadOnly="True"></asp:TextBox>
                                                    </td>
                                                    <td valign="middle" align="left">
                                                        <asp:Label ID="lblMsgCHRP" runat="server" CssClass="clslabel" EnableViewState="False">Dias antes del Check In</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="middle" align="center" colspan="4">
                                                        <asp:Label ID="lblMsgRatePlanHotel" runat="server" CssClass="Validators" EnableViewState="False">No tiene ninguna regla asignada</asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" align="left" height="50">
                                            <table id="TblRuleRatePLan" cellspacing="0" cellpadding="0" width="99%" border="0">
                                                <tr>
                                                    <td valign="middle" align="center" colspan="4" class="dgitem">
                                                        <asp:Label ID="Label2" runat="server" CssClass="bookingNormalLabel">Rate Plan</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="middle" align="left">
                                                        <asp:Label ID="LabelRP" runat="server" CssClass="clslabel" EnableViewState="False">Rate Plans</asp:Label>
                                                    </td>
                                                    <td valign="middle" align="left" colspan="3">
                                                        
                                                        <asp:DropDownList ID="ddlRatePlanConf" runat="server" Width="240px">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="middle" align="left">
                                                        <asp:Label ID="Label3" runat="server" CssClass="clslabel" EnableViewState="False"> Status:</asp:Label>
                                                    </td>
                                                    <td valign="middle" align="left" colspan="3">
                                                        <asp:Label ID="LabelStatRP" runat="server" CssClass="bookingNormalLabel" EnableViewState="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr width="100%">
                                                    <td valign="middle" align="left">
                                                        <asp:Label ID="lblMinNightsRP" runat="server" CssClass="clslabel" EnableViewState="False">Min. Nights</asp:Label>
                                                    </td>
                                                    <td valign="middle" align="left">
                                                        <asp:TextBox ID="txtMinNightsRP" runat="server" Width="40px" CssClass="textbox" ReadOnly="True"
                                                            MaxLength="3"></asp:TextBox>
                                                    </td>
                                                    <td valign="middle" align="left">
                                                        <asp:Label ID="lblMaxNightsRP" runat="server" CssClass="clslabel" EnableViewState="False">Max. Nights</asp:Label>
                                                    </td>
                                                    <td valign="middle" align="left">
                                                        <asp:TextBox ID="txtMaxNightsRP" runat="server" Width="40px" CssClass="textbox" ReadOnly="True"
                                                            MaxLength="3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="middle" align="left">
                                                        <asp:Label ID="lblAdvanceddaysRP" runat="server" CssClass="clslabel" EnableViewState="False">Advanced booking days:</asp:Label>
                                                    </td>
                                                    <td valign="middle" align="left">
                                                        <asp:TextBox ID="txtAdvancedDaysRP" runat="server" Width="40px" CssClass="textbox"
                                                            ReadOnly="True"></asp:TextBox>
                                                    </td>
                                                    <td valign="middle" align="left">
                                                    </td>
                                                    <td valign="middle" align="left">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblNoArrivalsRP" runat="server" CssClass="clslabel" EnableViewState="False">No Arrivals:</asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <table id="Table7" cellspacing="0" cellpadding="0" border="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lbldomingoRP" runat="server" CssClass="clsLabel" EnableViewState="False">Dom</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblLunesRP" runat="server" CssClass="clsLabel" EnableViewState="False">Lu</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblMartesRP" runat="server" CssClass="clsLabel" EnableViewState="False">Mar</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblMiercolesRP" runat="server" CssClass="clsLabel" EnableViewState="False">Mier</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblJuevesRP" runat="server" CssClass="clsLabel" EnableViewState="False">Jue</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblViernesRP" runat="server" CssClass="clsLabel" EnableViewState="False">Vi</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblSabadoRP" runat="server" CssClass="clsLabel" EnableViewState="False">Sab</asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="Chk7RP" runat="server" Enabled="False"></asp:CheckBox>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="Chk1RP" runat="server" Enabled="False"></asp:CheckBox>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="Chk2RP" runat="server" Enabled="False"></asp:CheckBox>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="Chk3RP" runat="server" Enabled="False"></asp:CheckBox>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="Chk4RP" runat="server" Enabled="False"></asp:CheckBox>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="Chk5RP" runat="server" Enabled="False"></asp:CheckBox>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="Chk6RP" runat="server" Enabled="False"></asp:CheckBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="middle" align="left">
                                                        <asp:Label ID="lblPrioridadCRP" runat="server" CssClass="clslabel" EnableViewState="False">Prioridad de cancelacion:</asp:Label>
                                                    </td>
                                                    <td valign="middle" align="left">
                                                        <asp:Label ID="lblCancelacionRP" runat="server" CssClass="clslabel" EnableViewState="False">Por Dias</asp:Label>
                                                    </td>
                                                    <td valign="middle" align="left">
                                                        <asp:TextBox ID="TextboxCRP" runat="server" Width="40px" CssClass="textbox" ReadOnly="True"></asp:TextBox>
                                                    </td>
                                                    <td valign="middle" align="left">
                                                        <asp:Label ID="lblMsgCRP" runat="server" CssClass="clslabel" EnableViewState="False">Dias antes del Check In</asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table id="TblNoRuleRatePLan" style="display: none" cellspacing="0" cellpadding="0"
                                                width="99%" border="0">
                                                <tr>
                                                    <td align="center" class="dgitem">
                                                        <asp:Label ID="Label4" runat="server" CssClass="bookingNormalLabel">Rate Plan</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 54px" align="center">
                                                        <asp:Label ID="lblMsgRP" runat="server" CssClass="Validators" EnableViewState="False">No existen reglas de bloqueo para RatePlans</asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td valign="top" align="center" height="50">
                                            <table id="TblRuleLockHotel" cellspacing="0" cellpadding="0" width="100%" border="0">
                                                <tr>
                                                    <td valign="middle" align="center" colspan="4" class="dgitem">
                                                        <asp:Label ID="lblLHotel" runat="server" CssClass="bookingNormalLabel">Hotel</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="middle" align="left">
                                                        <asp:Label ID="LabelStatusLH" runat="server" CssClass="clslabel" EnableViewState="False"> Status:</asp:Label>
                                                    </td>
                                                    <td valign="baseline" align="left" colspan="2">
                                                        <asp:Label ID="LabelStatLH" runat="server" CssClass="bookingNormalLabel" EnableViewState="False">Open</asp:Label>
                                                    </td>
                                                    <td valign="middle" align="left">
                                                    </td>
                                                </tr>
                                                <tr width="100%">
                                                    <td valign="middle" align="left">
                                                        <asp:Label ID="lblMinNightsLH" runat="server" CssClass="clslabel" EnableViewState="False">Min. Nights</asp:Label>
                                                    </td>
                                                    <td valign="middle" align="left">
                                                        <asp:TextBox ID="txtMinNightsLH" runat="server" Width="40px" CssClass="textbox" ReadOnly="True"
                                                            MaxLength="3"></asp:TextBox>
                                                    </td>
                                                    <td valign="baseline" align="left">
                                                        <asp:Label ID="lblMaxNightsLH" runat="server" CssClass="clslabel" EnableViewState="False">Max. Nights</asp:Label>
                                                    </td>
                                                    <td valign="middle" align="left">
                                                        <asp:TextBox ID="txtMaxNightsLH" runat="server" Width="40px" CssClass="textbox" ReadOnly="True"
                                                            MaxLength="3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="middle" align="left">
                                                        <asp:Label ID="lblAdvanceddaysLH" runat="server" CssClass="clslabel" EnableViewState="False">Advanced booking days:</asp:Label>
                                                    </td>
                                                    <td valign="middle" align="left">
                                                        <asp:TextBox ID="txtAdvancedDaysLH" runat="server" Width="40px" CssClass="textbox"
                                                            ReadOnly="True"></asp:TextBox>
                                                    </td>
                                                    <td valign="middle" align="left">
                                                    </td>
                                                    <td valign="middle" align="left">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblNoArrivalsLH" runat="server" CssClass="clslabel" EnableViewState="False">No Arrivals:</asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <table id="Table2" cellspacing="0" cellpadding="0" border="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lbldomingoLH" runat="server" CssClass="clsLabel" EnableViewState="False">Dom</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblLunesLH" runat="server" CssClass="clsLabel" EnableViewState="False">Lu</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblMartesLH" runat="server" CssClass="clsLabel" EnableViewState="False">Mar</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblMiercolesLH" runat="server" CssClass="clsLabel" EnableViewState="False">Mier</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblJuevesLH" runat="server" CssClass="clsLabel" EnableViewState="False">Jue</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblViernesLH" runat="server" CssClass="clsLabel" EnableViewState="False">Vi</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblSabadoLH" runat="server" CssClass="clsLabel" EnableViewState="False">Sab</asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="Chk7LH" runat="server" Enabled="False"></asp:CheckBox>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="Chk1LH" runat="server" Enabled="False"></asp:CheckBox>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="Chk2LH" runat="server" Enabled="False"></asp:CheckBox>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="Chk3LH" runat="server" Enabled="False"></asp:CheckBox>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="Chk4LH" runat="server" Enabled="False"></asp:CheckBox>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="Chk5LH" runat="server" Enabled="False"></asp:CheckBox>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="Chk6LH" runat="server" Enabled="False"></asp:CheckBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="middle" align="left">
                                                        <asp:Label ID="lblPrioridadCLH" runat="server" CssClass="clslabel" EnableViewState="False">Prioridad de cancelacion:</asp:Label>
                                                    </td>
                                                    <td valign="middle" align="left">
                                                        <asp:Label ID="lblCancelacionLH" runat="server" CssClass="clslabel" EnableViewState="False">Por Dias</asp:Label>
                                                    </td>
                                                    <td valign="middle" align="left">
                                                        <asp:TextBox ID="TextboxCLH" runat="server" Width="40px" CssClass="textbox" ReadOnly="True"></asp:TextBox>
                                                    </td>
                                                    <td valign="middle" align="left">
                                                        <asp:Label ID="lblMsgCLH" runat="server" CssClass="clslabel" EnableViewState="False">Dias antes del Check In</asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table id="TblNoRuleLockHotel" style="display: none" cellspacing="0" cellpadding="0"
                                                width="100%" border="0">
                                                <tr>
                                                    <td align="center" class="dgitem">
                                                        <asp:Label ID="Label5" runat="server" CssClass="bookingNormalLabel">Hotel</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 54px" align="center">
                                                        <asp:Label ID="lblMsgLH" runat="server" CssClass="Validators" EnableViewState="False">No existen reglas de bloqueo para el Hotel</asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="baseline" align="left" height="50">
                                            <table id="Table1" cellspacing="0" cellpadding="0" border="0" width="99%">
                                                <tr>
                                                    <td valign="middle" align="center" colspan="4" class="dgitem">
                                                        <asp:Label ID="lblTituloTarifasHabitacion" runat="server" CssClass="bookingNormalLabel">Tarifas de habitacion</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="middle" align="left">
                                                        <asp:Label ID="LabelRooms" runat="server" CssClass="clslabel" EnableViewState="False">Rooms</asp:Label>
                                                    </td>
                                                    <td valign="middle" align="left" colspan="3">
                                                        <asp:DropDownList ID="ddlRooms" runat="server" Width="240px">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="middle" align="left">
                                                        <asp:Label ID="LabelRatePlans" runat="server" CssClass="clslabel" EnableViewState="False">Rate Plans</asp:Label>
                                                    </td>
                                                    <td valign="middle" align="left" colspan="2">
                                                        <asp:DropDownList ID="ddlRatePlansRooms" runat="server" Width="240px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <input class="button" id="ButtonLoadTarifasHabitacion" onclick="cancelRule('ctlMensajeRuleConf1_PnlBox','ctlMensajeRuleConf1_TblPnl')"
                                                            type="button" value="Load" name="btnCancel" runat="server" width="60px">
                                                    </td>
                                                </tr>
                                                <tr width="100%">
                                                    <td valign="middle" align="left">
                                                        <asp:Label ID="lblMinNightsTH" runat="server" CssClass="clslabel" EnableViewState="False">Min. Nights</asp:Label>
                                                    </td>
                                                    <td valign="middle" align="left">
                                                        <asp:TextBox ID="txtMinNightsTH" runat="server" Width="40px" CssClass="textbox" ReadOnly="True"
                                                            MaxLength="3"></asp:TextBox>
                                                    </td>
                                                    <td valign="middle" align="left">
                                                        <asp:Label ID="lblMaxNightsTH" runat="server" CssClass="clslabel" EnableViewState="False">Max. Nights</asp:Label>
                                                    </td>
                                                    <td valign="middle" align="left">
                                                        <asp:TextBox ID="txtMaxNightsTH" runat="server" Width="40px" CssClass="textbox" ReadOnly="True"
                                                            MaxLength="3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="middle" align="left">
                                                        <asp:Label ID="lblAdvanceddaysTH" runat="server" CssClass="clslabel" EnableViewState="False">Advanced booking days:</asp:Label>
                                                    </td>
                                                    <td valign="middle" align="left">
                                                        <asp:TextBox ID="txtAdvancedDaysTH" runat="server" Width="40px" CssClass="textbox"
                                                            ReadOnly="True"></asp:TextBox>
                                                    </td>
                                                    <td valign="middle" align="left">
                                                    </td>
                                                    <td valign="middle" align="left">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblNoArrivalsTH" runat="server" CssClass="clslabel" EnableViewState="False">No Arrivals:</asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <table id="Table4" cellspacing="0" cellpadding="0" border="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lbldomingoTH" runat="server" CssClass="clsLabel" EnableViewState="False">Dom</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblLunesTH" runat="server" CssClass="clsLabel" EnableViewState="False">Lu</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblMartesTH" runat="server" CssClass="clsLabel" EnableViewState="False">Mar</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblMiercolesTH" runat="server" CssClass="clsLabel" EnableViewState="False">Mier</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblJuevesTH" runat="server" CssClass="clsLabel" EnableViewState="False">Jue</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblViernesTH" runat="server" CssClass="clsLabel" EnableViewState="False">Vi</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblSabadoTH" runat="server" CssClass="clsLabel" EnableViewState="False">Sab</asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="Chk7TH" runat="server" Enabled="False"></asp:CheckBox>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="Chk1TH" runat="server" Enabled="False"></asp:CheckBox>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="Chk2TH" runat="server" Enabled="False"></asp:CheckBox>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="Chk3TH" runat="server" Enabled="False"></asp:CheckBox>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="Chk4TH" runat="server" Enabled="False"></asp:CheckBox>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="Chk5TH" runat="server" Enabled="False"></asp:CheckBox>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="Chk6TH" runat="server" Enabled="False"></asp:CheckBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="middle" align="center" colspan="4">
                                                        <asp:Label ID="lblMsgRateRoom" runat="server" CssClass="Validators" EnableViewState="False">No tiene ninguna regla asignada</asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td valign="top" align="center" height="50">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="2">
                                            <input class="button" id="btnCancel" onclick="cancel('ctlMensajeRuleConf1_PnlBox','ctlMensajeRuleConf1_TblPnl')"
                                                type="button" value="Cancelar" name="btnCancel" runat="server" width="60px">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Panel>
