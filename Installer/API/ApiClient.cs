using System.Net.Http;
using System.Net.Http.Headers;

namespace Installer.HTTPRequestsLibrary
{
    /// <summary>
    /// Simple HTTP client used to send requests to an API.
    /// </summary>
    public class ApiClient : IDisposable
    {
        private readonly HttpClient _http;
        private bool _disposed;

        public ApiClient(string BaseUrl)
        {
            _http = new HttpClient()
            {
                BaseAddress = new Uri(BaseUrl)
            };
        }

        /// <summary>
        /// Releases the HTTP client and closes all connections
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;
            _http.Dispose();
            _disposed = true;
        }

        /// <summary>
        /// Gets the internal HttpClient used to send requests
        /// </summary>
        public HttpClient Http => _http;
    }
}