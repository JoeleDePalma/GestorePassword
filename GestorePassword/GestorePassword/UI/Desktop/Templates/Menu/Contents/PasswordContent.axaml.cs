using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using GestorePassword.UI.Desktop.Templates.Menu.Components;

namespace GestorePassword.UI.Desktop.Templates.Menu.Contents;

public class PasswordContent : TemplatedControl
{    
    private AddPasswordGrid? _addPasswordGrid;
    private Button? _addPasswordButton;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _addPasswordGrid = e.NameScope.Find<AddPasswordGrid>("AddPasswordGrid");
        _addPasswordButton = e.NameScope.Find<Button>("AddPasswordButton");

        if (_addPasswordButton != null)
            _addPasswordButton.Click += ManageAddPasswordGridVisibility!;

        if (_addPasswordGrid != null)
            _addPasswordGrid.Cancel += () => ManageAddPasswordGridVisibility(null!, new RoutedEventArgs());
    }

    public void ManageAddPasswordGridVisibility(object sender, RoutedEventArgs e)
    {
        if (_addPasswordGrid == null) return;

        _addPasswordGrid.IsVisible = !_addPasswordGrid.IsVisible;
    }
}