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
    /// Логика взаимодействия для LoginGraphPage.xaml
    /// </summary>
    public partial class LoginGraphPage : Window
    {
        private shopEntities db = new shopEntities();
        public LoginGraphPage()
        {
            InitializeComponent();
            LoadEmployeeFilterComboBox();
            LoadAllSessions();
        }
        private void LoadEmployeeFilterComboBox()
        {
            var employees = db.Users.ToList();
            EmployeeFilterComboBox.ItemsSource = employees;
        }

        private void LoadAllSessions()
        {
            var sessions = db.Sessions.Include("User").ToList();
            LoginDataGrid.ItemsSource = sessions;
        }

        private void LoadSessionsByEmployee(int userId)
        {
            var sessions = db.Sessions.Include("User")
                                      .Where(s => s.id_user == userId)
                                      .ToList();
            LoginDataGrid.ItemsSource = sessions;
        }

        private void EmployeeFilterComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var selectedEmployee = EmployeeFilterComboBox.SelectedItem as User;
            if (selectedEmployee != null)
            {
                LoadSessionsByEmployee(selectedEmployee.id);
            }
            else
            {
                LoadAllSessions();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            AdminPage adminPage = new AdminPage();
            adminPage.Show();
            this.Close();
        }
    }
}
