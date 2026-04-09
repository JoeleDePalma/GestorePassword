using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Libreria.HTTPRequestsLibrary
{
    /// <summary>
    /// Simple HTTP client used to send requests to an API.
    /// Supports setting a JWT token for authenticated requests
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
        /// Sets the JWT token in the Authorization header for all requests
        /// </summary>
        /// <param name="token">The JWT token as a string.</param>
        public void SetToken(string token)
        {
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        /// <summary>
        /// Sets the current app version in the header for all requests
        /// </summary>
        /// <param name="version">The current app version </param>
        public void SetVersion(Version version)
        {
            _http.DefaultRequestHeaders.Remove("GestorePassword-version");
            _http.DefaultRequestHeaders.Add("GestorePassword-version", version.ToString());
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