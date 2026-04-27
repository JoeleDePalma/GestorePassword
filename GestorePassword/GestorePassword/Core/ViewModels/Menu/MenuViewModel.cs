using CommunityToolkit.Mvvm.ComponentModel;
using GestorePassword.Core.Models;
using GestorePassword.Core.Services;
using Libreria.API;
using Libreria.DTOs.Passwords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GestorePassword.Core.ViewModels.Menu
{
    public partial class MenuViewModel : ObservableObject
    {

        public MenuViewModel()
        {
        }

        public 
        (
            int strongPasswords, 
            int weakPasswords, 
            string averagePasswordStrength, 
            int savedUsernameCount,
            DateTime lastPasswordCreatedAt
        ) GetPasswordsStatistics()
        {
            int strongPasswords;
            int weakPasswords;
            string averagePasswordStrength;
            int savedUsernameCount;
            DateTime lastPasswordCreatedAt;

            if (AppServices.passwordList != null && AppServices.passwordList.Count > 0)
            {
                strongPasswords = AppServices.passwordList
                .Where(p =>
                    p.Password.Length > 12 &&
                    p.Password.Any(char.IsDigit) &&
                    p.Password.Any(char.IsLetter) &&
                    p.Password.Any(char.IsLower) &&
                    p.Password.Any(char.IsUpper) &&
                    p.Password.Any(char.IsPunctuation)
                    ).Count();

                weakPasswords = AppServices.passwordList
                    .Where(p =>
                        !(
                        p.Password.Length > 12 &&
                        p.Password.Any(char.IsDigit) &&
                        p.Password.Any(char.IsLetter) &&
                        p.Password.Any(char.IsLower) &&
                        p.Password.Any(char.IsUpper) &&
                        p.Password.Any(char.IsPunctuation))
                        ).Count();

                averagePasswordStrength =
                    strongPasswords > weakPasswords ? "Forte" :
                    strongPasswords < weakPasswords ? "Debole" :
                    "Media";

                savedUsernameCount = AppServices.passwordList
                    .Where(p =>
                        !string.IsNullOrWhiteSpace(p.Username))
                            .Count();

                lastPasswordCreatedAt = AppServices.passwordList
                        .Max(p =>
                            p.CreatedAt);
            }
            else
            {
                strongPasswords = 0;
                weakPasswords = 0;
                averagePasswordStrength = "Nessuna";
                savedUsernameCount = 0;
                lastPasswordCreatedAt = DateTime.MinValue;
            }

            return (strongPasswords, weakPasswords, averagePasswordStrength, savedUsernameCount, lastPasswordCreatedAt);
        }

        public async Task SetAllPassword()
            => await PasswordRequest.SetAllPasswordsAsync();

        public string GeneratePassowrd(int length = 16)
            => PasswordGenerator.GeneratePassword(length);

        public async Task<(bool Success, string? ErrorString)> SaveNewPassword(string app, string? username, string password)
            => await PasswordRequest.CreateNewPasswordAsync(app, username, password);

        public async Task<(bool Success, string? ErrorString)> ModifyPassword(int id, string app, string? username, string password)
            => await PasswordRequest.ModifyPasswordAsync(id, app, username, password);

        public async Task<(bool Success, string? ErrorString)> DeletePassword(int id)
            => await PasswordRequest.DeletePasswordAsync(id);
    }
}
