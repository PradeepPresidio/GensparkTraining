using TeamColabApp.Models;
using TeamColabApp.Models.DTOs;

namespace TeamColabApp.Mappers
{
    public static class NotificationMapper
    {

        //  Request DTO -> Entity
        public static Notification RequestDtoToEntity(NotificationRequestDto dto)
        {
            return new Notification
            {
                Message = dto.Message,
                Type = dto.Type,
                UserId = dto.UserId ?? 0, 
                ProjectId = dto.ProjectId,
                ProjectTaskId = dto.ProjectTaskId,
                CreatedAt = DateTime.UtcNow,
                User = null 
            };
        }

        //  Entity -> Response DTO
        public static NotificationResponseDto EntityToResponseDto(Notification entity)
        {
            return new NotificationResponseDto
            {
                NotificationId = entity.NotificationId,
                Message = entity.Message,
                Type = entity.Type,
                CreatedAt = entity.CreatedAt,

                User = entity.User != null
                    ? new UserResponseDto
                    {
                        Id = entity.User.UserId,
                        Name = entity.User.Name,
                        Role = entity.User.Role,
                        IsActive = entity.User.IsActive,
                        CreatedAt = entity.User.CreatedAt,
                        UpdatedAt = entity.User.UpdatedAt,
                        Projects = [],
                        ProjectTasks = [],
                        Comments = [],
                        Notifications = [],
                        UserFiles = [],
                        // ProjectTaskIds = []
                    }
                    : null,

                Project = entity.Project != null
                    ? new ProjectResponseDto
                    {
                        ProjectId = entity.Project.ProjectId,
                        Title = entity.Project.Title,
                        Description = entity.Project.Description,
                        StartDate = entity.Project.StartDate,
                        EndDate = entity.Project.EndDate,
                        TeamLeaderId = entity.Project.TeamLeaderId,
                        IsActive = entity.Project.IsActive,
                        Users = [],
                        ProjectTasks = []
                    }
                    : null,

                ProjectTask = entity.ProjectTask != null
                    ? new ProjectTaskResponseDto
                    {
                        ProjectTaskId = entity.ProjectTask.ProjectTaskId,
                        Title = entity.ProjectTask.Title,
                        Description = entity.ProjectTask.Description,
                        Status = entity.ProjectTask.Status,
                        Priority = entity.ProjectTask.Priority,
                        StartDate = entity.ProjectTask.StartDate,
                        EndDate = entity.ProjectTask.EndDate,
                        CreatedAt = entity.ProjectTask.CreatedAt,
                        UpdatedAt = entity.ProjectTask.UpdatedAt,
                        IsActive = entity.ProjectTask.IsActive,
                        AssignedUsers = [],
                        Comments = [],
                        Notifications = [],
                        ProjectTaskFiles = []
                    }
                    : null
            };
        }
    }
}
