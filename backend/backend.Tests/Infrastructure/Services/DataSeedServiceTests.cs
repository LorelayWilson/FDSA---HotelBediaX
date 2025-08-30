using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using FluentAssertions;
using backend.Infrastructure.Data;
using backend.Infrastructure.Services;
using backend.Domain.Enums;

namespace backend.Tests.Infrastructure.Services
{
    /// <summary>
    /// Tests unitarios para DataSeedService
    /// Verifica solo la funcionalidad esencial del servicio
    /// </summary>
    public class DataSeedServiceTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly Mock<ILogger<DataSeedService>> _mockLogger;
        private readonly DataSeedService _service;

        public DataSeedServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _mockLogger = new Mock<ILogger<DataSeedService>>();
            _service = new DataSeedService(_context, _mockLogger.Object);
        }

        [Fact]
        public async Task SeedDataAsync_WithEmptyDatabase_ShouldPopulateDestinations()
        {
            // Arrange
            // Base de datos vacía

            // Act
            await _service.SeedDataAsync();

            // Assert
            var destinations = await _context.Destinations.ToListAsync();
            destinations.Should().NotBeEmpty();
            destinations.Should().HaveCountGreaterThan(0);
        }

        [Fact]
        public async Task SeedDataAsync_WithExistingData_ShouldNotDuplicateDestinations()
        {
            // Arrange
            await _service.SeedDataAsync();
            var initialCount = await _context.Destinations.CountAsync();

            // Act
            await _service.SeedDataAsync();

            // Assert
            var finalCount = await _context.Destinations.CountAsync();
            finalCount.Should().Be(initialCount);
        }

        [Fact]
        public async Task SeedDataAsync_ShouldCreateDestinationsWithAllTypes()
        {
            // Arrange
            var destinationTypes = Enum.GetValues<DestinationType>();

            // Act
            await _service.SeedDataAsync();

            // Assert
            var destinations = await _context.Destinations.ToListAsync();
            foreach (var type in destinationTypes)
            {
                destinations.Should().Contain(d => d.Type == type);
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
