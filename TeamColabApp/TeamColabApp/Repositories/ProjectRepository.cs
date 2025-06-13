using TeamColabApp.Models;
using TeamColabApp.Models.DTOs;
using TeamColabApp.Contexts;
using TeamColabApp.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace TeamColabApp.Repositories
{
    public class ProjectRepository : Repository<long, Project>
    {
        public ProjectRepository(TeamColabAppContext context) : base(context)
        {
        }

        public override async Task<Project?> GetByIdAsync(long id)
        {
            return await _context.Projects
                .Include(p => p.TeamLeader)
                .Include(p => p.TeamMembers)
                .Include(p => p.ProjectTasks)
                .Include(p => p.Comments)
                .Include(p => p.Notifications)
                .Include(p => p.ProjectFiles)
                .FirstOrDefaultAsync(p => p.ProjectId == id);
        }

        public override async Task<IEnumerable<Project?>> GetAllAsync()
        {
            return await _context.Projects
                .Include(p => p.TeamLeader)
                .Include(p => p.TeamMembers)
                .Include(p => p.ProjectTasks)
                .Include(p => p.Comments)
                .Include(p => p.Notifications)
                .Include(p => p.ProjectFiles)
                .ToListAsync();
        }
    }
}