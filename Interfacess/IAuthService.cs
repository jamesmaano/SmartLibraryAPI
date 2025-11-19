using MauiApp1.Models;

namespace MauiApp1.Interfaces
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(string username, string password, string email, string role);
        Task<Account?> LoginAsync(string username, string password);
        Task<List<Account>> GetAllAccountsAsync();
        Task<Account?> GetAccountByIdAsync(int id);
        Task<bool> DeleteAccountAsync(int id);
        string HashPassword(string password);
        bool VerifyPassword(string password, string hash);
    }
}