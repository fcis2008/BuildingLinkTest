using Microsoft.Data.Sqlite;
using System.Data;
using Dapper;
using BuildingLinkTest.Models;

public class DatabaseContext
{
    private readonly string _connectionString;

    public DatabaseContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection CreateConnection() => new SqliteConnection(_connectionString);

    public void CreateDriver(Driver driver)
    {
        using var connection = CreateConnection();
        connection.Open();
        var sql = "INSERT INTO Drivers (FirstName, LastName, Email, PhoneNumber) VALUES (@FirstName, @LastName, @Email, @PhoneNumber)";
        connection.Execute(sql, driver);
    }

    public Driver GetDriver(int id)
    {
        using var connection = CreateConnection();
        connection.Open();
        var sql = "SELECT * FROM Drivers WHERE Id = @Id";
        return connection.QuerySingleOrDefault<Driver>(sql, new { Id = id });
    }

    public IEnumerable<Driver> GetAllDrivers()
    {
        using (var connection = CreateConnection())
        {
            connection.Open();
            var sql = "SELECT * FROM Drivers";
            return connection.Query<Driver>(sql).ToList();
        }
    }

    public void UpdateDriver(Driver driver)
    {
        using var connection = CreateConnection();
        connection.Open();
        var sql = "UPDATE Drivers SET FirstName = @FirstName, LastName = @LastName, Email = @Email, PhoneNumber = @PhoneNumber WHERE Id = @Id";
        connection.Execute(sql, driver);
    }

    public void DeleteDriver(int id)
    {
        using var connection = CreateConnection();
        connection.Open();
        var sql = "DELETE FROM Drivers WHERE Id = @Id";
        connection.Execute(sql, new { Id = id });
    }

    public void InsertRandomDrivers(IEnumerable<Driver> drivers)
    {
        using var connection = CreateConnection();
        connection.Open();
        var sql = "INSERT INTO Drivers (FirstName, LastName, Email, PhoneNumber) VALUES (@FirstName, @LastName, @Email, @PhoneNumber)";
        connection.Execute(sql, drivers);
    }

    public IEnumerable<Driver> GetDriversAlphabetized()
    {
        using var connection = CreateConnection();
        connection.Open();
        var sql = "SELECT * FROM Drivers ORDER BY FirstName, LastName";
        return connection.Query<Driver>(sql).ToList();
    }

    public string AlphabetizeName(string name)
    {
        return new string(name.OrderBy(c => c).ToArray());
    }
}
   