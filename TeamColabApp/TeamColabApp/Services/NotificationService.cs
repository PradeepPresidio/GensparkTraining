using TeamColabApp.Models.DTOs;
using TeamColabApp.Mappers;
using TeamColabApp.Models;
using TeamColabApp.Interfaces;
using TeamColabApp.Repositories;
using TeamColabApp.Misc;
using TeamColabApp.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;


namespace TeamColabApp.Services
{
   
    public class NotificationService : INotificationService
{
    private readonly IRepository<long,Notification> _notificationRepository;
    private readonly TeamColabAppContext _context;
        private readonly IRepository<long, User> _userRepository; 
        private readonly IRepository<long, Project> _projectRepository; 
        private readonly IRepository<long, ProjectTask> _projectTaskRepository; 
    private readonly IHubContext<NotificationHub> _hubContext;

    public NotificationService(
        IRepository<long,Notification> notificationRepository,
        TeamColabAppContext context,
        IRepository<long, ProjectTask> projectTaskRepository,
        IRepository<long, User> userRepository,
        IRepository<long, Project> projectRepository,  
        IHubContext<NotificationHub> hubContext) 
    {
        _notificationRepository = notificationRepository;
        _userRepository = userRepository;
        _projectRepository = projectRepository;
        _projectTaskRepository = projectTaskRepository;
        _context = context;
        _hubContext = hubContext;
    }

        public async Task<NotificationResponseDto> AddNotificationAsync(NotificationRequestDto notificationRequest)
        {
            Notification entity = NotificationMapper.RequestDtoToEntity(notificationRequest);
            User? user = await _userRepository.GetByIdAsync(notificationRequest.UserId.Value);
            Project? project = await _projectRepository.GetByIdAsync(notificationRequest.ProjectId.Value);
            ProjectTask projectTask = await _projectTaskRepository.GetByIdAsync(notificationRequest.ProjectTaskId.Value);
            if (user!=null)
                entity.User = user;
            if (project != null)
                entity.Project = project;
            if (projectTask != null)
                entity.ProjectTask = projectTask;
            Notification added = await _notificationRepository.AddAsync(entity);
            NotificationResponseDto response = NotificationMapper.EntityToResponseDto(added);
             await _hubContext.Clients.User(entity.UserId.ToString())
                .SendAsync("ReceiveNotification", response.Message);
            return response;
        }

        public async Task<NotificationResponseDto> UpdateNotificationAsync(long notificationId, NotificationRequestDto notificationRequest)
        {
            Notification? existing = await _notificationRepository.GetByIdAsync(notificationId);
            if (existing == null)
                throw new KeyNotFoundException($"Notification with ID {notificationId} not found.");

            existing.Message = notificationRequest.Message;
            existing.Type = notificationRequest.Type;
            existing.UserId = notificationRequest.UserId;
            existing.ProjectId = notificationRequest.ProjectId;
            existing.ProjectTaskId = notificationRequest.ProjectTaskId;

            Notification updated = await _notificationRepository.UpdateAsync(notificationId, existing);
            NotificationResponseDto response = NotificationMapper.EntityToResponseDto(updated);
            return response;
        }

        public async Task<bool> DeleteNotificationAsync(long notificationId)
        {
            Notification? existing = await _notificationRepository.GetByIdAsync(notificationId);
            if (existing == null)
                return false;

            bool deleted = await _notificationRepository.DeleteAsync(notificationId);
            return deleted;
        }

        public async Task<IEnumerable<NotificationResponseDto>> GetNotificationsByUserNameAsync(string userName)
        {
            long userId = EntityNameToId.GetUserId(userName, _context);
            if (userId == 0)
                return Array.Empty<NotificationResponseDto>();

            IEnumerable<Notification?> notifications = await _notificationRepository.GetAllAsync();
            IEnumerable<Notification?> filtered = notifications.Where(n => n != null && n.UserId == userId);
            IEnumerable<NotificationResponseDto> result = filtered.Select(n => NotificationMapper.EntityToResponseDto(n!));
            return result;
        }

        public async Task<IEnumerable<NotificationResponseDto>> GetNotificationsByProjectNameAsync(string projectName)
        {
            long projectId = EntityNameToId.GetProjectId(projectName, _context);
            if (projectId == 0)
                return Array.Empty<NotificationResponseDto>();

            IEnumerable<Notification?> notifications = await _notificationRepository.GetAllAsync();
            IEnumerable<Notification?> filtered = notifications.Where(n => n != null && n.ProjectId == projectId);
            IEnumerable<NotificationResponseDto> result = filtered.Select(n => NotificationMapper.EntityToResponseDto(n!));
            return result;
        }

        public async Task<IEnumerable<NotificationResponseDto>> GetNotificationsByProjectTaskAsync(long projectTaskId)
        {
            if (projectTaskId == 0)
                return Array.Empty<NotificationResponseDto>();

            IEnumerable<Notification?> notifications = await _notificationRepository.GetAllAsync();
            IEnumerable<Notification?> filtered = notifications.Where(n => n != null && n.ProjectTask != null && n.ProjectTask.ProjectTaskId == projectTaskId);
            IEnumerable<NotificationResponseDto> result = filtered.Select(n => NotificationMapper.EntityToResponseDto(n!));
            return result;
        }

        public async Task<IEnumerable<NotificationResponseDto>> GetAllNotificationsAsync()
        {
            IEnumerable<Notification?> notifications = await _notificationRepository.GetAllAsync();
            IEnumerable<NotificationResponseDto> result = notifications.Where(n => n != null).Select(n => NotificationMapper.EntityToResponseDto(n!));
            return result;
        }
    }
}
