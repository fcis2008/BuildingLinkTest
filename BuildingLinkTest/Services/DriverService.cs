using AutoMapper;
using BuildingLinkTest.DTO;
using BuildingLinkTest.Models;
using BuildingLinkTest.Repository;
using Serilog;
using ILogger = Serilog.ILogger; // Alias to resolve ambiguity

namespace BuildingLinkTest.Services
{
    /// <summary>
    /// Service class for managing Driver entities.
    /// </summary>
    public class DriverService : BaseService<DriverCreateDto, DriverDto, Driver>, IDriverService
    {
        private readonly IDriverRepository _driverRepository;
        private readonly ILogger _logger;
        private static readonly Random _random = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="DriverService"/> class.
        /// </summary>
        /// <param name="driverRepository">The driver repository.</param>
        /// <param name="mapper">The AutoMapper instance.</param>
        /// <param name="logger">The logger instance.</param>
        public DriverService(IDriverRepository driverRepository, IMapper mapper, ILogger logger)
            : base(driverRepository, mapper)
        {
            _driverRepository = driverRepository;
            _logger = logger.ForContext<DriverService>();
        }

        /// <summary>
        /// Inserts a specified number of random drivers asynchronously.
        /// </summary>
        /// <param name="count">The number of drivers to insert.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task InsertRandomDriversAsync(int count)
        {
            try
            {
                List<Driver> drivers = new();

                for (int i = 0; i < count; i++)
                {
                    drivers.Add(new Driver
                    {
                        FirstName = GenerateRandomString(5),
                        LastName = GenerateRandomString(7),
                        Email = $"{GenerateRandomString(5)}@example.com",
                        PhoneNumber = $"555-{GenerateRandomString(4, true)}"
                    });
                }
                await _driverRepository.InsertRandomDriversAsync(drivers);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while inserting random drivers");
                throw new Exception("An error occurred while inserting random drivers", ex);
            }
        }

        /// <summary>
        /// Generates a random string of specified length.
        /// </summary>
        /// <param name="length">The length of the string.</param>
        /// <param name="digitsOnly">If true, generates a string with digits only.</param>
        /// <returns>A random string.</returns>
        private static string GenerateRandomString(int length, bool digitsOnly = false)
        {
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string digits = "0123456789";
            var chars = digitsOnly ? digits : letters;
            return new string(Enumerable.Repeat(chars, length).Select(s => s[_random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Retrieves and alphabetizes the list of drivers asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the list of alphabetized drivers.</returns>
        public async Task<IEnumerable<DriverDto>> GetDriversAlphabetizedAsync()
        {
            try
            {
                var drivers = await _driverRepository.GetDriversAlphabetizedAsync();
                return _mapper.Map<IEnumerable<DriverDto>>(drivers);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while retrieving and alphabetizing the drivers");
                throw new Exception("An error occurred while retrieving and alphabetizing the drivers", ex);
            }
        }

        /// <summary>
        /// Alphabetizes the characters in a given name asynchronously.
        /// </summary>
        /// <param name="name">The name to alphabetize.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the alphabetized name.</returns>
        public async Task<string> AlphabetizeNameAsync(string name)
        {
            try
            {
                return await Task.Run(() => new string(name.OrderBy(c => c).ToArray()));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while alphabetizing the name");
                throw new Exception("An error occurred while alphabetizing the name", ex);
            }
        }
    }
}