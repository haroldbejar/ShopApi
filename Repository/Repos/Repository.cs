using Domain.Data;
using Microsoft.EntityFrameworkCore;

namespace Repository.Repos
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly WebShopContext _context;
        public Repository(WebShopContext context)
        {
            _context = context;
        }

        protected DbSet<T> EntitySet
        {
            get
            {
                return _context.Set<T>();
            }
        }

        public async Task<int> CountAsync()
        {
            return await EntitySet.CountAsync();
        }

        public async Task<IReadOnlyCollection<T>> GetAllAsync(int pageNumber, int pageSize)
        {
            return await EntitySet
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await EntitySet.FindAsync(id);
        }

        public async Task InsertAsync(T entity)
        {
            EntitySet.Add(entity);
            await Save();
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await Save();
        }

        public async Task DeleteAsync(int id)
        {
            T entity = await EntitySet.FindAsync(id);
            EntitySet.Remove(entity);
            await Save();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
