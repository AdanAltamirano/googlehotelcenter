<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RetentionsConfig.aspx.vb" Inherits="RateManager.RetentionsConfig" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Internet Power</title>
    <!--<link href="css/notification-styles.min.css" type="text/css" rel="stylesheet" />-->
    <!--<link href="../../Includes/Retention/notification-styles.css" rel="stylesheet" />-->
    <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <link rel="stylesheet" href="/resources/demos/style.css">
    <script type="text/javascript" src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script type="text/javascript" src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <link href="../../Includes/Retention/bootstrap.css" rel="stylesheet" />
    <link href="../../Includes/estilos.css" rel="stylesheet" />
    <!--<script type="text/javascript" src="../../Includes/Script/jquery-3.1.1.min.js"></script>-->
    <style type="text/css">
        .row {
            vertical-align: central;
        }

        .align-right {
            text-align: right;
        }
    </style>
    <style>
        /* The Modal (background) */
        .modal {
            display: none; /* Hidden by default */
            position: fixed; /* Stay in place */
            z-index: 1; /* Sit on top */
            padding-top: 100px; /* Location of the box */
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
<body>
    <input type="hidden" runat="server" id="txtObjDelete" />
    <form id="form1" runat="server">
        <h2 class="text-center text-uppercase">
            <asp:Label runat="server" ID="lblPropertyNumber"></asp:Label>
        </h2>
        <div class="container">
            <div class="row">
                <div class="col-sm-3">
                    <asp:Label ID="lblToolEnabled" runat="server" Text="Herramienta habilitada"></asp:Label>
                </div>
                <div class="col-sm-4">
                    <asp:CheckBox ID="chkHabilitado" runat="server" AutoPostBack="True" OnCheckedChanged="chkHabilitado_CheckedChanged"></asp:CheckBox>
                </div>
            </div>
        </div>
        <div runat="server" id="toolDiv" class="container">
            <div class="well well-lg">
                <div class="row">
                    <div class="col-sm-3">
                        <asp:Label ID="Label2" runat="server" Text="URL del Sitio"></asp:Label>
                    </div>
                    <div class="col-sm-3">
                        <asp:TextBox ID="txtUrl" runat="server" Width="250px"></asp:TextBox>                        
                    </div>
                    <div class="col-sm-1">
                        <asp:Button runat="server" ID="btnUpdateURL" CssClass="button" Text="Actualizar" OnClick="btnUpdateURL_Click"/>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-3">
                        <asp:Label ID="Label3" runat="server" Text="Mostrar comparador"></asp:Label>
                    </div>
                    <div class="col-sm-4">
                        <asp:CheckBox ID="chkComparador" runat="server" AutoPostBack="True" OnCheckedChanged="chkComparador_CheckedChanged"></asp:CheckBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-3">
                        <asp:Label ID="Label4" runat="server" Text="Mostrar mensajes de notificaci&oacute;n"></asp:Label>
                    </div>
                    <div class="col-sm-4">
                        <asp:CheckBox ID="chkMensajes" runat="server" AutoPostBack="True" OnCheckedChanged="chkMensajes_CheckedChanged"></asp:CheckBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-3">
                        <asp:Label ID="Label5" runat="server" Text="Mostrar mensajes de retencion (salida)"></asp:Label>
                    </div>
                    <div class="col-sm-4">
                        <asp:CheckBox ID="chkRetencion" runat="server" AutoPostBack="True" OnCheckedChanged="chkRetencion_CheckedChanged"></asp:CheckBox>
                    </div>
                </div>
            </div>
            <br />
            <h2>Mensajes Personalizados</h2>
            <asp:GridView CssClass="datagrid" runat="server" ID="gridMessages"
                DataKeyNames="id_mensaje" AllowSorting="True" AutoGenerateColumns="False" OnRowDeleting="gridMessages_OnRowDeleting">
                <AlternatingRowStyle CssClass="dgAlternate" />
                <SelectedRowStyle CssClass="dgSelected" />
                <HeaderStyle CssClass="dgHeader" HorizontalAlign="Center"></HeaderStyle>
                <Columns>
                    <asp:BoundField DataField="icono" HeaderText="Icono" ItemStyle-Width="5%" ItemStyle-VerticalAlign="Middle"/>
                    <asp:BoundField DataField="mensaje" HeaderText="Mensaje" ItemStyle-Width="20%" ItemStyle-VerticalAlign="Middle"  />
                    <asp:BoundField DataField="fecha_inicio" HeaderText="Inicio" ItemStyle-Width="5%" ItemStyle-VerticalAlign="Middle" />
                    <asp:BoundField DataField="fecha_fin" HeaderText="Fin" ItemStyle-Width="5%"  ItemStyle-VerticalAlign="Middle" />
                    <asp:BoundField DataField="orden" HeaderText="Orden" ItemStyle-Width="5%" ItemStyle-VerticalAlign="Middle"  />
                    <asp:BoundField DataField="rutas" HeaderText="Rutas" ItemStyle-Width="15%" ItemStyle-VerticalAlign="Middle"  />
                    <asp:BoundField DataField="duracion" HeaderText="Duracion (Segundos)" ItemStyle-Width="8%" ItemStyle-VerticalAlign="Middle"/>
                    <asp:TemplateField ItemStyle-Width="4%">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkedit" runat="server" CssClass="dgLink" CausesValidation="False"
                                CommandName="Select">Editar</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="4%">
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEliminar2" Style="display: none" runat="server" CssClass="dgLink"
                                CausesValidation="False" CommandName="Delete">-</asp:LinkButton>
                            <asp:HyperLink ID="lnkEliminar" runat="server" CssClass="dgLink">Eliminar</asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:Button runat="server" CssClass="button" Text="Nuevo mensaje" ID="btnNuevo" OnClick="btnNuevo_OnClick" />
            <div runat="server" visible="false" id="divNuevo" class="well well-sm">
                <h2>Nuevo Mensaje Personalizado</h2>
                <br />
                <div class="row align-right">
                    <div class="col-sm-1">
                        <asp:Label runat="server" Text="Mensaje" ID="lblMessage"></asp:Label>
                    </div>
                    <div class="col-sm-5">
                        <asp:TextBox runat="server" ID="txtMessage" Width="450px"></asp:TextBox>
                    </div>
                    <div class="col-sm-1 align-right">
                        <asp:Label ID="Label6" runat="server" Text="Inicio"></asp:Label>
                    </div>
                    <div class="col-sm-2">
                        <asp:TextBox runat="server" ID="txtDateMsgStart" CssClass="TextBox" Width="80px"></asp:TextBox>
                    </div>
                    <div class="col-sm-1 align-right">
                        <asp:Label ID="Label7" runat="server" Text="Fin"></asp:Label>
                    </div>
                    <div class="col-sm-2">
                        <asp:TextBox runat="server" ID="txtDateMsgTo" CssClass="TextBox" Width="80px"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    &nbsp;
                </div>
                <div class="row">
                    <div class="col-sm-1 align-right">
                        <asp:Label ID="Label8" runat="server" Text="Rutas"></asp:Label>
                    </div>
                    <div class="col-sm-4">
                        <asp:TextBox runat="server" ID="txtRutas" TextMode="MultiLine" Width="350px" Height="40px"></asp:TextBox>
                    </div>
                    <div class="col-sm-1 align-right">
                        <asp:Label ID="Label9" runat="server" Text="Orden"></asp:Label>
                    </div>
                    <div class="col-sm-2">
                        <asp:TextBox runat="server" ID="txtOrden" Width="50px"></asp:TextBox>
                    </div>
                    <div class="col-sm-1 align-right">
                        <asp:Label ID="Label10" runat="server" Text="Duracion (Segundos)"></asp:Label>
                    </div>
                    <div class="col-sm-2 ">
                        <asp:TextBox runat="server" ID="txtDuracion" Width="50px"></asp:TextBox>
                    </div>
                    <div class="col-sm-1 align-right">
                        <asp:Label ID="Label17" runat="server" Text="Icono"></asp:Label>
                    </div>
                    <div class="col-sm-2 ">
                        <asp:TextBox runat="server" ID="txtIcon" Width="70px"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    &nbsp;
                </div>
                <div class="row">
                    <div class="col-sm-1">
                        <asp:Button runat="server" Text="Guardar" ID="btnNuevoGuardar" OnClick="btnNuevoGuardar_OnClick" CssClass="buttonNew" />
                    </div>
                    <div class="col-sm-1">
                        <asp:Button runat="server" Text="Cancelar" ID="btnNuevoCancelar" OnClick="btnNuevoCancelar_OnClick" CssClass="buttonNew" />
                    </div>

                    <div class="col-sm-6">
                        <asp:Label runat="server" ID="lblMessageError" CssClass="validators" Visible="false"></asp:Label>
                    </div>
                </div>
            </div>
            <br />
            <br />
            <h2>Mensajes de Retencion</h2>
            <asp:GridView CssClass="datagrid" runat="server" ID="gridRetentionMessages"
                AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="id_retencion">
                <AlternatingRowStyle CssClass="dgAlternate" />
                <SelectedRowStyle CssClass="dgSelected" />
                <HeaderStyle CssClass="dgHeader"></HeaderStyle>
                <Columns>
                    <asp:BoundField DataField="titulo" HeaderText="Titulo" ItemStyle-Width="8%"/>
                    <asp:BoundField DataField="mensaje" HeaderText="Mensaje" ItemStyle-Width="20%" />
                    <asp:BoundField DataField="texto_boton" HeaderText="Texto Botón" ItemStyle-Width="8%"/>
                    <asp:BoundField DataField="ruta" HeaderText="Rutas" ItemStyle-Width="15%" />
                    <asp:BoundField DataField="cuando" HeaderText="Veces" ItemStyle-Width="5%"/>
                    <asp:BoundField DataField="duracion" HeaderText="Duracion (Segundos)" ItemStyle-Width="5%"/>
                    <asp:BoundField DataField="plan_tarifario" HeaderText="Aplicación" ItemStyle-Width="5%"/>
                    <asp:BoundField DataField="codigo_acceso" HeaderText="Codigo Promo" ItemStyle-Width="5%" />
                    <asp:TemplateField ItemStyle-Width="4%">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkeditMsgRet" runat="server" CssClass="dgLink" CausesValidation="False"
                                CommandName="Select">Editar</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="4%">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEliminarMsgRet2" Style="display: none" runat="server" CssClass="dgLink"
                                CausesValidation="False" CommandName="Delete">-</asp:LinkButton>
                            <asp:HyperLink ID="lnkEliminarMsgRet" runat="server" CssClass="dgLink">Eliminar</asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <div>
                <asp:Button runat="server" CssClass="button" Text="Nuevo mensaje" ID="btnNuevoRetencion" OnClick="btnNuevoRetencion_OnClick" />
            </div>
            <br />
            <div runat="server" visible="false" id="divnuevoRetencion" class="well well-sm">
                <div class="row">
                    <div style="margin-left: 30px;">
                        <h2>Nuevo Mensaje De Retención</h2>
                    </div>
                </div>
                <div class="row">
                    &nbsp;
                </div>
                <div class="row">
                    <div class="col-sm-1 align-right">
                        <asp:Label runat="server" Text="Título" ID="Label1"></asp:Label>
                    </div>
                    <div class="col-sm-2">
                        <asp:TextBox runat="server" ID="txtRetTitle"></asp:TextBox>
                    </div>
                    <div class="col-sm-1 align-right">
                        <asp:Label ID="Label14" runat="server" Text="Texto Botón"></asp:Label>
                    </div>
                    <div class="col-sm-2">
                        <asp:TextBox runat="server" ID="txtRetAction"></asp:TextBox>
                    </div>
                    <div class="col-sm-1 align-right">
                        <asp:Label ID="Label11" runat="server" Text="Veces"></asp:Label>
                    </div>
                    <div class="col-sm-2 ">
                        <asp:TextBox runat="server" ID="txtRetTimes" Width="50px"></asp:TextBox>
                    </div>
                    <div class="col-sm-1 align-right">
                        <asp:Label ID="Label13" runat="server" Text="Duración"></asp:Label>
                    </div>
                    <div class="col-sm-1">
                        <asp:TextBox runat="server" ID="txtRetDuration" Width="50px"></asp:TextBox>
                    </div>
                    <div class="col-sm-1">&nbsp;</div>
                </div>
                <div class="row">
                    &nbsp;
                </div>
                <div class="row">
                    <div class="col-sm-1 align-right">
                        <asp:Label runat="server" Text="Mensaje" ID="lblMensajeRetencion"></asp:Label>
                    </div>
                    <div class="col-sm-5">
                        <asp:TextBox runat="server" ID="txtRetMessage" Width="450px"></asp:TextBox>
                    </div>
                    <div class="col-sm-1 align-right">
                        <asp:Label ID="Label15" runat="server" Text="Aplicación"></asp:Label>
                    </div>
                    <div class="col-sm-2">
                        <asp:TextBox runat="server" ID="txtRetApp" Width="100px"></asp:TextBox>
                    </div>
                    <div class="col-sm-1 align-right">
                        <asp:Label ID="Label16" runat="server" Text="Código Promo"></asp:Label>
                    </div>
                    <div class="col-sm-2">
                        <asp:TextBox runat="server" ID="txtRetAccessCode" Width="100px"></asp:TextBox>
                    </div>
                    <div class="col-sm-1">&nbsp;</div>
                </div>
                <div class="row">
                    &nbsp;
                </div>
                <div class="row">
                    <div class="col-sm-1 align-right">
                        <asp:Label ID="Label12" runat="server" Text="Rutas"></asp:Label>
                    </div>
                    <div class="col-sm-5">
                        <asp:TextBox runat="server" ID="txtRetRutes" TextMode="MultiLine" Width="450px" Height="70px"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    &nbsp;
                </div>
                <div class="row">
                    <div class="col-sm-1">
                        <center>
                            <asp:Button CssClass="buttonNew" runat="server" Text="Guardar" ID="btnGuardarRetencion" OnClick="btnNuevoGuardarRetencion_OnClick" /></center>
                    </div>
                    <div class="col-sm-1">
                        <center>
                            <asp:Button CssClass="buttonNew" runat="server" Text="Cancelar" ID="btnCancelarRetencion" OnClick="btnNuevoCancelarRetencion_OnClick" /></center>
                    </div>
                    <div class="col-sm-5">
                        <center>
                            <asp:Label ID="lblRetError" runat="server" CssClass="validators" Visible="false"></asp:Label>
                        </center>
                    </div>
                </div>
            </div>
            <br />
            <br />
            <h2>Mensajes Predefinidos</h2>
            <h5>El texto de los mensajes aplica de manera <strong>global</strong>. Active o desactive cada mensaje para mostrarlo en el sitio.</h5>
            <asp:GridView CssClass="table table-striped table-hover table-condensed" runat="server" ID="gridPredefinedMessages" DataKeyNames="id_mensaje" AllowSorting="True" AutoGenerateColumns="False" AutoGenerateEditButton="True" OnRowCancelingEdit="gridPredefinedMessages_RowCancelingEdit" OnRowEditing="gridPredefinedMessages_RowEditing" OnRowUpdating="gridPredefinedMessages_RowUpdating" EnableViewState="True" OnRowCommand="gridPredefinedMessages_RowCommand">
                <Columns>
                    <asp:BoundField DataField="mensaje" HeaderText="Mensaje" ControlStyle-Width="95%"/>
                    <asp:TemplateField HeaderText="Activado">
                        <ItemTemplate>
                            <asp:CheckBox AutoPostBack="true" runat="server" OnCheckedChanged="chkHabilitado_OnCheckedChanged" OnPreRender="chkHabilitado_OnPreRender" ID="chkHabilitado" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br />
            <br />
            <p>
                Etiquetas predefinidas para los mensajes:
            </p>
            <ul>
                <li><strong>[USUARIOS]: </strong>Numero de usuarios actualmente en linea.</li>
                <li><strong>[TIEMPO]: </strong>Tiempo en minutos desde la ultima reserva.</li>
                <li><strong>[NUMERO_HABITACIONES]: </strong>Numero de habitaciones restantes.</li>
                <li><strong>[NUMERO_RESERVAS]: </strong>Numero de reservas realizadas en el hotel el dia de hoy.</li>
                <li><strong>[TOTAL_USUARIOS]: </strong>Cantidad total de usuarios que ha visto el hotel durante el dia.</li>
            </ul>
        </div>
    </form>
    <!-- The Modal -->
    <div id="myModal" class="modal">

        <!-- Modal content -->
        <div class="modal-content">
            <div class="modal-header">
                <span class="close" onclick="closeModal()">&times;</span>
                <h2>Desactivar Promoción</h2>
            </div>
            <div class="modal-body">
                <h4 class="msg">¿Está seguro que desea desactivar esta promoción?</h4>
            </div>
            <div class="modal-footer">
                <input type="button" value="Sí" onclick="deactivatePromo();" />
                <input type="button" value="No" onclick="closeModal();" />
            </div>
        </div>
    </div>
    <script type="text/javascript">
        // Get the modal
        var modal = document.getElementById('myModal');

        // Get the <span> element that closes the modal
        var span = document.getElementsByClassName("close")[0];

        // When the user clicks the button, open the modal 
        var openModal = function (id, msg, title) {
            modal.style.display = "block";
            document.getElementById('txtObjDelete').value = id;
            $(".modal-body .msg").text(msg);
            $(".modal-header h2").text(title);
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
    <script type="text/javascript">
        $(document).ready(function () {
            parent.remove_leftMenu();
        });

        $(function () {
            var dateFormat = "mm/dd/yy",
              from = $("#txtDateMsgStart")
                .datepicker({
                    showOn: "button",
                    buttonImage: "/RateManager/Calendar/calbtn.gif",
                    buttonImageOnly: true,
                    buttonText: "Select date",
                    defaultDate: "+1w",
                    changeMonth: true,
                    numberOfMonths: 2
                })
                .on("change", function () {
                    to.datepicker("option", "minDate", getDate(this));
                }),
              to = $("#txtDateMsgTo").datepicker({
                  showOn: "button",
                  buttonImage: "/RateManager/Calendar/calbtn.gif",
                  buttonImageOnly: true,
                  buttonText: "Select date",
                  defaultDate: "+1w",
                  changeMonth: true,
                  numberOfMonths: 2
              })
              .on("change", function () {
                  from.datepicker("option", "maxDate", getDate(this));
              });

            function getDate(element) {
                var date;
                try {
                    date = $.datepicker.parseDate(dateFormat, element.value);
                } catch (error) {
                    date = null;
                }

                return date;
            }
        }); //DATEPICKER
    </script>
</body>
</html>
