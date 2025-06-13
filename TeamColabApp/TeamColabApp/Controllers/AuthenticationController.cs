using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TeamColabApp.Interfaces;
using TeamColabApp.Models.DTOs;
using TeamColabApp.Contexts;

namespace TeamColabApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ILogger<AuthenticationController> _logger;
        private readonly TeamColabAppContext _context;

        public AuthenticationController(
            IAuthenticationService authenticationService,
            ILogger<AuthenticationController> logger,
            TeamColabAppContext context)
        {
            _authenticationService = authenticationService;
            _logger = logger;
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<UserLoginResponseDto>> UserLogin([FromBody] UserLoginRequestDto loginRequest)
        {
            try
            {
                var result = await _authenticationService.Login(loginRequest);
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return Unauthorized(e.Message);
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                string? token = Request.Headers["Authorization"]
                    .FirstOrDefault()?.Split(" ").Last();

                if (string.IsNullOrEmpty(token))
                {
                    return BadRequest("Token is missing");
                }

                bool hasLoggedOut = await _authenticationService.Logout(token);
                if (!hasLoggedOut)
                {
                    return Unauthorized("Invalid token format.");
                }

                return Ok("Logged out successfully");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return Unauthorized(e.Message);
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto request)
        {
            try
            {
                var newTokenSet = await _authenticationService.RefreshToken(request.RefreshToken, request.Username);

                if (newTokenSet == null)
                    return Unauthorized("Invalid or expired refresh token.");

                return Ok(newTokenSet);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while refreshing token");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
            }
        }

    }
}
