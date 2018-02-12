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
        <xsl:for-each select="TarifasRestriccionesExcepciones/UpdateRateRestriction">
          <xsl:if test="position() = 1">
            <span>Deals room rates by occupation</span>
          </xsl:if>
        </xsl:for-each >
        <xsl:for-each select="TarifasRestriccionesExcepciones/UpdatePlanFaresNR">
          <xsl:if test="position() = 1">
            <span>Deals room net rates by occupation</span>
          </xsl:if>
        </xsl:for-each >

        <table border="1" cellpadding="0" cellspacing="0">
          <!--**** Tarifa Promociones ****-->
          <xsl:for-each select="TarifasRestriccionesExcepciones/UpdateRateRestriction">
            <xsl:if test="position() = 1">
              <xsl:call-template name="Header">
              </xsl:call-template>
            </xsl:if>
            <tr>
              <td style="text-align:right;">
                &#160;
                <xsl:value-of select="Descr_rateplan"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="Adultos"/>
              </td>
              <td style="text-align:right;">
                <xsl:call-template name="FormatNumber">
                  <xsl:with-param name="valor" select="TarifaAdulto"/>
                </xsl:call-template>

                <!--<xsl:value-of select="format-number(TarifaAdulto, '#####0.00', 'pesos')"/>-->
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

          <!--**** Tarifa Net Rate Promociones ****-->
          <xsl:for-each select="TarifasRestriccionesExcepciones/UpdatePlanFaresNR">
            <xsl:if test="position() = 1">
              <xsl:call-template name="HeaderNR">
              </xsl:call-template>
            </xsl:if>
            <tr>
              <td style="text-align:right;">
                &#160;
                <xsl:value-of select="Descr_rateplan"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="Adultos"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(TarifaAdulto, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:call-template name="FormatNumber">
                  <xsl:with-param name="valor" select="TarifaAdultoNR"/>
                </xsl:call-template>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="Ninios"/>
              </td>
              <td style="text-align:right;">
                <xsl:call-template name="FormatNumber">
                  <xsl:with-param name="valor" select="TarifaNinio"/>
                </xsl:call-template>
              </td>
              <td style="text-align:right;">
                <xsl:call-template name="FormatNumber">
                  <xsl:with-param name="valor" select="TarifaNinioNR"/>
                </xsl:call-template>
              </td>
              <td style="text-align:right;">
                <xsl:call-template name="FormatNumber">
                  <xsl:with-param name="valor" select="TarifaAdolescente"/>
                </xsl:call-template>
              </td>
              <td style="text-align:right;">
                <xsl:call-template name="FormatNumber">
                  <xsl:with-param name="valor" select="TarifaAdolescenteNR"/>
                </xsl:call-template>
              </td>
            </tr>
          </xsl:for-each>
        </table>

        <br/>
        <xsl:for-each select="TarifasRestriccionesExcepciones/_UpdateRateRestriction">
          <xsl:if test="position() = 1">
            <span>Deals room rates by occupation, Update</span>
          </xsl:if>
        </xsl:for-each >
        <xsl:for-each select="TarifasRestriccionesExcepciones/_UpdatePlanFaresNR">
          <xsl:if test="position() = 1">
            <span>Deals room net rates by occupation, Update</span>
          </xsl:if>
        </xsl:for-each >

        <table border="1" cellpadding="0" cellspacing="0">

          <!--**** Tarifa Promociones ****-->
          <xsl:for-each select="TarifasRestriccionesExcepciones/_UpdateRateRestriction">
            <xsl:if test="position() = 1">
              <xsl:call-template name="Header">
              </xsl:call-template>
            </xsl:if>
            <tr>
              <td style="text-align:right;">
                &#160;
                <xsl:value-of select="Descr_rateplan"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="Adultos"/>
              </td>
              <td style="text-align:right;">
                <xsl:call-template name="FormatNumber">
                  <xsl:with-param name="valor" select="TarifaAdulto"/>
                </xsl:call-template>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="Ninios"/>
              </td>
              <td style="text-align:right;">
                <xsl:call-template name="FormatNumber">
                  <xsl:with-param name="valor" select="TarifaNinio"/>
                </xsl:call-template>
              </td>
              <td style="text-align:right;">
                <xsl:call-template name="FormatNumber">
                  <xsl:with-param name="valor" select="TarifaAdolescente"/>
                </xsl:call-template>
              </td>
            </tr>
          </xsl:for-each>

          <!--**** Tarifa Net Rate Promociones ****-->
          <xsl:for-each select="TarifasRestriccionesExcepciones/_UpdatePlanFaresNR">
            <xsl:if test="position() = 1">
              <xsl:call-template name="HeaderNR">
              </xsl:call-template>
            </xsl:if>
            <tr>
              <td style="text-align:right;">
                &#160;
                <xsl:value-of select="Descr_rateplan"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="Adultos"/>
              </td>
              <td style="text-align:right;">
                <xsl:call-template name="FormatNumber">
                  <xsl:with-param name="valor" select="TarifaAdulto"/>
                </xsl:call-template>
              </td>
              <td style="text-align:right;">
                <xsl:call-template name="FormatNumber">
                  <xsl:with-param name="valor" select="TarifaAdultoNR"/>
                </xsl:call-template>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="Ninios"/>
              </td>
              <td style="text-align:right;">
                <xsl:call-template name="FormatNumber">
                  <xsl:with-param name="valor" select="TarifaNinio"/>
                </xsl:call-template>
              </td>
              <td style="text-align:right;">
                <xsl:call-template name="FormatNumber">
                  <xsl:with-param name="valor" select="TarifaNinioNR"/>
                </xsl:call-template>
              </td>
              <td style="text-align:right;">
                <xsl:call-template name="FormatNumber">
                  <xsl:with-param name="valor" select="TarifaAdolescente"/>
                </xsl:call-template>
              </td>
              <td style="text-align:right;">
                <xsl:call-template name="FormatNumber">
                  <xsl:with-param name="valor" select="TarifaAdolescenteNR"/>
                </xsl:call-template>
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

  <xsl:template name="HeaderNR">
    <tr bgcolor="#668800">
      <td>Rate Plan </td>
      <td>Adults</td>
      <td>Adult Price</td>
      <td>Adult Price NR</td>
      <td>Chils</td>
      <td>Child Price </td>
      <td>Child Price NR</td>
      <td>Junior Price</td>
      <td>Junior Price NR</td>
    </tr>
  </xsl:template>

  <xsl:template name="FormatNumber">
    <xsl:param name="valor" />
    <xsl:choose>
      <xsl:when test="$valor!=''">
        <xsl:value-of select="format-number($valor, '#####0.00', 'pesos')"/>
      </xsl:when>
      <xsl:otherwise>
        0
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

</xsl:stylesheet>

