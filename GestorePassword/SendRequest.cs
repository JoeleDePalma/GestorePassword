using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace GestorePassword
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
            => JsonSerializer.Deserialize<List<Password>>(await (await client.GetAsync(url)).Content.ReadAsStringAsync(), DeserializationOptions);

        public async Task<Password> GetPasswordByApp(string app)
        {
            var Response = await client.GetAsync($"{url}/ByApp/{app}");

            if (Response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;

            Password password = JsonSerializer.Deserialize<Password>(await Response.Content.ReadAsStringAsync(), DeserializationOptions);
            return password;
        }

        public async Task<HttpResponseMessage> AddPassword(Password NewPassword)
            =>await client.PostAsync(url, new StringContent(JsonSerializer.Serialize(NewPassword), Encoding.UTF8, "application/json"));
        

        public async Task<HttpResponseMessage> UpdatePassword(Password UpdatedPassword)
            =>await client.PutAsync($"{url}/ByApp/{UpdatedPassword.App}", new StringContent(JsonSerializer.Serialize(UpdatedPassword), Encoding.UTF8, "application/json"));
        

        public async Task<HttpResponseMessage> DeletePassword(string app)
            => await client.DeleteAsync($"{url}/ByApp/{app}");
    }
}
