using GestioneGUI;
using HTTPRequestsLibrary;
using Libreria.API;
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
using GestioneGUI.PasswordInterfaces;
using System.Linq;
using Services;

namespace GestorePassword
{
    /// <summary>
    /// Logica di interazione per MenuInterface.xaml
    /// </summary>
    public partial class MenuInterface : UserControl
    {
        private ApiClient Client { get; set; }
        private UserApi userApi { get; set; }
        private PasswordApi passwordApi { get; set; }
        private UserInfo userInfo { get; set; }
        private List<PasswordInfo> passwordInfoList { get; set; }

        public MenuInterface(ApiClient Client, UserApi userApi, PasswordApi passwordApi, UserInfo userInfo)
        {
            InitializeComponent();

            this.Client = Client;
            this.userApi = userApi;
            this.passwordApi = passwordApi;
            this.userInfo = userInfo;
            this.Client.SetToken(userInfo.Token);

            UsernamePanelBlock.Text = $"{userInfo.Username}";
            IdPanelBlock.Text = $"ID: #{userInfo.UserID}";
            CreatedAtPanelBlock.Text = $"Data di creazione: {userInfo.CreatedAt.ToString("dd/MM/yyyy")}";

            Loaded += UILoad;
        }

        private async void PasswordsLoad(object sender, RoutedEventArgs e, List<PasswordInfo> passwordInfoList)
        {
            PasswordStackPanel.Children.Clear();

            foreach (var p in passwordInfoList)
            {
                var cc = new ContentControl
                {
                    Style = (Style)FindResource("InfoPasswordContent"),
                    Content = p
                };

                PasswordStackPanel.Children.Add(cc);

                cc.ApplyTemplate();

                var passwordTB = (TextBlock)cc.Template.FindName("PasswordTextBlock", cc);
                var showBtn = (Button)cc.Template.FindName("ShowPasswordButton", cc);
                var updateBtn = (Button)cc.Template.FindName("UpdatePasswordButton", cc);
                var deleteBtn = (Button)cc.Template.FindName("DeletePasswordButton", cc);

                updateBtn.Click += UpdatePassword;
                showBtn.Click += ShowHidePassword;
                deleteBtn.Click += DeletePassword;

                string hiddenPassword = "";

                foreach (var c in p.Password)
                {
                    hiddenPassword += "*";
                }

                passwordTB.Text = hiddenPassword;
            }

            var addPasswordButton = new Button();
            addPasswordButton.Style = (Style) FindResource("AddPasswordButton");
            addPasswordButton.Click += CreatePassword;

            PasswordStackPanel.Children.Add(addPasswordButton);
        }
        
        public async Task<List<PasswordInfo>> GetAllPasswordsAsync(string masterPassword)
        {
            var response = await passwordApi.GetAllAsync(masterPassword);

            List<PasswordInfo> PasswordList = new() {};

            foreach(var p in response.Data)
            {
                PasswordList.Add
                (
                    new PasswordInfo()
                    {
                        Id = (int) p.Id,
                        App = p.AppName,
                        Username = p.AppUsername,
                        Password = p.Password,
                        CreatedAt = p.CreatedAt,
                        LastUpdateAt = p.LastUpdateAt
                    }
                );
            }

            return PasswordList;
        }

        public void CreatePassword(object sender, RoutedEventArgs e)
        {
            var main = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            main.MainContent.Content = new CreatePasswordInterface(Client, userApi, passwordApi, userInfo);
        }

        public void UpdatePassword(object sender, RoutedEventArgs e)
        {
            var button = e.Source as Button;

            if (button == null)
                return;

            var cc = FindParent<ContentControl>(button);

            if (cc == null)
                return;

            var dto = (PasswordInfo)cc.Content;
            var main = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            main.MainContent.Content = new UpdatePasswordInterface(Client, userApi, passwordApi, userInfo, dto);
        }

        public void ShowHidePassword(object sender, RoutedEventArgs e)
        {
            var button = e.Source as Button;

            if (button == null)
                return;

            var cc = FindParent<ContentControl>(button);

            if (cc == null)
                return;

            var textBlock = (TextBlock)cc.Template.FindName("PasswordTextBlock", cc);
            var isShown = textBlock.Text.Any(c => c != '*');
            var eyeImage = button.Content as Image;

            if (isShown)
            {
                string hiddenPassword = "";

                foreach (char c in textBlock.Text)
                {
                    hiddenPassword += '*';
                }

                textBlock.Text = hiddenPassword;
                eyeImage.Source = new BitmapImage(new Uri("/Images/closed_eye.png", UriKind.Relative));
            }

            else
            {
                textBlock.Text = (cc.Content as PasswordInfo).Password;
                eyeImage.Source = new BitmapImage(new Uri("/Images/opened_eye.png", UriKind.Relative));
            }
        }

        public async void StatisticsLoad(object sender, RoutedEventArgs e, List<PasswordInfo> passwordInfoList)
        {
            PasswordsCreatedPanelBlock.Text = $"Password salvate: {passwordInfoList.Count}";

            if (passwordInfoList.Count == 0)
            {
                LastPasswordCreatedAtPanelBlock.Text = $"Ultimo salvataggio:";
                LastPasswordUpdatedAtPanelBlock.Text = $"Ultima modifica:";
                LongestPasswordPanelBlock.Text = $"Password più lunga: 0 caratteri";
                ShortestPasswordPanelBlock.Text = $"Password più corta: 0 caratteri";
                AveragePasswordLengthPanelBlock.Text = $"Lunghezza media: 0 caratteri";
                return;
            }

            PasswordInfo lastPasswordCreated = passwordInfoList.ElementAt(0);

            foreach (var p in passwordInfoList)
            {
                if (p.CreatedAt > lastPasswordCreated.CreatedAt)
                    lastPasswordCreated = p;
            }

            LastPasswordCreatedAtPanelBlock.Text = $"Ultimo salvataggio: {lastPasswordCreated.CreatedAt}";

            PasswordInfo lastPasswordUpdated = passwordInfoList.ElementAt(0);

            foreach (var p in passwordInfoList)
            {
                if (p.LastUpdateAt > lastPasswordUpdated.LastUpdateAt)
                    lastPasswordUpdated = p;
            }

            LastPasswordUpdatedAtPanelBlock.Text = $"Ultima modifica: {lastPasswordUpdated.LastUpdateAt}";

            PasswordInfo longestPassword = passwordInfoList.ElementAt(0);

            foreach (var p in passwordInfoList)
            {
                if (p.Password.Length > longestPassword.Password.Length)
                    longestPassword = p;
            }

            LongestPasswordPanelBlock.Text = $"Password più lunga: {longestPassword.Password.Length} caratteri";

            PasswordInfo shortestPassword = passwordInfoList.ElementAt(0);

            foreach (var p in passwordInfoList)
            {
                if (p.Password.Length < shortestPassword.Password.Length)
                    shortestPassword = p;
            }

            ShortestPasswordPanelBlock.Text = $"Password più corta: {shortestPassword.Password.Length} caratteri";

            int totalPasswordsLength = 0;

            foreach (var p in passwordInfoList)
            {
                totalPasswordsLength += p.Password.Length;
            }

            int averagePasswordLength = (totalPasswordsLength / passwordInfoList.Count);
            AveragePasswordLengthPanelBlock.Text = $"Lunghezza media: {averagePasswordLength} caratteri";
        }

        public async void DeletePassword(object sender, RoutedEventArgs e)
        {
            bool isThereError = false; 

            var button = e.Source as Button;

            if (button == null)
                return;

            var cc = FindParent<ContentControl>(button);

            if (cc == null)
                return;

            var dto = cc.Content as PasswordInfo;

            if (dto == null)
                return;

            var confirm = new DeletePasswordWindow
            {
                Owner = Application.Current.MainWindow
            };

            confirm.ShowDialog();

            if (!confirm.Result)
                return;

            bool Success = default;
            int StatusCode = default;
            string? ErrorString = default;

            (Success, StatusCode, ErrorString) = await PasswordRequests.DeletePasswordAsync(passwordApi, dto.Id);

            if (!Success)
            {
                if (StatusCode == 404)
                {
                    MessageBox.Show("Nessuna password da eliminare trovata");
                    isThereError = true;
                }
                
                else if (StatusCode == 401)
                {
                    MessageBox.Show("Accesso non autorizzato rilevato");
                    isThereError = true;
                }
            }

            if (isThereError)
            {
                return;
            }

            UILoad(this, new RoutedEventArgs());
            MessageBox.Show("Password eliminata con successo!");
            return;
        }

        public async void UILoad(object sender, RoutedEventArgs e)
        {
            var passwordsList = await GetAllPasswordsAsync(userInfo.Password);
            PasswordsLoad(this, new RoutedEventArgs(), passwordsList);
            StatisticsLoad(this, new RoutedEventArgs(), passwordsList);
        }

        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(child);

            while (parent != null && parent is not T)
                parent = VisualTreeHelper.GetParent(parent);

            return parent as T;
        }
    }
}
