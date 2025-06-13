using TeamColabApp.Validations;
namespace TeamColabApp.Models.DTOs
{
    public class ProjectRequestDto
    {
        [ValidName]
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [DateAfter2000]
        public DateTime StartDate { get; set; }
        [DateAfter2000]
        public DateTime EndDate { get; set; }
        [ValidName]
        public required string TeamLeaderName { get; set; }
        public List<string> TeamMembersName { get; set; } = [];
    }
}