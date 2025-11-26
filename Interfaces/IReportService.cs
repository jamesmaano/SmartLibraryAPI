using SmartLibraryAPI.Models;

namespace SmartLibraryAPI.Interfaces
{
    public interface IReportService
    {
        Task<LibraryReport> GenerateOverdueBooksReportAsync();
        Task<LibraryReport> GenerateMostBorrowedBooksReportAsync();
        Task<LibraryReport> GenerateUserActivityReportAsync();
        Task<LibraryReport> GenerateFinesReportAsync();
        Task<LibraryReport> GenerateInventoryReportAsync();
    }
}