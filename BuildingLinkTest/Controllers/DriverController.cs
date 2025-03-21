using Microsoft.AspNetCore.Mvc;
using BuildingLinkTest.Services;
using BuildingLinkTest.DTO;
using ILogger = Serilog.ILogger; // Alias to resolve ambiguity

namespace BuildingLinkTest.Controllers
{
    /// <summary>
    /// Controller for managing drivers.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class DriverController : ControllerBase
    {
        private readonly IDriverService _service;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DriverController"/> class.
        /// </summary>
        /// <param name="service">The driver service.</param>
        /// <param name="logger">The logger instance.</param>
        public DriverController(IDriverService service, ILogger logger)
        {
            _service = service;
            _logger = logger.ForContext<DriverController>();
        }

        /// <summary>
        /// Inserts 10 random drivers.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>
        [HttpPost("insert-random")]
        public async Task<IActionResult> InsertRandomDrivers()
        {
            try
            {
                await _service.InsertRandomDriversAsync(10);
                _logger.Information("10 random drivers inserted successfully");
                return Ok(new { message = "10 random drivers inserted successfully" });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while inserting random drivers");
                return StatusCode(500, new { message = "An error occurred while inserting random drivers", details = ex.Message });
            }
        }

        /// <summary>
        /// Creates a new driver.
        /// </summary>
        /// <param name="driverCreateDto">The driver creation DTO.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateDriver([FromBody] DriverCreateDto driverCreateDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.Warning("Invalid model state for CreateDriver");
                return BadRequest(ModelState);
            }

            try
            {
                var lastInsertedRowID = await _service.CreateAsync(driverCreateDto);
                _logger.Information("Driver created successfully");
                return CreatedAtAction(nameof(GetDriver), new { id = lastInsertedRowID }, driverCreateDto);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while creating the driver");
                return StatusCode(500, new { message = "An error occurred while creating the driver", details = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves a driver by ID.
        /// </summary>
        /// <param name="id">The driver ID.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDriver(int id)
        {
            try
            {
                var driverReadDto = await _service.GetByIdAsync(id);
                if (driverReadDto == null)
                {
                    _logger.Warning("Driver not found with ID {DriverId}", id);
                    return NotFound(new { message = "Driver not found" });
                }
                _logger.Information("Driver retrieved successfully with ID {DriverId}", id);
                return Ok(new { message = "Driver retrieved successfully", driver = driverReadDto });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while retrieving the driver with ID {DriverId}", id);
                return StatusCode(500, new { message = "An error occurred while retrieving the driver", details = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves all drivers with pagination.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllDrivers(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var driverReadDtos = await _service.GetAllAsync(pageNumber, pageSize);
                _logger.Information("Drivers retrieved successfully");
                return Ok(new { message = "Drivers retrieved successfully", drivers = driverReadDtos });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while retrieving the drivers");
                return StatusCode(500, new { message = "An error occurred while retrieving the drivers", details = ex.Message });
            }
        }

        /// <summary>
        /// Updates a driver by ID.
        /// </summary>
        /// <param name="id">The driver ID.</param>
        /// <param name="driverDto">The driver DTO.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDriver(int id, [FromBody] DriverDto driverDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.Warning("Invalid model state for UpdateDriver");
                return BadRequest(ModelState);
            }

            try
            {
                await _service.UpdateAsync(id, driverDto);
                _logger.Information("Driver updated successfully with ID {DriverId}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while updating the driver with ID {DriverId}", id);
                return StatusCode(500, new { message = "An error occurred while updating the driver", details = ex.Message });
            }
        }

        /// <summary>
        /// Deletes a driver by ID.
        /// </summary>
        /// <param name="id">The driver ID.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDriver(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                _logger.Information("Driver deleted successfully with ID {DriverId}", id);
                return Ok(new { message = "Driver deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while deleting the driver with ID {DriverId}", id);
                return StatusCode(500, new { message = "An error occurred while deleting the driver", details = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves and alphabetizes the list of drivers.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>
        [HttpGet("alphabetized")]
        public async Task<IActionResult> GetDriversAlphabetized()
        {
            try
            {
                var driverReadDtos = await _service.GetDriversAlphabetizedAsync();
                _logger.Information("Drivers retrieved and alphabetized successfully");
                return Ok(new { message = "Drivers retrieved and alphabetized successfully", drivers = driverReadDtos });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while retrieving and alphabetizing the drivers");
                return StatusCode(500, new { message = "An error occurred while retrieving and alphabetizing the drivers", details = ex.Message });
            }
        }

        /// <summary>
        /// Alphabetizes the characters in a given name.
        /// </summary>
        /// <param name="name">The name to alphabetize.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>
        [HttpGet("alphabetize-name")]
        public async Task<IActionResult> AlphabetizeName(string name)
        {
            try
            {
                var alphabetizedName = await _service.AlphabetizeNameAsync(name);
                _logger.Information("Name alphabetized successfully");
                return Ok(new { message = "Name alphabetized successfully", alphabetizedName });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while alphabetizing the name");
                return StatusCode(500, new { message = "An error occurred while alphabetizing the name", details = ex.Message });
            }
        }
    }
}
