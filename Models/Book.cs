namespace MauiApp1.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public bool IsAvailable { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public Book(string title, string author, string isbn = "")
        {
            Title = title;
            Author = author;
            ISBN = isbn;
        }

        public override string ToString()
        {
            return $"{Title} by {Author} - {(IsAvailable ? "Available" : "Borrowed")}";
        }
    }
}