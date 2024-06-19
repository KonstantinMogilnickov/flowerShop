using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace flower
{
    public partial class Cart : Window
    {
        private List<CartItem> cartItems = new List<CartItem>();

        public Cart(List<CartItem> items)
        {
            InitializeComponent();
            cartItems = items;
            LoadCartItems();
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

            if (!string.IsNullOrEmpty(item.Product.ImagePath) && Uri.IsWellFormedUriString(item.Product.ImagePath, UriKind.RelativeOrAbsolute))
            {
                image.Source = new BitmapImage(new Uri(item.Product.ImagePath, UriKind.RelativeOrAbsolute));
            }
            else
            {
                image.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/placeholder.png"));
            }

            var priceText = new TextBlock
            {
                Text = $"{item.Product.Price} рублей",
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var nameText = new TextBlock
            {
                Text = item.Product.Name,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var decreaseButton = new Button { Content = "-", Tag = item.Product.Id };
            decreaseButton.Click += DecreaseQuantity_Click;

            var increaseButton = new Button { Content = "+", Tag = item.Product.Id };
            increaseButton.Click += IncreaseQuantity_Click;

            var quantityTextBox = new TextBox
            {
                Width = 30,
                Text = item.Quantity.ToString(),
                Tag = item.Product.Id
            };
            quantityTextBox.PreviewTextInput += QuantityTextBox_PreviewTextInput;
            quantityTextBox.TextChanged += QuantityTextBox_TextChanged;

            var buttonPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center,
                Children = { decreaseButton, quantityTextBox, increaseButton }
            };

            var addToCartButton = new Button
            {
                Content = "В корзину",
                Tag = item.Product.Id,
                IsEnabled = false
            };

            var productPanel = new StackPanel
            {
                Children = { image, priceText, nameText, buttonPanel, addToCartButton }
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
                        var itemToRemove = cartItems.FirstOrDefault(i => i.Product.Id == productId);
                        cartItems.Remove(itemToRemove);
                        LoadCartItems();
                    }
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
            decimal total = cartItems.Sum(i => i.products.price * i.Quantity);
            CheckoutButton.Content = $"Оформить ({total} рублей)";
        }

        private void VK_Click(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "https://vk.com/syrprizko",
                UseShellExecute = true
            });
        }

        private void Instagram_Click(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "https://www.instagram.com/teddyflowers_perm/",
                UseShellExecute = true
            });
        }

        private void Telegram_Click(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "https://web.telegram.org/",
                UseShellExecute = true
            });
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void CheckoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to checkout page
        }
        private void OpenVK(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://vk.com/syrprizko");
        }

        private void OpenInstagram(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.instagram.com/teddyflowers_perm/");
        }

        private void OpenTelegram(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://web.telegram.org/");
        }
    }

    public class CartItem
    {
        public product Product { get; set; }
        public int Quantity { get; set; }
    }
}
