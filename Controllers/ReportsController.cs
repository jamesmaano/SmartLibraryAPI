using Microsoft.AspNetCore.Mvc;
using SmartLibraryAPI.Interfaces;
using SmartLibraryAPI.Models;
using SmartLibraryAPI.Factories;

namespace SmartLibraryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("inventory")]
        public async Task<ActionResult> GetInventoryReport()
        {
            var report = await _reportService.GenerateInventoryReportAsync();
            return Ok(report);
        }

        [HttpGet("overdue")]
        public async Task<ActionResult> GetOverdueReport()
        {
            var report = await _reportService.GenerateOverdueBooksReportAsync();
            return Ok(report);
        }

        [HttpGet("mostBorrowed")]
        public async Task<ActionResult> GetMostBorrowedReport()
        {
            var report = await _reportService.GenerateMostBorrowedBooksReportAsync();
            return Ok(report);
        }

        [HttpGet("userActivity")]
        public async Task<ActionResult> GetUserActivityReport()
        {
            var report = await _reportService.GenerateUserActivityReportAsync();
            return Ok(report);
        }

        [HttpGet("fines")]
        public async Task<ActionResult> GetFinesReport()
        {
            var report = await _reportService.GenerateFinesReportAsync();
            return Ok(report);
        }
    }
}
