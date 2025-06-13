using TeamColabApp.Validations;
namespace TeamColabApp.Models.DTOs
{
    public class ProjectTaskResponseDto
    {
        public long ProjectTaskId { get; set; }
        [ValidName]
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [DateAfter2000]
        public DateTime StartDate { get; set; }
        [DateAfter2000]
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; } = true;
        public string Status { get; set; } = "Pending";
        public string Priority { get; set; } = "Medium";
        public ICollection<UserResponseDto>? AssignedUsers { get; set; } = [];
        public ProjectResponseDto? Project { get; set; } = new ProjectResponseDto();
        public ICollection<CommentResponseDto>? Comments { get; set; } = [];
        public ICollection<NotificationResponseDto>? Notifications { get; set; } = [];
        public ICollection<AppFileResponseDto>? ProjectTaskFiles { get; set; } = [];
        [DateAfter2000]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [DateAfter2000]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    }
}