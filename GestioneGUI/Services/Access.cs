using Libreria.DTOs.Passwords;
using System;
using System.Collections.Generic;
using System.Text;
using HTTPRequestsLibrary;
using Libreria.API;
using Libreria.DTOs.Users;

namespace Services
{
    public class Access
    {
        public static async Task<(bool, UserResponseDTO?, int, string?)> CreateUser(UserApi Client, string username, string password)
        {
            var dto = new RegisterDTO()
            {
                Username = username,
                Password = password
            };

            var response = await Client.RegisterAsync(dto);

            if (!response.Success)
            {
                return (false, null, response.StatusCode, response.ErrorString);
            }

            var infoDto = new UserResponseDTO()
            {
                UserID = response.Data.UserID,
                Username = username,
                CreatedAt = response.Data.CreatedAt
            };

            return (true, infoDto, response.StatusCode, null);
        }

        public static async Task<(bool, LoginResponseDTO?, int, string?)> Login(UserApi Client, string username, string password)
        {
            var dto = new LoginDTO()
            {
                Username = username,
                Password = password
            };

            var response = await Client.LoginAsync(dto);

            if (!response.Success)
            {
                return (false, null, response.StatusCode, response.ErrorString);
            }

            var infoDto = new LoginResponseDTO()
            {
                UserID = response.Data.UserID,
                Username = response.Data.Username,
                CreatedAt = response.Data.CreatedAt,
                Token = response.Data.Token
            };

            return (true, infoDto, response.StatusCode, null);
        }
    }
}
