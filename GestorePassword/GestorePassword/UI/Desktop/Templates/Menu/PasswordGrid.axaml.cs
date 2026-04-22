using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using System.Linq;
using System;

namespace GestorePassword.UI.Desktop.Templates.Menu
{
    public partial class PasswordGrid : TemplatedControl
    {
        private Button? _showButton;
        private Button? _hideButton;
        private Button? _modifyButton;
        private Button? _deleteButton;

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            _showButton = e.NameScope.Find<Button>("ShowPasswordButton");
            _hideButton = e.NameScope.Find<Button>("HidePasswordButton");
            _modifyButton = e.NameScope.Find<Button>("ModifyPasswordButton");
            _deleteButton = e.NameScope.Find<Button>("DeletePasswordButton");

            if (_showButton != null)
                _showButton.Click += ChangePasswordVisibility;

            if (_hideButton != null)
                _hideButton.Click += ChangePasswordVisibility;

            /*
            if (_modifyButton != null)
                _modifyButton.Click += ModifyPassword;

            if (_deleteButton != null)
                _deleteButton.Click += DeletePassword;*/
        }

        public DateTime createdAt { get; set; }
        public DateTime lastUpdateAt { get; set; }
        public int passwordId { get; set; }

        private string _hiddenPasswordText;
        public string hiddenPasswordText
        {
            get => _hiddenPasswordText;
            set
            {
                _hiddenPasswordText = new string('*', value?.Length ?? 0);
            }
        }

        private string _shownPasswordText;

        public string shownPasswordText 
        {
            get => _shownPasswordText;
            set => _shownPasswordText = value;
        }

        public static StyledProperty<string> InitialAppNameProperty = 
            AvaloniaProperty.Register<PasswordGrid, string>(nameof(InitialAppName));

        public string InitialAppName
        {
            get => GetValue(InitialAppNameProperty);
            set => SetValue(InitialAppNameProperty, value);
        }

        public static StyledProperty<string> AppNameProperty =
            AvaloniaProperty.Register<PasswordGrid, string>(nameof(AppName));

        public string AppName
        {
            get => GetValue(AppNameProperty);
            set => SetValue(AppNameProperty, value);
        }

        public static StyledProperty<string?> UsernameProperty =
            AvaloniaProperty.Register<PasswordGrid, string?>(nameof(Username));

        public string? Username
        {
            get => GetValue(UsernameProperty);
            set => SetValue(UsernameProperty, value);
        }

        public static StyledProperty<string> ShownPasswordProperty =
            AvaloniaProperty.Register<PasswordGrid, string>(nameof(ShownPassword));

        public string ShownPassword
        {
            get => GetValue(ShownPasswordProperty);
            set => SetValue(ShownPasswordProperty, value);
        }

        public void ChangePasswordVisibility(object sender, RoutedEventArgs e)
        {
            if (ShownPassword == hiddenPasswordText)
            {
                _hideButton.IsVisible = true;
                _showButton.IsVisible = false;
                ShownPassword = shownPasswordText;
                return;
            }

            _hideButton.IsVisible = false;
            _showButton.IsVisible = true;
            ShownPassword = hiddenPasswordText;

        }
    }
}