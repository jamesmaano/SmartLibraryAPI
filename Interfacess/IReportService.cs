namespace MauiApp1.Interfaces
{
    using MauiApp1.Models;

    public interface IReportService
    {
        Task<LibraryReport> GenerateOverdueBooksReportAsync();
        Task<LibraryReport> GenerateMostBorrowedBooksReportAsync();
        Task<LibraryReport> GenerateUserActivityReportAsync();
        Task<LibraryReport> GenerateFinesReportAsync();
        Task<LibraryReport> GenerateInventoryReportAsync();
    }
}