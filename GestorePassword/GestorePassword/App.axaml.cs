using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using GestorePassword.Core.ViewModels.User.Access;
using GestorePassword.Core.ViewModels.Menu;
using GestorePassword.UI.Desktop.Views.Menu;

#if ANDROID
using GestorePassword.UI.Android.Views.User.Access;
#else
using GestorePassword.UI.Desktop.Views.User.Access;
#endif

namespace GestorePassword;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        AppServices.apiClient = new("http://localhost:8080");
        AppServices.userApi = new(AppServices.apiClient);
        AppServices.passwordApi = new(AppServices.apiClient);
        AppServices.versionApi = new(AppServices.apiClient);
        AppServices.appVersion = new("1.3.0");
    }

    public override void OnFrameworkInitializationCompleted()
    {
#if ANDROID
        if (ApplicationLifetime is ISingleViewApplicationLifetime singleView)
        {
            singleView.MainView = new SignInView;
        }
#else
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                Content = new SignInView { },
                Width = 1600,
                Height = 900
            };
        }
#endif

        base.OnFrameworkInitializationCompleted();
    }
}