using BuildingLinkTest.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BuildingLinkTest.Services
{
    public interface IDriverService : IBaseService<DriverCreateDto, DriverDto>
    {
        Task InsertRandomDriversAsync(int count);
        Task<IEnumerable<DriverDto>> GetDriversAlphabetizedAsync();
        Task<string> AlphabetizeNameAsync(string name);
    }
}