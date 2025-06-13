using TeamColabApp.Validations;
namespace TeamColabApp.Models.DTOs
{

    public class UserRequestDto
    {
        [ValidName]
        public string Name { get; set; } = string.Empty;
        public string? Password { get; set; }
        public string Role { get; set; } = "User";
        // public bool IsActive { get; set; } = true;
    }
}