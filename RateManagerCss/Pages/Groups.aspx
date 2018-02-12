<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Groups.aspx.vb" Inherits="RateManager.Groups" %>

<%@ Import Namespace="RateManager" %>
<%@ Register TagPrefix="uc1" TagName="ctlMensajes" Src="../Modulos/ctlMensajes.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlPlanFares" Src="../Modulos/ctrlPlanFares.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrlPlanFaresExc" Src="../Modulos/CtrlPlanFaresExc.ascx" %>
<%@ Register TagPrefix="uc2" TagName="ctrlAutoComplete" Src="../Modulos/ctrlAutoComplete.ascx"   %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">


<head runat="server">
    <title></title>
    <link href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet" />

    
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.5/jquery.min.js"></script>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js"></script>
    <script type="text/javascript" src="../Includes/Script/JsSearch-1.0.js"></script>
    
    <script type="text/javascript" language="javascript">
        $ = jQuery.noConflict();
        jQuery(document).ready(function() {

        var list = jQuery('#<%= Me.ddlShowRates.ClientId %>');

            list.change(function() {
                var validators = new Array();
                validators.push({ id: '<%= Me.reqAdultFare.ClientID %>', hide: true });
                validators.push({ id: '<%= Me.reqChildFare.ClientID %>', hide: true });
                validators.push({ id: '<%= Me.reqTeenFare.ClientID %>', hide: true });
                validators.push({ id: '<%= Me.reqExtraAdultPrice.ClientID %>', hide: false });
                validators.push({ id: '<%= Me.reqExtraChildPrice.ClientID %>', hide: false });
                validators.push({ id: '<%= Me.reqExtraTeenPrice.ClientID %>', hide: false });

                //FireShowSelectRates(this, 'pnlTarifas', 'DivRates', validators);
            });
            //list.find('option:selected').removeAttr('selected');
            //list.find('option:eq(' + rateModeView.toString() + ')').attr('selected', 'selected');
            list.change();

            var gridID = $("#<%= hidGridLocked.ClientID%>").val();
            if (!(gridID===undefined)&& gridID != "") {
                //alert(gridID);
                showRatesInput(gridID);

            }
            $('span.currency').html('"<%=Currency %>"');
        });


        function FireShowSelectRates(ddl, rate, ocupa, validators) {
            if (ddl) {
                var normalValidate = false;
                if (ddl.selectedIndex == 0) {
                    $('#' + rate).css('display', 'inline');
                    $('#' + ocupa).hide();
                    normalValidate = true;
                } else {
                    $('#' + rate).css('display', 'none');
                    $('#' + ocupa).show();
                }

                $.each(validators, function() {
                    var items = $('#' + this.id)
                    if (this.hide && items.length > 0)
                        ValidatorEnable(items[0], normalValidate);
                    items.hide();
                })
            }
        }

        function optionSw(e) {
            var div1A;
            var div2B;
            var td1A;
            var td2B;

            var txt;
            txt = document.getElementById('txtDivP');
            txt.value = e;

            div1A = document.getElementById('divA2');
            div2B = document.getElementById('divB2');
            td1A = document.getElementById('TdPricing');
            td1B = document.getElementById('TdPricingE');
            switch (e) {
                case '1P':

                    div1A.style.display = 'block';
                    div2B.style.display = "none";

                    td1A.className = 'tabselected';
                    td1B.className = 'tab';
                    break;
                case '1E':

                    div1A.style.display = "none";
                    div2B.style.display = 'block';


                    td1A.className = 'tab';
                    td1B.className = 'tabselected';
                    break;
            }
            return true;
        }

        function FillPrices(Dg, typeFare, Price) {
            var P = document.getElementById(Price);
            var grid = document.getElementById(Dg);
            var item = grid.getElementsByTagName("tr");

            for (var i = 1; i < item.length; i++) {
                var txt = item[i].getElementsByTagName("input");
                var lbl = item[i].getElementsByTagName("span");

                for (var j = 0; j < lbl.length; j++) {
                    for (var t = 0; t < txt.length; t++) {
                        if (txt[t].id.indexOf(typeFare) != -1) {
                            txt[t].value = P.value;
                        }
                    }

                    if (lbl[j].id.indexOf("lblTotal") != -1) {
                        if (txt.length > 1) {
                            sP(txt[0].id, txt[1].id, lbl[j].id);
                        }
                        else {
                            sP(txt[0].id, '', lbl[j].id);
                        }
                    }
                }
            }
        }

        function showRatesInput(id) {

            var grid = $("#" + id);

            var inputs = $("#inputRates");
            inputs.css("display", "block");
            inputs.css("width", inputs.width() + 7);
            inputs.css("position", "absolute");
            inputs.css("top", grid.offset().top + grid[0].offsetHeight + 1);
            inputs.css("left", "0");
            //inputs.offset({ top: grid.offset().top + grid.height(), left: 0 });
        }

        function linkDel_Click() {
            $(window.parent.document).find('html, body').animate({ scrollTop: 0 }, 'slow');
        }

        SearchStart.AddParam
	    (
		    {
		        searchitems: [
			        { Item: 'Groups', IDSearch: 'IdConvenio', nameSearch: 'Codigo', isdefault: false },
			        { Item: 'Groups', IDSearch: 'IdConvenio', nameSearch: 'Nombre', isdefault: true }
			    ],
		        colModel: [
			        { display: '<%= RateManager.PortalCulture.GetString("00001") %>' }, { display: '<%= RateManager.PortalCulture.GetString("M000144") %>' },
			    ],
			        Data: [{ catalogo: 'Groups',
			        idCorporate: '<%= idCorporate%>'
                    }],
                    id: 'Groups',
		            index: 1
		        }
	    );
    
    </script>

    <style type="text/css">
        .tableRatesAgreement tr.DATAGRIDIN TD
        {
            color: #286FBF !important;
            font-weight: bold !important;
            border: 1px dashed gray;
        }
        /*table.datagrid .tableRatesAgreement TD
    {}*/</style>
</head>
<body>
    <form id="form1" runat="server">
    <iframe id="gToday:normal:agenda.js" style="z-index: 999; left: -500px; position: absolute;
        top: -500px" name="gToday:normal:agenda.js" src='<%=GeRequestApplicationPath(string.concat("/Calendar/",portalculture.getculture().Name.Substring(0, 2).ToLower(),"/ipopeng2.htm" )) %>'
        frameborder="0" width="174" scrolling="no" height="189"></iframe>
    <div>
        <div class="clear">
            <div class="mDiv">
            </div>
            <div>
                <asp:Label ID="lblTitleGroups" runat="server" EnableViewState="False" Text="Grupos"
                    CssClass="tituloSeccion" Visible="False"></asp:Label>
            </div>
            <table id="BookingContainer" cellpadding="2" cellspacing="0" border="0" align="center"
                width="96%">
                <tr id="trCorporates" runat="server">
                    <td>
                        <table align="center" cellpadding="0" cellspacing="0" border="0" style="margin-top: 20px;
                            width: 100%">
                            <tr>
                                <td align="left">
                                    <asp:Label ID="lblCorporates" runat="server" Text="Corporativo" CssClass="clslabel"
                                        EnableViewState="False"></asp:Label>
                                    <asp:DataGrid ID="dgCorporates" runat="server" CssClass="DataGrid" AutoGenerateColumns="False"
                                        AllowPaging="True" ShowFooter="True" PageSize="25">
                                        <FooterStyle HorizontalAlign="Right"></FooterStyle>
                                        <SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
                                        <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
                                        <ItemStyle CssClass="dgItem"></ItemStyle>
                                        <HeaderStyle CssClass="dgHeader"></HeaderStyle>
                                        <Columns>
                                            <asp:BoundColumn DataField="idCorporativo" HeaderText="ID"></asp:BoundColumn>
                                            <asp:TemplateColumn>
                                                <ItemTemplate>
                                                    <asp:Label ID="CorporateName" runat="server" Text='<%# Container.dataItem("NombreCorp") %>'
                                                        EnableViewState="False"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkSelectCorporate" runat="server" CausesValidation="False" CssClass="dgLink"
                                                        CommandName="Select" CommandArgument='<%# Container.dataItem("idCorporativo").toString() + "|" + Container.dataItem("NombreCorp") %>'
                                                        Text='<%# PortalCulture.GetString("M000640") %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                        </Columns>
                                        <PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
                                            Position="Bottom" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
                                    </asp:DataGrid>
                                    <asp:HiddenField ID="idSelectedCorpororate" runat="server" Visible="false" />
                                    <asp:HiddenField ID="idSelectedCorpororateName" runat="server" Visible="false" />
                                    <div style="height: 80px;">
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                        <center>
                            <div id="divMainGroup" runat="server" style="width: 1000px;" visible="false">
                                <a id="aTop"></a>
                                <table border="0" width="100%">
                                    <tr>
                                        <td colspan="3" align="left">
                                            <asp:Label ID="lblTitleGroupData" runat="server" Text="lblTitleCorporate" CssClass="bookingNormalLabel"></asp:Label>
                                            <asp:Button ID="btnSelectCorporate" runat="server" CssClass="lnkButton" Text="Seleccionar Corporativo"
                                                Visible="false" ValidationGroup="Corporate" /><br />
                                            <br />
                                            <hr />
                                            <uc2:ctrlAutoComplete ID="ctrlAutoComplete1" runat="server" />
                                            <asp:Label ID="lblGroup" runat="server" Text="Grupos" CssClass="tituloSeccion" EnableViewState="False"></asp:Label>
                                            <asp:Button ID="btnNewGroup" runat="server" class="ButtonNew" Style="float: right;"
                                                Text="Nuevo" CausesValidation="false"></asp:Button>
                                            
                                            <br />
                                            <br />
                                            <hr />
                                            <asp:DataGrid ID="dgConvenio" runat="server" CssClass="DataGrid" AutoGenerateColumns="False"
                                                    AllowPaging="True" ShowFooter="true" PageSize="10">
                                                    <SelectedItemStyle BackColor="#DDDDFF" BorderColor="#9999FF" BorderWidth="1"></SelectedItemStyle>
                                                    <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
                                                    <ItemStyle CssClass="dgItem"></ItemStyle>
                                                    <HeaderStyle CssClass="dgHeader"></HeaderStyle>
                                                    <Columns>
                                                       <asp:BoundColumn DataField="IdConvenio" HeaderText="ID" HeaderStyle-Width="15%"/>
                                                       <asp:BoundColumn DataField="Codigo" HeaderText="Codigo"  />
                                                       <asp:BoundColumn DataField="Nombre" HeaderText="Nombre"  />
                                                       <asp:BoundColumn DataField="Descripcion" Visible="false"  />
                                                        <asp:TemplateColumn ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkSelectCorporate" runat="server" CausesValidation="False" CssClass="dgLink"
                                                                    CommandName="Select" Text='<%# PortalCulture.GetString("M000640") %>'></asp:LinkButton>
                                                            </ItemTemplate>
                                                       </asp:TemplateColumn>
                                                       <asp:TemplateColumn ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="11%">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkEdit" runat="server" Text="" CommandName="Edit" CausesValidation="False"
                                                                    CssClass="dgLink"></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:TemplateColumn>
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkEliminar2" Style="display: none" runat="server" CssClass="dgLink"
                                                                    CausesValidation="False" CommandName="Delete">-</asp:LinkButton>
                                                                <asp:HyperLink ID="lnkEliminar" runat="server" CssClass="dgLink" onclick="linkDel_Click()"></asp:HyperLink>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                    </Columns>                                                    
                                                    <PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
                                                        Position="Bottom" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
                                                    </asp:DataGrid>
                                            <br />
                                            <br />
                                            <asp:Label Font-Size="16" ID="lblGroupSubtitle" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <table border="0" id="tabEditGroup" runat="server" width="100%" visible="false">
                                    <tr>
                                        <td colspan="3" align="left">
                                            <br />
                                            <br />
                                            <asp:Label ID="lblTitleGeneralInformation" runat="server" Text="Datos Generales"
                                                CssClass="bookingNormalLabel" EnableViewState="False"></asp:Label>
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="3">
                                            <asp:Label ID="lblCode" runat="server" Text="Codigo" CssClass="clslabel" EnableViewState="False"></asp:Label>
                                            <br />
                                            <asp:TextBox ID="txtCode" runat="server" Width="150px" MaxLength="20" TabIndex="2"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvCode" runat="server" ErrorMessage="Campo requerido"
                                                ValidationGroup="Group" ControlToValidate="txtCode" EnableViewState="false"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <table border="0" align="center" width="100%" id="pnlMainForm">
                                                <tr>
                                                    <td valign="top" align="left">
                                                        <asp:Label ID="lblName" runat="server" Text="Nombre" CssClass="clslabel" EnableViewState="False"></asp:Label>
                                                        <br />
                                                        <asp:TextBox ID="txtName" runat="server" Width="250px" MaxLength="50" TabIndex="3"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvName" runat="server" ErrorMessage="Campo requerido"
                                                            ValidationGroup="Group" ControlToValidate="txtName" EnableViewState="false"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td valign="top" align="left">
                                                    </td>
                                                    <td valign="top" align="left">
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" align="left">
                                            <asp:Label ID="lblDescripcion" runat="server" Text="Descripcion" CssClass="clslabel"
                                                EnableViewState="False"></asp:Label><br />
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Columns="100"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnSaveGroup" runat="server" class="Button" Text="Guardar" ValidationGroup="Group">
                                                        </asp:Button>
                                                        <asp:Button ID="btnCancelGroup" runat="server" class="Button" Text="Cancelar" CausesValidation="false"
                                                            Visible="false"></asp:Button>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblErrorGroup" runat="server" Text="No se pudo guardar el grupo" CssClass="validators"
                                                            Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <div id="divBlock" runat="server">
                                    <table border="0" id="tabLock" runat="server" visible="true" width="100%">
                                        <tr>
                                            <td colspan="3" align="left">
                                                <br />
                                                <br />
                                                <hr />
                                                <asp:Label ID="lblBlocks" runat="server" Text="Bloqueos" CssClass="tituloSeccion"
                                                    EnableViewState="False"></asp:Label>
                                                <asp:Label ID="lblLockSubtitle" runat="server" Text="" EnableViewState="False" Style="float: left;
                                                    margin-top: 8px; display: inline-block; font-size: 17px;"></asp:Label>
                                                <asp:Button ID="btnNewLock" runat="server" class="ButtonNew" Style="float: right;"
                                                    Text="Nuevo" CausesValidation="false" OnClick="btnNewLock_Click"></asp:Button>
                                                <br />
                                                <br />
                                                <hr />
                                            </td>
                                        </tr>
                                    </table>
                                    <table border="0" id="tabEditLock" runat="server" visible="true" width="100%" style="border: solid 1px #CCC;
                                        padding: 5px;">
                                        <tr>
                                            <td valign="top" align="left">
                                                <asp:Label ID="lblHotel" runat="server" Text="Hotel" CssClass="clslabel" EnableViewState="False"></asp:Label><br />
                                                <asp:DropDownList ID="ddlHotels" runat="server" Style="width: 250px;" TabIndex="8" AutoPostBack="true"
                                                    class="country">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCutoffDate" runat="server" Text="CutoffDate" CssClass="clslabel"
                                                    EnableViewState="False"></asp:Label>
                                                    <asp:RequiredFieldValidator ID="rfvCutoffDate" runat="server" ErrorMessage="*"
                                                    ValidationGroup="Locks" ControlToValidate="txtCutoffDate" EnableViewState="false" Display="Dynamic"></asp:RequiredFieldValidator><br />
                                                <a hidefocus onclick="if(self.gfPop)gfPop.fPopCalendar1(document.getElementById('<%response.write(txtCutoffDate.ClientId)%>'));return false;"
                                                    href="javascript:void(0)">
                                                    <asp:TextBox ID="txtCutoffDate" runat="server" Width="250px" MaxLength="50" TabIndex="12"></asp:TextBox>
                                                    <img class="PopcalTrigger" alt="" src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>'
                                                        align="absMiddle" border="0"></a><br />
                                                <asp:RangeValidator ID="rngValCutoffDate" runat="server" ValidationGroup="Locks"  ControlToValidate="txtCutoffDate" Display="Dynamic" 
                                                    Type="Date" ErrorMessage="Fechas invalida"></asp:RangeValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" align="left">
                                                <asp:Label ID="lblStartDate" runat="server" Text="De" CssClass="clslabel" EnableViewState="False"></asp:Label>
                                                <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ErrorMessage="*"
                                                    ValidationGroup="Locks" ControlToValidate="txtStartDate" EnableViewState="false"></asp:RequiredFieldValidator><br />
                                                <a hidefocus onclick="if(self.gfPop)gfPop.fPopCalendar(document.getElementById('<%response.write(txtEndDate.ClientId)%>'),document.getElementById('<%response.write(txtStartDate.ClientId)%>'));return false;"
                                                    href="javascript:void(0)">
                                                    <asp:TextBox ID="txtStartDate" runat="server" Width="250px" MaxLength="50" TabIndex="3"></asp:TextBox>
                                                    <img class="PopcalTrigger" alt="" src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>'
                                                        align="absMiddle" border="0">
                                                </a><br />
                                                <asp:RangeValidator ID="rngValStartDate" runat="server" ValidationGroup="Locks"  ControlToValidate="txtStartDate" Display="Dynamic" 
                                                    Type="Date" ErrorMessage="Fechas invalida"></asp:RangeValidator>
                                            </td>
                                            <td valign="top" align="left">
                                                <asp:Label ID="lblEndDate" runat="server" Text="Hasta" CssClass="clslabel" EnableViewState="False"></asp:Label>
                                                <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ErrorMessage="*" Display="Dynamic"
                                                    ValidationGroup="Locks" ControlToValidate="txtEndDate" EnableViewState="false"></asp:RequiredFieldValidator><br />
                                                <a hidefocus onclick="if(self.gfPop)gfPop.fPopCalendar1(document.getElementById('<%response.write(txtEndDate.ClientId)%>'));return false;"
                                                    href="javascript:void(0)">
                                                    <asp:TextBox ID="txtEndDate" runat="server" Width="250px" MaxLength="50" TabIndex="12"></asp:TextBox>
                                                    <img class="PopcalTrigger" alt="" src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>'
                                                        align="absMiddle" border="0"></a>
                                                <br />
                                                <asp:CompareValidator ID="cvalDates" runat="server" Display="Dynamic" ErrorMessage="La fecha final debe ser mayor a la inicial"
                                                 ValidationGroup="Locks" ControlToCompare="txtStartDate" ControlToValidate="txtEndDate" Operator="GreaterThan" Type="Date"></asp:CompareValidator>
                                                 <asp:RangeValidator ID="rngValEndDate" runat="server" ValidationGroup="Locks"  ControlToValidate="txtEndDate" Display="Dynamic" 
                                                    Type="Date" ErrorMessage="Fechas invalida"></asp:RangeValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td>
                                                <asp:Label ID="lblRatePlan" runat="server" CssClass="clslabel" EnableViewState="False">Rate Plan: </asp:Label>
                                                <br />
                                                <asp:DropDownList runat="server" ID="ddlRatePlan"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="1">
                                                <asp:Button ID="btnAddLock" runat="server" class="Button" Text="Agregar" ValidationGroup="Locks">
                                                </asp:Button>
                                                <asp:Button ID="btnCancelBlock" runat="server" class="Button" Text="Cancelar" CausesValidation="false">
                                                </asp:Button>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblErrorLock" runat="server" Text="No se pudo guardar el bloqueo"
                                                    Style="float: left" CssClass="validators" Visible="False"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                    <table width="100%">
                                        <tr>
                                            <td colspan="3">
                                                <asp:DataGrid ID="dgLocksForGroup" runat="server" CssClass="DataGrid" AutoGenerateColumns="False"
                                                    AllowPaging="True" ShowFooter="true" PageSize="25">
                                                    <FooterStyle HorizontalAlign="Right"></FooterStyle>
                                                    <SelectedItemStyle BackColor="#DDDDFF" BorderColor="#9999FF" BorderWidth="1"></SelectedItemStyle>
                                                    <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
                                                    <ItemStyle CssClass="dgItem"></ItemStyle>
                                                    <HeaderStyle CssClass="dgHeader"></HeaderStyle>
                                                    <Columns>
                                                        <asp:BoundColumn DataField="IdLockForGroup" HeaderText="ID"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Nombre" HeaderText="Nombre"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="IdRatePlan" Visible="True" HeaderText="Rate Plan"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="CutoffDate" Visible="false" HeaderText="CutoffDate" DataFormatString="{0:MM/dd/yyyy}">
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="StartDate" Visible="false" HeaderText="StartDate" DataFormatString="{0:MM/dd/yyyy}">
                                                        </asp:BoundColumn>
                                                        <asp:BoundColumn DataField="EndDate" Visible="false" HeaderText="EndDate" DataFormatString="{0:MM/dd/yyyy}">
                                                        </asp:BoundColumn>
                                                        <asp:TemplateColumn HeaderStyle-Width="130">
                                                            <HeaderTemplate>
                                                                CutoffDate<br />
                                                                StartDate<br />
                                                                EndDate
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <%# Eval("CutoffDate","{0:MM/dd/yyyy}") %><br />
                                                                <%# Eval("StartDate","{0:MM/dd/yyyy}") %><br />
                                                                <%# Eval("EndDate","{0:MM/dd/yyyy}") %>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:TemplateColumn ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkSelectCorporate" runat="server" CausesValidation="False" CssClass="dgLink"
                                                                    CommandName="Select" Text='<%# PortalCulture.GetString("M000640") %>'></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:TemplateColumn>
                                                            <HeaderStyle Width="10%"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkEliminar2" Style="display: none" runat="server" CssClass="dgLink"
                                                                    CausesValidation="False" CommandName="Delete">-</asp:LinkButton>
                                                                <asp:HyperLink ID="lnkEliminar" runat="server" CssClass="dgLink" onclick="linkDel_Click()"></asp:HyperLink>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn DataField="IdHotel" Visible="false"></asp:BoundColumn>
                                                        <asp:TemplateColumn>
                                                            <HeaderStyle></HeaderStyle>
                                                            <HeaderTemplate>
                                                                <center>
                                                                    Habitaciones</center>
                                                            </HeaderTemplate>
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:DataGrid ID="dgRooms" runat="server" CssClass="tableRatesAgreement" AutoGenerateColumns="false"
                                                                    OnItemCommand="dgRooms_ItemCommand" OnItemDataBound="dgRooms_ItemDataBound" AllowPaging="false"
                                                                    ShowFooter="false" Width="100%">
                                                                    <SelectedItemStyle CssClass="dgSelected" />
                                                                    <HeaderStyle CssClass="DATAGRIDIN" />
                                                                    <Columns>
                                                                        <asp:BoundColumn DataField="IdLockForGroup_Rooms" HeaderStyle-Width="6%" HeaderText="ID">
                                                                        </asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="IdLockForGroup" Visible="false"></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="Quantity" HeaderStyle-Width="6%" HeaderText="Cant"></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="idTipoHabitacion_Hotel" Visible="false"></asp:BoundColumn>
                                                                        <asp:TemplateColumn HeaderStyle-Width="40%" HeaderText="Habitacion">
                                                                            <ItemTemplate>
                                                                                <%# Eval("CodigoHabitacion") %>
                                                                                -
                                                                                <%# Eval("NombreHabitacion") %>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>
                                                                        <asp:BoundColumn DataField="CodigoHabitacion" Visible="false"></asp:BoundColumn>
                                                                        <asp:BoundColumn DataField="NombreHabitacion" Visible="false"></asp:BoundColumn>
                                                                        <asp:TemplateColumn HeaderStyle-Width="11%">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="lnkEdit" runat="server" Text="" CommandName="Select" CausesValidation="False"
                                                                                    CssClass="dgLink"></asp:LinkButton>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>
                                                                        <asp:TemplateColumn>
                                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="lnkEliminar2" Style="display: none" runat="server" CssClass="dgLink"
                                                                                    CausesValidation="False" CommandName="Delete">-</asp:LinkButton>
                                                                                <asp:HyperLink ID="lnkEliminar" runat="server" CssClass="dgLink" onclick="linkDel_Click()"></asp:HyperLink>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateColumn>
                                                                    </Columns>
                                                                </asp:DataGrid>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn DataField="RatePlanName" Visible="false"></asp:BoundColumn>
                                                        <asp:TemplateColumn>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkAddRoom" runat="server" Text="" CommandName="AddRoom" CausesValidation="False"
                                                                    CssClass="dgLink"></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn DataField="EdadAdolecente" Visible="false"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="PlusTax" Visible="false"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Moneda" Visible="false"></asp:BoundColumn>
                                                    </Columns>
                                                    <PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
                                                        Position="Bottom" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
                                                </asp:DataGrid>
                                            </td>
                                        </tr>
                                    </table>
                                    
                                    <div id="inputRates" style="display: none; border: solid 2px #444; padding: 8px 15px;text-align:left;width:100%;
                                        background: #fff;" enableviewstate="false">
                                        <asp:HiddenField ID="hidGridLocked" runat="server" Value="" />
                                        <asp:Label ID="lblRoomType" runat="server" Text="Tipo de habit"></asp:Label>:
                                        <asp:DropDownList ID="ddlRoomType" runat="server" AutoPostBack="true">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblQuantity" runat="server" Text="Cantidad" Style="margin-left: 30px;"></asp:Label>
                                        :
                                        <asp:TextBox ID="txtQuantity" runat="server" CssClass="TextBox" Visible="false"></asp:TextBox>  
                                        <asp:DropDownList ID="ddlQuantity" runat="server" AutoPostBack="false"></asp:DropDownList>
                                         <% If txtQuantity.Visible Then%>
                                        <asp:RequiredFieldValidator ID="rfvQuantity" runat="server" CssClass="Validators" ValidationGroup="Fare"
                                            ControlToValidate="txtQuantity" ErrorMessage="*" ForeColor=" " Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="rngValQuantity" runat="server" CssClass="Validators" ValidationGroup="Fare" MinimumValue="1" MaximumValue="999"
                                            ControlToValidate="txtQuantity" ErrorMessage="Cantidad invalida" Display="Dynamic" Type="Integer"></asp:RangeValidator>
                                        <% end if %>
                                        <br />
                                        <br />
                                        <div class="dgItem" align="center" style="display:none;">
                                            <!--<%=PortalCulture.GetString("00131")%>&nbsp;-&nbsp;<b><%=PortalCulture.GetString("M000263")%>&nbsp;<span
                                                class="currency"></span></b>&nbsp;-->
                                            <asp:Label ID="lblMonTar" runat="server" Font-Bold="True"></asp:Label>
                                        </div><br />
                                        <asp:DropDownList ID="ddlShowRates" runat="server" >
                                        </asp:DropDownList>
                                        <br /><br /><br />
                                        <div id="pnlTarifas" style="display: none; width: 49%; vertical-align: top;">
                                            <table width="100%" style="vertical-align: top;">
                                                <tr>
                                                    <td align="left" width="33%">
                                                        <asp:Label ID="lblPrecio" runat="server" CssClass="clsLabel" EnableViewState="False">Adulto:</asp:Label>
                                                    </td>
                                                    <td align="left" width="33%">
                                                        <asp:Label ID="LblChildPrice" runat="server" CssClass="clsLabel" EnableViewState="False">Niño</asp:Label>
                                                    </td>
                                                    <td align="left" width="33%">
                                                        <asp:Label ID="lblAdolescentePrice" runat="server" CssClass="clsLabel">Adolescente</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" valign="top">
                                                        <asp:TextBox ID="txtAdultFare" runat="server" CssClass="TextBox currency" Columns="10"
                                                            MaxLength="10" visible="false">1</asp:TextBox>
                                                        <% If txtAdultFare.Visible Then%>
                                                        <span class="currency"></span>
                                                        <asp:RequiredFieldValidator ID="reqAdultFare" runat="server" CssClass="Validators" ValidationGroup="Fare"
                                                            ControlToValidate="txtAdultFare" ErrorMessage="*" ForeColor=" " Display="Dynamic"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="ReValAdult" runat="server" CssClass="Validators"
                                                            ControlToValidate="txtAdultFare" ErrorMessage="Precio Inválido" ForeColor=" " ValidationGroup="Fare"
                                                            Display="Dynamic" ValidationExpression="^([0-9]*|\d*\.\d{1}?\d*)$"></asp:RegularExpressionValidator>
                                                        <% end if %>
                                                    </td>
                                                    <td align="left" valign="top">
                                                        <asp:TextBox ID="txtChildFare" runat="server" CssClass="TextBox currency" Columns="10"
                                                            MaxLength="10" visible="false">1</asp:TextBox>
                                                        <% If txtChildFare.Visible Then%>
                                                        <span class="currency"></span>
                                                        <asp:RequiredFieldValidator ID="reqChildFare" runat="server" CssClass="Validators" ValidationGroup="Fare"
                                                            ControlToValidate="txtChildFare" ErrorMessage="*" ForeColor=" " Display="Dynamic"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="ReValChild" runat="server" CssClass="Validators" ValidationGroup="Fare"
                                                            ControlToValidate="txtChildFare" ErrorMessage="Precio Inválido" ForeColor=" "
                                                            Display="Dynamic" ValidationExpression="^([0-9]*|\d*\.\d{1}?\d*)$"></asp:RegularExpressionValidator>
                                                        <% end if %>
                                                    </td>
                                                    <td align="left" valign="top">
                                                        <asp:TextBox ID="txtTeenFare" runat="server" CssClass="TextBox currency" Columns="10"
                                                            MaxLength="10" visible="false">1</asp:TextBox>
                                                        <% If txtTeenFare.Visible Then%>
                                                        <span class="currency"></span>
                                                        <asp:RequiredFieldValidator ID="reqTeenFare" runat="server" CssClass="Validators" ValidationGroup="Fare"
                                                            ControlToValidate="txtTeenFare" ErrorMessage="*" ForeColor=" " Display="Dynamic"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="ReValAdoslecenteFare" runat="server" CssClass="Validators" ValidationGroup="Fare"
                                                            ControlToValidate="txtTeenFare" ErrorMessage="Precio Inválido" ForeColor=" "
                                                            Display="Dynamic" ValidationExpression="^([0-9]*|\d*\.\d{1}?\d*)$"></asp:RegularExpressionValidator>
                                                        <% end if %>
                                                    </td>
                                                    
                                                </tr>
                                            </table>
                                        </div>
                                        <div style=" width: 49%; vertical-align: top;display:none;">
                                            <table width="100%" style="vertical-align: top;">
                                                <tr>
                                                    <td align="left" width="33%">
                                                        <asp:Label ID="lblExtraAdult" runat="server" CssClass="clsLabel" EnableViewState="False">Adulto extra:</asp:Label>
                                                    </td>
                                                    <td align="left" width="33%">
                                                        <asp:Label ID="lblExtraChildPrice" runat="server" CssClass="clsLabel" EnableViewState="False">Niño extra:</asp:Label>
                                                    </td>
                                                    <td align="left" width="33%">
                                                        <asp:Label ID="lblExtraAdolescente" runat="server" CssClass="clsLabel">Adolescente extra:</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" valign="top">
                                                        <asp:TextBox ID="txtExtraAdultPrice" runat="server" CssClass="TextBox currency" Columns="10"
                                                            MaxLength="10" Enabled="false" Visible="false">1</asp:TextBox>
                                                        <% If txtExtraAdultPrice.Visible Then%>
                                                        <span class="currency"></span>
                                                        <asp:RequiredFieldValidator ID="reqExtraAdultPrice" runat="server" CssClass="Validators" ValidationGroup="Fare"
                                                            ControlToValidate="txtExtraAdultPrice" ErrorMessage="*" ForeColor=" " Display="Dynamic"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="ReValAdultExtraPrice" runat="server" CssClass="Validators"
                                                            ControlToValidate="txtExtraAdultPrice" ErrorMessage="Precio Inválido" ForeColor=" " ValidationGroup="Fare"
                                                            Display="Dynamic" ValidationExpression="^([0-9]*|\d*\.\d{1}?\d*)$"></asp:RegularExpressionValidator>
                                                        <% end if %>
                                                    </td>
                                                    <td align="left" valign="top">
                                                        <asp:TextBox ID="txtExtraChildPrice" runat="server" CssClass="TextBox currency" Columns="10"
                                                            MaxLength="10" Enabled="false" Visible="false">1</asp:TextBox>
                                                        <% If txtExtraChildPrice.Visible Then%>
                                                        <span class="currency"></span>
                                                        <asp:RequiredFieldValidator ID="reqExtraChildPrice" runat="server" CssClass="Validators" ValidationGroup="Fare"
                                                            ControlToValidate="txtExtraChildPrice" ErrorMessage="*" ForeColor=" " Display="Dynamic"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="ReValChildExtraPrice" runat="server" CssClass="Validators" ValidationGroup="Fare"
                                                            ControlToValidate="txtExtraChildPrice" ErrorMessage="Precio Inválido" ForeColor=" "
                                                            Display="Dynamic" ValidationExpression="^([0-9]*|\d*\.\d{1}?\d*)$"></asp:RegularExpressionValidator>
                                                        <% end if %>
                                                    </td>
                                                    <td align="left" valign="top">
                                                        <asp:TextBox ID="txtExtraTeenPrice" runat="server" CssClass="TextBox currency" Columns="10"
                                                            MaxLength="10" Enabled="false" Visible="false">1</asp:TextBox>
                                                        <% If txtExtraTeenPrice.Visible Then%>
                                                        <span class="currency"></span>
                                                        <asp:RequiredFieldValidator ID="reqExtraTeenPrice" runat="server" CssClass="Validators" ValidationGroup="Fare"
                                                            ControlToValidate="txtExtraTeenPrice" ErrorMessage="*" ForeColor=" " Display="Dynamic"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="ReValTeenExtraPrice" runat="server" CssClass="Validators" ValidationGroup="Fare"
                                                            ControlToValidate="txtExtraTeenPrice" ErrorMessage="Precio Inválido" ForeColor=" "
                                                            Display="Dynamic" ValidationExpression="^([0-9]*|\d*\.\d{1}?\d*)$"></asp:RegularExpressionValidator>
                                                        <% end if %>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <br />
                                        <div style="display: none" id="DivRates" runat="server">
                                            <table border="0" cellspacing="0" cellpadding="0" width="100%">
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblPreciosTarifa" runat="server" EnableViewState="False" Visible="false"
                                                            CssClass="clsLabel">Tarifas <br>  Defina el precio de tarifa en ocupacion por adulto y el precio para niños, el campo total de tarifas mostrara el precio calculado por noche en habitación.</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="" align="center">
                                                        <asp:Label ID="lblPlusTax" runat="server" EnableViewState="False" CssClass="bookingNormalLabel"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <table border="1" cellspacing="1" cellpadding="1" width="100%">
                                                            <tr>
                                                                <td align="center">
                                                                    <table id="tblTAB2" border="0" cellspacing="0" cellpadding="0" width="100%">
                                                                        <tr>
                                                                            <td id="TdPricing" class="tabselected" onmouseover="javascript:this.style.cursor='pointer';"
                                                                                onclick="javascript:optionSw('1P');" width="50%" align="center" runat="server">
                                                                                <asp:Label ID="lblPricingNE" runat="server" EnableViewState="False" Font-Size="XX-Small">[Pricing]</asp:Label>
                                                                            </td>
                                                                            <td id="TdPricingE" class="tab" onmouseover="javascript:this.style.cursor='pointer';"
                                                                                onclick="javascript:optionSw('1E');" width="45%" align="center" runat="server">
                                                                                <asp:Label ID="lblPricingExc" runat="server" EnableViewState="False" Font-Size="XX-Small">[Pricing Exception]</asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <div style="min-height: 100px; width: 100%;" id="divA2" align="center" runat="server"
                                                                        ms_positioning="FlowLayout">
                                                                        <uc1:ctrlPlanFares ID="CtrlPlanFares2" runat="server">
                                                                        </uc1:ctrlPlanFares>
                                                                    </div>
                                                                    <div style="min-height: 100px; width: 100%; display: none" id="divB2" align="center"
                                                                        runat="server" ms_positioning="FlowLayout">
                                                                        <uc1:CtrlPlanFaresExc ID="CtrlPlanFaresExc2" runat="server">
                                                                        </uc1:CtrlPlanFaresExc>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <asp:TextBox Style="display: none" ID="txtDivP" runat="server"></asp:TextBox>
                                        <asp:Button ID="btnSaveFare" runat="server" EnableViewState="False" CssClass="Button" ValidationGroup="Fare"
                                            Text="Guardar" Width="85px"></asp:Button>
                                        <asp:Button ID="btnCancelFare" runat="server" EnableViewState="False" CssClass="Button"
                                            Text="Cancelar" CausesValidation="False" Width="85px"></asp:Button>
                                            <br /><br />
                                            <center><asp:Label ID="lblErrorFare" runat="server" Visible="false" CssClass="validators">No se pudo guardar la tarifa</asp:Label></center>
                                    </div>
                                </div>
                            </div>
                        </center>
                    </td>
                </tr>
            </table>
        </div>
        <center><asp:Label ID="lblError" runat="server" Visible="false" EnableViewState="True" CssClass="validators"></asp:Label></center>
        <uc1:ctlMensajes ID="CtlMensajes1" runat="server">
        </uc1:ctlMensajes>
        <uc1:ctlMensajes ID="CtlMensajes2" runat="server">
        </uc1:ctlMensajes>
        <uc1:ctlMensajes ID="CtlMensajes3" runat="server">
        </uc1:ctlMensajes>
    </form>
</body>
</html>
