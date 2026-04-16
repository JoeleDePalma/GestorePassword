using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;
using Libreria.API;

namespace GestorePassword.Core.ViewModels.Menu
{
    public partial class MenuViewModel : ObservableObject
    {
        private readonly UserApi _userApi;
        private readonly PasswordApi _passwordApi;

        public MenuViewModel()
        {
            _userApi = AppServices.userApi;
            _passwordApi = AppServices.passwordApi;
        }
    }
}
