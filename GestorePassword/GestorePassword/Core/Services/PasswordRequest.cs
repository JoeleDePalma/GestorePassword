using GestorePassword.Core.Models;
using Libreria.API;
using System;
using System.Collections.Generic;
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
    }
}
