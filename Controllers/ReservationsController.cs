using Microsoft.AspNetCore.Mvc;
using SmartLibraryAPI.Interfaces;
using SmartLibraryAPI.Models;
using SmartLibraryAPI.DTOs.Response;

namespace SmartLibraryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        private readonly IReservationRepository _reservationRepository;

        public ReservationsController(
            IReservationService reservationService,
            IReservationRepository reservationRepository)
        {
            _reservationService = reservationService;
            _reservationRepository = reservationRepository;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<Reservation>>>> GetAllReservations()
        {
            var reservations = await _reservationRepository.GetAllReservationsAsync();
            return Ok(ApiResponse<List<Reservation>>.SuccessResponse(
                "Reservations retrieved successfully",
                reservations));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Reservation>>> GetReservationById(int id)
        {
            var reservation = await _reservationRepository.GetReservationByIdAsync(id);
            if (reservation == null)
                return NotFound(ApiResponse<Reservation>.ErrorResponse("Reservation not found"));

            return Ok(ApiResponse<Reservation>.SuccessResponse(
                "Reservation retrieved successfully",
                reservation));
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<ApiResponse<List<Reservation>>>> GetUserReservations(int userId)
        {
            var reservations = await _reservationService.GetActiveReservationsAsync(userId);
            return Ok(ApiResponse<List<Reservation>>.SuccessResponse(
                "User reservations retrieved successfully",
                reservations));
        }

        [HttpGet("book/{bookId}")]
        public async Task<ActionResult<ApiResponse<List<Reservation>>>> GetBookReservations(int bookId)
        {
            var reservations = await _reservationRepository.GetReservationsByBookIdAsync(bookId);
            return Ok(ApiResponse<List<Reservation>>.SuccessResponse(
                "Book reservations retrieved successfully",
                reservations));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<object>>> ReserveBook([FromBody] ReservationDto reservationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<object>.ErrorResponse("Invalid input data"));

            var success = await _reservationService.ReserveBookAsync(
                reservationDto.UserId,
                reservationDto.BookId);

            if (!success)
                return BadRequest(ApiResponse<object>.ErrorResponse(
                    "Failed to create reservation. Book may already be reserved by user."));

            return Ok(ApiResponse<object>.SuccessResponse("Book reserved successfully"));
        }

        [HttpPut("{id}/cancel")]
        public async Task<ActionResult<ApiResponse<object>>> CancelReservation(int id)
        {
            var success = await _reservationService.CancelReservationAsync(id);
            if (!success)
                return BadRequest(ApiResponse<object>.ErrorResponse("Failed to cancel reservation"));

            return Ok(ApiResponse<object>.SuccessResponse("Reservation cancelled successfully"));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteReservation(int id)
        {
            var reservation = await _reservationRepository.GetReservationByIdAsync(id);
            if (reservation == null)
                return NotFound(ApiResponse<object>.ErrorResponse("Reservation not found"));

            await _reservationRepository.DeleteReservationAsync(id);
            return Ok(ApiResponse<object>.SuccessResponse("Reservation deleted successfully"));
        }
    }

    public class ReservationDto
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
    }
}