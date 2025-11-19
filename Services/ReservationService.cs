using MauiApp1.Interfaces;
using MauiApp1.Models;

namespace MauiApp1.Services
{
    public class ReservationService : IReservationService
    {
        private readonly List<Reservation> _reservations = new();
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;
        private int _nextId = 1;

        public ReservationService(IBookRepository bookRepository, IUserRepository userRepository)
        {
            _bookRepository = bookRepository;
            _userRepository = userRepository;
        }

        public async Task<bool> ReserveBookAsync(int userId, int bookId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            var book = await _bookRepository.GetBookByIdAsync(bookId);

            if (user == null || book == null)
                return false;

            // Check if user already has an active reservation for this book
            var existingReservation = _reservations
                .FirstOrDefault(r => r.UserId == userId && r.BookId == bookId && r.IsActive);

            if (existingReservation != null)
                return false;

            var reservation = new Reservation(userId, bookId, user, book)
            {
                Id = _nextId++
            };

            _reservations.Add(reservation);
            return true;
        }

        public Task<bool> CancelReservationAsync(int reservationId)
        {
            var reservation = _reservations.FirstOrDefault(r => r.Id == reservationId);
            if (reservation == null || !reservation.IsActive)
                return Task.FromResult(false);

            reservation.IsActive = false;
            return Task.FromResult(true);
        }

        public Task<List<Reservation>> GetActiveReservationsAsync(int userId)
        {
            var activeReservations = _reservations
                .Where(r => r.UserId == userId && r.IsActive)
                .OrderBy(r => r.ReservationDate)
                .ToList();
            return Task.FromResult(activeReservations);
        }

        public Task<Reservation?> GetNextReservationForBookAsync(int bookId)
        {
            var nextReservation = _reservations
                .Where(r => r.BookId == bookId && r.IsActive)
                .OrderBy(r => r.ReservationDate)
                .FirstOrDefault();
            return Task.FromResult(nextReservation);
        }

        public async Task<bool> NotifyUserBookAvailableAsync(int reservationId)
        {
            var reservation = _reservations.FirstOrDefault(r => r.Id == reservationId);
            if (reservation == null || !reservation.IsActive)
                return false;

            // In a real system, this would send an email or notification
            await Task.Run(() =>
                System.Diagnostics.Debug.WriteLine(
                    $"Notification: Book '{reservation.Book.Title}' is now available for {reservation.User.Name}")
            );

            return true;
        }
    }
}