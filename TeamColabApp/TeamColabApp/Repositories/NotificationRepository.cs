using TeamColabApp.Models;
using TeamColabApp.Models.DTOs;
using TeamColabApp.Contexts;
using TeamColabApp.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace TeamColabApp.Repositories
{ 
    public class NotificationRepository : Repository<long, Notification>
    {
        public NotificationRepository(TeamColabAppContext context) : base(context)
        {
        }

        public override async Task<Notification?> GetByIdAsync(long id)
        {
            return await _context.Notifications
                .Include(n => n.User)
                .Include(n => n.Project)
                .Include(n => n.ProjectTask)
                .FirstOrDefaultAsync(n => n.NotificationId == id);
        }

        public override async Task<IEnumerable<Notification?>> GetAllAsync()
        {
            return await _context.Notifications
                .Include(n => n.User)
                .Include(n => n.Project)
                .Include(n => n.ProjectTask)
                .ToListAsync();
        }
    }
}