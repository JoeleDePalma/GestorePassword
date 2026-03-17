using HTTPRequestsLibrary;
using Libreria.API;
using Libreria.DTOs.Passwords;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using Tests.APITests.Passwords.Interfaces;

namespace Tests.APITests.Passwords.Services
{
    public class PasswordApiTestService : IPasswordApiTestService
    {
        private PasswordApi _api;
        private ApiClient _client;

        public PasswordApiTestService(ApiClient Client)
        {
            _api = new PasswordApi(Client);
            _client = Client;
        }

        public async Task<int> CreateAsync(string token, CreatePasswordDTO NewPassword)
        {
            _client.SetToken(token);

            var response = await _api.CreateAsync(NewPassword);
            
            Assert.That(response.Success, Is.EqualTo(true));

            TestContext.WriteLine(response.Data.GetType().FullName);
            TestContext.WriteLine($"Saved ID: {response.Data.CredentialID}");
            
            return response.Data.CredentialID;
        }

        public async Task UpdateByIdAsync(string token, UpdatePasswordDTO UpdatedPassword, int passwordId)
        {
            _client.SetToken(token);

            TestContext.WriteLine("ID: " + passwordId);

            var response = await _api.UpdateByIdAsync(passwordId, UpdatedPassword);

            Assert.That(response.Success, Is.EqualTo(true));
        }

        public async Task UpdateByAppAsync(string token, UpdatePasswordDTO UpdatedPassword, string passwordApp)
        {
            _client.SetToken(token);

            var response = await _api.UpdateByAppAsync(passwordApp, UpdatedPassword);

            Assert.That(response.Success, Is.EqualTo(true));
        }

        public async Task GetAllAsync(string token, string masterPassword, string appExpected, string usernameExpected, string passwordExpected)
        {
            _client.SetToken(token);

            var response = await _api.GetAllAsync(masterPassword);

            Assert.That(response.Success, Is.EqualTo(true));
            Assert.That(response.Data[0].AppName , Is.EqualTo(appExpected));
            Assert.That(response.Data[0].AppUsername , Is.EqualTo(usernameExpected));
            Assert.That(response.Data[0].Password , Is.EqualTo(passwordExpected));
        }

        public async Task GetByIdAsync(string token, int passwordId, string masterPassword, string appExpected, string usernameExpected, string passwordExpected)
        {
            _client.SetToken(token);

            var response = await _api.GetByIdAsync(passwordId, masterPassword);

            Assert.That(response.Success, Is.EqualTo(true));
            Assert.That(response.Data.AppName, Is.EqualTo(appExpected));
            Assert.That(response.Data.AppUsername, Is.EqualTo(usernameExpected));
            Assert.That(response.Data.Password, Is.EqualTo(passwordExpected));
        }

        public async Task GetByAppAsync(string token, string passwordApp, string masterPassword, string appExpected, string usernameExpected, string passwordExpected)
        {
            _client.SetToken(token);

            var response = await _api.GetByAppAsync(passwordApp, masterPassword);

            Assert.That(response.Success, Is.EqualTo(true));
            Assert.That(response.Data.AppName, Is.EqualTo(appExpected));
            Assert.That(response.Data.AppUsername, Is.EqualTo(usernameExpected));
            Assert.That(response.Data.Password, Is.EqualTo(passwordExpected));
        }

        public async Task DeleteByIdAsync(string token, int passwordId)
        {
            _client.SetToken(token);

            var response = await _api.DeleteByIdAsync(passwordId);

            Assert.That(response.Success, Is.EqualTo(true));
        }

        public async Task DeleteByAppAsync(string token, string passwordApp)
        {
            _client.SetToken(token);

            var response = await _api.DeleteByAppAsync(passwordApp);

            Assert.That(response.Success, Is.EqualTo(true));
        }
    }
} 