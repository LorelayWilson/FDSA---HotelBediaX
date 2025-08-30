using Microsoft.EntityFrameworkCore;
using Xunit;
using FluentAssertions;
using backend.Infrastructure.Data;
using backend.Infrastructure.Repositories;
using backend.Domain.Entities;
using backend.Domain.Enums;
using backend.Application.DTOs;
using backend.Tests.Helpers;

namespace backend.Tests.Infrastructure.Repositories
{
    /// <summary>
    /// Tests unitarios para DestinationRepository
    /// Verifica solo la lógica de filtros y paginación esencial
    /// </summary>
    public class DestinationRepositoryTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly DestinationRepository _repository;

        public DestinationRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new DestinationRepository(_context);
        }

        [Fact]
        public async Task GetDestinationsWithFiltersAsync_WithNoFilters_ShouldReturnAllDestinations()
        {
            // Arrange
            var destinations = TestDataHelper.CreateTestDestinations();
            _context.Destinations.AddRange(destinations);
            await _context.SaveChangesAsync();

            var filter = new DestinationFilterDto
            {
                Page = 1,
                PageSize = 10
            };

            // Act
            var result = await _repository.GetDestinationsWithFiltersAsync(filter);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(destinations.Count);
            result.TotalCount.Should().Be(destinations.Count);
        }

        [Fact]
        public async Task GetDestinationsWithFiltersAsync_WithSearchTerm_ShouldFilterByName()
        {
            // Arrange
            var destinations = TestDataHelper.CreateTestDestinations();
            _context.Destinations.AddRange(destinations);
            await _context.SaveChangesAsync();

            var filter = new DestinationFilterDto
            {
                SearchTerm = "Cancún",
                Page = 1,
                PageSize = 10
            };

            // Act
            var result = await _repository.GetDestinationsWithFiltersAsync(filter);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().OnlyContain(d => d.Name.Contains("Cancún", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public async Task GetDestinationsWithFiltersAsync_WithPagination_ShouldReturnCorrectPage()
        {
            // Arrange
            var destinations = TestDataHelper.CreateTestDestinations();
            _context.Destinations.AddRange(destinations);
            await _context.SaveChangesAsync();

            var filter = new DestinationFilterDto
            {
                Page = 1,
                PageSize = 2
            };

            // Act
            var result = await _repository.GetDestinationsWithFiltersAsync(filter);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(2);
            result.Page.Should().Be(1);
            result.PageSize.Should().Be(2);
            result.TotalCount.Should().Be(destinations.Count);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
