using BlessDocument.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BlessDocument.Pages;

namespace BlessDocument.View
{
    /// <summary>
    /// Логика взаимодействия для HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : Window
    {
        public HomeWindow()
        {
            InitializeComponent();
            Update_Table();
            if (AuthPage.Globals.role != 1)
                Download.Visibility = Visibility.Hidden;
        }
        public static class Email
        {
            public static string text;
        }
        private void Update_Table()
        {
            DGrid.ItemsSource = BlessDocumentsEntities.GetContext().Archive.ToList();
        }

        private void AddDoc(object sender, RoutedEventArgs e)
        {
            AddNewArchiveWindow ANPW = new AddNewArchiveWindow(null);
            ANPW.ShowDialog();
            Update_Table();
        }

        private void SendDoc(object sender, RoutedEventArgs e)
        {
            if (DGrid.SelectedItem != null)
            {
                var choose = DGrid.SelectedItems.Cast<Archive>().ToList();
                int idProj = choose.FirstOrDefault().id;

                Archive proj = BlessDocumentsEntities.GetContext().Archive.Find(idProj);
                byte[] data = (byte[])proj.Files;

                TakeEmailWindow TEW = new TakeEmailWindow();
                TEW.ShowDialog();

                Thread send = new Thread(() => Send_File(data, proj));
                send.Start();
            }
            else
            {
                MessageBox.Show("Выберите отправляемый файл", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DownloadDoc(object sender, RoutedEventArgs e)
        {
            if (DGrid.SelectedItem != null)
            {
                var choose = DGrid.SelectedItems.Cast<Archive>().ToList();
                int idProj = choose.FirstOrDefault().id;

                Archive proj = BlessDocumentsEntities.GetContext().Archive.Find(idProj);
                byte[] data = (byte[])proj.Files;

                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                string fileName = path + $"\\{proj.FileName}.{proj.Type}";

                using (FileStream FS = new FileStream(fileName, FileMode.OpenOrCreate))
                {
                    FS.Write(data, 0, data.Length);
                    MessageBox.Show("Архив сохранён на рабочий стол", "Успех", MessageBoxButton.OK);
                }
            }
            else
                MessageBox.Show("Выберите работу у которой хотите скачать архив", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void EditDoc(object sender, RoutedEventArgs e)
        {
            if (DGrid.SelectedItem != null)
            {
                AddNewArchiveWindow ANPW = new AddNewArchiveWindow(DGrid.SelectedItem as Archive);
                ANPW.ShowDialog();
            }
            else
                MessageBox.Show("Выберите редактируемый проект!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);

            Update_Table();
        }

        private void DeleteDoc(object sender, RoutedEventArgs e)
        {
            if (DGrid.SelectedItem != null)
            {
                var Removing = DGrid.SelectedItems.Cast<Archive>().ToList();

                if (MessageBox.Show($"Вы точно хотите удалить следующие {Removing.Count()} элементов?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        foreach (var rem in Removing)
                        {
                            BlessDocumentsEntities.GetContext().Archive.Remove(rem);
                        }
                        BlessDocumentsEntities.GetContext().SaveChanges();
                        MessageBox.Show("Данные удалены", "Успех", MessageBoxButton.OK);
                        Update_Table();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите удаляемый проект!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        void Send_File(byte[] data, Archive proj)
        {
            if (Email.text != null)
            { 
            string tempFilePath = System.IO.Path.GetTempFileName();
            File.WriteAllBytes(tempFilePath, data);

            string email = Email.text;
            string subject = "Ваш файл";
            string body = "Файл который храниться в информационной системе Документооборота";

            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("radikimameev@mail.ru");
                    mail.To.Add(email);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.Attachments.Add(new Attachment(new MemoryStream((byte[])data), $"{proj.FileName}.{proj.Type}"));


                    using (SmtpClient smtp = new SmtpClient("smtp.mail.ru"))
                    {
                        smtp.Port = 587;
                        smtp.Credentials = new NetworkCredential("radikimameev@mail.ru", "hsxqabm7tfcA8L4w4CdH\r\n");
                        smtp.EnableSsl = true;

                        smtp.Send(mail);
                    }

                    MessageBox.Show($"Файл отправлен на почту", "Успех", MessageBoxButton.OK);
                    Email.text = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }
        }

        private void ExitWindow(object sender, RoutedEventArgs e)
        {
            AuthWindow AW = new AuthWindow();
            AW.Show();
            this.Close();
        }
    }
}


