<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Search.aspx.vb" Inherits="RateManager.Search" %>

<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../Modules/ctrlHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../Modules/ctrlFooter.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrMenu" Src="../Modules/CtrMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlSearchCompany" Src="../Modules/ctrlSearchCompany.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>Search</title>

    <script language="javascript" src="../../xmlHttp.js"></script>

    <script language="javascript" src="../../Logic.js"></script>

    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="https://schemas.microsoft.com/intellisense/ie5">
    <link rel="stylesheet" type="text/css" href="../../StyleSheets/Styles.css">
</head>
<body ms_positioning="FlowLayout" bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0">
    <form id="Form1" method="post" runat="server">
    <div id="tituloSeccion">
        <img src="../../Includes/imagenes/icono-big-plus.png" width="25" height="25">
       <asp:Label class="tituloSeccion" ID="lblTitulo" runat="server" EnableViewState="False">Buscar hotel</asp:Label>
    </div>
    <table id="bookingcontainer" cellspacing="0" cellpadding="2" width="650" border="0">
        <tr>
            <td>
                <table id="Table1" cellspacing="0" cellpadding="0" width="100%" align="center">                   
                    <tr>
                        <td>
                            <uc1:ctrlSearchCompany ID="CtrlSearchCompany1" runat="server"></uc1:ctrlSearchCompany>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnSearch" runat="server" Text="Actualizar lista" CssClass="Button"
                                EnableViewState="False" style="float: none !important;"></asp:Button>
                        </td>
                    </tr>
                    <tr height="5"> 
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:DataGrid ID="Grid"  GridLines="None" runat="server" Width="99%" CssClass="DataGrid" PageSize="15"
                                AllowPaging="True" AutoGenerateColumns="False" Border="0" CellSpacing="1" ShowFooter="True">
                                <FooterStyle HorizontalAlign="Right"></FooterStyle>
                                <SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
                                <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
                                <ItemStyle CssClass="dgItem"></ItemStyle>
                                <HeaderStyle CssClass="dgHeader"></HeaderStyle>
                                <Columns>
                                    <asp:TemplateColumn HeaderText="Nombre&#160;Empresa">
                                        <ItemStyle Width="20%"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkSelect" runat="server" CssClass="dgLink" CommandName="Select"
                                                CausesValidation="false">
														<%# databinder.eval(container.dataitem,"NombreEmpresa") %>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="NombreCorp" HeaderText="NombreCorp"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="idEmpresa"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="idRubro"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="ID"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="NombreEmpresa"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="Domicilio"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="Estado" HeaderText="Estado">
                                        <ItemStyle Width="15%"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Municipio" HeaderText="Municipio">
                                        <ItemStyle Width="10%"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Ciudad" HeaderText="Ciudad">
                                        <ItemStyle Width="10%"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Contacto_Nombre" HeaderText="Nombre contacto">
                                        <ItemStyle Width="10%"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Contacto_Email" HeaderText="Email contacto">
                                        <ItemStyle Width="25%"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Telefono" HeaderText="Telefono">
                                        <ItemStyle Width="10%"></ItemStyle>
                                    </asp:BoundColumn>
                                     <asp:BoundColumn DataField="StatusAvailability" HeaderText="Status Empresa">
                                        <ItemStyle Width="10%"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Active" HeaderText="Estatus del hotel">
                                        <ItemStyle Width="10%"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="UserPerfil" HeaderText="userperfil">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="Idpais" HeaderText="Idpais"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="EsMoroso"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="idcorporativo"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="idasociacion"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="isHouse"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="isSingleImgInv"></asp:BoundColumn>
                                </Columns>
                                <PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
                                    Position="Bottom" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
                            </asp:DataGrid>
                        </td>
                    </tr>
                    <tr height="5">
                        <td>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
