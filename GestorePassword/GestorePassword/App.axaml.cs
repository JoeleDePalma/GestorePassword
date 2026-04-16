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
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var vm = new MenuViewModel();

#if ANDROID
        if (ApplicationLifetime is ISingleViewApplicationLifetime singleView)
        {
            singleView.MainView = new SignInView
            {
                DataContext = vm
            };
        }
#else
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                Content = new MenuView { DataContext = vm },
                Width = 1600,
                Height = 900
            };
        }
#endif

        base.OnFrameworkInitializationCompleted();
    }
}