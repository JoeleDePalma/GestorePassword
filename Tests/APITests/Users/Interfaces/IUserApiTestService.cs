using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.APITests.Users.Interfaces
{
    public interface IUserApiTestService
    {
        public Task RegisterAsync(string username, string password);

        public Task<string> LoginAsync(string username, string password);

        public Task UpdateAsync(string newUsername, string newPassword, string token);

        public Task<int> GetByTokenAsync(string token, string usernameExpected);

        public Task DeleteByTokenAsync(string token);
    }
}
