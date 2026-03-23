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
    public partial class UpdatePassword : UserControl
    {
        private ApiClient Client { get; set; }
        private UserApi userApi { get; set; }
        private PasswordApi passwordApi { get; set; }
        private UserInfo userInfo { get; set; }

        public UpdatePassword(ApiClient Client, UserApi userApi, PasswordApi passwordApi, UserInfo userInfo)
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
    }
}
