using TeamColabApp.Interfaces;
using TeamColabApp.Models;
using TeamColabApp.Models.DTOs;
using TeamColabApp.Mappers;
using TeamColabApp.Repositories;
using Microsoft.EntityFrameworkCore;
using TeamColabApp.Contexts;
using TeamColabApp.Misc;

namespace TeamColabApp.Services
{
    public class ProjectTaskService : IProjectTaskService
    {
        private readonly IRepository<long,ProjectTask> _projectTaskRepository;
        private readonly IRepository<long,Project> _projectRepository;
        private readonly IRepository<long,User> _userRepository;
        private readonly TeamColabAppContext _context;

        public ProjectTaskService(IRepository<long,ProjectTask>  projectTaskRepo,IRepository<long,Project> projectRepo,IRepository<long,User> userRepo, TeamColabAppContext context)
        {
            _projectTaskRepository = projectTaskRepo;
            _projectRepository = projectRepo;
            _userRepository = userRepo;
            _context = context;
        }

        public async Task<ProjectTaskResponseDto> AddAsync(ProjectTaskRequestDto dto)
        {
            if (string.IsNullOrEmpty(dto.ProjectTitle))
            {
                throw new Exception("Project title is required.");
            }

            Project? project = await _projectRepository
                .GetAllAsync()
                .ContinueWith(t => t.Result.FirstOrDefault(p => p.Title == dto.ProjectTitle));

            if (project == null)
            {
                throw new Exception("Project not found.");
            }

            List<User> assignedUsers = new List<User>();
            if (dto.AssignedUserNames != null && dto.AssignedUserNames.Length > 0)
            {
                IEnumerable<User> users = await _userRepository.GetAllAsync();
                assignedUsers = users
                    .Where(u => dto.AssignedUserNames.Contains(u.Name))
                    .ToList();

                if (assignedUsers.Count != dto.AssignedUserNames.Length)
                {
                    throw new Exception("Some assigned users not found.");
                }
            }

            ProjectTask task = ProjectTaskMapper.RequestDtoToEntity(dto, assignedUsers, project);

            ProjectTask? createdTask = await _projectTaskRepository.AddAsync(task);
            return ProjectTaskMapper.EntityToResponseDto(createdTask);
        }


        public async Task<bool> SoftDeleteAsync(long id)
{
            long taskId = id;
        ProjectTask? task = await _projectTaskRepository.GetByIdAsync(taskId);
        if (task == null)
            return false;

        task.IsActive = false;
        task.UpdatedAt = DateTime.UtcNow;

        await _projectTaskRepository.UpdateAsync(taskId, task);
        return true;
}

        public async Task<bool> HardDeleteAsync(long id)
        {
            long taskId = id;
            ProjectTask? task = await _projectTaskRepository.GetByIdAsync(taskId);
            if (task == null)
                return false;

            return await _projectTaskRepository.DeleteAsync(taskId);
        }

        public async Task<ProjectTaskResponseDto> RetrieveSoftDeleteAsync(long id)
        {
            long taskId = id;
            ProjectTask? task = await _projectTaskRepository.GetByIdAsync(taskId);
            if (task == null)
                throw new Exception("Task not found.");

            task.IsActive = true;
            task.UpdatedAt = DateTime.UtcNow;

            ProjectTask updatedTask = await _projectTaskRepository.UpdateAsync(taskId, task);
            return ProjectTaskMapper.EntityToResponseDto(updatedTask);
        }

        public async Task<IEnumerable<ProjectTaskResponseDto>> GetAllAsync()
        {
            IEnumerable<ProjectTask?> tasks = await _projectTaskRepository.GetAllAsync();
            return tasks
                .Where(t => t != null && t.IsActive!=false)
                .Select(t => ProjectTaskMapper.EntityToResponseDto(t!));
        }

        public async Task<ProjectTaskResponseDto> GetProjectTaskAsync(string projectName, string taskName)
        {
            IEnumerable<ProjectTask?> tasks = await _projectTaskRepository.GetAllAsync();
            ProjectTask? task = tasks
                .FirstOrDefault(t => t != null && t.Project.Title == projectName && t.Title == taskName && t.IsActive!=false);
            if (task == null) throw new KeyNotFoundException("Project task not found.");
            return ProjectTaskMapper.EntityToResponseDto(task);
        }

        public async Task<IEnumerable<ProjectTaskResponseDto>> GetProjectTasksByProjectNameAsync(string projectName)
        {
            IEnumerable<ProjectTask?> tasks = await _projectTaskRepository.GetAllAsync();
            return tasks
                .Where(t => t != null && t.Project.Title == projectName && t.IsActive!=false)
                .Select(t => ProjectTaskMapper.EntityToResponseDto(t!));
        }

        public async Task<IEnumerable<ProjectTaskResponseDto>> GetProjectTasksByUserNameAsync(string projectName, string userName)
        {
            IEnumerable<ProjectTask?> tasks = await _projectTaskRepository.GetAllAsync();
            return tasks
                .Where(t => t != null && t.Project.Title == projectName && t.IsActive!=false &&
                            t.AssignedUsers != null && t.AssignedUsers.Any(u => u.Name == userName))
                .Select(t => ProjectTaskMapper.EntityToResponseDto(t!));
        }

        public async Task<IEnumerable<ProjectTaskResponseDto>> GetProjectTasksByStatusAsync(string projectName, string status)
        {
            IEnumerable<ProjectTask?> tasks = await _projectTaskRepository.GetAllAsync();
            return tasks
                .Where(t => t != null && t.IsActive!=false && t.Project.Title == projectName && t.Status == status)
                .Select(t => ProjectTaskMapper.EntityToResponseDto(t!));
        }

        public async Task<IEnumerable<ProjectTaskResponseDto>> GetProjectTasksByPriorityAsync(string projectName, string priority)
        {
            IEnumerable<ProjectTask?> tasks = await _projectTaskRepository.GetAllAsync();
            return tasks
                .Where(t => t != null && t.IsActive!=false && t.Project.Title == projectName && t.Priority == priority)
                .Select(t => ProjectTaskMapper.EntityToResponseDto(t!));
        }

        public async Task<IEnumerable<ProjectTaskResponseDto>> GetProjectTasksByDateRangeAsync(string projectName, DateTime startDate, DateTime endDate)
        {
            IEnumerable<ProjectTask?> tasks = await _projectTaskRepository.GetAllAsync();
            return tasks
                .Where(t => t != null && t.IsActive!=false && t.Project.Title == projectName &&
                            t.StartDate >= startDate && t.EndDate <= endDate)
                .Select(t => ProjectTaskMapper.EntityToResponseDto(t!));
        }

        public async Task<ProjectTaskResponseDto> UpdateAsync(long id, ProjectTaskRequestDto dto)
        {
            ProjectTask? task = await _projectTaskRepository.GetByIdAsync(id);
            if (task == null) throw new KeyNotFoundException("Task not found.");

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.StartDate = dto.StartDate;
            task.EndDate = dto.EndDate;
            task.Status = dto.Status;
            task.IsActive = dto.IsActive;
            task.UpdatedAt = DateTime.UtcNow;

            if (!string.IsNullOrEmpty(dto.ProjectTitle))
            {
                Project? project = await _projectRepository
                    .GetAllAsync()
                    .ContinueWith(t => t.Result.FirstOrDefault(p => p.Title == dto.ProjectTitle));
                if (project != null)
                {
                    task.Project = project;
                }
            }

            if (dto.AssignedUserNames != null && dto.AssignedUserNames.Length > 0)
            {
                IEnumerable<User> users = await _userRepository.GetAllAsync();
                List<User> assignedUsers = users
                    .Where(u => dto.AssignedUserNames.Contains(u.Name))
                    .ToList();

                if (assignedUsers.Count != dto.AssignedUserNames.Length)
                {
                    throw new Exception("Some assigned users not found.");
                }

                task.AssignedUsers = assignedUsers;
            }

            ProjectTask? updatedTask = await _projectTaskRepository.UpdateAsync(id, task);
            return ProjectTaskMapper.EntityToResponseDto(updatedTask);
        }

    }
}
