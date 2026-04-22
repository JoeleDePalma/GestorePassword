using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using GestorePassword.Core.ViewModels.User.Access;
using GestorePassword.UI.Desktop.Views.Menu;
using System;
using System.Diagnostics;

namespace GestorePassword.UI.Desktop.Views.User.Access
{
    public partial class SignInView : UserControl
    {
        private SignInViewModel _vm { get; set; }
        public SignInView()
        {
            InitializeComponent();
            DataContext = new SignInViewModel();
            _vm = (SignInViewModel)DataContext;

            ShowPasswordButton.Click += ShowHidePassword!;
            HidePasswordButton.Click += ShowHidePassword!;
            GoToSignUpViewButton.Click += GoToSignUpView!;
            UsernameInputTextBox.TextChanged += ManageLoginButton!;
            HiddenPasswordInput.TextChanged += ManageLoginButton!;
            ShownPasswordInput.TextChanged += ManageLoginButton!;
            SignInButton.Click += Login!;
        }

        public void ShowHidePassword(object sender, RoutedEventArgs e)
        {
            if (ShownPasswordInput.IsVisible)
            {
                HiddenPasswordInput.Text = ShownPasswordInput.Text;
                ShownPasswordInput.IsVisible = false;
                HiddenPasswordInput.IsVisible = true;
                ShowPasswordButton.IsVisible = true;
                HidePasswordButton.IsVisible = false;
            }
            else
            {
                ShownPasswordInput.Text = HiddenPasswordInput.Text;
                HiddenPasswordInput.IsVisible = false;
                ShownPasswordInput.IsVisible = true;
                ShowPasswordButton.IsVisible = false;
                HidePasswordButton.IsVisible = true;
            }
        }

        public void ManageLoginButton(object sender, RoutedEventArgs e)
        {
            var passwordInput = ShownPasswordInput.IsVisible ? ShownPasswordInput : HiddenPasswordInput;

            if (passwordInput.Text == null || UsernameInputTextBox.Text == null)
            {
                SignInButton.IsEnabled = false;
                return;
            }

            SignInButton.IsEnabled = true;
        }

        public async void Login(object sender, RoutedEventArgs e)
        {
            var passwordInput = ShownPasswordInput.IsVisible ? ShownPasswordInput : HiddenPasswordInput;

            ErrorTextBlock.IsVisible = false;
            LoadingGrid.IsVisible = true;

            bool success = default;
            string? errorString = default;

            try
            {
                (success, errorString) = await _vm.Login(UsernameInputTextBox.Text!, passwordInput.Text!);
            }
            catch 
            {
                ErrorTextBlock.Text = "Errore durante la richiesta";
                ErrorTextBlock.IsVisible = true;
                LoadingGrid.IsVisible = false;
                return; 
            }

            if (success)
            {
                var main = (MainWindow)TopLevel.GetTopLevel(this)!;
                main.ChangeInterface(new MenuView());
                return;
            }

            ErrorTextBlock.Text = errorString;
            ErrorTextBlock.IsVisible = true;
            LoadingGrid.IsVisible = false;
        }


        public void GoToSignUpView(object sender, RoutedEventArgs e)
        {
            var main = (MainWindow)TopLevel.GetTopLevel(this)!;
            main.ChangeInterface(new SignUpView());
        }
    }
}