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

namespace Amonic.pages
{
    /// <summary>
    /// Логика взаимодействия для EditRole.xaml
    /// </summary>
    public partial class EditRole : Page
    {
        public EditRole()
        {
            InitializeComponent();
            //string parameter = string.Empty;

            // MessageBox.Show(parameter);
            //txbEmamil.Text = parameter;
           
            var usr = Convert.ToInt32(Application.Current.Properties["Users"].ToString());
            var users = DBConn.db.Users.Where(u => u.ID == usr).FirstOrDefault();
            txbEmamil.Text = users.Email;
            txbFirstName.Text = users.FirstName;
            txbLastName.Text = users.LastName;
            cmbOffice.ItemsSource = DBConn.db.Offices.ToList();
            cmbOffice.Text= users.Offices.Title.ToString();
            if (users.RoleID == 1)
            {
                rdbtnAdmin.IsChecked = true;
            }
            else if (users.RoleID == 2)
            {
                rdbtnUser.IsChecked = true;
            }
         
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            var usr = Convert.ToInt32(Application.Current.Properties["Users"].ToString());
            var users = DBConn.db.Users.Where(u => u.ID == usr).FirstOrDefault();
            if (rdbtnAdmin.IsChecked == true)
            {
                users.RoleID = 1;
            }
            else if (rdbtnUser.IsChecked == true)
            {
                users.RoleID = 2;
            }
            DBConn.db.SaveChanges();
            MessageBox.Show("роль пользователя успешно изменена!");
            NavigationService.GetNavigationService(this).GoBack();

            /*
            var usr2 = new Users();
            usr2.ID = 10;
            usr2.Birthdate = DateTime.Now;
            usr2.Password = "1234";
            usr2.FirstName = "3";
            usr2.LastName = "3";
            usr2.Email = "3@3.ru";
            usr2.Active = true;
            usr2.RoleID = 2;
            usr2.OfficeID= (cmbOffice.SelectedItem as Offices).ID;
            DBConn.db.Users.Add(usr2);
            DBConn.db.SaveChanges();*/
        }
        /// <summary>
        /// переход на менб администратора
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GetNavigationService(this).GoBack();
        }
    }
}
