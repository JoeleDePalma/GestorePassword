using GestioneGUI;
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
    /// Logica di interazione per MenuInterface.xaml
    /// </summary>
    public partial class MenuInterface : UserControl
    {
        private ApiClient Client { get; set; }
        private UserApi userApi { get; set; }
        private PasswordApi passwordApi { get; set; }
        private UserInfo userInfo { get; set; }

        public MenuInterface(ApiClient Client, UserApi userApi, PasswordApi passwordApi, UserInfo userInfo)
        {
            InitializeComponent();
            this.Client = Client;
            this.userApi = userApi;
            this.passwordApi = passwordApi;
            this.userInfo = userInfo;
        }
    }
}
