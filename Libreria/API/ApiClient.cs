using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace HTTPRequestsLibrary
{
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

        public void SetToken(string token)
        {
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public void Dispose()
        {
            if (_disposed) return;
            _http.Dispose();
            _disposed = true;
        }
        public HttpClient Http => _http;
    }
}
