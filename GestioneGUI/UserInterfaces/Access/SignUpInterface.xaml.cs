using GestioneGUI;
using HTTPRequestsLibrary;
using Libreria.API;
using Libreria.DTOs.Users;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GestorePassword
{
    /// <summary>
    /// Logica di interazione per SignUpInterface.xaml
    /// </summary>
    public partial class SignUpInterface : UserControl
    {
        private ResizeMode? _previousResizeMode;
        private double? _previousWidth;
        private double? _previousHeight;

        private ApiClient Client { get; set; }
        public UserApi userApi { get; set; }
        public PasswordApi passwordApi { get; set; }

        public SignUpInterface(ApiClient Client, UserApi userApi, PasswordApi passwordApi)
        {
            this.Client = Client;
            this.userApi = userApi;
            this.passwordApi = passwordApi;

            InitializeComponent();
            Welcome_Text.Text = @"
                        Benvenuto in Password Manager!
                Crea il tuo account per iniziare a gestire 
            le tue password in modo sicuro e semplice.
        Password Manager ti offre un modo affidabile
      per conservare tutte le tue credenziali in un unico posto, 
    protette da una crittografia avanzata dei dati,
   inoltre offre un'interfaccia semplice e chiara, 
  per permettere a tutti di comprendere 
  e sfruttare tutte le funzionalità presenti!
  Inizia subito creando il tuo account!
";
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            this.Loaded += SignUpInterface_Loaded;
            this.Unloaded += SignUpInterface_Unloaded;
        }

        public void SwapPasswordStackPanel(object sender, RoutedEventArgs e)
        {
            if (PasswordBoxPasswordInput.Visibility == Visibility.Visible)
            {
                TextBoxPasswordInput.Text = PasswordBoxPasswordInput.Password;

                PasswordBoxPasswordInput.Visibility = Visibility.Collapsed;
                TextBoxPasswordInput.Visibility = Visibility.Visible;

                Image_PasswordState.Source = new BitmapImage(new Uri("/Images/opened_eye.png", UriKind.Relative));
            }

            else
            {
                PasswordBoxPasswordInput.Password = TextBoxPasswordInput.Text;

                PasswordBoxPasswordInput.Visibility = Visibility.Visible;
                TextBoxPasswordInput.Visibility = Visibility.Collapsed;

                Image_PasswordState.Source = new BitmapImage(new Uri("/Images/closed_eye.png", UriKind.Relative));

            }
        }

        public void SwapToSignInInterface(object sender, RoutedEventArgs e)
        {
            var main = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            main.MainContent.Content = new SignInInterface(Client, userApi, passwordApi);
        }   

        private void SignUpInterface_Loaded(object? sender, RoutedEventArgs e)
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

        private void SignUpInterface_Unloaded(object? sender, RoutedEventArgs e)
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

        private async void CreateAccount(object? sender, RoutedEventArgs e)
        {
            var username = TextBoxUsernameInput.Text;
            string password;

            if (TextBoxPasswordInput.IsVisible)
                password = TextBoxPasswordInput.Text;
            else
                password = PasswordBoxPasswordInput.Password;

            bool isThereUsernameError = false;
            bool isTherePasswordError = false;
            bool isThereServerError = false;

            if (string.IsNullOrWhiteSpace(username))
            {
                SetErrorBlock(UsernameErrorBlock, "Completare il campo", ref isThereUsernameError);
            }

            if (username.Length < 4)
            {
                SetErrorBlock(UsernameErrorBlock, "Il nome utente deve contenere\nalmeno 4 caratteri", ref isThereUsernameError);
            }

            else if (username.Length > 10)
            {
                SetErrorBlock(UsernameErrorBlock, "Il nome utente può contenere\nfino a 10 caratteri", ref isThereUsernameError);
            }

            else if (username.Any(c => !char.IsLetterOrDigit(c)))
            {
                SetErrorBlock(UsernameErrorBlock, "Il nome utente può contenere\nsolo caratteri alfanumerici", ref isThereUsernameError);
            }

            bool hasLetter = password.Any(char.IsLetter);
            bool hasDigit = password.Any(char.IsDigit);
            bool hasSpecial = password.Any(c => !char.IsLetterOrDigit(c));

            if (string.IsNullOrWhiteSpace(password))
            {
                SetErrorBlock(PasswordErrorBlock, "Completare il campo", ref isTherePasswordError);
            }

            if (password.Length < 8)
            {
                SetErrorBlock(PasswordErrorBlock, "La password deve contenere\nalmeno 8 caratteri", ref isTherePasswordError);
            }

            else if (password.Length > 50)
            {
                SetErrorBlock(PasswordErrorBlock, "La password può contenere\nfino a 50 caratteri", ref isTherePasswordError);
            }

            else if (!(hasLetter && hasDigit && hasSpecial))
            {
                SetErrorBlock(PasswordErrorBlock,
                    "La password deve contenere\nlettere, numeri e caratteri speciali",
                    ref isTherePasswordError);
            }

            if (!isThereUsernameError && UsernameErrorBlock.IsVisible)
                UsernameErrorBlock.Visibility = Visibility.Collapsed;

            if (!isTherePasswordError && PasswordErrorBlock.IsVisible)
                PasswordErrorBlock.Visibility = Visibility.Collapsed;

            if (isTherePasswordError || isThereUsernameError)
            {
                return;
            }

            bool success = default;
            UserResponseDTO registerDto = default;
            LoginResponseDTO loginDto = default;
            int statusCode = default;
            string? errorString = default;

            try
            {
                (success, registerDto, statusCode, errorString) = await AccessRequests.CreateUser(userApi, username, password);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Errore durante la richiesta" + ex.Message);
            }

            if (!success)
                if (statusCode == 400)
                    SetErrorBlock(UsernameErrorBlock, "Nome utente già esistente", ref isThereUsernameError);
                else
                    SetErrorBlock(PasswordErrorBlock, "Errore interno del server", ref isThereServerError);

            else
            {
                try
                {
                    (success, loginDto, statusCode, errorString) = await AccessRequests.Login(userApi, username, password);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Errore durante la richiesta" + ex.Message);
                    return;
                }

                UserInfo info = new UserInfo()
                {
                    UserID = loginDto.UserID,
                    Username = loginDto.Username,
                    Password = password,
                    CreatedAt = loginDto.CreatedAt,
                    Token = loginDto.Token
                };

                var main = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                main.MainContent.Content = new MenuInterface(Client, userApi, passwordApi, info);
            }
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

