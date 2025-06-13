using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using TeamColabApp.Interfaces;
using TeamColabApp.Models;

namespace TeamColabApp.Middleware
{
    public class TokenBlacklistMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TokenBlacklistMiddleware> _logger;

        public TokenBlacklistMiddleware(RequestDelegate next, ILogger<TokenBlacklistMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IRepository<long, UserToken> tokenRepository)
        {
            string? token = context.Request.Headers["Authorization"]
                .FirstOrDefault()?.Split(" ").Last();
            if (!string.IsNullOrEmpty(token))
            {
                var blacklisted = (await tokenRepository.GetAllAsync())
                    .Any(t => t != null && t.TokenType == TokenType.Blacklist && t.TokenValue == token);

                if (blacklisted)
                {
                    _logger.LogWarning("Blocked request with blacklisted token.");
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Token has been blacklisted.");
                    return;
                }
            }

            await _next(context);
        }
    }
}
