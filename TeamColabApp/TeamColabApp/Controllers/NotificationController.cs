using Microsoft.AspNetCore.Mvc;
using TeamColabApp.Interfaces;
using TeamColabApp.Models.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace TeamColabApp.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/notifications")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost]
        public async Task<ActionResult<NotificationResponseDto>> AddNotification([FromBody] NotificationRequestDto notificationRequest)
        {
            var result = await _notificationService.AddNotificationAsync(notificationRequest);
            return CreatedAtAction(nameof(GetAllNotifications), new { }, result);
        }

        [HttpPut("{notificationId:long}")]
        public async Task<ActionResult<NotificationResponseDto>> UpdateNotification(long notificationId, [FromBody] NotificationRequestDto notificationRequest)
        {
            var result = await _notificationService.UpdateNotificationAsync(notificationId, notificationRequest);
            return Ok(result);
        }

        [HttpDelete("{notificationId:long}")]
        public async Task<IActionResult> DeleteNotification(long notificationId)
        {
            bool success = await _notificationService.DeleteNotificationAsync(notificationId);
            if (!success)
                return NotFound($"Notification with ID {notificationId} not found.");
            return NoContent();
        }

        [HttpGet("user/{userName}")]
        public async Task<ActionResult<IEnumerable<NotificationResponseDto>>> GetNotificationsByUserName(string userName)
        {
            var notifications = await _notificationService.GetNotificationsByUserNameAsync(userName);
            return Ok(notifications);
        }

        [HttpGet("project/{projectName}")]
        public async Task<ActionResult<IEnumerable<NotificationResponseDto>>> GetNotificationsByProjectName(string projectName)
        {
            var notifications = await _notificationService.GetNotificationsByProjectNameAsync(projectName);
            return Ok(notifications);
        }

        [HttpGet("task/{projectTaskId:long}")]
        public async Task<ActionResult<IEnumerable<NotificationResponseDto>>> GetNotificationsByProjectTask(long projectTaskId)
        {
            var notifications = await _notificationService.GetNotificationsByProjectTaskAsync(projectTaskId);
            return Ok(notifications);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotificationResponseDto>>> GetAllNotifications()
        {
            var notifications = await _notificationService.GetAllNotificationsAsync();
            return Ok(notifications);
        }
    }
}
