using System.ComponentModel.DataAnnotations;

namespace SmartLibraryAPI.DTOs.Request
{
    public class BorrowBookRequest
    {
        [Required(ErrorMessage = "User ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "User ID must be greater than 0")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Book ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Book ID must be greater than 0")]
        public int BookId { get; set; }
    }
}