namespace BuildingLinkTest.Repository
{
    public interface IBaseRepository<T> where T : class
    {
        Task<int> CreateAsync(T entity);
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync(int pageNumber, int pageSize);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }
}
