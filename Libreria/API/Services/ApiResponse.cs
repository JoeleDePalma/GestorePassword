namespace Libreria.HTTPRequestsLibrary.Services
{
    /// <summary>
    /// Represents the standard structure returned by the server in HTTP responses
    /// </summary>
    /// <typeparam name="T">The type of data included in the response</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Indicates whether the request was successful
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// A message returned by the server, usually used for errors or status information
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// The data returned by the server, if any
        /// </summary>
        public T? Data { get; set; }
    }
}