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
        <div class="clear">

            <div hidden>
                <asp:Label Style="z-index: 0" ID="lblHotelSelected" runat="server" CssClass="bookingnormallabel">Hotel Seleccionado</asp:Label>
                <asp:Label ID="lblTitle" runat="server" EnableViewState="False">Configuración de Conexión Conflux</asp:Label>
            </div>

            <!-- Filtros -->
            <div style="display: flex; margin-bottom: 15px;">
                <asp:Label ID="lblCorp" runat="server" CssClass="bookingnormallabel" Style="margin-top: 0.8%; margin-right: 4px;">Corporativos: </asp:Label>
                <asp:DropDownList ID="ddlCorporatives" runat="server" Width="232px" AutoPostBack="True"></asp:DropDownList>

                <asp:Label ID="lblHotels" runat="server" CssClass="bookingnormallabel" Style="margin-top: 0.8%; margin-right: 4px;">Hoteles: </asp:Label>
                <asp:DropDownList ID="ddlHoteles" runat="server" Width="232px"></asp:DropDownList>

                <asp:Button ID="btnCargar" runat="server" EnableViewState="False" CssClass="button" Text="Cargar"></asp:Button>

                <!-- Inicia - Boton Excell -->
                <div class="followMenu">
                    <asp:HyperLink ID="btnExcel" runat="server" Enabled="True" NavigateUrl="../HotelAdministrator/pages/ExportExcellPMSCodes.aspx" ToolTip="Excel" ImageUrl="../Images/excel.png"></asp:HyperLink>
                    <asp:Button ID="btnSendToConflux" runat="server" EnableViewState="False" CssClass="button" Text="Enviar a Conflux" Visible="false"></asp:Button>
                </div>
                <!-- Fin - Boton Excell -->
            </div>

            <!-- Datos -->
            <div class="dgitem" Style="margin-bottom: 10px;">
                <asp:Label ID="lbl_PMS_Data" runat="server" EnableViewState="False" Visible="false">Datos PMS</asp:Label></div>
            <asp:Table runat="server" ID="generalTable" HorizontalAlign="Center">
                <asp:TableRow HorizontalAlign="center">
                    <asp:TableHeaderCell ID="lblRatesPlan" Visible="false">Rates Plan</asp:TableHeaderCell>
                    <asp:TableHeaderCell ID="lblRooms" Visible="false">Rooms</asp:TableHeaderCell>
                </asp:TableRow>
                <asp:TableRow HorizontalAlign="center">
                    <asp:TableCell BorderColor="White" BorderWidth="5px">
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
                    </asp:TableCell>

                    <asp:TableCell BorderColor="White" BorderWidth="5px">
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
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>

            <div id="ConfluxTables">
                <div class="dgitem">
                    <asp:Label ID="lbl_Conflux_Data_RP" CssClass="subTituloSeccion" Font-Size="Large" runat="server" EnableViewState="False" Visible="false">Datos Para Conflux - Rate Plans</asp:Label></div>

                <!-- Tabla Rateplans -->
                <asp:DataGrid ID="dgCFRP" runat="server" CssClass="datagrid" AutoGenerateColumns="True" ShowFooter="False">
                    <SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
                    <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
                    <ItemStyle CssClass="dgItem"></ItemStyle>
                    <HeaderStyle CssClass="dgHeader"></HeaderStyle>
                </asp:DataGrid>

                <div class="dgitem">
                    <asp:Label ID="lbl_Conflux_Data_RT" CssClass="subTituloSeccion" Font-Size="Large" runat="server" EnableViewState="False" Visible="false">Datos Para Conflux - Room Types</asp:Label></div>
                <!-- Tabla RoomTypes -->
                <asp:DataGrid ID="dgCFRT" runat="server" CssClass="datagrid" AutoGenerateColumns="True" ShowFooter="False">
                    <SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
                    <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
                    <ItemStyle CssClass="dgItem"></ItemStyle>
                    <HeaderStyle CssClass="dgHeader"></HeaderStyle>
                </asp:DataGrid>
            </div>

            <!-- botones inferiores -->
            <div hidden>
                <asp:Label ID="lblMsgActualizacion" runat="server" CssClass="Validators" Visible="False"> Operacion Exitosa</asp:Label>
                <asp:Button ID="btnAceptar" runat="server" EnableViewState="False" CssClass="button" Text="Guardar" Visible="false"></asp:Button>
                <asp:Button ID="btnCancel" runat="server" EnableViewState="False" CssClass="button" Text="Cancelar" CausesValidation="False"></asp:Button>
            </div>

            <uc1:ctlMensajes ID="CtlMensajes1" runat="server"></uc1:ctlMensajes>

        </div>
    </form>
</body>
</html>
