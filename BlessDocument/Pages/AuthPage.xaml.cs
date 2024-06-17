using BlessDocument.Model;
using BlessDocument.View;
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

namespace BlessDocument.Pages
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public static class Globals 
        {
            public static int role; 

        }
        public AuthPage()
        {
            InitializeComponent();
        }
        private void Click_Reg(object sender, RoutedEventArgs e)
        {
            RegPage pageReg = new RegPage();
            this.NavigationService.Navigate(pageReg);
        }

        private void Enter(object sender, RoutedEventArgs e)
        {
            var auth = BlessDocumentsEntities.GetContext().Users.FirstOrDefault(a => a.Login == LoginTxb.Text && a.Password == PassTxb.Password);
            if (auth != null) 
                {
                    Globals.role = auth.RoldeId;
                    HomeWindow homeWindow = new HomeWindow();
                    homeWindow.Show();
                    Application.Current.MainWindow.Close();
                }
            else
            {
                MessageBox.Show("Неправильный логин и пароль");
            }
        }

            
        }
    }

