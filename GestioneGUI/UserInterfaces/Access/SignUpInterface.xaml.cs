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

        public SignUpInterface()
        {
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

        public void SwapToSignInInterface(object sender, RoutedEventArgs e)
        {
            // Find the MainWindow and set its MainContent to SignInInterface
            var main = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            main.MainContent.Content = new SignInInterface();
        }   

        private void SignUpInterface_Loaded(object? sender, RoutedEventArgs e)
        {
            var main = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            if (main != null)
            {
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

        private void SignUpInterface_Unloaded(object? sender, RoutedEventArgs e)
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

