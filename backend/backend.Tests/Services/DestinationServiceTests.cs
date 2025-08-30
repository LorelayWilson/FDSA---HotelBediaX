using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Xunit;
using FluentAssertions;
using backend.Data;
using backend.DTOs;
using backend.Models;
using backend.Services;
using backend.Tests.Helpers;

namespace backend.Tests.Services
{
    /// <summary>
    /// Tests unitarios para DestinationService
    /// Prueba toda la lógica de negocio del servicio de destinos
    /// </summary>
    public class DestinationServiceTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly DestinationService _service;

        public DestinationServiceTests()
        {
            // Configurar base de datos en memoria para tests
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            
            // Configurar AutoMapper
            var config = new MapperConfiguration(cfg => cfg.AddProfile<backend.Mapping.AutoMapperProfile>());
            _mapper = config.CreateMapper();

            _service = new DestinationService(_context, _mapper);
        }

        [Fact]
        public async Task GetDestinationsAsync_WithoutFilters_ReturnsAllDestinations()
        {
            // Arrange
            var destinations = TestDataHelper.CreateTestDestinations();
            _context.Destinations.AddRange(destinations);
            await _context.SaveChangesAsync();

            var filter = new DestinationFilterDto { Page = 1, PageSize = 10 };

            // Act
            var result = await _service.GetDestinationsAsync(filter);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(3);
            result.TotalCount.Should().Be(3);
            result.Page.Should().Be(1);
            result.PageSize.Should().Be(10);
        }

        [Fact]
        public async Task GetDestinationsAsync_WithSearchTerm_ReturnsFilteredDestinations()
        {
            // Arrange
            var destinations = TestDataHelper.CreateTestDestinations();
            _context.Destinations.AddRange(destinations);
            await _context.SaveChangesAsync();

            var filter = new DestinationFilterDto 
            { 
                SearchTerm = "París", 
                Page = 1, 
                PageSize = 10 
            };

            // Act
            var result = await _service.GetDestinationsAsync(filter);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(1);
            result.Items.First().Name.Should().Contain("París");
        }

        [Fact]
        public async Task GetDestinationsAsync_WithCountryCodeFilter_ReturnsFilteredDestinations()
        {
            // Arrange
            var destinations = TestDataHelper.CreateTestDestinations();
            _context.Destinations.AddRange(destinations);
            await _context.SaveChangesAsync();

            var filter = new DestinationFilterDto 
            { 
                CountryCode = "MEX", 
                Page = 1, 
                PageSize = 10 
            };

            // Act
            var result = await _service.GetDestinationsAsync(filter);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(1);
            result.Items.First().CountryCode.Should().Be("MEX");
        }

        [Fact]
        public async Task GetDestinationsAsync_WithTypeFilter_ReturnsFilteredDestinations()
        {
            // Arrange
            var destinations = TestDataHelper.CreateTestDestinations();
            _context.Destinations.AddRange(destinations);
            await _context.SaveChangesAsync();

            var filter = new DestinationFilterDto 
            { 
                Type = DestinationType.City, 
                Page = 1, 
                PageSize = 10 
            };

            // Act
            var result = await _service.GetDestinationsAsync(filter);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(1);
            result.Items.First().Type.Should().Be(DestinationType.City);
        }

        [Fact]
        public async Task GetDestinationsAsync_WithPagination_ReturnsCorrectPage()
        {
            // Arrange
            var destinations = TestDataHelper.CreateTestDestinations();
            _context.Destinations.AddRange(destinations);
            await _context.SaveChangesAsync();

            var filter = new DestinationFilterDto 
            { 
                Page = 2, 
                PageSize = 1 
            };

            // Act
            var result = await _service.GetDestinationsAsync(filter);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(1);
            result.Page.Should().Be(2);
            result.TotalPages.Should().Be(3);
        }

        [Fact]
        public async Task GetDestinationByIdAsync_WithValidId_ReturnsDestination()
        {
            // Arrange
            var destination = TestDataHelper.CreateTestDestination();
            _context.Destinations.Add(destination);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetDestinationByIdAsync(destination.ID);

            // Assert
            result.Should().NotBeNull();
            result!.ID.Should().Be(destination.ID);
            result.Name.Should().Be(destination.Name);
        }

        [Fact]
        public async Task GetDestinationByIdAsync_WithInvalidId_ReturnsNull()
        {
            // Act
            var result = await _service.GetDestinationByIdAsync(999);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task CreateDestinationAsync_WithValidData_CreatesDestination()
        {
            // Arrange
            var createDto = new CreateDestinationDto
            {
                Name = "Nuevo Destino",
                Description = "Descripción del nuevo destino",
                CountryCode = "ESP",
                Type = DestinationType.Cultural
            };

            // Act
            var result = await _service.CreateDestinationAsync(createDto);

            // Assert
            result.Should().NotBeNull();
            result.ID.Should().BeGreaterThan(0);
            result.Name.Should().Be(createDto.Name);
            result.Description.Should().Be(createDto.Description);
            result.CountryCode.Should().Be(createDto.CountryCode);
            result.Type.Should().Be(createDto.Type);
            result.LastModif.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));

            // Verificar que se guardó en la base de datos
            var savedDestination = await _context.Destinations.FindAsync(result.ID);
            savedDestination.Should().NotBeNull();
        }

        [Fact]
        public async Task UpdateDestinationAsync_WithValidId_UpdatesDestination()
        {
            // Arrange
            var destination = TestDataHelper.CreateTestDestination();
            _context.Destinations.Add(destination);
            await _context.SaveChangesAsync();

            var updateDto = new UpdateDestinationDto
            {
                Name = "Nombre Actualizado",
                Description = "Descripción actualizada",
                CountryCode = "USA",
                Type = DestinationType.Adventure
            };

            // Act
            var result = await _service.UpdateDestinationAsync(destination.ID, updateDto);

            // Assert
            result.Should().NotBeNull();
            result!.Name.Should().Be(updateDto.Name);
            result.Description.Should().Be(updateDto.Description);
            result.CountryCode.Should().Be(updateDto.CountryCode);
            result.Type.Should().Be(updateDto.Type);
            result.LastModif.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }

        [Fact]
        public async Task UpdateDestinationAsync_WithInvalidId_ReturnsNull()
        {
            // Arrange
            var updateDto = new UpdateDestinationDto
            {
                Name = "Nombre Actualizado",
                Description = "Descripción actualizada",
                CountryCode = "USA",
                Type = DestinationType.Adventure
            };

            // Act
            var result = await _service.UpdateDestinationAsync(999, updateDto);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task DeleteDestinationAsync_WithValidId_DeletesDestination()
        {
            // Arrange
            var destination = TestDataHelper.CreateTestDestination();
            _context.Destinations.Add(destination);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.DeleteDestinationAsync(destination.ID);

            // Assert
            result.Should().BeTrue();

            // Verificar que se eliminó de la base de datos
            var deletedDestination = await _context.Destinations.FindAsync(destination.ID);
            deletedDestination.Should().BeNull();
        }

        [Fact]
        public async Task DeleteDestinationAsync_WithInvalidId_ReturnsFalse()
        {
            // Act
            var result = await _service.DeleteDestinationAsync(999);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task GetCountriesAsync_ReturnsUniqueCountries()
        {
            // Arrange
            var destinations = TestDataHelper.CreateTestDestinations();
            _context.Destinations.AddRange(destinations);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetCountriesAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(3);
            result.Should().Contain("MEX");
            result.Should().Contain("FRA");
            result.Should().Contain("JPN");
            result.Should().BeInAscendingOrder();
        }

        [Fact]
        public async Task GetCountriesAsync_WithNoDestinations_ReturnsEmptyList()
        {
            // Act
            var result = await _service.GetCountriesAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
