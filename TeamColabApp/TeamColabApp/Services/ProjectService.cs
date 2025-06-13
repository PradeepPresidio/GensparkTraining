using TeamColabApp.Models;
using TeamColabApp.Models.DTOs;
using TeamColabApp.Interfaces;
using TeamColabApp.Mappers;
using Microsoft.EntityFrameworkCore;
using TeamColabApp.Misc;
using TeamColabApp.Contexts;

namespace TeamColabApp.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IRepository<long, Project> _projectRepository;
        private readonly IRepository<long, User> _userRepository;
        private readonly TeamColabAppContext _context;

        public ProjectService(IRepository<long, Project> projectRepository, IRepository<long, User> userRepository, TeamColabAppContext teamColabAppContext)
        {
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _context = teamColabAppContext;
        }

        public async Task<ProjectResponseDto> AddProjectAsync(ProjectRequestDto projectRequestDto)
        {
            IEnumerable<Project?> projects = await _projectRepository.GetAllAsync();
            Project? existingProject = projects.FirstOrDefault(p =>p.Title == projectRequestDto.Title);
            if (existingProject != null)
            {
                throw new Exception("Duplicate project title error");
            }
            Project project = ProjectMapper.RequestDtoToEntity(projectRequestDto);
            long teamLeaderId = EntityNameToId.GetUserId(projectRequestDto.TeamLeaderName, _context);
            project.TeamLeaderId = teamLeaderId;
            List<User> teamMembers = new List<User>();
            foreach (string name in projectRequestDto.TeamMembersName)
            {
                User? user = await _userRepository.GetAllAsync()
                    .ContinueWith(t => t.Result.FirstOrDefault(u => u.Name == name));

                if (user != null)
                {
                    if (user.Role != "member")
                        throw new Exception("Only Users with role member can be a part of the team");
                    teamMembers.Add(user);
                }
            }

            project.TeamMembers = teamMembers;

            Project addedProject = await _projectRepository.AddAsync(project);
            return ProjectMapper.EntityToResponseDto(addedProject);
        }

        public async Task<bool> SoftDeleteProjectAsync(string title)
        {
            long projectId = EntityNameToId.GetProjectId(title, _context);
            Project? project = await _projectRepository.GetByIdAsync(projectId);
            if (project == null)
                return false;

            project.IsActive = false;
            project.UpdatedAt = DateTime.UtcNow;

            await _projectRepository.UpdateAsync(projectId,project);
            return true;
        }

        public async Task<bool> HardDeleteProjectAsync(string title)
        {

             long projectId = EntityNameToId.GetProjectId(title, _context);
            Project? project = await _projectRepository.GetByIdAsync(projectId);
            if (project == null)
                return false;

            return await _projectRepository.DeleteAsync(projectId);
        }

        public async Task<ProjectResponseDto> RetrieveSoftDeleteProjectAsync(string title)
        {
             long projectId = EntityNameToId.GetProjectId(title, _context);
            Project? project = await _projectRepository.GetByIdAsync(projectId);
            if (project == null)
                throw new Exception("Project not found.");

            project.IsActive = true;
            project.UpdatedAt = DateTime.UtcNow;

            Project updatedProject = await _projectRepository.UpdateAsync(projectId,project);
            return ProjectMapper.EntityToResponseDto(updatedProject);
        }

        public async Task<IEnumerable<ProjectResponseDto>> GetAllProjectsAsync()
        {
            IEnumerable<Project?> projects = await _projectRepository.GetAllAsync();

            return projects
                .Where(p => p != null && p.IsActive==true)
                .Select(p => ProjectMapper.EntityToResponseDto(p!))
                .ToList();
        }

        public async Task<ProjectResponseDto> GetProjectByTitleAsync(string title)
        {
            IEnumerable<Project?> projects = await _projectRepository.GetAllAsync();
            Project? project = projects.FirstOrDefault(p => p != null&& p.IsActive==true && p.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

            if (project == null)
            {
                throw new KeyNotFoundException("Project not found.");
            }

            return ProjectMapper.EntityToResponseDto(project);
        }

        public async Task<IEnumerable<ProjectResponseDto>> GetProjectsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            IEnumerable<Project?> projects = await _projectRepository.GetAllAsync();
            return projects
                .Where(p => p != null && p.IsActive==true && p.StartDate >= startDate && p.EndDate <= endDate)
                .Select(p => ProjectMapper.EntityToResponseDto(p!))
                .ToList();
        }

        public async Task<IEnumerable<ProjectResponseDto>> GetProjectsByPriorityAsync(string priority)
        {
            IEnumerable<Project?> projects = await _projectRepository.GetAllAsync();
            return projects
                .Where(p => p != null && p.IsActive==true && p.Priority.Equals(priority, StringComparison.OrdinalIgnoreCase))
                .Select(p => ProjectMapper.EntityToResponseDto(p!))
                .ToList();
        }

        public async Task<IEnumerable<ProjectResponseDto>> GetProjectsByStatusAsync(string status)
        {
            IEnumerable<Project?> projects = await _projectRepository.GetAllAsync();
            return projects
                .Where(p => p != null && p.IsActive==true && p.Status.Equals(status, StringComparison.OrdinalIgnoreCase))
                .Select(p => ProjectMapper.EntityToResponseDto(p!))
                .ToList();
        }

        public async Task<IEnumerable<ProjectResponseDto>> GetProjectsByTeamLeaderNameAsync(string name)
        {
            IEnumerable<Project?> projects = await _projectRepository.GetAllAsync();
            return projects
                .Where(p => p != null && p.IsActive==true&& p.TeamLeader != null && p.TeamLeader.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                .Select(p => ProjectMapper.EntityToResponseDto(p!))
                .ToList();
        }

        public async Task<IEnumerable<ProjectResponseDto>> GetProjectsByTeamMemberNameAsync(string teamMemberName)
        {
            IEnumerable<Project?> projects = await _projectRepository.GetAllAsync();
            return [.. projects
                .Where(p => p != null && p.IsActive==true && p.TeamMembers != null && p.TeamMembers.Any(u => u.Name.Equals(teamMemberName, StringComparison.OrdinalIgnoreCase)))
                .Select(p => ProjectMapper.EntityToResponseDto(p!))];
        }

        public async Task<IEnumerable<ProjectResponseDto>> GetProjectsByUserNameAsync(string name)
        {
            IEnumerable<Project?> projects = await _projectRepository.GetAllAsync();
            return [.. projects
                .Where(p =>
                    p != null &&
                    (
                        (p.TeamLeader != null && p.IsActive==true && p.TeamLeader.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) ||
                        (p.TeamMembers != null && p.IsActive==true && p.TeamMembers.Any(u => u.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                    )
                )
                .Select(p => ProjectMapper.EntityToResponseDto(p!))];
        }

        public async Task<ProjectResponseDto> UpdateProjectAsync(ProjectRequestDto projectRequestDto, string title)
        {
            IEnumerable<Project?> projects = await _projectRepository.GetAllAsync();
            Project? existingProject = projects.FirstOrDefault(p => p!=null && p.IsActive==true && p.Title == projectRequestDto.Title);
            if (existingProject != null)
                throw new Exception("Duplicate Project Title error");
            long id = EntityNameToId.GetProjectId(title,_context);
            existingProject = await _projectRepository.GetByIdAsync(id);
            if (existingProject == null)
            {
                throw new KeyNotFoundException("Project not found.");
            }
            long teamLeaderId = EntityNameToId.GetUserId(projectRequestDto.TeamLeaderName,_context);
            existingProject.Title = projectRequestDto.Title;
            existingProject.Description = projectRequestDto.Description;
            existingProject.StartDate = projectRequestDto.StartDate;
            existingProject.EndDate = projectRequestDto.EndDate;
            existingProject.TeamLeaderId = teamLeaderId;
            existingProject.UpdatedAt = DateTime.UtcNow;

            List<User> teamMembers = [];
            foreach (string name in projectRequestDto.TeamMembersName)
            {
                User? user = await _userRepository.GetAllAsync()
                    .ContinueWith(t => t.Result.FirstOrDefault(u => u!= null && u.Name == name));

                if (user != null)
                {
                    teamMembers.Add(user);
                }
            }

            existingProject.TeamMembers = teamMembers;

            Project updatedProject = await _projectRepository.UpdateAsync(id,existingProject);
            return ProjectMapper.EntityToResponseDto(updatedProject);
        }
    }
}
