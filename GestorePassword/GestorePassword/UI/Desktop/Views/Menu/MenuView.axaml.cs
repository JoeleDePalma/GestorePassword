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
using GestorePassword.UI.Desktop.Templates.Menu.Components;
using GestorePassword.Core.ViewModels.Menu;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using GestorePassword.Core.Models;
using Avalonia.Platform;
using Avalonia.Controls.Primitives;

namespace GestorePassword.UI.Desktop.Views.Menu
{
    public partial class MenuView : UserControl
    {
        private List<TemplatedControl> contentGrids { get; set; }
        private MenuViewModel _vm { get; set; }
        
        public MenuView()
        {
            InitializeComponent();
            DataContext = new MenuViewModel();
            _vm = (MenuViewModel)DataContext;

            contentGrids = new()
            {
                PasswordsGrid,
                // StatisticsGrid,
                // ProfileGrid,
                // LoadingGrid,
                // ModifyPasswordGrid,
                // DeletePasswordGrid
            };
            
            var _addPasswordGrid = PasswordsGrid.Find<AddPasswordGrid>("AddPasswordGrid");

            if (_addPasswordGrid != null)
                _addPasswordGrid.PasswordSaved += () => LoadUI(null!, new RoutedEventArgs());
                
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

            PasswordStackpanel.Children.Clear();

            PasswordGrid newGrid;
            Button _showButton;
            Button _hideButton;
            Button _modifyButton;
            Button _deleteButton;

            foreach (var p in AppServices.passwordList)
            {
                newGrid = new PasswordGrid()
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
                };
                

                newGrid.ShowClicked += grid =>
                {
                    (grid.HideButtonVisibility, grid.ShowButtonVisibility, grid.ShownPassword) = ChangePasswordVisibility(grid.ShownPassword, grid.hiddenPasswordText, grid.shownPasswordText);
                };

                newGrid.HideClicked += grid =>
                {
                    (grid.HideButtonVisibility, grid.ShowButtonVisibility, grid.ShownPassword) = ChangePasswordVisibility(grid.ShownPassword, grid.hiddenPasswordText, grid.shownPasswordText);
                };

                newGrid.ModifyClicked += grid =>
                {
                    ManageModifyPasswordGridVisibility(grid);
                };

                newGrid.DeleteClicked += grid =>
                {
                    ManageDeletePasswordGridVisibility(grid);
                };

                PasswordStackpanel.Children.Add(newGrid);
            }

            ShowPasswordsContent(null!, new());
            LoadingGrid.IsVisible = false;
        }

        public void ManageModifyPasswordGridVisibility(PasswordGrid sender)
        {
            if (ModifyPasswordGrid.IsVisible)
            {
                ModifyPasswordGrid.IsVisible = false;
                return;
            }

            if (AppServices.currentPassword == null)
                AppServices.currentPassword = new PasswordInfo();

            AppServices.currentPassword = new PasswordInfo()
            {
                Id = sender.passwordId,
                App = sender.AppName,
                Username = sender.Username,
                Password = sender.shownPasswordText,
                CreatedAt = sender.createdAt,
                LastUpdateAt = sender.lastUpdateAt
            };

            ModifyPasswordGrid.IsVisible = true;
            ModifingPasswordAppInput.Text = AppServices.currentPassword.App;
            ModifingPasswordUsernameInput.Text = AppServices.currentPassword.Username;
            HiddenModifingPasswordInput.Text = AppServices.currentPassword.Password;
            ShownModifingPasswordInput.Text = AppServices.currentPassword.Password;
        }

        public void ManageDeletePasswordGridVisibility(PasswordGrid sender)
        {
            if (DeletePasswordGrid.IsVisible)
            {
                DeletePasswordGrid.IsVisible = false;
                return;
            }

            if (AppServices.currentPassword == null)
                AppServices.currentPassword = new PasswordInfo();

            AppServices.currentPassword = new PasswordInfo()
            {
                Id = sender.passwordId,
                App = sender.AppName,
                Username = sender.Username,
                Password = sender.shownPasswordText,
                CreatedAt = sender.createdAt,
                LastUpdateAt = sender.lastUpdateAt
            };

            DeletePasswordGrid.IsVisible = true;
        }

        public (bool hideButtonVisibility, bool showButtonVisibility, string textToShow) 
        ChangePasswordVisibility(string ShownPassword, string hiddenPasswordText, string shownPasswordText)
        {
            if (ShownPassword == hiddenPasswordText) return (true, false, shownPasswordText);

            return (false, true, hiddenPasswordText);
        }

        public void ChangeModifingPasswordVisibility(object sender, RoutedEventArgs e)
        {
            try
            { 
                if (ShownModifingPasswordInput.IsVisible)
                {
                    HiddenModifingPasswordInput.Text = ShownModifingPasswordInput.Text;
                    ShownModifingPasswordInput.IsVisible = false;
                    HiddenModifingPasswordInput.IsVisible = true;
                    ModifingPasswordEyeImage.Source = new Bitmap(AssetLoader.Open(new Uri("avares://GestorePassword/UI/Desktop/Images/closed_eye.png")));
                    return;
                }

                ShownModifingPasswordInput.Text = HiddenModifingPasswordInput.Text;
                ShownModifingPasswordInput.IsVisible = true;
                HiddenModifingPasswordInput.IsVisible = false;
                ModifingPasswordEyeImage.Source = new Bitmap(AssetLoader.Open(new Uri("avares://GestorePassword/UI/Desktop/Images/opened_eye.png")));
            }
            catch (Exception ex)
            {
                ModifingPasswordErrorTextBlock.Text = ex.Message;
                ModifingPasswordErrorTextBlock.IsVisible = true;
            }
        }

        public void GoBackToMenu(object sender, RoutedEventArgs e)
        {
            ShowPasswordsContent(null!, new RoutedEventArgs());
        }

        public void GeneratePassword(object sender, RoutedEventArgs e)
        {
            var generatedPassword = _vm.GeneratePassword();

            if (AddPasswordGrid.IsVisible)
            {
                if (ShownAddingPasswordInput.IsVisible)
                {
                    ShownAddingPasswordInput.Text = generatedPassword;
                    return;
                }

                HiddenAddingPasswordInput.Text = generatedPassword;
                return;
            }

            if (ShownModifingPasswordInput.IsVisible)
            {
                ShownModifingPasswordInput.Text = generatedPassword;
                return;
            }

            HiddenModifingPasswordInput.Text = generatedPassword;
            return;
        }

        public async void DeletePassword(object sender, RoutedEventArgs e)
        {
            var passwordId = AppServices.currentPassword.Id;

            var response = await _vm.DeletePassword(passwordId);

            if (response.Success)
            {
                LoadUI(null!, new RoutedEventArgs());
                return;
            }

            DeletingPasswordErrorTextBlock.Text = response.ErrorString;
            DeletingPasswordErrorTextBlock.IsVisible = true;
        }

        public void SetAddingPasswordErrorTextBlock(string errorString)
        {
            ModifingPasswordErrorTextBlock.Text = errorString;
            ModifingPasswordErrorTextBlock.IsVisible = true;
            return;
        }

        public async Task SetAllPassword()
        {
            await _vm.SetAllPassword();
        }

        public void ShowOnly(TemplatedControl gridToShow)
        {
            foreach(var g in contentGrids)
                g.IsVisible = false;
            gridToShow.IsVisible = true;
        }
    }
}