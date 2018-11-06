<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Reports.aspx.vb" Inherits="RateManager.Reports1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    
    <asp:DataGrid runat="server" ID="dgReport" AutoGenerateColumns="false">
        <HeaderStyle Font-Size="22px" />
        <Columns>
            <asp:BoundColumn HeaderText="No. reservación IP" DataField="NoReservacion" />
            <asp:BoundColumn HeaderText="No. reservación F2GO" DataField="NoReservacionGalileo" />
            <asp:BoundColumn HeaderText="Status" DataField="Status" />
            <asp:BoundColumn HeaderText="Fecha de reservación" DataField="FechaReservacion" />
            <asp:BoundColumn HeaderText="Nombre del huesped" DataField="Nombre_cl" />
            <asp:BoundColumn HeaderText="Correo del huesped" DataField="Email_cl" />
            <asp:BoundColumn HeaderText="Hotel" DataField="HotelGNombre" />
            <asp:BoundColumn HeaderText="Fecha de llegada" DataField="CheckIn" />
            <asp:BoundColumn HeaderText="Fecha de salida" DataField="CheckOut" />
            <asp:BoundColumn HeaderText="Adultos" DataField="adultos" />
            <asp:BoundColumn HeaderText="Menores" DataField="ninios" />
            <asp:BoundColumn HeaderText="Código de promoción" DataField="AccessCode" />
            <asp:BoundColumn HeaderText="Forma de pago" DataField="paymentmethod" />
            <asp:BoundColumn HeaderText="Total" DataField="Total" />
            <asp:BoundColumn HeaderText="Moneda" DataField="TotalProviderCurrencyCode" />
        </Columns>
    </asp:DataGrid>
</body>
</html>
