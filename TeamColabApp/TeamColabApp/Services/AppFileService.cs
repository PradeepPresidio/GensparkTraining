using TeamColabApp.Models.DTOs;
using TeamColabApp.Mappers;
using TeamColabApp.Models;
using TeamColabApp.Interfaces;
using TeamColabApp.Repositories;
using TeamColabApp.Misc;
using TeamColabApp.Contexts;

namespace TeamColabApp.Services
{
    public class AppFileService : IAppFileService
    {
        private readonly IRepository<long,AppFile> _fileRepository;
        private readonly IRepository<long, User> _userRepository;
        private readonly IRepository<long, Project> _projectRepository;
        private readonly TeamColabAppContext _context;

        public AppFileService(IRepository<long,AppFile> fileRepository,IRepository<long,User>userRepository,IRepository<long,Project>projectRepository, TeamColabAppContext context)
        {
            _fileRepository = fileRepository;
            _userRepository = userRepository;
            _projectRepository = projectRepository;
            _context = context;
        }

        public async Task<AppFileResponseDto> UploadFileAsync(AppFileRequestDto fileRequest,byte[] fileContent)
        {
            long userId = EntityNameToId.GetUserId(fileRequest.Username, _context);
            long projectId = EntityNameToId.GetProjectId(fileRequest.ProjectTitle, _context);

            var entity = AppFileMapper.RequestDtoToEntity(fileRequest, userId, projectId);
            entity.Content = fileContent;
            var added = await _fileRepository.AddAsync(entity);
            return AppFileMapper.EntityToResponseDto(added);
        }

        public async Task<AppFileResponseDto> UpdateFileAsync(int fileId, AppFileRequestDto fileRequest,byte[] fileContent)
        {
            var existing = await _fileRepository.GetByIdAsync(fileId);
            if (existing == null || existing.IsDeleted)
                throw new KeyNotFoundException($"File with ID {fileId} not found.");

            long userId = EntityNameToId.GetUserId(fileRequest.Username, _context);
            long projectId = EntityNameToId.GetProjectId(fileRequest.ProjectTitle, _context);

            existing.FileName = fileRequest.FileName;
            existing.FileType = fileRequest.FileType;
            existing.Content = fileContent;
            existing.UserId = userId;
            existing.ProjectId = projectId;
            existing.ProjectTaskId = fileRequest.ProjectTaskId;
            existing.UpdatedDate = DateTime.UtcNow;

            var updated = await _fileRepository.UpdateAsync(fileId, existing);
            return AppFileMapper.EntityToResponseDto(updated);
}


        // public async Task<bool> DeleteFileAsync(int fileId)
        // {
        //     var existing = await _fileRepository.GetByIdAsync(fileId);
        //     if (existing == null || existing.IsDeleted)
        //         return false;
        //     existing.IsDeleted = true;
        //     existing.UpdatedDate = DateTime.UtcNow;
        //     await _fileRepository.UpdateAsync(fileId, existing);
        //     return true;
        // }
        public virtual async Task<bool> SoftDeleteFileAsync(long fileId)
        {
            AppFile? file = await _fileRepository.GetByIdAsync(fileId);
            if (file == null)
                return false;

            file.IsDeleted = true;
            // file.UploadedDate= DateTime.UtcNow;
            file.UpdatedDate= DateTime.UtcNow;

            await _fileRepository.UpdateAsync(fileId, file);
            return true;
        }

            public virtual async Task<bool> HardDeleteFileAsync(long fileId)
            {
                AppFile? file = await _fileRepository.GetByIdAsync(fileId);
                if (file == null)
                    return false;
                return await _fileRepository.DeleteAsync(fileId);
            }

            public virtual async Task<AppFileResponseDto> RetrieveSoftDeleteFileAsync(long fileId)
            {
                AppFile? file = await _fileRepository.GetByIdAsync(fileId);
                if (file == null)
                    throw new Exception("File not found.");

                file.IsDeleted = true;
                // file.UploadedDate= DateTime.UtcNow;
                file.UpdatedDate= DateTime.UtcNow;

                AppFile updated = await _fileRepository.UpdateAsync(fileId, file);
                return AppFileMapper.EntityToResponseDto(updated);
            }


        public async Task<IEnumerable<AppFileResponseDto>> GetFilesByUserNameAsync(string userName)
        {
            var userId = EntityNameToId.GetUserId(userName, _context);
            if (userId == 0) return [];

            var files = (await _fileRepository.GetAllAsync())
                .Where(f => f?.UserId == userId && !f.IsDeleted);

            return files
            .Where(f => f != null)
            .Select(f => AppFileMapper.EntityToResponseDto(f!));
        }

        public async Task<IEnumerable<AppFileResponseDto>> GetFilesByProjectNameAsync(string projectName)
        {
            var projectId = EntityNameToId.GetProjectId(projectName, _context);
            if (projectId == 0) return [];

            var files = (await _fileRepository.GetAllAsync())
                .Where(f => f != null && f.ProjectId == projectId && !f.IsDeleted);

            return files
            .Where(f => f != null)
            .Select(f=> AppFileMapper.EntityToResponseDto(f!));
        }

        public async Task<IEnumerable<AppFileResponseDto>> GetFilesByProjectTaskAsync(long projectTaskId)
        {
            
            var files = (await _fileRepository.GetAllAsync())
                .Where(f => f != null && f.ProjectTask != null && f.ProjectTask.ProjectTaskId == projectTaskId && !f.IsDeleted);

            return files
                .Where(f => f != null)
                .Select(f => AppFileMapper.EntityToResponseDto(f!));
        }

        public async Task<IEnumerable<AppFileResponseDto>> GetAllFilesAsync()
        {
            var files = (await _fileRepository.GetAllAsync())
                .Where(f => f != null && !f.IsDeleted);
            return files.Where(f => f != null)
                .Select(f => AppFileMapper.EntityToResponseDto(f!));
        }
    }
}
