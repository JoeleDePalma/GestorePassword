using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
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
  Sei già registrato?
";
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

