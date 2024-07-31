<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Ayuda.aspx.vb" Inherits="RateManager.Ayuda" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>Ayuda</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../StyleSheets/Styles.css" type="text/css" rel="stylesheet">
</head>
<body topmargin="5">
    <form id="Form1" method="post" runat="server">
    <div class="clear">
        <div class="mDiv">
        </div>
        <div>
            <asp:Label ID="lblTitle" runat="server" EnableViewState="False" Text="Documentación de ayuda del CRS"
                CssClass="tituloSeccion"></asp:Label>
        </div>
    </div>
    <table id="bookingcontainer" cellspacing="0" cellpadding="5" width="770" border="0">
        <tr>
            <td>
                <span runat="server" id="lblsubtitle">Descargue los manuales de ayuda del CRS y conozca
                    la funcionalidad del sistema.</span>
            </td>
        </tr>
        <tr>
            <td>
                <span id="lblpaso1" runat="server">1) Inicie con la Guía Rápida, le dará un panorama
                    y le enseñará las funciones elementales a realizar en el sistema. (Documento de
                    10 paginas)</span>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btnguiarapida" runat="server" CssClass="button" Text="Descargar Guía Rápida CRS"
                    Width="200px"></asp:Button>
            </td>
        </tr>
        <tr>
            <td>
                <span id="lblpaso2" runat="server">2) En el Manual de Usuario encontrará información
                    y ejemplos detallados de las funciones del sistema. (Documento de 42 paginas)</span>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btnmanual" runat="server" CssClass="button" Text="Descargar Manual de Usuario Básico CRS"
                    Width="270px"></asp:Button>
            </td>
        </tr>
        <tr>
            <td>
                <span id="lblpaso3" runat="server">3) Ahora que ya conoce el CRS, aprenda como crear
                    Tarifas Promocionales, Corporativas, Planes Familiares, determinar cuantas habitaciones
                    desea vender en un plan Especial. Si su hotel tienen un complejo manejo de tarifas,
                    aprenda a crear tarifas enlazadas entre habitaciones y planes, le facilitará enormemente
                    esta labor.</span>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btnmanualavanzado" runat="server" CssClass="button" Text="Descargar Manual de Usuario Avanzado CRS"
                    Width="300px"></asp:Button>
            </td>
        </tr>
        <tr>
            <td>
                <span id="lblRequired" runat="server">Se requiere de un perfil Avanzado, para tener
                    acceso a las opciones de Planes Tarifarios. Solicite éste acceso.</span>
            </td>
        </tr>
        <tr>
            <td>
                <span id="lblpaso4" runat="server">4) ABC DE LAS RESERVACIONES, 12 puntos que debe tener
                    en cuenta ahora que su hotel ha abierto el canal de reservaciones en línea.</span>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:HyperLink ID="hplABCReservas" CssClass="dglink" runat="server" NavigateUrl="http://www.univisit.net/ABC/13puntos.html"
                    Target="_blank">Abrir el ABC DE RESERVACIONES</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td>
                <span id="lblpaso5" runat="server">5) Conciliación y pago de comisiones. Este manual
                    lo guiará paso a paso con el proceso de conciliación, control de pagos y facturación
                    de comisiones.</span>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btnconciliacion" runat="server" CssClass="button" Text="Descargar Manual de conciliación y pago de comiciones"
                    Width="300px"></asp:Button>
            </td>
        </tr>
        </TR>
        <tr>
            <td>
                <span id="lblpaso6" runat="server">6) Promociones y paquetes. Este manual le ayudará
                    a hacer más atractivo su Hotel ante sus clientes potenciales ofreciendo promociones
                    como descuentos o noches gratis, así como paquetes que incluyan servicios adicionales
                    al hospedaje, a los clientes les encantará esta facilidad para organizar sus actividades
                    en el lugar visitado.</span>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btnPromociones" runat="server" CssClass="button" Text="Descargar Manual de Promociones"
                    Width="300px"></asp:Button>
            </td>
        </tr>
        <tr>
            <td>
                <div id="lblcontactanos" runat=server visible =false>
                    Cualquier problema con el Sistema Central de Reservaciones, no dude en contactarnos.<p />
                    DEPARTAMENTO ATENCIÓN A SOCIOS<p />
                    Norma.Hernandez@univisit.com ó al 01(612)1238740, ext. 122<p />
                    atencionsocios1@univisit.com ó al 01(612)1230525, ext. 125 Soporte en línea
                    <p />
                    Skype: univisit_norma MSN: univisit_norma@hotmail.com</div>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
