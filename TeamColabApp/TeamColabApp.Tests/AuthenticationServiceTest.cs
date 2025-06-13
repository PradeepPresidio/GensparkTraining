using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using TeamColabApp.Contexts;
using TeamColabApp.Interfaces;
using TeamColabApp.Models;
using TeamColabApp.Models.DTOs;
using TeamColabApp.Services;
using Xunit;
using Microsoft.EntityFrameworkCore;

namespace TeamColabApp.Tests
{
    public class AuthenticationServiceTest
    {
        private readonly AuthenticationService _authService;
        private readonly Mock<ITokenService> _tokenServiceMock = new();
        private readonly Mock<IEncryptionService> _encryptionServiceMock = new();
        private readonly Mock<IRepository<long, User>> _userRepoMock = new();
        private readonly Mock<IRepository<long, UserToken>> _tokenRepoMock = new();
        private readonly TeamColabAppContext _context;

        public AuthenticationServiceTest()
        {
            var options = new DbContextOptionsBuilder<TeamColabAppContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new TeamColabAppContext(options);

            var logger = new Mock<ILogger<AuthenticationService>>();
            _authService = new AuthenticationService(
                _tokenServiceMock.Object,
                _encryptionServiceMock.Object,
                _userRepoMock.Object,
                _tokenRepoMock.Object,
                logger.Object,
                _context
            );
        }

        [Fact]
        public async Task Login_ShouldReturnTokens_WhenCredentialsAreValid()
        {
            // Arrange
            User dbUser = new User
            {
                UserId = 1,
                Name = "TestUser",
                Password = "hashedpassword",
                Role = "Member" // Use string if enum is not available
            };
            _context.Users.Add(dbUser);
            _context.SaveChanges();

            UserLoginRequestDto request = new UserLoginRequestDto
            {
                Username = "TestUser",
                Password = "plainpassword"
            };

            _userRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(dbUser);
            _encryptionServiceMock.Setup(e => e.VerifyPassword("plainpassword", "hashedpassword")).ReturnsAsync(true);
            _tokenServiceMock.Setup(t => t.GenerateToken(dbUser)).ReturnsAsync("token");
            _tokenServiceMock.Setup(t => t.GenerateRefreshToken()).Returns("refresh-token");
            _tokenRepoMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

            // Act
            UserLoginResponseDto result = await _authService.Login(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("TestUser", result.Username);
            Assert.Equal("token", result.Token);
            Assert.Equal("refresh-token", result.RefreshToken);
        }

        [Fact]
        public async Task Logout_ShouldAddToBlacklist_WhenTokenIsValid()
        {
            // Arrange
            var handler = new JwtSecurityTokenHandler();
            var token = new JwtSecurityToken(issuer: "issuer", expires: DateTime.UtcNow.AddMinutes(10));
            string tokenStr = handler.WriteToken(token);

            _tokenRepoMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

            // Act
            bool result = await _authService.Logout(tokenStr);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task RefreshToken_ShouldReturnNewTokens_WhenValid()
        {
            // Arrange
            string oldRefreshToken = "old-refresh-token";
            UserToken storedToken = new UserToken
            {
                TokenId = 1,
                TokenValue = oldRefreshToken,
                TokenType = TokenType.Refresh,
                ExpiresAt = DateTime.UtcNow.AddDays(1)
            };

            User dbUser = new User
            {
                UserId = 2,
                Name = "User2",
                Password = "password",
                Role = "Member"
            };

            _context.Users.Add(dbUser);
            _context.SaveChanges();

            _tokenRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<UserToken> { storedToken });
            _userRepoMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(dbUser);
           _tokenRepoMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);
            _tokenServiceMock.Setup(t => t.GenerateToken(dbUser)).ReturnsAsync("new-access-token");
            _tokenServiceMock.Setup(t => t.GenerateRefreshToken()).Returns("new-refresh-token");
            _tokenRepoMock.Setup(r => r.AddAsync(It.IsAny<UserToken>())).ReturnsAsync(new UserToken());

            // Act
            UserLoginResponseDto? result = await _authService.RefreshToken(oldRefreshToken, "User2");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("User2", result.Username);
            Assert.Equal("new-access-token", result.Token);
            Assert.Equal("new-refresh-token", result.RefreshToken);
        }
    }
}
