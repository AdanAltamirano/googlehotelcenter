<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RegistroCasas.aspx.vb" Inherits="RateManager.RegistroCasas"%>
<%@import namespace="RateManager"%>

<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<%@ Register Src="~/Portal/Modules/CtrlPreserveScrolls.ascx" TagName="CtrlPreserveScrolls" TagPrefix="uc2" %>
<%@ Register Src="~/Modulos/CtrlIdioma.ascx" TagPrefix="uc2" TagName="CtrlIdioma" %>
<!--d-->
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>EditRegistro</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
        
    <script type="text/javascript" src='<%= (request.applicationpath & "/pages/Scripts/jquery.min.js").Replace("//","/") %>'></script>

    <script type="text/javascript" src='<%= (request.applicationpath & "/pages/Scripts/jquery.bgiframe.min.js").Replace("//", "/")%>'></script>

    <script type="text/javascript" src='<%= (request.applicationpath & "/pages/Scripts/jquery.autocomplete.min.js").Replace("//","/")%>'></script>

    <script type="text/javascript" language="javascript">
        jQuery.noConflict();
    </script>

    <%--    <script language='javascript' src='../Portal/Scripts/base.js'></script>
    <script language='javascript' src='../Portal/Scripts/Ajax.js'></script>--%>

        <script>
            function IniDate() {
                var fecha = new Date();
                var fecha2 = new Date(2030, 12, 31);
                var arr = new Array(3);
                arr[0] = [fecha.getFullYear(), fecha.getMonth() + 1, fecha.getDate()]
                arr[1] = [fecha2.getFullYear(), fecha2.getMonth() + 1, fecha2.getDate()];
                return arr;
            }

            function VerifyDate() {
                var txt1 = document.getElementById("txtInicio").value;
                var txt2 = document.getElementById("txtFinal").value;
            }
            function evalDates() {
                var cmb = document.getElementById("ddlStatus"); //combo
                var txt1 = document.getElementById("txtInicio"); //inicio
                var txt2 = document.getElementById("txtFinal"); //fin
                var xDia, xMes, xYear;

                if (cmb.selectedIndex >= 1) {
                    xDia = txt1.value.substring(3, 5);
                    xMes = txt1.value.substring(0, 2);
                    xMes = xMes - 1;
                    xYear = txt1.value.substring(6, 10);
                    //xYear = xYear - 1;	
                    var NewDt1 = new Date(xYear, xMes, xDia);

                    xDia = txt2.value.substring(3, 5);
                    xMes = txt2.value.substring(0, 2);
                    xMes = xMes - 1;
                    xYear = txt2.value.substring(6, 10);
                    //xYear = xYear - 1;	
                    var NewDt2 = new Date(xYear, xMes, xDia);

                    //Set 1 day in milliseconds
                    var one_day = 1000 * 60 * 60 * 24;

                    //Calculate difference btw the two dates, and convert to days		
                    one_day = (NewDt2.getTime() - NewDt1.getTime()) / (one_day);

                    NewDt1 = Date.parse(NewDt1);
                    NewDt2 = Date.parse(NewDt2);
                    var conf;
                    if (eval(one_day) >= 15) {
                        return true;
                    }
                    return false;
                }
            }


        </script>
	</HEAD>
	<body>
        <iframe id="gToday:normal:agenda.js" style="z-index: 999; left: -500px; position: absolute;
        top: -500px" name="gToday:normal:agenda.js" src='<%=GeRequestApplicationPath(string.concat("/Calendar/",portalculture.getculture().Name.Substring(0, 2).ToLower(),"/ipopeng2.htm" )) %>'
        frameborder="0" width="174" scrolling="no" height="189"></iframe>
		<form id="Form1" method="post" runat="server">
		 <div class="mDiv">
            <div class="title">
                <asp:Label ID="lblTitulo" runat="server" EnableViewState="False" Text="Registro de Propiedad" CssClass="tituloSeccion"></asp:Label>
            </div>
         </div>	
			<TABLE id="bookingcontainer" cellSpacing="0" width="650" align="center" border="0">
				<tr>
					<td>
<table cellspacing="0" cellpadding="2" width="700" align="left" border="0" id="tblCasa" runat="server">

    <tr>
        <td class="dgItem" align="center" colspan="4">
            <asp:Label ID="lblCasaTitulo" runat="server">Informacion Propiedad</asp:Label>
        </td>
    </tr>
</table>
<table cellspacing="0" cellpadding="2" width="700" align="left" border="0" id="Table6" runat="server">
    <tr>
        <td align="right" >
            <asp:Label ID="lblCasaNombre" runat="server" EnableViewState="False">Nombre :</asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtCasaNombre" runat="server"></asp:TextBox>
        </td>
        
    </tr>
    <tr>
        <td align="right" width="25%">
            <asp:Label ID="lblCasaDomicilio" runat="server" EnableViewState="False">Domicilio :</asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtCasaDomicilio" runat="server" MaxLength="120" Width="150px"></asp:TextBox><asp:RequiredFieldValidator
                ID="RequiredFieldValidator2" runat="server" CssClass="validators" ForeColor=" " ErrorMessage="El Domicilio de la Empresa es requerido"
                ControlToValidate="txtCasaDomicilio" ValidationGroup="Houses" Display="Dynamic">*</asp:RequiredFieldValidator>
        </td>
        <td align="right" style="height: 21px">
            <asp:Label ID="lblCasaMoneda" runat="server" CssClass="Darklabel" EnableViewState="False">Moneda :</asp:Label>
        </td>
        <td style="height: 21px">
            <asp:DropDownList ID="cmbCasaMonedas" runat="server" Width="180px"  AutoPostBack="true">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td align="right" style="height: 21px">
            <asp:Label ID="lblCasaPais" runat="server" EnableViewState="False">Pais :</asp:Label>
        </td>
        <td style="height: 21px">
            <asp:DropDownList ID="cmbCasaPaises" runat="server" Width="150px" AutoPostBack="true"></asp:DropDownList>
        </td>
        <td align="right" style="height: 21px">
            <asp:Label ID="lblCasaEstado" runat="server" EnableViewState="False">Estado :</asp:Label>
        </td>
        <td style="height: 21px">

            <asp:DropDownList ID="cmbCasaEstados" runat="server" Width="150px" AutoPostBack="true"></asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label ID="lblCasaMunicipio" runat="server" EnableViewState="False">Municipio :</asp:Label>
        </td>
        <td>
            <asp:DropDownList ID="cmbCasaMunicipio" runat="server" Width="150px" AutoPostBack="true"></asp:DropDownList>
        </td>
        <td align="right">
            <asp:Label ID="lblCasaCiudad" runat="server" EnableViewState="False">Ciudad :</asp:Label>
        </td>
        <td>

            <asp:DropDownList ID="cmbCasaCiudades" runat="server" Width="150px" AutoPostBack="true"></asp:DropDownList>
        </td>
    </tr>
    <tr>
        
        <td align="right">
            <asp:Label ID="lblCasaCp" runat="server" EnableViewState="False">Cod. Postal :</asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtCasaCp" runat="server" MaxLength="11" Width="150px"></asp:TextBox>
        </td>
         <td align="right">
            <asp:Label ID="lblTipoPago" runat="server" EnableViewState="False">Tipo de Pago :</asp:Label>
        </td>
        <td>
            <asp:DropDownList ID="ddlTipoPago" runat="server" Width="150px"></asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label ID="lblCasaFax" runat="server" CssClass="label" EnableViewState="False">Fax :</asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtCasaFax" runat="server" MaxLength="20" CssClass="textbox" Width="150px"></asp:TextBox>
        </td>
        <td align="right">
            <asp:Label ID="lblCasaTel" runat="server" EnableViewState="False">Teléfono :</asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtCasaTel" runat="server" MaxLength="20" Width="150px"></asp:TextBox><asp:RequiredFieldValidator
                ID="RequiredFieldValidator5" runat="server" CssClass="validators" ForeColor=" " ErrorMessage="Telefono de la Empresa es requerido"
                ControlToValidate="txtCasaTel" ValidationGroup="Houses" Display="Dynamic">*</asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>

    </tr>
    <tr>
        
        
    </tr>
    <tr>
        <td align="right" width="25%">
            <asp:Label ID="lblNombreContacto" runat="server" EnableViewState="False">Nombre Contacto :</asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtNombreContacto" runat="server" MaxLength="120" Width="150px"></asp:TextBox><asp:RequiredFieldValidator
                ID="RequiredFieldValidator1" runat="server" CssClass="validators" ForeColor=" " ErrorMessage="El nombre del contacto es requerido"
                ControlToValidate="txtNombreContacto" ValidationGroup="Houses" Display="Dynamic">*</asp:RequiredFieldValidator>
        </td>
        <td align="right">
            <asp:Label ID="lblCasaTelG" runat="server" CssClass="DarkLabel" EnableViewState="False">Telefono Propietario:</asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtCasaTelG" runat="server" MaxLength="20" CssClass="textbox" Width="150px"></asp:TextBox><asp:RequiredFieldValidator
                ID="RequiredFieldValidator8" runat="server" CssClass="validators" ForeColor=" " ControlToValidate="txtCasaTelG"
                Display="Dynamic">*</asp:RequiredFieldValidator>
        </td>
       
    </tr>
    <tr>
        <td align="right">
            <asp:Label ID="lblCasaCorreoG" runat="server" EnableViewState="False">Correo :</asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtCasaCorreoG" runat="server" MaxLength="80" CssClass="textbox" Width="150px"></asp:TextBox><asp:RequiredFieldValidator
                ID="RequiredFieldValidator7" runat="server" CssClass="validators" ForeColor=" " ControlToValidate="txtCasaCorreoG"
                Display="Dynamic" ValidationGroup="Houses">*</asp:RequiredFieldValidator>
        </td>
        
        <td align="right">
            <asp:Label ID="lblCasaPaginaWeb" runat="server" CssClass="label" EnableViewState="False">Pagina Web :</asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtCasaPaginaWeb" runat="server" MaxLength="80" CssClass="textbox" Width="150px"></asp:TextBox>
        </td>
        
    </tr>
    <tr>
         <td align="right">
            <asp:Label ID="lblCategory" runat="server" CssClass="DarkLabel" EnableViewState="False">Categoria :</asp:Label>
        </td>
        <td>
            <asp:DropDownList ID="ddlCategory" runat="server" Width="150px"></asp:DropDownList>
        </td>
        <td align="right">
            <asp:Label ID="lblArea" runat="server" CssClass="DarkLabel" EnableViewState="False">Area:</asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtArea" runat="server" MaxLength="5" CssClass="textbox" Width="50px"></asp:TextBox>
            <asp:DropDownList ID="ddlUnits" runat="server" Width="80px"></asp:DropDownList>
        </td>
       
    </tr>
    <tr>
        <td align="right">
            <asp:Label ID="lblBanios" runat="server" EnableViewState="False">Baños :</asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtBanios" runat="server" MaxLength="6" CssClass="textbox" Width="70px"></asp:TextBox>
        </td>
        

        <td align="right">
            <asp:Label ID="lblAmaLlaves" runat="server" EnableViewState="False">Ama de llaves :</asp:Label>
        </td>
        <td>
            <asp:CheckBox ID="amaLlaves" runat="server" CssClass="checkbox"></asp:CheckBox>
        </td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label ID="lblRooms" runat="server" EnableViewState="False">Habitaciones :</asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtRooms" runat="server" MaxLength="6" CssClass="textbox" Width="70px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label ID="lblPropietarioDesc" runat="server" EnableViewState="False">Acerca del Propietario :</asp:Label>
        </td>
        <td colspan="3">
            <uc2:CtrlIdioma runat="server" ID="ctrlIdiomaPropietario" />
        </td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label ID="lblCasaDesc" runat="server" EnableViewState="False">Descripcion de la propiedad :</asp:Label>
        </td>
        <td colspan="3">
            <uc2:CtrlIdioma runat="server" ID="mlDescriptionRoom" />
        </td>
    </tr>
</table>

<table cellspacing="0" cellpadding="3" width="700" align="left" border="0" id="Table3" runat="server">
    <tr>
        <td class="dgItem" align="center" colspan="4">
            <asp:Label ID="lblSecond" runat="server">Informacion de reservación</asp:Label>
        </td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label ID="lblCheckin" runat="server" EnableViewState="False" CssClass="clsLabel">Checkin :</asp:Label>
        </td>
        <td align="left" width="178">
            <asp:DropDownList ID="cmbCheckinHora" runat="server" Width="60px">
                <asp:ListItem Value="01">01</asp:ListItem>
                <asp:ListItem Value="02">02</asp:ListItem>
                <asp:ListItem Value="03">03</asp:ListItem>
                <asp:ListItem Value="04">04</asp:ListItem>
                <asp:ListItem Value="05">05</asp:ListItem>
                <asp:ListItem Value="06">06</asp:ListItem>
                <asp:ListItem Value="07">07</asp:ListItem>
                <asp:ListItem Value="08">08</asp:ListItem>
                <asp:ListItem Value="09">09</asp:ListItem>
                <asp:ListItem Value="10">10</asp:ListItem>
                <asp:ListItem Value="11">11</asp:ListItem>
                <asp:ListItem Value="12">12</asp:ListItem>
                <asp:ListItem Value="13">13</asp:ListItem>
                <asp:ListItem Value="14">14</asp:ListItem>
                <asp:ListItem Value="15">15</asp:ListItem>
                <asp:ListItem Value="16">16</asp:ListItem>
                <asp:ListItem Value="17">17</asp:ListItem>
                <asp:ListItem Value="18">18</asp:ListItem>
                <asp:ListItem Value="19">19</asp:ListItem>
                <asp:ListItem Value="20">20</asp:ListItem>
                <asp:ListItem Value="21">21</asp:ListItem>
                <asp:ListItem Value="22">22</asp:ListItem>
                <asp:ListItem Value="23">23</asp:ListItem>
            </asp:DropDownList>
            :
            <asp:DropDownList ID="cmbCheckinMin" runat="server" Width="60px">
                <asp:ListItem Value="00">00</asp:ListItem>
                <asp:ListItem Value="15">15</asp:ListItem>
                <asp:ListItem Value="30">30</asp:ListItem>
                <asp:ListItem Value="45">45</asp:ListItem>
            </asp:DropDownList>
        </td>
        <td  align="right">
            <asp:Label ID="lblCheckout" runat="server" EnableViewState="False" CssClass="clsLabel">Checkout  :</asp:Label>
        </td>
        <td style="width: 146px" align="left">
            <asp:DropDownList ID="cmbCheckoutHora" runat="server" Width="60px">
                <asp:ListItem Value="01">01</asp:ListItem>
                <asp:ListItem Value="02">02</asp:ListItem>
                <asp:ListItem Value="03">03</asp:ListItem>
                <asp:ListItem Value="04">04</asp:ListItem>
                <asp:ListItem Value="05">05</asp:ListItem>
                <asp:ListItem Value="06">06</asp:ListItem>
                <asp:ListItem Value="07">07</asp:ListItem>
                <asp:ListItem Value="08">08</asp:ListItem>
                <asp:ListItem Value="09">09</asp:ListItem>
                <asp:ListItem Value="10">10</asp:ListItem>
                <asp:ListItem Value="11">11</asp:ListItem>
                <asp:ListItem Value="12">12</asp:ListItem>
                <asp:ListItem Value="13">13</asp:ListItem>
                <asp:ListItem Value="14">14</asp:ListItem>
                <asp:ListItem Value="15">15</asp:ListItem>
                <asp:ListItem Value="16">16</asp:ListItem>
                <asp:ListItem Value="17">17</asp:ListItem>
                <asp:ListItem Value="18">18</asp:ListItem>
                <asp:ListItem Value="19">19</asp:ListItem>
                <asp:ListItem Value="20">20</asp:ListItem>
                <asp:ListItem Value="21">21</asp:ListItem>
                <asp:ListItem Value="22">22</asp:ListItem>
                <asp:ListItem Value="23">23</asp:ListItem>
            </asp:DropDownList>
            :
            <asp:DropDownList ID="cmbCheckoutMin" runat="server" Width="60px">
                <asp:ListItem Value="00">00</asp:ListItem>
                <asp:ListItem Value="15">15</asp:ListItem>
                <asp:ListItem Value="30">30</asp:ListItem>
                <asp:ListItem Value="45">45</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>

    <tr>
        <td align="right">
            <asp:Label ID="lblPlusTax" runat="server" EnableViewState="False" CssClass="clsLabel"></asp:Label>
        </td>
        <td style="width: 178px" align="left">
            <asp:CheckBox ID="chkPlusTax" runat="server"></asp:CheckBox>
            <asp:CheckBox ID="chkPlusTaxSrc" runat="server" style="display:none;"></asp:CheckBox>
        </td>
        <td style="width: 146px" align="right">
            <asp:Label ID="lblImpuesto" runat="server" EnableViewState="False" CssClass="clslabel">Impuesto :</asp:Label>
        </td>
        <td align="left">
            <asp:TextBox ID="txtImpuesto" runat="server" CssClass="textbox" Width="58px" MaxLength="5"></asp:TextBox>
            <input ID="txtImpuestoSrc" type=hidden runat=server />
            <asp:RequiredFieldValidator
                ID="RequiredFieldValidator3" runat="server" CssClass="Validators" ControlToValidate="txtImpuesto"
                ErrorMessage="Impuesto es requerido" Display="Dynamic">*</asp:RequiredFieldValidator><asp:RangeValidator
                    ID="RangeValidator9" runat="server" CssClass="Validators" ControlToValidate="txtImpuesto"
                    ErrorMessage="Impuesto es numerico (1-99)" Display="Dynamic" Type="Double" MaximumValue="99"
                    MinimumValue="0">*</asp:RangeValidator>
        </td>
    </tr>
    <tr>
        <td class="dgItem" align="center" colspan="4">
            <asp:Label ID="lblRules" runat="server">Reglas de reservacion</asp:Label>
        </td>

    </tr>
    <tr>
                        <td align="right">
                            <asp:Label ID="lblEstanciaMin" runat="server" EnableViewState="False" CssClass="clsLabel">Estancia Mínima :</asp:Label>
                        </td>
                        <td align="left" width="178">
                            <asp:TextBox ID="txtEstanciaMin" runat="server" CssClass="textbox" Width="51px" MaxLength="3">1</asp:TextBox><asp:RangeValidator
                                    ID="Rangevalidator14" runat="server" CssClass="Validators" ControlToValidate="txtEstanciaMin"
                                    ErrorMessage="Estancia Mínima es numerico (1-365)" Display="Dynamic" Type="Integer"
                                    MaximumValue="365" MinimumValue="1" ForeColor=" ">[1-365]</asp:RangeValidator>
                        </td>
                        <td style="width: 146px" align="right">
                            <asp:Label ID="lblMaxDiasRenta" runat="server" EnableViewState="False" CssClass="clsLabel"
                                Width="126px">Max. Dias de Renta :</asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtMaxDiasRenta" runat="server" CssClass="textbox" Width="51px"
                                MaxLength="4">0</asp:TextBox><asp:RequiredFieldValidator ID="rfvMaxDiasRenta" runat="server"
                                    CssClass="Validators" ControlToValidate="txtMaxDiasRenta" ErrorMessage="Max. Dias de Renta es requerido"
                                    Display="Dynamic" ForeColor=" ">*</asp:RequiredFieldValidator><asp:RangeValidator
                                        ID="RangeValidator1" runat="server" CssClass="Validators" ControlToValidate="txtMaxDiasRenta"
                                        ErrorMessage="Max. Dias de Renta es numerico (1-365)" Display="Dynamic" Type="Integer"
                                        MaximumValue="365" MinimumValue="0" ForeColor=" ">[0-365]</asp:RangeValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblEdadMinimaNiño" runat="server" EnableViewState="False" CssClass="clsLabel">No Cobrar :</asp:Label>
                        </td>
                        <td align="left" width="178">
                            <asp:TextBox ID="txtEdadMinimaNino" runat="server" CssClass="textbox" Width="51px"
                                MaxLength="2">0</asp:TextBox>
                                <asp:Label ID="lblNoCobrarAnios" runat="server" Text="Años" CssClass="clsLabel"></asp:Label>
                                <asp:RangeValidator ID="Rangevalidator2" runat="server"
                                    CssClass="Validators" ControlToValidate="txtEdadMinimaNino" ErrorMessage="Edad Min. de Niño es numerico (1-99)"
                                    Display="Dynamic" Type="Integer" MaximumValue="99" MinimumValue="0">*</asp:RangeValidator>
                            <asp:Label ID="lblErrorEdadNoCobrar" runat="server" CssClass="Validators" 
                                Visible="False">Label</asp:Label>
                        </td>
                        <td style="width: 146px" align="right">
                            <asp:Label ID="lblEdadMaximaNiño" runat="server" EnableViewState="False" CssClass="clsLabel">Edad Niño :</asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtEdadMaximaNino" runat="server" CssClass="textbox" Width="51px"
                                MaxLength="2"></asp:TextBox>
                                <asp:Label ID="lblEdadNinio" runat="server" Text="Años" CssClass="clsLabel"></asp:Label>
                                <asp:RequiredFieldValidator
                                ID="rfvEdadNinio" runat="server" CssClass="Validators" ControlToValidate="txtEdadMaximaNino"
                                ErrorMessage="Estancia Mínima es requerida" Display="Dynamic" 
                                ForeColor=" ">*</asp:RequiredFieldValidator><asp:RangeValidator ID="RangeValidator7" runat="server"
                                    CssClass="Validators" ControlToValidate="txtEdadMaximaNino" ErrorMessage="Edad Max. de Niño es numerico (1-99)"
                                    Display="Dynamic" Type="Integer" MaximumValue="99" MinimumValue="0">*</asp:RangeValidator>
                            <asp:Label ID="lblErrorEdadNinio" runat="server" CssClass="Validators" 
                                Visible="False">Label</asp:Label>
                        </td>
                    </tr>
                    <tr><td align="right">
            <asp:Label ID="lblMaxPeople" runat="server" EnableViewState="False">Maximo de personas :</asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtMaxPeople" runat="server" MaxLength="2" CssClass="textbox" Width="50px"></asp:TextBox>
        </td><td align="right">
                            <asp:Label ID="lblEdadMaximaAdo" runat="server" EnableViewState="False" 
                            CssClass="clsLabel">Edad Junior</asp:Label>
                        </td><td>
                            <asp:TextBox ID="txtEdadMaximaAdo" runat="server" CssClass="textbox" Width="51px"
                                MaxLength="2"></asp:TextBox><asp:Label ID="lblJuniorAnios" runat="server" Text="Años" CssClass="clsLabel"></asp:Label>
                            <asp:RangeValidator ID="rvEdadaAdolecente" runat="server"
                                    CssClass="Validators" ControlToValidate="txtEdadMaximaAdo" ErrorMessage="Edad Max. de Adolecente es numerico (1-99)"
                                    Display="Dynamic" Type="Integer" MaximumValue="99" MinimumValue="0">*</asp:RangeValidator>
                        </td>
                    </tr>
    <tr>
        <td style="width: 146px" align="right">
                            <asp:Label ID="lblStatusA" runat="server" EnableViewState="False" CssClass="clsLabel">Disponibilidad :</asp:Label>
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="ddlStatus" runat="server">
                                <asp:ListItem Value="O">Abierto</asp:ListItem>
                                <asp:ListItem Value="C">Cerrado</asp:ListItem>
                                <asp:ListItem Value="N">Ninguna Llegada</asp:ListItem>
                            </asp:DropDownList>
                        </td>
    </tr>
    <tr>
                        <td align="right">
                            <p>
                                <asp:Label ID="lblNoArrivals" runat="server" EnableViewState="False" CssClass="clsLabel">No arrivals:</asp:Label></p>
                        </td>
                        <td align="left" width="178">
                            <table id="Table4" cellspacing="1" cellpadding="1" border="0" frame="void">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblDomingo" runat="server" EnableViewState="False" CssClass="clsLabel">Do</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblLunes" runat="server" EnableViewState="False" CssClass="clslabel">Lu</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblMartes" runat="server" EnableViewState="False" CssClass="clslabel">Ma</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblMiercoles" runat="server" EnableViewState="False" CssClass="clslabel">Mi</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblJueves" runat="server" EnableViewState="False" CssClass="clslabel">Ju</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblViernes" runat="server" EnableViewState="False" CssClass="clslabel">Vi </asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblSabado" runat="server" EnableViewState="False" CssClass="clsLabel">Sa</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="D" runat="server"></asp:CheckBox>
                                    </td>
                                    <td style="width: 15px">
                                        <asp:CheckBox ID="L" runat="server"></asp:CheckBox>
                                    </td>
                                    <td style="width: 22px">
                                        <asp:CheckBox ID="Ma" runat="server"></asp:CheckBox>
                                    </td>
                                    <td style="width: 19px">
                                        <asp:CheckBox ID="Mi" runat="server"></asp:CheckBox>
                                    </td>
                                    <td style="width: 11px">
                                        <asp:CheckBox ID="J" runat="server"></asp:CheckBox>
                                    </td>
                                    <td style="width: 11px">
                                        <asp:CheckBox ID="V" runat="server"></asp:CheckBox>
                                    </td>
                                    <td style="width: 15px">
                                        <asp:CheckBox ID="S" runat="server"></asp:CheckBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="width: 146px" align="right">
                            <asp:Label ID="lblDiasAnticipados" runat="server" EnableViewState="False" CssClass="clsLabel">Advanced booking:</asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtDiasAnticipados" runat="server" CssClass="textbox" Width="51px"
                                MaxLength="2">0</asp:TextBox><asp:Label ID="lblDays" runat="server" EnableViewState="False" CssClass="clsLabel">Dias</asp:Label><asp:RequiredFieldValidator
                                    ID="RequiredFieldValidator4" runat="server" CssClass="Validators" ControlToValidate="txtDiasAnticipados"
                                    ErrorMessage="Max. Dias de Renta es requerido" Display="Dynamic" ForeColor=" ">*</asp:RequiredFieldValidator><asp:RangeValidator
                                        ID="RangeValidator5" runat="server" CssClass="Validators" ControlToValidate="txtDiasAnticipados"
                                        ErrorMessage="Dias anticipados para reservar es numerico (1-99)" Display="Dynamic"
                                        Type="Integer" MaximumValue="99" MinimumValue="0" ForeColor=" ">*</asp:RangeValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                        </td>
                        <td align="left" width="178">
                        </td>
                        <td style="width: 146px" align="right">
                        </td>
                        <td align="left">
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblLatitud" runat="server" EnableViewState="False" CssClass="clsLabel">Latitud</asp:Label>
                        </td>
                        <td align="left" width="178">
                            <asp:TextBox ID="txtLatitud" runat="server" MaxLength="15" Columns="12"></asp:TextBox><asp:RangeValidator
                                ID="RvalLat" runat="server" CssClass="Validators" ControlToValidate="txtLatitud"
                                ErrorMessage="-99999.9999 - 99999.9999" Display="Dynamic" Type="Double" MaximumValue="9999.9999999999"
                                MinimumValue="-9999.9999999999"></asp:RangeValidator>
                        </td>
                        <td style="width: 146px" align="right">
                            <asp:Label ID="lblLongitud" runat="server" EnableViewState="False" CssClass="clsLabel">Longitud</asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtLongitud" runat="server" MaxLength="15" Columns="12"></asp:TextBox><asp:RangeValidator
                                ID="rvLong" runat="server" CssClass="Validators" ControlToValidate="txtLongitud"
                                ErrorMessage="-9999.9999 - 9999.9999" Display="Dynamic" Type="Double" MaximumValue="9999.9999999999"
                                MinimumValue="-9999.9999999999"></asp:RangeValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="dgitem" align="center" colspan="4">
                            <asp:Label ID="lbltitlepolity" runat="server" EnableViewState="False" CssClass="clsdarklabel">Cancellation Policy:</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblCancel" runat="server" EnableViewState="False" CssClass="clsLabel">Cancell</asp:Label>
                        </td>
                        <td colspan="3">
                            <table>
                                <tr>
                                    <td align="right" width="25%">
                                        <asp:DropDownList ID="ddlCancelationPolicy" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                    <td align="center" width="15%">
                                        <asp:Label ID="lblAux" runat="server" CssClass="clsLabel"></asp:Label>
                                    </td>
                                    <td align="right" width="25%">
                                        <asp:TextBox ID="txtCancellationPolicy" runat="server" CssClass="textbox" Width="56px"
                                            MaxLength="3" Columns="3"></asp:TextBox><asp:DropDownList ID="ddlHour" runat="server">
                                                <asp:ListItem Value="01">01</asp:ListItem>
                                                <asp:ListItem Value="02">02</asp:ListItem>
                                                <asp:ListItem Value="03">03</asp:ListItem>
                                                <asp:ListItem Value="04">04</asp:ListItem>
                                                <asp:ListItem Value="05">05</asp:ListItem>
                                                <asp:ListItem Value="06">06</asp:ListItem>
                                                <asp:ListItem Value="07">07</asp:ListItem>
                                                <asp:ListItem Value="08">08</asp:ListItem>
                                                <asp:ListItem Value="09">09</asp:ListItem>
                                                <asp:ListItem Value="10">10</asp:ListItem>
                                                <asp:ListItem Value="11">11</asp:ListItem>
                                                <asp:ListItem Value="12">12</asp:ListItem>
                                                <asp:ListItem Value="13">13</asp:ListItem>
                                                <asp:ListItem Value="14">14</asp:ListItem>
                                                <asp:ListItem Value="15">15</asp:ListItem>
                                                <asp:ListItem Value="16">16</asp:ListItem>
                                                <asp:ListItem Value="17">17</asp:ListItem>
                                                <asp:ListItem Value="18">18</asp:ListItem>
                                                <asp:ListItem Value="19">19</asp:ListItem>
                                                <asp:ListItem Value="20">20</asp:ListItem>
                                                <asp:ListItem Value="21">21</asp:ListItem>
                                                <asp:ListItem Value="22">22</asp:ListItem>
                                                <asp:ListItem Value="23">23</asp:ListItem>
                                            </asp:DropDownList>
                                        <asp:Label ID="lblSep" runat="server">:</asp:Label><asp:DropDownList ID="ddlMinutes"
                                            runat="server">
                                            <asp:ListItem Value="00">00</asp:ListItem>
                                            <asp:ListItem Value="15">15</asp:ListItem>
                                            <asp:ListItem Value="30">30</asp:ListItem>
                                            <asp:ListItem Value="45">45</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td width="35%">
                                        <asp:Label ID="lblEDaysHour" runat="server" CssClass="clslabel">Days / Hours</asp:Label><asp:Label
                                            ID="lblErrorHours" runat="server" CssClass="validators" Visible="False"></asp:Label><asp:RangeValidator
                                                ID="RVCancelation" runat="server" CssClass="validators" ControlToValidate="txtCancellationPolicy"
                                                ErrorMessage="0-999" Display="Dynamic" MaximumValue="9999" MinimumValue="0"></asp:RangeValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:CheckBox runat="server" ID="chkNonCancelable" Text="No Cancelable" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblCancelPolitiesReview" runat="server" EnableViewState="False" CssClass="clsLabel">Políticas de Cancelación Review</asp:Label>
                        </td>
                        <td style="text-align: left;" colspan="3">
                            <uc2:CtrlIdioma ID="txtCancelPolitiesReview" runat="server" RequiredText="true"></uc2:CtrlIdioma>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblCancelPolitiesFull" runat="server" EnableViewState="False" CssClass="clsLabel">Políticas de Cancelación Full</asp:Label>
                        </td>
                        <td style="text-align: left;" colspan="3">
                            <p>
                                <uc2:CtrlIdioma ID="txtCancelPolitiesFull" runat="server" RequiredText="true"></uc2:CtrlIdioma>
                            </p>
                        </td>
                    </tr>

</table>
<table cellspacing="0" cellpadding="3" width="700" align="left" border="0" id="tblAmenidades" runat="server">

    <tr>
        <td class="dgItem" align="center" colspan="4">
            <asp:Label ID="lblAmenidades" runat="server">Amenidades de la propiedad</asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <TABLE id="TABLE1" cellSpacing="0" cellPadding="2" width="650" border="0">
				<tr>
					<td>
						<TABLE id="Table2" cellspacing=0  cellPadding="0" width="100%" border="0">
							<tr>
								<td>
									<TABLE cellspacing=0 cellPadding="0" width="100%" border="0">
                                        <asp:Panel ID="amenitiesPanel" runat="server"></asp:Panel>
										    <!--tr>
											    <td class="clsdarklabel">
											        <asp:Label ID="lblCatAmenidad" runat="server" CssClass="DarkLabel"> Categoria</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="amenidad1" runat="server" CssClass="checkbox" />
                                                    <asp:Label ID="lblAmenidad1" runat="server" CssClass="Label"> Amenidad</asp:Label>
                                                </td>

                                            </tr-->
										    <TR>
											    <TD>

											    </TD>
										    </TR>
									</TABLE>
								</td>
							</tr>
                        </TABLE>
                        </td>
                    </tr>
							<tr>
								<td>
								</td>
							</tr>
						</TABLE>
					</td>
				</tr>
</TABLE>

<table cellspacing="0" cellpadding="3" width="700" align="left" border="0" id="tblRates" runat="server">

    <tr>
        <td class="dgItem" align="center" colspan="4">
            <asp:Label ID="lblRates" runat="server">Tarifa de la propiedad</asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <table border="0" cellSpacing="1" cellPadding="1" width="100%">
				<tr>
					<td><asp:label id="lblStartDate" runat="server" CssClass="clslabel" EnableViewState="False">Desde:</asp:label></td>
					<td><a hidefocus onclick="if(self.gfPop)gfPop.fPopCalendar(document.getElementById('<%response.write(txtDateTo.ClientId)%>'),document.getElementById('<%response.write(txtDateFrom.ClientId)%>'));return false;"
href="javascript:void(0)">
<asp:TextBox ID="txtDateFrom" runat="server" CssClass="textbox" Columns="10" Width="84px"
    MaxLength="10">

</asp:TextBox>
<img class="PopcalTrigger" alt="" src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>'
        align="absMiddle" border="0">
</a>
					</td>
                    
                    <td>
                        <asp:Label id="lblAdult" runat="server" CssClass="Label" >Adultos</asp:Label>
                        <asp:TextBox id="txtAdult" runat="server" CssClass="TextBox" width ="60px" />
                        
                        <asp:Label id="curr1" runat="server" CssClass="Label" >EUR</asp:Label>
                    </td>
				</tr>
                <tr>

                    <td>
                        <asp:label id="lblEndDate" runat="server" CssClass="clsLabel" EnableViewState="False">Hasta:</asp:label>
                        

                    </td>
                    <td><a hidefocus onclick="if(self.gfPop)gfPop.fPopCalendar1(document.getElementById('<%response.write(txtDateTo.ClientId)%>'));return false;"
                                                        href="javascript:void(0)">
                                                        <asp:TextBox ID="txtDateTo" runat="server" CssClass="textbox" Columns="10" Width="84px" MaxLength="10">
                                                        </asp:TextBox>
                                                        
                                                        <img class="PopcalTrigger" alt="" src='<%=GeRequestApplicationPath("/Calendar/calbtn.gif")%>'
                                                                align="absMiddle" border="0">

                                                    </a>
					</td>
                    
                    <td>
                        <asp:Label id="lblChild" runat="server" CssClass="Label" >Niños</asp:Label>
                        <asp:TextBox id="txtChild" runat="server" CssClass="TextBox" width ="60px" />
                        <asp:Label id="Curr2" runat="server" CssClass="Label" >EUR</asp:Label>
                    </td>
                </tr>
				<tr>
					<td colSpan="2"><asp:label id="lblDateErrorSign" runat="server" CssClass="Validators" EnableViewState="False"
							Visible="False">Fecha Invalida</asp:label></td>
				</tr>
			</table>
        </td>
    </tr>
    <tr>
					<td align=center>
						<asp:button id="cmdCancelar" runat="server" CssClass="Button" Text="Cancelar" CausesValidation="false"></asp:button>
						<asp:Button id="cmdAceptar" runat="server" Text="Guardar" CssClass="Button"></asp:Button>
						<asp:Button id="cmbPublish" runat="server" Text="Guardar y Publicar" CssClass="Button" Visible="false"></asp:Button>
					</td>
				</tr>
</table>
<asp:Label ID="lblError" runat="server"></asp:Label>

<uc2:CtrlPreserveScrolls ID="CtrlPreserveScrolls1" runat="server" />

					</td>
				</tr>
				
			</TABLE>
		</form>

         <script type="text/javascript">




            
        
             function FireUpdatingRates(chk, chksrc, id, idsrc) {
                 var echk = document.getElementById(chk);
                 var echks = document.getElementById(chksrc);
                 var e = document.getElementById(id);
                 var es = document.getElementById(idsrc);
                 if (echk && echks && e && es) {
                     if ((echk.checked != echks.checked) || (e.value != es.value)) {
                         e = document.getElementById('hiddenPostBack');
                         if (e) { e.value= confirm(resource); }
                         return true;
                     }
                 }            
                 return true;
             }
        
             function LoadMsg(ddl, lblAux, lblDay, dia, hora, specific, aux, canc) {
                 var l1 = document.getElementById(lblDay);
                 var l2 = document.getElementById(lblAux);
                 var d = document.getElementById(ddl);
                 var txt = document.getElementById('txtCancellationPolicy');
                 var ddl1 = document.getElementById('ddlHour');
                 var ddl2 = document.getElementById('ddlMinutes');
                 var lbls = document.getElementById('lblSep');

                 txt.style.display = 'block';
                 ddl1.style.display = 'none';
                 ddl2.style.display = 'none';
                 lbls.style.display = 'none';
                 switch (d.selectedIndex) {
                     case 0:
                         l1.firstChild.nodeValue = dia;
                         l2.firstChild.nodeValue = canc;

                         break;
                     case 1:
                         l1.firstChild.nodeValue = hora;
                         l2.firstChild.nodeValue = canc;
                         break;
                     case 2:
                         l1.firstChild.nodeValue = specific;
                         l2.firstChild.nodeValue = aux;
                         txt.style.display = 'none';
                         ddl1.style.display = 'block';
                         ddl2.style.display = 'block';
                         lbls.style.display = 'block';
                         break;
                 }

             }
    </script>
	</body>
</HTML>
