using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Strategies
{
    public class SearchCriteria
    {
        public List<string> Attributes { get; } = new();
        public List<string> Keywords { get; } = new();

        public bool Matches(IReadOnlyDictionary<string, string> attributes)
        {
            if (Attributes.Count == 0 || Keywords.Count == 0) return false;
            return Keywords.All(keyword => Attributes.Any(name => attributes.TryGetValue(name, out var value) && value.Contains(keyword, StringComparison.CurrentCultureIgnoreCase)));
        }
        public string BuildQuery()
        { var conditions = Keywords.Select(k => "(" + string.Join("or", Attributes.Select(a => $"contains(@{a}, {Literal(k)})")) + ")");
            return $"//student[{string.Join(" and ", conditions)}]";
        }
        private static string Literal(string s) => s.Contains('\'') ? $"\"" : $"'{s}'";
    }
}
