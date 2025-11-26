namespace SmartLibraryAPI.DTOs.Response
{
    public class UserResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public required string UserType { get; set; }
        public int BorrowLimit { get; set; }
        public int ReturnDays { get; set; }
        public DateTime RegisteredDate { get; set; }
    }
}