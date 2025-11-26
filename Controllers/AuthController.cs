using Microsoft.AspNetCore.Mvc;
using SmartLibraryAPI.Interfaces;
using SmartLibraryAPI.Models;
using SmartLibraryAPI.Factories;
using SmartLibraryAPI.DTOs.Request;
using SmartLibraryAPI.DTOs.Response;

namespace SmartLibraryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;

        public AuthController(IAuthService authService, IUserRepository userRepository)
        {
            _authService = authService;
            _userRepository = userRepository;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<object>>> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<object>.ErrorResponse("Invalid input data"));

            var success = await _authService.RegisterAsync(
                request.Username,
                request.Password,
                request.Email,
                request.FullName,
                request.StudentId,
                request.Role);

            if (!success)
                return BadRequest(ApiResponse<object>.ErrorResponse("Username already exists"));

            // Also create a User entry for Student role
            if (request.Role == "Student")
            {
                var user = UserFactory.CreateUser(
                    "Student",
                    request.FullName,
                    request.Email,
                    "",
                    request.StudentId);
                await _userRepository.AddUserAsync(user);
            }

            return Ok(ApiResponse<object>.SuccessResponse("Registration successful"));
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<LoginResponse>>> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<LoginResponse>.ErrorResponse("Invalid input data"));

            var account = await _authService.LoginAsync(request.Username, request.Password);
            if (account == null)
                return Unauthorized(ApiResponse<LoginResponse>.ErrorResponse("Invalid username or password"));

            var response = new LoginResponse
            {
                Id = account.Id,
                Username = account.Username,
                Email = account.Email,
                FullName = account.FullName,
                Role = account.Role,
                LoginTime = DateTime.UtcNow
            };

            return Ok(ApiResponse<LoginResponse>.SuccessResponse("Login successful", response));
        }

        [HttpPut("change-password")]
        public async Task<ActionResult<ApiResponse<object>>> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<object>.ErrorResponse("Invalid input data"));

            var success = await _authService.ChangePasswordAsync(
                request.AccountId,
                request.CurrentPassword,
                request.NewPassword);

            if (!success)
                return BadRequest(ApiResponse<object>.ErrorResponse("Invalid current password or account not found"));

            return Ok(ApiResponse<object>.SuccessResponse("Password changed successfully"));
        }

        [HttpGet("accounts")]
        public async Task<ActionResult<ApiResponse<List<Account>>>> GetAllAccounts()
        {
            var accounts = await _authService.GetAllAccountsAsync();
            return Ok(ApiResponse<List<Account>>.SuccessResponse("Accounts retrieved successfully", accounts));
        }

        [HttpGet("accounts/{id}")]
        public async Task<ActionResult<ApiResponse<Account>>> GetAccountById(int id)
        {
            var account = await _authService.GetAccountByIdAsync(id);
            if (account == null)
                return NotFound(ApiResponse<Account>.ErrorResponse("Account not found"));

            return Ok(ApiResponse<Account>.SuccessResponse("Account retrieved successfully", account));
        }

        [HttpDelete("accounts/{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteAccount(int id)
        {
            var success = await _authService.DeleteAccountAsync(id);
            if (!success)
                return NotFound(ApiResponse<object>.ErrorResponse("Account not found"));

            return Ok(ApiResponse<object>.SuccessResponse("Account deleted successfully"));
        }
    }
}