using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using System.Linq;
using System;

namespace GestorePassword.UI.Desktop.Templates.Menu.Components
{
    public partial class PasswordGrid : TemplatedControl
    {
        public event Action<PasswordGrid>? ShowClicked;
        public event Action<PasswordGrid>? HideClicked;
        public event Action<PasswordGrid>? ModifyClicked;
        public event Action<PasswordGrid>? DeleteClicked;

        private Button? _showButton;
        private Button? _hideButton;
        private Button? _modifyButton;
        private Button? _deleteButton;

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            var showButton = e.NameScope.Find<Button>("ShowPasswordButton");
            var hideButton = e.NameScope.Find<Button>("HidePasswordButton");
            var modifyButton = e.NameScope.Find<Button>("ModifyPasswordButton");
            var deleteButton = e.NameScope.Find<Button>("DeletePasswordButton");

            if (showButton != null)
                showButton.Click += (_, __) => ShowClicked?.Invoke(this);

            if (hideButton != null)
                hideButton.Click += (_, __) => HideClicked?.Invoke(this);

            if (modifyButton != null)
                modifyButton.Click += (_, __) => ModifyClicked?.Invoke(this);

            if (deleteButton != null)
                deleteButton.Click += (_, __) => DeleteClicked?.Invoke(this);

            ShowButtonVisibility = true;
            HideButtonVisibility = false;
            ShownPassword = hiddenPasswordText;
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
                _hiddenPasswordText = new string('●', value?.Length ?? 0);
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

        public static StyledProperty<bool> HideButtonVisibilityProperty =
            AvaloniaProperty.Register<PasswordGrid, bool>(nameof(HideButtonVisibility));

        public bool HideButtonVisibility
        {
            get => GetValue(HideButtonVisibilityProperty);
            set => SetValue(HideButtonVisibilityProperty, value);
        }

        public static StyledProperty<bool> ShowButtonVisibilityProperty =
            AvaloniaProperty.Register<PasswordGrid, bool>(nameof(ShowButtonVisibilityProperty));

        public bool ShowButtonVisibility
        {
            get => GetValue(ShowButtonVisibilityProperty);
            set => SetValue(ShowButtonVisibilityProperty, value);
        }
    }
}