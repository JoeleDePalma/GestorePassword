using GestorePassword;
using Libreria.HTTPRequestsLibrary;
using Libreria.API;
using Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace GestorePassword.PasswordInterfaces
{
    /// <summary>
    /// Logica di interazione per CreatePassword.xaml
    /// </summary>
    public partial class CreatePasswordInterface : UserControl
    {
        private ApiClient Client { get; set; }
        private UserApi userApi { get; set; }
        private PasswordApi passwordApi { get; set; }
        private UserInfo userInfo { get; set; }

        public CreatePasswordInterface()
        {
            InitializeComponent();
            var main = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            this.Client = main.client;
            this.userApi = main.userApi;
            this.passwordApi = main.passwordApi;
            this.userInfo = main.userInfo;
        }

        private void GeneratePassword_Click(object sender, RoutedEventArgs e)
        {
            string pwd = PasswordGenerator.GenerateSecurePassword();
            ShownPasswordInput.Text = pwd;
            HiddenPasswordInput.Password = pwd;
        }

        private void SwapPasswordMode(object sender, RoutedEventArgs e)
        {
            var hiddenPasswordString = HiddenPasswordInput.Password;
            var shownPasswordString = ShownPasswordInput.Text;

            if (HiddenPasswordInput.IsVisible)
            {
                if (hiddenPasswordString != shownPasswordString)
                    ShownPasswordInput.Text = HiddenPasswordInput.Password;

                HiddenPasswordInput.Visibility = Visibility.Collapsed;
                ShownPasswordInput.Visibility = Visibility.Visible;

                EyeImage.Source = EyeImage.Source = new BitmapImage(new Uri("/Images/opened_eye.png", UriKind.Relative));
            }

            else
            {
                if (hiddenPasswordString != shownPasswordString)
                    HiddenPasswordInput.Password = ShownPasswordInput.Text;

                HiddenPasswordInput.Visibility = Visibility.Visible;
                ShownPasswordInput.Visibility = Visibility.Collapsed;

                EyeImage.Source = EyeImage.Source = new BitmapImage(new Uri("/Images/closed_eye.png", UriKind.Relative));
            }
        }

        private void BackToMenu(object sender, RoutedEventArgs e)
        {
            var main = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            main.MainContent.Content = new MenuInterface();
        }

        private async void SavePassword(object sender, RoutedEventArgs e)
        {
            var app = AppInput.Text;
            var username = UsernameInput.Text;
            string password = default;

            bool isThereRequestError = false;
            bool isThereAppError = false;
            bool isTherePasswordError = false;

            if (ShownPasswordInput.IsVisible)
                password = ShownPasswordInput.Text;
            else
                password = HiddenPasswordInput.Password;

            if (string.IsNullOrWhiteSpace(app))
                SetErrorBlock(AppErrorBlock, "Completare il campo obbligatorio*", ref isThereAppError);

            if (string.IsNullOrWhiteSpace(password))
                SetErrorBlock(PasswordErrorBlock, "Completare il campo obbligatorio*", ref isTherePasswordError);

            if (password.All(c => c == '*'))
                SetErrorBlock(PasswordErrorBlock, "La password non può contenere solo asterischi", ref isTherePasswordError);

            if (!isThereAppError && !string.IsNullOrWhiteSpace(AppErrorBlock.Text))
                AppErrorBlock.Text = "";

            if (!isTherePasswordError && !string.IsNullOrWhiteSpace(PasswordErrorBlock.Text))
                PasswordErrorBlock.Text = "";

            if (isThereAppError || isTherePasswordError)
            {
                return;
            }

            bool Success = default;
            string? errorString = default;

            try
            {
                LoadingTextBlock.Visibility = Visibility.Visible;
                GeneratePasswordButton.IsEnabled = false;
                SavePasswordButton.IsEnabled = false;
                GoBackToMenuButton.IsEnabled = false;

                (Success, errorString) = await PasswordRequests.CreatePasswordAsync(passwordApi, app, username, password, userInfo.Password);
            }
            catch (Exception ex)
            {
                SetErrorBlock(PasswordErrorBlock, "Si è verificato un errore durante la richiesta", ref isThereRequestError);
            }

            if (!Success)
                SetErrorBlock(AppErrorBlock, errorString, ref isThereRequestError);

            if (isThereRequestError)
            {
                LoadingTextBlock.Visibility = Visibility.Collapsed;
                GeneratePasswordButton.IsEnabled = true;
                SavePasswordButton.IsEnabled = true;
                GoBackToMenuButton.IsEnabled = true;
                return;
            }

            MessageBox.Show("Nuova password salvata con successo!");
            BackToMenu(this, new RoutedEventArgs());
        }

        private void SetErrorBlock(TextBlock errorBlock, string error, ref bool isThereError)
        {
            isThereError = true;

            errorBlock.Text = error;

            if (!errorBlock.IsVisible)
                errorBlock.Visibility = Visibility.Visible;
        }
    }
}
