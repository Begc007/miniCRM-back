namespace miniCRM_back.Configs {
    public class PagedResult<T> : Result<T> {
        private readonly PaginationMetadata? _pagination;

        private PagedResult(bool isSuccess, T? value, PaginationMetadata? pagination, string? errorCode, string? errorMessage)
            : base(isSuccess, value, errorCode, errorMessage) {
            _pagination = pagination;
        }

        public static PagedResult<T> Success(T value, PaginationMetadata pagination)
            => new PagedResult<T>(true, value, pagination, null, null);

        public static new PagedResult<T> Failure(string errorCode, string errorMessage)
            => new PagedResult<T>(false, default, null, errorCode, errorMessage);

        public PaginationMetadata Pagination => IsSuccess
            ? _pagination!
            : throw new InvalidOperationException("Cannot access Pagination on a failed result");
    }
}
