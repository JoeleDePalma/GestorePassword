using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using GestioneGUI;
using HTTPRequestsLibrary;
using Libreria.API;
using Libreria.DTOs.Users;
using Services;

namespace GestorePassword
{
    /// <summary>
    /// Logica di interazione per SignInInterface.xaml
    /// </summary>
    public partial class SignInInterface : UserControl
    {
        private ResizeMode? _previousResizeMode;
        private double? _previousWidth;
        private double? _previousHeight;

        private ApiClient Client { get; set; }
        public UserApi userApi { get; set; }
        public PasswordApi passwordApi { get; set; }

        public SignInInterface(ApiClient Client, UserApi userApi, PasswordApi passwordApi)
        {
            InitializeComponent();
            this.Loaded += SignInInterface_Loaded;
            this.Unloaded += SignInInterface_Unloaded;
            this.Client = Client;
            this.userApi = userApi;
            this.passwordApi = passwordApi;
        }

        public void SwapToSignUpInterface(object sender, RoutedEventArgs e)
        {
            var main = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            if (main != null)
            {
                main.MainContent.Content = new SignUpInterface(Client, userApi, passwordApi);
            }
        }

        public void SwapPasswordStackPanel(object sender, RoutedEventArgs e)
        {
            if (PasswordBox_Input.IsVisible)
            {
                TextBoxPasswordInput.Text = PasswordBox_Input.Password;

                PasswordBox_Input.Visibility = Visibility.Collapsed;
                TextBoxPasswordInput.Visibility = Visibility.Visible;

                Image_PasswordState.Source = new BitmapImage(new Uri("/Images/opened_eye.png", UriKind.Relative));
            }

            else
            {
                PasswordBox_Input.Password = TextBoxPasswordInput.Text;

                PasswordBox_Input.Visibility = Visibility.Visible;
                TextBoxPasswordInput.Visibility = Visibility.Collapsed;

                Image_PasswordState.Source = new BitmapImage(new Uri("/Images/closed_eye.png", UriKind.Relative));
            }
        }

        private void SignInInterface_Loaded(object? sender, RoutedEventArgs e)
        {
            var main = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            if (main != null)
            {

                _previousResizeMode = main.ResizeMode;

                _previousWidth = main.Width;
                _previousHeight = main.Height;
                main.ResizeMode = ResizeMode.NoResize;

                main.Width = 800;
                main.Height = 500;
            }
        }

        private void SignInInterface_Unloaded(object? sender, RoutedEventArgs e)
        {
            var main = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            if (main != null && _previousResizeMode.HasValue)
            {
                main.ResizeMode = _previousResizeMode.Value;
                _previousResizeMode = null;

                if (_previousWidth.HasValue)
                {
                    main.Width = _previousWidth.Value;
                    _previousWidth = null;
                }
                if (_previousHeight.HasValue)
                {
                    main.Height = _previousHeight.Value;
                    _previousHeight = null;
                }
            }
        }

        private async void Login(object? sender, RoutedEventArgs e)
        {
            var username = TextBoxUsernameInput.Text;
            var password = TextBoxPasswordInput.Text;

            bool isThereUsernameError = false;
            bool isTherePasswordError = false;

            if (string.IsNullOrWhiteSpace(username))
                SetErrorBlock(UsernameErrorBlock, "Completare il campo", ref isThereUsernameError);

            if (string.IsNullOrWhiteSpace(password))
                SetErrorBlock(PasswordErrorBlock, "Completare il campo", ref isTherePasswordError);

            if (!isThereUsernameError && UsernameErrorBlock.IsVisible)
                UsernameErrorBlock.Visibility = Visibility.Collapsed;

            if (!isTherePasswordError && PasswordErrorBlock.IsVisible)
                PasswordErrorBlock.Visibility = Visibility.Collapsed;

            if (isThereUsernameError || isTherePasswordError)
            {
                return;
            }

            bool Success = default;
            LoginResponseDTO? response = default;
            int StatusCode = default;
            string? errorString = default;

            try
            {
                (Success, response, StatusCode, errorString) = await Access.Login(userApi, username, password);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errore durante la richiesta" + ex);
            }

            var info = new UserInfo()
            {
                UserID = response.UserID,
                Username = response.Username,
                CreatedAt = response.CreatedAt,
                Token = response.Token
            };

            var main = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            main.MainContent.Content = new MenuInterface(Client, userApi, passwordApi, info);
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
