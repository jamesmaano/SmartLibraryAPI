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
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<User>>>> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return Ok(ApiResponse<List<User>>.SuccessResponse("Users retrieved successfully", users));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<User>>> GetUserById(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
                return NotFound(ApiResponse<User>.ErrorResponse("User not found"));

            return Ok(ApiResponse<User>.SuccessResponse("User retrieved successfully", user));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<User>>> AddUser([FromBody] AddUserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<User>.ErrorResponse("Invalid input data"));

            var user = UserFactory.CreateUser(
                request.UserType,
                request.Name,
                request.Email ?? "",
                request.PhoneNumber ?? "",
                request.IdNumber ?? ""
            );

            await _userRepository.AddUserAsync(user);

            return CreatedAtAction(
                nameof(GetUserById),
                new { id = user.Id },
                ApiResponse<User>.SuccessResponse("User added successfully", user));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteUser(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
                return NotFound(ApiResponse<object>.ErrorResponse("User not found"));

            await _userRepository.DeleteUserAsync(id);
            return Ok(ApiResponse<object>.SuccessResponse("User deleted successfully"));
        }
    }
}