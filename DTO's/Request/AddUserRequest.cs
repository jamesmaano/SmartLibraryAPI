using System.ComponentModel.DataAnnotations;

namespace SmartLibraryAPI.DTOs.Request
{
    public class AddUserRequest
    {
        [Required(ErrorMessage = "User type is required")]
        [RegularExpression("^(Student|Faculty)$",
            ErrorMessage = "User type must be either 'Student' or 'Faculty'")]
        public required string UserType { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(255, ErrorMessage = "Name cannot exceed 255 characters")]
        public required string Name { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format")]
        [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
        public string? PhoneNumber { get; set; }

        [StringLength(50, ErrorMessage = "ID number cannot exceed 50 characters")]
        public string? IdNumber { get; set; }
    }
}