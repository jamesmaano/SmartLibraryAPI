using SmartLibraryAPI.Models;

namespace SmartLibraryAPI.Interfaces
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(string username, string password, string email, string fullName, string studentId, string role);
        Task<Account?> LoginAsync(string username, string password);
        Task<bool> ChangePasswordAsync(int accountId, string currentPassword, string newPassword);
        Task<List<Account>> GetAllAccountsAsync();
        Task<Account?> GetAccountByIdAsync(int id);
        Task<bool> DeleteAccountAsync(int id);
        string HashPassword(string password);
        bool VerifyPassword(string password, string hash);
    }
}