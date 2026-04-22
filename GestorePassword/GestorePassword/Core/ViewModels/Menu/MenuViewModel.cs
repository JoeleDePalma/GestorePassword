using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;
using Libreria.API;
using System.Linq;
using GestorePassword.Core.Models;
using System.Threading.Tasks;
using GestorePassword.Core.Services;

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
            int strongPasswords = AppServices.passwordList
                .Where(p =>
                    p.Password.Length > 12 &&
                    p.Password.Any(char.IsDigit) &&
                    p.Password.Any(char.IsLetter) &&
                    p.Password.Any(char.IsLower) &&
                    p.Password.Any(char.IsUpper) &&
                    p.Password.Any(char.IsPunctuation)
                    ).Count();

            int weakPasswords = AppServices.passwordList
                .Where(p =>
                    !(
                    p.Password.Length > 12 &&
                    p.Password.Any(char.IsDigit) &&
                    p.Password.Any(char.IsLetter) &&
                    p.Password.Any(char.IsLower) &&
                    p.Password.Any(char.IsUpper) &&
                    p.Password.Any(char.IsPunctuation))
                    ).Count();

            string averagePasswordStrength =
                strongPasswords > weakPasswords ? "Forte" :
                strongPasswords < weakPasswords ? "Debole" :
                "Media";

            int savedUsernameCount = AppServices.passwordList
                .Where(p => 
                    !string.IsNullOrWhiteSpace(p.Username))
                        .Count();

            DateTime lastPasswordCreatedAt = AppServices.passwordList
                    .Max(p =>
                        p.CreatedAt);

            return (strongPasswords, weakPasswords, averagePasswordStrength, savedUsernameCount, lastPasswordCreatedAt);
        }

        public async Task SetAllPassword()
            => await PasswordRequest.SetAllPasswordsAsync();
    }
}
