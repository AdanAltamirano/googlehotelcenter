<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LogReport.aspx.vb" Inherits="RateManager.LogReport" %>

<%@ Import Namespace="RateManager" %>
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Bitácora de Sistema</title>
    <link href="../../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
    <script type="text/javascript" src="../../Includes/Script/jquery-1.4.2.min.js"></script>
<style>
        /* ══════════════════════════════════════════════
           Diseño Final: Azul Corporativo y Centrado
           ══════════════════════════════════════════════ */

        body {
            background: #f4f6f9 !important;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            color: #2d3748;
        }

        /* Título de la sección */
        .tituloSeccion {
            color: #2c5282 !important;
            font-size: 20px;
            font-weight: 800 !important;
            margin: 20px 0;
            display: block;
        }

        /* ── Barra de Filtros ── */
        .filter-bar {
            background: #ffffff;
            border: 1px solid #e2e8f0;
            border-radius: 10px;
            padding: 16px 20px;
            margin-bottom: 20px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.02);
        }
        .filter-bar label {
            font-weight: 800;
            color: #2c5282;
            font-size: 11px;
            text-transform: uppercase;
            letter-spacing: 0.8px;
        }
        .filter-bar input[type=text], .filter-bar select {
            border: 1px solid #cbd5e0 !important;
            border-radius: 6px !important;
            padding: 6px 10px !important;
            font-size: 13px !important;
            font-weight: 600;
        }

        /* ── BLOQUE NOMBRE DEL HOTEL (Header Principal) ── */
        table.dgHeader {
            background: #2c5282 !important;
            color: #ffffff !important;
            border-radius: 10px 10px 0 0;
            width: 100% !important;
            margin-top: 25px;
            border: none !important;
        }
        table.dgHeader td {
            color: #ffffff !important;
            padding: 14px 18px !important;
            font-weight: 700;
            font-size: 16px;
        }

        /* ── TABLA DE DATOS ── */
        .table-wrapper {
            background: #ffffff;
            border: 1px solid #e2e8f0;
            border-top: none;
            border-bottom-left-radius: 10px;
            border-bottom-right-radius: 10px;
            overflow: hidden;
            box-shadow: 0 4px 6px rgba(0,0,0,0.05);
        }
        table.DataGrid {
            width: 100% !important;
            border-collapse: collapse !important;
        }

        /* Cabecera de Columnas con Línea Azul Fuerte */
        table.DataGrid tr.dgHeader th,
        table.DataGrid tr.dgHeader td {
            background: #ffffff !important;
            color: #2c5282 !important;
            text-transform: uppercase;
            font-size: 12px !important;
            padding: 12px 10px !important;
            font-weight: 900 !important;
            border-bottom: 2px solid #2c5282 !important; /* LÍNEA AZUL PARA TÍTULOS */
            text-align: left;
        }

        /* Contenido con Línea Azul Sutil */
        table.DataGrid tr.dgItem td,
        table.DataGrid tr.dgAlternate td {
            padding: 12px 10px !important;
            color: #4a5568 !important;
            border-bottom: 1px solid #d1dceb !important; /* LÍNEA AZUL PARA REGISTROS */
            font-size: 13px;
            font-weight: 600;
            vertical-align: middle;
        }
        table.DataGrid tr.dgItem { background: #ffffff !important; }
        table.DataGrid tr.dgAlternate { background: #f8fafc !important; }

        table.DataGrid tr.dgItem:hover,
        table.DataGrid tr.dgAlternate:hover {
            background: #f1f7ff !important;
            transition: background 0.2s ease;
        }

        /* ── BOTONES DE ACCIÓN (Escala Pequeña) ── */
        .detail-link, 
        table.DataGrid a:not(.badge) {
            display: inline-block;
            padding: 3px 8px !important;
            background-color: #ffffff !important;
            color: #2c5282 !important;
            border: 1px solid #2c5282 !important;
            border-radius: 4px !important;
            font-size: 10px !important;
            font-weight: 800 !important;
            text-decoration: none !important;
            text-transform: uppercase;
            letter-spacing: 0.5px;
            transition: all 0.2s ease;
            text-align: center;
        }

        .detail-link:hover, 
        table.DataGrid a:not(.badge):hover {
            background-color: #2c5282 !important;
            color: #ffffff !important;
        }

        /* ── BADGES Y CONTADORES ── */
        .badge {
            display: inline-block;
            padding: 4px 10px;
            border-radius: 6px;
            font-size: 10px;
            font-weight: 800;
            color: #fff;
            text-transform: uppercase;
        }
        .badge-login  { background: #3182ce; }
        .badge-create { background: #38a169; }
        .badge-edit   { background: #dd6b20; }
        .badge-delete { background: #e53e3e; }

        .record-count {
            background: #ebf4ff;
            color: #2a4365;
            padding: 12px 18px;
            border-radius: 8px;
            font-size: 13px;
            font-weight: 700;
            margin-bottom: 15px;
            border: 1px solid #bee3f8;
        }

        /* ── PAGINACIÓN ── */
        .pager-bar {
            padding: 18px;
            text-align: center;
            background: #ffffff;
            border-top: 2px solid #edf2f7;
        }
        .pager-bar a {
            display: inline-block;
            padding: 6px 12px;
            margin: 0 3px;
            border: 1px solid #e2e8f0;
            border-radius: 6px;
            color: #4a5568;
            font-size: 13px;
            font-weight: 700;
            text-decoration: none;
        }
        .pager-bar a.pager-active {
            background: #2c5282;
            color: #ffffff;
            border-color: #2c5282;
        }

        /* ── Loading Spinner (Centrado Perfecto Fijo) ── */
        #loadingProcess {
            display: none;
            position: fixed;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            z-index: 9999;
            width: 180px;
            padding: 15px 20px;
            color: #2c5282;
            background: #ffffff;
            border: 1px solid #cbd5e0;
            border-radius: 50px;
            box-shadow: 0 10px 25px rgba(44, 82, 130, 0.25);
            font-weight: 700;
            text-align: center;
            font-size: 15px;
        }
        .spinner {
            display: inline-block;
            width: 22px;
            height: 22px;
            border: 3px solid #d0e2f0;
            border-top-color: #2b6cb0;
            border-radius: 50%;
            animation: spin 0.75s linear infinite;
            vertical-align: middle;
            margin-right: 10px;
        }
        @keyframes spin { to { transform: rotate(360deg); } }
    </style>
</head>
<body bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0">
    <%-- Calendar popup iframe (required by existing date picker) --%>
    <iframe id="gToday:normal:agenda.js"
        style="z-index:999;left:-500px;position:absolute;top:-500px"
        name="gToday:normal:agenda.js"
        src='<%=GeRequestApplicationPath(String.Concat("/Calendar/", portalculture.getculture().Name.Substring(0, 2).ToLower(), "/ipopeng2.htm"))%>'
        frameborder="0" width="174" scrolling="no" height="189"></iframe>

    <form id="Form1" method="post" runat="server">

        <%-- Page title --%>
        <div class="clear">
            <div class="title">
                <asp:Label ID="lbltitle" runat="server" EnableViewState="False" CssClass="tituloSeccion" Text="Bitácora de sistema"></asp:Label>
            </div>
        </div>

        <%-- Filter bar --%>
        <div class="filter-bar">
            <table>
                <tr>
                    <td><label><asp:Label ID="lblInicio" runat="server" EnableViewState="False">Desde</asp:Label></label></td>
                    <td>
                        <a hidefocus onclick="if(self.gfPop)gfPop.fPopCalendar(document.getElementById('txtFinal'),document.getElementById('txtInicio'));return false;" href="javascript:void(0)">
                            <asp:TextBox ID="txtInicio" runat="server" CssClass="textbox" MaxLength="10" Width="84px" Columns="10"></asp:TextBox>
                            <img class="PopcalTrigger" alt="" src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>' align="absMiddle" border="0">
                        </a>
                    </td>
                    <td><label><asp:Label ID="lblFinal" runat="server" EnableViewState="False">Hasta</asp:Label></label></td>
                    <td>
                        <a hidefocus onclick="if(self.gfPop)gfPop.fPopCalendar1(document.getElementById('txtFinal'));return false;" href="javascript:void(0)">
                            <asp:TextBox ID="txtFinal" runat="server" CssClass="textbox" MaxLength="10" Width="84px" Columns="10"></asp:TextBox>
                            <img class="PopcalTrigger" alt="" src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>' align="absMiddle" border="0">
                        </a>
                    </td>
                    <td><label><asp:Label ID="lblHotel" runat="server" EnableViewState="False">Hotel:</asp:Label></label></td>
                    <td><asp:DropDownList ID="ddlHoteles" runat="server"></asp:DropDownList></td>
                    <td><label><asp:Label ID="lblAccion" runat="server" EnableViewState="False">Acción:</asp:Label></label></td>
                    <td><asp:DropDownList ID="ddlAccion" runat="server"></asp:DropDownList></td>
                    <td><label><asp:Label ID="lblUsuario" runat="server" EnableViewState="False">Usuario:</asp:Label></label></td>
                    <td><asp:TextBox ID="txtUsuario" runat="server" CssClass="textbox" Width="150px" MaxLength="100"></asp:TextBox></td>
                    <td>
                        <asp:Button ID="btnSearch" runat="server" EnableViewState="False" CssClass="button" Text="Buscar"></asp:Button>
                    </td>
                </tr>
            </table>
        </div>

        <%-- Loading indicator --%>
        <div id="loadingProcess">
            <span class="spinner"></span>Cargando registros&hellip;
        </div>

        <%-- Record count --%>
        <div class="record-count">
            <asp:Label ID="lblTotal" runat="server" EnableViewState="False"></asp:Label>
        </div>

        <%-- Results --%>
        <div class="table-wrapper">
            <asp:DataList ID="dlHoteles" runat="server" EnableViewState="true" Width="100%" HorizontalAlign="LEFT">
                <ItemTemplate>
                    <table class="dgHeader" id="tshow" width="100%" border="0" runat="server">
                        <tr>
                            <td id="tdshow" style="cursor:pointer;width:20px;text-align:center" runat="server">-</td>
                            <td class="clsHelpLabel"><%# DataBinder.Eval(Container, "DataItem.Nombre") %></td>
                        </tr>
                    </table>
                    <asp:DataGrid ID="dgLog" runat="server" EnableViewState="false" Width="100%"
                        AllowPaging="false" AutoGenerateColumns="False" CssClass="DataGrid">
                        <AlternatingItemStyle CssClass="dgAlternate"></AlternatingItemStyle>
                        <ItemStyle CssClass="dgItem"></ItemStyle>
                        <HeaderStyle CssClass="dgHeader"></HeaderStyle>
                        <Columns>
                            <asp:BoundColumn HeaderText="Usuario"    DataField="Usuario"></asp:BoundColumn>
                            <asp:TemplateColumn>
                                <ItemTemplate>
                                    <asp:HyperLink ID="hplPagina" runat="server"></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn HeaderText="Pagina"    DataField="Pagina"  Visible="False"></asp:BoundColumn>
                            <asp:BoundColumn HeaderText="Accion"    DataField="Accion"></asp:BoundColumn>
                            <asp:BoundColumn HeaderText="Fecha"     DataField="Fecha"></asp:BoundColumn>
                            <asp:BoundColumn HeaderText="Hora"      DataField="Fecha"></asp:BoundColumn>
                            <asp:BoundColumn HeaderText="Nota"      DataField="Nota"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Ver Detalles">
                                <ItemTemplate>
                                    <asp:HyperLink ID="lnkDetalle" runat="server"></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn HeaderText="idlog"     DataField="idlog"   Visible="false"></asp:BoundColumn>
                            <asp:BoundColumn HeaderText="link"      DataField="link"    Visible="False"></asp:BoundColumn>
                        </Columns>
                    </asp:DataGrid>
                </ItemTemplate>
            </asp:DataList>
        </div>

    </form>

    <script type="text/javascript">
        $(function () {
            $('#loadingProcess').hide();
            $('table.DataGrid').each(function () {
                initGridPagination($(this), 30);
            });
        });

        function FireUpdateStatus() {
            $('#loadingProcess').show();
            return true;
        }

        function showHotel(idgrid, idtd) {
            var dg = document.getElementById(idgrid);
            if (dg) {
                if (dg.style.display === 'none') {
                    dg.style.display = '';
                    document.getElementById(idtd).innerHTML = '-';
                } else {
                    dg.style.display = 'none';
                    document.getElementById(idtd).innerHTML = '+';
                }
            }
        }

        function initGridPagination($grid, pageSize) {
            var $rows = $grid.find('tr.dgItem, tr.dgAlternate');
            if ($rows.length <= pageSize) return;

            var totalRows = $rows.length;
            var totalPages = Math.ceil(totalRows / pageSize);
            var currentPage = 1;

            var $pager = $('<div class="pager-bar"></div>');
            $grid.after($pager);

            function showPage(page) {
                currentPage = page;
                $rows.each(function (i) {
                    if (i >= (page - 1) * pageSize && i < page * pageSize) {
                        $(this).show();
                    } else {
                        $(this).hide();
                    }
                });
                renderPager();
            }

            function renderPager() {
                $pager.empty();
                var start = (currentPage - 1) * pageSize + 1;
                var end = Math.min(currentPage * pageSize, totalRows);
                $pager.append('<span class="pager-info">' + start + '-' + end + ' de ' + totalRows + '</span>');

                if (currentPage > 1) {
                    (function (p) {
                        $('<a href="javascript:void(0)">\u2039 Ant</a>').click(function () { showPage(p); }).appendTo($pager);
                    })(currentPage - 1);
                }

                var startP = Math.max(1, currentPage - 3);
                var endP   = Math.min(totalPages, startP + 6);
                startP = Math.max(1, endP - 6);

                for (var p = startP; p <= endP; p++) {
                    (function (n) {
                        var $btn = $('<a href="javascript:void(0)">' + n + '</a>');
                        if (n === currentPage) $btn.addClass('pager-active');
                        $btn.click(function () { showPage(n); }).appendTo($pager);
                    })(p);
                }

                if (currentPage < totalPages) {
                    (function (p) {
                        $('<a href="javascript:void(0)">Sig \u203a</a>').click(function () { showPage(p); }).appendTo($pager);
                    })(currentPage + 1);
                }
            }

            showPage(1);
        }
    </script>
</body>
</html>
