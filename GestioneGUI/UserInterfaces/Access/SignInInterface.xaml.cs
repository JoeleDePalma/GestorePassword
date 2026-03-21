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
        private ResizeMode? _previousResizeMode;
        private double? _previousWidth;
        private double? _previousHeight;

        public SignInInterface()
        {
            InitializeComponent();
            this.Loaded += SignInInterface_Loaded;
            this.Unloaded += SignInInterface_Unloaded;
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

        private void SignInInterface_Loaded(object? sender, RoutedEventArgs e)
        {
            var main = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            if (main != null)
            {
                // save previous mode and disable resizing
                _previousResizeMode = main.ResizeMode;
                // save previous size
                _previousWidth = main.Width;
                _previousHeight = main.Height;
                main.ResizeMode = ResizeMode.NoResize;
                // set required size for sign in/up interfaces
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
                // restore previous size if available
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
    }
}
