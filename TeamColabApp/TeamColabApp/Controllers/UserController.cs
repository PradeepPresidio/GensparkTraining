using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamColabApp.Interfaces;
using TeamColabApp.Models.DTOs;
using TeamColabApp.Misc;
namespace TeamColabApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet("getAllUsers")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("by-name/{name}")]
        [Authorize]
        public async Task<ActionResult<UserResponseDto>> GetUserByName(string name)
        {
            try
            {
                var user = await _userService.GetUserByName(name);
                return Ok(user);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
        }

        [HttpGet("role/{role}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetUsersByRole(string role)
        {
            var users = await _userService.GetUsersByRoleAsync(role);
            return Ok(users);
        }

        [HttpGet("project/{projectName}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetUsersUnderProject(string projectName)
        {
            var users = await _userService.GetUsersUnderProjectAsync(projectName);
            return Ok(users);
        }

        [HttpPost]
        public async Task<ActionResult<UserResponseDto>> CreateUser([FromBody] UserRequestDto dto)
        {
            try
            {
                var user = await _userService.AddUserAsync(dto);
                return CreatedAtAction(nameof(GetUserByName), new { name = user.Name }, user);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{name}")]
        [Authorize]
        public async Task<ActionResult<UserResponseDto>> UpdateUser(string name, [FromBody] UserRequestDto dto)
        {
            try
            {
                // int id = EntityNameToId.GetUserId(name, _userService._context);
                // if (id == 0)
                // {
                //     return NotFound($"User with name '{name}' not found.");
                // }
                var updatedUser = await _userService.UpdateUserAsync(dto, name);
                return Ok(updatedUser);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPut("softDelete/{name}")]
        [Authorize]
        public async Task<ActionResult<bool>> SoftDeleteUser(string name)
        {
            try
            {
                bool isdeleted = await _userService.SoftDeleteUserAsync(name);
                if (!isdeleted)
                {
                    return StatusCode(500, "Failed to Soft Delete User");
                }
                return Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return StatusCode(500, "Internal Server error");
            }
        }
        [HttpPut("restore/{username}")]
        [Authorize]
        public async Task<ActionResult<UserResponseDto>> RestoreSoftDeletedUser(string username)
        {
            try
            {
                UserResponseDto restoredUser = await _userService.RetrieveSoftDeleteUserAsync(username);
                return Ok(restoredUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to restore user: {username}");
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{name}")]
        [Authorize]
        public async Task<ActionResult<bool>> HardDeleteUser(string name)
        {
            try
            {
                bool deleted = await _userService.HardDeleteUserAsync(name);
                if (!deleted)
                {
                    return StatusCode(500, "Failed to delete user");
                }
                return Ok(true);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
        }
    }
}
