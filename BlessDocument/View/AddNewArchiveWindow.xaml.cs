using BlessDocument.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
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
    /// Логика взаимодействия для AddNewArchiveWindow.xaml
    /// </summary>
    public partial class AddNewArchiveWindow : Window
    {
        Archive _currentArchive = new Archive();
        public AddNewArchiveWindow(Archive selectedArchive)
        {
            InitializeComponent();
            if (selectedArchive != null)
            {
                _currentArchive = selectedArchive;
                SelectFileBtn.Background = Brushes.LightGreen;
                SelectFileBtn.Content = "Изменить";
                AddBtn.Content = "Сохранить";
                windowTitle.Text = "Изменение файла";
            }

            DataContext = _currentArchive;
        }

        private void Select_Files(object sender, RoutedEventArgs e)
        {
            if (_currentArchive.Files != null)
            {
                _currentArchive.Files = null;
            }

            OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.Filter = "ZIP архив(*.zip)|*.zip"; можно выбрать фильт файлов, которые можно булет выбрать
            openFileDialog.ShowDialog();
            if (openFileDialog.FileName != null)
            {
                byte[] file_bytes = File.ReadAllBytes(openFileDialog.FileName);
                _currentArchive.Files = file_bytes;
                string[] parts = openFileDialog.SafeFileName.Split('.');
                _currentArchive.FileName = parts[0];
                _currentArchive.Type = parts[1];
                SelectFileBtn.Background = Brushes.LightGreen;
                SelectFileBtn.Content = "Выбрано";
            }
        }

        private void Add_File(object sender, RoutedEventArgs e)
        {
            StringBuilder Errors = new StringBuilder();
            if (FirstNameTB.Text == "")
                Errors.AppendLine("Укажите имя!");
            if (LastNameTB.Text == "")
                Errors.AppendLine("Укажите фамилию!");
            if (SelectFileBtn.Background != Brushes.LightGreen)
                Errors.AppendLine("Не указан файл!");

            if (Errors.Length > 0)
            {
                MessageBox.Show(Errors.ToString(), "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_currentArchive.id == 0)
            {
                BlessDocumentsEntities.GetContext().Archive.Add(_currentArchive);
            }

            try
            {
                BlessDocumentsEntities.GetContext().SaveChanges();
                MessageBox.Show("Проект сохранен", "Успех", MessageBoxButton.OK);
                Close();
            }
            catch (DbEntityValidationException ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
