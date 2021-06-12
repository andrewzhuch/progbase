using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;

namespace ProductRepo
{
    public static class ProductRepo
    {
        public static long Insert(Product.Product product)
        {
            string dataBaseFile = "./../products.db";
            SqliteConnection connection = new SqliteConnection($"Data Source = {dataBaseFile}");
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = 
            @"
                INSERT INTO products (name, info, price, onStorage, createdAt) 
                VALUES ($name, $info, $price, $onStorage, $createdAt);

                SELECT last_insert_rowid();
            ";
            command.Parameters.AddWithValue("$name", product.name);
            command.Parameters.AddWithValue("$info", product.info);
            command.Parameters.AddWithValue("$price", product.price);
            command.Parameters.AddWithValue("$onStorage", product.onStorage.ToString());
            command.Parameters.AddWithValue("$createdAt", product.createdAt);
            long newId = (long)command.ExecuteScalar();
            connection.Close();
            return newId;
        }
        public static bool DeleteById(Product.Product product)
        {
            string dataBaseFile = "./../products.db";
            SqliteConnection connection = new SqliteConnection($"Data Source = {dataBaseFile}");
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"DELETE FROM products WHERE id = $id";
            command.Parameters.AddWithValue("$id", product.id);
            int changes = command.ExecuteNonQuery();
            if(changes == 0)
            {
                connection.Close();
                return false;
            }
            else
            {
                connection.Close();
                return true;
            }
        }
        public static void UpdateProduct(Product.Product product, long id1, DateTime time)
        {
            string dataBaseFile = "./../products.db";
            SqliteConnection connection = new SqliteConnection($"Data Source = {dataBaseFile}");
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"UPDATE products SET 
            name = $name, info = $info, price = $price, onStorage = $onStorage, createdAt = $createdAt 
            WHERE id = $id;";
            command.Parameters.AddWithValue("$name", product.name);
            command.Parameters.AddWithValue("$info", product.info);
            command.Parameters.AddWithValue("$price", product.price);
            command.Parameters.AddWithValue("$onStorage", product.onStorage.ToString());
            command.Parameters.AddWithValue("$createdAt", time);
            command.Parameters.AddWithValue("$id", id1);
            int nCHanged = command.ExecuteNonQuery();
            connection.Close();
        }
        public static int GetTotalPages()
        {
            string dataBaseFile = "./../products.db";
            SqliteConnection connection = new SqliteConnection($"Data Source = {dataBaseFile}");
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT COUNT(*) FROM products";
            long count = (long)command.ExecuteScalar();
            const int pageSize = 10;
            connection.Close();
            return (int)Math.Ceiling(count / (double)pageSize);
        }
        public static List<Product.Product> GetPage(int pageNumber)
        {
            const int pageSize = 10;
            string dataBaseFile = "./../products.db";
            SqliteConnection connection = new SqliteConnection($"Data Source = {dataBaseFile}");
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM products LIMIT $pageSize OFFSET $pageSize * ($pageNumber - 1)";
            command.Parameters.AddWithValue("$pageSize", pageSize);
            command.Parameters.AddWithValue("$pageNumber", pageNumber);
            SqliteDataReader reader = command.ExecuteReader();
            List<Product.Product> products = new List<Product.Product>();
            while(reader.Read())
            {
                Product.Product product = new Product.Product();
                product.id = int.Parse(reader.GetString(0));
                product.name = reader.GetString(1);
                product.info = reader.GetString(2);
                product.price = int.Parse(reader.GetString(3));
                product.onStorage = bool.Parse(reader.GetString(4));
                product.createdAt = DateTime.Parse(reader.GetString(5));
                products.Add(product);
            }
            reader.Close();
            connection.Close();
            return products;
        }
    }
}
