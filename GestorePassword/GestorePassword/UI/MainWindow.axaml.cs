using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GestorePassword;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    public void ChangeInterface(UserControl newInterface)
    {
        Content = newInterface;
    }
}