using Microsoft.AspNetCore.Http;

namespace RoyalVilla.DTO
{
    public class ApiResponse<TData>
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public TData? Data { get; set; }
        public object? Errors { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public static ApiResponse<TData> Create(bool success, int statusCode, string? message = null, TData? data = default, object? errors = null)
        {
            return new ApiResponse<TData>
            {
                Success = success,
                StatusCode = statusCode,
                Message = message,
                Data = data,
                Errors = errors
            };
        }

        public static ApiResponse<TData> Ok(TData? data = default, string? message = "Request successful") =>
            Create(true, StatusCodes.Status200OK, message, data: data);

        public static ApiResponse<TData> CreatedAt(TData? data = default, string message = "Resource created successfully") =>
            Create(true, StatusCodes.Status201Created, message, data: data);

        public static ApiResponse<TData> NoContent(string message = "No content available") =>
            Create(true, StatusCodes.Status204NoContent, message);

        public static ApiResponse<TData> BadRequest(string message, object? errors = null) =>
            Create(false, StatusCodes.Status400BadRequest, message, errors: errors ?? "Bad Request");

        public static ApiResponse<TData> NotFound(string message) => Create(false, StatusCodes.Status404NotFound, message);

        public static ApiResponse<TData> InternalServerError(string message = "An unexpected error occurred") =>
            Create(false, StatusCodes.Status500InternalServerError, message);

        public static ApiResponse<TData> Conflict(string? message = "Conflict occurred") =>
            Create(false, StatusCodes.Status409Conflict, message);

        public static ApiResponse<TData> Error(int statusCode, string message, object? errors = default) =>
            Create(false, statusCode, message, errors: errors);
    }
}
