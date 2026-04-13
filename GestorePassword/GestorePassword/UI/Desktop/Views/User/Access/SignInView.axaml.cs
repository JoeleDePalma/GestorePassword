using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace GestorePassword.UI.Desktop.Views.User.Access
{
    public partial class SignInView : UserControl
    {
        public SignInView()
        {
            InitializeComponent();

            ShowPasswordButton.Click += ShowHidePassword!;
            HidePasswordButton.Click += ShowHidePassword!;
            GoToSignUpViewButton.Click += GoToSignUpView!;
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

        public void GoToSignUpView(object sender, RoutedEventArgs e)
        {
            var main = (MainWindow)TopLevel.GetTopLevel(this)!;
            main.ChangeInterface(new SignUpView());
        }
    }
}