<%@ Register TagPrefix="uc1" TagName="Activities" Src="../Portal/Modules/Contenido/Activities.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlFooter" Src="../Portal/Modules/ctrlFooter.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CtrMenu" Src="../Portal/Modules/CtrMenu.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" ValidateRequest="False" CodeBehind="ActivitiesInformation.aspx.vb"
    Inherits="RateManager.ActivitiesInformation" %>

<%@ Register TagPrefix="uc1" TagName="ctlServices" Src="../Portal/Modules/Contenido/ctlServices.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ctrlHeader" Src="../Portal/Modules/ctrlHeader.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>ActivitiesInformation</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
</head>
<body bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <div class="clear">
        <div class="mDiv" width="25" height="25">
        </div>
        <div class="title">
            <asp:Label ID="lblTitle" runat="server" EnableViewState="False" Text="Editar actividades del hotel"
                CssClass="tituloSeccion"></asp:Label>
        </div>
    </div>
    <table id="bookingcontainer" cellspacing="0" cellpadding="2" width="100%" border="0" style=" height:600px; ">
        <tr>
            <td style=" vertical-align:top; ">
                <table id="Table1" cellspacing=0 cellpadding="0" width="100%" border="0">
                    <tr>
                        <td style="vertical-align:top;">
                            <table cellspacing=0 cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td>
                                        <uc1:Activities ID="Activities1" runat="server"></uc1:Activities>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <uc1:ctlServices ID="CtlServices1" runat="server"></uc1:ctlServices>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
