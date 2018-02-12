<%@ Import NameSpace = "RateManager" %>
<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../Portal/Modules/ctrlFooter.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../Portal/Modules/ctrlHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrMenu" Src="../Portal/Modules/CtrMenu.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="FaresRoom.aspx.vb" Inherits="RateManager.FaresRoom"%>
<%@ Register TagPrefix="uc1" TagName="ctrRateAplication" Src="../Modulos/ctrRateAplication.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>FaresRoom</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
		</LINK>
	</HEAD>
	<body MS_POSITIONING="FlowLayout" bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0">
		<form id="Form1" method="post" runat="server">
		 <iframe id="gToday:normal:agenda.js" style="Z-INDEX: 999; LEFT: -500px; POSITION: absolute; TOP: -500px" 
            name="gToday:normal:agenda.js"
            src='<%=GeRequestApplicationPath(string.concat("/Calendar/",portalculture.getculture().Name.Substring(0, 2).ToLower(),"/ipopeng2.htm" )) %>'
            frameborder="0" width="174" scrolling="no" height="189">
         </iframe>
			<TABLE id="bookingcontainer" cellSpacing="0" cellPadding="2" width="650" border="0">
				<tr>
					<td>
						<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" align="center" border="0">
							<TR>
								<TD class="Titulo" colSpan="2"><asp:label id="lblTitle" runat="server" EnableViewState="False">Rates</asp:label></TD>
							</TR>
							<TR>
								<TD colSpan="2"><asp:datagrid id="dgRates" runat="server" AutoGenerateColumns="False" Width="100%">
										<AlternatingItemStyle Cssclass="dgAlternate"></AlternatingItemStyle>
										<ItemStyle Cssclass="dgItem"></ItemStyle>
										<HeaderStyle Cssclass="dgHeader"></HeaderStyle>
										<Columns>
											<asp:BoundColumn DataField="FechaInicia"></asp:BoundColumn>
											<asp:BoundColumn DataField="FechaFinaliza"></asp:BoundColumn>
											<asp:BoundColumn DataField="Precio">
												<ItemStyle HorizontalAlign="Right"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="PrecioExtraAdulto">
												<ItemStyle HorizontalAlign="Right"></ItemStyle>
											</asp:BoundColumn>
											<asp:BoundColumn DataField="PrecioExtraNinio">
												<ItemStyle HorizontalAlign="Right"></ItemStyle>
											</asp:BoundColumn>
										</Columns>
									</asp:datagrid></TD>
							</TR>
							<TR>
								<TD align="center" colSpan="2"><uc1:ctrrateaplication id="CtrRateAplication1" runat="server"></uc1:ctrrateaplication></TD>
							</TR>
							<TR>
								<TD align="center" colSpan="2"><asp:label id="lblError" runat="server" CssClass="Validators">Error</asp:label></TD>
							</TR>
							<TR>
								<TD align="center" colSpan="2">
									<asp:Button id="btnSave" runat="server" Text="Save" CssClass="button" EnableViewState="False"></asp:Button>
                                    <asp:Button ID="cmdCancel" runat="server" Text="Button" /></TD>
							</TR>
						</TABLE>
					</td>
				</tr>
			</TABLE>
		</form>
	</body>
</HTML>
