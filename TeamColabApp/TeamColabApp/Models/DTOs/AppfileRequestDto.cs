using Microsoft.AspNetCore.Http;
using TeamColabApp.Validations;
namespace TeamColabApp.Models.DTOs
{
    public class AppFileRequestDto
    {   [ValidName]
        public string FileName { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public IFormFile? File { get; set; }
        //    public byte[]? FileContent { get; set; } 
        [ValidName]
        public string? Username { get; set; }
        [ValidName]
        public string? ProjectTitle { get; set; }
        public int? ProjectTaskId { get; set; }
    }
}
