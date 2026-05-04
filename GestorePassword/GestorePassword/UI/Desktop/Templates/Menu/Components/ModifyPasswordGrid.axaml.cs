using Avalonia;
using Avalonia.Controls.Primitives;
using GestorePassword.Core.ViewModels.Menu;

namespace GestorePassword.UI.Desktop.Templates.Menu.Components;

public class ModifyPasswordGrid : TemplatedControl
{
    public static readonly StyledProperty<MenuViewModel?> ViewModelProperty =
        AvaloniaProperty.Register<AddPasswordGrid, MenuViewModel?>(nameof(ViewModel));

    public MenuViewModel? ViewModel
    {
        get => GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }

    private MenuViewModel? _vm;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _vm = ViewModel;
    }

    public async void SavePassword(object sender, RoutedEventArgs e)
    {
        string app = default!;
        string username = default!;
        string password = default!;

        if (AddPasswordGrid.IsVisible)
        {
            app = AddingPasswordAppInput.Text!;
            username = AddingPasswordUsernameInput.Text!;

            if (ShownAddingPasswordInput.IsVisible)
            {
                password = ShownAddingPasswordInput.Text!;
            }
            else
            {
                password = HiddenAddingPasswordInput.Text!;
            }
        }
        else
        {
            app = ModifingPasswordAppInput.Text!;
            username = ModifingPasswordUsernameInput.Text!;

            if (ShownModifingPasswordInput.IsVisible)
            {
                password = ShownModifingPasswordInput.Text!;
            }
            else
            {
                password = HiddenModifingPasswordInput.Text!;
            }
        }

        if (string.IsNullOrWhiteSpace(app))
        {
            SetAddingPasswordErrorTextBlock("Inserire l'app");
            return;
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            SetAddingPasswordErrorTextBlock("Inserire la password");
            return;
        }

        if (password.All(c => char.Equals(c, '*')))
        {
            SetAddingPasswordErrorTextBlock("La password non può contenere solo asterischi");
            return;
        }

        LoadingGrid.IsVisible = true;

        (bool Success, string? ErrorString) response;

        if (AddPasswordGrid.IsVisible)
            response = await _vm.SaveNewPassword(app, username, password);

        else
            response = await _vm.ModifyPassword(AppServices.currentPassword.Id, app, username, password);

        LoadingGrid.IsVisible = false;

        if (!response.Success)
        {
            SetAddingPasswordErrorTextBlock(response.ErrorString!);
            return;
        }

        if (ShownAddingPasswordInput.IsVisible && AddPasswordGrid.IsVisible)
        {
            AddingPasswordAppInput.Text = null;
            AddingPasswordUsernameInput.Text = null;
            ShownAddingPasswordInput.Text = null;
            HiddenAddingPasswordInput.Text = null;
            ShownAddingPasswordInput.IsVisible = false;
            HiddenAddingPasswordInput.IsVisible = true;
            AddingPasswordEyeImage.Source = new Bitmap(AssetLoader.Open(new Uri("avares://GestorePassword/UI/Desktop/Images/closed_eye.png")));
        }
        else if (ShownModifingPasswordInput.IsVisible)
        {
            ModifingPasswordAppInput.Text = null;
            ModifingPasswordUsernameInput.Text = null;
            ShownModifingPasswordInput.Text = null;
            HiddenModifingPasswordInput.Text = null;
            ShownModifingPasswordInput.IsVisible = false;
            HiddenModifingPasswordInput.IsVisible = true;
            ModifingPasswordEyeImage.Source = new Bitmap(AssetLoader.Open(new Uri("avares://GestorePassword/UI/Desktop/Images/closed_eye.png")));
        }

        LoadUI(null!, new RoutedEventArgs());
    }
}