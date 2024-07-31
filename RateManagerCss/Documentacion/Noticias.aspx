<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Noticias.aspx.vb" Inherits="RateManager.Noticias"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Noticias</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body topMargin="5">
		<form id="Form1" method="post" runat="server">
			<table id="bookingcontainer" cellSpacing="0" cellPadding="5" width="100%" border="0">
				<tr>
					  <td>
                        <div id="tituloSeccion">
                            <img src="../Includes/imagenes/icono-big-plus.png" width="25" height="25" />
                            <span id="lblTitle" runat="server" class="tituloSeccion">Bienvenido</span>
                        </div>
                    </td>
				</tr>
                <tr>
                    <td style="height: 25px">
                        <asp:Label ID="lblOrderNews" runat="server">Ordenar por fecha:</asp:Label>
                        <asp:DropDownList
                            ID="ddlOrderNews" runat="server"  AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:DataGrid ID="GridNews" runat="server" PageSize="15" AllowPaging="True" AutoGenerateColumns="False"
                            Border="0" CellSpacing="1" ShowFooter="True" Width="100%" AllowSorting="True" class="DataGrid">
                            <FooterStyle HorizontalAlign="Right"></FooterStyle>
                            <SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
                            <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
                            <ItemStyle CssClass="dgItem"></ItemStyle>
                            <HeaderStyle CssClass="dgHeader"></HeaderStyle>
                            <Columns>
                                <asp:BoundColumn DataField="Description"></asp:BoundColumn>
                                <asp:HyperLinkColumn Target="_blank" DataNavigateUrlField="Link" DataTextField="Link"
                                    DataTextFormatString="(PDF)"></asp:HyperLinkColumn>
                            </Columns>
                            <PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
                                Position="Top" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
                        </asp:DataGrid>
                    </td>
                </tr>
			</table>
		</form>
	</body>
</HTML>
