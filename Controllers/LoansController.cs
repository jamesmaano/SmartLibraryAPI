using Microsoft.AspNetCore.Mvc;
using SmartLibraryAPI.Interfaces;
using SmartLibraryAPI.Models;
using SmartLibraryAPI.DTOs.Request;
using SmartLibraryAPI.DTOs.Response;

namespace SmartLibraryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoansController : ControllerBase
    {
        private readonly ILoanService _loanService;

        public LoansController(ILoanService loanService)
        {
            _loanService = loanService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<Loan>>>> GetAllLoans()
        {
            var loans = await _loanService.GetAllLoansAsync();
            return Ok(ApiResponse<List<Loan>>.SuccessResponse("Loans retrieved successfully", loans));
        }

        [HttpGet("overdue")]
        public async Task<ActionResult<ApiResponse<List<Loan>>>> GetOverdueLoans()
        {
            var loans = await _loanService.GetOverdueLoansAsync();
            return Ok(ApiResponse<List<Loan>>.SuccessResponse("Overdue loans retrieved successfully", loans));
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<ApiResponse<List<Loan>>>> GetLoansByUser(int userId)
        {
            var allLoans = await _loanService.GetAllLoansAsync();
            var userLoans = allLoans.Where(l => l.UserId == userId).ToList();
            return Ok(ApiResponse<List<Loan>>.SuccessResponse("User loans retrieved successfully", userLoans));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<object>>> BorrowBook([FromBody] BorrowBookRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<object>.ErrorResponse("Invalid input data"));

            var success = await _loanService.BorrowBookAsync(request.UserId, request.BookId);
            if (!success)
                return BadRequest(ApiResponse<object>.ErrorResponse(
                    "Failed to borrow book. Check availability and borrow limit."));

            return Ok(ApiResponse<object>.SuccessResponse("Book borrowed successfully"));
        }

        [HttpPut("{id}/return")]
        public async Task<ActionResult<ApiResponse<object>>> ReturnBook(int id)
        {
            var success = await _loanService.ReturnBookAsync(id);
            if (!success)
                return BadRequest(ApiResponse<object>.ErrorResponse("Failed to return book"));

            return Ok(ApiResponse<object>.SuccessResponse("Book returned successfully"));
        }
    }
}