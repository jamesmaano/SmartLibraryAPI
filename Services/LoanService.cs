using Microsoft.EntityFrameworkCore;
using SmartLibraryAPI.Data;
using SmartLibraryAPI.Interfaces;
using SmartLibraryAPI.Models;

namespace SmartLibraryAPI.Services
{
    public class LoanService : ILoanService
    {
        private readonly LibraryDbContext _context;
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;

        public LoanService(
            LibraryDbContext context,
            IBookRepository bookRepository,
            IUserRepository userRepository)
        {
            _context = context;
            _bookRepository = bookRepository;
            _userRepository = userRepository;
        }

        public async Task<bool> BorrowBookAsync(int userId, int bookId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            var book = await _bookRepository.GetBookByIdAsync(bookId);

            // Validate user and book exist
            if (user == null || book == null)
                return false;

            // Check if book is available
            if (!book.IsAvailable)
                return false;

            // Check user's active loan count against their borrow limit
            var activeLoans = await GetActiveLoansAsync(userId);
            if (activeLoans.Count >= user.BorrowLimit)
                return false;

            // Check if user has unpaid fines
            var unpaidFines = await _context.Fines
                .Where(f => f.UserId == userId && !f.IsPaid)
                .AnyAsync();

            if (unpaidFines)
                return false;

            // Check if book is reserved by someone else
            var reservation = await _context.Reservations
                .Where(r => r.BookId == bookId && r.IsActive)
                .OrderBy(r => r.ReservationDate)
                .FirstOrDefaultAsync();

            if (reservation != null && reservation.UserId != userId)
                return false;

            // Create loan
            var loan = new Loan(userId, bookId, user, book, user.ReturnDays);
            _context.Loans.Add(loan);

            // Update book availability
            book.IsAvailable = false;

            // Cancel reservation if user had one
            if (reservation != null && reservation.UserId == userId)
            {
                reservation.IsActive = false;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ReturnBookAsync(int loanId)
        {
            var loan = await _context.Loans
                .Include(l => l.Book)
                .FirstOrDefaultAsync(l => l.Id == loanId);

            if (loan == null || loan.IsReturned)
                return false;

            // Mark loan as returned
            loan.IsReturned = true;
            loan.ReturnDate = DateTime.Now;

            // Make book available
            loan.Book.IsAvailable = true;

            // Check if there's a fine (overdue)
            if (DateTime.Now > loan.DueDate)
            {
                var daysOverdue = (int)(DateTime.Now - loan.DueDate).TotalDays;
                var existingFine = await _context.Fines
                    .FirstOrDefaultAsync(f => f.LoanId == loanId);

                if (existingFine == null)
                {
                    var fine = new Fine(loanId, loan.UserId, daysOverdue);
                    _context.Fines.Add(fine);
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Loan>> GetActiveLoansAsync(int userId)
        {
            return await _context.Loans
                .Include(l => l.User)
                .Include(l => l.Book)
                .Where(l => l.UserId == userId && !l.IsReturned)
                .ToListAsync();
        }

        public async Task<List<Loan>> GetAllLoansAsync()
        {
            return await _context.Loans
                .Include(l => l.User)
                .Include(l => l.Book)
                .ToListAsync();
        }

        public async Task<List<Loan>> GetOverdueLoansAsync()
        {
            return await _context.Loans
                .Include(l => l.User)
                .Include(l => l.Book)
                .Where(l => !l.IsReturned && l.DueDate < DateTime.Now)
                .ToListAsync();
        }
    }
}