using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using BuildingLinkTest.Controllers;
using BuildingLinkTest.Services;
using BuildingLinkTest.DTO;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BuildingLinkTest.Tests
{
    public class DriverControllerTests
    {
        private readonly Mock<IDriverService> _mockService;
        private readonly Mock<ILogger> _mockLogger;
        private readonly DriverController _controller;

        public DriverControllerTests()
        {
            _mockService = new Mock<IDriverService>();
            _mockLogger = new Mock<ILogger>();

            // Setup the mock logger to return itself when ForContext<DriverController>() is called
            _mockLogger.Setup(logger => logger.ForContext<DriverController>()).Returns(_mockLogger.Object);

            _controller = new DriverController(_mockService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task InsertRandomDrivers_ReturnsOkResult()
        {
            // Arrange
            _mockService.Setup(service => service.InsertRandomDriversAsync(It.IsAny<int>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.InsertRandomDrivers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task CreateDriver_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var driverCreateDto = new DriverCreateDto { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", PhoneNumber = "555-1234" };
            _mockService.Setup(service => service.CreateAsync(It.IsAny<DriverCreateDto>())).ReturnsAsync(1);

            // Act
            var result = await _controller.CreateDriver(driverCreateDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(201, createdAtActionResult.StatusCode);
        }

        [Fact]
        public async Task GetDriver_ReturnsOkResult_WhenDriverExists()
        {
            // Arrange
            var driverDto = new DriverDto { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", PhoneNumber = "555-1234" };
            _mockService.Setup(service => service.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(driverDto);

            // Act
            var result = await _controller.GetDriver(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task GetDriver_ReturnsNotFoundResult_WhenDriverDoesNotExist()
        {
            // Arrange
            _mockService.Setup(service => service.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((DriverDto)null);

            // Act
            var result = await _controller.GetDriver(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task GetAllDrivers_ReturnsOkResult()
        {
            // Arrange
            var driverDtos = new List<DriverDto> { new DriverDto { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", PhoneNumber = "555-1234" } };
            _mockService.Setup(service => service.GetAllAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(driverDtos);

            // Act
            var result = await _controller.GetAllDrivers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task UpdateDriver_ReturnsNoContentResult()
        {
            // Arrange
            var driverDto = new DriverDto { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", PhoneNumber = "555-1234" };
            _mockService.Setup(service => service.UpdateAsync(It.IsAny<int>(), It.IsAny<DriverDto>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateDriver(1, driverDto);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, noContentResult.StatusCode);
        }

        [Fact]
        public async Task DeleteDriver_ReturnsOkResult()
        {
            // Arrange
            _mockService.Setup(service => service.DeleteAsync(It.IsAny<int>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteDriver(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task GetDriversAlphabetized_ReturnsOkResult()
        {
            // Arrange
            var driverDtos = new List<DriverDto> { new DriverDto { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", PhoneNumber = "555-1234" } };
            _mockService.Setup(service => service.GetDriversAlphabetizedAsync()).ReturnsAsync(driverDtos);

            // Act
            var result = await _controller.GetDriversAlphabetized();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task AlphabetizeName_ReturnsOkResult()
        {
            // Arrange
            var name = "John";
            var alphabetizedName = "Jhno";
            _mockService.Setup(service => service.AlphabetizeNameAsync(It.IsAny<string>())).ReturnsAsync(alphabetizedName);

            // Act
            var result = await _controller.AlphabetizeName(name);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }
    }
}
