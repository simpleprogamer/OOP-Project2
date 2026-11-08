<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="html" encoding="UTF-8" indent="yes"/>

	<xsl:variable name ="upper" select ="'ABCDEFGHIJKLMNOPQRSTUVWXYZАБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЬЮЯ'"/>
	<xsl:variable name ="lower" select ="'abcdefghijklmnopqrstuvwxyzабвгґдеєжзиіїйклмнопрстуфхцчшщьюя'"/>
	<xsl:variable name ="base" select ="' fullName faculty department course'"/>
	<xsl:template match="/report">
		<html>
			<head>
				<title>Результати пошуку</title>
				<style>
					body { font-family: Arial, sans-serif; margin: 16px; }
					h1 { font-size: 20px; }
					.query {background # f3f6fb; border: 1px solid #c9d6ea; padding: 8px 12px; margin-bottom: 14px; font-size: 14px;}
					.query p ( margin: 4px 0;}
					code {background:  #fff; padding: 1px 4px; border: 1px solid #ddd; word-break: break-all; }
					.empty {color: #a33; font-weight: bold;}
					table { border-collapse: collapse; width: 100%; }
					th, td { border: 1px solid #999; padding: 6px 8px; text-align: left; vertical-align: top; font-size: 14px; }
					th { background: #e8eef7; }
					th.searched { background: #ffe9a8; }
					tr:nth-child(even) td { background: #f7f7f7; }
					mark { background: #ffd54a; padding: 0 2px; border-radius: 2px; }
				</style>
			</head>
			<body>
				<h1>Результати пошуку в списку мешканців гуртожитку</h1>

				<div class ="query">
					<p>
						<b>	Метод аналізу: </b>
						<xsl:value-of select="query/@method"/>
					</p>
					<p>
						<b>	Атрибути пошуку: </b>
						<xsl:for-each select ="query/attribute">
							<xsl:value-of select ="@name"/>
							<xsl:if test ="position() != last()">, </xsl:if>
						</xsl:for-each>
					</p>
					<p>
						<b>	Ключові слова:	</b>
						<xsl:for-each select ="query/keyword">
							<mark>
								<xsl:value-of select="."/>
							</mark>
							<xsl:if test="position() != last()">
								, <xsl:text> </xsl:text>
							</xsl:if>
						</xsl:for-each>
					</p>
					<p>
						<b> Запит:</b>
						<code>
							<xsl:value-of select="@query/@xpath"/>
						</code>
					</p>
					<p>
						<b></b>
						<xsl:value-of select="count(students/student)"/>
					</p>
				</div>
				<xsl:choose>
					<xsl:when test="count(students/student) = 0">
						<p class="empty"> За вказаними умовами нічого не знайдено.</p>
					</xsl:when>
					<xsl:otherwise>
						<table>
							<thead>
								<tr>
									<th>№</th>
									<xsl:call-template name="head">
										<xsl:with-param name ="name" select="'fullName'"/>
										<xsl:with-param name ="label" select="'П.І.П'"/>
									</xsl:call-template>
									<xsl:call-template name="head">
										<xsl:with-param name ="name" select="'faculty'"/>
										<xsl:with-param name ="label" select="'Факультет'"/>
									</xsl:call-template>
									<xsl:call-template name="head">
										<xsl:with-param name ="name" select="'department'"/>
										<xsl:with-param name ="label" select="'Кафедра'"/>
									</xsl:call-template>
									<xsl:call-template name="head">
										<xsl:with-param name ="name" select="'course'"/>
										<xsl:with-param name ="label" select="'Курс'"/>
									</xsl:call-template>
									<xsl:for-each select="query/attribute[not(contains($base, concat('',@name, '')))]">
										<th class ="searched">
											<xsl:value-of select="@name"/>
										</th>
									</xsl:for-each>
									<th>Місце проживання (з датами)</th>
								</tr>
							</thead>
							<tbody>
								<xsl:for-each select ="students/student">
									<xsl:variable name="st" select ="."/>
									<tr>
										<td>
											<xsl:value-of select="position()"/>
										</td>
										<xsl:call-template name="cell">
											<xsl:with-param name="st" select="$st"/>
											<xsl:with-param name="name" select="fullName"/>
										</xsl:call-template>
										<xsl:call-template name="cell">
											<xsl:with-param name="st" select="$st"/>
											<xsl:with-param name="name" select="faculty"/>
										</xsl:call-template>
										<xsl:call-template name="cell">
											<xsl:with-param name="st" select="$st"/>
											<xsl:with-param name="name" select="department"/>
										</xsl:call-template>
										<xsl:call-template name="cell">
											<xsl:with-param name="st" select="$st"/>
											<xsl:with-param name="name" select="course"/>
										</xsl:call-template>
										<xsl:for-each select="/report/query/attribute[not(contains($base, concat(' ', @naem, ' ')))]">
											<xsl:call-template name ="cell">
												<xsl:with-param name ="st" select="$st"/>
												<xsl:with-param name="name" select="string(@name)"/>
											</xsl:call-template>
										</xsl:for-each>
										<td>
											<xsl:for-each select ="residence">
												<div>
													<xsl:value-of select="@dormitory"/>, кімн. <xsl:value-of select="@room"/>
													(<xsl:value-of select="@dateFrom"/> - <xsl:value-of select="@dateTo"/>
												</div>
											</xsl:for-each>
										</td>
									</tr>
								</xsl:for-each>
							</tbody>
						</table>
					</xsl:otherwise>
				</xsl:choose>
			</body>
		</html>
	</xsl:template>
	<xsl:template name="head">
		<xsl:param name="name"/>
		<xsl:param name="label"/>
		<th>
			<xsl:if test="/report/query/attribute[@name = $name]">
				<xsl:attribute name ="class">searched</xsl:attribute>
			</xsl:if>
			<xsl:value-of select ="$label"/>
		</th>
	</xsl:template>
	<xsl:template name="cell">
		<xsl:param name="st"/>
		<xsl:param name="name"/>
		<td>
			<xsl:variable name="val" select="string($st/@*[name() = $name])"/>
			<xsl:choose>
				<xsl:when test="/report/query/attribute[@name = $name]">
					<xsl:call-template name ="highlight">
						<xsl:with-param name="text" select="$val"/>
					</xsl:call-template>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$val"/>
				</xsl:otherwise>
			</xsl:choose>
		</td>
	</xsl:template>
	<xsl:template name="highlight">
		<xsl:param name="text"/>
		<xsl:param name="k" select="1"/>
		<xsl:choose>
			<xsl:when test="$k &gt; count(/report/query/keyword)">
				<xsl:value-of select ="$text"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:variable name="kw" select="translate(string(/report/query/keyword[$k]), $upper, $lower)"/>
				<xsl:variable name="low" select="translate($text, $upper, $lower)"/>
				<xsl:choose>
					<xsl:when test="string-length($kw) &gt; 0 and contains($low, $kw)">
						<xsl:variable name="blen" select="string-length(substring-before($low, $kw))"/>
						<xsl:variable name="klen" select="string-length($kw)"/>
						<xsl:call-template name="highlight">
							<xsl:with-param name="text" select="substring($text, 1, $blen)"/>
							<xsl:with-param name="k" select="$k + 1"/>
						</xsl:call-template>
						<mark>
							<xsl:value-of select="substring($text, $blen + 1, $klen)"/>
						</mark>
						<xsl:call-template name="highlight">
							<xsl:with-param name ="text" select="substring($text, $blen + $klen + 1)"/>
							<xsl:with-param name="k" select="$k"/>
						</xsl:call-template>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name ="highlight">
							<xsl:with-param name="text" select="$text"/>
							<xsl:with-param name="k" select="$k + 1"/>
						</xsl:call-template>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:otherwise>
			</xsl:choose>
	</xsl:template>
</xsl:stylesheet>
