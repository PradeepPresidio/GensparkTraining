using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TeamColabApp.Models;
using TeamColabApp.Services;
using Xunit;

namespace TeamColabApp.Tests
{
    public class TokenServiceTests
    {
        private readonly IConfiguration _configuration;

        public TokenServiceTests()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                { "Keys:JwtTokenKey", "this_is_a_dummy_jwt_secret_key_1234567890" }
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings!)
                .Build();
        }

        [Fact]
        public async Task GenerateToken_ReturnsValidJwt()
        {
            // Arrange
            var tokenService = new TokenService(_configuration);
            var user = new User { Name = "john", Role = "Admin" };

            // Act
            string token = await tokenService.GenerateToken(user);

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(token));

            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Keys:JwtTokenKey"]!)),
                ValidateLifetime = false 
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
            Assert.Equal("john", principal.FindFirstValue(ClaimTypes.NameIdentifier));
            Assert.Equal("Admin", principal.FindFirstValue(ClaimTypes.Role));
        }

        [Fact]
        public void GenerateRefreshToken_ReturnsNonEmptyToken()
        {
            // Arrange
            var tokenService = new TokenService(_configuration);

            // Act
            string refreshToken = tokenService.GenerateRefreshToken();

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(refreshToken));
            Assert.True(refreshToken.Length > 50); 
        }

        [Fact]
        public void Constructor_ThrowsIfKeyMissing()
        {
            // Arrange
            var badConfig = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>()).Build();

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => new TokenService(badConfig));
            Assert.Equal("JWT token key is not configured.", ex.Message);
        }
    }
}
