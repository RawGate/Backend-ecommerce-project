using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using backend_teamwork1.DTOs;
using backend_teamwork1.Services;
using backend_teamwork.Services;

namespace backend_teamwork1.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly AuthService _authService;

        public UserController(UserService userService, AuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [HttpGet]
        //[Authorize(Roles = "admin")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllUsers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var users = await _userService.GetAllUsersAsync(pageNumber, pageSize);
                return ApiResponse.Success(users, "All users retrieved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse.ServerError($"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{userId}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return ApiResponse.NotFound("User not found");
                }
                return ApiResponse.Success(user, "User retrieved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse.ServerError($"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddUser(CreateUserDto createUserDto)
        {
            try
            {
                var newUserDto = await _userService.AddUserAsync(createUserDto);
                return ApiResponse.Created(newUserDto, "User created successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse.ServerError($"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{userId}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateUser(Guid userId, UpdateUserDto updateUserDto)
        {
            try
            {
                var updatedUserDto = await _userService.UpdateUserAsync(userId, updateUserDto);
                return ApiResponse.Success(updatedUserDto, "User updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse.ServerError($"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{userId}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            try
            {
                var isDeleted = await _userService.DeleteUserAsync(userId);
                if (!isDeleted)
                {
                    return ApiResponse.NotFound("User not found");
                }
                return ApiResponse.Success(true, "User deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse.ServerError($"Internal server error: {ex.Message}");
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var user = await _userService.LoginAsync(loginDto);
                if (user == null)
                {
                    return ApiResponse.NotFound("User not found or invalid credentials");
                }

                var token = _authService.GenerateJwt(user);

                return ApiResponse.Success(new { Token = token, User = user }, "User logged in successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse.ServerError($"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetUserProfile()
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var user = await _userService.GetUserByIdAsync(userId);

                if (user == null)
                {
                    return ApiResponse.NotFound("User profile not found");
                }

                var isAdmin = User.IsInRole("admin");

                return ApiResponse.Success(new { User = user, IsAdmin = isAdmin }, "User profile retrieved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse.ServerError($"Internal server error: {ex.Message}");
            }
        }
    }
}
