using CommunityToolkit.Mvvm.ComponentModel;
using Libreria.API;
using Libreria.DTOs.Users;
using Libreria.HTTPRequestsLibrary;
using Libreria.HTTPRequestsLibrary.Services;
using System.Threading.Tasks;
using GestorePassword.Core.Services;

namespace GestorePassword.Core.ViewModels.User.Access
{
    public partial class SignInViewModel : ObservableObject
    {
        public SignInViewModel()
        {
        }

        public async Task<(bool, string?)> Login(string username, string password)
            => await UserRequest.LoginAsync(username, password);
    }
}