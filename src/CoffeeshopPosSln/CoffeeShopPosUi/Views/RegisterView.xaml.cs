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
using CoffeeShopPosUi.ViewModels;

namespace CoffeeShopPosUi.Views
{
    /// <summary>
    /// Logique d'interaction pour RegisterView.xaml
    /// </summary>
    public partial class RegisterView : Window
    {
        public RegisterView()
        {
            InitializeComponent();
        }
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as RegisterViewModel;
            if (viewModel != null)
            {
                viewModel.Password = ((PasswordBox)sender).Password;
            }
        }
    }
}
