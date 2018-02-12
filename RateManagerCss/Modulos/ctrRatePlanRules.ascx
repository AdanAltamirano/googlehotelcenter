<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ctrRatePlanRules.ascx.vb" Inherits="RateManager.ctrRatePlanRules" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="uc1" TagName="CtrlIdioma" Src="CtrlIdioma.ascx" %>
<%@ Import Namespace="RateManager" %>

<script type="text/javascript">

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

        $('#<%= Me.chkRateRules.ClientId %>').click(function () {
            EnableValidators_<%= Me.txtCancelPolitiesReview.Id %>(!this.checked);
            EnableValidators_<%= Me.txtCancelPolitiesFull.Id %>(!this.checked);
            EnableValidators_<%= Me.txtGuaranteePolicy.Id %>(!this.checked);
            EnableValidators_<%= Me.txtPolicyCreditCard.Id %>(!this.checked);
        });

        $('#<%= Me.ddlHour.ClientId %>, #<%= Me.ddlMinutes.ClientId %>, #<%= Me.txtCancellationPolicy.ClientId %>').change(function () {
            showTimeChangedNotify = true;
        });

        $('#<%= Me.ddlCancelationPolicy.ClientId %>').change(function () {
            showTimeChangedNotify = true;

            $('#<%= Me.ddlHour.ClientId %>, #<%= Me.lblSep.ClientId %>, #<%= Me.ddlMinutes.ClientId %>, #<%= Me.txtCancellationPolicy.ClientId %>, #<%= Me.lblAux.ClientId %>, #<%= Me.lblEDaysHour.ClientId %>').hide();
            if ($(this).attr('selectedIndex') > 0) {
                $('#<%= Me.lblAux.ClientId %>, #<%= Me.lblEDaysHour.ClientId %>').show();

                if ($(this).attr('selectedIndex') == 1 || $(this).attr('selectedIndex') == 2) {
                    $('#<%= Me.txtCancellationPolicy.ClientId %>').show();
                    $('#<%= Me.lblAux.ClientId %>').html('<%= PortalCulture.GetString("00413") %>');
                    if ($(this).attr('selectedIndex') == 1) {
                        $('#<%= Me.lblEDaysHour.ClientId %>').html('<%= PortalCulture.GetString("00410") %>');
                    } else {
                        $('#<%= Me.lblEDaysHour.ClientId %>').html('<%= PortalCulture.GetString("00409") %>');
                    }
                } else if ($(this).attr('selectedIndex') == 3) {
                    $('#<%= Me.ddlHour.ClientId %>, #<%= Me.ddlMinutes.ClientId %>, #<%= Me.lblSep.ClientId %>').show();
                    $('#<%= Me.lblAux.ClientId %>').html('<%= PortalCulture.GetString("00412") %>');
                    $('#<%= Me.lblEDaysHour.ClientId %>').html('<%= PortalCulture.GetString("00411") %>');
                }
        }

        });
    });

</script>

<style type="text/css">
    .description.token {
        padding: 10px 15px;
        font-style: italic;
    }

        .description.token b {
            font-style: normal;
        }
</style>

<table id="Table1" cellspacing="1" cellpadding="0" width="100%" align="center" border="0" class="Form">
    <tr>
        <td align="right">
            <asp:Label ID="lblEDescripcion" runat="server" EnableViewState="False" CssClass="clsLabel">Description:</asp:Label></td>
        <td>
            <asp:TextBox ID="txtDescription" runat="server" CssClass="textbox" MaxLength="20" Columns="20"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="VALIDATORS" Display="Dynamic"
                ControlToValidate="txtDescription" ErrorMessage="*"></asp:RequiredFieldValidator></td>
        <td align="right">
            <asp:Label ID="lblERateRules" runat="server" EnableViewState="False" CssClass="clsLabel">Rate Rules</asp:Label></td>
        <td>
            <asp:CheckBox ID="chkRateRules" runat="server" CssClass="clslabel" Text="Use Default"></asp:CheckBox></td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label ID="lblEVerReq" runat="server" EnableViewState="False" CssClass="clslabel">ID Verification Required</asp:Label></td>
        <td>
            <asp:DropDownList ID="ddlVerReq" runat="server"></asp:DropDownList></td>
        <td align="right">
            <asp:Label ID="lblERequired" runat="server" EnableViewState="False" CssClass="clsLabel">Required to book:</asp:Label></td>
        <td>
            <asp:DropDownList ID="ddlGuarDep" runat="server"></asp:DropDownList></td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label ID="lblMinNights" runat="server" EnableViewState="False" CssClass="clslabel">Min. Nights</asp:Label></td>
        <td>
            <asp:TextBox ID="txtMinNights" runat="server" CssClass="textbox" MaxLength="3" Width="40px"></asp:TextBox><asp:RangeValidator ID="RVMinNights" runat="server" CssClass="validators" Display="Dynamic" ControlToValidate="txtMinNights"
                ErrorMessage="1-999" MaximumValue="999" MinimumValue="1" Type="Integer"></asp:RangeValidator></td>
        <td align="right">
            <asp:Label ID="lblMaxNights" runat="server" EnableViewState="False" CssClass="clslabel">Max. Nights</asp:Label></td>
        <td>
            <asp:TextBox ID="txtMaxNights" runat="server" CssClass="textbox" MaxLength="3" Width="40px"></asp:TextBox><asp:RangeValidator ID="RVMaxDays" runat="server" CssClass="validators" Display="Dynamic" ControlToValidate="txtMaxNights"
                ErrorMessage="0-250" MaximumValue="250" MinimumValue="0" Type="Integer"></asp:RangeValidator></td>
    </tr>
    <tr>
        <td align="right" style="HEIGHT: 59px">
            <asp:Label ID="lblAdvanceddays" runat="server" EnableViewState="False" CssClass="clslabel">Advanced booking days:</asp:Label></td>
        <td style="HEIGHT: 59px">
            <span style="margin-left: 10px">Min: </span>
            <asp:TextBox ID="txtAdvancedDays" runat="server" CssClass="textbox" Width="40px"></asp:TextBox>
            <asp:RangeValidator ID="RVAdvanced" runat="server" CssClass="validators" Display="Dynamic" ControlToValidate="txtAdvancedDays" ErrorMessage="0-999" MaximumValue="999" MinimumValue="0" Type="Integer"></asp:RangeValidator>
            <span style="margin-left: 10px">Max: </span>
            <asp:TextBox ID="txtMaxAdvancedDays" runat="server" CssClass="textbox" Width="40px"></asp:TextBox>
        </td>
        <td align="right" style="HEIGHT: 59px">
            <asp:Label ID="lblNoArrivals" runat="server" EnableViewState="False" CssClass="clslabel">No Arrivals:</asp:Label></td>
        <td style="HEIGHT: 58px">
            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                <tr>
                    <td>
                        <asp:Label ID="lbldomingo" runat="server" EnableViewState="False" CssClass="clsLabel">Dom</asp:Label></td>
                    <td>
                        <asp:Label ID="lblLunes" runat="server" EnableViewState="False" CssClass="clsLabel">Lu</asp:Label></td>
                    <td>
                        <asp:Label ID="lblMartes" runat="server" EnableViewState="False" CssClass="clsLabel">Mar</asp:Label></td>
                    <td>
                        <asp:Label ID="lblMiercoles" runat="server" EnableViewState="False" CssClass="clsLabel">Mier</asp:Label></td>
                    <td>
                        <asp:Label ID="lblJueves" runat="server" EnableViewState="False" CssClass="clsLabel">Jue</asp:Label></td>
                    <td>
                        <asp:Label ID="lblViernes" runat="server" EnableViewState="False" CssClass="clsLabel">Vi</asp:Label></td>
                    <td>
                        <asp:Label ID="lblSabado" runat="server" EnableViewState="False" CssClass="clsLabel">Sab</asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="Chk7" runat="server"></asp:CheckBox></td>
                    <td>
                        <asp:CheckBox ID="Chk1" runat="server"></asp:CheckBox></td>
                    <td>
                        <asp:CheckBox ID="Chk2" runat="server"></asp:CheckBox></td>
                    <td>
                        <asp:CheckBox ID="Chk3" runat="server"></asp:CheckBox></td>
                    <td>
                        <asp:CheckBox ID="Chk4" runat="server"></asp:CheckBox></td>
                    <td>
                        <asp:CheckBox ID="Chk5" runat="server"></asp:CheckBox></td>
                    <td>
                        <asp:CheckBox ID="Chk6" runat="server"></asp:CheckBox></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="dgItem" align="center" colspan="4">
            <asp:Label ID="lbltitlepolity" runat="server" EnableViewState="False" CssClass="clsdarklabel">Cancellation Policy:</asp:Label></td>
    </tr>
    <tr>
        <td colspan="4">
            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                <tr>
                    <td align="right">
                        <asp:Label ID="lblCancel" runat="server" EnableViewState="False" CssClass="clsLabel">Cancell</asp:Label></td>
                    <td colspan="3">
                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                            <tr>
                                <td align="left" width="20%">
                                    <asp:DropDownList ID="ddlCancelationPolicy" runat="server"></asp:DropDownList></td>
                                <td align="center" width="25%">
                                    <asp:Label ID="lblAux" runat="server"></asp:Label>
                                </td>
                                <td align="left" width="25%">
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
                                </td>
                                <td align="left" width="30%">
                                    <input type="hidden" id="varCancelationTime" runat="server" value="" />
                                    <asp:Label ID="lblEDaysHour" runat="server" CssClass="clslabel">Days / Hours</asp:Label>
                                    <asp:Label ID="lblErrorHours" runat="server" CssClass="validators" Visible="False"></asp:Label>
                                    <asp:RangeValidator ID="RVCancelation" runat="server" CssClass="validators" Display="Dynamic" ControlToValidate="txtCancellationPolicy"
                                        ErrorMessage="0-999" MaximumValue="9999" MinimumValue="0"></asp:RangeValidator>

                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:CheckBox runat="server" ID="chkNonCancelable" Text="No Cancelable" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" class="description token">
                        <%=PortalCulture.GetString("01356")%>
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="right">
                        <asp:Label ID="lblCancelPolitiesReview" runat="server" EnableViewState="False" CssClass="clsLabel">Políticas de Cancelación Review</asp:Label></td>
                    <td colspan="3">
                        <uc1:CtrlIdioma ID="txtCancelPolitiesReview" runat="server" RequiredText="true"></uc1:CtrlIdioma>
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="right" style="HEIGHT: 24px">
                        <asp:Label ID="lblCancelPolitiesFull" runat="server" EnableViewState="False" CssClass="clsLabel">Políticas de Cancelación Full</asp:Label></td>
                    <td colspan="3" style="HEIGHT: 24px">
                        <p>
                            <uc1:CtrlIdioma ID="txtCancelPolitiesFull" runat="server" RequiredText="true"></uc1:CtrlIdioma>
                        </p>
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="center" colspan="4" class="dgItem">
                        <asp:Label ID="lblTtitleGuarantee" EnableViewState="False" runat="server" CssClass="clsdarklabel">Guarantee Policy:</asp:Label></td>
                </tr>
                <tr>
                    <td style="HEIGHT: 19px" valign="top" align="right">
                        <asp:Label ID="lblGuaranteePolicy" EnableViewState="False" runat="server" CssClass="clsLabel"> Políticas de Garantia</asp:Label></td>
                    <td style="HEIGHT: 19px" colspan="3">
                        <uc1:CtrlIdioma ID="txtGuaranteePolicy" runat="server" RequiredText="true"></uc1:CtrlIdioma>
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="right">
                        <asp:Label ID="lblPolicyCreditCard" EnableViewState="False" runat="server" CssClass="clsLabel">Políticas de Tarjeta de Crédito</asp:Label></td>
                    <td colspan="3">
                        <uc1:CtrlIdioma ID="txtPolicyCreditCard" runat="server" RequiredText="true"></uc1:CtrlIdioma>
                    </td>
                </tr>
                <% If Me.edicion AndAlso (Not Me.txtCancelPolitiesFull.Published OrElse Not Me.txtCancelPolitiesReview.Published OrElse Not Me.txtGuaranteePolicy.Published OrElse Not Me.txtPolicyCreditCard.Published) Then%>
                <tr>
                    <td colspan="4">
                        <span class="validators"><%=RateManager.PortalCulture.GetString(If(CType(Me.Page, RateManager.PaginaBase).IsSupervisor, "01365", "01392"))%></span>
                    </td>
                </tr>
                <% End If%>
            </table>
        </td>
    </tr>
</table>

