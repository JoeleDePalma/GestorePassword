namespace GestioneDb.Services.Common
{
    /// <summary>
    /// Represents the standardized result returned by controllers.
    /// These values are intercepted by <c>CatchStatusCodeMiddleware</c> 
    /// and transformed into an <c>ApiResponse{T}</c> object
    /// </summary>
    /// <typeparam name="T">The type of data returned by the controller </typeparam>
    public class Result<T>
    {
        public bool Success { get; set; }
        public StatusCode responseStatusCode { get; set; }
        public T Data { get; set; }
        public string? ErrorString { get; set; }

        public static Result<T> Ok(T data, StatusCode statusCode) =>
            new Result<T> { Success = true, responseStatusCode = statusCode, Data = data };

        public static Result<T> Fail(StatusCode statusCode, string errorString) =>
            new Result<T> { Success = false, responseStatusCode = statusCode, ErrorString = errorString };
    }
}
