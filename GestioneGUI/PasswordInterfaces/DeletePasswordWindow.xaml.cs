using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GestioneGUI.PasswordInterfaces
{
    /// <summary>
    /// Logica di interazione per DeletePasswordWindow.xaml
    /// </summary>
    public partial class DeletePasswordWindow : Window
    {
        public bool Result { get; private set; }

        public DeletePasswordWindow()
        {
            InitializeComponent();

            YesButton.Click += (s, e) =>
            {
                Result = true;
                Close();
            };

            NoButton.Click += (s, e) =>
            {
                Result = false;
                Close();
            };
        }
    }
}
