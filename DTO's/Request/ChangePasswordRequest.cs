using System.ComponentModel.DataAnnotations;

namespace SmartLibraryAPI.DTOs.Request
{
    public class ChangePasswordRequest
    {
        [Required(ErrorMessage = "Account ID is required")]
        public int AccountId { get; set; }

        [Required(ErrorMessage = "Current password is required")]
        public required string CurrentPassword { get; set; }

        [Required(ErrorMessage = "New password is required")]
        [StringLength(100, MinimumLength = 6,
            ErrorMessage = "Password must be between 6 and 100 characters")]
        public required string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
        public string? ConfirmPassword { get; set; }
    }
}