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
using GestorePassword.Core.Models;

namespace GestorePassword.UI.Desktop.Views.Menu
{
    public partial class MenuView : UserControl
    {
        private List<Grid> contentGrids {get; set;}
        private UserApi userApi { get; set; }
        private PasswordApi passwordApi { get; set; }
        private ApiClient apiClient { get; set; }
        private VersionApi versionApi { get; set; }
        private List<PasswordInfo> passwordsList { get; set; }
        private UserInfo currentUser { get; set; }
        private Version appVersion { get; set; }

        public MenuView()
        {
            InitializeComponent();

            contentGrids = new()
            {
                PasswordsGrid,
                StatisticsGrid,
                ProfileGrid    
            };

            ShowPasswordsButton.Click += ShowPasswordsContent!;
            ShowStatisticsButton.Click += ShowStatisticsContent!;
            ShowProfileButton.Click += ShowProfileContent!;
        }

        public void ShowPasswordsContent(object sender, RoutedEventArgs e)
            => ShowOnly(PasswordsGrid);

        public void ShowStatisticsContent(object sender, RoutedEventArgs e)
        {
            ShowOnly(StatisticsGrid);

            if (passwordsList == null) return;

            SavedPasswordsStatisticGrid.InfoValueText = passwordsList.Count().ToString();

            SavedAppUsernameStatisticGrid.InfoValueText = passwordsList
                .Where(p => 
                    !string.IsNullOrWhiteSpace(p.Username))
                        .Count().ToString();

            int strongPasswords = passwordsList
                .Where(p =>
                    p.Password.Length > 12 &&
                    p.Password.Any(char.IsDigit) &&
                    p.Password.Any(char.IsLetter) &&
                    p.Password.Any(char.IsLower) &&
                    p.Password.Any(char.IsUpper) &&
                    p.Password.Any(char.IsPunctuation)
                    ).Count();

            int weakPasswords = passwordsList
                .Where(p =>
                    !(
                    p.Password.Length > 12 &&
                    p.Password.Any(char.IsDigit) &&
                    p.Password.Any(char.IsLetter) &&
                    p.Password.Any(char.IsLower) &&
                    p.Password.Any(char.IsUpper) &&
                    p.Password.Any(char.IsPunctuation))
                    ).Count();

            StrongPasswordsGrid.InfoValueText = strongPasswords.ToString();

            WeakPasswordsGrid.InfoValueText = weakPasswords.ToString();

            AveragePasswordsStrengthGrid.InfoValueText = 
                strongPasswords > weakPasswords ? "Forte" : 
                strongPasswords < weakPasswords ? "Debole" : 
                "Media";

            LastPasswordAddedTimeGrid.InfoValueText = passwordsList.Max(p => p.CreatedAt).ToString("dd/MM/yyyy HH:mm");
        }

        public void ShowProfileContent(object sender, RoutedEventArgs e)
            => ShowOnly(ProfileGrid);

        public void ShowOnly(Grid gridToShow)
        {
            foreach(var g in contentGrids)
                g.IsVisible = false;

            gridToShow.IsVisible = true;
        }
    }
}