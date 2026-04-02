namespace GestioneDb.Services.Common
{
    /// <summary>
    /// Represents the standardized structure returned to the client in HTTP responses.
    /// This object is produced by <c>CatchStatusCodeMiddleware</c> after processing a <c>Result{T}</c>.
    /// </summary>
    /// <typeparam name="T">The type of data included in the response.</typeparam>
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}