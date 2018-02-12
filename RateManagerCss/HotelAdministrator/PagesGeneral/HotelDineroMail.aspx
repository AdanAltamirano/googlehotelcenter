<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="HotelDineroMail.aspx.vb" Inherits="RateManager.HotelDineroMail" %>

<%@ Register src="../../Modulos/CtrRatePlanDineroMail.ascx" tagname="CtrRatePlanDineroMail" tagprefix="uc1" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
     <link href="../../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
</head>
<body bottommargin="0" leftmargin="0" rightmargin="0">
    <form id="Form1" method="post" runat="server">
    <div class="clear">
        <div>
            <asp:Label ID="lblTitle" runat="server" class="tituloSeccion">Hotel Configuration</asp:Label>
        </div>
    </div>
    <table id="Bookingcontainer" cellspacing="0" cellpadding="2" align="center"
        border="0">
        <tr>
            <td>
                <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                    <tr>
                        <td colspan="4">
                            <uc1:CtrRatePlanDineroMail ID="CtrRatePlanDineroMail1" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                            <asp:Label
                                ID="lblError" runat="server" CssClass="Validators" Visible="False">ERROR</asp:Label>
                        </td>
                    </tr>                   
                    <tr>
                        <td align="center" colspan="4">
                            <asp:Button ID="cmdEliminar" runat="server" CssClass="Button" Text="Eliminar" 
                                CausesValidation =false >
                            </asp:Button><asp:Button ID="btnSave" runat="server" CssClass="Button" Text="Guardar">
                            </asp:Button>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>

    <script>
        function LoadMsg(ddl, lblAux, lblDay, dia, hora, specific, aux, canc) {
            var l1 = document.getElementById(lblDay);
            var l2 = document.getElementById(lblAux);
            var d = document.getElementById(ddl);
            var txt = document.getElementById('txtCancellationPolicy');
            var ddl1 = document.getElementById('ddlHour');
            var ddl2 = document.getElementById('ddlMinutes');
            var lbls = document.getElementById('lblSep');

            txt.style.display = '';
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
                    ddl1.style.display = '';
                    ddl2.style.display = '';
                    lbls.style.display = '';
                    break;
            }

        }
    </script>

</body>
</html>
