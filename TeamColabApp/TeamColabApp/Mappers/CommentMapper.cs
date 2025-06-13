using TeamColabApp.Models;
using TeamColabApp.Models.DTOs;

namespace TeamColabApp.Mappers
{
    public static class CommentMapper
    {

        //  Request DTO -> Entity
        public static Comment RequestDtoToEntity(CommentRequestDto dto)
        {
            return new Comment
            {
                Content = dto.Content,
                // UserId = dto.UserId,
                // ProjectId = dto.ProjectId,
                ProjectTaskId = dto.ProjectTaskId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                User = null,
                Project = null,
                ProjectTask = null
            };
        }

        //  Entity -> Response DTO
        public static CommentResponseDto EntityToResponseDto(Comment entity)
        {
            return new CommentResponseDto
            {
                CommentId = entity.CommentId,
                Content = entity.Content,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                ProjectId = entity.ProjectId ?? 0,
                ProjectTaskId = entity.ProjectTaskId ?? 0,
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
                        TeamLeaderId= entity.Project.TeamLeaderId,
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
