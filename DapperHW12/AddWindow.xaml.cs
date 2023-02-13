using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
using Dapper;

namespace DapperHW12
{
    /// <summary>
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        public Product Product { get; set; }
        int id= 0;
        public AddWindow()
        {
            InitializeComponent();

            DataContext = this;
            Product = new();
            var con = new SqlConnection();
            var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
            con = new SqlConnection(configuration.GetConnectionString("conStr1"));
            var command = "SELECT * FROM PRODUCT";
            var reader = con.ExecuteReader(command);
            var table = new DataTable();
            table.Load(reader);
            Product.Id = table.Rows.Count;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            StringBuilder builder = new StringBuilder();

            if (string.IsNullOrWhiteSpace(Product.Name))
                builder.Append($"{nameof(Product.Name)} Cannot Be Empty\n");

            if (Product.Price <= 0)
                builder.Append($"{nameof(Product.Price)} Cannot be below or equal to 0\n");

            if (Product.Stock < 0)
                builder.Append($"{nameof(Product.Stock)} Cannot be below 0\n");

            if (builder.Length > 0)
            {
                MessageBox.Show(builder.ToString());
                return;
            }

            DialogResult = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
