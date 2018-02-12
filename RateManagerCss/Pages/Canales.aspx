<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../Portal/Modules/ctrlFooter.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../Portal/Modules/ctrlHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrlIdioma" Src="../Modulos/CtrlIdioma.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Canales.aspx.vb" Inherits="RateManager.Canales" %>

<%@ Register TagPrefix="uc1" TagName="ctlMensajes" Src="../Modulos/ctlMensajes.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrMenu" Src="../Portal/Modules/CtrMenu.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>Contracts Net Rates</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
    
     <script language="javascript" type="text/javascript">

         function FireShow(ID, IDcmd, show) {
             var e = document.getElementById(ID);
             var c = document.getElementById(IDcmd);
             if (e) {
                 e.style.display = show ? 'block' : 'none';
             }
             if (c) {
                 c.style.display = !show ? 'block' : 'none';
             }

         }
			
    </script>
</head>
<body bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
		<div class="clear">
            <div class="mDiv">
            </div>
            <div class="title">
                <asp:Label ID="lbltitle" runat="server" EnableViewState="False" Text="Administración de Canales" CssClass="tituloSeccion"></asp:Label>
            </div>
         </div>    
    <table id="bookingcontainer" cellspacing="0" cellpadding="2" width="650" border="0">
        <tr>
            <td>
                <table id="tblMain" cellspacing="0" cellpadding="0" width="100%" align="center" border="0">                    
                    <tr>
                        <td style="text-align: right;">
                            <input type="button" id="cmdNew" class="ButtonNew" runat="server" causesvalidation="false"
                                value="New" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="pnlData" runat="server">
                                <table id="tblData" cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                                    <tr>
                                        <td align="center" colspan="3">
                                            <asp:Label ID="lblMsgActualizacion" runat="server" CssClass="Validators" Visible="False"> Operacion Exitosa</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="2">
                                            <asp:Label ID="lblNombre" runat="server" EnableViewState="False" CssClass="clslabel">Nombre:</asp:Label>
                                        </td>
                                        <td align="left">
                                            <uc1:CtrlIdioma ID="CtrlIdiomaNombre" runat="server"></uc1:CtrlIdioma>
                                            <asp:Label ID="lblMsgTitle" runat="server" CssClass="Validators" Visible="False"> Proporcione el titulo en ambos idiomas</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="2">
                                            <asp:Label ID="lblLink" runat="server" EnableViewState="False" CssClass="clslabel"> Link:</asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="TextBoxLink" runat="server" Width="296px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="2">
                                            <asp:Label ID="lblDescripcion" runat="server" EnableViewState="False" CssClass="clslabel"> Descripción:</asp:Label>
                                        </td>
                                        <td align="left">
                                            <uc1:CtrlIdioma ID="CtrlIdiomaDescripcion" runat="server"></uc1:CtrlIdioma>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="3">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="3">
                                            <asp:Button ID="btnAceptar" runat="server" EnableViewState="False" CssClass="button"
                                                Text="Guardar"></asp:Button>&nbsp;&nbsp;<asp:Button ID="btnCancel" runat="server"
                                                    EnableViewState="False" CssClass="button" Text="Cancelar" CausesValidation="False">
                                                </asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DataGrid ID="dgCanales" runat="server" AutoGenerateColumns="False" Width="99%"
                                AllowPaging="True" PageSize="20" CssClass="datagrid" ShowFooter="True">
                                <FooterStyle HorizontalAlign="Right"></FooterStyle>
                                <SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
                                <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
                                <ItemStyle CssClass="dgItem"></ItemStyle>
                                <HeaderStyle CssClass="dgHeader"></HeaderStyle>
                                <Columns>
                                    <asp:BoundColumn DataField="Title" HeaderText="Nombre">
                                        <ItemStyle Width="30%"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="idCanal">
                                        <ItemStyle Width="30%"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn>
                                        <ItemStyle Width="10%"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Select" CssClass="dgLink">Editar</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn>
                                        <ItemStyle Width="10%"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEliminar2" Style="display: none" runat="server" CssClass="dgLink"
                                                CommandName="Eliminar">Eliminar</asp:LinkButton>
                                            <asp:HyperLink ID="lnkEliminar" runat="server" CssClass="dglink">Eliminar</asp:HyperLink>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                                <PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
                                    Position="Top" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
                            </asp:DataGrid>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <uc1:ctlMensajes ID="CtlMensajes1" runat="server"></uc1:ctlMensajes>
    </form>
</body>
</html>
