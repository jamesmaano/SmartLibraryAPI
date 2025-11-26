using System;

namespace SmartLibraryAPI.Models
{
    public partial class Fine
    {
        public int Id { get; set; }
        public int LoanId { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public DateTime IssuedDate { get; set; }
        public DateTime? PaidDate { get; set; }
        public bool IsPaid { get; set; }

        public virtual Loan Loan { get; set; } = null!;
        public virtual User User { get; set; } = null!;

        public Fine() { }

        // Constructor matching your service calls
        public Fine(int loanId, int userId, decimal amount)
        {
            LoanId = loanId;
            UserId = userId;
            Amount = amount;
            IssuedDate = DateTime.Now; // default to now
            IsPaid = false;
        }
    }
}
