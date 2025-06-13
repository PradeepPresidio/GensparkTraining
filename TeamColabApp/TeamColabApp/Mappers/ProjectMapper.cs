using TeamColabApp.Models;
using TeamColabApp.Models.DTOs;
using TeamColabApp.Misc;
namespace TeamColabApp.Mappers
{
    public static class ProjectMapper
    {
        //  Request DTO -> Entity
        public static Project RequestDtoToEntity(ProjectRequestDto dto)
        {
            // long teamLeaderId = Misc.EntityNameToId.GetUserId(dto.TeamLeaderName);
            return new Project
            {
                Title = dto.Title,
                Description = dto.Description,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                // TeamLeader = dto.TeamLeader,
                // TeamLeader = null, 
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true,
                TeamMembers = []
            };
        }

        // Entity -> Response DTO
        public static ProjectResponseDto EntityToResponseDto(Project entity)
    {
    return new ProjectResponseDto
    {
        ProjectId = entity.ProjectId,
        Title = entity.Title,
        Description = entity.Description,
        StartDate = entity.StartDate,
        EndDate = entity.EndDate,
        TeamLeaderId = entity.TeamLeaderId,
        TeamLeader = entity.TeamLeader != null
            ? new UserResponseDto
            {
                Id = entity.TeamLeader.UserId,
                Name = entity.TeamLeader.Name,
                Role = entity.TeamLeader.Role,
                IsActive = entity.TeamLeader.IsActive,
                CreatedAt = entity.TeamLeader.CreatedAt,
                UpdatedAt = entity.TeamLeader.UpdatedAt,
                Projects = [],
                ProjectTasks = [],
                Comments = [],
                Notifications = [],
                UserFiles = [],
                // ProjectTaskIds = []
            }
            : null,
        Status = entity.Status,
        Priority = entity.Priority,
        CreatedAt = entity.CreatedAt,
        UpdatedAt = entity.UpdatedAt,
        IsActive = entity.IsActive,
        Users = entity.TeamMembers?.Select(user => new UserResponseDto
        {
            Id = user.UserId,
            Name = user.Name,
            Role = user.Role,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            Projects = [],
            ProjectTasks = [],
            Comments = [],
            Notifications = [],
            UserFiles = [],
            // ProjectTaskIds = []
        }).ToList() ?? [],
        ProjectTasks = entity.ProjectTasks?.Select(task => new ProjectTaskResponseDto
        {
            ProjectTaskId = task.ProjectTaskId,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            Priority = task.Priority,
            StartDate = task.StartDate,
            EndDate = task.EndDate,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt,
            IsActive = task.IsActive,
            AssignedUsers = [],
            Comments = [],
            Notifications = [],
            ProjectTaskFiles = []
        }).ToList() ?? []
    };
}
    }
}
