namespace ServiceDefaults.Dtos.Responses
{
    public record ApiResponse<T>
    {
        public int StatusCode { get; set; }

        public bool IsSuccess { get; set; }

        public string? Message { get; set; }

        public IEnumerable<string>? Errors { get; set; }

        public T? Data { get; set; }

        public static ApiResponse<T> Success(int statusCode, T data, string? message = null)
        {
            return new ApiResponse<T>
            {
                StatusCode = statusCode,
                IsSuccess = true,
                Message = message,
                Data = data
            };
        }

        public static ApiResponse<T> Success(int statusCode, string? message = null)
        {
            return new ApiResponse<T>
            {
                IsSuccess = true,
                StatusCode = statusCode,
                Message = message
            };
        }

        public static ApiResponse<T> Failure(int statusCode, IEnumerable<string> errors, string? message = null)
        {
            return new ApiResponse<T>
            {
                StatusCode = statusCode,
                IsSuccess = false,
                Message = message,
                Errors = errors
            };
        }

        public static ApiResponse<T> Failure(int statusCode, string error, string? message = null)
        {
            return new ApiResponse<T>
            {
                StatusCode = statusCode,
                IsSuccess = false,
                Message = message,
                Errors = [error]
            };
        }
    }
}
