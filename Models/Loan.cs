namespace MauiApp1.Models
{
    public class Loan
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool IsReturned { get; set; } = false;

        public User User { get; set; }
        public Book Book { get; set; }

        public Loan(int userId, int bookId, User user, Book book, int returnDays)
        {
            UserId = userId;
            BookId = bookId;
            User = user;
            Book = book;
            BorrowDate = DateTime.Now;
            DueDate = DateTime.Now.AddDays(returnDays);
        }
    }
}