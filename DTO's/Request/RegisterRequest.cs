using System.ComponentModel.DataAnnotations;

namespace SmartLibraryAPI.DTOs.Request
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters")]
        public required string Username { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Full name is required")]
        [StringLength(255, ErrorMessage = "Full name cannot exceed 255 characters")]
        public required string FullName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6,
            ErrorMessage = "Password must be between 6 and 100 characters")]
        public required string Password { get; set; }

        [Required(ErrorMessage = "Student ID is required")]
        [StringLength(50, ErrorMessage = "Student ID cannot exceed 50 characters")]
        public required string StudentId { get; set; }

        [StringLength(50, ErrorMessage = "Role cannot exceed 50 characters")]
        public string Role { get; set; } = "Student";
    }
}
