<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AmenitiesInformationCCT.aspx.vb" ValidateRequest="false" Inherits="RateManager.AmenitiesInformationCCT" %>
<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../Portal/Modules/ctrlFooter.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../Portal/Modules/ctrlHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrMenu" Src="../Portal/Modules/CtrMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="InfoAmenidadesCCT" Src="../Portal/Modules/Contenido/InfoAmenidadesCCT.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>InfoPropiedad</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="5" rightMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
        <div id="tituloSeccion">
            <img src="../Includes/imagenes/icono-big-plus.png" width="25" height="25" />
            <asp:Label ID="lbltitle" class="tituloSeccion" runat="server" EnableViewState="False">Cambiar logo</asp:Label>
        </div>
			<TABLE id="bookingcontainer" cellSpacing="0" cellPadding="2" width="650" border="0">
				<tr>
					<td>
						<TABLE cellspacing=0 cellPadding="0" width="100%" border="0" id="Table2">
							<tr>
								<td><TABLE id="Table1" cellspacing=0 cellPadding="0" width="100%" border="0">										
										<tr>
											<td>
											    <uc1:InfoAmenidadesCCT ID="InfoAmenidadesCCT1" runat="server" />
											</td>
										</tr>
										<TR>
											<TD>
											</TD>
										</TR>
									</TABLE>
								</td>
							</tr>
							<tr>
								<td></td>
							</tr>
						</TABLE>
					</td>
				</tr>
			</TABLE>
		</form>
	</body>
</HTML>