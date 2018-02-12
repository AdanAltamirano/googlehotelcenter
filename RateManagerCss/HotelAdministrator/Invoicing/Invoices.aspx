<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Invoices.aspx.vb" Inherits="RateManager.Invoices" %>

<%@ Register TagPrefix="uc1" TagName="ctrHotelsUserChain" Src="../../Modulos/ctrHotelsUserChain.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>Invoices</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link rel="stylesheet" type="text/css" href="../../StyleSheets/Styles.css">
    <style>
        /* The Modal (background) */
        .modal {
            display: none; /* Hidden by default */
            position: fixed; /* Stay in place */
            z-index: 1; /* Sit on top */
            padding-top: 40%; /* Location of the box */
            left: 0;
            top: 0;
            width: 100%; /* Full width */
            height: 100%; /* Full height */
            overflow: auto; /* Enable scroll if needed */
            background-color: rgb(0,0,0); /* Fallback color */
            background-color: rgba(0,0,0,0.4); /* Black w/ opacity */
        }

        /* Modal Content */
        .modal-content {
            position: relative;
            background-color: #fefefe;
            margin: auto;
            padding: 0;
            border: 1px solid #888;
            width: 50%;
            box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2),0 6px 20px 0 rgba(0,0,0,0.19);
            -webkit-animation-name: animatetop;
            -webkit-animation-duration: 0.4s;
            animation-name: animatetop;
            animation-duration: 0.4s;
        }

        /* Add Animation */
        @-webkit-keyframes animatetop {
            from {
                top: -300px;
                opacity: 0;
            }

            to {
                top: 0;
                opacity: 1;
            }
        }

        @keyframes animatetop {
            from {
                top: -300px;
                opacity: 0;
            }

            to {
                top: 0;
                opacity: 1;
            }
        }

        /* The Close Button */
        .close {
            color: white;
            float: right;
            font-size: 28px;
            font-weight: bold;
        }

            .close:hover,
            .close:focus {
                color: #000;
                text-decoration: none;
                cursor: pointer;
            }

        .modal-header {
            padding: 2px 16px;
            background-color: #3E7BAC;
            color: white;
        }

        .modal-body {
            padding: 2px 16px;
        }

        .modal-footer {
            padding: 2px 16px;
            background-color: #3E7BAC;
            color: white;
        }
    </style>
</head>

<body bottommargin="0" leftmargin="0" rightmargin="0" topmargin="5">
    <form id="Form1" method="post" runat="server">
        <div>
            <div class="mDiv">
            </div>
            <div class="title">
                <asp:Label ID="lblTitle" runat="server" EnableViewState="False"
                    CssClass="tituloSeccion"><%=RateManager.PortalCulture.GetString("00881")%> </asp:Label>
            </div>
        </div>
        <table id="bookingcontainer" border="0" cellspacing="0" cellpadding="0" width="100%" align="center">
            <tr>
                <td>
                    <table id="Table1" border="0" cellspacing="0" cellpadding="0" width="100%">

                        <tr>
                            <td colspan="3" align="center">
                                <uc1:ctrHotelsUserChain ID="CtrHotelsUserChain1" runat="server"></uc1:ctrHotelsUserChain>
                            </td>
                        </tr>
                        <tr>
                            <td height="15"></td>
                            <td height="15" align="center"></td>
                            <td height="15"></td>
                        </tr>
                        <tr>
                            <td height="15" width="10">
                                <asp:Image ID="imgEC" runat="server" ImageUrl="~/Includes/imagenes/menos.png"></asp:Image><asp:CheckBox ID="ckbEC" runat="server" Text=" " Height="2px"></asp:CheckBox></td>
                            <td height="15" align="left">
                                <asp:Label ID="lblTituloEstadoCuentaConciliar" runat="server" CssClass="bookingTitleLabel" Style="padding-left: 4px;">Estados de Cuenta a Conciliar</asp:Label></td>
                            <td height="15"></td>
                        </tr>
                        <tr id="trMostrarEC" runat="server">
                            <td height="15" width="10"></td>
                            <td height="15">
                                <table id="Table3" border="0" cellspacing="0" cellpadding="0" width="100%">
                                    <tr>
                                        <td align="left">
                                            <table id="Table4" border="0" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td valign="top" width="280">
                                                        <asp:Label ID="lblOpcion1" runat="server" CssClass="bookingnormallabel">Estados de Cuenta a Conciliar por Periodo</asp:Label></td>
                                                    <td valign="top" width="32"></td>
                                                    <td valign="middle" align="left"></td>
                                                </tr>
                                                <tr>
                                                    <td height="58" valign="top">
                                                        <table id="Table2" border="0" cellspacing="0" cellpadding="0" width="100%">
                                                            <tr>
                                                                <td>&nbsp;&nbsp;
																		<asp:CheckBox ID="ckbPeriodo" runat="server" Text="Periodo"></asp:CheckBox></td>
                                                                <td></td>
                                                            </tr>
                                                            <tr id="trPeriodo" runat="server">
                                                                <td colspan="2">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
																		<asp:DropDownList ID="ddlperiodo" runat="server">
                                                                            <asp:ListItem Value="1">Enero</asp:ListItem>
                                                                            <asp:ListItem Value="2">Febrero</asp:ListItem>
                                                                            <asp:ListItem Value="3">Marzo</asp:ListItem>
                                                                            <asp:ListItem Value="4">Abril</asp:ListItem>
                                                                            <asp:ListItem Value="5">Mayo</asp:ListItem>
                                                                            <asp:ListItem Value="6">Junio</asp:ListItem>
                                                                            <asp:ListItem Value="7">Julio</asp:ListItem>
                                                                            <asp:ListItem Value="8">Agosto</asp:ListItem>
                                                                            <asp:ListItem Value="9">Septiembre</asp:ListItem>
                                                                            <asp:ListItem Value="10">Octubre</asp:ListItem>
                                                                            <asp:ListItem Value="11">Noviembre</asp:ListItem>
                                                                            <asp:ListItem Value="12">Diciembre</asp:ListItem>
                                                                        </asp:DropDownList>&nbsp;
																		<asp:DropDownList ID="ddlyear" runat="server">
                                                                            <asp:ListItem Value="2006">2006</asp:ListItem>
                                                                            <asp:ListItem Value="2007">2007</asp:ListItem>
                                                                            <asp:ListItem Value="2008">2008</asp:ListItem>
                                                                            <asp:ListItem Value="2009">2009</asp:ListItem>
                                                                        </asp:DropDownList></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td height="58" valign="top"></td>
                                                    <td height="58" valign="middle" align="left">
                                                        <asp:Button ID="btnload" runat="server" CssClass="button" Text="Mostrar"></asp:Button></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblInfoEstadoCuentaConciliar" runat="server" CssClass="bookingNormalLabel">No hay Estado de Cuenta por Conciliar</asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td align="left"></td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:DataGrid ID="dgCuentasConciliables" runat="server" CssClass="DataGrid" AutoGenerateColumns="False"
                                                Width="100%" AllowPaging="True" BorderColor="WhiteSmoke">
                                                <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
                                                <ItemStyle CssClass="dgitem"></ItemStyle>
                                                <HeaderStyle CssClass="DGHeader"></HeaderStyle>
                                                <Columns>
                                                    <asp:TemplateColumn>
                                                        <ItemStyle Width="25px"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblNumeracionEC" runat="server" CssClass="bookingNormalLabel"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Estado Cuenta">
                                                        <HeaderStyle></HeaderStyle>
                                                        <ItemStyle Width="150px"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:HyperLink ID="lnkCuentaEC" runat="server" CssClass="DGLink"></asp:HyperLink>
                                                            <asp:Label ID="lblCuentaEC" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Fecha Cuenta">
                                                        <HeaderStyle></HeaderStyle>
                                                        <ItemStyle Width="180px"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFechaCuentaEC" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Referencia Bancaria">
                                                        <ItemStyle Width="150px"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblReferenciaBancariaEC" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Limite para Conciliar">
                                                        <ItemStyle Width="155px"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFechaLimiteConciliarEC" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Conciliar">
                                                        <HeaderStyle></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center" Width="85px"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:HyperLink ID="lnkConciliarEC" runat="server" CssClass="DGLink"></asp:HyperLink>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Importe">
                                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblImporteEC" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                </Columns>
                                                <PagerStyle HorizontalAlign="Right" CssClass="dgpager" Mode="NumericPages"></PagerStyle>
                                            </asp:DataGrid></td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblTotalEC" runat="server" CssClass="bookingNormalLabel"></asp:Label></td>
                                    </tr>
                                </table>
                            </td>
                            <td height="15"></td>
                        </tr>
                        <tr>
                            <td height="30" width="10"></td>
                            <td height="30"></td>
                            <td height="30"></td>
                        </tr>
                        <tr>
                            <td height="15" width="12">
                                <asp:Image ID="imgF" runat="server" ImageUrl="~/Includes/imagenes/menos.png"></asp:Image><asp:CheckBox ID="ckbF" runat="server" Text=" " Height="2px"></asp:CheckBox></td>
                            <td height="15" align="left">
                                <asp:Label ID="lblSubTitulo1" runat="server" CssClass="bookingTitleLabel" Style="padding-left: 4px;">Facturas</asp:Label></td>
                            <td height="15"></td>
                        </tr>
                        <tr id="trMostrarF" runat="server">
                            <td height="15" width="10"></td>
                            <td height="15" align="center">
                                <table id="Table5" border="0" cellspacing="0" cellpadding="0" width="100%">
                                    <tr>
                                        <td height="15" align="center">
                                            <p align="left">
                                                <table id="Table7" border="0" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td width="280">
                                                            <asp:Label ID="lblOpcion2" runat="server" CssClass="bookingnormallabel">Tipo de Factura</asp:Label></td>
                                                        <td style="WIDTH: 32px" width="32">
                                                            <p>&nbsp;</p>
                                                        </td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td height="50" valign="top">
                                                            <table id="Table8" border="0" cellspacing="0" cellpadding="0" width="100%">
                                                                <tr>
                                                                    <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
																			<asp:DropDownList ID="ddlTipo" runat="server" Width="192px">
                                                                                <asp:ListItem Value="0">Todas</asp:ListItem>
                                                                                <asp:ListItem Value="1">Activas</asp:ListItem>
                                                                                <asp:ListItem Value="2">Activas - Pendiente Pago</asp:ListItem>
                                                                                <asp:ListItem Value="3">Canceladas</asp:ListItem>
                                                                            </asp:DropDownList>&nbsp;</td>
                                                                    <td></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td height="50" width="32"></td>
                                                        <td height="50" valign="middle" align="left">
                                                            <asp:Button ID="btnMostrarFactura" runat="server" CssClass="button" Text="Mostrar"></asp:Button></td>
                                                    </tr>
                                                </table>
                                            </p>
                                        </td>
                                    </tr>
                                    <tr id="trFDInfo" runat="server">
                                        <td height="15" align="center">
                                            <asp:Label ID="lblInfoFacturaDigital" runat="server" CssClass="Validators">No hay Facturas Digitales</asp:Label></td>
                                    </tr>
                                    <tr id="trFDTitulo" runat="server">
                                        <td height="15" align="left">
                                            <asp:Label ID="lblTituloFacturaDigital" runat="server" CssClass="bookingTitleLabel">Facturas Digitales</asp:Label></td>
                                    </tr>
                                    <tr id="trFDDatos" runat="server">
                                        <td align="center">
                                            <asp:DataGrid ID="dgFacturasDigitales" runat="server" CssClass="DataGrid" AutoGenerateColumns="False"
                                                Width="100%" AllowPaging="True" BorderColor="WhiteSmoke">
                                                <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
                                                <ItemStyle CssClass="dgitem"></ItemStyle>
                                                <HeaderStyle CssClass="DGHeader"></HeaderStyle>
                                                <Columns>
                                                    <asp:TemplateColumn>
                                                        <ItemStyle Width="25px"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblNumeracion" runat="server" CssClass="bookingNormalLabel"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Estado&lt;br&gt;Cuenta">
                                                        <HeaderStyle></HeaderStyle>
                                                        <ItemStyle Width="75px"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:HyperLink ID="lnkCuenta" runat="server" CssClass="DGLink"></asp:HyperLink>
                                                            <asp:Label ID="lblCuenta" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Fecha Cuenta">
                                                        <HeaderStyle></HeaderStyle>
                                                        <ItemStyle Width="130px"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFechaCuenta" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Factura">
                                                        <ItemStyle Width="65px"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFactura" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Fecha Factura">
                                                        <ItemStyle Width="120px"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFechaFactura" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Referencia &lt;br&gt; Bancaria">
                                                        <ItemStyle Width="97px"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblReferenciaBancaria" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Pendiente &lt;br&gt; Pago">
                                                        <HeaderStyle></HeaderStyle>
                                                        <ItemStyle Width="75px"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:HyperLink ID="lnkVerDetalle" runat="server" CssClass="DGLink"></asp:HyperLink>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Descargar">
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        <ItemTemplate>
                                                            <div style="position: relative; float: left; width: 120px; display: inline; display: none;">
                                                                <asp:HyperLink ID="hypPdf" runat="server" ImageUrl="../../Images/pdf.gif"></asp:HyperLink>
                                                                <asp:HyperLink ID="hypXml" runat="server" ImageUrl="../../Images/xml.gif"></asp:HyperLink>
                                                                <asp:HyperLink ID="hypExc" runat="server" ImageUrl="../../Images/excel.png"></asp:HyperLink>
                                                                <br />
                                                                <asp:Label ID="lblMsgSelloSat" runat="server" Text=""></asp:Label>
                                                            </div>
                                                            <linkbutton id="lknDescarga" onclick="openModal()" class="dgLink">Descargar</linkbutton>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Debe">
                                                        <ItemStyle HorizontalAlign="Right" Width="95px"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStatus" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Importe">
                                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblImporte" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                </Columns>
                                                <PagerStyle HorizontalAlign="Right" CssClass="dgpager" Mode="NumericPages"></PagerStyle>
                                            </asp:DataGrid></td>
                                    </tr>
                                    <tr id="trFDTotal" runat="server">
                                        <td height="15" align="right">
                                            <asp:Label ID="lblTotalFD" runat="server" CssClass="bookingNormalLabel"></asp:Label></td>
                                    </tr>
                                    <tr id="trFDTotalDebe" runat="server">
                                        <td height="15" align="right">
                                            <asp:Label ID="lblTotalDebeFD" runat="server" CssClass="bookingNormalLabel"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td height="15" align="center"></td>
                                    </tr>
                                    <tr id="trFMInfo" runat="server">
                                        <td align="center">
                                            <asp:Label ID="lblInfoFacturaManual" runat="server" CssClass="Validators">No hay Facturas Manuales</asp:Label></td>
                                    </tr>
                                    <tr id="trFMTitulo" runat="server">
                                        <td height="15" align="left">
                                            <asp:Label ID="lblTituloFacturaManual" runat="server" CssClass="bookingTitleLabel">Facturas Manuales</asp:Label></td>
                                    </tr>
                                    <tr id="trFMDatos" runat="server">
                                        <td height="15" align="center">
                                            <asp:DataGrid ID="dgFacturasManuales" runat="server" CssClass="DataGrid" AutoGenerateColumns="False"
                                                Width="100%" AllowPaging="True" BorderColor="WhiteSmoke">
                                                <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
                                                <ItemStyle CssClass="dgitem"></ItemStyle>
                                                <HeaderStyle CssClass="DGHeader"></HeaderStyle>
                                                <Columns>
                                                    <asp:TemplateColumn>
                                                        <ItemStyle Width="25px"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblNumeracionFD" runat="server" CssClass="bookingNormalLabel"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Estado&lt;br&gt;Cuenta">
                                                        <HeaderStyle></HeaderStyle>
                                                        <ItemStyle Width="80px"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:HyperLink ID="lnkCuentaFM" runat="server" CssClass="DGLink"></asp:HyperLink>
                                                            <asp:Label ID="lblCuentaFM" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Fecha Cuenta">
                                                        <HeaderStyle></HeaderStyle>
                                                        <ItemStyle Width="160px"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFechaCuentaFM" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Factura">
                                                        <ItemStyle Width="70px"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFacturaFM" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Fecha Factura">
                                                        <ItemStyle Width="130px"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFechaFacturaFM" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Referencia  Bancaria">
                                                        <ItemStyle Width="105px"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblReferenciaBancariaFM" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Pendiente  Pago">
                                                        <HeaderStyle></HeaderStyle>
                                                        <ItemStyle Width="75px"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:HyperLink ID="lnkVerDetalleFM" runat="server" CssClass="DGLink"></asp:HyperLink>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Debe">
                                                        <ItemStyle Width="100px" HorizontalAlign="Right"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStatusFM" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Importe">
                                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblImporteFM" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                </Columns>
                                                <PagerStyle HorizontalAlign="Right" CssClass="dgpager" Mode="NumericPages"></PagerStyle>
                                            </asp:DataGrid></td>
                                    </tr>
                                    <tr id="trFMTotal" runat="server">
                                        <td height="15" align="right">
                                            <asp:Label ID="lblTotalFM" runat="server" CssClass="bookingNormalLabel"></asp:Label></td>
                                    </tr>
                                    <tr id="trFMTotalDebe" runat="server">
                                        <td height="15" align="right">
                                            <asp:Label ID="lblTotalDebeFM" runat="server" CssClass="bookingNormalLabel"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td height="15" align="right"></td>
                                    </tr>
                                    <tr id="trFCInfo" runat="server">
                                        <td height="15" align="center">
                                            <asp:Label ID="lblInfoFacturaCancelada" runat="server" CssClass="Validators">No hay Facturas Canceladas</asp:Label></td>
                                    </tr>
                                    <tr id="trFCTitulo" runat="server">
                                        <td height="15" align="left">
                                            <asp:Label ID="lblTituloFacturaCancelada" runat="server" CssClass="bookingTitleLabel">Facturas Canceladas</asp:Label></td>
                                    </tr>
                                    <tr id="trFCDatos" runat="server">
                                        <td height="15" align="center">
                                            <asp:DataGrid ID="dgFacturasCanceladas" runat="server" CssClass="DataGrid" AutoGenerateColumns="False"
                                                Width="100%" AllowPaging="True" BorderColor="WhiteSmoke">
                                                <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
                                                <ItemStyle CssClass="dgitem"></ItemStyle>
                                                <HeaderStyle CssClass="DGHeader"></HeaderStyle>
                                                <Columns>
                                                    <asp:TemplateColumn>
                                                        <ItemStyle Width="25px"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblNumeracionFC" runat="server" CssClass="bookingNormalLabel"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Estado&lt;br&gt;Cuenta">
                                                        <HeaderStyle></HeaderStyle>
                                                        <ItemStyle Width="75px"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:HyperLink ID="lnkCuentaFC" runat="server" CssClass="DGLink"></asp:HyperLink>
                                                            <asp:Label ID="lblCuentaFC" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Fecha Cuenta">
                                                        <HeaderStyle></HeaderStyle>
                                                        <ItemStyle Width="130px"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFechaCuentaFC" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Factura">
                                                        <ItemStyle Width="65px"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFacturaFC" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Fecha Factura">
                                                        <ItemStyle Width="120px"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFechaFacturaFC" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Referencia &lt;br&gt; Bancaria">
                                                        <ItemStyle Width="105px"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblReferenciaBancariaFC" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Fecha Cancelaci&#243;n">
                                                        <ItemStyle Width="120px"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFechaCancelacionFC" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Motivo Cancelaci&#243;n">
                                                        <ItemStyle Width="145px"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblMotivoCancelacionFC" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Descargar" HeaderStyle-Width="80px">
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        <ItemTemplate>
                                                            <div style="position: relative; float: left; width: 120px; display: inline;display:none;">
                                                                <asp:HyperLink ID="hypPdfFC" runat="server" ImageUrl="../../Images/pdf.gif"></asp:HyperLink>
                                                                <asp:HyperLink ID="hypXmlFC" runat="server" ImageUrl="../../Images/xml.gif"></asp:HyperLink>
                                                                <asp:HyperLink ID="hypExcFC" runat="server" ImageUrl="../../Images/excel.png"></asp:HyperLink>
                                                            </div>
                                                            <linkbutton onclick="openModal();" class="dgLink">Descargar</linkbutton>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                </Columns>
                                                <PagerStyle HorizontalAlign="Right" CssClass="dgpager" Mode="NumericPages"></PagerStyle>
                                            </asp:DataGrid></td>
                                    </tr>
                                </table>
                            </td>
                            <td height="15"></td>
                        </tr>
                        <tr>
                            <td height="15"></td>
                            <td height="15" align="right"></td>
                            <td height="15"></td>
                        </tr>
                        <tr>
                            <td height="20"></td>
                            <td height="20" align="center"></td>
                            <td height="20"></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td></td>
            </tr>
        </table>
        <script>
            function ShowOrHide(e) {
                var chk = document.getElementById(e);
                var dv = document.getElementById('trPeriodo');
                if (chk.checked) {
                    dv.style.display = 'block';
                }
                else {
                    dv.style.display = 'none';
                }

            }

            function showMessage() {
                alert("Por el momento la descarga no está disponible. Favor de comunicarse al área de cobranza para obtener su factura.");
            }

            function expandOrCollapse(expanderElementId, expandableElementId, checkBox) {
                var expander = document.getElementById(expanderElementId);
                var e = document.getElementById(expandableElementId);
                var ckb = document.getElementById(checkBox);

                if (e) {
                    e.style.display = (expander.src.indexOf("mas.png") != -1) ? "block" : "none";
                }

                ckb.checked = true
                if (ckb) {
                    if (expander.src.indexOf("mas.png") != -1) {
                        ckb.checked = false;
                    }
                    ckb.style.display = "none";
                }

                if (expander) {
                    if (expander.src) {
                        expander.src = (expander.src.indexOf("mas.png") != -1) ? expander.src.replace("mas.png", "menos.png") : expander.src.replace("menos.png", "mas.png");
                    }
                }
            }
        </script>
    </form>
        <!-- The Modal -->
    <div id="myModal" class="modal">

        <!-- Modal content -->
        <div class="modal-content">
            <div class="modal-header">
                <span class="close" onclick="closeModal()">&times;</span>
                <h2>Aviso</h2>
            </div>
            <div class="modal-body">
                <h4 class="msg">Por el momento la descarga no está disponible.</h4>
                <h4 class="msg">Favor de comunicarse al área de cobranza para obtener su factura.</h4>
            </div>
            <div class="modal-footer">
                <input type="button" value="Cerrar" onclick="closeModal();" />
            </div>
        </div>

    </div>
    <script type="text/javascript">
        // Get the modal
        var modal = document.getElementById('myModal');

        // Get the <span> element that closes the modal
        var span = document.getElementsByClassName("close")[0];

        // When the user clicks the button, open the modal 
        var openModal = function () {
            modal.style.display = "block";
            
            $(".modal-body .msg").text("Por el momento la descarga no está disponible.\n Favor de comunicarse al área de cobranza para obtener su factura.");
            $(".modal-header h2").text("Aviso");
        }

        // When the user clicks on <span> (x), close the modal
        var closeModal = function () {
            modal.style.display = "none";
        }

        // When the user clicks anywhere outside of the modal, close it
        window.onclick = function (event) {
            if (event.target == modal) {
                modal.style.display = "none";
            }
        }

        var deactivatePromo = function () {
            var id = document.getElementById('txtObjDelete').value;
            document.getElementById(id).click();
        }
    </script>
</body>
</html>
