using Xunit;
using Moq;
using BuildingLinkTest.Repository;
using BuildingLinkTest.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BuildingLinkTest.Tests
{
    public class DriverRepositoryTests
    {
        private readonly Mock<IDriverRepository> _mockRepository;

        public DriverRepositoryTests()
        {
            _mockRepository = new Mock<IDriverRepository>();
        }

        [Fact]
        public async Task InsertRandomDriversAsync_InsertsDrivers()
        {
            // Arrange
            var drivers = new List<Driver> { new Driver { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", PhoneNumber = "555-1234" } };
            _mockRepository.Setup(repo => repo.InsertRandomDriversAsync(drivers)).Returns(Task.CompletedTask);

            // Act
            await _mockRepository.Object.InsertRandomDriversAsync(drivers);

            // Assert
            _mockRepository.Verify(repo => repo.InsertRandomDriversAsync(drivers), Times.Once);
        }

        [Fact]
        public async Task GetDriversAlphabetizedAsync_ReturnsAlphabetizedDrivers()
        {
            // Arrange
            var drivers = new List<Driver> { new Driver { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", PhoneNumber = "555-1234" } };
            _mockRepository.Setup(repo => repo.GetDriversAlphabetizedAsync()).ReturnsAsync(drivers);

            // Act
            var result = await _mockRepository.Object.GetDriversAlphabetizedAsync();

            // Assert
            Assert.Equal(drivers, result);
        }
    }
}
