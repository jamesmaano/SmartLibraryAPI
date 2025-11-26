using Microsoft.AspNetCore.Mvc;
using SmartLibraryAPI.Interfaces;
using SmartLibraryAPI.Models;
using SmartLibraryAPI.DTOs.Response;

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
        public async Task<ActionResult<ApiResponse<List<Fine>>>> GetUserFines(int userId)
        {
            var fines = await _fineService.GetUnpaidFinesAsync(userId);
            return Ok(ApiResponse<List<Fine>>.SuccessResponse("User fines retrieved successfully", fines));
        }

        [HttpGet("user/{userId}/total")]
        public async Task<ActionResult<ApiResponse<decimal>>> GetUserTotalFines(int userId)
        {
            var total = await _fineService.GetTotalFinesAsync(userId);
            return Ok(ApiResponse<decimal>.SuccessResponse("Total fines calculated successfully", total));
        }

        [HttpPut("{fineId}/pay")]
        public async Task<ActionResult<ApiResponse<object>>> PayFine(int fineId)
        {
            var success = await _fineService.PayFineAsync(fineId);
            if (!success)
                return BadRequest(ApiResponse<object>.ErrorResponse("Failed to pay fine"));

            return Ok(ApiResponse<object>.SuccessResponse("Fine paid successfully"));
        }
    }
}