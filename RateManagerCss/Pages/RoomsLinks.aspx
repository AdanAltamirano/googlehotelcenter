<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../Portal/Modules/ctrlFooter.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../Portal/Modules/ctrlHeader.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" ValidateRequest="false" CodeBehind="RoomsLinks.aspx.vb"
    Inherits="RateManager.RoomsLinks" %>

<%@ Register TagPrefix="uc1" TagName="ctlMensajes" Src="../Modulos/ctlMensajes.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrMenu" Src="../Portal/Modules/CtrMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="RoomLink" Src="../Modulos/RoomLink.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>RatePlansLinks</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet">

    <script type="text/javascript">
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

</head>
<body ms_positioning="FlowLayout" bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0">
    <form id="Form1" method="post" runat="server">
    <div class="clear">
        <img class="bgplus" src="../Includes/imagenes/icono-big-plus.png" width="25" height="25" />
        <asp:Label ID="lblTitle" runat="server" EnableViewState="False" CssClass="tituloSeccion"> Rooms Linked</asp:Label>
        <input type="button" id="cmdNew" class="ButtonNew" runat="server" causesvalidation="false"
            value="New" style="float: right; width: 85px; margin-right: 8px;" />
    </div>
    <div runat="server" id="divContenedor">
        <table id="bookingcontainer" cellspacing="0" cellpadding="0" width="100%" border="0">
            <tr>
                <td class="tdContent">
                    <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0" align="center">
                        <tr>
                            <td align="left" colspan="2">
                                <br>
                                <uc1:RoomLink ID="CtrRoomLink1" runat="server"></uc1:RoomLink>
                                <br>
                                <asp:Label ID="lblError" runat="server" CssClass="Validators">El plan ya tiene un link</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Button ID="btnAceptar" runat="server" Text="Guardar" CssClass="button" EnableViewState="False" CausesValidation=true >
                                </asp:Button>
                                <asp:Button ID="btnCancel" runat="server" Text="Cancelar" CssClass="button" CausesValidation="False"
                                    EnableViewState="False"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <div class="clear">
        <asp:DataGrid ID="dgLinks" runat="server" GridLines="None" AutoGenerateColumns="False"
            Width="99%" AllowPaging="True" PageSize="20" CssClass="DataGrid" ShowFooter="True">
            <SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
            <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
            <ItemStyle CssClass="dgItem"></ItemStyle>
            <HeaderStyle CssClass="dgHeader"></HeaderStyle>
            <FooterStyle HorizontalAlign="Right"></FooterStyle>
            <Columns>
                <asp:BoundColumn DataField="RoomSource" HeaderText="source">
                    <ItemStyle Width="30%"></ItemStyle>
                </asp:BoundColumn>
                <asp:BoundColumn DataField="RoomTarget" HeaderText="source">
                    <ItemStyle Width="30%"></ItemStyle>
                </asp:BoundColumn>
                <asp:BoundColumn DataField="OnePersonRatio" HeaderText="1 person Ratio">
                    <HeaderStyle Width="10%" HorizontalAlign="center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="center" />
                </asp:BoundColumn>
                <asp:BoundColumn DataField="OnePersonOffset" HeaderText="1 person Offset">
                    <HeaderStyle Width="10%" HorizontalAlign="Right"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundColumn>
                <asp:BoundColumn Visible="False" DataField="RoundAmount" HeaderText="Round Amount">
                </asp:BoundColumn>
                <asp:TemplateColumn>
                    <ItemStyle Width="10%" HorizontalAlign="Center"></ItemStyle>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Select" CssClass="dgLink">Editar</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle Width="10%" HorizontalAlign="Center"></ItemStyle>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkEliminar2" Style="display: none" runat="server" CssClass="dgLink"
                            CommandName="Eliminar">Eliminar</asp:LinkButton>
                        <asp:HyperLink ID="lnkEliminar" runat="server" CssClass="dglink">Eliminar</asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn Visible="False" DataField="idtipohabitacion_Target"></asp:BoundColumn>
                <asp:BoundColumn Visible="False" DataField="idtipohabitacion_Source"></asp:BoundColumn>
                <asp:BoundColumn Visible="False" DataField="RoomTarget"></asp:BoundColumn>
                <asp:BoundColumn Visible="False" DataField="NameTarget"></asp:BoundColumn>
                <asp:BoundColumn Visible="False" DataField="NameSource"></asp:BoundColumn>
            </Columns>
            <PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
                Position="Bottom" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
        </asp:DataGrid>
    </div>
    <div class="clear">
        <uc1:ctlMensajes ID="CtlMensajes1" runat="server"></uc1:ctlMensajes>
    </form>
</body>
</html>
