using TeamColabApp.Models;
using TeamColabApp.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TeamColabApp.Mappers
{
    public static class ProjectTaskMapper
    {
        // Request DTO -> Entity
        public static ProjectTask RequestDtoToEntity(ProjectTaskRequestDto dto, List<User> assignedUsers, Project project)
        {
            return new ProjectTask
            {
                Title = dto.Title,
                Description = dto.Description,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                AssignedUsers = assignedUsers,
                Status = dto.Status,
                IsActive = dto.IsActive,
                Project = project,
                ProjectId = project.ProjectId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Priority = dto.Priority
            };
        }

        //  Entity -> Response DTO
        public static ProjectTaskResponseDto EntityToResponseDto(ProjectTask entity)
        {
            return new ProjectTaskResponseDto
            {
                ProjectTaskId = entity.ProjectTaskId,
                Title = entity.Title,
                Description = entity.Description,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                Status = entity.Status,
                Priority = entity.Priority,
                IsActive = entity.IsActive,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                AssignedUsers = entity.AssignedUsers?.Select(u => new UserResponseDto
                {
                    Id = u.UserId,
                    Name = u.Name,
                    Role = u.Role,
                    IsActive = u.IsActive,
                    CreatedAt = u.CreatedAt,
                    UpdatedAt = u.UpdatedAt,
                    Projects = [],
                    ProjectTasks = [],
                    Comments = [],
                    Notifications = [],
                    UserFiles = [],
                    // ProjectTaskIds = []
                }).ToList() ?? [],

                Project = entity.Project != null
                    ? new ProjectResponseDto
                    {
                        ProjectId = entity.Project.ProjectId,
                        Title = entity.Project.Title,
                        Description = entity.Project.Description,
                        StartDate = entity.Project.StartDate,
                        EndDate = entity.Project.EndDate,
                        TeamLeaderId = entity.Project.TeamLeaderId,
                        TeamLeader = entity.Project.TeamLeader != null
                            ? new UserResponseDto
                            {
                                Id = entity.Project.TeamLeader.UserId,
                                Name = entity.Project.TeamLeader.Name,
                                Role = entity.Project.TeamLeader.Role,
                                IsActive = entity.Project.TeamLeader.IsActive,
                                CreatedAt = entity.Project.TeamLeader.CreatedAt,
                                UpdatedAt = entity.Project.TeamLeader.UpdatedAt
                            }
                            : null,
                        Status = entity.Project.Status,
                        Priority = entity.Project.Priority,
                        CreatedAt = entity.Project.CreatedAt,
                        UpdatedAt = entity.Project.UpdatedAt,
                        IsActive = entity.Project.IsActive,
                        Users = [],
                        ProjectTasks = []
                    }
                    : null,

                Comments = entity.Comments?.Select(c => CommentMapper.EntityToResponseDto(c)).ToList() ?? [],

                Notifications = entity.Notifications?.Select(n => NotificationMapper.EntityToResponseDto(n)).ToList() ?? [],

                ProjectTaskFiles = entity.ProjectTaskFiles?.Select(f => AppFileMapper.EntityToResponseDto(f)).ToList() ?? []
            };
        }
    }
}
