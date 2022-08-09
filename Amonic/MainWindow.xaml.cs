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

namespace Amonic
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            frmNavigate.Content = new Login();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            //дописать закрытие, если ничего не ввели в поле
            if ((Application.Current.Resources["LogUser"])==null)
                {
                return;
            }
            else
            
            {
                var usr = Convert.ToInt32((Application.Current.Resources["LogUser"]).ToString());
                var usertrack = DBConn.db.Tracking.Where(t => t.UserID == usr && t.Logout == null).FirstOrDefault();
                usertrack.Logout = DateTime.Now;
                DBConn.db.SaveChanges();
            }
        }
    }
}
