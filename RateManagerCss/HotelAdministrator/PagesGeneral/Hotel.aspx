<%@ Register TagPrefix="uc1" TagName="CtrlIdioma" Src="../../Modulos/CtrlIdioma.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Hotel.aspx.vb" Inherits="RateManager.Hotel" %>

<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../../Portal/Modules/ctrlHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrMenu" Src="../../Portal/Modules/CtrMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../../Portal/Modules/ctrlFooter.ascx" %>
<%@ Register src="../../Modulos/ctlMensajes.ascx" tagname="ctlMensajes" tagprefix="uc2" %>
<html>
<head>
    <title>Hotel</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
    <script src="<%= Me.ResolveUrl("~/Pages/Scripts/jquery.min.js") %>" type="text/javascript"></script>
</head>
<body bottommargin="0" leftmargin="0" rightmargin="0">
    <form id="Form1" method="post" runat="server">
    <div id="tituloSeccion">
        <img    class="bgplus"   src="../../Includes/imagenes/icono-big-plus.png" width="25" height="25" />
        <asp:Label ID="lblTitle" class="tituloSeccion" runat="server" EnableViewState="False">Hotel Configuration</asp:Label>
    </div>
    <table id="bookingcontainer" cellspacing="0" cellpadding="2" width="720"
        border="0">
        <tr>
            <td>
                <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">                    
                    <tr>
                        <td align="right">
                            <asp:Label Style="z-index: 0" ID="lblEsmoroso" runat="server" EnableViewState="False"
                                CssClass="clsLabel">Es Moroso :</asp:Label>
                        </td>
                        <td style="width: 178px" align="left">
                            <asp:CheckBox ID="chkEsMoroso" runat="server"></asp:CheckBox>
                        </td>
                        <td style="width: 146px" align="right">
                            <asp:Label Style="z-index: 0" ID="lblEsPagoCero" runat="server" EnableViewState="False"
                                CssClass="clsLabel">Es Pago Cero :</asp:Label>
                        </td>
                        <td align="left">
                            <asp:CheckBox ID="chkEspagoCero" runat="server"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblCategoria" runat="server" EnableViewState="False" CssClass="clsLabel">Categoria :</asp:Label>
                        </td>
                        <td style="width: 178px" align="left" >
                            <asp:DropDownList ID="cmbCategoria" runat="server" Width="154px">
                                <asp:ListItem Value="1">1 Estrella</asp:ListItem>
                                <asp:ListItem Value="2">2 Estrellas</asp:ListItem>
                                <asp:ListItem Value="3">3 Estrellas</asp:ListItem>
                                <asp:ListItem Value="4">4 Estrellas</asp:ListItem>
                                <asp:ListItem Value="5">5 Estrellas</asp:ListItem>
                                <asp:ListItem Value="0">Especial</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width: 146px" align="right">
                            <asp:Label ID="lblMoneda" runat="server" EnableViewState="False" CssClass="clsLabel">Moneda :</asp:Label>
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="cmbMonedas" runat="server" Width="155px" AutoPostBack="True">
                            </asp:DropDownList>
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
                        <td style="width: 146px" align="right">
                            <asp:Label ID="lblCheckout" runat="server" EnableViewState="False" CssClass="clsLabel">Checkout  :</asp:Label>
                        </td>
                        <td align="left">
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
                                ID="RequiredFieldValidator8" runat="server" CssClass="Validators" ControlToValidate="txtImpuesto"
                                ErrorMessage="Impuesto es requerido" Display="Dynamic">*</asp:RequiredFieldValidator><asp:RangeValidator
                                    ID="RangeValidator9" runat="server" CssClass="Validators" ControlToValidate="txtImpuesto"
                                    ErrorMessage="Impuesto es numerico (1-99)" Display="Dynamic" Type="Double" MaximumValue="99"
                                    MinimumValue="0">*</asp:RangeValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblPerfil" runat="server" EnableViewState="False" CssClass="clsLabel">Perfil de Usuarios</asp:Label>
                        </td>
                        <td style="width: 178px" align="left">
                            <asp:DropDownList ID="ddlPerfil" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 146px" align="right">
                            <asp:Label ID="lblCommision" runat="server" EnableViewState="False" CssClass="clsLabel">Standard Travel Agent Commission %:</asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtCommision" runat="server" CssClass="textbox" Width="58px" MaxLength="5"></asp:TextBox>                            
                            <asp:RequiredFieldValidator
                                ID="RequiredFieldValidator12" runat="server" CssClass="Validators" ControlToValidate="txtCommision"
                                ErrorMessage="Impuesto es requerido" Display="Dynamic">*</asp:RequiredFieldValidator><asp:RangeValidator
                                    ID="RangeValidator13" runat="server" CssClass="Validators" ControlToValidate="txtCommision"
                                    ErrorMessage="Impuesto es numerico (1-99)" Display="Dynamic" Type="Double" MaximumValue="99"
                                    MinimumValue="0">(0-99)</asp:RangeValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblAmhm" runat="server" CssClass="clsLabel">Amhm Reservations</asp:Label>
                        </td>
                        <td style="width: 178px" align="left">
                            <asp:CheckBox ID="chkAmhm" runat="server" CssClass="clsLabel"></asp:CheckBox>
                        </td>
                        <td style="width: 146px" align="right">
                            <asp:Label ID="lblEmprTour" runat="server" CssClass="clsLabel">EMPRtour</asp:Label>
                        </td>
                        <td align="left">
                            <asp:CheckBox ID="chkEmprTour" runat="server" CssClass="clsLabel"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 11px" align="right">
                            <asp:Label ID="lblAvailOnGDS" runat="server" CssClass="clsLabel">Avail On GDS</asp:Label>                            
                        </td>
                        <td style="height: 11px" align="left">
                            <asp:CheckBox ID="chkAvailOnGDS" runat="server" CssClass="clsLabel"></asp:CheckBox>
                        </td>
                        <td style="height: 11px" align="right">
                            <asp:Label ID="lblAvailOnPortal" runat="server" CssClass="clsLabel">Available on Portal</asp:Label>
                        </td>
                        <td style="height: 11px" align="left">
                            <asp:CheckBox ID="chkAvailOnPortal" runat="server" CssClass="clsLabel"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 16px" align="right">
                            <asp:Label ID="lblAvailOnOnePage" runat="server" CssClass="clsLabel">Avail On One Page</asp:Label>
                        </td>
                        <td style="height: 16px" align="left">
                            <asp:CheckBox ID="chkAvailOnOnePage" runat="server"></asp:CheckBox>
                        </td>
                        <td style="height: 16px" align="right">
                            <asp:Label ID="lblCadena" runat="server" CssClass="clsLabel">lblCadena</asp:Label>
                        </td>
                        <td style="height: 16px" align="left">
                            <asp:DropDownList ID="ddlCorporativos" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 16px" align="right">
                            <asp:Label ID="lblAvailOnADS" runat="server" CssClass="clsLabel">Avail On ADS</asp:Label>
                        </td>
                        <td style="height: 16px" align="left">
                            <asp:CheckBox ID="chkAvailOnADS" runat="server" CssClass="clsLabel"></asp:CheckBox>
                        </td>
                        <td style="height: 16px" align="right">
                            <asp:Label ID="lblAvlOnCorpModule" runat="server" CssClass="clsLabel" Visible="false">Avail On Corporate Module</asp:Label>
                            <asp:Label ID="lblSaveCurrencyShonw" runat="server" CssClass="clsLabel" Visible="true">Save Currency Shown :</asp:Label>
                        </td>
                        <td style="height: 16px" align="left">
                            <asp:CheckBox ID="chkAvlOnCorpModule" runat="server" CssClass="clsLabel" Visible="false"></asp:CheckBox>
                            <asp:CheckBox ID="chkSaveCurrencyShown" runat="server" CssClass="clsLabel" Visible="true"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 16px" align="right">
                            <asp:Label ID="lblSingleImgInv" runat="server" CssClass="clsLabel" Visible="false">Single Image Inventory: </asp:Label>
                        </td>
                        <td style="height: 16px" align="left">
                            <asp:CheckBox ID="chkSingleImgInv" runat="server" CssClass="clsLabel" Visible="false"></asp:CheckBox>
                        </td>
                        <td style="height: 16px" align="right">
                            <asp:Label ID="lblPushNotif" runat="server" CssClass="clsLabel" Visible="false">Notificación Push PMS: </asp:Label>
                        </td>
                        <td style="height: 16px" align="left">
                            <asp:CheckBox ID="chkPushNotif" runat="server" CssClass="clsLabel" Visible="false"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                       <td style="height:16px" align="right"></td>
                       <td style="height:16px" align="left"></td>
                       <td style="height:16px" align="right">
                            <asp:Label ID="lblEcotasa" runat="server" CssClass="clsLabel" Visible="true">ECOTASA $:</asp:Label>
                       </td>
                        <td>
                            <asp:TextBox ID="txtEcotasa" runat="server" CssClass="textbox" Width="58px" MaxLength="5"></asp:TextBox>
                            <asp:Label ID="lblEcotasaMessage" runat="server" CssClass="clsLabel" Visible="true">Per room per night</asp:Label>
                            <asp:RangeValidator
                                    ID="RangeValidatorTxtEcotasa" runat="server" CssClass="Validators" ControlToValidate="txtEcotasa"
                                    ErrorMessage="Ecotasa es numerico (1-99)" Display="Dynamic" Type="Double" MaximumValue="99"
                                    MinimumValue="0">(0-99)</asp:RangeValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="dgitem" align="center" colspan="4">
                            <asp:Label ID="lblConfirmationEmail" runat="server" EnableViewState="False">Confirmation Email</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblEmailLanguage" runat="server" EnableViewState="False" CssClass="clslabel">indicate the language of the mail:</asp:Label>
                        </td>
                        <td style="width: 178px" align="left">
                            <asp:DropDownList ID="ddlEmailLanguage" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 146px" align="right">
                            <asp:Label ID="lblFaxEmail" style="display:none" runat="server" EnableViewState="False">Enviar Por Fax las reservaciones</asp:Label>
                        </td>
                        <td align="left">
                            <asp:CheckBox ID="chkFaxSend" runat="server"  style="display:none"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblEmail" runat="server" EnableViewState="False" CssClass="clsLabel"
                                Width="150px">Indicate Email From Reservations:</asp:Label>
                        </td>
                        <td style="width: 178px" align="left" colspan="3">
                            <asp:Label ID="lblErrorMail" runat="server" CssClass="Validators" Visible="False">Label</asp:Label><asp:TextBox
                                ID="txtEmailReservas" runat="server" CssClass="textbox" Width="464px" MaxLength="200"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                        </td>
                        <td colspan =3>
                            <asp:CheckBox ID="chkTransUp" runat="server" Text="Send transactions update by Email" CssClass="clsLabel">
                            </asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="dgItem" align="center" colspan="4">
                            <asp:Label ID="lblRules" runat="server" EnableViewState="False" CssClass="clsLabel">Reservation rules </asp:Label>
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
                    <tr><td></td><td></td><td align="right">
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
                        <td align="right" valign="top">
                            <asp:Label ID="lblMinCuartos" runat="server" EnableViewState="False" CssClass="clsLabel">Minimum number of rooms to reserve</asp:Label>
                        </td>
                        <td align="left" width="178">
                            <asp:TextBox ID="txtMinCuartos" runat="server" Width="32px" MaxLength="2">1</asp:TextBox><asp:RangeValidator
                                ID="Rangevalidator3" runat="server" CssClass="Validators" ControlToValidate="txtMinCuartos"
                                ErrorMessage="1-99" Type="Integer" MaximumValue="99" MinimumValue="1"></asp:RangeValidator>
                        </td>
                        <td style="width: 146px" align="right" valign="top">
                            <asp:Label ID="lblMaxCuartos" runat="server" EnableViewState="False" CssClass="clsLabel">Maximum number of rooms to reserve</asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtMaxCuartos" runat="server" Width="32px" MaxLength="2">10</asp:TextBox>
                            
                            <asp:RangeValidator
                                ID="RvMaxCuartos" runat="server" CssClass="Validators" ControlToValidate="txtMaxCuartos"
                                ErrorMessage="1-99" Type="Integer" MaximumValue="99" MinimumValue="1"></asp:RangeValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top">
                            <asp:Label ID="lblMinOcupacion" runat="server" EnableViewState="False" CssClass="clsLabel">Ocupacion minima:</asp:Label>
                        </td>
                        <td align="left" width="178">
                            <asp:TextBox ID="txtMinOcupacion" runat="server" CssClass="textbox" Width="51px"
                                MaxLength="2"></asp:TextBox>
                            <asp:RangeValidator ID="Rangevalidator4" runat="server" CssClass="Validators" ControlToValidate="txtMinOcupacion"
                                ErrorMessage="1-99" Type="Integer" MaximumValue="99" MinimumValue="1"></asp:RangeValidator>
                        </td>
                        <td style="width: 146px" align="right" valign="top">
                            <asp:Label ID="lblMaxOcupacion" runat="server" EnableViewState="False" CssClass="clsLabel">Edad Maxima de Niño :</asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtMaxOcupacion" runat="server" CssClass="textbox" Width="51px"
                                MaxLength="2"></asp:TextBox>
                            <asp:RangeValidator ID="Rangevalidator6" runat="server" CssClass="Validators" ControlToValidate="txtMaxOcupacion"
                                ErrorMessage="1-99" Type="Integer" MaximumValue="99" MinimumValue="1"></asp:RangeValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblAllowDeposit" runat="server" CssClass="clsLabel">Allow deposit</asp:Label>
                        </td>
                        <td align="left" width="178">
                            <asp:CheckBox ID="chkAllowDeposit" runat="server"></asp:CheckBox>
                        </td>
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
                            <table id="Table3" cellspacing="1" cellpadding="1" border="0" frame="void">
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
                            <uc1:CtrlIdioma ID="txtCancelPolitiesReview" runat="server" RequiredText="true"></uc1:CtrlIdioma>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblCancelPolitiesFull" runat="server" EnableViewState="False" CssClass="clsLabel">Políticas de Cancelación Full</asp:Label>
                        </td>
                        <td style="text-align: left;" colspan="3">
                            <p>
                                <uc1:CtrlIdioma ID="txtCancelPolitiesFull" runat="server" RequiredText="true"></uc1:CtrlIdioma>
                            </p>
                        </td>
                    </tr>
                    <tr>
                        <td class="dgItem" align="center" colspan="4">
                            <asp:Label ID="lblConfigGDS" runat="server" EnableViewState="False" CssClass="clsLabel"> GDS Configuration</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblPropertyNumber" runat="server" EnableViewState="False" CssClass="clslabel"
                                DESIGNTIMEDRAGDROP="820">Property number:</asp:Label>
                        </td>
                        <td style="width: 178px" align="left">
                            <asp:TextBox ID="txtPropertyNumber" runat="server" CssClass="textbox" Width="92px"
                                MaxLength="5" Columns="6" DESIGNTIMEDRAGDROP="818"></asp:TextBox>
                        </td>
                        <td style="width: 146px" align="right">
                            <asp:Label ID="lblChainCode" runat="server" EnableViewState="False" CssClass="clslabel">Chain Code:</asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtChainCode" runat="server" CssClass="textbox" Width="58px" 
                                MaxLength="2"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblGetRates" runat="server" EnableViewState="False" CssClass="clslabel">Get Rates:</asp:Label>
                        </td>
                        <td style="width: 178px" align="left">
                            <asp:RadioButton ID="rbGetRates_True" runat="server" CssClass="clsLabel" Text="Yes"
                                GroupName="GetRates"></asp:RadioButton><asp:RadioButton ID="rbGetRates_False" runat="server"
                                    CssClass="clsLabel" Text="No" GroupName="GetRates" Checked="True"></asp:RadioButton>
                        </td>
                        <td style="width: 146px" align="right">
                        </td>
                        <td align="left">
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblIdGal" runat="server" EnableViewState="False" CssClass="clsLabel">Galileo:</asp:Label>
                        </td>
                        <td style="width: 178px" align="left">
                            <asp:TextBox ID="txtIdGal" runat="server" CssClass="textbox" Width="144px" MaxLength="12"></asp:TextBox><asp:Label
                                ID="lblErrorPGalileo" runat="server" CssClass="Validators" Visible="False" ForeColor="Red">ERROR</asp:Label>
                        </td>
                        <td style="width: 146px" align="right">
                            <asp:Label ID="lblIdWorldSpan" runat="server" EnableViewState="False" CssClass="clsLabel">World Span:</asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtIdWorldSpan" runat="server" CssClass="textbox" Width="144px"
                                MaxLength="12"></asp:TextBox><asp:Label ID="lblErrorPWorld" runat="server" CssClass="Validators"
                                    Visible="False" ForeColor="Red">ERROR</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblIdSabre" runat="server" EnableViewState="False" CssClass="clsLabel">Sabre:</asp:Label>
                        </td>
                        <td style="width: 178px" align="left">
                            <asp:TextBox ID="txtIdSabre" runat="server" CssClass="textbox" Width="144px" MaxLength="12"></asp:TextBox><asp:Label
                                ID="lblErrorPSabre" runat="server" CssClass="Validators" Visible="False" ForeColor="Red">ERROR</asp:Label>
                        </td>
                        <td style="width: 146px" align="right">
                            <asp:Label ID="lblIdAmadeus" runat="server" EnableViewState="False" CssClass="clsLabel">Amadeus:</asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtIdAmadeus" runat="server" CssClass="textbox" Width="144px" MaxLength="12"></asp:TextBox><asp:Label
                                ID="lblErrorPAmadeus" runat="server" CssClass="Validators" Visible="False" ForeColor="Red">ERROR</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblCreditCardPolicies" runat="server" EnableViewState="False" CssClass="clsLabel">Políticas de Tarjeta de Crédito</asp:Label>
                        </td>
                        <td style="text-align: left;" colspan="3">
                            <p>
                                <uc1:CtrlIdioma ID="txtCreditCardPolicies" runat="server" RequiredText="true"></uc1:CtrlIdioma>
                            </p>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblGuarantyPolicies" runat="server" EnableViewState="False" CssClass="clsLabel">Políticas de Garantía</asp:Label>
                        </td>
                        <td style="text-align: left;" colspan="3">
                            <p>
                                <uc1:CtrlIdioma ID="txtGuarantyPolicies" runat="server" RequiredText="true"></uc1:CtrlIdioma>
                            </p>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblExtraCharges" runat="server" EnableViewState="False" CssClass="clsLabel">Cargos Extras</asp:Label>
                        </td>
                        <td style="text-align: left;" colspan="3">
                            <p>
                                <uc1:CtrlIdioma ID="txtExtraCharges" runat="server"></uc1:CtrlIdioma>
                            </p>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td colspan="3">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                            <asp:Label ID="lblError" runat="server" CssClass="Validators" Visible="False" ForeColor="Red">ERROR</asp:Label><asp:Label
                                ID="lblErrorDate" runat="server" CssClass="Validators" Visible="False">ERROR</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblServiceCharge" runat="server" EnableViewState="False" CssClass="clsLabel"
                                Visible="False" DESIGNTIMEDRAGDROP="50">Service charge:</asp:Label>
                        </td>
                        <td style="width: 178px" align="left">
                            <asp:TextBox ID="txtServiceCharge" runat="server" CssClass="textbox" Width="58px"
                                MaxLength="4" Visible="False" DESIGNTIMEDRAGDROP="52"></asp:TextBox><asp:Label ID="Label2"
                                    runat="server" CssClass="clsLabel" Visible="False" DESIGNTIMEDRAGDROP="53" Font-Bold="True">dlls</asp:Label>
                        </td>
                        <td style="width: 146px" align="right">
                        </td>
                        <td align="left">
                        </td>
                    </tr>
	                <% If (Not Me.txtCancelPolitiesReview.Published OrElse Not Me.txtCancelPolitiesFull.Published OrElse Not Me.txtCreditCardPolicies.Published OrElse Not Me.txtGuarantyPolicies.Published OrElse Not Me.txtExtraCharges.Published) Then%>
	                <tr>
	                    <td colspan="4">
	                        <span class="validators"><%=RateManager.PortalCulture.GetString(If(CType(Me.Page, RateManager.PaginaBase).IsSupervisor, "01365", "01392"))%></span>
	                    </td>
	                </tr>
	                <% end if %>	
                    <tr>
                        <td align="center" colspan="4">
                            <input id = "hiddenPostBack" type=hidden runat=server />
                            
                            <% If me.issupervisor then %>
                            <asp:Button ID="btnPublsh" runat="server" CssClass="Button" Width="160px" Text="Publish">
                            </asp:Button>
                            <% end if %>
                            <asp:Button ID="btnSave" runat="server" CssClass="Button" Width="160px" Text="Guardar cambios">
                            </asp:Button>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <uc2:ctlMensajes ID="ctlMensajes1" runat="server" />
    </form>

    

    <script type="text/javascript">




        $(document).ready(function() {
            $('#<%= Me.btnSave.ClientId %> ,#<%= Me.btnPublsh.ClientId %>').click(function() {
                var echk = $('#<%= Me.chkPlusTax.ClientId %>');
                var echks = $('#<%= Me.chkPlusTaxSrc.ClientId %>');
                var e = $('#<%= Me.txtImpuesto.ClientId %>');
                var es = $('#<%= Me.txtImpuestoSrc.ClientId %>');
                if (echk.length > 0 && echks.length > 0 && e.length > 0 && es.length > 0) {
                    if ((echk[0].checked != echks[0].checked) || (e.val() != es.val())) {
                        e = $('#hiddenPostBack');
                        if (e.length > 0) { e.val(confirm('<%=RateManager.PortalCulture.GetString("01366")%>')); }
                        return true;
                    }
                }
                return true;                
            });
        });
        /*var resource = '<%=RateManager.PortalCulture.GetString("01366")%>'
        
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
        }*/
        
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
</html>
