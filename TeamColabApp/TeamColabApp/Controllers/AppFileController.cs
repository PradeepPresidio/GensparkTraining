using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TeamColabApp.Interfaces;
using TeamColabApp.Models.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace TeamColabApp.Controllers
{
    
    [ApiController]
    [Authorize]
    [Route("api/files")]
    public class AppFileController : ControllerBase
    {
        private readonly IAppFileService _fileService;

        public AppFileController(IAppFileService fileService)
        {
            _fileService = fileService;
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<AppFileResponseDto>> UploadFile([FromForm] AppFileRequestDto fileRequest)
        {
            if (fileRequest.File == null || fileRequest.File.Length == 0)
                return BadRequest("No file uploaded.");

            using MemoryStream memoryStream = new MemoryStream();
            await fileRequest.File.CopyToAsync(memoryStream);
            // fileRequest.FileContent = memoryStream.ToArray();
            byte[] FileContent = memoryStream.ToArray();

            var result = await _fileService.UploadFileAsync(fileRequest, FileContent);
            return Ok(result);
        }

        [HttpPut("{fileId:long}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<AppFileResponseDto>> UpdateFile(long fileId, [FromForm] AppFileRequestDto fileRequest)
        {
            if (fileRequest.File == null)
            {
                return NoContent();
            }
            using MemoryStream memoryStream = new MemoryStream();
            await fileRequest.File.CopyToAsync(memoryStream);
            // fileRequest.FileContent = memoryStream.ToArray();
            byte[] FileContent = memoryStream.ToArray();
            var result = await _fileService.UpdateFileAsync((int)fileId, fileRequest, FileContent);
            return Ok(result);
        }

        [HttpDelete("soft/{fileId:long}")]
        public async Task<IActionResult> SoftDeleteFile(long fileId)
        {
            bool success = await _fileService.SoftDeleteFileAsync(fileId);
            if (!success)
                return NotFound($"File with ID {fileId} not found.");
            return Ok();
        }

        [HttpDelete("hard/{fileId:long}")]
        public async Task<IActionResult> HardDeleteFile(long fileId)
        {
            bool success = await _fileService.HardDeleteFileAsync(fileId);
            if (!success)
                return NotFound($"File with ID {fileId} not found.");
            return Ok();
        }

        [HttpPut("restore/{fileId:long}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<AppFileResponseDto>> RetrieveSoftDeleteFile(long fileId)
        {
            var restoredFile = await _fileService.RetrieveSoftDeleteFileAsync(fileId);
            return Ok(restoredFile);
        }

        [HttpGet("user/{userName}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<IEnumerable<AppFileResponseDto>>> GetFilesByUserName(string userName)
        {
            var files = await _fileService.GetFilesByUserNameAsync(userName);
            return Ok(files);
        }

        [HttpGet("project/{projectTitle}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<IEnumerable<AppFileResponseDto>>> GetFilesByProjectName(string projectTitle)
        {
            var files = await _fileService.GetFilesByProjectNameAsync(projectTitle);
            return Ok(files);
        }

        [HttpGet("task/{projectTaskId:long}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<IEnumerable<AppFileResponseDto>>> GetFilesByProjectTask(long projectTaskId)
        {
            var files = await _fileService.GetFilesByProjectTaskAsync(projectTaskId);
            return Ok(files);
        }

            [HttpGet("pageNo/{page}/pageSize/{pageSize}")]
            [Consumes("multipart/form-data")]
            public async Task<ActionResult<IEnumerable<AppFileResponseDto>>> GetAllFiles(int page = 1, int pageSize = 5)
            {
                    if (page < 1 || pageSize < 1)
                        return BadRequest("Page and pageSize must be greater than 0.");

                    var files = await _fileService.GetAllFilesAsync();
                    var pagedFiles = files
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    return Ok(pagedFiles);
            }
    }
}
