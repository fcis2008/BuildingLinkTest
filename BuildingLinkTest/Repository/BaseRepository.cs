using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;
using Serilog;
using ILogger = Serilog.ILogger; // Alias to resolve ambiguity


namespace BuildingLinkTest.Repository
{
    /// <summary>
    /// Base repository class for managing database operations.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly string _connectionString;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository{T}"/> class.
        /// </summary>
        /// <param name="connectionString">The database connection string.</param>
        public BaseRepository(string connectionString)
        {
            _connectionString = connectionString;
            _logger = Log.ForContext<BaseRepository<T>>();
        }

        /// <summary>
        /// Creates a new database connection.
        /// </summary>
        /// <returns>An <see cref="IDbConnection"/> object.</returns>
        public IDbConnection CreateConnection() => new SqliteConnection(_connectionString);

        /// <summary>
        /// Creates a new entity in the database.
        /// </summary>
        /// <param name="entity">The entity to create.</param>
        /// <returns>The ID of the created entity.</returns>
        public async Task<int> CreateAsync(T entity)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();
                var sql = $"INSERT INTO {typeof(T).Name}s ({GetColumns()}) VALUES ({GetColumns("@")})";
                await connection.ExecuteAsync(sql, entity);
                return connection.ExecuteScalar<int>("SELECT last_insert_rowid()");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"An error occurred while creating the {typeof(T).Name}");
                throw new Exception($"An error occurred while creating the {typeof(T).Name}", ex);
            }
        }

        /// <summary>
        /// Retrieves an entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <returns>The entity with the specified ID.</returns>
        public async Task<T> GetByIdAsync(int id)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();
                var sql = $"SELECT * FROM {typeof(T).Name}s WHERE Id = @Id";
                return await connection.QuerySingleOrDefaultAsync<T>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"An error occurred while retrieving the {typeof(T).Name}");
                throw new Exception($"An error occurred while retrieving the {typeof(T).Name}", ex);
            }
        }

        /// <summary>
        /// Retrieves all entities with pagination.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns>A list of entities.</returns>
        public async Task<IEnumerable<T>> GetAllAsync(int pageNumber, int pageSize)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();
                var sql = $"SELECT * FROM {typeof(T).Name}s ORDER BY Id LIMIT @PageSize OFFSET @Offset";
                return (await connection.QueryAsync<T>(sql, new { Offset = (pageNumber - 1) * pageSize, PageSize = pageSize })).ToList();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"An error occurred while retrieving the {typeof(T).Name}s");
                throw new Exception($"An error occurred while retrieving the {typeof(T).Name}s", ex);
            }
        }

        /// <summary>
        /// Updates an existing entity in the database.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        public async Task UpdateAsync(T entity)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();
                var sql = $"UPDATE {typeof(T).Name}s SET {GetUpdateColumns()} WHERE Id = @Id";
                await connection.ExecuteAsync(sql, entity);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"An error occurred while updating the {typeof(T).Name}");
                throw new Exception($"An error occurred while updating the {typeof(T).Name}", ex);
            }
        }

        /// <summary>
        /// Deletes an entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the entity to delete.</param>
        public async Task DeleteAsync(int id)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();
                var sql = $"DELETE FROM {typeof(T).Name}s WHERE Id = @Id";
                await connection.ExecuteAsync(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"An error occurred while deleting the {typeof(T).Name}");
                throw new Exception($"An error occurred while deleting the {typeof(T).Name}", ex);
            }
        }

        /// <summary>
        /// Gets the columns of the entity for SQL queries.
        /// </summary>
        /// <param name="prefix">The prefix for the columns.</param>
        /// <returns>A comma-separated string of columns.</returns>
        private static string GetColumns(string prefix = "")
        {
            var properties = typeof(T).GetProperties().Where(p => !p.Name.Equals("id", StringComparison.CurrentCultureIgnoreCase)).Select(p => p.Name);
            return string.Join(", ", properties.Select(p => $"{prefix}{p}"));
        }

        /// <summary>
        /// Gets the columns for the update SQL query.
        /// </summary>
        /// <returns>A comma-separated string of columns for the update query.</returns>
        private static string GetUpdateColumns()
        {
            var properties = typeof(T).GetProperties().Select(p => p.Name);
            return string.Join(", ", properties.Select(p => $"{p} = @{p}"));
        }
    }
}
