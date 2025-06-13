using TeamColabApp.Validations;
namespace TeamColabApp.Models.DTOs
{
    public class UserResponseDto
    {
        public long Id { get; set; }
        [ValidName]
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
        public bool IsActive { get; set; } = true;
        [DateAfter2000]
        public DateTime CreatedAt { get; set; }
        [DateAfter2000]
        public DateTime UpdatedAt { get; set; }
        public ICollection<ProjectResponseDto>? Projects { get; set; } = [];
        public ICollection<ProjectTaskResponseDto>? ProjectTasks { get; set; } = [];
        public ICollection<CommentResponseDto>? Comments { get; set; } = [];
        public ICollection<NotificationResponseDto>? Notifications { get; set; } = [];
        public ICollection<AppFileResponseDto>? UserFiles { get; set; } = [];
        // public List<int> ProjectTaskIds { get; set; } = [];
    }
}