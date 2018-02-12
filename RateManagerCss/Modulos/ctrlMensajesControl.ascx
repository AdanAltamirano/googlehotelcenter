<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ctrlMensajesControl.ascx.vb"
    Inherits="RateManager.ctrlMensajesControl" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>

<script src="<%= Me.ResolveUrl("~/Pages/Scripts/jquery.min.js") %>" type="text/javascript"></script>
<script>    
    function cancel(pnl1, pnl2) {
        document.getElementById(pnl1).style.display = 'none';
        document.getElementById(pnl2).style.display = 'none';
        var arrSelects = document.getElementsByTagName('SELECT');
        for (var i = 0; i < arrSelects.length; i++)
        { arrSelects[i].style.display = ''; }

    }

    function ok(pnl1, pnl2) {
        var txt = document.getElementById('txtIdObj');
        document.getElementById(txt.Value).click();

    }
    function show(pnl1, pnl2, obj) {
        document.getElementById(pnl1).style.display = "block";
        document.getElementById(pnl2).style.display = "block";
        var arrSelects = document.getElementsByTagName('SELECT');
        for (var i = 0; i < arrSelects.length; i++)
        { arrSelects[i].style.display = 'None'; }
        var txt = document.getElementById('txtIdObj');
        txt.Value = obj;
        resizeIframe('frmPrincipal');

    }
</script>

<div id="dvpopup">
    <input id="txtIdObj" type="hidden" name="txtIdObj">
    <asp:Panel ID="TblPnl" Style="display: none; z-index: 997; filter: alpha(opacity=25); left: 150px; position: fixed; top: 0px; background-color: whitesmoke;
        moz-opacity: .25; opacity: .25" Height="100%" Width="100%" runat="server">
    </asp:Panel>
    <asp:Panel ID="PnlBox" runat="server">
        <table height="100%" cellspacing="0" cellpadding="0" width="100%" align="center"
            border="0">
            <tr>
                <td valign="middle">
                    <table id="bookingcontainer" height="100" cellspacing="1" cellpadding="0" width="300"
                        align="center" border="0">
                        <tr>
                            <td class="titulo" align="center">
                                <asp:Label ID="lblTitle" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td valign="middle" align="center" height="50">
                                <table id="TblMai" cellspacing="0" cellpadding="5" width="300" border="0">
                                    <tr>
                                        <td valign="middle" align="center" width="5">
                                            <asp:Image ID="img" runat="server" Visible="False"></asp:Image>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblPrompt" runat="server" CssClass="clsDarkLabel"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Panel ID="pnlPromptInput" runat="server">
                                    <table id="Table1" cellspacing="1" cellpadding="1" width="100%" border="0">
                                        <tr>
                                            <td align="right" width="50%">
                                                <input class="button" id="btnOk" onclick="javascript:ok('TblPnl','PnlBox')" type="button"
                                                    value="Aceptar" name="btnOk" runat="server" width="60px">
                                            </td>
                                            <td align="left" width="50%">
                                                <input class="button" id="btnCancel" type="button" value="Cancelar" name="btnCancel"
                                                    runat="server" width="60px">
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>
</div>
