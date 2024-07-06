namespace Repository.Repos
{
    public interface IRepository<T> where T : class
    {
        Task<IReadOnlyCollection<T>> GetAllAsync(int pageNumber, int pageSize);
        Task<T> GetByIdAsync(int id);
        Task UpdateAsync(T entity);
        Task InsertAsync(T entity);
        Task<int> CountAsync();
        Task DeleteAsync(int id);
    }
}
