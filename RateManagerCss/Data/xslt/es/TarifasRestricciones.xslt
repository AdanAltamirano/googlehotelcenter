<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:decimal-format name="pesos" grouping-separator=","/>

  <xsl:template match="/">
    <html>
      <style type="text/css">
        .logdiff{font-family:-apple-system,BlinkMacSystemFont,'Segoe UI',Arial,sans-serif;font-size:12px;color:#1e293b;padding:6px;}
        .ld-summary{display:none;align-items:center;gap:8px;padding:8px 12px;background:linear-gradient(135deg,#eff6ff,#dbeafe);border:1px solid #bfdbfe;border-radius:8px;margin-bottom:10px;font-size:11px;color:#1d4ed8;font-weight:600;}
        .ld-summary-icon{font-size:16px;}
        .ld-card{border-radius:8px;margin-bottom:6px;overflow:hidden;box-shadow:0 2px 10px rgba(0,0,0,.12);}
        .ld-head{padding:10px 14px;font-weight:700;font-size:13px;display:flex;align-items:center;gap:10px;}
        .ld-head.before{background:linear-gradient(135deg,#fff7ed 0%,#fef3c7 100%);border-left:5px solid #f59e0b;color:#92400e;}
        .ld-head.after{background:linear-gradient(135deg,#f0fdf4 0%,#dcfce7 100%);border-left:5px solid #22c55e;color:#14532d;}
        .ld-badge{display:inline-block;padding:3px 10px;border-radius:20px;font-size:10px;font-weight:800;letter-spacing:1px;}
        .ld-badge.before{background:linear-gradient(135deg,#f59e0b,#d97706);color:#fff;box-shadow:0 1px 3px rgba(217,119,6,.4);}
        .ld-badge.after{background:linear-gradient(135deg,#22c55e,#16a34a);color:#fff;box-shadow:0 1px 3px rgba(22,163,74,.4);}
        .ld-sep{display:flex;align-items:center;gap:8px;margin:4px 0 8px;}
        .ld-sep-line{flex:1;height:1px;background:#e2e8f0;}
        .ld-sep-txt{font-size:9px;font-weight:800;letter-spacing:1.5px;color:#94a3b8;text-transform:uppercase;padding:2px 8px;border:1px solid #e2e8f0;border-radius:4px;white-space:nowrap;}
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
        .logdiff td.ch-b{background:#fff1f2 !important;color:#dc2626 !important;font-weight:700;border-left:3px solid #f87171 !important;text-decoration:line-through;}
        .logdiff td.ch-a{background:#f0fdf4 !important;color:#16a34a !important;font-weight:700;border-left:3px solid #4ade80 !important;}
        .ld-legend{display:none;align-items:center;gap:14px;font-size:10px;padding:6px 12px;color:#64748b;background:#f8fafc;border-top:1px solid #e2e8f0;}
        .ld-legend span{display:inline-flex;align-items:center;gap:5px;}
        .ld-dot{display:inline-block;width:10px;height:10px;border-radius:2px;}
      </style>
      <script type="text/javascript">
        <xsl:text disable-output-escaping="yes">
function ldDiff(){
  var pairs=[
    ['tr-b-r','tr-a-r'],
    ['tr-b-nr','tr-a-nr'],
    ['tr-b-d','tr-a-d']
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

          <xsl:if test="(TarifasRestricciones/UpdateRateRestriction or TarifasRestricciones/UpdatePlanFaresNR or TarifasRestricciones/UpdateRateRestrictionByDay) and (TarifasRestricciones/_UpdateRateRestriction or TarifasRestricciones/_UpdatePlanFaresNR or TarifasRestricciones/_UpdateRateRestrictionByDay)">
            <div class="ld-summary" id="ld-summary">
              <span class="ld-summary-icon">&#9432;</span>
              <span id="ld-count"></span>
            </div>
          </xsl:if>

          <!-- ===== ANTES ===== -->
          <xsl:if test="TarifasRestricciones/UpdateRateRestriction or TarifasRestricciones/UpdatePlanFaresNR or TarifasRestricciones/UpdateRateRestrictionByDay">
            <div class="ld-card">
              <div class="ld-head before">
                <span class="ld-badge before">&#9664; ANTES</span>
                <xsl:if test="TarifasRestricciones/UpdateRateRestriction">Tarifas por ocupación</xsl:if>
                <xsl:if test="TarifasRestricciones/UpdatePlanFaresNR"> &#183; Tarifas NR</xsl:if>
                <xsl:if test="TarifasRestricciones/UpdateRateRestrictionByDay"> &#183; Tarifas por día</xsl:if>
              </div>

              <xsl:if test="TarifasRestricciones/UpdateRateRestriction">
                <table class="ld-tbl" id="tr-b-r">
                  <thead><tr>
                    <th>Plan tarifario</th><th>Adultos</th><th>P.Adulto Extra</th>
                    <th>Niños</th><th>P.Niño</th><th>P.Adolescente</th>
                  </tr></thead>
                  <tbody>
                    <xsl:for-each select="TarifasRestricciones/UpdateRateRestriction">
                      <tr>
                        <td><xsl:value-of select="Descr_rateplan"/></td>
                        <td class="r"><xsl:value-of select="Adultos"/></td>
                        <td class="r"><xsl:value-of select="format-number(TarifaAdulto,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="Ninios"/></td>
                        <td class="r"><xsl:value-of select="format-number(TarifaNinio,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(TarifaAdolescente,'#####0.00','pesos')"/></td>
                      </tr>
                    </xsl:for-each>
                  </tbody>
                </table>
              </xsl:if>

              <xsl:if test="TarifasRestricciones/UpdatePlanFaresNR">
                <table class="ld-tbl ld-tbl-rules" id="tr-b-nr">
                  <thead><tr>
                    <th>Plan tarifario</th><th>Adultos</th><th>P.Adulto</th><th>P.Adulto NR</th><th>P.Adulto Exc</th>
                    <th>Niños</th><th>P.Niño</th><th>P.Niño NR</th><th>P.Niño Exc</th>
                    <th>P.Adol.</th><th>P.Adol.NR</th><th>P.Adol.Exc</th>
                  </tr></thead>
                  <tbody>
                    <xsl:for-each select="TarifasRestricciones/UpdatePlanFaresNR">
                      <tr>
                        <td><xsl:value-of select="Descr_rateplan"/></td>
                        <td class="r"><xsl:value-of select="Adultos"/></td>
                        <td class="r"><xsl:value-of select="format-number(TarifaAdulto,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(TarifaAdultoNR,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(TarifaAdultoExc,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="Ninios"/></td>
                        <td class="r"><xsl:value-of select="format-number(TarifaNinio,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(TarifaNinioNR,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(TarifaNinioExc,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(TarifaAdolescente,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(TarifaAdolescenteNR,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(TarifaAdolescenteExc,'#####0.00','pesos')"/></td>
                      </tr>
                    </xsl:for-each>
                  </tbody>
                </table>
              </xsl:if>

              <xsl:if test="TarifasRestricciones/UpdateRateRestrictionByDay">
                <table class="ld-tbl ld-tbl-rules" id="tr-b-d">
                  <thead><tr>
                    <th>Plan tarifario</th><th>Adultos</th><th>P.Adulto</th><th>Niños</th><th>P.Niño</th>
                  </tr></thead>
                  <tbody>
                    <xsl:for-each select="TarifasRestricciones/UpdateRateRestrictionByDay">
                      <tr>
                        <td><xsl:value-of select="Descr_rateplan"/></td>
                        <td class="r"><xsl:value-of select="Adultos"/></td>
                        <td class="r"><xsl:value-of select="format-number(TarifaAdulto,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="Ninios"/></td>
                        <td class="r"><xsl:value-of select="format-number(TarifaNinio,'#####0.00','pesos')"/></td>
                      </tr>
                    </xsl:for-each>
                  </tbody>
                </table>
              </xsl:if>
            </div>
          </xsl:if>

          <xsl:if test="(TarifasRestricciones/UpdateRateRestriction or TarifasRestricciones/UpdatePlanFaresNR or TarifasRestricciones/UpdateRateRestrictionByDay) and (TarifasRestricciones/_UpdateRateRestriction or TarifasRestricciones/_UpdatePlanFaresNR or TarifasRestricciones/_UpdateRateRestrictionByDay)">
            <div class="ld-sep">
              <div class="ld-sep-line"></div>
              <div class="ld-sep-txt">&#9660; ACTUALIZADO A</div>
              <div class="ld-sep-line"></div>
            </div>
          </xsl:if>

          <!-- ===== DESPUÉS ===== -->
          <xsl:if test="TarifasRestricciones/_UpdateRateRestriction or TarifasRestricciones/_UpdatePlanFaresNR or TarifasRestricciones/_UpdateRateRestrictionByDay">
            <div class="ld-card">
              <div class="ld-head after">
                <span class="ld-badge after">&#9654; DESPUÉS</span>
                <xsl:if test="TarifasRestricciones/_UpdateRateRestriction">Tarifas por ocupación</xsl:if>
                <xsl:if test="TarifasRestricciones/_UpdatePlanFaresNR"> &#183; Tarifas NR</xsl:if>
                <xsl:if test="TarifasRestricciones/_UpdateRateRestrictionByDay"> &#183; Tarifas por día</xsl:if>
              </div>

              <xsl:if test="TarifasRestricciones/_UpdateRateRestriction">
                <table class="ld-tbl" id="tr-a-r">
                  <thead><tr>
                    <th>Plan tarifario</th><th>Adultos</th><th>P.Adulto Extra</th>
                    <th>Niños</th><th>P.Niño</th><th>P.Adolescente</th>
                  </tr></thead>
                  <tbody>
                    <xsl:for-each select="TarifasRestricciones/_UpdateRateRestriction">
                      <tr>
                        <td><xsl:value-of select="Descr_rateplan"/></td>
                        <td class="r"><xsl:value-of select="Adultos"/></td>
                        <td class="r"><xsl:value-of select="format-number(TarifaAdulto,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="Ninios"/></td>
                        <td class="r"><xsl:value-of select="format-number(TarifaNinio,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(TarifaAdolescente,'#####0.00','pesos')"/></td>
                      </tr>
                    </xsl:for-each>
                  </tbody>
                </table>
              </xsl:if>

              <xsl:if test="TarifasRestricciones/_UpdatePlanFaresNR">
                <table class="ld-tbl ld-tbl-rules" id="tr-a-nr">
                  <thead><tr>
                    <th>Plan tarifario</th><th>Adultos</th><th>P.Adulto</th><th>P.Adulto NR</th><th>P.Adulto Exc</th>
                    <th>Niños</th><th>P.Niño</th><th>P.Niño NR</th><th>P.Niño Exc</th>
                    <th>P.Adol.</th><th>P.Adol.NR</th><th>P.Adol.Exc</th>
                  </tr></thead>
                  <tbody>
                    <xsl:for-each select="TarifasRestricciones/_UpdatePlanFaresNR">
                      <tr>
                        <td><xsl:value-of select="Descr_rateplan"/></td>
                        <td class="r"><xsl:value-of select="Adultos"/></td>
                        <td class="r"><xsl:value-of select="format-number(TarifaAdulto,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(TarifaAdultoNR,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(TarifaAdultoExc,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="Ninios"/></td>
                        <td class="r"><xsl:value-of select="format-number(TarifaNinio,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(TarifaNinioNR,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(TarifaNinioExc,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(TarifaAdolescente,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(TarifaAdolescenteNR,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="format-number(TarifaAdolescenteExc,'#####0.00','pesos')"/></td>
                      </tr>
                    </xsl:for-each>
                  </tbody>
                </table>
              </xsl:if>

              <xsl:if test="TarifasRestricciones/_UpdateRateRestrictionByDay">
                <table class="ld-tbl ld-tbl-rules" id="tr-a-d">
                  <thead><tr>
                    <th>Plan tarifario</th><th>Adultos</th><th>P.Adulto</th><th>Niños</th><th>P.Niño</th>
                  </tr></thead>
                  <tbody>
                    <xsl:for-each select="TarifasRestricciones/_UpdateRateRestrictionByDay">
                      <tr>
                        <td><xsl:value-of select="Descr_rateplan"/></td>
                        <td class="r"><xsl:value-of select="Adultos"/></td>
                        <td class="r"><xsl:value-of select="format-number(TarifaAdulto,'#####0.00','pesos')"/></td>
                        <td class="r"><xsl:value-of select="Ninios"/></td>
                        <td class="r"><xsl:value-of select="format-number(TarifaNinio,'#####0.00','pesos')"/></td>
                      </tr>
                    </xsl:for-each>
                  </tbody>
                </table>
              </xsl:if>
            </div>
          </xsl:if>

          <div id="ld-legend" class="ld-legend">
            <span><span class="ld-dot" style="background:#fee2e2;border:1px solid #fca5a5;"></span> Valor anterior (tachado)</span>
            <span><span class="ld-dot" style="background:#f0fdf4;border:1px solid #4ade80;"></span> Valor actualizado</span>
          </div>

        </div>
      </body>
    </html>
  </xsl:template>

</xsl:stylesheet>
