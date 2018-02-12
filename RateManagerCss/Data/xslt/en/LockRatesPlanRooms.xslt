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
        <xsl:for-each select="LockRatesPlanRooms/UpdateLockRates">
          <xsl:if test="position() = 1">
            <span>Room availability by rate plan</span>
          </xsl:if>
        </xsl:for-each >
        <xsl:for-each select="LockRatesPlanRooms/UpdateRoomSegment">
          <xsl:if test="position() = 1">
            <span>Room availability by segment rate plan</span>
          </xsl:if>
        </xsl:for-each >
        <table border="1" cellpadding="0" cellspacing="0"  >
          <xsl:for-each select="LockRatesPlanRooms/UpdateLockRates">
            <xsl:if test="position() = 1">
              <xsl:call-template name="Header">
              </xsl:call-template>
            </xsl:if>
            <tr>
              <td>
                <xsl:value-of select="Descr_room"/>
                <br/>
                <xsl:value-of select="Descr_rateplans"/>
              </td>
              <td>
                <xsl:call-template name="FormatDate">
                  <xsl:with-param name="DateTime" select="StartDate"/>
                </xsl:call-template>
              </td>
              <td>
                <xsl:call-template name="FormatDate">
                  <xsl:with-param name="DateTime" select="EndDate"/>
                </xsl:call-template>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="RoomsAvailable"/>
              </td>
            </tr>
          </xsl:for-each>
          <xsl:for-each select="LockRatesPlanRooms/UpdateRoomSegment">
            <xsl:if test="position() = 1">
              <xsl:call-template name="HeaderSeg">
              </xsl:call-template>
            </xsl:if>
            <tr>
              <td>
                <xsl:value-of select="Descr_room"/>
                <br/>
                <xsl:value-of select="Descr_rateplans"/>
              </td>
              <td>
                <xsl:call-template name="FormatDate">
                  <xsl:with-param name="DateTime" select="StartDate"/>
                </xsl:call-template>
              </td>
              <td>
                <xsl:call-template name="FormatDate">
                  <xsl:with-param name="DateTime" select="EndDate"/>
                </xsl:call-template>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="RoomsAvailable"/>
              </td>
            </tr>
          </xsl:for-each>
        </table>

        <br/>
        <xsl:for-each select="LockRatesPlanRooms/_UpdateLockRates">
          <xsl:if test="position() = 1">
            <span>Room availability by rate plan, Update</span>
          </xsl:if>
        </xsl:for-each >
        <xsl:for-each select="LockRatesPlanRooms/_UpdateRoomSegment">
          <xsl:if test="position() = 1">
            <span>Room availability by segment rate plan, Update</span>
          </xsl:if>
        </xsl:for-each >
        <table border="1" cellpadding="0" cellspacing="0"  >
          <xsl:for-each select="LockRatesPlanRooms/_UpdateLockRates">
            <xsl:if test="position() = 1">
              <xsl:call-template name="Header">
              </xsl:call-template>
            </xsl:if>
            <tr>
              <td>
                <xsl:value-of select="Descr_room"/>
                <br/>
                <xsl:value-of select="Descr_rateplans"/>
              </td>
              <td>
                <xsl:call-template name="FormatDate">
                  <xsl:with-param name="DateTime" select="StartDate"/>
                </xsl:call-template>
              </td>
              <td>
                <xsl:call-template name="FormatDate">
                  <xsl:with-param name="DateTime" select="EndDate"/>
                </xsl:call-template>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="RoomsAvailable"/>
              </td>

            </tr>
          </xsl:for-each>
          <xsl:for-each select="LockRatesPlanRooms/_UpdateRoomSegment">
            <xsl:if test="position() = 1">
              <xsl:call-template name="HeaderSeg">
              </xsl:call-template>
            </xsl:if>
            <tr>
              <td>
                <xsl:value-of select="Descr_room"/>
                <br/>
                <xsl:value-of select="Descr_rateplans"/>
              </td>
              <td>
                <xsl:call-template name="FormatDate">
                  <xsl:with-param name="DateTime" select="StartDate"/>
                </xsl:call-template>
              </td>
              <td>
                <xsl:call-template name="FormatDate">
                  <xsl:with-param name="DateTime" select="EndDate"/>
                </xsl:call-template>
              </td>
              <td style="text-align:right;">
                <xsl:value-of select="RoomsAvailable"/>
              </td>
            </tr>
          </xsl:for-each>
        </table>
      </body>
    </html>
  </xsl:template>

  <xsl:template name="Header">
    <tr bgcolor="#668800">
      <td>Room Type</td>
      <td>Start Date</td>
      <td>End Date</td>
      <td>Rooms</td>
    </tr>
  </xsl:template>

  <xsl:template name="HeaderSeg">
    <tr bgcolor="#668800">
      <td>Room Type</td>
      <td>Start Date</td>
      <td>End Date</td>
      <td>Rooms</td>
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

