<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ExportExcell.aspx.vb" Inherits="RateManager.ExportExcel"%>
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
<asp:BoundColumn DataField="TravelerName" HeaderText="Cliente"></asp:BoundColumn>
<asp:BoundColumn DataField="TravelerAddress" HeaderText="Guest Address"></asp:BoundColumn>
<asp:BoundColumn DataField="TravelerCity" HeaderText="City"></asp:BoundColumn>
<asp:BoundColumn DataField="TravelerState" HeaderText="State"></asp:BoundColumn>
<asp:BoundColumn DataField="TravelerZip" HeaderText="Zip"></asp:BoundColumn>
<asp:BoundColumn DataField="TravelerPhoneHome" HeaderText="Phone"></asp:BoundColumn>
<asp:BoundColumn DataField="cliemailcliente" HeaderText="Phone"></asp:BoundColumn>
<asp:BoundColumn HeaderText="Itinerario" DataField="NoReservacion" DataFormatString="'{0:D}'" ></asp:BoundColumn>
<asp:BoundColumn DataField="CheckIn" HeaderText="Llegada"></asp:BoundColumn>
<asp:BoundColumn DataField="NombreHabitacion" HeaderText="Tipo de habitaci&#243;n"></asp:BoundColumn>
<asp:BoundColumn DataField="IATA" HeaderText="IATA"></asp:BoundColumn>
<asp:BoundColumn DataField="Agency" HeaderText="Agency"></asp:BoundColumn>
<asp:BoundColumn DataField="earlyOut" HeaderText="Early CheckOut"></asp:BoundColumn>
<asp:BoundColumn DataField="noshow" HeaderText="No Show"></asp:BoundColumn>
<asp:BoundColumn DataField="PorcPor" HeaderText="Comision"></asp:BoundColumn>
</Columns>

<PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right" Position="Top" CssClass="dgPager" Mode="NumericPages">
</PagerStyle>
</asp:datagrid>

    </form>

  </body>
</HTML>
