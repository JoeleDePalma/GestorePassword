using CommunityToolkit.Mvvm.ComponentModel;
using GestorePassword.Core.Services;
using Libreria.API;
using Libreria.DTOs.Users;
using Libreria.HTTPRequestsLibrary;
using System.Threading.Tasks;

namespace GestorePassword.Core.ViewModels.User.Access
{
    public partial class SignUpViewModel : ObservableObject
    {
        public SignUpViewModel()
        {
        }

        public async Task<(bool, string?)> Register(string username, string password)
            => await UserRequest.RegisterAsync(username, password);
    }
}