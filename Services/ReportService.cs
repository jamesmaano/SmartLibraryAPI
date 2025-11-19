using MauiApp1.Interfaces;
using MauiApp1.Models;

namespace MauiApp1.Services
{
    public class ReportService : IReportService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILoanService _loanService;
        private readonly IFineService _fineService;

        public ReportService(
            IBookRepository bookRepository,
            IUserRepository userRepository,
            ILoanService loanService,
            IFineService fineService)
        {
            _bookRepository = bookRepository;
            _userRepository = userRepository;
            _loanService = loanService;
            _fineService = fineService;
        }

        public async Task<LibraryReport> GenerateOverdueBooksReportAsync()
        {
            var report = new LibraryReport("Overdue Books Report");
            var overdueLoans = await _loanService.GetOverdueLoansAsync();

            report.Data["TotalOverdueBooks"] = overdueLoans.Count;
            report.Data["OverdueLoans"] = overdueLoans.Select(loan => new
            {
                loan.User.Name,
                BookTitle = loan.Book.Title,
                loan.DueDate,
                DaysOverdue = (DateTime.Now - loan.DueDate).Days
            }).ToList();

            return report;
        }

        public async Task<LibraryReport> GenerateMostBorrowedBooksReportAsync()
        {
            var report = new LibraryReport("Most Borrowed Books Report");
            var allLoans = await _loanService.GetAllLoansAsync();

            var mostBorrowed = allLoans
                .GroupBy(l => new { l.BookId, l.Book.Title, l.Book.Author })
                .Select(g => new
                {
                    g.Key.Title,
                    g.Key.Author,
                    BorrowCount = g.Count()
                })
                .OrderByDescending(x => x.BorrowCount)
                .Take(10)
                .ToList();

            report.Data["MostBorrowedBooks"] = mostBorrowed;
            return report;
        }

        public async Task<LibraryReport> GenerateUserActivityReportAsync()
        {
            var report = new LibraryReport("User Activity Report");
            var allUsers = await _userRepository.GetAllUsersAsync();
            var allLoans = await _loanService.GetAllLoansAsync();

            var userActivity = allUsers.Select(user => new
            {
                user.Name,
                UserType = user.GetType().Name,
                ActiveLoans = allLoans.Count(l => l.UserId == user.Id && !l.IsReturned),
                TotalBorrowed = allLoans.Count(l => l.UserId == user.Id),
                user.RegisteredDate
            }).ToList();

            report.Data["TotalUsers"] = allUsers.Count;
            report.Data["StudentCount"] = allUsers.Count(u => u is Student);
            report.Data["FacultyCount"] = allUsers.Count(u => u is Faculty);
            report.Data["UserActivity"] = userActivity;

            return report;
        }

        public async Task<LibraryReport> GenerateFinesReportAsync()
        {
            var report = new LibraryReport("Fines Report");
            var allUsers = await _userRepository.GetAllUsersAsync();

            var finesData = new List<object>();
            decimal totalUnpaidFines = 0;

            foreach (var user in allUsers)
            {
                var unpaidFines = await _fineService.GetUnpaidFinesAsync(user.Id);
                var totalFines = await _fineService.GetTotalFinesAsync(user.Id);

                if (unpaidFines.Any())
                {
                    finesData.Add(new
                    {
                        user.Name,
                        UnpaidFinesCount = unpaidFines.Count,
                        TotalAmount = totalFines
                    });
                    totalUnpaidFines += totalFines;
                }
            }

            report.Data["TotalUnpaidFines"] = totalUnpaidFines;
            report.Data["UsersWithFines"] = finesData.Count;
            report.Data["FinesDetails"] = finesData;

            return report;
        }

        public async Task<LibraryReport> GenerateInventoryReportAsync()
        {
            var report = new LibraryReport("Library Inventory Report");
            var allBooks = await _bookRepository.GetAllBooksAsync();

            report.Data["TotalBooks"] = allBooks.Count;
            report.Data["AvailableBooks"] = allBooks.Count(b => b.IsAvailable);
            report.Data["BorrowedBooks"] = allBooks.Count(b => !b.IsAvailable);
            report.Data["Books"] = allBooks.Select(b => new
            {
                b.Title,
                b.Author,
                b.ISBN,
                Status = b.IsAvailable ? "Available" : "Borrowed"
            }).ToList();

            return report;
        }
    }
}