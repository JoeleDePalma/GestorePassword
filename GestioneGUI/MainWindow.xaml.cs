using GestioneGUI.PasswordInterfaces;
using HTTPRequestsLibrary;
using Libreria.API;
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

namespace GestorePassword
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ApiClient Client { get; set; }
        public UserApi userApi { get; set; }
        public PasswordApi passwordApi { get; set; }

        public MainWindow()
        {
            Client = new ApiClient("http://localhost:5211");
            userApi = new UserApi(Client);
            passwordApi = new PasswordApi(Client);

            InitializeComponent();
            MainContent.Content = new SignInInterface(Client, userApi, passwordApi);
            this.SizeChanged += Window_SizeChanged;
        }

        private bool _isResizing = false;
        private double aspectRatio = 800.0 / 500.0; 

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_isResizing) return;

            _isResizing = true;

            double newWidth = this.Width;
            double newHeight = this.Height;

            if (e.WidthChanged)
            {
                newHeight = newWidth / aspectRatio;

                if (newHeight < this.MinHeight)
                {
                    newHeight = this.MinHeight;
                    newWidth = newHeight * aspectRatio;
                }
            }
            else if (e.HeightChanged)
            {
                newWidth = newHeight * aspectRatio;

                if (newWidth < this.MinWidth)
                {
                    newWidth = this.MinWidth;
                    newHeight = newWidth / aspectRatio;
                }
            }

            this.Width = newWidth;
            this.Height = newHeight;

            _isResizing = false;
        }
    }
}
