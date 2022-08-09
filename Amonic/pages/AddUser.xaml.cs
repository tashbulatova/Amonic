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
using System.Text.RegularExpressions;

namespace Amonic.pages
{
    /// <summary>
    /// Логика взаимодействия для AddUser.xaml
    /// </summary>
    public partial class AddUser : Page
    {
        public AddUser()
        {
            InitializeComponent();
            cmbOffice.ItemsSource = DBConn.db.Offices.ToList();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var message = "";
            if (String.IsNullOrEmpty(this.txbEmail.Text) ||
                String.IsNullOrEmpty(this.txbFirstName.Text) ||
                String.IsNullOrEmpty(this.txbLastName.Text) ||
                String.IsNullOrEmpty(this.dpBith.Text) ||
                String.IsNullOrEmpty(this.cmbOffice.Text) ||
                String.IsNullOrEmpty(this.psbPass.Password)
                )
            {
                message += "Все поля должны быть заполнены!\n";
            }

            if (!Regex.Match(this.txbEmail.Text, @"^.+\@.+\..+").Success)
            {
                message += "Email должен  набран по шаблону Х@Х.Х \n";
            }
            if (!Regex.Match(this.psbPass.Password, @".{8}").Success)
            {
                message += "Пароль должен содержать минимум 8 знаков \n";
            }
            if (!Regex.Match(this.psbPass.Password, @"[A-ZА-Я]{1}").Success)
            {
                message += "Пароль должен содержать минимум 1 прописную букву \n";
            }
            if (!Regex.Match(this.psbPass.Password, @"[a-zа-я]{1}").Success)
            {
                message += "Пароль должен содержать минимум 1 строчных знаков \n";
            }
            if (!Regex.Match(this.psbPass.Password, @"[!@#$%^&*()№;%:?*]").Success)
            {
                message += "Пароль должен содержать спецсимвол";
            }
            var diff = (DateTime.Now).Subtract(Convert.ToDateTime(dpBith.Text)).TotalDays;
            if (diff < 365 * 18)
            {
                message += "Пользователю должно быть более 18 лет";
            }
            if (String.IsNullOrEmpty(message))
            {
                try
                {
                    var user = new Users();
                    user.ID = DBConn.db.Users.Max(u => u.ID) + 1;
                    user.Email = txbEmail.Text;
                    user.FirstName = txbFirstName.Text;
                    user.LastName = txbLastName.Text;
                    user.RoleID = 2;
                    user.OfficeID = (cmbOffice.SelectedItem as Offices).ID;
                    user.Birthdate = Convert.ToDateTime(dpBith.Text);
                    user.Active = true;
                    user.Password = psbPass.Password;
                    DBConn.db.Users.Add(user);
                    DBConn.db.SaveChanges();
                    NavigationService.GetNavigationService(this).GoBack();
                }
                catch
                {
                    MessageBox.Show("произошла непредвиденная ошибка! Обратитесь к администратор");
                    Application.Current.Shutdown();

                }
            }
            else
                MessageBox.Show(message);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GetNavigationService(this).GoBack();
        }
    }
}
