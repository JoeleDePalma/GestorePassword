using Libreria.DTOs.Passwords;
using System;
using System.Collections.Generic;
using System.Text;
using Libreria.API;
using GestorePassword;

namespace Services
{
    public class PasswordRequests
    {
        public static async Task<(bool, string?)> CreatePasswordAsync(PasswordApi Client, string app, string? username, string password, string masterPassword)
        {
            CreatePasswordDTO dto = new()
            {
                AppName = app,
                AppUsername = username,
                Password = password,
                MasterPassword = masterPassword
            };

            var response = await Client.CreateAsync(dto);

            return (response.Success, response.Message ?? null);
        }

        public static async Task<(bool, string?)> UpdatePasswordAsync(PasswordApi Client, int id, string? app, string? username, string? password, string masterPassword)
        {
            UpdatePasswordDTO dto = new()
            {
                AppName = app,
                AppUsername = username,
                Password = password,
                MasterPassword = masterPassword
            };

            var response = await Client.UpdateByIdAsync(id, dto);

            return (response.Success, response.Message);
        }

        public static async Task<(bool, string?)> DeletePasswordAsync(PasswordApi Client, int Id)
        {
            var response = await Client.DeleteByIdAsync(Id);

            return (response.Success, response.Message);
        }
    }
}
