using TeamColabApp.Models.DTOs;
namespace TeamColabApp.Interfaces
{
    public interface IAppFileService
    {
        public virtual Task<AppFileResponseDto> UploadFileAsync(AppFileRequestDto fileRequest,byte[] fileContent)
        {
            throw new NotImplementedException();
        }
        public virtual Task<AppFileResponseDto> UpdateFileAsync(int fileId, AppFileRequestDto fileRequest,byte[] fileContent)
        {
            throw new NotImplementedException();
        }
        public virtual Task<bool> SoftDeleteFileAsync(long FileId)
        {
            throw new NotImplementedException();
        }
        public virtual Task<bool> HardDeleteFileAsync(long FileId)
        {
            throw new NotImplementedException();
        }
        public virtual Task<AppFileResponseDto> RetrieveSoftDeleteFileAsync(long FileId)
        {
            throw new NotImplementedException();
        }
        public virtual Task<IEnumerable<AppFileResponseDto>> GetFilesByUserNameAsync(string userName)
        {
            throw new NotImplementedException();
        }
        public virtual Task<IEnumerable<AppFileResponseDto>> GetFilesByProjectNameAsync(string projectName)
        {
            throw new NotImplementedException();
        }
        public virtual Task<IEnumerable<AppFileResponseDto>> GetFilesByProjectTaskAsync(long projectTaskId)
        {
            throw new NotImplementedException();
        }
        public virtual Task<IEnumerable<AppFileResponseDto>> GetAllFilesAsync()
        {
            throw new NotImplementedException();
        }
    }
}