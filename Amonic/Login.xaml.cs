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
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        private int timeLeft;
        // количество неверных входов
        public int numError = 0;
        public Login()
        {
            InitializeComponent();
           
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            //закрытие приложения
            Application.Current.Shutdown();
            

           

        }
        /// <summary>
        /// проверка пользователя по БД
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            
            var login = txbLogin.Text;
            var pas = psbPass.Password;
           //экземпляр трекинг
            var trackUser = new Tracking();
            //если количество неудачных входов меньше 4
            if (numError < 4)
            {
                //вызываем метод проверки пользователя по БД
                var user = TestUser(login, pas);
                //если нашли запись в БД
                if (user != null)
                {
                    //проверяем роль пользователя
                    var role = TestRole(user);
                    //дата входа=текущему времени и дате
                    trackUser.Login = DateTime.Now;
                    //идентификатор=залогиненному пользователю
                    trackUser.UserID = user.ID;
                    //в ресурсы записываем идентификатор авторизованного пользователя
                    Application.Current.Resources["LogUser"] = user.ID;
                    //если в таблице трекинга есть записи
                    if (DBConn.db.Tracking.Count() >0)
                    {
                        //делаем инкремент по идентификатору
                        trackUser.TrackID = (int)(DBConn.db.Tracking.Max(t => t.TrackID) + 1);
                    }
                    else
                    {
                        //присваем идентификатору значение 1
                        trackUser.TrackID = 1;
                    }
                    //если учетная запись заблокирована
                    if (user.Active == false)
                    {
                        //выдаем сообщение пользователю
                        MessageBox.Show("Учетная запись заблокирована администратором!");
                        //записываем время выхода на текущее
                        trackUser.Logout = DateTime.Now;
                        //указываем причину 
                        trackUser.Reason = "отказ системы. Пользователь заблокирован";
                        //добавляем и сохраняем
                        DBConn.db.Tracking.Add(trackUser);
                        DBConn.db.SaveChanges();
                    }
                    //если учетная запись не заблокирована
                    else
                    {
                        //сохраняем запись в таблицу трекинг
                        DBConn.db.Tracking.Add(trackUser);
                        DBConn.db.SaveChanges();
                       
                        //trackUser
                        //тестируем роль пользователя
                        switch (role)
                        {
                            //если администратор
                            case 1:
                                {

                                    //MessageBox.Show("admin!");
                                    NavigationService.GetNavigationService(this).Navigate(new Uri("pages/AdminMenu.xaml", UriKind.RelativeOrAbsolute));
                                    break;
                                }
                            //если пользователь
                            case 2:
                                {

                                    //MessageBox.Show("user!");
                                    NavigationService.GetNavigationService(this).Navigate(new Uri("pages/UserMenu.xaml", UriKind.RelativeOrAbsolute));
                                    break;
                                }
                            // в остальных случаях
                            default:
                                {
                                    break;
                                }
                        }
                    }
                }
            }
            else
            {
                //если больше 4 неудачных входов
                timeLeft = 10;
                lblTime.Visibility = Visibility;
                txbLogin.IsEnabled = false;
                psbPass.IsEnabled = false;
                var dispetcherTimer = new System.Windows.Threading.DispatcherTimer();
                dispetcherTimer.Tick += new EventHandler(dispetcherTimer_Tick);
                dispetcherTimer.Interval = new TimeSpan(0, 0, 1);
                dispetcherTimer.Start();

                /*
                lblTime.Visibility = Visibility;
                lblTime.Content = DateTime.Now;

                txbLogin.IsEnabled = false;
                psbPass.IsEnabled = false;
                */

            }

        }
        private void dispetcherTimer_Tick(object sender,EventArgs e)
        {
            if (timeLeft <= 10 && timeLeft >= 0)
            {

                //var start = new Timer();

                lblTime.Content = timeLeft.ToString();
                timeLeft--;

            }
            else
            {
                lblTime.Visibility=Visibility.Hidden;
                txbLogin.IsEnabled = true;
                psbPass.IsEnabled = true;
            }
        }
        /// <summary>
        /// метод, проверяющий пользователя по БД
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <param name="pas">Пароль пользователя</param>
        /// <returns></returns>
        public Users TestUser(string login, string pas)
        {
            //ищем пользователя в таблице пользователя с логином и паролем
            var user = DBConn.db.Users.Where(u => u.Email == login && u.Password == pas).FirstOrDefault();
            //если пользователь найден
            if (user != null)
            {
                return user;
            }
            //неудачный вход
            else
            {
                MessageBox.Show("такого пользователя не найдено в системе!");

             //суммируем количество неудачных входов
                numError++;
                return null;
            }
        }
        /// <summary>
        /// метод, проверяющий роль пользователя
        /// </summary>
        /// <param name="user">пользователь, передаваемый после проверки</param>
        /// <returns></returns>
        public int TestRole(Users user)
        {
            //если админитсратор
            if (user.RoleID == 1)
            {
                return 1;
            }
            //если пользователь
            else
            { return 2; }
        }
        
    }
}
