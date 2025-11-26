namespace SmartLibraryAPI.DTOs.Response
{
    public class FineResponse
    {
        public int Id { get; set; }
        public int LoanId { get; set; }
        public int UserId { get; set; }
        public required string UserName { get; set; }
        public decimal Amount { get; set; }
        public DateTime IssuedDate { get; set; }
        public bool IsPaid { get; set; }
        public DateTime? PaidDate { get; set; }
    }
}
