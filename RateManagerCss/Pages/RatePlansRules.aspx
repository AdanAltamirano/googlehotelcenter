<%@ Register TagPrefix="uc1" TagName="ctlMensajes" Src="../Modulos/ctlMensajes.ascx" %>
<%@ Import Namespace="RateManager" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RatePlansRules.aspx.vb"
    Inherits="RateManager.RatePlansRules" %>

<%@ Register TagPrefix="uc1" TagName="CtrMenu" Src="../Portal/Modules/CtrMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../Portal/Modules/ctrlHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../Portal/Modules/ctrlFooter.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrRatePlanRules" Src="../Modulos/ctrRatePlanRules.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>Rate Plans Rules</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet">

    <script src="../Pages/Scripts/jquery.min.js" type="text/javascript"></script>

</head>

<script>
    function ShowNewInfo(value) {
        if (value == 1) {
            document.getElementById("dvContent").style.display = "block";
            document.getElementById("dvContent2").style.display = "block";

            document.getElementById("<%=btnAceptar.clientid %>").style.display = "block";
            document.getElementById("<%=btnCancel.clientid %>").style.display = "block";
            var btn = document.getElementById("<%=btnPublicar.clientid %>");
            if (btn) {
                btn.style.display = "block";
            }

        } else {
            document.getElementById("dvContent").style.display = "none";
            document.getElementById("dvContent2").style.display = "none";
            document.getElementById("<%=btnAceptar.clientid %>").style.display = "none";
            document.getElementById("<%=btnCancel.clientid %>").style.display = "none";
            var btn = document.getElementById("<%=btnPublicar.clientid %>");
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
</script>

<body ms_positioning="FlowLayout" bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0">
    <form id="Form1" method="post" runat="server">
    <div class="clear">
        

        <input type="button" id="cmdNew" class="ButtonNew" runat="server" causesvalidation="false"
            value="New" style=" width:85px;"/>
        <div class="mDiv">
        </div>
        <div class="title">
            <asp:Label ID="lbltitle" runat="server" EnableViewState="False" class="tituloSeccion">Rate Plans Rules </asp:Label>
        </div>
 </div>
        <div runat="server" id="divContenedor">
            <table id="bookingcontainer clear" width="100%" cellspacing="0" cellpadding="2" border="0">
            <tr>
                <td>
                    <table cellspacing="0" cellpadding="0" width="100%" border="0" align="center">                       
                        <tr class="trTitle rounded-corners" id="dvContent2">
                            <td class="dgitem" align="left">
                                <asp:Label ID="lblCtrlTitle" runat="server" CssClass="bookingnormallabel" EnableViewState="False">Label</asp:Label>
                            </td>
                        </tr>
                        <tr class="trContent rounded-corners" id="dvContent">
                            <td class="tdContent" colspan="2">
                                <uc1:ctrRatePlanRules ID="CtrRatePlanRules1" runat="server"></uc1:ctrRatePlanRules>
                                <br />
                               
                                <%If Me.IsSupervisor Then
								            Me.btnPublicar.Text = RateManager.PortalCulture.GetString("01364")
                                %>
                                <asp:Button ID="btnPublicar" runat="server" Text="Publicar" CssClass="button" EnableViewState="False">
                                </asp:Button>
                                <%
									End If
                                %>
                                 <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" CssClass="button" EnableViewState="False">
                                </asp:Button>
                                <asp:Button ID="btnCancel" runat="server" Text="Cancelar" CausesValidation="False"
                                    CssClass="button" EnableViewState="False"></asp:Button>
                                <br />
                                <asp:Label ID="lblError" runat="server" CssClass="Validators" Visible="False">Error</asp:Label>
                                <asp:Label ID="lblErrorDelete" runat="server" CssClass="Validators">Label</asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        </div>
   
    <div class="clear">
        <asp:DataGrid ID="grid" runat="server" GridLines="None" AutoGenerateColumns="False"
            AllowPaging="True" CssClass="datagrid" PageSize="20" Width="99%" ShowFooter="True">
            <SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
            <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
            <ItemStyle CssClass="dgItem"></ItemStyle>
            <HeaderStyle CssClass="dgHeader"></HeaderStyle>
            <FooterStyle HorizontalAlign="Right"></FooterStyle>
            <Columns>
                <asp:TemplateColumn Visible="False" HeaderText="Code">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkIdRatePlan" runat="server" CommandName="Select" CausesValidation="False"
                            CssClass="dgLink">
														<%#databinder.eval(container.dataitem,"idrule")%>
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn DataField="RuleDescription" HeaderText="Rule">
                    <ItemStyle Width="40%"></ItemStyle>
                </asp:BoundColumn>
                <asp:BoundColumn DataField="GuarDep" HeaderText="Guar/Dep">
                    <ItemStyle Width="20%"></ItemStyle>
                </asp:BoundColumn>
                <asp:BoundColumn DataField="ReqVerification" HeaderText="Req. Verification">
                    <ItemStyle Width="20%"></ItemStyle>
                </asp:BoundColumn>
                <asp:TemplateColumn>
                    <HeaderStyle Width="10%"></HeaderStyle>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Select" CausesValidation="False"
                            CssClass="dgLink">Editar
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderStyle Width="10%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkEliminar2" runat="server" CssClass="dgLink" CausesValidation="False"
                            CommandName="Eliminar"  style="display:none;">Delete</asp:LinkButton>
                        <asp:HyperLink ID="lnkEliminar" runat="server" CssClass="dglink">Eliminar</asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn Visible="False" DataField="RuleDescription"></asp:BoundColumn>
            </Columns>
            <PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
                Position="Bottom" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
        </asp:DataGrid>
    </div>
    <uc1:ctlMensajes ID="CtlMensajes1" runat="server"></uc1:ctlMensajes>
    </form>

    <script type="text/javascript">
        var warningMessage = '<%=  RateManager.PortalCulture.GetString("01349") %>';

        $(document).ready(function() {
            $('#<%= Me.btnAceptar.ClientId %> ,#<%= Me.btnPublicar.ClientId %>').click(function() {
                var result = false;
                if (showTimeChangedNotify && CacellationTimeHasChanged()) {
                    showTimeChangedNotify = false;
                    result = confirm('<%=  RateManager.PortalCulture.GetString("01349") %>');
                } else { result = true; } return result;
            });
        });

        function ShowDescription(Plan, lbl) {
            var S = (document.getElementById(Plan)).value;
            var e = document.getElementById(lbl);
            e.innerText = S;
        }

        /*function LoadMsg(ddl, lblAux,lblDay,dia,hora,specific,aux,canc,ddlHour,ddlMinutes,lblSep,txt) 
        {
        var l1 = document.getElementById(lblDay);
        var l2 = document.getElementById(lblAux);
        var d = document.getElementById(ddl);			
        var ddl1 = document.getElementById(ddlHour);			
        var ddl2 = document.getElementById(ddlMinutes);				
        var lbls = document.getElementById(lblSep);				
        var txt = document.getElementById(txt);			
        txt.style.display='';
        ddl1.style.display='none';
        ddl2.style.display='none';
        lbls.style.display='none';
        l1.style.display='';
        l2.style.display= '';
        switch (d.selectedIndex)
        {
        case 0:
        //l1.firstChild.nodeValue = '';
        //l2.firstChild.nodeValue = '';
        txt.style.display='none';
        l1.style.display='none';
        l2.style.display= 'none';
        break;
        case 1:
        l1.firstChild.nodeValue = dia;
        l2.firstChild.nodeValue = canc;
        break;
        case 2:
        l1.firstChild.nodeValue = hora;
        l2.firstChild.nodeValue = canc;
        break;
        case 3:
        l1.firstChild.nodeValue = specific;
        l2.firstChild.nodeValue = aux;
        txt.style.display='none';
        ddl1.style.display='';
        ddl2.style.display='';
        lbls.style.display='';
        break;					
        }
    			
        }*/
    </script>

</body>
</html>
