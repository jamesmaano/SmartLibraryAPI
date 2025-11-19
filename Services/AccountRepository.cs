using MauiApp1.Interfaces;
using MauiApp1.Models;

namespace MauiApp1.Services
{
    public class AccountRepository : IAccountRepository
    {
        private readonly List<Account> _accounts = new();
        private int _nextId = 1;

        public Task<List<Account>> GetAllAccountsAsync()
        {
            return Task.FromResult(_accounts.ToList());
        }

        public Task<Account?> GetAccountByIdAsync(int id)
        {
            var account = _accounts.FirstOrDefault(a => a.Id == id);
            return Task.FromResult(account);
        }

        public Task<Account?> GetAccountByUsernameAsync(string username)
        {
            var account = _accounts.FirstOrDefault(a => a.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(account);
        }

        public Task AddAccountAsync(Account account)
        {
            account.Id = _nextId++;
            _accounts.Add(account);
            return Task.CompletedTask;
        }

        public Task UpdateAccountAsync(Account account)
        {
            var existing = _accounts.FirstOrDefault(a => a.Id == account.Id);
            if (existing != null)
            {
                var index = _accounts.IndexOf(existing);
                _accounts[index] = account;
            }
            return Task.CompletedTask;
        }

        public Task DeleteAccountAsync(int id)
        {
            var account = _accounts.FirstOrDefault(a => a.Id == id);
            if (account != null)
                _accounts.Remove(account);
            return Task.CompletedTask;
        }
    }
}