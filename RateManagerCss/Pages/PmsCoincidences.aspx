<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PmsCoincidences.aspx.vb" Inherits="RateManager.PmsCoincidences" %>

<%@ Register TagPrefix="uc1" TagName="CtrMenu" Src="../Portal/Modules/CtrMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctlMensajes" Src="../Modulos/ctlMensajes.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../Portal/Modules/ctrlHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../Portal/Modules/ctrlFooter.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>PMS Coincidences</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link rel="stylesheet" type="text/css" href="../StyleSheets/Styles.css">
</head>
<body bottommargin="0" leftmargin="0" rightmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
        <table id="bookingcontainer" border="0" cellspacing="0" cellpadding="2" width="650">
            <tr>
                <td>
                    <table id="Table1" border="0" cellspacing="0" cellpadding="0" width="100%" align="center">
                        <tr>
                            <td class="Titulo" colspan="3" align="center">
                                <asp:Label ID="lblTitle" runat="server" EnableViewState="False">Configuración de Conexión PMS</asp:Label></td>
                        </tr>
                        <tr>
                            <td style="height: 22px" width="50%" colspan="2" align="left">
                                <asp:DropDownList ID="ddlCorporatives" runat="server" Width="232px" AutoPostBack="True"></asp:DropDownList>&nbsp;
                                <asp:DropDownList ID="ddlHoteles" runat="server" Width="232px"></asp:DropDownList>&nbsp;
									<asp:Button Style="z-index: 0" ID="btnCargar" runat="server" EnableViewState="False" CssClass="button"
                                        Text="Cargar"></asp:Button>
                                <!-- Boton Excell -->
                                <div style="margin-left: 15px;">
                                    <asp:HyperLink ID="btnExcel" runat="server" Enabled="True" NavigateUrl="../HotelAdministrator/pages/ExportExcellPMSCodes.aspx"
                                        ToolTip="Excel" ImageUrl="../Images/excel.png"></asp:HyperLink>
                                </div>
                            </td>
                        </tr>
                        <tr align="center">
                            <td style="height: 13px"></td>
                            <td style="height: 13px"></td>
                        </tr>
                        <tr align="center">
                            <asp:Label Style="z-index: 0" ID="lblHotelSelected" runat="server" CssClass="bookingnormallabel">Hotel Seleccionado</asp:Label>
                        </tr>
                        <tr>
                            <td style="height: 11px" align="center"></td>
                            <td style="height: 11px" align="center"></td>
                        </tr>
                        <tr align="center">
                            <td>
                                <asp:Label ID="lblRatesPlan" runat="server" CssClass="bookingnormallabel">Rates Plan</asp:Label></td>
                            <td>
                                <asp:Label ID="lblHoteles" runat="server" CssClass="bookingnormallabel">Rooms</asp:Label></td>
                        </tr>
                        <tr id="renglonDatos" runat="server">
                            <td valign="top" align="center">
                                <div style="height: 350px; overflow: auto" id="ratesPlan" runat="server" ms_positioning="FlowLayout">
                                    <asp:DataGrid ID="dgRatesPlan" runat="server" CssClass="datagrid" AutoGenerateColumns="False" ShowFooter="False">
                                        <SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
                                        <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
                                        <ItemStyle CssClass="dgItem"></ItemStyle>
                                        <HeaderStyle CssClass="dgHeader"></HeaderStyle>
                                        <Columns>
                                            <asp:BoundColumn DataField="idRatePlan" HeaderText="Codigo"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="Nombre" HeaderText="Nombre"></asp:BoundColumn>
                                            <asp:BoundColumn Visible="False" DataField="idRatePlanPMS"></asp:BoundColumn>
                                            <asp:TemplateColumn HeaderText="Hotel" Visible="false">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtidRatePlanPMS" runat="server" MaxLength="100"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                        </Columns>
                                    </asp:DataGrid>
                                    <asp:Label Style="z-index: 0" ID="msgRatesPlan" runat="server" CssClass="Validators" Visible="False">No existen Planes Tarifarios</asp:Label>
                                </div>
                            </td>
                            <td valign="top">
                                <div style="height: 350px; overflow: auto" id="rooms" runat="server" ms_positioning="FlowLayout">
                                    <asp:DataGrid ID="dgRooms" runat="server" CssClass="datagrid" AutoGenerateColumns="False" ShowFooter="False">
                                        <SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
                                        <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
                                        <ItemStyle CssClass="dgItem"></ItemStyle>
                                        <HeaderStyle CssClass="dgHeader"></HeaderStyle>
                                        <Columns>
                                            <asp:BoundColumn DataField="CodigoHabitacion" HeaderText="Codigo"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="Nombre" HeaderText="Nombre"></asp:BoundColumn>
                                            <asp:BoundColumn Visible="False" DataField="CodigoHabitacionPMS"></asp:BoundColumn>
                                            <asp:TemplateColumn Visible="false">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtCodigoHabitacionPMS" runat="server" MaxLength="100"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                        </Columns>
                                    </asp:DataGrid>
                                    <asp:Label Style="z-index: 0" ID="msgRooms" runat="server" CssClass="Validators" Visible="False">No existen Habitaciones</asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 22px" colspan="3" align="center">
                                <asp:Label ID="lblMsgActualizacion" runat="server" CssClass="Validators" Visible="False"> Operacion Exitosa</asp:Label></td>
                        </tr>
                        <tr id="renglonBotones" runat="server">
                            <td align="center" colspan="3">
                                <asp:Button ID="btnAceptar" runat="server" EnableViewState="False" CssClass="button" Text="Guardar" Visible="false"></asp:Button>
                                <asp:Button ID="btnCancel" runat="server" EnableViewState="False" CssClass="button" Text="Cancelar"
                                    CausesValidation="False"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <uc1:ctlMensajes ID="CtlMensajes1" runat="server"></uc1:ctlMensajes>
    </form>
</body>
</html>
