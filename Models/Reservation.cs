namespace MauiApp1.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime ReservationDate { get; set; }
        public bool IsActive { get; set; } = true;

        public User User { get; set; }
        public Book Book { get; set; }

        public Reservation(int userId, int bookId, User user, Book book)
        {
            UserId = userId;
            BookId = bookId;
            User = user;
            Book = book;
            ReservationDate = DateTime.Now;
        }
    }
}