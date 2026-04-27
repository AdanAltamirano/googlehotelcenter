<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:decimal-format name="pesos" grouping-separator=","/>

  <xsl:template match="/">
    <html>
      <style type="text/css">
        .logdiff{font-family:-apple-system,BlinkMacSystemFont,'Segoe UI',Arial,sans-serif;font-size:12px;color:#1e293b;padding:6px;}
        /* Summary bar */
        .ld-summary{display:none;align-items:center;gap:8px;padding:8px 12px;background:linear-gradient(135deg,#eff6ff,#dbeafe);border:1px solid #bfdbfe;border-radius:8px;margin-bottom:10px;font-size:11px;color:#1d4ed8;font-weight:600;}
        .ld-summary-icon{font-size:16px;}
        /* Cards */
        .ld-card{border-radius:8px;margin-bottom:6px;overflow:hidden;box-shadow:0 2px 10px rgba(0,0,0,.12);}
        .ld-head{padding:10px 14px;font-weight:700;font-size:13px;display:flex;align-items:center;gap:10px;}
        .ld-head.before{background:linear-gradient(135deg,#fff7ed 0%,#fef3c7 100%);border-left:5px solid #f59e0b;color:#92400e;}
        .ld-head.after{background:linear-gradient(135deg,#f0fdf4 0%,#dcfce7 100%);border-left:5px solid #22c55e;color:#14532d;}
        .ld-head.deleted{background:linear-gradient(135deg,#fef2f2 0%,#fee2e2 100%);border-left:5px solid #dc2626;color:#991b1b;}
        /* Badges */
        .ld-badge{display:inline-block;padding:3px 10px;border-radius:20px;font-size:10px;font-weight:800;letter-spacing:1px;}
        .ld-badge.before{background:linear-gradient(135deg,#f59e0b,#d97706);color:#fff;box-shadow:0 1px 3px rgba(217,119,6,.4);}
        .ld-badge.after{background:linear-gradient(135deg,#22c55e,#16a34a);color:#fff;box-shadow:0 1px 3px rgba(22,163,74,.4);}
        .ld-badge.deleted{background:linear-gradient(135deg,#ef4444,#b91c1c);color:#fff;box-shadow:0 1px 3px rgba(185,28,28,.4);}
        /* Estilo de filas en estado ELIMINADO */
        .ld-deleted-rows td{background:#fef2f2 !important;color:#991b1b !important;}
        /* Separator */
        .ld-sep{display:flex;align-items:center;gap:8px;margin:4px 0 8px;}
        .ld-sep-line{flex:1;height:1px;background:#e2e8f0;}
        .ld-sep-txt{font-size:9px;font-weight:800;letter-spacing:1.5px;color:#94a3b8;text-transform:uppercase;padding:2px 8px;border:1px solid #e2e8f0;border-radius:4px;white-space:nowrap;}
        /* Tables */
        .ld-tbl{width:100%;border-collapse:collapse;font-size:11px;}
        .ld-tbl th{background:linear-gradient(180deg,#475569,#334155);color:#f8fafc;padding:6px 8px;text-align:left;font-weight:600;white-space:nowrap;border-right:1px solid #4a5568;}
        .ld-tbl th:last-child{border-right:none;}
        .ld-tbl td{padding:5px 8px;white-space:nowrap;border-bottom:1px solid #e2e8f0;border-right:1px solid #f1f5f9;transition:background .15s;}
        .ld-tbl td:last-child{border-right:none;}
        .ld-tbl tbody tr:nth-child(even) td{background:#f8fafc;}
        .ld-tbl tbody tr:nth-child(odd) td{background:#fff;}
        .ld-tbl tbody tr:hover td{background:#eff6ff !important;cursor:default;}
        .ld-tbl .r{text-align:right;}
        .ld-tbl-rules thead th{background:linear-gradient(180deg,#64748b,#475569);}
        /* Changed cells */
        .logdiff td.ch-b{background:#fff1f2 !important;color:#dc2626 !important;font-weight:700;border-left:3px solid #f87171 !important;text-decoration:line-through;}
        .logdiff td.ch-a{background:#f0fdf4 !important;color:#16a34a !important;font-weight:700;border-left:3px solid #4ade80 !important;}
        /* Legend */
        .ld-legend{display:none;align-items:center;gap:14px;font-size:10px;padding:6px 12px;color:#64748b;background:#f8fafc;border-top:1px solid #e2e8f0;}
        .ld-legend span{display:inline-flex;align-items:center;gap:5px;}
        .ld-dot{display:inline-block;width:10px;height:10px;border-radius:2px;}
      </style>
      <script type="text/javascript">
        <xsl:text disable-output-escaping="yes">
function ldDiff(){
  var pairs=[
    ['ld-b-r','ld-a-r'],['ld-b-r2','ld-a-r2'],
    ['ld-b-d','ld-a-d'],
    ['ld-b-n','ld-a-n'],['ld-b-n2','ld-a-n2']
  ];
  var total=0;
  for(var p=0;p&lt;pairs.length;p++){
    var b=document.getElementById(pairs[p][0]);
    var a=document.getElementById(pairs[p][1]);
    if(!b||!a)continue;
    var br=b.querySelectorAll('tbody tr');
    var ar=a.querySelectorAll('tbody tr');
    for(var r=0;r&lt;br.length&amp;&amp;r&lt;ar.length;r++){
      var bc=br[r].querySelectorAll('td');
      var ac=ar[r].querySelectorAll('td');
      for(var c=0;c&lt;bc.length&amp;&amp;c&lt;ac.length;c++){
        if(bc[c].textContent.trim()!==ac[c].textContent.trim()){
          bc[c].className='ch-b';
          ac[c].className='ch-a';
          total++;
        }
      }
    }
  }
  var leg=document.getElementById('ld-legend');
  if(leg&amp;&amp;total>0){leg.style.display='flex';}
  var bar=document.getElementById('ld-summary');
  var cnt=document.getElementById('ld-count');
  if(bar&amp;&amp;total>0){
    bar.style.display='flex';
    if(cnt){cnt.textContent=total+' campo'+(total!==1?'s':'')+' modificado'+(total!==1?'s':'');}
  }
}
document.readyState!=='loading'?ldDiff():document.addEventListener('DOMContentLoaded',ldDiff);
        </xsl:text>
      </script>
      <body>
        <div class="logdiff">

          <!-- Barra de resumen (visible sólo si hay cambios) -->
          <xsl:if test="(Tarifas/UpdateRate or Tarifas/UpdateRateByDay or Tarifas/UpdateRateNR) and (Tarifas/_UpdateRate or Tarifas/_UpdateRateByDay or Tarifas/_UpdateRateNR)">
            <div class="ld-summary" id="ld-summary">
              <span class="ld-summary-icon">&#9432;</span>
              <span id="ld-count"></span>
            </div>
          </xsl:if>

          <!-- Detectar estado ELIMINADO: hay "antes" pero NO hay "despues" -->
          <xsl:variable name="isDeleted" select="(Tarifas/UpdateRate or Tarifas/UpdateRateByDay or Tarifas/UpdateRateNR) and not(Tarifas/_UpdateRate or Tarifas/_UpdateRateByDay or Tarifas/_UpdateRateNR)"/>

          <!-- ===== ANTES / ELIMINADO ===== -->
          <xsl:if test="Tarifas/UpdateRate or Tarifas/UpdateRateByDay or Tarifas/UpdateRateNR">
            <div>
              <xsl:attribute name="class">
                <xsl:choose>
                  <xsl:when test="$isDeleted">ld-card ld-deleted-rows</xsl:when>
                  <xsl:otherwise>ld-card</xsl:otherwise>
                </xsl:choose>
              </xsl:attribute>
              <xsl:choose>
                <xsl:when test="$isDeleted">
                  <div class="ld-head deleted">
                    <span class="ld-badge deleted">&#10006; ELIMINADO</span>
                    <xsl:if test="Tarifas/UpdateRate">Tarifas eliminadas</xsl:if>
                    <xsl:if test="Tarifas/UpdateRateByDay"> &#183; Tarifas por Día</xsl:if>
                    <xsl:if test="Tarifas/UpdateRateNR"> &#183; Tarifas Netas</xsl:if>
                  </div>
                </xsl:when>
                <xsl:otherwise>
                  <div class="ld-head before">
                    <span class="ld-badge before">&#9664; ANTES</span>
                    <xsl:if test="Tarifas/UpdateRate">Tarifas</xsl:if>
                    <xsl:if test="Tarifas/UpdateRateByDay"> &#183; Tarifas por Día</xsl:if>
                    <xsl:if test="Tarifas/UpdateRateNR"> &#183; Tarifas Netas</xsl:if>
                  </div>
                </xsl:otherwise>
              </xsl:choose>

              <xsl:if test="Tarifas/UpdateRate">
                <table class="ld-tbl" id="ld-b-r">
                  <thead><tr>
                    <th>Habitación/Plan</th><th>Fecha Inicia</th><th>Fecha Fin</th>
                    <th>Precio</th><th>P.Niño</th><th>P. Adol.</th>
                    <th>P.Adulto Extra</th><th>P.Niño Extra</th><th>P.Adol Extra</th>
                  </tr></thead>
                  <tbody>
                    <xsl:for-each select="Tarifas/UpdateRate">
                      <tr>
                        <td><xsl:value-of select="Descr_rateplan"/></td>
                        <td><xsl:call-template name="FormatDate"><xsl:with-param name="DateTime" select="FechaInicia"/></xsl:call-template></td>
                        <td><xsl:call-template name="FormatDate"><xsl:with-param name="DateTime" select="FechaFinaliza"/></xsl:call-template></td>
                        <td class="r"><xsl:value-of select="format-number(Precio,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(niniosrate,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(PrecioAdolescente,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(PrecioExtraAdulto,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(PrecioExtraNinio,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(PrecioAdolescenteExtra,'#####0.00','pesos')"/></td>
                      </tr>
                    </xsl:for-each>
                  </tbody>
                </table>
                <table class="ld-tbl ld-tbl-rules" id="ld-b-r2">
                  <thead><tr>
                    <th>Reglas Def</th><th>Pers.</th><th>Max. Adultos</th><th>Min. Adultos</th>
                    <th>Max. Niños</th><th>Pers. Extras</th><th>Adv. Booking</th>
                    <th>Max. días</th><th>Min. días</th><th>Arrivos</th>
                    <th>Excepciones</th><th>Lista Esp.</th><th>Vent. Inicio</th>
                    <th>Vent. Fin</th><th>Desc.Promoción</th>
                  </tr></thead>
                  <tbody>
                    <xsl:for-each select="Tarifas/UpdateRate">
                      <tr>
                        <td><xsl:value-of select="RateRulesDefault"/></td>
                        <td><xsl:value-of select="Personas"/></td>
                        <td><xsl:value-of select="MaxAdultos"/></td>
                        <td><xsl:value-of select="MinAdultos"/></td>
                        <td><xsl:value-of select="MaxNinios"/></td>
                        <td><xsl:value-of select="PersonasExtras"/></td>
                        <td><xsl:value-of select="AdvBooking"/></td>
                        <td><xsl:value-of select="MaxDias"/></td>
                        <td><xsl:value-of select="MinDias"/></td>
                        <td><xsl:value-of select="NoArrivos"/></td>
                        <td><xsl:value-of select="Excepciones"/></td>
                        <td><xsl:value-of select="waitListAvailable"/></td>
                        <td><xsl:call-template name="FormatDate"><xsl:with-param name="DateTime" select="BookingWindowStart"/></xsl:call-template></td>
                        <td><xsl:call-template name="FormatDate"><xsl:with-param name="DateTime" select="BookingWindowEnd"/></xsl:call-template></td>
                        <td class="r">
                          <xsl:variable name="dp1"><xsl:value-of select="DescPromotion"/></xsl:variable>
                          <xsl:if test="$dp1!=''"><xsl:value-of select="format-number(DescPromotion,'#####0.00','pesos')"/></xsl:if>
                        </td>
                      </tr>
                    </xsl:for-each>
                  </tbody>
                </table>
              </xsl:if>

              <xsl:if test="Tarifas/UpdateRateByDay">
                <table class="ld-tbl" id="ld-b-d">
                  <thead><tr>
                    <th>Habitación/Plan</th><th>Fecha Inicia</th><th>Fecha Fin</th>
                    <th>Precio Adulto</th><th>P. Extra Adulto</th><th>P. Niño</th><th>P. Extra Niño</th>
                  </tr></thead>
                  <tbody>
                    <xsl:for-each select="Tarifas/UpdateRateByDay">
                      <tr>
                        <td><xsl:value-of select="CodigoTarifa"/></td>
                        <td><xsl:call-template name="FormatDate"><xsl:with-param name="DateTime" select="FechaInicia"/></xsl:call-template></td>
                        <td><xsl:call-template name="FormatDate"><xsl:with-param name="DateTime" select="FechaFinaliza"/></xsl:call-template></td>
                        <td class="r"><xsl:value-of select="format-number(Precio,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(PrecioExtraAdulto,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(niniosrate,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(PrecioExtraNinio,'#####0.00','pesos')"/></td>
                      </tr>
                    </xsl:for-each>
                  </tbody>
                </table>
              </xsl:if>

              <xsl:if test="Tarifas/UpdateRateNR">
                <table class="ld-tbl" id="ld-b-n">
                  <thead><tr>
                    <th>Habitación/Plan</th><th>Fecha Inicia</th><th>Fecha Fin</th>
                    <th>Precio</th><th>P.Niño</th><th>P. Adol.</th>
                    <th>P.Adulto Extra</th><th>P.Niño Extra</th><th>P.Adol Extra</th>
                    <th>Precio NR</th><th>P.Niño NR</th><th>P. Adol. NR</th>
                    <th>P.Adulto Extra NR</th><th>P.Niño Extra NR</th><th>P.Adol Extra NR</th>
                  </tr></thead>
                  <tbody>
                    <xsl:for-each select="Tarifas/UpdateRateNR">
                      <tr>
                        <td><xsl:value-of select="Descr_rateplan"/></td>
                        <td><xsl:call-template name="FormatDate"><xsl:with-param name="DateTime" select="FechaInicia"/></xsl:call-template></td>
                        <td><xsl:call-template name="FormatDate"><xsl:with-param name="DateTime" select="FechaFinaliza"/></xsl:call-template></td>
                        <td class="r"><xsl:value-of select="format-number(Precio,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(niniosrate,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(PrecioAdolescente,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(PrecioExtraAdulto,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(PrecioExtraNinio,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(PrecioAdolescenteExtra,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(PrecioNR,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(niniosrateNR,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(PrecioAdolescenteNR,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(PrecioExtraAdultoNR,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(PrecioExtraNinioNR,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(PrecioAdolescenteExtraNR,'#####0.00','pesos')"/></td>
                      </tr>
                    </xsl:for-each>
                  </tbody>
                </table>
                <table class="ld-tbl ld-tbl-rules" id="ld-b-n2">
                  <thead><tr>
                    <th>Reglas Def</th><th>Pers.</th><th>Max. Adultos</th><th>Min. Adultos</th>
                    <th>Max. Niños</th><th>Pers. Extras</th><th>Adv. Booking</th>
                    <th>Max. días</th><th>Min. días</th><th>Arrivos</th>
                    <th>Excepciones</th><th>Lista Esp.</th><th>Vent. Inicio</th>
                    <th>Vent. Fin</th><th>Desc.Promoción</th>
                  </tr></thead>
                  <tbody>
                    <xsl:for-each select="Tarifas/UpdateRateNR">
                      <tr>
                        <td><xsl:value-of select="RateRulesDefault"/></td>
                        <td><xsl:value-of select="Personas"/></td>
                        <td><xsl:value-of select="MaxAdultos"/></td>
                        <td><xsl:value-of select="MinAdultos"/></td>
                        <td><xsl:value-of select="MaxNinios"/></td>
                        <td><xsl:value-of select="PersonasExtras"/></td>
                        <td><xsl:value-of select="AdvBooking"/></td>
                        <td><xsl:value-of select="MaxDias"/></td>
                        <td><xsl:value-of select="MinDias"/></td>
                        <td><xsl:value-of select="NoArrivos"/></td>
                        <td><xsl:value-of select="Excepciones"/></td>
                        <td><xsl:value-of select="waitListAvailable"/></td>
                        <td><xsl:call-template name="FormatDate"><xsl:with-param name="DateTime" select="BookingWindowStart"/></xsl:call-template></td>
                        <td><xsl:call-template name="FormatDate"><xsl:with-param name="DateTime" select="BookingWindowEnd"/></xsl:call-template></td>
                        <td class="r">
                          <xsl:variable name="dp2"><xsl:value-of select="DescPromotion"/></xsl:variable>
                          <xsl:if test="$dp2!=''"><xsl:value-of select="format-number(DescPromotion,'#####0.00','pesos')"/></xsl:if>
                        </td>
                      </tr>
                    </xsl:for-each>
                  </tbody>
                </table>
              </xsl:if>
            </div>
          </xsl:if>

          <!-- Separador entre secciones -->
          <xsl:if test="(Tarifas/UpdateRate or Tarifas/UpdateRateByDay or Tarifas/UpdateRateNR) and (Tarifas/_UpdateRate or Tarifas/_UpdateRateByDay or Tarifas/_UpdateRateNR)">
            <div class="ld-sep">
              <div class="ld-sep-line"></div>
              <div class="ld-sep-txt">&#9660; ACTUALIZADO A</div>
              <div class="ld-sep-line"></div>
            </div>
          </xsl:if>

          <!-- ===== DESPUÉS ===== -->
          <xsl:if test="Tarifas/_UpdateRate or Tarifas/_UpdateRateByDay or Tarifas/_UpdateRateNR">
            <div class="ld-card">
              <div class="ld-head after">
                <span class="ld-badge after">&#9654; DESPUÉS</span>
                <xsl:if test="Tarifas/_UpdateRate">Tarifas</xsl:if>
                <xsl:if test="Tarifas/_UpdateRateByDay"> &#183; Tarifas por Día</xsl:if>
                <xsl:if test="Tarifas/_UpdateRateNR"> &#183; Tarifas Netas</xsl:if>
              </div>

              <xsl:if test="Tarifas/_UpdateRate">
                <table class="ld-tbl" id="ld-a-r">
                  <thead><tr>
                    <th>Habitación/Plan</th><th>Fecha Inicia</th><th>Fecha Fin</th>
                    <th>Precio</th><th>P.Niño</th><th>P. Adol.</th>
                    <th>P.Adulto Extra</th><th>P.Niño Extra</th><th>P.Adol Extra</th>
                  </tr></thead>
                  <tbody>
                    <xsl:for-each select="Tarifas/_UpdateRate">
                      <tr>
                        <td><xsl:value-of select="Descr_rateplan"/></td>
                        <td><xsl:call-template name="FormatDate"><xsl:with-param name="DateTime" select="FechaInicia"/></xsl:call-template></td>
                        <td><xsl:call-template name="FormatDate"><xsl:with-param name="DateTime" select="FechaFinaliza"/></xsl:call-template></td>
                        <td class="r"><xsl:value-of select="format-number(Precio,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(NiniosRate,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(PrecioAdolescente,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(PrecioExtraAdulto,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(PrecioExtraNinio,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(PrecioAdolescenteExtra,'#####0.00','pesos')"/></td>
                      </tr>
                    </xsl:for-each>
                  </tbody>
                </table>
                <table class="ld-tbl ld-tbl-rules" id="ld-a-r2">
                  <thead><tr>
                    <th>Reglas Def</th><th>Pers.</th><th>Max. Adultos</th><th>Min. Adultos</th>
                    <th>Max. Niños</th><th>Pers. Extras</th><th>Adv. Booking</th>
                    <th>Max. días</th><th>Min. días</th><th>Arrivos</th>
                    <th>Excepciones</th><th>Lista Esp.</th><th>Vent. Inicio</th>
                    <th>Vent. Fin</th><th>Desc.Promoción</th>
                  </tr></thead>
                  <tbody>
                    <xsl:for-each select="Tarifas/_UpdateRate">
                      <tr>
                        <td><xsl:value-of select="RateRulesDefault"/></td>
                        <td><xsl:value-of select="Personas"/></td>
                        <td><xsl:value-of select="MaxAdultos"/></td>
                        <td><xsl:value-of select="MinAdultos"/></td>
                        <td><xsl:value-of select="MaxNinios"/></td>
                        <td><xsl:value-of select="PersonasExtras"/></td>
                        <td><xsl:value-of select="AdvBooking"/></td>
                        <td><xsl:value-of select="MaxDias"/></td>
                        <td><xsl:value-of select="MinDias"/></td>
                        <td><xsl:value-of select="NoArrivos"/></td>
                        <td><xsl:value-of select="Excepciones"/></td>
                        <td><xsl:value-of select="waitListAvailable"/></td>
                        <td><xsl:call-template name="FormatDate"><xsl:with-param name="DateTime" select="BookingWindowStart"/></xsl:call-template></td>
                        <td><xsl:call-template name="FormatDate"><xsl:with-param name="DateTime" select="BookingWindowEnd"/></xsl:call-template></td>
                        <td class="r">
                          <xsl:variable name="dp3"><xsl:value-of select="DescPromotion"/></xsl:variable>
                          <xsl:if test="$dp3!=''"><xsl:value-of select="format-number(DescPromotion,'#####0.00','pesos')"/></xsl:if>
                        </td>
                      </tr>
                    </xsl:for-each>
                  </tbody>
                </table>
              </xsl:if>

              <xsl:if test="Tarifas/_UpdateRateByDay">
                <table class="ld-tbl" id="ld-a-d">
                  <thead><tr>
                    <th>Habitación/Plan</th><th>Fecha Inicia</th><th>Fecha Fin</th>
                    <th>Precio Adulto</th><th>P. Extra Adulto</th><th>P. Niño</th><th>P. Extra Niño</th>
                  </tr></thead>
                  <tbody>
                    <xsl:for-each select="Tarifas/_UpdateRateByDay">
                      <tr>
                        <td><xsl:value-of select="CodigoTarifa"/></td>
                        <td><xsl:call-template name="FormatDate"><xsl:with-param name="DateTime" select="FechaInicia"/></xsl:call-template></td>
                        <td><xsl:call-template name="FormatDate"><xsl:with-param name="DateTime" select="FechaFinaliza"/></xsl:call-template></td>
                        <td class="r"><xsl:value-of select="format-number(Precio,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(PrecioExtraAdulto,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(NiniosRate,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(PrecioExtraNinio,'#####0.00','pesos')"/></td>
                      </tr>
                    </xsl:for-each>
                  </tbody>
                </table>
              </xsl:if>

              <xsl:if test="Tarifas/_UpdateRateNR">
                <table class="ld-tbl" id="ld-a-n">
                  <thead><tr>
                    <th>Habitación/Plan</th><th>Fecha Inicia</th><th>Fecha Fin</th>
                    <th>Precio</th><th>P.Niño</th><th>P. Adol.</th>
                    <th>P.Adulto Extra</th><th>P.Niño Extra</th><th>P.Adol Extra</th>
                    <th>Precio NR</th><th>P.Niño NR</th><th>P. Adol. NR</th>
                    <th>P.Adulto Extra NR</th><th>P.Niño Extra NR</th><th>P.Adol Extra NR</th>
                  </tr></thead>
                  <tbody>
                    <xsl:for-each select="Tarifas/_UpdateRateNR">
                      <tr>
                        <td><xsl:value-of select="Descr_rateplan"/></td>
                        <td><xsl:call-template name="FormatDate"><xsl:with-param name="DateTime" select="FechaInicia"/></xsl:call-template></td>
                        <td><xsl:call-template name="FormatDate"><xsl:with-param name="DateTime" select="FechaFinaliza"/></xsl:call-template></td>
                        <td class="r"><xsl:value-of select="format-number(Precio,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(NiniosRate,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(PrecioAdolescente,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(PrecioExtraAdulto,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(PrecioExtraNinio,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(PrecioAdolescenteExtra,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(PrecioNR,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(NiniosRateNR,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(PrecioAdolescenteNR,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(PrecioExtraAdultoNR,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(PrecioExtraNinioNR,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(PrecioAdolescenteExtraNR,'#####0.00','pesos')"/></td>
                      </tr>
                    </xsl:for-each>
                  </tbody>
                </table>
                <table class="ld-tbl ld-tbl-rules" id="ld-a-n2">
                  <thead><tr>
                    <th>Reglas Def</th><th>Pers.</th><th>Max. Adultos</th><th>Min. Adultos</th>
                    <th>Max. Niños</th><th>Pers. Extras</th><th>Adv. Booking</th>
                    <th>Max. días</th><th>Min. días</th><th>Arrivos</th>
                    <th>Excepciones</th><th>Lista Esp.</th><th>Vent. Inicio</th>
                    <th>Vent. Fin</th><th>Desc.Promoción</th>
                  </tr></thead>
                  <tbody>
                    <xsl:for-each select="Tarifas/_UpdateRateNR">
                      <tr>
                        <td><xsl:value-of select="RateRulesDefault"/></td>
                        <td><xsl:value-of select="Personas"/></td>
                        <td><xsl:value-of select="MaxAdultos"/></td>
                        <td><xsl:value-of select="MinAdultos"/></td>
                        <td><xsl:value-of select="MaxNinios"/></td>
                        <td><xsl:value-of select="PersonasExtras"/></td>
                        <td><xsl:value-of select="AdvBooking"/></td>
                        <td><xsl:value-of select="MaxDias"/></td>
                        <td><xsl:value-of select="MinDias"/></td>
                        <td><xsl:value-of select="NoArrivos"/></td>
                        <td><xsl:value-of select="Excepciones"/></td>
                        <td><xsl:value-of select="waitListAvailable"/></td>
                        <td><xsl:call-template name="FormatDate"><xsl:with-param name="DateTime" select="BookingWindowStart"/></xsl:call-template></td>
                        <td><xsl:call-template name="FormatDate"><xsl:with-param name="DateTime" select="BookingWindowEnd"/></xsl:call-template></td>
                        <td class="r">
                          <xsl:variable name="dp4"><xsl:value-of select="DescPromotion"/></xsl:variable>
                          <xsl:if test="$dp4!=''"><xsl:value-of select="format-number(DescPromotion,'#####0.00','pesos')"/></xsl:if>
                        </td>
                      </tr>
                    </xsl:for-each>
                  </tbody>
                </table>
              </xsl:if>
            </div>
          </xsl:if>

          <!-- Leyenda -->
          <div id="ld-legend" class="ld-legend">
            <span><span class="ld-dot" style="background:#fee2e2;border:1px solid #fca5a5;"></span> Valor anterior (tachado)</span>
            <span><span class="ld-dot" style="background:#f0fdf4;border:1px solid #4ade80;"></span> Valor actualizado</span>
          </div>

        </div>
      </body>
    </html>
  </xsl:template>

  <xsl:template name="FormatDate">
    <xsl:param name="DateTime"/>
    <xsl:variable name="year"><xsl:value-of select="substring($DateTime,1,4)"/></xsl:variable>
    <xsl:variable name="year-temp"><xsl:value-of select="substring-after($DateTime,'-')"/></xsl:variable>
    <xsl:variable name="mo"><xsl:value-of select="substring($year-temp,1,2)"/></xsl:variable>
    <xsl:variable name="mo-temp"><xsl:value-of select="substring-after($year-temp,'-')"/></xsl:variable>
    <xsl:variable name="day"><xsl:value-of select="substring($mo-temp,1,2)"/></xsl:variable>
    <xsl:value-of select="$day"/><xsl:value-of select="'-'"/>
    <xsl:choose>
      <xsl:when test="$mo = '01'">Ene</xsl:when><xsl:when test="$mo = '02'">Feb</xsl:when>
      <xsl:when test="$mo = '03'">Mar</xsl:when><xsl:when test="$mo = '04'">Abr</xsl:when>
      <xsl:when test="$mo = '05'">May</xsl:when><xsl:when test="$mo = '06'">Jun</xsl:when>
      <xsl:when test="$mo = '07'">Jul</xsl:when><xsl:when test="$mo = '08'">Ago</xsl:when>
      <xsl:when test="$mo = '09'">Sep</xsl:when><xsl:when test="$mo = '10'">Oct</xsl:when>
      <xsl:when test="$mo = '11'">Nov</xsl:when><xsl:when test="$mo = '12'">Dic</xsl:when>
    </xsl:choose>
    <xsl:value-of select="'-'"/><xsl:value-of select="$year"/>
  </xsl:template>

</xsl:stylesheet>
