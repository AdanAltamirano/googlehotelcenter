<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" encoding="UTF-8"/>


	<xsl:template match="/">
		<xsl:element name="MoneyExchangeRS">
			<xsl:element name="Response">
			<xsl:choose >
						<xsl:when test="//Type='C'">
							<xsl:element name="ResponseType">
								<xsl:text >0</xsl:text>
							</xsl:element>
							<xsl:element name="CurrencyResponse">
								<xsl:for-each select="//PairStatus">
									<xsl:element name="CurrencyArray">
										<xsl:element name="Status">
											<xsl:value-of select="PairStatusInd"/>
										</xsl:element>
										<xsl:element name="exMessage">
											<xsl:choose >
												<xsl:when test="PairStatusInd=0">
													<xsl:text >Successful execution</xsl:text>
												</xsl:when>
												<xsl:when test="PairStatusInd=1">
													<xsl:text >Base currency not found</xsl:text>
												</xsl:when>
												<xsl:when test="PairStatusInd=2">
													<xsl:text >Base currency rate not in use</xsl:text>
												</xsl:when>
												<xsl:when test="PairStatusInd=3">
													<xsl:text >Second currency not found</xsl:text>
												</xsl:when>
												<xsl:when test="PairStatusInd=4">
													<xsl:text >Second currency rate not in use</xsl:text>
												</xsl:when>
												
											</xsl:choose>
										</xsl:element>
										
										<xsl:element name="CurrencySource">
											<xsl:value-of select="Currency1"/>
										</xsl:element>
										<xsl:element name="CurrencyTarget">
											<xsl:value-of select="Currency2"/>
										</xsl:element>
										<xsl:element name="DecPosSource">
											<xsl:value-of select="DecPos1"/>
										</xsl:element>
										<xsl:element name="DecPosTarget">
											<xsl:value-of select="DecPos2"/>
										</xsl:element>
										<xsl:element name="FlagCurrency">
											<xsl:value-of select="Calc1TypeInd"/>
										</xsl:element>
										<xsl:element name="BaseSource">
											<xsl:call-template name="Convertirdecimal">
												<xsl:with-param name="cadena">
													<xsl:value-of select="number(ConvInd1)"/>
												</xsl:with-param>
												<xsl:with-param name="decimal">
													<xsl:value-of select="DecPos1"/>
											</xsl:with-param>
											</xsl:call-template>
										</xsl:element>
										<xsl:element name="BaseTarget">
											<xsl:call-template name="Convertirdecimal">
												<xsl:with-param name="cadena">
													<xsl:value-of select="number(ConvInd2)"/>
												</xsl:with-param>
												<xsl:with-param name="decimal">
													<xsl:value-of select="DecPos2"/>
											</xsl:with-param>
											</xsl:call-template>
										</xsl:element>
										<xsl:element name="CurrencyAmt">
											<xsl:value-of select="ConvInd1"/>
										</xsl:element>
									</xsl:element>
								</xsl:for-each>
							</xsl:element>
						</xsl:when>
						<xsl:otherwise >
							<xsl:element name="Error">
								<xsl:element name="ErrorNumber">
									<xsl:value-of select="//ErrNum"/>
								</xsl:element>	
								<xsl:element name="ErrorDescription">
									<xsl:value-of select="//ErrMsg"/>
								</xsl:element>
								<xsl:element name="exMessage">
								<xsl:value-of select="//Type"/>
								</xsl:element>
							</xsl:element>
						</xsl:otherwise>
			</xsl:choose>
				
				
			</xsl:element>
		</xsl:element>
	</xsl:template>

	<xsl:template name="Convertirdecimal">
		<xsl:param name="cadena"/>
		<xsl:param name="decimal">2</xsl:param>
		<xsl:variable name="longitud">
			<xsl:value-of select="string-length($cadena)"/>
		</xsl:variable>
		<xsl:variable name="cpri">
			<xsl:value-of select="substring($cadena,1,number($longitud -$decimal))"/>
		</xsl:variable>
		<xsl:variable name="csec">
			<xsl:value-of select="substring-after($cadena,$cpri)"/>
		</xsl:variable>
		<xsl:value-of select="$cpri"/>
		<xsl:text>.</xsl:text>
		<xsl:value-of select="$csec"/>
	</xsl:template>
</xsl:stylesheet><!-- Stylus Studio meta-information - (c) 2004-2006. Progress Software Corporation. All rights reserved.
<metaInformation>
<scenarios/><MapperMetaTag><MapperInfo srcSchemaPathIsRelative="yes" srcSchemaInterpretAsXML="no" destSchemaPath="" destSchemaRoot="" destSchemaPathIsRelative="yes" destSchemaInterpretAsXML="no"/><MapperBlockPosition></MapperBlockPosition><TemplateContext></TemplateContext><MapperFilter side="source"></MapperFilter></MapperMetaTag>
</metaInformation>
-->