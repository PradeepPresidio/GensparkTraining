using Xunit;
using System.Threading.Tasks;
using TeamColabApp.Models;
using TeamColabApp.Services; 
using TeamColabApp.Interfaces;

namespace TeamColabApp.Tests.Services
{
    public class EncryptionServiceTests
    {
        private readonly IEncryptionService _encryptionService;

        public EncryptionServiceTests()
        {
            _encryptionService = new EncryptionService();
        }

        [Fact]
        public async Task EncryptData_ShouldHashDataSuccessfully()
        {
            // Arrange
            EncryptModel model = new EncryptModel { Data = "mysecret" };

            // Act
            EncryptModel result = await _encryptionService.EncryptData(model);

            // Assert
            Assert.NotNull(result.EncryptedString);
            Assert.NotEqual("mysecret", result.EncryptedString);
            Assert.StartsWith("$2", result.EncryptedString); 
        }

        [Fact]
        public async Task VerifyPassword_ShouldReturnTrue_ForCorrectPassword()
        {
            // Arrange
            string password = "password123";
            string hashed = BCrypt.Net.BCrypt.HashPassword(password);

            // Act
            bool isValid = await _encryptionService.VerifyPassword(password, hashed);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public async Task VerifyPassword_ShouldReturnFalse_ForIncorrectPassword()
        {
            // Arrange
            string password = "password123";
            string hashed = BCrypt.Net.BCrypt.HashPassword(password);

            // Act
            bool isValid = await _encryptionService.VerifyPassword("wrongpassword", hashed);

            // Assert
            Assert.False(isValid);
        }
    }
}
