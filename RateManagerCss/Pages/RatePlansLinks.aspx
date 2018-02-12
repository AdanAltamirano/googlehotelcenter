<%@ Import Namespace="RateManager" %>
<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../Portal/Modules/ctrlFooter.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../Portal/Modules/ctrlHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrMenu" Src="../Portal/Modules/CtrMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctlMensajes" Src="../Modulos/ctlMensajes.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RatePlansLinks.aspx.vb"
    Inherits="RateManager.RatePlansLinks" %>
<%@ Register TagPrefix="uc1" TagName="ctrRatePlanLink" Src="../Modulos/ctrRatePlanLink.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>    
    <title>RatePlansLinks</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
    
    <script type="text/javascript">
        function DesabilidaValidadores() {
            
            var i;
            for (i = 0; i < Page_Validators.length; i++) {
                    ValidatorEnable(Page_Validators[i], false);    
            }
            
            return;
        }
    
    </script>
    
</head>
<script>
    function ShowNewInfo(value) {
        if (value == 1) {
            document.getElementById("dvContent").style.display = "block";
            document.getElementById("dvContent2").style.display = "block";

            document.getElementById("<%=btnAceptar.clientid %>").style.display = "block";
            document.getElementById("<%=btnCancel.clientid %>").style.display = "block";
           

        } else {
            document.getElementById("dvContent").style.display = "none";
            document.getElementById("dvContent2").style.display = "none";
            document.getElementById("<%=btnAceptar.clientid %>").style.display = "none";
            document.getElementById("<%=btnCancel.clientid %>").style.display = "none";
           
        }
    }

    function FireShow(ID, IDcmd, show) {
        var e = document.getElementById(ID);
        var c = document.getElementById(IDcmd);
        if (e) {
            e.style.display = show ? 'block' : 'none';
        }
        if (c) {
            c.style.display = !show ? 'block' : 'none';
        }
        onResizeIframe();
    }
</script>
<body bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">

<div class="clear">
        <input type="button" id="cmdNew" class="ButtonNew" runat="server" causesvalidation="false"
            value="New" style=" width:85px;" />
        <div class="mDiv">
        </div>
        <div class="title">
            <asp:Label ID="lblTitle" runat="server" EnableViewState="False"  class="tituloSeccion">Linked Rate Plans</asp:Label>
        </div>
</div>      
    <div runat="server" id="divContenedor">
        <table id="bookingcontainer clear" width="100%" cellspacing="0" cellpadding="2"  border="0">
  
        <tr>
            <td>
                <table id="Table1" cellspacing="0" cellpadding="0" width="100%" align="center" border="0">                
                   <tr class="trTitle rounded-corners"  id="dvContent2">
                                <td class="dgitem" align="left">
                                  <asp:label id="lblctrlTitle" runat="server" CssClass="bookingnormallabel">Add linked Rate Plan</asp:label>
                                </td>
                            </tr>
                    <tr class="trContent rounded-corners"  id="dvContent">
                                <td class="tdContent">
                                <uc1:ctrRatePlanLink ID="CtrRatePlanLink1" runat="server"></uc1:ctrRatePlanLink>
                                <br />
                                 <asp:Button ID="btnAceptar" runat="server" Text="Guardar" CssClass="button" EnableViewState="False">
                            </asp:Button>
                            <asp:Button ID="btnCancel" runat="server" Text="Cancelar" CssClass="button" CausesValidation="False"
                                EnableViewState="False"></asp:Button>
                              
                                </td>
                    </tr>
                    <tr>
                    <td> 
                                   <asp:Label Style="z-index: 0" ID="lblError" runat="server" CssClass="Validators"
                                Visible="False"></asp:Label></td>
                    </tr>
                  
                </table>
            </td>
        </tr>
    </table>
   </div>
   <div class="clear">
    <asp:DataGrid ID="dglinks"  GridLines="None" runat="server" PageSize="20" AllowPaging="True" AutoGenerateColumns="False"
                                CssClass="datagrid" Width="99%" ShowFooter="True">
                                <SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
                                <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
                                <ItemStyle CssClass="dgItem"></ItemStyle>
                                <HeaderStyle CssClass="dgHeader"></HeaderStyle>
                                <FooterStyle HorizontalAlign="Right"></FooterStyle>
                                <Columns>
                                    <asp:BoundColumn DataField="RateCodeSource" HeaderText="source">
                                        <ItemStyle Width="30%"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="RateCodeTarget" HeaderText="Tarjet">
                                        <ItemStyle Width="30%"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="OnePersonRatio" HeaderText="Ratio">
                                        <HeaderStyle Width="10%" HorizontalAlign="center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="OnePersonOffset" HeaderText="Offset">
                                        <HeaderStyle Width="10%" HorizontalAlign="Right"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="RoundAmount" HeaderText="Round Amount">
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn>
                                        <HeaderStyle Width="10%"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEdit" runat="server" CssClass="dglink" CommandName="Select" CausesValidation="false" >Editar</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn>
                                        <HeaderStyle Width="10%"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEliminar2" runat="server" CssClass="dgLink" CommandName="Eliminar" CausesValidation=false 
                                                Style="display: none">Eliminar</asp:LinkButton>
                                            <asp:HyperLink ID="lnkEliminar" runat="server" CssClass="dglink"  >Eliminar</asp:HyperLink>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn Visible="False" DataField="TargetRatePlan"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="SourceRatePlan" HeaderText="Original Rate Plan">
                                    </asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="RateCodeTarget"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="TargetName"></asp:BoundColumn>
                                    <asp:BoundColumn Visible="False" DataField="SourceName"></asp:BoundColumn>
                                </Columns>
                                <PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
                                    Position="Bottom" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
                            </asp:DataGrid>
   </div>
    <uc1:ctlMensajes ID="CtlMensajes1" runat="server"></uc1:ctlMensajes>
    </form>
</body>
</html>
