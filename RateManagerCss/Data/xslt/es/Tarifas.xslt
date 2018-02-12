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
        <xsl:for-each select="Tarifas/UpdateRate">
          <xsl:if test="position() = 1">
            <span>Tarifas</span>
          </xsl:if>

        </xsl:for-each>


        <xsl:for-each select="Tarifas/UpdateRateByDay">
          <xsl:if test="position() = 1">
            <span>Tarifas por día</span>
          </xsl:if>
        </xsl:for-each>


        <xsl:for-each select="Tarifas/UpdateRateNR">
          <xsl:if test="position() = 1">
            <span>Tarifas Netas</span>
          </xsl:if>
        </xsl:for-each>

        <table border="1" cellspacing="0" cellpadding="0">

          <!-- ***** Actualizacion Tarifas ***** -->
          <xsl:for-each select="Tarifas/UpdateRate">
            <xsl:if test="position() = 1">
              <xsl:call-template name="HeaderTarifa">
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
                <xsl:value-of select="format-number(niniosrate, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(PrecioAdolescente, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(PrecioExtraAdulto, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(PrecioExtraNinio, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(PrecioAdolescenteExtra, '#####0.00', 'pesos')"/>
              </td>
            </tr>
          </xsl:for-each>
          <!--** Continuacion **-->
          <xsl:for-each select="Tarifas/UpdateRate">
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
                <xsl:value-of select="Excepciones"/>
              </td>
              <td>
                &#160;
                <xsl:value-of select="waitListAvailable"/>
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

          <!-- ***** Actualizacion de tarifas por dia  ***** -->
          <xsl:for-each select="Tarifas/UpdateRateByDay">
            <xsl:if test="position() = 1">
              <xsl:call-template name="HeaderTarifaByDay">
              </xsl:call-template>
            </xsl:if>
            <tr>
              <td>
                <xsl:value-of select="CodigoTarifa"/>
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
                <xsl:value-of select="format-number(PrecioExtraAdulto, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(niniosrate, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(PrecioExtraNinio, '#####0.00', 'pesos')"/>
              </td>
            </tr>
          </xsl:for-each>
          <!-- ***** Actualizacion de tarifas por dia  ***** -->
          <xsl:for-each select="Tarifas/UpdateRateNR">
            <xsl:if test="position() = 1">
              <xsl:call-template name="HeaderTarifaNet">
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
                <xsl:value-of select="format-number(niniosrate, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(PrecioAdolescente, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(PrecioExtraAdulto, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(PrecioExtraNinio, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(PrecioAdolescenteExtra, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(PrecioNR, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(niniosrateNR, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(PrecioAdolescenteNR, '#####0.00', 'pesos')"/>
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
          <xsl:for-each select="Tarifas/UpdateRateNR">
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
                <xsl:value-of select="Excepciones"/>
              </td>
              <td>
                &#160;
                <xsl:value-of select="waitListAvailable"/>
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
        <xsl:for-each select="Tarifas/_UpdateRate">
          <xsl:if test="position() = 1">
            <span>Tarifas, Actualizado</span>
          </xsl:if>

        </xsl:for-each>


        <xsl:for-each select="Tarifas/_UpdateRateByDay">
          <xsl:if test="position() = 1">
            <span>Tarifas por día, Actualizado</span>
          </xsl:if>
        </xsl:for-each>


        <xsl:for-each select="Tarifas/_UpdateRateNR">
          <xsl:if test="position() = 1">
            <span>Tarifas Netas, Actualizado</span>
          </xsl:if>
        </xsl:for-each>
        <table border="1" cellspacing="0" cellpadding="0">
          <xsl:for-each select="Tarifas/_UpdateRate">
            <xsl:if test="position() = 1">
              <xsl:call-template name="HeaderTarifa">
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
                <xsl:value-of select="format-number(NiniosRate, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(PrecioAdolescente, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(PrecioExtraAdulto, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(PrecioExtraNinio, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(PrecioAdolescenteExtra, '#####0.00', 'pesos')"/>
              </td>
            </tr>
          </xsl:for-each>
          <!--** Continuacion **-->
          <xsl:for-each select="Tarifas/_UpdateRate">
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
                <xsl:value-of select="Excepciones"/>
              </td>
              <td>
                &#160;
                <xsl:value-of select="waitListAvailable"/>
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

          <xsl:for-each select="Tarifas/_UpdateRateByDay">
            <xsl:if test="position() = 1">
              <xsl:call-template name="HeaderTarifaByDay">
              </xsl:call-template>
            </xsl:if>
            <tr>
              <td>
                <xsl:value-of select="CodigoTarifa"/>
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
                <xsl:value-of select="format-number(PrecioExtraAdulto, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(NiniosRate, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(PrecioExtraNinio, '#####0.00', 'pesos')"/>
              </td>
            </tr>
          </xsl:for-each>

          <!--***** TARIFAS NETAS *****-->
          <xsl:for-each select="Tarifas/_UpdateRateNR">
            <xsl:if test="position() = 1">
              <xsl:call-template name="HeaderTarifaNet">
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
                <xsl:value-of select="format-number(NiniosRate, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(PrecioAdolescente, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(PrecioExtraAdulto, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(PrecioExtraNinio, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(PrecioAdolescenteExtra, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(PrecioNR, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(NiniosRateNR, '#####0.00', 'pesos')"/>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="format-number(PrecioAdolescenteNR, '#####0.00', 'pesos')"/>
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
          <xsl:for-each select="Tarifas/_UpdateRateNR">
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
                <xsl:value-of select="Excepciones"/>
              </td>
              <td>
                &#160;
                <xsl:value-of select="waitListAvailable"/>
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
      <td>Habitación/Plan</td>
      <td>Fecha Inicia</td>
      <td>Fecha Fin</td>
      <td>Precio</td>
      <td>P.Niño</td>
      <td>P.Adolescente</td>
      <td>P.Adulto Extra</td>
      <td>P.Niño Extra</td>
      <td>P.Adol Extra</td>
      <td>Arrivos</td>
    </tr>
  </xsl:template>


  <xsl:template name="HeaderTarifa">
    <tr bgcolor="#668800">
      <td>Habitación/Plan</td>
      <td>Fecha Inicia</td>
      <td>Fecha Fin</td>
      <td>Precio</td>
      <td>P.Niño</td>
      <td>P. Adol.</td>
      <td>P.Adulto Extra</td>
      <td>P.Niño Extra</td>
      <td>P.Adol Extra</td>
    </tr>
  </xsl:template>

  <xsl:template name="HeaderTarifaNet">
    <tr bgcolor="#668800">
      <td>Habitación/Plan</td>
      <td>Fecha Inicia</td>
      <td>Fecha Fin</td>
      <td>Precio</td>
      <td>P.Niño</td>
      <td>P. Adol.</td>
      <td>P.Adulto Extra</td>
      <td>P.Niño Extra</td>
      <td>P.Adol Extra</td>
      <td>Precio NR</td>
      <td>P.Niño NR</td>
      <td>P. Adol. NR</td>
      <td>P.Adulto Extra NR</td>
      <td>P.Niño Extra NR</td>
      <td>P.Adol Extra NR</td>
    </tr>
  </xsl:template>

  <xsl:template name="HeaderTarifa2">
    <tr bgcolor="#668800">
      <td>Reglas Def</td>
      <td>Pers.</td>
      <td>Max. Adultos</td>
      <td>Min. Adultos</td>
      <td>Max. Niños</td>
      <td>Pers. Extras</td>
      <td>Adv. Booking</td>
      <td>Max. días</td>
      <td>Min. días</td>
      <td>Arrivos</td>
      <td>Excepciones</td>
      <td>Lista Esp.</td>
      <td>Vent. Inicio</td>
      <td>Vent. Fin</td>
      <td>Desc.Promoción</td>
    </tr>
  </xsl:template>

  <xsl:template name="HeaderTarifaByDay">
    <tr bgcolor="#668800">
      <td>Room/Rate Plan</td>
      <td>Start Date</td>
      <td>End Date</td>
      <td>Adult Price</td>
      <td>Adult ext.P.</td>
      <td>Child P. </td>
      <td>Child ext.P.</td>
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
      <xsl:when test="$mo = '01'">Ene</xsl:when>
      <xsl:when test="$mo = '02'">Feb</xsl:when>
      <xsl:when test="$mo = '03'">Mar</xsl:when>
      <xsl:when test="$mo = '04'">Abr</xsl:when>
      <xsl:when test="$mo = '05'">May</xsl:when>
      <xsl:when test="$mo = '06'">Jun</xsl:when>
      <xsl:when test="$mo = '07'">Jul</xsl:when>
      <xsl:when test="$mo = '08'">Ago</xsl:when>
      <xsl:when test="$mo = '09'">Sep</xsl:when>
      <xsl:when test="$mo = '10'">Oct</xsl:when>
      <xsl:when test="$mo = '11'">Nov</xsl:when>
      <xsl:when test="$mo = '12'">Dic</xsl:when>
    </xsl:choose>
    <xsl:value-of select="'-'"/>
    <xsl:value-of select="$year"/>
  </xsl:template>

</xsl:stylesheet>