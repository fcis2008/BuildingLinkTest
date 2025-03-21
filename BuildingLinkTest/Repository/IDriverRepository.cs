using BuildingLinkTest.Models;

namespace BuildingLinkTest.Repository
{
    public interface IDriverRepository : IBaseRepository<Driver>
    {
        Task InsertRandomDriversAsync(IEnumerable<Driver> drivers);
        Task<IEnumerable<Driver>> GetDriversAlphabetizedAsync();
    }
}