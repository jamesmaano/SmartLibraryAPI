namespace MauiApp1.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string Role { get; set; } // User, Librarian, Admin
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? LastLogin { get; set; }
        public bool IsActive { get; set; } = true;

        public Account(string username, string passwordHash, string email, string role)
        {
            Username = username;
            PasswordHash = passwordHash;
            Email = email;
            Role = role;
        }
    }
}