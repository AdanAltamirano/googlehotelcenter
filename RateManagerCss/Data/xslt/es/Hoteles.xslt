<?xml version="1.0" encoding="utf-8"?>
<!-- Edited by XMLSpy® -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:template match="/">
    <html>
      <style>
        body
        {
        color: #000;
        font: normal 11px Arial, helvetica, sans-serif;
        }
        th
        {
        color: #000;
        font: normal 11px Arial, helvetica, sans-serif;
        }
        td
        {
        padding-left:4px;
        padding-right:4px;
        }
        th
        {
        padding-left:4px;
        padding-right:4px;
        }
        span
        {
        font: bold 14px Arial; color: #336666;
        }
      </style>
      <body>
        <br/>
        <span>Información General Hotel</span>
        <table border="1" cellpadding="0" cellspacing="0">
          <xsl:for-each select="Hoteles/UpdateHotel">
            <xsl:if test="position() = 1">
              <xsl:call-template name="Header">
              </xsl:call-template>
            </xsl:if>
            <tr>
              <td>
                <xsl:choose>
                  <xsl:when test="UserPerfil = '0'">
                    Basic
                  </xsl:when>
                  <xsl:when test="UserPerfil = '1'">
                    Medium
                  </xsl:when>
                  <xsl:when test="UserPerfil = '2'">
                    Avanced
                  </xsl:when>
                  <xsl:when test="UserPerfil = '3'">
                    NetRate
                  </xsl:when>
                  <xsl:when test="UserPerfil = '4'">
                    Mixed
                  </xsl:when>
                </xsl:choose>
              </td>
              <td>
                <xsl:choose>
                  <xsl:when test="StatusAvailability = 'O'">
                    Open
                  </xsl:when>
                  <xsl:when test="StatusAvailability = 'C'">
                    Close
                  </xsl:when>
                  <xsl:when test="StatusAvailability = 'N'">
                    No Arrival
                  </xsl:when>
                </xsl:choose>
              </td>
              <td  style="text-align:center;">
                <xsl:value-of select="EmailReservas"/>
              </td>
              <td  style="text-align:center;">
                <xsl:value-of select="MinLengthStay"/>
              </td>
              <td  style="text-align:center;">
                <xsl:value-of select="MaxDiasRenta"/>
              </td>
              <td  style="text-align:center;">
                <xsl:value-of select="MinNumCuartos"/>
              </td>
              <td  style="text-align:center;">
                <xsl:value-of select="MaxNumCuartos"/>
              </td>
              <td  style="text-align:center;">
                <xsl:value-of select="Codigo"/>
              </td>
              <td  style="text-align:center;">
                <xsl:value-of select="PropertyNumber"/>
              </td>
              <td  style="text-align:center;">
                <xsl:value-of select="ChainCode"/>
              </td>
              <td  style="text-align:center;">
                <xsl:value-of select="PGalileo"/>
              </td>
              <td  style="text-align:center;">
                <xsl:value-of select="PWorldspan"/>
              </td>
              <td  style="text-align:center;">
                <xsl:value-of select="PSabre"/>
              </td>
              <td  style="text-align:center;">
                <xsl:value-of select="PAmadeus"/>
              </td>
              <td  style="text-align:center;">
                <xsl:choose>
                  <xsl:when test="AvailOnPortal = 'true'">
                    Yes
                  </xsl:when>
                  <xsl:otherwise>
                    no
                  </xsl:otherwise>
                </xsl:choose>
              </td>
              <td  style="text-align:center;">
                <xsl:value-of select="NoArrivals"/>
              </td>
            </tr>
          </xsl:for-each>
        </table>

        <br/>
        <span>Información General Hotel, Actualizado</span>
        <table border="1" cellpadding="0" cellspacing="0">
          <xsl:for-each select="Hoteles/_UpdateHotel">
            <tr>
              <td>
                <xsl:choose>
                  <xsl:when test="UserPerfil = '0'">
                    Basic
                  </xsl:when>
                  <xsl:when test="UserPerfil = '1'">
                    Medium
                  </xsl:when>
                  <xsl:when test="UserPerfil = '2'">
                    Avanced
                  </xsl:when>
                  <xsl:when test="UserPerfil = '3'">
                    NetRate
                  </xsl:when>
                  <xsl:when test="UserPerfil = '4'">
                    Mixed
                  </xsl:when>
                </xsl:choose>
              </td>
              <td>
                <xsl:choose>
                  <xsl:when test="StatusAvailability = 'O'">
                    Open
                  </xsl:when>
                  <xsl:when test="StatusAvailability = 'C'">
                    Close
                  </xsl:when>
                  <xsl:when test="StatusAvailability = 'N'">
                    No Arrival
                  </xsl:when>
                </xsl:choose>
              </td>

              <td  style="text-align:center;">
                <xsl:value-of select="EmailReservas"/>
              </td>
              <td  style="text-align:center;">
                <xsl:value-of select="MinLengthStay"/>
              </td>
              <td  style="text-align:center;">
                <xsl:value-of select="MaxDiasRenta"/>
              </td>
              <td  style="text-align:center;">
                <xsl:value-of select="MinNumCuartos"/>
              </td>
              <td  style="text-align:center;">
                <xsl:value-of select="MaxNumCuartos"/>
              </td>
              <td  style="text-align:center;">
                <xsl:value-of select="Codigo"/>
              </td>
              <td  style="text-align:center;">
                <xsl:value-of select="PropertyNumber"/>
              </td>
              <td  style="text-align:center;">
                <xsl:value-of select="ChainCode"/>
              </td>
              <td  style="text-align:center;">
                <xsl:value-of select="PGalileo"/>
              </td>
              <td  style="text-align:center;">
                <xsl:value-of select="PWorldspan"/>
              </td>
              <td  style="text-align:center;">
                <xsl:value-of select="PSabre"/>
              </td>
              <td  style="text-align:center;">
                <xsl:value-of select="PAmadeus"/>
              </td>
              <td  style="text-align:center;">
                <xsl:choose>
                  <xsl:when test="AvailOnPortal = 'true'">
                    Yes
                  </xsl:when>
                  <xsl:otherwise>
                    no
                  </xsl:otherwise>
                </xsl:choose>
              </td>
              <td  style="text-align:center;">
                <xsl:value-of select="NoArrivals"/>
              </td>

            </tr>
          </xsl:for-each>
        </table>

      </body>
    </html>
  </xsl:template>

  <xsl:template name="Header">

    <tr bgcolor="#668800">
      <td>Perfil Usuario</td>
      <td>Estado Hotel</td>
      <td>Correo Reservaciones</td>
      <td>Min. Noches</td>
      <td>Max. Noches</td>
      <td>Min Hab reserv</td>
      <td>Max Hab reserv</td>
      <td>Moneda</td>
      <td>Número Propiedad</td>
      <td>Código Cadena</td>
      <td>ID Galileo</td>
      <td>ID WorldSpan</td>
      <td>ID Sabre</td>
      <td>ID Amadeus</td>
      <td>Disp. Portal</td>
      <td>
        No Arrivos<br/>DLMMJVS
      </td>
    </tr>
  </xsl:template>

  <xsl:template name="FormatDate">
    <xsl:param name="DateTime" />
    <!-- new date format 2006-01-14T08:55:22 -->
    <xsl:variable name="year">
      <xsl:value-of select="substring($DateTime,1,4)" />
    </xsl:variable>
    <xsl:variable name="year-temp">
      <xsl:value-of select="substring-after($DateTime,'-')" />
    </xsl:variable>
    <xsl:variable name="mo">
      <xsl:value-of select="substring($year-temp,1,2)" />
    </xsl:variable>
    <xsl:variable name="mo-temp">
      <xsl:value-of select="substring-after($year-temp,'-')" />
    </xsl:variable>
    <xsl:variable name="day">
      <xsl:value-of select="substring($mo-temp,1,2)" />
    </xsl:variable>

    <xsl:variable name="day-temp">
      <xsl:value-of select="substring-after($DateTime,'-')" />
    </xsl:variable>

    <xsl:variable name="time">
      <xsl:value-of select="substring-after($year-temp,' ')" />
    </xsl:variable>
    <xsl:variable name="hh">
      <xsl:value-of select="substring($time,1,2)" />
    </xsl:variable>
    <xsl:variable name="mm">
      <xsl:value-of select="substring($time,4,2)" />
    </xsl:variable>
    <xsl:variable name="ss">
      <xsl:value-of select="substring($time,7,2)" />
    </xsl:variable>

    <xsl:value-of select="$day"/>
    <xsl:value-of select="'-'"/>
    <xsl:choose>
      <xsl:when test="$mo = '01'">Jan</xsl:when>
      <xsl:when test="$mo = '02'">Feb</xsl:when>
      <xsl:when test="$mo = '03'">Mar</xsl:when>
      <xsl:when test="$mo = '04'">Apr</xsl:when>
      <xsl:when test="$mo = '05'">May</xsl:when>
      <xsl:when test="$mo = '06'">Jun</xsl:when>
      <xsl:when test="$mo = '07'">Jul</xsl:when>
      <xsl:when test="$mo = '08'">Aug</xsl:when>
      <xsl:when test="$mo = '09'">Sep</xsl:when>
      <xsl:when test="$mo = '10'">Oct</xsl:when>
      <xsl:when test="$mo = '11'">Nov</xsl:when>
      <xsl:when test="$mo = '12'">Dec</xsl:when>
    </xsl:choose>
    <xsl:value-of select="'-'"/>
    <xsl:value-of select="$year"/>
  </xsl:template>


</xsl:stylesheet>