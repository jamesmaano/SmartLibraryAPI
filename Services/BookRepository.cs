using MauiApp1.Interfaces;
using MauiApp1.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MauiApp1.Services
{
    public class BookRepository : IBookRepository
    {
        private readonly DatabaseHelper _dbHelper;

        public BookRepository()
        {
            _dbHelper = new DatabaseHelper();
        }

        public async Task<List<Book>> GetAllBooksAsync()
        {
            var books = new List<Book>();
            using var connection = _dbHelper.GetConnection();
            await connection.OpenAsync();

            var query = "SELECT * FROM Books";
            using var command = new MySqlCommand(query, connection);
            using var reader = (MySqlDataReader)await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                books.Add(MapReaderToBook(reader));
            }

            return books;
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            using var connection = _dbHelper.GetConnection();
            await connection.OpenAsync();

            var query = "SELECT * FROM Books WHERE Id = @Id";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = (MySqlDataReader)await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapReaderToBook(reader);
            }

            return null;
        }

        public async Task<Book?> GetBookByTitleAsync(string title)
        {
            using var connection = _dbHelper.GetConnection();
            await connection.OpenAsync();

            var query = "SELECT * FROM Books WHERE Title = @Title";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Title", title);

            using var reader = (MySqlDataReader)await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapReaderToBook(reader);
            }

            return null;
        }

        public async Task AddBookAsync(Book book)
        {
            using var connection = _dbHelper.GetConnection();
            await connection.OpenAsync();

            var query = @"INSERT INTO Books (Title, Author, ISBN, IsAvailable, CreatedDate) 
                         VALUES (@Title, @Author, @ISBN, @IsAvailable, @CreatedDate)";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Title", book.Title);
            command.Parameters.AddWithValue("@Author", book.Author);
            command.Parameters.AddWithValue("@ISBN", book.ISBN);
            command.Parameters.AddWithValue("@IsAvailable", book.IsAvailable);
            command.Parameters.AddWithValue("@CreatedDate", book.CreatedDate);

            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateBookAsync(Book book)
        {
            using var connection = _dbHelper.GetConnection();
            await connection.OpenAsync();

            var query = @"UPDATE Books 
                         SET Title = @Title, Author = @Author, ISBN = @ISBN, 
                             IsAvailable = @IsAvailable 
                         WHERE Id = @Id";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", book.Id);
            command.Parameters.AddWithValue("@Title", book.Title);
            command.Parameters.AddWithValue("@Author", book.Author);
            command.Parameters.AddWithValue("@ISBN", book.ISBN);
            command.Parameters.AddWithValue("@IsAvailable", book.IsAvailable);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteBookAsync(int id)
        {
            using var connection = _dbHelper.GetConnection();
            await connection.OpenAsync();

            var query = "DELETE FROM Books WHERE Id = @Id";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);

            await command.ExecuteNonQueryAsync();
        }

        private Book MapReaderToBook(MySqlDataReader reader)
        {
            return new Book(
                reader.GetString("Title"),
                reader.GetString("Author"),
                reader.GetString("ISBN"))
            {
                Id = reader.GetInt32("Id"),
                IsAvailable = reader.GetBoolean("IsAvailable"),
                CreatedDate = reader.GetDateTime("CreatedDate")
            };
        }
    }
}