<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MapeoPlanesF2G.aspx.vb" Inherits="RateManager.MapeoPlanesF2G" %>
<%@ Register TagPrefix="uc1" TagName="CtrMenu" Src="../../Portal/Modules/CtrMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctlMensajes" Src="../../Modulos/ctlMensajes.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../../Portal/Modules/ctrlHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../../Portal/Modules/ctrlFooter.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>PMS Coincidences</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link rel="stylesheet" type="text/css" href="../../StyleSheets/Styles.css">
</head>
<body bottommargin="0" leftmargin="0" rightmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
        <table id="bookingcontainer" border="0" cellspacing="0" cellpadding="2" width="650">
            <tr>
                <td>
                    <table id="Table1" border="0" cellspacing="0" cellpadding="0" width="100%" align="center">
                        <tr>
                            <td class="Titulo" colspan="3" align="center">
                                <asp:Label ID="lblTitle" runat="server" EnableViewState="False">Configuración de Planes Front2Go</asp:Label></td>
                        </tr>
                        <tr>
                            <td height="5" colspan="3"></td>
                        </tr>
                        <tr>
                            <td width="50%" align="left">
                                <asp:Label ID="Label1" runat="server" CssClass="clslabel" Visible="false">Hoteles</asp:Label></td>
                            <td width="50%"></td>
                        </tr>
                        <tr>
                            <td style="HEIGHT: 22px" width="50%" colspan="2" align="left">
                                <asp:DropDownList Visible="false" ID="ddlHoteles" runat="server" Width="232px"></asp:DropDownList>&nbsp;
									<asp:Button Style="Z-INDEX: 0" Visible="false" ID="btnCargar" runat="server" EnableViewState="False" CssClass="button"
                                        Text="Cargar"></asp:Button></td>
                        </tr>
                        <tr>
                            <td style="HEIGHT: 13px" width="50%" align="center"></td>
                            <td style="HEIGHT: 13px" width="50%"></td>
                        </tr>
                        <tr>
                            <td style="HEIGHT: 11px" width="50%" colspan="2" align="center">
                                <asp:Label Style="Z-INDEX: 0" ID="lblHotelSelected" runat="server" CssClass="bookingnormallabel">Hotel Seleccionado</asp:Label></td>
                        </tr>
                        <tr>
                            <td style="HEIGHT: 11px" width="50%" align="center"></td>
                            <td style="HEIGHT: 11px" width="50%" align="center"></td>
                        </tr>
                        <tr id="renglonEtiquetas" runat="server">
                            <td style="HEIGHT: 11px" width="50%" align="center">
                                <asp:Label Style="Z-INDEX: 0" ID="lblRatesPlan" runat="server" CssClass="bookingnormallabel">Rates Plan</asp:Label></td>
                            <td style="HEIGHT: 11px" width="50%" align="center">
                                <asp:Label Style="Z-INDEX: 0" ID="lblHoteles" Visible="false" runat="server" CssClass="bookingnormallabel">Rooms</asp:Label></td>
                        </tr>
                        <tr id="renglonDatos" runat="server">
                            <td valign="top" width="50%" align="center">
                                <div style="HEIGHT: 350px; OVERFLOW: auto" id="ratesPlan" runat="server" ms_positioning="FlowLayout">
                                    <asp:DataGrid ID="dgRatesPlan" runat="server" CssClass="datagrid" Width="99%" AutoGenerateColumns="False"
                                        PageSize="5" ShowFooter="True">
                                        <FooterStyle HorizontalAlign="Right"></FooterStyle>
                                        <SelectedItemStyle CssClass="dgSelected"></SelectedItemStyle>
                                        <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
                                        <ItemStyle CssClass="dgItem"></ItemStyle>
                                        <HeaderStyle CssClass="dgHeader"></HeaderStyle>
                                        <Columns>
                                            <asp:BoundColumn DataField="idRatePlan" HeaderText="Codigo">
                                                <ItemStyle Width="30%"></ItemStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="Nombre" HeaderText="Nombre">
                                                <ItemStyle Width="30%"></ItemStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn Visible="False" DataField="idRatePlan_F2G">
                                                <ItemStyle Width="30%"></ItemStyle>
                                            </asp:BoundColumn>
                                            <asp:TemplateColumn HeaderText="Código F2G">
                                                <ItemStyle Width="10%"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtidRatePlanF2G" runat="server" Width="160px" MaxLength="100" Text=""></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                             <asp:BoundColumn Visible="False" DataField="PromoCode">
                                                <ItemStyle Width="30%"></ItemStyle>
                                            </asp:BoundColumn>
                                            <asp:TemplateColumn HeaderText="Código Promoción">
                                              <ItemStyle Width="10%"></ItemStyle>
                                               <ItemTemplate>
                                                <asp:TextBox ID="txtIdPromoCodePMS" runat="server" Width="160px" MaxLength="100" Text=""></asp:TextBox>
                                               </ItemTemplate>
                                            </asp:TemplateColumn>
                                        </Columns>
                                        <PagerStyle NextPageText="Siguiente &gt;&gt;" PrevPageText="&lt;&lt; Anterior" HorizontalAlign="Right"
                                            Position="Top" CssClass="dgPager" Mode="NumericPages"></PagerStyle>
                                    </asp:DataGrid><asp:Label Style="Z-INDEX: 0" ID="msgRatesPlan" runat="server" CssClass="Validators" Visible="False">No existen Planes Tarifarios</asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="HEIGHT: 22px" colspan="3" align="center">
                                <asp:Label ID="lblMsgActualizacion" runat="server" CssClass="Validators" Visible="False"> Operacion Exitosa</asp:Label></td>
                        </tr>
                        <tr id="renglonBotones" runat="server">
                            <td align="center" colspan="3">
                                <asp:Button ID="btnAceptar" runat="server" EnableViewState="False" CssClass="button" Text="Guardar"></asp:Button>
                                <asp:Button ID="btnCancel" runat="server" EnableViewState="False" CssClass="button" Text="Cancelar"
                                    CausesValidation="False"></asp:Button>
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
