<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="HotelBeds.aspx.vb" Inherits="RateManager.HotelBeds" %>
<%@ Import Namespace="Ratemanager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <div style="float: right; width: 50%">
        <div class="title">
            <asp:Label runat="server" ID="lblTitleRelated" CssClass="tituloSeccion">Ciudades y Destino Relacionados</asp:Label>
        </div>
        <div class="clear">
        </div>
        <div>
            <asp:Label runat="server" ID="lblSearchRelated" CssClass="clsLabel">Búsqueda de Ciudad-Destino:</asp:Label>
            <asp:TextBox ID="txtSearchRelated" runat="server" Width="100" Height="20" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
            <asp:Button ID="btnSearchRelated" runat="server" Text="Buscar" OnClick="btnSearchRelated_Click"
                CssClass="button" />
        </div>
        <div class="tabla" style="width: 90%">
            <center class="divSearch">
                <asp:GridView ID="gvRelacion" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvRelacion_RowDataBound"
                    ShowFooter="true" CssClass="DataGrid" GridLines="None" AllowPaging="true" PageSize="10" OnRowDeleting="gvRelacion_RowDeleting"
                    OnRowEditing="gvRelacion_RowEditing" OnRowCancelingEdit="gvRelacion_RowCancelingEdit" OnRowUpdating="gvRelacion_RowUpdating">
                    <Columns>
                        <asp:BoundField DataField="idRelacion" HeaderText="idRelacion" InsertVisible="False" Visible="true"
                            ReadOnly="True" SortExpression="idRelacion" ItemStyle-Width="10%">
                            <HeaderStyle CssClass="dgHeader" />
                        </asp:BoundField>
                        <asp:BoundField DataField="idCiudad" HeaderText="idCiudad" SortExpression="idCiudad"
                            ReadOnly="true" ItemStyle-Width="10%">
                            <HeaderStyle CssClass="dgHeader" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre" ReadOnly="true"
                            ItemStyle-Width="20%">
                            <HeaderStyle CssClass="dgHeader" />
                        </asp:BoundField>
                        <%--<asp:BoundField DataField="codigo" HeaderText="codigo" SortExpression="codigo">
                            <HeaderStyle CssClass="dgHeader" />
                        </asp:BoundField>--%>
                        <asp:TemplateField ItemStyle-Width="" HeaderText="Código HB">
                            <ItemStyle Width="10%" />
                            <HeaderStyle CssClass="dgHeader" />
                            <ItemTemplate>
                                <asp:Label ID="lblCodigo" runat="server" Text='<%# DataBinder.Eval(Container.Dataitem,"codigo") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtCodigo" runat="server" CssClass="" Style="display: block;" Columns="5"
                                    MaxLength="3" Width="40px" Text='<%# DataBinder.Eval(Container.Dataitem,"codigo") %>'></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:BoundField DataField="ZoneCode" HeaderText="ZoneCode" SortExpression="ZoneCode">
                            <HeaderStyle CssClass="dgHeader" />
                        </asp:BoundField>--%>
                        <asp:TemplateField ItemStyle-Width="" HeaderText="ZoneCode HB">
                            <ItemStyle Width="10%" />
                            <HeaderStyle CssClass="dgHeader" />
                            <ItemTemplate>
                                <asp:Label ID="lblZoneCode" runat="server" Text='<%# DataBinder.Eval(Container.Dataitem,"ZoneCode") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtZoneCode" runat="server" CssClass="" Style="display: block;"
                                    Columns="5" MaxLength="5" Width="30px" Text='<%# DataBinder.Eval(Container.Dataitem,"ZoneCode") %>'></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action" ItemStyle-Width="20%">
                            <HeaderStyle CssClass="dgHeader" />
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEdit" Text='<%#PortalCulture.GetString("00763") %>' runat="server" CommandName="Edit" />&nbsp;&nbsp;
                                <asp:LinkButton ID="btnEliminar" Text='<%#PortalCulture.GetString("00451") %>' runat="server" CommandName="Delete" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="btnUpdate" Text='<%#PortalCulture.GetString("M000142") %>' runat="server" CommandName="Update"
                                    ValidationGroup="v1" />
                                <asp:LinkButton ID="btnCancel" Text='<%#PortalCulture.GetString("00413") %>' runat="server" CommandName="Cancel" />
                            </EditItemTemplate>
                            <ControlStyle ForeColor="#6600FF" />
                            <ItemStyle Font-Size="Medium" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </center>
        </div>
        <div class="clear">
        </div>
        <div>
            <asp:Button runat="server" ID="btnNuevo" Text="Add" CssClass="button" />
            <div>
                <asp:Label runat="server" id="lblAddError" Visible="false" CssClass="Validators" EnableViewState="false">Error</asp:Label>
            </div>
        </div>
        <div id="divAddRelation" style="display: block; margin-top: 10px" runat="server">
            <div style="margin-top: 5px">
                <asp:Button runat="server" ID="btnAddRelation" Text="Add" CssClass="button" />
                <asp:Button runat="server" ID="btnCancelRelation" Text="Cancel" CssClass="button" />                
            </div>            
        </div>
    </div>
    <div style="">
        <div class="tituloSeccion">
            <asp:Label runat="server" ID="lblTitleUVCities" CssClass="tituloSeccion">Ciudades de Univisit</asp:Label>
        </div>
        <div class="clear">
        </div>
        <div>
            <asp:Label runat="server" ID="lblSearch" CssClass="clsLabel">Búsqueda de Ciudad:</asp:Label>
            <asp:TextBox ID="txtSearch" runat="server" Width="100" Height="20" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click"
                CssClass="button" />
        </div>
        <div class="tabla" style="width: 85%">
            <center class="divSearch">
                <asp:GridView ID="gridList" runat="server" AutoGenerateColumns="False" OnRowDataBound="gridList_RowDataBound"
                    CssClass="DataGrid" GridLines="None" AllowPaging="true" PageSize="10">
                    <Columns>
                        <asp:BoundField DataField="IdCiudad" HeaderText="IdCiudad" InsertVisible="False"
                            ReadOnly="True" SortExpression="IdCiudad">
                            <HeaderStyle CssClass="dgHeader" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre">
                            <HeaderStyle CssClass="dgHeader" />
                        </asp:BoundField>
                        <asp:BoundField DataField="codigo" HeaderText="codigo" SortExpression="codigo">
                            <HeaderStyle CssClass="dgHeader" />
                        </asp:BoundField>
                        <asp:BoundField DataField="NumRel" HeaderText="NumRel" SortExpression="NumRel">
                            <ItemStyle Width="20%" />
                            <HeaderStyle CssClass="dgHeader" />
                        </asp:BoundField>
                        <asp:TemplateField>
                            <ItemStyle Width="15%" HorizontalAlign="Center"/>
                            <HeaderStyle CssClass="dgHeader" />
                            <ItemTemplate>
                                <asp:LinkButton ID="btnSeleccionar" Text='<%#PortalCulture.GetString("00753") %>' runat="server" CommandName="Select" Visible="false"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:HyperLinkField DataNavigateUrlFields="IdCiudad" DataNavigateUrlFormatString="HotelBeds_ModifyDestination.aspx?IdCiudad={0}"
                            Text="Modificar" Visible="false">
                            <HeaderStyle CssClass="dgHeader" />
                        </asp:HyperLinkField>
                    </Columns>
                </asp:GridView>
            </center>
        </div>
        <br />
        <br />
        <div class="tituloSeccion">
            <asp:Label runat="server" ID="lblTitleDestinations" CssClass="tituloSeccion">Destinos de Hotel Beds</asp:Label>
        </div>
        <div class="clear">
        </div>
        <div>
            <asp:Label runat="server" ID="lblSearchDestination" CssClass="clsLabel">Búsqueda de Destinos:</asp:Label>
            <asp:TextBox ID="txtSearchDestination" runat="server" Width="100" Height="20" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
            <asp:Button ID="btnSearchDestination" runat="server" Text="Buscar" OnClick="btnSearchDestination_Click"
                CssClass="button" />
        </div>
        <div class="tabla" style="width: 85%">
            <center class="divSearch">
                <asp:GridView ID="gvDestinosHB" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvDestinosHB_RowDataBound"
                    CssClass="DataGrid" GridLines="None" AllowPaging="true" PageSize="10">
                    <Columns>
                        <asp:BoundField DataField="DestinationCode" HeaderText="DestinationCode" InsertVisible="False"
                            ReadOnly="True" SortExpression="DestinationCode">
                            <HeaderStyle CssClass="dgHeader" />
                        </asp:BoundField>
                        <asp:BoundField DataField="CountryCode" HeaderText="CountryCode" SortExpression="CountryCode">
                            <HeaderStyle CssClass="dgHeader" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DestinationName" HeaderText="DestinationName" SortExpression="DestinationName">
                            <HeaderStyle CssClass="dgHeader" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ZoneCode" HeaderText="ZoneCode" SortExpression="ZoneCode">
                            <HeaderStyle CssClass="dgHeader" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ZoneName" HeaderText="Zone Name" SortExpression="ZoneName">
                            <HeaderStyle CssClass="dgHeader" />
                        </asp:BoundField>
                        <asp:BoundField DataField="idCiudad" HeaderText="idCiudad" SortExpression="idCiudad">
                            <HeaderStyle CssClass="dgHeader" />
                        </asp:BoundField>
                        <asp:BoundField DataField="codigo" HeaderText="codigo" SortExpression="codigo">
                            <HeaderStyle CssClass="dgHeader" />
                        </asp:BoundField>
                        <asp:TemplateField>
                            <ItemStyle Width="15%" HorizontalAlign="Center"/>
                            <HeaderStyle CssClass="dgHeader" />
                            <ItemTemplate>
                                <asp:LinkButton ID="btnSeleccionar" Text='<%#PortalCulture.GetString("00753") %>' runat="server" CommandName="Select" Visible="false"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </center>
        </div>
    </div>
    </form>

    <script type="text/javascript">
        function showAddDestination(btn, div, lblError) {
            if (btn.style.display == 'block') {
                btn.style.display = 'none';
                div.style.display = 'block';
                lblError.style.display = "none";
            }
        }
    </script>

</body>
</html>
<%-- <asp:Content ID="ctntPrincipal" ContentPlaceHolderID="ContainerPage" runat="server">
       
    </asp:Content>--%>