using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using MauiApp1.Models;

namespace MauiApp1.Services
{
    public class XmlAttributeLoader
    {
        public List<string> LoadAttributes(string path)
        {
            var names = new List<string>();
            var settings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Prohibit,
                IgnoreComments = true

            };
            using var reader = XmlReader.Create(path, settings);
            while (reader.Read())
            {
                if (reader.NodeType != XmlNodeType.Element || reader.Name != XMLNames.MainNode) continue;
                if (!reader.HasAttributes) continue;

                while (reader.MoveToNextAttribute())
                    if(!names.Contains(reader.Name)) names.Add(reader.Name);
                reader.MoveToElement();
            }
            return names;
        }
    }
}
