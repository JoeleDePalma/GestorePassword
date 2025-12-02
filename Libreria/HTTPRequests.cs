using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using System.Text.Json;
using System.Net.Http;

namespace HTTPRequestsLibrary
{ 
    public class SendRequest
    {
        private readonly HttpClient client;
        private readonly string url;
        private readonly JsonSerializerOptions DeserializationOptions;

        public SendRequest()
        {
            client = new HttpClient();
            url = "https://localhost:7012/api/Passwords";
            DeserializationOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<List<Password>> GetPasswords()
        {
            var response = await client.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Password>>(json, DeserializationOptions);
        }

        public async Task<Password> GetPasswordByApp(string app)
        {
            var Response = await client.GetAsync($"{url}/ByApp/{app}");

            if (Response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;

            var json = await Response.Content.ReadAsStringAsync();
            Password password = JsonSerializer.Deserialize<Password>(json, DeserializationOptions);
            return password;
         }

        public async Task<HttpResponseMessage> AddPassword(Password NewPassword)
            => await client.PostAsync(url, new StringContent(JsonSerializer.Serialize(NewPassword), Encoding.UTF8, "application/json"));


        public async Task<HttpResponseMessage> UpdatePassword(Password UpdatedPassword)
            => await client.PutAsync($"{url}/ByApp/{UpdatedPassword.App}", new StringContent(JsonSerializer.Serialize(UpdatedPassword), Encoding.UTF8, "application/json"));

        public async Task<HttpResponseMessage> DeletePasswords()
            => await client.DeleteAsync(url);
        public async Task<HttpResponseMessage> DeletePasswordByApp(string app)
            => await client.DeleteAsync($"{url}/ByApp/{app}");
    }
}