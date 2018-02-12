<%@ Register TagPrefix="uc1" TagName="CtrMenu" Src="../Portal/Modules/CtrMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../Portal/Modules/ctrlHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlImages" Src="../Portal/Modules/Contenido/ctrlImages.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Imagenes.aspx.vb" Inherits="RateManager.Imagenes1" ValidateRequest="False"%>
<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../Portal/Modules/ctrlFooter.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Imagenes</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
		<div class="clear">
            <div class="mDiv">
            </div>
            <div class="title">
                <asp:Label ID="lbltitle" runat="server" EnableViewState="False" Text="Imagenes" CssClass="tituloSeccion"></asp:Label>
            </div>
         </div>
			<TABLE id="bookingcontainer" style="Z-INDEX: 101; "
				cellSpacing="0" cellPadding="2" width="650" border="0">
				<TR>
					<TD>
						<TABLE id="Table2" cellspacing=0 cellPadding="0" width="100%" border="0">
							<TR>
								<TD>
									<TABLE id="Table1" cellspacing=0 cellPadding="0" width="100%" border="0">
										
										<TR>
											<TD>
												<uc1:ctrlImages id="CtrlImages1" runat="server"></uc1:ctrlImages></TD>
										</TR>
										<TR>
											<TD></TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
							<TR>
								<TD></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
