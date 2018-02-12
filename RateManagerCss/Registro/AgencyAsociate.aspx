<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AgencyAsociate.aspx.vb" Inherits="RateManager.AgencyAsociate" %>

<%@ Register Src="../Modulos/ctrlAutoComplete.ascx" TagName="ctrlAutoComplete" TagPrefix="uc2" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.5/jquery.min.js"></script>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js"></script>
    <script type="text/javascript" src="../Includes/Script/JsSearch-1.0.js"></script>

    <script type="text/javascript" language="javascript">
        SearchStart.AddParam
	    (
		    {
		        searchitems: [
			        { Item: 'Agencies', IDSearch: 'idEmpresa', nameSearch: 'Contacto_Nombre', isdefault: false },
			        { Item: 'Agencies', IDSearch: 'idEmpresa', nameSearch: 'Nombre', isdefault: true }
		        ],
		        colModel: [
			        { display: '<%= RateManager.PortalCulture.GetString("00257") %>' },
			        { display: '<%= RateManager.PortalCulture.GetString("M000144") %>' },
		        ],
		        Data: [{
		            catalogo: 'Agencies',
		            idSegmento: '<%= idSegmento%>'
		        }],
		        id: 'Agencies',
		        index: 1
		    }
	    );
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div class="clear">
                <div></div>
                <div>
                    <asp:Label ID="lblTitleForm" runat="server" class="tituloSeccion">Asociar Usuario Agencia</asp:Label>
                </div>
            </div>
            <table cellpadding="0" cellspacing="0" border="0" style="width: 100%">
                <tr>
                    <td></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <uc2:ctrlAutoComplete ID="ctrlAutoComplete1" runat="server" />
                        <asp:DataGrid ID="dgAgencies" runat="server" CssClass="DataGrid" AutoGenerateColumns="False" Width="70%"
                            AllowPaging="True" PageSize="20">
                            <SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
                            <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
                            <ItemStyle CssClass="dgItem"></ItemStyle>
                            <HeaderStyle CssClass="dgHeader"></HeaderStyle>
                            <Columns>
                                <asp:BoundColumn Visible="False" DataField="idEmpresa"></asp:BoundColumn>
                                <asp:BoundColumn Visible="False" DataField="Nombre"></asp:BoundColumn>
                                <asp:TemplateColumn HeaderText="Nombre&#160;Empresa">
                                    <ItemStyle Width="20%"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkSelect" runat="server" CssClass="dgLink" CommandName="Select"
                                            CausesValidation="false">
														                    <%# databinder.eval(container.dataitem,"Nombre") %>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:BoundColumn HeaderText="Ciudad" DataField="Ciudad"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Pais" DataField="idPais"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Contacto" DataField="Contacto_Nombre"></asp:BoundColumn>
                            </Columns>
                            <PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
                                Position="Top" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
                        </asp:DataGrid>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="height: 14px;"></td>
                </tr>
                <tr>
                    <td colspan="2"></td>
                </tr>
            </table>
            <asp:Panel ID="pnlUser" runat="server" Visible="false">
                <table cellpadding="0" cellspacing="0" border="0" style="width: 100%">
                    <tr>
                        <td class="dgItem" align="center" colspan="2">
                            <asp:Label ID="lblInformacion" runat="server">Usuarios de agencia</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table>
                                <tr>
                                    <tr>
                                        <td width="20%" align="right">
                                            <asp:Label ID="lblNombre" runat="server">Nombre: </asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNombre" runat="server" MaxLength="40" Columns="22"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblApellido" runat="server">Apellido: </asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtApellido" runat="server" MaxLength="40" Columns="22"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <td width="15%" align="right">
                                        <span id="lblemail" runat="server">Email:</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEmail" runat="server" MaxLength="40" Columns="22"></asp:TextBox>
                                    <td colspan="2">
                                        <asp:Label ID="lblAddError" Visible="false" runat="server" CssClass="validators">Este Email ya ha sido registrado</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" align="right">
                                        <asp:Button ID="btnSave" runat="server" Text="Add" CssClass="button" CausesValidation="false"></asp:Button>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 60%" valign="top">

                            <asp:DataGrid ID="dgUsuarios" runat="server" Width="90%" CssClass="DataGrid" AllowPaging="True" PageSize="20"
                                AutoGenerateColumns="False">
                                <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
                                <ItemStyle CssClass="dgItem"></ItemStyle>
                                <HeaderStyle CssClass="dgHeader"></HeaderStyle>
                                <Columns>
                                    <asp:BoundColumn Visible="False" DataField="IdUsuario"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="email" HeaderText="Correo electr&#243;nico"></asp:BoundColumn>
                                    <asp:TemplateColumn>
                                        <ItemStyle HorizontalAlign="Center" Width="25%"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEdit" runat="server" CssClass="dgLink" CausesValidation="False" CommandName="Select">Edit</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn>
                                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEliminar2" Style="display: none" runat="server" CssClass="dgLink" CommandName="Delete"
                                                CausesValidation="false">-</asp:LinkButton>
                                            <asp:LinkButton ID="lnkEliminar" runat="server" CssClass="dgLink" CommandName="Delete" CausesValidation="false">Desasociar</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                                <PagerStyle CssClass="dgPager" HorizontalAlign="Right" Mode="NumericPages" Position="Top" PrevPageText="<< Anterior"
                                    NextPageText="Siguiente >>"></PagerStyle>
                            </asp:DataGrid>
                        </td>
                        <td style="width: 40%; vertical-align: top;">
                            <table>
                                <tr>
                                    <td>
                                        <div runat="server" id="divEdit">
                                            <table cellpadding="0" cellspacing="0" border="0" style="margin-left: 20px;">
                                                <tr>
                                                    <td class="tdRight">
                                                        <asp:Label ID="lblUser" runat="server" EnableViewState="False" CssClass="clslabel">Correo Electronico:</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblCorreo" runat="server" CssClass="clsHelpLabel">test@oz.com.mx</asp:Label>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td class="tdRight">
                                                        <asp:Label ID="Label2" runat="server" EnableViewState="False" CssClass="clslabel">Password:</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblPassword" runat="server" CssClass="clsHelpLabel">tes1x</asp:Label>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td class="tdRight">
                                                        <asp:Label ID="lblPass" runat="server" EnableViewState="False" CssClass="clslabel">Password:</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtPassword" runat="server" CssClass="textbox" Columns="20" MaxLength="20" TextMode="Password"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="Validators" ErrorMessage="La confirmación de la contraseña es requerida."
                                                            ForeColor=" " Display="Dynamic" ControlToValidate="txtPassword">*</asp:RequiredFieldValidator>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td class="tdRight">
                                                        <asp:Label ID="lblConfPass" runat="server" EnableViewState="False" CssClass="clslabel">Confirm Password:</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtConfirmPass" runat="server" CssClass="textbox" Columns="20" MaxLength="20" TextMode="Password"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" CssClass="Validators" ErrorMessage="La confirmación de la contraseña es requerida."
                                                            ForeColor=" " Display="Dynamic" ControlToValidate="txtConfirmPass">*</asp:RequiredFieldValidator>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:CompareValidator ID="CompareValidator1" runat="server" CssClass="Validators" ErrorMessage="La confirmación y la nueva contraseña no coinciden."
                                                            ForeColor=" " Display="Dynamic" ControlToValidate="txtConfirmPass" ControlToCompare="txtPassword"></asp:CompareValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" style="float: right !important;">
                                                        <asp:Button ID="btnNew" runat="server" EnableViewState="False" CssClass="button" Text="New" CausesValidation="False"></asp:Button><asp:Button ID="btnModify" runat="server" EnableViewState="False" CssClass="button" Text="Guardar"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center;">
                                        <asp:Label ID="lblError" runat="server" CssClass="validators" Visible="false">Error</asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
    </form>
</body>
</html>
