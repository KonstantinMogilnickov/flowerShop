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
            if (ValidateField())
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
                    gender = gender

                };
                db.clients.Add(client);
                db.SaveChanges();


                MessageBox.Show("Регистрация успешна!");

                // Добавьте логику для сохранения данных в базу данных или другую необходимую логику
            }
        }

        private bool ValidateField()
        {
            string userName = txtName.Text.Trim();
            string userSurname = txtSurname.Text.Trim();
            string userPatronymic = txtPatronymic.Text.Trim();
            string userPhone = txtPhone.Text.Trim();
            string userEmail = txtEmail.Text.Trim();
            string userLogin = txtLogin.Text.Trim();

            string userPassword = txtPassword.Text.Trim(); 

            if (string.IsNullOrEmpty(userName) ||
                string.IsNullOrEmpty(userSurname) ||
                string.IsNullOrEmpty(userPatronymic) ||
                string.IsNullOrEmpty(userPhone) ||
                string.IsNullOrEmpty(userEmail) ||
                string.IsNullOrEmpty(userLogin) ||
                string.IsNullOrEmpty(userPassword))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            EmployeeMainWindow employeeMainWindow = new EmployeeMainWindow();
            employeeMainWindow.Show();
            this.Close();
        }
    }
}
