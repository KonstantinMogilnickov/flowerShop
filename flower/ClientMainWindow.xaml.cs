using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Логика взаимодействия для ClientMainWindow.xaml
    /// </summary>
   
    public partial class ClientMainWindow : Window
    {
        private client currentUser;
        public ClientMainWindow(client client)
        {
            InitializeComponent();
            this.currentUser = client;
        }

        private void OpenVK(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://vk.com/syrprizko",
                UseShellExecute = true
            });
        }

        private void OpenInstagram(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://www.instagram.com/teddyflowers_perm/",
                UseShellExecute = true
            });
        }

        private void OpenTelegram(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://web.telegram.org/",
                UseShellExecute = true
            });
        }

        private void FreshFlowers_Click(object sender, MouseButtonEventArgs e)
        {
            Products products = new Products(2, currentUser);
            products.Show();
            this.Close();
        }

        private void Bouquets_Click(object sender, MouseButtonEventArgs e)
        {
            Products products = new Products(1, currentUser);
            products.Show();
            this.Close();
        }

        private void IndoorPlants_Click(object sender, MouseButtonEventArgs e)
        {
            Products products = new Products(3, currentUser);
            products.Show();
            this.Close();
        }

        private void Seedlings_Click(object sender, MouseButtonEventArgs e)
        {
            Products products = new Products(4, currentUser);
            products.Show();
            this.Close();
        }

        private void CartButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Category_MouseEnter(object sender, MouseEventArgs e)
        {
            var border = sender as Border;
            border.Background = new SolidColorBrush(Colors.LightGray);
        }

        private void Category_MouseLeave(object sender, MouseEventArgs e)
        {
            var border = sender as Border;
            border.Background = new SolidColorBrush(Colors.Transparent);
        }

        
    }
}
