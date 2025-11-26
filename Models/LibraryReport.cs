using System.ComponentModel.DataAnnotations;

namespace SmartLibraryAPI.Models
{
    public class LibraryReport
    {
        [Key]
        public string ReportTitle { get; set; } = string.Empty;
        public DateTime GeneratedDate { get; set; } = DateTime.Now;
        public Dictionary<string, object> Data { get; set; } = new();

        // Constructor for EF Core
        public LibraryReport() { }

        public LibraryReport(string title)
        {
            ReportTitle = title;
        }
    }
}