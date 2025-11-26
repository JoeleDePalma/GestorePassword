using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.Json;

namespace GestorePassword
{
    public class Password
    {
        public int? Id { get; set; }
        public string App { get; set; }
        public byte[] EncryptedPassword { get; set; }
    }
    /// <summary>
    /// Login functions for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SendRequest sendRequest;

        public MainWindow()
        {
            InitializeComponent();
            sendRequest = new SendRequest();
        }

        private async void SetHTTPTestTextAsync(object sender, RoutedEventArgs e)
        {
        }
    }
}
