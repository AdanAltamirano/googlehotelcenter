<%@ Import Namespace="RateManager" %>
<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../Portal/Modules/ctrlFooter.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../Portal/Modules/ctrlHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrMenu" Src="../Portal/Modules/CtrMenu.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RatePlanContentPortal.aspx.vb" Inherits="RateManager.RatePlanContentPortal" ValidateRequest="false"  %>

<%@ Register src="../Modulos/CtrRatePlanContain.ascx" tagname="CtrRatePlanContain" tagprefix="uc1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
  <title>RatePlansLinks</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet" />
</head>
<script>
function ShowNewInfo(value) {
    if (value == 1) {
        document.getElementById("dvContent").style.display = "block";
        } else {
        document.getElementById("dvContent").style.display = "none";        
        }
}
</script>
<body  ms_positioning="FlowLayout" bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0">
    <form id="Form1" method="post" runat="server">
      
       <div class="clear"> 
     <table id="bookingcontainer clear" border="0" cellspacing="0" cellpadding="2"  width="100%">
        <tr>
            <td class="titulo" align="left">
                                  <img src="../Includes/imagenes/icono-big-plus.png" width="25" height="25" />
                     <asp:Label ID="lblTitle"  class="tituloSeccion"  runat="server" EnableViewState="False">Rates Plans Content</asp:Label>
             </td>
            </tr>
            <tr class="trTitle rounded-corners" id="dvContent2">
                                <td class="dgitem" align="left">
                                    
                                     <asp:label id="lblContPortal" EnableViewState="False" CssClass="bookingnormallabel" runat="server" >Contenido En Portales</asp:label>
                                </td>
                     </tr>
            <tr class="trContent rounded-corners" id="dvContent">
                                <td class="tdContent">
                                
                                 <table id="Table2" border="0" cellspacing="0" cellpadding="0" width="100%" align="center"  class="form">
                    
                    
                    
                    <tr>
                        <td colspan="2" align="center">
                            <uc1:CtrRatePlanContain ID="CtrRatePlanContain1" runat="server" />
                            <br>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <asp:Label ID="lblError" runat="server" CssClass="Validators" Visible="False">Error</asp:Label><asp:Label
                                ID="lblErrorSource" runat="server" CssClass="Validators" Visible="False">*</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 20px" colspan="2" align="center">
                            <asp:Button ID="btnSave" runat="server" EnableViewState="False"
                                    CssClass="Button" Text="Guardar" Visible =false ></asp:Button><asp:Button ID="btncancel" runat="server"
                                        EnableViewState="False" CssClass="Button" Text="Cancelar" CausesValidation="False" >
                                    </asp:Button>
                        </td>
                    </tr>
                   
                </table>
                                </td>
                                </tr>
      </table>
      </div>
      <div class="clear">
       <asp:DataGrid ID="dgRateplan" GridLines="None"  runat="server" Width="99%" AllowPaging="True" PageSize="20"
                                AutoGenerateColumns="False" CssClass="datagrid" ShowFooter="True">
                                <SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
                                <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
                                <ItemStyle CssClass="dgItem"></ItemStyle>
                                <HeaderStyle CssClass="dgHeader"></HeaderStyle>
                                <FooterStyle HorizontalAlign="Right"></FooterStyle>
                                <Columns>
                                    <asp:BoundColumn Visible="False" DataField="idrateplan"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="orden" HeaderText="Orden">
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="codigotarifa" HeaderText="C&#243;digo">
                                        <HeaderStyle Width="9%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Name" HeaderText="Nombre">
                                        <HeaderStyle Width="40%"></HeaderStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Segment" HeaderText="Segmento">
                                        <HeaderStyle Width="23%"></HeaderStyle>
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn>
                                        <HeaderStyle Width="10%"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkedit" runat="server" CssClass="dgLink" CausesValidation="False"
                                                CommandName="Select">editar</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    
                                    <asp:BoundColumn Visible="False" DataField="SegmentRacPrinc"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="codigotarifa"></asp:BoundColumn>
                                </Columns>
                                <PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
                                    Position="Top" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
                            </asp:DataGrid>
      </div>
   
    </form>
</body>
</html>
