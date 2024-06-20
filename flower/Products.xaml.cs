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
    /// Логика взаимодействия для Products.xaml
    /// </summary>
    public class CartItem
    {
        public product Product { get; set; }
        public int Quantity { get; set; }
    }
    public partial class Products : Window
    {
        private List<CartItem> cartItems = new List<CartItem>();
        private shopEntities db = new shopEntities();
        private client currentUser;
        private int category;
        public Products(int category, client client)
        {
            InitializeComponent();
            this.category = category;
            this.currentUser = client;
            LoadProducts();
        }

        private void LoadProducts()
        {
            var products = db.products.Where(p => p.id_category == category).ToList();
            foreach (var product in products)
            {
                var productCard = CreateProductCard(product);
                ProductsPanel.Children.Add(productCard);
            }
        }

        private Border CreateProductCard(product product)
        {
            var image = new Image
            {
                Width = 200,
                Height = 200,
                Margin = new Thickness(10),
                Stretch = Stretch.Uniform
            };

            // Проверка наличия изображения и установка пути
            if (!string.IsNullOrEmpty(product.imagepath) && Uri.IsWellFormedUriString(product.imagepath, UriKind.RelativeOrAbsolute))
            {
                image.Source = new BitmapImage(new Uri(product.imagepath, UriKind.RelativeOrAbsolute));
            }
            else
            {
                // Заглушка, если изображение отсутствует
                image.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/placeholder.png"));
            }

            var priceText = new TextBlock
            {
                Text = $"{product.price} рублей",
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var nameText = new TextBlock
            {
                Text = product.name,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var decreaseButton = new Button { Content = "-", Tag = product.id };
            decreaseButton.Click += DecreaseQuantity_Click;

            var increaseButton = new Button { Content = "+", Tag = product.id };
            increaseButton.Click += IncreaseQuantity_Click;

            var quantityTextBox = new TextBox
            {
                Width = 30,
                Text = "0",
                Tag = product.id
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
                Tag = product.id
            };
            addToCartButton.Click += AddToCart_Click;

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
            return ProductsPanel.Children.OfType<Border>()
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
            // Реализуйте перерасчет корзины здесь
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var productId = (int)button.Tag;
            var textBox = FindQuantityTextBox(productId);
            int quantity = int.Parse(textBox.Text);

            if (quantity > 0)
            {
                var product = db.products.FirstOrDefault(p => p.id == productId);
                AddToCart(product, quantity);
                var cartItem = new CartItem { Product = product, Quantity = quantity };
                button.IsEnabled = false;
                SaveCartItemToDatabase(cartItem);
            }
            else
            {
                MessageBox.Show("Выберите количество товара для добавления в корзину.");
            }
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

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ClientMainWindow clientMainWindow = new ClientMainWindow(currentUser);
            clientMainWindow.Show();
            this.Close();
        }

        private void CartButton_Click(object sender, RoutedEventArgs e)
        {
            basket cartWindow = new basket(cartItems, currentUser);
            cartWindow.Show();
            this.Close();
        }

        private void AddToCart(product product, int quantity)
        {
            var existingItem = cartItems.FirstOrDefault(item => item.Product.id == product.id);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cartItems.Add(new CartItem { Product = product, Quantity = quantity });
       
            }

            // Обновляем корзину, если необходимо
            UpdateCartTotal();
        }
        private void UpdateCartTotal()
        {
      
            decimal total = (decimal)cartItems.Sum(item => item.Product.price * item.Quantity);
           
        }

        private void SaveCartItemToDatabase(CartItem cartItem)
        {
            // Проверка, что все необходимые объекты не равны null
            if (cartItem == null)
            {
                throw new ArgumentNullException(nameof(cartItem));
            }

            if (cartItem.Product == null)
            {
                throw new ArgumentNullException(nameof(cartItem.Product));
            }

            if (currentUser == null)
            {
                throw new InvalidOperationException("Текущий пользователь не установлен.");
            }

            if (db == null)
            {
                throw new InvalidOperationException("Контекст базы данных не инициализирован.");
            }

            var existingCartItem = db.carts.FirstOrDefault(ci => ci.id_product == cartItem.Product.id && ci.id_user == currentUser.id);

            if (existingCartItem != null)
            {
                // Обновляем количество и сумму существующего элемента в корзине
                existingCartItem.quantity += cartItem.Quantity;
                existingCartItem.amount = (int)(existingCartItem.quantity * cartItem.Product.price);
            }
            else
            {
                int summ = (int)(cartItem.Product.price * cartItem.Quantity);
                // Добавляем новый элемент в корзину
                cart basket = db.carts.Add(new cart
                {
                    id_product = cartItem.Product.id,
                    id_user = currentUser.id, // Предположим, что у вас есть текущий пользователь
                    quantity = cartItem.Quantity,
                    amount = summ
                });
            }

            db.SaveChanges();
        }
    }
}
