<%@ Page Language="vb" AutoEventWireup="false" Codebehind="SubscribersEmailList.aspx.vb" Inherits="RateManager.SubscribersEmailList"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>SubscribersEmailList</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body bottomMargin="0" leftMargin="0" rightMargin="0">
		<form id="Form1" method="post" runat="server">
		 <div class="clear">
            <div class="mDiv">
            </div>
            <div class="title">
                <asp:Label ID="lbltitle" runat="server" EnableViewState="False" Text="Listado de correos electrónicos de subscriptores" CssClass="tituloSeccion"></asp:Label>
            </div>
         </div>				
			<TABLE id="bookingcontainer" cellSpacing="0" cellPadding="2" width="471" align="center"
				border="0">				
				<TR>
					<TD align="right" width="20%"><asp:button id="btnExport" style="CURSOR: pointer" runat="server" CssClass="button" Text="Exportar a excel"></asp:button></TD>
					<TD></TD>
				</TR>
				<TR>
					<TD align="center" colSpan="2"><asp:datagrid id="dgEmailList" runat="server" ShowFooter="True" AutoGenerateColumns="False" CssClass="DataGrid" >
							<SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
							<AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
							<ItemStyle CssClass="dgItem"></ItemStyle>
							<HeaderStyle HorizontalAlign="left" CssClass="dgHeader" VerticalAlign="Middle"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="Email" HeaderText="Correo Electronico"></asp:BoundColumn>
								<asp:BoundColumn DataField="Fecha" HeaderText="Fecha de Registro"></asp:BoundColumn>
							</Columns>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD colSpan="2"></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
