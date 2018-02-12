<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ClientList.aspx.vb" Inherits="RateManager.ClientList" %>
<%@ Register Src="../../ListDate/Date.ascx" TagName="Date" TagPrefix="uc1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head runat="server">
    <title>Clients</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
</head>
<body>

    <form id="Form1" runat="server">
     <div class="clear">
        <div class="mDiv">
        </div>
        <div class="title">
            <asp:Label ID="lblTitle" runat="server" EnableViewState="False" 
                CssClass="tituloSeccion"><%=Me.GetLabel("01186")%></asp:Label>
        </div>
    </div>
    <table id="bookingcontainer" cellspacing="0" cellpadding="2" width="750" border="0">
       
        <tr>
            <td>
                <table id="Table1" border="0" cellspacing="1" cellpadding="1" width="100%">
                    <tr class="dgHeader">
                        <td class="dgitem" align="center" colspan="3">
                            <span><%=GetLabel("00479")%></span>
                        </td>
                    </tr>
                    <tr>
                        <td width="25%" align="right">
                            <span class="clsLabel"><%=GetLabel("M000025")%>:</span>
                        </td>
                        <td width="70%" align="left">
                            <script type="text/javascript">
                                function ValidatePortal(source, arguments) 
                                { 
                                    var types = document.getElementById(<%= "'"+Me.lstPortales.ClientID+"'" %>); 
                                    if(types.options[types.selectedIndex].value != '-1') 
                                        arguments.IsValid=true; 
                                    else 
                                        arguments.IsValid=false; 
                                    return arguments.IsValid;
                                }
                            </script>
                            <asp:DropDownList ID="lstPortales" runat="server">
                            </asp:DropDownList>
                            <asp:CustomValidator ID="valPortales" runat="server" ControlToValidate="lstPortales" Display="Dynamic" ClientValidationFunction="ValidatePortal"><%=GetLabel("01012")%></asp:CustomValidator>                            
                            <asp:Button Visible="false" ID="btnBuscar" runat="server" EnableViewState="False" CssClass="button" Text="Buscar" CausesValidation="true"></asp:Button>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                <table id="Table2" border="0" cellspacing="1" cellpadding="1" width="100%">
                    <tr class="dgHeader">
                        <td class="dgitem" align="center" colspan="4">
                            <span><%=GetLabel("00241")%></span>
                        </td>
                    </tr>
                    <tr class="dgHeader">
                        <td colspan="4" align="center">
                            <span><%=GetLabel("M0BT0000235")%></span>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <%
                                Me.chkFechas.Text = Me.GetLabel("00517")
                            %>
                            <asp:CheckBox ID="chkFechas" runat="server" Text="Incluir Fechas" CssClass="clslabel" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <span class="clsLabel"><%=GetLabel("00108")%>:</span>
                        </td>
                        <td align="left">
                            <uc1:Date ID="txtDesde" runat="server" />
                        </td>
                        <td align="right">
                            <span class="clsLabel"><%=GetLabel("00109")%>:</span>
                        </td>
                        <td align="left">
                            <uc1:Date ID="txtHasta" runat="server" />
                        </td>
                    </tr>
                    <tr class="dgHeader">
                        <td colspan="4" align="center">
                            <span class="clsHeaderTitle"><%=GetLabel("00420")%></span>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <span class="clsLabel"><%=GetLabel("00073")%>:</span>
                        </td>
                        <td align="left">                            
                            <asp:TextBox ID="txtNombre" runat="server" Width="205px"></asp:TextBox>                            
                        </td>
                        <td align="right">
                            <span class="clsLabel"><%=GetLabel("00163")%>:</span>
                        </td>
                        <td align="left">                            
                            <asp:TextBox ID="txtEmail" runat="server" Width="212px"></asp:TextBox>                            
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="right">                            
                            <asp:Button ID="btnFiltrar" runat="server" Text="Buscar" EnableViewState="False" CssClass="button"  CausesValidation="true"/>                            
                        </td>
                    </tr>
                    <tr style="padding-top:4px; padding-bottom:4px; "><td align= "center" colspan="4" >
                        <asp:Label ID="lblFiltro" runat="server" Text="" CssClass="clsHelpLabel" ></asp:Label>
                    </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr height="5">
            <td>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:DataGrid ID="lstClientes" runat="server" CssClass="DataGrid" 
                    ShowFooter="True" AutoGenerateColumns="False" AllowPaging="True" Width="95%" 
                    PageSize="15">
                    <SelectedItemStyle CssClass="dgSelected" Font-Bold="False" Font-Italic="False" 
                        Font-Overline="False" Font-Strikeout="False" Font-Underline="False" 
                        HorizontalAlign="Left"></SelectedItemStyle>
                    <AlternatingItemStyle CssClass="dgAlternate" Font-Bold="False" 
                        Font-Italic="False" Font-Overline="False" Font-Strikeout="False" 
                        Font-Underline="False" HorizontalAlign="Left"></AlternatingItemStyle>
                    <ItemStyle CssClass="dgItem" Font-Bold="False" Font-Italic="False" 
                        Font-Overline="False" Font-Strikeout="False" Font-Underline="False" 
                        HorizontalAlign="Left"></ItemStyle>
                    <HeaderStyle CssClass="dgHeader" Font-Bold="False" Font-Italic="False" 
                        Font-Overline="False" Font-Strikeout="False" Font-Underline="False" 
                        HorizontalAlign="Center"></HeaderStyle>
                    <FooterStyle HorizontalAlign="Right"></FooterStyle>
                    <Columns>
                        <asp:BoundColumn Visible="False" DataField="Id" HeaderText="ID"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Email" HeaderText="Email"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Nombre" HeaderText="Nombre"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Telefono" HeaderText="Telefono">
                            <HeaderStyle Width="100px" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Domicilio" HeaderText="Domicilio"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CodigoPostal" HeaderText="CP">
                            <HeaderStyle Width="30px" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Ciudad" HeaderText="Ciudad"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Company" HeaderText="Compañia"></asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="Promotions">
                            <ItemTemplate>
                                <span>
                                    <%#If(Eval("WantPromotions") = 1, GetLabel("00030"), GetLabel("00031"))%></span>
                            </ItemTemplate>
                            <HeaderStyle Width="20px" />
                        </asp:TemplateColumn>
                    </Columns>
                    <PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
                        Position="Top" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
                </asp:DataGrid>
            </td>
        </tr>
        <tr height="5">
            <td>
            </td>
        </tr>
    </table>
    </td> </tr> </table>
    </form>
</body>
</html>
