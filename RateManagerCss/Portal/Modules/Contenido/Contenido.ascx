<%@ Register TagPrefix="uc1" TagName="CtrlIdiomaFCk" Src="CtrlIdiomaFCk.ascx" %>
<%@ Control Language="vb" AutoEventWireup="false" Codebehind="Contenido.ascx.vb" Inherits="RateManager.Contenidos" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="uc1" TagName="CtrlIdioma" Src="CtlIdioma.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Imagenes" Src="Imagenes.ascx" %>



<TABLE id=Table1 cellSpacing=0 cellPadding=0 width="100%" border=1>
  <TR>
    <TD id=tcContenido runat="server"><asp:linkbutton id=lnkContenido runat="server" CssClass="dgLink" Visible="False" Font-Bold="True" Font-Size="8pt" ForeColor="Black">Contenido</asp:linkbutton></TD>
    <TD id=tcDefault runat="server"><asp:linkbutton id=lnkContenidoDef runat="server" CssClass="dgLink" Visible="False" Font-Size="8pt">Default</asp:linkbutton></TD></TR>
  <TR>
    <TD colSpan=2 height="100%"><asp:panel id=Tcont 
       Width="100%" Height="100%" Runat="server">
      <TABLE class=caja height="100%" cellSpacing=1 cellPadding=1 width="100%" 
      align=left border=0>
        <TR>
          <TD vAlign=top align=left colSpan=2>
<asp:label id=lblContenido runat="server" CssClass="clsLabel">Contenido</asp:label> 
<uc1:CtrlIdiomaFCk id=CtrlIdiomaFCk1 runat="server" Visible="False"></uc1:CtrlIdiomaFCk></TD></TR>
        <TR>
          <TD vAlign=top align=left colSpan=2 height="100%">
<uc1:CtrlIdioma id=CtrlIdioma1 runat="server" visible="false" Height="150"></uc1:CtrlIdioma>
<asp:textbox id=txtcontenido runat="server" CssClass="TextBox" Width="100%" Height="136px" Rows="6" Columns="30" TextMode="MultiLine"></asp:textbox></TD></TR>
        <TR>
          <TD vAlign=top align=left colSpan=2>
<asp:label id=lblURL runat="server" CssClass="clsLabel">HRef</asp:label></TD></TR>
        <TR>
          <TD vAlign=top align=center colSpan=2>
<asp:textbox id=txtURL runat="server" CssClass="TextBox" Width="100%" Rows="3" Columns="50"></asp:textbox></TD></TR>
        <TR>
          <TD align=left>
<asp:regularexpressionvalidator id=REWwidth runat="server" CssClass="Validators" ForeColor=" " ErrorMessage="Width debe ser Numerico" ControlToValidate="txtWidth" ValidationExpression="(D-)?\d{1,3}" Display="Dynamic"></asp:regularexpressionvalidator></TD>
          <TD>
<asp:regularexpressionvalidator id=REVHeight runat="server" CssClass="Validators" ForeColor=" " ErrorMessage="Height debe ser Numerico" ControlToValidate="txtHeight" ValidationExpression="(D-)?\d{1,3}" Display="Dynamic"></asp:regularexpressionvalidator></TD></TR>
        <TR vAlign=top height=1>
          <TD align=left>
<asp:label id=lblWidth runat="server" CssClass="clsLabel">Width</asp:label></TD>
          <TD>
<asp:label id=lblHeight runat="server" CssClass="clsLabel">Height</asp:label></TD></TR>
        <TR vAlign=top height=1>
          <TD align=left>
<asp:textbox id=txtWidth runat="server" CssClass="TextBox" Width="50px" MaxLength="3">0</asp:textbox></TD>
          <TD>
<asp:textbox id=txtHeight runat="server" CssClass="TextBox" Width="50px" MaxLength="3">0</asp:textbox></TD></TR>
        <TR>
          <TD align=center colSpan=2 >
    <asp:image id=ShowImg runat="server" load="javascript:this.style.display='block'" onerror="javascript:this.style.display='none'" Style='<%# Resize(true,"200px") %>' ></asp:image><BR>
<asp:label id=lblURLArchivo runat="server" CssClass="clsLabel" Width="40px"></asp:label>aa
<asp:ImageButton id=btnQuitar runat="server" ImageUrl="../../../Images/delete.gif"></asp:ImageButton></TD></TR>
        <TR>
          <TD colSpan=2>
<uc1:imagenes id=ImgControl runat="server"></uc1:imagenes>
<asp:label id=lblMensaje runat="server" Visible="False"></asp:label></TD></TR></TABLE></asp:panel>
			<!-- tabla default --><asp:panel 
      id=TContDef Visible="False" Width="100%" 
      Runat="server">
      <TABLE class=caja cellSpacing=2 width="100%" align=left border=0>
        <TR>
          <TD vAlign=top align=left colSpan=2>
<asp:label id=lblcontenidoDef runat="server" CssClass="clsLabel">Contenido Default</asp:label></TD></TR>
        <TR>
          <TD vAlign=top align=left colSpan=2>
<asp:textbox id=txtcontenidodef runat="server" CssClass="TextBox" Width="100%" Rows="6" Columns="30" TextMode="MultiLine"></asp:textbox></TD></TR>
        <TR>
          <TD vAlign=top align=left colSpan=2>
<asp:label id=lblURLDef runat="server" CssClass="clsLabel">HRef Default</asp:label></TD></TR>
        <TR>
          <TD vAlign=top align=center colSpan=2>
<asp:textbox id=txturlDef runat="server" CssClass="TextBox" Width="100%" Rows="3" Columns="50"></asp:textbox></TD></TR>
        <TR vAlign=top height=1>
          <TD align=left>
<asp:label id=lblwidthDef runat="server" CssClass="clslabel">Width Def</asp:label></TD>
          <TD>
<asp:label id=lblHeightDef runat="server" CssClass="clslabel">Height Def</asp:label></TD></TR>
        <TR vAlign=top height=1>
          <TD align=left>
<asp:textbox id=txtwidthDef runat="server" CssClass="TextBox" Width="50px" MaxLength="3">0</asp:textbox>
<asp:regularexpressionvalidator id=revWidthDef runat="server" CssClass="Validators" ErrorMessage="Width debe ser Numerico" ControlToValidate="txtWidthDef" ValidationExpression="(D-)?\d{1,3}" Display="Dynamic">*</asp:regularexpressionvalidator></TD>
          <TD>
<asp:textbox id=txtHeightDef runat="server" CssClass="TextBox" Width="50px" MaxLength="3">0</asp:textbox>
<asp:regularexpressionvalidator id=revHeightDef runat="server" CssClass="Validators" ErrorMessage="Height debe ser Numerico" ControlToValidate="txtHeightDef" ValidationExpression="(D-)?\d{1,3}" Display="Dynamic">*</asp:regularexpressionvalidator></TD></TR>
        <TR>
          <TD align=center colSpan=2>
<asp:image id=ShowImgDef runat="server" load="javascript:this.style.display='block'" onerror="javascript:this.style.display='none'"></asp:image><BR>
<asp:label id=lblURLArchivoDef runat="server" CssClass="clsLabel" Width="40px"></asp:label></TD></TR>
        <TR>
          <TD colSpan=2>
<uc1:imagenes id=imgControlDef runat="server"></uc1:imagenes>
<asp:label id=lblmensajeDef runat="server" Visible="False"></asp:label></TD></TR></TABLE></asp:panel>
</TD></TR></TABLE>
