<%@ Control Language="vb" AutoEventWireup="false" Codebehind="CtrlIdiomaRFCk.ascx.vb" Inherits="RateManager.CtrlIdiomaRFCk" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="uc1" TagName="CtrlIdiomaFCk" Src="../Portal/Modules/Contenido/CtrlIdiomaFCk.ascx" %>

<script runat="server">

    
</script>

<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
	<TR>
		<td>
			<uc1:CtrlIdiomaFCk id="CtrlIdiomaFCk1" runat="server"></uc1:CtrlIdiomaFCk>
		</td>
	</TR>
	<tr>
		<td>
			<asp:RequiredFieldValidator id="rfvDefaultText" runat="server" Display="Dynamic" CssClass="validators" ControlToValidate="TextBox1"
				Enabled="False">El texto en ingles es requerido</asp:RequiredFieldValidator>
			<asp:TextBox id="TextBox1" runat="server" Width="32px" Visible="False"></asp:TextBox>
		</td>
	</tr>
</TABLE>
