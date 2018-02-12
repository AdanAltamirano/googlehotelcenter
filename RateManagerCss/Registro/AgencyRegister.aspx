<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AgencyRegister.aspx.vb" Trace="false"
    Inherits="RateManager.AgencyRegister" %>

<%@ Register Src="../Portal/Modules/Contenido/AgenciasModulo.ascx" TagName="AgenciasModulo"
    TagPrefix="uc1" %>
<%@ Register Src="../Modulos/ctrlAutoComplete.ascx" TagName="ctrlAutoComplete" TagPrefix="uc2" %>
<%@ Register TagPrefix="uc1" TagName="ctlMensajes" Src="../Modulos/ctlMensajes.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Agency Registers</title>
    <link href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.5/jquery.min.js"></script>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js"></script>
    <script type="text/javascript" src="../Includes/Script/JsSearch-1.0.js"></script>
    <script language="javascript" type="text/javascript">

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

        SearchStart.AddParam
	    (
		    {
		        searchitems: [
			    { Item: 'Agencies', IDSearch: 'idEmpresa', nameSearch: 'Contacto_Nombre', isdefault: false },
			    { Item: 'Agencies', IDSearch: 'idEmpresa', nameSearch: 'Nombre', isdefault: true }
			    ],
		        colModel: [
			        { display: '<%= RateManager.PortalCulture.GetString("00257") %>' }, { display: '<%= RateManager.PortalCulture.GetString("M000144") %>' },
			    ],
			        Data: [{ catalogo: 'Agencies',
		            idSegmento: '<%= idSegmento%>'
    }],
		            id: 'Agencies',
		            index: 1
		        }
	    );
		        function cmdNew_onclick() {

		        }

    </script>
</head>
<body><%  %>

    <form id="form1" runat="server">
    <div class="clear">
        <input type="button" id="cmdNew" class="ButtonNew" runat="server" causesvalidation="false"
            value="New" style="width: 85px;" onclick="return cmdNew_onclick()" />
        <div class="title">
            <asp:Label ID="lblTitulo" runat="server" EnableViewState="False" Text="Registro de Agencias"
                CssClass="tituloSeccion"></asp:Label>
        </div>
        <div runat="server" id="divContenedor" style="display:none;">
            <table id="bookingcontainer" cellspacing="0" width="650" align="center" border="0">
                <tr>
                    <td>
                        <uc1:AgenciasModulo ID="AgenciasModulo1" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="cmdCancelar" runat="server" CssClass="Button" Text="Cancelar" Visible="False"
                            CausesValidation="False"></asp:Button>
                        <asp:Button ID="BtnNuevo" runat="server" Text="Nuevo" CssClass="Button" CausesValidation="False"
                            Width="64px"></asp:Button>
                        <asp:Button ID="cmdAceptar" runat="server" Text="Guardar" CssClass="Button" Visible="True">
                        </asp:Button>
                        
                        <asp:Button ID="btncancel" runat="server" EnableViewState="False" CssClass="Button"
                            Text="Cancelar" CausesValidation="False"></asp:Button>
                        <br />
                        <asp:Label ID="lblError" runat="server" CssClass="Validators" Visible="False">Error</asp:Label><asp:Label
                            ID="lblErrorSource" runat="server" CssClass="Validators" Visible="False">*</asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="clear">
        <uc2:ctrlAutoComplete ID="ctrlAutoComplete1" runat="server" />
        <asp:DataGrid ID="grid" runat="server" Width="99%" AllowPaging="True" PageSize="25"
            GridLines="None" AutoGenerateColumns="False" CssClass="datagrid" ShowFooter="True">
            <SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
            <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
            <ItemStyle CssClass="dgItem"></ItemStyle>
            <HeaderStyle CssClass="dgHeader"></HeaderStyle>
            <FooterStyle HorizontalAlign="Right"></FooterStyle>
            <Columns>
                <asp:BoundColumn Visible="False" DataField="idEmpresa"></asp:BoundColumn>
                <asp:BoundColumn Visible="True" DataField="idAgencia" HeaderText="ID"></asp:BoundColumn>
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
            </Columns>
            <PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
                Position="Bottom" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
        </asp:DataGrid>
    </div>
    <uc1:ctlMensajes ID="CtlMensajes1" runat="server"></uc1:ctlMensajes>
    </form>
</body>
</html>
