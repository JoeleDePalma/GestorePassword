using CommunityToolkit.Mvvm.ComponentModel;
using Libreria.API;
using Libreria.HTTPRequestsLibrary;

namespace GestorePassword.Core.ViewModels.User.Access
{
    public partial class SignUpViewModel : ObservableObject
    {
        private readonly ApiClient _apiClient;
        private readonly UserApi _userApi;

        public SignUpViewModel()
        {
            _apiClient = AppServices.apiClient;
            _userApi = AppServices.userApi;
        }
    }
}