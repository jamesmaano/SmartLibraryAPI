namespace MauiApp1.Interfaces
{
    using MauiApp1.Models;

    public interface IFineService
    {
        Task<Fine?> CalculateFineAsync(int loanId);
        Task<List<Fine>> GetUnpaidFinesAsync(int userId);
        Task<bool> PayFineAsync(int fineId);
        Task<decimal> GetTotalFinesAsync(int userId);
    }
}