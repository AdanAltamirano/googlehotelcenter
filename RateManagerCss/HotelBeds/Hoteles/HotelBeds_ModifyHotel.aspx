<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="HotelBeds_ModifyHotel.aspx.vb" Inherits="RateManager.HotelBeds_ModifyHotel" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
    <link href="../../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
    </head>
<body>
    <form id="form1" runat="server">
        <div class="divTopCiudad">
        <asp:Label ID="lblTopCiudad" runat="server"></asp:Label>
    </div>
    <div class="divCiudad">
        <div style="float: left; padding: 1px 5px 1px 1px;">
            <asp:Literal ID="litCity" runat="server"></asp:Literal>
            <br />
        </div>
        <div style="width: 80%;">
            <center>
                <asp:Label ID="lblMsgRel" runat="server"></asp:Label></center>
            <asp:GridView ID="gridHotelRel" runat="server" AutoGenerateColumns="False" OnRowCommand="gridHotelRel_RowCommand"
                Width="100%" CssClass="DataGrid" GridLines="None">
                <Columns>
                    <asp:BoundField DataField="idHotelsFix" HeaderText="idHotelsFix" 
                        SortExpression="idHotelsFix" >
                    <HeaderStyle CssClass="dgHeader" />
                    </asp:BoundField>
                    <asp:BoundField DataField="HotelCode" HeaderText="HotelCode" 
                        SortExpression="HotelCode" >
                    <HeaderStyle CssClass="dgHeader" />
                    </asp:BoundField>
                    <asp:BoundField DataField="HBName" HeaderText="HBName" SortExpression="HBName" >
                    <HeaderStyle CssClass="dgHeader" />
                    </asp:BoundField>
                    <asp:BoundField DataField="HBCity" HeaderText="HBCity" SortExpression="HBCity" >
                    <HeaderStyle CssClass="dgHeader" />
                    </asp:BoundField>
                    <asp:BoundField DataField="HBCountry" HeaderText="HBCountry" 
                        SortExpression="HBCountry" >
                    <HeaderStyle CssClass="dgHeader" />
                    </asp:BoundField>
                    <asp:ButtonField Text="Remover" HeaderText="Remover" CommandName="Del" >
                    <HeaderStyle CssClass="dgHeader" />
                    </asp:ButtonField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <br />
    <br />
    Busqueda de destinos HotelBeds:
    <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
    <asp:Button ID="btnSearch" runat="server" Text="Buscar" OnClick="btnSearch_Click" CssClass="button" />
    <div class="tabla">
        <div class="divTopCiudad">
            <asp:Label ID="lblMSG_Result" runat="server"></asp:Label>
        </div>
        <div class="divSearch">
            <div style="clear: both;">
                <asp:GridView ID="gridHotelsRes" runat="server" AutoGenerateColumns="False" Style="width: 100%;"
                    OnRowCommand="gridHotelRes_RowCommand" OnRowDataBound="gridHotelsRes_RowDataBound" CssClass="DataGrid" GridLines="None">
                    <Columns>
                        <asp:BoundField DataField="HotelCode" HeaderText="HotelCode" 
                            SortExpression="HotelCode" >
                        <HeaderStyle CssClass="dgHeader" />
                        </asp:BoundField>
                        <asp:BoundField DataField="HBName" HeaderText="HBName" SortExpression="HBName" >
                        <HeaderStyle CssClass="dgHeader" />
                        </asp:BoundField>
                        <asp:BoundField DataField="HBCity" HeaderText="HBCity" SortExpression="HBCity" >
                        <HeaderStyle CssClass="dgHeader" />
                        </asp:BoundField>
                        <asp:BoundField DataField="HBCountry" HeaderText="HBCountry" 
                            SortExpression="HBCountry" >
                        <HeaderStyle CssClass="dgHeader" />
                        </asp:BoundField>
                        <asp:BoundField DataField="idHotel" HeaderText="idHotel" 
                            SortExpression="idHotel" >
                        <HeaderStyle CssClass="dgHeader" />
                        </asp:BoundField>
                        <asp:ButtonField Text="Agregar" HeaderText="Agregar" CommandName="Add" >
                        <HeaderStyle CssClass="dgHeader" />
                        </asp:ButtonField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
    <br />
    <br />
    <a href="HotelBeds.aspx">Atras</a>
    </form>
</body>
</html>
