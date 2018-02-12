<%@ Page Language="vb" AutoEventWireup="false" Codebehind="FoodInformation.aspx.vb" Inherits="RateManager.FoodInformation" ValidateRequest="False" %>
<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../Portal/Modules/ctrlHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrMenu" Src="../Portal/Modules/CtrMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="InfoComidas" Src="../Portal/Modules/Contenido/InfoComidas.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../Portal/Modules/ctrlFooter.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>FoodInformation</title>
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
                <asp:Label ID="lbltitle" runat="server" EnableViewState="False" Text="Editar informacion de restaurant"
                    CssClass="tituloSeccion"></asp:Label>
            </div>
         </div>
			<TABLE id="bookingcontainer" cellSpacing="0" cellPadding="2" border="0" style=" height:600px; width:100%; ">
				<tr>
					<td style=" vertical-align:top; ">
						<TABLE cellspacing=0 cellPadding="0" width="100%" border="0" id="Table2">
							<tr>
								<td><TABLE id="Table1" cellspacing=0 cellPadding="0" width="100%" border="0">										
										<tr>
											<td>
												<uc1:InfoComidas id="InfoComidas1" runat="server"></uc1:InfoComidas></td>
										</tr>
										<TR>
											<TD>
											</TD>
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
