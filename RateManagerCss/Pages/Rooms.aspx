<%@ Register TagPrefix="uc1" TagName="CtrMenu" Src="../Portal/Modules/CtrMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../Portal/Modules/ctrlFooter.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../Portal/Modules/ctrlHeader.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" ValidateRequest="false" CodeBehind="Rooms.aspx.vb" Debug="true"
    Inherits="RateManager.Rooms" %>

<%@ Register TagPrefix="uc1" TagName="ctlMensajes" Src="../Modulos/ctlMensajes.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlRooms" Src="../Modulos/ctrlRooms.ascx" %>
<%@ Register src="../Modulos/ctrlAutoComplete.ascx" tagname="ctrlAutoComplete" tagprefix="uc2" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>Rooms</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
    
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.5/jquery.min.js"></script>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js"></script>
    <script type="text/javascript" src="../Includes/Script/JsSearch-1.0.js"></script>
    


    <script language="javascript">
	<!--        /* Messaje script */
        function hideElms(elmTag) {
            for (i = 0; i < document.all.tags(elmTag).length; i++) {
                obj = document.all.tags(elmTag)[i];
                if (!obj || !obj.offsetParent) continue;
                obj.style.visibility = "hidden";
            }
        }
        
        function __ok() {

        }
        
        function wMsgShow() {
            var ns4 = (document.layers) ? true : false;
            var ie4 = (document.all) ? true : false;

            var winW = (ns4) ? window.innerWidth - 16 : document.body.offsetWidth - 20;
            var winH = (ns4) ? window.innerHeight : document.body.offsetHeight;

            if (ie4) hideElms('SELECT');

            var div = document.getElementById("divAlpha")
            div.style.width = "100%";
            div.style.height = "100%";
            div.style.filter = "alpha(opacity=30)";
            div.style.MozOpacity = 0.3;
            div.style.position = "absolute";
            div.style.top = 0;
            div.style.left = 0;
            div.style.zIndex = 998;
            div.style.display = 'block';

            var wMsg = document.getElementById("TableMsg");
            wMsg.style.display = "block";
            wMsg.style.position = "absolute";
            wMsg.style.left = (winW - parseInt(wMsg.style.width)) / 2;
            wMsg.style.top = (winH - parseInt(wMsg.style.height)) / 2;
            wMsg.style.zIndex = 999;
        }

        function wMsgHide() {
            var div = document.getElementById("divAlpha");
            div.style.display = "none";
            var wMsg = document.getElementById("TableMsg");
            wMsg.style.display = "none";
        }

        //-->
        function validaInputFile() {
            var objs = document.getElementsByTagName("INPUT");
            for (i = 0; i < objs.length; i++) {
                if (objs[i].type == 'file') {
                    objs[i].disabled = true;
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
			{ Item: 'TipoHabitaciones_Hotel', IDSearch: 'idTipoHabitacion_Hotel', nameSearch: 'TipoHabitacion', isdefault: true },
			{ Item: 'TipoHabitaciones_Hotel', IDSearch: 'idTipoHabitacion_Hotel', nameSearch: 'NombreHabitacion', isdefault: false }
			],
		    colModel: [
			    { display: '<%= RateManager.PortalCulture.GetString("00057") %>' },
			    { display: '<%= RateManager.PortalCulture.GetString("M000066") %>' }
			],
			    Data: [{ catalogo: 'Rooms', idHotel: '<%= MyBase.cInfoActual.Hotel %>', idIdioma: '<%= RateManager.PortalCulture.GetIDCulture %>'}],
		    id: 'Rooms',
            index: 1
		}
	);

    </script>

</head>
<body bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <uc1:ctlMensajes ID="CtlMensajes1" runat="server"></uc1:ctlMensajes>
    <div class="clear">
        <div class="clear">
            <div style="float: left">
                <img class="bgplus" src="../Includes/imagenes/icono-big-plus.png" width="25" height="25" />
                <asp:Label ID="lblTitleForm" runat="server" class="tituloSeccion">Habitaciones</asp:Label>
            </div>
            <div style="float: right">
                <input type="button" id="cmdNew" class="ButtonNew" runat="server" causesvalidation="false"
                    value="New" style="float: right; width: 84px; margin-right:4
                    px; " />

            &nbsp;</div>
        </div>
        <div runat="server" id="divContenedor">
            <table id="bookingcontainer clear" width="100%" cellspacing="0" cellpadding="2" border="0">           
                <tr>
                    <td colspan="2">
                        <asp:TextBox ID="txtDiv1" Style="display: none" runat="server" Width="34px" CssClass="TextBox">0</asp:TextBox><asp:TextBox
                            ID="txtDiv2" Style="display: none" runat="server" Width="34px" CssClass="TextBox">0</asp:TextBox><asp:TextBox
                                ID="txtDiv3" Style="display: none" runat="server" Width="34px" CssClass="TextBox">0</asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="AcountPanel" runat="server" BorderStyle="None">
                            <table id="Table1" border="0" cellspacing="0" cellpadding="0" width="100%" align="center">
                                <tr class="trTitle rounded-corners">
                                    <td class="dgitem" align="left">
                                        <asp:Label ID="lblMsg" runat="server" DESIGNTIMEDRAGDROP="987">Modificando tipo de habitación</asp:Label>
                                    </td>
                                </tr>
                                <tr class="trContent rounded-corners">
                                    <td class="tdContent">
                                        <asp:Panel ID="PanelRooms" runat="server" HorizontalAlign="Center">
                                            <uc1:ctrlRooms ID="CtrlRooms1" runat="server"></uc1:ctrlRooms>
                                            <asp:Button ID="btnNuevo" runat="server" Width="85px" CssClass="Button" Text="Nuevo"
                                                CausesValidation="False" Style="display: none"></asp:Button>
                                            
                                            <%  If Me.IsSupervisor Then
                                                    Me.btnPublicar.Text = RateManager.PortalCulture.GetString("01364")
                                            %>
                                            <asp:Button ID="btnPublicar" runat="server" Width="85px" CssClass="Button" Text="Publicar"  CausesValidation =true >
                                            </asp:Button>
                                            <% End If%>
                                            <asp:Button ID="btnGuardar" runat="server" Width="85px" CssClass="Button" Text="Guardar" CausesValidation =true >
                                            </asp:Button>
                                            <asp:Button ID="btnOcultarDivContenedor" runat="server" Width="85px" CssClass="Button"
                                                Text="Cancelar" CausesValidation="False"></asp:Button>
                                            <br>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <asp:Label ID="lblError" runat="server" CssClass="Validators" Visible="False"></asp:Label>
    <asp:Label ID="lblDeleteError" runat="server"  CssClass="Validators" Visible="False"></asp:Label>
    <div class=clear >
    <div class="clear">
    
    <uc2:ctrlAutoComplete ID="ctrlAutoComplete1" runat="server" />
        <div style="float:left; margin-bottom:15px;" >
           <asp:Label ID="lblFilter" runat="server" Text="Filtro:" ></asp:Label>
           <asp:DropDownList ID="ddlFilter" runat="server" AutoPostBack="True">
            <asp:ListItem Text="Solo activos" Value="1"></asp:ListItem>
            <asp:ListItem Text="Solo no activos" Value="0"></asp:ListItem>
            <asp:ListItem Text="Activos y no activos" Value="-1"></asp:ListItem>
           </asp:DropDownList>
        </div>
        <asp:DataGrid ID="grid" runat="server" GridLines="None" Width="99%" AutoGenerateColumns="False" AllowPaging="True"
            CssClass="DataGrid" PageSize="20" ShowFooter="True" Borde="0">
            <FooterStyle HorizontalAlign="Right"></FooterStyle>
            <SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
            <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
            <ItemStyle CssClass="dgItem"></ItemStyle>
            <HeaderStyle CssClass="dgHeader"></HeaderStyle>
            <Columns>
                <asp:BoundColumn Visible="False" HeaderText="Id"></asp:BoundColumn>
                
                 <asp:BoundColumn
                 HeaderText="CodigoHabitacion">
                    <ItemStyle Width="9%"></ItemStyle>
                </asp:BoundColumn>
                <asp:BoundColumn
                 HeaderText="Tipo">
                    <ItemStyle Width="28%"></ItemStyle>
                </asp:BoundColumn>
                <asp:BoundColumn HeaderText="Nombre">
                    <ItemStyle Width="27%"></ItemStyle>
                </asp:BoundColumn>
                <asp:BoundColumn Visible="False" HeaderText="Hab.">
                    <HeaderStyle HorizontalAlign="center" />
                    <ItemStyle Width="9%" HorizontalAlign="center"></ItemStyle>
                </asp:BoundColumn>
                <asp:BoundColumn HeaderText="Pers.">
                    <HeaderStyle HorizontalAlign="center" />
                    <ItemStyle HorizontalAlign="center"></ItemStyle>
                </asp:BoundColumn>
                <asp:BoundColumn HeaderText="Min Adultos">
                    <HeaderStyle HorizontalAlign="center" />
                    <ItemStyle Width="9%" HorizontalAlign="center"></ItemStyle>
                </asp:BoundColumn>
                <asp:BoundColumn HeaderText="Max Adultos">
                    <HeaderStyle HorizontalAlign="center" />
                    <ItemStyle Width="9%" HorizontalAlign="center"></ItemStyle>
                </asp:BoundColumn>
                <asp:BoundColumn HeaderText="Ni&#241;os">
                    <HeaderStyle HorizontalAlign="center" />
                    <ItemStyle Width="9%" HorizontalAlign="center"></ItemStyle>
                </asp:BoundColumn>
                <asp:BoundColumn HeaderText="Per. Extras">
                    <HeaderStyle HorizontalAlign="center" />
                    <ItemStyle Width="9%" HorizontalAlign="center"></ItemStyle>
                </asp:BoundColumn>
                <asp:BoundColumn Visible="False" HeaderText="Extra Beds"></asp:BoundColumn>
                <asp:BoundColumn Visible="False" HeaderText="Extra Bed Price">
                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                </asp:BoundColumn>
                <asp:TemplateColumn>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center" Width="9%"></ItemStyle>
                    <ItemTemplate>
                        <asp:LinkButton ID="ibtnEdit" runat="server" CssClass="dgLink" CausesValidation="False"
                            CommandName="Select"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center" Width="9%"></ItemStyle>
                    <ItemTemplate>
                        <asp:LinkButton ID="ibtnDelete2" Style="display: none" runat="server" CssClass="dgLink"
                            CausesValidation="False" CommandName="Delete"></asp:LinkButton>
                        <asp:HyperLink ID="ibtnDelete" runat="server" CssClass="dglink">Delete</asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn Visible="False" HeaderText="Description"></asp:BoundColumn>
                <asp:BoundColumn Visible="False" HeaderText="Eliminada"></asp:BoundColumn>
            </Columns>
            <PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
                Position="Bottom" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
        </asp:DataGrid>
    </div>
    <div id="Div1" style="filter: alpha(opacity=30); background-color: gray; width: 264px;
        display: none; height: 12px" ms_positioning="FlowLayout">
    </div>
    <asp:Button ID="UpdateImgs" runat="server" CausesValidation="False" Text="UpdateImgs"
        Visible="False"></asp:Button>
    <div id="divAlpha" style="filter: alpha(opacity=30); background-color: gray; width: 264px;
        display: none; height: 12px" designtimedragdrop="61" ms_positioning="FlowLayout">
    </div>
    </div>
    </form>
</body>
</html>
