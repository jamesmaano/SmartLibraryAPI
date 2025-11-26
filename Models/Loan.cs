using System;

namespace SmartLibraryAPI.Models
{
    public partial class Loan
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool IsReturned { get; set; }

        public virtual Book Book { get; set; } = null!;
        public virtual User User { get; set; } = null!;

        public Loan() { }

        // Constructor matching your LoanService (5 args)
        public Loan(int id, int userId, int bookId, DateTime borrowDate, DateTime dueDate)
        {
            Id = id;
            UserId = userId;
            BookId = bookId;
            BorrowDate = borrowDate;
            DueDate = dueDate;
            IsReturned = false;
        }
    }
}
