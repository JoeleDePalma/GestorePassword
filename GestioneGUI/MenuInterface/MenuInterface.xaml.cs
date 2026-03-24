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

            Loaded += MenuInterface_Loaded;
        }

        private async void MenuInterface_Loaded(object sender, RoutedEventArgs e)
        {
            passwordInfoList = await GetAllPasswordsAsync(userInfo.Password);

            foreach (var p in passwordInfoList)
            {
                var cc = new ContentControl
                {
                    Style = (Style)FindResource("InfoPasswordContent"),
                    Content = p
                };

                PasswordStackPanel.Children.Add(cc);

                cc.ApplyTemplate();

                var showBtn = (Button)cc.Template.FindName("ShowPasswordButton", cc);
                var updateBtn = (Button)cc.Template.FindName("UpdatePasswordButton", cc);
                var deleteBtn = (Button)cc.Template.FindName("DeletePasswordButton", cc);

                updateBtn.Click += UpdatePassword;
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

        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(child);

            while (parent != null && parent is not T)
                parent = VisualTreeHelper.GetParent(parent);

            return parent as T;
        }
    }
}
