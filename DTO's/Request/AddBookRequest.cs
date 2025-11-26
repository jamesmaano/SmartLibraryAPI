using System.ComponentModel.DataAnnotations;

namespace SmartLibraryAPI.DTOs.Request
{
    public class AddBookRequest
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(255, ErrorMessage = "Title cannot exceed 255 characters")]
        public required string Title { get; set; }

        [Required(ErrorMessage = "Author is required")]
        [StringLength(255, ErrorMessage = "Author cannot exceed 255 characters")]
        public required string Author { get; set; }

        [StringLength(20, ErrorMessage = "ISBN cannot exceed 20 characters")]
        public string? ISBN { get; set; }
    }
}