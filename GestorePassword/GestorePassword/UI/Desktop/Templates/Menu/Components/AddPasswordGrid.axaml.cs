using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media.Imaging;
using Avalonia.Interactivity;
using Avalonia.Platform;
using Avalonia;
using GestorePassword.Core.ViewModels.Menu;

namespace GestorePassword.UI.Desktop.Templates.Menu.Components;

public class AddPasswordGrid : TemplatedControl
{
    public static readonly StyledProperty<MenuViewModel?> ViewModelProperty =
        AvaloniaProperty.Register<AddPasswordGrid, MenuViewModel?>(nameof(ViewModel));

    public MenuViewModel? ViewModel
    {
        get => GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }

    public event Action? PasswordSaved;
    public event Action? Cancel;

    private TextBox? _shownAddingPasswordInput;
    private TextBox? _hiddenAddingPasswordInput;
    private Image? _addingPasswordEyeImage;
    private TextBlock? _addingPasswordErrorTextBlock;
    private Button? _changeAddingPasswordVisibilityButton;
    private Button? _savePasswordButton;
    private Button? _cancelButton;
    private Button? _generatePasswordButton;
    private TextBox? _addingPasswordAppInput; 
    private TextBox? _addingPasswordUsernameInput;
    private TemplatedControl? _loadingAnimationGrid;
    private MenuViewModel? _vm;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _shownAddingPasswordInput = e.NameScope.Find<TextBox>("ShownAddingPasswordInput");
        _hiddenAddingPasswordInput = e.NameScope.Find<TextBox>("HiddenAddingPasswordInput");
        _addingPasswordEyeImage = e.NameScope.Find<Image>("AddingPasswordEyeImage");
        _changeAddingPasswordVisibilityButton = e.NameScope.Find<Button>("ChangeAddingPasswordVisibilityButton");
        _savePasswordButton = e.NameScope.Find<Button>("SavePasswordButton");
        _cancelButton = e.NameScope.Find<Button>("CancelButton");
        _generatePasswordButton = e.NameScope.Find<Button>("GeneratePasswordButton");
        _addingPasswordAppInput = e.NameScope.Find<TextBox>("AddingPasswordAppInput");
        _addingPasswordUsernameInput = e.NameScope.Find<TextBox>("AddingPasswordUsernameInput");
        _shownAddingPasswordInput = e.NameScope.Find<TextBox>("ShownAddingPasswordInput");
        _hiddenAddingPasswordInput = e.NameScope.Find<TextBox>("HiddenAddingPasswordInput");
        _loadingAnimationGrid = e.NameScope.Find<TemplatedControl>("LoadingAnimation");
        _vm = ViewModel!;

        if (_changeAddingPasswordVisibilityButton != null) 
            _changeAddingPasswordVisibilityButton.Click += ChangeAddingPasswordVisibility!;
    
        if (_savePasswordButton != null)
            _savePasswordButton.Click += SavePassword!;
    
        if (_cancelButton != null)
            _cancelButton.Click += GoBackToMenu!;

        if (_generatePasswordButton != null)
            _generatePasswordButton.Click += GeneratePassword!;
    }

    public void ChangeAddingPasswordVisibility(object sender, RoutedEventArgs e)
    {
        if (_shownAddingPasswordInput == null ||
            _hiddenAddingPasswordInput == null || 
            _addingPasswordEyeImage == null) return;

        try
        {
            if (_shownAddingPasswordInput.IsVisible)
            {
                _hiddenAddingPasswordInput.Text = _shownAddingPasswordInput.Text;
                _shownAddingPasswordInput.IsVisible = false;
                _hiddenAddingPasswordInput.IsVisible = true;
                _addingPasswordEyeImage.Source = new Bitmap(AssetLoader.Open(new Uri("avares://GestorePassword/UI/Desktop/Images/closed_eye.png")));
                return;
            }

            _shownAddingPasswordInput.Text = _hiddenAddingPasswordInput.Text;
            _shownAddingPasswordInput.IsVisible = true;
            _hiddenAddingPasswordInput.IsVisible = false;
            _addingPasswordEyeImage.Source = new Bitmap(AssetLoader.Open(new Uri("avares://GestorePassword/UI/Desktop/Images/opened_eye.png")));
        }
        catch (Exception ex)
        {
            SetAddingPasswordErrorTextBlock(ex.Message);
        }
    }

    public void SetAddingPasswordErrorTextBlock(string errorString)
    {
        if (_addingPasswordErrorTextBlock == null) return;

        _addingPasswordErrorTextBlock.Text = errorString;
        _addingPasswordErrorTextBlock.IsVisible = true;
    }

    public async void SavePassword(object sender, RoutedEventArgs e)
    {
        string app = default!;
        string username = default!;
        string password = default!;

        app = _addingPasswordAppInput!.Text!;
        username = _addingPasswordUsernameInput!.Text!;

        if (_shownAddingPasswordInput!.IsVisible)
        {
            password = _shownAddingPasswordInput.Text!;
        }
        else
        {
            password = _hiddenAddingPasswordInput!.Text!;
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

        if (_loadingAnimationGrid != null)
            _loadingAnimationGrid!.IsVisible = true;

        (bool Success, string? ErrorString) response;

        response = await _vm!.SaveNewPassword(app, username, password);

        _loadingAnimationGrid!.IsVisible = false;

        if (!response.Success)
        {
            SetAddingPasswordErrorTextBlock(response.ErrorString!);
            return;
        }

        if (_shownAddingPasswordInput.IsVisible)
        {
            _addingPasswordAppInput.Text = null;
            _addingPasswordUsernameInput.Text = null;
            _shownAddingPasswordInput.Text = null;
            _hiddenAddingPasswordInput!.Text = null;
            _shownAddingPasswordInput.IsVisible = false;
            _hiddenAddingPasswordInput.IsVisible = true;
            _addingPasswordEyeImage!.Source = new Bitmap(AssetLoader.Open(new Uri("avares://GestorePassword/UI/Desktop/Images/closed_eye.png")));
        }

        PasswordSaved!.Invoke();
    }

    public void GoBackToMenu(object sender, RoutedEventArgs e)
        => Cancel!.Invoke();

    public void GeneratePassword(object sender, RoutedEventArgs e)
    {
        var generatedPassword = _vm!.GeneratePassword();

        if (_shownAddingPasswordInput!.IsVisible)
        {
            _shownAddingPasswordInput.Text = generatedPassword;
            return;
        }

        _hiddenAddingPasswordInput!.Text = generatedPassword;
        return;
    }
}