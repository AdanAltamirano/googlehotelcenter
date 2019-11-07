<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ctrlChangePassword.ascx.vb" Inherits="RateManager.ctrlChangePassword" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<table id="Table1" cellspacing="1" cellpadding="1" border="0" width="100%">
    <tr>
        <td class="TituloTabla" colspan="2" align="center">
            <asp:Label ID="lblMsg" runat="server" CssClass="clsHelpLabel" EnableViewState="False"> Para cambiar por la nueva contraseña, llene la siguiente información</asp:Label></td>
    </tr>
    <tr>
        <td align="right" width="50%">
            <asp:Label ID="lblEmail" runat="server" CssClass="clsDarkLabel" EnableViewState="False">Correo electrónico:</asp:Label></td>
        <td align="left" width="50%">
            <asp:Label ID="lblEmailUser" runat="server" CssClass="clsDarkLabel" EnableViewState="False">Email de usuario</asp:Label></td>
    </tr>
    <tr>
        <td align="center" colspan="2">
            <asp:RequiredFieldValidator ID="rfvPasswordRequired" CssClass="Validators" runat="server" Display="Dynamic"
                ErrorMessage="Contraseña anterior es requerida" ControlToValidate="txtPassword" ForeColor=" "></asp:RequiredFieldValidator></td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label ID="lbloldpass" runat="server" CssClass="clsDarkLabel" EnableViewState="False">Contraseña anterior:</asp:Label></td>
        <td align="left">
            <asp:TextBox ID="txtPassword" runat="server" CssClass="TextBox" MaxLength="12" TextMode="Password"></asp:TextBox></td>
    </tr>
    <tr>
        <td align="center" colspan="2">
            <asp:CustomValidator ID="cvPassValid" runat="server" CssClass="Validators" Display="Dynamic" ErrorMessage="Contraseña anterior no coincide con la cantraseña almacenada"
                ForeColor=" "></asp:CustomValidator></td>
    </tr>
    <tr>
        <td align="center" colspan="2">
            <asp:CompareValidator ID="cvPasswordConfirmCompare" runat="server" CssClass="Validators" Display="Dynamic"
                ErrorMessage="Nueva contraseña y confirmación deben de coincidir" ControlToValidate="txtPasswordConfirm"
                ControlToCompare="txtNewPassword" ForeColor=" "></asp:CompareValidator></td>
    </tr>
    <tr>
        <td align="center" colspan="2">
            <asp:RequiredFieldValidator ID="rfvNewPass" runat="server" CssClass="Validators" Display="Dynamic" ErrorMessage="Nueva contraseña es requerida"
                ControlToValidate="txtNewPassword" ForeColor=" "></asp:RequiredFieldValidator></td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label ID="lblnuevopass" runat="server" CssClass="clsDarkLabel" EnableViewState="False">Contraseña Nueva:</asp:Label></td>
        <td align="left">
            <asp:TextBox ID="txtNewPassword" runat="server" CssClass="TextBox" MaxLength="12" TextMode="Password"></asp:TextBox></td>
    </tr>
    <tr>
        <td align="center" colspan="2">
            <asp:RequiredFieldValidator ID="rfvPasswordConfirmRequired" runat="server" CssClass="Validators" Display="Dynamic"
                ErrorMessage="Debe confirmar la nueva contraseña" ControlToValidate="txtPasswordConfirm" ForeColor=" "></asp:RequiredFieldValidator></td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label ID="lblPasswordConfirm" runat="server" CssClass="clsDarkLabel" EnableViewState="False">Confirmar contraseña:</asp:Label></td>
        <td align="left">
            <asp:TextBox ID="txtPasswordConfirm" runat="server" CssClass="TextBox" TextMode="Password" MaxLength="12"></asp:TextBox></td>
    </tr>
    <tr>
        <td td align="center" colspan="2">
            <asp:RegularExpressionValidator ID="revNewPassword" runat="server" Display="Dynamic" ControlToValidate="txtNewPassword" CssClass="Validators" 
                ErrorMessage="La contraseña debe ser de al menos 8 caracteres y contener almenos una letra, una letra mayúscula, un número y un carácter especial." 
                ValidationExpression="^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!_%*#?&/])[A-Za-z\d$@$!_%*#?&/]{8,}$"></asp:RegularExpressionValidator>
        </td>
    </tr>
</table>
