using Libreria.API;
using Libreria.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestorePassword.Core.Services
{
    public class UserRequest
    {
        public static async Task<(bool Success, string? ErrorString)> LoginAsync(string username, string password)
        {
            try
            {
                var dto = new LoginDTO()
                {
                    Username = username,
                    Password = password
                };

                var response = await AppServices.userApi.LoginAsync(dto);

                if (!response.Success)
                {
                    return (false, response.Message ?? null);
                }

                if (AppServices.currentUser == null)
                    AppServices.currentUser = new();

                AppServices.currentUser = new()
                {
                    UserID = response.Data!.UserID,
                    Username = response.Data.Username,
                    Password = password,
                    CreatedAt = response.Data.CreatedAt,
                    Token = response.Data.Token!
                };

                AppServices.apiClient.SetToken(response.Data.Token!);

                return (true, null);
            }
            catch
            {
                return (false, "Errore durante la richiesta");
            }
        }

        public static async Task<(bool, string?)> RegisterAsync(string username, string password)
        {
            try
            {
                var dto = new RegisterDTO()
                {
                    Username = username,
                    Password = password
                };

                var response = await AppServices.userApi.RegisterAsync(dto);

                if (!response.Success)
                {
                    return (false, response.Message ?? null);
                }

                var loginResponse = await LoginAsync(username, password);

                if (!loginResponse.Success)
                {
                    return (false, loginResponse.ErrorString);
                }

                return (true, null);
            }
            catch
            {
                return (false, "Errore durante la richiesta");
            }
        }
    }
}