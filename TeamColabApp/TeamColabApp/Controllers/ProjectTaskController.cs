using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamColabApp.Interfaces;
using TeamColabApp.Models.DTOs;

namespace TeamColabApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProjectTaskController : ControllerBase
    {
        private readonly IProjectTaskService _projectTaskService;

        public ProjectTaskController(IProjectTaskService service)
        {
            _projectTaskService = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectTaskResponseDto>>> GetAllProjectTasks()
        {
            var tasks = await _projectTaskService.GetAllAsync();
            return Ok(tasks);
        }

        [HttpGet("project/{projectName}/task/{taskName}")]
        public async Task<ActionResult<ProjectTaskResponseDto>> GetProjectTask(string projectName, string taskName)
        {
            try
            {
                var task = await _projectTaskService.GetProjectTaskAsync(projectName, taskName);
                return Ok(task);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("project/{projectName}")]
        public async Task<ActionResult<IEnumerable<ProjectTaskResponseDto>>> GetTasksByProject(string projectName)
        {
            var tasks = await _projectTaskService.GetProjectTasksByProjectNameAsync(projectName);
            return Ok(tasks);
        }

        [HttpGet("project/{projectName}/user/{userName}")]
        public async Task<ActionResult<IEnumerable<ProjectTaskResponseDto>>> GetTasksByUser(string projectName, string userName)
        {
            var tasks = await _projectTaskService.GetProjectTasksByUserNameAsync(projectName, userName);
            return Ok(tasks);
        }

        [HttpGet("project/{projectName}/status/{status}")]
        public async Task<ActionResult<IEnumerable<ProjectTaskResponseDto>>> GetTasksByStatus(string projectName, string status)
        {
            var tasks = await _projectTaskService.GetProjectTasksByStatusAsync(projectName, status);
            return Ok(tasks);
        }

        [HttpGet("project/{projectName}/priority/{priority}")]
        public async Task<ActionResult<IEnumerable<ProjectTaskResponseDto>>> GetTasksByPriority(string projectName, string priority)
        {
            var tasks = await _projectTaskService.GetProjectTasksByPriorityAsync(projectName, priority);
            return Ok(tasks);
        }

        [HttpGet("project/{projectName}/range")]
        public async Task<ActionResult<IEnumerable<ProjectTaskResponseDto>>> GetTasksByDateRange(
            string projectName, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var tasks = await _projectTaskService.GetProjectTasksByDateRangeAsync(projectName, startDate, endDate);
            return Ok(tasks);
        }

        [HttpPost]
        [Authorize(Roles = "leader")]
        public async Task<ActionResult<ProjectTaskResponseDto>> CreateProjectTask([FromBody] ProjectTaskRequestDto dto)
        {
            var created = await _projectTaskService.AddAsync(dto);
            return CreatedAtAction(nameof(GetProjectTask), new { projectName = created.Project?.Title, taskName = created.Title }, created);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "leader")]
        public async Task<ActionResult<ProjectTaskResponseDto>> UpdateProjectTask(long id, [FromBody] ProjectTaskRequestDto dto)
        {
            try
            {
                var updated = await _projectTaskService.UpdateAsync(id, dto);
                return Ok(updated);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("soft-delete/{id}")]
        [Authorize(Roles = "leader")]
        public async Task<IActionResult> SoftDeleteProjectTask(long id)
        {
            bool result = await _projectTaskService.SoftDeleteAsync(id);
            if (!result)
                return NotFound("Project task not found.");

            return Ok("Project task soft deleted successfully.");
        }

        [HttpDelete("hard-delete/{id}")]
        [Authorize(Roles = "leader")]
        public async Task<IActionResult> HardDeleteProjectTask(long id)
        {
            bool result = await _projectTaskService.HardDeleteAsync(id);
            if (!result)
                return NotFound("Project task not found.");

            return Ok("Project task hard deleted successfully.");
        }

        [HttpPut("restore/{id}")]
        [Authorize(Roles = "leader")]
        public async Task<IActionResult> RestoreSoftDeletedProjectTask(long id)
        {
            try
            {
                ProjectTaskResponseDto restoredTask = await _projectTaskService.RetrieveSoftDeleteAsync(id);
                return Ok(restoredTask);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }   
    }
}
