using Xunit;
using Moq;
using BuildingLinkTest.Services;
using BuildingLinkTest.Repository;
using BuildingLinkTest.DTO;
using BuildingLinkTest.Models;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BuildingLinkTest.Tests
{
    public class DriverServiceTests
    {
        private readonly Mock<IDriverRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly DriverService _service;

        public DriverServiceTests()
        {
            _mockRepository = new Mock<IDriverRepository>();
            _mockMapper = new Mock<IMapper>();
            _service = new DriverService(_mockRepository.Object, _mockMapper.Object, Mock.Of<Serilog.ILogger>());
        }

        [Fact]
        public async Task InsertRandomDriversAsync_InsertsDrivers()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.InsertRandomDriversAsync(It.IsAny<IEnumerable<Driver>>())).Returns(Task.CompletedTask);

            // Act
            await _service.InsertRandomDriversAsync(10);

            // Assert
            _mockRepository.Verify(repo => repo.InsertRandomDriversAsync(It.IsAny<IEnumerable<Driver>>()), Times.Once);
        }

        [Fact]
        public async Task GetDriversAlphabetizedAsync_ReturnsAlphabetizedDrivers()
        {
            // Arrange
            var drivers = new List<Driver> { new Driver { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", PhoneNumber = "555-1234" } };
            var driverDtos = new List<DriverDto> { new DriverDto { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", PhoneNumber = "555-1234" } };
            _mockRepository.Setup(repo => repo.GetDriversAlphabetizedAsync()).ReturnsAsync(drivers);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<DriverDto>>(drivers)).Returns(driverDtos);

            // Act
            var result = await _service.GetDriversAlphabetizedAsync();

            // Assert
            Assert.Equal(driverDtos, result);
        }

        [Fact]
        public async Task AlphabetizeNameAsync_ReturnsAlphabetizedName()
        {
            // Arrange
            var name = "John";
            var alphabetizedName = "Jhno";

            // Act
            var result = await _service.AlphabetizeNameAsync(name);

            // Assert
            Assert.Equal(alphabetizedName, result);
        }
    }
}
