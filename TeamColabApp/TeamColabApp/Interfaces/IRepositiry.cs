namespace TeamColabApp.Interfaces
{
    public interface IRepository<K, T> where T : class
    {
        public virtual Task<T?> GetByIdAsync(K id)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IEnumerable<T?>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public virtual Task<T> AddAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual Task<T> UpdateAsync(K key,T entity)
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> DeleteAsync(K id)
        {
            throw new NotImplementedException();
        }
    }
}