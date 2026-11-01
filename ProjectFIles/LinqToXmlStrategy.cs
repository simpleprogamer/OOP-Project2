using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MauiApp1.Models;

namespace MauiApp1.Strategies
{
    public class LinqToXmlStrategy : IAnalysisStrategy
    {
        public string Name => "LINQ to XML";
        public List<Resident> Search(string path, SearchCriteria criteria)
        {
            var doc = XDocument.Load(path);

            return doc.Descendants(XMLNames.MainNode)
                .Select(e => new
                {
                    Element = e,
                    Attrs =e.Attributes().ToDictionary(a => a.Name.LocalName, a => a.Value)
                })
                .Where(x => criteria.Matches(x.Attrs))
                .Select(x =>
                {
                    var resident = Resident.FromAttributes(x.Attrs);
                    resident.Residences.AddRange(x.Element.Elements(XMLNames.ResidenceNode).Select(r => Residence.From(n => (string?)r.Attribute(n))));
                    return resident;
                })
                .ToList();
        }
    }
}
