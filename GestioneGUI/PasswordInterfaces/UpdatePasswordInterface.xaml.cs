using GestorePassword;
using HTTPRequestsLibrary;
using Libreria.API;
using Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GestioneGUI.PasswordInterfaces
{
    /// <summary>
    /// Logica di interazione per UpdatePassword.xaml
    /// </summary>
    public partial class UpdatePasswordInterface : UserControl
    {
        private ApiClient Client { get; set; }
        private UserApi userApi { get; set; }
        private PasswordApi passwordApi { get; set; }
        private UserInfo userInfo { get; set; }
        private PasswordInfo passwordDto { get; set; }

        public UpdatePasswordInterface(ApiClient Client, UserApi userApi, PasswordApi passwordApi, UserInfo userInfo, PasswordInfo passwordDto)
        {
            InitializeComponent();

            this.Client = Client;
            this.userApi = userApi;
            this.passwordApi = passwordApi;
            this.userInfo = userInfo;
            this.passwordDto = passwordDto;

            AppInput.Text = passwordDto.App;
            UsernameInput.Text = passwordDto.Username;
            ShownPasswordInput.Text = passwordDto.Password;
            HiddenPasswordInput.Password = passwordDto.Password;
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
            main.MainContent.Content = new MenuInterface(Client, userApi, passwordApi, userInfo);
        }

        private async void UpdatePassword(object seder, RoutedEventArgs e)
        {
            var app = AppInput.Text;
            var username = UsernameInput.Text;
            string password = default;

            bool isThereError = false;
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

            if (!isThereAppError && !string.IsNullOrWhiteSpace(AppErrorBlock.Text))
                AppErrorBlock.Text = "";

            if (!isTherePasswordError && !string.IsNullOrWhiteSpace(PasswordErrorBlock.Text))
                PasswordErrorBlock.Text = "";

            if (isThereAppError || isTherePasswordError)
            {
                return;
            }

            bool Success = default;
            int StatusCode = default;
            string? ErrorString = default;

            try
            {
                (Success, StatusCode, ErrorString) = await PasswordRequests.UpdatePasswordAsync(passwordApi, passwordDto.Id, app, username, password, userInfo.Password);
            }
            catch (Exception ex)
            {
                SetErrorBlock(PasswordErrorBlock, "Si è verificato un errore durante la richiesta", ref isThereError);
            }

            if (!Success)
                if (StatusCode == 404)
                    SetErrorBlock(PasswordErrorBlock, "Nessuna password corrispondente trovata", ref isThereError);

                else if (StatusCode == 401)
                    SetErrorBlock(PasswordErrorBlock, "Accesso non autorizzato rilevato", ref isThereError);

            if (isThereError)
            {
                return;
            }

            MessageBox.Show("Password aggiornata con successo!");
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
