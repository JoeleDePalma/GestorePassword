using GestorePassword;
using Libreria.API;
using Services;
using System.Windows;

namespace GestioneGUI.PasswordInterfaces
{
    public partial class DeletePasswordWindow : Window
    {
        private bool HasFinished { get; set; } = false;
        public bool IsThereError { get; private set; } = false;
        private PasswordApi passwordApi { get; set; }
        private PasswordInfo passwordInfo { get; set; }

        public DeletePasswordWindow(PasswordInfo passwordInfo)
        {
            InitializeComponent();

            var main = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            passwordApi = main.passwordApi;
            this.passwordInfo = passwordInfo;
        }

        private async void DeletePassword(object sender, RoutedEventArgs e)
        {
            bool Success = default;
            int StatusCode = default;
            string? ErrorString = default;

            try
            {
                DecisionStackPanel.Visibility = Visibility.Collapsed;
                LoadingTextBlock.Visibility = Visibility.Visible;
                (Success, StatusCode, ErrorString) = await PasswordRequests.DeletePasswordAsync(passwordApi, passwordInfo.Id);
            }
            catch
            {
                MessageBox.Show("Errore durante la richiesta");
                IsThereError = true;
                DialogResult = IsThereError;
                GoBack(new(), new RoutedEventArgs());
                return;
            }

            if (!Success)
            {
                if (StatusCode == 404)
                {
                    MessageBox.Show("Nessuna password trovata");
                    IsThereError = true;
                }
                else if (StatusCode == 401)
                {
                    MessageBox.Show("Accesso non autorizzato");
                    IsThereError = true;
                }
            }

            DialogResult = IsThereError;
            GoBack(new(), new RoutedEventArgs());
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            HasFinished = true;
            Close();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (!HasFinished)
                e.Cancel = true;
        }
    }
}