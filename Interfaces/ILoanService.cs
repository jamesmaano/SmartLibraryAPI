using SmartLibraryAPI.Models;

namespace SmartLibraryAPI.Interfaces
{
    public interface ILoanService
    {
        Task<bool> BorrowBookAsync(int userId, int bookId);
        Task<bool> ReturnBookAsync(int loanId);
        Task<List<Loan>> GetActiveLoansAsync(int userId);
        Task<List<Loan>> GetAllLoansAsync();
        Task<List<Loan>> GetOverdueLoansAsync();
    }
}