<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:decimal-format name="pesos" grouping-separator=","/>

  <xsl:template match="/">
    <html>
      <style type="text/css">
        .logdiff{font-family:Arial,Helvetica,sans-serif;font-size:12px;color:#2d3748;padding:4px;}
        .ld-card{border-radius:6px;margin-bottom:12px;overflow:hidden;box-shadow:0 1px 4px rgba(0,0,0,.15);border:1px solid #e2e8f0;}
        .ld-head{padding:8px 12px;font-weight:bold;font-size:12px;display:flex;align-items:center;gap:8px;}
        .ld-head.before{background:#fffbeb;border-left:4px solid #d97706;color:#92400e;}
        .ld-head.after{background:#ecfdf5;border-left:4px solid #059669;color:#065f46;}
        .ld-badge{display:inline-block;padding:2px 8px;border-radius:10px;font-size:10px;font-weight:bold;letter-spacing:.5px;}
        .ld-badge.before{background:#d97706;color:#fff;}
        .ld-badge.after{background:#059669;color:#fff;}
        .ld-tbl{width:100%;border-collapse:collapse;font-size:11px;}
        .ld-tbl th{background:#334155;color:#f1f5f9;padding:5px 8px;text-align:left;font-weight:600;white-space:nowrap;}
        .ld-tbl td{padding:4px 8px;white-space:nowrap;border-bottom:1px solid #e2e8f0;}
        .ld-tbl tbody tr:nth-child(even) td{background:#f8fafc;}
        .ld-tbl tbody tr:nth-child(odd) td{background:#ffffff;}
        .ld-tbl .r{text-align:right;}
        .ld-tbl-rules thead th{background:#475569;}
        .logdiff td.ch-b{background:#fee2e2 !important;color:#b91c1c !important;font-weight:bold;}
        .logdiff td.ch-a{background:#d1fae5 !important;color:#047857 !important;font-weight:bold;}
        .ld-legend{font-size:10px;padding:4px 12px 6px;color:#64748b;display:flex;gap:12px;}
        .ld-legend span{display:inline-flex;align-items:center;gap:4px;}
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
  var changed=false;
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
          changed=true;
        }
      }
    }
  }
  var leg=document.getElementById('tr-legend');
  if(leg){leg.style.display=changed?'flex':'none';}
}
document.readyState!=='loading'?ldDiff():document.addEventListener('DOMContentLoaded',ldDiff);
        </xsl:text>
      </script>
      <body>
        <div class="logdiff">

          <!-- ===== BEFORE ===== -->
          <xsl:if test="TarifasRestricciones/UpdateRateRestriction or TarifasRestricciones/UpdatePlanFaresNR or TarifasRestricciones/UpdateRateRestrictionByDay">
            <div class="ld-card">
              <div class="ld-head before">
                <span class="ld-badge before">BEFORE</span>
                <xsl:if test="TarifasRestricciones/UpdateRateRestriction">Rates by Occupancy</xsl:if>
                <xsl:if test="TarifasRestricciones/UpdatePlanFaresNR"> &#183; Net Rates</xsl:if>
                <xsl:if test="TarifasRestricciones/UpdateRateRestrictionByDay"> &#183; Rates by Day</xsl:if>
              </div>

              <xsl:if test="TarifasRestricciones/UpdateRateRestriction">
                <table class="ld-tbl" id="tr-b-r">
                  <thead>
                    <tr>
                      <th>Rate Plan</th>
                      <th>Adults</th>
                      <th>Adult Extra P.</th>
                      <th>Children</th>
                      <th>Child P.</th>
                      <th>Teen P.</th>
                    </tr>
                  </thead>
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
                  <thead>
                    <tr>
                      <th>Rate Plan</th>
                      <th>Adults</th>
                      <th>Adult P.</th>
                      <th>Adult NR</th>
                      <th>Adult Exc</th>
                      <th>Children</th>
                      <th>Child P.</th>
                      <th>Child NR</th>
                      <th>Child Exc</th>
                      <th>Teen P.</th>
                      <th>Teen NR</th>
                      <th>Teen Exc</th>
                    </tr>
                  </thead>
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
                  <thead>
                    <tr>
                      <th>Rate Plan</th>
                      <th>Adults</th>
                      <th>Adult Price</th>
                      <th>Children</th>
                      <th>Child Price</th>
                    </tr>
                  </thead>
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

          <!-- ===== AFTER ===== -->
          <xsl:if test="TarifasRestricciones/_UpdateRateRestriction or TarifasRestricciones/_UpdatePlanFaresNR or TarifasRestricciones/_UpdateRateRestrictionByDay">
            <div class="ld-card">
              <div class="ld-head after">
                <span class="ld-badge after">AFTER</span>
                <xsl:if test="TarifasRestricciones/_UpdateRateRestriction">Rates by Occupancy</xsl:if>
                <xsl:if test="TarifasRestricciones/_UpdatePlanFaresNR"> &#183; Net Rates</xsl:if>
                <xsl:if test="TarifasRestricciones/_UpdateRateRestrictionByDay"> &#183; Rates by Day</xsl:if>
              </div>

              <xsl:if test="TarifasRestricciones/_UpdateRateRestriction">
                <table class="ld-tbl" id="tr-a-r">
                  <thead>
                    <tr>
                      <th>Rate Plan</th>
                      <th>Adults</th>
                      <th>Adult Extra P.</th>
                      <th>Children</th>
                      <th>Child P.</th>
                      <th>Teen P.</th>
                    </tr>
                  </thead>
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
                  <thead>
                    <tr>
                      <th>Rate Plan</th>
                      <th>Adults</th>
                      <th>Adult P.</th>
                      <th>Adult NR</th>
                      <th>Adult Exc</th>
                      <th>Children</th>
                      <th>Child P.</th>
                      <th>Child NR</th>
                      <th>Child Exc</th>
                      <th>Teen P.</th>
                      <th>Teen NR</th>
                      <th>Teen Exc</th>
                    </tr>
                  </thead>
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
                  <thead>
                    <tr>
                      <th>Rate Plan</th>
                      <th>Adults</th>
                      <th>Adult Price</th>
                      <th>Children</th>
                      <th>Child Price</th>
                    </tr>
                  </thead>
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

          <!-- Diff legend -->
          <div id="tr-legend" class="ld-legend" style="display:none;">
            <span><span class="ld-dot" style="background:#fee2e2;border:1px solid #fca5a5;"></span> Previous value</span>
            <span><span class="ld-dot" style="background:#d1fae5;border:1px solid #6ee7b7;"></span> Updated value</span>
          </div>

        </div>
      </body>
    </html>
  </xsl:template>

</xsl:stylesheet>
