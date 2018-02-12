<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="HotelFix.aspx.vb" Inherits="RateManager.HotelFix" %>
<%@ Import Namespace="RateManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
    <link href="../../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
    </head>
<body>
    <form id="form1" runat="server">
        <div>
        <div class="tituloSeccion">
            <asp:Label ID="lblTitulo1" runat="server" EnableViewState="false">Hoteles Agregados</asp:Label>
            
        </div>
        <div class="clear"></div>
        <asp:GridView ID="gridList" runat="server" AutoGenerateColumns="False" 
                EnableModelValidation="True" AllowPaging="True" 
            onpageindexchanging="gridList_PageIndexChanging" PageSize="25" CssClass="DataGrid" GridLines="None">
        <Columns>
            <asp:BoundField DataField="HotelCode" HeaderText="HotelCode" 
                            SortExpression="HotelCode" >
                        <HeaderStyle CssClass="dgHeader" />
                        <ItemStyle BackColor="#DFEBFF" />
                        </asp:BoundField>
                        <asp:BoundField DataField="HBName" HeaderText="HBName" SortExpression="HBName" >
                        <HeaderStyle CssClass="dgHeader" />
                        <ItemStyle BackColor="#DFEBFF" />
                        </asp:BoundField>
                        <asp:BoundField DataField="HBCity" HeaderText="HBCity" SortExpression="HBCity" >
                        <HeaderStyle CssClass="dgHeader" />
                        <ItemStyle BackColor="#DFEBFF" />
                        </asp:BoundField>
                        <asp:BoundField DataField="HBCountry" HeaderText="HBCountry" 
                            SortExpression="HBCountry" >
                        <HeaderStyle CssClass="dgHeader" />
                        <ItemStyle BackColor="#DFEBFF" />
                        </asp:BoundField>
                        <asp:BoundField DataField="idHotel" HeaderText="idHotel" 
                SortExpression="idHotel" >
                        <HeaderStyle CssClass="dgHeader" />
            </asp:BoundField>
                        <asp:BoundField DataField="UVName" HeaderText="UVName" 
                SortExpression="UVName" >
                        <HeaderStyle CssClass="dgHeader" />
            </asp:BoundField>
                        <asp:BoundField DataField="UVCity" HeaderText="UVCity" 
                SortExpression="UVCity" >
                        <HeaderStyle CssClass="dgHeader" />
            </asp:BoundField>
                        <asp:BoundField DataField="UVCountry" HeaderText="UVCountry" 
                SortExpression="UVCountry" >
                        <HeaderStyle CssClass="dgHeader" />
            </asp:BoundField>
                        <asp:HyperLinkField DataNavigateUrlFields="idHotel" Visible="false" DataNavigateUrlFormatString="HotelBeds_ModifyHotel.aspx?id={0}"
                        Text="Modificar" >
            <HeaderStyle CssClass="dgHeader" />
            </asp:HyperLinkField>
        </Columns>
    </asp:GridView>
    </div>
     <br /><br />
    <a href="HotelBeds.aspx">Atras</a>
    </form>
</body>
</html>
