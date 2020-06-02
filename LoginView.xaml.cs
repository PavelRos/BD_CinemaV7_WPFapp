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
using System.Windows.Shapes;
using BD_CinemaV7.Models;
using BD_CinemaV7.Context;
using System.Security.Cryptography;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public static int id_user;
        public LoginView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            User user = null;
            string pass;
            byte[] bytes = Encoding.Unicode.GetBytes(PassBox.Password);
            MD5CryptoServiceProvider CSP =
                new MD5CryptoServiceProvider();
            using (MsSqlContext db = new MsSqlContext())
            {
                byte[] byteHash = CSP.ComputeHash(bytes);
                pass = string.Empty;
                foreach (byte b in byteHash)
                    pass += string.Format("{0:x2}", b);
                user = db.Users.FirstOrDefault(u => u.Email == LoginBox.Text && u.Password == pass);
                if (user != null)
                {
                    if (user.RoleId == 0)
                    {
                        MessageBox.Show("Пользователь найден, авторизация прошла успешно");
                        id_user = user.Id;
                        HallView_ hall = new HallView_();
                        hall.Show();
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Данный пользователь не обладает правами администратора");
                        return;
                    }
                   
                }
                else
                {
                    MessageBox.Show("Пользователя с таким логином или паролем не существует");
                    return;

                }

            }
        }
    }
}
