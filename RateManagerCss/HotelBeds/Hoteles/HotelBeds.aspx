<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="HotelBeds.aspx.vb" Inherits="RateManager.HotelBeds1" %>
<%@ Import Namespace="Ratemanager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../StyleSheets/Styles.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="tituloSeccion">
        <asp:Label runat="server" ID="lblTitleADO" CssClass="tituloSeccion">Listado De Hoteles Contratados</asp:Label>
    </div>
    <div class="clear">
    </div>
    <div>
        <div>
            <a href="HotelFix.aspx" runat="server"><asp:Label runat="server" ID="lblAgregados" CssClass="clsLabel">Agregados</asp:Label></a><br />
            <asp:Label runat="server" ID="lblSearch" CssClass="clsLabel">Búsqueda de Hoteles:</asp:Label>
            <asp:TextBox ID="txtSearch" onkeydown="return (event.keyCode!=13);" runat="server" Width="150px" Height="20" CssClass="textbox"></asp:TextBox>
            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click"
                CssClass="button" />
        </div>
        <div class="tabla" style="">
            <center class="divSearch" style="">
                <asp:GridView ID="gridListHotels" runat="server" AutoGenerateColumns="False" Style="width: 100%;"
                    AllowPaging="True" PageSize="10" EnableModelValidation="True" OnRowUpdating="gridListHotels_RowUpdating"
                    OnRowCommand="gridListHotels_RowCommand" OnRowCancelingEdit="gridListHotels_RowCancelingEdit"
                    OnRowDataBound="gridListHotels_RowDataBound" CssClass="DataGrid" GridLines="None"
                    OnRowEditing="gridListHotels_RowEditing" OnRowDeleting="gridListHotels_RowDeleting">
                    <Columns>
                        <asp:BoundField DataField="HotelCode" HeaderText="HotelCode" SortExpression="HotelCode"
                            Visible="false">
                            <HeaderStyle CssClass="dgHeader" />
                            <ItemStyle BackColor="#DFEBFF" />
                        </asp:BoundField>
                        <asp:BoundField DataField="HBName" HeaderText="HBName" SortExpression="HBName" Visible="false">
                            <HeaderStyle CssClass="dgHeader" />
                            <ItemStyle BackColor="#DFEBFF" />
                        </asp:BoundField>
                        <asp:BoundField DataField="HBCity" HeaderText="HBCity" SortExpression="HBCity" Visible="false">
                            <HeaderStyle CssClass="dgHeader" />
                            <ItemStyle BackColor="#DFEBFF" />
                        </asp:BoundField>
                        <asp:BoundField DataField="HBCountry" HeaderText="HBCountry" SortExpression="HBCountry"
                            Visible="false">
                            <HeaderStyle CssClass="dgHeader" />
                            <ItemStyle BackColor="#DFEBFF" />
                        </asp:BoundField>
                        <asp:ButtonField Text="Relacionarlos" HeaderText="Relacionarlos" Visible="false"
                            CommandName="Rel">
                            <HeaderStyle CssClass="dgHeader" />
                        </asp:ButtonField>
                        <asp:BoundField DataField="idHotel" HeaderText="idHotel" SortExpression="idHotel"
                            ReadOnly="True">
                            <HeaderStyle CssClass="dgHeader" />
                        </asp:BoundField>
                        <asp:BoundField DataField="UVName" HeaderText="UVName" SortExpression="UVName" ReadOnly="True">
                            <HeaderStyle CssClass="dgHeader" />
                        </asp:BoundField>
                        <asp:BoundField DataField="UVCity" HeaderText="UVCity" SortExpression="UVCity" ReadOnly="True">
                            <HeaderStyle CssClass="dgHeader" />
                        </asp:BoundField>
                        <asp:BoundField DataField="UVCountry" HeaderText="UVCountry" SortExpression="UVCountry"
                            ReadOnly="True">
                            <HeaderStyle CssClass="dgHeader" />
                        </asp:BoundField>
                        <asp:HyperLinkField DataNavigateUrlFields="idHotel" DataNavigateUrlFormatString="HotelBeds_ModifyHotel.aspx?id={0}"
                            Visible="false" Text="Modificar">
                            <HeaderStyle CssClass="dgHeader" />
                        </asp:HyperLinkField>
                        <%--<asp:BoundField DataField="HotelCode" HeaderText="HotelCode" ItemStyle-Width="10%"
                            SortExpression="HotelCode" ReadOnly="false">
                            <HeaderStyle CssClass="dgHeader" />
                        </asp:BoundField>--%>
                        <asp:TemplateField ItemStyle-Width="10%" HeaderText="Relación HB">
                            <HeaderStyle CssClass="dgHeader" />
                            <ItemTemplate>
                                <asp:Label ID="lblRelacion" runat="server" Text='<%# DataBinder.Eval(Container.Dataitem,"HotelCode") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtRelacion" runat="server" CssClass="" Style="display: block;"
                                    Columns="15" MaxLength="15" Width="60px" Text='<%# DataBinder.Eval(Container.Dataitem,"HotelCode") %>'></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action" ItemStyle-Width="10%">
                            <HeaderStyle CssClass="dgHeader" />
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEdit" Text='<%#PortalCulture.GetString("00763") %>' runat="server" CommandName="Edit" />&nbsp;&nbsp;
                                <asp:LinkButton ID="btnDelete" Text='<%#PortalCulture.GetString("00451") %>' runat="server" CommandName="Delete" />
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
        <div class="tituloSeccion" style="margin-top: 20px">
            <asp:Label runat="server" ID="lblTitleHB" CssClass="tituloSeccion">Listado de hoteles de Hotel Beds</asp:Label>
        </div>
        <div class="clear">
        </div>
        <div style="margin-top: 10px">        
           <asp:Label runat="server" ID="lblSeachBeds" CssClass="clsLabel">Búsqueda de Hoteles:</asp:Label>
            <asp:TextBox ID="txtSearchBeds" runat="server" Width="150px" Height="20" CssClass="textbox" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
            <asp:Button ID="btnBuscarBeds" runat="server" Text="Buscar" OnClick="btnBuscarBeds_Click"
                CssClass="button" />
        </div>
        <div>
            <asp:Label runat="server" ID="lblError" CssClass="Validators" Visible="false">*</asp:Label>
        </div>
        <div class="tabla" style="">
            <center class="divSearch" style="">
                <asp:GridView ID="gvBeds" runat="server" AutoGenerateColumns="False" Style="width: 100%;"
                    AllowPaging="true" PageSize="10" EnableModelValidation="True" CssClass="DataGrid"
                    GridLines="None">
                    <Columns>
                        <asp:BoundField DataField="HotelCode" HeaderText="HotelCode" SortExpression="HotelCode"
                            Visible="true">
                            <HeaderStyle CssClass="dgHeader" />
                        </asp:BoundField>
                        <asp:BoundField DataField="HBName" HeaderText="HBName" SortExpression="HBName" Visible="true">
                            <HeaderStyle CssClass="dgHeader" />
                        </asp:BoundField>
                        <asp:BoundField DataField="HBCity" HeaderText="HBCity" SortExpression="HBCity" Visible="true">
                            <HeaderStyle CssClass="dgHeader" />
                        </asp:BoundField>
                        <asp:BoundField DataField="HBCountry" HeaderText="HBCountry" SortExpression="HBCountry"
                            Visible="true">
                            <HeaderStyle CssClass="dgHeader" />
                        </asp:BoundField>
                        <asp:ButtonField Text="Relacionarlos" HeaderText="Relacionarlos" Visible="false"
                            CommandName="Rel">
                            <HeaderStyle CssClass="dgHeader" />
                        </asp:ButtonField>
                        <asp:BoundField DataField="idHotel" HeaderText="idHotel" Visible="true" SortExpression="idHotel">
                            <HeaderStyle CssClass="dgHeader" />
                        </asp:BoundField>
                        <asp:BoundField DataField="UVName" HeaderText="UVName" SortExpression="UVName" Visible="false">
                            <HeaderStyle CssClass="dgHeader" />
                        </asp:BoundField>
                        <asp:BoundField DataField="UVCity" HeaderText="UVCity" SortExpression="UVCity" Visible="false">
                            <HeaderStyle CssClass="dgHeader" />
                        </asp:BoundField>
                        <asp:BoundField DataField="UVCountry" HeaderText="UVCountry" Visible="false" SortExpression="UVCountry">
                            <HeaderStyle CssClass="dgHeader" />
                        </asp:BoundField>
                        <asp:HyperLinkField DataNavigateUrlFields="idHotel" DataNavigateUrlFormatString="HotelBeds_ModifyHotel.aspx?id={0}"
                            Text="Modificar" Visible="false">
                            <HeaderStyle CssClass="dgHeader" />
                        </asp:HyperLinkField>
                    </Columns>
                </asp:GridView>
            </center>
        </div>
    </div>
    </form>

    <script type="text/javascript">
        
    </script>

</body>
</html>
