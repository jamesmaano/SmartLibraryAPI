using Microsoft.AspNetCore.Mvc;
using MauiApp1.Interfaces;
using MauiApp1.Models;
using MauiApp1.Factories;

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
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return NotFound(new { message = "User not found" });
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult> AddUser([FromBody] UserDto userDto)
        {
            if (string.IsNullOrWhiteSpace(userDto.UserType) || string.IsNullOrWhiteSpace(userDto.Name))
                return BadRequest(new { message = "User type and name are required" });

            var user = UserFactory.CreateUser(
                userDto.UserType ?? "",
                userDto.Name ?? "",
                userDto.Email ?? "",
                userDto.PhoneNumber ?? "",
                userDto.IdNumber ?? ""
            );
            await _userRepository.AddUserAsync(user);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return NotFound(new { message = "User not found" });

            await _userRepository.DeleteUserAsync(id);
            return Ok(new { message = "User deleted successfully" });
        }
    }

    public class UserDto
    {
        public string? UserType { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? IdNumber { get; set; }
    }
}