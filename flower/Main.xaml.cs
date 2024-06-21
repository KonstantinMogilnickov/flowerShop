using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Security.Cryptography;

namespace flower
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        shopEntities db = new shopEntities();
        public Main()
        {
            InitializeComponent();
            LoadComboBoxDivision();
        }
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInputs())
            {
                if (db.Users.Any(emp => emp.login == LoginTextBox.Text))
                {
                    MessageBox.Show("Логин уже занят.");
                    return;
                }
                division selectedDivision = (division)DepartmentComboBox.SelectedItem;

                var user = new User
                {
                    surname = LastNameTextBox.Text,
                    name = FirstNameTextBox.Text,
                    patrynumic = MiddleNameTextBox.Text,
                    passport = PassportNumberTextBox.Text,
                    division = selectedDivision,
                    phone = PhoneNumberTextBox.Text,
                    email = EmailTextBox.Text,
                    login = LoginTextBox.Text,
                    password = HashPassword(PasswordBox.Password),
                    id_role = 1
                };

                db.Users.Add(user);
                db.SaveChanges();
                MessageBox.Show("Регистрация прошла успешно!");
                MainWindow main = new MainWindow();
                main.Show();
                this.Close();
            }
        }

        private void LoadComboBoxDivision()
        {
            var division1 = db.divisions.ToList();
            DepartmentComboBox.ItemsSource = division1;
            DepartmentComboBox.DisplayMemberPath = "name";
        }


        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(LastNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(FirstNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(PassportNumberTextBox.Text) ||
                string.IsNullOrWhiteSpace(DepartmentComboBox.Text) ||
                string.IsNullOrWhiteSpace(PhoneNumberTextBox.Text) ||
                string.IsNullOrWhiteSpace(EmailTextBox.Text) ||
                string.IsNullOrWhiteSpace(LoginTextBox.Text) ||
                string.IsNullOrWhiteSpace(PasswordBox.Password) ||
                string.IsNullOrWhiteSpace(ConfirmPasswordBox.Password))
            {
                MessageBox.Show("Все поля обязательны для заполнения.");
                return false;
            }

            if (!Regex.IsMatch(EmailTextBox.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Некорректный формат email.");
                return false;
            }

            if (PasswordBox.Password != ConfirmPasswordBox.Password)
            {
                MessageBox.Show("Пароли не совпадают.");
                return false;
            }

            if (!ValidatePassword(PasswordBox.Password))
            {
                MessageBox.Show("Пароль должен быть не менее 6 символов и содержать цифру.");
                return false;
            }

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void back(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
