<%@ Register TagPrefix="uc1" TagName="ctrHotelsUserChain" Src="../../../Modulos/ctrHotelsUserChain.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="PaymentsRegisteredByHotel.aspx.vb" Inherits="RateManager.PaymentsRegisteredByHotel" %>
<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../../../Portal/Modules/ctrlHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../../../Portal/Modules/ctrlFooter.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>PaymentsRegisteredByHotel</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../../../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="5" rightMargin="0">
		<form id="Form1" method="post" runat="server">
		<div class="mDiv">
            </div>
            <div class="title">
                <asp:Label ID="lblTitle" runat="server" EnableViewState="False" 
                    CssClass="tituloSeccion">Pagos registrados por el hotel</asp:Label>
            </div>
        </div>
			<TABLE id="bookingcontainer" cellSpacing="0" cellPadding="0" width="760" border="0" align="center">				
				<TR>
					<TD colSpan="3" height="20"></TD>
				</TR>
				<tr>
					<td align="center" colspan="3">
						<uc1:ctrHotelsUserChain id="CtrHotelsUserChain1" runat="server" Visible="False"></uc1:ctrHotelsUserChain></td>
				</tr>
				<TR>
					<TD align="left" colSpan="3">
						<TABLE id="Table2" cellSpacing="1" cellPadding="3" border="0">
							<TR>
								<TD align="right" width="20"></TD>
								<TD align="right"><asp:label id="lblShowBy" runat="server" CssClass="bookingNormalLabel" EnableViewState="False">Mostrar por:</asp:label></TD>
								<TD><asp:dropdownlist id="ddlShowBy" runat="server" AutoPostBack="True"></asp:dropdownlist></TD>
								<TD></TD>
							</TR>
							<TR>
								<TD height="15"></TD>
								<TD height="15"></TD>
								<TD height="15"></TD>
								<TD height="15"></TD>
							</TR>
							<TR>
								<TD height="15"></TD>
								<TD colSpan="3" height="15">
									<TABLE id="Table5" cellSpacing="1" cellPadding="2" width="100%" border="0">
										<TR id="trMonth" runat="server">
											<TD class="DataGridAlternatedItem" align="right"><asp:label id="lblMonth" runat="server" CssClass="clsLabel" EnableViewState="False">Mes:</asp:label></TD>
											<TD class="textBox"><asp:dropdownlist id="ddlMonth" runat="server"></asp:dropdownlist></TD>
											<TD width="15"></TD>
										</TR>
										<TR id="trYear" runat="server">
											<TD class="DataGridAlternatedItem" align="right"><asp:label id="lblYear" runat="server" CssClass="clsLabel" EnableViewState="False">Año:</asp:label></TD>
											<TD class="textBox"><asp:dropdownlist id="ddlYear" runat="server"></asp:dropdownlist></TD>
											<TD width="15"></TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
							<TR>
								<TD align="center"></TD>
								<TD align="center" colSpan="3"><asp:button id="btnShow" runat="server" Text="Mostrar" CssClass="button" EnableViewState="False"></asp:button></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<tr style="padding-top:4px; padding-bottom:4px;"><td colspan =3 align=center>
							        <asp:Label id="lblHelpFiltro" runat="server" CssClass="clsHelpLabel" EnableViewState="False"></asp:Label>
							</td></tr>
				<TR>
					<TD colSpan="3" height="20"></TD>
				</TR>
				<TR>
					<TD align="center" colSpan="3"><asp:label id="lblMsg" runat="server" CssClass="Validators" Visible="False">No Hay Pagos Registrados</asp:label></TD>
				</TR>
				<TR id="trGridPayments" runat="server">
					<TD width="15"></TD>
					<TD><asp:datagrid id="dgPayments" runat="server" CssClass="DataGrid" Width="100%" AutoGenerateColumns="False"
							EnableViewState="False">
							<AlternatingItemStyle CssClass="dgalternate"></AlternatingItemStyle>
							<ItemStyle CssClass="dgitem"></ItemStyle>
							<HeaderStyle CssClass="dgheader"></HeaderStyle>
							<Columns>
								<asp:TemplateColumn HeaderText="Banco">
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lblBankName" runat="server"></asp:Label><br>
										<asp:Label CssClass="clsLabelDataGrid" id="lblAccountNumber" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Fecha de Registro">
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lblRegistrationDate" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Tipo">
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<ItemTemplate>
										<P>
											<asp:Label id=lblTransactionNumber runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "TransactionReference") %>'>
											</asp:Label><BR>
											<asp:Label id="lblType" runat="server" CssClass="clsLabelDataGrid"></asp:Label><br>
											<asp:Label id="lblReferenceBank" runat="server"></asp:Label></P>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Fecha de Dep&#243;sito">
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lblPaymentDate" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Tipo de Cambio">
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lblMoneyExchange" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Importe Registrado">
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lblAmount" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Estado">
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lblStatus" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:datagrid></TD>
					<TD width="15"></TD>
				</TR>
				<TR>
					<TD colSpan="3" height="10"></TD>
				</TR>
				<TR>
					<TD align="right" colSpan="2"><asp:label id="lblTotalAccepted" runat="server" CssClass="bookingNormalLabel" Visible="False"
							EnableViewState="False">Total de Pagos Acreditados:</asp:label><asp:label id="lblTotal" runat="server" CssClass="clsHelpLabel" Visible="False" EnableViewState="False"></asp:label></TD>
					<TD width="10"></TD>
				</TR>
				<TR>
					<TD width="10" height="30"></TD>
					<TD class="clsLabel" align="left" height="30"></TD>
					<TD width="10" height="30"></TD>
				</TR>
				<TR>
					<TD width="10"></TD>
					<TD class="clsLabel" align="left">
						<TABLE id="Table4" cellSpacing="1" cellPadding="5" width="300" border="0">
							<TR>
								<TD width="15"></TD>
								<TD>
									<asp:hyperlink id="hplGoToTaskList" runat="server" CssClass="dgLink" EnableViewState="False">Ir a Lista de Tareas</asp:hyperlink></TD>
							</TR>
						</TABLE>
					</TD>
					<TD width="10"></TD>
				</TR>
				<TR>
					<TD width="10" height="15"></TD>
					<TD class="clsLabel" align="left" height="15"></TD>
					<TD width="10" height="15"></TD>
				</TR>
				<TR>
					<TD width="10"></TD>
					<TD class="clsLabel" align="right"><asp:literal id="ltlNote" runat="server" Text='<b>NOTA:</b> Los pagos en estado <i>"No Acreditado"</i> aún no han sido verificados con el banco.<BR> En el momento en que se verifiquen su estado cambiará a <i>"Acreditado"</i>'
							EnableViewState="False"></asp:literal></TD>
					<TD width="10"></TD>
				</TR>
				<TR>
					<TD width="10" height="10"></TD>
					<TD class="clsLabel" align="right" height="10"></TD>
					<TD width="10" height="10"></TD>
				</TR>
				<TR>
					<TD width="10"></TD>
					<TD class="clsLabel" align="right"></TD>
					<TD width="10"></TD>
				</TR>
				<TR>
					<TD colSpan="3" height="40"></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
