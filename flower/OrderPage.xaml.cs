using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace flower
{
    /// <summary>
    /// Логика взаимодействия для OrderPage.xaml
    /// </summary>
    public partial class OrderPage : Window
    {
        private List<CartItem> cartItems;
        private client currentUser;
        private decimal totalAmount;
        public OrderPage(List<CartItem> items, client user, decimal total)
        {
            InitializeComponent();
            cartItems = items;
            currentUser = user;
            totalAmount = total;
            LoadUserData();
            DisplayOrderDetails();
        }

        private void DisplayOrderDetails()
        {
            OrderCostTextBlock.Text = $"Стоимость: {totalAmount} рублей";
        }

        private void LoadUserData()
        {
            LastNameTextBox.Text = currentUser.surname;
            FirstNameTextBox.Text = currentUser.name;
            MiddleNameTextBox.Text = currentUser.patronymic;
            PhoneTextBox.Text = currentUser.phone;
            EmailTextBox.Text = currentUser.email;
            StreetTextBox.Text = currentUser.street;
            HouseTextBox.Text = currentUser.home;
            ApartmentTextBox.Text = currentUser.flat;
            EntranceTextBox.Text = currentUser.entrance;
            FloorTextBox.Text = currentUser.floor;
        }

        private bool ValidateInput()
        {
    
            if (string.IsNullOrWhiteSpace(LastNameTextBox.Text) || LastNameTextBox.Text.Length > 20 ||
                string.IsNullOrWhiteSpace(FirstNameTextBox.Text) || FirstNameTextBox.Text.Length > 20 ||
                string.IsNullOrWhiteSpace(MiddleNameTextBox.Text) || MiddleNameTextBox.Text.Length > 20)
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля корректно.");
                return false;
            }

            if (!Regex.IsMatch(EmailTextBox.Text, @"^[^\s@]+@[^\s@]+\.[^\s@]+$") || EmailTextBox.Text.Length > 40)
            {
                MessageBox.Show("Пожалуйста, введите корректный email.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(StreetTextBox.Text) || StreetTextBox.Text.Length > 30 ||
                string.IsNullOrWhiteSpace(HouseTextBox.Text) || HouseTextBox.Text.Length > 5 ||
                string.IsNullOrWhiteSpace(ApartmentTextBox.Text) || ApartmentTextBox.Text.Length > 5)
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля корректно.");
                return false;
            }

 
            if (!string.IsNullOrWhiteSpace(EntranceTextBox.Text) && !int.TryParse(EntranceTextBox.Text, out _) ||
                !string.IsNullOrWhiteSpace(FloorTextBox.Text) && !int.TryParse(FloorTextBox.Text, out _))
            {
                MessageBox.Show("Пожалуйста, введите корректные данные для подъезда и этажа.");
                return false;
            }

            return true;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ClientMainWindow clientMainWindow = new ClientMainWindow(currentUser);
            clientMainWindow.Show();
            this.Close();
        }

        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                using (var db = new shopEntities())
                {
                    var newOrder = new order
                    {
                        id_user = currentUser.id,
                        amount = (int?)totalAmount,
                        quantity = cartItems.Sum(i => i.Quantity)
                    };

                    db.orders.Add(newOrder);
                    db.SaveChanges();

                    foreach (var item in cartItems)
                    {
                        var productInOrder = new product_in_order
                        {
                            id_order = newOrder.id,
                            id_product = item.Product.id,
                            quantity = item.Quantity,
                            price = item.Product.price
                        };

                        db.product_in_order.Add(productInOrder);
                    }

                    db.SaveChanges(); 

    
                    foreach (var item in cartItems)
                    {
                        var productToRemove = db.carts.FirstOrDefault(p => p.id_product == item.Product.id);
                        if (productToRemove != null)
                        {
                            db.carts.Remove(productToRemove);
                        }
                    }

                    db.SaveChanges();
                    db.SaveChanges(); 

       
                    cartItems.Clear();


                    MessageBox.Show("Заказ успешно оформлен!");

                }
            }
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
    }
}
