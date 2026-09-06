using System;
using System.IO;
using Microsoft.Data.Sqlite;

namespace TaskManagerApp.Data
{
    public class DatabaseHelper
    {
        private readonly string connectionString;

        public DatabaseHelper()
        {
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tasks.db");
            connectionString = $"Data Source={dbPath}";
            CreateTableIfNotExists();
        }

        private void CreateTableIfNotExists()
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Tasks (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NOT NULL,
                    Description TEXT,
                    IsCompleted INTEGER NOT NULL DEFAULT 0,
                    DateCreated TEXT NOT NULL
                );
            ";
            command.ExecuteNonQuery();


        }
    }
}
