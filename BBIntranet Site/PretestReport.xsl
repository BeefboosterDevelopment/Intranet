<?xml version="1.0" encoding="utf-8"?>

<!DOCTYPE xsl:stylesheet [
  <!ENTITY nl "&#10;">
  <!ENTITY nbsp "&#160;">
  <!ENTITY amp "&#38;#38;">
]>

<xsl:stylesheet version="1.0"  xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:fo="http://www.w3.org/1999/XSL/Format">

  <xsl:template match="/">
    <xsl:apply-templates/>
  </xsl:template>

      
      
  <xsl:template match="PreTestStrainData">
    <br />
    <h3 style="font-family:Times New Roman;color:#933;">
      <xsl:value-of select='@ReportYear' />&nbsp;Pre-Test Report
    </h3>

    <table width="100%" border="0" cellpadding="1" >
    <tr>
      <th width="30%" bgcolor="#993333" scope="col">
        <div align="left">
          <span class="style5">Strain</span>
        </div>
      </th>
      <th width="14%" bgcolor="#993333" scope="col">
        <div align="left">
          <span class="style5">M1</span>
        </div>
      </th>
      <th width="14%" bgcolor="#993333" scope="col">
        <div align="left">
          <span class="style5">M2</span>
        </div>
      </th>
      <th width="14%" bgcolor="#993333" scope="col">
        <div align="left">
          <span class="style5">M3</span>
        </div>
      </th>
      <th width="14%" bgcolor="#993333" scope="col">
        <div align="left">
          <span class="style5">M4</span>
        </div>
      </th>
      <th width="14%" bgcolor="#993333" scope="col">
        <div align="left">
          <span class="style5">TX</span>
        </div>
      </th>
    </tr>

      
      
    <tr>
      <td bgcolor="#F5EAEA">
        <table cellspacing="0" cellpadding="0">
          <tr>
            <td width="141" height="17" class="style6">Number On Test</td>
          </tr>
        </table>
      </td>
      <td bgcolor="#F5EAEA">
        <xsl:value-of select='//STRAIN[@CODE="M1"]/@NUMONTEST' />
      </td>
      <td bgcolor="#F5EAEA">
        <xsl:value-of select='//STRAIN[@CODE="M2"]/@NUMONTEST' />
      </td>
      <td bgcolor="#F5EAEA">
        <xsl:value-of select='//STRAIN[@CODE="M3"]/@NUMONTEST' />
      </td>
      <td bgcolor="#F5EAEA">
        <xsl:value-of select='//STRAIN[@CODE="M4"]/@NUMONTEST' />
      </td>
      <td bgcolor="#F5EAEA">
        <xsl:value-of select='//STRAIN[@CODE="TX"]/@NUMONTEST' />
      </td>
    </tr>
    <tr>
      <td bgcolor="#F5EAEA">
        <table cellspacing="0" cellpadding="0">
          <tr>
            <td width="141" height="17" class="style6">Average Birth Date</td>
          </tr>
        </table>
      </td>
      <td bgcolor="#F5EAEA">
        <xsl:value-of select='//STRAIN[@CODE="M1"]/@BDATE' />
      </td>
      <td bgcolor="#F5EAEA">
        <xsl:value-of select='//STRAIN[@CODE="M2"]/@BDATE' />
      </td>
      <td bgcolor="#F5EAEA">
        <xsl:value-of select='//STRAIN[@CODE="M3"]/@BDATE' />
      </td>
      <td bgcolor="#F5EAEA">
        <xsl:value-of select='//STRAIN[@CODE="M4"]/@BDATE' />
      </td>
      <td bgcolor="#F5EAEA">
        <xsl:value-of select='//STRAIN[@CODE="TX"]/@BDATE' />
      </td>
    </tr>
    <tr>
      <td bgcolor="#F5EAEA">
        <table cellspacing="0" cellpadding="0">
          <tr>
            <td width="141" height="17" class="style6">Average Birth Weight</td>
          </tr>
        </table>
      </td>
      <td bgcolor="#F5EAEA">
        <xsl:value-of select='//STRAIN[@CODE="M1"]/@BWT' />
      </td>
      <td bgcolor="#F5EAEA">
        <xsl:value-of select='//STRAIN[@CODE="M2"]/@BWT' />
      </td>
      <td bgcolor="#F5EAEA">
        <xsl:value-of select='//STRAIN[@CODE="M3"]/@BWT' />
      </td>
      <td bgcolor="#F5EAEA">
        <xsl:value-of select='//STRAIN[@CODE="M4"]/@BWT' />
      </td>
      <td bgcolor="#F5EAEA">
        <xsl:value-of select='//STRAIN[@CODE="TX"]/@BWT' />
      </td>
    </tr>
    <tr>
      <td bgcolor="#F5EAEA">
        <table cellspacing="0" cellpadding="0">
          <tr>
            <td width="141" height="17" class="style6">Average Wean Age (days)</td>
          </tr>
        </table>
      </td>
      <td bgcolor="#F5EAEA">
        <xsl:value-of select='//STRAIN[@CODE="M1"]/@WEANAGE' />
      </td>
      <td bgcolor="#F5EAEA">
        <xsl:value-of select='//STRAIN[@CODE="M2"]/@WEANAGE' />
      </td>
      <td bgcolor="#F5EAEA">
        <xsl:value-of select='//STRAIN[@CODE="M3"]/@WEANAGE' />
      </td>
      <td bgcolor="#F5EAEA">
        <xsl:value-of select='//STRAIN[@CODE="M4"]/@WEANAGE' />
      </td>
      <td bgcolor="#F5EAEA">
        <xsl:value-of select='//STRAIN[@CODE="TX"]/@WEANAGE' />
      </td>
    </tr>
    <tr>
      <td bgcolor="#F5EAEA">
        <table cellspacing="0" cellpadding="0">
          <tr>
            <td width="141" height="17" class="style6">180 Day Weight</td>
          </tr>
        </table>
      </td>
      <td bgcolor="#F5EAEA">
        <xsl:value-of select='//STRAIN[@CODE="M1"]/@WWT' />
      </td>
      <td bgcolor="#F5EAEA">
        <xsl:value-of select='//STRAIN[@CODE="M2"]/@WWT' />
      </td>
      <td bgcolor="#F5EAEA">
        <xsl:value-of select='//STRAIN[@CODE="M3"]/@WWT' />
      </td>
      <td bgcolor="#F5EAEA">
        <xsl:value-of select='//STRAIN[@CODE="M4"]/@WWT' />
      </td>
      <td bgcolor="#F5EAEA">
        <xsl:value-of select='//STRAIN[@CODE="TX"]/@WWT' />
      </td>
    </tr>
    <tr>
      <td bgcolor="#F5EAEA">
        <table cellspacing="0" cellpadding="0">
          <tr>
            <td width="141" height="17" class="style6">ADG Birth To Weaning</td>
          </tr>
        </table>
      </td>
      <td bgcolor="#F5EAEA">
        <xsl:value-of select='//STRAIN[@CODE="M1"]/@ADGBW' />
      </td>
      <td bgcolor="#F5EAEA">
        <xsl:value-of select='//STRAIN[@CODE="M2"]/@ADGBW' />
      </td>
      <td bgcolor="#F5EAEA">
        <xsl:value-of select='//STRAIN[@CODE="M3"]/@ADGBW' />
      </td>
      <td bgcolor="#F5EAEA">
        <xsl:value-of select='//STRAIN[@CODE="M4"]/@ADGBW' />
      </td>
      <td bgcolor="#F5EAEA">
        <xsl:value-of select='//STRAIN[@CODE="TX"]/@ADGBW' />
      </td>
    </tr>
    <tr>
      <td bgcolor="#F5EAEA">
        <table cellspacing="0" cellpadding="0">
          <tr>
            <td width="141" height="17" class="style6">Wean WPDA</td>
          </tr>
        </table>
      </td>
      <td bgcolor="#F5EAEA">
        <xsl:value-of select='//STRAIN[@CODE="M1"]/@WWPDA' />
      </td>
      <td bgcolor="#F5EAEA">
        <xsl:value-of select='//STRAIN[@CODE="M2"]/@WWPDA' />
      </td>
      <td bgcolor="#F5EAEA">
        <xsl:value-of select='//STRAIN[@CODE="M3"]/@WWPDA' />
      </td>
      <td bgcolor="#F5EAEA">
        <xsl:value-of select='//STRAIN[@CODE="M4"]/@WWPDA' />
      </td>
      <td bgcolor="#F5EAEA">
        <xsl:value-of select='//STRAIN[@CODE="TX"]/@WWPDA' />
      </td>
    </tr>
    <tr>
      <td bgcolor="#F5EAEA">
        <table cellspacing="0" cellpadding="0">
          <tr>
            <td width="141" height="17" class="style6">On Test Weight</td>
          </tr>
        </table>
      </td>
      <td bgcolor="#F5EAEA">
        <xsl:value-of select='//STRAIN[@CODE="M1"]/@ONTESTWT' />
      </td>
      <td bgcolor="#F5EAEA">
        <xsl:value-of select='//STRAIN[@CODE="M2"]/@ONTESTWT' />
      </td>
      <td bgcolor="#F5EAEA">
        <xsl:value-of select='//STRAIN[@CODE="M3"]/@ONTESTWT' />
      </td>
      <td bgcolor="#F5EAEA">
        <xsl:value-of select='//STRAIN[@CODE="M4"]/@ONTESTWT' />
      </td>
      <td bgcolor="#F5EAEA">
        <xsl:value-of select='//STRAIN[@CODE="TX"]/@ONTESTWT' />
      </td>
    </tr>
      <tr>
        <td colspan="2"  style="font-family:Times New Roman;font-size:.8em;" align="left">
          (weight in pounds)
        </td>
        <td colspan="4"  align="right" style="font-family:Times New Roman;font-size:.7em;color:#9999ff">
			<xsl:choose>
				<xsl:when test="@ERROR!=''">
					<SPAN  style="font-size:1em;color:red;">
						<xsl:value-of select="@ERROR" />
					</SPAN>
				</xsl:when>
				<xsl:otherwise>
					Generated on:&nbsp;<xsl:value-of select='@GeneratedOn' />
				</xsl:otherwise>
			</xsl:choose>

        </td>
      </tr>
  </table>
  </xsl:template>

</xsl:stylesheet> 

