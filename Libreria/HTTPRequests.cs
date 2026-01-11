using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace HTTPRequestsLibrary
{
    public class SendRequest : IDisposable
    {
        private readonly HttpClient client; 
        private readonly string url; 
        private readonly JsonSerializerOptions DeserializationOptions;
        private bool disposed;
        public SendRequest()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            client = new HttpClient(handler) { BaseAddress = new Uri("http://localhost:5211/") };
            url = "api/Passwords";
            DeserializationOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public void Dispose()
        {
            if (disposed) return;
            client?.Dispose();
            disposed = true;
        }

        public async Task<List<Password>> GetPasswords()
            => JsonSerializer.Deserialize<List<Password>>(await (await client.GetAsync(url)).Content.ReadAsStringAsync(), DeserializationOptions);

        public async Task<Password> GetPasswordByApp(string app)
        {
            var Response = await client.GetAsync($"{url}/ByApp/{app}");

            if (Response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;

            Password? password = JsonSerializer.Deserialize<Password>(await Response.Content.ReadAsStringAsync(), DeserializationOptions);
            return password;
        }

        public async Task<HttpResponseMessage> AddPassword(Password NewPassword)
            => await client.PostAsync(url, new StringContent(JsonSerializer.Serialize(NewPassword), Encoding.UTF8, "application/json"));

        public async Task<HttpResponseMessage> UpdatePasswordByApp(Password UpdatedPassword)
            => await client.PutAsync($"{url}/ByApp/{UpdatedPassword.App}", new StringContent(JsonSerializer.Serialize(UpdatedPassword), Encoding.UTF8, "application/json"));

        public async Task<HttpResponseMessage> DeletePasswordByApp(string app)
            => await client.DeleteAsync($"{url}/ByApp/{app}");

        public async Task<HttpResponseMessage> DeletePasswords()
            => await client.DeleteAsync(url);
    }
}
