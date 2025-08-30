using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using Xunit;
using FluentAssertions;
using backend.Data;
using backend.DTOs;
using backend.Models;

namespace backend.Tests.Integration
{
    /// <summary>
    /// Tests de integración para DestinationsController
    /// Prueba el flujo completo de la API con base de datos real
    /// </summary>
    public class DestinationsControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        private readonly ApplicationDbContext _context;

        public DestinationsControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Reemplazar la base de datos con una en memoria para tests
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                    if (descriptor != null)
                        services.Remove(descriptor);

                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("IntegrationTestDb");
                    });
                });
            });

            _client = _factory.CreateClient();
            
            // Obtener el contexto de la base de datos
            var scope = _factory.Services.CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        }

        [Fact]
        public async Task GetDestinations_ReturnsOkResult()
        {
            // Arrange
            await SeedTestDataAsync();

            // Act
            var response = await _client.GetAsync("/api/destinations");

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var destinations = await response.Content.ReadFromJsonAsync<PagedResultDto<DestinationDto>>();
            destinations.Should().NotBeNull();
            destinations!.Items.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetDestination_WithValidId_ReturnsDestination()
        {
            // Arrange
            var destination = await SeedSingleDestinationAsync();

            // Act
            var response = await _client.GetAsync($"/api/destinations/{destination.ID}");

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var result = await response.Content.ReadFromJsonAsync<DestinationDto>();
            result.Should().NotBeNull();
            result!.ID.Should().Be(destination.ID);
            result.Name.Should().Be(destination.Name);
        }

        [Fact]
        public async Task GetDestination_WithInvalidId_ReturnsNotFound()
        {
            // Act
            var response = await _client.GetAsync("/api/destinations/999");

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreateDestination_WithValidData_ReturnsCreatedResult()
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
            var response = await _client.PostAsJsonAsync("/api/destinations", createDto);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
            var result = await response.Content.ReadFromJsonAsync<DestinationDto>();
            result.Should().NotBeNull();
            result!.Name.Should().Be(createDto.Name);
            result.Description.Should().Be(createDto.Description);
            result.CountryCode.Should().Be(createDto.CountryCode);
            result.Type.Should().Be(createDto.Type);
            result.ID.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task CreateDestination_WithInvalidData_ReturnsBadRequest()
        {
            // Arrange
            var createDto = new CreateDestinationDto
            {
                Name = "", // Nombre vacío
                Description = "Descripción válida",
                CountryCode = "ESP",
                Type = DestinationType.Cultural
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/destinations", createDto);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task UpdateDestination_WithValidData_ReturnsOkResult()
        {
            // Arrange
            var destination = await SeedSingleDestinationAsync();
            var updateDto = new UpdateDestinationDto
            {
                Name = "Nombre Actualizado",
                Description = "Descripción actualizada",
                CountryCode = "USA",
                Type = DestinationType.Adventure
            };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/destinations/{destination.ID}", updateDto);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var result = await response.Content.ReadFromJsonAsync<DestinationDto>();
            result.Should().NotBeNull();
            result!.Name.Should().Be(updateDto.Name);
            result.Description.Should().Be(updateDto.Description);
            result.CountryCode.Should().Be(updateDto.CountryCode);
            result.Type.Should().Be(updateDto.Type);
        }

        [Fact]
        public async Task UpdateDestination_WithInvalidId_ReturnsNotFound()
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
            var response = await _client.PutAsJsonAsync("/api/destinations/999", updateDto);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteDestination_WithValidId_ReturnsNoContent()
        {
            // Arrange
            var destination = await SeedSingleDestinationAsync();

            // Act
            var response = await _client.DeleteAsync($"/api/destinations/{destination.ID}");

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
            
            // Verificar que se eliminó de la base de datos haciendo una nueva consulta HTTP
            var getResponse = await _client.GetAsync($"/api/destinations/{destination.ID}");
            getResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteDestination_WithInvalidId_ReturnsNotFound()
        {
            // Act
            var response = await _client.DeleteAsync("/api/destinations/999");

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetCountries_ReturnsListOfCountries()
        {
            // Arrange
            await SeedTestDataAsync();

            // Act
            var response = await _client.GetAsync("/api/destinations/countries");

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var countries = await response.Content.ReadFromJsonAsync<List<string>>();
            countries.Should().NotBeNull();
            countries.Should().NotBeEmpty();
            countries.Should().Contain("MEX");
            countries.Should().Contain("FRA");
        }

        [Fact]
        public async Task GetDestinationTypes_ReturnsListOfTypes()
        {
            // Act
            var response = await _client.GetAsync("/api/destinations/types");

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var types = await response.Content.ReadFromJsonAsync<List<string>>();
            types.Should().NotBeNull();
            types.Should().Contain("Beach");
            types.Should().Contain("Mountain");
            types.Should().Contain("City");
            types.Should().Contain("Cultural");
            types.Should().Contain("Adventure");
            types.Should().Contain("Relax");
        }

        [Fact]
        public async Task GetDestinations_WithSearchTerm_ReturnsFilteredResults()
        {
            // Arrange
            await SeedTestDataAsync();

            // Act
            var response = await _client.GetAsync("/api/destinations?searchTerm=Tokio");

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var result = await response.Content.ReadFromJsonAsync<PagedResultDto<DestinationDto>>();
            result.Should().NotBeNull();
            result!.Items.Should().HaveCount(1);
            result.Items.First().Name.Should().Contain("Tokio");
        }

        [Fact]
        public async Task GetDestinations_WithCountryFilter_ReturnsFilteredResults()
        {
            // Arrange
            await SeedTestDataAsync();

            // Act
            var response = await _client.GetAsync("/api/destinations?countryCode=MEX");

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var result = await response.Content.ReadFromJsonAsync<PagedResultDto<DestinationDto>>();
            result.Should().NotBeNull();
            result!.Items.Should().AllSatisfy(d => d.CountryCode.Should().Be("MEX"));
        }

        [Fact]
        public async Task GetDestinations_WithTypeFilter_ReturnsFilteredResults()
        {
            // Arrange
            await SeedTestDataAsync();

            // Act
            var response = await _client.GetAsync("/api/destinations?type=City");

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var result = await response.Content.ReadFromJsonAsync<PagedResultDto<DestinationDto>>();
            result.Should().NotBeNull();
            result!.Items.Should().AllSatisfy(d => d.Type.Should().Be(DestinationType.City));
        }

        [Fact]
        public async Task GetDestinations_WithPagination_ReturnsCorrectPage()
        {
            // Arrange
            await SeedTestDataAsync();

            // Act
            var response = await _client.GetAsync("/api/destinations?page=1&pageSize=2");

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var result = await response.Content.ReadFromJsonAsync<PagedResultDto<DestinationDto>>();
            result.Should().NotBeNull();
            result!.Items.Should().HaveCount(2);
            result.Page.Should().Be(1);
            result.PageSize.Should().Be(2);
        }

        private async Task SeedTestDataAsync()
        {
            var destinations = new List<Destination>
            {
                new Destination
                {
                    Name = "Cancún",
                    Description = "Hermosa playa en el Caribe mexicano",
                    CountryCode = "MEX",
                    Type = DestinationType.Beach,
                    LastModif = DateTime.UtcNow
                },
                new Destination
                {
                    Name = "París",
                    Description = "La ciudad de la luz y el amor",
                    CountryCode = "FRA",
                    Type = DestinationType.City,
                    LastModif = DateTime.UtcNow
                },
                new Destination
                {
                    Name = "Tokio",
                    Description = "Metrópolis moderna con tradición milenaria",
                    CountryCode = "JPN",
                    Type = DestinationType.Cultural,
                    LastModif = DateTime.UtcNow
                }
            };

            _context.Destinations.AddRange(destinations);
            await _context.SaveChangesAsync();
        }

        private async Task<Destination> SeedSingleDestinationAsync()
        {
            var destination = new Destination
            {
                Name = "Test Destination",
                Description = "Test Description",
                CountryCode = "TST",
                Type = DestinationType.Beach,
                LastModif = DateTime.UtcNow
            };

            _context.Destinations.Add(destination);
            await _context.SaveChangesAsync();
            return destination;
        }

        public void Dispose()
        {
            _context.Dispose();
            _client.Dispose();
        }
    }
}
