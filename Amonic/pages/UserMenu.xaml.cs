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
using System.Data.Entity;

namespace Amonic.pages
{
    /// <summary>
    /// Логика взаимодействия для UserMenu.xaml
    /// </summary>
    public partial class UserMenu : Page
    {
        public UserMenu()
        {
            InitializeComponent();
            var user = Convert.ToInt32(Application.Current.Resources["LogUser"].ToString());
            grdLog.ItemsSource = DBConn.db.Tracking.Where(t => t.UserID == user).ToList();
            var logusr = DBConn.db.Users.Where(u=>u.ID==user).FirstOrDefault();
            int crash = 0;
            foreach (Tracking crashnum in  DBConn.db.Tracking.Where(u => u.UserID == user && u.Reason != null))
            {
                crash++;
            }

            double timeSpent = 0;
            foreach (Tracking sessionTime in DBConn.db.Tracking.Where(u => u.UserID == user && u.Logout != null)) {
                timeSpent += sessionTime.Logout.Value.Subtract(sessionTime.Login).TotalMinutes;
            }
            
            int timeSpentH=0;
            if (timeSpent > 60 * 24)
            {
                while (timeSpent > 60*24)
                {
                    var timeSpentD = (int)timeSpent % (60*24);
                    timeSpent = timeSpent - 60*24;
                }
            }
            //hours
            if (timeSpent > 60)
            {
                while (timeSpent > 60)
                {
                   timeSpentH = (int)timeSpent % 60;
                    timeSpent = timeSpent - 60;
                }
            }
            else
            {
                timeSpentH = 0;
            }
                       
            var time = new TimeSpan((int)timeSpentH, (int)timeSpent,0);

            MessageBox.Show(timeSpentH.ToString()+"  "+timeSpent.ToString());
            txblHi.Text = String.Format("Hi {0} {1}, Welcome to Amonic Airlines \n Time spent on system {2}     Number of crashes: {3}",logusr.FirstName,logusr.LastName,time.ToString(),crash);
        }

        private void btnExit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();

        }
    }
}
