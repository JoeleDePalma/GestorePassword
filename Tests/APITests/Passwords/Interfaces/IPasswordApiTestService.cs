using Libreria.DTOs.Passwords;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.APITests.Passwords.Interfaces
{
    public interface IPasswordApiTestService
    {
        public Task<int> CreateAsync(string token, CreatePasswordDTO NewPassword);
        public Task UpdateByIdAsync(string token, UpdatePasswordDTO UpdatedPassword, int passwordId);
        public Task UpdateByAppAsync(string token, UpdatePasswordDTO UpdatedPassword, string passwordApp);
        public Task GetAllAsync(string token, string masterPassword, string appExpected, string usernameExpected, string passwordExpected);
        public Task GetByIdAsync(string token, int passwordId, string masterPassword, string appExpected, string usernameExpected, string passwordExpected);
        public Task GetByAppAsync(string token, string passwordApp, string masterPassword, string appExpected, string usernameExpected, string passwordExpected);
        public Task DeleteByIdAsync(string token, int passwordId);
        public Task DeleteByAppAsync(string token, string passwordApp);
    }
}
