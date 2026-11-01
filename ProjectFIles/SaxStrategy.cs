using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using MauiApp1.Models;

namespace MauiApp1.Strategies
{
    public class SaxStrategy : IAnalysisStrategy
    {
        public string Name => "SAX API";

        public List<Resident> Search(string path, SearchCriteria criteria)
        {
            var result = new List<Resident>();
            var settings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Prohibit,
                IgnoreComments = true,
                IgnoreWhitespace = true
            };
            using var reader = XmlReader.Create(path, settings);
            Resident? current = null;
            while (reader.Read()) 
            {
                switch (reader.NodeType) 
                {
                    case XmlNodeType.Element when reader.Name == XMLNames.MainNode:
                        var attrs = ReadAttributes(reader);
                    current = criteria.Matches(attrs) ? Resident.FromAttributes(attrs) : null;
                    if (current != null) result.Add(current);
                    if (reader.IsEmptyElement) current = null;
                        break;
                    case XmlNodeType.Element when reader.Name == XMLNames.ResidenceNode && current != null:    
                        var resAttrs = ReadAttributes(reader);
                        current.Residences.Add(Residence.From(n => resAttrs.GetValueOrDefault(n)));
                        break;
                    case XmlNodeType.EndElement when reader.Name == XMLNames.MainNode:
                        current = null;
                        break;
                }
        }
            return result;
        }
        private static Dictionary<string, string> ReadAttributes(XmlReader reader)
        {
            var attrs = new Dictionary<string, string>();
            if (reader.HasAttributes)
            { 
                while (reader.MoveToNextAttribute()) attrs[reader.Name] = reader.Value;
                reader.MoveToElement();
            }
            return attrs;
        }
        }
}
