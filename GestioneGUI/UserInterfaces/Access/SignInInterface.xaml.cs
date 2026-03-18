using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace GestorePassword
{
    /// <summary>
    /// Logica di interazione per SignInInterface.xaml
    /// </summary>
    public partial class SignInInterface : UserControl
    {
        public SignInInterface()
        {
            InitializeComponent();
        }

        public void SwapToSignUpInterface(object sender, RoutedEventArgs e)
        {
            var main = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            if (main != null)
            {
                main.MainContent.Content = new SignUpInterface();
            }
        }

        public void SwapPasswordStackPanel(object sender, RoutedEventArgs e)
        {
            if (PasswordBox_Input.Visibility == Visibility.Visible)
            {
                TextBox_Input.Text = PasswordBox_Input.Password;

                PasswordBox_Input.Visibility = Visibility.Collapsed;
                TextBox_Input.Visibility = Visibility.Visible;

                Image_PasswordState.Source = new BitmapImage(new Uri("/Images/opened_eye.png", UriKind.Relative));
            }

            else
            {
                PasswordBox_Input.Password = TextBox_Input.Text;

                PasswordBox_Input.Visibility = Visibility.Visible;
                TextBox_Input.Visibility = Visibility.Collapsed;

                Image_PasswordState.Source = new BitmapImage(new Uri("/Images/closed_eye.png", UriKind.Relative));

            }
        }
    }
}