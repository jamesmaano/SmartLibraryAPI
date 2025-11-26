namespace SmartLibraryAPI.DTOs.Response
{
    public class LoginResponse
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string FullName { get; set; }
        public required string Role { get; set; }
        public string? Token { get; set; }
        public DateTime LoginTime { get; set; } = DateTime.UtcNow;
    }
}