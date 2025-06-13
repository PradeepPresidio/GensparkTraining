using TeamColabApp.Models;
using TeamColabApp.Models.DTOs;
namespace TeamColabApp.Interfaces
{
    public interface IProjectTaskService
    {
        public virtual Task<ProjectTaskResponseDto> GetProjectTaskAsync(string projectName, string projectTaskName)
        {
            throw new NotImplementedException();
        }
        public virtual Task<IEnumerable<ProjectTaskResponseDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
        public virtual Task<ProjectTaskResponseDto> AddAsync(ProjectTaskRequestDto projectTaskRequestDto)
        {
            throw new NotImplementedException();
        }
        public virtual Task<ProjectTaskResponseDto> UpdateAsync(long id, ProjectTaskRequestDto projectTaskRequestDto)
        {
            throw new NotImplementedException();
        }
        public virtual Task<bool> SoftDeleteAsync(long id)
        {
            throw new NotImplementedException();
        }
        public virtual Task<bool> HardDeleteAsync(long id)
        {
            throw new NotImplementedException();
        }
        public virtual Task<ProjectTaskResponseDto> RetrieveSoftDeleteAsync(long id)
        {
            throw new NotImplementedException();
        }
        public virtual Task<IEnumerable<ProjectTaskResponseDto>> GetProjectTasksByProjectNameAsync(string projectName)
        {
            throw new NotImplementedException();
        }
        public virtual Task<IEnumerable<ProjectTaskResponseDto>> GetProjectTasksByUserNameAsync(string projectName,string userName)
        {
            throw new NotImplementedException();
        }
        public virtual Task<IEnumerable<ProjectTaskResponseDto>> GetProjectTasksByStatusAsync(string projectName, string status)
        {
            throw new NotImplementedException();
        }
        public virtual Task<IEnumerable<ProjectTaskResponseDto>> GetProjectTasksByPriorityAsync(string projectName, string priority)
        {
            throw new NotImplementedException();
        }
        public virtual Task<IEnumerable<ProjectTaskResponseDto>> GetProjectTasksByDateRangeAsync(string projectName, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

    }
}