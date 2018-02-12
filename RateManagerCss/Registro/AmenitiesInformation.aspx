<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../Portal/Modules/ctrlHeader.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AmenitiesInformation.aspx.vb"
    ValidateRequest="False" Inherits="RateManager.AmenitiesInformation" %>

<%@ Register TagPrefix="uc1" TagName="CtrMenu" Src="../Portal/Modules/CtrMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="InfoAmenidades" Src="../Portal/Modules/Contenido/InfoAmenidades.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../Portal/Modules/ctrlFooter.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlAmenities" Src="../Portal/Modules/Contenido/ctrlAmenities.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>AmenitiesInformation</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
</head>
<body bottommargin="0" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <div id="tituloSeccion">
        <img src="../Includes/imagenes/icono-big-plus.png" width="25" height="25">
        <asp:Label class="tituloSeccion" ID="lbltitle" runat="server" EnableViewState="False">Buscar hotel</asp:Label>
    </div>
    <table id="bookingcontainer" cellspacing="0" cellpadding="2" width="650" border="0">
        <tr>
            <td>
                <table id="Table1" cellspacing=0 cellpadding="0" width="100%" border="0">
                    <tr>
                        <td>
                            <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td>
                                        <uc1:InfoAmenidades ID="InfoAmenidades1" runat="server"></uc1:InfoAmenidades>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
