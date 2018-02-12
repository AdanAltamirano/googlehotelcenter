<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="HotelBeds_ModifyDestination.aspx.vb" Inherits="RateManager.HotelBeds_ModifyDestination"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
    <link href="../../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
    </head>
<body>
    <form id="form1" runat="server">
        <div>
        <div class="divTopCiudad">
        <asp:Label ID="lblTopCiudad" runat="server"></asp:Label>
    </div>
    <div class="divCiudad">
        <div style="float: left; padding: 1px 5px 1px 1px;">
            <asp:Literal ID="litCity" runat="server"></asp:Literal>
            <br />
            <br />
            <asp:Button ID="btnAddToSkipped" runat="server" Text="Omitir ciudad" OnClick="btnAddSkipped_Click"
                UseSubmitBehavior="False" CssClass="button" />
        </div>
        <div style="width: 80%;">
            <center>
                <asp:Label ID="lblMsgRel" runat="server"></asp:Label></center>
                <asp:Button ID="btnDel" runat="server" Text="Borrar destinos seleccionados" OnClick="btnDel_Click"
                    UseSubmitBehavior="False" CssClass="float-right button button" /><br /><br />
            <asp:GridView ID="gridDestRel" runat="server" AutoGenerateColumns="False" OnRowCommand="gridDestRel_RowCommand"
                Width="100%" CssClass="DataGrid" GridLines="None">
                <Columns>
                        <asp:BoundField DataField="idDestination" HeaderText="idDestination" 
                            SortExpression="idDestination" >
                        <HeaderStyle CssClass="dgHeader" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DestinationCode" HeaderText="DestinationCode" 
                            SortExpression="DestinationCode" >
                        <HeaderStyle CssClass="dgHeader" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DestinationName" HeaderText="DestinationName" 
                            SortExpression="DestinationName" >
                        <HeaderStyle CssClass="dgHeader" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ZoneName" HeaderText="ZoneName" 
                            SortExpression="ZoneName" >
                        <HeaderStyle CssClass="dgHeader" />
                        </asp:BoundField>
                        <asp:BoundField DataField="idCiudad" HeaderText="idCiudad" 
                            SortExpression="idCiudad" >
                        <HeaderStyle CssClass="dgHeader" />
                        </asp:BoundField>
                        <asp:BoundField DataField="codigo" HeaderText="codigo" SortExpression="codigo" >
                        <HeaderStyle CssClass="dgHeader" />
                        </asp:BoundField>
                        <asp:BoundField DataField="idDestination" HeaderText="idDestination" 
                            SortExpression="idDestination" >
                        <HeaderStyle CssClass="dgHeader" />
                        </asp:BoundField>
                        <asp:ButtonField Text="Remover" HeaderText="Remover" CommandName="Del" >
                        <HeaderStyle CssClass="dgHeader" />
                        </asp:ButtonField>
                        <asp:TemplateField HeaderText="Seleccionar">
                            <HeaderTemplate>
                                Seleccionar
                                <asp:CheckBox ID="chkSelectA" runat="server"  OnCheckedChanged="chkSelectA_CheckedChanged"
                                    AutoPostBack="true" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBox2" runat="server" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="dgHeader" />
                        </asp:TemplateField>
                </Columns>
            </asp:GridView><br /><br />
            <asp:Button ID="btnDel2" runat="server" Text="Borrar destinos seleccionados" OnClick="btnDel_Click"
                    UseSubmitBehavior="False" CssClass="button float-right" />
        </div>
    </div>
    <br />
    <br />
    <br />
    Busqueda de destinos HotelBeds:
    <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
    <asp:Button ID="btnSearch" runat="server" Text="Buscar" OnClick="btnSearch_Click" CssClass="button" />
    <asp:Button ID="btnAddTop" runat="server" Text="Agregar destinos seleccionados" OnClick="btnAdd_Click"
        UseSubmitBehavior="False" CssClass="button float-right" />
    <div class="tabla">
        <div class="divTopCiudad">
            <asp:Label ID="lblMSG_Result" runat="server"></asp:Label>
        </div>
        <div class="divSearch">
            <div style="clear: both;">
                <asp:GridView ID="gridDestRes" runat="server" AutoGenerateColumns="False" OnRowCommand="gridDestRes_RowCommand"
                    OnRowDataBound="gridDestRes_RowDataBound" Style="width: 100%;" CssClass="DataGrid" GridLines="None">
                    <Columns>
                       
                        <asp:BoundField DataField="DestinationCode" HeaderText="DestinationCode" 
                            SortExpression="DestinationCode" >
                        <HeaderStyle CssClass="dgHeader" />
                        </asp:BoundField>
                        <asp:BoundField DataField="CountryCode" HeaderText="CountryCode" 
                            SortExpression="CountryCode" >
                        <HeaderStyle CssClass="dgHeader" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DestinationName" HeaderText="DestinationName" 
                            SortExpression="DestinationName" >
                        <HeaderStyle CssClass="dgHeader" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ZoneCode" HeaderText="ZoneCode" 
                            SortExpression="ZoneCode" >
                        <HeaderStyle CssClass="dgHeader" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ZoneName" HeaderText="ZoneName" 
                            SortExpression="ZoneName" >
                        <HeaderStyle CssClass="dgHeader" />
                        </asp:BoundField>
                        <asp:BoundField DataField="idCiudad" HeaderText="idCiudad" 
                            SortExpression="idCiudad" >
                        <HeaderStyle CssClass="dgHeader" />
                        </asp:BoundField>
                        <asp:BoundField DataField="codigo" HeaderText="codigo" SortExpression="codigo" >
                        <HeaderStyle CssClass="dgHeader" />
                        </asp:BoundField>
                        <asp:ButtonField Text="Agregar" HeaderText="Agregar" CommandName="Add" >
                        <HeaderStyle CssClass="dgHeader" />
                        </asp:ButtonField>
                        <asp:TemplateField HeaderText="Seleccionar">
                            <HeaderTemplate>
                                Seleccionar
                                <asp:CheckBox ID="chkSelectAll" runat="server" OnCheckedChanged="chkSelectAll_CheckedChanged"
                                    AutoPostBack="true" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBox1" runat="server" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="dgHeader" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
    <asp:Button ID="btnAddBottom" runat="server" Text="Agregar destinos seleccionados"
        OnClick="btnAdd_Click" CssClass="button float-right" />
    <br />
    <br />
    <a href="HotelBeds.aspx">Atras</a>
    </div>
    </form>
</body>
</html>
   <%-- <asp:Content ID="ctntPrincipal" ContentPlaceHolderID="ContainerPage" runat="server">
        
    </asp:Content>--%>

