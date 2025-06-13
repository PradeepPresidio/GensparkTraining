using TeamColabApp.Models;
using TeamColabApp.Models.DTOs;
using TeamColabApp.Contexts;
using TeamColabApp.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace TeamColabApp.Repositories
{
    public class UserTokenRepository : Repository<long, UserToken>
    {
        public UserTokenRepository(TeamColabAppContext context) : base(context)
        {
        }

        public override async Task<UserToken?> GetByIdAsync(long id)
        {
            return await _context.UserTokens.FirstOrDefaultAsync(ut => ut.TokenId == id);
        }

        public override async Task<IEnumerable<UserToken?>> GetAllAsync()
        {
            return await _context.UserTokens.ToListAsync();
        }
    }
}