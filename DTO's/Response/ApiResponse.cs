namespace SmartLibraryAPI.DTOs.Response
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public required string Message { get; set; }
        public T? Data { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public static ApiResponse<T> SuccessResponse(string message, T? data = default)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        public static ApiResponse<T> ErrorResponse(string message)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Data = default
            };
        }
    }
}