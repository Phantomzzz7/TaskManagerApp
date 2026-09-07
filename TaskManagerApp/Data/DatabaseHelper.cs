using System;
using System.IO;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using TaskManagerApp.Models;

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
        public void AddTask(string title, string description)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
        INSERT INTO Tasks (Title, Description, IsCompleted, DateCreated)
        VALUES ($title, $description, 0, $dateCreated);
    ";
            command.Parameters.AddWithValue("$title", title);
            command.Parameters.AddWithValue("$description", description);
            command.Parameters.AddWithValue("$dateCreated", DateTime.Now.ToString("s"));
            command.ExecuteNonQuery();
        }

        public List <TaskItem> GetAllTasks()
        {
            var tasks = new List <TaskItem>();
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, Title, Description, IsCompleted, DateCreated FROM Tasks;";
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var task = new TaskItem
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Description = reader.GetString(2),
                    IsCompleted = reader.GetInt32(3) == 1,
                    DataCreated = DateTime.Parse(reader.GetString(4))
                };
                tasks.Add(task);
            }
            return tasks;
        }
        public void UpdateTaskStatus(int id, bool isCompleted)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
        UPDATE Tasks
        SET IsCompleted = $isCompleted
        WHERE Id = $id;
    ";
            command.Parameters.AddWithValue("$isCompleted", isCompleted ? 1 : 0);
            command.Parameters.AddWithValue("$id", id);
            command.ExecuteNonQuery();
        }
        public void DeleteTask(int id)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Tasks WHERE Id = $id;";
            command.Parameters.AddWithValue("$id", id);
            command.ExecuteNonQuery();
        }
    }
}
