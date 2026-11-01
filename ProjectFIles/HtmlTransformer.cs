using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Xsl;


namespace MauiApp1.Services
{
    public class HtmlTransformer
    {
        private readonly string _xslPath;
        public HtmlTransformer(string xslPath) => _xslPath = xslPath;
        public string Transform(string xmlPath)
        {
            var xslt = new XslCompiledTransform();
            xslt.Load(_xslPath);

            using var ms = new MemoryStream();
            using (var writer = XmlWriter.Create(ms, xslt.OutputSettings))
            {
                xslt.Transform(xmlPath, null, writer);
            }
            return Encoding.UTF8.GetString(ms.ToArray()).TrimStart('\uFEFF');

        }

    }
}
