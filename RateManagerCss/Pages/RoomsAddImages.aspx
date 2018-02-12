<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RoomsAddImages.aspx.vb" Inherits="RateManager.RoomsAddImages"%>
<%@ Register TagPrefix="uc1" TagName="CtrMenu" Src="../Portal/Modules/CtrMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../Portal/Modules/ctrlHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../Portal/Modules/ctrlFooter.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Photo" Src="../Portal/Modules/Contenido/PhotoRooms.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>PhotoInformation</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="5" rightMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
		<div class="clear">
            <div class="mDiv">
            </div>
            <div class="title">
                <asp:Label ID="lbltitle" runat="server" EnableViewState="False" Text="Fotos Adicionales" CssClass="tituloSeccion"></asp:Label>
            </div>
         </div>
			<TABLE id="bookingcontainer" cellSpacing="0" cellPadding="2" width="650" border="0">
				<tr>
					<td>
						<TABLE id="Table1" cellSpacing= "0" cellPadding="0" width="100%" border="0">
							<tr>
								<td>
									<TABLE  cellspacing= "0" cellPadding="0" width="100%" border="0">
										
										<tr>
											<td></td>
										</tr>
										<TR>
											<TD>
												<uc1:Photo id="Photo1" runat="server"></uc1:Photo></TD>
										</TR>
									</TABLE>
								</td>
							</tr>
							<tr>
								<td>
								</td>
							</tr>
						</TABLE>
					</td>
				</tr>
			</TABLE>
		</form>
	</body>
</HTML>
