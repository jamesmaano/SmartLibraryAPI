namespace MauiApp1.Interfaces
{
    using MauiApp1.Models;

    public interface IUserRepository
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByEmailAsync(string email);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
    }
}