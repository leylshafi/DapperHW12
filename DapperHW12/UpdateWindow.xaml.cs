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

namespace DapperHW12
{
    /// <summary>
    /// Interaction logic for UpdateWindow.xaml
    /// </summary>
    public partial class UpdateWindow : Window
    {
        public Product Product { get; set; }

        public UpdateWindow(Product product)
        {
            InitializeComponent();
            DataContext = this;

            Product = product;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder builder = new StringBuilder();

            if (string.IsNullOrWhiteSpace(Product.Name))
                builder.Append($"{nameof(Product.Name)} Can Not Be Null\n");

            if (Product.Price <= 0)
                builder.Append($"{nameof(Product.Price)} Price>0\n");

            if (Product.Stock < 0)
                builder.Append($"{nameof(Product.Stock)} Price>=0\n");

            if (builder.Length > 0)
            {
                MessageBox.Show(builder.ToString());
                return;
            }

            DialogResult = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
