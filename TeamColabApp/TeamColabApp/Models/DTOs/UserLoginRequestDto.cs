using TeamColabApp.Validations;
namespace TeamColabApp.Models.DTOs
{
    public class UserLoginRequestDto
    {
        [ValidName]
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
