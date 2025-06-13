using System.Threading.Tasks;
using TeamColabApp.Models.DTOs;

namespace TeamColabApp.Interfaces
{
    public interface IAuthenticationService
    {
        Task<UserLoginResponseDto> Login(UserLoginRequestDto loginRequest);
        Task<bool> Logout(string username);
        Task<UserLoginResponseDto?> RefreshToken(string refreshToken, string username);
    }
}
