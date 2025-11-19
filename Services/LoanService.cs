using MauiApp1.Interfaces;
using MauiApp1.Models;

namespace MauiApp1.Services
{
    public class LoanService : ILoanService
    {
        private readonly List<Loan> _loans = new();
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;
        private int _nextId = 1;

        public LoanService(IBookRepository bookRepository, IUserRepository userRepository)
        {
            _bookRepository = bookRepository;
            _userRepository = userRepository;
        }

        public async Task<bool> BorrowBookAsync(int userId, int bookId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            var book = await _bookRepository.GetBookByIdAsync(bookId);

            if (user == null || book == null)
                return false;

            if (!book.IsAvailable)
                return false;

            var activeLoans = await GetActiveLoansAsync(userId);
            if (activeLoans.Count >= user.BorrowLimit)
                return false;

            var loan = new Loan(userId, bookId, user, book, user.ReturnDays)
            {
                Id = _nextId++
            };

            _loans.Add(loan);
            book.IsAvailable = false;
            await _bookRepository.UpdateBookAsync(book);

            return true;
        }

        public async Task<bool> ReturnBookAsync(int loanId)
        {
            var loan = _loans.FirstOrDefault(l => l.Id == loanId);
            if (loan == null || loan.IsReturned)
                return false;

            loan.IsReturned = true;
            loan.ReturnDate = DateTime.Now;

            var book = await _bookRepository.GetBookByIdAsync(loan.BookId);
            if (book != null)
            {
                book.IsAvailable = true;
                await _bookRepository.UpdateBookAsync(book);
            }

            return true;
        }

        public Task<List<Loan>> GetActiveLoansAsync(int userId)
        {
            var activeLoans = _loans
                .Where(l => l.UserId == userId && !l.IsReturned)
                .ToList();
            return Task.FromResult(activeLoans);
        }

        public Task<List<Loan>> GetAllLoansAsync()
        {
            return Task.FromResult(_loans);
        }

        public Task<List<Loan>> GetOverdueLoansAsync()
        {
            var overdueLoans = _loans
                .Where(l => !l.IsReturned && l.DueDate < DateTime.Now)
                .ToList();
            return Task.FromResult(overdueLoans);
        }
    }
}