namespace miniCRM_back.Configs {
    public class Result<T> {
        public bool IsSuccess { get; }
        protected readonly T? _value;
        protected readonly string? _errorCode;
        protected readonly string? _errorMessage;

        protected Result(bool isSuccess, T? value, string? errorCode, string? errorMessage) {
            IsSuccess = isSuccess;
            _value = value;
            _errorCode = errorCode;
            _errorMessage = errorMessage;
        }

        public static Result<T> Success(T value) =>
            new Result<T>(true, value, null, null);

        public static Result<T> Failure(string errorCode, string errorMessage) =>
            new Result<T>(false, default, errorCode, errorMessage);

        public T Value => IsSuccess
            ? _value!
            : throw new InvalidOperationException("Cannot access Value on a failed result");

        public string ErrorCode => !IsSuccess
            ? _errorCode!
            : throw new InvalidOperationException("Cannot access ErrorCode on a successful result");

        public string ErrorMessage => !IsSuccess
            ? _errorMessage!
            : throw new InvalidOperationException("Cannot access ErrorMessage on a successful result");

    }
}
