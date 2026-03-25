using Libreria.DTOs.Passwords;
using System;
using System.Collections.Generic;
using System.Text;
using Libreria.API;
using GestioneGUI;

namespace Services
{
    public class PasswordRequests
    {
        public static async Task<(bool, int, string?)> CreatePasswordAsync(PasswordApi Client, string app, string? username, string password, string masterPassword)
        {
            CreatePasswordDTO dto = new()
            {
                AppName = app,
                AppUsername = username,
                Password = password,
                MasterPassword = masterPassword
            };

            var response = await Client.CreateAsync(dto);

            return (response.Success, response.StatusCode, response.ErrorString);
        }

        public static async Task<(bool, int, string?)> UpdatePasswordAsync(PasswordApi Client, int id, string? app, string? username, string? password, string masterPassword)
        {
            UpdatePasswordDTO dto = new()
            {
                AppName = app,
                AppUsername = username,
                Password = password,
                MasterPassword = masterPassword
            };

            var response = await Client.UpdateByIdAsync(id, dto);

            return (response.Success, response.StatusCode, response.ErrorString);
        }

        public static async Task<(bool, int, string?)> DeletePasswordAsync(PasswordApi Client, int Id)
        {
            var response = await Client.DeleteByIdAsync(Id);

            return (response.Success, response.StatusCode, response.ErrorString);
        }
    }
}
