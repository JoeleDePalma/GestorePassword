using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using GestorePassword.Core.ViewModels.User.Access;
using GestorePassword.UI.Desktop.Views.Menu;
using GestorePassword.UI.Desktop.Views.User.Access;
using System.IO.IsolatedStorage;
using System.Linq;

namespace GestorePassword.UI.Desktop.Views.User.Access
{
    public partial class SignUpView : UserControl
    {
        private SignUpViewModel _vm { get; set; }
        public SignUpView()
        {
            InitializeComponent();
            DataContext = new SignUpViewModel();
            _vm = (SignUpViewModel)DataContext;

            ShowPasswordButton.Click += ShowHidePassword!;
            HidePasswordButton.Click += ShowHidePassword!;
            GoToSignInViewButton.Click += GoToSignInView!;
            UsernameInputTextBox.TextChanged += ManageRegisterButton!;
            HiddenPasswordInput.TextChanged += ManageRegisterButton!;
            ShownPasswordInput.TextChanged += ManageRegisterButton!;
            SignUpButton.Click += Register!;
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

        public void ManageRegisterButton(object sender, RoutedEventArgs e)
        {
            var passwordInput = ShownPasswordInput.IsVisible ? ShownPasswordInput : HiddenPasswordInput;
            var username = UsernameInputTextBox.Text;
            var password = passwordInput.Text;

            if (username == null || password == null)
            {
                ErrorTextBlock.Text = "Completare i campi";
                SignUpButton.IsEnabled = false;
                ErrorTextBlock.IsVisible = true;
                return;
            }

            if (username!.Length < 4)
            {
                ErrorTextBlock.Text = "Il nome utente deve contenere\nalmeno 4 caratteri";
                SignUpButton.IsEnabled = false;
                ErrorTextBlock.IsVisible = true;
                return;
            }

            if (username.Length > 12)
            {
                ErrorTextBlock.Text = "Il nome utente deve contenere\nmassimo 12 caratteri";
                SignUpButton.IsEnabled = false;
                ErrorTextBlock.IsVisible = true;
                return;
            }

            if (username.Any(c => c == ' '))
            {
                ErrorTextBlock.Text = "Il nome utente non può contenere\nspazi vuoti";
                SignUpButton.IsEnabled = false;
                ErrorTextBlock.IsVisible = true;
                return;
            }

            if (username.Any(c => !char.IsLetterOrDigit(c)))
            {
                ErrorTextBlock.Text = "Il nome utente può contenere\nsolo lettere e numeri";
                SignUpButton.IsEnabled = false;
                ErrorTextBlock.IsVisible = true;
                return;
            }

            if (password.Length < 8)
            {
                ErrorTextBlock.Text = "La password deve contenere almeno 8 caratteri";
                SignUpButton.IsEnabled = false;
                ErrorTextBlock.IsVisible = true;
                return;
            }

            if (password.Any(c => c == ' '))
            {
                ErrorTextBlock.Text = "La password non può contenere\nspazi vuoti";
                SignUpButton.IsEnabled = false;
                ErrorTextBlock.IsVisible = true;
                return;
            }

            if (password.All(c => !char.IsUpper(c)))
            {
                ErrorTextBlock.Text = "La password deve contenere\nalmeno una lettera maiuscola";
                SignUpButton.IsEnabled = false;
                ErrorTextBlock.IsVisible = true;
                return;
            }

            if (password.All(c => !char.IsLower(c)))
            {
                ErrorTextBlock.Text = "La password deve contenere\nalmeno una lettera minuscola";
                SignUpButton.IsEnabled = false;
                ErrorTextBlock.IsVisible = true;
                return;
            }

            if (password.All(c => !char.IsDigit(c)))
            {
                ErrorTextBlock.Text = "La password deve contenere almeno un numero";
                SignUpButton.IsEnabled = false;
                ErrorTextBlock.IsVisible = true;
                return;
            }

            if (password.All(c => !char.IsPunctuation(c)))
            {
                ErrorTextBlock.Text = "La password deve contenere almeno\nun carattere speciale";
                SignUpButton.IsEnabled = false;
                ErrorTextBlock.IsVisible = true;
                return;
            }

            if (username == password)
            {
                ErrorTextBlock.Text = "Username e password non posso essere uguale";
                SignUpButton.IsEnabled = false;
                ErrorTextBlock.IsVisible = true;
                return;
            }

            ErrorTextBlock.IsVisible = false;
            SignUpButton.IsEnabled = true;
        }

        public async void Register(object sender, RoutedEventArgs e)
        {
            var passwordInput = ShownPasswordInput.IsVisible ? ShownPasswordInput : HiddenPasswordInput;

            ErrorTextBlock.IsVisible = false;
            LoadingGrid.IsVisible = true;

            bool success = default;
            string? errorString = default;

            try
            {
                (success, errorString) = await _vm.Register(UsernameInputTextBox.Text!, passwordInput.Text!);
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

        public void GoToSignInView(object sender, RoutedEventArgs e)
        {
            var main = (MainWindow)TopLevel.GetTopLevel(this)!;
            main.ChangeInterface(new SignInView());
        }
    }
}