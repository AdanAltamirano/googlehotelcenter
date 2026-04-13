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
		    <table style="min-width: 1200px;" border="1" cellpadding="0" cellspacing="0">
				
					<xsl:call-template name="Header">
						<xsl:with-param name="row" select="Hoteles/UpdateHotel[1]"/>
					</xsl:call-template>
				
				<xsl:for-each select="Hoteles/UpdateHotel">
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
							<xsl:choose>
								<xsl:when test="AvailOnGDS = 'true'">Yes</xsl:when>
								<xsl:otherwise>No</xsl:otherwise>
							</xsl:choose>
						</td>
						<td  style="text-align:center;">
							<xsl:choose>
								<xsl:when test="AvailOnADS = 'true'">Yes</xsl:when>
								<xsl:otherwise>No</xsl:otherwise>
							</xsl:choose>
						</td>
						<td  style="text-align:center;">
							<xsl:choose>
								<xsl:when test="AvailOnOnePage = 'true'">Yes</xsl:when>
								<xsl:otherwise>No</xsl:otherwise>
							</xsl:choose>
						</td>
						<td  style="text-align:center;">
							<xsl:value-of select="NoArrivals"/>
						</td>
						<td  style="text-align:center;">
							<xsl:choose>
								<xsl:when test="AllowBankDeposit = 'true'">Yes</xsl:when>
								<xsl:otherwise>No</xsl:otherwise>
							</xsl:choose>
						</td>
						<td style="text-align:center;">
							<xsl:choose>
								<xsl:when test="PlusTax = 'true'">Yes</xsl:when>
								<xsl:otherwise>No</xsl:otherwise>
							</xsl:choose>
						</td>
						<td  style="text-align:center;">
							<xsl:value-of select="format-number(Impuesto, '#,##0.00')"/>
						</td>
						<td  style="text-align:center;">
							<xsl:value-of select="format-number(Ecotasa, '#,##0.00')"/>
						</td>
						<td  style="text-align:center;">
							<xsl:value-of select="format-number(ComisionAgentes, '#,##0.00')"/>
						</td>
						<td style="text-align:center;">
							<xsl:choose>
								<xsl:when test="EsMoroso = 'true'">Yes</xsl:when>
								<xsl:otherwise>No</xsl:otherwise>
							</xsl:choose>
						</td>
						<td style="text-align:center;">
							<xsl:choose>
								<xsl:when test="EsPagoCero = 'true'">Yes</xsl:when>
								<xsl:otherwise>No</xsl:otherwise>
							</xsl:choose>
						</td>
						<td style="text-align:center;">
							<xsl:choose>
								<xsl:when test="AmhmRes = 'true'">Yes</xsl:when>
								<xsl:otherwise>No</xsl:otherwise>
							</xsl:choose>
						</td>
						<td style="text-align:center;">
							<xsl:choose>
								<xsl:when test="Emprhotur = 'true'">Yes</xsl:when>
								<xsl:otherwise>No</xsl:otherwise>
							</xsl:choose>
						</td>
						<td>
							<xsl:choose>
								<!-- Hora específica -->
								<xsl:when test="normalize-space(CancelPriorSpecificT) != ''">
									<xsl:variable name="hora" select="normalize-space(CancelPriorSpecificT)"/>
									<xsl:value-of select="concat(substring($hora,1,2), ':', substring($hora,3,2))"/>
								</xsl:when>
								<!-- Horas -->
								<xsl:when test="normalize-space(CancelPriorHours) != ''">
									<xsl:value-of select="concat(CancelPriorHours, ' hrs')"/>
								</xsl:when>
								<!-- Días -->
								<xsl:when test="normalize-space(DiasMinCancelar) != ''">
									<xsl:value-of select="concat(DiasMinCancelar, ' días')"/>
								</xsl:when>
								<!-- Si no hay ninguno -->
								<xsl:otherwise>
									<!-- vacío -->
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td  style="text-align:center;">
							<xsl:value-of select="MinNumCuartos"/>
						</td>
						<td  style="text-align:center;">
							<xsl:value-of select="MaxNumCuartos"/>
						</td>
						<td  style="text-align:center;">
							<xsl:value-of select="MinNumOcupacion"/>
						</td>
						<td  style="text-align:center;">
							<xsl:value-of select="MaxNumOcupacion"/>
						</td>
						<td  style="text-align:center;">
							<xsl:value-of select="MaxEdadNinio"/>
						</td>
						<td  style="text-align:center;">
							<xsl:value-of select="EdadAdolecente"/>
						</td>
						<td  style="text-align:center;">
							<xsl:value-of select="MinEdadNinio"/>
						</td>
						<td  style="text-align:center;">
							<xsl:value-of select="DiasLibres"/>
						</td>
					</tr>
				</xsl:for-each>
			</table>
        <br/>
        <span>Información General Hotel, Actualizado</span>
        <table border="1" cellpadding="0" cellspacing="0">
			<xsl:call-template name="Header">
				<xsl:with-param name="row" select="Hoteles/_UpdateHotel[1]"/>
			</xsl:call-template>
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
				<xsl:choose>
					<xsl:when test="AvailOnGDS = 'true'">Yes</xsl:when>
					<xsl:otherwise>No</xsl:otherwise>
				</xsl:choose>
			  </td>
			  <td  style="text-align:center;">
				<xsl:choose>
					<xsl:when test="AvailOnADS = 'true'">Yes</xsl:when>
					<xsl:otherwise>No</xsl:otherwise>
				</xsl:choose>
			  </td>
			  <td  style="text-align:center;">
				<xsl:choose>
					<xsl:when test="AvailOnOnePage = 'true'">Yes</xsl:when>
					<xsl:otherwise>No</xsl:otherwise>
				</xsl:choose>
			  </td>
              <td  style="text-align:center;">
                <xsl:value-of select="NoArrivals"/>
              </td>
			  <td  style="text-align:center;">
				<xsl:choose>
					<xsl:when test="AllowBankDeposit = 'true'">Yes</xsl:when>
					<xsl:otherwise>No</xsl:otherwise>
				</xsl:choose>
			  </td>
			  <td style="text-align:center;">
				<xsl:choose>
					<xsl:when test="PlusTax = 'true'">Yes</xsl:when>
					<xsl:otherwise>No</xsl:otherwise>
				</xsl:choose>
			  </td>
			  <td  style="text-align:center;">
				<xsl:value-of select="format-number(Impuesto, '#,##0.00')"/>
			  </td>
			  <td  style="text-align:center;">
				<xsl:value-of select="format-number(Ecotasa, '#,##0.00')"/>
			  </td>
			  <td  style="text-align:center;">
				<xsl:value-of select="format-number(ComisionAgentes, '#,##0.00')"/>
			  </td>
			  <td style="text-align:center;">
				<xsl:choose>
					<xsl:when test="EsMoroso = 'true'">Yes</xsl:when>
					<xsl:otherwise>No</xsl:otherwise>
				</xsl:choose>
			  </td>
			  <td style="text-align:center;">
				<xsl:choose>
					<xsl:when test="EsPagoCero = 'true'">Yes</xsl:when>
					<xsl:otherwise>No</xsl:otherwise>
				</xsl:choose>
			  </td>
			  <td style="text-align:center;">
				<xsl:choose>
					<xsl:when test="AmhmRes = 'true'">Yes</xsl:when>
					<xsl:otherwise>No</xsl:otherwise>
				</xsl:choose>
			  </td>
			  <td style="text-align:center;">
				<xsl:choose>
					<xsl:when test="Emprhotur = 'true'">Yes</xsl:when>
					<xsl:otherwise>No</xsl:otherwise>
				</xsl:choose>
			  </td>
			  <td>
				<xsl:choose>
					<!-- Hora específica -->
					<xsl:when test="normalize-space(CancelPriorSpecificT) != ''">
						<xsl:variable name="hora" select="normalize-space(CancelPriorSpecificT)"/>
						<xsl:value-of select="concat(substring($hora,1,2), ':', substring($hora,3,2))"/>
					</xsl:when>
					<!-- Horas -->
					<xsl:when test="normalize-space(CancelPriorHours) != ''">
						<xsl:value-of select="concat(CancelPriorHours, ' hrs')"/>
					</xsl:when>
					<!-- Días -->
					<xsl:when test="normalize-space(DiasMinCancelar) != ''">
						<xsl:value-of select="concat(DiasMinCancelar, ' días')"/>
					</xsl:when>
					<!-- Si no hay ninguno -->
					<xsl:otherwise>
						<!-- vacío -->
					</xsl:otherwise>
				</xsl:choose>
			  </td>
				<td  style="text-align:center;">
					<xsl:value-of select="MinNumCuartos"/>
				</td>
				<td  style="text-align:center;">
					<xsl:value-of select="MaxNumCuartos"/>
				</td>
				<td  style="text-align:center;">
					<xsl:value-of select="MinNumOcupacion"/>
				</td>
				<td  style="text-align:center;">
					<xsl:value-of select="MaxNumOcupacion"/>
				</td>
				<td  style="text-align:center;">
					<xsl:value-of select="MaxEdadNinio"/>
				</td>
				<td  style="text-align:center;">
					<xsl:value-of select="EdadAdolecente"/>
				</td>
				<td  style="text-align:center;">
					<xsl:value-of select="MinEdadNinio"/>
				</td>
				<td  style="text-align:center;">
					<xsl:value-of select="DiasLibres"/>
				</td>
            </tr>
          </xsl:for-each>
        </table>

      </body>
    </html>
  </xsl:template>

  <xsl:template name="Header">
	<xsl:param name="row"/>
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
	  <td>Disp. GDS</td>
	  <td>Disp. ADS</td>
	  <td>Disp. Unipantalla</td>
      <td>
        No Arrivos<br/>DLMMJVS
      </td>
	  <td>Permite depósito bancario</td>
	  <td>Precios Incluyen Impuesto</td>
	  <td>Impuesto</td>
	  <td>Ecotasa</td>
	  <td>Comisión Agentes Viajes</td>
	  <td>Es Moroso</td>
	  <td>Es Pago Cero</td>
	  <td>Reservaciones Amhm</td>
	  <td>Socio EMPRHOTUR</td>
	  <td>
		<xsl:choose>
			<xsl:when test="normalize-space($row/CancelPriorSpecificT) != ''">
				Cancelación Hora Específica
			</xsl:when>
			<xsl:when test="normalize-space($row/CancelPriorHours) != ''">
				Cancelación Por Horas
			</xsl:when>
			<xsl:when test="normalize-space($row/DiasMinCancelar) != ''">
				Cancelación Por Días
			</xsl:when>
			<xsl:otherwise></xsl:otherwise>
		</xsl:choose>
	  </td>
	  <td>Mínimo Habitaciones Reservar</td>
	  <td>Máximo Habitaciones Reservar</td>
	  <td>Mínimo Ocupación Reservar</td>
	  <td>Máximo Ocupación Reservar</td>
	  <td>Edad Niño</td>
	  <td>Edad Junior</td>
	  <td>Edad No Cobrar</td>
	  <td>Reservar Días Antes</td>
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