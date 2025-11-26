using SmartLibraryAPI.Models;

namespace SmartLibraryAPI.Interfaces
{
    public interface IAccountRepository
    {
        Task<List<Account>> GetAllAccountsAsync();
        Task<Account?> GetAccountByIdAsync(int id);
        Task<Account?> GetAccountByUsernameAsync(string username);
        Task AddAccountAsync(Account account);
        Task UpdateAccountAsync(Account account);
        Task DeleteAccountAsync(int id);
    }
}