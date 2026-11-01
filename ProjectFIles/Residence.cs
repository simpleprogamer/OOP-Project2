using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Models
{
    public class Residence
    {
        public string Dormitory { get; set; } = string.Empty;
        public string Room { get; set; } = string.Empty;
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public static Residence From(Func<string, string?> getAttribute) => new()
        {
            Dormitory = getAttribute("dormitory") ?? "",
            Room = getAttribute("room") ?? "",
            DateFrom = ParseDate(getAttribute("dateFrom")),
            DateTo = ParseDate(getAttribute("dateTo"))
        };
        private static DateTime? ParseDate(string? value )
            => DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out var d) ? d : null;
        private static string Fmt(DateTime? d) => d?.ToString("dd-MM-yyyy") ?? "?";
        public override string ToString() => $"{Dormitory}, {Room} ({Fmt(DateFrom)} - {Fmt(DateTo)})";
    }
}
