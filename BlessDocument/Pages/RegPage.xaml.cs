using BlessDocument.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
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
    /// Логика взаимодействия для RegPage.xaml
    /// </summary>
    public partial class RegPage : Page
    {
        public RegPage()
        {
            InitializeComponent();
        }
        private void Click_Back(object sender, RoutedEventArgs e)
        {
            AuthPage pageAuth = new AuthPage();
            this.NavigationService.Navigate(pageAuth);
        }

        private void AddPersonal(object sender, RoutedEventArgs e)
        {
            Users _currentUser = new Users();

            StringBuilder Errors = new StringBuilder();
            if (LoginTxb.Text == "")
                Errors.AppendLine("Укажите логин!");
            if (PassTxb.Password == "")
                Errors.AppendLine("Укажите пароль!");

            if (Errors.Length > 0)
            {
                MessageBox.Show(Errors.ToString(), "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            _currentUser.Login = LoginTxb.Text;
            _currentUser.Password = PassTxb.Password;
            _currentUser.RoldeId = 2;
            BlessDocumentsEntities.GetContext().Users.Add(_currentUser);

            try
            {
                BlessDocumentsEntities.GetContext().SaveChanges();
                MessageBox.Show("Пользователь сохранен", "Успех", MessageBoxButton.OK);
                AuthPage pageAuth = new AuthPage();
                this.NavigationService.Navigate(pageAuth);
            }
            catch (DbEntityValidationException ex)
            {
                foreach (DbEntityValidationResult validationError in ex.EntityValidationErrors)
                {
                    MessageBox.Show("Object: " + validationError.Entry.Entity.ToString());
                    foreach (DbValidationError err in validationError.ValidationErrors)
                    {
                        MessageBox.Show(err.ErrorMessage + "");
                    }
                }
            }

        }
    }
}
