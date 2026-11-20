<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="html" encoding="UTF-8" indent="yes"/>

	<xsl:variable name ="upper" select ="'ABCDEFGHIJKLMNOPQRSTUVWXYZАБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЬЮЯ'"/>
	<xsl:variable name ="lower" select ="'abcdefghijklmnopqrstuvwxyzабвгґдеєжзиіїйклмнопрстуфхцчшщьюя'"/>
	<xsl:template name ="title">
	<xsl:param name ="name"/>
	<xsl:choose>
		<xsl:when test="$name = 'id'">ID</xsl:when>
		      <xsl:when test="$name = 'fullName'">П.І.П.</xsl:when>
      <xsl:when test="$name = 'faculty'">Факультет</xsl:when>
      <xsl:when test="$name = 'department'">Кафедра</xsl:when>
      <xsl:when test="$name = 'group'">Група</xsl:when>
      <xsl:when test="$name = 'course'">Курс</xsl:when>
      <xsl:when test="$name = 'email'">Email</xsl:when>
      <xsl:when test="$name = 'phone'">Телефон</xsl:when>
      <xsl:when test="$name = 'scholarship'">Стипендія</xsl:when>
      <xsl:when test="$name = 'birthYear'">Рік народження</xsl:when>
      <xsl:when test="$name = 'citizenship'">Громадянство</xsl:when>
      <xsl:when test="$name = 'note'">Примітка</xsl:when>
		<xsl:otherwise>
			<xsl:value-of select="$name"/>
		</xsl:otherwise>	
	</xsl:choose>
	</xsl:template>
<xsl:template match="/report">
		<html>
			<head>
				<title>Результати пошуку</title>
				<style>
          body { font-family: Arial, sans-serif; margin: 16px; }
          h1 { font-size: 20px; }
          .query { background: #f3f6fb; border: 1px solid #c9d6ea; padding: 8px 12px; margin-bottom: 14px; font-size: 14px; }
          .query p { margin: 4px 0; }
          code { background: #ffffff; color: #000000; padding: 1px 4px; border: 1px solid #ddd; word-break: break-all; }
          .empty { color: #a33; font-weight: bold; }
          .legend { font-size: 13px; color: #444; margin: 0 0 8px 0; }
          table { border-collapse: collapse; width: 100%; }
          th, td { border: 1px solid #888888; padding: 6px 8px; text-align: left; vertical-align: top; font-size: 14px; }
          th { background: #e8eef7; color: #000000; }
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
						<b>	Умови пошуку: </b>
						<xsl:text> Атрибут</xsl:text>
						<xsl:for-each select ="query/attribute">
						    <xsl:text>«</xsl:text>
							<xsl:call-template name ="title">
								<xsl:with-param name="name" select="string(@name)"/>
							</xsl:call-template>
							<xsl:text> » (</xsl:text><xsl:value-of select="@name"/><xsl:text>)</xsl:text>
							<xsl:if test ="position() != last()">
								<xsl:text>, </xsl:text>
							</xsl:if>
							</xsl:for-each>
						<xsl:text> містить кожне із слів:</xsl:text>
						<xsl:for-each select="query/keyword">
							<mark>
								<xsl:value-of select="."/>
							</mark>
							<xsl:if test="position() != last()">
								<xsl:text> </xsl:text>
							</xsl:if>				
						</xsl:for-each>
					</p>
					<xsl:if test="string(query/@xpath) != ''">
						<p>
							<b>XPath-запит:</b>
							<code>
								<xsl:value-of select="query/@xpath"/>
							</code>
						</p>
					</xsl:if>
					<p>
						<b> Знайдено записів:</b>
						<xsl:text></xsl:text>
						<xsl:value-of select="count(students/student)"/>
					</p>
				</div>
				<xsl:choose>
					<xsl:when test="count(students/student) = 0">
						<p class="empty"> За вказаними умовами нічого не знайдено.</p>
					</xsl:when>
					<xsl:otherwise>
						<p class="legend">Назви колонок вказують, що означає кожне значення в рядку. Колонка атрибута пошуку виділена жовтим, знайдені слова підсвічені.</p>
						<table border="1" cellpadding="6" cellspacing="0"></table>
							<table>
							<thead>
								<tr>
									<th style="background:#e8eef7">№</th>
                  <xsl:for-each select="columns/column">
                    <xsl:variable name="n" select="string(@name)"/>
                    <th>
                      <xsl:attribute name="style">
                        <xsl:choose>
                          <xsl:when test="/report/query/attribute[@name = $n]">C</xsl:when>
                          <xsl:otherwise>background:#e8eef7</xsl:otherwise>
                        </xsl:choose>
                      </xsl:attribute>
                      <xsl:call-template name="title"><xsl:with-param name="name" select="$n"/></xsl:call-template>
                    </th>
                  </xsl:for-each>
									<th style="background:#e8eef7">Місце проживання (з датами)</th>
								</tr>
							</thead>
              <tbody>
                <xsl:for-each select="students/student">
                  <xsl:variable name="st" select="."/>
                  <tr>
                    <td><xsl:value-of select="position()"/></td>
                    <xsl:for-each select="/report/columns/column">
                      <xsl:call-template name="cell">
                        <xsl:with-param name="st" select="$st"/>
                        <xsl:with-param name="name" select="string(@name)"/>
                      </xsl:call-template>
                    </xsl:for-each>
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
					</xsl:otherwise>
				</xsl:choose>
				</body>
			</html>
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
