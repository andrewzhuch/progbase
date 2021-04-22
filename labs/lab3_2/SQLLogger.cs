using System;
using Microsoft.Data.Sqlite;
namespace lab3_2
{
    public class SQLLogger : ILogger
    {
        private string _filePath;
        public SQLLogger(string path)
        {
            this._filePath = path;
        }
        public void Log(string message)
        {
            SqliteConnection connection = new SqliteConnection($"Data Source = {_filePath}");
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = 
            @"
                INSERT INTO logs (timestamp, type, message) 
                VALUES ($timestamp, $type, $message);

                SELECT last_insert_rowid();
            ";
            string type = "LOG";
            command.Parameters.AddWithValue("$timestamp", DateTime.UtcNow.ToString("o"));
            command.Parameters.AddWithValue("$type", type);
            command.Parameters.AddWithValue("$message", message);
            long newId = (long)command.ExecuteScalar();
            connection.Close();
        }

        public void LogError(string errorMessage)
        {
            SqliteConnection connection = new SqliteConnection($"Data Source = {_filePath}");
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = 
            @"
                INSERT INTO logs (timestamp, type, message) 
                VALUES ($timestamp, $type, $message);

                SELECT last_insert_rowid();
            ";
            string type = "ERROR";
            command.Parameters.AddWithValue("$timestamp", DateTime.UtcNow.ToString("o"));
            command.Parameters.AddWithValue("$type", type);
            command.Parameters.AddWithValue("$message", errorMessage);
            long newId = (long)command.ExecuteScalar();
            connection.Close();
        }
    }
}