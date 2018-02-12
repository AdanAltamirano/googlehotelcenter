<%@ Register TagPrefix="uc1" TagName="Policies" Src="../Portal/Modules/Contenido/Policies.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../Portal/Modules/ctrlFooter.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrMenu" Src="../Portal/Modules/CtrMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../Portal/Modules/ctrlHeader.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="PoliciesInformation.aspx.vb" Inherits="RateManager.PoliciesInformation" ValidateRequest="False" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>PoliciesInformation</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="5" rightMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
		<div class="clear">
            <div class="mDiv">
            </div>
            <div class="title">
                <asp:Label ID="lbltitle" runat="server" EnableViewState="False" Text="Editar el contenido del hotel"
                    CssClass="tituloSeccion"></asp:Label>
            </div>
         </div>
			<TABLE id="bookingcontainer" cellSpacing="0" cellPadding="2" width="650" border="0" style=" height:600px; ">
				<tr>
					<td style=" vertical-align:top; ">
						<TABLE id="Table1" cellspacing=0 cellPadding="0" width="100%" border="0">
							<tr>
								<td>
									<TABLE cellspacing=0 cellPadding="0" width="100%" border="0">										
										<tr>
											<td></td>
										</tr>
										<TR>
											<TD><uc1:policies id="Policies1" runat="server"></uc1:policies></TD>
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
