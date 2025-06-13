using TeamColabApp.Validations;
namespace TeamColabApp.Models.DTOs
{
    public class CommentResponseDto
    {
        public long CommentId { get; set; }
        public string Content { get; set; } = string.Empty;
        [DateAfter2000]
        public DateTime CreatedAt { get; set; }
        [DateAfter2000]
        public DateTime UpdatedAt { get; set; }
        public UserResponseDto? User { get; set; } = new UserResponseDto();
        public ProjectResponseDto? Project { get; set; } = new ProjectResponseDto();
        public ProjectTaskResponseDto? ProjectTask { get; set; } = new ProjectTaskResponseDto();
        public long ProjectId { get; set; }
        public long ProjectTaskId { get; set; }
    }
}