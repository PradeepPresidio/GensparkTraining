using TeamColabApp.Validations;
namespace TeamColabApp.Models.DTOs
{
    public class NotificationResponseDto
    {
        public long NotificationId { get; set; }
        [ValidNotificationMessage]
        public string Message { get; set; } = string.Empty;
        public string Type { get; set; } = "General";
        public UserResponseDto? User { get; set; } = new UserResponseDto();
        public ProjectResponseDto? Project { get; set; } = new ProjectResponseDto();
        public ProjectTaskResponseDto? ProjectTask { get; set; } = new ProjectTaskResponseDto();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}