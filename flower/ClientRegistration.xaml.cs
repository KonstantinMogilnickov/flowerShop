using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для ClientRegistration.xaml
    /// </summary>
    public partial class ClientRegistration : Window
    {
        private shopEntities db = new shopEntities();
       
        public ClientRegistration()
        {
            InitializeComponent();
        }

        private void btnRegistration_Click(object sender, RoutedEventArgs e)
        {
            string gender = "";
            if(rbMan.IsChecked == true)
            {
                gender = "М";
            }
            else
            {
                gender = "Ж";
            }
            if (ValidateInputs())
            {
                client client = new client()
                {
                    surname = txtSurname.Text.Trim(),
                    name = txtName.Text.Trim(),
                    patronymic = txtPatronymic.Text.Trim(),
                    phone = txtPhone.Text.Trim(),
                    email = txtEmail.Text.Trim(),
                    street = txtStreet.Text.Trim(),
                    home = txtHome.Text.Trim(),
                    flat = txtFlat.Text.Trim(),
                    entrance = txtEntrance.Text.Trim(),
                    floor = txtFloor.Text.Trim(),
                    date_of_birth = dpDateOfBirth.SelectedDate,
                    gender = gender,
                    login = txtLogin.Text,
                    passwordHash = HashPassword(txtPassword.Password)

                };
                db.clients.Add(client);
                db.SaveChanges();


                MessageBox.Show("Регистрация успешна!");
                EmployeeMainWindow employeeMainWindow = new EmployeeMainWindow();
                employeeMainWindow.Show();
                Close();
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            EmployeeMainWindow employeeMainWindow = new EmployeeMainWindow();
            employeeMainWindow.Show();
            this.Close();
        }
        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtSurname.Text) ||
                string.IsNullOrWhiteSpace(txtPatronymic.Text) ||
                string.IsNullOrWhiteSpace(txtPhone.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtStreet.Text) ||
                string.IsNullOrWhiteSpace(txtHome.Text) ||
                string.IsNullOrWhiteSpace(txtFlat.Text) ||
                string.IsNullOrWhiteSpace(txtEntrance.Text)||
                string.IsNullOrWhiteSpace(txtFloor.Text) ||
                string.IsNullOrWhiteSpace(txtLogin.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Password) ||
                string.IsNullOrWhiteSpace(dpDateOfBirth.Text)
                )
            {
                MessageBox.Show("Все поля обязательны для заполнения.");
                return false;
            }

            if (!Regex.IsMatch(txtEmail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Некорректный формат email.");
                return false;
            }

            //if (!ValidatePassword(txtPassword.Password))
            //{
            //    MessageBox.Show("Пароль должен быть не менее 6 символов и содержать цифру.");
            //    return false;
            //}

            return true;
        }

        private bool ValidatePassword(string password)
        {
            if (password.Length < 6)
                return false;

            if (!password.Any(char.IsDigit))
                return false;

            return true;
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
