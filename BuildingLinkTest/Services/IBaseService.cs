namespace BuildingLinkTest.Services
{
    public interface IBaseService<TCreateDto, TDto>
    {
        Task<int> CreateAsync(TCreateDto createDto);
        Task<TDto> GetByIdAsync(int id);
        Task<IEnumerable<TDto>> GetAllAsync(int pageNumber, int pageSize);
        Task UpdateAsync(int id, TDto updateDto);
        Task DeleteAsync(int id);
    }
}