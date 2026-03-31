namespace GestioneDb.Services.Common
{
    public class Result<T>
    {
        public bool Success { get; set; }
        public StatusCode responseStatusCode { get; set; }
        public T Data { get; set; }
        public string? ErrorString { get; set; }

        public static Result<T> Ok(T data, StatusCode statusCode) =>
            new Result<T> { Success = true, responseStatusCode = statusCode, Data = data };

        public static Result<T> Fail(StatusCode statusCode, string errorString = "") =>
            new Result<T> { Success = false, responseStatusCode = statusCode, ErrorString = errorString };
    }
}
