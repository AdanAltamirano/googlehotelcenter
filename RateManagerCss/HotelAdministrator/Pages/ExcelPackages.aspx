<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ExcelPackages.aspx.vb" Inherits="RateManager.ExcelPackages" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
  <HEAD>
    <title>ExportExcel</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name=vs_defaultClientScript content="JavaScript">    
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
  </HEAD>
  <body MS_POSITIONING="GridLayout">

    <form id="Form1" method="post" runat="server">
<asp:datagrid id=Grid style="Z-INDEX: 101; LEFT: 8px; POSITION: absolute; TOP: 8px" runat="server" AutoGenerateColumns="False" Width="95%" PageSize="15" CssClass="DataGrid">
<FooterStyle HorizontalAlign="Right">
</FooterStyle>

<SelectedItemStyle CssClass="dgSelected">
</SelectedItemStyle>

<AlternatingItemStyle CssClass="dgAlternate">
</AlternatingItemStyle>

<ItemStyle CssClass="dgItem">
</ItemStyle>

<HeaderStyle CssClass="dgHeader">
</HeaderStyle>

<Columns>
<asp:BoundColumn DataField="packageid" HeaderText="ID Paquete"></asp:BoundColumn>
<asp:BoundColumn DataField="Itinerary" HeaderText="Itinerario"></asp:BoundColumn>
<asp:BoundColumn DataField="TravelerName" HeaderText="Viajero"></asp:BoundColumn>
<asp:BoundColumn DataField="FechaReservacion" HeaderText="Fecha de Reservación"></asp:BoundColumn>
<asp:BoundColumn DataField="checkin" HeaderText="Llegada"></asp:BoundColumn>
<asp:BoundColumn DataField="checkOut" HeaderText="Salida"></asp:BoundColumn>
<asp:BoundColumn DataField="ActivityReservationId" HeaderText="Activity" ></asp:BoundColumn>
<asp:BoundColumn DataField="AutobusReservationId" HeaderText="Autobus"></asp:BoundColumn>
<asp:BoundColumn DataField="Status" HeaderText="Status"></asp:BoundColumn>
<asp:BoundColumn DataField="PorcPor" HeaderText="Comisión"></asp:BoundColumn>
</Columns>

<PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right" Position="Top" CssClass="dgPager" Mode="NumericPages">
</PagerStyle>
</asp:datagrid>

    </form>

  </body>
</HTML>
