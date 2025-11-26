using SmartLibraryAPI.Interfaces;
using SmartLibraryAPI.Models;

namespace SmartLibraryAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAccountRepository _accountRepository;

        public AuthService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<bool> RegisterAsync(
            string username,
            string password,
            string email,
            string fullName,
            string studentId,
            string role)
        {
            var existing = await _accountRepository.GetAccountByUsernameAsync(username);
            if (existing != null)
                return false;

            var hashedPassword = HashPassword(password);
            var account = new Account(username, hashedPassword, email, fullName, studentId, role);
            await _accountRepository.AddAccountAsync(account);
            return true;
        }

        public async Task<Account?> LoginAsync(string username, string password)
        {
            var account = await _accountRepository.GetAccountByUsernameAsync(username);
            if (account == null)
                return null;

            if (!VerifyPassword(password, account.PasswordHash))
                return null;

            account.LastLogin = DateTime.UtcNow; // ✅ Changed from DateTime.Now
            await _accountRepository.UpdateAccountAsync(account);
            return account;
        }

        public async Task<bool> ChangePasswordAsync(int accountId, string currentPassword, string newPassword)
        {
            var account = await _accountRepository.GetAccountByIdAsync(accountId);
            if (account == null)
                return false;

            if (!VerifyPassword(currentPassword, account.PasswordHash))
                return false;

            account.PasswordHash = HashPassword(newPassword);
            await _accountRepository.UpdateAccountAsync(account);
            return true;
        }

        public Task<List<Account>> GetAllAccountsAsync()
        {
            return _accountRepository.GetAllAccountsAsync();
        }

        public Task<Account?> GetAccountByIdAsync(int id)
        {
            return _accountRepository.GetAccountByIdAsync(id);
        }

        public async Task<bool> DeleteAccountAsync(int id)
        {
            await _accountRepository.DeleteAccountAsync(id);
            return true;
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));
        }

        public bool VerifyPassword(string password, string hash)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(password, hash);
            }
            catch
            {
                return false;
            }
        }
    }
}