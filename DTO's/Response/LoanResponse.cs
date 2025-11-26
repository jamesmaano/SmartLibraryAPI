namespace SmartLibraryAPI.DTOs.Response
{
    public class LoanResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public required string UserName { get; set; }
        public int BookId { get; set; }
        public required string BookTitle { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool IsReturned { get; set; }
        public bool IsOverdue => !IsReturned && DateTime.Now > DueDate;
        public int DaysOverdue => IsOverdue ? (DateTime.Now - DueDate).Days : 0;
    }
}