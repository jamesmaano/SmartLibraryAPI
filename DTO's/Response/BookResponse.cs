namespace SmartLibraryAPI.DTOs.Response
{
    public class BookResponse
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Author { get; set; }
        public string? ISBN { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
