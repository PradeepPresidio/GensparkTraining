using TeamColabApp.Validations;
namespace TeamColabApp.Models.DTOs
{
    public class AppFileResponseDto
    {
        public long FileId { get; set; }
        [ValidName]
        public string FileName { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public byte[] Content { get; set; }
        public UserResponseDto? User { get; set; } = new UserResponseDto();
        public ProjectResponseDto? Project { get; set; } = new ProjectResponseDto();
        public ProjectTaskResponseDto? ProjectTask { get; set; } = new ProjectTaskResponseDto();
        [DateAfter2000]
        public DateTime UploadedDate { get; set; }
        [DateAfter2000]
        public DateTime UpdatedDate { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}