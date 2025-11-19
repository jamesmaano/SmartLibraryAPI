using Microsoft.AspNetCore.Mvc;
using MauiApp1.Interfaces;
using MauiApp1.Models;

namespace SmartLibraryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest(new { message = "Username and password are required" });

            var success = await _authService.RegisterAsync(dto.Username, dto.Password, dto.Email, dto.Role);
            if (!success)
                return BadRequest(new { message = "Username already exists" });

            return Ok(new { message = "Registration successful" });
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDto dto)
        {
            var account = await _authService.LoginAsync(dto.Username, dto.Password);

            if (account == null)
                return Unauthorized(new { message = "Invalid username or password" });

            return Ok(new
            {
                message = "Login successful",
                accountId = account.Id,
                username = account.Username,
                role = account.Role,
                email = account.Email
            });
        }

        [HttpGet("accounts")]
        public async Task<ActionResult> GetAllAccounts()
        {
            var accounts = await _authService.GetAllAccountsAsync();
            return Ok(accounts);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAccount(int id)
        {
            var success = await _authService.DeleteAccountAsync(id);
            if (!success)
                return NotFound(new { message = "Account not found" });

            return Ok(new { message = "Account deleted successfully" });
        }
    }

    public class RegisterDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Role { get; set; } = "User"; // User, Librarian, Admin
    }

    public class LoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}