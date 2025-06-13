using TeamColabApp.Models;
using TeamColabApp.Models.DTOs;
using TeamColabApp.Contexts;
using TeamColabApp.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace TeamColabApp.Repositories
{
    public class UserRepository : Repository<long, User>
    {
        public UserRepository(TeamColabAppContext context) : base(context)
        {
        }

        public override async Task<User?> GetByIdAsync(long id)
        {
            return await _context.Users
                .Include(u => u.Projects)
                .Include(u => u.ProjectTasks)
                .Include(u => u.Comments)
                .Include(u => u.Notifications)
                .Include(u => u.UserFiles)
                .FirstOrDefaultAsync(u => u.UserId == id);
        }
        public override async Task<IEnumerable<User?>> GetAllAsync()
        {
            return await _context.Users
                .Include(u => u.Projects)
                .Include(u => u.ProjectTasks)
                .Include(u => u.Comments)
                .Include(u => u.Notifications)
                .Include(u => u.UserFiles)
                .ToListAsync();
        }
    }
}