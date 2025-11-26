using Microsoft.EntityFrameworkCore;
using SmartLibraryAPI.Data;
using SmartLibraryAPI.Interfaces;
using SmartLibraryAPI.Models;

namespace SmartLibraryAPI.Services
{
    public class AccountRepository : IAccountRepository
    {
        private readonly LibraryDbContext _context;

        public AccountRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<List<Account>> GetAllAccountsAsync()
        {
            return await _context.Accounts.ToListAsync();
        }

        public async Task<Account?> GetAccountByIdAsync(int id)
        {
            return await _context.Accounts.FindAsync(id);
        }

        public async Task<Account?> GetAccountByUsernameAsync(string username)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.Username == username);
        }

        public async Task AddAccountAsync(Account account)
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAccountAsync(Account account)
        {
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAccountAsync(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account != null)
            {
                _context.Accounts.Remove(account);
                await _context.SaveChangesAsync();
            }
        }
    }
}
