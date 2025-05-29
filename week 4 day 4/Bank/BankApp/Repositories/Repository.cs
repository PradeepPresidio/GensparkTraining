using BankApp.Contexts;
using BankApp.Interfaces;

namespace BankApp.Repositories
{
    public abstract class Repository<K, T> : IRepository<K, T> where T : class
    {
        protected readonly BankAppContext _bankAppContext;

        public Repository(BankAppContext bankAppContext)
        {
            _bankAppContext = bankAppContext;
        }
        public async Task<T> Add(T item)
        {
            _bankAppContext.Add(item);
            await _bankAppContext.SaveChangesAsync();
            return item;
        }

        public async Task<T> Delete(K key)
        {
            var item = await Get(key);
            if (item != null)
            {
                _bankAppContext.Remove(item);
                await _bankAppContext.SaveChangesAsync();
                return item;
            }
            throw new Exception("No such item found for deleting");
        }

        public abstract Task<T> Get(K key);


        public abstract Task<IEnumerable<T>> GetAll();


        public async Task<T> Update(K key, T item)
        {
            var myItem = await Get(key);
            if (myItem != null)
            {
                _bankAppContext.Entry(myItem).CurrentValues.SetValues(item);
                await _bankAppContext.SaveChangesAsync();
                return item;
            }
            throw new Exception("No such item found for updation");
        }
    }
}
