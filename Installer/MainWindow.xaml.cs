using Installer.HTTPRequestsLibrary;
using Installer.API;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
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

namespace Installer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ApiClient _client { get; set; }
        private VersionApi _versionApi { get; set; }
        private bool _hasFinished = false;

        public MainWindow()
        {
            _client = new ApiClient("https://gestorepassword.fly.dev");
            _versionApi = new VersionApi(_client);

            InitializeComponent();
        }

        public async void UpdateApp(object sender, RoutedEventArgs e)
        {
            UpdateButton.Content = "Aggiornamento...";
            UpdateButton.IsEnabled = false;

            var isAppOpen = IsProcessOpen("GestorePassword");

            if (isAppOpen)
            {
                isAppOpen = WaitForProcessToClose("GestorePassword");
            }

            if (isAppOpen)
            {
                MessageBox.Show("L'applicazione è ancora aperta, di conseguenza non può essere aggiornata");
                _hasFinished = true;
                Application.Current.Shutdown();
                return;
            }

            var response = await _versionApi.GetLatestAppVersionZipAsync();

            if (!response.Success || response.Data is null)
            {
                MessageBox.Show("Impossibile scaricare il pacchetto di aggiornamento.");
                _hasFinished = true;
                Application.Current.Shutdown();
                return;
            }

            var zipBytes = response.Data.Content;
            var fileName = response.Data.FileName;

            var tempFolder = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "GestorePasswordUpdate");
            Directory.CreateDirectory(tempFolder);

            var zipPath = System.IO.Path.Combine(tempFolder, fileName);
            File.WriteAllBytes(zipPath, zipBytes);

            var appFolder = AppDomain.CurrentDomain.BaseDirectory;

            ZipFile.ExtractToDirectory(zipPath, appFolder, overwriteFiles: true);

            Process.Start(System.IO.Path.Combine(appFolder, "GestorePassword.exe"));
            _hasFinished = true;
            Application.Current.Shutdown();
        }

        bool IsProcessOpen(string processName)
            => Process.GetProcessesByName(processName).Length > 0;
        
        bool WaitForProcessToClose(string processName, int timeoutMs = 10000)
        {
            var sw = Stopwatch.StartNew();

            while (IsProcessOpen(processName))
            {
                if (sw.ElapsedMilliseconds > timeoutMs)
                    return false;

                Thread.Sleep(200);
            }

            return true;
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (!_hasFinished)
                e.Cancel = true;
        }
    }
}