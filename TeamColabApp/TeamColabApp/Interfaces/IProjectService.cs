using TeamColabApp.Models;
using TeamColabApp.Models.DTOs;
namespace TeamColabApp.Interfaces
{
    public interface IProjectService
    {
        public virtual Task<ProjectResponseDto> GetProjectByTitleAsync(string title) {
            throw new NotImplementedException();
        }
        public virtual Task<IEnumerable<ProjectResponseDto>> GetAllProjectsAsync() {
            throw new NotImplementedException();
        }
        public virtual Task<ProjectResponseDto> AddProjectAsync(ProjectRequestDto projectRequestDto)
        {
            throw new NotImplementedException();
        }
        public virtual Task<ProjectResponseDto> UpdateProjectAsync(ProjectRequestDto projectRequestDto, string projectId)
        {
            throw new NotImplementedException();
        }
        public virtual Task<bool> SoftDeleteProjectAsync(string title)
        {
            throw new NotImplementedException();
        }
        public virtual Task<bool> HardDeleteProjectAsync(string title)
        {
            throw new NotImplementedException();
        }
        public virtual Task<ProjectResponseDto> RetrieveSoftDeleteProjectAsync(string title)
        {
            throw new NotImplementedException();
        }
        public virtual Task<IEnumerable<ProjectResponseDto>> GetProjectsByUserNameAsync(string Name)
        {
            throw new NotImplementedException();
        }
        public virtual Task<IEnumerable<ProjectResponseDto>> GetProjectsByStatusAsync(string status)
        {
            throw new NotImplementedException();
        }
        public virtual Task<IEnumerable<ProjectResponseDto>> GetProjectsByPriorityAsync(string priority)
        {
            throw new NotImplementedException();
        }
        public virtual Task<IEnumerable<ProjectResponseDto>> GetProjectsByTeamLeaderNameAsync(string Name)
        {
            throw new NotImplementedException();
        }
        public virtual Task<IEnumerable<ProjectResponseDto>> GetProjectsByTeamMemberNameAsync(string teamMemberName)
        {
            throw new NotImplementedException();
        }
        public virtual Task<IEnumerable<ProjectResponseDto>> GetProjectsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

    }
}