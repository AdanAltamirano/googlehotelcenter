<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Convenios.aspx.vb" Inherits="RateManager.Convenios" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<%@ Register TagPrefix="uc2" TagName="ctrlAutoComplete" Src="../Modulos/ctrlAutoComplete.ascx"   %>
<%@ Register TagPrefix="uc1" TagName="ctlMensajes" Src="../Modulos/ctlMensajes.ascx" %>
<%@ Import Namespace="RateManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Covenios</title>

    <script type="text/javascript" src='<%= (request.applicationpath & "/pages/Scripts/jquery.min.js").Replace("//","/") %>'></script>

    <script type="text/javascript" src='<%= (request.applicationpath & "/pages/Scripts/jquery.bgiframe.min.js").Replace("//","/")%>'></script>

    <script type="text/javascript" src='<%= (request.applicationpath & "/pages/Scripts/jquery.autocomplete.min.js").Replace("//","/")%>'></script>

    <script type="text/javascript" src='<%= (request.applicationpath & "/pages/Scripts/Convenios.js?").Replace("//","/") &  today%>'></script>

    <script type="text/javascript" src='<%= (request.applicationpath & "/pages/Scripts/Location.js?").Replace("//","/") &  today%>'></script>

    <script type="text/javascript" src='<%= (request.applicationpath & "/pages/Scripts/JsToolTips.js?").Replace("//","/") &  today%>'></script>
    
    
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js"></script>
    
    <link href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet" />
    <link href="../Reports/CRstyle.css" type="text/css" rel="stylesheet" />
    
    <script type="text/javascript" src="../Includes/Script/JsSearch-1.0.js"></script>
    <script type="text/javascript">
        $=jQuery.noConflict();
	    <% if ReportAgreement.visible=false then %>
	    if (history.forward(1)) history.replace(history.forward(1));
	    <% end if %>
	    var alert1 = '<%=PortalCulture.GetString("01194") %>';
	    var alert2 = '<%=PortalCulture.GetString("01195") %>';
	    var alert3 = '<%=PortalCulture.GetString("01196") %>';
	    var alert4 = '<%=PortalCulture.GetString("01198") %>';
	    var alert5 = '<%=PortalCulture.GetString("01212") %>';
	    var alert6 = '<%=PortalCulture.GetString("01213") %>';
	    var alert7 = '<%=PortalCulture.GetString("01221") %>';
	    var label1 = '<%=PortalCulture.GetString("M000165") %>';
	    var label2 = '<%=PortalCulture.GetString("M000166") %>';
	    var label3 = '<%=PortalCulture.GetString("M000226",true) %>';
	    var label4 = '<%=PortalCulture.GetString("M000227",true) %>';
	    var label5 = '<%=PortalCulture.GetString("M000263", True) %>';
	    var alt1 = '<%=PortalCulture.GetString("01197") %>';
	    var title1 = '<%=PortalCulture.GetString("01197") %>';
	    var alt2 = '<%=PortalCulture.GetString("00762") %>';
	    var title2 = '<%=PortalCulture.GetString("00762") %>';
	    var id1 = '<%= hdnUsedEspecificRestrictions.ClientID %>';
	    var id2 = '<%= hdnEspecificRestrictions.ClientID %>';
	    var id3 = '<%= hdnGeneralRestrictions.ClientID %>';
	    //var id4 = '<%--= dgHotels.ClientId --%>';
	    var id5 = '<%= FormAgreement.ClientId %>';
	    document.onmouseup = getmousepositionUp;
	    
	    SearchStart.AddParam
	    (
		    {
		        searchitems: [
			        { Item: 'Agreements', IDSearch: 'IdConvenio', nameSearch: 'Referencia', isdefault: false },
			        { Item: 'Agreements', IDSearch: 'IdConvenio', nameSearch: 'Nombre', isdefault: true }
			    ],
		        colModel: [
			        { display: '<%= RateManager.PortalCulture.GetString("00001") %>' }, { display: '<%= RateManager.PortalCulture.GetString("M000144") %>' },
			    ],
			        Data: [{ catalogo: 'Agreements',
			        idCorporate: '<%= idCorporate%>'
                    }],
                    id: 'Agreements',
		            index: 1
		        }
	    );
    </script>

</head>
<body onload="Restoreform();" bottommargin="0" leftmargin="0" rightmargin="0" topmargin="0"
    ms_positioning="FlowLayout">
    <form id="Form1" runat="server">
    <div class="clear">
        <div class="mDiv">
        </div>
        <div>
            <asp:Label ID="lblTitle" runat="server" EnableViewState="False" Text="Convenios"
                CssClass="tituloSeccion"></asp:Label>
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
                    <div id="FormAgreement" runat="server">
                        <a id="aTop"></a>
                        <table align="center" border="0" style="width:954px;">
                            <tr>
                                <td colspan="3" align="left">
                                    <asp:Button ID="btnSelectCorporate" runat="server" CssClass="lnkButton" Text="Seleccionar Corporativo"
                                        Visible="false" ValidationGroup="Corporate" /><br />
                                    <uc2:ctrlAutoComplete ID="ctrlAutoComplete1" runat="server" CausesValidation="false" />
                                    <br />
                                    <a href="#" onclick="ShowAllAgreements(<%=idCorporate.toString()%>)">Ver todos los convenios</a>
                                    
                                    <div ID="divAllAgreements" runat="server" class="ConveniosHomoClave" style="display: none; ">
                                            <a onclick='CloseAllAgreements();' style="cursor: hand; float: right;">
                                                <img src="../Images/close.png" /></a>
                                            <table align="center" id="Table2">
                                                <thead>
                                                    <tr>
                                                        <th align="center">
                                                            <h2><%=PortalCulture.GetString("01211")%></h2>
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr>
                                                        <td align="left">
                                                            <div id="divAllAgreementsContent" style="margin-bottom:4px;"></div>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                        
                                        
                                    <asp:Button ID="btnNewConvenio" runat="server" class="ButtonNew" Style="float: right;"
                                                Text="Nuevo" CausesValidation="false"></asp:Button><br />
                                    
                                    <asp:DataGrid ID="dgAgreementWorking" runat="server" CssClass="DataGrid" AutoGenerateColumns="False"
                                        AllowPaging="True" ShowFooter="true" PageSize="10" >
                                        <SelectedItemStyle BackColor="#DDDDFF" BorderColor="#9999FF" BorderWidth="1"></SelectedItemStyle>
                                        <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
                                        <ItemStyle CssClass="dgItem"></ItemStyle>
                                        <HeaderStyle CssClass="dgHeader"></HeaderStyle>
                                        <Columns>
                                           <asp:BoundColumn DataField="IdConvenio" HeaderText="ID" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                           <asp:BoundColumn DataField="Referencia" Visible="false"  />
                                           <asp:BoundColumn DataField="Nombre" Visible="false"  />
                                           <asp:BoundColumn DataField="ConvenioDescription" HeaderText="Descripcion" />
                                           <asp:TemplateColumn ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkSelectCorporate" runat="server" CausesValidation="False" CssClass="dgLink"
                                                        CommandName="Select" Text='<%# PortalCulture.GetString("M000640") %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                        </Columns>                                                    
                                        <PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
                                            Position="Bottom" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
                                    </asp:DataGrid>
                                    
                                     
                                </td>
                            </tr>
                        </table>
                         <table id="tabEditConvenio" runat="server" align="center" border="0" visible="false">
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
                                <td align="left">
                                    <asp:Label ID="lblNoAgreement" runat="server" Text="No Convenio" CssClass="clslabel"
                                        EnableViewState="False"></asp:Label>
                                    <asp:RequiredFieldValidator ID="rfvNoAgreement" runat="server" ErrorMessage="Campo requerido"
                                        ControlToValidate="txtNoAgreement" EnableViewState="false" ValidationGroup="Agreement"></asp:RequiredFieldValidator><br />
                                    <asp:TextBox ID="txtNoAgreement" runat="server" Width="150px" MaxLength="20" TabIndex="2"></asp:TextBox>
                                    <asp:CheckBox ID="chkCC" runat="server" />
                                </td>
                                <td>
                                    <span id="lblCheckinHour" runat="server">Hora de llegada:</span><br />
                                    <asp:DropDownList ID="ddlhour" runat="server">
                                        <asp:ListItem>00</asp:ListItem>
                                        <asp:ListItem>01</asp:ListItem>
                                        <asp:ListItem>02</asp:ListItem>
                                        <asp:ListItem>03</asp:ListItem>
                                        <asp:ListItem>04</asp:ListItem>
                                        <asp:ListItem>05</asp:ListItem>
                                        <asp:ListItem>06</asp:ListItem>
                                        <asp:ListItem>07</asp:ListItem>
                                        <asp:ListItem>08</asp:ListItem>
                                        <asp:ListItem>09</asp:ListItem>
                                        <asp:ListItem>10</asp:ListItem>
                                        <asp:ListItem>11</asp:ListItem>
                                        <asp:ListItem>12</asp:ListItem>
                                        <asp:ListItem>13</asp:ListItem>
                                        <asp:ListItem>14</asp:ListItem>
                                        <asp:ListItem>15</asp:ListItem>
                                        <asp:ListItem>16</asp:ListItem>
                                        <asp:ListItem>17</asp:ListItem>
                                        <asp:ListItem>18</asp:ListItem>
                                        <asp:ListItem>19</asp:ListItem>
                                        <asp:ListItem>20</asp:ListItem>
                                        <asp:ListItem>21</asp:ListItem>
                                        <asp:ListItem>22</asp:ListItem>
                                        <asp:ListItem>23</asp:ListItem>
                                    </asp:DropDownList>
                                    :
                                    <asp:DropDownList ID="ddlmin" runat="server">
                                        <asp:ListItem>00</asp:ListItem>
                                        <asp:ListItem>15</asp:ListItem>
                                        <asp:ListItem>30</asp:ListItem>
                                        <asp:ListItem>45</asp:ListItem>
                                    </asp:DropDownList>
                                    <%Me.chkIsGeneral.Text = Me.GetLabel("chkIsGeneral")%>
                                    <asp:CheckBox ID="chkIsGeneral" TabIndex="2" Checked="false" runat="server" Style="display: none;
                                        visibility: hidden;" />
                                </td>
                                <td>
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
                                                <asp:Label ID="lblAgency" runat="server" Text="Empresa" CssClass="clslabel" EnableViewState="False"></asp:Label>
                                                <asp:RequiredFieldValidator ID="rfvAgency" runat="server" ErrorMessage="Campo requerido"
                                                    ControlToValidate="txtAgency" EnableViewState="false" ValidationGroup="Agreement"></asp:RequiredFieldValidator><br />
                                                <asp:TextBox ID="txtAgency" runat="server" Width="250px" MaxLength="50" TabIndex="3"></asp:TextBox>
                                            </td>
                                            <td valign="top" align="left">
                                                <asp:Label ID="lblCountry" runat="server" Text="Pais" CssClass="clslabel" EnableViewState="False"></asp:Label><br />
                                                <%--<anthem:dropdownlist id="ddlCountries" runat="server" width="250px" AutoCallBack="True" TabIndex="8"></anthem:dropdownlist>--%>
                                                <%--asp:DropDownList ID="ddlCountries" runat="server" Width="250px" TabIndex="8" CssClass="country"></asp:DropDownList>--%>
                                                <select id="ddlCountries" runat="server" style="width: 250px;" tabindex="8" class="country" Group="Contacto">
                                                </select>
                                                <input id="selectedCountry" type="hidden" runat="server" value="" />
                                            </td>
                                            <td valign="top" align="left">
                                                <asp:Label ID="lblContact" runat="server" Text="Contacto" CssClass="clslabel" EnableViewState="False"></asp:Label>
                                                <asp:RequiredFieldValidator ID="rfvContact" runat="server" ErrorMessage="Campo requerido"
                                                    ControlToValidate="txtContact" EnableViewState="false" ValidationGroup="Agreement"></asp:RequiredFieldValidator><br />
                                                <asp:TextBox ID="txtContact" runat="server" Width="250px" MaxLength="50" TabIndex="12"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="lblAddress" runat="server" Text="Domicilio" CssClass="clslabel" EnableViewState="False"></asp:Label><br />
                                                <asp:RequiredFieldValidator ID="rqfAddress" runat="server" ErrorMessage="Campo requerido"
                                                    ControlToValidate="txtAddress" EnableViewState="false" ValidationGroup="Agreement"></asp:RequiredFieldValidator><br />
                                                <asp:TextBox ID="txtAddress" runat="server" Width="250px" MaxLength="50" TabIndex="4"></asp:TextBox>
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="lblState" runat="server" Text="Estado" CssClass="clslabel" EnableViewState="False"></asp:Label><br />
                                                <%--<anthem:dropdownlist id="ddlStates" runat="server" width="250px" AutoCallBack="True" TabIndex="9"></anthem:dropdownlist>--%>
                                                <select id="ddlStates" runat="server" style="width: 250px;" tabindex="9" class="state" Group="Contacto">
                                                </select>
                                                <input id="selectedState" type="hidden" runat="server" value="" />
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="lblJob" runat="server" Text="Puesto" CssClass="clslabel" EnableViewState="False"></asp:Label>
                                                <asp:RequiredFieldValidator ID="rfvJob" runat="server" ErrorMessage="Campo requerido"
                                                    ControlToValidate="txtJob" EnableViewState="false" ValidationGroup="Agreement"></asp:RequiredFieldValidator><br />
                                                <asp:TextBox ID="txtJob" runat="server" Width="250px" MaxLength="50" TabIndex="13"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="lblTown" runat="server" Text="Colonia" CssClass="clslabel" EnableViewState="False"></asp:Label><br />
                                                <asp:RequiredFieldValidator ID="rfvTown" runat="server" ErrorMessage="Campo requerido"
                                                    ControlToValidate="txtTown" EnableViewState="false" ValidationGroup="Agreement"></asp:RequiredFieldValidator><br />
                                                <asp:TextBox ID="txtTown" runat="server" Width="250px" MaxLength="20" TabIndex="5"></asp:TextBox>
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="lblDistrict" runat="server" Text="Municipio" CssClass="clslabel" EnableViewState="False"></asp:Label><br />
                                                <%--<anthem:dropdownlist id="ddlDistricts" runat="server" width="250px" AutoCallBack="True" TabIndex="10"></anthem:dropdownlist>--%>
                                                <select id="ddlDistricts" runat="server" style="width: 250px;" tabindex="10" class="district" Group="Contacto">
                                                </select>
                                                <input id="selectedDistrict" type="hidden" runat="server" value="" />
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="lblEmail" runat="server" Text="Correo Electronico" CssClass="clslabel"
                                                    EnableViewState="False"></asp:Label>
                                                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ErrorMessage="Campo requerido"
                                                    ControlToValidate="txtEmail" EnableViewState="false" ValidationGroup="Agreement"></asp:RequiredFieldValidator><br />
                                                <asp:RegularExpressionValidator ID="revEmail" runat="server" ErrorMessage="Correo electronico invalido"
                                                    ControlToValidate="txtEmail" ValidationExpression="^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"
                                                    ValidationGroup="Agreement"></asp:RegularExpressionValidator><br />
                                                <asp:TextBox ID="txtEmail" runat="server" Width="250px" MaxLength="50" TabIndex="14"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="lblZIP" runat="server" Text="Codigo Postal" CssClass="clslabel" EnableViewState="False"></asp:Label><br />
                                                <asp:RequiredFieldValidator ID="rqfZIP" runat="server" ErrorMessage="Campo requerido"
                                                    ControlToValidate="txtZIP" EnableViewState="false" ValidationGroup="Agreement"></asp:RequiredFieldValidator><br />
                                                <asp:TextBox ID="txtZIP" runat="server" Width="150px" MaxLength="11" TabIndex="6"></asp:TextBox>
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="lblCity" runat="server" Text="Ciudad" CssClass="clslabel" EnableViewState="False"></asp:Label><br />
                                                <%--<anthem:dropdownlist id="ddlCities" runat="server" width="250px" TabIndex="11"></anthem:dropdownlist>--%>
                                                <select id="ddlCities" runat="server" style="width: 250px;" tabindex="11" class="city" Group="Contacto">
                                                </select>
                                                <asp:CustomValidator Display="Dynamic" ID="rfvCity" runat="server" ErrorMessage="Campo requerido"
                                                    EnableViewState="false" ValidationGroup="Agreement" ClientValidationFunction="ValidateCity"></asp:CustomValidator>
                                                <input id="selectedCity" type="hidden" runat="server" value="" class="SelectedCity" Group="Contacto" />
                                            </td>
                                            <td align="left">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="lblPhone" runat="server" Text="Telefono" CssClass="clslabel" EnableViewState="False"></asp:Label><br />
                                                <asp:RequiredFieldValidator ID="rqfPhone" runat="server" ErrorMessage="Campo requerido"
                                                    ControlToValidate="txtPhone" EnableViewState="false" ValidationGroup="Agreement"></asp:RequiredFieldValidator><br />
                                                <asp:TextBox ID="txtPhone" runat="server" Width="150px" MaxLength="25" TabIndex="7"></asp:TextBox>
                                            </td>
                                            <td align="left">                                            
                                                <asp:CheckBox ID="chkIsAgency" TabIndex="12" Checked="false" runat="server" visible="false"/>
                                                <asp:Label ID="lblSegmento" runat="server" Text="Segmento" CssClass="clslabel" EnableViewState="False"></asp:Label><br />
                                                <asp:DropDownList Id="cmbSegmento" runat="server" Width="150px" AutoPostBack="false" style="width: 250px;"></asp:DropDownList>
                                            </td>
                                            <td align="left">
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="left">
                                    <br />
                                    <br />
                                    <span class="bookingNormalLabel">
                                        <%=PortalCulture.GetString("01436")%></span>
                                    <asp:RequiredFieldValidator ID="rfvCorporateContact" runat="server" ErrorMessage="La información de emisor es requerida"
                                        ControlToValidate="txtCorporateContact" EnableViewState="false" ValidationGroup="Agreement"></asp:RequiredFieldValidator>
                                    <hr />
                                </td>
                            </tr>
                            <tr id="pnlOwnerOffice">
                                <td colspan="3" align="left">
                                    <span class="clslabel">
                                        <%=PortalCulture.GetString("01437")%>:</span>
                                    <input id="txtOwnerOffice" type="hidden" runat="server" value="" />
                                    <br />
                                    <select id="lstOwnerOffice">
                                        <option value="0" selected="selected">--
                                            <%=PortalCulture.GetString("00753")%>
                                            --</option>
                                    </select>
                                </td>
                            </tr>
                            <tr id="pnlOwnerOfficeContact" style="display: none">
                                <td colspan="3" align="left" style="display: none">
                                    <span class="clslabel">
                                        <%=PortalCulture.GetString("01209")%>:</span>
                                    <input id="txtOwnerOfficeContact" type="hidden" runat="server" value="" />
                                    <br />
                                    <select id="lstOwnerOfficeContact">
                                        <option value="0" selected="selected">--
                                            <%=PortalCulture.GetString("00753")%>
                                            --</option>
                                    </select>
                                </td>
                            </tr>
                            <tr id="pnlOwnerContact" style="display: none">
                                <td align="left">
                                    <asp:Label ID="lblCorporateContact" runat="server" Text="Contacto" CssClass="clslabel"
                                        EnableViewState="False"></asp:Label>
                                    <br />
                                    <asp:TextBox ID="txtCorporateContact" runat="server" Width="250px" MaxLength="50"
                                        TabIndex="15"></asp:TextBox>
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblCorporateJob" runat="server" Text="Puesto" CssClass="clslabel"
                                        EnableViewState="False"></asp:Label>
                                    <br />
                                    <asp:TextBox ID="txtCorporateJob" runat="server" Width="250px" MaxLength="50" TabIndex="16"></asp:TextBox>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="left">
                                    <br />
                                    <br />
                                    <span class="bookingNormalLabel">
                                        <%=PortalCulture.GetString("01438")%></span>
                                    <hr />
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <br />
                                </td>
                            </tr>
                            <tr style="display:none">
                                <td colspan="3" align="left">
                                    <asp:Label ID="lblCopyAgreement" runat="server" Text="Copiar Convenio" CssClass="clslabel"
                                        EnableViewState="False"></asp:Label>
                                    <asp:DropDownList ID="ddlAgreements" runat="server" Width="96%" AutoPostBack="True"
                                        TabIndex="17">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="left">
                                    <asp:Label ID="lblGeneralRestrictions" runat="server" Text="Restricciones Generales"
                                        CssClass="clslabel" EnableViewState="False"></asp:Label><br />
                                    <table>
                                        <tr>
                                            <td>
                                                <textarea id="txtGeneralRestrictions" class="txtRestriction" rows="3" cols="100"
                                                    onkeyup="maxLength(event,this,250);" onkeydown="maxLength(event,this,250);" maxlength="250"
                                                    tabindex="18"></textarea>
                                            </td>
                                            <td>
                                                <input type="button" id="btnGeneralRestrictions" value='<%= PortalCulture.GetString("A00673") %>'
                                                    onclick="AddRestriction(true,'txtGeneralRestrictions','<%= hdnGeneralRestrictions.ClientID %>');"
                                                    class="Button" tabindex="19" />
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:HiddenField ID="hdnGeneralRestrictions" runat="server" />
                                    <table id="tbGeneralRestrictions" class="tableRestrictions">
                                        <tbody>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                               
                               <br /><br />
                                 <asp:DataGrid ID="dgHomoClaves" runat="server" CssClass="DataGrid" AutoGenerateColumns="False"
                                    AllowPaging="True" ShowFooter="True" PageSize="25" Visible="false">
                                    <FooterStyle HorizontalAlign="Right"></FooterStyle>
                                    <SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
                                    <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
                                    <ItemStyle CssClass="dgItem"></ItemStyle>
                                    <HeaderStyle CssClass="dgHeader"></HeaderStyle>
                                    <Columns>
                                        <asp:BoundColumn Visible="False" DataField="idConvenioHomoClave"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="HomoClave" HeaderText="HomoClave"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="Company" HeaderText="Company"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="Contact" HeaderText="Contact"></asp:BoundColumn>
                                        <asp:TemplateColumn>
                                            <HeaderStyle Width="10%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkEliminar2" Style="display: none" runat="server" CssClass="dgLink"
                                                    CausesValidation="False" CommandName="Delete">-</asp:LinkButton>
                                                <asp:HyperLink ID="lnkEliminar" runat="server" CssClass="dgLink"></asp:HyperLink>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                    </Columns>
                                    <PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
                                    Position="Bottom" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
                                </asp:DataGrid>
                                 <input type="button" class="Button" value="<%=PortalCulture.GetString("01580")%>" onclick="ShowHomoClave()" style="float:right;" />
                                <br />
                                 <asp:Label
                                    ID="lblErrorHomoClaves" runat="server" CssClass="Validators" Visible="False" EnableViewState="false">*</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    
                                         <div ID="divHomoclave" runat="server" class="ConveniosHomoClave" style="display: none; ">
                                            <a onclick='CloseHomoClave();' style="cursor: hand; float: right;">
                                                <img src="../Images/close.png" /></a>
                                            <table align="center" id="Table1">
                                                <thead>
                                                    <tr>
                                                        <th align="center">
                                                            <h2><%=PortalCulture.GetString("01580")%></h2>
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr>
                                                        <td align="left">
                                                        
                                                            <table border="0" align="center" width="100%" id="Table3">
                                                                <tr>
                                                                    <td>
                                                                       <asp:Label ID="lblHomoClave" runat="server" Text="HomoClave" CssClass="clslabel"
                                                                            EnableViewState="False"></asp:Label>
                                                                        <asp:RequiredFieldValidator ID="rfvHomoClave" runat="server" ErrorMessage="Campo requerido"
                                                                            ControlToValidate="txtHomoClave" EnableViewState="false" ValidationGroup="HAgreement"></asp:RequiredFieldValidator><br />
                                                                        <asp:TextBox ID="txtHomoClave" runat="server" Width="150px" MaxLength="2" 
                                                                            TabIndex="124"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top" align="left">
                                                                        <asp:Label ID="lblHAgency" runat="server" Text="Empresa" CssClass="clslabel" EnableViewState="False"></asp:Label>
                                                                        <asp:RequiredFieldValidator ID="rfvHAgency" runat="server" ErrorMessage="Campo requerido"
                                                                            ControlToValidate="txtHAgency" EnableViewState="false" ValidationGroup="HAgreement"></asp:RequiredFieldValidator><br />
                                                                        <asp:TextBox ID="txtHAgency" runat="server" Width="250px" MaxLength="50" 
                                                                            TabIndex="125"></asp:TextBox>
                                                                    </td>
                                                                    <td valign="top" align="left">
                                                                        <asp:Label ID="lblHCountry" runat="server" Text="Pais" CssClass="clslabel" EnableViewState="False"></asp:Label><br />

                                                                        <select id="ddlHCountries" runat="server" style="width: 250px;" tabindex="130" class="country" Group="HomoClave">
                                                                        </select>
                                                                        <input id="selectedHCountry" type="hidden" runat="server" value="" />
                                                                    </td>
                                                                    <td valign="top" align="left">
                                                                        <asp:Label ID="lblHContact" runat="server" Text="Contacto" CssClass="clslabel" EnableViewState="False"></asp:Label>
                                                                        <asp:RequiredFieldValidator ID="rfvHContact" runat="server" ErrorMessage="Campo requerido"
                                                                            ControlToValidate="txtHContact" EnableViewState="false" ValidationGroup="HAgreement"></asp:RequiredFieldValidator><br />
                                                                        <asp:TextBox ID="txtHContact" runat="server" Width="250px" MaxLength="50" TabIndex="134"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left">
                                                                        <asp:Label ID="lblHAddress" runat="server" Text="Domicilio" CssClass="clslabel" EnableViewState="False"></asp:Label><br />
                                                                        <asp:RequiredFieldValidator ID="rfvHAddress" runat="server" ErrorMessage="Campo requerido"
                                                                            ControlToValidate="txtHAddress" EnableViewState="false" ValidationGroup="HAgreement"></asp:RequiredFieldValidator><br />
                                                                        <asp:TextBox ID="txtHAddress" runat="server" Width="250px" MaxLength="50" TabIndex="126"></asp:TextBox>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:Label ID="lblHState" runat="server" Text="Estado" CssClass="clslabel" EnableViewState="False"></asp:Label><br />
                                                                        
                                                                        <select id="ddlHStates" runat="server" style="width: 250px;" tabindex="131" class="state" Group="HomoClave">
                                                                        </select>
                                                                        <input id="selectedHState" type="hidden" runat="server" value="" />
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:Label ID="lblHJob" runat="server" Text="Puesto" CssClass="clslabel" EnableViewState="False"></asp:Label>
                                                                        <asp:RequiredFieldValidator ID="rfvHJob" runat="server" ErrorMessage="Campo requerido"
                                                                            ControlToValidate="txtHJob" EnableViewState="false" ValidationGroup="HAgreement"></asp:RequiredFieldValidator><br />
                                                                        <asp:TextBox ID="txtHJob" runat="server" Width="250px" MaxLength="50" TabIndex="135"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left">
                                                                        <asp:Label ID="lblHTown" runat="server" Text="Colonia" CssClass="clslabel" EnableViewState="False"></asp:Label><br />
                                                                        <asp:RequiredFieldValidator ID="rfvHTown" runat="server" ErrorMessage="Campo requerido"
                                                                            ControlToValidate="txtHTown" EnableViewState="false" ValidationGroup="HAgreement"></asp:RequiredFieldValidator><br />
                                                                        <asp:TextBox ID="txtHTown" runat="server" Width="250px" MaxLength="20" TabIndex="127"></asp:TextBox>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:Label ID="lblHDistrict" runat="server" Text="Municipio" CssClass="clslabel" EnableViewState="False"></asp:Label><br />
                                                                        
                                                                        <select id="ddlHDistricts" runat="server" style="width: 250px;" tabindex="132" class="district" Group="HomoClave">
                                                                        </select>
                                                                        <input id="selectedHDistrict" type="hidden" runat="server" value="" />
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:Label ID="lblHEmail" runat="server" Text="Correo Electronico" CssClass="clslabel"
                                                                            EnableViewState="False"></asp:Label>
                                                                        <asp:RequiredFieldValidator ID="rfvHEmail" runat="server" ErrorMessage="Campo requerido"
                                                                            ControlToValidate="txtHEmail" EnableViewState="false" ValidationGroup="HAgreement"></asp:RequiredFieldValidator><br />
                                                                        <asp:RegularExpressionValidator ID="revHEmail" runat="server" ErrorMessage="Correo electronico invalido"
                                                                            ControlToValidate="txtHEmail" ValidationExpression="^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"
                                                                            ValidationGroup="HAgreement"></asp:RegularExpressionValidator><br />
                                                                        <asp:TextBox ID="txtHEmail" runat="server" Width="250px" MaxLength="50" TabIndex="136"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left">
                                                                        <asp:Label ID="lblHZIP" runat="server" Text="Codigo Postal" CssClass="clslabel" EnableViewState="False"></asp:Label><br />
                                                                        <asp:RequiredFieldValidator ID="rfvHZIP" runat="server" ErrorMessage="Campo requerido"
                                                                            ControlToValidate="txtHZIP" EnableViewState="false" ValidationGroup="HAgreement"></asp:RequiredFieldValidator><br />
                                                                        <asp:TextBox ID="txtHZIP" runat="server" Width="150px" MaxLength="11" TabIndex="128"></asp:TextBox>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:Label ID="lblHCity" runat="server" Text="Ciudad" CssClass="clslabel" EnableViewState="False"></asp:Label><br />
                                                                        
                                                                        <select id="ddlHCities" runat="server" style="width: 250px;" tabindex="133" class="city" Group="HomoClave">
                                                                        </select>
                                                                       <%--<asp:CustomValidator Display="Dynamic" ID="rfvHCity" runat="server" ErrorMessage="Campo requerido"
                                                                            EnableViewState="false" ValidationGroup="HAgreement" ClientValidationFunction="ValidateCity"></asp:CustomValidator>
                                                                        --%>
                                                                        <input id="selectedHCity" type="hidden" runat="server" value="" Group="HomoClave" class="SelectedCity" />
                                                                    </td>
                                                                    <td align="left">
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left">
                                                                        <asp:Label ID="lblHPhone" runat="server" Text="Telefono" CssClass="clslabel" EnableViewState="False"></asp:Label><br />
                                                                        <asp:RequiredFieldValidator ID="rfvHPhone" runat="server" ErrorMessage="Campo requerido"
                                                                            ControlToValidate="txtHPhone" EnableViewState="false" ValidationGroup="HAgreement"></asp:RequiredFieldValidator><br />
                                                                        <asp:TextBox ID="txtHPhone" runat="server" Width="150px" MaxLength="25" TabIndex="129"></asp:TextBox>
                                                                    </td>
                                                                    <td align="left">
                                                                        
                                                                    </td>
                                                                    <td align="left">
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="3"><asp:Label ID="lblHError" runat="server" CssClass="Validators" ></asp:Label></td>
                                                                </tr>
                                                            </table>
                                                         
                                                        </td>
                                                    </tr>
                                                </tbody>
                                                <tfoot>
                                                    <tr>
                                                        <th align="center">
                                                           <asp:Button ID="bnAgregarHomoClave" runat="server" Text="Agregar HomoClave" ValidationGroup="HAgreement"
                                                                class="Button" Visible="false"  />
                                                        </th>
                                                    </tr>
                                                </tfoot>
                                            </table>
                                        </div>
                                </td>
                            </tr>
                            
                            
                            <tr>
                                <td colspan="3">
                                    <br />
                                    <br />
                                    <br />
                                    <div style="float: left">
                                        <asp:Label ID="lblTitleHotels" runat="server" Text="Seleccione los planes que aplicarán en el convenio"
                                            CssClass="bookingNormalLabel" EnableViewState="False"></asp:Label>&nbsp;
                                        <asp:Label ID="lblTip" runat="server" Text="Para agregar un plan tarifario o una restricción especifica seleccione la imagen [+]"
                                            CssClass="clslabel" EnableViewState="False" Visible="false"></asp:Label>
                                        
                                    </div>
                                    <br />
                                    <hr />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Panel runat="server" DefaultButton="btnTarifaCovnenio" >
                                        <asp:Label ID="lblTarifaConvenio" runat="server" Text="Buscar Plan tarifario"></asp:Label>
                                        <asp:TextBox ID="txtTarifaConvenio" runat="server" ></asp:TextBox> 
                                        <asp:RequiredFieldValidator ID="rqfTarifaConvenio" runat="server" ErrorMessage="*"
                                            ControlToValidate="txtTarifaConvenio" EnableViewState="false" ValidationGroup="TarifaConvenio"></asp:RequiredFieldValidator>
                                        <asp:Button ID="btnTarifaCovnenio" runat="server" Text="Buscar" CssClass="Button" ValidationGroup="TarifaConvenio" style="float:none;" /> 
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3"> 
                                    <asp:DataList ID="dlRatesPlan" runat="server" RepeatDirection="Horizontal" >
                                        <SeparatorStyle Width="9px"  />
                                        <SeparatorTemplate>, </SeparatorTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"IdRatePlan") %>' OnClick="LinkRatePlan_Click"  ></asp:LinkButton>
                                        </ItemTemplate>
                                        <AlternatingItemStyle Width="4px" />
                                    </asp:DataList>
                                    <br />
                                    <asp:DataList ID="dlHoteles" runat="server" RepeatColumns="3">
                                    <ItemStyle CssClass="dgItem"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkAllHotels" runat="server" Text="Seleccionar todos" onclick="CheckAllHotels(this)"/>
                                    </HeaderTemplate>
                                    
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chk" runat="server" onclick="chkHotel(this)" style="" />
                                        <%#Eval("Hotel")%>                                        
                                        <asp:HiddenField ID="Hotel" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"idHotel") %>'></asp:HiddenField>
                                        <asp:HiddenField ID="IniState" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"Check") %>'></asp:HiddenField>
                                        <asp:HiddenField ID="RateCode" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"idRatePlan") %>'></asp:HiddenField>
                                    </ItemTemplate>
                                    
                                    </asp:DataList>
                                    <asp:Label ID="lblMsgNumHoteles" runat="server"></asp:Label>
                                    <br />
                                    <br />
                                    <asp:Label ID="lblMsgErrorNumHoteles" runat="server"></asp:Label>
                                </td>
                            </tr>
                           
                            <tr id="trEspecificRestrictions" style="display: none">
                                <td colspan="3" align="left">
                                    <br />
                                    <asp:Label ID="lblEspecificRestrictions" runat="server" Text="Restricciones Especificas"
                                        CssClass="clslabel" EnableViewState="False"></asp:Label><br />
                                    <textarea id="txtEspecificRestrictions" class="txtRestriction" rows="3" cols="100"
                                        onkeyup="maxLength(event,this,250);" onkeydown="maxLength(event,this,250);" maxlength="250"
                                        style="display: none"></textarea>
                                    <input type="button" id="btnEspecificRestrictions" value='<%= PortalCulture.GetString("A00673") %>'
                                        onclick="AddRestriction(false,'txtEspecificRestrictions','<%= hdnEspecificRestrictions.ClientID %>');"
                                        style="display: none" />
                                    <asp:HiddenField ID="hdnEspecificRestrictions" runat="server" />
                                    <asp:HiddenField ID="hdnUsedEspecificRestrictions" runat="server" />
                                    <table id="tbEspecificRestrictions" class="tableRestrictions">
                                        <tbody>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">                                    
                                    <div style="float: right; border: 1px solid #8ebdd9;border-radius: 4px;">
                                        <%
                                            Me.imgSave.OnClientClick = "return ShowMsgPrint(this,true);"
                                            Me.imgSave.ToolTip = PortalCulture.GetString("A00153")
                                            'Me.imgPrint.ToolTip = PortalCulture.GetString("00606")
                                                Me.imgCancel.ToolTip = PortalCulture.GetString("A00143")
                                                Me.imgDelete.ToolTip = PortalCulture.GetString("00103")
                                        %>
                                         <asp:ImageButton ID="imgSave" runat="server" ImageUrl="~/Images/save.png" EnableViewState="False"
                                            ValidationGroup="Agreement" CausesValidation="true" TabIndex="20" />
                                        
                                        <input type="hidden" id="printQuestionShowed" value="True" />
                                        <input type="hidden" id="valShowValidators" name="ShowValidators" value="False" />
                                        <asp:ImageButton ID="ImgSaveNoPrint" runat="server" ImageUrl="~/Images/sav2.gif"
                                            EnableViewState="False" ValidationGroup="Agreement" TabIndex="21" CausesValidation="true"
                                            Style="display: none;" />
                                        <%--<asp:ImageButton ID="imgPrint" runat="server" ImageUrl="~/Images/print.gif" EnableViewState="False"
                                            ValidationGroup="Agreement" TabIndex="21" CausesValidation="true" />
                                            --%>
                                        <asp:ImageButton ID="imgDelete" runat="server" ImageUrl="~/Images/delete.gif" EnableViewState="False"
                                            ValidationGroup="CancelAgreement" OnClientClick="return DeleteAgreement();" TabIndex="22"
                                            Visible="false" />
                                        <asp:ImageButton ID="imgCancel" runat="server" ImageUrl="~/Images/Close.png" EnableViewState="False"
                                            ValidationGroup="CancelAgreement" TabIndex="23" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <div id="BackgroundLayer" style="display: none; position: absolute; position: fixed;
                            top: 0px; left: 0px; background-color: Gray; width: 100%; z-index: 1000; filter: alpha(opacity=50);
                            -moz-opacity: 0.5; opacity: 0.5; -khtml-opacity: 0.5">
                        </div>
                        <div id="RateSearch" class="RateSearch" style="z-index: 1001; display: none; ">
                            <a onclick='CloseRate();' style="cursor: hand; float: right;">
                                <img src="../Images/close.png" /></a>
                            <table align="center" id="tbRateSearch">
                                <thead>
                                    <tr>
                                        <th align="center">
                                            <h2>
                                                <%=PortalCulture.GetString("01200")%></h2>
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td align="left">
                                            <asp:Label ID="lblRateSearch" runat="server" Text="Plan Tarifario" CssClass="clslabel"
                                                EnableViewState="False"></asp:Label><br />
                                            <input type="text" id="txtRateSearch" autocomplete="off" class="ac_input" style="width: 200px;
                                                font-size: 12px;" />
                                            <input type="hidden" id="idHotel" value="-1" />
                                            <input type="hidden" id="idRateSearch" value="-1" />
                                            <input type="hidden" id="idChkHotel" value="" />
                                            <input type="hidden" id="idTableRates" value="" />
                                            <input type="hidden" id="idHotelRates" value="" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <a onclick='ShowRatesPlan();' style="cursor: hand; float: right;" class="dgLink">
                                                <%=PortalCulture.GetString("01222")%></a>
                                        </td>
                                    </tr>
                                </tbody>
                                <tfoot>
                                    <tr>
                                        <th align="center">
                                            <input type="button" id="btnRateSearch" value='<%=PortalCulture.GetString("M000153")%>'
                                                class="Button" onclick="SearchRate();" />
                                        </th>
                                    </tr>
                                </tfoot>
                            </table>
                            <table align="center" id="tbRatesPlan" style="display: none">
                                <thead>
                                    <tr>
                                        <th align="center">
                                            <h2>
                                                <%=PortalCulture.GetString("01200")%></h2>
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td id="tdRatesPlan">
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <div id="ListRestrictions" class="ListRestrictions" style="z-index: 1001; display: none;">
                            <img src="../Images/close.png" onclick='CloseListRestrictions();' style="cursor: hand;
                                float: right;" />
                            <table id="tbListRestrictions" align="center">
                                <thead>
                                    <tr>
                                        <th align="center">
                                            <h2>
                                                <%=PortalCulture.GetString("01190")%></h2>
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                </tbody>
                                <tfoot>
                                    <tr>
                                        <th>
                                            <input type="button" id="btnAddNewEspecificRestriction" value='<%=PortalCulture.GetString("01203")%>'
                                                class="Button" onclick="ShowAddEspecificRestriction();" />
                                            <input type="button" id="btnListRestrictions" value='<%=PortalCulture.GetString("M000153")%>'
                                                class="Button" onclick="SelectRestriction();" />
                                            <input type="hidden" id="idHotelRestrictions" value="-1" />
                                            <input type="hidden" id="idRatePlanRestrictions" value="-1" />
                                            <input type="hidden" id="idRateRestrictions" value="-1" />
                                            <input type="hidden" id="idTypeRoomRestrictions" value="-1" />
                                            <input type="hidden" id="idRestrictionRestrictions" value="-1" />
                                            <input type="hidden" id="isException" value="-1" />
                                        </th>
                                    </tr>
                                </tfoot>
                            </table>
                            <table id="tbAddEspecificRestriction" align="center">
                                <thead>
                                    <tr>
                                        <th align="center">
                                            <h2>
                                                <%=PortalCulture.GetString("01190")%></h2>
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td>
                                            <textarea id="txtListEspecificRestriction" rows="3" cols="25" onkeyup="maxLength(event,this,250);"
                                                onkeydown="maxLength(event,this,250);" maxlength="250"></textarea>
                                            <table id="tbListEspecificRestrictions" class="tableEspecificRestrictions">
                                                <tbody>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                </tbody>
                                <tfoot>
                                    <tr>
                                        <th>
                                            <input type="button" id="btnAddEspecificRestriction" value='<%= PortalCulture.GetString("A00673") %>'
                                                onclick="AddRestriction(false,'txtListEspecificRestriction','<%= hdnEspecificRestrictions.ClientID %>');/*ShowListRestrictions($('#idHotelRestrictions').val(), $('#idRatePlanRestrictions').val(), $('#idRateRestrictions').val(), $('#idTypeRoomRestrictions').val(), $('#idRestrictionRestrictions').val(), $('#isException').val());*/"
                                                class="Button" />
                                        </th>
                                    </tr>
                                </tfoot>
                            </table>
                        </div>
                        <div id="MsgSave" class="MsgSave" style="z-index: 1001; display: none;">
                            <table align="center">
                                <tbody>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblMsg" runat="server" Text="Mensaje" CssClass="clslabel" EnableViewState="False"></asp:Label>
                                        </td>
                                    </tr>
                                </tbody>
                                <tfoot>
                                    <tr>
                                        <th>
                                            <br />
                                            <input type="button" id="btnAccept" value='<%=PortalCulture.GetString("M000153")%>'
                                                class="Button" onclick="CloseMsgSave();" />
                                        </th>
                                    </tr>
                                </tfoot>
                            </table>
                        </div>
                        <div id="MsgPrint" style="z-index: 1001; display: none;">                            
	                        <div class="boxMsg">
                                <table align="center">
                                <tbody>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblQuestion" runat="server" CssClass="clslabel" EnableViewState="False"><%=PortalCulture.GetString("01583")%></asp:Label>
                                        </td>
                                    </tr>
                                </tbody>
                                <tfoot>
                                    <tr>
                                        <th align="center">
                                            <br />
                                            <%--<input type="button" id="btnYesPrint" value='<%=PortalCulture.GetString("00030")%>'
                                                class="Button" onclick="ShowMsgPrint(this, false); " />
                                            &nbsp; --%>
                                            <input type="button" id="btnNoPrint" value='<%=PortalCulture.GetString("00030")%>'
                                                class="Button" onclick="ShowMsgPrint(this, false); " style="float:none;width:80px" />
                                            &nbsp;
                                            <input type="button" id="btnCancel" value='<%=PortalCulture.GetString("A00143")%>'
                                                class="Button" onclick="ShowMsgPrint(this, false); "style="float:none;width:80px" />
                                        </th>
                                    </tr>
                                </tfoot>
                            </table>
                            </div>                            
                        </div>
                        <div id="quote" style="display: none;">
                        </div>
                        <asp:HiddenField ID="hdnIdAgreement" runat="server" Value="0" />
                        <asp:HiddenField ID="isPrinter" runat="server" Value="0" />
                    </div>
                    <div id="ReportAgreement" runat="server" visible="false" class="cntReport">
                        <asp:Button ID="cmdBack" runat="server" CssClass="lnkButton" Text="Back" EnableViewState="False" />
                        <div class="crReport">
                            <!--<CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true"
                                EnableDrillDown="False" EnableParameterPrompt="False" EnableDatabaseLogonPrompt="False"
                                HasCrystalLogo="False" HasViewList="False" DisplayGroupTree="False" HasDrillUpButton="False"
                                ReuseParameterValuesOnRefresh="True" EnableToolTips="False" HasToggleGroupTreeButton="False"
                                Height="50px" Width="350px" />-->
                        </div>
                    </div>
                </td>
            </tr>
        </table>
        <asp:DataGrid ID="dgRatesPlan" runat="server" CssClass="DataGrid" AutoGenerateColumns="False"
            AllowPaging="False" ShowFooter="False" Visible="false" EnableViewState="false"
            Width="98%">
            <FooterStyle HorizontalAlign="Right"></FooterStyle>
            <SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
            <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
            <ItemStyle CssClass="dgItem"></ItemStyle>
            <HeaderStyle CssClass="dgHeader"></HeaderStyle>
            <Columns>
                <asp:TemplateColumn HeaderStyle-Width="400" ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:Label ID="RatePlan" runat="server" Text='<%# Container.dataItem("RatePlanCode") & " - " & Container.dataItem("name") %>'
                            EnableViewState="False"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderStyle-Width="100" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <a onclick='<%# "javascript:document.getElementById(""idRateSearch"").value=""" & Container.dataItem("idRatePlan") & """;SearchRate();" %>'
                            style="cursor: hand;" class="dgLink">
                            <%= PortalCulture.GetString("M000640") %></a>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>
    </div>
    <%--        <div id="imgLoading" style="display:none; position:absolute; z-index:2;">
            <img style="vertical-align:middle;" src="images/indicator.gif">
        </div>--%>
    <iframe id="gToday:normal:agenda.js" style="z-index: 999; left: -500px; position: absolute;
        top: -500px" name="gToday:normal:agenda.js" src='<%=GeRequestApplicationPath(string.concat("/Calendar/",portalculture.getculture().Name.Substring(0, 2).ToLower(),"/ipopeng2.htm" )) %>'
        frameborder="0" width="174" scrolling="no" height="189"></iframe>

    <script type='text/javascript'>
                var parametros = new Object();
                parametros.exists = true;
                parametros.baseUrl = "<%= Request.ApplicationPath %>";
                if (!(parametros.baseUrl.match('/$') == '/')) { parametros.baseUrl += '/'; }
                parametros.lang = "<%= PortalCulture.GetIDCulture().ToString() %>";
                parametros.controls = new Object();
                parametros.controls.city = "SelectedCity";
                parametros.controls.district = "<%= me.selectedDistrict.clientId %>";
                parametros.controls.state = "<%= me.selectedState.clientId %>";
                parametros.controls.country = "<%= me.selectedCountry.clientId %>";
               // parametros.controls.city2 = "<%= me.selectedHCity.clientId %>";

                SetParameters(parametros);

                var validators = new Array('<%= rfvAgency.ClientId %>', '<%= rfvContact.ClientId %>', '<%= rfvJob.ClientId %>', '<%= revEmail.ClientId %>', '<%= rfvCity.ClientId %>');
                

                $(document).ready(
                    function() {
                        $('#valShowValidators').val('false');
<%-- CV                        $('#<%= Me.imgCancel.ClientId %>, #<%= Me.imgPrint.ClientId %>').click(function() {$('#valShowValidators').val('true');}); 
//                      viktor  $('#<%= Me.imgSave.ClientId %>, #<%= Me.imgPrint.ClientId %>').click(function() { if(!Page_IsValid) { window.location = '#aTop';} })
                    $('#<%= Me.ImgSaveNoPrint.ClientId %>, #<%= Me.imgPrint.ClientId %>').click(function() { if(!Page_IsValid) { window.location = '#aTop';} }) --%>
                        var officeSelected = $('#<%= Me.txtOwnerOffice.ClientId %>').val();
                        var contactSelected = $('#<%= Me.txtOwnerOfficeContact.ClientId %>').val();
                    
                        var corporate = <%= if(Me.idSelectedCorpororate.Value.Length > 0, Me.idSelectedCorpororate.Value, "0") %>;
                        $('#lstOwnerOffice').change(function() {
                            var item = $(this).find(':selected').data('info');
                            $('#pnlOwnerContact').hide();
                            $('#pnlOwnerOfficeContact').hide();
                            if (item == null || item.id == null || item.id == 0) {
                                $('#<%= Me.txtOwnerOffice.ClientId %>').val(0);
                                $('#<%= Me.txtCorporateContact.ClientId %>').val('');
                                $('#<%= Me.txtCorporateJob.ClientId %>').val('');
                                ValidatorValidate($('#<%= rfvCorporateContact.ClientId %>')[0]);
                            } else if (item.id == -1) {
                                $('#<%= Me.txtOwnerOffice.ClientId %>').val(-1);
                                if(officeSelected == 0){
                                    $('#<%= Me.txtCorporateContact.ClientId %>').val('');
                                    $('#<%= Me.txtCorporateJob.ClientId %>').val('');
                                }
                                $('#pnlOwnerContact').show();
                            } else {
                                $('#<%= Me.txtOwnerOffice.ClientId %>').val(item.id);
                                $('#<%= Me.txtCorporateContact.ClientId %>').val(item.name);
                                $('#<%= Me.txtCorporateJob.ClientId %>').val('');
                                ValidatorValidate($('#<%= rfvCorporateContact.ClientId %>')[0]);
                                $.ajax({
                                    url: 'convenios.aspx',
                                    dataType: 'json',
                                    type: 'GET',
                                    data: { a: 'getcontacts', o: item.id },
                                    success: function(data, textStatus, XMLHttpRequest) {
                                        if (data.error != null) {
                                            alert(data.error);
                                        } else {
                                            var list = $('#lstOwnerOfficeContact');
                                            list.find('option').remove();
                                            list.append($('<option></option>').val(0).html('-- <%= PortalCulture.GetString("00753") %> --').data('info', {id:0, name:''}));
                                            $.each(data, function() {
                                                var item = $('<option></option>');
                                                item.val(this.id);
                                                item.html(this.name + (this.position.length > 0 ? ' (' + this.position + ')' : ''));
                                                item.data('info', this);
                                                list.append(item);
                                            });
                                            if(list.find('option').length > 1){
                                                $('#pnlOwnerOfficeContact').show();
                                            }
                                            if(contactSelected > 0) {
                                                list.val(contactSelected);
                                                list.change();
                                            }
                                        }
                                    }
                                });
                            }
                            officeSelected = 0;
                        });

                        $('#lstOwnerOfficeContact').change(function() {
                            var item = $(this).find(':selected').data('info');
                            if (item == null || item.id == null || item.id == 0) {
                                $('#<%= Me.txtOwnerOfficeContact.ClientId %>').val(0);
                                item = $('#lstOwnerOffice').find(':selected').data('info');
                            } else {
                                $('#<%= Me.txtOwnerOfficeContact.ClientId %>').val(item.id);
                            }
                            $('#<%= Me.txtCorporateContact.ClientId %>').val(item != null ? item.name: '');
                            $('#<%= Me.txtCorporateJob.ClientId %>').val(item == null || item.position == null ? '' : item.position);
                            contactSelected = 0;
                        });
                        
                        if(corporate != null && corporate > 0){
                        
                            $('#pnlOwnerOffice').hide();
                            $('#pnlOwnerContact').hide();
                            $('#pnlOwnerOfficeContact').hide();
                        
                            $.ajax({
                                url: 'convenios.aspx',
                                dataType: 'json',
                                type: 'GET',
                                data: { a: 'getoffices', c: corporate },
                                success: function(data, textStatus, XMLHttpRequest) {
                                    if (data.error != null) {
                                        alert(data.error);
                                    } else if(data.length > 0){      
                                        var list = $('#lstOwnerOffice');
                                        list.find('option').remove();
                                        list.append($('<option></option>').val(0).html('-- <%= PortalCulture.GetString("00753") %> --'));
                                        $.each(data, function() {
                                            var item = $('<option></option>');
                                            item.val(this.id);
                                            item.html(this.name);
                                            item.data('info', this);
                                            list.append(item);
                                        });
                                        list.append($('<option></option>').val(-1).html('<%= PortalCulture.GetString("00027") %>').data('info', { id: -1, name: '' }));                                  
                                        $('#pnlOwnerOffice').show();
                                        $('#lstOwnerOffice').val(officeSelected);
                                        if(officeSelected != 0) {
                                            $('#lstOwnerOffice').change();
                                        }                                
                                    } else {
                                        $('#<%= Me.txtOwnerOffice.ClientId %>').val(-1);
                                        $('#pnlOwnerContact').show();
                                    }
                                }
                            });
                        }
                        
                        /*var control = $('#<%= Me.chkIsGeneral.ClientId %>');
						control.click(function() {
                            for (i = 0; i < validators.length; i++) {
                                if (document.getElementById(validators[i]) != null) {
                                    ValidatorEnable(document.getElementById(validators[i]), !$(this).is(':checked'));                                    
                                }
                            }
                            if($(this).is(':checked')){
                                <%--$('#<%= Me.imgPrint.ClientId %>').hide(); --%>
                                $('#pnlMainForm').hide();
                            }
                            else{
                                <%-- $('#<%= Me.imgPrint.ClientId %>').show(); --%>
                                $('#pnlMainForm').show();
                            }
                        });
						control.click();
						<% If Me.Request.Form("ShowValidators") is Nothing orelse not Me.Request.Form("ShowValidators").ToString().ToLower() = "true" Then %>
						$.each(validators, function() {
						    $('#' + this).css('visibility', 'hidden');
						});                        
                        <% End If %>
						if (control.is(':checked'))
							control.removeAttr('checked'); 
						else   
							control.attr('checked', 'checked'); */
							
                    }
                    
                );
                
    </script>

    <script type="text/javascript">
        $().ready(function() {
            $(document).keypress(function(e) {
                if (e.keyCode == 13) {
                    e.cancelBubble = true;
                    e.returnvalue = false;
                    return false;
                }
            });

            $("#txtRateSearch").autocomplete("convenios.aspx", {
                extraParams: { a: "SearchRates", h: function() { return $("#idHotel").val(); } },
                dataType: 'xml',
                type: 'GET',
                width: 200,
                minChars: 2,
                delay: 200,
                noCache: true,
                mustMatch: false,
                selectFirst: true
            }).result(function(event, data, formatted) {
                if (data) {
                    $("#txtRateSearch").val(data[0]);
                    $("#idRateSearch").val(data[1]);
                }
            });

        });


        function ShowRate(idTableRates, idHotelRates, idHotel, idChkHotel) {
            var pos = new Object(); ;
            pos.left = 0;
            pos.top = 0; //viktor                    

            document.getElementById('idTableRates').value = idTableRates;
            document.getElementById('idHotelRates').value = idHotelRates;
            document.getElementById('idChkHotel').value = idChkHotel;
            document.getElementById('idHotel').value = idHotel;
//            $("#txtRateSearch").flushCache();
            ResetRateSearch('');
            document.onmouseup = getmousePos;
            getMousePositionDetailPos(pos, 600);
            var lixlpixel_tooltip = document.getElementById('RateSearch');
            if (lixlpixel_tooltip) {
                lixlpixel_tooltip.style.left = pos.left;
                lixlpixel_tooltip.style.top = pos.top;
            }
            //onResizeIframe(200);
        }

        function CloseRate() {
            document.getElementById('idTableRates').value = '';
            document.getElementById('idHotelRates').value = '';
            document.getElementById('idChkHotel').value = '';
            document.getElementById('idHotel').value = '-1';
            ResetRateSearch('none');
        }

        function chkHotel(chk) {
            $("#dlHoteles_ctl00_chkAllHotels").removeAttr("checked");
        }
        

        function CheckAllHotels(chk) {
            if (chk.checked)
                $("#dlHoteles input").attr("checked", "checked");
            else
                $("#dlHoteles input").removeAttr("checked");
        }
        
         function ShowAllAgreements(idCorporate) {
            $("#divAllAgreementsContent").html("<img style='vertical-align:middle;' src='images/indicator.gif'>");
            LoadAgreements(idCorporate);
            $("#<%= divAllAgreements.ClientID%>").css("display", "inline");
        }
         function CloseAllAgreements() {
            $("#divAllAgreementsContent").html("");
            $("#<%= divAllAgreements.ClientID%>").css("display", "none");
            onResizeIframe();
        }
        
        function ShowHomoClave() {
            

            $("#<%=txtHAgency.ClientID %>").val("");
            $("#<%=txtHAddress.ClientID %>").val("");
            $("#<%=txtHTown.ClientID %>").val("");
            $("#<%=txtHZIP.ClientID %>").val("");
            $("#<%=txtHPhone.ClientID %>").val("");
            $("#<%=txtHContact.ClientID %>").val("");
            $("#<%=txtHJob.ClientID %>").val("");
            $("#<%=txtHEmail.ClientID %>").val("");
            $("#<%=txtHomoClave.ClientID %>").val("");

            $("#<%= divHomoclave.ClientID%>").css("display", "inline");
        }
        function CloseHomoClave() {
            $("#<%= divHomoclave.ClientID%>").css("display", "none");
        }

       
        
        function SelectHotel(chkHotel, idHotel, idTable) {
            if (chkHotel.checked)
                BuildRatesPlanTable(idHotel, idTable);
            document.getElementById("Rates_" + idTable).style.display = (chkHotel.checked ? '' : 'none');
            document.getElementById('Plus_' + idHotel).src = (chkHotel.checked ? '../Includes/imagenes/menos.png' : '../Includes/imagenes/mas.png');

        }

        function ResetRateSearch(display) {
            document.getElementById('txtRateSearch').value = '';
            document.getElementById('idRateSearch').value = '-1';
            document.getElementById('BackgroundLayer').style.display = display;
            document.getElementById('RateSearch').style.display = display;
            document.getElementById('tbRateSearch').style.display = display;
            $("#tbRatesPlan").hide();
        }

        function SearchRate() {
            var idRateSearch = document.getElementById('idRateSearch').value;
            if (idRateSearch != '-1') {
                var HotelRates = document.getElementById(document.getElementById('idHotelRates').value);
                var findRate = false;
                var arrayHotelRates = new Array();
                if (HotelRates.value != '') {
                    arrayHotelRates = StringToObject(HotelRates.value, 'Object');
                    for (var i = 0; i < arrayHotelRates.length; i++)
                        if (arrayHotelRates[i].IdRatePlan == idRateSearch)
                        findRate = true;
                }

                if (!findRate) {
                    var idHotel = document.getElementById('idHotel').value;
                    $("#quote").load("convenios.aspx?a=SearchRate&ag=" + "<%= idConvenio%>" + "&h=" + idHotel + "&r=" + idRateSearch, function() {
                        var ratePlan = StringToObject($("#quote").text(), 'Object');
                        if (ratePlan.Rates) {
                            arrayHotelRates[arrayHotelRates.length] = ratePlan;
                            HotelRates.value = ObjectToString(arrayHotelRates);
                            AddRatePlan(document.getElementById('idTableRates').value, ratePlan);
                            var idChkHotel = document.getElementById('idChkHotel').value;
                            if (!document.getElementById(idChkHotel).checked) document.getElementById(idChkHotel).click();
                            CloseRate();
                        } else {
                            alert(alert1);
                        }
                    });
                } else {
                    alert(alert2);
                }
            } else {
                alert(alert3);
            }
            //onResizeIframe(200);
        }

        function AddRatePlan(idTable, ratePlan) {
            for (var i = 0; i < ratePlan.Rates.length; i++) {
                for (var j = 0; j < ratePlan.Rates[i].Restrictions.length; j++) {
                    var countRows = $("#Rates_" + idTable).find('tbody').find('tr').length;
                    var row = $('<tr>');
                    $("#Rates_" + idTable).find('tbody').append(row);
                    if (i == 0 && j == 0)
                        row.append($('<td valign=\'top\'>').attr({ rowspan: ratePlan.TotalRatesRestrictions, style: 'position:relative;' })
                                    .append($('<div>').attr({ style: 'position:relative; width:100%; ' })
					                .append($('<div>').html(ratePlan.RateCode + ",<br/>" + ratePlan.Name))
					                .append($('<div>').attr({ style: 'position:absolute; top:0; right:0;' }).append($('<img src=\'../Images/delete.gif\' onclick=\'DeleteRatePlan(' + idTable + ',' + ratePlan.IdHotel + ',"' + ratePlan.IdRatePlan + '");\' style=\'cursor:hand;\ alt=\'' + alt2 + '\' title=\'' + title2 + '\'>')))
					                )
				                );

                    row.append($('<td valign=\'top\'>')
				                .append($('<div>').attr({ style: 'float:left;' }).html('<strong id=\'' + ratePlan.IdHotel + '_' + ratePlan.IdRatePlan + '_' + ratePlan.Rates[i].IdRate + '_' + ratePlan.Rates[i].Restrictions[j].IdRoomType + '_' + ratePlan.Rates[i].Restrictions[j].IdsRestrictions + '_' + ratePlan.Rates[i].Restrictions[j].isException + '\'>' + ratePlan.Rates[i].Restrictions[j].Exceptions + '&nbsp;<span class="reference"></span></strong><br/>' + ratePlan.Rates[i].Restrictions[j].NameRoom))
				                .append($('<div>').attr({ style: 'float:right;' }).append($('<img src=\'../Images/star.gif\' onclick=\'ShowListRestrictions(' + ratePlan.IdHotel + ',"' + ratePlan.IdRatePlan + '",' + ratePlan.Rates[i].IdRate + ',' + ratePlan.Rates[i].Restrictions[j].IdRoomType + ',"' + ratePlan.Rates[i].Restrictions[j].IdsRestrictions + '",' + ratePlan.Rates[i].Restrictions[j].isException + ');\' style=\'cursor:hand;\' alt=\'' + alt1 + '\' title=\'' + title1 + '\'>')))
			                );

                    var ChildrenRate = '';
                    var TeenRate = '';
                    var rateRange = '';

                    var restriction = ratePlan.Rates[i].Restrictions[j];

                    if (!restriction.ChildrenRates) { restriction.ChildrenRates = new Array(); }

                    for (var k = 0; k < restriction.ChildrenRates.length; k++) {
                        if (k + 1 < restriction.ChildrenRates.length && restriction.ChildrenRates[k].Rate == restriction.ChildrenRates[k + 1].Rate) {
                            rateRange = (rateRange == '' ? restriction.ChildrenRates[k].Count : rateRange);
                        } else {
                            rateRange += (rateRange == '' ? '' : '-') + restriction.ChildrenRates[k].Count;
                            ChildrenRate += ((restriction.isException && restriction.ChildrenRates[k].RateExc > 0) || (!restriction.isException && restriction.ChildrenRates[k].Rate > 0) ? '<br/>' + label2 + '(' + rateRange + '): ' + (restriction.isException ? restriction.ChildrenRates[k].RateExc.toFixed(2) : restriction.ChildrenRates[k].Rate.toFixed(2)) : '');
                            rateRange = '';
                        }
                    }

                    for (var k = 0; k < restriction.ChildrenRates.length; k++) {
                        if (k + 1 < restriction.ChildrenRates.length && restriction.ChildrenRates[k].TeenRate == restriction.ChildrenRates[k + 1].TeenRate) {
                            rateRange = (rateRange == '' ? restriction.ChildrenRates[k].Count : rateRange);
                        } else {
                            rateRange += (rateRange == '' ? '' : '-') + restriction.ChildrenRates[k].Count;
                            TeenRate += ((restriction.isException && restriction.ChildrenRates[k].TeenRateExc > 0) || (!restriction.isException && restriction.ChildrenRates[k].TeenRate > 0) ? '<br/><%=PortalCulture.GetString("01282")%>(' + rateRange + '): ' + (restriction.isException ? restriction.ChildrenRates[k].TeenRateExc.toFixed(2) : restriction.ChildrenRates[k].TeenRate.toFixed(2)) : '');
                            rateRange = '';
                        }
                    }

                    //row.append($('<td align=\'right\'>').html(label1 + '(' + (ratePlan.Rates[i].Restrictions[j].VariousAdults ? (ratePlan.Rates[i].Restrictions[j].MinAdults == ratePlan.Rates[i].Restrictions[j].MaxAdults ? ratePlan.Rates[i].Restrictions[j].MaxAdults : ratePlan.Rates[i].Restrictions[j].MinAdults + '-' + ratePlan.Rates[i].Restrictions[j].MaxAdults) : ratePlan.Rates[i].Restrictions[j].Adults) + '): ' + eval($("#" + $("#idhotelData_" + idTable).val()).val())[2] + ratePlan.Rates[i].Restrictions[j].AdultRate.toFixed(2) + ChildrenRate + (ratePlan.Rates[i].Restrictions[j].AdultRateExt > 0 ? '<br/>' + label3 + ' ' + eval($("#" + $("#idhotelData_" + idTable).val()).val())[2] + ratePlan.Rates[i].Restrictions[j].AdultRateExt.toFixed(2) : '') + (ratePlan.Rates[i].Restrictions[j].ChildRateExt > 0 ? '<br/>' + label4 + ' ' + eval($("#" + $("#idhotelData_" + idTable).val()).val())[2] + ratePlan.Rates[i].Restrictions[j].ChildRateExt.toFixed(2) : '')));
                    var ratesLabel = label1 + '(' +
                                (
                                    (
                                        restriction.VariousAdults
                                        ?
                                        (restriction.MinAdults == restriction.MaxAdults ? '' : restriction.MinAdults + '-') + restriction.MaxAdults
                                        :
                                        restriction.Adults
                                    )
                                    +
                                    '): '
                                    +
                                    restriction.AdultRate.toFixed(2)
                                    +
                                    ChildrenRate
                                    +
                                    TeenRate
                                    +
                                    (restriction.AdultRateExt > 0 ? '<br/>' + label3 + ' ' + restriction.AdultRateExt.toFixed(2) : '')
                                    +
                                    (restriction.ChildRateExt > 0 ? '<br/>' + label4 + ' ' + restriction.ChildRateExt.toFixed(2) : '')
                                    +
                                    (restriction.TeenRateExt > 0 ? '<br/><%=PortalCulture.GetString("01311")%>: ' + restriction.TeenRateExt.toFixed(2) : '')
                                );


                    row.append($('<td align=\'right\'>').html(ratesLabel));

                    if (j == 0) {
                        var currency = (ratePlan.Currency.length == 0) ? eval($("#" + $("#idhotelData_" + idTable).val()).val())[1] : ratePlan.Currency;
                        row.append($('<td valign=\'top\' align=\'center\'>').attr({ rowspan: ratePlan.Rates[i].Restrictions.length }).html(ratePlan.Rates[i].DateStart + ' - ' + ratePlan.Rates[i].DateEnd + '<br/>' + label5 + ' ' + currency));
                    }

                    if (countRows == 0) {
                        row.append($('<td valign=\'top\' align=\'center\'>').attr({ id: 'Tax_' + ratePlan.IdHotel, rowspan: ratePlan.TotalRatesRestrictions }).html(eval($("#" + $("#idhotelData_" + idTable).val()).val())[0]));
                        //row.append($('<td valign=\'top\' align=\'center\'>').attr({ id: 'Vigencia_' + ratePlan.IdHotel, rowspan: ratePlan.TotalRatesRestrictions }).append($('<input type="text" id="iVigencia_' + ratePlan.IdHotel + "/><A hideFocus onclick="if(self.gfPop)gfPop.fPopCalendar1("""iVigencia_' + ratePlan.IdHotel + '""");return false;" href="javascript:void(0)" ><img src=\'../Calendar/calbtn.gif\' style=\'cursor:hand;\' alt=\'' + alt1 + '\'>')));
                        //row.append($('<td valign=\'top\' align=\'center\'>').attr({ id: 'Vigencia_' + ratePlan.IdHotel, rowspan: ratePlan.TotalRatesRestrictions }).append($('<input type=\'text\' id=\'iVigencia_' + ratePlan.IdHotel + '/><A hideFocus onclick=\'alert();\' href=\'javascript:void(0)\'><img src=\'../Calendar/calbtn.gif\'  style=\'cursor:hand;\'></a>')));
                        row.append($('<td valign=\'top\' align=\'right\'>').attr({ id: 'Vigencia_' + ratePlan.IdHotel, rowspan: ratePlan.TotalRatesRestrictions }).append($('<input onclick="javascript:ShowVigencyCalendar(\'' + ratePlan.IdHotel + '\', \'' + idTable + '\');" onkeypress=\'return false;\' onkeydown=\'return false;\' type=\'text\' id=\'iVigencia_' + ratePlan.IdHotel + '\'/><a onclick="javascript:ShowVigencyCalendar(\'' + ratePlan.IdHotel + '\', \'' + idTable + '\');"><img  class=PopcalTrigger border=0 alt="" align="absMiddle" src="' + parametros.baseUrl + 'Calendar/calbtn.gif"></a>')));
                        $('#iVigencia_' + ratePlan.IdHotel).val(ratePlan.Vigencia);
                        $('#iVigencia_' + ratePlan.IdHotel).css("width", "70px");
                        $('#Vigencia_' + ratePlan.IdHotel + ' input, #Vigencia_' + ratePlan.IdHotel + ' img').css("vertical-align", "middle");
                        $('#Vigencia_' + ratePlan.IdHotel).css("width", "110px");

                        //$('#Vigencia_' + ratePlan.IdHotel + ' a').attr('onclick', "if(self.gfPop){gfPop.fPopCalendar3(document.getElementById('iVigencia_" + ratePlan.IdHotel + "'), document.getElementById('" + $('#idVigencia_' + idTable).val() + "')); alert('');} return false;");
                        //$('#Vigencia_' + ratePlan.IdHotel + ' a, #Vigencia_' + ratePlan.IdHotel + ' input').attr('onclick', "");
                        $('#Vigencia_' + ratePlan.IdHotel + ' a').css('cursor', 'pointer');

                        $('#' + $('#idVigencia_' + idTable).val()).val(ratePlan.Vigencia);
                    }
                    else if (i == 0 && j == 0) {
                        $("#Tax_" + ratePlan.IdHotel).attr({ rowspan: countRows + ratePlan.TotalRatesRestrictions });
                        $("#Vigencia_" + ratePlan.IdHotel).attr({ rowspan: countRows + ratePlan.TotalRatesRestrictions });
                    }


                }
            }
        }

        function DeleteRatePlan(idTable, idHotel, IdRatePlan) {
            if (confirm(alert5)) {
                var HotelRates = document.getElementById(document.getElementById('idhotelRates_' + idTable).value);
                arrayHotelRates = StringToObject(HotelRates.value, 'Object');
                var idRatePlanDetele = -1;
                var findRate = false;
                for (var i = 0; i < arrayHotelRates.length; i++) {
                    if (arrayHotelRates[i].IdRatePlan == IdRatePlan) {
                        findRate = true;
                        idRatePlanDetele = i;
                    }
                }

                if (findRate) {
                    arrayHotelRates.splice(idRatePlanDetele, 1);
                    HotelRates.value = ObjectToString(arrayHotelRates);

                    BuildRatesPlanTable(idHotel, idTable);
                    var UsedEspecificRestrictions = document.getElementById(id1);
                    var usedEspecificRestrictions = StringToObject(UsedEspecificRestrictions.value, 'Array');

                    for (var i = 0; i < usedEspecificRestrictions.length; i++) {
                        if (usedEspecificRestrictions[i][0] == idHotel) {
                            if (usedEspecificRestrictions[i][1] == IdRatePlan) {
                                usedEspecificRestrictions.splice(i, 1);
                                i--;
                            }
                        }
                    }

                    UsedEspecificRestrictions.value = ObjectToString(usedEspecificRestrictions);
                }
            }
        }

        function AddRestriction(isGeneral, idTxt, idHiddenRestriction) {
            var txt, idTable;
            if (isGeneral) {
                txt = document.getElementById(idTxt);
                idTable = "tbGeneralRestrictions";
            } else {
                txt = document.getElementById(idTxt);
                idTable = "tbEspecificRestrictions";
            }

            txt.value = txt.value.replace(/^(\s|\&nbsp;)*|(\s|\&nbsp;)*$/g, "");
            if (txt.value != '') {
                var restrictions = document.getElementById(idHiddenRestriction);
                var arrayRestrictions = StringToObject(restrictions.value, 'Array');

                var nRestrictions = -1;
                $.each(arrayRestrictions, function() {
                    nRestrictions = (this[0] > nRestrictions) ? this[0] : nRestrictions;
                });
                nRestrictions++;

                var pointRestriction = '';
                for (var i = 0; i <= nRestrictions; i++) pointRestriction += '*';

                var newRestriction = (isGeneral ? '' : pointRestriction) + txt.value;
                arrayRestrictions[arrayRestrictions.length] = new Array(nRestrictions, newRestriction);
                txt.value = '';
                restrictions.value = ObjectToString(arrayRestrictions);

                AddRestrictionToTable(idTable, newRestriction, isGeneral, idHiddenRestriction, nRestrictions);

                //Liga directo la restriccion...
                var UsedEspecificRestrictions = document.getElementById(id1);
                var usedRestrictions = StringToObject(UsedEspecificRestrictions.value, 'Array');
                var idHotel = document.getElementById('idHotelRestrictions').value;
                var IdRatePlan = document.getElementById('idRatePlanRestrictions').value;
                var IdRate = document.getElementById('idRateRestrictions').value;
                var IdTypeRoom = document.getElementById('idTypeRoomRestrictions').value;
                var idRestriction = document.getElementById('idRestrictionRestrictions').value;
                var isException = document.getElementById('isException').value;
                var radioValue = nRestrictions;

                usedRestrictions[usedRestrictions.length] = new Array(eval(idHotel), IdRatePlan, eval(IdRate), eval(IdTypeRoom), idRestriction, eval(radioValue), eval(isException));
                UsedEspecificRestrictions.value = ObjectToString(usedRestrictions);

                MarkRate(idHotel, IdRatePlan, IdRate, IdTypeRoom, idRestriction, radioValue, isException);
                CloseListRestrictions();
            }
        }

        function DeleteRestriction(isGeneral, idHiddenRestriction, nRestriction) {
            if (confirm(alert6)) {
                var allRestrictions = eval('(' + $('#' + idHiddenRestriction).val() + ')');
                if (!isGeneral) {
                    var allReferences = eval('(' + $('#' + id1).val() + ')');
                    var newReferences = $.grep(allReferences, function(item, idx) {
                        return item[5] == nRestriction;
                    }, true);

                    if (newReferences.length < allReferences.length) {
                        if (!confirm(alert4)) return false;

                        $('#' + id1).val(ObjectToString(newReferences));

                        var patt = new RegExp('^(.*&nbsp;)?[\\*]{' + (nRestriction + 1).toString() + '}(&nbsp;(.*))?$', 'i');
                        $('.reference').each(function() {
                            $(this).html($(this).html().replace(patt, '$1$3'));
                        });
                        //RestoreHotelsTable();
                    }
                }

                var list = $('#' + (isGeneral ? 'tbGeneralRestrictions' : 'tbEspecificRestrictions'));

                list.find('tbody').html('');

                allRestrictions = $.grep(allRestrictions, function(item) {
                    return item[0] == nRestriction;
                }, true);

                $('#' + idHiddenRestriction).val(ObjectToString(allRestrictions));

                $.each(allRestrictions, function() {
                    AddRestrictionToTable(list.attr('id'), this[1], isGeneral, idHiddenRestriction, this[0]);
                });

                if (isGeneral && allRestrictions.length == 0) list.hide();
            }
        }

        function AddRestrictionToTable(idTable, descRestriction, isGeneral, idHiddenRestriction, nRestriction) {
            if (!isGeneral) $("#trEspecificRestrictions").show();
            $("#" + idTable).find('tbody')
		.append($('<tr>')
			.append($('<td align=\'left\'>')
				.append($('<div>').attr({ style: 'float:left;word-wrap:break-word;width:100%;' }).html(descRestriction))
				.append($('<div>').attr({ style: 'float:right;' }).append($('<img src=\'../Images/delete.gif\' onclick=\'DeleteRestriction(' + isGeneral + ',"' + idHiddenRestriction + '",' + nRestriction + ');\' style=\'cursor:hand;\'>')))
			)
		);
        }

        function ShowListRestrictions(idHotel, IdRatePlan, IdTarifa, IdTypeRoom, idRestriction, isException) {
            var pos = new Object(); ;
            pos.left = 0;
            pos.top = 0;

            var restrictions = eval('(' + ($('#' + id2).val().length > 0 ? $('#' + id2).val() : '[]') + ')');
            var usedRestrictions = eval('(' + ($('#' + id1).val().length > 0 ? $('#' + id1).val() : '[]') + ')');

            $('#tbListRestrictions, #tbAddEspecificRestriction').hide();

            if (restrictions.length > 0) {
                //var list = $('<ul/>').addClass('list restrictions');
                var list = $('<ul class="list restrictions"></ul>');

                $.each(restrictions, function(idx) {
                    //var item = $('<li/>').append($('<input />', { type: 'checkbox', value: this[0] }));                            
                    list.append('<li><input type="checkbox" value="' + this[0] + '"><span>' + this[1] + '</span></li>');
                    //item.append(item);
                });

                var selectedItems = $.grep(usedRestrictions, function(item) {
                    return (item[0] == idHotel && item[1] == IdRatePlan && item[2] == IdTarifa && item[3] == IdTypeRoom && item[4] == idRestriction && item[6] == isException);
                });

                var selected = new Array();

                $.each(selectedItems, function(idx) {
                    selected.push(this[5].toString());
                });

                list.find(':checkbox').filter(function(idx) {
                    return !($.inArray($(this).val(), selected) == -1);
                }).attr('checked', 'checked');

                var tBody = $('#tbListRestrictions tbody');
                tBody.html('<tr><td></td></tr>');
                tBody.find('td').append(list);

                $("#tbListRestrictions").show();
            } else {
                $("#tbAddEspecificRestriction").show();
            }
            $('#idHotelRestrictions').val(idHotel);
            $('#idRatePlanRestrictions').val(IdRatePlan);
            $('#idRateRestrictions').val(IdTarifa);
            $('#idTypeRoomRestrictions').val(IdTypeRoom);
            $('#idRestrictionRestrictions').val(idRestriction);
            $('#isException').val(isException);
            ResetListRestrictions('');
            document.onmouseup = getmousePos;
            getMousePositionDetailPos(pos, 0);
            //                    alert(pos.top);
            var lixlpixel_tooltip = document.getElementById('ListRestrictions');
            if (lixlpixel_tooltip) {
                lixlpixel_tooltip.style.left = pos.left;
                lixlpixel_tooltip.style.top = pos.top;
            }

        }

        function ShowAddEspecificRestriction() {
            var restrictions = StringToObject(document.getElementById(id2).value, 'Array');

            $("#tbListEspecificRestrictions").find('tbody').html('');
            for (var i = 0; i <= restrictions.length - 1; i++) {
                $("#tbListEspecificRestrictions").find('tbody')
			.append($('<tr>')
				.append($('<td align=\'left\'>').html(restrictions[i][1]))
			);
            }
            $("#tbListRestrictions").hide();
            $("#tbAddEspecificRestriction").show();
        }

        function CloseListRestrictions() {
            $("#tbListRestrictions, #tbAddEspecificRestriction").hide();
            document.getElementById('idHotelRestrictions').value = '-1';
            document.getElementById('idRatePlanRestrictions').value = '-1';
            document.getElementById('idRateRestrictions').value = '-1';
            document.getElementById('idTypeRoomRestrictions').value = '-1';
            document.getElementById('idRestrictionRestrictions').value = '-1';
            document.getElementById('isException').value = '-1';
            ResetListRestrictions('none');
        }

        function ResetListRestrictions(display) {
            $('#BackgroundLayer, #ListRestrictions').css('display', display);
        }

        function SelectRestriction() {

            var usedRestrictions = eval('(' + $('#' + id1).val() + ')');
            var idHotel = $('#idHotelRestrictions').val();
            var IdRatePlan = $('#idRatePlanRestrictions').val();
            var IdRate = $('#idRateRestrictions').val();
            var IdTypeRoom = $('#idTypeRoomRestrictions').val();
            var idRestriction = $('#idRestrictionRestrictions').val();
            var isException = ($('#isException').val().toString().toLowerCase() == 'true');

            usedRestrictions = $.grep(usedRestrictions, function(item) {
                return (item[0] == idHotel && item[1] == IdRatePlan && item[2] == IdRate && item[3] == IdTypeRoom && item[4] == idRestriction && item[6] == isException);
            }, true);

            var reference = $('#' + idHotel + '_' + IdRatePlan + '_' + IdRate + '_' + IdTypeRoom + '_' + idRestriction + '_' + isException.toString()).find('.reference');
            var sep = '';
            reference.html('');
            $('#tbListRestrictions ul input:checked').each(function(idx) {
                usedRestrictions.push(new Array(eval(idHotel), IdRatePlan, eval(IdRate), eval(IdTypeRoom), idRestriction, eval($(this).val()), eval(isException)));
                for (var i = 0; i <= $(this).val(); i++) {
                    reference.append(sep + '*');
                    sep = '';
                }
                sep = '&nbsp;';
            });
            $('#' + id1).val(ObjectToString(usedRestrictions));
            CloseListRestrictions();
        }

        function MarkRate(idHotel, IdRatePlan, IdRate, IdTypeRoom, idRestriction, nPoints, isException) {
            var reference = $('#' + idHotel + '_' + IdRatePlan + '_' + IdRate + '_' + IdTypeRoom + '_' + idRestriction + '_' + isException.toString()).find('.reference');
            reference.append((reference.html.length > 0) ? '&nbsp;' : '');
            for (var i = 0; i <= nPoints; i++) reference.append('*');
        }

        function StringToObject(string, objectType) {
            var sentence;
            var objectResult;

            if (string == '') {
                if (objectType == 'Array')
                    sentence = '[]';
                else
                    sentence = '{}';
            } else {
                sentence = string;
            }

            try {
                objectResult = eval('(' + sentence + ')');
            } catch (e) {
                objectResult = new Object();
            }
            return objectResult;
        }

        function ObjectToString(o) {
            var a = '';
            if (o.constructor == Array) {
                a += '[';
            }
            if (o.constructor == Object) {
                a += '{';
            }
            for (var i in o) {
                if (o.constructor != Array) {
                    a += '"' + i + '":';
                }
                if (o[i] == null) {
                    a += 'null,';
                } else if (o[i].constructor == Object) {
                    a += ObjectToString(o[i]) + ',';
                } else if (o[i].constructor == Array) {
                    a += ObjectToString(o[i]) + ',';
                } else if (o[i].constructor == String) {
                    a += '"' + o[i] + '",';
                } else {
                    a += o[i] + ',';
                }
            }
            if (o.constructor == Object) {
                a += '},';
            }
            if (o.constructor == Array) {
                a += '],';
            }
            return a.substr(0, a.length - 1).split(',}').join('}').split(',]').join(']');
        }

        function Restoreform() {
            if (document.getElementById(id5)) {
//CARLOS                RestoreHotelsTable();
                RestoreRestrictions();
            }
            onResizeIframe(500);
        }

        function RestoreRestrictions() {
            var generalRestrictions = StringToObject(document.getElementById(id3).value, 'Array');
            var especificRestrictions = StringToObject(document.getElementById(id2).value, 'Array');

            for (var i = 0; i < generalRestrictions.length; i++)
                AddRestrictionToTable('tbGeneralRestrictions', generalRestrictions[i][1], true, id3, generalRestrictions[i][0]);

            for (var i = 0; i < especificRestrictions.length; i++)
                AddRestrictionToTable('tbEspecificRestrictions', especificRestrictions[i][1], false, id2, especificRestrictions[i][0]);
        }

        function SetEspecificRestrictions(idHotel) {
            var usedEspecificRestrictions = StringToObject(document.getElementById(id1).value, 'Array');

            for (var i = 0; i < usedEspecificRestrictions.length; i++)
                if (usedEspecificRestrictions[i][0] == idHotel)
                MarkRate(usedEspecificRestrictions[i][0], usedEspecificRestrictions[i][1], usedEspecificRestrictions[i][2], usedEspecificRestrictions[i][3], usedEspecificRestrictions[i][4], usedEspecificRestrictions[i][5], usedEspecificRestrictions[i][6]);
        }
        
        function ltrim(src) {
            src= src.replace(/^\s+/,'').replace(/\s+$/,'');
            return(src);
        }

        function RestoreHotelsTable() {
            var inputsHotels = $("#" + id4).find('input');

            for (var i = 0, j = 0; i < inputsHotels.length; i++) {
                if (inputsHotels[i].type == 'checkbox') {
                    if (inputsHotels[i].checked) {
                        var idHotel = inputsHotels[i].onclick.toString();
                        idHotel = idHotel.substring(idHotel.indexOf(',') + 1, idHotel.lastIndexOf(','));
                        idHotel = ltrim(idHotel);
                        document.getElementById('Plus_' + idHotel).src = '../Includes/imagenes/menos.png';
                        
                        BuildRatesPlanTable(idHotel, j);
                    }
                    j++;
                }
            }
        }

        function BuildRatesPlanTable(idHotel, id) {
            var ratesPlan = StringToObject(document.getElementById(document.getElementById('idhotelRates_' + id).value).value, 'Object');
            $("#Rates_" + id).show().find('tbody').html('');
            for (var k = 0; k < ratesPlan.length; k++) {
                AddRatePlan(id, ratesPlan[k]);
            }
            SetEspecificRestrictions(idHotel)
        }

        function maxLength(e, obj, num) {
            k = (document.all) ? e.keyCode : e.which;
            if (k == 8 || k == 0 || k == 37 || k == 38 || k == 39 || k == 40 || k == 46) { return true; }
            else { if (obj.value.length >= num) { obj.value = obj.value.substring(0, num); } }
        }

        function ShowMsgSave() {
            document.getElementById('BackgroundLayer').style.display = '';
            document.getElementById('MsgSave').style.display = '';
        }

        function CloseMsgSave() {
            document.getElementById('BackgroundLayer').style.display = 'none';
            document.getElementById('MsgSave').style.display = 'none';
        }

        function DeleteAgreement() {
            if (!confirm(alert7)) {
                event.cancelBubble = true;
                event.returnvalue = false;
                return false;
            }
        }

        function ShowRatesPlan() {
            var idHotel = document.getElementById('idHotel').value;
            $("#quote").load("convenios.aspx?a=GetRates&h=" + idHotel, function() {
                $("#tdRatesPlan").html($("#quote").html());
                $("#tbRateSearch").hide();
                $("#tbRatesPlan").show();
            });
            
        }

        function ShowVigencyCalendar(hotel, table) {
            if (self.gfPop) {
                var input = document.getElementById('iVigencia_' + hotel);
                var idHidden = $('#idVigencia_' + table).val();
                var hidden = document.getElementById(idHidden);
                gfPop.fPopCalendar3(input, hidden);
                //alert('');
            }
            return false;
        }


        function ShowMsgPrint(sender, e) {
            var result = $('#<%= Me.chkIsGeneral.ClientID%>').attr('checked');

            if (!result) {
                if ($('#printQuestionShowed').val().toLowerCase() == 'false') {
                    result = true;
                    e = false;
                }

                if (e) {
                    $('#BackgroundLayer').show();
                    $('#MsgPrint').show();
                } else {
                    $('#BackgroundLayer').hide();
                    $('#MsgPrint').hide();
                }

                if (sender.id == 'btnNoPrint') {
                    if (Page_IsValid) {
                        $('#printQuestionShowed').val('False');
                    }
                    //viktor $("#<%= Me.imgSave.clientId %>").click();
                    $("#<%= Me.ImgSaveNoPrint.clientId %>").click();

                } else if (sender.id == 'btnYesPrint') {
                    <%--$("#<%= Me.imgPrint.clientId %>").click(); --%>
                    var pr = $('#<%= Me.isPrinter.ClientID%>');
                    if (pr) pr.Value = 1;

                }
            }
            return result;

        }

        function ValidateCity(src, arg) {
            arg.IsValid = ($('#<%= me.selectedCity.clientId %>').val() != '')
        }
        
        function LoadAgreements(idCorporate){
            $.ajax({
                url: 'convenios.aspx',
                dataType: 'json',
                type: 'GET',
                data: { a: 'getagreements',c: idCorporate},
                success: function(data, textStatus, XMLHttpRequest) {
                    if (data.error != null) {
                        alert(data.error);
                    } else if(data.length > 0){      
                        
                        var table= $("<table></table>");
                        table.append("<tr><td><center><strong><%=PortalCulture.GetString("M0BT0000195") %></strong></center></td><td><center><strong>ID</strong></center></td><td><center><strong><%=PortalCulture.GetString("00695") %></strong></center></td><td><center><strong><%=PortalCulture.GetString("00741") %></strong></center><td><center><strong><%=PortalCulture.GetString("M0BT0000347") %></tr>");
                        $.each(data, function() {
                            var tr = $('<tr></tr>');
                            tr.append("<td><center>"+this.Numero+"</center></td>")
                            tr.append("<td><center>"+this.idConvenio+"</center></td>")
                            tr.append("<td><center>"+this.Referencia+"</center></td>")
                            tr.append("<td>"+this.Nombre+"</td>")
                            tr.append("<td>"+this.idRatePlan+"</td>")
                            table.append(tr);
                        });
                        
                         $('#divAllAgreementsContent').html("").append(table);
                         
                        onResizeIframe(document.getElementById('divAllAgreementsContent').offsetHeight);
                        
                    } else {
                         $('#divAllAgreementsContent').html("No se pudo cargar los convenios");
                    }
                },
                error: function(response) {
                    $('#divAllAgreementsContent').html("No se pudo cargar los convenios");
                }
            });        
        }
               
    </script>

    <script type="text/javascript">
        $(document).ready(function() {
            onResizeIframe(500);
        });
    </script>

    <uc1:ctlMensajes ID="CtlMensajes1" runat="server"></uc1:ctlMensajes>
    </form>
</body>
</html>
