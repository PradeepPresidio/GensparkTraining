using TeamColabApp.Models;
using TeamColabApp.Models.DTOs;

namespace TeamColabApp.Mappers
{
    public static class AppFileMapper
    {

        // Request DTO -> Entity
        public static AppFile RequestDtoToEntity(AppFileRequestDto dto,long userId,long projectId)
        {
            return new AppFile
            {
                FileName = dto.FileName,
                FileType = dto.FileType,
                // Content = dto.FileContent ?? Array.Empty<byte>(),
                UserId = userId,
                ProjectId = projectId,
                ProjectTaskId = dto.ProjectTaskId,
                UploadedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };
        }

        // Entity -> Response DTO
        public static AppFileResponseDto EntityToResponseDto(AppFile entity)
        {
            return new AppFileResponseDto
            {
                FileId = entity.FileId,
                FileName = entity.FileName,
                FileType = entity.FileType,
                Content = entity.Content,
                UploadedDate = entity.UploadedDate,
                UpdatedDate = entity.UpdatedDate,
                IsDeleted = entity.IsDeleted,
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
