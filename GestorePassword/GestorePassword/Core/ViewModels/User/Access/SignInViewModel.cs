using CommunityToolkit.Mvvm.ComponentModel;
using Libreria.API;

namespace GestorePassword.Core.ViewModels.User.Access;

public partial class SignInViewModel : ObservableObject
{
    private readonly UserApi _userApi;

    public SignInViewModel()
    {
        _userApi = AppServices.userApi;
    }
}
