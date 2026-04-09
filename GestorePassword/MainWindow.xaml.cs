using GestorePassword;
using GestorePassword.PasswordInterfaces;
using Libreria.API;
using Libreria.HTTPRequestsLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
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
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ApiClient client { get; set; }
        public UserApi userApi { get; set; }
        public PasswordApi passwordApi { get; set; }
        public VersionApi versionApi { get; set; }
        public UserInfo userInfo { get; set; }
        public Version appVersion { get; set; }

        public MainWindow()
        {
            appVersion = new("1.2.0");

            client = new ApiClient("https://gestorepassword.fly.dev");
            userApi = new UserApi(client);
            passwordApi = new PasswordApi(client);
            versionApi = new VersionApi(client);

            InitializeComponent();
            Loaded += LoadFirstInterface;
            SizeChanged += Window_SizeChanged;
        }

        private bool _isResizing = false;     
        private double aspectRatio = 800.0 / 500.0; 

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_isResizing) return;

            _isResizing = true;

            double newWidth = Width;
            double newHeight = Height;

            if (e.WidthChanged)
            {
                newHeight = newWidth / aspectRatio;

                if (newHeight < MinHeight)
                {
                    newHeight = MinHeight;
                    newWidth = newHeight * aspectRatio;
                }
            }
            else if (e.HeightChanged)
            {
                newWidth = newHeight * aspectRatio;

                if (newWidth < MinWidth)
                {
                    newWidth = MinWidth;
                    newHeight = newWidth / aspectRatio;
                }
            }

            Width = newWidth;
            Height = newHeight;

            _isResizing = false;
        }

        private async void LoadFirstInterface(object? sender, RoutedEventArgs e)
        {
            CheckingVersionTextBlock.Visibility = Visibility.Visible;

            try
            {
                client.SetVersion(appVersion);
                var response = await versionApi.CheckVersionAsync();

                CheckingVersionTextBlock.Visibility = Visibility.Collapsed;

                if (!response.Success)
                {
                    MessageBox.Show($"Errore durante il controllo della versione: {response.Message}");

                    string installerPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Installer.exe");

                    if (!File.Exists(installerPath))
                        throw new FileNotFoundException("Installer.exe non trovato", installerPath);

                    Process.Start(installerPath);
                    await Task.Delay(500);

                    Application.Current.Shutdown();
                    return;
                }

                MainContent.Content = new SignInInterface();
            }
            catch (HttpRequestException)
            {
                CheckingVersionTextBlock.Visibility = Visibility.Collapsed;

                MessageBox.Show(
                    "Impossibile verificare la versione dell'app.\n" +
                    "Controlla la connessione a Internet e riprova.",
                    "Errore di connessione",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );

                Application.Current.Shutdown();
            }
            catch (FileNotFoundException)
            {
                CheckingVersionTextBlock.Visibility = Visibility.Collapsed;

                MessageBox.Show(
                    "Installer.exe non trovato.\n" +
                    "Reinstalla l'applicazione.",
                    "File mancante",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );

                Application.Current.Shutdown();
            }
            catch (Win32Exception)
            {
                CheckingVersionTextBlock.Visibility = Visibility.Collapsed;

                MessageBox.Show(
                    "Impossibile avviare Installer.exe.\n" +
                    "Il file potrebbe essere corrotto.",
                    "Errore di avvio",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );

                Application.Current.Shutdown();
            }
        }
    }
}
