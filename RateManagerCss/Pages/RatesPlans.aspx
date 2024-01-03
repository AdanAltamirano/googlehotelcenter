<%@ Import Namespace="RateManager" %>
<%@ Register TagPrefix="uc1" TagName="ctrRatePlan" Src="../Modulos/ctrRatePlan.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../Portal/Modules/ctrlHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../Portal/Modules/ctrlFooter.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctlMensajes" Src="../Modulos/ctlMensajes.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RatesPlans.aspx.vb" Inherits="RateManager.RatesPlans" %>

<%@ Register TagPrefix="uc1" TagName="CtrMenu" Src="../Portal/Modules/CtrMenu.ascx" %>
<%@ Register Src="../Modulos/ctrlAutoComplete.ascx" TagName="ctrlAutoComplete" TagPrefix="uc2" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>Rates Plans</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link rel="stylesheet" type="text/css" href="../StyleSheets/Styles.css">

    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.5/jquery.min.js"></script>

    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js"></script>

    <script type="text/javascript" src="../Includes/Script/JsSearch-1.0.js"></script>

    <style>
        /* The Modal (background) */
        .modal {
            display: none; /* Hidden by default */
            position: fixed; /* Stay in place */
            z-index: 1; /* Sit on top */
            padding-top: 100px; /* Location of the box */
            left: 0;
            top: 0;
            width: 100%; /* Full width */
            height: 100%; /* Full height */
            overflow: auto; /* Enable scroll if needed */
            background-color: rgb(0,0,0); /* Fallback color */
            background-color: rgba(0,0,0,0.4); /* Black w/ opacity */
        }

        /* Modal Content */
        .modal-content {
            position: relative;
            background-color: #fefefe;
            margin: auto;
            padding: 0;
            border: 1px solid #888;
            width: 50%;
            box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2),0 6px 20px 0 rgba(0,0,0,0.19);
            -webkit-animation-name: animatetop;
            -webkit-animation-duration: 0.4s;
            animation-name: animatetop;
            animation-duration: 0.4s;
        }

        /* Add Animation */
        @-webkit-keyframes animatetop {
            from {
                top: -300px;
                opacity: 0;
            }

            to {
                top: 0;
                opacity: 1;
            }
        }

        @keyframes animatetop {
            from {
                top: -300px;
                opacity: 0;
            }

            to {
                top: 0;
                opacity: 1;
            }
        }

        /* The Close Button */
        .close {
            color: white;
            float: right;
            font-size: 28px;
            font-weight: bold;
        }

            .close:hover,
            .close:focus {
                color: #000;
                text-decoration: none;
                cursor: pointer;
            }

        .modal-header {
            padding: 2px 16px;
            background-color: #3E7BAC;
            color: white;
        }

        .modal-body {
            padding: 2px 16px;
        }

        .modal-footer {
            padding: 2px 16px;
            background-color: #3E7BAC;
            color: white;
        }
    </style>

</head>

<script>
    function ShowNewInfo(value) {
        var btn;
        if (value == 1) {
            document.getElementById("dvContent").style.display = "block";
            document.getElementById("dvContent2").style.display = "block";

            document.getElementById("<%=btnSave.clientid %>").style.display = "block";
            document.getElementById("<%=btncancel.clientid %>").style.display = "block";
            btn = document.getElementById("<%=btnPublish.clientid %>");
            if (btn) {
                btn.style.display = "block";
            }

        } else {
            document.getElementById("dvContent").style.display = "none";
            document.getElementById("dvContent2").style.display = "none";
            document.getElementById("<%=btnSave.clientid %>").style.display = "none";
            document.getElementById("<%=btncancel.clientid %>").style.display = "none";
            btn = document.getElementById("<%=btnPublish.clientid %>");
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
                    { Item: 'RatePlans', IDSearch: 'IdRatePlan', nameSearch: 'CodigoTarifa', isdefault: true },
                    { Item: 'RatePlans', IDSearch: 'IdRatePlan', nameSearch: 'name', isdefault: false }
                ],
                colModel: [
                    { display: '<%= RateManager.PortalCulture.GetString("00001") %>' },
                { display: '<%= RateManager.PortalCulture.GetString("00073") %>' },
            ],
            Data: [{
                catalogo: 'RatesPlans', idHotel: '<%= MyBase.cInfoActual.Hotel %>',
                idIdioma: '<%= RateManager.PortalCulture.GetIDCulture %>',
                IsSupervisor: '<%= MyBase.IsSupervisor %>'
            }],
            id: 'RatePlan',
            index: 1
        }
    );

    function validarkeyCode(e) { // 1
        tecla = (document.all) ? e.keyCode : e.which; // 2
        if (tecla == 8) return true; // 3
        patron = /[A-Za-z0-9\s]/; // 4
        te = String.fromCharCode(tecla); // 5
        return patron.test(te); // 6
    }

</script>

<body bottommargin="0" leftmargin="0" rightmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <script type="text/javascript">
        // Get the modal
        var modal = document.getElementById('msgGoogleHC');

        // Get the <span> element that closes the modal
        var span = document.getElementsByClassName("close")[0];

        // When the user clicks the button, open the modal 
        var openModal = function (id, msg, title) {
            modal.style.display = "block";
            document.getElementById('txtObjDelete').value = id;
            $(".modal-body .msg").text(msg);
            $(".modal-header h2").text(title);
        }

        // When the user clicks on <span> (x), close the modal
        var closeModal = function () {
            modal.style.display = "none";
        }

        // When the user clicks anywhere outside of the modal, close it
        window.onclick = function (event) {
            if (event.target == modal) {
                modal.style.display = "none";
            }
        }

    </script>


    <iframe id="gToday:normal:agenda.js" style="z-index: 999; left: -500px; position: absolute; top: -500px"
        name="gToday:normal:agenda.js" src='<%=GeRequestApplicationPath(string.concat("/Calendar/",portalculture.getculture().Name.Substring(0, 2).ToLower(),"/ipopeng2.htm" )) %>'
        frameborder="0" width="174" scrolling="no" height="189"></iframe>
    <form id="Form1" method="post" runat="server">
        <div class="clear">
            <input type="button" id="cmdNew" class="ButtonNew" runat="server" causesvalidation="false"
                value="New" style="width: 85px;" />
            <div class="mDiv">
            </div>
            <div class="title">
                <asp:Label ID="lblTitle" runat="server" EnableViewState="False" class="tituloSeccion">Rates Plans</asp:Label>
            </div>
        </div>
        <div runat="server" id="divContenedor">

            <table id="bookingcontainer clear" border="0" cellspacing="0" cellpadding="2" width="100%">
                <tr>
                    <td>
                        <table id="Table2" border="0" cellspacing="0" cellpadding="0" width="100%" align="center">
                            <tr class="trTitle rounded-corners" id="dvContent2">
                                <td class="dgitem" align="left">
                                    <asp:Label ID="lblMsg" runat="server" DESIGNTIMEDRAGDROP="987">Modificando tipo de habitación</asp:Label>
                                </td>
                            </tr>
                            <tr class="trContent rounded-corners" id="dvContent">
                                <td class="tdContent" colspan="2">
                                    <br />
                                    <uc1:ctrRatePlan ID="CtrRatePlan1" runat="server"></uc1:ctrRatePlan>
                                    <br />
                                    <%  If Me.IsSupervisor Then
                                            Me.btnPublish.Text = RateManager.PortalCulture.GetString("01364")
                                    %>
                                    <asp:Button ID="btnPublish" runat="server" EnableViewState="False" CssClass="Button"
                                        Text="Publicar" OnClientClick="return ShowWarnings();" CausesValidation="true"></asp:Button>
                                    <% End if %>
                                    <asp:Button ID="btnSave2" runat="server" EnableViewState="False" CssClass="Button"
                                        Text="Guardar" CausesValidation="true"></asp:Button>
                                    <asp:Button ID="btnSave" runat="server" EnableViewState="False" CssClass="Button"
                                        Text="Guardar" OnClientClick="return ShowWarnings();" CausesValidation="true"></asp:Button>
                                    <asp:Button ID="btncancel" runat="server" EnableViewState="False" CssClass="Button"
                                        Text="Cancelar" CausesValidation="False"></asp:Button>
                                    <br />
                                    <asp:Label ID="lblError" runat="server" CssClass="Validators" Visible="False">Error</asp:Label><asp:Label
                                        ID="lblErrorSource" runat="server" CssClass="Validators" Visible="False">*</asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div class="clear">
            <uc2:ctrlAutoComplete ID="ctrlAutoComplete1" runat="server" />
            <div style="float: left; margin-bottom: 15px;">
                <asp:Label ID="lblFilter" runat="server" Text="Filtro:"></asp:Label>
                <asp:DropDownList ID="ddlDeletedFilter" runat="server" AutoPostBack="True">
                    <asp:ListItem Text="Solo activos" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Solo no activos" Value="0"></asp:ListItem>
                    <asp:ListItem Text="Activos y no activos" Value="-1"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <asp:DataGrid ID="grid" runat="server" Width="99%" AllowPaging="True" PageSize="20"
                GridLines="None" AutoGenerateColumns="False" CssClass="datagrid" ShowFooter="True">
                <SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
                <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
                <ItemStyle CssClass="dgItem"></ItemStyle>
                <HeaderStyle CssClass="dgHeader"></HeaderStyle>
                <FooterStyle HorizontalAlign="Right"></FooterStyle>
                <Columns>
                    <asp:BoundColumn Visible="False" DataField="idrateplan"></asp:BoundColumn>
                    <asp:BoundColumn DataField="orden" HeaderText="Orden">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="codigotarifa" HeaderText="C&#243;digo">
                        <HeaderStyle Width="9%"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Name" HeaderText="Nombre">
                        <HeaderStyle Width="40%"></HeaderStyle>
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Segment" HeaderText="Segmento">
                        <HeaderStyle Width="23%"></HeaderStyle>
                    </asp:BoundColumn>
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
                    <asp:TemplateColumn>
                        <HeaderStyle Width="10%"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkActivar2" Style="display: none" runat="server" CssClass="dgLink"
                                CausesValidation="False" CommandName="Active">-</asp:LinkButton>
                            <asp:HyperLink ID="lnkActivar" runat="server" CssClass="dgLink"></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:BoundColumn Visible="False" DataField="SegmentRacPrinc"></asp:BoundColumn>
                    <asp:BoundColumn Visible="False" DataField="codigotarifa"></asp:BoundColumn>
                    <asp:BoundColumn Visible="False" DataField="Deleted"></asp:BoundColumn>
                </Columns>
                <PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
                    Position="Bottom" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
            </asp:DataGrid>
        </div>
        <uc1:ctlMensajes ID="CtlMensajes1" runat="server"></uc1:ctlMensajes>
        <uc1:ctlMensajes ID="CtlMensajes2" runat="server"></uc1:ctlMensajes>
        <uc1:ctlMensajes ID="CtlMensajes3" runat="server"></uc1:ctlMensajes>
    </form>
</body>
</html>
