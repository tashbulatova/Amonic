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
using System.Windows.Forms.DataVisualization.Charting;


namespace Amonic.pages
{
    /// <summary>
    /// Логика взаимодействия для AdminMenu.xaml
    /// </summary>
    public partial class AdminMenu : Page
    {
        public AdminMenu()
        {
            InitializeComponent();
            grdUser.ItemsSource = DBConn.db.Users.ToList();
            cmbOffice.ItemsSource= DBConn.db.Offices.ToList();
            //создание графика
            grafics.ChartAreas.Add(new ChartArea("Default"));
            grafics.Series.Add(new Series("Series1"));
            grafics.Series["Series1"].ChartArea = "Default";
            grafics.Series["Series1"].ChartType = SeriesChartType.Line;
           
            var numData = DBConn.db.Users.Count();
            string[] stringX = new string[numData];
            double[] stringY = new double[numData] ;
            int i = 0;
            foreach (Users user in DBConn.db.Users.ToList())
            {

                stringX[i] = user.FirstName;
                stringY[i] = user.Bith;
                i++;
                
            }

           
            //string[] dataX = new string[] { "36","38" };
            //double[] dataY = new double[] { 1 , 2};
            grafics.Series["Series1"].Points.DataBindXY(stringX, stringY);


        }
        

        private void cmbOffice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var office = (this.cmbOffice.SelectedValue as Offices).ID;
            
            var off = DBConn.db.Users.Where(o => o.OfficeID == office).ToList();
            

           
            grdUser.ItemsSource = off;
            

        }

        private void btnRole_Click(object sender, RoutedEventArgs e)
        {
            var usr = grdUser.SelectedItem as Users;
            //  MessageBox.Show(usr.ID.ToString());
            if (usr == null)
            {
                MessageBox.Show("Не выбран пользователь для изменения роли");
            }
            else
            {
                Application.Current.Properties["Users"] = usr.ID;
                NavigationService.GetNavigationService(this).Navigate(new Uri("/pages/EditRole.xaml", UriKind.Relative));
            }
        }

        private void editRunner(object sender, RoutedEventArgs e)
        {

        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var usr = grdUser.SelectedItem as Users;
            if (usr.Active == true)
            {
                usr.Active = false;
            }
            else if (usr.Active == false)
            {
                usr.Active = true;
            }
            else
            {
                MessageBox.Show("состояний кроме заблокирован/разблокирован не предусмотрено!!!");
            }
            DBConn.db.SaveChanges();
        }

        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GetNavigationService(this).Navigate(new Uri("/pages/AddUser.xaml", UriKind.RelativeOrAbsolute));
        }

        private void grdUser_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            //var row=grdUser.ItemContainerGenerator.ContainerFromItem(e) as DataGridRow;
            //if (row.)
            //if (Convert.ToInt32(grdUser.Columns[2])>25)
          /* 
          foreach (e.Row col in grdU)
            if (usr.Bith>30)
            {
                e.Row.Background = Brushes.Red;
            }
            */
            
        }
    }
}
