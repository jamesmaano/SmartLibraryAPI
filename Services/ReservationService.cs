using Microsoft.EntityFrameworkCore;
using SmartLibraryAPI.Data;
using SmartLibraryAPI.Interfaces;
using SmartLibraryAPI.Models;

namespace SmartLibraryAPI.Services
{
    public class ReservationService : IReservationService
    {
        private readonly LibraryDbContext _context;
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;

        public ReservationService(
            LibraryDbContext context,
            IBookRepository bookRepository,
            IUserRepository userRepository)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<bool> ReserveBookAsync(int userId, int bookId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            var book = await _bookRepository.GetBookByIdAsync(bookId);

            if (user == null || book == null)
                return false;

            var existingReservation = await _context.Reservations
                .FirstOrDefaultAsync(r => r.UserId == userId && r.BookId == bookId && r.IsActive);

            if (existingReservation != null)
                return false;

            var reservation = new Reservation
            {
                UserId = userId,
                BookId = bookId,
                User = user,
                Book = book,
                ReservationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                IsActive = true
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CancelReservationAsync(int reservationId)
        {
            var reservation = await _context.Reservations.FindAsync(reservationId);
            if (reservation == null || !reservation.IsActive)
                return false;

            reservation.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Reservation>> GetActiveReservationsAsync(int userId)
        {
            return await _context.Reservations
                .Include(r => r.User)
                .Include(r => r.Book)
                .Where(r => r.UserId == userId && r.IsActive)
                .OrderBy(r => r.ReservationDate)
                .ToListAsync();
        }

        public async Task<Reservation?> GetNextReservationForBookAsync(int bookId)
        {
            return await _context.Reservations
                .Include(r => r.User)
                .Include(r => r.Book)
                .Where(r => r.BookId == bookId && r.IsActive)
                .OrderBy(r => r.ReservationDate)
                .FirstOrDefaultAsync();
        }

        public Task<bool> NotifyUserBookAvailableAsync(int reservationId)
        {
            // Placeholder for notification logic
            return Task.FromResult(true);
        }
    }
}
