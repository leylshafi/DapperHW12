using ControlzEx.Standard;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Z.Dapper.Plus;

namespace DapperHW12;

public partial class MainWindow : Window
{
    SqlConnection con;
    string conStr = string.Empty;
    string command = string.Empty;
    DataTable table;
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] String propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    private List<Product>? _products;

    public List<Product>? Products
    {
        get => _products;
        set
        {
            _products = value;
            OnPropertyChanged();
        }
    }
    public MainWindow()
    {
        InitializeComponent();
        Connect();
        DataContext = this;
    }
    private void Connect()
    {
        var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();
        conStr = configuration.GetConnectionString("conStr1");
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        con = new SqlConnection("Data Source=LEILASHAFI;Initial Catalog=master;Integrated Security=True;");

        DbCreate.CreateDb(con);
        var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();
        con = new SqlConnection(configuration.GetConnectionString("conStr1"));
        command = "SELECT * FROM PRODUCT";
        var reader = con.ExecuteReader(command);
        table = new DataTable();
        table.Load(reader);
        DataList.ItemsSource = table.AsDataView();


    }

    private async void Button_Click_1(object sender, RoutedEventArgs e)
    {
        string getDataCommand;
        if (string.IsNullOrWhiteSpace(txtbox.Text))
        {
            getDataCommand = $"SELECT * FROM PRODUCT";
        }
        else getDataCommand = $"SELECT * FROM PRODUCT WHERE Name LIKE '%{txtbox.Text}%'";
        SelectAll(getDataCommand);
        txtbox.Text = null;
    }

    private void SelectAll(string getDataCommand)
    {
        var reader = con.ExecuteReader(getDataCommand);
        table = new DataTable();
        table.Load(reader);
        DataList.ItemsSource = table.AsDataView();
    }

    private void Button_Click_2(object sender, RoutedEventArgs e)
    {
        
        AddWindow addWindow = new();

        addWindow.ShowDialog();

        if (addWindow.DialogResult == true)
        {
            var p = addWindow.Product;

            var addCommand = "INSERT INTO Product VALUES(@name,@description,@price,@stock)";

            con.Execute(addCommand, new { p.Name, p.Description, p.Price, p.Stock });

            var getDataCommand = "SELECT * FROM Product";

            var reader = con.ExecuteReader(getDataCommand);
            table = new DataTable();
            table.Load(reader);
            DataList.ItemsSource = table.AsDataView();
        }
    }

    private void Button_Click_3(object sender, RoutedEventArgs e)
    {

        var clearCommand = "TRUNCATE TABLE Product";

        con.Execute(clearCommand);
        MessageBox.Show("Database has successfully cleared");
        var getDataCommand = "SELECT * FROM Product";

        SelectAll(getDataCommand);

    }

    private async void Button_Click_4(object sender, RoutedEventArgs e)
    {

        List<Product> products = new();
        Product selectedProduct;
        foreach (DataRowView item in DataList.SelectedItems)
        {
            selectedProduct = new();
            selectedProduct.Name = item.Row["Name"].ToString();
            selectedProduct.Id = (int)item.Row["Id"];
            selectedProduct.Price = (int)item.Row["Price"];
            selectedProduct.Description = item.Row["Description"].ToString();
            selectedProduct.Stock = (int)item.Row["Stock"];
            products.Add(selectedProduct);
            
            
        }
        con.BulkDelete(products);

        var getDataCommand = "SELECT * FROM Product";

        SelectAll(getDataCommand);
    }

    private void Button_Click_5(object sender, RoutedEventArgs e)
    {
        Product product;
        if (DataList.SelectedItem is DataRowView item)
        {
            product = new();
            product.Name = item.Row["Name"].ToString();
            product.Id = (int)item.Row["Id"];
            product.Price = (int)item.Row["Price"];
            product.Description = item.Row["Description"].ToString();
            product.Stock = (int)item.Row["Stock"];
            

            UpdateWindow updateWindow = new(product);

            updateWindow.ShowDialog();


            if (updateWindow.DialogResult == true)
            {
                var updateCommand = "UPDATE Product SET Name = @name, Description = @description, Price = @price, Stock = @stock WHERE Id = @id";


                con.Execute(updateCommand, new { product.Name, product.Description, product.Price, product.Stock, product.Id });


                var getDataCommand = "SELECT * FROM Product";

                SelectAll(getDataCommand);
            }

           
        }
    }
}
