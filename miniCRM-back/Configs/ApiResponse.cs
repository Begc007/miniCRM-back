namespace miniCRM_back.Configs {
    public class ApiResponse<T> {
        public bool Success { get; set; }
        public required string Message { get; set; }
        public string? ErrorCode { get; set; }
        public T? Data { get; set; }
        public PaginationMetadata? Pagination { get; set; }
        public static ApiResponse<T> SuccessResponse(T data, string? message = null, PaginationMetadata? pagination = null) {
            return new ApiResponse<T> {
                Success = true,
                Data = data,
                Message = message ?? "Operation completed successfully",
                Pagination = pagination
            };
        }

        public static ApiResponse<T> ErrorResponse(string errorCode, string message) {
            return new ApiResponse<T> {
                Success = false,
                ErrorCode = errorCode,
                Message = message
            };
        }
    }

    
}
