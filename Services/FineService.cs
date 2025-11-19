using MauiApp1.Interfaces;
using MauiApp1.Models;

namespace MauiApp1.Services
{
    public class FineService : IFineService
    {
        private readonly List<Fine> _fines = new();
        private readonly ILoanService _loanService;
        private int _nextId = 1;

        public FineService(ILoanService loanService)
        {
            _loanService = loanService;
        }

        public async Task<Fine?> CalculateFineAsync(int loanId)
        {
            var allLoans = await _loanService.GetAllLoansAsync();
            var loan = allLoans.FirstOrDefault(l => l.Id == loanId);

            if (loan == null || loan.IsReturned)
                return null;

            var returnDate = loan.ReturnDate ?? DateTime.Now;
            if (returnDate <= loan.DueDate)
                return null;

            var daysOverdue = (int)(returnDate - loan.DueDate).TotalDays;
            if (daysOverdue <= 0)
                return null;

            var existingFine = _fines.FirstOrDefault(f => f.LoanId == loanId);
            if (existingFine != null)
                return existingFine;

            var fine = new Fine(loanId, loan.UserId, daysOverdue)
            {
                Id = _nextId++
            };

            _fines.Add(fine);
            return fine;
        }

        public Task<List<Fine>> GetUnpaidFinesAsync(int userId)
        {
            var unpaidFines = _fines
                .Where(f => f.UserId == userId && !f.IsPaid)
                .ToList();
            return Task.FromResult(unpaidFines);
        }

        public Task<bool> PayFineAsync(int fineId)
        {
            var fine = _fines.FirstOrDefault(f => f.Id == fineId);
            if (fine == null || fine.IsPaid)
                return Task.FromResult(false);

            fine.IsPaid = true;
            fine.PaidDate = DateTime.Now;
            return Task.FromResult(true);
        }

        public Task<decimal> GetTotalFinesAsync(int userId)
        {
            var totalFines = _fines
                .Where(f => f.UserId == userId && !f.IsPaid)
                .Sum(f => f.Amount);
            return Task.FromResult(totalFines);
        }
    }
}