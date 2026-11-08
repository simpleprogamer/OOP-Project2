using MauiApp1.Models;
using MauiApp1.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;


namespace MauiApp1.Services
{
    public class HtmlTransformer
    {
        private readonly string _xslPath;
        public HtmlTransformer(string xslPath) => _xslPath = xslPath;
        public string Transform(IEnumerable<Resident> results, SearchCriteria criteria, string methodName)
        {
            XDocument report = BuildReport(results, criteria, methodName);
            var xslt = new XslCompiledTransform();
            xslt.Load(_xslPath);

            using var ms = new MemoryStream();
            using (var writer = XmlWriter.Create(ms, xslt.OutputSettings))
            {
                xslt.Transform(report.CreateReader(), null, writer);
            }
            return Encoding.UTF8.GetString(ms.ToArray()).TrimStart('\uFEFF');

        }
        private static XDocument BuildReport(IEnumerable<Resident> results, SearchCriteria criteria, string methodName)
        {
            var query = new XElement("query", new XAttribute("method", methodName), new XAttribute("xpath", criteria.BuildQuery()), criteria.Attributes.Select(a => new XElement("attribute", new XAttribute("naem", a))), criteria.Keywords.Select(k => new XElement("keyword", k)));
            var students = new XElement("students", results.Select(r => new XElement("student", r.Attributes.Select(p => new XAttribute(p.Key, p.Value)), r.Residences.Select(res => new XElement("residence", new XAttribute("dormitory", res.Dormitory), new XAttribute("room", res.Room), new XAttribute("dateFrom", res.DateFrom?.ToString("dd.MM.yyyy") ?? "?"), new XAttribute("dateTo", res.DateTo?.ToString("dd.MM.yyyy") ?? "?"))))));
            return new XDocument(new XElement("report", query, students));         
        }
    }
}
