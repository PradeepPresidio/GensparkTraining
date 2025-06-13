using TeamColabApp.Validations;
namespace TeamColabApp.Models.DTOs
{
    public class NotificationRequestDto
    {
        [ValidNotificationMessage]
        public string Message { get; set; } = string.Empty;
        public string Type { get; set; } = "General";
        public long? UserId { get; set; }
        public long? ProjectId { get; set; }
        public long? ProjectTaskId { get; set; }
    }
}