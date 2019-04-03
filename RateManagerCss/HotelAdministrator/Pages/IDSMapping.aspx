<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="IDSMapping.aspx.vb" Inherits="RateManager.IDSMapping"%>
<%@ Import Namespace="RateManager" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../../Includes/estilos.css" rel="stylesheet" />
</head>

<body>
    <form id="form1" runat="server">
        <div>
            <asp:GridView Width="700px" ShowFooter="true" ID="gvRatesPlan" runat="server" AutoGenerateColumns="false"
                CssClass="DataGrid" GridLines="None" AllowPaging="true" PageSize="10" OnRowCancelingEdit="gvRatesPlan_RowCancelEditing"
                OnRowUpdating="gvRatesPlan_RowUpdating" OnRowEditing="gvRatesPlan_RowEditing">
                <Columns>
                    <asp:BoundField DataField="ChannelIdRateCRS" HeaderText="ID" InsertVisible="false" Visible="true"
                        ReadOnly="true" SortExpression="">
                        <HeaderStyle CssClass="dgHeader" />
                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Fuente">
                        <ItemStyle Width="5%" />
                        <HeaderStyle CssClass="dgHeader" />                        
                        <ItemTemplate>
                            <asp:Label ID="lblSource" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Source")%>'
                                Style="margin-left: 10px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Código de la fuente">
                        <ItemStyle Width="10%" HorizontalAlign="Center" />
                        <HeaderStyle CssClass="dgHeader" />
                        <ItemTemplate>
                            <asp:Label ID="lblRateSourceId" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RateSourceId")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Código IP">
                        <ItemStyle Width="10%" HorizontalAlign="Center" />
                        <HeaderStyle CssClass="dgHeader" />
                        <ItemTemplate>
                            <asp:Label ID="lblIPCode" runat="server" Text='Sin Código'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtIPCode" runat="server" CssClass="" Style="display: block;" Columns="50"
                                MaxLength="50" Width="80px" Text=''></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="" ItemStyle-Width="20%">
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEliminar" CausesValidation="false" Text='<%#PortalCulture.GetString("00103") %>'
                                runat="server" CommandName="Delete" Visible="false" />
                            <asp:LinkButton ID="btnEdit" CausesValidation="false" style="display:none;" Text='<%#PortalCulture.GetString("00763") %>'
                                runat="server" CommandName="Edit" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton ID="btnUpdate" CausesValidation="false" Text='<%#PortalCulture.GetString("M000142") %>'
                                runat="server" CommandName="Update" ValidationGroup="v1" />
                            <asp:LinkButton ID="btnCancel" CausesValidation="false" Text='<%#PortalCulture.GetString("00413") %>'
                                runat="server" CommandName="Cancel" />
                        </EditItemTemplate>
                        <ControlStyle ForeColor="#6600FF" />
                        <ItemStyle Font-Size="Medium" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <div>
            <asp:GridView Width="700px" ShowFooter="true" ID="gvRooms" runat="server" AutoGenerateColumns="false"
                CssClass="DataGrid" GridLines="None" AllowPaging="true" PageSize="10" OnRowCancelingEdit="gvRooms_RowCancelEditing"
                OnRowUpdating="gvRooms_RowUpdating" OnRowEditing="gvRooms_RowEditing">
                <Columns>
                    <asp:BoundField DataField="ChannelIdRateCRS" HeaderText="ID" InsertVisible="false" Visible="true"
                        ReadOnly="true" SortExpression="">
                        <HeaderStyle CssClass="dgHeader" />
                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Fuente">
                        <ItemStyle Width="5%" />
                        <HeaderStyle CssClass="dgHeader" />                        
                        <ItemTemplate>
                            <asp:Label ID="lblSource" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Source")%>'
                                Style="margin-left: 10px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Código de la fuente">
                        <ItemStyle Width="10%" HorizontalAlign="Center" />
                        <HeaderStyle CssClass="dgHeader" />
                        <ItemTemplate>
                            <asp:Label ID="lblRateSourceId" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RoomSourceId")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Código IP">
                        <ItemStyle Width="10%" HorizontalAlign="Center" />
                        <HeaderStyle CssClass="dgHeader" />
                        <ItemTemplate>
                            <asp:Label ID="lblIPCode" runat="server" Text='Sin Código'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtIPCode" runat="server" CssClass="" Style="display: block;" Columns="50"
                                MaxLength="50" Width="80px" Text=''></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="" ItemStyle-Width="20%">
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEliminar" CausesValidation="false" Text='<%#PortalCulture.GetString("00103") %>'
                                runat="server" CommandName="Delete" Visible="false" />
                            <asp:LinkButton ID="btnEdit" CausesValidation="false" style="display:none;" Text='<%#PortalCulture.GetString("00763") %>'
                                runat="server" CommandName="Edit" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton ID="btnUpdate" CausesValidation="false" Text='<%#PortalCulture.GetString("M000142") %>'
                                runat="server" CommandName="Update" ValidationGroup="v1" />
                            <asp:LinkButton ID="btnCancel" CausesValidation="false" Text='<%#PortalCulture.GetString("00413") %>'
                                runat="server" CommandName="Cancel" />
                        </EditItemTemplate>
                        <ControlStyle ForeColor="#6600FF" />
                        <ItemStyle Font-Size="Medium" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <div>
            <asp:Label CssClass="validators" ID="lblError" runat="server" Visible="false">Error</asp:Label>
        </div>
    </form>
</body>
</html>
