<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ContenidoFCk.ascx.vb" Inherits="RateManager.ContenidoFCk" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="uc1" TagName="CtrlIdiomaFCk" Src="CtrlIdiomaFCk.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Imagenes" Src="Imagenes.ascx" %>
<script type="text/javascript" src='<%= (request.applicationpath & "/pages/Scripts/jquery.min.js").Replace("//","/") %>'></script>

<TABLE id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
	<TR>
		<TD id="tcContenido" runat="server"><asp:linkbutton id="lnkContenido" runat="server" ForeColor="Black" Font-Size="8pt" Font-Bold="True"
				Visible="False" CssClass="dgLink">Contenido</asp:linkbutton></TD>
		<TD id="tcDefault" runat="server"><asp:linkbutton id="lnkContenidoDef" runat="server" Font-Size="8pt" Visible="False" CssClass="dgLink">Default</asp:linkbutton></TD>
	</TR>
	<TR>
		<TD colSpan="2" height="300"><asp:panel id="Tcont" Runat="server" Height="100%" Width="100%">
				<TABLE class="caja" id="Table2" cellSpacing="1" cellPadding="1" width="100%"
					align="left" border="0">
					<TR>
						<TD vAlign="top" align="left" colSpan="2">
							<asp:label id="lblContenido" runat="server" CssClass="clsLabel">Contenido</asp:label></TD>
					</TR>
					<TR>
						<TD vAlign="top" align="left" colSpan="2">
							<uc1:CtrlIdiomaFCk id="CtrlIdiomaFCk1" runat="server"></uc1:CtrlIdiomaFCk>
							<asp:textbox id="txtcontenido" runat="server" CssClass="TextBox" Height="136px" Width="100%"
								TextMode="MultiLine" Columns="30" Rows="6"></asp:textbox></TD>
					</TR>
					<TR>
						<TD vAlign="top" align="left" colSpan="2">
							<asp:label id="lblURL" runat="server" CssClass="clsLabel">HRef</asp:label></TD>
					</TR>
					<TR>
						<TD vAlign="top" align="center" colSpan="2">
							<asp:textbox id="txtURL" runat="server" CssClass="TextBox" Width="100%" Columns="50" Rows="3"></asp:textbox></TD>
					</TR>
					<TR>
						<TD align="left">
							<asp:regularexpressionvalidator id="REWwidth" runat="server" ForeColor=" " CssClass="Validators" Display="Dynamic"
								ValidationExpression="(D-)?\d{1,3}" ControlToValidate="txtWidth" ErrorMessage="Width debe ser Numerico"></asp:regularexpressionvalidator></TD>
						<TD>
							<asp:regularexpressionvalidator id="REVHeight" runat="server" ForeColor=" " CssClass="Validators" Display="Dynamic"
								ValidationExpression="(D-)?\d{1,3}" ControlToValidate="txtHeight" ErrorMessage="Height debe ser Numerico"></asp:regularexpressionvalidator></TD>
					</TR>
					<TR vAlign="top" height="1">
						<TD align="left">
							<asp:label id="lblWidth" runat="server" CssClass="clsLabel">Width</asp:label></TD>
						<TD>
							<asp:label id="lblHeight" runat="server" CssClass="clsLabel">Height</asp:label></TD>
					</TR>
					<TR vAlign="top" height="1">
						<TD align="left">
							<asp:textbox id="txtWidth" runat="server" CssClass="TextBox" Width="50px" MaxLength="3">0</asp:textbox></TD>
						<TD>
							<asp:textbox id="txtHeight" runat="server" CssClass="TextBox" Width="50px" MaxLength="3">0</asp:textbox></TD>
					</TR>
					<TR>
						<TD align="center" colSpan="2" valign=top >
							<asp:image id="ShowImg" runat="server" onerror="javascript:this.style.display='none'" load="javascript:this.style.display='block'" style=" width:200px; "></asp:image>
					</TR>
					<TR>
					    <td style="text-align:right;"><asp:ImageButton id="btnQuitar" runat="server" ImageUrl="../../../Images/delete.gif"></asp:ImageButton></td>
						<TD align="center" colSpan="2" style=" text-align:left;">
							<asp:label id="lblURLArchivo" runat="server" CssClass="clsLabel" Width="40px"></asp:label>
							
						</TD></TR>
					<TR>
						<TD colSpan="2">
							<uc1:imagenes id="ImgControl" runat="server"></uc1:imagenes>
							<asp:label id="lblMensaje" runat="server" Visible="False"></asp:label></TD>
					</TR>
				</TABLE>
			</asp:panel><!-- tabla default --><asp:panel id="TContDef" Visible="False" Runat="server" Width="100%">
				<TABLE class="caja" id="Table3" cellSpacing="2" width="100%" align="left" border="0">
					<TR>
						<TD vAlign="top" align="left" colSpan="2">
							<asp:label id="lblcontenidoDef" runat="server" CssClass="clsLabel">Contenido Default</asp:label></TD>
					</TR>
					<TR>
						<TD vAlign="top" align="left" colSpan="2">
							<asp:textbox id="txtcontenidodef" runat="server" CssClass="TextBox" Width="100%" TextMode="MultiLine"
								Columns="30" Rows="6"></asp:textbox></TD>
					</TR>
					<TR>
						<TD vAlign="top" align="left" colSpan="2">
							<asp:label id="lblURLDef" runat="server" CssClass="clsLabel">HRef Default</asp:label></TD>
					</TR>
					<TR>
						<TD vAlign="top" align="center" colSpan="2">
							<asp:textbox id="txturlDef" runat="server" CssClass="TextBox" Width="100%" Columns="50" Rows="3"></asp:textbox></TD>
					</TR>
					<TR vAlign="top" height="1">
						<TD align="left">
							<asp:label id="lblwidthDef" runat="server" CssClass="clslabel">Width Def</asp:label></TD>
						<TD>
							<asp:label id="lblHeightDef" runat="server" CssClass="clslabel">Height Def</asp:label></TD>
					</TR>
					<TR vAlign="top" height="1">
						<TD align="left">
							<asp:textbox id="txtwidthDef" runat="server" CssClass="TextBox" Width="50px" MaxLength="3">0</asp:textbox>
							<asp:regularexpressionvalidator id="revWidthDef" runat="server" CssClass="Validators" Display="Dynamic" ValidationExpression="(D-)?\d{1,3}"
								ControlToValidate="txtWidthDef" ErrorMessage="Width debe ser Numerico">*</asp:regularexpressionvalidator></TD>
						<TD>
							<asp:textbox id="txtHeightDef" runat="server" CssClass="TextBox" Width="50px" MaxLength="3">0</asp:textbox>
							<asp:regularexpressionvalidator id="revHeightDef" runat="server" CssClass="Validators" Display="Dynamic" ValidationExpression="(D-)?\d{1,3}"
								ControlToValidate="txtHeightDef" ErrorMessage="Height debe ser Numerico">*</asp:regularexpressionvalidator></TD>
					</TR>
					<TR>
						<TD align="center" colSpan="2">
							<asp:image id="ShowImgDef" runat="server" onerror="javascript:this.style.display='none'" load="javascript:this.style.display='block'"></asp:image><BR>
							<asp:label id="lblURLArchivoDef" runat="server" CssClass="clsLabel" Width="40px"></asp:label></TD>
					</TR>
					<TR>
						<TD colSpan="2">
							<uc1:imagenes id="imgControlDef" runat="server"></uc1:imagenes>
							<asp:label id="lblmensajeDef" runat="server" Visible="False"></asp:label></TD>
					</TR>
				</TABLE>
			</asp:panel></TD>
	</TR>
</TABLE>
<script type="text/javascript">
    $(document).ready(function() {
        onResizeIframe(200);
    });
</script>
