using Microsoft.EntityFrameworkCore;
using Xunit;
using FluentAssertions;
using backend.Infrastructure.Data;
using backend.Domain.Interfaces;
using backend.Infrastructure.UnitOfWork;
using backend.Domain.Entities;
using backend.Domain.Enums;

namespace backend.Tests.Infrastructure.UnitOfWork
{
    /// <summary>
    /// Tests unitarios para UnitOfWork
    /// Verifica la coordinación de transacciones
    /// </summary>
    public class UnitOfWorkTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly backend.Infrastructure.UnitOfWork.UnitOfWork _unitOfWork;

        public UnitOfWorkTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _unitOfWork = new backend.Infrastructure.UnitOfWork.UnitOfWork(_context);
        }

        [Fact]
        public void UnitOfWork_ShouldHaveDestinationsRepository()
        {
            // Act & Assert
            _unitOfWork.Destinations.Should().NotBeNull();
        }

        [Fact]
        public async Task SaveChangesAsync_ShouldSaveChangesToDatabase()
        {
            // Arrange
            var destination = new Destination
            {
                Name = "Test Destination",
                Description = "Test Description",
                CountryCode = "TST",
                Type = DestinationType.Beach,
                LastModif = DateTime.UtcNow
            };

            _context.Destinations.Add(destination);

            // Act
            var result = await _unitOfWork.SaveChangesAsync();

            // Assert
            result.Should().BeGreaterThan(0);
            var savedDestination = await _context.Destinations.FirstOrDefaultAsync();
            savedDestination.Should().NotBeNull();
            savedDestination!.Name.Should().Be("Test Destination");
        }

        [Fact]
        public async Task SaveChangesAsync_WithNoChanges_ShouldReturnZero()
        {
            // Act
            var result = await _unitOfWork.SaveChangesAsync();

            // Assert
            result.Should().Be(0);
        }

        [Fact]
        public void Dispose_ShouldDisposeContext()
        {
            // Act
            _unitOfWork.Dispose();

            // Assert - No exception should be thrown
            // The context should be disposed properly
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
