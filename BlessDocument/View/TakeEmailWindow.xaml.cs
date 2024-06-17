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
    /// Логика взаимодействия для TakeEmailWindow.xaml
    /// </summary>
    public partial class TakeEmailWindow : Window
    {
        public TakeEmailWindow()
        {
            InitializeComponent();
        }
        private void Send_Email(object sender, RoutedEventArgs e)
        {
            HomeWindow.Email.text = EmailTB.Text;
            Close();
        }
    }
}
