using TeamColabApp.Models;
using TeamColabApp.Models.DTOs;
using TeamColabApp.Contexts;
using TeamColabApp.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace TeamColabApp.Repositories
{ 
    public class AppFileRepository : Repository<long, AppFile>
    {
        public AppFileRepository(TeamColabAppContext context) : base(context)
        {
        }

        public override async Task<AppFile?> GetByIdAsync(long id)
        {
            return await _context.AppFiles
                .Include(f => f.User)
                .Include(f => f.Project)
                .Include(f => f.ProjectTask)
                .FirstOrDefaultAsync(f => f.FileId == id);
        }

        public override async Task<IEnumerable<AppFile?>> GetAllAsync()
        {
            return await _context.AppFiles
                .Include(f => f.User)
                .Include(f => f.Project)
                .Include(f => f.ProjectTask)
                .ToListAsync();
        }
    }
}