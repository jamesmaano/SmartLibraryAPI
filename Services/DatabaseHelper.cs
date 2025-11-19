using MySql.Data.MySqlClient;
using System;
using System.Threading.Tasks;

namespace MauiApp1.Services
{
    public class DatabaseHelper
    {
        private readonly string _connectionString;

        public DatabaseHelper()
        {
            // Default XAMPP MySQL connection
            // If you changed MySQL port to 3307, update it here
            _connectionString = "Server=localhost;Port=3306;Database=smartlibrarydb;User=root;Password=;";
        }

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                using var connection = GetConnection();
                await connection.OpenAsync();
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Database connection failed: {ex.Message}");
                return false;
            }
        }
    }
}