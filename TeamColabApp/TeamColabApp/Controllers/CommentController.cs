using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamColabApp.Interfaces;
using TeamColabApp.Models.DTOs;

namespace TeamColabApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost]
        public async Task<ActionResult<CommentResponseDto>> AddComment([FromBody] CommentRequestDto request)
        {
            var result = await _commentService.AddCommentAsync(request);
            return CreatedAtAction(nameof(GetAllComments), new { id = result.CommentId }, result);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<CommentResponseDto>> UpdateComment(long id, [FromBody] CommentRequestDto request)
        {
            try
            {
                var result = await _commentService.UpdateCommentAsync(id, request);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteComment(long id)
        {
            bool deleted = await _commentService.DeleteCommentAsync(id);
            if (!deleted)
                return NotFound($"Comment with ID {id} not found.");
            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentResponseDto>>> GetAllComments()
        {
            var result = await _commentService.GetAllCommentsAsync();
            return Ok(result);
        }

        [HttpGet("user/{username}")]
        public async Task<ActionResult<IEnumerable<CommentResponseDto>>> GetCommentsByUser(string username)
        {
            var result = await _commentService.GetCommentsByUserNameAsync(username);
            return Ok(result);
        }

        [HttpGet("project/{projectName}")]
        public async Task<ActionResult<IEnumerable<CommentResponseDto>>> GetCommentsByProject(string projectName)
        {
            var result = await _commentService.GetCommentsByProjectNameAsync(projectName);
            return Ok(result);
        }

        [HttpGet("task/{taskId}")]
        public async Task<ActionResult<IEnumerable<CommentResponseDto>>> GetCommentsByTask(long taskId)
        {
            var result = await _commentService.GetCommentsByProjectTaskAsync(taskId);
            return Ok(result);
        }
    }
}
