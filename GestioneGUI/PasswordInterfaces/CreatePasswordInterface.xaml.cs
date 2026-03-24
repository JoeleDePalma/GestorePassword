using GestorePassword;
using HTTPRequestsLibrary;
using Libreria.API;
using Libreria.DTOs.Passwords;
using Libreria.DTOs.Users;
using Services;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GestioneGUI.PasswordInterfaces
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

        public CreatePasswordInterface(ApiClient Client, UserApi userApi, PasswordApi passwordApi, UserInfo userInfo)
        {
            InitializeComponent();
            this.Client = Client;
            this.userApi = userApi;
            this.passwordApi = passwordApi;
            this.userInfo = userInfo;
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

        private async void SavePassword(object sender, RoutedEventArgs e)
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
                (Success, StatusCode, ErrorString) = await PasswordRequests.CreatePasswordAsync(passwordApi, app, username, password, userInfo.Password);
            }
            catch (Exception ex)
            {
                SetErrorBlock(PasswordErrorBlock, "Si è verificato un errore durante la richiesta", ref isThereError);
            }

            if (!Success)
                if (StatusCode == 400)
                    SetErrorBlock(AppErrorBlock, "Hai già salvato una password di quest'app", ref isThereError);

            if (isThereError)
            {
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
