<%@ Import Namespace="RateManager" %>
<%@ Register TagPrefix="uc1" TagName="ctrRatePlan" Src="../Modulos/ctrRatePlan.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../Portal/Modules/ctrlHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../Portal/Modules/ctrlFooter.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctlMensajes" Src="../Modulos/ctlMensajes.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Package.aspx.vb" Inherits="RateManager.Package"
    EnableEventValidation="true" %>

<%@ Register TagPrefix="uc1" TagName="ctrlPackage" Src="../Modulos/ctrlPackage.ascx" %>
<%@ Register src="../Modulos/ctrlAutoComplete.ascx" tagname="ctrlAutoComplete" tagprefix="uc2" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>Package</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet">

    <script language='javascript' src='../modulos/packages.js'></script>

    <script src="../Pages/Scripts/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js"></script>
    <script type="text/javascript" src="../Includes/Script/JsSearch-1.0.js"></script>

    <script>
        var resource01026 = '<%= RateManager.PortalCulture.GetString("01026") %>'
        var resource00094 = '<%= RateManager.PortalCulture.GetString("00094") %>'


        function ShowWarnings() {
            var result = true;            
            if ($('#<%= CtrlPackage1.ddlRulesClientID %> option:selected').val() == 0) {
                result = confirm('<%= RateManager.PortalCulture.GetString("01351") %>');
            }
            return result;
        }

        function IniDate() {
            var fecha = new Date();
            var fecha2 = new Date(2030, 12, 31);
            var arr = new Array(3);
            arr[0] = [fecha.getFullYear(), fecha.getMonth() + 1, fecha.getDate()]
            arr[1] = [fecha2.getFullYear(), fecha2.getMonth() + 1, fecha2.getDate()];
            return arr;
        }
        
        function validarkeyCode(e) { // 1
            tecla = (document.all) ? e.keyCode : e.which; // 2
            if (tecla == 8) return true; // 3
            patron = /[A-Za-z0-9\s]/; // 4
            te = String.fromCharCode(tecla); // 5
            return patron.test(te); // 6
        } 
    </script>

</head>

<script>
    function ShowNewInfo(value) {
        if (value == 1) {
            document.getElementById("dvContent").style.display = "block";
            document.getElementById("dvContent2").style.display = "block";

            document.getElementById("<%=btnSave.clientid %>").style.display = "block";
            document.getElementById("<%=btnCancel.clientid %>").style.display = "block";
            var btn = document.getElementById("<%=btnPublish.clientid %>");
            if (btn) {
                btn.style.display = "block";
            }

        } else {
            document.getElementById("dvContent").style.display = "none";
            document.getElementById("dvContent2").style.display = "none";
            document.getElementById("<%=btnSave.clientid %>").style.display = "none";
            document.getElementById("<%=btnCancel.clientid %>").style.display = "none";
            var btn = document.getElementById("<%=btnPublish.clientid %>");
            if (btn) {
                btn.style.display = "none";
            }
        }
    }

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
		    { Item: 'Paquetes', IDSearch: 'RateCode', nameSearch: 'RateCode', isdefault: true },
			{ Item: 'Paquetes', IDSearch: 'RateCode', nameSearch: 'Name', isdefault: true }
			],
		    colModel: [
			    { display: '<%= RateManager.PortalCulture.GetString("00001") %>' },
			    { display: '<%= RateManager.PortalCulture.GetString("00073") %>' }
			],
		    Data: [{ catalogo: 'Package', idHotel: '<%= MyBase.cInfoActual.Hotel %>',
		        idIdioma: '<%= RateManager.PortalCulture.GetIDCulture %>'
            }],
            id: 'Packages',
		    index: 1
		    }
	);
    
</script>

<body bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <div class="clear">
        <input type="button" id="cmdNew" class="ButtonNew" runat="server" causesvalidation="false"
            value="New" style="width: 85px;" />
        <div class="mDiv">
        </div>
        <div class="title">
            <asp:Label ID="lblTitle" runat="server" EnableViewState="False" class="tituloSeccion"> Paquetes</asp:Label>
        </div>
    </div>
    <div runat="server" id="divContenedor">
        <table id="bookingcontainer clear" width="100%" cellspacing="0" cellpadding="2" border="0">
            <tr>
                <td>
                    <table id="Table2" cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                        <tr class="trTitle rounded-corners" id="dvContent2">
                            <td class="dgitem" align="left">
                                <asp:Label ID="lblctrtitulo" EnableViewState="False" CssClass="bookingnormallabel"
                                    runat="server"> Nuevo Paquete</asp:Label>
                            </td>
                        </tr>
                        <tr class="trContent rounded-corners" id="dvContent">
                            <td class="tdContent">
                                <hr />
                                <uc1:ctrlPackage ID="CtrlPackage1" runat="server"></uc1:ctrlPackage>
                                <br />
                                <%  If Me.IsSupervisor Then
                                        Me.btnPublish.Text = RateManager.PortalCulture.GetString("01364")
                                %>
                                &nbsp;<asp:Button ID="btnPublish" runat="server" Text="Publicar" CssClass="button"
                                    EnableViewState="False" OnClientClick="return ShowWarnings();" CausesValidation="true">
                                </asp:Button>
                                <% End If%>
                                <asp:Button ID="btnSave" runat="server" EnableViewState="False" CssClass="Button"
                                    Text="Guardar" OnClientClick="return ShowWarnings();" CausesValidation="true">
                                </asp:Button>
                                <asp:Button ID="btncancel" runat="server" EnableViewState="False" CssClass="Button"
                                    Text="Cancelar" CausesValidation="False"></asp:Button>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblError" runat="server" CssClass="Validators" Visible="False">Error</asp:Label><asp:Label
                                    ID="lblErrorSource" runat="server" CssClass="Validators" Visible="False">*</asp:Label>
                                
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
            
        <uc2:ctrlAutoComplete ID="ctrlAutoComplete1" runat="server" />        
        

    <div class="clear">
        <asp:DataGrid ID="dgPackage" GridLines="None" runat="server" Width="99%" AllowPaging="True"
            AutoGenerateColumns="False" CssClass="datagrid" ShowFooter="True" PageSize="20">
            <FooterStyle HorizontalAlign="Right"></FooterStyle>
            <SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
            <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
            <ItemStyle CssClass="dgItem"></ItemStyle>
            <HeaderStyle CssClass="dgHeader"></HeaderStyle>
            <Columns>
                <asp:BoundColumn DataField="RateCode" HeaderText="C&#243;digo">
                    <HeaderStyle Width="10%"></HeaderStyle>
                </asp:BoundColumn>
                <asp:BoundColumn DataField="Name" HeaderText="Nombre">
                    <HeaderStyle Width="40%"></HeaderStyle>
                </asp:BoundColumn>
                <asp:TemplateColumn>
                    <HeaderStyle Width="15%"></HeaderStyle>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkedit" runat="server" CommandName="Select" CausesValidation="False"
                            CssClass="dgLink">editar</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderStyle Width="15%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkEliminar2" Style="display: none" runat="server" CssClass="dgLink"
                            CausesValidation="False" CommandName="Delete">-</asp:LinkButton>
                        <asp:HyperLink ID="lnkEliminar" runat="server" CssClass="dgLink">Eliminar</asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn Visible="False" DataField="idrateplan"></asp:BoundColumn>
                <asp:BoundColumn Visible="False" DataField="idpaquete"></asp:BoundColumn>
            </Columns>
            <PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
                Position="Bottom" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
        </asp:DataGrid>
    </div>
    <uc1:ctlMensajes ID="CtlMensajes1" runat="server"></uc1:ctlMensajes>
    </form>
</body>
</html>
