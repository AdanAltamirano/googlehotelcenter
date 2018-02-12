<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Reports.aspx.vb" Inherits="RateManager.Reports" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>Search</title>

    <script language="javascript" src="../../xmlHttp.js"></script>

    <script language="javascript" src="../../Logic.js"></script>

    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="https://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../../StyleSheets/Styles.css" type="text/css" rel="stylesheet" />

    <script type="text/javascript" src="../../Includes/Script/jquery-1.4.2.min.js"></script>

    <script type="text/javascript">
        function FireUpdateStatus() {
           // $('#loadingProcess').show();
            return true;
        }
    </script>

</head>
<body bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0" ms_positioning="FlowLayout" >
    <form id="Form1" method="post" runat="server">
    <div class="clear">
        <div class="mDiv">
        </div>
        <div class="title">
            <asp:Label ID="lbltitle" runat="server" EnableViewState="False" Text="Reportes" CssClass="tituloSeccion"></asp:Label>
        </div>
    </div>
    <div id="loadingProcess"></div>
    
    <table id="bookingcontainer" cellspacing="0" cellpadding="2" width="650" border="0">
        <tr>
            <td>
                <table id="Table1" cellspacing="0" cellpadding="0" width="100%" align="center">
                    <tr>
                        <td align="left" height="33">
                            <asp:Label ID="lblSelectReport" runat="server">Seleccione un reporte</asp:Label><asp:DropDownList
                                ID="ddlSelecReport" runat="server" Width="472px" AutoPostBack="True">
                                <asp:ListItem Value="-1" Selected="True"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" height="26">
                            <asp:Label ID="lblReportDesc" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" height="26">
                            <p align="left">
                                <asp:Label ID="lblDataReport" runat="server" Visible="False" EnableViewState="False"><b>
												Datos del reporte </b>
                                </asp:Label></p>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" height="116">
                            <table id="Table2" cellspacing="0" cellpadding="0" width="100%" align="center" runat="server">
                                <tbody>
                                    <tr>
                                        <td align="left" width="109" height="13">
                                            <asp:Label ID="lblParam1" runat="server" EnableViewState="False" Visible="False">Label</asp:Label>
                                        </td>
                                        <td width="231" height="13">
                                            <asp:TextBox ID="txtBParam1" runat="server" EnableViewState="False" Visible="False"></asp:TextBox>
                                        </td>
                                        <td align="left" width="104" height="13">
                                            <asp:Label ID="lblParam2" runat="server" EnableViewState="False" Visible="False">Label
                                        </td>
                                        <td>
                                            </asp:label><td height="13">
                                                <asp:TextBox ID="txtBParam2" runat="server" EnableViewState="False" Visible="False"></asp:TextBox>
                                            </td>
                                    </tr>
                                    <tr>
                                        <td align="left" width="109" height="18">
                                            <asp:Label ID="lblParam3" runat="server" EnableViewState="False" Visible="False">Label</asp:Label>
                                        </td>
                                        <td width="231" height="18">
                                            <asp:TextBox ID="txtBParam3" runat="server" EnableViewState="False" Visible="False"></asp:TextBox>
                                        </td>
                                        <td align="left" width="104" height="18">
                                            <asp:Label ID="lblParam4" runat="server" EnableViewState="False" Visible="False">Label</asp:Label>
                                        </td>
                                        <td height="18">
                                            <asp:TextBox ID="txtBParam4" runat="server" EnableViewState="False" Visible="False"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" width="109">
                                            <asp:Label ID="lblParam5" runat="server" EnableViewState="False" Visible="False">Label</asp:Label>
                                        </td>
                                        <td width="231">
                                            <asp:TextBox ID="txtBParam5" runat="server" EnableViewState="False" Visible="False"></asp:TextBox>
                                        </td>
                                        <td align="left" width="104">
                                            <asp:Label ID="lblParam6" runat="server" EnableViewState="False" Visible="False">Label</asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBParam6" runat="server" EnableViewState="False" Visible="False"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" height="32" colspan="4" width="400">
                                            <asp:Button ID="btnExpReport" runat="server" EnableViewState="False" Text="Exportar reporte a Excel"
                                                CssClass="Button" Visible="False"></asp:Button>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                        </td>
                    </tr>
                    <tr height="5">
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:DataGrid ID="GridReport" runat="server" EnableViewState="False" Width="638px"
                                CssClass="DataGrid" PageSize="15" AllowPaging="true" AutoGenerateColumns="True"
                                Border="0" CellSpacing="1" ShowFooter="True">
                                <FooterStyle HorizontalAlign="Right"></FooterStyle>
                                <SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
                                <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
                                <ItemStyle CssClass="dgItem"></ItemStyle>
                                <HeaderStyle CssClass="dgHeader"></HeaderStyle>
                                <PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
                                    Position="Top" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
                            </asp:DataGrid>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" height="20">
                        </td>
                    </tr>
                    <tr height="5">
                        <td>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>

<script type="text/javascript">
    $().ready(function() {
        $('#loadingProcess').hide();
    });  
</script>

</html>
