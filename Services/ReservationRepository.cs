using MauiApp1.Interfaces;
using MauiApp1.Models;

namespace MauiApp1.Services
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly List<Reservation> _reservations = new();
        private int _nextId = 1;

        public Task<List<Reservation>> GetAllReservationsAsync()
        {
            return Task.FromResult(_reservations);
        }

        public Task<Reservation?> GetReservationByIdAsync(int id)
        {
            var reservation = _reservations.FirstOrDefault(r => r.Id == id);
            return Task.FromResult(reservation);
        }

        public Task<List<Reservation>> GetReservationsByUserIdAsync(int userId)
        {
            var reservations = _reservations.Where(r => r.UserId == userId).ToList();
            return Task.FromResult(reservations);
        }

        public Task<List<Reservation>> GetReservationsByBookIdAsync(int bookId)
        {
            var reservations = _reservations.Where(r => r.BookId == bookId).ToList();
            return Task.FromResult(reservations);
        }

        public Task AddReservationAsync(Reservation reservation)
        {
            reservation.Id = _nextId++;
            _reservations.Add(reservation);
            return Task.CompletedTask;
        }

        public Task UpdateReservationAsync(Reservation reservation)
        {
            var existing = _reservations.FirstOrDefault(r => r.Id == reservation.Id);
            if (existing != null)
            {
                existing.IsActive = reservation.IsActive;
            }
            return Task.CompletedTask;
        }

        public Task DeleteReservationAsync(int id)
        {
            var reservation = _reservations.FirstOrDefault(r => r.Id == id);
            if (reservation != null)
                _reservations.Remove(reservation);
            return Task.CompletedTask;
        }
    }
}