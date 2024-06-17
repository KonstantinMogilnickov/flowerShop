using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

namespace flower
{
    /// <summary>
    /// Логика взаимодействия для Autorization.xaml
    /// </summary>
    public partial class Autorization : Window
    {
        private shopEntities db = new shopEntities();
        public Autorization()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text;
            string password = HashPassword(PasswordBox.Password);

            var user = db.Users.FirstOrDefault(u => u.login == login && u.password == password);

            if (user != null)
            {
                MessageBox.Show("Авторизация успешна!");
                if(user.id_role == 2)
                {
                    AdminPage adminPage = new AdminPage();
                    adminPage.Show();
                    this.Close();
                }

                Session session = new Session()
                {
                    id_user = user.id,
                    date = DateTime.Now
                };
                db.Sessions.Add(session);
                db.SaveChanges();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль.");
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }
    }
}
