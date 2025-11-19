using Microsoft.AspNetCore.Mvc;
using MauiApp1.Interfaces;
using MauiApp1.Models;

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
        public async Task<ActionResult<List<Loan>>> GetAllLoans()
        {
            var loans = await _loanService.GetAllLoansAsync();
            return Ok(loans);
        }

        [HttpGet("overdue")]
        public async Task<ActionResult<List<Loan>>> GetOverdueLoans()
        {
            var loans = await _loanService.GetOverdueLoansAsync();
            return Ok(loans);
        }

        [HttpPost]
        public async Task<ActionResult> BorrowBook([FromBody] LoanDto loanDto)
        {
            var success = await _loanService.BorrowBookAsync(loanDto.UserId, loanDto.BookId);
            if (!success) return BadRequest(new { message = "Failed to borrow book. Check availability and borrow limit." });
            return Ok(new { message = "Book borrowed successfully" });
        }

        [HttpPut("{id}/return")]
        public async Task<ActionResult> ReturnBook(int id)
        {
            var success = await _loanService.ReturnBookAsync(id);
            if (!success) return BadRequest(new { message = "Failed to return book" });
            return Ok(new { message = "Book returned successfully" });
        }
    }

    public class LoanDto
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
    }
}