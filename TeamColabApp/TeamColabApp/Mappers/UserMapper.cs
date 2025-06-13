using TeamColabApp.Models;
using TeamColabApp.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TeamColabApp.Mappers
{
    public static class UserMapper
    {
        // Request DTO -> Entity
        public static User RequestDtoToEntity(UserRequestDto dto)
        {
            return new User
            {
                Name = dto.Name,
                Password = null, //hash the password before saving
                Role = dto.Role,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }

        // 2. Entity -> Response DTO
        public static UserResponseDto EntityToResponseDto(User entity)
        {
            return new UserResponseDto
            {
                Id = entity.UserId,
                Name = entity.Name,
                Role = entity.Role,
                IsActive = entity.IsActive,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,

                Projects = entity.Projects?.Select(p => new ProjectResponseDto
                {
                    ProjectId = p.ProjectId,
                    Title = p.Title,
                    Description = p.Description,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    TeamLeaderId = p.TeamLeaderId,
                    Status = p.Status,
                    Priority = p.Priority,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    IsActive = p.IsActive,
                    TeamLeader = p.TeamLeader != null ? new UserResponseDto
                    {
                        Id = p.TeamLeader.UserId,
                        Name = p.TeamLeader.Name,
                        Role = p.TeamLeader.Role,
                        IsActive = p.TeamLeader.IsActive,
                        CreatedAt = p.TeamLeader.CreatedAt,
                        UpdatedAt = p.TeamLeader.UpdatedAt
                    } : null,
                    Users = [],
                    ProjectTasks = []
                }).ToList() ?? [],

                ProjectTasks = entity.ProjectTasks?.Select(pt => new ProjectTaskResponseDto
                {
                    ProjectTaskId = pt.ProjectTaskId,
                    Title = pt.Title,
                    Description = pt.Description,
                    StartDate = pt.StartDate,
                    EndDate = pt.EndDate,
                    Status = pt.Status,
                    Priority = pt.Priority,
                    IsActive = pt.IsActive,
                    CreatedAt = pt.CreatedAt,
                    UpdatedAt = pt.UpdatedAt,
                    AssignedUsers = pt.AssignedUsers?.Select(u => new UserResponseDto
                    {
                        Id = u.UserId,
                        Name = u.Name,
                        Role = u.Role,
                        IsActive = u.IsActive,
                        CreatedAt = u.CreatedAt,
                        UpdatedAt = u.UpdatedAt
                    }).ToList() ?? [],
                    Project = null,  
                    Comments = [],
                    Notifications = [],
                    ProjectTaskFiles = []
                }).ToList() ?? [],

                Comments = entity.Comments?.Select(c => CommentMapper.EntityToResponseDto(c)).ToList() ?? [],

                Notifications = entity.Notifications?.Select(n => NotificationMapper.EntityToResponseDto(n)).ToList() ?? [],

                UserFiles = entity.UserFiles?.Select(f => AppFileMapper.EntityToResponseDto(f)).ToList() ?? [],

                // ProjectTaskIds = entity.ProjectTasks?.Select(pt => pt.ProjectTaskId).ToList() ?? []
            };
        }
    }
}
