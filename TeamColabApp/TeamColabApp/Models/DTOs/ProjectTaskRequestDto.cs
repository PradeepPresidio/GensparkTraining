using TeamColabApp.Validations;
namespace TeamColabApp.Models.DTOs
{
    public class ProjectTaskRequestDto
    {
        [ValidName]
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string[]? AssignedUserNames { get; set; }
        [ValidName]
        public string? ProjectTitle { get; set; }
        [DateAfter2000]
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        [DateAfter2000]
        public DateTime EndDate { get; set; } = DateTime.UtcNow.AddDays(7);
        public string Status { get; set; } = "OnGoing";
        public bool IsActive { get; set; } = true;
        public string? Priority { get; set; } = "Medium";
    }
}