using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamColabApp.Interfaces;
using TeamColabApp.Models.DTOs;

namespace TeamColabApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpPost]
        [Authorize(Roles = "leader")]
        public async Task<IActionResult> AddProject([FromBody] ProjectRequestDto projectRequestDto)
        {
            var result = await _projectService.AddProjectAsync(projectRequestDto);
            return CreatedAtAction(nameof(GetProjectByTitle), new { title = result.Title }, result);
        }

        [HttpGet("/getAllProjects")]
        [Authorize]
        public async Task<IActionResult> GetAllProjects()
        {
            var result = await _projectService.GetAllProjectsAsync();
            return Ok(result);
        }

        [HttpGet("title/{title}")]
        [Authorize]
        public async Task<IActionResult> GetProjectByTitle(string title)
        {
            var result = await _projectService.GetProjectByTitleAsync(title);
            return Ok(result);
        }

        [HttpGet("team-leader/{name}")]
        [Authorize]
        public async Task<IActionResult> GetProjectsByTeamLeaderName(string name)
        {
            var result = await _projectService.GetProjectsByTeamLeaderNameAsync(name);
            return Ok(result);
        }

        [HttpGet("team-member/{name}")]
        [Authorize]
        public async Task<IActionResult> GetProjectsByTeamMemberName(string name)
        {
            var result = await _projectService.GetProjectsByTeamMemberNameAsync(name);
            return Ok(result);
        }

        [HttpGet("user/{name}")]
        [Authorize]
        public async Task<IActionResult> GetProjectsByUserName(string name)
        {
            var result = await _projectService.GetProjectsByUserNameAsync(name);
            return Ok(result);
        }

        [HttpGet("status/{status}")]
        [Authorize]
        public async Task<IActionResult> GetProjectsByStatus(string status)
        {
            var result = await _projectService.GetProjectsByStatusAsync(status);
            return Ok(result);
        }

        [HttpGet("priority/{priority}")]
        [Authorize]
        public async Task<IActionResult> GetProjectsByPriority(string priority)
        {
            var result = await _projectService.GetProjectsByPriorityAsync(priority);
            return Ok(result);
        }

        [HttpGet("date-range")]
        [Authorize]
        public async Task<IActionResult> GetProjectsByDateRange([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            var result = await _projectService.GetProjectsByDateRangeAsync(start, end);
            return Ok(result);
        }

        [HttpPut("{title}")]
        [Authorize(Roles = "leader")]
        public async Task<IActionResult> UpdateProject(string title, [FromBody] ProjectRequestDto projectRequestDto)
        {
            var result = await _projectService.UpdateProjectAsync(projectRequestDto, title);
            return Ok(result);
        }

        [HttpPut("{title}/soft-delete")]
        [Authorize(Roles = "leader")]
        public async Task<IActionResult> SoftDeleteProject(string title)
        {
            var success = await _projectService.SoftDeleteProjectAsync(title);
            if (!success) return NotFound("Project not found.");
            return Ok("Project soft deleted.");
        }

        [HttpDelete("{title}/hard-delete")]
        [Authorize(Roles = "leader")]
        public async Task<IActionResult> HardDeleteProject(string title)
        {
            var success = await _projectService.HardDeleteProjectAsync(title);
            if (!success) return NotFound("Project not found.");
            return Ok("Project hard deleted.");
        }

        [HttpPut("{title}/restore")]
        [Authorize(Roles = "leader")]
        public async Task<IActionResult> RestoreProject(string title)
        {
            var result = await _projectService.RetrieveSoftDeleteProjectAsync(title);
            return Ok(result);
        }
    }
}
