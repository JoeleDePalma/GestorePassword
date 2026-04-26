using GestorePassword.Core.Models;
using Libreria.API;
using Libreria.DTOs.Passwords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorePassword.Core.Services
{
    public class PasswordRequest
    {
        public static async Task SetAllPasswordsAsync()
        {
            try
            {
                var response = await AppServices.passwordApi.GetAllAsync(AppServices.currentUser.Password);

                if (!response.Success || response.Data == null) return;

                if (AppServices.passwordList == null)
                    AppServices.passwordList = new List<PasswordInfo>();

                AppServices.passwordList.Clear();

                foreach (var p in response.Data!)
                {
                    AppServices.passwordList.Add(new PasswordInfo()
                    {
                        Id = p.Id,
                        App = p.AppName,
                        Username = p.AppUsername,
                        Password = p.Password,
                        CreatedAt = p.CreatedAt,
                        LastUpdateAt = p.LastUpdateAt
                    });
                }
            }
            catch
            {
                return;
            }
        }

        public static async Task<(bool Success, string? ErrorString)> CreateNewPasswordAsync(string app, string? username, string password)
        {
            try
            {
                var newPassword = new CreatePasswordDTO()
                {
                    AppName = app,
                    AppUsername = username,
                    Password = password,
                    MasterPassword = AppServices.currentUser.Password
                };

                var response = await AppServices.passwordApi.CreateAsync(newPassword);

                if (response.Success)
                {
                    var savedPassword = new PasswordInfo()
                    {
                        Id = response.Data!.Id,
                        App = app,
                        Username = username,
                        Password = password,
                        CreatedAt = response.Data.CreatedAt,
                        LastUpdateAt = response.Data.LastUpdateAt
                    };

                    AppServices.passwordList.Add(savedPassword);

                    return (response.Success, null);
                }

                return (response.Success, response.Message);
            }
            catch (Exception ex)
            {
                return (false,  ex.Message);
            }
        }

        public static async Task<(bool Success, string? ErrorString)> ModifyPasswordAsync(int id, string app, string? username, string password)
        {
            try
            {
                var modifiedPassword = new UpdatePasswordDTO()
                {
                    AppName = app,
                    AppUsername = username,
                    Password = password,
                    MasterPassword = AppServices.currentUser.Password
                };

                var response = await AppServices.passwordApi.UpdateByIdAsync(id, modifiedPassword);

                if (response.Success)
                {
                    var savedPassword = new PasswordInfo()
                    {
                        Id = id,
                        App = app,
                        Username = username,
                        Password = response.Data!.Password,
                        CreatedAt = response.Data.CreatedAt,
                        LastUpdateAt = response.Data.LastUpdateAt
                    };

                    var index = AppServices.passwordList.FindIndex(p => int.Equals(p.Id, savedPassword.Id));

                    if (index != -1)
                    {
                        AppServices.passwordList[index] = savedPassword;
                        return (true, null);
                    }

                    AppServices.passwordList.Add(savedPassword);

                    return (true, null);
                }

                return (false, response.Message);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public static async Task<(bool Success, string? ErrorString)> DeletePasswordAsync(int id)
        {
            try
            {
                var response = await AppServices.passwordApi.DeleteByIdAsync(id);

                if (response.Success)
                {
                    var index = AppServices.passwordList.FindIndex(p => int.Equals(p.Id, id));

                    if (index != -1)
                        AppServices.passwordList.RemoveAt(index);
                    
                    return (true, null);
                }

                return (false, response.Message);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
