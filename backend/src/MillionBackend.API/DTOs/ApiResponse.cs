namespace MillionBackend.API.DTOs;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public List<string> Errors { get; set; } = new List<string>();
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public static ApiResponse<T> SuccessResult(T data, string message = "Operation completed successfully.")
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data,
            Errors = new List<string>()
        };
    }

    public static ApiResponse<T> ErrorResult(string message, List<string>? errors = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Data = default,
            Errors = errors ?? new List<string>()
        };
    }

    public static ApiResponse<T> ValidationErrorResult(List<string> validationErrors)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = "Validation failed.",
            Data = default,
            Errors = validationErrors
        };
    }
}

// Non-generic version for simple responses
public class ApiResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = new List<string>();
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public static ApiResponse SuccessResult(string message = "Operation completed successfully.")
    {
        return new ApiResponse
        {
            Success = true,
            Message = message,
            Errors = new List<string>()
        };
    }

    public static ApiResponse ErrorResult(string message, List<string>? errors = null)
    {
        return new ApiResponse
        {
            Success = false,
            Message = message,
            Errors = errors ?? new List<string>()
        };
    }
}
