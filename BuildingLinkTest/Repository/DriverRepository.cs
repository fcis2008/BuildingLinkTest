using BuildingLinkTest.Models;
using Dapper;
using Serilog;
using ILogger = Serilog.ILogger; // Alias to resolve ambiguity

namespace BuildingLinkTest.Repository
{
    /// <summary>
    /// Repository for managing driver data.
    /// </summary>
    public class DriverRepository : BaseRepository<Driver>, IDriverRepository
    {
        private readonly string _connectionString;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DriverRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string for the database.</param>
        public DriverRepository(string connectionString) : base(connectionString)
        {
            _connectionString = connectionString;
            _logger = Log.ForContext<DriverRepository>();
        }

        /// <summary>
        /// Creates a new database connection.
        /// </summary>
        /// <returns>A new database connection.</returns>
        //public IDbConnection CreateConnection() => new SqliteConnection(_connectionString);

        /// <summary>
        /// Gets all drivers in alphabetical order.
        /// </summary>
        /// <returns>A list of drivers in alphabetical order.</returns>
        public async Task<IEnumerable<Driver>> GetDriversAlphabetizedAsync()
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();
                var sql = "SELECT * FROM Drivers ORDER BY FirstName, LastName";
                return (await connection.QueryAsync<Driver>(sql)).ToList();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while retrieving and alphabetizing the drivers");
                throw new Exception("An error occurred while retrieving and alphabetizing the drivers", ex);
            }
        }

        public async Task InsertRandomDriversAsync(IEnumerable<Driver> drivers)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();
                var sql = "INSERT INTO Drivers (FirstName, LastName, Email, PhoneNumber) VALUES (@FirstName, @LastName, @Email, @PhoneNumber)";
                await connection.ExecuteAsync(sql, drivers);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while inserting random drivers");
                throw new Exception("An error occurred while inserting random drivers", ex);
            }
        }
    }
}   