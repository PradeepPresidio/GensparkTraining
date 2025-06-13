using TeamColabApp.Models;
using TeamColabApp.Models.DTOs;
namespace TeamColabApp.Interfaces
{
    public class IUserService
    {
        public virtual Task<UserResponseDto> GetUserByName(string Name)
        {
            throw new NotImplementedException();
        }
        public virtual Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        public virtual Task<UserResponseDto> AddUserAsync(UserRequestDto userRequestDto)
        {
            throw new NotImplementedException();
        }

        public virtual Task<UserResponseDto> UpdateUserAsync(UserRequestDto userRequestDto, string name)
        {
            throw new NotImplementedException();
        }
        public virtual Task<bool> SoftDeleteUserAsync(string name)
        {
            throw new NotImplementedException();
        }
        public virtual Task<bool> HardDeleteUserAsync(string name)
        {
            throw new NotImplementedException();
        }
        public virtual Task<UserResponseDto> RetrieveSoftDeleteUserAsync(string name)
        {
            throw new NotImplementedException();
        }
        public virtual Task<IEnumerable<UserResponseDto>> GetUsersByRoleAsync(string role)
        {
            throw new NotImplementedException();
        }
        public virtual Task<IEnumerable<UserResponseDto>> GetUsersUnderProjectAsync(string name)
        {
            throw new NotImplementedException();
        }
    }
}