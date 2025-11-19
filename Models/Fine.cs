namespace MauiApp1.Models
{
    public class Fine
    {
        public int Id { get; set; }
        public int LoanId { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public DateTime IssuedDate { get; set; }
        public bool IsPaid { get; set; } = false;
        public DateTime? PaidDate { get; set; }

        public const decimal DailyFineAmount = 5.00m;

        public Fine(int loanId, int userId, int daysOverdue)
        {
            LoanId = loanId;
            UserId = userId;
            Amount = daysOverdue * DailyFineAmount;
            IssuedDate = DateTime.Now;
        }
    }
}
