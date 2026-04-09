using Libreria.HTTPRequestsLibrary;
using Libreria.API;
using Libreria.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Text;
using Tests.APITests.Users.Interfaces;

namespace Tests.APITests.Users.Services
{
    public class UserApiTestService : IUserApiTestService
    {
        private UserApi _api;
        private ApiClient _client;

        public UserApiTestService(ApiClient Client)
        {
            _api = new UserApi(Client);
            _client = Client;
        }

        public async Task RegisterAsync(string username, string password)
        {
            var dto = new RegisterDTO()
            {
                Username = username,
                Password = password
            };

            var response = await _api.RegisterAsync(dto);

            Assert.That(response.Success, Is.EqualTo(true));
        }

        public async Task<string> LoginAsync(string username, string password)
        {
            var dto = new LoginDTO()
            {
                Username = username,
                Password = password
            };

            var response = await _api.LoginAsync(dto);

            Assert.That(response.Success, Is.EqualTo(true));

            Console.WriteLine(response.Data.Token); 

            return response.Data.Token.ToString();
        }

        public async Task UpdateAsync(string newUsername, string newPassword, string token)
        {
            _client.SetToken(token);

            var dto = new UpdateUserDTO()
            {
                Username = newUsername,
                Password = newPassword
            };

            var response = await _api.UpdateByIdAsync(dto);

            Assert.That(response.Success, Is.EqualTo(true));
        }

        public async Task<int> GetByTokenAsync(string token, string usernameExpected)
        {
            _client.SetToken(token);
            
            var response = await _api.GetByTokenAsync();
            
            Assert.That(response.Success, Is.EqualTo(true));
            Assert.That(response.Data.Username, Is.EqualTo(usernameExpected));

            return response.Data.UserID;
        }

        public async Task GetByIdAsync(int id, string usernameExpected)
        {
            var response = await _api.GetByIdAsync(id);

            Assert.That(response.Success, Is.EqualTo(true));
            Assert.That(response.Data.Username, Is.EqualTo(usernameExpected));
        }

        public async Task DeleteByTokenAsync(string token)
        {
            _client.SetToken(token);

            var response = await _api.DeleteByTokenAsync();

            Assert.That(response.Success, Is.EqualTo(true));
        }
    }
}
