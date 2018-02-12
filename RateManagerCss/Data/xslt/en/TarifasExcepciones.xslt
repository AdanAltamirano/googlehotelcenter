<?xml version="1.0" encoding="utf-8"?>
<!-- Edited by XMLSpy® -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:decimal-format name="pesos" grouping-separator=","/>

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
        <xsl:for-each select="TarifasExcepciones/UpdateRatePromo">
          <xsl:if test="position() = 1">
            <span>Deals  Rates</span>
          </xsl:if>
        </xsl:for-each >
        <xsl:for-each select="TarifasExcepciones/UpdateRateNR">
          <xsl:if test="position() = 1">
            <span>Deals Rates Net Rate</span>
          </xsl:if>
        </xsl:for-each >

        <table border="1" cellspacing="0" cellpadding="0">
          <!-- ***** Actualizacion Tarifas ***** -->
          <xsl:for-each select="TarifasExcepciones/UpdateRatePromo">
            <xsl:if test="position() = 1">
              <xsl:call-template name="Header">
              </xsl:call-template>
            </xsl:if>
            <tr>
              <td>
                <xsl:value-of select="Descr_rateplan"/>
              </td>
              <td>
                <xsl:call-template name="FormatDate">
                  <xsl:with-param name="DateTime" select="FechaInicia"/>
                </xsl:call-template>
              </td>
              <td>
                <xsl:call-template name="FormatDate">
                  <xsl:with-param name="DateTime" select="FechaFinaliza"/>
                </xsl:call-template>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(Precio, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(PrecioNinio, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(PrecioAdolescente, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(PrecioExtraAdulto, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(PrecioNinioExtra, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(PrecioAdolescenteExtra, '#####0.00', 'pesos')"/>
              </td>

            </tr>
          </xsl:for-each>
          <!--** Continuacion **-->
          <xsl:for-each select="TarifasExcepciones/UpdateRatePromo">
            <xsl:if test="position() = 1">
              <xsl:call-template name="HeaderTarifa2">
              </xsl:call-template>
            </xsl:if>
            <tr>
              <td>
                &#160;
                <xsl:value-of select="RateRulesDefault"/>
              </td>
              <td>
                &#160;
                <xsl:value-of select="Personas"/>
              </td>
              <td>
                &#160;
                <xsl:value-of select="MaxAdultos"/>
              </td>
              <td>
                &#160;
                <xsl:value-of select="MinAdultos"/>
              </td>
              <td>
                &#160;
                <xsl:value-of select="MaxNinios"/>
              </td>
              <td>
                &#160;
                <xsl:value-of select="PersonasExtras"/>
              </td>
              <td>
                &#160;
                <xsl:value-of select="AdvBooking"/>
              </td>
              <td>
                &#160;
                <xsl:value-of select="MaxDias"/>
              </td>
              <td>
                &#160;
                <xsl:value-of select="MinDias"/>
              </td>
              <td>
                &#160;
                <xsl:value-of select="NoArrivos"/>
              </td>
              <td>
                &#160;
                <xsl:value-of select="ApplyDay"/>
              </td>
              <td>
                &#160;
                <xsl:value-of select="Excepciones"/>
              </td>
              <td>
                &#160;
                <xsl:call-template name="FormatDate">
                  <xsl:with-param name="DateTime" select="BookingWindowStart"/>
                </xsl:call-template>
              </td>
              <td>
                &#160;
                <xsl:call-template name="FormatDate">
                  <xsl:with-param name="DateTime" select="BookingWindowEnd"/>
                </xsl:call-template>
              </td>
              <td style="text-align:right;">
                &#160;
                <xsl:variable name="sDescPromotion1">
                  <xsl:value-of select="DescPromotion"/>
                </xsl:variable>
                <xsl:if test="$sDescPromotion1!=''">
                  <xsl:value-of select="format-number(DescPromotion, '#####0.00', 'pesos')"/>
                </xsl:if>
              </td>
            </tr>
          </xsl:for-each>

          <!-- ***** Actualizacion Tarifas Net rates ***** -->
          <xsl:for-each select="TarifasExcepciones/UpdateRateNR">
            <xsl:if test="position() = 1">
              <xsl:call-template name="HeaderNR">
              </xsl:call-template>
            </xsl:if>
            <tr>
              <td>
                <xsl:value-of select="Descr_rateplan"/>
              </td>
              <td>
                <xsl:call-template name="FormatDate">
                  <xsl:with-param name="DateTime" select="FechaInicia"/>
                </xsl:call-template>
              </td>
              <td>
                <xsl:call-template name="FormatDate">
                  <xsl:with-param name="DateTime" select="FechaFinaliza"/>
                </xsl:call-template>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(Precio, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(PrecioNinio, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(PrecioAdolescente, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(PrecioExtraAdulto, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(PrecioNinioExtra, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(PrecioAdolescenteExtra, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(TarifaAdultoNR, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(TarifaNinioNR, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(TarifaAdolescenteNR, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(PrecioExtraAdultoNR, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(PrecioExtraNinioNR, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(PrecioAdolescenteExtraNR, '#####0.00', 'pesos')"/>
              </td>

            </tr>
          </xsl:for-each>
          <!--** Continuacion **-->
          <xsl:for-each select="TarifasExcepciones/UpdateRateNR">
            <xsl:if test="position() = 1">
              <xsl:call-template name="HeaderTarifa2">
              </xsl:call-template>
            </xsl:if>
            <tr>
              <td>
                &#160;
                <xsl:value-of select="RateRulesDefault"/>
              </td>
              <td>
                &#160;
                <xsl:value-of select="Personas"/>
              </td>
              <td>
                &#160;
                <xsl:value-of select="MaxAdultos"/>
              </td>
              <td>
                &#160;
                <xsl:value-of select="MinAdultos"/>
              </td>
              <td>
                &#160;
                <xsl:value-of select="MaxNinios"/>
              </td>
              <td>
                &#160;
                <xsl:value-of select="PersonasExtras"/>
              </td>
              <td>
                &#160;
                <xsl:value-of select="AdvBooking"/>
              </td>
              <td>
                &#160;
                <xsl:value-of select="MaxDias"/>
              </td>
              <td>
                &#160;
                <xsl:value-of select="MinDias"/>
              </td>
              <td>
                &#160;
                <xsl:value-of select="NoArrivos"/>
              </td>
              <td>
                &#160;
                <xsl:value-of select="ApplyDay"/>
              </td>
              <td>
                &#160;
                <xsl:value-of select="Excepciones"/>
              </td>
              <td>
                &#160;
                <xsl:call-template name="FormatDate">
                  <xsl:with-param name="DateTime" select="BookingWindowStart"/>
                </xsl:call-template>
              </td>
              <td>
                &#160;
                <xsl:call-template name="FormatDate">
                  <xsl:with-param name="DateTime" select="BookingWindowEnd"/>
                </xsl:call-template>
              </td>
              <td style="text-align:right;">
                &#160;
                <xsl:variable name="sDescPromotion2">
                  <xsl:value-of select="DescPromotion"/>
                </xsl:variable>
                <xsl:if test="$sDescPromotion2!=''">
                  <xsl:value-of select="format-number(DescPromotion, '#####0.00', 'pesos')"/>
                </xsl:if>
              </td>
            </tr>
          </xsl:for-each>
        </table>

        <br/>
        <xsl:for-each select="TarifasExcepciones/_UpdateRatePromo">
          <xsl:if test="position() = 1">
            <span>Deals  Rates, Update</span>
          </xsl:if>
        </xsl:for-each >
        <xsl:for-each select="TarifasExcepciones/_UpdateRateNR">
          <xsl:if test="position() = 1">
            <span>Deals Rates Net Rate, Update</span>
          </xsl:if>
        </xsl:for-each >
        <table border="1" cellspacing="0" cellpadding="0">
          <xsl:for-each select="TarifasExcepciones/_UpdateRatePromo">
            <xsl:if test="position() = 1">
              <xsl:call-template name="Header">
              </xsl:call-template>
            </xsl:if>
            <tr>
              <td>
                <xsl:value-of select="Descr_rateplan"/>
              </td>
              <td>
                <xsl:call-template name="FormatDate">
                  <xsl:with-param name="DateTime" select="FechaInicia"/>
                </xsl:call-template>
              </td>
              <td>
                <xsl:call-template name="FormatDate">
                  <xsl:with-param name="DateTime" select="FechaFinaliza"/>
                </xsl:call-template>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(Precio, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(PrecioNinio, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(PrecioAdolescente, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(PrecioExtraAdulto, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(PrecioNinioExtra, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(PrecioAdolescenteExtra, '#####0.00', 'pesos')"/>
              </td>
            </tr>
          </xsl:for-each>
          <!--** Continuacion **-->
          <xsl:for-each select="TarifasExcepciones/_UpdateRatePromo">
            <xsl:if test="position() = 1">
              <xsl:call-template name="HeaderTarifa2">
              </xsl:call-template>
            </xsl:if>
            <tr>
              <td>
                &#160;
                <xsl:value-of select="RateRulesDefault"/>
              </td>
              <td>
                &#160;
                <xsl:value-of select="Personas"/>
              </td>
              <td>
                &#160;
                <xsl:value-of select="MaxAdultos"/>
              </td>
              <td>
                &#160;
                <xsl:value-of select="MinAdultos"/>
              </td>
              <td>
                &#160;
                <xsl:value-of select="MaxNinios"/>
              </td>
              <td>
                &#160;
                <xsl:value-of select="PersonasExtras"/>
              </td>
              <td>
                &#160;
                <xsl:value-of select="AdvBooking"/>
              </td>
              <td>
                &#160;
                <xsl:value-of select="MaxDias"/>
              </td>
              <td>
                &#160;
                <xsl:value-of select="MinDias"/>
              </td>
              <td>
                &#160;
                <xsl:value-of select="NoArrivos"/>
              </td>
              <td>
                &#160;
                <xsl:value-of select="ApplyDay"/>
              </td>
              <td>
                &#160;
                <xsl:value-of select="Excepciones"/>
              </td>
              <td>
                &#160;
                <xsl:call-template name="FormatDate">
                  <xsl:with-param name="DateTime" select="BookingWindowStart"/>
                </xsl:call-template>
              </td>
              <td>
                &#160;
                <xsl:call-template name="FormatDate">
                  <xsl:with-param name="DateTime" select="BookingWindowEnd"/>
                </xsl:call-template>
              </td>
              <td style="text-align:right;">
                &#160;
                <xsl:variable name="sDescPromotion3">
                  <xsl:value-of select="DescPromotion"/>
                </xsl:variable>
                <xsl:if test="$sDescPromotion3!=''">
                  <xsl:value-of select="format-number(DescPromotion, '#####0.00', 'pesos')"/>
                </xsl:if>
              </td>
            </tr>
          </xsl:for-each>

          <!-- ***** Actualizacion Tarifas Net rates ***** -->
          <xsl:for-each select="TarifasExcepciones/_UpdateRateNR">
            <xsl:if test="position() = 1">
              <xsl:call-template name="HeaderNR">
              </xsl:call-template>
            </xsl:if>
            <tr>
              <td>
                <xsl:value-of select="Descr_rateplan"/>
              </td>
              <td>
                <xsl:call-template name="FormatDate">
                  <xsl:with-param name="DateTime" select="FechaInicia"/>
                </xsl:call-template>
              </td>
              <td>
                <xsl:call-template name="FormatDate">
                  <xsl:with-param name="DateTime" select="FechaFinaliza"/>
                </xsl:call-template>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(Precio, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(PrecioNinio, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(PrecioAdolescente, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(PrecioExtraAdulto, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(PrecioNinioExtra, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(PrecioAdolescenteExtra, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(TarifaAdultoNR, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(TarifaNinioNR, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(TarifaAdolescenteNR, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(PrecioExtraAdultoNR, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(PrecioExtraNinioNR, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(PrecioAdolescenteExtraNR, '#####0.00', 'pesos')"/>
              </td>
            </tr>
          </xsl:for-each>
          <!--** Continuacion **-->
          <xsl:for-each select="TarifasExcepciones/_UpdateRateNR">
            <xsl:if test="position() = 1">
              <xsl:call-template name="HeaderTarifa2">
              </xsl:call-template>
            </xsl:if>
            <tr>
              <td>
                &#160;
                <xsl:value-of select="RateRulesDefault"/>
              </td>
              <td>
                &#160;
                <xsl:value-of select="Personas"/>
              </td>
              <td>
                &#160;
                <xsl:value-of select="MaxAdultos"/>
              </td>
              <td>
                &#160;
                <xsl:value-of select="MinAdultos"/>
              </td>
              <td>
                &#160;
                <xsl:value-of select="MaxNinios"/>
              </td>
              <td>
                &#160;
                <xsl:value-of select="PersonasExtras"/>
              </td>
              <td>
                &#160;
                <xsl:value-of select="AdvBooking"/>
              </td>
              <td>
                &#160;
                <xsl:value-of select="MaxDias"/>
              </td>
              <td>
                &#160;
                <xsl:value-of select="MinDias"/>
              </td>
              <td>
                &#160;
                <xsl:value-of select="NoArrivos"/>
              </td>
              <td>
                &#160;
                <xsl:value-of select="ApplyDay"/>
              </td>
              <td>
                &#160;
                <xsl:value-of select="Excepciones"/>
              </td>
              <td>
                &#160;
                <xsl:call-template name="FormatDate">
                  <xsl:with-param name="DateTime" select="BookingWindowStart"/>
                </xsl:call-template>
              </td>
              <td>
                &#160;
                <xsl:call-template name="FormatDate">
                  <xsl:with-param name="DateTime" select="BookingWindowEnd"/>
                </xsl:call-template>
              </td>
              <td style="text-align:right;">
                &#160;
                <xsl:variable name="sDescPromotion4">
                  <xsl:value-of select="DescPromotion"/>
                </xsl:variable>
                <xsl:if test="$sDescPromotion4!=''">
                  <xsl:value-of select="format-number(DescPromotion, '#####0.00', 'pesos')"/>
                </xsl:if>
              </td>
            </tr>
          </xsl:for-each>
        </table>

      </body>
    </html>
  </xsl:template>

  <xsl:template name="Header">

    <tr bgcolor="#668800">
      <td>Rate Plan </td>
      <td>Start Date</td>
      <td>End Date</td>
      <td>Adult Price</td>
      <td>Child P. </td>
      <td>Junior P.</td>
      <td>Adult ext.P.</td>
      <td>Child ext.P.</td>
      <td>Junior ext.P.</td>
    </tr>
  </xsl:template>

  <xsl:template name="HeaderNR">

    <tr bgcolor="#668800">
      <td>Rate Plan </td>
      <td>Start Date</td>
      <td>End Date</td>
      <td>Adult Price</td>
      <td>Child P. </td>
      <td>Junior P.</td>
      <td>Adult ext.P.</td>
      <td>Child ext.P.</td>
      <td>Junior ext.P.</td>
      <td>Adult Price NR</td>
      <td>Child P. NR</td>
      <td>Junior P. NR</td>
      <td>Adult ext.P. NR</td>
      <td>Child ext.P. NR</td>
      <td>Junior ext.P. NR</td>
    </tr>
  </xsl:template>

  <xsl:template name="HeaderTarifa2">
    <tr bgcolor="#668800">
      <td>Default Rules</td>
      <td>People.</td>
      <td>Max. Adults</td>
      <td>Min. Adults</td>
      <td>Max. Children</td>
      <td>People Extra</td>
      <td>Adv. Booking</td>
      <td>Max. days</td>
      <td>Min. days</td>
      <td>Arrival</td>
      <td>Apply Day [SMTWTFS]</td>
      <td>Excepctions</td>
      <td>Window Star</td>
      <td>Window End</td>
      <td>Disc Promotion</td>
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
  </xsl:template>

</xsl:stylesheet>