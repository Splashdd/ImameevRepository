using BlessDocument.Pages;
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
using System.Windows.Shapes;

namespace BlessDocument.View
{
    /// <summary>
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        public static AuthWindow window;
        public AuthWindow()
        {
            InitializeComponent();
            window = this;

            AuthOrRegFrame.Content = new AuthPage();
        }
        private void Click_Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void Window_MoueDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                AuthWindow.window.DragMove();
            }
        }
    }
}
