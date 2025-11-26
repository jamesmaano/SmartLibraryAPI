using System.ComponentModel.DataAnnotations;

namespace SmartLibraryAPI.Models
{
    public class OperationResult
    {
        [Key]
        public required bool Success { get; set; }
        public required string Message { get; set; }
        public object? Data { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}