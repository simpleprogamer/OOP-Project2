using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Models
{
    public class Resident
    {
        private static readonly HashSet<string> KnownAttributes = new() { "id", "fullName", "faculty", "departament", "course" };
        public string FullName { get; set; } = string.Empty;
        public string Faculty { get; set; } = string.Empty;
        public string Departament { get; set; } = string.Empty;
        public int Course { get; set; }
        public Dictionary<string, string> Attributes { get; } = new();
        public List<Residence> Residences { get; } = new();
        public static Resident FromAttributes(IReadOnlyDictionary<string, string> attrs)
        {
            var r = new Resident();
            foreach (var pair in attrs) r.Attributes[pair.Key] = pair.Value;
            r.FullName = attrs.GetValueOrDefault("fullName", "");
            r.Faculty = attrs.GetValueOrDefault("faculty", "");
            r.Departament = attrs.GetValueOrDefault("departament", "");
            r.Course = int.TryParse(attrs.GetValueOrDefault("course", ""), out var c) ? c : 0;
            return r;
        }
        public string Format(int index)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"{index}.{(FullName == "" ? "(Without FullName)}" : FullName)}");
            sb.AppendLine($" Faculty: {Show(Faculty)}");
            sb.AppendLine($" Departament: {Show(Departament)}");
            sb.AppendLine($" Course: {(Course == 0 ? "-" : Course.ToString())}");

            var others = Attributes.Where(p => !KnownAttributes.Contains(p.Key)).Select(p => $"{p.Key}={p.Value}").ToList();
            if (others.Count > 0)
                sb.AppendLine($" Other attributes: {string.Join(", ", others)}");
            if (Residences.Count == 0)
                sb.AppendLine("Residences: -");
            else
            {
                sb.AppendLine("Residences:");
                foreach (var res in Residences)
                    sb.AppendLine($"  - {res}");
            }
            return sb.ToString();
        }

        private static string Show(string value) => value == "" ? "(Without value)" : value;
    }
}
