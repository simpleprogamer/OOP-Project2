<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="html" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/dormitory">
		<html>
			<head>
				<title>Гуртожиток</title>
				<style>
					body { font-family: Arial, sans-serif; margin: 16px; }
					h1 { font-size: 20px; }
					table { border-collapse: collapse; width: 100%; }
					th, td { border: 1px solid #999; padding: 6px 8px; text-align: left; vertical-align: top; font-size: 14px; }
					th { background: #e8eef7; }
					tr:nth-child(even) td { background: #f7f7f7; }
				</style>
			</head>
			<body>
				<h1>Список мешканців гуртожитку</h1>
				<table>
					<thead>
						<tr>
							<th>№</th>
							<th>П.І.П.</th>
							<th>Факультет</th>
							<th>Кафедра</th>
							<th>Курс</th>
							<th>Місце проживання (з датами)</th>
						</tr>
					</thead>
					<tbody>
						<xsl:for-each select="student">
							<xsl:sort select="@fullName"/>
							<tr>
								<td>
									<xsl:value-of select="position()"/>
								</td>
								<td>
									<xsl:value-of select="@fullName"/>
								</td>
								<td>
									<xsl:value-of select="@faculty"/>
								</td>
								<td>
									<xsl:value-of select="@department"/>
								</td>
								<td>
									<xsl:value-of select="@course"/>
								</td>
								<td>
									<xsl:for-each select="residence">
										<div>
											<xsl:value-of select="@dormitory"/>, кімн. <xsl:value-of select="@room"/>
											(<xsl:value-of select="@dateFrom"/> — <xsl:value-of select="@dateTo"/>)
										</div>
									</xsl:for-each>
								</td>
							</tr>
						</xsl:for-each>
					</tbody>
				</table>
			</body>
		</html>
	</xsl:template>
</xsl:stylesheet>