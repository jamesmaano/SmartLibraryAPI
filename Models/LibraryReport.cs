namespace MauiApp1.Models
{
    public class LibraryReport
    {
        public string ReportTitle { get; set; }
        public DateTime GeneratedDate { get; set; }
        public Dictionary<string, object> Data { get; set; }

        public LibraryReport(string title)
        {
            ReportTitle = title;
            GeneratedDate = DateTime.Now;
            Data = new Dictionary<string, object>();
        }
    }
}