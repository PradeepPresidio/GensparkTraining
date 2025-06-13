using System.ComponentModel.DataAnnotations.Schema;
using TeamColabApp.Validations;
namespace TeamColabApp.Models.DTOs
{
    public class ProjectResponseDto
    {
        public long ProjectId { get; set; }
        [ValidName]
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [ValidName]
        public DateTime StartDate { get; set; }
        [ValidName]
        public DateTime EndDate { get; set; }
        public long? TeamLeaderId { get; set; }
        [ForeignKey("TeamLeaderId")]
        public UserResponseDto? TeamLeader { get; set; }
        public string Status { get; set; } = "Ongoing";
        public string Priority { get; set; } = "Medium";
        [DateAfter2000]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [DateAfter2000]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
        public List<UserResponseDto>? Users { get; set; } = [];
        public List<ProjectTaskResponseDto>? ProjectTasks { get; set; } = [];
        
    }
}