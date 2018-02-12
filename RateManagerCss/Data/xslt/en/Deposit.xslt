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
        <table>
          <tr>
            <td valign="top">
              <table border="1" cellpadding="0" cellspacing="0">
                <xsl:for-each select="Deposit/_UpdateDeposit">
                  <xsl:if test="position() = 1">
                    <xsl:call-template name="Header">
                    </xsl:call-template>
                  </xsl:if>
                  <tr>
                    <td>                      
                      <xsl:value-of select="noReservacion"/>                                            
                    </td>
                    <td>
                      <xsl:value-of select="nocuenta"/>
                    </td>
                    <td>
                      <xsl:value-of select="banco"/>
                    </td>
                    <td>
                      <xsl:value-of select="dep_monto"/>
                      <xsl:value-of select="dep_moneda"/>
                    </td>
                    <td>
                      <xsl:call-template name="FormatDate">
                        <xsl:with-param name="DateTime" select="fecha"/>
                      </xsl:call-template>
                    </td>
                    <td>
                      <xsl:value-of select="idUsuario"/>
                    </td>
                    <td>
                      <xsl:value-of select="observacion"/>
                    </td>
                  </tr>
                </xsl:for-each>

                <xsl:for-each select="Deposit/_UpdatePayment">
                  <xsl:if test="position() = 1">
                    <xsl:call-template name="Header1">
                    </xsl:call-template>
                  </xsl:if>
                  <tr>
                    <td>
                      <xsl:value-of select="noReservacion"/>
                    </td>
                    <td>
                      <xsl:value-of select="NoAutorizacion"/>
                    </td>
                    <td>
                      <xsl:value-of select="paymentSource"/>
                    </td>
                    <td>
                      <xsl:value-of select="Referencia"/>
                    </td>
                    <td>
                      <xsl:value-of select="Folio"/>
                    </td>

                    <td>
                      <xsl:value-of select="Monto"/>
                      <xsl:value-of select="Moneda"/>
                    </td>
                    <td>
                      <xsl:value-of select="idUsuario"/>
                    </td>
                  </tr>
                </xsl:for-each>
              </table>
            </td>           
          </tr>
        </table>
      </body>
    </html>
  </xsl:template>

  <xsl:template name="Header">
    <span>Deposit confirmation</span>
    <tr bgcolor="#668800">
      <td>Reservation Number</td>
      <td>Bank Acount</td>
      <td>Bank Name</td>
      <td>Amount Deposit</td>
      <td>Date</td>
      <td>User</td>
      <td>Observation</td>
    </tr>
  </xsl:template>
  <xsl:template name="Header1">
    <span>Deposit confirmation (other payments) </span>
    <tr bgcolor="#668800">
      <td>Reservation Number</td>
      <td>Authorization Number</td>
      <td>Payment Type</td>
      <td>Reference</td>
      <td>ID</td>
      <td>Amount Deposit</td>
      <td>User</td>
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
      <xsl:value-of select="substring-after($day-temp,' ')" />
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
      <xsl:when test="$mo = '01'">Jun</xsl:when>
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
    <!--<xsl:text> </xsl:text><xsl:value-of select="$hh"/>:<xsl:value-of select="$mm"/>-->
  </xsl:template>


</xsl:stylesheet>