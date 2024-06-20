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
    /// Логика взаимодействия для basket.xaml
    /// </summary>
    public partial class basket : Window
    {
        private List<CartItem> cartItems = new List<CartItem>();
        private client currentUser;
        public basket(List<CartItem> items, client client)
        {
            InitializeComponent();
            cartItems = items;
            this.currentUser = client;
            LoadCartItems();
            UpdateCartTotal();
            LoadCartFromDatabase();
        }

        private void LoadCartItems()
        {
            CartItemsPanel.Children.Clear();
            foreach (var item in cartItems)
            {
                var productCard = CreateProductCard(item);
                CartItemsPanel.Children.Add(productCard);
            }
        }

        private Border CreateProductCard(CartItem item)
        {
            var image = new Image
            {
                Width = 200,
                Height = 200,
                Margin = new Thickness(10),
                Stretch = Stretch.Uniform
            };

            if (!string.IsNullOrEmpty(item.Product.imagepath) && Uri.IsWellFormedUriString(item.Product.imagepath, UriKind.RelativeOrAbsolute))
            {
                image.Source = new BitmapImage(new Uri(item.Product.imagepath, UriKind.RelativeOrAbsolute));
            }
            else
            {
                image.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/placeholder.png"));
            }

            var priceText = new TextBlock
            {
                Text = $"{item.Product.price} рублей",
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var nameText = new TextBlock
            {
                Text = item.Product.name,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var decreaseButton = new Button { Content = "-", Tag = item.Product.id };
            decreaseButton.Click += DecreaseQuantity_Click;

            var increaseButton = new Button { Content = "+", Tag = item.Product.id };
            increaseButton.Click += IncreaseQuantity_Click;

            var quantityTextBox = new TextBox
            {
                Width = 30,
                Text = item.Quantity.ToString(),
                Tag = item.Product.id
            };
            quantityTextBox.PreviewTextInput += QuantityTextBox_PreviewTextInput;
            quantityTextBox.TextChanged += QuantityTextBox_TextChanged;

            var buttonPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center,
                Children = { decreaseButton, quantityTextBox, increaseButton }
            };

            var productPanel = new StackPanel
            {
                Children = { image, priceText, nameText, buttonPanel }
            };

            var border = new Border
            {
                Child = productPanel,
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1),
                Margin = new Thickness(5),
                Padding = new Thickness(10),
                Background = Brushes.White
            };

            return border;
        }


        private void DecreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var productId = (int)button.Tag;
            var textBox = FindQuantityTextBox(productId);
            int quantity = int.Parse(textBox.Text);

            if (quantity > 0)
            {
                quantity--;
                textBox.Text = quantity.ToString();

                if (quantity == 0)
                {
                    var result = MessageBox.Show("Удалить товар из корзины?", "Подтверждение", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        var itemToRemove = cartItems.FirstOrDefault(i => i.Product.id == productId);
                        cartItems.Remove(itemToRemove);
                        LoadCartItems();
                        UpdateCartTotal();
                    }
                }
                else
                {
                    UpdateCartTotal();
                }
            }
        }

        private void IncreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var productId = (int)button.Tag;
            var textBox = FindQuantityTextBox(productId);
            int quantity = int.Parse(textBox.Text);

            quantity++;
            textBox.Text = quantity.ToString();
            UpdateCartTotal();
        }

        private TextBox FindQuantityTextBox(int productId)
        {
            return CartItemsPanel.Children.OfType<Border>()
                .Select(border => border.Child as StackPanel)
                .SelectMany(panel => panel.Children.OfType<StackPanel>())
                .SelectMany(buttonPanel => buttonPanel.Children.OfType<TextBox>())
                .FirstOrDefault(tb => (int)tb.Tag == productId);
        }

        private void QuantityTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out _);
        }

        private void QuantityTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            int productId = (int)textBox.Tag;
            int quantity = 0;
            if (int.TryParse(textBox.Text, out quantity))
            {
                var item = cartItems.FirstOrDefault(i => i.Product.id == productId);
                if (item != null)
                {
                    item.Quantity = quantity;
                    UpdateCartTotal();
                }
            }
        }

        private void UpdateCartTotal()
        {
            decimal total = (decimal)cartItems.Sum(i => i.Product.price * i.Quantity);
            CheckoutButton.Content = $"Оформить ({total} рублей)";
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

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ClientMainWindow mainWindow = new ClientMainWindow(currentUser);
            mainWindow.Show();
            this.Close();
        }

        private void CheckoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to checkout page
        }

        private void LoadCartFromDatabase()
        {
            if (currentUser == null)
            {
                throw new InvalidOperationException("Текущий пользователь не установлен.");
            }

            using (var db = new shopEntities())
            {
                var cartItemsFromDb = db.carts
                    .Where(c => c.id_user == currentUser.id)
                    .Select(c => new
                    {
                        c.quantity,
                        c.product
                    }).ToList();

                cartItems.Clear();

                foreach (var item in cartItemsFromDb)
                {
                    var cartItem = new CartItem
                    {
                        Product = item.product,
                        Quantity = (int)item.quantity
                    };
                    cartItems.Add(cartItem);
                }
            }

            LoadCartItems();
            UpdateCartTotal();
        }
    }
}

