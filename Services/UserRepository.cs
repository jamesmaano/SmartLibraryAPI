using MauiApp1.Interfaces;
using MauiApp1.Models;
using MySql.Data.MySqlClient;

namespace MauiApp1.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseHelper _dbHelper;

        public UserRepository()
        {
            _dbHelper = new DatabaseHelper();
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            var users = new List<User>();
            using var connection = _dbHelper.GetConnection();
            await connection.OpenAsync();

            var query = "SELECT * FROM Users";
            using var command = new MySqlCommand(query, connection);
            using var reader = (MySqlDataReader)await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                users.Add(MapReaderToUser(reader));
            }

            return users;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            using var connection = _dbHelper.GetConnection();
            await connection.OpenAsync();

            var query = "SELECT * FROM Users WHERE Id = @Id";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = (MySqlDataReader)await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapReaderToUser(reader);
            }

            return null;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            using var connection = _dbHelper.GetConnection();
            await connection.OpenAsync();

            var query = "SELECT * FROM Users WHERE Email = @Email";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Email", email);

            using var reader = (MySqlDataReader)await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapReaderToUser(reader);
            }

            return null;
        }

        public async Task AddUserAsync(User user)
        {
            using var connection = _dbHelper.GetConnection();
            await connection.OpenAsync();

            var userType = user is Student ? "Student" : "Faculty";
            var query = @"INSERT INTO Users (Name, Email, PhoneNumber, UserType, StudentId, EmployeeId, RegisteredDate) 
                         VALUES (@Name, @Email, @PhoneNumber, @UserType, @StudentId, @EmployeeId, @RegisteredDate)";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Name", user.Name);
            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
            command.Parameters.AddWithValue("@UserType", userType);
            command.Parameters.AddWithValue("@StudentId", user is Student student ? student.StudentId : (object)DBNull.Value);
            command.Parameters.AddWithValue("@EmployeeId", user is Faculty faculty ? faculty.EmployeeId : (object)DBNull.Value);
            command.Parameters.AddWithValue("@RegisteredDate", user.RegisteredDate);

            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            using var connection = _dbHelper.GetConnection();
            await connection.OpenAsync();

            var userType = user is Student ? "Student" : "Faculty";
            var query = @"UPDATE Users 
                         SET Name = @Name, Email = @Email, PhoneNumber = @PhoneNumber, 
                             UserType = @UserType, StudentId = @StudentId, EmployeeId = @EmployeeId 
                         WHERE Id = @Id";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", user.Id);
            command.Parameters.AddWithValue("@Name", user.Name);
            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
            command.Parameters.AddWithValue("@UserType", userType);
            command.Parameters.AddWithValue("@StudentId", user is Student student ? student.StudentId : (object)DBNull.Value);
            command.Parameters.AddWithValue("@EmployeeId", user is Faculty faculty ? faculty.EmployeeId : (object)DBNull.Value);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            using var connection = _dbHelper.GetConnection();
            await connection.OpenAsync();

            var query = "DELETE FROM Users WHERE Id = @Id";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);

            await command.ExecuteNonQueryAsync();
        }

        private User MapReaderToUser(MySqlDataReader reader)
        {
            var userType = reader.GetString("UserType");
            var name = reader.GetString("Name");
            var email = reader.IsDBNull(reader.GetOrdinal("Email")) ? "" : reader.GetString("Email");
            var phoneNumber = reader.IsDBNull(reader.GetOrdinal("PhoneNumber")) ? "" : reader.GetString("PhoneNumber");

            User user;
            if (userType == "Student")
            {
                var studentId = reader.IsDBNull(reader.GetOrdinal("StudentId")) ? "" : reader.GetString("StudentId");
                user = new Student(name, email, phoneNumber, studentId);
            }
            else
            {
                var employeeId = reader.IsDBNull(reader.GetOrdinal("EmployeeId")) ? "" : reader.GetString("EmployeeId");
                user = new Faculty(name, email, phoneNumber, employeeId);
            }

            user.Id = reader.GetInt32("Id");
            user.RegisteredDate = reader.GetDateTime("RegisteredDate");

            return user;
        }
    }
}