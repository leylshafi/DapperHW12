using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DapperHW12;

public class DbCreate
{
    static public void CreateDb(SqlConnection? sqlConnection)
    {
        sqlConnection?.Open();
        string query = $"SELECT COUNT(*) FROM sys.databases WHERE name = 'MyShopDb'";
        SqlCommand checkDatabaseCommand = new SqlCommand(query, sqlConnection);

        int databaseCount = (int)checkDatabaseCommand.ExecuteScalar();


        if (databaseCount == 0)
        {

            sqlConnection?.Execute("CREATE DATABASE MyShopDb");

            sqlConnection.Execute(@"
							use [MyShopDb]
							Create Table [dbo].[Product] (
							[Id]	INT IDENTITY (1,1) PRIMARY KEY NOT NULL,
							[Name]	NVARCHAR(100) NOT NULL,
							[Description] NVARCHAR(MAX) NOT NULL,
							[Price] INT NOT NULL,
							[Stock] INT NOT NULL)");
            sqlConnection.Execute(@"
							use [MyShopDb]
							INSERT INTO [Product] VALUES('cola', 'beverage', 10, 11);
							INSERT INTO [Product] VALUES('fanta', 'beverage', 15, 30);
							INSERT INTO [Product] VALUES('pepsi', 'beverage', 5, 16);
							INSERT INTO [Product] VALUES('sprite', 'beverage', 8, 19);");
            MessageBox.Show($"The database created successfully.");
        }
        else
        {
            MessageBox.Show($"The database has already created.");
        }
        sqlConnection?.Close();

    }
}
