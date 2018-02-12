<%@ Page Language="vb" AutoEventWireup="false" EnableEventValidation="false" CodeBehind="PaymentRegistration.aspx.vb"
    Inherits="RateManager.PaymentRegistration" %>

<%@ Register TagPrefix="uc1" TagName="ctrHotelsUserChain" Src="../../../Modulos/ctrHotelsUserChain.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../../../Portal/Modules/ctrlHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../../../Portal/Modules/ctrlFooter.ascx" %>
<%@ Register TagPrefix="uc1" TagName="HotelBalanceViewer" Src="../Modules/HotelBalanceViewer.ascx" %>
<%@ Import Namespace="RateManager" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>PaymentRegistration</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../../../StyleSheets/Styles.css" type="text/css" rel="stylesheet">

    <script src="../Script/Util.js"></script>

    <script language="javascript" type="text/javascript">



        function ddlChangeBank(strNoAcc) {

            var array = null;
            var ddl = document.getElementById("ddlAccountNumber");
            var ddlBank = document.getElementById("ddlBankName");
            var hdn = document.getElementById("hdnAccountNumber");
            var swFound = 0;
            if (ddl && ddlBank) {
                var idx = 0;
                //ddlClear(ddl);
                ddl.options.length = 0;
                array = AccountsArray;
                for (var i = 0; i < array.length; i++) {
                    if (array[i][0] == ddlBank.value) {
                        var opt = document.createElement("option");
                        opt.value = array[i][1];
                        opt.text = array[i][2];
                        ddl.options.add(opt, idx);
                        swFound = 1;
                        idx++;
                        disabledEnabled(false);
                    }
                }
                if (!swFound) {
                    var opt = document.createElement("option");
                    opt.value = -1;
                    opt.text = strNoAcc;
                    ddl.options.add(opt);
                    disabledEnabled(true);
                }
                ddlChangeAccounts();
                hdn.value = ddl.options[0].value;
            }
        }

        function ddlClear(ddl) {
            while (ddl.options.length > 0) ddl.options.remove(0);
        }

        function onTransactionBankChanged(selectId, inputId) {
            var select = document.getElementById(selectId);
            var input = document.getElementById(inputId);
            if (select && input) {
                input.value = (select.value != 'Otro') ? select.value : "";
                input.style.display = (select.value == 'Otro') ? "block" : "none";
            }
        }

        function disabledEnabled(value) {
            var txt = document.getElementById("txtTransactionNumber");
            txt.disabled = value;
            txt = document.getElementById("txtAmount");
            txt.disabled = value;
            /*txt = document.getElementById("txtRefCompanyID");
            txt.disabled = value;
            txt = document.getElementById("txtRefFolio");
            txt.disabled = value;*/
            txt = document.getElementById("txtDate");
            txt.disabled = value;
            txt = document.getElementById("txtHour");
            txt.disabled = value;
            txt = document.getElementById("txtMinutes");
            txt.disabled = value;
            txt = document.getElementById("txtReferenceBank");
            txt.disabled = value;
            var ddlDis = document.getElementById("ddlPaymentType");
            ddlDis.disabled = value;
            ddlDis = document.getElementById("ddlTime");
            ddlDis.disabled = value;
            var btn = document.getElementById("btnRegister");
            btn.disabled = value;
        }

        function ddlChangeAccounts() {
            var array = null;
            var ddl = document.getElementById("ddlAccountNumber");
            var hidden = document.getElementById("hdnAccountNumber");
            var Clabe = document.getElementById("lblClabe");
            var AccountName = document.getElementById("lblAccountName");
            var Currency = document.getElementById("lblCurrency");
            var sClabe = document.getElementById("lblsClabe");
            var txt = document.getElementById("lblMoney");
            //var hdnAccCurr = document.getElementById("hdnAccountCurrency");

            var AccountNameAux = document.getElementById("lblAccountName.ClientID");


            var swAccounts = false;

            //trAccountName.style.display = "block";
            //trClabe.style.display = "block";

            if (ddl && hidden) {
                hidden.value = ddl.value;
            }
            if (Clabe && AccountName && Currency) {
                array = AccountsArray;
                for (var i = 0; i < array.length; i++) {
                    if (array[i][2] == ddl.options[ddl.selectedIndex].text) {

                        //trAccountName.style.display = "block";
                        AccountName.innerText = array[i][3] + "     ";
                        AccountName.innerHtml = array[i][3] + "     ";
                        AccountName.firstChild.nodeValue = array[i][3] + "     ";



                        Currency.innerText = array[i][5];
                        Currency.innerHtml = array[i][5];
                        Currency.firstChild.nodeValue = array[i][5];



                        if (array[i][4] != null && array[i][4] != '') {
                            Clabe.innerText = array[i][4];
                            Clabe.innerHtml = array[i][4];
                            Clabe.firstChild.nodeValue = array[i][4];
                            //trClabe.style.display = "block";
                        } else {
                            //trClabe.style.display = "none";
                        }
                        /*if (txt && hdnAccCurr){
                        txt.innerText = array[i][6];
                        hdnAccCurr.value = txt.innerText;								
                        }*/
                        swAccounts = true;
                    }
                }
                if (!swAccounts) {
                    //trAccountName.style.display = "none";
                    //trClabe.style.display = "none";
                    //txt.innerText = "";
                    //hdnAccCurr.value = "";
                }
            } else {
                //trAccountName.style.display = "none";
                //trClabe.style.display = "none";
                //txt.innerText = "";
                //hdnAccCurr.value = "";
            }
        }

    </script>

    </head>
<body bottommargin="0" leftmargin="0" topmargin="5" rightmargin="0">
    <form id="Form1" method="post" runat="server">
    <div class="mDiv">
    </div>
    <div class="title">
        <asp:Label ID="lblTitle" runat="server" EnableViewState="False" CssClass="tituloSeccion">Registro de Pagos</asp:Label>
    </div>
    </div>
    <table id="bookingcontainer" cellspacing="0" cellpadding="0" width="" align="center"
        border="0">
        <tr>
            <td colspan="3" align="center">
                <uc1:ctrHotelsUserChain ID="CtrHotelsUserChain1" runat="server"></uc1:ctrHotelsUserChain>
            </td>
        </tr>              
        <tr>
            <td width="15" height="10">
            </td>
            <td>
                <asp:Label ID="lblMsg" runat="server" CssClass="Validators" Visible="False" Width="250px"
                    Height="100%">Número de Referencia No Válido <br> [Pago no registrado]</asp:Label>
            </td>
            <td width="15" height="10">
            </td>
        </tr>
        <tr>
            <td width="15" height="10">
            </td>
            <td>
                <asp:Label ID="lblMsg2" runat="server" CssClass="Validators" Visible="False" Height="100%">Pago no registrado <BR> [El tipo de moneda para la factura con el numero de referencia dado <BR> no coincide con el tipo de moneda para la cuenta de banco seleccionada]</asp:Label>
            </td>
            <td width="15" height="10">
            </td>
        </tr>
        <tr>
            <td width="15" height="10">
            </td>
            <td height="10">
            </td>
            <td width="15" height="10">
            </td>
        </tr>
        <tr>
            <td width="15" height="20">
            </td>
            <td align="left">
                <table id="Table2" cellspacing="0" cellpadding="0" width="" border="0">
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblBankName" runat="server" CssClass="clsLabel" EnableViewState="False">Banco:</asp:Label>
                        </td>
                        <td class="textBox">
                            <asp:DropDownList ID="ddlBankName" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblAccountNumber" runat="server" CssClass="clsLabel" EnableViewState="False">Número de Cuenta:</asp:Label>
                        </td>
                        <td class="textBox">
                            <asp:DropDownList ID="ddlAccountNumber" runat="server">
                            </asp:DropDownList>
                            <input id="hdnAccountNumber" style="width: 8px; height: 22px" type="hidden" size="1"
                                name="hdnAccountNumber" runat="server">
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblsAccountName" runat="server" CssClass="clsLabel" EnableViewState="False">Nombre de Cuenta:</asp:Label>
                        </td>
                        <td>
                            <span id="lblAccountName">Cuenta</span> ( <span id="lblCurrency">Cuenta en Pesos</span>
                            )
                        </td>
                    </tr>
                    <tr id="trClabe">
                        <td align="right">
                            <asp:Label ID="lblsClabe" runat="server" CssClass="clsLabel" EnableViewState="False">CLABE:</asp:Label>
                        </td>
                        <td class="textBox">
                            <asp:Label ID="lblClabe" runat="server">Clabe</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblSourceBank" runat="server" CssClass="clsLabel" EnableViewState="False">Banco donde realizó el pago:</asp:Label>
                        </td>
                        <td>
                            <table id="Table10" cellspacing="0" cellpadding="0" border="0">
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlSourceBank" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                    <td width="5">
                                    </td>
                                    <td nowrap>
                                        <asp:TextBox ID="txtSourceBank" runat="server" CssClass="textBox"></asp:TextBox>
                                    </td>
                                    <td width="5">
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="rfvSourceBank" runat="server" CssClass="Validators"
                                            Height="100%" ControlToValidate="txtSourceBank" ErrorMessage="[*]" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblTransactionNumber" runat="server" CssClass="clsLabel" EnableViewState="False">Número de Transacción:</asp:Label>
                        </td>
                        <td>
                            <table id="Table8" cellspacing="0" cellpadding="0" border="0">
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtTransactionNumber" runat="server" CssClass="textBox" MaxLength="25"></asp:TextBox>
                                    </td>
                                    <td width="5">
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlPaymentType" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                    <td width="5">
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="Validators"
                                            Height="100%" ControlToValidate="txtTransactionNumber" ErrorMessage="[*]" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblReferenceBank" runat="server" EnableViewState="False" CssClass="clsLabel"> Referencia bancaria :</asp:Label>
                        </td>
                        <td valign="top">
                            <table id="Table1" cellspacing="0" cellpadding="0" border="0">
                                <tr>
                                    <td valign="top">
                                        <asp:TextBox ID="txtReferenceBank" runat="server" CssClass="textBox" MaxLength="20"></asp:TextBox>
                                    </td>
                                    <td width="5">
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        <p>
                                            <asp:RequiredFieldValidator ID="rfvReferenceBank" runat="server" CssClass="Validators"
                                                Height="100%" Display="Dynamic" ErrorMessage="[*]" ControlToValidate="txtReferenceBank"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvReferenceBank" runat="server" ErrorMessage="Referencia invalida"
                                                ClientValidationFunction="ClientValidate" ControlToValidate="txtReferenceBank"
                                                CssClass="Validators" Display="Dynamic"></asp:CustomValidator></p>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblsDateTime" runat="server" CssClass="clsLabel" EnableViewState="False">Fecha y Hora:</asp:Label>
                        </td>
                        <td valign="middle">
                            <table id="Table4" cellspacing="0" cellpadding="0" width="300" border="0">
                                <tr>
                                    <td style="height: 30px">
                                        <asp:TextBox ID="txtDate" runat="server" CssClass="textBox" Width="88px">mm/dd/aaaa</asp:TextBox><asp:Literal
                                            ID="litCalendar" runat="server"></asp:Literal>
                                    </td>
                                    <td style="height: 30px">
                                        <asp:TextBox ID="txtHour" runat="server" CssClass="textBox" Width="32px" MaxLength="2">12</asp:TextBox><asp:Label
                                            ID="Label4" runat="server" CssClass="bookingNormalLabel">:</asp:Label><asp:TextBox
                                                ID="txtMinutes" runat="server" CssClass="textBox" Width="32px" MaxLength="2">00</asp:TextBox>&nbsp;<asp:DropDownList
                                                    ID="ddlTime" runat="server">
                                                    <asp:ListItem Value="a.m.">am</asp:ListItem>
                                                    <asp:ListItem Value="p.m.">pm</asp:ListItem>
                                                </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" CssClass="Validators"
                                            Height="100%" ControlToValidate="txtDate" ErrorMessage="[mm/dd/aaaa]" Display="Dynamic"
                                            ValidationExpression="(0|1)\d/(0|1|2|3)\d/20(0|1|2|3|4|5)\d"></asp:RegularExpressionValidator>
                                    </td>
                                    <td>
                                        <asp:RangeValidator ID="rvHour" runat="server" CssClass="Validators" Height="100%"
                                            ControlToValidate="txtHour" ErrorMessage="[01-12]" Display="Dynamic" Type="Integer"
                                            MaximumValue="12" MinimumValue="01"></asp:RangeValidator><asp:RangeValidator ID="rvMinutes"
                                                runat="server" CssClass="Validators" Height="100%" ControlToValidate="txtMinutes"
                                                ErrorMessage="[00-59]" Display="Dynamic" Type="Integer" MaximumValue="59" MinimumValue="00"></asp:RangeValidator>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblAmount" runat="server" CssClass="clsLabel" EnableViewState="False">Cantidad Pagada: $</asp:Label>
                        </td>
                        <td>
                            <table id="Table9" cellspacing="0" cellpadding="0" border="0">
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtAmount" runat="server" CssClass="textBox" Width="86px" MaxLength="12"></asp:TextBox>
                                    </td>
                                    <td width="5">
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlCurrency" runat="server" Enabled="False">
                                        </asp:DropDownList>
                                    </td>
                                    <td width="5">
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" CssClass="Validators"
                                            Height="100%" Display="Dynamic" ErrorMessage="[*]" ControlToValidate="txtAmount"></asp:RequiredFieldValidator><asp:CompareValidator
                                                ID="cvAmountFormat" runat="server" CssClass="Validators" Height="100%" ControlToValidate="txtAmount"
                                                ErrorMessage="[Formato no válido]" Type="Currency" Operator="GreaterThan" ValueToCompare="0.0"></asp:CompareValidator>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 18px;   align="right">
                        </td>
                        <td style="height: 18px">
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan =2>
                            <table border="0" cellpadding="1" cellspacing="1">
                                <tr>
                                    <td>
                                        <div id="divConfirmation" style="position: absolute; padding-bottom: 0px; background-color: #ffffff;
                                            padding-left: 0px; width: 420px; padding-right: 0px; display: none; height: 180px;
                                            padding-top: 0px" ms_positioning="FlowLayout">
                                             <div class="boxMsg">
                                                <table id="Table7" cellspacing="1" cellpadding="1" 
                                            
                                                width="100%" border="0">
                                                <tr>
                                                    <td >
                                                        <table    id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                                            <tr>
                                                                <td class="titleboxMsg">
                                                                    <asp:Label ID="lblConfirmationTitle" runat="server" CssClass="bookingNormalLabel"
                                                                        EnableViewState="False">Registro de Pagos</asp:Label>
                                                                        <div style="float:right; ">
                                                                    <img id="IMG1" style="cursor: pointer" onclick="javascript:document.getElementById('divConfirmation').style.display = 'none';"
                                                                        alt="" src="../../../Images/close.png" runat="server">    
                                                                        </div>
                                                                </td>
                                                                <td align="right">
                                                                    
                                                                </td>
                                                            </tr>                                                            

                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <br>
                                                        <asp:Label ID="lblConfirmationPrompt" runat="server" CssClass="clsLabel" EnableViewState="False"> ¿Confirma que desa registrar este pago?</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <br>
                                                        <table id="Table3" cellspacing="1" cellpadding="1" width="100%" border="0">
                                                            <tr>
                                                                <td class="textBox" align="center" width="50%">
                                                                    <asp:LinkButton ID="lnkYes" runat="server" CssClass="dgLink">Si</asp:LinkButton>
                                                                </td>
                                                                <td class="textBox" align="center" width="50%">
                                                                    <asp:HyperLink ID="hypNo" runat="server" CssClass="dgLink" NavigateUrl="#">No</asp:HyperLink>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <br>
                                                    </td>
                                                </tr>
                                            </table>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <input class="button" id="btnRegister" onclick="javascript:document.getElementById('divConfirmation').style.display = 'block';"
                                            type="button" value="Registrar" name="btnRegister" runat="server">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                        </td>
                    </tr>
                </table>
            </td>
            <td width="15" height="20">
            </td>
        </tr>
        <tr>
            <td width="15" height="30">
            </td>
            <td height="30">
            </td>
            <td width="15" height="30">
            </td>
        </tr>
        <tr>
            <td width="15" height="20">
            </td>
            <td align="left">
                <table id="Table5" cellspacing="1" cellpadding="5" width="300" border="0">
                    <tr>
                        <td>
                            <asp:HyperLink ID="hplGoToTaskList" runat="server" CssClass="dgLink" EnableViewState="False">Ir a Lista de Tareas</asp:HyperLink>
                        </td>
                    </tr>
                </table>
            </td>
            <td width="15" height="20">
            </td>
        </tr>
        <tr>
            <td width="15" height="20">
            </td>
            <td>
            </td>
            <td width="15" height="20">
            </td>
        </tr>
    </table>
    </form>
    <div id="scriptContainer" runat="server">
    </div>
    <iframe id="gToday:normal:agenda.js" style="z-index: 999; left: -500px; position: absolute;
        top: -500px" name="gToday:normal:agenda.js" src='<%=GeRequestApplicationPath(string.concat("/Calendar/",portalculture.getculture().Name.Substring(0, 2).ToLower(),"/ipopeng2.htm" )) %>'
        frameborder="0" width="174" scrolling="no" height="189"></iframe>

    <script language="javascript">
        function ClientValidate(source, arguments) {
            var array = null;
            array = ReferencesArray;
            if (array) {
                arguments.IsValid = false;
                for (var i = 0; i < array.length; i++) {
                    if (array[i] == arguments.Value.toUpperCase()) {
                        arguments.IsValid = true;

                    }
                }
            }

        }
		  
    </script>

</body>
</html>
