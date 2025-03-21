using BuildingLinkTest.Repository;
using BuildingLinkTest.Services;
using Serilog;
using ILogger = Serilog.ILogger; // Alias to resolve ambiguity

namespace BuildingLinkTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            try
            {
                Log.Information("Starting up");
                var builder = WebApplication.CreateBuilder(args);

                // Add services to the container.
                builder.Host.UseSerilog(); // Use Serilog

                builder.Services.AddControllers();
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                // Register AutoMapper
                builder.Services.AddAutoMapper(typeof(Program));

                builder.Services.AddSingleton(Log.Logger);

                // Register DatabaseContext
                builder.Services.AddSingleton(new DatabaseContext("Data Source=drivers.db"));

                // Register Repositories and Services
                builder.Services.AddScoped<IDriverRepository>(provider => new DriverRepository("Data Source=drivers.db"));
                builder.Services.AddScoped<IDriverService, DriverService>();

                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();
                app.UseAuthorization();
                app.MapControllers();
                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
