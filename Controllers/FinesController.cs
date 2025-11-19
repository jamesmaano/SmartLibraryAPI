using Microsoft.AspNetCore.Mvc;
using MauiApp1.Interfaces;

namespace SmartLibraryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FinesController : ControllerBase
    {
        private readonly IFineService _fineService;

        public FinesController(IFineService fineService)
        {
            _fineService = fineService;
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult> GetUserFines(int userId)
        {
            var fines = await _fineService.GetUnpaidFinesAsync(userId);
            return Ok(fines);
        }

        [HttpPut("{fineId}/pay")]
        public async Task<ActionResult> PayFine(int fineId)
        {
            var success = await _fineService.PayFineAsync(fineId);
            if (!success) return BadRequest(new { message = "Failed to pay fine" });
            return Ok(new { message = "Fine paid successfully" });
        }
    }
}