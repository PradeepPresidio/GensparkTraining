using TeamColabApp.Models;
using TeamColabApp.Models.DTOs;
using TeamColabApp.Contexts;
using TeamColabApp.Interfaces;
namespace TeamColabApp.Interfaces
{
    public abstract class Repository<K,T> : IRepository<K, T> where T : class
    {
        public readonly TeamColabAppContext _context;
        public Repository(TeamColabAppContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public abstract Task<T?> GetByIdAsync(K id);
        public abstract Task<IEnumerable<T?>> GetAllAsync();

        public virtual async Task<T> AddAsync(T entity)
        {
            _context.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<T> UpdateAsync(K key, T entity)
        {
            var item = await GetByIdAsync(key);
            if (item == null)
            {
                throw new KeyNotFoundException($"Entity with key {key} not found.");
            }
            _context.Update(entity);
            _context.SaveChanges();
            return entity;
        }

        public virtual async Task<bool> DeleteAsync(K id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Entity with key {id} not found.");
            }
            _context.Remove(entity);
            _context.SaveChanges();
            return true;
        }
    }
}