using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using MauiApp1.Models;

namespace MauiApp1.Strategies
{
    public class DomStrategy : IAnalysisStrategy
    {
        public string Name => "DOM API";
        public List<Resident> Search(string path, SearchCriteria criteria)
        {
            var result = new List<Resident>();
            var doc = new XmlDocument { XmlResolver = null };
            doc.Load(path);

            foreach (XmlNode node in doc.GetElementsByTagName(XMLNames.MainNode))
            {
                var attrs = new Dictionary<string, string>();
                if (node.Attributes != null)
                    foreach (XmlAttribute a in node.Attributes) attrs[a.Name] = a.Value;
                if (!criteria.Matches(attrs)) continue;
                var resident = Resident.FromAttributes(attrs);
                foreach (XmlNode child in node.ChildNodes)
                {
                    if (child.NodeType == XmlNodeType.Element && child.Name == XMLNames.ResidenceNode)
                        resident.Residences.Add(Residence.From(n => child.Attributes?[n]?.Value));

                }
                result.Add(resident);
            }
            return result;
        }
    }
}
