<%@ Import Namespace="RateManager" %>
<%@ Register TagPrefix="uc1" TagName="ctrlDeposits" Src="../Modules/ctrlDeposits.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlPayments" Src="../Modules/ctrlPayments.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlReservationQueryDeposits" Src="../Modules/ctrlReservationQueryDeposits.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Deposits.aspx.vb" Inherits="RateManager.Deposits" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>Deposits</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
    <script src="../../Includes/Script/jquery-3.1.1.min.js"></script>
    <script>
        function ShowOrHide(e) {
            var chk = document.getElementById(e);
            var dv = document.getElementById('divContainerlbl');
            var dv2 = document.getElementById('divContainerddl');
            if (eval(chk)) {
                if (chk.checked) {
                    dv.style.display = '';
                    dv2.style.display = '';
                }
                else {
                    dv.style.display = 'none';
                    dv2.style.display = 'none';
                }
            }

        }

    </script>
</head>
<body>
    <iframe title="x" id="gToday:normal:agenda.js" style="Z-INDEX: 999; LEFT: -500px; POSITION: absolute; TOP: -500px"
        name="gToday:normal:agenda.js"
        src='<%=GeRequestApplicationPath(string.concat("/Calendar/",portalculture.getculture().Name.Substring(0, 2).ToLower(),"/ipopeng2.htm" )) %>'
        frameborder="0" width="174" scrolling="no" height="189"></iframe>
    <form id="Form1" method="post" runat="server">
        <div class="clear">
            <div class="mDiv"></div>
            <div>
                <asp:Label ID="lblTitle" runat="server" EnableViewState="False"
                    Text="Confirmación de Depositos" CssClass="tituloSeccion"></asp:Label>
            </div>
        </div>
        <table id="bookingcontainer" cellspacing="0" cellpadding="2" width="780" border="0">
            <tr>
                <td>
                    <table id="Table1" cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Panel ID="Panel1" runat="server">
                                    <table id="Table11" border="0" cellspacing="0" cellpadding="0" width="100%" align="center">
                                        <tr height="5">
                                            <td colspan="2"></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="center">
                                                <uc1:ctrlReservationQueryDeposits ID="CtrlReservationQueryDeposits1" runat="server"></uc1:ctrlReservationQueryDeposits>
                                            </td>
                                        </tr>
                                        <tr height="5">
                                            <td colspan="2"></td>
                                        </tr>
                                        <caption>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:DataGrid ID="dgReservas" runat="server" AllowPaging="True"
                                                        AutoGenerateColumns="False" CssClass="datagrid" GridLines="None" PageSize="20"
                                                        ShowFooter="True" Width="99%">
                                                        <FooterStyle HorizontalAlign="Right" />
                                                        <AlternatingItemStyle CssClass="dgAlternate" />
                                                        <ItemStyle CssClass="dgItem" />
                                                        <HeaderStyle CssClass="dgHeader" />
                                                        <Columns>
                                                            <asp:TemplateColumn HeaderText="Itinerary">
                                                                <ItemStyle Width="20%" />
                                                                <ItemTemplate>
                                                                    <asp:HyperLink ID="Itinerary" runat="server" CssClass="dglink">
                                                                        <%# DataBinder.Eval(Container, "DataItem.NoReservacion")%>
                                                                    </asp:HyperLink>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn DataField="Nombre" HeaderText="Hotel">
                                                                <ItemStyle Width="25%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="FechaReservacion" HeaderText="Date"></asp:BoundColumn>
                                                            <asp:BoundColumn DataField="Cliente" HeaderText="Customer">
                                                                <ItemStyle Width="25%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="CheckIn" HeaderText="Arrival">
                                                                <ItemStyle Width="10%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="CheckOut" HeaderText="Departure" Visible="False">
                                                                <ItemStyle Width="10%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="Cantidad" HeaderText="Rooms">
                                                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="status" HeaderText="Status" Visible="False"></asp:BoundColumn>
                                                            <asp:TemplateColumn HeaderText="Status" Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblStatus" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn DataField="StatusConf" HeaderText="StatusConf" Visible="False"></asp:BoundColumn>
                                                            <asp:BoundColumn DataField="MotivoCancelacion" HeaderText="MotivoCancelacion"
                                                                Visible="False"></asp:BoundColumn>
                                                            <asp:TemplateColumn HeaderText="confirm">
                                                                <ItemStyle Width="25%" />
                                                                <ItemTemplate>
                                                                    <asp:Button ID="btnRegistrar" runat="server" CommandName="Registrar"
                                                                        CssClass="button" Text="Registrar Deposito" />
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:TemplateColumn HeaderText="cancel">
                                                                <ItemStyle Width="25%" />
                                                                <ItemTemplate>
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:CheckBox ID="chkCancel" runat="server" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lblCancel" runat="server" CssClass="clslabel">
                                                                                    <%# DataBinder.Eval(Container, "DataItem.MotivoCancelacion")%> </asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtMotivoCancelacion" runat="server" Columns="12"
                                                                                    CssClass="textbox" MaxLength="20" size="12" Width="118px"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn DataField="MotivoCancelacion" Visible="False"></asp:BoundColumn>
                                                            <asp:BoundColumn DataField="ID" Visible="False"></asp:BoundColumn>
                                                            <asp:BoundColumn DataField="NoReservacion" Visible="False"></asp:BoundColumn>
                                                            <asp:BoundColumn DataField="IsNetRateUv" Visible="False"></asp:BoundColumn>
                                                        </Columns>
                                                        <PagerStyle CssClass="dgPager" HorizontalAlign="Right" Mode="NumericPages"
                                                            NextPageText="Siguiente &gt;&gt;" Position="Bottom"
                                                            PrevPageText="&lt;&lt; Anterior" />
                                                    </asp:DataGrid>
                                                </td>
                                            </tr>
                                        </caption>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Button ID="btnSave" runat="server" CssClass="button"
                                    EnableViewState="False" Text="Save" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Label ID="lblHelp2" runat="server" EnableViewState="False">Label</asp:Label>
                                <input id="inputReserva" runat="server" type="hidden" />
                            </td>
                        </tr>
                    </table>
                    </asp:panel>
									<asp:Panel ID="Panel2" runat="server" Visible="False">
                                        <uc1:ctrlDeposits ID="CtrlDeposits1" runat="server" Visible="false"></uc1:ctrlDeposits>
                                        <uc1:ctrlPayments ID="pnlPayments" runat="server" Visible="false"></uc1:ctrlPayments>
                                        <br>
                                        <asp:Button ID="btnGuardar" runat="server" OnClientClick="showLoading();" EnableViewState="False" CssClass="button" Text="Save"></asp:Button>
                                        <asp:Button ID="btnCancelar" runat="server" EnableViewState="False" CssClass="button" Text="Cancelar"
                                            CausesValidation="False"></asp:Button>
                                        <div id="loading" style="display:none;">
                                            <img src="../../Includes/imagenes/loader.gif" alt="Espere por favor..." />
                                        </div>
                                    </asp:Panel>
                </td>
            </tr>
        </table>
        </TD>
				</TR>
			</TABLE>
    </form>
    <script>
        function ShowControls(ck, txt, lbl, pmsCode) {
            var e = document.getElementById(ck);
            var l = document.getElementById(lbl);
            var t = document.getElementById(txt);
            if (pmsCode != "&nbsp;") {
                l.firstChild.nodeValue = pmsCode;
                t.value = pmsCode;
            }
            if (e.checked == false) {
                if (l.firstChild) {
                    l.firstChild.nodeValue = "";
                }


                l.style.display = '';
                t.style.display = 'none';

            }
            else {
                t.style.display = '';
                l.style.display = 'none';
            }
        }

        function showLoading() {
            var cuenta = $('#CtrlDeposits1_txtCuenta');
            var banco = $('#CtrlDeposits1_txtBanco');
            var monto = $('#CtrlDeposits1_txtAmountDep');

            if (cuenta[0].value !== "" && banco[0].value !== "" && monto[0].value !== "")
            {
                $('#loading').show();
            }            
        }

        $(document).ready(function () {
            $('#loading').hide();
        });
    
    </script>
</body>
</html>
