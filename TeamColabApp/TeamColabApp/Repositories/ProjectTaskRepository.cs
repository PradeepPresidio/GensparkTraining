using TeamColabApp.Models;
using TeamColabApp.Models.DTOs;
using TeamColabApp.Contexts;
using TeamColabApp.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TeamColabApp.Repositories
{
    public class ProjectTaskRepository : Repository<long, ProjectTask>
    {
        public ProjectTaskRepository(TeamColabAppContext context) : base(context)
        {
        }

        public override async Task<ProjectTask?> GetByIdAsync(long id)
        {
            return await _context.ProjectTasks
                .Include(t => t.AssignedUsers)
                .Include(t => t.Comments)
                .Include(t => t.Notifications)
                .Include(t => t.ProjectTaskFiles)
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.ProjectTaskId == id);
        }

        public override async Task<IEnumerable<ProjectTask?>> GetAllAsync()
        {
            return await _context.ProjectTasks
                .Include(t => t.AssignedUsers)
                .Include(t => t.Comments)
                .Include(t => t.Notifications)
                .Include(t => t.ProjectTaskFiles)
                .Include(t => t.Project)
                .ToListAsync();
        }
    }
}
