
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" encoding="UTF-8"/>


	<xsl:template match="/">
		<xsl:element name="CurrencyConversion_6_0">
			<xsl:element name="CurrencyConversionMods">
				<xsl:element name="CurrencyAry">
					<xsl:for-each select="//Currency">
						<xsl:element name="Currency">
							<xsl:element name="Currency1">
								<xsl:value-of select="CurrencySource"/>
							</xsl:element>
							<xsl:element name="Currency2">
								<xsl:value-of select="CurrencyTarget"/>
							</xsl:element>
						</xsl:element>
					</xsl:for-each>
				</xsl:element>
			</xsl:element>
		</xsl:element>
	</xsl:template>
</xsl:stylesheet><!-- Stylus Studio meta-information - (c) 2004-2006. Progress Software Corporation. All rights reserved.
<metaInformation>
<scenarios/><MapperMetaTag><MapperInfo srcSchemaPathIsRelative="yes" srcSchemaInterpretAsXML="no" destSchemaPath="" destSchemaRoot="" destSchemaPathIsRelative="yes" destSchemaInterpretAsXML="no"/><MapperBlockPosition></MapperBlockPosition><TemplateContext></TemplateContext><MapperFilter side="source"></MapperFilter></MapperMetaTag>
</metaInformation>
-->