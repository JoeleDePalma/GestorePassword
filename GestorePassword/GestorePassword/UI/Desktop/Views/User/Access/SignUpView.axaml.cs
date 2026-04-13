using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using GestorePassword.UI.Desktop.Views.User.Access;

namespace GestorePassword;

public partial class SignUpView : UserControl
{
    public SignUpView()
    {
        InitializeComponent();

        ShowPasswordButton.Click += ShowHidePassword!;
        HidePasswordButton.Click += ShowHidePassword!;
        GoToSignInViewButton.Click += GoToSignInView!;
    }

    public void ShowHidePassword(object sender, RoutedEventArgs e)
    {
        if (ShownPasswordInput.IsVisible)
        {
            HiddenPasswordInput.Text = ShownPasswordInput.Text;
            ShownPasswordInput.IsVisible = false;
            HiddenPasswordInput.IsVisible = true;
            ShowPasswordButton.IsVisible = true;
            HidePasswordButton.IsVisible = false;
        }
        else
        {
            ShownPasswordInput.Text = HiddenPasswordInput.Text;
            HiddenPasswordInput.IsVisible = false;
            ShownPasswordInput.IsVisible = true;
            ShowPasswordButton.IsVisible = false;
            HidePasswordButton.IsVisible = true;
        }
    }

    public void GoToSignInView(object sender, RoutedEventArgs e)
    {
        var main = (MainWindow)TopLevel.GetTopLevel(this)!;
        main.ChangeInterface(new SignInView());
    }
}