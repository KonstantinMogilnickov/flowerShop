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
    /// Логика взаимодействия для AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Window
    {
        public AdminPage()
        {
            InitializeComponent();
        }

        private void EmployeesListButton_Click(object sender, RoutedEventArgs e)
        {
            UsersList usersList = new UsersList();
            usersList.Show();
            this.Close();
        }

        private void LoginGraphButton_Click(object sender, RoutedEventArgs e)
        {
            LoginGraphPage loginGraphPage = new LoginGraphPage();
            loginGraphPage.Show();
            this.Close();
        }
    }
}
