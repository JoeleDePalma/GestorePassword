using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;

namespace GestorePassword;

public partial class MainWindow : Window
{

    public MainWindow()
    {
        InitializeComponent();

        MinWidth = 800;
        MinHeight = 450;
    }

    public void ChangeInterface(UserControl newInterface)
    {
        Content = newInterface;
    }
}