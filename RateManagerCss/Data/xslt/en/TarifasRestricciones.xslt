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
        <xsl:for-each select="TarifasRestricciones/UpdateRateRestriction">
          <xsl:if test="position() = 1">
            <span>Room rates by occupation</span>
          </xsl:if>
        </xsl:for-each >

        <xsl:for-each select="TarifasRestricciones/UpdatePlanFaresNR">
          <xsl:if test="position() = 1">
            <span>Room rates by occupation by day</span>
          </xsl:if>
        </xsl:for-each >

        <xsl:for-each select="TarifasRestricciones/UpdateRateRestrictionByDay">
          <xsl:if test="position() = 1">
            <span>Room net rates by occupation</span>
          </xsl:if>
        </xsl:for-each>

        <table border="1" cellpadding="0" cellspacing="0">
          <xsl:for-each select="TarifasRestricciones/UpdateRateRestriction">
            <xsl:if test="position() = 1">
              <xsl:call-template name="Header">
              </xsl:call-template>
            </xsl:if>
            <tr>
              <td style="text-align:right;">
                <xsl:value-of select="Descr_rateplan"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="Adultos"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(TarifaAdulto, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="Ninios"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(TarifaNinio, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(TarifaAdolescente, '#####0.00', 'pesos')"/>
              </td>
            </tr>
          </xsl:for-each>

          <!--***** RESTRICCIONES TARIFAS NR *****-->
          <xsl:for-each select="TarifasRestricciones/UpdatePlanFaresNR">
            <xsl:if test="position() = 1">
              <xsl:call-template name="HeaderNR">
              </xsl:call-template>
            </xsl:if>
            <tr>
              <td style="text-align:right;">
                <xsl:value-of select="Descr_rateplan"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="Adultos"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(TarifaAdulto, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(TarifaAdultoNR, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(TarifaAdultoExc, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="Ninios"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(TarifaNinio, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(TarifaNinioNR, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(TarifaNinioExc, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(TarifaAdolescente, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(TarifaAdolescenteNR, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(TarifaAdolescenteExc, '#####0.00', 'pesos')"/>
              </td>
            </tr>
          </xsl:for-each>

          <xsl:for-each select="TarifasRestricciones/UpdateRateRestrictionByDay">
            <xsl:if test="position() = 1">
              <xsl:call-template name="HeaderDay">
              </xsl:call-template>
            </xsl:if>
            <tr>
              <td style="text-align:right;">
                <xsl:value-of select="Descr_rateplan"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="Adultos"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(TarifaAdulto, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="Ninios"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(TarifaNinio, '#####0.00', 'pesos')"/>
              </td>
            </tr>
          </xsl:for-each>
        </table>


        <br/>
        <xsl:for-each select="TarifasRestricciones/_UpdateRateRestriction">
          <xsl:if test="position() = 1">
            <span>Room rates by occupatio, Update</span>
          </xsl:if>
        </xsl:for-each >

        <xsl:for-each select="TarifasRestricciones/_UpdatePlanFaresNR">
          <xsl:if test="position() = 1">
            <span>Room rates by occupation by day, Update</span>
          </xsl:if>
        </xsl:for-each >

        <xsl:for-each select="TarifasRestricciones/_UpdateRateRestrictionByDay">
          <xsl:if test="position() = 1">
            <span>Room net rates by occupation, Update</span>
          </xsl:if>
        </xsl:for-each>
        <table border="1" cellpadding="0" cellspacing="0">
          <xsl:for-each select="TarifasRestricciones/_UpdateRateRestriction">
            <xsl:if test="position() = 1">
              <xsl:call-template name="Header">
              </xsl:call-template>
            </xsl:if>
            <tr>
              <td style="text-align:right;">
                <xsl:value-of select="Descr_rateplan"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="Adultos"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(TarifaAdulto, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="Ninios"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(TarifaNinio, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(TarifaAdolescente, '#####0.00', 'pesos')"/>
              </td>
            </tr>
          </xsl:for-each>

          <!--***** RESTRICCIONES TARIFAS NR *****-->
          <xsl:for-each select="TarifasRestricciones/_UpdatePlanFaresNR">
            <xsl:if test="position() = 1">
              <xsl:call-template name="HeaderNR">
              </xsl:call-template>
            </xsl:if>
            <tr>
              <td style="text-align:right;">
                <xsl:value-of select="Descr_rateplan"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="Adultos"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(TarifaAdulto, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(TarifaAdultoNR, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(TarifaAdultoExc, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="Ninios"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(TarifaNinio, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(TarifaNinioNR, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(TarifaNinioExc, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(TarifaAdolescente, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(TarifaAdolescenteNR, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(TarifaAdolescenteExc, '#####0.00', 'pesos')"/>
              </td>
            </tr>
          </xsl:for-each>

          <xsl:for-each select="TarifasRestricciones/_UpdateRateRestrictionByDay">
            <xsl:if test="position() = 1">
              <xsl:call-template name="HeaderDay">
              </xsl:call-template>
            </xsl:if>
            <tr>
              <td style="text-align:right;">
                <xsl:value-of select="Descr_rateplan"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="Adultos"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(TarifaAdulto, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="Ninios"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(TarifaNinio, '#####0.00', 'pesos')"/>
              </td>
            </tr>
          </xsl:for-each>
        </table>

      </body>
    </html>
  </xsl:template>

  <xsl:template name="Header">
    <tr bgcolor="#668800">
      <td>Rate Plan</td>
      <td>Adults</td>
      <td>Adult Price</td>
      <td>Chils</td>
      <td>Child Price</td>
      <td>Junior Price</td>
    </tr>
  </xsl:template>

  <xsl:template name="HeaderDay">
    <tr bgcolor="#668800">
      <td>Rate Plan</td>
      <td>Adults</td>
      <td>Adult Price</td>
      <td>Chils</td>
      <td>Child Price</td>
    </tr>
  </xsl:template>

  <xsl:template name="HeaderNR">
    <tr bgcolor="#668800">
      <td>Rate Plan </td>
      <td>Adults</td>
      <td>Adult Price</td>
      <td>Adult Price NR</td>
      <td>Adult Price Ex</td>
      <td>Chils</td>
      <td>Child P. </td>
      <td>Child P. NR</td>
      <td>Child P. Ex</td>
      <td>Junior P.</td>
      <td>Junior P. NR</td>
      <td>Junior P. Ex</td>
    </tr>
  </xsl:template>

</xsl:stylesheet>

