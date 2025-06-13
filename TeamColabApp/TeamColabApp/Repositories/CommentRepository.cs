using TeamColabApp.Models;
using TeamColabApp.Models.DTOs;
using TeamColabApp.Contexts;
using TeamColabApp.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace TeamColabApp.Repositories
{ 
    public class CommentRepository : Repository<long, Comment>
    {
        public CommentRepository(TeamColabAppContext context) : base(context)
        {
        }

        public override async Task<Comment?> GetByIdAsync(long id)
        {
            return await _context.Comments
                .Include(c => c.User)
                .Include(c => c.Project)
                .Include(c => c.ProjectTask)
                .FirstOrDefaultAsync(c => c.CommentId == id);
        }

        public override async Task<IEnumerable<Comment?>> GetAllAsync()
        {
            return await _context.Comments
                .Include(c => c.User)
                .Include(c => c.Project)
                .Include(c => c.ProjectTask)
                .ToListAsync();
        }
    }
}