using Avalonia;
using Avalonia.Controls.Primitives;

namespace GestorePassword.UI.Desktop.Templates.Menu.Components
{
    public class UserInfoGrid : TemplatedControl
    {
        public static StyledProperty<string> CreatedAtUserProperty =
            AvaloniaProperty.Register<UserInfoGrid, string>(nameof(CreatedAtUser));

        public string CreatedAtUser
        {
            get => GetValue(CreatedAtUserProperty);
            set => SetValue(CreatedAtUserProperty, value);
        }

        public static StyledProperty<string> SavedUsernameCountProperty =
            AvaloniaProperty.Register<UserInfoGrid, string>(nameof(SavedUsernameCount));

        public string SavedUsernameCount
        {
            get => GetValue(SavedUsernameCountProperty);
            set => SetValue(SavedUsernameCountProperty, value);
        }

        public static StyledProperty<string> LastPasswordCreatedAtProperty =
            AvaloniaProperty.Register<UserInfoGrid, string>(nameof(LastPasswordCreatedAt));

        public string LastPasswordCreatedAt
        {
            get => GetValue(LastPasswordCreatedAtProperty);
            set => SetValue(LastPasswordCreatedAtProperty, value);
        }

        public static StyledProperty<string> AveragePasswordStrengthProperty =
            AvaloniaProperty.Register<UserInfoGrid, string>(nameof(AveragePasswordStrength));

        public string AveragePasswordStrength
        {
            get => GetValue(AveragePasswordStrengthProperty);
            set => SetValue(AveragePasswordStrengthProperty, value);
        }
    }
}