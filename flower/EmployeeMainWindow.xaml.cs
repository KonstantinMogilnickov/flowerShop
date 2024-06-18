﻿using System;
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
    /// Логика взаимодействия для EmployeeMainWindow.xaml
    /// </summary>
    public partial class EmployeeMainWindow : Window
    {
        public EmployeeMainWindow()
        {
            InitializeComponent();
        }

        private void btnRegisterClient_Click(object sender, RoutedEventArgs e)
        {
            ClientRegistration clientRegistration = new ClientRegistration();
            clientRegistration.Show();
            this.Close();
        }
    }
}
