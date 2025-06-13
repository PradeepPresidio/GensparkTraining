using TeamColabApp.Models;

namespace TeamColabApp.Interfaces
{
    public interface ITokenService
    {
        public Task<string> GenerateToken(User user);
        public string GenerateRefreshToken();
    }
}