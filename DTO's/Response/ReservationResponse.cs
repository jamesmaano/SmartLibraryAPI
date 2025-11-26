namespace SmartLibraryAPI.DTOs.Response
{
    public class ReservationResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public required string UserName { get; set; }
        public int BookId { get; set; }
        public required string BookTitle { get; set; }
        public DateTime ReservationDate { get; set; }
        public bool IsActive { get; set; }
    }
}