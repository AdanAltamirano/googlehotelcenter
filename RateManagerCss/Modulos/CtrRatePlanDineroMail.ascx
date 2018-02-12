<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CtrRatePlanDineroMail.ascx.vb"
    Inherits="RateManager.CtrRatePlanDineroMail" %>

<script>
    function FireShowDineroMail(id, div) {
        var e = document.getElementById(id);
        var p = document.getElementById(div);
        if (e && p) {
            p.style.display = e.checked ? 'block' : 'none';
        }
    }
    function FireChkList(id, chk) {
        var e = document.getElementById(id);
        var ck = document.getElementById(chk);
        var list = e.getElementsByTagName("input");
        var isChk = false;

        for (var i = 0; i <= list.length - 1; i++) {
            if (list[i].type == 'checkbox') {
                if (list[i].checked) {
                    isChk = true;
                    break;
                }
            }
        }
        if (ck) ck.checked = isChk;
    }

    function FireChkListAll(id, chklist) {
        var e = document.getElementById(id);
        var cl = document.getElementById(chklist);
        var list = cl.getElementsByTagName("input");

        for (var i = 0; i <= list.length - 1; i++) {
            if (list[i].type == 'checkbox') {
                list[i].checked = e.checked
            }
        }
    }

    function ValidaCuentaDinero(id, chk) {
        //        var ck = document.getElementById(chk);
        //        var e = document.getElementById(id);
        //        if (ck) {
        //            if (ck.checked) {
        //                if (e.innerHTML == '')
        //                    alert('vacio');
        //            }
        //        }
        return true;
    }    
    
</script>

<div id="divContentDineroMail" runat="server" >
    <table border="0" cellspacing="0" cellpadding="0" width =100%>
        
        <tr>
            <td align="right">
                <asp:Label ID="lblCuenta" runat="server" Text="Cuenta No:" EnableViewState="False"></asp:Label>
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtCta" runat="server" MaxLength="30" Style="width: 220px;" Width="190px" cssclass="TextBox"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td colspan="2">
                <asp:RequiredFieldValidator ID="rfvCuenta" runat="server" ErrorMessage="*"
                    ControlToValidate="txtCta" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:CheckBox ID="chkTC" runat="server" CssClass="clsLabel" Text="Tarjeta de Credito" >
                </asp:CheckBox>
            </td>
            <td>
                <asp:CheckBoxList ID="chkListTC" runat="server" RepeatDirection="Horizontal" style="width: auto !important">
                </asp:CheckBoxList>
            </td>
        </tr>
        <tr>
            <td colspan="2">
            </td>
            <td>
                <table cellpadding="0" cellspacing="0" style="width: auto !important">
                    <tr>
                        <td style="padding-left: 14px; height: 20px">
                            <span class="bookingNormalLabel">1</span>
                        </td>
                        <td style="padding-left: 34px; height: 20px">
                            <span class="bookingNormalLabel">3</span>
                        </td>
                        <td style="padding-left: 30px; height: 20px">
                            <span class="bookingNormalLabel">6</span>
                        </td>
                        <td style="padding-left: 28px; height: 20px">
                            <span class="bookingNormalLabel">12</span>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">
            </td>
            <td>
                <asp:Label ID="lblNota" runat="server" Text="Las opciones de 3,6,12 meses" Style="font-size: 10px;"
                    EnableViewState="False"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:CheckBox ID="chkAmex" runat="server" CssClass="clsLabel" Text="Tarjeta American Express" style="width:auto !important">
                </asp:CheckBox>
            </td>
            <td>
                <asp:CheckBoxList ID="chkListAmex" runat="server" RepeatDirection="Horizontal" style="width:auto !important">
                </asp:CheckBoxList>
            </td>
        </tr>
        <tr>
            <td colspan="2">
            </td>
            <td>
                <table cellpadding="0" cellspacing="0" style="width: auto !important">
                    <tr>
                        <td style="padding-left: 14px; height: 20px">
                           <span class="bookingNormalLabel">1</span>
                        </td>
                        <td style="padding-left: 34px;  height: 20px">
                           <span class="bookingNormalLabel">3</span>
                        </td>
                        <td style="padding-left: 30px;  height: 20px">
                            <span class="bookingNormalLabel">6</span>
                        </td>
                        <td style="padding-left: 28px;  height: 20px">
                            <span class="bookingNormalLabel">12</span>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:CheckBox ID="chkOxxo" runat="server" CssClass="clsLabel" Text="Pago Tiendas Oxxo">
                </asp:CheckBox>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:CheckBox ID="chkEleven" runat="server" CssClass="clsLabel" Text="Pago Tiendas 7Eleven">
                </asp:CheckBox>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:CheckBox ID="chkTL" runat="server" CssClass="clsLabel" Text="Transferencia Banca en Linea">
                </asp:CheckBox>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:CheckBox ID="chkEfectivo" runat="server" CssClass="clsLabel" Text="Medio En Efectivo">
                </asp:CheckBox>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:CheckBox ID="chkDineroMail" runat="server" CssClass="clsLabel" Text="Fondos DineroMail">
                </asp:CheckBox>
            </td>
            <td>
            </td>
        </tr>
    </table>
</div>
