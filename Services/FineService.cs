using Microsoft.EntityFrameworkCore;
using SmartLibraryAPI.Data;
using SmartLibraryAPI.Interfaces;
using SmartLibraryAPI.Models;

namespace SmartLibraryAPI.Services
{
    public class FineService : IFineService
    {
        private readonly LibraryDbContext _context;

        public FineService(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<Fine?> CalculateFineAsync(int loanId)
        {
            var loan = await _context.Loans.FirstOrDefaultAsync(l => l.Id == loanId);

            if (loan == null)
                return null;

            // Check if fine already exists
            var existingFine = await _context.Fines.FirstOrDefaultAsync(f => f.LoanId == loanId);
            if (existingFine != null)
                return existingFine;

            // Only calculate fine if book is overdue
            DateTime comparisonDate = loan.IsReturned ? loan.ReturnDate!.Value : DateTime.Now;

            if (comparisonDate <= loan.DueDate)
                return null;

            var daysOverdue = (int)(comparisonDate - loan.DueDate).TotalDays;
            if (daysOverdue <= 0)
                return null;

            // Create new fine
            var fine = new Fine(loanId, loan.UserId, daysOverdue);
            _context.Fines.Add(fine);
            await _context.SaveChangesAsync();

            return fine;
        }

        public async Task<List<Fine>> GetUnpaidFinesAsync(int userId)
        {
            return await _context.Fines
                .Where(f => f.UserId == userId && !f.IsPaid)
                .ToListAsync();
        }

        public async Task<bool> PayFineAsync(int fineId)
        {
            var fine = await _context.Fines.FindAsync(fineId);
            if (fine == null || fine.IsPaid)
                return false;

            fine.IsPaid = true;
            fine.PaidDate = DateTime.Now;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<decimal> GetTotalFinesAsync(int userId)
        {
            return await _context.Fines
                .Where(f => f.UserId == userId && !f.IsPaid)
                .SumAsync(f => f.Amount);
        }
    }
}