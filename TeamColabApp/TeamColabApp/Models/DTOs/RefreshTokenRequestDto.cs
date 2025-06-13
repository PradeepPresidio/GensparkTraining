using TeamColabApp.Validations;
namespace TeamColabApp.Models.DTOs
{
    public class RefreshTokenRequestDto
    {
        public string RefreshToken { get; set; } = string.Empty;
        [ValidName]
        public string Username { get; set; } = string.Empty;
    }
}