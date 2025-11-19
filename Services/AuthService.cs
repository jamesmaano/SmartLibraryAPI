using MauiApp1.Interfaces;
using MauiApp1.Models;
using System.Security.Cryptography;
using System.Text;

namespace MauiApp1.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAccountRepository _accountRepository;

        public AuthService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<bool> RegisterAsync(string username, string password, string email, string role)
        {
            var existing = await _accountRepository.GetAccountByUsernameAsync(username);
            if (existing != null) return false;

            var passwordHash = HashPassword(password);
            var account = new Account(username, passwordHash, email, role);

            await _accountRepository.AddAccountAsync(account);
            return true;
        }

        public async Task<Account?> LoginAsync(string username, string password)
        {
            var account = await _accountRepository.GetAccountByUsernameAsync(username);

            if (account == null || !account.IsActive)
                return null;

            if (!VerifyPassword(password, account.PasswordHash))
                return null;

            account.LastLogin = DateTime.Now;
            await _accountRepository.UpdateAccountAsync(account);

            return account;
        }

        public async Task<List<Account>> GetAllAccountsAsync()
        {
            return await _accountRepository.GetAllAccountsAsync();
        }

        public async Task<Account?> GetAccountByIdAsync(int id)
        {
            return await _accountRepository.GetAccountByIdAsync(id);
        }

        public async Task<bool> DeleteAccountAsync(int id)
        {
            var account = await _accountRepository.GetAccountByIdAsync(id);
            if (account == null) return false;

            await _accountRepository.DeleteAccountAsync(id);
            return true;
        }

        public string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        public bool VerifyPassword(string password, string hash)
        {
            var newHash = HashPassword(password);
            return newHash == hash;
        }
    }
}