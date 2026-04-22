using System.Collections.Generic;
using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Libreria.API;
using Libreria.DTOs.Passwords;
using Libreria.HTTPRequestsLibrary;
using System.Linq;
using GestorePassword.UI.Desktop.Templates.Menu;
using GestorePassword.Core.ViewModels.Menu;
using System.Threading.Tasks;

namespace GestorePassword.UI.Desktop.Views.Menu
{
    public partial class MenuView : UserControl
    {
        private List<Grid> contentGrids {get; set;}
        private MenuViewModel _vm {get; set;}
        public MenuView()
        {
            InitializeComponent();
            DataContext = new MenuViewModel();
            _vm = (MenuViewModel)DataContext;

            contentGrids = new()
            {
                PasswordsGrid,
                StatisticsGrid,
                ProfileGrid    
            };
            
            UserInitialTextBlock.Text = AppServices.currentUser.Username[0].ToString();
            UserUsernameTextBlock.Text = AppServices.currentUser.Username;
            ShowPasswordsButton.Click += ShowPasswordsContent!;
            ShowStatisticsButton.Click += ShowStatisticsContent!;
            ShowProfileButton.Click += ShowProfileContent!;
            Loaded += LoadUI!;
        }
        public void ShowPasswordsContent(object sender, RoutedEventArgs e)
            => ShowOnly(PasswordsGrid);
        public void ShowStatisticsContent(object sender, RoutedEventArgs e)
        {
            ShowOnly(StatisticsGrid);
            if (AppServices.passwordList == null)
            {
                SavedPasswordsStatisticsGrid.InfoValueText = "0";
                SavedAppUsernameStatisticsGrid.InfoValueText = "0";
                StrongPasswordsStatisticsGrid.InfoValueText = "0";
                WeakPasswordsStatisticsGrid.InfoValueText = "0";
                AveragePasswordStrengthStatisticsGrid.InfoValueText = "";
                LastPasswordCreatedAtStatisticsGrid.InfoValueText = "Nessuna";
                return;
            }
            SavedPasswordsStatisticsGrid.InfoValueText = AppServices.passwordList.Count().ToString();
            (
                int strongPasswords,
                int weakPasswords,
                string averagePasswordStrength,
                int savedUsernameCount,
                DateTime lastPasswordCreatedAt
            ) = _vm.GetPasswordsStatistics();
            SavedAppUsernameStatisticsGrid.InfoValueText = savedUsernameCount.ToString();
            StrongPasswordsStatisticsGrid.InfoValueText = strongPasswords.ToString();
            WeakPasswordsStatisticsGrid.InfoValueText = weakPasswords.ToString();
            AveragePasswordStrengthStatisticsGrid.InfoValueText = averagePasswordStrength;
            LastPasswordCreatedAtStatisticsGrid.InfoValueText = lastPasswordCreatedAt.ToString("dd/MM/yyyy");
            
        }
        public void ShowProfileContent(object sender, RoutedEventArgs e)
        {
            ShowOnly(ProfileGrid);
            if (AppServices.currentUser != null)
            {
                ProfileUsernameTextBlock.Text = AppServices.currentUser.Username;
                UserInfoContentGrid.CreatedAtUser = $"Data di creazione: {AppServices.currentUser.CreatedAt.ToString("dd/MM/yyyy")}";
            }
            if (AppServices.passwordList == null)
            {
                UserInfoContentGrid.SavedUsernameCount = "Nomi utenti salvati: 0";
                UserInfoContentGrid.LastPasswordCreatedAt = "Ultima aggiunta: nessuna";
                UserInfoContentGrid.AveragePasswordStrength = "Sicurezza media password:";
                return;
            }
            (
                _, _, 
                string averagePasswordStrength, 
                int savedUsernameCount, 
                DateTime lastPasswordCreatedAt
            ) = _vm.GetPasswordsStatistics();
            UserInfoContentGrid.SavedUsernameCount = $"Nomi utenti salvati: {savedUsernameCount.ToString()}";
            UserInfoContentGrid.LastPasswordCreatedAt = $"Ultima aggiunta: {lastPasswordCreatedAt.ToString("dd/MM/yyyy")}";
            UserInfoContentGrid.AveragePasswordStrength = $"Sicurezza media password: {averagePasswordStrength}";
        }

        public async void LoadUI(object sender, RoutedEventArgs e)
        {
            LoadingGrid.IsVisible = true;
            await SetAllPassword();

            if (AppServices.passwordList == null)
            {
                LoadingGrid.IsVisible = false;
                return;
            }

            foreach (var p in AppServices.passwordList)
            {
                PasswordStackpanel.Children.Add
                (
                    new PasswordGrid()
                    {
                        InitialAppName = p.App[0].ToString(),
                        AppName = p.App,
                        Username = p.Username,
                        ShownPassword = p.Password,
                        shownPasswordText = p.Password,
                        hiddenPasswordText = p.Password,
                        passwordId = p.Id,
                        createdAt = p.CreatedAt,
                        lastUpdateAt = p.LastUpdateAt
                    }
                );
            }

            LoadingGrid.IsVisible = false;
        }

        public async Task SetAllPassword()
        {
            await _vm.SetAllPassword();
        }

        public void ShowOnly(Grid gridToShow)
        {
            foreach(var g in contentGrids)
                g.IsVisible = false;
            gridToShow.IsVisible = true;
        }
    }
}