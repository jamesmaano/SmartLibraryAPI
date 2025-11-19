namespace MauiApp1.Interfaces
{
    using MauiApp1.Models;

    public interface IReservationRepository
    {
        Task<List<Reservation>> GetAllReservationsAsync();
        Task<Reservation?> GetReservationByIdAsync(int id);
        Task<List<Reservation>> GetReservationsByUserIdAsync(int userId);
        Task<List<Reservation>> GetReservationsByBookIdAsync(int bookId);
        Task AddReservationAsync(Reservation reservation);
        Task UpdateReservationAsync(Reservation reservation);
        Task DeleteReservationAsync(int id);
    }
}