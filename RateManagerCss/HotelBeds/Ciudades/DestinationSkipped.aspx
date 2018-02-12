<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DestinationSkipped.aspx.vb" Inherits="RateManager.DestinationSkipped" %>
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
            <span id="lblTitulo" class="tituloSeccion">Ciudades omitidas</span>            
        </div>
        <div class="clear"></div>
         Buscar: 
            <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
            <asp:Button ID="btnSearch" runat="server" Text="Buscar" CssClass="button"
            onclick="btnSearch_Click" />
            <br /><br />
        <asp:GridView ID="gridList" runat="server" AutoGenerateColumns="False" 
            onrowcommand="gridList_RowCommand" 
            onpageindexchanging="gridList_PageIndexChanging" AllowPaging="True" 
            PageSize="25" CssClass="DataGrid" GridLines="None">
            <Columns>
                <asp:BoundField DataField="IdCiudad" HeaderText="IdCiudad" 
                    InsertVisible="False" ReadOnly="True" SortExpression="IdCiudad" >
                <HeaderStyle CssClass="dgHeader" />
                </asp:BoundField>
                <asp:BoundField DataField="Nombre" HeaderText="Nombre" 
                    SortExpression="Nombre" >
                <HeaderStyle CssClass="dgHeader" />
                </asp:BoundField>
                <asp:BoundField DataField="codigo" HeaderText="codigo" 
                    SortExpression="codigo" >
                <HeaderStyle CssClass="dgHeader" />
                </asp:BoundField>
                <asp:BoundField DataField="NumRel" HeaderText="NumRel" 
                    SortExpression="NumRel" >
                <HeaderStyle CssClass="dgHeader" />
                </asp:BoundField>
                <asp:ButtonField Text="Restaurar" HeaderText="Restaurar" CommandName="Res"  >
                <HeaderStyle CssClass="dgHeader" />
                </asp:ButtonField>
            </Columns>
        </asp:GridView>
    </div>
    </form>
</body>
</html>    

