using SmartLibraryAPI.Models;

namespace SmartLibraryAPI.Interfaces
{
    public interface IReservationService
    {
        Task<bool> ReserveBookAsync(int userId, int bookId);
        Task<bool> CancelReservationAsync(int reservationId);
        Task<List<Reservation>> GetActiveReservationsAsync(int userId);
        Task<Reservation?> GetNextReservationForBookAsync(int bookId);
        Task<bool> NotifyUserBookAvailableAsync(int reservationId);
    }
}