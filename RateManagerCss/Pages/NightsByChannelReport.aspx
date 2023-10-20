<%@ Import Namespace="RateManager" %>

<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../Portal/Modules/ctrlHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../Portal/Modules/ctrlFooter.ascx" %>
<%@ Register Src="../Modulos/ctrlAutoComplete.ascx" TagName="ctrlAutoComplete" TagPrefix="uc2" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="NightsByChannelReport.aspx.vb" Inherits="RateManager.NightsByChannelReport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Reporte - Noches por canal</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link rel="stylesheet" type="text/css" href="../StyleSheets/Styles.css">

    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.5/jquery.min.js"></script>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js"></script>
    <script type="text/javascript" src="../Includes/Script/JsSearch-1.0.js"></script>

    <script>
        SearchStart.AddParam
            (
                {
                    searchitems: [
                        { Item: 'Properties', IDSearch: 'IdEmpresa', nameSearch: 'IdEmpresa', isdefault: false }
                        , { Item: 'Properties', IDSearch: 'IdEmpresa', nameSearch: 'NombreEmpresa', isdefault: true }
                    ],
                    colModel: [
                        { display: '<%= RateManager.PortalCulture.GetString("00001") %>' },
                        { display: '<%= RateManager.PortalCulture.GetString("00073") %>' },
                    ],
                    Data: [{
                        catalogo: 'Properties', idUsuario: '<%= MyBase.Usuario %>', idPais: 'MX,HN',
                        idIdioma: '<%= RateManager.PortalCulture.GetIDCulture %>',
                        IsSupervisor: '<%= MyBase.IsSupervisor %>'
                    }],
                    id: 'IdEmpresa',
                    index: 1
                }
            );
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="clear">

            <!-- Filtros -->
            <div style="display: flex; margin-bottom: 15px;">

                <!-- Filtro Status -->
                <div hidden>
                    <asp:Label ID="lblFilter" runat="server" Text="Filtro:"></asp:Label>
                    <asp:DropDownList ID="ddlDeletedFilter" runat="server" AutoPostBack="False">
                        <asp:ListItem Text="Solo activos" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Solo no activos" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Todos" Value=""></asp:ListItem>
                    </asp:DropDownList>

                </div>
                <!-- Filtro Año -->
                <div style="margin-left: 15px;">
                    <asp:Label ID="Label_year" runat="server" Text="Año:"></asp:Label>
                    <asp:DropDownList ID="ddlYear" runat="server" AutoPostBack="False">
                    </asp:DropDownList>
                </div>
                <!-- Filtro Mes -->
                <div style="margin-left: 15px;">
                    <asp:Label ID="Label1" runat="server" Text="Mes:"></asp:Label>
                    <asp:DropDownList ID="ddlMonth" runat="server" AutoPostBack="False">
                        <asp:ListItem Text="Enero" Value="01"></asp:ListItem>
                        <asp:ListItem Text="Febrero" Value="02"></asp:ListItem>
                        <asp:ListItem Text="Marzo" Value="03"></asp:ListItem>
                        <asp:ListItem Text="Abril" Value="04"></asp:ListItem>
                        <asp:ListItem Text="Mayo" Value="05"></asp:ListItem>
                        <asp:ListItem Text="Junio" Value="06"></asp:ListItem>
                        <asp:ListItem Text="Julio" Value="07"></asp:ListItem>
                        <asp:ListItem Text="Agosto" Value="08"></asp:ListItem>
                        <asp:ListItem Text="Septiembre" Value="09"></asp:ListItem>
                        <asp:ListItem Text="Octubre" Value="10"></asp:ListItem>
                        <asp:ListItem Text="Noviembre" Value="11"></asp:ListItem>
                        <asp:ListItem Text="Diciembre" Value="12"></asp:ListItem>
                    </asp:DropDownList>
                </div>

                <!-- Boton Excell -->
                <div style="margin-left: 15px;">
                    <asp:HyperLink ID="btnExcel" runat="server" Enabled="True" NavigateUrl="../HotelAdministrator/pages/ExportExcellReportNBC.aspx"
                        ToolTip="Excel" ImageUrl="../Images/excel.png"></asp:HyperLink>
                </div>

                <!-- Autocomplete -->
                <div style="width: 90%">
                    <span id="lblAutoComplete" style="float: right; margin-right: 53%;">Hotel: </span>
                    <uc2:ctrlAutoComplete ID="ctrlAutoCompleteHotels" runat="server" />
                </div>
            </div>

            <!-- DataGrid -->
            <div>
                <asp:DataGrid ID="nightsByChannel" runat="server" Width="70%" AllowPaging="False" PageSize="20"
                    GridLines="Both" AutoGenerateColumns="True" CssClass="DataGrid" ShowFooter="True">
                    <SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
                    <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
                    <ItemStyle CssClass="dgItem"></ItemStyle>
                    <HeaderStyle CssClass="dgHeader"></HeaderStyle>
                    <FooterStyle HorizontalAlign="Right"></FooterStyle>
                </asp:DataGrid>
            </div>
        </div>
    </form>
</body>
</html>
